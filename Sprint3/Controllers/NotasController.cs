
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprint3.DTOs;
using Sprint3.Services;

namespace Sprint3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotasController : ControllerBase
    {
        private readonly INotaService _service;

        public NotasController(INotaService service)
        {
            _service = service;
        }

        // GET: api/Notas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notas = await _service.GetAllAsync();
            return Ok(notas);
        }

        // GET: api/Notas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var nota = await _service.GetByIdAsync(id);

            if (nota == null)
                return NotFound("Nota não encontrada.");

            return Ok(nota);
        }

        // POST: api/Notas
        [HttpPost]
        public async Task<IActionResult> Create(NotaDTO nota)
        {
            nota.Id = 0;

            try
            {
                await _service.AddAsync(nota);

                return Ok(new
                {
                    mensagem = "Nota cadastrada com sucesso."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    mensagem = ex.Message
                });
            }
        }

        // PUT: api/Notas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NotaDTO nota)
        {
            if (id != nota.Id)
                return BadRequest("O ID informado não corresponde ao ID da nota.");

            var existente = await _service.GetByIdAsync(id);

            if (existente == null)
                return NotFound("Nota não encontrada.");

            try
            {
                await _service.UpdateAsync(nota);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    mensagem = ex.Message
                });
            }
        }

        // DELETE: api/Notas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nota = await _service.GetByIdAsync(id);

            if (nota == null)
                return NotFound("Nota não encontrada.");

            await _service.DeleteAsync(id);

            return NoContent();
        }

        // GET: api/Notas/media/5
        [HttpGet("media/{matriculaId}")]
        public async Task<IActionResult> CalcularMedia(int matriculaId)
        {
            var media = await _service.CalcularMediaAsync(matriculaId);

            if (media == null)
                return NotFound("Não existem notas para esta matrícula.");

            return Ok(new
            {
                matriculaId,
                media
            });
        }

        // GET: api/Notas/situacao/5
        [HttpGet("situacao/{matriculaId}")]
        public async Task<IActionResult> VerificarSituacao(int matriculaId)
        {
            var situacao = await _service.VerificarSituacaoAsync(matriculaId);

            return Ok(new
            {
                matriculaId,
                situacao
            });
        }
    }
}
