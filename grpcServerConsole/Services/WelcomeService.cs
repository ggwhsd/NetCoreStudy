using Grpc.Core;
using GrpcGreeter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grpcServerConsole.Services
{
    public class WelcomeService: Welcomer.WelcomerBase
    {
        public override Task<HelloReplyW> SayHello(HelloRequestW request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReplyW
            {
                Message = "Welcome Hello " + request.Name + " " + context.Deadline.ToString() + " " + context.Method + " " + context.Host + " " + context.Peer 
            });
        }


        public override async Task SayHelloServerStream(HelloRequestW request, IServerStreamWriter<HelloReplyW> responseStream, ServerCallContext context)
        {

            for (var i = 0; i < 5; i++)
            {
                if (!context.CancellationToken.IsCancellationRequested)
                {
                    await responseStream.WriteAsync(new HelloReplyW { Message = "ServerStream " + DateTime.Now.ToLongTimeString()});
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                else
                {
                    break;
                }
            }
        }

        public override async Task<HelloReplyW> SayHelloClientStream(IAsyncStreamReader<HelloRequestW> requestStream, ServerCallContext context)
        {
            
            while (await requestStream.MoveNext())
            {
                var message = requestStream.Current;
                Console.WriteLine("ClientStream receive " + message);
                // ...
            }
            return new HelloReplyW { Message = "ClientStream " + DateTime.Now.ToLongTimeString() };
        }

        public override async Task SayHelloBiStream(IAsyncStreamReader<HelloRequestW> requestStream,
    IServerStreamWriter<HelloReplyW> responseStream, ServerCallContext context)
        {
   
            // Read requests in a background task.
            var readTask = Task.Run(async () =>
            {
                await foreach (var message in requestStream.ReadAllAsync())
                {
                    // Process request.
                    Console.WriteLine("BiStream receive " + message);
                }
            });

            // Send responses until the client signals that it is complete.
            while (!readTask.IsCompleted)
            {
                await responseStream.WriteAsync(new HelloReplyW { Message = "BiStream "+ DateTime.Now.ToLongTimeString() });
                await responseStream.WriteAsync(new HelloReplyW { Message = "BiStream " + DateTime.Now.ToLongTimeString() });
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }

    }
}
