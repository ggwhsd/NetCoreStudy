using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

/*
 * 
 * https://www.jianshu.com/p/ac7a1538d849
https://www.cnblogs.com/ZGZzhong/p/12907117.html


Install-Package Microsoft.EntityframeworkCore
Install-Package Microsoft.EntityframeworkCore.Tools
Install-Package MySql.Data.EntityFrameworkCore
#这个没有安装 Install-Package MySql.Data.EntityFrameworkCore.Design

#如下从数据库中生成对应的实体类
Scaffold-DbContext  "server=localhost;port=3306;database=;user=;password=;" MySql.Data.EntityFrameworkCore  -OutputDir Models
#如下通过当前上下文，生成一个数据库迁移脚本，这样可以在空数据库环境中搭建新环境。
dotnet ef migrations add InitDB  -c foxmmContext
#生成对应的sql文件，这样可以按照sql方式更细数据库
dotnet ef migrations script -c foxmmContext -o ".\\testCreateNewDatabase.sql"
#直接根据context来更新数据库
dotnet ef database update -c TicketPlatformContext


 */
namespace EFCoreDbFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Models.foxmmContext())
            {
                db.Database.EnsureCreated();
                db.Emp.Add(new Models.Emp { FirstName ="ttt",LastName="ff",Birthdate= DateTime.Now});
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All emps in database:");
                foreach (var emp in db.Emp)
                {
                    Console.WriteLine(" - {0} {1} {2} {3}",emp.Empno,emp.FirstName,emp.LastName, emp.Birthdate);
                }
                EntityState stat;
               var delEmp= db.Emp.Where(p => p.FirstName.Equals("ttt")).First();
                if (delEmp != null)
                {
                    stat = db.Entry(delEmp).State;
                    Console.WriteLine(stat.ToString());
                }
                db.Emp.Remove(delEmp);
                count = db.SaveChanges();
                Console.WriteLine("{0} records deleted in database", count);

                Console.WriteLine("All emps in database:");
                foreach (var emp in db.Emp)
                {
                    Console.WriteLine(" - {0} {1} {2} {3}", emp.Empno, emp.FirstName, emp.LastName, emp.Birthdate);
                }
                stat = db.Entry(delEmp).State;
                Console.WriteLine(stat.ToString());
            }

            Console.ReadLine();
        }
    }
}
