using System;
using System.Collections.Generic;

namespace EFCoreDbFirst.Models
{
    public partial class Marketmakerprop
    {
        public string StrategyId { get; set; }
        public string InstrumentId { get; set; }
        public string ExchangeId { get; set; }
        public double TickPrice { get; set; }
        public string MmorderType { get; set; }
        public string MmpriceType { get; set; }
        public double MmpriceBidOffset { get; set; }
        public double MmpriceAskOffset { get; set; }
        public string FunctionType { get; set; }
        public string PositionEffect { get; set; }
        public byte MarketMakerVolume { get; set; }
        public int JoinVolume1 { get; set; }
        public int JoinVolume2 { get; set; }
        public int DelayUpdateQuote { get; set; }
        public int AllowAskTradedVolume { get; set; }
        public int AllowBidTradedVolume { get; set; }
        public int MarketSpread { get; set; }
        public int RefillPeriodMilli { get; set; }
        public string BaseInstrId { get; set; }
        public double ComboAskSpread { get; set; }
        public double ComboAskSpreadOffset { get; set; }
        public double ComboBidSpread { get; set; }
        public double ComboBidSpreadOffset { get; set; }
        public string TradeInstruId { get; set; }
        public string TradeTypes { get; set; }
        public int RefillTime { get; set; }
        public string DeltaHedge { get; set; }
        public string ThedgeMode { get; set; }
        public string ThedgeInstrId { get; set; }
        public string ThedgePositionEffect { get; set; }
        public double ThedgePriceOffset { get; set; }
        public string OhedgeMode { get; set; }
        public string OhedgeInstrId { get; set; }
        public string OhedgePositionEffect { get; set; }
        public double OhedgePriceOffset1 { get; set; }
        public double OhedgePriceOffset2 { get; set; }
        public double OhedgePercentage { get; set; }
        public double OhedgeLostBidPrice { get; set; }
        public double OhedgeLostAskPrice { get; set; }
        public int OhedgePayUpStepTicks { get; set; }
        public int OhedgePayUpMaxCount { get; set; }
        public byte IsEnabed { get; set; }
        public int Bid5Volume1 { get; set; }
        public int Ask5Volume1 { get; set; }
        public int MoveBid5Lv { get; set; }
        public int MoveAsk5Lv { get; set; }
    }
}
