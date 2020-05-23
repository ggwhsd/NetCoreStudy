using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreDbFirst.Models
{
    public partial class foxmmContext : DbContext
    {
        public foxmmContext()
        {
        }

        public foxmmContext(DbContextOptions<foxmmContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Emp> Emp { get; set; }
        public virtual DbSet<Instrument> Instrument { get; set; }
        public virtual DbSet<Marketmakerprop> Marketmakerprop { get; set; }
        public virtual DbSet<Spreadstatisticrules> Spreadstatisticrules { get; set; }
        public virtual DbSet<Tradetime> Tradetime { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;database=foxmm;user=root;password=gugw12121;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Emp>(entity =>
            {
                entity.HasKey(e => e.Empno)
                    .HasName("PRIMARY");

                entity.ToTable("emp");

                entity.Property(e => e.Empno)
                    .HasColumnName("empno")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.ToTable("instrument");

                entity.Property(e => e.InstrumentId)
                    .HasColumnName("InstrumentID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.ExchangeId)
                    .IsRequired()
                    .HasColumnName("ExchangeID")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.InstrumentName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OpenDate).HasColumnType("date");

                entity.Property(e => e.ProductClass)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'xxx'")
                    .HasComment("工具类型：期权、期货、现货等等");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnName("ProductID")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'xxx'");

                entity.Property(e => e.UnderlyingInstrId)
                    .HasColumnName("UnderlyingInstrID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'xxx'")
                    .HasComment("如果是期权，就会有标的合约");

                entity.Property(e => e.VolumeMultiple).HasComment("合约乘数");
            });

            modelBuilder.Entity<Marketmakerprop>(entity =>
            {
                entity.HasKey(e => e.StrategyId)
                    .HasName("PRIMARY");

                entity.ToTable("marketmakerprop");

                entity.Property(e => e.StrategyId)
                    .HasColumnName("StrategyID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AllowAskTradedVolume).HasDefaultValueSql("'10'");

                entity.Property(e => e.AllowBidTradedVolume).HasDefaultValueSql("'10'");

                entity.Property(e => e.Ask5Volume1).HasColumnName("ask5Volume1");

                entity.Property(e => e.BaseInstrId)
                    .IsRequired()
                    .HasColumnName("baseInstrID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("relation instr");

                entity.Property(e => e.Bid5Volume1).HasColumnName("bid5Volume1");

                entity.Property(e => e.ComboAskSpreadOffset).HasColumnName("ComboAskSpread_offset");

                entity.Property(e => e.ComboBidSpreadOffset).HasColumnName("ComboBidSpread_offset");

                entity.Property(e => e.DelayUpdateQuote).HasComment(" unit is millisecond, 0 means not use this function,it will work when it is greater than 0 ");

                entity.Property(e => e.DeltaHedge)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasDefaultValueSql("'F'");

                entity.Property(e => e.ExchangeId)
                    .IsRequired()
                    .HasColumnName("ExchangeID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FunctionType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasDefaultValueSql("'c'")
                    .HasComment("history： 'c' means can marketmaker ，‘s' just subscribe the market");

                entity.Property(e => e.InstrumentId)
                    .IsRequired()
                    .HasColumnName("InstrumentID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsEnabed).HasColumnName("isEnabed");

                entity.Property(e => e.JoinVolume1).HasColumnName("joinVolume1");

                entity.Property(e => e.JoinVolume2).HasColumnName("joinVolume2");

                entity.Property(e => e.MarketMakerVolume).HasDefaultValueSql("'1'");

                entity.Property(e => e.MarketSpread)
                    .HasColumnName("marketSpread")
                    .HasDefaultValueSql("'1'")
                    .HasComment(" market spread or  quote spread greater equal than marketspread will stop marketmaker ");

                entity.Property(e => e.MmorderType)
                    .IsRequired()
                    .HasColumnName("MMOrderType")
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasDefaultValueSql("'O'")
                    .HasComment(@"[O]rder,[Q]uote,[R]eQuote
");

                entity.Property(e => e.MmpriceAskOffset)
                    .HasColumnName("MMPriceAskOffset")
                    .HasComment(@"x * tickPrice used forMMPriceType. Mid 
");

                entity.Property(e => e.MmpriceBidOffset)
                    .HasColumnName("MMPriceBidOffset")
                    .HasComment(@"x * tickPrice used forMMPriceType. Mid 
");

                entity.Property(e => e.MmpriceType)
                    .IsRequired()
                    .HasColumnName("MMPriceType")
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasDefaultValueSql("'J'")
                    .HasComment("[M]id,[J]oinMarket,[WeightAvg]");

                entity.Property(e => e.MoveAsk5Lv).HasColumnName("MoveAsk5LV");

                entity.Property(e => e.MoveBid5Lv).HasColumnName("MoveBid5LV");

                entity.Property(e => e.OhedgeInstrId)
                    .IsRequired()
                    .HasColumnName("OHedge_InstrId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OhedgeLostAskPrice)
                    .HasColumnName("OHedge_LostAskPrice")
                    .HasComment("must greater than 0 ，means lost price of hedge order based market opposite price");

                entity.Property(e => e.OhedgeLostBidPrice)
                    .HasColumnName("OHedge_LostBidPrice")
                    .HasComment("must greater than 0 ，means lost price of hedge order based market opposite price");

                entity.Property(e => e.OhedgeMode)
                    .IsRequired()
                    .HasColumnName("OHedge_Mode")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'LastPaid'");

                entity.Property(e => e.OhedgePayUpMaxCount)
                    .HasColumnName("OHedge_PayUpMaxCount")
                    .HasDefaultValueSql("'1'")
                    .HasComment("must greater equal 0 , integar, default = 1, e.g. : 2 means I will try to send a new stopLost order if first stopLost order is not traded");

                entity.Property(e => e.OhedgePayUpStepTicks)
                    .HasColumnName("OHedge_PayUpStepTicks")
                    .HasDefaultValueSql("'1'")
                    .HasComment("must greater equal 0 , integar, default = 1");

                entity.Property(e => e.OhedgePercentage).HasColumnName("OHedge_Percentage");

                entity.Property(e => e.OhedgePositionEffect)
                    .IsRequired()
                    .HasColumnName("OHedge_PositionEffect")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'OPEN'");

                entity.Property(e => e.OhedgePriceOffset1).HasColumnName("OHedge_PriceOffset1");

                entity.Property(e => e.OhedgePriceOffset2)
                    .HasColumnName("OHedge_PriceOffset2")
                    .HasComment("less 0 means it is some ticks below market (e.g. more difficult to trade with market)");

                entity.Property(e => e.PositionEffect)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'OPEN'")
                    .HasComment("OPEN,CLOSE,CLOSE_TODAY,CLOSE_YESTERDAY");

                entity.Property(e => e.RefillPeriodMilli)
                    .HasColumnName("refillPeriodMilli")
                    .HasComment("milliseconds");

                entity.Property(e => e.RefillTime).HasComment(@"refilltime for temporily trade stop
");

                entity.Property(e => e.ThedgeInstrId)
                    .IsRequired()
                    .HasColumnName("THedge_InstrId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ThedgeMode)
                    .IsRequired()
                    .HasColumnName("THedge_Mode")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'LastPaid'");

                entity.Property(e => e.ThedgePositionEffect)
                    .IsRequired()
                    .HasColumnName("THedge_PositionEffect")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'OPEN'");

                entity.Property(e => e.ThedgePriceOffset).HasColumnName("THedge_PriceOffset");

                entity.Property(e => e.TickPrice).HasColumnName("tickPrice");

                entity.Property(e => e.TradeInstruId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment(@"when this instrid traded the strategy will stop temporily
");

                entity.Property(e => e.TradeTypes)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'M'")
                    .HasComment(@"which types trade will stop temporily
");
            });

            modelBuilder.Entity<Spreadstatisticrules>(entity =>
            {
                entity.HasKey(e => e.RuleId)
                    .HasName("PRIMARY");

                entity.ToTable("spreadstatisticrules");

                entity.Property(e => e.RuleId)
                    .HasColumnName("ruleId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InstrId)
                    .IsRequired()
                    .HasColumnName("instrId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpreadTicks).HasColumnName("spreadTicks");

                entity.Property(e => e.TargetSec).HasColumnName("targetSec");

                entity.Property(e => e.TickPrice).HasColumnName("tickPrice");

                entity.Property(e => e.TradeTimeName)
                    .IsRequired()
                    .HasColumnName("tradeTimeName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Volume).HasColumnName("volume");
            });

            modelBuilder.Entity<Tradetime>(entity =>
            {
                entity.ToTable("tradetime");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BeginTime)
                    .HasColumnName("beginTime")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.EndTime)
                    .HasColumnName("endTime")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("标识");

                entity.Property(e => e.Phase)
                    .HasColumnName("phase")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("阶段，每个阶段的时间不可以重复");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LastConnectTime).HasColumnName("lastConnectTime");

                entity.Property(e => e.PassWord)
                    .HasColumnName("passWord")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
