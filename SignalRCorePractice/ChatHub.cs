using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRCorePractice
{

    internal class ChatHub:Hub
    {
        public async Task Send(string message,string from)//2つのstringを受け取る
        {
            await Clients.All.SendAsync("Receive", $"{message}:::{from}");
        }
    }
}