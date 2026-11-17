using System.ComponentModel.DataAnnotations;

namespace Sprint3.DTOs
{
    public class NotaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        public int MatriculaId { get; set; }

        [Required(ErrorMessage = "O valor da nota é obrigatório.")]
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public decimal Valor { get; set; }

        [StringLength(100)]
        public string? Descricao { get; set; }

        public DateTime DataLancamento { get; set; }
    }
}