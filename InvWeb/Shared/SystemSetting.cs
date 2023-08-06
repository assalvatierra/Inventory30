namespace InvWeb.Shared
{
    public class SystemSetting
    {
        public string GetValue(string sCode)
        {
            string sLabel = sCode;

            if (sCode == "Store") sLabel = "Warehouse";
            if (sCode == "AppName") sLabel = "Inventory";
            if (sCode == "AppVersion") sLabel = "Beta 1.1";
            if (sCode == "Tenant") sLabel = "RealSteel";

            return sLabel;

        }

    }
}
