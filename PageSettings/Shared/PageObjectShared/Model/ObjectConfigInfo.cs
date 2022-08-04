using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PageObjectShared.Model
{
    public interface IObjectConfigInfo
    {
        public string ObjectCode { get;  }
        public string TenantCode { get;  }
        public string Version { get;  }
        public int Order { get;  }
        public int Validate(Object o);

    }        
}
