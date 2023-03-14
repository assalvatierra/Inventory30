using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.PurchaseOrder
{
    public class InvPOHdrModel
    {
        public IList<InvPoHdr>? InvPoHdrs { get; set; }
        public int StoreId { get; set; }
        public string? Status { get; set; }
        public string? Order { get; set; }
        public bool IsAdmin { get; set; }
    }
}

