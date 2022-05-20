using System;
using System.IO;
using Microsoft.ML;
using TaxiFarePrediction;

namespace TaxiFarePrediction
{
    class Program
    {
        //

        static void Main(string[] args)
        {
            //包含具有用于定型模型的数据集的文件的路径。
            string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-train.csv");
            
            //包含用于存储定型模型的文件的路径。
            string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

            MLContext mlContext = new MLContext(seed: 0);
            
            var model = Train(mlContext, _trainDataPath);

            Evaluate(mlContext, model);

            TestSinglePrediction(mlContext, model);

            Console.WriteLine("Hello World!");
        }

        static ITransformer Train(MLContext mlContext, string dataPath)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(dataPath, hasHeader: true, separatorChar: ',');
            //由于要预测出租车车费，FareAmount 列是将预测的 Label（模型的输出）。 使用 CopyColumnsEstimator 转换类以复制 FareAmount，并添加以下代码：
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount").
                //定型模型的算法需要数字特性,转换为数字
                Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName: "VendorId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
                //因为默认情况下，学习算法仅处理“特征”列的特征，只有这样，才会让其对新的列作为特征列进行学习。
                .Append(mlContext.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded", "PassengerCount", "TripDistance", "PaymentTypeEncoded"))
                //添加回归学习模型
                .Append(mlContext.Regression.Trainers.FastTree());
            //训练
            var model = pipeline.Fit(dataView);

            return model;
        }

       static void Evaluate(MLContext mlContext, ITransformer model)
        {
            //包含具有用于评估模型的数据集的文件的路径。
            string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-test.csv");
            IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(_testDataPath, hasHeader: true, separatorChar: ',');
            //预测数据集
            var predictions = model.Transform(dataView);

            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");  //RSquared 在 0 和 1 之间取值，值越接近 1，模型就越好
            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}"); // 指标越低，模型就越好。
        }

        static void TestSinglePrediction(MLContext mlContext, ITransformer model)
        {
            
            var predictionFunction = mlContext.Model.CreatePredictionEngine<TaxiTrip, TaxiTripFarePrediction>(model);

            var taxiTripSample = new TaxiTrip()
            {
                VendorId = "VTS",
                RateCode = "1",
                PassengerCount = 1,
                TripTime = 1140,
                TripDistance = 3.75f,
                PaymentType = "CRD",
                FareAmount = 0 // To predict. Actual/Observed = 15.5
            };

            var prediction = predictionFunction.Predict(taxiTripSample);  //对单个数据实例进行预测

            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted fare: {prediction.FareAmount:0.####}, actual fare: 15.5");
            Console.WriteLine($"**********************************************************************");
        }
    }
}
