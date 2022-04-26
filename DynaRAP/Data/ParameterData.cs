using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ParameterData
    {
        static int UniqueID = 4;
        public ParameterData()
        {
            ID = UniqueID++;
        }
        public ParameterData(int id, int parentId, string dirType, string dirName)
        {
            ID = id;
            ParentID = parentId;
            DirType = dirType;
            DirName = dirName;
        }
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string DirType { get; set; }
        public string DirName { get; set; }
    }

}
