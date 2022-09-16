using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ParamModuleData
    {
        string seq = string.Empty;
        string moduleName = string.Empty;
        string copyFromSeq = string.Empty;

        public string Seq { get => seq; set => seq = value; }
        public string ModuleName { get => moduleName; set => moduleName = value; }
        public string CopyFromSeq { get => copyFromSeq; set => copyFromSeq = value; }

        public ParamModuleData()
        {
        }

        public ParamModuleData(string seq, string moduleName, string copyFromSeq)
        {
            this.seq = seq;
            this.moduleName = moduleName;
            this.copyFromSeq = copyFromSeq;
        }

    }
}
