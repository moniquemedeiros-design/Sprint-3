
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint3.Models
{
    public class Turma
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da turma é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(2000, 2100, ErrorMessage = "Informe um ano válido.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "O semestre é obrigatório.")]
        [Range(1, 2, ErrorMessage = "O semestre deve ser 1 ou 2.")]
        public int Semestre { get; set; }

        // Relacionamento com Professor
        [ForeignKey("Professor")]
        public int ProfessorId { get; set; }

        public Professor? Professor { get; set; }

        // Relacionamento com Disciplina
        [ForeignKey("Disciplina")]
        public int DisciplinaId { get; set; }

        public Disciplina? Disciplina { get; set; }
    }
}
