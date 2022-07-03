using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageConfiguration.Interfaces
{
    public interface IObjectConfigServices
    {
        public void setTargetVersion(string targetVersion);
        public Model.ObjectConfigInfo getObjectConfig(string objectCode);
    }
}
