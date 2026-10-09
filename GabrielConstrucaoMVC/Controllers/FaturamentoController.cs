using GabrielConstrucaoMVC.Data;
using GabrielConstrucaoMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GabrielConstrucaoMVC.Controllers
{
    public class FaturamentoController : Controller
    {
        private readonly AppDbContext _context;

        public FaturamentoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var registros = _context.RegistrosFaturamento
                .OrderByDescending(registro => registro.DataRegistro)
                .ToList();

            return View(registros);
        }

        public IActionResult Cadastrar()
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public IActionResult Editar(int id)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var registro = _context.RegistrosFaturamento.Find(id);

            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(RegistroFaturamento registro)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var registroExistente =
                _context.RegistrosFaturamento.Find(registro.Id);

            if (registroExistente == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(registro);
            }

            registroExistente.Valor = registro.Valor;
            registroExistente.DataRegistro = registro.DataRegistro;
            registroExistente.FormaPagamento = registro.FormaPagamento;
            registroExistente.Descricao = registro.Descricao;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var registro = _context.RegistrosFaturamento.Find(id);

            if (registro == null)
            {
                return NotFound();
            }

            _context.RegistrosFaturamento.Remove(registro);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(RegistroFaturamento registro)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(registro);
            }

            _context.RegistrosFaturamento.Add(registro);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}