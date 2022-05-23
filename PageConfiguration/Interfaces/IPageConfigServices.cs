using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageConfiguration.Interfaces
{
    public interface IPageConfigServices
    {
        public Model.PageConfigInfo getPageConfig(string pageCode);
    }
}
