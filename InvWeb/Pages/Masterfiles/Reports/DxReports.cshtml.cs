using InvWeb.Pages.Masterfiles.ItemMaster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.Reports;
using CoreLib.Models.Inventory;
using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Interfaces.Reports;

namespace InvWeb.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IReportServices _reportService;

        public IList<Report> invReports { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IReportServices reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }


        public async Task OnGetAsync()
        {
            this.invReports = this._reportService.GetAvailableReportsByUserId("AAA");

        }
    }
}
