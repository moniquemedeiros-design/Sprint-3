
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprint3.DTOs;
using Sprint3.Services;

namespace Sprint3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/cadastrar
        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar(
            [FromBody] CadastroUsuarioDTO cadastro)
        {
            var cadastrado = await _authService.CadastrarAsync(cadastro);

            if (!cadastrado)
            {
                return Conflict(new
                {
                    mensagem = "Este e-mail já está cadastrado."
                });
            }

            return Ok(new
            {
                mensagem = "Usuário cadastrado com sucesso."
            });
        }

        // POST: api/Auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDTO login)
        {
            var token = await _authService.LoginAsync(login);

            if (token == null)
            {
                return Unauthorized(new
                {
                    mensagem = "E-mail ou senha inválidos."
                });
            }

            return Ok(new
            {
                mensagem = "Login realizado com sucesso.",
                token = token
            });
        }
    }
}
