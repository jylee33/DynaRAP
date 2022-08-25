using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class BinTableSaveRequest
    {
        public string command { get; set; }
        public string metaName { get; set; }
        public List<string> parts { get; set; }
        public List<string> selectedShortBlocks { get; set; }
        public DataProps dataProps { get; set; }
        public List<PickUpParam> pickUpParams { get; set; }
    }

    public class DataProps
    {
        public string key { get; set; }
        public string key2 { get; set; }
        public string tags { get; set; }
    }

    public class PickUpParam
    {
        public string paramSeq { get; set; }
        public string paramPack { get; set; }
        public string fieldType { get; set; }
        public string fieldPropSeq { get; set; }
        public string paramKey { get; set; }
        public string adamsKey { get; set; }
        public string zaeroKey { get; set; }
        public string grtKey { get; set; }
        public string fltpKey { get; set; }
        public string fltsKey { get; set; }
        public List<UserParamTable> userParamTable { get; set; }
    }

    public class UserParamTable
    {
        public double nominal { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }


}
