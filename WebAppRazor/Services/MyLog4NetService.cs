using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppRazor.Services
{
    /// <summary>
    /// 把第三方的类库封装成一个类，将该类注入到netcore中。通过类名进行访问。
    /// </summary>
    public class MyLog4NetService
    {
        private ILoggerRepository repository;
        public MyLog4NetService()
        {
            //repository代表一套环境配置，简单理解就是对应一个log4Net配置文件。最终都是在LogManager中有保存这些配置。
            repository = LogManager.CreateRepository("Log4NETRepository");
            //将日志文件和repository进行绑定数据。
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            
        }

        public ILog GetLogger<T>()
        {
            ILog logger = LogManager.GetLogger(repository.Name, typeof(T));
            return logger;
        }
    }
}
