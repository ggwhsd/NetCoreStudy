using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppRazor.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MyConfigServiceCollectionExtensions
    {
        public static
            IServiceCollection AddMyConfig(this IServiceCollection services, IConfiguration config)
        {
            //此处为示例，如下代码已经放在了Startup中操作。
            //services.Configure<PositionOptions>(config.GetSection(WebAppRazor.Configuration.PositionOptions.Position));
            return services;
        }
    }
}
