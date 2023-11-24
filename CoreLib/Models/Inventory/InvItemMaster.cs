using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Models.Inventory
{
    public partial class InvItemMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvItemMaster()
        {
            this.InvTrxDtlxItemMasters = new HashSet<InvTrxDtlxItemMaster>();
        }


        public int Id { get; set; }
        public int InvItemId { get; set; }
        public string LotNo { get; set; }
        public string BatchNo { get; set; }
        public string ItemQty { get; set; }
        public int InvUomId { get; set; }
        public string Remarks { get; set; }
        public int InvItemBrandId { get; set; }
        public int InvItemOriginId { get; set; }

        public virtual InvItem InvItem { get; set; }
        public virtual InvUom InvUom { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvTrxDtlxItemMaster> InvTrxDtlxItemMasters { get; set; }
        public virtual InvItemBrand InvItemBrand { get; set; }
        public virtual InvItemOrigin InvItemOrigin { get; set; }
    }
}
