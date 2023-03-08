using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Receiving
{
    public class ReceivingIndexModel
    {
        public IList<InvTrxHdr>? InvTrxHdrs { get; set; }
        public int StoreId { get; set; }
        public string? Status { get; set; }
        public string? Order { get; set; }
        public bool IsAdmin { get; set; }
    }
}
