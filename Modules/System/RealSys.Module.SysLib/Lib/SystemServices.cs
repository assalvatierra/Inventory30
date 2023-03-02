using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Models.System;
using JobsV1.Models;
using CoreLib.Interfaces;
using CoreLib.Interfaces.System;

namespace RealSys.Module.SysLib.Lib
{
    public class SystemServices: ISystemServices
    {
        private readonly SysDBContext context;
        public SystemServices(SysDBContext _context) { 
            this.context = _context;
        }

        public virtual IQueryable<SysService> getServices(int _userId)
        {
            return this.context.SysServices;
        }
    }
}
