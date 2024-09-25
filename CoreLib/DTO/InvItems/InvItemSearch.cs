using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.InvItems
{
    public class InvItemSearch
    {

        public int Id { get; set; }
        public int MasterId { get; set; }
        public int ItemId { get; set; }
        public int ItemQty { get; set; }
        public int ReleasedQty { get; set; }
        public int ItemOnHoldQty { get; set; }
        public int StockOnHand { get; set; }
        public int AvailableQty { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string BatchNo { get; set; }
        public string LotNo { get; set; }
        public string Brand { get; set; }
        public string Origin { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }
    }
}
