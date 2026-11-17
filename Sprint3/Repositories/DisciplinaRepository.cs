
using Microsoft.EntityFrameworkCore;
using Sprint3.Data;
using Sprint3.Models;

namespace Sprint3.Repositories
{
    public class DisciplinaRepository : IDisciplinaRepository
    {
        private readonly AppDbContext _context;

        public DisciplinaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Listar todas as disciplinas
        public async Task<List<Disciplina>> GetAllAsync()
        {
            return await _context.Disciplinas
                .AsNoTracking()
                .ToListAsync();
        }

        // Buscar disciplina pelo ID
        public async Task<Disciplina?> GetByIdAsync(int id)
        {
            return await _context.Disciplinas.FindAsync(id);
        }

        // Cadastrar disciplina
        public async Task AddAsync(Disciplina disciplina)
        {
            await _context.Disciplinas.AddAsync(disciplina);
            await _context.SaveChangesAsync();
        }

        // Atualizar disciplina
        public async Task UpdateAsync(Disciplina disciplina)
        {
            var disciplinaExistente = await _context.Disciplinas
                .FindAsync(disciplina.Id);

            if (disciplinaExistente != null)
            {
                disciplinaExistente.Nome = disciplina.Nome;
                disciplinaExistente.Descricao = disciplina.Descricao;
                disciplinaExistente.CargaHoraria = disciplina.CargaHoraria;
                disciplinaExistente.Ativa = disciplina.Ativa;

                await _context.SaveChangesAsync();
            }
        }

        // Excluir disciplina
        public async Task DeleteAsync(Disciplina disciplina)
        {
            _context.Disciplinas.Remove(disciplina);
            await _context.SaveChangesAsync();
        }
    }
}
