using Microsoft.EntityFrameworkCore;
using Sprint3.Data;
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AppDbContext _context;

        public AlunoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos os alunos
        public async Task<IEnumerable<Aluno>> GetAllAsync()
        {
            return await _context.Alunos
                .AsNoTracking()
                .ToListAsync();
        }

        // Buscar aluno pelo ID
        public async Task<Aluno?> GetByIdAsync(int id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        // Cadastrar aluno
        public async Task AddAsync(Aluno aluno)
        {
            await _context.Alunos.AddAsync(aluno);
            await _context.SaveChangesAsync();
        }

        // Atualizar aluno
        public async Task UpdateAsync(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
            await _context.SaveChangesAsync();
        }

        // Excluir aluno
        public async Task DeleteAsync(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
        }
    }
}