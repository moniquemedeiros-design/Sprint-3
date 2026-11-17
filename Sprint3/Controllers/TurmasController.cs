
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprint3.DTOs;
using Sprint3.Services;

namespace Sprint3.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaService _service;

        public TurmasController(ITurmaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var turmas = await _service.GetAllAsync();
            return Ok(turmas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var turma = await _service.GetByIdAsync(id);

            if (turma == null)
                return NotFound("Turma não encontrada.");

            return Ok(turma);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TurmaDTO turma)
        {
            try
            {
                await _service.AddAsync(turma);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = turma.Id },
                    turma
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TurmaDTO turma)
        {
            if (id != turma.Id)
                return BadRequest("O ID informado não corresponde ao ID da turma.");

            try
            {
                await _service.UpdateAsync(id, turma);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Turma não encontrada.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var excluida = await _service.DeleteAsync(id);

            if (!excluida)
                return NotFound("Turma não encontrada.");

            return NoContent();
        }
    }
}
