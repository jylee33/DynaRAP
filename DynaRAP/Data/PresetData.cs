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
}
