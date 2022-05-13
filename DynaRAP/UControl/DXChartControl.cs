using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraCharts;

namespace DynaRAP.UControl
{
    public partial class DXChartControl : UserControl
    {
        public enum DrawTypes
        {
            DT_1D, DT_2D, DT_MINMAX, DT_POTATO
        }

        private ChartControl m_chart;
        private List<string> m_series;
        private string m_filename;
        private int m_pageIndex;
        private int m_pageSize;
        private int m_totalPages;
        private DataTable m_table;
        private DrawTypes m_drawTypes;
        private List<DLL_DATA> m_dllDatas;


        public DXChartControl()
        {
            InitializeComponent();

            m_series = new List<string>();
            m_filename = @"C:\temp\a.xls";
            m_pageIndex = 0;
            m_pageSize = 100000;
            m_totalPages = 0;

            this.m_chart = new ChartControl();
            this.Controls.Add(this.m_chart);
            this.m_chart.Dock = DockStyle.Fill;
            this.m_chart.ContextMenuStrip = this.contextMenuStrip;

            m_drawTypes = DrawTypes.DT_1D;
            mnuDrawChart1D.Checked = true;

            m_dllDatas = new List<DLL_DATA>();
            SetDllDatas();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void SetDllDatas()
        {
            var data = new DLL_DATA();
            data.Name = "LH Wing BL1870";
            data.ButtockLine = "1870";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW901P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW903P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW905P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            data.Name = "LH Wing BL2955";
            data.ButtockLine = "2955";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW907P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW909P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW911P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            data.Name = "LH Wing BL3405";
            data.ButtockLine = "3405";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW913P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW915P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW917P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            data.Name = "LH Wing BL4385";
            data.ButtockLine = "4385";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW919P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW921P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW923P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            data.Name = "LH Wing BL5160";
            data.ButtockLine = "5160";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW925P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW927P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW929P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            //ReadDataTable(m_filename);
        }

        public void DrawChart(DataTable table, string seriesName = "Series1")
        {
            m_table = table;
            this.pnPaging.Visible = true;
            if (this.m_chart.Series.Count > 0)
                this.m_chart.Series.Clear();
            this.m_chart.Series.Add(new Series(seriesName, ViewType.Line));
            DrawChart_1D();
        }

        public void DrawChart(DrawTypes type, string seriesStr, string chartTitle, List<SeriesPointData> points, string titleX = "", string titleY = "", int pageIndex = 0, int pageSize = 50000)
        {
            this.pnPaging.Visible = false;

            if (this.m_chart.Series.Count > 0)
                this.m_chart.Series.Clear();

            DrawChart(MakeTableData(points));

            return;

            switch (type)
            {
                case DrawTypes.DT_1D:
                    {
                        this.pnPaging.Visible = true;
                        this.m_chart.Series.Add(new Series(seriesStr, ViewType.Line));

                        DrawChart_1D();
                    }
                    break;

                case DrawTypes.DT_2D:
                    {
                        this.pnPaging.Visible = true;
                        this.m_chart.Series.Add(new Series(seriesStr, ViewType.Line));

                        DrawChart_2D();
                    }
                    break;
            }
        }

        private DataTable MakeTableData(List<SeriesPointData> points)
        {
            if (null == m_table)
                m_table = new DataTable();

            m_table.Columns.Clear();
            m_table.Rows.Clear();

            int index = 0;

            foreach (SeriesPointData point in points)
            {
                if (0 == index)
                {
                    m_table.Columns.Add("Argument", typeof(DateTime));
                    m_table.Columns.Add("Value", typeof(double));
                }

                DataRow row = m_table.NewRow();
                row["Argument"] = point.Argument;
                row["Value"] = point.Value;
                m_table.Rows.Add(row);

                index++;
            }

            return m_table;
        }

        private void DrawChart_1D(string axisTitleX = "", int pageIndex = 0, int pageSize = 50000)
        {
            var dataSource = this.m_table.AsEnumerable().Skip(m_pageSize * m_pageIndex).Take(this.m_pageSize);

            foreach (DataRow row in dataSource)
                this.m_chart.Series[0].Points.Add(new SeriesPoint(row["Argument"], row["Value"]));

            this.m_chart.Series[0].ArgumentScaleType = ScaleType.DateTime;
            this.m_chart.Series[0].ArgumentDataMember = "Argument";
            this.m_chart.Series[0].ValueScaleType = ScaleType.Numerical;
            this.m_chart.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });

            this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

            diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.WholeRange.SideMarginsValue = 0L;

            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.DateTimeScaleOptions.GridOffset = 1;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
            diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffff}";

            this.btnMoveFirst.Enabled = this.btnMoveLeft.Enabled = (this.m_pageIndex > 0);
            this.btnMoveLast.Enabled = this.btnMoveRight.Enabled = (this.m_pageIndex < this.m_totalPages);

            this.lblPages.Text = string.Format("{0} page of {1} pages.", this.m_pageIndex + 1, this.m_totalPages + 1);

            m_pageIndex = pageIndex;
            m_pageSize = pageSize;
        }

        private void DrawChart_2D(string axisTitleX = "", string axisTitleY = "", int pageIndex = 0, int pageSize = 50000)
        {
            var dataSource = this.m_table.AsEnumerable().Skip(m_pageSize * m_pageIndex).Take(this.m_pageSize);

            foreach (DataRow row in dataSource)
                this.m_chart.Series[0].Points.Add(new SeriesPoint(row["Argument"], row["Value"]));

            this.m_chart.Series[0].ArgumentScaleType = ScaleType.DateTime;
            this.m_chart.Series[0].ArgumentDataMember = "Argument";
            this.m_chart.Series[0].ValueScaleType = ScaleType.Numerical;
            this.m_chart.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });

            this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

            diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.Default;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.WholeRange.SideMarginsValue = 0L;

            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.DateTimeScaleOptions.GridOffset = 1;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
            diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffff}";

            this.btnMoveFirst.Enabled = this.btnMoveLeft.Enabled = (this.m_pageIndex > 0);
            this.btnMoveLast.Enabled = this.btnMoveRight.Enabled = (this.m_pageIndex < this.m_totalPages);

            this.lblPages.Text = string.Format("{0} page of {1} pages.", this.m_pageIndex + 1, this.m_totalPages + 1);

            m_pageIndex = pageIndex;
            m_pageSize = pageSize;
        }

        private void DrawChart_MinMax(string title, DataTable data, string axisTitleX = "", string axisTitleY = "")
        {
        }

        private void DrawChart_Potato(string title, DataTable data, string axisTitleX = "", string axisTitleY = "")
        {

        }

        private void InitPropertyControl()
        {
            Splitter splitter = new Splitter();
            splitter.Width = 10;
            splitter.Dock = DockStyle.Right;

            this.Controls.Add(splitter);

            PropertyGrid propertyGrid = new PropertyGrid();
            propertyGrid.SelectedObject = this.m_chart;
            propertyGrid.Dock = DockStyle.Right;
            propertyGrid.Width = 300;

            this.Controls.Add(propertyGrid);

            this.m_chart.Dock = DockStyle.Fill;

            this.Controls.Add(this.m_chart);

            this.m_chart.BringToFront();
        }


        private void ReadDataTable(string filename)
        {
            int index = 1;

            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs, Encoding.UTF8, false))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            var values = line.Split(',');

                            if (string.IsNullOrEmpty(values[0]))
                                continue;

                            if (values[0].Equals("DATE"))
                            {
                                for (int i = 1; i < values.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(values[i]))
                                    {
                                        if (null == m_series)
                                            m_series = new List<string>();

                                        m_series.Add(values[i]);
                                    }
                                }

                                continue;
                            }


                            foreach (DLL_DATA dll in m_dllDatas)
                            {
                                foreach (DLL_DATA.Parameter param in dll.parameters)
                                {
                                    if (null == param.data)
                                    {
                                        param.data = new DataTable();
                                        param.data.Columns.Add("Series", typeof(string));
                                        param.data.Columns.Add("Argument", typeof(DateTime));
                                        param.data.Columns.Add("Value", typeof(double));
                                    }

                                    index = m_series.FindIndex(f => f.Contains(param.Name));

                                    string[] splits = values[0].Split(':');
                                    var arg = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);

                                    var argument = DateTime.ParseExact(arg, "yyyy-MM-dd HH:mm:ss.ffffff", null);
                                    var value = double.Parse(values[index + 1]);

                                    DataRow row = param.data.NewRow();
                                    row["Series"] = param.Name;
                                    row["Argument"] = argument;
                                    row["Value"] = value;
                                    param.data.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private List<SeriesPointData> ReadDataList(string filename, bool isFirst = false)
        {
            int index = 1;
            List<SeriesPointData> data = new List<SeriesPointData>();
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
                                if (isFirst)
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
                                    index = m_series.FindIndex(f => f.Contains(this.cbSeries.Text));

                                continue;
                            }

                            if (!isFirst)
                            {
                                string[] splits = values[0].Split(':');
                                var arg = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);
                                var argument = DateTime.ParseExact(arg, "yyyy-MM-dd HH:mm:ss.ffffff", null);
                                var value = double.Parse(values[index + 1]);
                                data.Add(new SeriesPointData(this.cbSeries.Text, argument, value));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return data;
        }

        private void mnuDrawChart1D_Click(object sender, EventArgs e)
        {
            this.m_drawTypes = DrawTypes.DT_1D;
            mnuDrawChart1D.Checked = true;
            mnuDrawChart2D.Checked = false;
            DrawChart(this.m_drawTypes, this.cbSeries.Text, this.cbSeries.Text, ReadDataList(m_filename), "", "", 0, 50000);
            //this.cbSeries.Enabled = false;
            //ReadDataList(m_filename, true);
            //this.cbSeries.Enabled = true;
        }

        private void cbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbSeries.Enabled == false)
                return;

            DrawChart(this.m_drawTypes, this.cbSeries.Text, this.cbSeries.Text, ReadDataList(m_filename), "", "", 0, 50000);
        }

        private void mnuDrawChart2D_Click(object sender, EventArgs e)
        {
            this.m_drawTypes = DrawTypes.DT_2D;
            mnuDrawChart1D.Checked = false;
            mnuDrawChart2D.Checked = true;
            DrawChart(this.m_drawTypes, this.cbSeries.Text, this.cbSeries.Text, ReadDataList(m_filename), "", "", 0, 50000);
            //this.cbSeries.Enabled = false;            
            //ReadDataList(m_filename, true);
            //this.cbSeries.Enabled = true;
        }

        private void mnuFileRead_Click(object sender, EventArgs e)
        {
            this.cbSeries.Enabled = false;
            ReadDataList(m_filename, true);
            this.cbSeries.Enabled = true;

            if (this.cbSeries.Items.Count > 0)
            {
                mnuDrawChart1D.Enabled = true;
                mnuDrawChart2D.Enabled = true;
            }
            else
            {
                mnuDrawChart1D.Enabled = false;
                mnuDrawChart2D.Enabled = false;
            }
        }
    }

    #region 1D SeriesPoint Data
    public class SeriesPointData
    {
        public string SeriesName { get; private set; }
        public DateTime Argument { get; private set; }
        public double Value { get; private set; }

        public SeriesPointData(string name, DateTime arg, double val)
        {
            SeriesName = name;
            Argument = arg;
            Value = val;
        }
    }
    #endregion

    #region For Potato
    public class DLL_DATA
    {
        public string Name { get; set; }
        public string ButtockLine { get; set; }
        public List<Parameter> parameters { get; set; }
        public class Parameter
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Unit { get; set; }
            public DataTable data { get; set; }
        }
    }
    #endregion
}
