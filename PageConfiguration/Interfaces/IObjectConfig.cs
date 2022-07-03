using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageConfiguration.Interfaces
{
    public interface IObjectConfig
    {
        public string TenantCode { get; }
        public IList<Model.ObjectConfigInfo> objectConfigInfo { get; }
    }
}
