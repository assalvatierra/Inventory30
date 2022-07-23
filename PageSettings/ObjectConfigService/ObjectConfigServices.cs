using PageObjectShared.Model;
using PageObjectShared.Interfaces;

namespace ObjectConfigService
{
    public class ObjectConfigServices
    {

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

        private void loadObjectConfigClasses()
        {
            /* to-do (future versions) :  to load using reflection */
            IList<IObjectConfig> _classes = new List<IObjectConfig>
            {
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
    }
}