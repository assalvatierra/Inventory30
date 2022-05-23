using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageConfiguration.Interfaces;

namespace PageConfiguration
{
    public class PageConfigServices: IPageConfigServices
    {
        private string _DEFAULTCONFIGS = "DEFAULT"; 

        private string _tenantCode = "DEFAULT";
        private string _version = "LATEST";

        private List<IPageConfig> _configClasses;
        private List<Model.PageConfigInfo> _config;

        public PageConfigServices()
        //public PageConfigServices(string TenantCode, string Version)
        {
            //    if(!string.IsNullOrEmpty(TenantCode))
            //        this._tenantCode = TenantCode;
            //    if (!string.IsNullOrEmpty(Version))
            //        this._version = Version;


            this._configClasses = new List<IPageConfig>();
            this._config = new List<Model.PageConfigInfo>();

            this.loadPageConfigClasses();
            this.loadPageConfigurationsFromClasses();

        }
        public Model.PageConfigInfo getPageConfig(string pageCode)
        {
            if(this._version=="LATEST")
                return this._config.Where(d => d.PageCode == pageCode).OrderByDescending(v=>v.Order).FirstOrDefault();
            else
                return this._config.Where(d => d.PageCode == pageCode && d.Version==this._version).OrderByDescending(v => v.Order).FirstOrDefault();
        
        }

        private void loadPageConfigurationsFromClasses()
        {
            foreach(var configClass in this._configClasses)
            {
                this._config.AddRange(configClass.pageConfigInfo);
            }
        }

     
        private void loadPageConfigClasses()
        {
            IList < IPageConfig > _classes = new List<IPageConfig>
            {
                new Basic.PageConfigBasic(),
                new Client.vpro_config()
            };

            this._configClasses = new List<IPageConfig>();
            foreach (var configClass in _classes)
            {
                if(configClass.TenantCode == this._tenantCode || configClass.TenantCode == this._DEFAULTCONFIGS)
                    this._configClasses.Add(configClass);
            }


        }
    }

}
