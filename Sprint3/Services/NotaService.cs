
using Sprint3.DTOs;
using Sprint3.Models;
using Sprint3.Repositories;

namespace Sprint3.Services
{
    public class NotaService : INotaService
    {
        private readonly INotaRepository _repository;
        private readonly IMatriculaService _matriculaService;

        public NotaService(
            INotaRepository repository,
            IMatriculaService matriculaService)
        {
            _repository = repository;
            _matriculaService = matriculaService;
        }

        public async Task<List<NotaDTO>> GetAllAsync()
        {
            var notas = await _repository.GetAllAsync();

            return notas.Select(n => new NotaDTO
            {
                Id = n.Id,
                MatriculaId = n.MatriculaId,
                Valor = n.Valor,
                Descricao = n.Descricao,
                DataLancamento = n.DataLancamento
            }).ToList();
        }

        public async Task<NotaDTO?> GetByIdAsync(int id)
        {
            var n = await _repository.GetByIdAsync(id);

            if (n == null)
                return null;

            return new NotaDTO
            {
                Id = n.Id,
                MatriculaId = n.MatriculaId,
                Valor = n.Valor,
                Descricao = n.Descricao,
                DataLancamento = n.DataLancamento
            };
        }

        public async Task AddAsync(NotaDTO nota)
        {
            var matriculaExiste =
                await _matriculaService.ExisteAsync(nota.MatriculaId);

            if (!matriculaExiste)
                throw new ArgumentException(
                    "A matrícula informada não existe.");

            var novaNota = new Nota
            {
                MatriculaId = nota.MatriculaId,
                Valor = nota.Valor,
                Descricao = nota.Descricao,
                DataLancamento = nota.DataLancamento
            };

            await _repository.AddAsync(novaNota);

            nota.Id = novaNota.Id;
        }

        public async Task UpdateAsync(NotaDTO nota)
        {
            var notaExistente =
                await _repository.GetByIdAsync(nota.Id);

            if (notaExistente == null)
                throw new KeyNotFoundException(
                    "Nota não encontrada.");

            var matriculaExiste =
                await _matriculaService.ExisteAsync(nota.MatriculaId);

            if (!matriculaExiste)
                throw new ArgumentException(
                    "A matrícula informada não existe.");

            notaExistente.MatriculaId = nota.MatriculaId;
            notaExistente.Valor = nota.Valor;
            notaExistente.Descricao = nota.Descricao;
            notaExistente.DataLancamento = nota.DataLancamento;

            await _repository.UpdateAsync(notaExistente);
        }

        public async Task DeleteAsync(int id)
        {
            var nota = await _repository.GetByIdAsync(id);

            if (nota != null)
            {
                await _repository.DeleteAsync(nota);
            }
        }

        public async Task<decimal?> CalcularMediaAsync(int matriculaId)
        {
            var notas =
                await _repository.GetByMatriculaIdAsync(matriculaId);

            if (notas == null || notas.Count == 0)
                return null;

            return Math.Round(notas.Average(n => n.Valor), 2);
        }

        public async Task<string> VerificarSituacaoAsync(int matriculaId)
        {
            var media = await CalcularMediaAsync(matriculaId);

            if (media == null)
                return "Sem notas";

            if (media >= 7)
                return "Aprovado";

            if (media >= 5)
                return "Recuperação";

            return "Reprovado";
        }
    }
}
