using Microsoft.AspNetCore.SignalR;
using Shared;
using System.Threading.Tasks;

namespace SignalRCorePractice
{

    internal class ChatHub : Hub
    {
        public async Task Send(string message, string from)//2つのstringを受け取る
        {
            await Clients.All.SendAsync("Receive", message, from);
        }

        public async Task SendObject(Message message)
        {
            await Clients.All.SendAsync("ReceiveObject", message);
        }
    }
}