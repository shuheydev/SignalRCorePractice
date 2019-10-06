using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRCorePractice
{
    internal class ChatHub:Hub
    {
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", message);
        }
    }
}