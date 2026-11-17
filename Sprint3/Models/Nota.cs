
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint3.Models
{
    public class Nota
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MatriculaId { get; set; }

        [ForeignKey("MatriculaId")]
        public Matricula? Matricula { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Valor { get; set; }

        [StringLength(100)]
        public string? Descricao { get; set; }

        public DateTime DataLancamento { get; set; } = DateTime.UtcNow;
    }
}
