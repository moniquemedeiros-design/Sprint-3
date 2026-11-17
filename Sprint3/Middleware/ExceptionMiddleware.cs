
using System.Net;
using System.Text.Json;

namespace Sprint3.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro inesperado na API.");

                context.Response.Clear();
                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";

                var resposta = new
                {
                    status = context.Response.StatusCode,
                    mensagem = "Ocorreu um erro interno. Tente novamente mais tarde."
                };

                var json = JsonSerializer.Serialize(resposta);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
