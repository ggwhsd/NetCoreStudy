# NetCoreStudy
learn programming in netcore 5 platform.

https://docs.microsoft.com/zh-cn/aspnet/core/?view=aspnetcore-5.0

## 第一个控制台项目 

dotnet new console --output myConsoleApp
dotnet run --project myConsoleApp
然后使用visual studio code打开这个项目。

* myConsoleApp/IndexStudy.cs 

  展示了字典的使用方法，[,]操作符的自定义和使用，还有一个分形算法的示例。

* myConsoleApp/Lambda.cs 

  lambda的使用，以及Action、Func、Predicate等委托类型的使用。

* myConsoleApp/LinqUse.cs 

  展示了使用Linq进行洗牌的应用，与用循环代码生成洗牌的方法相比，Linq快捷简化了代码。

* myConsoleApp/AsyncTask.cs 

  展示了异步任务的await、async、ContinueWith、Task.WhenAll、并行方法Parallel等方法的使用。异步任务模式，其实现中也用到了线程池，但是比直接操作线程Thread方式更容易使用也更高效开发。

* myConsoleApp/TaskWait.cs 

  重点对Task这个类进行的演示，Task的几种创建方式，Task可以获取其ID，也可以给控制任务顺序，Task也是可以当作同步使用的。
同时对task的异常捕获，cancel操作等做了示例。

* myConsoleApp/DataModelStudy.cs

对于record类型的示例。C# 10新增的类型。

## 第一个web项目 

## aspnet core5的grpc server
[示例](./GrpcService1/Startup.cs)

完全基于c#的dotnet-grpc，底层不适用c的grpc库。在一个已经准备好环境的项目中，额外添加一个grpc服务4步骤
1. 定义proto消息
2. 在项目编辑项目文件添加该proto的路径
3. 编写对应服务
4. 在startup.cs中启用该服务。

使用的时候，https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-5.0&tabs=visual-studio。
* 创建aspnet core 5的grpc模板项目。新建一个grpc service可以通过默认的示例服务greet.proto。

* 在Protos目录下，新增一个自定义的welcome.proto定义消息结构

``` csharp
syntax = "proto3";

option csharp_namespace = "GrpcGreeter.Services";

package welcome;

// The greeting service definition.
service Welcomer {
  // Sends a greeting
  rpc SayHello (HelloRequestW) returns (HelloReplyW);
}

// The request message containing the user's name.
message HelloRequestW {
  string name = 1;
}

// The response message containing the greetings.
message HelloReplyW {
  string message = 1;
}

```

* 右键项目编辑项目文件GrpcGreeter.csproj中，添加welcome.proto，此时visual studio2019会自动使用grpc工具自动生成中间代码。

(该步骤也可以通过右键proto文件，生成操作选项选择Protobuf compiler，然后选择server only来自动添加如下。)

``` csharp
  <ItemGroup>
    <Protobuf Include="Protos\greet.proto" GrpcServices="Server" />
    <Protobuf Include="Protos\welcome.proto" GrpcServices="Server" />
  </ItemGroup>
```

* 定义好了通信消息结构，接下来定义grpc的welcome服务了，在services目录下添加 WelcomerService.cs。

``` csharp
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcGreeter.Services
{
    public class WelcomerService : Welcomer.WelcomerBase
    {
        private readonly ILogger<WelcomerService> _logger;
        public WelcomerService(ILogger<WelcomerService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReplyW> SayHello(HelloRequestW request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReplyW
            {
                Message = "yes welcome service say :  " + request.Name
            });
        }

    }
}
```

* 在启动startup.cs中配置对应服务可用

``` csharp
app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<WelcomerService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
```

## grpc client

[示例](./gRPCClient/Program.cs)

* 新建一个netcore控制台，这里无需aspnet core项目。

visual studio 2019通过nuget安装Grpc.Net.Client、Google.Protobuf 和 Grpc.Tools。
如果项目工作准备好后，实际上每次添加新的服务只要三步就可以，非常简单。

* 新建Protos目录，将server项目中的welcome和greet.proto两个文件拷贝过来。

* 右键项目编辑项目文件，在<ItemGroup>中添加如下

(该步骤也可以通过右键proto文件，生成操作选项选择Protobuf compiler，然后选择client only来自动添加如下。)

```
    <Protobuf Include="Protos\greet.proto" GrpcServices="Client" />
    <Protobuf Include="Protos\welcome.proto" GrpcServices="Client" />
```

* 为了示例，这里直接在program.cs中修改代码了，中代码如下

``` csharp
class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

            await Task.Delay(2000);
            reply =await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

            await Task.Delay(2000);
            var client2 = new Welcomer.WelcomerClient(channel);
            var reply2 = await client2.SayHelloAsync(
                              new HelloRequestW { Name = "WelcomeClient" });
            Console.WriteLine("Greeting: " + reply2.Message);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
```

* grpc有四种服务，分别可以在Proto文件下，通过如下定义：

```
// The greeting service definition.
service Welcomer {
  // Sends a greeting
  // Unary : 一个请求 -> 一个回复
  rpc SayHello (HelloRequestW) returns (HelloReplyW);
  // Server streaming : 一个请求 -> 数次回复
  rpc SayHelloServerStream (HelloRequestW) return (stream  HelloReplyW);
  // Client streaming : 多个请求 -> 一次回复
  rpc SayHelloServerStream (HelloRequestW) return (stream  HelloReplyW);
  // 双向 streaming : 多个请求 <-> 多次回复
  rpc SayHelloServerStream (stream HelloRequestW) return (stream  HelloReplyW);

}
```

## net core5的grpc server
[示例1](./grpcServerConsole/Program.cs)
* 不依赖于asp.net core5的grpc模板，完全基于net core的空项目开始。以下方法也可以基于.net core的winform[示例2](./grpcServerWinForm/Program.cs)。

nuget安装grpc.aspnetcore

* 新建Protos目录，照抄greet.proto文件。

* 右键proto选择生成编译位Protobuffer 选择server and client。

文件生成在obj\Debug\net5.0\Protos,这样客户端程序就可以直接拷贝这个cs文件了。

* 编写GreeterServices，照抄。

* Program.cs需要引入aspnetcore的http服务器。

```csharp

using grpcServerConsole.Services;
using GrpcService1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.AspNetCore;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace grpcServerConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            Console.WriteLine("Hello World!");

            var d = Greeter.BindService(new GreeterService());
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    }

    class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
               

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}


```

## net core5的signalR实时应用入门

https://github.com/aspnet/SignalR-samples

这个可以代替websocket进行快速的开发。[示例1](./SignalRChat/Startup.cs)
步骤1: 创建一个asp.net core web项目
步骤2：配置启用signalR以及配置对应路由路径,在startup.cs文件中配置。
```
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                //2.添加signalR对应的hub，hub为signalR的应用
                endpoints.MapHub<ChatHub>("/chatHub");
            });
            
           
        }
```

步骤3：新建Hubs目录，新建ChatHub类，如下，
```
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
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

```
步骤4：准备对应的客户端signalr.js。这个后续其他项目就可以直接拷贝了，不用每次都这么安装。
1. 在“解决方案资源管理器”中，右键单击项目，然后选择“添加”>“客户端库” 。
2. 在“添加客户端库”对话框中，对于“提供程序”，选择“unpkg”。
3. 对于“库”，输入 @microsoft/signalr@latest。
4. 选择“选择特定文件”，展开“dist/browser”文件夹，然后选择“signalr.js”和“signalr.min.js”。
5. 将“目标位置”设置为 wwwroot/js/signalr/，然后选择“安装”。

步骤5：编写wwwroot/js/chat.js，用于页面使用
```
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
```

步骤6：添加Razor页面，Pages/index1.cshtml
```
@page
<div class="container">
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-2">User</div>
        <div class="col-4"><input type="text" id="userInput" /></div>
    </div>
    <div class="row">
        <div class="col-2">Message</div>
        <div class="col-4"><input type="text" id="messageInput" /></div>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-6">
            <input type="button" id="sendButton" value="Send Message" />
        </div>
    </div>
</div>
<div class="row">
    <div class="col-12">
        <hr />
    </div>
</div>
<div class="row">
    <div class="col-6">
        <ul id="messagesList"></ul>
    </div>
</div>
<script src="~/js/signalr/dist/browser/signalr.js"></script>
<script src="~/js/chat.js"></script>
```
# signalR入门2：组管理和组调用

可以对每个客户端连接，将其分类到不同自定义的组中，这样可以方便进行组内消息广播发送。
1. 添加组。在chatHub中添加方法
```
/// <summary>
        /// 继承实现该方法，可以在客户端连接成功时调用
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
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
```
2. 服务器端对于组内的消息进行调用。
```
 public async Task SendMessage(string user, string message)
{
   //对所有接入这个Hub的客户端进行发送消息
	//await Clients.All.SendAsync("ReceiveMessage", user, message + DateTime.Now.ToLongTimeString());
	//对组内的客户端进行发送消息
	await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message + " Group " + DateTime.Now.ToLongTimeString());
	//只发送给调用者
	//await Clients.Caller.SendAsync("ReceiveMessage",user, message + " caller " +DateTime.Now.ToLongTimeString());
}
```
# signalR入门3：从控制器中访问Hub

之前两个例子都是从Hub自己调用中进行处理发送消息给客户端，即都是由客户端的一个请求触发。如果我们需要写有服务器端根据不同的情况主动推送消息，则需要能够从Hub外部调用才可以。

这里使用控制器来处理，通过依赖注入方式。

1. 启用mvc路由，（.NET 5的配置方式）
```
app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                //2.添加signalR对应的hub，hub为signalR的应用
                endpoints.MapHub<ChatHub>("/chatHub");

                // 添加mvc路由配置
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
```

2. 添加控制器

```
public class HomeController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public HomeController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage"," HomeController " , $"Home page loaded at: {DateTime.Now}");
            return View();
        }
    }
```

3. 测试

输入 http://localhost:5000/Home, 则在index1中会显示"HomeController says Home page loaded at: 2021/12/25 17:49:36"


# signalR入门4：Hub添加多个请求方法

1. 在ChatHub中添加如下方法
```
public async Task SendCallerMessage(string user, string message)
        {
            //只发送给调用者
            await Clients.Caller.SendAsync("ReceiveMessage",user, message + " caller " +DateTime.Now.ToLongTimeString());
        }
```

2. 在chat.js中添加如下代码
```
document.getElementById("sendButton2").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendCallerMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

```

3. 在index1.cshtml中添加一个id为sendButton2的按钮即可。


## MVC框架中，添加了cookie的验证功能

1. 在startup.cs中启用 app.UseAuthentication();
2. 在startup.cs中,添加cookie的配置
```
public const string CookieScheme = "YourSchemeName";

 public void ConfigureServices(IServiceCollection services){

...


services.AddAuthentication(CookieScheme) // Sets the default scheme to cookies
            .AddCookie(CookieScheme, options =>
            {
                options.AccessDeniedPath = "/account/denied";
                options.LoginPath = "/account/login";
            });

            // Example of how to customize a particular instance of cookie options and
            // is able to also use other services.
            services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureMyCookie>();

}
```

3. 在HomeController中添加方法，并在方法上添加 Authorize 表示该action需要验证。

```
 [Authorize]
        public IActionResult MyClaims()
        {
            return View();
        }
```
4. 添加具体验证控制器AccountController.cs，核心代码如下
```
//先验证登录用户密码，若正确，则创建cookie票据，后续操作就无需再输入用户密码了。
if (ValidateLogin(userName, password))
            {
                var claims = new List<Claim>
                {
                    new Claim("user", userName),
                    new Claim("role", "Member")
                };
                var authProperties = new AuthenticationProperties();
                authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1);
                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")), authProperties);


```

5. 添加对应试图，Home/MyClaims.cshtml， Account/Login.cshtml, Account/AccessDenied.cshtml

## MVC框架中，使用restful api（也叫web api）

1. 添加restful api的类： 右键项目，添加新项->api控制器(空)，取名ValuesController
2. 双击ValuesController.cs文件，添加第一个方法如下，第一行表示请求路径，即/api/Values，第一个方法Get，表示默认Get /api/Values 请求。
[Authorize]可以使用验证，web api也是可以使用和web中一样的验证。
```
[Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // Get /api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1","value2" };
        }
    }
```
3. restful 常用几种请求方法和路径，添加如下几个方法示例。 具体看代码。

## 


## 串口项目
* SerialConsole

## 本地调用
https://docs.microsoft.com/zh-cn/dotnet/standard/native-interop/pinvoke
