
using Microsoft.EntityFrameworkCore;
using Sprint3.Data;
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly AppDbContext _context;

        public ProfessorRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos os professores
        public async Task<List<Professor>> GetAllAsync()
        {
            return await _context.Professores
                .AsNoTracking()
                .ToListAsync();
        }

        // Buscar professor pelo ID
        public async Task<Professor?> GetByIdAsync(int id)
        {
            return await _context.Professores.FindAsync(id);
        }

        // Cadastrar professor
        public async Task AddAsync(Professor professor)
        {
            await _context.Professores.AddAsync(professor);
            await _context.SaveChangesAsync();
        }

        // Atualizar professor
        public async Task UpdateAsync(Professor professor)
        {
            var professorExistente = await _context.Professores
                .FindAsync(professor.Id);

            if (professorExistente != null)
            {
                professorExistente.Nome = professor.Nome;
                professorExistente.CPF = professor.CPF;
                professorExistente.Email = professor.Email;
                professorExistente.Especialidade = professor.Especialidade;
                professorExistente.Ativo = professor.Ativo;

                await _context.SaveChangesAsync();
            }
        }

        // Excluir professor
        public async Task DeleteAsync(Professor professor)
        {
            _context.Professores.Remove(professor);
            await _context.SaveChangesAsync();
        }
    }
}
