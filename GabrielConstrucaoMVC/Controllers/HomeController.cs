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
        var produtos = _context.Produtos
            .Where(produto => produto.Area == "Construção")
            .ToList();

        return View(produtos);
    }

    public IActionResult PetArea()
    {
        var produtosPet = _context.Produtos
            .Where(produto => produto.Area == "Pet")
            .ToList();

        return View(produtosPet);
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

            return RedirectToAction("Dashboard", "Home");
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

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("AdminLogado");

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetString("AdminLogado") != "true")
        {
            return RedirectToAction("Login");
        }

        var hoje = DateTime.Today;
        var amanha = hoje.AddDays(1);

        var inicioMes = new DateTime(
            hoje.Year,
            hoje.Month,
            1
        );

        ViewBag.TotalContatosWhatsApp =
            _context.ContatosWhatsApp.Count();

        ViewBag.ContatosHoje =
            _context.ContatosWhatsApp.Count(contato =>
                contato.DataContato >= hoje &&
                contato.DataContato < amanha
            );

        ViewBag.ContatosMes =
            _context.ContatosWhatsApp.Count(contato =>
                contato.DataContato >= inicioMes
            );

        var inicioGrafico = hoje.AddDays(-6);

        var contatosUltimosDias = _context.ContatosWhatsApp
            .Where(contato =>
                contato.DataContato >= inicioGrafico &&
                contato.DataContato < amanha)
            .ToList();

        var rotulosGrafico = new List<string>();
        var valoresGrafico = new List<int>();

        for (int i = 0; i < 7; i++)
        {
            var data = inicioGrafico.AddDays(i);

            rotulosGrafico.Add(data.ToString("dd/MM"));

            valoresGrafico.Add(
                contatosUltimosDias.Count(contato =>
                    contato.DataContato.Date == data.Date)
            );
        }

        ViewBag.RotulosGrafico = rotulosGrafico;
        ViewBag.ValoresGrafico = valoresGrafico;

        return View();
    }

    [HttpPost]
    public IActionResult RegistrarContatoWhatsApp()
    {
        var contato = new ContatoWhatsApp();

        _context.ContatosWhatsApp.Add(contato);
        _context.SaveChanges();

        return Ok();
    }
}