using System.ComponentModel.DataAnnotations;

namespace Sprint3.DTOs
{
    public class TurmaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da turma é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Range(2000, 2100)]
        public int Ano { get; set; }

        [Range(1, 2)]
        public int Semestre { get; set; }

        [Required]
        public int ProfessorId { get; set; }

        [Required]
        public int DisciplinaId { get; set; }
    }
}