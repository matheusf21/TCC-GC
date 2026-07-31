using System.ComponentModel.DataAnnotations;

namespace GabrielConstrucaoMVC.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
        public string Categoria { get; set; } = string.Empty;

        public string? NomeImagem { get; set; }

        public bool MaisVendido { get; set; }

        public bool ForaDeEstoque { get; set; }
    }
}