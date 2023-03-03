using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Models.SysDB;

namespace CoreLib.Interfaces.System
{
    public interface ISystemServices
    {
        public IQueryable<SysService> getServices(int _userId);
    }
}
