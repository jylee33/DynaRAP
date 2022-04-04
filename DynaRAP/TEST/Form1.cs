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

namespace DynaRAP.TEST
{
    public partial class Form1 : Form
    {
        Series series1 = new Series();
        ChartArea myChartArea = new ChartArea("LineChartArea");

        SizeF curRange = SizeF.Empty;
        List<SizeF> ranges = new List<SizeF>();
        List<int> selectedIndices = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //myChartArea.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Hours;

            //myChartArea.AxisX.MajorGrid.Interval = 1D;
            //myChartArea.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            //myChartArea.AxisX.MinorGrid.Enabled = true;
            //myChartArea.AxisX.MinorGrid.Interval = 3D;
            //myChartArea.AxisX.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Hours;
            //myChartArea.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gray;
            //myChartArea.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            //myChartArea.AxisX.MinorTickMark.Enabled = true;
            //myChartArea.AxisX.MinorTickMark.Interval = 3D;
            //myChartArea.AxisX.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Hours;
            ////myChartArea.AxisX.ScaleView.MinSize = 3D;
            ////myChartArea.AxisX.ScaleView.MinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            //myChartArea.AxisX.ScaleView.Size = 3D;
            //myChartArea.AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            //myChartArea.AxisX.ScaleView.SmallScrollMinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Hours;
            //myChartArea.AxisX.ScaleView.SmallScrollSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;

            ////
            myChartArea.CursorX.IsUserEnabled = true;
            myChartArea.CursorX.IsUserSelectionEnabled = true;
            myChartArea.AxisX.ScaleView.Zoomable = false;
            ////


            chart1.ChartAreas.RemoveAt(0);
            chart1.ChartAreas.Add(myChartArea);

            series1.ChartType = SeriesChartType.Line;
            series1.Name = "VAS";
            series1.XValueType = ChartValueType.DateTime;
            series1.IsValueShownAsLabel = false;
            //series1.IsVisibleInLegend = false;
            series1.LabelForeColor = Color.Red;
            series1.MarkerStyle = MarkerStyle.Square;
            series1.MarkerSize = 3;
            series1.MarkerColor = Color.Red;

            SettingMyData();
            chart1.Series.Add(series1);
        }

        private void SettingMyData()
        {
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 1, 00), 4);
            Push_Data(series1, new DateTime(2021, 1, 2, 1, 1, 01), 3);
            Push_Data(series1, new DateTime(2021, 1, 3, 1, 1, 02), 5);
            Push_Data(series1, new DateTime(2021, 1, 4, 1, 1, 03), 3);
            Push_Data(series1, new DateTime(2021, 1, 5, 1, 1, 04), 4);
            Push_Data(series1, new DateTime(2021, 1, 6, 1, 1, 05), 2);
            Push_Data(series1, new DateTime(2021, 1, 7, 1, 1, 06), 4);
            Push_Data(series1, new DateTime(2021, 1, 8, 1, 1, 07), 5);
            Push_Data(series1, new DateTime(2021, 1, 9, 1, 1, 08), 4);
            Push_Data(series1, new DateTime(2021, 1, 10, 1, 1, 09), 3);
            Push_Data(series1, new DateTime(2021, 1, 11, 1, 1, 10), 1);
            Push_Data(series1, new DateTime(2021, 1, 12, 1, 1, 11), 3);
            Push_Data(series1, new DateTime(2021, 1, 13, 1, 1, 12), 5);
            Push_Data(series1, new DateTime(2021, 1, 14, 1, 1, 13), 3);
            Push_Data(series1, new DateTime(2021, 1, 15, 1, 1, 14), 4);
            Push_Data(series1, new DateTime(2021, 1, 16, 1, 1, 15), 2);
            Push_Data(series1, new DateTime(2021, 1, 17, 1, 1, 16), 4);
            Push_Data(series1, new DateTime(2021, 1, 18, 1, 1, 17), 5);
            Push_Data(series1, new DateTime(2021, 1, 19, 1, 1, 18), 4);
            Push_Data(series1, new DateTime(2021, 1, 20, 1, 1, 19), 3);
            Push_Data(series1, new DateTime(2021, 1, 21, 1, 1, 20), 1);
            Push_Data(series1, new DateTime(2021, 1, 22, 1, 1, 21), 2);
            Push_Data(series1, new DateTime(2021, 1, 23, 1, 1, 22), 5);
            Push_Data(series1, new DateTime(2021, 1, 24, 1, 1, 23), 3);
            Push_Data(series1, new DateTime(2021, 1, 25, 1, 1, 24), 5);
            Push_Data(series1, new DateTime(2021, 1, 26, 1, 1, 25), 3);
            Push_Data(series1, new DateTime(2021, 1, 27, 1, 1, 26), 4);
            Push_Data(series1, new DateTime(2021, 1, 28, 1, 1, 27), 2);
            Push_Data(series1, new DateTime(2021, 1, 29, 1, 1, 28), 4);
            Push_Data(series1, new DateTime(2021, 1, 30, 1, 1, 29), 5);
            Push_Data(series1, new DateTime(2021, 1, 31, 1, 1, 30), 4);
            Push_Data(series1, new DateTime(2021, 2, 1, 1, 1, 31), 3);
            Push_Data(series1, new DateTime(2021, 2, 2, 1, 1, 32), 5);
            Push_Data(series1, new DateTime(2021, 2, 3, 1, 1, 33), 3);
            Push_Data(series1, new DateTime(2021, 2, 4, 1, 1, 34), 4);
            Push_Data(series1, new DateTime(2021, 2, 5, 1, 1, 35), 2);
            Push_Data(series1, new DateTime(2021, 2, 6, 1, 1, 36), 4);
            Push_Data(series1, new DateTime(2021, 2, 7, 1, 1, 37), 5);
            Push_Data(series1, new DateTime(2021, 2, 8, 1, 1, 38), 4);
            Push_Data(series1, new DateTime(2021, 2, 9, 1, 1, 39), 3);
            Push_Data(series1, new DateTime(2021, 2, 10, 1, 1, 40), 1);
            Push_Data(series1, new DateTime(2021, 2, 11, 1, 1, 41), 3);
            Push_Data(series1, new DateTime(2021, 2, 12, 1, 1, 42), 5);
            Push_Data(series1, new DateTime(2021, 2, 13, 1, 1, 43), 3);
            Push_Data(series1, new DateTime(2021, 2, 14, 1, 1, 44), 4);
            Push_Data(series1, new DateTime(2021, 2, 15, 1, 1, 45), 2);
            Push_Data(series1, new DateTime(2021, 2, 16, 1, 1, 46), 4);
            Push_Data(series1, new DateTime(2021, 2, 17, 1, 1, 47), 5);
            Push_Data(series1, new DateTime(2021, 2, 18, 1, 1, 48), 4);
            Push_Data(series1, new DateTime(2021, 2, 19, 1, 1, 49), 3);
            Push_Data(series1, new DateTime(2021, 2, 20, 1, 1, 50), 1);
            Push_Data(series1, new DateTime(2021, 2, 21, 1, 1, 51), 2);
            Push_Data(series1, new DateTime(2021, 2, 22, 1, 1, 52), 5);
            Push_Data(series1, new DateTime(2021, 2, 23, 1, 1, 53), 3);
            Push_Data(series1, new DateTime(2021, 2, 24, 1, 1, 54), 5);
            Push_Data(series1, new DateTime(2021, 2, 25, 1, 1, 55), 3);
            Push_Data(series1, new DateTime(2021, 2, 26, 1, 1, 56), 4);
            Push_Data(series1, new DateTime(2021, 2, 27, 1, 1, 57), 2);
            Push_Data(series1, new DateTime(2021, 2, 28, 1, 1, 58), 4);

            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 00), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 01), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 02), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 03), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 04), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 05), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 06), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 07), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 08), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 09), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 10), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 11), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 12), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 13), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 14), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 15), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 16), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 17), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 18), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 19), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 20), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 21), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 22), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 23), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 24), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 25), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 26), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 27), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 28), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 29), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 30), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 31), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 32), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 33), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 34), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 35), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 36), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 37), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 38), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 39), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 40), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 41), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 42), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 43), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 44), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 45), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 46), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 47), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 48), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 49), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 50), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 51), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 52), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 53), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 54), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 55), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 56), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 57), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 58), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 2, 59), 5);

            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 00), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 01), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 02), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 03), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 04), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 05), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 06), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 07), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 08), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 09), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 10), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 11), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 12), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 13), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 14), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 15), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 16), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 17), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 18), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 19), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 20), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 21), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 22), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 23), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 24), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 25), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 26), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 27), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 28), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 29), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 30), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 31), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 32), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 33), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 34), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 35), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 36), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 37), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 38), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 39), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 40), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 41), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 42), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 43), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 44), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 45), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 46), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 47), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 48), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 49), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 50), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 51), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 52), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 53), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 54), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 55), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 56), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 57), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 58), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 3, 59), 5);

            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 00), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 01), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 02), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 03), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 04), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 05), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 06), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 07), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 08), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 09), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 10), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 11), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 12), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 13), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 14), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 15), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 16), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 17), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 18), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 19), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 20), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 21), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 22), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 23), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 24), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 25), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 26), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 27), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 28), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 29), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 30), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 31), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 32), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 33), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 34), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 35), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 36), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 37), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 38), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 39), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 40), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 41), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 42), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 43), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 44), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 45), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 46), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 47), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 48), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 49), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 50), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 51), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 52), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 53), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 54), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 55), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 56), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 57), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 58), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 4, 59), 5);

            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 00), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 01), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 02), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 03), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 04), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 05), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 06), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 07), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 08), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 09), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 10), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 11), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 12), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 13), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 14), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 15), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 16), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 17), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 18), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 19), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 20), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 21), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 22), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 23), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 24), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 25), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 26), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 27), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 28), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 29), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 30), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 31), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 32), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 33), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 34), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 35), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 36), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 37), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 38), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 39), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 40), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 41), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 42), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 43), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 44), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 45), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 46), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 47), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 48), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 49), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 50), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 51), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 52), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 53), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 54), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 55), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 56), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 57), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 58), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 5, 59), 5);

            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 00), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 01), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 02), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 03), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 04), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 05), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 06), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 07), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 08), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 09), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 10), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 11), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 12), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 13), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 14), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 15), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 16), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 17), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 18), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 19), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 20), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 21), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 22), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 23), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 24), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 25), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 26), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 27), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 28), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 29), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 30), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 31), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 32), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 33), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 34), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 35), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 36), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 37), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 38), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 39), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 40), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 41), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 42), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 43), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 44), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 45), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 46), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 47), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 48), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 49), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 50), 1);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 51), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 52), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 53), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 54), 5);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 55), 3);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 56), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 57), 2);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 58), 4);
            Push_Data(series1, new DateTime(2021, 1, 1, 1, 6, 59), 5);


        }

        private void Push_Data(Series series, DateTime dt, int data)
        {
            DataPoint dp = new DataPoint(); //데이타 기록하기 정도
            dp.SetValueXY(dt, data);
            series.Points.Add(dp);

        }

        private void chart1_SelectionRangeChanging(object sender, CursorEventArgs e)
        {
            curRange = new SizeF((float)e.NewSelectionStart, (float)e.NewSelectionEnd);
            //DateTime dt1 =  DateTime.FromOADate(e.NewSelectionStart);
            //DateTime dt2 =  DateTime.FromOADate(e.NewSelectionEnd);
            //Console.WriteLine("start : {0}, end : {1}", dt1, dt2);
        }

        private void chart1_SelectionRangeChanged(object sender, CursorEventArgs e)
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

            if (!dt1.Equals(dt2))
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

    }
}
