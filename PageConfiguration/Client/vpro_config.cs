using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageConfiguration.Interfaces;

namespace PageConfiguration.Client
{
    public class vpro_config : IPageConfig
    {
        private string _tenantid = "VPRO";
        private IList<Model.PageConfigInfo> pageConfigInfos = new List<Model.PageConfigInfo>();

        public vpro_config()
        {

        }
        public string TenantCode { get{return this._tenantid;} }
        public IList<Model.PageConfigInfo> pageConfigInfo
        {
            get
            {
                return this.pageConfigInfos;
            }
        }

    }
}
