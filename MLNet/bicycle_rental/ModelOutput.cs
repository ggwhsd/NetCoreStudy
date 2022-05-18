using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bicycle_rental
{
    class ModelOutput
    {
        /// <summary>
        /// 预测时段内的预测值。
        /// </summary>
        public float[] ForecastedRentals { get; set; }
        /// <summary>
        /// 预测时段内的最低预测值
        /// </summary>
        public float[] LowerBoundRentals { get; set; }
        /// <summary>
        /// 预测时段内的最高预测值
        /// </summary>
        public float[] UpperBoundRentals { get; set; }
    }
}
