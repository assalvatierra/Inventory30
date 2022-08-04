using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectShared.Interfaces
{
    public interface IObjectConfig
    {
        public string TenantCode { get; }
        public IList<Model.IObjectConfigInfo> objectConfigInfo { get; }

    }
}
