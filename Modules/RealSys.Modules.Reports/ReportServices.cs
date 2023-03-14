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
            id = "";
            return _context.Reports
                //.Include(i=>i.RptReportCats.Where(a=>a.RptCategoryId==1))
                //.Include(i=>i.RptReportUsers.Where(a=>a.AspNetUserId==id))
                .ToList();

        }
    }
}