using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.PurchaseOrder
{
    public class InvPOItemDelete
    {
        public InvPoItem InvPoItem { get; set; }

        public int InvHdrId { get; set; }
    }
}
