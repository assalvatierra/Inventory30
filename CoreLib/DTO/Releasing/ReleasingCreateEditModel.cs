using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Releasing
{
    public class ReleasingCreateEditModel
    {
        public InvTrxHdr InvTrxHdr { get; set; }
        public SelectList InvStoresList { get; set; }
        public SelectList InvTrxHdrStatusList { get; set; }
        public SelectList InvTrxTypeList { get; set; }
        public string User { get; set; }
        public int StoreId { get; set; }
    }
}
