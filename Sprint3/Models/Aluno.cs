using System.ComponentModel.DataAnnotations;

namespace Sprint3.Models
{
    public class Aluno
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

        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        [StringLength(20)]
        public string Matricula { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; } = true;
    }
}