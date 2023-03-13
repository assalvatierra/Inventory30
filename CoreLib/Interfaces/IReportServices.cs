using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.Reports;

namespace RealSys.CoreLib.Interfaces.Reports
{
    public interface IReportServices
    {
        public IList<Report> GetAvailableReportsByUserId(string id);
    }
}
