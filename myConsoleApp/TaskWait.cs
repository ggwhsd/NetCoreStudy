using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace myConsoleApp
{
    
    class TaskWait
    {
        public static void TestTaskOne()
        {
            //Action是一个委托类型,这里是声明一个Action类型的对象action
            //等号右边是一个语句块的lambda函数
            Action<object> action = (object obj) =>
            {
                Console.WriteLine("Task = {0} obj = {1} threadId = {2}", Task.CurrentId, obj, Thread.CurrentThread.ManagedThreadId);

            };
            action("直接调用已经赋值的委托对象，这个是普通执行方式");

            Task t1 = new Task(action, "通过赋值委托对象Action，new方式创建Task对象");
            Task t2 = Task.Factory.StartNew(action,"通过Task.Factory.StartNew创建并开始一个Task对象，并赋值Action委托对象,");
            t2.Wait(); //这里和直接使用async+await的方式不同，这里是等待任务完成，类似于线程中的join，不过，Task是线程池，不需要单独创建线程，会统一使用一个线程池。
            t1.Start(); //开始运行task
            Console.WriteLine("t1 has been launched. (Main Thread={0})",
                          Thread.CurrentThread.ManagedThreadId);

            t1.Wait();


            Task t3 = Task.Run(() => {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                              Task.CurrentId, "通过Task.Run方式直接调用并开始一个Task",
                               Thread.CurrentThread.ManagedThreadId);
            });

            t3.Wait();

            Task t4 = new Task(action, "同步执行task");
            t4.RunSynchronously();
            t4.Wait();//Although the task was run synchronously, it is a good practice，to wait for it in the event exceptions were thrown by the task.

        }

        public static void TestTaskTwo()
        {
            Task t1 = Task.Run(
                () => { Thread.Sleep(2000); }
                );
            Console.WriteLine("t1 Status: {0}", t1.Status);
            try
            {
                t1.Wait();
                Console.WriteLine("t1 status {0}",t1.Status);
            }
            catch (AggregateException e)
            {
                Console.WriteLine(" {0}", e.StackTrace);
            }
        }

        public static void TestTaskThree_TimeOut()
        {
            Task taska = Task.Run(()=>
            {
                Thread.Sleep(2000);

            });

            try
            {
                taska.Wait(1000);
                bool isComplete = taska.IsCompleted;
                Console.WriteLine("timeout .  status {0}", isComplete.ToString());
            }
            catch (AggregateException e)
            {

            }
        }

        public static void TestTaskAny()
        {
            var tasks = new Task[10];
            var rnd = new Random();
            for (int icr = 0; icr <= 9; icr++)
            {
                tasks[icr] = Task.Run(() => Thread.Sleep(rnd.Next(500, 3500)));

            }
            try {
                int index = Task.WaitAny(tasks);
                Console.WriteLine("Task #{0} completed first.\n", tasks[index].Id);
                Console.WriteLine("Status of all tasks:");
                foreach (var t in tasks)
                    Console.WriteLine("   Task #{0}: {1}", t.Id, t.Status);

            }
            catch (AggregateException)
            {
                Console.WriteLine("An exception occurred.");
            }

            Task.WaitAll(tasks);
            Console.WriteLine("Status of all tasks:");
            foreach (var t in tasks)
                Console.WriteLine("   Task #{0}: {1}", t.Id, t.Status);
        }


        public static void TestTaskCancel()
        {
            // Create a cancellation token and cancel it.
            var source1 = new CancellationTokenSource();
            var token1 = source1.Token;
            source1.Cancel();

            // Create a cancellation token for later cancellation.
            var source2 = new CancellationTokenSource();
            var token2 = source2.Token;

            // Create a series of tasks that will complete, be cancelled, 
            // timeout, or throw an exception.
            Task[] tasks = new Task[12];
            for (int i = 0; i < 12; i++)
            {
                switch (i % 4)
                {
                    // Task should run to completion.
                    case 0:
                        tasks[i] = Task.Run(() => Thread.Sleep(2000));
                        break;
                    // Task should be set to canceled state.
                    case 1:
                        tasks[i] = Task.Run(() => Thread.Sleep(2000),
                                 token1);
                        break;
                    case 2:
                        // Task should throw an exception.
                        tasks[i] = Task.Run(() => { Thread.Sleep(2000); throw new NotSupportedException(); });
                        break;
                    case 3:
                        // Task should examine cancellation token.
                        tasks[i] = Task.Run(() => {
                            Thread.Sleep(2000);
                            if (token2.IsCancellationRequested)
                                token2.ThrowIfCancellationRequested();
                            Thread.Sleep(500);
                        }, token2);
                        break;
                }
            }
            Thread.Sleep(250);
            source2.Cancel();
            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("One or more exceptions occurred:");
                foreach (var ex in ae.InnerExceptions)
                    Console.WriteLine("   {0}: {1}", ex.GetType().Name, ex.Message);
            }

            Console.WriteLine("\nStatus of tasks:");
            foreach (var t in tasks)
            {
                Console.WriteLine("   Task #{0}: {1}", t.Id, t.Status);
                if (t.Exception != null)
                {
                    foreach (var ex in t.Exception.InnerExceptions)
                        Console.WriteLine("      {0}: {1}", ex.GetType().Name,
                                          ex.Message);
                }
            }



        }
    }
}
