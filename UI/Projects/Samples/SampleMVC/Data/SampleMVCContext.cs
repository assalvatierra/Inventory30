using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.SysDB;

namespace SampleMVC.Data
{
    public class SampleMVCContext : DbContext
    {
        public SampleMVCContext (DbContextOptions<SampleMVCContext> options)
            : base(options)
        {
        }

        public DbSet<CoreLib.Models.SysDB.EntAddress> EntAddress { get; set; } = default!;
    }
}
