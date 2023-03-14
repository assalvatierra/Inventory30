using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreLib.DTO.PurchaseOrder
{
    public class InvPOHdrCreateEditModel
    {
        [Required]
        public InvPoHdr InvPoHdr { get; set; }
        public SelectList? InvPoHdrStatusId { get; set; }
        public SelectList? InvStoreId { get; set; }
        public SelectList? InvSupplierId { get; set; }
        public string? UserId { get; set; }
        public int StoreId { get; set; }
    }
}
