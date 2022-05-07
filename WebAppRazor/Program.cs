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
        /// 入口函数，目前使用的.net5还是从main开始。
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //1)获取建造者
            //2)进行Build()创建对象
            //3)调用Run启动运行。
            var host = CreateHostBuilder(args).Build();
            //输出所有环境变量
            //通过Get方式主动获取注册的对象
            var config = host.Services.GetRequiredService<IConfiguration>();
            //获取配置：环境变量、json文件、xml文件
            foreach (var c in config.AsEnumerable())
            {
                Console.WriteLine(c.Key + " = " + c.Value);
            }
            host.Run();
        }

        /// <summary>
        /// 创建主机的Builder，建造者模式。
        /// </summary>
        /// <param name="args">命令行参数</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>

        
    // ConfigureWebHostDefaults方法用于配置主机，参数为一个委托。委托里面具体实现了各种配置，包括可用组件以及添加服务。
    Host.CreateDefaultBuilder(args).ConfigureHostConfiguration(
                configHost =>
                    {
                        
                        //修改主机的配置，这些配置会被IHostEnvironment来使用。
                        //configHost.SetBasePath(Directory.GetCurrentDirectory());
                        
                        //configHost.AddJsonFile("hostsetting.json", optional: true, reloadOnChange:true);
                        //实际上在CreateDefaultBuilder中已经添加了所有的环境变量，这里只是给个示例。

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
                        //在这里配置应用服务器相关的，比如启动url，只有这里配置的json文件的对应url，才能被有效使用，如果在ConfigureHostConfiguration中配置则不会被web app使用，只会读取到配置对象中。
                        configure.AddJsonFile("hostsetting.json", optional: true, reloadOnChange: true);
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //UseStartup是一个扩展方法，
                    
                    webBuilder.UseStartup<Startup>();
                });

   
    }
}
