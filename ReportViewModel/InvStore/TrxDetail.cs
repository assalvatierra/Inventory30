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
        public int Count { get; set; }
        public string Operation { get; set; }

    }
}
