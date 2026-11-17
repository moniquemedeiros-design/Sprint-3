using System.ComponentModel.DataAnnotations;

namespace Sprint3.DTOs
{
    public class MatriculaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O aluno é obrigatório.")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "A turma é obrigatória.")]
        public int TurmaId { get; set; }

        public DateTime DataMatricula { get; set; }

        public bool Ativa { get; set; }
    }
}