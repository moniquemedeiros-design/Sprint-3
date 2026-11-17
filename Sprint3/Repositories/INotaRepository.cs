
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public interface INotaRepository
    {
        Task<List<Nota>> GetAllAsync();
        Task<Nota?> GetByIdAsync(int id);
        Task<List<Nota>> GetByMatriculaIdAsync(int matriculaId);
        Task AddAsync(Nota nota);
        Task UpdateAsync(Nota nota);
        Task DeleteAsync(Nota nota);
    }
}
