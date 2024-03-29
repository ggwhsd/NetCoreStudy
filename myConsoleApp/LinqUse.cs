﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace myConsoleApp
{
    public static class Extensions
    {
        /// <summary>
        /// 扩展linq语法的自定义函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        //依次返回两个迭代器的值
        public static IEnumerable<T> InterleaveSequenceWith<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();
            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                yield return firstIter.Current;
                yield return secondIter.Current;
            }
        }

        //比较两个迭代器中的值是否相等
        public static bool SequenceEquals<T>
    (this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstIter = first.GetEnumerator();
            var secondIter = second.GetEnumerator();

            while (firstIter.MoveNext() && secondIter.MoveNext())
            {
                if (!firstIter.Current.Equals(secondIter.Current))
                {
                    return false;
                }
            }

            return true;
        }
        //日志
        public static IEnumerable<T> LogQuery<T> (this IEnumerable<T> sequence, string tag)
        {
            if (sequence is null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }
            // File.AppendText creates a new file if the file doesn't exist.
            using (var writer = File.AppendText("debug.log"))
            {
                writer.WriteLine($"Executing Query {tag}");
            }

            return sequence;
        }
    }
    class LinqUse
    {
        static IEnumerable<string> Suits()
        {
            yield return "梅花";
            yield return "方块";
            yield return "红桃";
            yield return "黑桃";


        }
        static IEnumerable<string> Ranks()
        {
            yield return "2";
            yield return "3";
            yield return "4";
            yield return "5";
            yield return "6";
            yield return "7";
            yield return "8";
            yield return "9";
            yield return "10";
            yield return "J";
            yield return "Q";
            yield return "K";
            yield return "A";
        }

        
        public void TestOne()
        {
            Console.WriteLine("【Linq的使用，ToArray可以减少重复无效开销】");
            long size = GC.GetTotalMemory(false);
            //使用toArray可以缓存结果，否则每次都会重新generation扑克牌的序列。

            //var startingDeck = (from s in Suits().LogQuery("Suit Generation")
            //                   from r in Ranks().LogQuery("Rank Generation")
            //                   select new { Suit = s, Rank = r }).LogQuery("Starting Deck");
            //从Suits中获取每个元素，赋值给s，从Ranks中获取每个元素赋值给r，创建新的结果对象属性Suit，Rank。并将迭代结果s和r赋值给Suit、Rank。
            //两个from，没有其他条件，则是可以看作两层循环遍历
            //结果为匿名类型的对象
            var startingDeck = (from s in Suits().LogQuery("Suit Generation")
                                from r in Ranks().LogQuery("Rank Generation")
                                select new { Suit = s, Rank = r }).LogQuery("Starting Deck").ToArray();
            
            size = GC.GetTotalMemory(false);
            // Display each card that we've generated and placed in startingDeck in the console
            foreach (var card in startingDeck)
            {
                Console.WriteLine(card.Suit + card.Rank);
            }

            // 52 cards in a deck, so 52 / 2 = 26
            //一副扑克拆成两半，互相插入的洗牌方式，即原先第1张之后插入第26张，原先第2张之后插入第27张
            //提取前26个元素
            var top = startingDeck.Take(26).LogQuery("top Half");
            //跳过26个元素，返回第27个元素的迭代器
            var bottom = startingDeck.Skip(26).LogQuery("Bottom Half");
            Console.WriteLine("第1次洗牌=================>");
            //var shuffle = top.InterleaveSequenceWith(bottom).LogQuery("Shuffle");
           var shuffle = top.InterleaveSequenceWith(bottom).LogQuery("Shuffle").ToArray();
            foreach (var c in shuffle)
            {
                Console.WriteLine(c.Suit + c.Rank);
            }
            
            int i = 2;

            while (i <= 8)
            {
                Console.WriteLine($"第{i}次洗牌=================>");
                //一副扑克拆成两半，互相插入的洗牌方式，即原先第1张之后插入第26张，原先第2张之后插入第27张
                //shuffle = shuffle.Take(26).LogQuery("top Half").InterleaveSequenceWith(shuffle.Skip(26).LogQuery("Bottom Half")).LogQuery("Shuffle");
                shuffle = shuffle.Take(26).LogQuery("top Half").InterleaveSequenceWith(shuffle.Skip(26).LogQuery("Bottom Half")).LogQuery("Shuffle").ToArray();
                foreach (var c in shuffle)
                {
                    Console.WriteLine(c.Suit + c.Rank);
                }
                i++;
            }

            if (shuffle.SequenceEquals(startingDeck) == true)
            {
                Console.WriteLine("洗完之后的牌和之前相同");
            }

        }
    }
}
