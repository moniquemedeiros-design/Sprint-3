
using Sprint3.DTOs;

namespace Sprint3.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDTO login);

        Task<bool> CadastrarAsync(CadastroUsuarioDTO cadastro);
    }
}
