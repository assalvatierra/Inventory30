using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportViewModel.InvStore
{
    public class TrxHdr
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public IList<TrxDetail> Details { get; set; }

    }
}
