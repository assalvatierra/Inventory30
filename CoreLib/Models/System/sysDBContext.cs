using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobsV1.Models;

namespace CoreLib.Models.System
{
    public class SysDBContext : DbContext
    {
        public SysDBContext(DbContextOptions<SysDBContext> options)
            : base(options)
        {
        }

        public DbSet<SysService> SysServices { get; set; } = default!;
        //public DbSet<City> Cities { get; set; } = default!;
        //public DbSet<Country> Countries { get; set; } = default!;
        //public DbSet<SupplierType> SupplierTypes { get; set; } = default!;
    }
}
