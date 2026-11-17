
using Sprint3.DTOs;
using Sprint3.Models;
using Sprint3.Repositories;

namespace Sprint3.Services
{
    public class DisciplinaService : IDisciplinaService
    {
        private readonly IDisciplinaRepository _repository;

        public DisciplinaService(IDisciplinaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DisciplinaDTO>> GetAllAsync()
        {
            var disciplinas = await _repository.GetAllAsync();

            return disciplinas.Select(disciplina => new DisciplinaDTO
            {
                Id = disciplina.Id,
                Nome = disciplina.Nome,
                Descricao = disciplina.Descricao,
                CargaHoraria = disciplina.CargaHoraria,
                Ativa = disciplina.Ativa
            });
        }

        public async Task<DisciplinaDTO?> GetByIdAsync(int id)
        {
            var disciplina = await _repository.GetByIdAsync(id);

            if (disciplina == null)
                return null;

            return new DisciplinaDTO
            {
                Id = disciplina.Id,
                Nome = disciplina.Nome,
                Descricao = disciplina.Descricao,
                CargaHoraria = disciplina.CargaHoraria,
                Ativa = disciplina.Ativa
            };
        }

        public async Task AddAsync(DisciplinaDTO disciplina)
        {
            var novaDisciplina = new Disciplina
            {
                Nome = disciplina.Nome,
                Descricao = disciplina.Descricao,
                CargaHoraria = disciplina.CargaHoraria,
                Ativa = disciplina.Ativa
            };

            await _repository.AddAsync(novaDisciplina);

            disciplina.Id = novaDisciplina.Id;
        }

        public async Task UpdateAsync(int id, DisciplinaDTO disciplina)
        {
            var disciplinaExistente = await _repository.GetByIdAsync(id);

            if (disciplinaExistente == null)
                throw new KeyNotFoundException("Disciplina não encontrada.");

            disciplinaExistente.Nome = disciplina.Nome;
            disciplinaExistente.Descricao = disciplina.Descricao;
            disciplinaExistente.CargaHoraria = disciplina.CargaHoraria;
            disciplinaExistente.Ativa = disciplina.Ativa;

            await _repository.UpdateAsync(disciplinaExistente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var disciplina = await _repository.GetByIdAsync(id);

            if (disciplina == null)
                return false;

            await _repository.DeleteAsync(disciplina);

            return true;
        }
    }
}
