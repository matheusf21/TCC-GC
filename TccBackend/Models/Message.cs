namespace TccBackend.Models;

public class Message
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Conteudo { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;
    public bool Lida { get; set; } = false;
}