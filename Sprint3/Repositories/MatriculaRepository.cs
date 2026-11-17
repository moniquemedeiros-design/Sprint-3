
using Microsoft.EntityFrameworkCore;
using Sprint3.Data;
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly AppDbContext _context;

        public MatriculaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as matrículas
        public async Task<List<Matricula>> GetAllAsync()
        {
            return await _context.Matriculas
                .AsNoTracking()
                .Include(m => m.Aluno)
                .Include(m => m.Turma)
                    .ThenInclude(t => t!.Professor)
                .Include(m => m.Turma)
                    .ThenInclude(t => t!.Disciplina)
                .ToListAsync();
        }

        // Buscar matrícula pelo ID
        public async Task<Matricula?> GetByIdAsync(int id)
        {
            return await _context.Matriculas
                .AsNoTracking()
                .Include(m => m.Aluno)
                .Include(m => m.Turma)
                    .ThenInclude(t => t!.Professor)
                .Include(m => m.Turma)
                    .ThenInclude(t => t!.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // Cadastrar matrícula
        public async Task AddAsync(Matricula matricula)
        {
            await _context.Matriculas.AddAsync(matricula);
            await _context.SaveChangesAsync();
        }

        // Atualizar matrícula
        public async Task UpdateAsync(Matricula matricula)
        {
            var matriculaExistente = await _context.Matriculas
                .FindAsync(matricula.Id);

            if (matriculaExistente != null)
            {
                matriculaExistente.AlunoId = matricula.AlunoId;
                matriculaExistente.TurmaId = matricula.TurmaId;
                matriculaExistente.DataMatricula = matricula.DataMatricula;
                matriculaExistente.Ativa = matricula.Ativa;

                await _context.SaveChangesAsync();
            }
        }

        // Excluir matrícula
        public async Task DeleteAsync(Matricula matricula)
        {
            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();
        }
    }
}
