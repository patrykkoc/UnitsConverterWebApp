using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnitsConverterWebApp.Models;

namespace UnitsConverterWebApp.Data
{
    public class UnitsConverterWebAppContext : DbContext
    {
        public UnitsConverterWebAppContext (DbContextOptions<UnitsConverterWebAppContext> options)
            : base(options)
        {
        }

      
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Unit> Units { get; set; } = default!;
    }
}
