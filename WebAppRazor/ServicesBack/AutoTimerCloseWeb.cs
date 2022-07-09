using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAppRazor.ServicesBack
{
    public class AutoTimerCloseWeb : IHostedService
    {
        private readonly ILogger<AutoTimerCloseWeb> _logger;

        private IHost _web;

        public IConfiguration Configuration { get; }

        private DateTime stopTime;

        public AutoTimerCloseWeb(ILogger<AutoTimerCloseWeb> logger, IHost web, IConfiguration configuration)
        {
            _logger = logger;
            _web = web;
            this.Configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("AutoTimerCloseWeb start");
            stopTime = DateTime.Parse(Configuration["AutoCloseTime"]);


            Run();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("AutoTimerCloseWeb stop");
            return Task.CompletedTask;
        }

        public async void Run()
        {

            while (true)
            {

                await Task.Delay(500);
                if (DateTime.Now.Hour == stopTime.Hour && DateTime.Now.Minute == stopTime.Minute)
                {
                    break;
                }
            }
            _web.StopAsync();
        }
    }
}
