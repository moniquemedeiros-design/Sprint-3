
using Sprint3.DTOs;

namespace Sprint3.Services
{
    public interface IProfessorService
    {
        Task<IEnumerable<ProfessorDTO>> GetAllAsync();

        Task<ProfessorDTO?> GetByIdAsync(int id);

        Task AddAsync(ProfessorDTO professor);

        Task UpdateAsync(int id, ProfessorDTO professor);

        Task<bool> DeleteAsync(int id);
    }
}
