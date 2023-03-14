using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.PurchaseOrder
{
    public class InvPOHdrDetailsModel
    {
        public InvPoHdr InvPoHdr { get; set; }
        public IEnumerable<InvPoItem>? InvPoItems { get; set; }
        public int StoreId { get; set; }
    }
}
