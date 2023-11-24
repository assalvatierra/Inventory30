//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CoreLib.Inventory.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RealSys.CoreLib.Models.Reports;

namespace CoreLib.Models.Inventory
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CoreLib.Inventory.Models.Users.AppUser>? AppUsers { get; set; }
        public DbSet<InvStore>? InvStores { get; set; }
        public DbSet<InvItem>? InvItems { get; set; }
        public DbSet<InvUom>? InvUoms { get; set; }
        public DbSet<InvClassification>? InvClassifications { get; set; }
        public DbSet<InvSupplier>? InvSuppliers { get; set; }
        public DbSet<InvSupplierItem>? InvSupplierItems { get; set; }
        public DbSet<InvPoHdr>? InvPoHdrs { get; set; }
        public DbSet<InvPoItem>? InvPoItems { get; set; }
        public DbSet<InvPoHdrStatus>? InvPoHdrStatus { get; set; }
        public DbSet<InvTrxHdrStatus>? InvTrxHdrStatus { get; set; }
        public DbSet<InvTrxHdr>? InvTrxHdrs { get; set; }
        public DbSet<InvTrxDtl>? InvTrxDtls { get; set; }
        public DbSet<InvTrxType>? InvTrxTypes { get; set; }
        public DbSet<InvItemClass>? InvItemClasses { get; set; }
        public DbSet<InvTrxDtlOperator>? InvTrxDtlOperators { get; set; }
        public DbSet<InvStoreUser>? InvStoreUsers { get; set; }
        public DbSet<InvUomConversion>? InvUomConversions { get; set; }
        public DbSet<InvUomConvItem>? InvUomConvItems { get; set; }
        public DbSet<InvWarningLevel>? InvWarningLevels { get; set; }
        public DbSet<InvWarningType>? InvWarningTypes { get; set; }
        public DbSet<InvCategory>? InvCategories { get; set; }
        public DbSet<InvItemSysDefinedSpecs>? InvItemSysDefinedSpecs { get; set; }
        public DbSet<InvItemSpec_Steel>? InvItemSpec_Steel { get; set; }
        public DbSet<InvCategorySpecDef>? InvCategorySpecDefs { get; set; }
        public DbSet<InvCustomSpecType>? InvCustomSpecTypes { get; set; }
        public DbSet<InvCustomSpec>? InvCustomSpecs { get; set; }
        public DbSet<InvCatCustomSpec>? InvCatCustomSpecs { get; set; }
        public DbSet<InvItemCustomSpec>? InvItemCustomSpecs { get; set; }

        public virtual DbSet<SteelMainCat>? SteelMainCats { get; set; }
        public virtual DbSet<SteelSubCat>? SteelSubCats { get; set; }
        public virtual DbSet<SteelBrand>? SteelBrands { get; set; }
        public virtual DbSet<SteelMaterial>? SteelMaterials { get; set; }
        public virtual DbSet<SteelOrigin>? SteelOrigins { get; set; }
        public virtual DbSet<SteelMaterialGrade>? SteelMaterialGrades { get; set; }
        public virtual DbSet<SysLabel> SysLabels { get; set; }
        public virtual DbSet<SysSetting> SysSettings { get; set; }
        public virtual DbSet<SteelSize> SteelSizes { get; set; }

        public virtual DbSet<InvTrxApproval>? InvTrxApprovals { get; set; }
        public virtual DbSet<InvPOApproval>? InvPoApprovals { get; set; }

        public virtual DbSet<Report>? Reports { get; set; }
        public virtual DbSet<RptCategory>? rptCategories { get; set; }
        public  DbSet<RptReportCat>? rptReportCats { get; set; }
        public virtual DbSet<RptReportUser>? rptReportUsers { get; set; }
        public virtual DbSet<RptReportRole>? rptReportRoles { get; set; }
        public virtual DbSet<RptAccessType>? rptAccessTypes { get; set; }


        public virtual DbSet<InvItemMaster>? InvItemMasters { get; set; }
        public virtual DbSet<InvTrxDtlxItemMaster>? InvTrxDtlxItemMasters { get; set; }
        public virtual DbSet<InvItemBrand>? InvItemBrands { get; set; }
        public virtual DbSet<InvItemOrigin>? InvItemOrigins { get; set; }

    }
}
