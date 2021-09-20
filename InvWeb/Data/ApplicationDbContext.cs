using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebDBSchema.Models;


namespace InvWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<InvStore> InvStores { get; set; }
        public DbSet<InvItem> InvItems { get; set; }
        public DbSet<InvUom> InvUoms { get; set; }
        public DbSet<InvClassification> InvClassifications { get; set; }
        public DbSet<InvSupplier> InvSuppliers { get; set; }

    }
}
