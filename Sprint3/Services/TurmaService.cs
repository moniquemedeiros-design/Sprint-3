
using Sprint3.DTOs;
using Sprint3.Models;
using Sprint3.Repositories;

namespace Sprint3.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _repository;
        private readonly IProfessorService _professorService;
        private readonly IDisciplinaService _disciplinaService;

        public TurmaService(
            ITurmaRepository repository,
            IProfessorService professorService,
            IDisciplinaService disciplinaService)
        {
            _repository = repository;
            _professorService = professorService;
            _disciplinaService = disciplinaService;
        }

        public async Task<IEnumerable<TurmaDTO>> GetAllAsync()
        {
            var turmas = await _repository.GetAllAsync();

            return turmas.Select(turma => new TurmaDTO
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Ano = turma.Ano,
                Semestre = turma.Semestre,
                ProfessorId = turma.ProfessorId,
                DisciplinaId = turma.DisciplinaId
            });
        }

        public async Task<TurmaDTO?> GetByIdAsync(int id)
        {
            var turma = await _repository.GetByIdAsync(id);

            if (turma == null)
                return null;

            return new TurmaDTO
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Ano = turma.Ano,
                Semestre = turma.Semestre,
                ProfessorId = turma.ProfessorId,
                DisciplinaId = turma.DisciplinaId
            };
        }

        public async Task AddAsync(TurmaDTO turma)
        {
            await ValidarRelacionamentosAsync(
                turma.ProfessorId,
                turma.DisciplinaId);

            var novaTurma = new Turma
            {
                Nome = turma.Nome,
                Ano = turma.Ano,
                Semestre = turma.Semestre,
                ProfessorId = turma.ProfessorId,
                DisciplinaId = turma.DisciplinaId
            };

            await _repository.AddAsync(novaTurma);

            turma.Id = novaTurma.Id;
        }

        public async Task UpdateAsync(int id, TurmaDTO turma)
        {
            var turmaExistente = await _repository.GetByIdAsync(id);

            if (turmaExistente == null)
                throw new KeyNotFoundException("Turma não encontrada.");

            await ValidarRelacionamentosAsync(
                turma.ProfessorId,
                turma.DisciplinaId);

            turmaExistente.Nome = turma.Nome;
            turmaExistente.Ano = turma.Ano;
            turmaExistente.Semestre = turma.Semestre;
            turmaExistente.ProfessorId = turma.ProfessorId;
            turmaExistente.DisciplinaId = turma.DisciplinaId;

            await _repository.UpdateAsync(turmaExistente);
        }

        private async Task ValidarRelacionamentosAsync(
            int professorId,
            int disciplinaId)
        {
            var professor = await _professorService.GetByIdAsync(professorId);

            if (professor == null)
                throw new ArgumentException(
                    "O professor informado não existe.");

            var disciplina = await _disciplinaService.GetByIdAsync(disciplinaId);

            if (disciplina == null)
                throw new ArgumentException(
                    "A disciplina informada não existe.");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var turma = await _repository.GetByIdAsync(id);

            if (turma == null)
                return false;

            await _repository.DeleteAsync(turma);

            return true;
        }
    }
}
