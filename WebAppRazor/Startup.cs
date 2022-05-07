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
    /// ���������������࣬��������ν����������������֣�ֻҪ�ڶ�ӦPrograms�д���Hostʱ���õ�������������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ����ע���ȡ�������࣬˵���ڵ���Startup��ķ���ʱ���������Ѿ������ˣ�����ConfigureServices���޷����޸������������Ĭ�ϳ���·����json�ļ�·���������Ҫ��ӣ�������Program����ӡ�
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

            //�ڷ�����ע������,����кܶ�������Ҫ���룬�����ͨ����չ�����ķ�ʽ����Щ���õ����е�һ�����ļ���
           services.Configure<Configuration.PositionOptions>(Configuration.GetSection(WebAppRazor.Configuration.PositionOptions.Position));
            //�Զ������չ����
            services.AddMyConfig(Configuration);
            // ��ִ�����д���ǰ���Ѿ������ٶ����������ȥ�ˡ�
            services.AddRazorPages();
            //�൱���������һ��Startup.configure
            services.AddTransient<IStartupFilter,
                      RequestSetOptionsStartupFilter>();
            services.AddTransient<IHostedService, LifetimeEventsHostedService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// http����ܵ�����ʵ�����������ģʽ�е���������һ��http�������ܵ��У��ܵ������кܶ���м����������д�����������ݸ���һ���м������Ȼ�м��Ҳ����ִ����ֹ����
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;

            //ʹ��use���Խ��м򵥵Ĵ���

            #region Use����м��
            ////use����1������Use����չ�����������м������Ҫͨ��invoke������һ���м����ͬʱ����õ�await������Ҫ��async
            //app.Use(async (context, next) =>
            //{
            //    // Do work that doesn't write to the Response.
            //    _logger.LogInformation("Hello from 1nd delegate. begin ");
            //    await next.Invoke();
            //    _logger.LogInformation("Hello from 1nd delegate. end ");
            //    // Do logging or other work that doesn't write to the Response.
            //});
            ////use����2������ԭ����Use����
            //app.Use(
            //     (next) =>{
            //         return async c => {
            //             _logger.LogInformation("Hello from 2nd delegate. begin ");
            //             await next.Invoke(c);
            //             _logger.LogInformation("Hello from 2nd delegate. end ");
            //         };

            //    }
            //    );

            /////use����3������use����չ�������������һ���м��������������Invoke
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

            #region map��ʽ���
            /*
            app.Map("/map1", HandleMapTest1);
              
            app.Map("/mapPageExample/map", HandleMultiSeg);
            //url����ӳ����õ��м���������м������ִ���ˣ���Ϊ��map�м����û�е��ú������м����
          
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

            //������̬�ļ����ʷ�ʽ����֮��ſ��Է���wwwroot·���µľ�̬��Դ��
            app.UseStaticFiles();

            //����·�ɣ�������http request��pipeline�к��ʵ�λ�ü�����·�ɵ㣬���UseEndpoings����·�ɷ�ʽ��
            app.UseRouting();
            //������Ȩ������λ����Ҫ������·�ɺ�·����֤��ʽ֮�䡣
            app.UseAuthorization();
            //���ö˵�·�ɷ�ʽ�������UseRouting
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });


            #region �񻷾�����
            
            _logger.LogInformation($"{env.ApplicationName} {env.EnvironmentName}");
            _logger.LogInformation($"{env.ContentRootPath}");
            _logger.LogInformation($"{env.WebRootPath}");
            _logger.LogInformation($"{env.WebRootFileProvider.ToString()}");
            _logger.LogInformation($"��������:{env.IsDevelopment()}");
            #endregion
        }

        private static void HandleMapTest1(IApplicationBuilder app)
        {
            //������м����δ���ú����ģ�����������·��������ͽ����ˡ�
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }
        private static void HandleMultiSeg(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
                   {
                       _logger.LogInformation(" HandleMultiSegִ����map�����������Ƿ�ִ����ҳ���� ");
                       
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
