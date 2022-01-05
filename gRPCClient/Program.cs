using Grpc.Net.Client;
using GrpcService1;
using GrpcGreeter.Services;
using System;
using System.Threading.Tasks;

namespace gRPCClient
{
    class Program
    {



        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");


            #region 创建一个基于channel的客户端Greeter

            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

            
            await Task.Delay(2000);
            
            reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            #endregion

            await Task.Delay(2000);
            #region 创建基于同一个channel的客户端 Welcomer
            try
            {
                var client2 = new Welcomer.WelcomerClient(channel);
                var reply2 = await client2.SayHelloAsync(
                                  new HelloRequestW { Name = "WelcomeClient" });
                Console.WriteLine("Greeting: " + reply2.Message);
            }
            catch (Exception err)
            {
                Console.WriteLine("exception" + err.StackTrace + " \r\n "+err.Message);
            }
            #endregion


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
