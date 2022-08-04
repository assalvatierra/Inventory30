using PageObjectShared.Interfaces;
using PageObjectShared.Model;

namespace BaseObject
{
    public class ObjectConfigBasic:IObjectConfig
    {
        public ObjectConfigBasic()
        {

        }

        string IObjectConfig.TenantCode => throw new NotImplementedException();


        public IList<ObjectConfigInfo> objectConfigInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        } 


    }
}