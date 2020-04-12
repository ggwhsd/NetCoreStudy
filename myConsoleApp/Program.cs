using System;
using System.Threading;
using System.Threading.Tasks;

namespace myConsoleApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //AsyncProgram.TestOne();
            //AsyncProgram.TestWithContinue();
            //AsyncProgram.Testpallral();
            /*
             Lambda lam = new Lambda();
             lam.Init();
             lam.Search(6);
             lam.TestDelegate();
             lam.TestLambda();

             LinqUse lu = new LinqUse();
             lu.TestOne();
             */

            // TaskWait.TestTaskTwo();
            //TaskWait.TestTaskThree_TimeOut();
            //TaskWait.TestTaskAny();
            TaskWait.TestTaskCancel();
            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("继续输入");
            }

        }



    }
}
