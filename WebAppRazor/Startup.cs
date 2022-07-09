using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppRazor.Middles;
using WebAppRazor.Services;
using WebAppRazor.Services.AuthorizonHandler;
using WebAppRazor.ServicesBack;
using WebAppRazor.Utils;

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
            services.AddSingleton<MyLog4NetService>();
            //在服务中注册配置,如果有很多配置需要读入，则可以通过扩展方法的方式将这些配置到集中到一个类文件中，参考MyConfigServiceCollectionExtensions
            services.Configure<Configuration.PositionOptions>(Configuration.GetSection(WebAppRazor.Configuration.PositionOptions.Position));
            //自定义的扩展方法
            services.AddMyConfig(Configuration);
            // 在执行这行代码前，已经有两百多个服务加入进去了。
            services.AddRazorPages();
            //services.AddRazorPages(options =>
            //{
            //    options.Conventions.AuthorizePage("/Index");
            //});
                //相当于又添加了一个Startup.configure
                services.AddTransient<IStartupFilter,
                      RequestSetOptionsStartupFilter>();
            services.AddTransient<IHostedService, LifetimeEventsHostedService>();
            //注入泛型
            services.AddTransient(typeof(TemplateServiceInterface<>), typeof(TemplateService<>));

            services.AddDirectoryBrowser();
            //添加cookie认证服务
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;  //滑动窗口方式，如果一个request时发现超过了设定的过时时间一半了，则重新分配一个cookie并且重新记时间。
              //  options.AccessDeniedPath = "/Forbidden/";
            });
            #region 基于角色
            //添加授权策略，如果是基于角色的，可以不用如下添加授权策略服务,如下代码等价于应用角色，从这里可以看出，把角色看作策略的子集比较合适。
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("RequireAdministratorRole",
            //         policy => policy.RequireRole("Administrator"));
            //});
            #endregion

            #region 基于Claim
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("UserXXX", policy =>
            //        policy.RequireClaim("username","111","222"));
            //});
            #endregion

            #region 基于策略授权 
            //添加授权策略，基于策略，策略方式的授权可以扩展，此处扩展了一个IAuthorizationRequirement需求，该接口是空的，用于标记作用。
            //会被IAuthorizationService服务使用。默认IAuthorizationService的实现
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AtLeast21", policy =>
                    policy.Requirements.Add(new MinimumAgeRequirement(21)));
            });
            //注册自定义Requirement的IAuthorizationHandler
            services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
            #endregion

            services.AddHostedService<AutoTimerCloseWeb>();
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

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.WebRootPath, "png")),
                // Path.Combine("D:/", "")),
                RequestPath = "/MyDisk"
            });

            //启动路由，在整个http request的pipeline中合适的位置加入了路由点，结合UseEndpoings具体路由方式，
            app.UseRouting();
            app.Use(next => async context =>
            {
                string clientIP = context.Connection.RemoteIpAddress.ToString();
                logger.LogInformation("客户端ip:"+ clientIP);
                if (clientIP != null && clientIP == "127.0.0.1")
                    await next(context);
                else
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync("不被允许访问的ip");
                }

            });
            //启用认证，配合添加cookie认证服务
            app.UseAuthentication();
            //启动授权中间件，调用位置需要在启动路由和路由验证方式之间。启用之后会去找授权服务。
            app.UseAuthorization();

            app.Use(next => async context =>
            {
                using (new MyStopwatch(logger, $"统计EndPoints中间件的处理耗时"))
                {
                    await next(context);
                    logger.LogWarning("返回编码:" + context.Response.ContentType);
                }
            });

            //启用端点路由方式，得配合UseRouting。如果在这一步中有对应的endpoint，则不会往后续的中间件上执行。否则后续的中间件会继续执行。
            app.UseEndpoints(endpoints =>
            {
              

                endpoints.MapRazorPages();
                endpoints.MapGet("/MapGet", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                //endpoints.MapGet("/index", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!  instead of Index.cshtml, current index writeAsync will be send to client");
                //});
                endpoints.MapGet("/hello/{name:alpha}", async context =>
                {
                    var name = context.Request.RouteValues["name"];
                    await context.Response.WriteAsync($"Hello {name}!");
                });


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
