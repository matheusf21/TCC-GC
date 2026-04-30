namespace TccBackend.Models;

public class Product
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Imagem { get; set; }
    public string Status { get; set; } // normal, mais_vendido, fora_estoque
}