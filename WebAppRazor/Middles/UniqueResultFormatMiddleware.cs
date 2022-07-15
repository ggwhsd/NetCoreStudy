using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppRazor.Models;

namespace WebAppRazor.Middles
{
    public class UniqueResultFormatMiddleware
    {
        private readonly RequestDelegate _next;
        public UniqueResultFormatMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            Exception err = null;
            try
            {
                //抛给下一个中间件
                await _next(context);
                //等下一个中间件调用完成返回以后，才会执行后续，所以后续的操作是在response过程中发生的
            }
            catch (Exception ex)
            {
                err = ex;
            }
            finally
            {
                await WriteWebApiUniqueFormatAsync(context, err);
            }
        }
        /// <summary>
        /// 对于异常信息，会按照统一格式返回一个数据结构
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task WriteWebApiUniqueFormatAsync(HttpContext context, Exception exception)
        {
            if (exception != null)
            {
                var response = context.Response;
                var message = exception.InnerException == null ? exception.Message : exception.InnerException.Message;
                response.ContentType = "application/json";
                await response.WriteAsync(JsonConvert.SerializeObject(ResultModel.Error(message, 400))).ConfigureAwait(false);
            }
            else
            {
                var code = context.Response.StatusCode;
                switch (code)
                {
                    case 200:
                        return;
                    case 204:
                        return;
                    case 401:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(ResultModel.Error("请重新登录.", code))).ConfigureAwait(false);
                        break;
                    case 400: //数据校验默认的400异常模块会先执行Response，所以下面这行只是附着在之后的信息，无法达到统一格式的效果
                              // await  context.Response.WriteAsync(JsonConvert.SerializeObject(ResultModel.Error("数据校验错误.", code))).ConfigureAwait(false);
                        break;
                    default:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(ResultModel.Error("未知错误", code))).ConfigureAwait(false);
                        break;
                }
            }
        }
    }
}
