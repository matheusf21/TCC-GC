using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GabrielConstrucaoMVC.Models;
using GabrielConstrucaoMVC.Data;

namespace GabrielConstrucaoMVC.Controllers;

public class HomeController : Controller
{
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
        var produtos = _context.Produtos.ToList();

        return View(produtos);
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

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string usuario, string senha)
    {
        var admin = _context.Admins
            .FirstOrDefault(a => a.Usuario == usuario && a.Senha == senha);

        if (admin != null)
        {
            HttpContext.Session.SetString("AdminLogado", "true");
            return RedirectToAction("AdminMessages");
        }

        ViewBag.Erro = "Usuário ou senha inválidos.";
        return View();
    }

    public IActionResult AdminMessages()
    {
        if (HttpContext.Session.GetString("AdminLogado") != "true")
        {
            return RedirectToAction("Login");
        }

        var mensagens = _context.Messages
            .OrderByDescending(m => m.DataEnvio)
            .ToList();

        return View(mensagens);
    }

    [HttpPost]
    public IActionResult ExcluirMensagem(int id)
    {
        if (HttpContext.Session.GetString("AdminLogado") != "true")
        {
            return RedirectToAction("Login");
        }

        var mensagem = _context.Messages.Find(id);

        if (mensagem != null)
        {
            _context.Messages.Remove(mensagem);
            _context.SaveChanges();
        }

        return RedirectToAction("AdminMessages");
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}