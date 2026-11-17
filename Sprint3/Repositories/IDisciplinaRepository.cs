
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public interface IDisciplinaRepository
    {
        Task<List<Disciplina>> GetAllAsync();
        Task<Disciplina?> GetByIdAsync(int id);
        Task AddAsync(Disciplina disciplina);
        Task UpdateAsync(Disciplina disciplina);
        Task DeleteAsync(Disciplina disciplina);
    }
}
