
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprint3.DTOs;
using Sprint3.Services;

namespace Sprint3.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DisciplinasController : ControllerBase
    {
        private readonly IDisciplinaService _service;

        public DisciplinasController(IDisciplinaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var disciplinas = await _service.GetAllAsync();

            return Ok(disciplinas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var disciplina = await _service.GetByIdAsync(id);

            if (disciplina == null)
                return NotFound("Disciplina não encontrada.");

            return Ok(disciplina);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DisciplinaDTO disciplina)
        {
            await _service.AddAsync(disciplina);

            return CreatedAtAction(
                nameof(GetById),
                new { id = disciplina.Id },
                disciplina
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DisciplinaDTO disciplina)
        {
            if (id != disciplina.Id)
                return BadRequest("O ID informado não corresponde ao ID da disciplina.");

            try
            {
                await _service.UpdateAsync(id, disciplina);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Disciplina não encontrada.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var excluida = await _service.DeleteAsync(id);

            if (!excluida)
                return NotFound("Disciplina não encontrada.");

            return NoContent();
        }
    }
}
