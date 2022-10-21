using DevExpress.XtraBars.Docking;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DynaRAP.Data;
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

namespace DynaRAP.UControl
{
    public partial class SBIntervalControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler ViewBtnClicked;
        SplittedSB sb;
        DataTable dt;

        DockPanel panelChart = null;
        ChartControl chartControl = null;

        public string Title
        {
            set
            {
                edtSbName.Text = value;
            }
        }

        public SplittedSB Sb
        {
            get { return this.sb; }
        }

        public DataTable Dt { get => dt; set => dt = value; }

        public SBIntervalControl()
        {
            InitializeComponent();
        }

        public SBIntervalControl(SplittedSB sb, DataTable dt = null) : this()
        {
            this.sb = sb;
            this.dt = dt;
        }

        private void SBIntervalControl_Load(object sender, EventArgs e)
        {
            edtSbName.Text = sb.SbName;
            edtStartTime.Text = sb.StartTime;
            edtEndTime.Text = sb.EndTime;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //if (this.ViewBtnClicked != null)
            //    this.ViewBtnClicked(this, new EventArgs());

            DateTime sTime = DateTime.Now;
            DateTime eTime = DateTime.Now; 
            if(this.sb.StartTime.Length < 8)
            {
                sTime = Utils.GetDateFromJulian(this.sb.StartTime);
                sTime = Utils.GetDateFromJulian(this.sb.EndTime);
            }
            else
                {
                sTime = DateTime.ParseExact(this.sb.StartTime, "yyyy-MM-dd HH:mm:ss.ffffff", null);
                    eTime = DateTime.Now; DateTime.ParseExact(this.sb.EndTime, "yyyy-MM-dd HH:mm:ss.ffffff", null);
                }
            DataTable dt2 = GetShortBlockData(sTime, eTime);

            if (dt2 != null)
            {
                MainForm mainForm = this.ParentForm as MainForm;

                if (chartControl != null)
                {
                    chartControl.Dispose();
                    chartControl = null;
                }

                chartControl = new ChartControl();

                chartControl.Series.Clear();

                DevExpress.XtraCharts.Series series = new DevExpress.XtraCharts.Series("Series1", ViewType.Line);
                chartControl.Series.Add(series);

                series.DataSource = dt2;

                series.ArgumentScaleType = ScaleType.DateTime;
                series.ArgumentDataMember = "Argument";
                series.ValueScaleType = ScaleType.Numerical;
                series.ValueDataMembers.AddRange(new string[] { "Value" });

                //((XYDiagram)chartControl.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
                chartControl.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                XYDiagram diagram = (XYDiagram)chartControl.Diagram;

                diagram.EnableAxisXScrolling = true;
                diagram.EnableAxisXZooming = true;
                diagram.EnableAxisYScrolling = true;
                diagram.EnableAxisYZooming = true;
                diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
                diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
                diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;
                diagram.AxisX.DateTimeScaleOptions.AutoGrid = false;
                diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
                diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.fff}";
                //diag.AxisX.Label.TextPattern = "{A:MMM-dd HH}";

                if (panelChart == null)
                {
                    panelChart = new DockPanel();
                    panelChart = mainForm.DockManager1.AddPanel(DockingStyle.Float);
                    panelChart.FloatLocation = new Point(500, 100);
                    panelChart.FloatSize = new Size(1058, 528);
                    panelChart.Name = this.sb.SbName;
                    panelChart.Text = this.sb.SbName;
                    chartControl.Dock = DockStyle.Fill;
                    panelChart.Controls.Add(chartControl);
                    panelChart.ClosedPanel += PanelChart_ClosedPanel;
                }
                else
                {
                    panelChart.Name = this.sb.SbName;
                    panelChart.Text = this.sb.SbName;
                    //panelChart.Controls.Clear();
                    chartControl.Dock = DockStyle.Fill;
                    panelChart.Controls.Add(chartControl);
                    panelChart.Show();
                    panelChart.Focus();
                }
            }

        }

        private void PanelChart_ClosedPanel(object sender, DockPanelEventArgs e)
        {
        }

        private DataTable GetShortBlockData(DateTime sTime, DateTime eTime)
        {
            //string t1 = Utils.GetJulianFromDate(sTime);
            //string t2 = Utils.GetJulianFromDate(eTime);

            DataRow[] result = this.dt.Select(String.Format("Argument >= #{0}# AND Argument <= #{1}#", sTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), eTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff")));

            DataTable table = new DataTable("Table1");
            table.Columns.Add("Argument", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));

            foreach (DataRow row in result)
            {
                table.ImportRow(row);
            }
            return table;

        }
    }
}
