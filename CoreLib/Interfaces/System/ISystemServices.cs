using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobsV1.Models;

namespace CoreLib.Interfaces.System
{
    public interface ISystemServices
    {
        public IQueryable<SysService> getServices(int _userId);
    }
}
