using System;
using System.Collections.Generic;
using System.Collections;
using PageConfigShared.Interfaces;
using PageConfigShared.Model;

namespace VproConfig
{
    public class vpro_config : IPageConfig
    {
        private string _tenantid = "VPRO";
        private IList<PageConfigInfo> pageConfigInfos = new List<PageConfigInfo>();

        private string _companyName = "VPRO Inc";
        public vpro_config()
        {
            this.initializePageInfoData();
        }
        public string TenantCode { get { return this._tenantid; } }
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
                PageCode = "rpt001",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm.cshtml",
                genericConfigKeys = new Hashtable()
                {
                    {"SubTitle","Receiving Form" }, {"Company",_companyName },{"Branch","" }, {"Party","Party"}, {"RefNo","RefNo"},
                    {"Person1","Prepared By" },{"Person2","Approved By" },{"Person3","Performed By" },
                    {"Name1","Admin" },{"Name2","Manager" },{"Name3","Custodian" }
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
                    {"SubTitle","Release Form" }, {"Company",_companyName },{"Branch","" }, {"Party","Party"}, {"RefNo","RefNo"},
                    {"Person1","Prepared By" },{"Person2","Approved By" },{"Person3","Performed By" },
                    {"Name1","Admin" },{"Name2","Manager" },{"Name3","Custodian" }

                },


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
                    {"SubTitle","Adjustment Form" }, {"Company",_companyName },{"Branch","" }, {"Party","Party"}, {"RefNo","RefNo"},
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
                    {"SubTitle","Purchase Request Form" }, {"Company",_companyName },{"Branch","" }, {"Party","Party"}, {"RefNo","RefNo"},
                    {"Person1","Prepared By:" },{"Person2","Approved By:" },{"Person3","Performed By:" },
                    {"Name1","Admin" },{"Name2","Manager" },{"Name3","Custodian" }
                },


            });


        }

    }
}