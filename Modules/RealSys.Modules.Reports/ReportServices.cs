using CoreLib.Models.Inventory;
using RealSys.CoreLib.Interfaces.Reports;
using RealSys.CoreLib.Models.Reports;

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
            return _context.Reports.ToList();

        }
    }
}