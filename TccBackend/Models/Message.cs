namespace TccBackend.Models;

public class Message
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Conteudo { get; set; } = string.Empty;

    public DateTime Data { get; set; } = DateTime.Now;

    public bool Lida { get; set; } = false;
}