using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Models.Inventory
{
    public partial class InvItemOrigin
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvItemMaster> InvItemMasters { get; set; }
    }
}
