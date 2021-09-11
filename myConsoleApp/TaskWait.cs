using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace myConsoleApp
{
    
    class TaskWait
    {
        /// <summary>
        /// Task创建方式1：new Task(Action,args) ,创建任务但不启动，需要调用Start才能启动异步任务，调用RunSynchronously则启动同步任务
        /// Task创建方式2：Task.Factory.StartNew(Action,args) ,创建任务并且启动
        /// Task创建方式3： Task.Run(Action),创建任务并且启动，这个方法不能提供委托Action的参数，所以采用lambda方式也很好
        /// 
        /// Task.Wait() 表示当前线程中阻塞等待异步的Task任务完成
        /// Task.Start() 表示启动异步任务
        /// Task.RunSynchronously() 使用当前线程执行Task
        /// </summary>
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
            Console.WriteLine("t1.Id=" + t1.Id);
            Task t2 = Task.Factory.StartNew(action,"通过Task.Factory.StartNew创建并 开始 一个Task对象，并赋值Action委托对象,");
            Console.WriteLine("t2.Id=" + t2.Id);
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
        /// <summary>
        /// 针对Task的异常捕获
        /// </summary>
        public static void TestTaskTwo()
        {
            Console.WriteLine("【针对Task的异常捕获】");
            Task t1 = Task.Run(
                () => { 
                    Thread.Sleep(2000);
                    int j = 0;
                    int i = 100 / j;
                }
                );
            Console.WriteLine("t1 Status: {0}", t1.Status);
            try
            {
                t1.Wait();
                Console.WriteLine("t1 status {0}",t1.Status);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("t1 status {0}", t1.Status);
                Console.WriteLine(" {0}", e.StackTrace);
            }
        }
        /// <summary>
        /// 定时等待Task.Wait(1000);
        /// </summary>
        public static void TestTaskThree_TimeOut()
        {
            Console.WriteLine("【定时等待Task.Wait(1000)】");
            Task taska = Task.Run(()=>
            {
                Thread.Sleep(2000);
              
            });

            try
            {
                bool isComplete = taska.IsCompleted;
                while (isComplete == false) { 
                    taska.Wait(1000);
                    isComplete = taska.IsCompleted;
                    Console.WriteLine("timeout .  status {0}", isComplete.ToString());
                }
            }
            catch (AggregateException e)
            {

            }

        }
        /// <summary>
        /// Task.WaitAny(Task[]) 只要有一个任务完成，就会返回对应任务所在Task数组中的序号
        /// 
        /// </summary>
        public static void TestTaskAny()
        {
            Console.WriteLine("【Task.WaitAny(Task[]) 只要有一个任务完成，就会返回对应任务所在Task数组中的序号】");
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

        /// <summary>
        ///  对于一组任务的执行，并取消其中部分任务，通过task.Exception.InnerExceptions获取任务异常
        ///  在waitall时，如果出现异常，可以通过AggregateException来获取所有出现的异常。
        ///  
        /// CancellationTokenSource的创建和立即执行Cancel，后续如果简历任务和这个有关，则相当于任务初始状态为Cancel，TaskScheduler就不会调用该任务了。
       ///   Cancel实际上只是修改了token状态，没有做任何其他操作，Task在调度时会检查这个token状态，但是一旦调度task运行了，则会执行task中的函数，除非函数中自己有对token状态的检查，否则任务状态就一直是Running
        /// </summary>
        public static void TestTaskCancel()
        {
            Console.WriteLine("【对Task进行Cancel控制】");

            Action action = () =>
            {
                Thread.Sleep(2000);
              

            };
            Action actionException = () =>
            {
                Thread.Sleep(2000);
                throw new NotSupportedException();
                
                 
            };

            
            var source1 = new CancellationTokenSource();
            var token1 = source1.Token;
            source1.Cancel();

            //创建一个CancellationTokenSource，并将其Token交给后续的Task任务，这样就可以操作Cancel任务了，
            //但是这个只是一个标志，真正Cancel状态。
            var source2 = new CancellationTokenSource();
            var token2 = source2.Token;

            // 创建一些任务
            Task[] tasks = new Task[12];
            for (int i = 0; i < 12; i++)
            {
                switch (i % 4)
                {
                    //这个任务可以顺利执行完成
                    case 0:
                        tasks[i] = Task.Run(action);
                        break;
                    //会受token1控制
                    case 1:
                        tasks[i] = Task.Run(action,
                                 token1);
                        break;
                    case 2:
                        //会抛出异常
                        tasks[i] = Task.Run(actionException);
                        break;
                    case 3:
                        //任务中，会检查token2的状态，一旦任务开始运行running状态
                        tasks[i] = Task.Run(() => {
                            Thread.Sleep(2000);
                            Console.WriteLine("Task = {0} threadId = {1}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
                            if (token2.IsCancellationRequested)
                                token2.ThrowIfCancellationRequested();
                            Thread.Sleep(500);
                        }, token2);
                        break;
                }
            }
            
            var point =Console.GetCursorPosition();

            for (int i = 0; i < 12; i++)
            {
                //立即查看任务状态，可以看到token1相关的四个任务状态变为为Cancel，
                //此处看到任务为Cancel，不表示任务初始状态为Cancel，而是调度结果是Cancel。这是表示taskscheduler检查其token，发现token的cancel为true，因此将任务状态从WaitToRan也调度为Cancel
                
                Console.WriteLine($"Task #{tasks[i].Id} {tasks[i].Status}");
            }
            Thread.Sleep(250);
            //当执行这行代码时，source2相关的三个任务，其中有两个已经时Running状态，一个时WaitToRun
            //根据观察，task[3] task[7]运行直到检查token2.IsCancellationRequested主动抛出异常,task[12]并未运行。
            source2.Cancel();

            for (int i = 0; i < 12; i++)
            {
                Console.SetCursorPosition(point.Left + 40, point.Top+i);
                Console.WriteLine($"Task #{tasks[i].Id} {tasks[i].Status}");
            }
            Thread.Sleep(2000);
            
            for (int i = 0; i < 12; i++)
            {
                Console.SetCursorPosition(point.Left + 80, point.Top+i);
                Console.WriteLine($"Task #{tasks[i].Id} {tasks[i].Status}");
            }

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
