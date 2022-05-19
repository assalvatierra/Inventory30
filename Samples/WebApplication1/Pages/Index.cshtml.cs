using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string strView1 = "PartialSample";

        public RazorClassLib01.SampleModel hdr;

        private void initializeSample()
        {
            this.hdr = this.generateSampleHeader();
            this.hdr.Details = this.generateSampleDetails(this.hdr.Id);

        }

        private RazorClassLib01.SampleModel generateSampleHeader()
        {
            RazorClassLib01.SampleModel hdr = new RazorClassLib01.SampleModel()
            {
                Id = 1,
                Name = "Sample 01",
                Remarks = "This is a sample"
            };

            return hdr;
        }

        private List<RazorClassLib01.sampleDetail>  generateSampleDetails(int hdrId)
        {
            List<RazorClassLib01.sampleDetail> dtlData = new List<RazorClassLib01.sampleDetail>();
            dtlData.Add(new RazorClassLib01.sampleDetail() { Id = 1, Description = "Detail 01", Remarks = "Sample Dtl 01", ModelId = hdrId });
            dtlData.Add(new RazorClassLib01.sampleDetail() { Id = 1, Description = "Detail 02", Remarks = "Sample Dtl 02", ModelId = hdrId });
            dtlData.Add(new RazorClassLib01.sampleDetail() { Id = 1, Description = "Detail 03", Remarks = "Sample Dtl 03", ModelId = hdrId });
            dtlData.Add(new RazorClassLib01.sampleDetail() { Id = 1, Description = "Detail 04", Remarks = "Sample Dtl 04", ModelId = hdrId });

            return dtlData;
        }



        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            this.initializeSample();

        }
    }
}
