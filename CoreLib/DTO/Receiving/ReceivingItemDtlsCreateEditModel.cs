using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Receiving
{
    public class ReceivingItemDtlsCreateEditModel
    {
        public InvTrxDtl? InvTrxDtl { get; set; }
        public SelectList? InvTrxHdrs { get; set; }
        public SelectList? InvTrxDtlOperators { get; set; }
        public SelectList? InvItems { get; set; }
        public SelectList? InvUoms { get; set; }
        public int HrdId { get; set; }
        public int StoreId { get; set; }
        public string? SelectedItem { get; set; }
    }
}
