using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DllParamDataAddBulkRequest
    {
        public string command { get; set; }
        public string dllSeq { get; set; }
        public string paramSeq { get; set; }
        public List<string> data { get; set; }
    }

}
