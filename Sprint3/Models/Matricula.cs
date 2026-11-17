
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint3.Models
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AlunoId { get; set; }

        [ForeignKey("AlunoId")]
        public Aluno? Aluno { get; set; }

        [Required]
        public int TurmaId { get; set; }

        [ForeignKey("TurmaId")]
        public Turma? Turma { get; set; }

        public DateTime DataMatricula { get; set; } = DateTime.UtcNow;

        public bool Ativa { get; set; } = true;

        public ICollection<Nota> Notas { get; set; } = new List<Nota>();
    }
}
