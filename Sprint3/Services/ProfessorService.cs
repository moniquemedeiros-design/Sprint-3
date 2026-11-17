
using Sprint3.DTOs;
using Sprint3.Models;
using Sprint3.Repositories;

namespace Sprint3.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _repository;

        public ProfessorService(IProfessorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProfessorDTO>> GetAllAsync()
        {
            var professores = await _repository.GetAllAsync();

            return professores.Select(professor => new ProfessorDTO
            {
                Id = professor.Id,
                Nome = professor.Nome,
                CPF = professor.CPF,
                Email = professor.Email,
                Especialidade = professor.Especialidade,
                Ativo = professor.Ativo
            });
        }

        public async Task<ProfessorDTO?> GetByIdAsync(int id)
        {
            var professor = await _repository.GetByIdAsync(id);

            if (professor == null)
                return null;

            return new ProfessorDTO
            {
                Id = professor.Id,
                Nome = professor.Nome,
                CPF = professor.CPF,
                Email = professor.Email,
                Especialidade = professor.Especialidade,
                Ativo = professor.Ativo
            };
        }

        public async Task AddAsync(ProfessorDTO professor)
        {
            var novoProfessor = new Professor
            {
                Nome = professor.Nome,
                CPF = professor.CPF,
                Email = professor.Email,
                Especialidade = professor.Especialidade,
                Ativo = professor.Ativo
            };

            await _repository.AddAsync(novoProfessor);

            professor.Id = novoProfessor.Id;
        }

        public async Task UpdateAsync(int id, ProfessorDTO professor)
        {
            var professorExistente = await _repository.GetByIdAsync(id);

            if (professorExistente == null)
                throw new KeyNotFoundException("Professor não encontrado.");

            professorExistente.Nome = professor.Nome;
            professorExistente.CPF = professor.CPF;
            professorExistente.Email = professor.Email;
            professorExistente.Especialidade = professor.Especialidade;
            professorExistente.Ativo = professor.Ativo;

            await _repository.UpdateAsync(professorExistente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var professor = await _repository.GetByIdAsync(id);

            if (professor == null)
                return false;

            await _repository.DeleteAsync(professor);

            return true;
        }
    }
}
