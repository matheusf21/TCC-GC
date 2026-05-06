using Microsoft.AspNetCore.Mvc;
using TccBackend.Data;
using TccBackend.Models;

namespace TccBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly AppDbContext _context;

    public MessageController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/message
    [HttpGet]
    public IActionResult Get()
    {
        var messages = _context.Messages.ToList();
        return Ok(messages);
    }

    // POST: api/message
    [HttpPost]
    public IActionResult Post(Message message)
    {
        _context.Messages.Add(message);
        _context.SaveChanges();
        return Ok(message);
    }

    // DELETE: api/message/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var msg = _context.Messages.Find(id);

        if (msg == null)
            return NotFound();

        _context.Messages.Remove(msg);
        _context.SaveChanges();

        return Ok();
    }
}