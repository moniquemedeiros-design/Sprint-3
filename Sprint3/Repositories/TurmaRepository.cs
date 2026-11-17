
using Microsoft.EntityFrameworkCore;
using Sprint3.Data;
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly AppDbContext _context;

        public TurmaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as turmas
        public async Task<List<Turma>> GetAllAsync()
        {
            return await _context.Turmas
                .AsNoTracking()
                .Include(t => t.Professor)
                .Include(t => t.Disciplina)
                .ToListAsync();
        }

        // Buscar turma pelo ID
        public async Task<Turma?> GetByIdAsync(int id)
        {
            return await _context.Turmas
                .AsNoTracking()
                .Include(t => t.Professor)
                .Include(t => t.Disciplina)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Cadastrar turma
        public async Task AddAsync(Turma turma)
        {
            await _context.Turmas.AddAsync(turma);
            await _context.SaveChangesAsync();
        }

        // Atualizar turma
        public async Task UpdateAsync(Turma turma)
        {
            var turmaExistente = await _context.Turmas
                .FindAsync(turma.Id);

            if (turmaExistente != null)
            {
                turmaExistente.Nome = turma.Nome;
                turmaExistente.Ano = turma.Ano;
                turmaExistente.Semestre = turma.Semestre;
                turmaExistente.ProfessorId = turma.ProfessorId;
                turmaExistente.DisciplinaId = turma.DisciplinaId;

                await _context.SaveChangesAsync();
            }
        }

        // Excluir turma
        public async Task DeleteAsync(Turma turma)
        {
            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();
        }
    }
}
