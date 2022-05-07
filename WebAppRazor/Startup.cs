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
using WebAppRazor.Services;

namespace WebAppRazor
{
    /// <summary>
    /// 这里是启动配置类，名字无所谓，可以是任意的名字，只要在对应Programs中创建Host时调用的启动配置类名
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 依赖注入获取到配置类，说明在调用Startup类的方法时，配置类已经加载了，所以ConfigureServices中无法再修改配置类的类似默认程序路径、json文件路径，如果需要添加，必须再Program中添加。
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }
        private static ILogger<Startup> _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //在服务中注册配置,如果有很多配置需要读入，则可以通过扩展方法的方式将这些配置到集中到一个类文件中
           services.Configure<Configuration.PositionOptions>(Configuration.GetSection(WebAppRazor.Configuration.PositionOptions.Position));
            //自定义的扩展方法
            services.AddMyConfig(Configuration);
            // 在执行这行代码前，已经有两百多个服务加入进去了。
            services.AddRazorPages();
            //相当于又添加了一个Startup.configure
            services.AddTransient<IStartupFilter,
                      RequestSetOptionsStartupFilter>();
            services.AddTransient<IHostedService, LifetimeEventsHostedService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// http请求管道，其实就是类似设计模式中的责任链，一个http请求进入管道中，管道里面有很多个中间件对请求进行处理并将结果传递给下一个中间件，当然中间件也可以执行终止请求。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;

            //使用use可以进行简单的处理。

            #region Use添加中间件
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

            //return;
            #endregion

            #region map方式添加
            /*
            app.Map("/map1", HandleMapTest1);
              
            app.Map("/mapPageExample/map", HandleMultiSeg);
            //url进行映射调用的中间件，后续中间件不会执行了，因为在map中间件中没有调用后续的中间件。
          
            app.MapWhen(context => context.Request.Query.ContainsKey("branch"),
                              HandleBranch);
          
            return;
              */
            #endregion

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


            #region 获环境配置
            
            _logger.LogInformation($"{env.ApplicationName} {env.EnvironmentName}");
            _logger.LogInformation($"{env.ContentRootPath}");
            _logger.LogInformation($"{env.WebRootPath}");
            _logger.LogInformation($"{env.WebRootFileProvider.ToString()}");
            _logger.LogInformation($"开发环境:{env.IsDevelopment()}");
            #endregion
        }

        private static void HandleMapTest1(IApplicationBuilder app)
        {
            //在这个中间件中未调用后续的，所以走这条路的请求到这就结束了。
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }
        private static void HandleMultiSeg(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
                   {
                       _logger.LogInformation(" HandleMultiSeg执行了map操作，看看是否执行网页访问 ");
                       
                    }
                );

            
            }
        private static void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var branchVer = context.Request.Query["branch"];
                await context.Response.WriteAsync($"Branch used = {branchVer}");
            });
        }


    }
}
