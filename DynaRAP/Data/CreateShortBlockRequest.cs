using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class CreateShortBlockRequest
    {
        public string command { get; set; }
        public string blockMetaSeq { get; set; }
        public string partSeq { get; set; }
        public double sliceTime { get; set; }
        public double overlap { get; set; }
        public List<Parameter> parameters { get; set; }
        public List<ShortBlock> shortBlocks { get; set; }
        public bool forcedCreate { get; set; }
    }

    public class Parameter
    {
        public string paramSeq { get; set; }
        public string paramPack { get; set; }
        public string paramKey { get; set; }
        public string adamsKey { get; set; }
        public string zaeroKey { get; set; }
        public string grtKey { get; set; }
        public string fltpKey { get; set; }
        public string fltsKey { get; set; }
        public string propSeq { get; set; }
    }

    public class ShortBlock
    {
        public int blockNo { get; set; }
        public string blockName { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
    }


}
