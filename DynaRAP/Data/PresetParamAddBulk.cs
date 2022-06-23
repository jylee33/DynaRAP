using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class PresetParamAddBulk
    {
        public string command { get; set; }
        public List<Param> @params { get; set; }
    }

    public class Param
    {
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
        public string paramSeq { get; set; }
        public string paramPack { get; set; }

        public Param(string presetPack, string presetSeq, string paramSeq, string paramPack)
        {
            this.presetPack = presetPack;
            this.presetSeq = presetSeq;
            this.paramSeq = paramSeq;
            this.paramPack = paramPack;
        }
    }

}
