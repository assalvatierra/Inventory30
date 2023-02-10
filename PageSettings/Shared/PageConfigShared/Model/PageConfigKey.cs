using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageConfigShared.Model
{
    public class PageConfigKey
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Remarks { get; set; } = string.Empty;
    }
}
