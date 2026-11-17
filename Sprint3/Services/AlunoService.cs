
using Sprint3.DTOs;
using Sprint3.Models;
using Sprint3.Repositories;

namespace Sprint3.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _repository;

        public AlunoService(IAlunoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AlunoDTO>> GetAllAsync()
        {
            var alunos = await _repository.GetAllAsync();

            return alunos.Select(aluno => new AlunoDTO
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                CPF = aluno.CPF,
                Email = aluno.Email,
                Matricula = aluno.Matricula,
                DataNascimento = aluno.DataNascimento,
                Ativo = aluno.Ativo
            });
        }

        public async Task<AlunoDTO?> GetByIdAsync(int id)
        {
            var aluno = await _repository.GetByIdAsync(id);

            if (aluno == null)
                return null;

            return new AlunoDTO
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                CPF = aluno.CPF,
                Email = aluno.Email,
                Matricula = aluno.Matricula,
                DataNascimento = aluno.DataNascimento,
                Ativo = aluno.Ativo
            };
        }

        public async Task AddAsync(AlunoDTO aluno)
        {
            var novoAluno = new Aluno
            {
                Nome = aluno.Nome,
                CPF = aluno.CPF,
                Email = aluno.Email,
                Matricula = aluno.Matricula,
                DataNascimento = aluno.DataNascimento,
                Ativo = aluno.Ativo
            };

            await _repository.AddAsync(novoAluno);

            aluno.Id = novoAluno.Id;
        }

        public async Task UpdateAsync(int id, AlunoDTO aluno)
        {
            var alunoExistente = await _repository.GetByIdAsync(id);

            if (alunoExistente == null)
                throw new KeyNotFoundException("Aluno não encontrado.");

            alunoExistente.Nome = aluno.Nome;
            alunoExistente.CPF = aluno.CPF;
            alunoExistente.Email = aluno.Email;
            alunoExistente.Matricula = aluno.Matricula;
            alunoExistente.DataNascimento = aluno.DataNascimento;
            alunoExistente.Ativo = aluno.Ativo;

            await _repository.UpdateAsync(alunoExistente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var aluno = await _repository.GetByIdAsync(id);

            if (aluno == null)
                return false;

            await _repository.DeleteAsync(aluno);

            return true;
        }
    }
}
