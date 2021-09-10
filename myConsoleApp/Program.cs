﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace myConsoleApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            IndexStudy.Test();

            Lambda lam = new Lambda();
            lam.Init();
            lam.Search(6);
            lam.TestDelegate();
            lam.TestLambda();
            lam.TestLambda2();

            LinqUse lu = new LinqUse();
            lu.TestOne();

            AsyncTask.ConsumeManyTime();
            Console.WriteLine("按任意键继续");
            Console.ReadLine();
            AsyncTask.ContinueTaskWithConsumeManyTime();
            Console.WriteLine("按任意键继续");
            Console.ReadLine();
            AsyncTask.TaskWhenConsumeManyTime();
            Console.WriteLine("按任意键继续");
            Console.ReadLine();
            AsyncTask.TestOne();
            //AsyncTask.TestWithContinue();
            //AsyncTask.Testpallral();




            // TaskWait.TestTaskOne();
            // TaskWait.TestTaskTwo();
            //TaskWait.TestTaskThree_TimeOut();
            //TaskWait.TestTaskAny();
            //TaskWait.TestTaskCancel();
            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("输入stop则退出222");
            }

        }



    }
}
