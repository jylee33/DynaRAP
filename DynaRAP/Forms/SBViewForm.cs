using System;
using System.Data;

namespace DynaRAP.Forms
{
    public partial class SBViewForm : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dt = null;

        public SBViewForm()
        {
            InitializeComponent();
        }

        public SBViewForm(DataTable dt) : this()
        {
            this.dt = dt;
        }

        private void SBViewForm_Load(object sender, EventArgs e)
        {
            // Form Test
            //dt = SetData();
            //this.dxChartControl1.DrawChart(dt);
        }


        private DataTable CreateData(string[,] data)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Argument", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));

            for (int i = 0; i < data.GetLength(0); i++)
            {
                DataRow row = table.NewRow();
                row["Argument"] = data[i, 0];
                row["Value"] = double.Parse(data[i, 1]);
                table.Rows.Add(row);
            }

            return table;
        }

        private DataTable SetData()
        {
            string[,] datas = new string[,]
            {
                { "2022-05-13 10:10:10.12345", "-160867" },
                { "2022-05-13 10:10:10.12445", "-101472" },
                { "2022-05-13 10:10:10.12545", "-82136" },
                { "2022-05-13 10:10:10.1265", "-47617" },
                { "2022-05-13 10:10:10.12745" , "-38" },
                //{ "Maximum" , "1870", "484452" },
                //{ "Maximum" , "2955", "315471" },
                //{ "Maximum" , "3405", "243369" },
                //{ "Maximum" , "4385", "115840" },
                //{ "Maximum" , "5160" , "33739" },
            };

            return CreateData(datas);
        }
    }
}