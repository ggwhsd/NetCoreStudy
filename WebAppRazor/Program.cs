using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppRazor
{
    public class Program
    {
        /// <summary>
        /// ��ں�����Ŀǰʹ�õ�.net5���Ǵ�main��ʼ��
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //1)��ȡ������
            //2)����Build()��������
            //3)����Run�������С�
            var host = CreateHostBuilder(args).Build();
            //������л�������
            //ͨ��Get��ʽ������ȡע��Ķ���
            var config = host.Services.GetRequiredService<IConfiguration>();
            //��ȡ���ã�����������json�ļ���xml�ļ�
            foreach (var c in config.AsEnumerable())
            {
                Console.WriteLine(c.Key + " = " + c.Value);
            }
            host.Run();
        }

        /// <summary>
        /// ����������Builder��������ģʽ��
        /// </summary>
        /// <param name="args">�����в���</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>

        
    // ConfigureWebHostDefaults����������������������Ϊһ��ί�С�ί���������ʵ���˸������ã�������������Լ���ӷ���
    Host.CreateDefaultBuilder(args).ConfigureHostConfiguration(
                configHost =>
                    {
                        
                        //�޸����������ã���Щ���ûᱻIHostEnvironment��ʹ�á�
                        //configHost.SetBasePath(Directory.GetCurrentDirectory());
                        
                        //configHost.AddJsonFile("hostsetting.json", optional: true, reloadOnChange:true);
                        //ʵ������CreateDefaultBuilder���Ѿ���������еĻ�������������ֻ�Ǹ���ʾ����

                        configHost.AddEnvironmentVariables(prefix: "PREFIX_");

                        //var switchMappings = new Dictionary<string, string>()
                        // {
                        //     { "-k1", "key1" },
                        //     { "-k2", "key2" },
                        //     { "--k3", "key3" },
                        //     { "--k4", "key4" },
                        //     { "--k5", "key5" },
                        //     { "--k6", "key6" },
                        // };

                        //configHost.AddCommandLine(args, switchMappings); 
                    }
                ).ConfigureAppConfiguration((context, configure)=>
                    {
                        //����������Ӧ�÷�������صģ���������url��ֻ���������õ�json�ļ��Ķ�Ӧurl�����ܱ���Чʹ�ã������ConfigureHostConfiguration�������򲻻ᱻweb appʹ�ã�ֻ���ȡ�����ö����С�
                        configure.AddJsonFile("hostsetting.json", optional: true, reloadOnChange: true);
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //UseStartup��һ����չ������
                    
                    webBuilder.UseStartup<Startup>();
                });

   
    }
}
