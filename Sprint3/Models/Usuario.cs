
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sprint3.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonIgnore]
        public string SenhaHash { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}