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

        // Totais de faturamento

        ViewBag.FaturamentoHoje =
            _context.RegistrosFaturamento
                .Where(registro =>
                    registro.DataRegistro >= hoje &&
                    registro.DataRegistro < amanha)
                .Sum(registro => (decimal?)registro.Valor) ?? 0;

        ViewBag.FaturamentoMes =
            _context.RegistrosFaturamento
                .Where(registro =>
                    registro.DataRegistro >= inicioMes &&
                    registro.DataRegistro < amanha)
                .Sum(registro => (decimal?)registro.Valor) ?? 0;

        var inicioAno = new DateTime(hoje.Year, 1, 1);

        ViewBag.FaturamentoAno =
            _context.RegistrosFaturamento
                .Where(registro =>
                    registro.DataRegistro >= inicioAno &&
                    registro.DataRegistro < amanha)
                .Sum(registro => (decimal?)registro.Valor) ?? 0;


        // Gráfico dos últimos 7 dias

        var inicioSeteDias = hoje.AddDays(-6);

        var registrosSeteDias = _context.RegistrosFaturamento
            .Where(registro =>
                registro.DataRegistro >= inicioSeteDias &&
                registro.DataRegistro < amanha)
            .ToList();

        var rotulosSeteDias = new List<string>();
        var valoresSeteDias = new List<decimal>();

        for (int i = 0; i < 7; i++)
        {
            var data = inicioSeteDias.AddDays(i);

            rotulosSeteDias.Add(data.ToString("dd/MM"));

            valoresSeteDias.Add(
                registrosSeteDias
                    .Where(registro =>
                        registro.DataRegistro.Date == data.Date)
                    .Sum(registro => registro.Valor)
            );
        }

        ViewBag.RotulosFaturamentoSeteDias = rotulosSeteDias;
        ViewBag.ValoresFaturamentoSeteDias = valoresSeteDias;


        // Gráfico do mês atual

        var registrosMes = _context.RegistrosFaturamento
            .Where(registro =>
                registro.DataRegistro >= inicioMes &&
                registro.DataRegistro < amanha)
            .ToList();

        var rotulosMes = new List<string>();
        var valoresMes = new List<decimal>();

        for (int dia = 1; dia <= hoje.Day; dia++)
        {
            var data = new DateTime(
                hoje.Year,
                hoje.Month,
                dia
            );

            rotulosMes.Add(data.ToString("dd/MM"));

            valoresMes.Add(
                registrosMes
                    .Where(registro =>
                        registro.DataRegistro.Date == data.Date)
                    .Sum(registro => registro.Valor)
            );
        }

        ViewBag.RotulosFaturamentoMes = rotulosMes;
        ViewBag.ValoresFaturamentoMes = valoresMes;


        // Gráfico do ano atual

        var registrosAno = _context.RegistrosFaturamento
            .Where(registro =>
                registro.DataRegistro >= inicioAno &&
                registro.DataRegistro < amanha)
            .ToList();

        var nomesMeses = new[]
        {
    "Jan", "Fev", "Mar", "Abr",
    "Mai", "Jun", "Jul", "Ago",
    "Set", "Out", "Nov", "Dez"
};

        var valoresAno = new List<decimal>();

        for (int mes = 1; mes <= 12; mes++)
        {
            valoresAno.Add(
                registrosAno
                    .Where(registro =>
                        registro.DataRegistro.Month == mes)
                    .Sum(registro => registro.Valor)
            );
        }

        ViewBag.RotulosFaturamentoAno = nomesMeses;
        ViewBag.ValoresFaturamentoAno = valoresAno;

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