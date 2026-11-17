using Sprint3.DTOs;

namespace Sprint3.Services
{
    public interface IAlunoService
    {
        Task<IEnumerable<AlunoDTO>> GetAllAsync();

        Task<AlunoDTO?> GetByIdAsync(int id);

        Task AddAsync(AlunoDTO aluno);

        Task UpdateAsync(int id, AlunoDTO aluno);

        Task<bool> DeleteAsync(int id);
    }
}