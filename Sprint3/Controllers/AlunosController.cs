
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Sprint3.DTOs;
using Sprint3.Services;

namespace Sprint3.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _service;

        public AlunosController(IAlunoService service)
        {
            _service = service;
        }

        // GET: api/Alunos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alunos = await _service.GetAllAsync();

            return Ok(alunos);
        }

        // GET: api/Alunos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aluno = await _service.GetByIdAsync(id);

            if (aluno == null)
                return NotFound(new
                {
                    mensagem = "Aluno não encontrado."
                });

            return Ok(aluno);
        }

        // POST: api/Alunos
        [HttpPost]
        public async Task<IActionResult> Create(AlunoDTO aluno)
        {
            await _service.AddAsync(aluno);

            return CreatedAtAction(
                nameof(GetById),
                new { id = aluno.Id },
                aluno
            );
        }

        // PUT: api/Alunos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AlunoDTO aluno)
        {
            try
            {
                await _service.UpdateAsync(id, aluno);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    mensagem = "Aluno não encontrado."
                });
            }
        }

        // DELETE: api/Alunos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var excluido = await _service.DeleteAsync(id);

                if (!excluido)
                {
                    return NotFound(new
                    {
                        mensagem = "Aluno não encontrado."
                    });
                }

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    mensagem = "Não é possível excluir este aluno porque existem matrículas ou outros registros vinculados a ele. Você pode desativar o aluno em vez de excluí-lo."
                });
            }
        }
    }
}
