using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAppRazor.Services
{
    /// <summary>
    /// 用于监听主机的生命周期变化. 通过 services.AddTransient<IHostedService, LifetimeEventsHostedService>(); 注册到容器中。
    /// </summary>
    public class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
        }

        private DateTime start;
        private DateTime stopping;
        private DateTime stopped;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            start = DateTime.Now;
            _logger.LogInformation("OnStarted has been called.》》》》》》》》》》》》》》》》》》》》》》》》" + start.ToString());
            
            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            stopping = DateTime.Now;
            _logger.LogInformation("OnStopping has been called.》》》》》》》》》》》》》》》》》》》》》》》》" + start.ToString());
           
            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            stopped = DateTime.Now;
            _logger.LogInformation("OnStopped has been called.》》》》》》》》》》》》》》》》》》》》》》》》"+ stopping.ToString());
            
            // Perform post-stopped activities here
        }
    }
}
