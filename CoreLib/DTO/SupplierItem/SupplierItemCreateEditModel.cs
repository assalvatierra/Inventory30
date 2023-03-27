using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.SupplierItem
{
    public class SupplierItemCreateEditModel
    {
        public IList<InvSupplierItem> InvSupplierItem { get; set; }
        public SelectList? SupplierList { get; set; }
        public SelectList? InvItemList { get; set; }

    }
}
