using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.InvItems
{
    public class InvItemStoreStocks
    {
        //   Id = itemdtl.InvItemId,
        //InvItem = itemDetails,
        //                ItemQty = services.GetAvailableCountByItem(id, itemdtl.InvTrxHdr.InvStoreId),
        //                InvTrxHdr = itemdtl.InvTrxHdr,
        //                InvUom = itemdtl.InvUom

        public int Id { get; set; }
        public string InvItem_Code { get; set; }
        public string InvItem_Description { get; set; }
        public InvTrxHdr InvTrxHdr { get; set; }
        public string InvUom { get; set; }
        public int Qty_Stock { get; set; }
        public int Qty_Released { get; set; }
        public int Qty_Available { get; set; }
        public int Qty_Approved { get; set; }
        public string store { get; set; }
        public int storeId { get; set; }

    }
}
