using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
            CreateHostBuilder(args).Build().Run();
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
                        //configHost.AddJsonFile("hostsetting.json", optional: true);
                        //ʵ������CreateDefaultBuilder���Ѿ���������еĻ�������������ֻ�Ǹ���ʾ����
                        configHost.AddEnvironmentVariables(prefix: "PREFIX_");

                        //configHost.AddCommandLine(args); 
                    }
                ).ConfigureAppConfiguration((context, configure)=>
                    {
                        
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //UseStartup��һ����չ������
                    
                    webBuilder.UseStartup<Startup>();
                });

   
    }
}
