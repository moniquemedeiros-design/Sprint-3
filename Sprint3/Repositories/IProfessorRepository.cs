
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public interface IProfessorRepository
    {
        Task<List<Professor>> GetAllAsync();

        Task<Professor?> GetByIdAsync(int id);

        Task AddAsync(Professor professor);

        Task UpdateAsync(Professor professor);

        Task DeleteAsync(Professor professor);
    }
}
