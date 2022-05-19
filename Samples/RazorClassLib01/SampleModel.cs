using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorClassLib01
{
    public class SampleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public IEnumerable<sampleDetail> Details { get; set; }
    }


    public class sampleDetail
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Description { get; set; }

        public string Remarks { get; set; }

    }
}
