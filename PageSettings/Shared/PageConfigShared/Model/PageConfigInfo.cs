﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PageConfigShared.Model
{
    public class PageConfigInfo
    {
        public string PageCode { get; set; }
        public string TenantCode { get; set; }
        public string Version { get; set; }
        public int Order { get; set; }
        public string ViewName { get; set; }
        public List<PageConfigKey> ConfigKeys { get; set; }
        public Hashtable genericConfigKeys { get; set; }
    }
}