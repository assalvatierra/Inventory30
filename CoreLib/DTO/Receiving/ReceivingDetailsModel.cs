using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Receiving
{
    public class ReceivingDetailsModel
    {
        public InvTrxHdr InvTrxHdr { get; set; }
        public IEnumerable<InvTrxDtl>? InvTrxDtls { get; set; }
        public int StoreId { get; set; }
    }
}
