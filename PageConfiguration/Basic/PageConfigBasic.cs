﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageConfiguration.Interfaces;

namespace PageConfiguration.Basic
{
    public class PageConfigBasic: IPageConfig
    {
        public string TenantCode { get { return this._tenantcode; } }

        private string _tenantcode = "DEFAULT";
        private IList<Model.PageConfigInfo> pageConfigInfos = new List<Model.PageConfigInfo>();
        public PageConfigBasic()
        {
            this.initializePageInfoData();
        }

        public IList<Model.PageConfigInfo> pageConfigInfo
        {
            get
            {
                return this.pageConfigInfos;
            }
        }



        private void initializePageInfoData()
        {
            this.pageConfigInfos = new List<Model.PageConfigInfo>();

            this.pageConfigInfos.Add(new Model.PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "unused 01",
                Order = 1,
                Version = "",
                ViewName = "",
                ConfigKeys = new List<Model.PageConfigKey>
                {
                    new Model.PageConfigKey(){ Key="key1", Value="data1",Remarks=""},
                    new Model.PageConfigKey(){ Key="key2", Value="data2",Remarks=""},
                    new Model.PageConfigKey(){ Key="key3", Value="data3",Remarks=""},
                    new Model.PageConfigKey(){ Key="key4", Value="data4",Remarks=""},

                }
            });

            this.pageConfigInfos.Add(new Model.PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "rpt002",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm.cshtml",
                genericConfigKeys = new Hashtable()
                {
                    {"SubTitle","Release Form" }, {"Company","Company" }
                },

 
            });

            this.pageConfigInfos.Add(new Model.PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "rpt001",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm.cshtml",
                genericConfigKeys = new Hashtable()
                { 
                    {"SubTitle","Receiving Form" }, {"Company","Company" } 
                }

 

            });



            this.pageConfigInfos.Add(new Model.PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "rpt003",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm_Adjustment.cshtml",
                genericConfigKeys = new Hashtable()
                {
                    {"SubTitle","Adjustment Form" }, {"Company","Company" }
                },


            });


            this.pageConfigInfos.Add(new Model.PageConfigInfo()
            {
                TenantCode = this.TenantCode,
                PageCode = "rpt004",
                Order = 1,
                Version = "",
                ViewName = "~/Areas/InvStore/TrxPrintForm_PO.cshtml",
                genericConfigKeys = new Hashtable()
                {
                    {"SubTitle","Purchase Order Form" }, {"Company","Company" }
                },


            });
        }


    }
}