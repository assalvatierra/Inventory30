using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectShared.Interfaces
{
    public interface IObjectConfig
    {
        public string TenantCode { get; set; }
        public string PageCode { get; set; }
        public string Version { get; set; }
        public int Order { get; set; }

        public int ValidateModel();

    }
}
