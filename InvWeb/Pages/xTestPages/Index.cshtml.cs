using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReportViewModel.InvStore;

namespace InvWeb.Pages.xTestPages
{
    public class IndexModel : PageModel
    {
        public string rptView = "~/Areas/InvStore/TrxPrintForm.cshtml";

        public TrxHdr hdr;

        private void initializeSample()
        {
            this.hdr = this.generateSampleHeader();
            this.hdr.Details = this.generateSampleDetails(this.hdr.Id);

        }

        private TrxHdr generateSampleHeader()
        {
            TrxHdr hdr = new TrxHdr()
            {
                Id = 1,
                Name = "Sample 01",
                Remarks = "This is a sample"
            };

            return hdr;
        }

        private List<TrxDetail> generateSampleDetails(int hdrId)
        {
            List<TrxDetail> dtlData = new List<TrxDetail>();
            dtlData.Add(new TrxDetail() { Id = 1, Description = "Detail 01", Remarks = "Sample Dtl 01", TrxHdrId = hdrId });
            dtlData.Add(new TrxDetail() { Id = 1, Description = "Detail 02", Remarks = "Sample Dtl 02", TrxHdrId = hdrId });
            dtlData.Add(new TrxDetail() { Id = 1, Description = "Detail 03", Remarks = "Sample Dtl 03", TrxHdrId = hdrId });
            dtlData.Add(new TrxDetail() { Id = 1, Description = "Detail 04", Remarks = "Sample Dtl 04", TrxHdrId = hdrId });

            return dtlData;
        }


        public void OnGet()
        {
            this.initializeSample();

        }
    }
}
