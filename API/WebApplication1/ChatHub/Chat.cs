using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.ChatHub
{
    public class Chat : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",user,message);
        }
    }
}
