using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppRazor.Middles;

namespace WebAppRazor
{
    /// <summary>
    /// 这里是启动配置类，名字无所谓，可以是任意的名字，只要在对应Programs中创建Host时调用的启动配置类名
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }
   

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 在执行这行代码前，已经有两百多个服务加入进去了。
            services.AddRazorPages();
            services.AddTransient<IStartupFilter,
                      RequestSetOptionsStartupFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// http请求管道，其实就是类似设计模式中的责任链，一个http请求进入管道中，管道里面有很多个中间件对请求进行处理并将结果传递给下一个中间件，当然中间件也可以执行终止请求。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> _logger)
        {


            ////use方法1：用了Use的扩展方法，加入中间件，需要通过invoke调用下一个中间件，同时如果用到await，则需要用async
            //app.Use(async (context, next) =>
            //{
            //    // Do work that doesn't write to the Response.
            //    _logger.LogInformation("Hello from 1nd delegate. begin ");
            //    await next.Invoke();
            //    _logger.LogInformation("Hello from 1nd delegate. end ");
            //    // Do logging or other work that doesn't write to the Response.
            //});
            ////use方法2：用了原本的Use方法
            //app.Use(
            //     (next) =>{
            //         return async c => {
            //             _logger.LogInformation("Hello from 2nd delegate. begin ");
            //             await next.Invoke(c);
            //             _logger.LogInformation("Hello from 2nd delegate. end ");
            //         };
                  
            //    }
            //    );

            /////use方法3：用了use的扩展方法。加入最后一个中间件，因此无需调用Invoke
            //app.Run(async context =>
            //{
            //    await Task.Run(() =>
            //    {
            //        _logger.LogInformation("Hello from 3nd delegate. begin ");
            //        _logger.LogInformation("Hello from 3nd delegate. end ");
            //    }
            //    );


            //});

         

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //启动静态文件访问方式，打开之后才可以访问wwwroot路径下的静态资源。
            app.UseStaticFiles();

            //启动路由，在整个http request的pipeline中合适的位置加入了路由点，结合UseEndpoings具体路由方式，
            app.UseRouting();
            //启动授权，调用位置需要在启动路由和路由验证方式之间。
            app.UseAuthorization();
            //启用端点路由方式，得配合UseRouting
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }


    }
}
