using PageObjectShared.Interfaces;
using PageObjectShared.Model;

namespace vproObject
{
    public class ObjectConfigBasic : IObjectConfig
    {
        public ObjectConfigBasic()
        {

        }

        private string _tenantCode = "VPRO";
        string IObjectConfig.TenantCode
        {
            get => this._tenantCode;
            //set { this._tenantCode = value; }  
        }


        public IList<IObjectConfigInfo> objectConfigInfo
        {
            get
            {
                List<IObjectConfigInfo> _objlist = new List<IObjectConfigInfo>();
                _objlist.Add(new Classes.SampleObject());


                return _objlist;
            }
        }


    }
}