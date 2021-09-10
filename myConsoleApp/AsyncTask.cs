using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace myConsoleApp
{
    class AsyncTask
    {
        static int i = 0;
        //模拟耗时的运算或者IO
        public static bool GetSomeThing(string a)
        {
            Thread.Sleep((new Random()).Next() % 3 * 1000);
          
            i++;
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " " + a+ "【5-】 <" + Thread.CurrentThread.ManagedThreadId.ToString() + ">");
            return true;
        }
        //生成一个task类型的方法，其中包装调用GetSomeThing
        public static Task<bool> GetSomeThingAsync(string a)
        {
            return Task.Run<bool>(() => { return GetSomeThing(a); }
            );
        }






        //方式1：包装给调用方使用的方法，使用async修饰，因为里面有await。 调用方执行到await会立即返回，后续的内容不再直接调用， await之后的方法都会在可能在其他线程中使用，
        public async static void ConsumeManyTime()
        {
            Console.WriteLine("【简单的异步调用Task方法】");
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【3-】 <" + Thread.CurrentThread.ManagedThreadId.ToString()+">");
            bool result = await GetSomeThingAsync("test");
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【4-】 <" + Thread.CurrentThread.ManagedThreadId.ToString()+">");
        }
        //方式2：这种调用方式是在异步执行完毕后，可以继续执行预定的后续函数，可以理解为一种回调机制。
        public static void ContinueTaskWithConsumeManyTime()
        {
            Console.WriteLine("【在异步t1执行后继续执行后续任务，t1.ContinueWith】");
            Task<bool> t1 = GetSomeThingAsync("test");
            t1.ContinueWith(t => { Console.WriteLine("after await finished " + t.Result + " <" + Thread.CurrentThread.ManagedThreadId.ToString() + "> " + i.ToString()); });
        }
        //方式3：多任务等待方式
        public static async void TaskWhenConsumeManyTime()
        {
            Console.WriteLine("【在异步t1和t2都执行后继续执行后续任务，Task.WhenAll(t1,t2)】");
            Task<bool> t1 = GetSomeThingAsync("task1");
            Task<bool> t2 = GetSomeThingAsync("task2");
            await Task.WhenAll(t1, t2);
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【6-】<" + Thread.CurrentThread.ManagedThreadId.ToString()+">");

        }

        ///async 和await的基本使用
        public static void TestOne()
        {
            Console.WriteLine("【async 和await的基本使用】");
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【1-】<" + Thread.CurrentThread.ManagedThreadId.ToString()+">");
            ConsumeManyTime();
            ConsumeManyTime();
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【2-】<" + Thread.CurrentThread.ManagedThreadId.ToString()+">");
            Console.WriteLine("继续输入");
            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("继续输入");
            }
        }

        public static void TestWithContinue()
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【1-】<" + Thread.CurrentThread.ManagedThreadId.ToString() + ">");
            ContinueTaskWithConsumeManyTime();
            ContinueTaskWithConsumeManyTime();
            ContinueTaskWithConsumeManyTime();
            ContinueTaskWithConsumeManyTime();
            ContinueTaskWithConsumeManyTime();
            ContinueTaskWithConsumeManyTime();
            ContinueTaskWithConsumeManyTime();
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【2-】<" + Thread.CurrentThread.ManagedThreadId.ToString()+">");
            Console.WriteLine("继续输入");
            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("继续输入");
            }
        }

        public static void TestWhen()
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【1-】<" + Thread.CurrentThread.ManagedThreadId.ToString()+">");
            TaskWhenConsumeManyTime();

            Console.WriteLine(DateTime.Now.ToLongTimeString() + "【2-】<" + Thread.CurrentThread.ManagedThreadId.ToString()+">");
            Console.WriteLine("继续输入");
            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("继续输入");
            }
        }


        public static void Testpallral()
        {
            ParallelLoopResult result =

                  Parallel.For(0, 100, async (int i) =>

                  {

                      Console.WriteLine(DateTime.Now.ToLongTimeString() + "{0}, task: {1}, thread: {2}", i,

                      Task.CurrentId, Thread.CurrentThread.ManagedThreadId);

                      await Task.Delay(10);
                      Console.WriteLine(DateTime.Now.ToLongTimeString() + "after delay");

                  });

            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("继续输入");
            }
        }

        public static void Testpallral2()
        {
            ParallelLoopResult result =
                  Parallel.For(0, 100, async (int i, ParallelLoopState pls) =>
                  {
                      Console.WriteLine(DateTime.Now.ToLongTimeString() + "{0}, task: {1}, thread: {2}", i,
                      Task.CurrentId, Thread.CurrentThread.ManagedThreadId);

                      await Task.Delay(10);
                      if (i > 5) pls.Break();
                      Console.WriteLine(DateTime.Now.ToLongTimeString() + "after delay");

                  }
            );

            while (Console.ReadLine() != "stop")
            {
                Console.WriteLine("继续输入");
            }
        }
    }
}
