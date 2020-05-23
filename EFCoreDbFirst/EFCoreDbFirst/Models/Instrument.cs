using System;
using System.Collections.Generic;

namespace EFCoreDbFirst.Models
{
    public partial class Instrument
    {
        public string InstrumentId { get; set; }
        public string ExchangeId { get; set; }
        public double PriceTick { get; set; }
        public string InstrumentName { get; set; }
        public string ProductId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int MaxMarketOrderVolume { get; set; }
        public int MinMarketOrderVolume { get; set; }
        public int MaxLimitOrderVolume { get; set; }
        public int MinLimitOrderVolume { get; set; }
        public int VolumeMultiple { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string ProductClass { get; set; }
        public byte IsTrading { get; set; }
        public string UnderlyingInstrId { get; set; }
    }
}
