using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DynaRAP.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.Forms
{
    public partial class SBViewForm : DevExpress.XtraEditors.XtraForm
    {
        private ChartControl m_chart;
        private List<string> m_series;
        private int m_pageIndex;
        private int m_pageSize;
        private int m_totalPages;
        private DataTable m_table;
        private DrawTypes m_drawTypes;
        private List<DLL_DATA> m_dllDatas;
        private DataTable dt = null;

        public SBViewForm()
        {
            InitializeComponent();
        }

        public SBViewForm(DataTable dt) : this()
        {
            this.dt = dt;
        }

        private void SBViewForm_Load(object sender, EventArgs e)
        {
            m_series = new List<string>();
            m_pageIndex = 0;
            m_pageSize = 100000;
            m_totalPages = 0;

            this.m_chart = new ChartControl();
            this.Controls.Add(this.m_chart);
            this.m_chart.Dock = DockStyle.Fill;

            m_drawTypes = DrawTypes.DT_1D;

            m_dllDatas = new List<DLL_DATA>();
            SetDllDatas();

            DrawChart(dt);
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

        }

        public void DrawChart(DataTable table, string seriesName = "Series1")
        {
            m_table = table;
            if (this.m_chart.Series.Count > 0)
                this.m_chart.Series.Clear();
            this.m_chart.Series.Add(new Series(seriesName, ViewType.Line));
            DrawChart_1D();
        }

        public void DrawChart(DrawTypes type, string seriesStr, string chartTitle, List<SeriesPointData> points, string titleX = "", string titleY = "", int pageIndex = 0, int pageSize = 50000)
        {
            if (this.m_chart.Series.Count > 0)
                this.m_chart.Series.Clear();

            DrawChart(MakeTableData(points));

            return;

            switch (type)
            {
                case DrawTypes.DT_1D:
                    {
                        this.m_chart.Series.Add(new Series(seriesStr, ViewType.Line));

                        DrawChart_1D();
                    }
                    break;

                case DrawTypes.DT_2D:
                    {
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
            this.m_chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

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


    }
}