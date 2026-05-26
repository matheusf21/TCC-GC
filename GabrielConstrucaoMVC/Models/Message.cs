using System;
using System.ComponentModel.DataAnnotations;

namespace GabrielConstrucaoMVC.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Assunto { get; set; } = string.Empty;

        [Required]
        public string Conteudo { get; set; } = string.Empty;

        public DateTime DataEnvio { get; set; } = DateTime.Now;
    }
}