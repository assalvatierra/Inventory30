using CoreLib.Inventory.Models;

namespace CoreLib.Models.Inventory
{

    using System;
    using System.Collections.Generic;
    
    public partial class InvItemMaster
    {
        public InvItemMaster()
        {
            this.InvTrxDtlxItemMasters = new HashSet<InvTrxDtlxItemMaster>();
        }

        public int Id { get; set; }
        public int InvItemId { get; set; }
        public string? LotNo { get; set; }
        public string BatchNo { get; set; }
        public string ItemQty { get; set; }
        public int InvUomId { get; set; }
        public string? Remarks { get; set; }
        public int InvItemBrandId { get; set; }
        public int InvItemOriginId { get; set; }

        public virtual InvItem? InvItem { get; set; }
        public virtual InvUom? InvUom { get; set; }
        public virtual ICollection<InvTrxDtlxItemMaster>? InvTrxDtlxItemMasters { get; set; }
        public virtual InvItemBrand? InvItemBrand { get; set; }
        public virtual InvItemOrigin? InvItemOrigin { get; set; }
    }
}
