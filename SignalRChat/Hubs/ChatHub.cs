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
           //对所有接入这个Hub的客户端进行发送消息
            //await Clients.All.SendAsync("ReceiveMessage", user, message + DateTime.Now.ToLongTimeString());
            //对组内的客户端进行发送消息
            await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message + " Group " + DateTime.Now.ToLongTimeString());
            //只发送给调用者
            //await Clients.Caller.SendAsync("ReceiveMessage",user, message + " caller " +DateTime.Now.ToLongTimeString());
        }

        
        public async Task SendCallerMessage(string user, string message)
        {
            //只发送给调用者
            await Clients.Caller.SendAsync("ReceiveMessage",user, message + " caller " +DateTime.Now.ToLongTimeString());
        }

        public async Task SendMessage2(string user, string message)
        {
            //只发送给调用者
            await Clients.Caller.SendAsync("ReceiveMessage2", user, message + " caller " + DateTime.Now.ToLongTimeString());
        }



        /// <summary>
        /// 继承实现该方法，可以在客户端连接成功时调用
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.UserIdentifier);
            //获取当前连接的connectionId，将其加入组“SignalR Users”中。
            if(DateTime.Now.Second%2==0)
                await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }
        /// <summary>
        /// 断开连接时，需要移出组
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
