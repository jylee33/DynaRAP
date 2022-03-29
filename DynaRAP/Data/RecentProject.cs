using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class RecentProject
    {
        public RecentProject()
        {
        }
        public RecentProject(string date, string projectName)
        {
            Date = date;
            ProjectName = projectName;
        }
        public string Date { get; set; }
        public string ProjectName { get; set; }
    }
}
