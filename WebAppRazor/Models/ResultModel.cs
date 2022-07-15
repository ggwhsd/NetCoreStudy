using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppRazor.Models
{
    public class ResultModel
    {
        /// <summary>
        /// 状态码，对于错误情况下，可以有多个错误原因，使用状态码来区分,0标识正确
        /// </summary>
        public int ReturnCode { get; set; } = 0;
        /// <summary>
        /// 错误信息，作为错误的补充说明
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 数据是否为多结果的集合
        /// </summary>
        public bool DataIsList { get; set; } = false;
        /// <summary>
        /// 数据条数，数据集合才会用到这个字段，Data中包含的数据记录数量。
        /// </summary>
        public int DataCount { get; set; } = 1;
        /// <summary>
        /// 添加静态方法，方便快速使用
        /// 单记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultModel Ok(object data)
        {
            return new ResultModel { Data = data, ErrorMessage = null, DataIsList = false, DataCount = 1, ReturnCode = 0 };
        }
        /// <summary>
        ///
        /// 数据为集合列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultModel Ok(object data, int count)
        {
            return new ResultModel { Data = data, ErrorMessage = null, DataIsList = true, DataCount = count, ReturnCode = 0 };
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="str">错误信息</param>
        /// <param name="code">状态码</param>
        /// <returns></returns>
        public static ResultModel Error(string str, int code)
        {
            return new ResultModel { Data = null, ErrorMessage = str, DataIsList = false, DataCount = 0, ReturnCode = code };
        }
    }
}
