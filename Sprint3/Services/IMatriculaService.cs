
using Sprint3.DTOs;

namespace Sprint3.Services
{
    public interface IMatriculaService
    {
        Task<List<MatriculaDTO>> GetAllAsync();
        Task<MatriculaDTO?> GetByIdAsync(int id);
        Task AddAsync(MatriculaDTO matricula);
        Task UpdateAsync(MatriculaDTO matricula);
        Task DeleteAsync(int id);

        Task<bool> ExisteAsync(int id);
    }
}
