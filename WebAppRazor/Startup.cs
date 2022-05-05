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
    /// ���������������࣬��������ν����������������֣�ֻҪ�ڶ�ӦPrograms�д���Hostʱ���õ�������������
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
            // ��ִ�����д���ǰ���Ѿ������ٶ����������ȥ�ˡ�
            services.AddRazorPages();
            services.AddTransient<IStartupFilter,
                      RequestSetOptionsStartupFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// http����ܵ�����ʵ�����������ģʽ�е���������һ��http�������ܵ��У��ܵ������кܶ���м����������д�����������ݸ���һ���м������Ȼ�м��Ҳ����ִ����ֹ����
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> _logger)
        {


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
        }


    }
}
