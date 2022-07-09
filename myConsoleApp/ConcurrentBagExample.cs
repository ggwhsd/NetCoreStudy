using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    /// <summary>
    /// 微软在Microsoft.Extensions.ObjectPool中已有这个对象。这里通过例子来看看自定义实现的方式.
    /// ConcurrentBag.add是添加元素
    /// ConcurrentBag.TryTake是返回元素并将其删除
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T>
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _objects = new ConcurrentBag<T>();
        }

        public T Get() => _objects.TryTake(out T item) ? item : _objectGenerator();

        public void Return(T item) => _objects.Add(item);
    }
    /// <summary>
    /// 大概4M内存空间
    /// </summary>
    class ExampleObject
    {
        public int[] Nums { get; set; }

        public ExampleObject()
        {
            Nums = new int[1000000];
            var rand = new Random();
            for (int i = 0; i < Nums.Length; i++)
            {
                Nums[i] = rand.Next();
            }
        }

        public double GetValue(long i) => Math.Sqrt(Nums[i]);
    }
    class ConcurrentBagExample
    {
        CancellationTokenSource cts;
        ParallelLoopResult result;
        ObjectPool<ExampleObject> pool = new ObjectPool<ExampleObject>(() => new ExampleObject());

        /// <summary>
        /// 这个方法只会创建两个对象，后续都是复用两个
        /// </summary>
        public void NewObject()
        {
            
            int count = 10000;
            while (count > 0)
            {
                count--;
                var example1=pool.Get();
                var example2 = pool.Get();
                pool.Return(example1);
                pool.Return(example2);
            }
            count = 0;
        }
        public void Run()
        {
            cts = new CancellationTokenSource();
            message.Clear();
            _ = Task.Run(async () =>
            {
                while (cts.IsCancellationRequested == false)
                {
                    await Task.Delay(1000);
                    message.Append("waiting ");
                }
                
            }
            );

            if(pool==null)
                pool = new ObjectPool<ExampleObject>(() => new ExampleObject());

            result =Parallel.For(0, 10000000, new ParallelOptions() { MaxDegreeOfParallelism = 4}, (i, loopState) =>
             {
                 var example = pool.Get();
                 try
                 {
                     message.Append(" " + example.GetValue(i% 1000000));
                 }
                 finally
                 {
                     pool.Return(example);
                 }
                 if (cts.Token.IsCancellationRequested)
                 {
                     loopState.Stop();
                 }
                 
             });
            
            Task.Run(async () => { while (true)
                {
                    if (result.IsCompleted)
                    {
                        isCompleted = true;
                        break;
                    }
                    await Task.Delay(50);
                }
            });
        }

        public StringBuilder message = new StringBuilder();

        public bool isCompleted;

        public void Stop()
        {
            cts.Cancel();
            
        }
    }
}
