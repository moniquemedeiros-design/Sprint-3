
using Sprint3.DTOs;
using Sprint3.Models;
using Sprint3.Repositories;

namespace Sprint3.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _repository;
        private readonly IAlunoService _alunoService;
        private readonly ITurmaService _turmaService;

        public MatriculaService(
            IMatriculaRepository repository,
            IAlunoService alunoService,
            ITurmaService turmaService)
        {
            _repository = repository;
            _alunoService = alunoService;
            _turmaService = turmaService;
        }

        public async Task<List<MatriculaDTO>> GetAllAsync()
        {
            var matriculas = await _repository.GetAllAsync();

            return matriculas.Select(m => new MatriculaDTO
            {
                Id = m.Id,
                AlunoId = m.AlunoId,
                TurmaId = m.TurmaId,
                DataMatricula = m.DataMatricula,
                Ativa = m.Ativa
            }).ToList();
        }

        public async Task<MatriculaDTO?> GetByIdAsync(int id)
        {
            var m = await _repository.GetByIdAsync(id);

            if (m == null)
                return null;

            return new MatriculaDTO
            {
                Id = m.Id,
                AlunoId = m.AlunoId,
                TurmaId = m.TurmaId,
                DataMatricula = m.DataMatricula,
                Ativa = m.Ativa
            };
        }

        public async Task AddAsync(MatriculaDTO matricula)
        {
            await ValidarRelacionamentosAsync(
                matricula.AlunoId,
                matricula.TurmaId);

            var novaMatricula = new Matricula
            {
                AlunoId = matricula.AlunoId,
                TurmaId = matricula.TurmaId,
                DataMatricula = matricula.DataMatricula,
                Ativa = matricula.Ativa
            };

            await _repository.AddAsync(novaMatricula);

            matricula.Id = novaMatricula.Id;
        }

        public async Task UpdateAsync(MatriculaDTO matricula)
        {
            var matriculaExistente =
                await _repository.GetByIdAsync(matricula.Id);

            if (matriculaExistente == null)
                throw new KeyNotFoundException(
                    "Matrícula não encontrada.");

            await ValidarRelacionamentosAsync(
                matricula.AlunoId,
                matricula.TurmaId);

            matriculaExistente.AlunoId = matricula.AlunoId;
            matriculaExistente.TurmaId = matricula.TurmaId;
            matriculaExistente.DataMatricula = matricula.DataMatricula;
            matriculaExistente.Ativa = matricula.Ativa;

            await _repository.UpdateAsync(matriculaExistente);
        }

        private async Task ValidarRelacionamentosAsync(
            int alunoId,
            int turmaId)
        {
            var aluno = await _alunoService.GetByIdAsync(alunoId);

            if (aluno == null)
                throw new ArgumentException(
                    "O aluno informado não existe.");

            var turma = await _turmaService.GetByIdAsync(turmaId);

            if (turma == null)
                throw new ArgumentException(
                    "A turma informada não existe.");
        }

        public async Task DeleteAsync(int id)
        {
            var matricula = await _repository.GetByIdAsync(id);

            if (matricula != null)
            {
                await _repository.DeleteAsync(matricula);
            }
        }

        public async Task<bool> ExisteAsync(int id)
        {
            var matricula = await _repository.GetByIdAsync(id);

            return matricula != null;
        }
    }
}
