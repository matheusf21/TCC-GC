using GabrielConstrucaoMVC.Data;
using GabrielConstrucaoMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GabrielConstrucaoMVC.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProdutosController(
            AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Cadastrar(Produto produto, IFormFile? imagem)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            if (imagem != null && imagem.Length > 0)
            {
                string extensao = Path.GetExtension(imagem.FileName).ToLower();

                string[] extensoesPermitidas =
                {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

                if (!extensoesPermitidas.Contains(extensao))
                {
                    ModelState.AddModelError(
                        "NomeImagem",
                        "Selecione uma imagem JPG, JPEG, PNG ou WEBP."
                    );
                }
                else if (imagem.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        "NomeImagem",
                        "A imagem deve possuir no máximo 5 MB."
                    );
                }
                else
                {
                    string nomeArquivo = Guid.NewGuid().ToString() + extensao;

                    string pastaImagens = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "uploads",
                        "produtos"
                    );

                    Directory.CreateDirectory(pastaImagens);

                    string caminhoCompleto = Path.Combine(
                        pastaImagens,
                        nomeArquivo
                    );

                    using (var arquivo = new FileStream(
                        caminhoCompleto,
                        FileMode.Create))
                    {
                        imagem.CopyTo(arquivo);
                    }

                    produto.NomeImagem = nomeArquivo;
                }
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