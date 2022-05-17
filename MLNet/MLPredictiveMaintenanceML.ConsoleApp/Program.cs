// This file was auto-generated by ML.NET Model Builder. 

using System;
using MLPredictiveMaintenanceML.Model;

namespace MLPredictiveMaintenanceML.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                UDI = 1F,
                Air_temperature_K = 298.1F,
                Process_temperature_K = 308.6F,
                Rotational_speed_rpm = 1551F,
                Torque_Nm = 42.8F,
                Tool_wearmin = 0F,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Machine_failure with predicted Machine_failure from sample data...\n\n");
            Console.WriteLine($"UDI: {sampleData.UDI}");
            Console.WriteLine($"Air_temperature_K: {sampleData.Air_temperature_K}");
            Console.WriteLine($"Process_temperature_K: {sampleData.Process_temperature_K}");
            Console.WriteLine($"Rotational_speed_rpm: {sampleData.Rotational_speed_rpm}");
            Console.WriteLine($"Torque_Nm: {sampleData.Torque_Nm}");
            Console.WriteLine($"Tool_wearmin: {sampleData.Tool_wearmin}");
            Console.WriteLine($"\n\nPredicted Machine_failure value {predictionResult.Prediction} \nPredicted Machine_failure scores: [{String.Join(",", predictionResult.Score)}]\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}
