using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myConsoleApp
{




    /// <summary>
    /// 主要用于示例几种数据类型: class \ record \ struct
    /// class 是主要类型，类，可以继承，可以多态。
    /// record 是c# 10才有的类型，一般用于用于不可更改的数据使用场所，多用于方便进行值相等，使用Equals或者==会比较里面每个属性都相等，则认为相等，这是与class的主要差别。
    /// struct 是一个简单的数据类型，用于存储数据，可修改数据，不能继承。
    /// </summary>
    class DataModelStudy
    {
        public void createRecord()
        {
            Person p = new("One", 35);
            //record的ToString方法默认是输出 key=value
            Console.WriteLine(p);
        }

        public void equalRecords()
        {
            Person p = new("One", 35);
            Person p2 = new("One", 35);
            Console.WriteLine("使用==的结果是:" + (p == p2));
            Console.WriteLine("使用Equals的结果是:" + (p.Equals(p2)));
            Console.WriteLine("使用RefrenceEquals的结果是：" + (ReferenceEquals(p, p2)));
        }

        /// <summary>
        /// with可以实现复制一个已有的record对象的数据并修改with之后指定的值，将其生成一个新的record.
        /// with是一个浅复制
        /// </summary>
        public void withRecord()
        {
            Person p = new("One", 35) { schools = new string[2] };
            Person p2 = p with { age = 20 };
            p.schools[0] = "123";
            p.city = "city";
            Console.WriteLine(p);
            Console.WriteLine(p2);
        }
    }


    /// <summary>
    /// 定义一个record类型，看起来像类的构造函数声明,这种圆括号的比较简单，如果数据多了，也是可以用{}这种方式的。
    /// 圆括号里面的是必须参数，大括号里面的是非必须。
    /// </summary>
    public record Person(string name, int age)
    {
        /// <summary>
        /// 设置了 set，所以可以后续修改。
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 属于引用类型，所以是浅复制，复制的只是引用地址，并没有创建新的内存对象。所以，虽然是get和init，表示初始化时需要赋值，但是schools[0]里面的元素时可以修改的。
        /// </summary>
        public string[] schools { get; init; }
    }


}
