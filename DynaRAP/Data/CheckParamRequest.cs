using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class CheckParamRequest
    {
        public string command { get; set; }
        public string presetPack { get; set; }
        public object presetSeq { get; set; }
        public string dataType { get; set; }
        public string headerRow { get; set; }
        public string importFilePath { get; set; }
    }

}
