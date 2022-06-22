using ChatWebAPI.Data;
using ChatWebAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatWebAPI.Hubs
{
    public class MessageHub:Hub
    {
        private readonly ChatWebAPIContext _context;

        public MessageHub(ChatWebAPIContext context)
        {
            _context = context;
        }

        public async Task sendMessage1(string message, string contactId)
        {
            User u = _context.User.FirstOrDefault(x => x.UserName == contactId);
            //await Clients.All.SendAsync("messageRecived", message);
            //await Clients.User(u.Id.ToString()).SendAsync("messageRecived", message);
        }
    }
}
