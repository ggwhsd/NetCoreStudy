using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace bicycle_rental
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 定义相关路径
            string rootDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../"));
            string dbFilePath = Path.Combine(rootDir, "Data", "DailyDemand.mdf");
            string modelPath = Path.Combine(rootDir, "MLModel.zip");
            var connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbFilePath};Integrated Security=True;Connect Timeout=30;";
            #endregion
            //创建ML环境
            MLContext mlContext = new MLContext();
            //加载数据加载器
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<ModelInput>();
            //建立具有查询语句的数据源
            string query = "SELECT RentalDate, CAST(Year as REAL) as Year, CAST(TotalRentals as REAL) as TotalRentals FROM Rentals";
            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance,
                                connectionString,
                                query);
            //使用加载器加载数据源
            IDataView dataView = loader.Load(dbSource);

            //获取第一年数据，用于训练，<1
            IDataView firstYearData = mlContext.Data.FilterRowsByColumn(dataView, "Year", upperBound: 1);
            //获取第二年数据，用于预测,>=1
            IDataView secondYearData = mlContext.Data.FilterRowsByColumn(dataView, "Year", lowerBound: 1);
            //训练模型
            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                                                outputColumnName: "ForecastedRentals",
                                                inputColumnName: "TotalRentals",
                                                windowSize: 7,   //7条数据为一个时段分析
                                                seriesLength: 30,
                                                trainSize: 365,  //获取365条数据
                                                horizon: 7,  //预测7个值
                                                confidenceLevel: 0.95f,   //预测的值得是上下限范围内
                                                confidenceLowerBoundColumn: "LowerBoundRentals",
                                                confidenceUpperBoundColumn: "UpperBoundRentals");
            //开始训练
            SsaForecastingTransformer forecaster = forecastingPipeline.Fit(firstYearData);
            //进行评估
            Evaluate(secondYearData, forecaster, mlContext);
            //保存训练之后的模型，以用于其他项目
            var forecastEngine = forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);
            forecastEngine.CheckPoint(mlContext, modelPath);
            //进行预测
            Forecast(secondYearData, 7, forecastEngine, mlContext);

            Console.WriteLine("Hello World!");
        }

        static void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            //评估
            IDataView predictions = model.Transform(testData);
            IEnumerable<float> actual =
                mlContext.Data.CreateEnumerable<ModelInput>(testData, true)
                    .Select(observed => observed.TotalRentals);

            IEnumerable<float> forecast =
                mlContext.Data.CreateEnumerable<ModelOutput>(predictions, true)
                    .Select(prediction => prediction.ForecastedRentals[0]);
            //计算实际值和预测值之间的差值（通常称为“误差”）。
            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);
            //平均绝对误差
            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            //均方根误差
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error
            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");
        }

        static void Forecast(IDataView testData, int horizon, TimeSeriesPredictionEngine<ModelInput, ModelOutput> forecaster, MLContext mlContext)
        {
            ModelOutput forecast = forecaster.Predict();
            IEnumerable<string> forecastOutput =
    mlContext.Data.CreateEnumerable<ModelInput>(testData, reuseRowObject: false)
        .Take(horizon)
        .Select((ModelInput rental, int index) =>
        {
            string rentalDate = rental.RentalDate.ToShortDateString();
            float actualRentals = rental.TotalRentals;
            float lowerEstimate = Math.Max(0, forecast.LowerBoundRentals[index]);
            float estimate = forecast.ForecastedRentals[index];
            float upperEstimate = forecast.UpperBoundRentals[index];
            return $"Date: {rentalDate}\n" +
            $"Actual Rentals: {actualRentals}\n" +
            $"Lower Estimate: {lowerEstimate}\n" +
            $"Forecast: {estimate}\n" +
            $"Upper Estimate: {upperEstimate}\n";
        });

            Console.WriteLine("Rental Forecast");
            Console.WriteLine("---------------------");
            foreach (var prediction in forecastOutput)
            {
                Console.WriteLine(prediction);
            }
        }
    }
  

}
