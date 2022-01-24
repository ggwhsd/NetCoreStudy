using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class DataModelStudy
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

        /// <summary>
        /// anonymous types 是一种快捷的使用类对象的方式，就如同lambda函数一样，无需声明类类型，直接由编译器生成，对一些只读数据类型的对象进行包装定义。
        /// 创建方式跟类对象一样，也是需要使用new，只是后面无需类名。
        /// 匿名对象的ToString方法默认行为也是输出类似 Key-Value样式的数据，跟record类似。
        /// </summary>
        public void anonymousTypesOne()
        {
            Console.WriteLine("创建一个匿名对象，包含两个成员:");
            var anonymousObject = new { Name = "One", Age = 12 };
            Console.WriteLine($"匿名对象的Name {anonymousObject.Name} Age:{anonymousObject.Age}");
            // var anonymousObject2 = anonymousObject with { Name = "Two" };  // C# 10才支持with 匿名， 9只支持with record
            Console.WriteLine(anonymousObject);


            Console.WriteLine("创建一个由匿名对象组成的匿名数组:");
            var array = new[] { new { Name = "One", Age = 12 }, new { Name = "Two", Age = 13 } };
            foreach (var a in array)
            {
                Console.WriteLine(a);
            }
        }

        /// <summary>
        /// 匿名对象的一个常用场所就是Linq，因为Linq结果很多时候都是根据应用选择不同的字段，而且多数时候都只读数据，所以采用匿名类进行操作会方便很多。
        /// 
        /// </summary>
        public void anonymousTypesLinq()
        {

           
            List<Person> persons = new List<Person>();
            int i = 100;
            while (i > 0)
            {
                persons.Add(new Person("鲁班 " + i.ToString() + " 号", i) { city = "城市 " + i.ToString()+" 号"});
                i--;
            }

            //使用匿名对象进行操作，免去了重新创建的过程.
            //只从person对象中取出两个字段，而不是所有字段。
            var queryResults = from p in persons
                               where (p.age < 10 && p.age>3)
                                   //select new { NameNew = p.name, AgeNew = p.age };  //可以创建新的属性名
                               select new { p.name, p.age } ;  //使用默认属性名，
            
            foreach (var v in queryResults)
            {
                Console.WriteLine(v);
            }

            persons.Add(new Person("鲁班 5 号 插班生", 5));
            //linq语法是定义不会执行，每次调用时才会去执行查询，所以这里还会继续重新查询，而不是用上次的结果。
            foreach (var v in queryResults)
            {
                Console.WriteLine(v);
            }

        }
        /// <summary>
        /// 元组,一种方便快捷的数据类型，无需声明结构，即可立即使用。
        /// 元组中的属性都是可以修改值的，并不是只读，这点和其他某类编程语言不大一样。
        /// </summary>
        public void TupleOne()
        {
            Random rnd = new Random();
            //第一种场景：直接赋值
            var results = (1, 2, 3, "Name","Time");  // 等价于 var ( Item1,  Item2,  Item3,  Item4,  Item5)
            Console.WriteLine(results);
            Console.WriteLine(results.Item1 + "," + results.Item2 + "," + results.Item3);

            //第二种场景：函数返回元组
            results = GetTupleDataOne(rnd);
            results.Item2 = 3;
            Console.WriteLine(results);

            //第三种场景：逐个字段命名
            (int i1, int i2, int i3, string name, string time) = GetTupleDataOne(rnd);
            Console.WriteLine($"{i1} {i2} {i3} {name} {time}");
            i1 = i2 = i3 = 10;
            //第四种场景：已有变量
            string Name, Time;
            (i1, i2, i3, Name, Time) = GetTupleDataOne(rnd);
            Console.WriteLine($"{i1} {i2} {i3} {name} {Time}");

            //第五种场景：遇到弃元，虽然元组返回多个数据，但是我只想要其中某两个的时候，可以使用这种。
            (_,_,_,Name, Time) = GetTupleDataOne(rnd);
            Console.WriteLine($"{i1} {i2} {i3} {name} {Time}");

            //第六种场景：对于类对象，也可以方便的获取其成员变成元组数据，这里使用Deconstruct方法。这样后续使用该功能的时候，就不用主动明显的调用方法。
            var p = new PersonEx("A1", "B1", "C1", "D1", "G1");

            // 调用Deconstruct方法，快速获取p的数据，并赋值到元组上。
            var (fName, lName, city, state) = p;
            var (fName1, lName1) = p;
            Console.WriteLine($" {fName} {lName}  {city} {state}");


            //第七种场景：可以声明Deconstruct为静态类的静态函数，第一个参数为需要处理的对象类型。以下功能依赖于ReflectionExtensions的Deconstruct方法。
            Type dateType = typeof(DateTime);
            PropertyInfo prop = dateType.GetProperty("Now");
            var (isStatic, isRO, isIndexed, propType) = prop;
            Console.WriteLine($"\nThe {dateType.FullName}.{prop.Name} property:");
            Console.WriteLine($"   PropertyType: {propType.Name}");
            Console.WriteLine($"   Static:       {isStatic}");
            Console.WriteLine($"   Read-only:    {isRO}");
            Console.WriteLine($"   Indexed:      {isIndexed}");
            Type listType = typeof(List<>);
            prop = listType.GetProperty("Item",BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            var (hasGetAndSet, sameAccess, accessibility, getAccessibility, setAccessibility) = prop;
            Console.Write($"\nAccessibility of the {listType.FullName}.{prop.Name} property: ");

            if (!hasGetAndSet | sameAccess)
            {
                Console.WriteLine(accessibility);
            }
            else
            {
                Console.WriteLine($"\n   The get accessor: {getAccessibility}");
                Console.WriteLine($"   The set accessor: {setAccessibility}");
            }

        }

        private (int, int, int, string, string) GetTupleDataOne(Random rnd)
        {
            return (1, 1, 1, "One", (rnd).Next(1, 1000).ToString());
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


    /// <summary>
    /// 微软网站上的示例
    /// </summary>
    public static class ReflectionExtensions
    {
        public static void Deconstruct(this PropertyInfo p, out bool isStatic,
                                   out bool isReadOnly, out bool isIndexed,
                                   out Type propertyType)
        {
            var getter = p.GetMethod;

            // Is the property read-only?
            isReadOnly = !p.CanWrite;

            // Is the property instance or static?
            isStatic = getter.IsStatic;

            // Is the property indexed?
            isIndexed = p.GetIndexParameters().Length > 0;

            // Get the property type.
            propertyType = p.PropertyType;
        }

        public static void Deconstruct(this PropertyInfo p, out bool hasGetAndSet,
                                   out bool sameAccess, out string access,
                                   out string getAccess, out string setAccess)
        {
            hasGetAndSet = sameAccess = false;
            string getAccessTemp = null;
            string setAccessTemp = null;

            MethodInfo getter = null;
            if (p.CanRead)
                getter = p.GetMethod;

            MethodInfo setter = null;
            if (p.CanWrite)
                setter = p.SetMethod;

            if (setter != null && getter != null)
                hasGetAndSet = true;

            if (getter != null)
            {
                if (getter.IsPublic)
                    getAccessTemp = "public";
                else if (getter.IsPrivate)
                    getAccessTemp = "private";
                else if (getter.IsAssembly)
                    getAccessTemp = "internal";
                else if (getter.IsFamily)
                    getAccessTemp = "protected";
                else if (getter.IsFamilyOrAssembly)
                    getAccessTemp = "protected internal";
            }

            if (setter != null)
            {
                if (setter.IsPublic)
                    setAccessTemp = "public";
                else if (setter.IsPrivate)
                    setAccessTemp = "private";
                else if (setter.IsAssembly)
                    setAccessTemp = "internal";
                else if (setter.IsFamily)
                    setAccessTemp = "protected";
                else if (setter.IsFamilyOrAssembly)
                    setAccessTemp = "protected internal";
            }

            // Are the accessibility of the getter and setter the same?
            if (setAccessTemp == getAccessTemp)
            {
                sameAccess = true;
                access = getAccessTemp;
                getAccess = setAccess = String.Empty;
            }
            else
            {
                access = null;
                getAccess = getAccessTemp;
                setAccess = setAccessTemp;
            }
        }
    }

    class PersonEx
    {
        public string N1 { get; set; }
        public string N2 { get; set; }
        public string N3 { get; set; }
        public string C1 { get; set; }
        public string S1 { get; set; }
        public PersonEx(string n1, string n2, string n3,
                  string c1, string s1)
        {
            N1 = n1;
            N2 = n2;
            N3 = n3;
            C1 = c1;
            S1 = s1;
        }
        // Return the first and last name.
        public void Deconstruct(out string n1, out string n3)
        {
            n1 = N1;
            n3 = N3;
        }
        public void Deconstruct(out string n1, out string n2, out string n3)
        {
            n1 = N1;
            n2 = N2;
            n3 = N3;
        }
        public void Deconstruct(out string n1, out string n3,
                          out string c1, out string s1)
        {
            n1 = N1;
            n3 = N3;
            c1 = C1;
            s1 = S1;
        }

    }

}
