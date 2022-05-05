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
        /// 入口函数，目前使用的.net5还是从main开始。
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //1)获取建造者
            //2)进行Build()创建对象
            //3)调用Run启动运行。
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 创建主机的Builder，建造者模式。
        /// </summary>
        /// <param name="args">命令行参数</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>

            
            // ConfigureWebHostDefaults方法用于配置主机，参数为一个委托。委托里面具体实现了各种配置，包括可用组件以及添加服务。
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
                {
                    //UseStartup是一个扩展方法，
                    webBuilder.UseStartup<Startup>();
                });

   
    }
}
