using System;
using System.Collections.Generic;

namespace EFCoreDbFirst.Models
{
    public partial class Tradetime
    {
        public string Name { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public string Phase { get; set; }
        public int Id { get; set; }
    }
}
