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

        IList<ObjectConfigInfo> IObjectConfig.objectConfigInfo => throw new NotImplementedException();


    }
}