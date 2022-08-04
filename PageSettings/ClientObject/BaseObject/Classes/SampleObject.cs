using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageObjectShared.Model;

namespace BaseObject.Classes
{
    public class SampleObject : IObjectConfigInfo
    {
        public string ObjectCode { get => "SAMPLE101"; }
        public string TenantCode { get => "DEFAULT"; }
        public string Version { get => "LATEST"; }
        public int Order { get => 1;  }

        public int Validate(object o)
        {
            return 1;
        }
    }
}
