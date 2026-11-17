
using Sprint3.DTOs;

namespace Sprint3.Services
{
    public interface ITurmaService
    {
        Task<IEnumerable<TurmaDTO>> GetAllAsync();

        Task<TurmaDTO?> GetByIdAsync(int id);

        Task AddAsync(TurmaDTO turma);

        Task UpdateAsync(int id, TurmaDTO turma);

        Task<bool> DeleteAsync(int id);
    }
}
