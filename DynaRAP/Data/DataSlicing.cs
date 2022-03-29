using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DataSlicing
    {
        public DataSlicing()
        {
        }
        public DataSlicing(string date, string slicingName)
        {
            Date = date;
            SlicingName = slicingName;
        }
        public string Date { get; set; }
        public string SlicingName { get; set; }
    }
}
