using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Releasing
{
    public class ReceivingDetailsModel
    {
        public InvTrxHdr? InvTrxHdr { get; set; }
        public IEnumerable<InvTrxDtl>? InvTrxDtls { get; set; }
        public InvTrxApproval? InvTrxApproval { get;set; }
    }
}
