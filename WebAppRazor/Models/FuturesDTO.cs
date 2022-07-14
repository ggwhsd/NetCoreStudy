using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppRazor.Models
{
    public class FuturesDTO
    {
        [StringLength(100)]  //不超过100个字符
        public string Name { get; set; }
        [Range(0, 150)]  //年龄不小小于0，也不能大于150
        public int Age { get; set; }
        [Range(typeof(DateTime), "1/1/2022", "12/1/2023",
        ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime ReleaseDate { get; set; }
    }
}
