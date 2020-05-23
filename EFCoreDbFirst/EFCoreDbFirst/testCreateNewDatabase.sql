  IF EXISTS(SELECT 1 FROM information_schema.tables 
  WHERE table_name = '
'__EFMigrationsHistory'' AND table_schema = DATABASE()) 
BEGIN
CREATE TABLE `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

END;

CREATE TABLE `emp` (
    `empno` int unsigned NOT NULL AUTO_INCREMENT,
    `first_name` varchar(20) NULL,
    `last_name` varchar(20) NULL,
    `birthdate` date NULL,
    PRIMARY KEY (`empno`)
);

CREATE TABLE `instrument` (
    `InstrumentID` varchar(50) NOT NULL,
    `ExchangeID` varchar(45) NOT NULL,
    `PriceTick` double NOT NULL,
    `InstrumentName` varchar(50) NOT NULL,
    `ProductID` varchar(45) NOT NULL DEFAULT 'xxx',
    `CreateDate` date NULL,
    `MaxMarketOrderVolume` int NOT NULL,
    `MinMarketOrderVolume` int NOT NULL,
    `MaxLimitOrderVolume` int NOT NULL,
    `MinLimitOrderVolume` int NOT NULL,
    `VolumeMultiple` int NOT NULL,
    `OpenDate` date NULL,
    `ExpireDate` date NULL,
    `ProductClass` varchar(45) NOT NULL DEFAULT 'xxx',
    `IsTrading` tinyint NOT NULL,
    `UnderlyingInstrID` varchar(50) NULL DEFAULT 'xxx',
    PRIMARY KEY (`InstrumentID`)
);

CREATE TABLE `marketmakerprop` (
    `StrategyID` varchar(50) NOT NULL,
    `InstrumentID` varchar(50) NOT NULL,
    `ExchangeID` varchar(50) NOT NULL,
    `tickPrice` double NOT NULL,
    `MMOrderType` char(1) NOT NULL DEFAULT 'O',
    `MMPriceType` char(1) NOT NULL DEFAULT 'J',
    `MMPriceBidOffset` double NOT NULL,
    `MMPriceAskOffset` double NOT NULL,
    `FunctionType` char(1) NOT NULL DEFAULT 'c',
    `PositionEffect` varchar(15) NOT NULL DEFAULT 'OPEN',
    `MarketMakerVolume` tinyint NOT NULL DEFAULT '1',
    `joinVolume1` int NOT NULL,
    `joinVolume2` int NOT NULL,
    `DelayUpdateQuote` int NOT NULL,
    `AllowAskTradedVolume` int NOT NULL DEFAULT '10',
    `AllowBidTradedVolume` int NOT NULL DEFAULT '10',
    `marketSpread` int NOT NULL DEFAULT '1',
    `refillPeriodMilli` int NOT NULL,
    `baseInstrID` varchar(50) NOT NULL,
    `ComboAskSpread` double NOT NULL,
    `ComboAskSpread_offset` double NOT NULL,
    `ComboBidSpread` double NOT NULL,
    `ComboBidSpread_offset` double NOT NULL,
    `TradeInstruId` varchar(50) NOT NULL,
    `TradeTypes` varchar(45) NOT NULL DEFAULT 'M',
    `RefillTime` int NOT NULL,
    `DeltaHedge` char(1) NOT NULL DEFAULT 'F',
    `THedge_Mode` varchar(45) NOT NULL DEFAULT 'LastPaid',
    `THedge_InstrId` varchar(50) NOT NULL,
    `THedge_PositionEffect` varchar(15) NOT NULL DEFAULT 'OPEN',
    `THedge_PriceOffset` double NOT NULL,
    `OHedge_Mode` varchar(45) NOT NULL DEFAULT 'LastPaid',
    `OHedge_InstrId` varchar(50) NOT NULL,
    `OHedge_PositionEffect` varchar(50) NOT NULL DEFAULT 'OPEN',
    `OHedge_PriceOffset1` double NOT NULL,
    `OHedge_PriceOffset2` double NOT NULL,
    `OHedge_Percentage` double NOT NULL,
    `OHedge_LostBidPrice` double NOT NULL,
    `OHedge_LostAskPrice` double NOT NULL,
    `OHedge_PayUpStepTicks` int NOT NULL DEFAULT '1',
    `OHedge_PayUpMaxCount` int NOT NULL DEFAULT '1',
    `isEnabed` tinyint NOT NULL,
    `bid5Volume1` int NOT NULL,
    `ask5Volume1` int NOT NULL,
    `MoveBid5LV` int NOT NULL,
    `MoveAsk5LV` int NOT NULL,
    PRIMARY KEY (`StrategyID`)
);

CREATE TABLE `spreadstatisticrules` (
    `ruleId` varchar(50) NOT NULL,
    `instrId` varchar(50) NOT NULL,
    `volume` int NOT NULL,
    `spreadTicks` int NOT NULL,
    `tickPrice` double NOT NULL,
    `targetSec` double NOT NULL,
    `tradeTimeName` varchar(50) NOT NULL,
    PRIMARY KEY (`ruleId`)
);

CREATE TABLE `tradetime` (
    `id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `beginTime` varchar(45) NULL,
    `endTime` varchar(45) NULL,
    `phase` varchar(45) NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `users` (
    `id` int NOT NULL AUTO_INCREMENT,
    `userName` varchar(45) NULL,
    `passWord` varchar(45) NULL,
    `lastConnectTime` datetime NULL,
    PRIMARY KEY (`id`)
);

CREATE UNIQUE INDEX `id_UNIQUE` ON `tradetime` (`id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20200523132302_InitDB', '3.1.4');

