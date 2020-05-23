using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDbFirst
{
    class AppDbContext: DbContext
    {
        public AppDbContext()
        {

        }
        public DbSet<Blog> Blogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //SslModel=None 这和ssl协议有关系。如果不指定会报错
            optionsBuilder.UseMySQL("server=localhost;port=3306;database=foxmm;user=root;password=gugw12121;SslMode=None");
        }
    }
}
