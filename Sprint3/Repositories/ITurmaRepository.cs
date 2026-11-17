using Sprint3.Models;

namespace Sprint3.Repositories
{
    public interface ITurmaRepository
    {
        Task<List<Turma>> GetAllAsync();
        Task<Turma?> GetByIdAsync(int id);
        Task AddAsync(Turma turma);
        Task UpdateAsync(Turma turma);
        Task DeleteAsync(Turma turma);
    }
}