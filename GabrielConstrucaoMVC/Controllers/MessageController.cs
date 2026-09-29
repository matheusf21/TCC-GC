using GabrielConstrucaoMVC.Data;
using GabrielConstrucaoMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GabrielConstrucaoMVC.Controllers
{
    public class MessageController : Controller
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Enviar(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();

            TempData["Sucesso"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("Contact", "Home");
        }
    }
    
}