﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectShared.Interfaces
{
    public interface IObjectConfigServices
    {
        public void setTargetVersion(string targetVersion);
        public Model.ObjectConfigInfo getObjectConfig(string objectCode);
    }
}