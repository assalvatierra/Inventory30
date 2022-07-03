using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PageConfiguration.Model
{
    public class ObjectConfigInfo
    {
        public string PageCode { get; set; }
        public string TenantCode { get; set; }
        public string Version { get; set; }
        public int Order { get; set; }
        public string ObjectName { get; set; }
        public Hashtable genericConfigKeys { get; set; }
    }
}
