using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DynaRAP.UTIL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DynaRAP.UControl
{
    public partial class ImportParamControl : DevExpress.XtraEditors.XtraUserControl
    {
        private Dictionary<string, List<string>> dicData;
        string selKey = String.Empty;
        Series series1 = new Series();
        ChartArea myChartArea = new ChartArea("LineChartArea");

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

        public ImportParamControl()
        {
            InitializeComponent();
        }

        private void ImportParamControl_Load(object sender, EventArgs e)
        {
            labelControl1.Visible = false;
            InitComboList();
            InitChart();
        }

        private void InitComboList()
        {
            cboParameter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboParameter.SelectedIndexChanged += cboParameter_SelectedIndexChanged;
            cboParameter.SelectedIndex = -1;
        }

        private void cboParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboParameter.EditValue != null)
            {
                selKey = cboParameter.EditValue.ToString();
                Push_Data(selKey);
            }
        }

        private void InitChart()
        {
            ////
            myChartArea.CursorX.IsUserEnabled = true;
            myChartArea.CursorX.IsUserSelectionEnabled = true;
            myChartArea.AxisX.ScaleView.Zoomable = false;
            ////


            chart1.ChartAreas.RemoveAt(0);
            chart1.ChartAreas.Add(myChartArea);

            chart1.SelectionRangeChanging += Chart1_SelectionRangeChanging;
            chart1.SelectionRangeChanged += Chart1_SelectionRangeChanged;

            series1.ChartType = SeriesChartType.Line;
            series1.Name = "VAL";
            series1.XValueType = ChartValueType.DateTime;
            series1.IsValueShownAsLabel = false;
            series1.IsVisibleInLegend = false;
            series1.LabelForeColor = Color.Red;
            series1.MarkerStyle = MarkerStyle.Square;
            series1.MarkerSize = 3;
            series1.MarkerColor = Color.Red;


            chart1.Series.Add(series1);

        }

        private void Chart1_SelectionRangeChanging(object sender, CursorEventArgs e)
        {
            curRange = new SizeF((float)e.NewSelectionStart, (float)e.NewSelectionEnd);
        }

        private void Chart1_SelectionRangeChanged(object sender, CursorEventArgs e)
        {
            //Console.WriteLine(curRange.ToString());

            selectedIndices.Union(collectDataPoints(chart1.Series[0],
                                  curRange.Width, curRange.Height))
                           .Distinct();

            StripLine sl = new StripLine();
            sl.BackColor = Color.FromArgb(255, Color.LightSeaGreen);
            sl.IntervalOffset = Math.Min(curRange.Width, curRange.Height);
            sl.StripWidth = Math.Abs(curRange.Height - curRange.Width);
            chart1.ChartAreas[0].AxisX.StripLines.Add(sl);

            //int x1 = ((int)((PointF)curRange).X);
            //int x2 = ((int)((PointF)curRange).X) + ((int)sl.StripWidth);

            //Console.WriteLine("x1 : {0}, x2 : {1}", x1, x2);
            //Console.WriteLine("x1 : {0}, x2 : {1}", ((PointF)curRange).X, ((PointF)curRange).X);

            DateTime dt1 = DateTime.FromOADate(((PointF)curRange).X);
            DateTime dt2 = DateTime.FromOADate(((PointF)curRange).X + sl.StripWidth);

            //if (!dt1.Equals(dt2))
            {
                Console.WriteLine("start : {0}, end : {1}", dt1, dt2);
                ranges.Add(curRange);
            }
        }

        List<int> collectDataPoints(Series s, double min, double max)
        {
            List<int> hits = new List<int>();
            for (int i = 0; i < s.Points.Count; i++)
                if (s.Points[i].XValue >= min && s.Points[i].XValue <= max) hits.Add(i);
            return hits;
        }

        private void Push_Data(string selKey)
        {
            if (dicData[selKey] != null)
            {
                series1.Points.Clear();

                int i = 0;
                foreach (string value in dicData[selKey])
                {
                    string day = dicData["DATE"][i++];
                    DateTime dt = Utils.GetDateFromJulian(day);
                    float data = float.Parse(value);
                    Push_Data(series1, dt, data);
                }
            }
        }

        private void Push_Data(Series series, DateTime dt, float data)
        {
            Console.WriteLine(string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff} - {1}", dt, data));
            DataPoint dp = new DataPoint(); //데이타 기록하기 정도
            dp.SetValueXY(dt, data);
            series.Points.Add(dp);

        }

    }
}
