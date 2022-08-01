//using PageObjectShared.Model;
using PageObjectShared.Interfaces;

namespace ObjectConfigService
{
    public class ObjectConfigServices: IObjectConfigServices
    {
        private string _DEFAULTCONFIGS = "DEFAULT";

        private string _tenantCode = "DEFAULT";
        private string _version = "LATEST";
        private List<IObjectConfig> _objectClasses =  new List<IObjectConfig>();
 

        public ObjectConfigServices(string tenantcode, string version)
        {
            if (!string.IsNullOrEmpty(tenantcode))
                this._tenantCode = tenantcode;
            if (!string.IsNullOrEmpty(version))
                this._version = version;

            this.loadObjectConfigClasses();

        }

        public List<IObjectConfig> getObjectConfig(string objectCode)
        {
            var configs = this._objectClasses.Where(d => d.PageCode == objectCode);

            List<IObjectConfig> tenantConfigs = configs.ToList();
            if (!string.IsNullOrEmpty(this._tenantCode))
            {
                tenantConfigs = configs.Where(d => d.TenantCode == this._tenantCode).ToList();
                if (tenantConfigs.Count() == 0)
                    tenantConfigs = configs.Where(d => d.TenantCode == _DEFAULTCONFIGS).ToList();
            }

            List<IObjectConfig> latestConfigs = latestConfigs = tenantConfigs;
            if (this._version != "LATEST")
                latestConfigs = tenantConfigs.Where(d => d.Version == this._version).ToList();


            return latestConfigs;


        }

        private void loadObjectConfigClasses()
        {
            /* to-do (future versions) :  to load using reflection */

            this._objectClasses.Add(new BaseObject.ObjectConfigBasic());
 
            /* end load */


        }

        public void setTargetVersion(string targetVersion)
        {
            this._version = targetVersion;
        }

    }
}