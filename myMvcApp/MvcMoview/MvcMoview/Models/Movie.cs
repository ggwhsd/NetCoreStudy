using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMoview.Models
{
    public class Movie
    {
        public int Id { get; set; }   //数据库需要 Id 字段以获取主键。
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        /*
         * 
         * 指定数据的类型 (Date)。 通过此特性：
用户无需在数据字段中输入时间信息。
仅显示日期，而非时间信息。
         */
        [Display(Name = "Release Date")]
        [DataType(DataType.DateTime)]
        public DateTime ReleaseTime { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        [Required]
        public string Rating { get; set; }

    }
}
