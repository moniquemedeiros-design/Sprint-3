
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sprint3.Data;
using Sprint3.DTOs;
using Sprint3.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sprint3.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public AuthService(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        // Cadastrar usuário
        public async Task<bool> CadastrarAsync(CadastroUsuarioDTO cadastro)
        {
            var email = cadastro.Email.Trim().ToLowerInvariant();

            var existe = await _context.Usuarios
                .AnyAsync(u => u.Email == email);

            if (existe)
                return false;

            var usuario = new Usuario
            {
                Nome = cadastro.Nome.Trim(),
                Email = email,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };

            usuario.SenhaHash = _passwordHasher.HashPassword(
                usuario,
                cadastro.Senha
            );

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return true;
        }

        // Realizar login
        public async Task<string?> LoginAsync(LoginDTO login)
        {
            var email = login.Email.Trim().ToLowerInvariant();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null || !usuario.Ativo)
                return null;

            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.SenhaHash,
                login.Senha
            );

            if (resultado == PasswordVerificationResult.Failed)
                return null;

            // Atualizar o hash caso o padrão antigo seja identificado
            if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
            {
                usuario.SenhaHash = _passwordHasher.HashPassword(
                    usuario,
                    login.Senha
                );

                await _context.SaveChangesAsync();
            }

            // Obter configurações JWT
            var chave = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(chave) ||
                string.IsNullOrWhiteSpace(issuer) ||
                string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException(
                    "A configuração JWT está incompleta.");
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(chave));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            // Informações do usuário no token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // Gerar token JWT
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
