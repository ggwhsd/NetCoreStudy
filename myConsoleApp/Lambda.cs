using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myConsoleApp
{
    class Lambda
    {
        class Item
        {
            public int NO { get; set; }
            public Item(int no)
            {
                this.NO = no;
            }
        }

        List<Item> items;

        public void Init()
        {
            items = new List<Item>();

            int i = 0;
            while (i < 10)
            {
                items.Add(new Item(i));
                i++;
            }

        }

        public void Search(int no)
        {
            Console.WriteLine("【Linq的基础where使用】");
            //linq表达式语句，where作为条件
            IEnumerable<Item> results = items.Where( (Item x)=> (x.NO < no) );
            foreach (var i in results)
            {
                Console.WriteLine($"item {i.NO}");
            }
        }

        public void TestDelegate()
        {
            Console.WriteLine("【委托类型Predicate的使用示例】");
            Predicate<Item> p = new Predicate<Item>( (x) => x.NO == 1);
            IEnumerable<Item> results = items.FindAll(p);
            foreach (var i in results)
            {
                Console.WriteLine($"item {i.NO}");
            }
        }
        public void TestLambda()
        {
            Console.WriteLine("【lambda的使用】");
            IEnumerable<Item> results = items.FindAll((Item x) => x.NO == 1);
            foreach (var i in results)
            {
                Console.WriteLine($"item {i.NO}");
            }
        }
        //表达式lambda
        public void TestLambda2()
        {
            Console.WriteLine("【委托类型Action、Fun结合lambda的使用】");
            Func<int, int> square = x => x * x;
            Console.WriteLine($"square 3 = { square(3)}");

            Action line = () => Console.WriteLine("   line null ");

            Action<int> input = (int x) => Console.WriteLine(x);

            Action<int,int> input2 = (int x,int y) => Console.WriteLine(x+y);

            Func<int, int, bool> testForEquality = (x, y) => x == y;
        }
        //语句lambda
        public void TestLambda3()
        {
            Console.WriteLine("【委托类型Action结合lambda的使用,lambda体为多行代码】");
            Action<string> greet = name =>
            {
                string greeting = $"Hello {name}!";
                Console.WriteLine(greeting);
            };


        }
    }
}
