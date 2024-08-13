using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportViewModel.InvStore
{
    public class TrxHdr
    {
        public TrxHdr()
        {
            this.pageSetting = new Hashtable();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public IList<TrxDetail> Details { get; set; }

        public string Party { get; set; }
        public string Type { get; set; }
        public string PaidTo { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }

        public int GetTotalItemsCount()
        {
            if (this.Details != null)
            {
                return this.Details.Count();
            }
            return 0;
        }

        public int GetTotalItemsQty()
        {
            if (this.Details != null)
            {
                return this.Details.Select(i=>i.Qty).Sum();
            }
            return 0;
        }
        public Hashtable pageSetting = new Hashtable();
       // public TrxPageSetting pageSetting = new TrxPageSetting();

    }
}
