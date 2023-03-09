using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Common.TrxDetails
{
    public class TrxDetailsItemDeleteModel
    {
        public InvTrxDtl InvTrxDtl { get; set; }
        public int HrdId { get; set; }
    }
}
