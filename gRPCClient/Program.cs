using Grpc.Net.Client;
using GrpcService1;
using GrpcGreeter.Services;
using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace gRPCClient
{
    class Program
    {
        //模式1：针对服务端为一元调用示例： 创建一个基于channel的客户端Greeter
        public static async void TestSimpleClient(GrpcChannel GprcChannel)
        {

           //1. 创建客户端
            var client = new Greeter.GreeterClient(GprcChannel);

            //2. 调用rpc方法,异步回去结果
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

            
            await Task.Delay(2000);

            
            reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
      
        }

        //模式2：针对服务端为一元调用示例 以及 针对服务端为服务端流示， 创建一个基于channel的客户端Greeter
        public static async void TestServerStreamClient(GrpcChannel GprcChannel)
        {
            //针对服务端为一元调用示例
            var client2 = new Welcomer.WelcomerClient(GprcChannel);
            var reply2 = await client2.SayHelloAsync( new HelloRequestW { Name = "SayHelloAsync " });
            Console.WriteLine("SayHelloAsync: " + reply2.Message);


            //针对服务端为服务端流示
            var call = client2.SayHelloServerStream(new HelloRequestW { Name = "ServerStreamRequest " });

            while (await call.ResponseStream.MoveNext())
            {
                //直到服务端对应SayHelloServerStream方法调用完毕，MoveNext才会结束。
                Console.WriteLine("SayHelloServerStream: " + call.ResponseStream.Current.Message);
            }
            


        }

        //模式3：针对服务端为客户端流示：创建基于同一个channel的客户端 Welcomer
        public static async void TestClientStream(GrpcChannel GprcChannel)
        {
            var client = new Welcomer.WelcomerClient(GprcChannel);
            using var call = client.SayHelloClientStream(); 

            for (var i = 0; i < 3; i++)
            {
                //发送多个客户端请求
                await call.RequestStream.WriteAsync(new HelloRequestW {Name = "SayHelloClientStream" });
             }
            //通知服务端“我客户端已经发送完毕”
            await call.RequestStream.CompleteAsync();

            var response = await call;
            Console.WriteLine($"SayHelloClientStream Response: {response.Message}");
        }

        //模式4：针对 服务端 为 客户端+服务端 流示：创建基于同一个channel的客户端 Welcomer
        public static async void TestClientServerStream(GrpcChannel GprcChannel)
        {
            var client = new Welcomer.WelcomerClient(GprcChannel);
            using var call = client.SayHelloBiStream();

            Console.WriteLine("Starting background task to receive messages");
            var readTask = Task.Run(async () => {
                //开启一个后台接收线程
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine("ClientServerStream receive:" + response.Message);
                   
                }
            });
            
            for (var i = 0; i < 3; i++)
            {
                //发送多个客户端请求
                await call.RequestStream.WriteAsync(new HelloRequestW { Name = "SayHelloClientServerStream " });
            }
            //开始发送“我客户端已经发送完毕”
            await call.RequestStream.CompleteAsync();
          

            await readTask; //等待后台接收线程完毕
        }


        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            // using变量，超出作用域则会自动释放资源
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");


            TestSimpleClient(channel);

             await Task.Delay(2000);
            TestServerStreamClient(channel);
            await Task.Delay(2000);

            TestClientStream(channel);
            await Task.Delay(2000);

            TestClientServerStream(channel);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
