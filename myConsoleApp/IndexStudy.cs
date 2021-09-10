using System;
using System.Collections.Generic;
using System.Text;
using DateMeasurements = System.Collections.Generic.Dictionary<System.DateTime,int>;
using CityDataMeasurements =
    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<System.DateTime, int>>;

namespace myConsoleApp
{
    class IndexStudy
    {
        

        public class ArgsActions
        {
            readonly private Dictionary<string, Action> argsActions = new Dictionary<string, Action>();

            public Action this[string s]
            {
                get
                {
                    Action action;
                    Action defaultAction = () => { Console.WriteLine("nothing"); };
                    return argsActions.TryGetValue(s, out action) ? action : defaultAction;
                }
            }

            public void SetOption(string s, Action a)
            {
                argsActions[s] = a;
            }
        }

        public class ArgsProcessor
        {
            private readonly ArgsActions actions;

            public ArgsProcessor(ArgsActions actions)
            {
                this.actions = actions;
            }

            public void Process(string[] args)
            {
                foreach (var arg in args)
                {
                    actions[arg]?.Invoke();  //执行arg对应的委托
                }
            }

        }
        
        class DoAction {
            public void PrintAdd()
            {
                Console.WriteLine("PrintAdd");
            }
            public void PrintMod()
            {
                Console.WriteLine("PrintMod");
            }
    }
        public static void  Test()
        {
            //实际上就是一种表驱动编程方法的实践
            Console.WriteLine("【委托的使用，字典的使用，以及[,]多维操作符的自定义】");
            ArgsActions aas = new ArgsActions();
            DoAction da = new DoAction();
            Action a1 = da.PrintAdd;
            Action a2 = da.PrintMod;

            aas.SetOption("add", a1);
            aas.SetOption("mod", a2);
            ArgsProcessor ap = new ArgsProcessor(aas);
            ap.Process(new string[]{ "add","mod"});

           
        }


 //一个出名的分形算法
 //演示循环迭代和数学计算
        public class Mandelbrot
        {
             private readonly int maxIterations;

            public Mandelbrot(int maxIterations)
            {
                this.maxIterations = maxIterations;
            }
            
            public int this[double x, double y]
            {
                get
                {
                    var iterations = 0;
                    var x0 = x;
                    var y0 = y;

                    while ((x * x + y * y < 4) &&
                        (iterations < maxIterations))
                    {
                        var newX = x * x - y * y + x0;
                        y = 2 * x * y + y0;
                        x = newX;
                        iterations++;
                    }
                    return iterations;
                }
            }
        }



        #region 多维方式，示例2


public class HistoricalWeatherData
    {
        readonly CityDataMeasurements storage = new CityDataMeasurements();

        public int this[string city, DateTime date]
        {
            get
            {
                var cityData = default(DateMeasurements);

                if (!storage.TryGetValue(city, out cityData))
                    throw new ArgumentOutOfRangeException(nameof(city), "City not found");

                // strip out any time portion:
                var index = date.Date;
                var measure = default(int);
                if (cityData.TryGetValue(index, out measure))
                    return measure;
                throw new ArgumentOutOfRangeException(nameof(date), "Date not found");
            }
            set
            {
                var cityData = default(DateMeasurements);

                if (!storage.TryGetValue(city, out cityData))
                {
                    cityData = new DateMeasurements();
                    storage.Add(city, cityData);
                }

                // Strip out any time portion:
                var index = date.Date;
                cityData[index] = value;
            }
        }
    }
    #endregion

    public static void TestTwo()
        {
            Console.WriteLine("【Mandelbrot，数学公式，以及m[1,3]这种格式】");
            Mandelbrot m = new Mandelbrot(3);
            Console.WriteLine(m[1, 3].ToString());
            Console.WriteLine(m[0.001, 0.02].ToString());
            Console.WriteLine(m[3, 3].ToString());


        }

        public static void TestThree()
        {
            Console.WriteLine("【字典、抛出异常、hw[\"City\",\"日期\"]】");
            HistoricalWeatherData hw = new HistoricalWeatherData();
            hw["Shanghai", DateTime.Now.AddDays(-1)] = 1;
            hw["Shanghai", DateTime.Now] = 2;
            hw["Beijing", DateTime.Now] = 3;

            Console.WriteLine(hw["Shanghai", DateTime.Now.AddDays(-1)]);
            Console.WriteLine(hw["Shanghai", DateTime.Now]);
            Console.WriteLine(hw["Beijing", DateTime.Now]);

        }
    }
}
