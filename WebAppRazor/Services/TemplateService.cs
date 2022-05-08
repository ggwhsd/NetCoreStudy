using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppRazor.Services
{
    public interface TemplateServiceInterface<T>
    {
        public string Serve(T t);
    }
    /// <summary>
    /// 自定义泛型类和接口，通过依赖注入使用。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TemplateService<T> : TemplateServiceInterface<T> 
    {
        public T value;
        public TemplateService()
        {
          
        }


        public string Serve(T v)
        {
            return v.ToString();
        }
    }

   
}
