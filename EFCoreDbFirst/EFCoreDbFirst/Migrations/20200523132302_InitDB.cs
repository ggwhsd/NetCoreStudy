using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace EFCoreDbFirst.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "emp",
                columns: table => new
                {
                    empno = table.Column<int>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    last_name = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    birthdate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.empno);
                });

            migrationBuilder.CreateTable(
                name: "instrument",
                columns: table => new
                {
                    InstrumentID = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ExchangeID = table.Column<string>(unicode: false, maxLength: 45, nullable: false),
                    PriceTick = table.Column<double>(nullable: false),
                    InstrumentName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductID = table.Column<string>(unicode: false, maxLength: 45, nullable: false, defaultValueSql: "'xxx'"),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    MaxMarketOrderVolume = table.Column<int>(nullable: false),
                    MinMarketOrderVolume = table.Column<int>(nullable: false),
                    MaxLimitOrderVolume = table.Column<int>(nullable: false),
                    MinLimitOrderVolume = table.Column<int>(nullable: false),
                    VolumeMultiple = table.Column<int>(nullable: false, comment: "合约乘数"),
                    OpenDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "date", nullable: true),
                    ProductClass = table.Column<string>(unicode: false, maxLength: 45, nullable: false, defaultValueSql: "'xxx'", comment: "工具类型：期权、期货、现货等等"),
                    IsTrading = table.Column<byte>(nullable: false),
                    UnderlyingInstrID = table.Column<string>(unicode: false, maxLength: 50, nullable: true, defaultValueSql: "'xxx'", comment: "如果是期权，就会有标的合约")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instrument", x => x.InstrumentID);
                });

            migrationBuilder.CreateTable(
                name: "marketmakerprop",
                columns: table => new
                {
                    StrategyID = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    InstrumentID = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ExchangeID = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    tickPrice = table.Column<double>(nullable: false),
                    MMOrderType = table.Column<string>(fixedLength: true, maxLength: 1, nullable: false, defaultValueSql: "'O'", comment: @"[O]rder,[Q]uote,[R]eQuote
"),
                    MMPriceType = table.Column<string>(fixedLength: true, maxLength: 1, nullable: false, defaultValueSql: "'J'", comment: "[M]id,[J]oinMarket,[WeightAvg]"),
                    MMPriceBidOffset = table.Column<double>(nullable: false, comment: @"x * tickPrice used forMMPriceType. Mid 
"),
                    MMPriceAskOffset = table.Column<double>(nullable: false, comment: @"x * tickPrice used forMMPriceType. Mid 
"),
                    FunctionType = table.Column<string>(fixedLength: true, maxLength: 1, nullable: false, defaultValueSql: "'c'", comment: "history： 'c' means can marketmaker ，‘s' just subscribe the market"),
                    PositionEffect = table.Column<string>(unicode: false, maxLength: 15, nullable: false, defaultValueSql: "'OPEN'", comment: "OPEN,CLOSE,CLOSE_TODAY,CLOSE_YESTERDAY"),
                    MarketMakerVolume = table.Column<byte>(nullable: false, defaultValueSql: "'1'"),
                    joinVolume1 = table.Column<int>(nullable: false),
                    joinVolume2 = table.Column<int>(nullable: false),
                    DelayUpdateQuote = table.Column<int>(nullable: false, comment: " unit is millisecond, 0 means not use this function,it will work when it is greater than 0 "),
                    AllowAskTradedVolume = table.Column<int>(nullable: false, defaultValueSql: "'10'"),
                    AllowBidTradedVolume = table.Column<int>(nullable: false, defaultValueSql: "'10'"),
                    marketSpread = table.Column<int>(nullable: false, defaultValueSql: "'1'", comment: " market spread or  quote spread greater equal than marketspread will stop marketmaker "),
                    refillPeriodMilli = table.Column<int>(nullable: false, comment: "milliseconds"),
                    baseInstrID = table.Column<string>(unicode: false, maxLength: 50, nullable: false, comment: "relation instr"),
                    ComboAskSpread = table.Column<double>(nullable: false),
                    ComboAskSpread_offset = table.Column<double>(nullable: false),
                    ComboBidSpread = table.Column<double>(nullable: false),
                    ComboBidSpread_offset = table.Column<double>(nullable: false),
                    TradeInstruId = table.Column<string>(unicode: false, maxLength: 50, nullable: false, comment: @"when this instrid traded the strategy will stop temporily
"),
                    TradeTypes = table.Column<string>(unicode: false, maxLength: 45, nullable: false, defaultValueSql: "'M'", comment: @"which types trade will stop temporily
"),
                    RefillTime = table.Column<int>(nullable: false, comment: @"refilltime for temporily trade stop
"),
                    DeltaHedge = table.Column<string>(fixedLength: true, maxLength: 1, nullable: false, defaultValueSql: "'F'"),
                    THedge_Mode = table.Column<string>(unicode: false, maxLength: 45, nullable: false, defaultValueSql: "'LastPaid'"),
                    THedge_InstrId = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    THedge_PositionEffect = table.Column<string>(unicode: false, maxLength: 15, nullable: false, defaultValueSql: "'OPEN'"),
                    THedge_PriceOffset = table.Column<double>(nullable: false),
                    OHedge_Mode = table.Column<string>(unicode: false, maxLength: 45, nullable: false, defaultValueSql: "'LastPaid'"),
                    OHedge_InstrId = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    OHedge_PositionEffect = table.Column<string>(unicode: false, maxLength: 50, nullable: false, defaultValueSql: "'OPEN'"),
                    OHedge_PriceOffset1 = table.Column<double>(nullable: false),
                    OHedge_PriceOffset2 = table.Column<double>(nullable: false, comment: "less 0 means it is some ticks below market (e.g. more difficult to trade with market)"),
                    OHedge_Percentage = table.Column<double>(nullable: false),
                    OHedge_LostBidPrice = table.Column<double>(nullable: false, comment: "must greater than 0 ，means lost price of hedge order based market opposite price"),
                    OHedge_LostAskPrice = table.Column<double>(nullable: false, comment: "must greater than 0 ，means lost price of hedge order based market opposite price"),
                    OHedge_PayUpStepTicks = table.Column<int>(nullable: false, defaultValueSql: "'1'", comment: "must greater equal 0 , integar, default = 1"),
                    OHedge_PayUpMaxCount = table.Column<int>(nullable: false, defaultValueSql: "'1'", comment: "must greater equal 0 , integar, default = 1, e.g. : 2 means I will try to send a new stopLost order if first stopLost order is not traded"),
                    isEnabed = table.Column<byte>(nullable: false),
                    bid5Volume1 = table.Column<int>(nullable: false),
                    ask5Volume1 = table.Column<int>(nullable: false),
                    MoveBid5LV = table.Column<int>(nullable: false),
                    MoveAsk5LV = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.StrategyID);
                });

            migrationBuilder.CreateTable(
                name: "spreadstatisticrules",
                columns: table => new
                {
                    ruleId = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    instrId = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    volume = table.Column<int>(nullable: false),
                    spreadTicks = table.Column<int>(nullable: false),
                    tickPrice = table.Column<double>(nullable: false),
                    targetSec = table.Column<double>(nullable: false),
                    tradeTimeName = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.ruleId);
                });

            migrationBuilder.CreateTable(
                name: "tradetime",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false, comment: "标识"),
                    beginTime = table.Column<string>(unicode: false, maxLength: 45, nullable: true),
                    endTime = table.Column<string>(unicode: false, maxLength: 45, nullable: true),
                    phase = table.Column<string>(unicode: false, maxLength: 45, nullable: true, comment: "阶段，每个阶段的时间不可以重复")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tradetime", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    userName = table.Column<string>(unicode: false, maxLength: 45, nullable: true),
                    passWord = table.Column<string>(unicode: false, maxLength: 45, nullable: true),
                    lastConnectTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "tradetime",
                column: "id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "emp");

            migrationBuilder.DropTable(
                name: "instrument");

            migrationBuilder.DropTable(
                name: "marketmakerprop");

            migrationBuilder.DropTable(
                name: "spreadstatisticrules");

            migrationBuilder.DropTable(
                name: "tradetime");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
