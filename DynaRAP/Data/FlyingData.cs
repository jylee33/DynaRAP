using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class FlyingData
    {
        static int UniqueID = 4;
        public FlyingData()
        {
            ID = UniqueID++;
        }
        public FlyingData(int id, int parentId, string flyingName, bool? check)
        {
            ID = id;
            ParentID = parentId;
            FlyingName = flyingName;
            Check = check;
        }
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string FlyingName { get; set; }
        public bool? Check { get; set; }
    }

}
