using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MvcMoview.Models
{
    public class MvcMoviewContext : DbContext
    {
        public MvcMoviewContext (DbContextOptions<MvcMoviewContext> options)
            : base(options)
        {
        }

        public DbSet<MvcMoview.Models.Movie> Movie { get; set; }
    }
}
