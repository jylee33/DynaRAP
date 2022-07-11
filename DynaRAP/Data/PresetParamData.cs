using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class PresetParamData
    {
        public PresetParamData()
        {
        }
        public PresetParamData(string paramKey, string adamsKey, string zaeroKey, string grtKey, string fltpKey, string fltsKey, string partInfo, string partInfoSub, string seq, string propSeq, string paramPack, int del)
        {
            ParamKey = paramKey;
            AdamsKey = adamsKey;
            ZaeroKey = zaeroKey;
            GrtKey = grtKey;
            FltpKey = fltpKey;
            FltsKey = fltsKey;
            PartInfo = partInfo;
            PartInfoSub = partInfoSub;
            Seq = seq;
            PropSeq = propSeq;
            ParamPack = paramPack;
            Del = del;
        }
        public string ParamKey { get; set; }
        public string AdamsKey { get; set; }
        public string ZaeroKey { get; set; }
        public string GrtKey { get; set; }
        public string FltpKey { get; set; }
        public string FltsKey { get; set; }
        public string PartInfo { get; set; }
        public string PartInfoSub { get; set; }
        public string Seq { get; set; }
        public string PropSeq { get; set; }
        public string ParamPack { get; set; }
        public int Del { get; set; }
    }
}
