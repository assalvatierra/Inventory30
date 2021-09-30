using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDBSchema.Models.Items
{
    public class ItemSearchResult
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Item { get; set; }
        public int Qty { get; set; }
        public string Uom { get; set; }
        public string InvStore { get; set; }
    }

    
}