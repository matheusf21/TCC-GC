using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabrielConstrucaoMVC.Models
{
    public class RegistroFaturamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "O valor deve ser maior que zero.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateTime DataRegistro { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? Descricao { get; set; }

        [StringLength(30)]
        public string? FormaPagamento { get; set; }
    }
}