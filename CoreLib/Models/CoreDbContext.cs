////using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using CoreLib.Inventory.Models;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;



//namespace CoreLib.Data
//{
//    public class CoreDbContext : DbContext
//    {
//        public CoreDbContext(DbContextOptions<CoreDbContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<CoreLib.Inventory.Models.Users.AppUser> AppUsers { get; set; }
//        public DbSet<InvStore> InvStores { get; set; }
//        public DbSet<InvItem> InvItems { get; set; }
//        public DbSet<InvUom> InvUoms { get; set; }
//        public DbSet<InvClassification> InvClassifications { get; set; }
//        public DbSet<InvSupplier> InvSuppliers { get; set; }
//        public DbSet<InvSupplierItem> InvSupplierItems { get; set; }
//        public DbSet<InvPoHdr> InvPoHdrs { get; set; }
//        public DbSet<InvPoItem> InvPoItems { get; set; }
//        public DbSet<InvPoHdrStatus> InvPoHdrStatus { get; set; }
//        public DbSet<InvTrxHdrStatus> InvTrxHdrStatus { get; set; }
//        public DbSet<InvTrxHdr> InvTrxHdrs { get; set; }
//        public DbSet<InvTrxDtl> InvTrxDtls { get; set; }
//        public DbSet<InvTrxType> InvTrxTypes { get; set; }
//        public DbSet<InvItemClass> InvItemClasses { get; set; }
//        public DbSet<InvTrxDtlOperator> InvTrxDtlOperators { get; set; }
//        public DbSet<InvStoreUser> InvStoreUsers { get; set; }
//        public DbSet<InvUomConversion> InvUomConversions { get; set; }
//        public DbSet<InvUomConvItem> InvUomConvItems { get; set; }
//        public DbSet<InvWarningLevel> InvWarningLevels { get; set; }
//        public DbSet<InvWarningType> InvWarningTypes { get; set; }
//        public DbSet<InvCategory> InvCategories { get; set; }
//        public DbSet<InvItemSysDefinedSpecs> InvItemSysDefinedSpecs { get; set; }
//        public DbSet<InvItemSpec_Steel> InvItemSpec_Steel { get; set; }
//        public DbSet<InvCategorySpecDef> InvCategorySpecDefs { get; set; }
//        public DbSet<CoreLib.Inventory.Models.InvCustomSpecType> InvCustomSpecTypes { get; set; }
//        public DbSet<CoreLib.Inventory.Models.InvCustomSpec> InvCustomSpecs { get; set; }
//        public DbSet<CoreLib.Inventory.Models.InvCatCustomSpec> InvCatCustomSpecs { get; set; }
//        public DbSet<CoreLib.Inventory.Models.InvItemCustomSpec> InvItemCustomSpecs { get; set; }

//    }
//}
