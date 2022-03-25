using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
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
using Series = DevExpress.XtraCharts.Series;

namespace DynaRAP.UControl
{
    public partial class ScenarioSummaryControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ScenarioSummaryControl()
        {
            InitializeComponent();
        }

        private void ScenarioSummaryControl_Load(object sender, EventArgs e)
        {
            InitializeDataOverview();
            InitializeDataSlicingList();
        }

        private void InitializeDataOverview()
        {
            chartControl1.BackColor = Color.FromArgb(45, 45, 48);
            chartControl1.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;

            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            XYDiagram diagram = chartControl1.Diagram as XYDiagram;
            AxisX axisX = diagram.AxisX;
            AxisY axisY = diagram.AxisY;
            axisX.Visibility = DevExpress.Utils.DefaultBoolean.False;
            axisY.Visibility = DevExpress.Utils.DefaultBoolean.False;

            // Create a line series, bind it to data and add to the chartControl1.
            Series series = new Series("Temperature", ViewType.Line);
            series.DataSource = DataPoint.GetDataPoints();
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
            diagram.AxisX.Label.TextPattern = "{A:MMM, d (HH:mm)}";
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 9;
            diagram.AxisX.WholeRange.SideMarginsValue = 0.5;
            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;

        }

        private void InitializeDataSlicingList()
        {
            List<DataSlicing> list = new List<DataSlicing>();
            DateTime dtNow = DateTime.Now;
            string strNow = string.Format("{0:yyyy/MM/dd HH:mm:ss}", dtNow);

            list.Add(new DataSlicing(strNow, "구간 이름 1 구간 이름 1 구간 이름 1 구간 이름 1 구간 이름 1 구간 이름 1 구간 이름 1 구간 이름 1 구간 이름 1 구간 이름 1 "));
            list.Add(new DataSlicing(strNow, "구간 이름 2"));
            list.Add(new DataSlicing(strNow, "구간 이름 3"));

            this.gridControl1.DataSource = list;

            gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView1.OptionsView.ShowColumnHeaders = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ColumnAutoWidth = true;

            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

            //gridView1.GridControl.BackColor = Color.FromArgb(45, 45, 48);
            gridView1.Appearance.Empty.BackColor = Color.FromArgb(45, 45, 48);

            GridColumn colDate = gridView1.Columns["Date"];
            GridColumn colProjectName = gridView1.Columns["ProjectName"];
            colDate.OptionsColumn.FixedWidth = true;
            colDate.Width = 130;
            //colProjectName.Width = 300;

        }
    }

    public class DataSlicing
    {
        public DataSlicing()
        {
        }
        public DataSlicing(string date, string slicingName)
        {
            Date = date;
            SlicingName = slicingName;
        }
        public string Date { get; set; }
        public string SlicingName { get; set; }
    }

    public class DataPoint
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public DataPoint(DateTime date, double value)
        {
            this.Date = date;
            this.Value = value;
        }
        public static List<DataPoint> GetDataPoints()
        {
            List<DataPoint> data = new List<DataPoint> {
                new DataPoint(new DateTime(2019, 6, 1, 0, 0, 0), 56.1226),
                new DataPoint(new DateTime(2019, 6, 1, 3, 0, 0), 50.18432),
                new DataPoint(new DateTime(2019, 6, 1, 6, 0, 0), 51.51443),
                new DataPoint(new DateTime(2019, 6, 1, 9, 0, 0), 60.2624),
                new DataPoint(new DateTime(2019, 6, 1, 12, 0, 0), 64.04412),
                new DataPoint(new DateTime(2019, 6, 1, 15, 0, 0), 66.56123),
                new DataPoint(new DateTime(2019, 6, 1, 18, 0, 0), 65.48127),
                new DataPoint(new DateTime(2019, 6, 1, 21, 0, 0), 60.4412),
                new DataPoint(new DateTime(2019, 6, 2, 0, 0, 0), 57.2341),
                new DataPoint(new DateTime(2019, 6, 2, 3, 0, 0), 52.3469),
                new DataPoint(new DateTime(2019, 6, 2, 6, 0, 0), 51.82341),
                new DataPoint(new DateTime(2019, 6, 2, 9, 0, 0), 61.532),
                new DataPoint(new DateTime(2019, 6, 2, 12, 0, 0), 63.8641),
                new DataPoint(new DateTime(2019, 6, 2, 15, 0, 0), 65.12374),
                new DataPoint(new DateTime(2019, 6, 2, 18, 0, 0), 65.6321)};

            Random randomObj = new Random();
            int randValue = 0;

            for (int i = 1; i < 2022; i++)
            {
                randValue = randomObj.Next();

                data.Add(new DataPoint(new DateTime(i, 6, 1, 0, 0, 0), randValue));
            }

            return data;
        }
    }

}
