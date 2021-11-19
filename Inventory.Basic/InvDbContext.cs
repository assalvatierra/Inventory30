using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Entities;

namespace Inventory.Basic
{
    public class InvDbContext: DbContext
    {
        public InvDbContext(DbContextOptions<InvDbContext> options)
            : base(options)
        {
        }

        public DbSet<InvStore> InvStores { get; set; }
        public DbSet<InvItem> InvItems { get; set; }
        public DbSet<InvUom> InvUoms { get; set; }
        public DbSet<InvClassification> InvClassifications { get; set; }
        public DbSet<InvSupplier> InvSuppliers { get; set; }
        public DbSet<InvSupplierItem> InvSupplierItems { get; set; }
        public DbSet<InvPoHdr> InvPoHdrs { get; set; }
        public DbSet<InvPoItem> InvPoItems { get; set; }
        public DbSet<InvPoHdrStatus> InvPoHdrStatus { get; set; }
        public DbSet<InvTrxHdrStatus> InvTrxHdrStatus { get; set; }
        public DbSet<InvTrxHdr> InvTrxHdrs { get; set; }
        public DbSet<InvTrxDtl> InvTrxDtls { get; set; }
        public DbSet<InvTrxType> InvTrxTypes { get; set; }
        public DbSet<InvItemClass> InvItemClasses { get; set; }
        public DbSet<InvTrxDtlOperator> InvTrxDtlOperators { get; set; }
        public DbSet<InvStoreUser> InvStoreUsers { get; set; }

    }




}
