using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
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
            chartOverview.BackColor = Color.FromArgb(45, 45, 48);
            chartOverview.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;

            chartOverview.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            XYDiagram diagram = chartOverview.Diagram as XYDiagram;
            AxisX axisX = diagram.AxisX;
            AxisY axisY = diagram.AxisY;
            axisX.Visibility = DevExpress.Utils.DefaultBoolean.False;
            axisY.Visibility = DevExpress.Utils.DefaultBoolean.False;

            // Create a line series, bind it to data and add to the chartControl1.
            Series series = new Series("Temperature", ViewType.Line);
            series.DataSource = OverviewDataPoint.GetDataPoints();
            series.ArgumentDataMember = "Date";
            series.ValueDataMembers.AddRange("Value");
            chartOverview.Series.Add(series);

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
            chartOverview.Titles.Add(chartTitle);

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

    

    

}
