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
        public bool forcedImport { get; set; }
        public string qEquation { get; set; }
        public LpfOption lpfOption { get; set; }
        public HpfOption hpfOption { get; set; }
        public Dictionary<string, string> tempMappingParams { get; set; }
        //public TempMappingParams tempMappingParams { get; set; }
        public List<Part> parts { get; set; }
    }

    public class HpfOption
    {
        public string n { get; set; }
        public string cutoff { get; set; }
        public string btype { get; set; }
    }

    public class LpfOption
    {
        public string n { get; set; }
        public string cutoff { get; set; }
        public string btype { get; set; }
    }

    public class Part
    {
        public string partName { get; set; }
        public string julianStartAt { get; set; }
        public string julianEndAt { get; set; }
        public string offsetStartAt { get; set; }
        public string offsetEndAt { get; set; }

        public Part(string partName, string julianStartAt, string julianEndAt, string offsetStartAt, string offsetEndAt)
        {
            this.partName = partName;
            this.julianStartAt = julianStartAt;
            this.julianEndAt = julianEndAt;
            this.offsetStartAt = offsetStartAt;
            this.offsetEndAt = offsetEndAt;
        }
    }

}
