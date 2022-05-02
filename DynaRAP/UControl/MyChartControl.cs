using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace DynaRAP.UControl
{
    public delegate void SelectedRageEventHandler(object minValue, object maxValue);
    public partial class MyChartControl : UserControl
    {
        public event SelectedRageEventHandler OnSelectedRage;

        private object m_xaxisMinValue;
        private object m_xaxisMaxValue;
        private List<SeriesDataPoint> m_data;

        private int m_totalPages;
        private int m_pageIndex;

        private string m_filename;
        private List<string> m_series;


        private int m_pageSize;
        public int PageSize
        {
            get { return this.m_pageSize; }
            set
            {
                this.m_pageSize = value;
                this.txtPageSize.Text = this.m_pageSize.ToString();
            }
        }

        public MyChartControl()
        {
            InitializeComponent();

            this.m_xaxisMinValue = null;
            this.m_xaxisMaxValue = null;

            this.m_pageIndex = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.pnPaging.Visible = true;
        }

        private void rangeControl_RangeChanged(object sender, DevExpress.XtraEditors.RangeControlRangeEventArgs range)
        {
            m_xaxisMinValue = range.Range.Minimum;
            m_xaxisMaxValue = range.Range.Maximum;

            Debug.Print(string.Format("-----> minimum: {0}, maxmum: {1}", m_xaxisMinValue, m_xaxisMaxValue));
        }


        private void DrawChart()
        {
            if (this.chartControl.Series.Count > 0)
                this.chartControl.Series.Clear();

            var datas = m_data.Skip(m_pageSize * m_pageIndex).Take(this.m_pageSize);

            foreach (SeriesDataPoint data in datas)
            {
                if (this.chartControl.Series.Count == 0)
                    chartControl.Series.Add(new Series(data.SeriesName, ViewType.Line));
                chartControl.Series[0].Points.Add(new SeriesPoint(data.Argument, data.Value));
            }

            this.chartControl.Series[0].ArgumentScaleType = ScaleType.DateTime;
            this.chartControl.Series[0].ArgumentDataMember = "Argument";
            this.chartControl.Series[0].ValueScaleType = ScaleType.Numerical;
            this.chartControl.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });

            XYDiagram diagram = this.chartControl.Diagram as XYDiagram;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.DateTimeScaleOptions.GridOffset = 1;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
            diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffff}";

            this.rangeControl.Client = this.chartControl;
            diagram.RangeControlDateTimeGridOptions.GridMode = ChartRangeControlClientGridMode.Manual;
            diagram.RangeControlDateTimeGridOptions.GridOffset = 1;
            diagram.RangeControlDateTimeGridOptions.GridSpacing = 60;
            //diagram.RangeControlDateTimeGridOptions.SnapMode = ChartRangeControlClientSnapMode.Manual;
            //diagram.RangeControlDateTimeGridOptions.SnapSpacing = 1L;
            //diagram.RangeControlDateTimeGridOptions.LabelFormat = "{A:HH:mm:ss.ffff}";
            diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Millisecond;

            this.btnMoveFirst.Enabled = this.btnMoveLeft.Enabled = (this.m_pageIndex > 0);
            this.btnMoveLast.Enabled = this.btnMoveRight.Enabled = (this.m_pageIndex < this.m_totalPages);

            this.lblPages.Text = string.Format("{0} page of {1} pages.", this.m_pageIndex + 1, this.m_totalPages + 1);
        }

        private void fileOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files(*.*)|*.*|CSV Files|*.csv";
            if (DialogResult.OK == ofd.ShowDialog())
            {
                this.m_data = ReadData(ofd.FileName);

                this.m_totalPages = this.m_data.Count / this.m_pageSize;

                //this.pnPaging.Visible = (this.m_data.Count > this.m_pageSize);

                DrawChart();
            }
        }

        private void getXAxisRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != OnSelectedRage)
                OnSelectedRage(m_xaxisMinValue, m_xaxisMaxValue);
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            if (this.m_pageIndex < this.m_totalPages)
                this.m_pageIndex++;
            DrawChart();
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            if (this.m_pageIndex > 0)
                this.m_pageIndex--;
            DrawChart();
        }

        private void btnMoveLast_Click(object sender, EventArgs e)
        {
            this.m_pageIndex = this.m_totalPages;
            DrawChart();
        }

        private void btnMoveFirst_Click(object sender, EventArgs e)
        {
            this.m_pageIndex = 0;
            DrawChart();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPageSize.Text))
            {
                this.txtPageSize.Focus();
                return;
            }

            this.m_pageIndex = 0;
            this.m_pageSize = Convert.ToInt32(this.txtPageSize.Text);

            this.m_totalPages = this.m_data.Count / this.m_pageSize;

            //this.pnPaging.Visible = (this.m_data.Count > this.m_pageSize);

            DrawChart();
        }

        private void txtPageSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void cbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == m_data)
                return;

            m_data = ReadData(this.m_filename);
            DrawChart();
        }


        List<SeriesDataPoint> ReadData(string filename)
        {
            int index = 1;
            List<SeriesDataPoint> seriesList = new List<SeriesDataPoint>();
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs, Encoding.UTF8, false))
                    {
                        if (null == m_series)
                            m_series = new List<string>();

                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            var values = line.Split(',');

                            if (string.IsNullOrEmpty(values[0]))
                                continue;

                            if (values[0].Equals("DATE"))
                            {
                                if (!filename.Equals(m_filename))
                                {
                                    for (int i = 1; i < values.Length; i++)
                                    {
                                        if (!string.IsNullOrEmpty(values[i]))
                                        {
                                            m_series.Add(values[i]);
                                            this.cbSeries.Items.Add(values[i]);
                                        }
                                    }
                                    this.cbSeries.SelectedIndex = 0;
                                }
                                else
                                    index = m_series.FindIndex(f => f.Equals(this.cbSeries.Text));

                                continue;
                            }

                            string[] splits = values[0].Split(':');
                            var arg = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);

                            var argument = DateTime.ParseExact(arg, "yyyy-MM-dd HH:mm:ss.ffffff", null);
                            var value = double.Parse(values[index + 1]);

                            seriesList.Add(new SeriesDataPoint(m_series[index], argument, value));
                        }
                    }
                }

                m_filename = filename;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return seriesList;
        }
    }

    public class SeriesDataPoint
    {
        public string SeriesName { get; private set; }
        public DateTime Argument { get; private set; }
        public double Value { get; private set; }

        public SeriesDataPoint(string name, DateTime arg, double val)
        {
            SeriesName = name;
            Argument = arg;
            Value = val;
        }
    }
}
