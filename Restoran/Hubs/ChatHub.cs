using Microsoft.AspNetCore.SignalR;
using Restoran.Data;
using Restoran.Models;

namespace Restoran.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                Timestamp = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Send to all clients with timestamp
            await Clients.All.SendAsync("ReceiveMessage", user, message, chatMessage.Timestamp.ToString("HH:mm"));
        }

        // Typing indicator
        public async Task Typing(string user)
        {
            await Clients.Others.SendAsync("UserTyping", user);
        }
    }

       
    
}
