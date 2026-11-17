
using System.ComponentModel.DataAnnotations;

namespace Sprint3.Models
{
    public class Disciplina
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da disciplina é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(300)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A carga horária é obrigatória.")]
        [Range(1, 1000, ErrorMessage = "A carga horária deve estar entre 1 e 1000 horas.")]
        public int CargaHoraria { get; set; }

        public bool Ativa { get; set; } = true;
    }
}
