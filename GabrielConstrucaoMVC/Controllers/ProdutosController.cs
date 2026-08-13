using GabrielConstrucaoMVC.Data;
using GabrielConstrucaoMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GabrielConstrucaoMVC.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var produtos = _context.Produtos.ToList();

            return View(produtos);
        }

        public IActionResult Cadastrar()
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Produto produto)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(produto);
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Produto produto)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(produto);
            }

            _context.Produtos.Update(produto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Excluir(int id)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExcluirConfirmado(int id)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
