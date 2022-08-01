using PageObjectShared.Interfaces;
using PageObjectShared.Model;

namespace BaseObject
{
    public class ObjectConfigBasic:IObjectConfig
    {
        public ObjectConfigBasic()
        {

        }

        public string TenantCode { get; set; } = "DEFAULT";
        public string PageCode { get; set; } = "SAMPLE.101";
        public string Version { get; set; } = "LATEST";
        public int Order { get; set; } = 1;

        public int ValidateModel()
        {
            return 0;
        }



    }
}