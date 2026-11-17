
using Sprint3.DTOs;

namespace Sprint3.Services
{
    public interface INotaService
    {
        Task<List<NotaDTO>> GetAllAsync();
        Task<NotaDTO?> GetByIdAsync(int id);
        Task AddAsync(NotaDTO nota);
        Task UpdateAsync(NotaDTO nota);
        Task DeleteAsync(int id);

        Task<decimal?> CalcularMediaAsync(int matriculaId);
        Task<string> VerificarSituacaoAsync(int matriculaId);
    }
}
