using System;
using System.Collections.Generic;
using System.Collections;
using PageConfigShared.Interfaces;
using PageConfigShared.Model;

namespace BaseConfig
{
    public class PageConfigBasic : IPageConfig
    {
        public string TenantCode { get { return this._tenantcode; } }

        private string _tenantcode = "DEFAULT";
        private IList<PageConfigInfo> pageConfigInfos = new List<PageConfigInfo>();

        public PageConfigBasic()
        {
            this.initializePageInfoData();
        }

        public IList<PageConfigInfo> pageConfigInfo
        {
            get
            {
                return this.pageConfigInfos;
            }
        }



        private void initializePageInfoData()
        {
            this.pageConfigInfos = new List<PageConfigInfo>();

            this.pageConfigInfos.Add(new PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "unused 01",
                Order = 1,
                Version = "",
                ViewName = "",
                ConfigKeys = new List<PageConfigKey>
                {
                    new PageConfigKey(){ Key="key1", Value="data1",Remarks=""},
                    new PageConfigKey(){ Key="key2", Value="data2",Remarks=""},
                    new PageConfigKey(){ Key="key3", Value="data3",Remarks=""},
                    new PageConfigKey(){ Key="key4", Value="data4",Remarks=""},

                }
            });

            this.pageConfigInfos.Add(new PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "rpt002",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm.cshtml",
                genericConfigKeys = new Hashtable()
                {
                    {"SubTitle","Release Form" }, {"Company","Company" },{"Branch","" }, {"Party","Party"}, {"RefNo","RefNo"},
                    {"Person1","Prepared By:" },{"Person2","Approved By:" },{"Person3","Performed By:" },
                    {"Name1","Admin" },{"Name2","Manager" },{"Name3","Custodian" }
                },


            });

            this.pageConfigInfos.Add(new PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "rpt001",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm.cshtml",
                genericConfigKeys = new Hashtable()
                {
                    {"SubTitle","Receiving Form" }, {"Company","Company" },{"Branch","" }, {"Party","Party"}, {"RefNo","RefNo"},
                    {"Person1","Prepared By:" },{"Person2","Approved By:" },{"Person3","Performed By:" },
                    {"Name1","Admin" },{"Name2","Manager" },{"Name3","Custodian" }
                }
            });



            this.pageConfigInfos.Add(new PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "rpt003",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm.cshtml",
                genericConfigKeys = new Hashtable()
                {
                    {"SubTitle","Adjustment Form" }, {"Company","Company" },{"Branch","" }, {"Party","Party"}, {"RefNo","RefNo"},
                    {"Person1","Prepared By:" },{"Person2","Approved By:" },{"Person3","Performed By:" },
                    {"Name1","Admin" },{"Name2","Manager" },{"Name3","Custodian" }
                },


            });


            this.pageConfigInfos.Add(new PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "rpt004",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm.cshtml",
                genericConfigKeys = new Hashtable()
                {
                    {"SubTitle","Purchase Request Form" }, {"Company","Company" },{"Branch","" }, {"Party","Party"}, {"RefNo","RefNo"},
                    {"Person1","Prepared By:" },{"Person2","Approved By:" },{"Person3","Performed By:" },
                    {"Name1","Admin" },{"Name2","Manager" },{"Name3","Custodian" }
                },


            });

            this.pageConfigInfos.Add(new PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "Sample101",
                Order = 1,
                Version = "",
                ViewName = "",
                genericConfigKeys = new Hashtable()
                {
                    {"Message","Sample101 msg" }
                }


            });



        }


    }
}
