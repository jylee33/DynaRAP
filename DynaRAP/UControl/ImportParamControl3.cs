using DevExpress.XtraCharts;
using DevExpress.XtraEditors.Controls;
using DynaRAP.UTIL;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DynaRAP.UControl
{
    public partial class ImportParamControl3 : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        private Dictionary<string, List<string>> dicData;
        string selKey = String.Empty;
        List<double> listData = new List<double>();

        SizeF curRange = SizeF.Empty;
        List<SizeF> ranges = new List<SizeF>();
        List<int> selectedIndices = new List<int>();

        public string Title
        {
            set
            {
                labelControl1.Text = value;
            }
        }

        public Dictionary<string, List<string>> DicData
        {
            set
            {
                this.dicData = value;
                List<string> paramList = value.Keys.ToList();
                cboParameter.Properties.Items.AddRange(paramList);
                cboParameter.Properties.Items.Remove("DATE");

                cboParameter.SelectedIndex = 0;
            }
        }

        public ImportParamControl3()
        {
            InitializeComponent();
        
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void ImportParamControl3_Load(object sender, EventArgs e)
        {
            labelControl1.Visible = false;
            InitComboList();
            InitChart();
        }

        private void InitComboList()
        {
            cboParameter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboParameter.Properties.DropDownRows = 15;
            cboParameter.SelectedIndexChanged += cboParameter_SelectedIndexChanged;

            List<string> paramList = dicData.Keys.ToList();
            cboParameter.Properties.Items.AddRange(paramList);
            cboParameter.Properties.Items.Remove("DATE");

            cboParameter.SelectedIndex = -1;
        }

        private void cboParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboParameter.EditValue != null)
            {
                selKey = cboParameter.EditValue.ToString();
                AddChartData(selKey);
            }
        }

        private void AddChartData(string strKey)
        {
            chartControl1.Series.Clear();

            Series series = new Series("Series1", ViewType.Line);
            chartControl1.Series.Add(series);

            series.DataSource = GetChartValues(strKey);

            series.ArgumentScaleType = ScaleType.DateTime;
            //series.ArgumentScaleType = ScaleType.Numerical;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            //((XYDiagram)chartControl1.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            XYDiagram diag = (XYDiagram)chartControl1.Diagram;
            diag.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
            diag.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diag.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;
            diag.AxisX.DateTimeScaleOptions.AutoGrid = false;
            diag.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            //diag.AxisX.Label.TextPattern = "{A:MMM-dd HH}";

        }

        private DataTable GetChartValues(string strKey)
        {
            // Create an empty table.
            DataTable table = new DataTable("Table1");

            // Add two columns to the table.
            //table.Columns.Add("Argument", typeof(Int32));
            table.Columns.Add("Argument", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));

            DataRow row = null;
            int i = 0;
            listData.Clear();
            foreach (string value in dicData[selKey])
            {
                row = table.NewRow();
                string day = dicData["DATE"][i];
                DateTime dt = Utils.GetDateFromJulian(day);
                double data = double.Parse(value);
                listData.Add(data);
                row["Argument"] = dt;
                //row["Argument"] = i;
                row["Value"] = data;
                table.Rows.Add(row);
                i++;
            }

            return table;
        }

        private void InitChart()
        {


        }


    }
}
