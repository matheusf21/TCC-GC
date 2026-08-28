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

            string extensao = "";

            if (imagem != null && imagem.Length > 0)
            {
                extensao = Path.GetExtension(imagem.FileName).ToLower();

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
            }

            if (!ModelState.IsValid)
            {
                foreach (var erro in ModelState)
                {
                    foreach (var mensagem in erro.Value.Errors)
                    {
                        Console.WriteLine(
                            $"CAMPO: {erro.Key} | ERRO: {mensagem.ErrorMessage}"
                        );
                    }
                }

                return View(produto);
            }

            if (imagem != null && imagem.Length > 0)
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
        public IActionResult Editar(Produto produto, IFormFile? imagem)
        {
            if (HttpContext.Session.GetString("AdminLogado") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            var produtoExistente = _context.Produtos.Find(produto.Id);

            if (produtoExistente == null)
            {
                return NotFound();
            }
            string? caminhoImagemAntiga = null;
            string extensao = "";

            if (imagem != null && imagem.Length > 0)
            {
                if (!string.IsNullOrEmpty(produtoExistente.NomeImagem))
                {
                    caminhoImagemAntiga = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "uploads",
                        "produtos",
                        produtoExistente.NomeImagem
                    );
                }
                extensao = Path.GetExtension(imagem.FileName).ToLower();

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
            }

            if (!ModelState.IsValid)
            {
                produto.NomeImagem = produtoExistente.NomeImagem;
                return View(produto);
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.Categoria = produto.Categoria;
            produtoExistente.MaisVendido = produto.MaisVendido;
            produtoExistente.ForaDeEstoque = produto.ForaDeEstoque;

            if (imagem != null && imagem.Length > 0)
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

                produtoExistente.NomeImagem = nomeArquivo;
            }

            _context.SaveChanges();

            if (caminhoImagemAntiga != null &&
                System.IO.File.Exists(caminhoImagemAntiga))
            {
                System.IO.File.Delete(caminhoImagemAntiga);
            }

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

            string? caminhoImagem = null;

            if (!string.IsNullOrEmpty(produto.NomeImagem))
            {
                caminhoImagem = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "uploads",
                    "produtos",
                    produto.NomeImagem
                );
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            if (caminhoImagem != null && System.IO.File.Exists(caminhoImagem))
            {
                System.IO.File.Delete(caminhoImagem);
            }

            return RedirectToAction("Index");
        }

    }
}