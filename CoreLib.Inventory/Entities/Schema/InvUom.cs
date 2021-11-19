
namespace CoreLib.Inventory.Entities
{
    using System;
    using System.Collections.Generic;

    public partial class InvUom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvUom()
        {
            this.InvItems = new HashSet<InvItem>();
            this.InvPoItems = new HashSet<InvPoItem>();
            this.InvRecItems = new HashSet<InvRecItem>();
            this.InvRequestItems = new HashSet<InvRequestItem>();
            this.InvAdjItems = new HashSet<InvAdjItem>();
            this.InvTrxDtls = new HashSet<InvTrxDtl>();
        }

        public int Id { get; set; }
        public string uom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvItem> InvItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvPoItem> InvPoItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvRecItem> InvRecItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvRequestItem> InvRequestItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvAdjItem> InvAdjItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvTrxDtl> InvTrxDtls { get; set; }
    }
}
