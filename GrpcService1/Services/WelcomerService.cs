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
