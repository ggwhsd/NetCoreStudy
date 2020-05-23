using System;
using System.Collections.Generic;

namespace EFCoreDbFirst.Models
{
    public partial class Spreadstatisticrules
    {
        public string RuleId { get; set; }
        public string InstrId { get; set; }
        public int Volume { get; set; }
        public int SpreadTicks { get; set; }
        public double TickPrice { get; set; }
        public double TargetSec { get; set; }
        public string TradeTimeName { get; set; }
    }
}
