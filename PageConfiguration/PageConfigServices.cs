﻿using System;
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

        public PageConfigServices(string tenantcode, string version)
        //public PageConfigServices(string TenantCode, string Version)
        {
            if (!string.IsNullOrEmpty(tenantcode))
                this._tenantCode = tenantcode;
            if (!string.IsNullOrEmpty(version))
                this._version = version;



            this._configClasses = new List<IPageConfig>();
            this._config = new List<Model.PageConfigInfo>();

            this.loadPageConfigClasses();
            this.loadPageConfigurationsFromClasses();

        }

        public void setTargetVersion(string targetVersion)
        {
            this._version = targetVersion;
        }

        public Model.PageConfigInfo getPageConfig(string pageCode)
        {
            var configs = this._config.Where(d=>d.PageCode==pageCode);

            List<Model.PageConfigInfo> tenantConfigs = configs.ToList();
            if (!string.IsNullOrEmpty(this._tenantCode))
            {
                tenantConfigs = configs.Where(d => d.TenantCode == this._tenantCode).ToList();
                if (tenantConfigs.Count() == 0)
                    tenantConfigs = configs.Where(d => d.TenantCode == _DEFAULTCONFIGS).ToList();
            }
 
            List<Model.PageConfigInfo> latestConfigs = latestConfigs = tenantConfigs; 
            if (this._version != "LATEST")
                latestConfigs = tenantConfigs.Where(d => d.Version == this._version).ToList();
          

            return latestConfigs.OrderByDescending(v => v.Order).FirstOrDefault();
         
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
                new Client.vpro_config(),
                new Basic.PageConfigBasic()
            };

            this._configClasses = new List<IPageConfig>();
            foreach (var configClass in _classes)
            {
                //if(configClass.TenantCode == this._tenantCode || configClass.TenantCode == this._DEFAULTCONFIGS)
                    this._configClasses.Add(configClass);
            }


        }
    }

}