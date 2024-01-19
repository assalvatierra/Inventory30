using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Models.Inventory
{
    public partial class InvTrxDtlxItemMaster
    {
        public int Id { get; set; }
        public int InvTrxDtlId { get; set; }
        public int InvItemMasterId { get; set; }

        public virtual InvTrxDtl InvTrxDtl { get; set; }
        public virtual InvItemMaster InvItemMaster { get; set; }
    }
}
