using System.ComponentModel.DataAnnotations;

namespace Sprint3.Models
{
    public class Professor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14)]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}