using PageObjectShared.Model;
using PageObjectShared.Interfaces;
using BaseObject;

namespace ObjectConfigService
{
    public class ObjectConfigServices: IObjectConfigServices
    {
        private string _DEFAULTCONFIGS = "DEFAULT";
        private string _tenantCode = "DEFAULT";
        private string _version = "LATEST";
        private List<IObjectConfig> _objectClasses =  new List<IObjectConfig>();
        private List<ObjectConfigInfo> _config = new List<ObjectConfigInfo>();


        public ObjectConfigServices(string tenantcode, string version)
        {
            if (!string.IsNullOrEmpty(tenantcode))
                this._tenantCode = tenantcode;
            if (!string.IsNullOrEmpty(version))
                this._version = version;

            this.loadObjectConfigClasses();
            this.loadObjectConfigurationsFromClasses();

         

        }

        private void loadObjectConfigurationsFromClasses()
        {
            foreach (var objectClass in this._objectClasses)
            {
                this._config.AddRange(objectClass.objectConfigInfo);
            }
        }

        public List<ObjectConfigInfo> getObjectConfig(string objectCode)
        {
            var configs = this._config.Where(d => d.ObjectCode == objectCode);

            List<ObjectConfigInfo> tenantConfigs = configs.ToList();
            if (!string.IsNullOrEmpty(this._tenantCode))
            {
                tenantConfigs = configs.Where(d => d.TenantCode == this._tenantCode).ToList();
                if (tenantConfigs.Count() == 0)
                    tenantConfigs = configs.Where(d => d.TenantCode == _DEFAULTCONFIGS).ToList();
            }

            List<ObjectConfigInfo> latestConfigs = latestConfigs = tenantConfigs;
            if (this._version != "LATEST")
                latestConfigs = tenantConfigs.Where(d => d.Version == this._version).ToList();


            return latestConfigs.OrderByDescending(v => v.Order).ToList();

        }

        private void loadPageConfigurationsFromClasses()
        {
            foreach (var configClass in this._objectClasses)
            {
                this._config.AddRange(configClass.objectConfigInfo);
            }
        }

        private void loadObjectConfigClasses()
        {
            /* to-do (future versions) :  to load using reflection */
            IList<IObjectConfig> _classes = new List<IObjectConfig>
            {
                new BaseObject.ObjectConfigBasic()
                //new VproConfig.vpro_config(),
                //new BaseConfig.PageConfigBasic()
            };
            /* end load */



            this._objectClasses = new List<IObjectConfig>();
            foreach (var objectClass in _classes)
            {
                //if(configClass.TenantCode == this._tenantCode || configClass.TenantCode == this._DEFAULTCONFIGS)
                this._objectClasses.Add(objectClass);
            }

        }

        public void setTargetVersion(string targetVersion)
        {
            this._version = targetVersion;
        }

    }
}