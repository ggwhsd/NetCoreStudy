# NetCoreStudy
learn programming in netcore 5 platform.

https://docs.microsoft.com/zh-cn/aspnet/core/?view=aspnetcore-5.0

## 第一个控制台项目 

dotnet new console --output myConsoleApp
dotnet run --project myConsoleApp
然后使用visual studio code打开这个项目。

* myConsoleApp/IndexStudy.cs 

  展示了字典的使用方法，[,]操作符的自定义和使用，还有一个分形算法的示例。

* myConsoleApp/Lambda.cs 

  lambda的使用，以及Action、Func、Predicate等委托类型的使用。

* myConsoleApp/LinqUse.cs 

  展示了使用Linq进行洗牌的应用，与用循环代码生成洗牌的方法相比，Linq快捷简化了代码。

* myConsoleApp/AsyncTask.cs 

  展示了异步任务的await、async、ContinueWith、Task.WhenAll、并行方法Parallel等方法的使用。异步任务模式，其实现中也用到了线程池，但是比直接操作线程Thread方式更容易使用也更高效开发。

* myConsoleApp/TaskWait.cs 

  重点对Task这个类进行的演示，Task的几种创建方式，Task可以获取其ID，也可以给控制任务顺序，Task也是可以当作同步使用的。
同时对task的异常捕获，cancel操作等做了示例。

## 第一个web项目 

## 串口项目
* SerialConsole

## 本地调用
https://docs.microsoft.com/zh-cn/dotnet/standard/native-interop/pinvoke
