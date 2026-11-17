
using Sprint3.DTOs;

namespace Sprint3.Services
{
    public interface IDisciplinaService
    {
        Task<IEnumerable<DisciplinaDTO>> GetAllAsync();

        Task<DisciplinaDTO?> GetByIdAsync(int id);

        Task AddAsync(DisciplinaDTO disciplina);

        Task UpdateAsync(int id, DisciplinaDTO disciplina);

        Task<bool> DeleteAsync(int id);
    }
}
