using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.SysDB;

namespace RazorSamples.Data
{
    public class RazorSamplesContext : DbContext
    {
        public RazorSamplesContext (DbContextOptions<RazorSamplesContext> options)
            : base(options)
        {
        }

        public DbSet<CoreLib.Models.SysDB.SysService> SysService { get; set; } = default!;
    }
}
