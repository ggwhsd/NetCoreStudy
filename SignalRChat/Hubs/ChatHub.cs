using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    //3. 创建signalR的一个应用示例，一定要继承Hub
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
           
            await Clients.All.SendAsync("ReceiveMessage", user, message + DateTime.Now.ToLongTimeString());
        }

        public async Task SendMessage2()
        {
            await Clients.All.SendAsync("ReceiveMessage2 ",DateTime.Now.ToLongTimeString());
        }
    }
}
