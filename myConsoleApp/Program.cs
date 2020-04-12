using System;
using System.Threading;
using System.Threading.Tasks;

namespace myConsoleApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            /*
              IndexStudy.Test();
              IndexStudy.TestTwo();
              IndexStudy.TestThree();
              */
            Lambda lam = new Lambda();
            lam.Init();
            lam.Search(6);
            lam.TestDelegate();
            lam.TestLambda();

            LinqUse lu = new LinqUse();
            lu.TestOne();

            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("继续输入");
            }

        }



    }
}
