using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class PresetData
    {
        string presetName = string.Empty;
        string presetPack = string.Empty;

        public string PresetName { get => presetName; set => presetName = value; }
        public string PresetPack { get => presetPack; set => presetPack = value; }

        public PresetData()
        {
        }

        public PresetData(string presetName, string presetPack)
        {
            this.presetName = presetName;
            this.presetPack = presetPack;
        }

    }

    public class FlyingTpyeResponse
    {

        public int code { get; set; }
        public string message { get; set; }
        public List<FlyingType> response { get; set; }

        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }
    }

    public class FlyingType
    {
        public string typeCode { get; set; }
        public string typeName { get; set; }
        public int Delete { get; set; }
    }

    public class FlyingTpyeRequest
    {
        public string command { get; set; }
        public List<FlyingType> flightTypes { get; set; }
    }

}
