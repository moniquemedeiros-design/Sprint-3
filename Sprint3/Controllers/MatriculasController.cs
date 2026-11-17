
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sprint3.DTOs;
using Sprint3.Services;

namespace Sprint3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MatriculasController : ControllerBase
    {
        private readonly IMatriculaService _service;

        public MatriculasController(IMatriculaService service)
        {
            _service = service;
        }

        // GET: api/Matriculas
        [HttpGet]
        public async Task<ActionResult<List<MatriculaDTO>>> GetAll()
        {
            var matriculas = await _service.GetAllAsync();
            return Ok(matriculas);
        }

        // GET: api/Matriculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatriculaDTO>> GetById(int id)
        {
            var matricula = await _service.GetByIdAsync(id);

            if (matricula == null)
                return NotFound();

            return Ok(matricula);
        }

        // POST: api/Matriculas
        [HttpPost]
        public async Task<ActionResult<MatriculaDTO>> Create(MatriculaDTO matricula)
        {
            matricula.Id = 0;

            try
            {
                await _service.AddAsync(matricula);

                return Ok(new
                {
                    mensagem = "Matrícula cadastrada com sucesso."
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

        // PUT: api/Matriculas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MatriculaDTO matricula)
        {
            if (id != matricula.Id)
                return BadRequest();

            var existente = await _service.GetByIdAsync(id);

            if (existente == null)
                return NotFound();

            try
            {
                await _service.UpdateAsync(matricula);
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

        // DELETE: api/Matriculas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var matricula = await _service.GetByIdAsync(id);

            if (matricula == null)
                return NotFound();

            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
