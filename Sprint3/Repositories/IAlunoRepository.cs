using Sprint3.Models;

namespace Sprint3.Repositories
{
    public interface IAlunoRepository
    {
        Task<IEnumerable<Aluno>> GetAllAsync();

        Task<Aluno?> GetByIdAsync(int id);

        Task AddAsync(Aluno aluno);

        Task UpdateAsync(Aluno aluno);

        Task DeleteAsync(Aluno aluno);
    }
}