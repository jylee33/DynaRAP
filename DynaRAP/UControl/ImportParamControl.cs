using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DynaRAP.EventData;
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
    public partial class ImportParamControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler<SelectedRangeEventArgs> OnSelectedRange;
        public event EventHandler DeleteBtnClicked;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        private Dictionary<string, List<string>> dicData;
        string selKey = String.Empty;
        List<double> listData = new List<double>();

        SizeF curRange = SizeF.Empty;
        List<SizeF> ranges = new List<SizeF>();
        List<int> selectedIndices = new List<int>();

        object minValue = null;
        object maxValue = null;

        public string Title
        {
            set
            {
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

        public ImportParamControl()
        {
            InitializeComponent();
        
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void ImportParamControl_Load(object sender, EventArgs e)
        {
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
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            //((XYDiagram)chartControl1.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.AutoGrid = false;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.fff}";
            //diag.AxisX.Label.TextPattern = "{A:MMM-dd HH}";

            this.rangeControl1.Client = this.chartControl1;
            rangeControl1.RangeChanged += RangeControl1_RangeChanged;
            rangeControl1.ShowLabels = true;
            diagram.RangeControlDateTimeGridOptions.GridMode = ChartRangeControlClientGridMode.Manual;
            diagram.RangeControlDateTimeGridOptions.GridOffset = 1;
            diagram.RangeControlDateTimeGridOptions.GridSpacing = 60;
            diagram.RangeControlDateTimeGridOptions.LabelFormat = "HH:mm:ss.fff";
            diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Millisecond;

            rangeControl1.SelectedRange = new RangeControlRange(minValue, maxValue);
        }

        private void RangeControl1_RangeChanged(object sender, DevExpress.XtraEditors.RangeControlRangeEventArgs range)
        {
            //Console.WriteLine("start : {0}, end : {1}", range.Range.Minimum, range.Range.Maximum);
            this.minValue = range.Range.Minimum;
            this.maxValue = range.Range.Maximum;

            Sync();

        }

        public void SelectRegion(object min, object max)
        {
            this.minValue = min;
            this.maxValue = max;

            rangeControl1.RangeChanged -= RangeControl1_RangeChanged;
            rangeControl1.SelectedRange = new RangeControlRange(min, max);
            rangeControl1.RangeChanged += RangeControl1_RangeChanged;
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

                if (i == 0)
                {
                    minValue = dt;
                    maxValue = dt;
                }

                i++;
            }

            return table;
        }

        private void InitChart()
        {


        }

        public void Sync()
        {
            SelectedRangeEventArgs args = new SelectedRangeEventArgs(minValue, maxValue);
            if (null != OnSelectedRange)
                OnSelectedRange(this, args);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.DeleteBtnClicked != null)
                this.DeleteBtnClicked(this, new EventArgs());
        }
    }
}
