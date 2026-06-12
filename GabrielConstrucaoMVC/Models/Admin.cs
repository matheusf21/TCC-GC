using System.ComponentModel.DataAnnotations;

namespace GabrielConstrucaoMVC.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}