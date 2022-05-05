using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
                {
                    //UseStartup��һ����չ������
                    webBuilder.UseStartup<Startup>();
                });

   
    }
}
