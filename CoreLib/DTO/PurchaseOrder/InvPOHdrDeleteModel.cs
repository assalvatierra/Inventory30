using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.PurchaseOrder
{
    public class InvPOHdrDeleteModel
    {
        public InvPoHdr InvPoHdr { get; set; }

        public int StoreId { get; set; }

    }
}
