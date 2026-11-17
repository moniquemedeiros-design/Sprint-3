
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Sprint3.Data;
using Sprint3.Repositories;
using Sprint3.Services;
using Sprint3.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Registro dos Controllers
builder.Services.AddControllers();

// Configuração CORS - Permitir comunicação com o Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularFront", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Injeção de dependência - Alunos
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IAlunoService, AlunoService>();

// Injeção de dependência - Professores
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();

// Injeção de dependência - Disciplinas
builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();
builder.Services.AddScoped<IDisciplinaService, DisciplinaService>();

// Injeção de dependência - Turmas
builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
builder.Services.AddScoped<ITurmaService, TurmaService>();

// Injeção de dependência - Matrículas
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<IMatriculaService, MatriculaService>();

// Injeção de dependência - Notas
builder.Services.AddScoped<INotaRepository, NotaRepository>();
builder.Services.AddScoped<INotaService, NotaService>();

// Injeção de dependência - Autenticação
builder.Services.AddScoped<IAuthService, AuthService>();

// Conexão com o banco de dados MySQL
var conexao = builder.Configuration.GetConnectionString("ConexaoMySQL");

if (string.IsNullOrWhiteSpace(conexao))
{
    throw new InvalidOperationException(
        "A string de conexão com o MySQL não foi configurada."
    );
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        conexao,
        ServerVersion.AutoDetect(conexao)
    )
);

// Configuração JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException(
        "As configurações JWT estão incompletas."
    );
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

// Configuração de autorização
builder.Services.AddAuthorization();

// Configuração OpenAPI
builder.Services.AddOpenApi();

// Configuração Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT para acessar os endpoints protegidos."
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)]
                = new List<string>()
        }
    );
});

var app = builder.Build();

// Middleware global de tratamento de exceções
app.UseMiddleware<ExceptionMiddleware>();

// Configuração do ambiente
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirecionamento HTTPS
app.UseHttpsRedirection();

// Ativar CORS para o Angular
app.UseCors("AngularFront");

// Autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento dos Controllers
app.MapControllers();

app.Run();
