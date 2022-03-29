using DevExpress.XtraEditors;
using DynaRAP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.TEST
{
    public partial class GridGroupingForm : DevExpress.XtraEditors.XtraForm
    {
        public GridGroupingForm()
        {
            InitializeComponent();
        }

        private void GridGroupingForm_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = CreateData();

            gridView1.Columns["ParentID"].Visible = false;

            new GridCheckMarksSelection(gridView1);
            gridView1.Columns.ColumnByFieldName("ParentID").GroupIndex = 0;
            this.gridView1.ExpandAllGroups();

        }

        private BindingList<FlyingData> CreateData()
        {
            BindingList<FlyingData> list = new BindingList<FlyingData>();
            //list.Add(new FlyingData(0, -1, "ImportModuleScenarioName", null));
            list.Add(new FlyingData(1, 0, "비행데이터", null));
            list.Add(new FlyingData(2, 0, "버펫팅데이터", null));
            list.Add(new FlyingData(3, 0, "Short Block", null));

            FlyingData data = new FlyingData();
            data.ParentID = 1;
            data.FlyingName = "2022-03-03_형상A_1호기.bin";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 1;
            data.FlyingName = "2022-03-03_형상B_3호기.bin";
            data.Check = false;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 1;
            data.FlyingName = "2022-03-03_형상A_2호기.bin";
            data.Check = false;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 2;
            data.FlyingName = "버펫팅_01.bpt";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 2;
            data.FlyingName = "버펫팅_02.bpt";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 2;
            data.FlyingName = "버펫팅_03.bpt";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 2;
            data.FlyingName = "버펫팅_04.bpt";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_01.sbl";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_02.sbl";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_03.sbl";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_04.sbl";
            data.Check = true;
            list.Add(data);

            data = new FlyingData();
            data.ParentID = 3;
            data.FlyingName = "ShortBlock_05.sbl";
            data.Check = true;
            list.Add(data);

            return list;
        }
    }
}