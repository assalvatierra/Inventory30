using CoreLib.Models.Inventory;
using RealSys.CoreLib.Interfaces.Reports;
using RealSys.CoreLib.Models.Reports;
using Microsoft.EntityFrameworkCore;

namespace RealSys.Modules.Reports
{
    public class ReportServices:IReportServices
    {
        private readonly ApplicationDbContext _context;
        public ReportServices(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IList<Report> GetAvailableReportsByUserId(string id)
        {
            var rptIds = new List<int> { 1, 2 };
            return _context.Reports
                .Include(i=>i.RptReportCats).ThenInclude(i=>i.RptCategory)
                //.Include(i=>i.RptReportUsers.Where(a=>a.AspNetUserId==id))
                .Where( s=> rptIds.Contains (s.Id) )
                .ToList();

        }
    }
}