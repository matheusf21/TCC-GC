using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GabrielConstrucaoMVC.Models;
using GabrielConstrucaoMVC.Data;

namespace GabrielConstrucaoMVC.Controllers;

public class HomeController : Controller
{
    [HttpPost]
    public IActionResult ExcluirMensagem(int id)
    {
        var mensagem = _context.Messages.Find(id);

        if (mensagem != null)
        {
            _context.Messages.Remove(mensagem);
            _context.SaveChanges();
        }

        return RedirectToAction("AdminMessages");
    }
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Products()
    {
        return View();
    }

    public IActionResult PetArea()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Services()
    {
        return View();
    }

    public IActionResult AdminMessages()
    {
        var mensagens = _context.Messages
            .OrderByDescending(m => m.DataEnvio)
            .ToList();

        return View(mensagens);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}