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
            if (o == null) return -1;


            //WebDBSchema.Models.InvCategory data = (WebDBSchema.Models.InvCategory)o;


            //if(data.Id > 0)
            //    return 1;
            //else
            //    return -1;

            return 1;
        }
    }
}
