using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PageObjectShared.Model
{
    public class ObjectConfigInfo
    {
        public string PageCode { get; set; } = String.Empty;
        public string TenantCode { get; set; } = String.Empty ;
        public string Version { get; set; } = String.Empty;
        public int Order { get; set; }
        public string ObjectName { get; set; }
        public Hashtable genericConfigKeys { get; set; }
    }
}
