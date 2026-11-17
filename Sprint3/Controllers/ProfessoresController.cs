
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprint3.DTOs;
using Sprint3.Services;

namespace Sprint3.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessoresController : ControllerBase
    {
        private readonly IProfessorService _service;

        public ProfessoresController(IProfessorService service)
        {
            _service = service;
        }

        // GET: api/Professores
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var professores = await _service.GetAllAsync();

            return Ok(professores);
        }

        // GET: api/Professores/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var professor = await _service.GetByIdAsync(id);

            if (professor == null)
                return NotFound(new { mensagem = "Professor não encontrado." });

            return Ok(professor);
        }

        // POST: api/Professores
        [HttpPost]
        public async Task<IActionResult> Create(ProfessorDTO professor)
        {
            await _service.AddAsync(professor);

            return CreatedAtAction(
                nameof(GetById),
                new { id = professor.Id },
                professor
            );
        }

        // PUT: api/Professores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProfessorDTO professor)
        {
            try
            {
                await _service.UpdateAsync(id, professor);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensagem = "Professor não encontrado." });
            }
        }

        // DELETE: api/Professores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var excluido = await _service.DeleteAsync(id);

            if (!excluido)
                return NotFound(new { mensagem = "Professor não encontrado." });

            return NoContent();
        }
    }
}
