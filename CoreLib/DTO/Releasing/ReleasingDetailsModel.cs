using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Releasing
{
    public class ReleasingDetailsModel
    {
        public InvTrxHdr InvTrxHdr { get; set; }
        public IEnumerable<InvTrxDtl> InvTrxDtls { get; set; }
    }
}
