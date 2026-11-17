
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public interface IMatriculaRepository
    {
        Task<List<Matricula>> GetAllAsync();
        Task<Matricula?> GetByIdAsync(int id);
        Task AddAsync(Matricula matricula);
        Task UpdateAsync(Matricula matricula);
        Task DeleteAsync(Matricula matricula);
    }
}
