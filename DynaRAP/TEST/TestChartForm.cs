using DevExpress.XtraCharts;
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
    public partial class TestChartForm : DevExpress.XtraEditors.XtraForm
    {
        public TestChartForm()
        {
            InitializeComponent();
        }

        private void TestChartForm_Load(object sender, EventArgs e)
        {
            InitChart();
        }

        private void InitChart()
        {
            chartControl1.BackColor = Color.FromArgb(45, 45, 48);
            chartControl1.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;

            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            chartControl1.MouseClick += ChartControl1_MouseClick;

            XYDiagram diagram = chartControl1.Diagram as XYDiagram;
            AxisX axisX = diagram.AxisX;
            AxisY axisY = diagram.AxisY;
            //axisX.Visibility = DevExpress.Utils.DefaultBoolean.False;
            //axisY.Visibility = DevExpress.Utils.DefaultBoolean.False;

            // Create a line series, bind it to data and add to the chartControl1.
            Series series = new Series("Temperature", ViewType.Line);
            series.DataSource = OverviewDataPoint.GetDataPoints();
            series.ArgumentDataMember = "Date";
            series.ValueDataMembers.AddRange("Value");
            chartControl1.Series.Add(series);

            //series.View.Color = Color.FromArgb(255, 0, 0);

            // Enable series markers.
            LineSeriesView view = (LineSeriesView)series.View;
            view.MarkerVisibility = DevExpress.Utils.DefaultBoolean.False;

            // Display series labels and customize their text format.
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            series.Label.ResolveOverlappingMode = ResolveOverlappingMode.HideOverlapped;
            series.Label.TextPattern = "{V:.#}";

            // Create a chart title.
            ChartTitle chartTitle = new ChartTitle();
            chartTitle.Text = "Temperature (°F)";
            chartControl1.Titles.Add(chartTitle);

            // Customize axes.
            //diagram.AxisX.Label.TextPattern = "{A:MMM, d (HH:mm)}";
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 9;
            diagram.AxisX.WholeRange.SideMarginsValue = 0.5;
            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;

        }

        private void ChartControl1_MouseClick(object sender, MouseEventArgs e)
        {
            ChartControl chart = sender as ChartControl;
            if(e.Button == MouseButtons.Left)
            {
                ChartHitInfo hi = chart.CalcHitInfo(e.X, e.Y);
                SeriesPoint point = hi.SeriesPoint;
                if(point != null)
                {
                    Console.WriteLine("point is {0}", point.Argument.ToString());
                }
            }
            else if(e.Button== MouseButtons.Right)
            {
                Strip sl = new Strip();
                sl.Color = Color.FromArgb(255, Color.LightSeaGreen);
                //sl..IntervalOffset = 0;// Math.Min(curRange.Width, curRange.Height);
                //sl.StripWidth = 10;// Math.Abs(curRange.Height - curRange.Width);
                XYDiagram diagram = chart.Diagram as XYDiagram;
                AxisX axisX = diagram.AxisX;
                AxisY axisY = diagram.AxisY;
                axisX.Strips.Clear();
                axisX.Strips.Add(sl);
            }
        }


        private void chartControl1_RegionChanged(object sender, EventArgs e)
        {

        }
    }
}