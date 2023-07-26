using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.SupplierItem
{
    public class SupplierItemIndexModel
    {
        public int SupplierId { get; set; }
        public IList<InvSupplierItem> InvSupplierItem { get; set; }
    }
}
