using CoreLib.Models.Inventory;
using RealSys.CoreLib.Interfaces.Reports;
using RealSys.CoreLib.Models.Reports;
using Microsoft.EntityFrameworkCore;

namespace RealSys.Modules.Reports
{
    public class ReportRepo:IReportRepo
    {
        private readonly ApplicationDbContext _context;
        public ReportRepo(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IList<Report> GetAvailableReportsByIds(IList<int> rptIds)
        {
            return _context.Reports
                .Include(i=>i.RptReportCats).ThenInclude(i=>i.RptCategory)
                //.Include(i=>i.RptReportUsers.Where(a=>a.AspNetUserId==id))
                .Where( s=> rptIds.Contains (s.Id) )
                .ToList();

        }

        //public IList<int>? _getUserReportsByRoleIds(IList<int> roleIds)
        //{
        //    return _context.rptReportRoles.Where(d=> roleIds.Contains (d.Id))
        //        .Select(s=>s.Id).ToList() as IList<int>;
        //}
        public IList<int>? GetUserReportsByUsername(string userName)
        {
            return _context.rptReportUsers.Where(d => d.AspNetUserId == userName)
                .Select(s=>s.ReportId).ToList();
        }

        public string GetUserAccessLevel(string userName)
        {
            string userLevel = this._context.RptUserLevels.Where(d => d.AspNetUserId == userName).ToList().FirstOrDefault().RptRole;
            if (string.IsNullOrEmpty(userLevel)) userLevel = "USER";

            return userLevel;

        }
        //RptUserLevel


        public IQueryable<RptCategory> rptCategories 
        {
            get
            {
                return this._context.rptCategories;
            }
        }
    }
}