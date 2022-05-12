using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ImportRequest
    {
        public string command { get; set; }
        public string sourcePath { get; set; }
        public string presetPack { get; set; }
        public string presetSeq { get; set; }
        public string flightSeq { get; set; }
        public string flightAt { get; set; }
        public string dataType { get; set; }
        public List<Part> parts { get; set; }
    }

    public class Part
    {
        public string partName { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }

        public Part(string partName, string julianStartAt, string julianEndAt)
        {
            this.partName = partName;
            this.julianStartAt = julianStartAt;
            this.julianEndAt = julianEndAt;
        }
    }

}
