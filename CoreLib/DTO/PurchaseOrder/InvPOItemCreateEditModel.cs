using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.PurchaseOrder
{
    public class InvPOItemCreateEditModel
    {
        public InvPoItem InvPoItem { get; set; }
        public SelectList? InvItemList { get; set; }
        public SelectList? InvPoHdrList { get; set; }
        public SelectList? InvUomList { get; set; }
        public int? HdrId { get; set; }

    }
}
