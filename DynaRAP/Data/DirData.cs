using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class DirData
    {
        static int UniqueID = 4;
        public DirData()
        {
            ID = UniqueID++;
        }
        public DirData(int id, int parentId, string dirType, string dirName, string refSeq, string refSubSeq)
        {
            ID = id;
            ParentID = parentId;
            DirType = dirType;
            DirName = dirName;
            RefSeq = refSeq;
            RefSubSeq = refSubSeq;
        }
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string DirType { get; set; }
        public string DirName { get; set; }
        public string RefSeq { get; set; }
        public string RefSubSeq { get; set; }
    }

}
