
using Microsoft.EntityFrameworkCore;
using Sprint3.Data;
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public class NotaRepository : INotaRepository
    {
        private readonly AppDbContext _context;

        public NotaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as notas
        public async Task<List<Nota>> GetAllAsync()
        {
            return await _context.Notas
                .AsNoTracking()
                .Include(n => n.Matricula)
                    .ThenInclude(m => m!.Aluno)
                .Include(n => n.Matricula)
                    .ThenInclude(m => m!.Turma)
                .ToListAsync();
        }

        // Buscar nota pelo ID
        public async Task<Nota?> GetByIdAsync(int id)
        {
            return await _context.Notas
                .AsNoTracking()
                .Include(n => n.Matricula)
                    .ThenInclude(m => m!.Aluno)
                .Include(n => n.Matricula)
                    .ThenInclude(m => m!.Turma)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        // Buscar notas pelo ID da matrícula
        public async Task<List<Nota>> GetByMatriculaIdAsync(int matriculaId)
        {
            return await _context.Notas
                .AsNoTracking()
                .Where(n => n.MatriculaId == matriculaId)
                .OrderBy(n => n.DataLancamento)
                .ToListAsync();
        }

        // Cadastrar nota
        public async Task AddAsync(Nota nota)
        {
            await _context.Notas.AddAsync(nota);
            await _context.SaveChangesAsync();
        }

        // Atualizar nota
        public async Task UpdateAsync(Nota nota)
        {
            var notaExistente = await _context.Notas
                .FindAsync(nota.Id);

            if (notaExistente != null)
            {
                notaExistente.MatriculaId = nota.MatriculaId;
                notaExistente.Valor = nota.Valor;
                notaExistente.Descricao = nota.Descricao;
                notaExistente.DataLancamento = nota.DataLancamento;

                await _context.SaveChangesAsync();
            }
        }

        // Excluir nota
        public async Task DeleteAsync(Nota nota)
        {
            _context.Notas.Remove(nota);
            await _context.SaveChangesAsync();
        }
    }
}
