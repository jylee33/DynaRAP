using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Common
{
    #region For Potato
    public class DLL_DATA
    {
        public string Name { get; set; }
        public string ButtockLine { get; set; }
        public List<Parameter> parameters { get; set; }
        public class Parameter
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Unit { get; set; }
            public DataTable data { get; set; }
        }
    }
    #endregion

}
