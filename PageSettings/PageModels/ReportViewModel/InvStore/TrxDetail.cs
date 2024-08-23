using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportViewModel.InvStore
{
    public class TrxDetail
    {
        public int Id { get; set; }
        public int TrxHdrId { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public int Qty { get; set; }
        public string Uom { get; set; }
        public int Count { get; set; }
        public string Operation { get; set; }
        public string LotNo { get; set; }
        public string BatchNo { get; set; }

        public IList<TrxDetail_SubItems> subItems { get; set; }

    }

    public class TrxDetail_SubItems
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Brand { get; set; }
        public string Origin { get; set; }
        public int Qty { get; set; }
        public string Remarks { get; set; }
    }
}
