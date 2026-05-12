using Microsoft.AspNetCore.Mvc;
using TccBackend.Data;
using TccBackend.Models;

namespace TccBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    // LISTAR PRODUTOS
    [HttpGet]
    public IActionResult Get()
    {
        var products = _context.Products.ToList();
        return Ok(products);
    }

    // ADICIONAR PRODUTO
    [HttpPost]
    public IActionResult Post(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();

        return Ok(product);
    }

    // DELETAR PRODUTO
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);

        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok();
    }

    // TESTE PELO NAVEGADOR
    [HttpGet("teste")]
    public IActionResult Teste()
    {
        var product = new Product
        {
            Nome = "Teclado Gamer",
            Descricao = "Teclado mecânico RGB",
            Imagem = "teclado.jpg",
            Status = "Disponível"
        };

        _context.Products.Add(product);
        _context.SaveChanges();

        return Ok(product);
    }
}