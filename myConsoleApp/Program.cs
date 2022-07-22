using System;
using System.Threading;
using System.Threading.Tasks;
using WinFormsApp1;

namespace myConsoleApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            /*
            IndexStudy.Test();

            Lambda lam = new Lambda();
            lam.Init();
            lam.Search(6);
            lam.TestDelegate();
            lam.TestLambda();
            lam.TestLambda2();

            LinqUse lu = new LinqUse();
            lu.TestOne();*/

            /*AsyncTask.ConsumeManyTime();
            Console.WriteLine("按任意键继续");
            Console.ReadLine();
            AsyncTask.ContinueTaskWithConsumeManyTime();
            Console.WriteLine("按任意键继续");
            Console.ReadLine();
            AsyncTask.TaskWhenConsumeManyTime();
            Console.WriteLine("按任意键继续");
            Console.ReadLine();
            AsyncTask.TestOne();
            AsyncTask.TestWithContinue();
            AsyncTask.Testpallral();
            AsyncTask.Testpallral2();
            */

            /*TaskWait.TestTaskOne();
            TaskWait.TestTaskTwo();
            TaskWait.TestTaskThree_TimeOut();
            TaskWait.TestTaskAny();
            TaskWait.TestTaskCancel();*/

            // DataModelStudy dataModel = new DataModelStudy();
            /*dataModel.createRecord();
            dataModel.equalRecords();
            dataModel.withRecord();
            dataModel.anonymousTypesOne();
            dataModel.anonymousTypesLinq();*/

            // dataModel.TupleOne();
            /*
                        _= Task.Run(async () => {
                            ConcurrentBagExample be = new ConcurrentBagExample();
                            be.Run();
                            await Task.Delay(1000);
                            be.Stop();
                            await Task.Delay(1000);
                            Console.WriteLine(be.message.ToString());
                        });


                        _= Task.Run(() =>
                        {
                            ConcurrentBagExample be1 = new ConcurrentBagExample();
                            int count = 100;
                            while (count-- > 0)
                            {
                                be1.NewObject();
                            }
                        });
                        */

            CmdLineParserExample example = new CmdLineParserExample();
            example.DoArgs(args);
           // example.DoArgs_MoreEnvironment(args);


            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("输入stop则退出222");
            }

        }



    }
}
