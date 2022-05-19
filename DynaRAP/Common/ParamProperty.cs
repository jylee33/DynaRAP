using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Common
{
    public class ParamProperty
    {
        string code;
        string unit;

        public string Code { get => code; set => code = value; }
        public string Unit { get => unit; set => unit = value; }

        public ParamProperty(string code, string unit)
        {
            this.code = code;
            this.unit = unit;
        }

    }
}
