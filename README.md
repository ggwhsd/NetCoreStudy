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

这个可以代替websocket进行快速搞笑的开发。
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


## 串口项目
* SerialConsole

## 本地调用
https://docs.microsoft.com/zh-cn/dotnet/standard/native-interop/pinvoke
