using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// http����ܵ�����ʵ�����������ģʽ�е���������һ��http�������ܵ��У��ܵ������кܶ���м����������д�����������ݸ���һ���м������Ȼ�м��Ҳ����ִ����ֹ����
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
