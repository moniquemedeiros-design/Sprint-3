
using System.ComponentModel.DataAnnotations;

namespace Sprint3.DTOs
{
    public class CadastroUsuarioDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Senha { get; set; } = string.Empty;
    }
}
