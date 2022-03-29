using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class ProjectData
    {
        static int UniqueID = 4;
        public ProjectData()
        {
            ID = UniqueID++;
        }
        public ProjectData(int id, int parentId, string projectName, int link)
        {
            ID = id;
            ParentID = parentId;
            ProjectName = projectName;
            Link = link;
        }
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string ProjectName { get; set; }
        public int Link { get; set; }
    }

    public class ProjectDataGenertor
    {
        public static List<ProjectData> CreateData()
        {
            List<ProjectData> sales = new List<ProjectData>();
            sales.Add(new ProjectData(0, -1, "Project Name", 0));
            sales.Add(new ProjectData(1, 0, "비행데이터", 0));
            sales.Add(new ProjectData(2, 0, "버펫팅데이터", 0));
            sales.Add(new ProjectData(3, 0, "Short Block", 0));

            ProjectData data = new ProjectData();
            data.ParentID = 1;
            data.ProjectName = "2022-03-03_형상A_1호기.bin";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 1;
            data.ProjectName = "2022-03-03_형상B_3호기.bin";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 1;
            data.ProjectName = "2022-03-03_형상A_2호기.bin";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 2;
            data.ProjectName = "버펫팅_01.bpt";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 2;
            data.ProjectName = "버펫팅_02.bpt";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 2;
            data.ProjectName = "버펫팅_03.bpt";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 2;
            data.ProjectName = "버펫팅_04.bpt";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_01.sbl";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_02.sbl";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_03.sbl";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_04.sbl";
            data.Link = 1;
            sales.Add(data);

            data = new ProjectData();
            data.ParentID = 3;
            data.ProjectName = "ShortBlock_05.sbl";
            data.Link = 1;
            sales.Add(data);

            return sales;
        }
    }

}
