using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bicycle_rental
{

    class ModelInput
    {
        /// <summary>
        /// RentalDate：观测日期。
        /// </summary>
        public DateTime RentalDate { get; set; }
        /// <summary>
        /// 观测年份编码（0=2011，1=2012）。
        /// </summary>
        public float Year { get; set; }
        /// <summary>
        /// TotalRentals：观测日当天自行车租赁总数。
        /// </summary>
        public float TotalRentals { get; set; }
    }
}
