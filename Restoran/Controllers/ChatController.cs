using Microsoft.AspNetCore.Mvc;
using Restoran.Data;

public class ChatController : Controller
{
    private readonly ApplicationDbContext _context;
    /// ya constructor hai 
    public ChatController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var messages = _context.ChatMessages
            .OrderBy(m => m.Timestamp)
            .ToList();

        return View(messages); // Pass to view
    }
}
