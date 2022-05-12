using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraCharts;

namespace DynaRAP.UControl
{
    public partial class MyLineChart : UserControl
    {
        public MyLineChart()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.splitContainer1.SplitterDistance = this.Width - 200;

            DrawChart(null);
        }

        private DataTable CreateData()
        {
            string[,] data = new string[,]
            {
                { "Minimum" , "1870", "-160867" },
                { "Minimum" , "2955", "-101472" },
                { "Minimum" , "3405", "-82136" },
                { "Minimum" , "4385", "-47617" },
                { "Minimum" , "5160" , "-38" },
                { "Maximum" , "1870", "484452" },
                { "Maximum" , "2955", "315471" },
                { "Maximum" , "3405", "243369" },
                { "Maximum" , "4385", "115840" },
                { "Maximum" , "5160" , "33739" },
            };

            DataTable table = new DataTable();
            table.Columns.Add("Operation", typeof(string));
            table.Columns.Add("Argument", typeof(double));
            table.Columns.Add("Value", typeof(double));

            for (int i = 0; i < data.GetLength(0); i++)
            {
                DataRow row = table.NewRow();
                row["Operation"] = data[i, 0];
                row["Argument"] = double.Parse(data[i, 1]);
                row["Value"] = double.Parse(data[i, 2]);
                table.Rows.Add(row);
            }

            return table;
        }

        public void DrawChart(DataTable data)
        {
#if DEBUG
            data = CreateData();
#endif

            Dictionary<string, Series> seriesInfo = new Dictionary<string, Series>();
            Series series;
            foreach (DataRow row in data.Rows)
            {
                string operation = row["Operation"].ToString();
                if (seriesInfo.TryGetValue(operation, out series) == false)
                {
                    series = new Series(operation, ViewType.Spline);
                    seriesInfo.Add(operation, series);

                    series.ArgumentDataMember = "Argument";
                    series.ArgumentScaleType = ScaleType.Numerical;
                    series.ValueDataMembers.AddRange(new string[] { "Value" });
                    series.ValueScaleType = ScaleType.Numerical;

                    this.chartControl.Series.Add(series);
                }

                double parameter = double.Parse(row["Argument"].ToString());
                double minmax = double.Parse(row["Value"].ToString());
                SeriesPoint point = new SeriesPoint(parameter, minmax);
                series.Points.Add(point);
            }

            XYDiagram diagram = this.chartControl.Diagram as XYDiagram;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Automatic;

            this.propertyGrid.SelectedObject = new ChartOption(this.chartControl);
        }
    }

    public class ChartOption
    {
        private ChartControl m_chart;
        public ChartOption(ChartControl chart)
        {
            this.m_chart = chart;
        }

        #region Properties
        [DisplayName("Legend Visibillity")]
        [Category("Chart Option")]
        [Description("Description of Legend Visibillity")]
        [DefaultValue(DevExpress.Utils.DefaultBoolean.Default)]
        public DevExpress.Utils.DefaultBoolean LegendV
        {
            get
            {
                return this.m_chart.Legend.Visibility;
            }
            set
            {
                this.m_chart.Legend.Visibility = value;
            }
        }

        [DisplayName("EnableAxisXScrolling")]
        [Category("Chart Option")]
        [Description("Description of EnableAxisXScrolling")]
        [DefaultValue(false)]
        public bool EnableAxisXScrolling
        {
            get
            {
                return ((XYDiagram)this.m_chart.Diagram).EnableAxisXScrolling;
            }
            set
            {
                ((XYDiagram)this.m_chart.Diagram).EnableAxisXScrolling = value;
            }
        }

        [DisplayName("EnableAxisXZooming")]
        [Category("Chart Option")]
        [Description("Description of EnableAxisXZooming")]
        [DefaultValue(false)]
        public bool EnableAxisXZooming
        {
            get
            {
                return ((XYDiagram)this.m_chart.Diagram).EnableAxisXZooming;
            }
            set
            {
                ((XYDiagram)this.m_chart.Diagram).EnableAxisXZooming = value;
            }
        }

        [DisplayName("AxisX.NumericScaleOptions.GridSpacing")]
        [Category("Chart Option")]
        [Description("Description of AxisX.NumericScaleOptions.GridSpacing")]
        [DefaultValue(500)]
        public double AxisXGridSpacing
        {
            get
            {
                return ((XYDiagram)this.m_chart.Diagram).AxisX.NumericScaleOptions.GridSpacing;
            }
            set
            {
                ((XYDiagram)this.m_chart.Diagram).AxisX.NumericScaleOptions.GridSpacing = value;
            }
        }

        [DisplayName("AxisX.NumericScaleOptions.GridOffset")]
        [Category("Chart Option")]
        [Description("Description of AxisX.NumericScaleOptions.GridOffset")]
        [DefaultValue(500)]
        public double AxisXGridOffset
        {
            get
            {
                return ((XYDiagram)this.m_chart.Diagram).AxisX.NumericScaleOptions.GridOffset;
            }
            set
            {
                ((XYDiagram)this.m_chart.Diagram).AxisX.NumericScaleOptions.GridOffset = value;
            }
        }

        [DisplayName("AxisY.NumericScaleOptions.GridSpacing")]
        [Category("Chart Option")]
        [Description("Description of AxisY.NumericScaleOptions.GridSpacing")]
        [DefaultValue(500)]
        public double AxisYGridSpacing
        {
            get
            {
                return ((XYDiagram)this.m_chart.Diagram).AxisY.NumericScaleOptions.GridSpacing;
            }
            set
            {
                ((XYDiagram)this.m_chart.Diagram).AxisY.NumericScaleOptions.GridSpacing = value;
            }
        }

        [DisplayName("AxisY.NumericScaleOptions.GridOffset")]
        [Category("Chart Option")]
        [Description("Description of AxisY.NumericScaleOptions.GridOffset")]
        [DefaultValue(500)]
        public double AxisYGridOffset
        {
            get
            {
                return ((XYDiagram)this.m_chart.Diagram).AxisY.NumericScaleOptions.GridOffset;
            }
            set
            {
                ((XYDiagram)this.m_chart.Diagram).AxisY.NumericScaleOptions.GridOffset = value;
            }
        }

        [DisplayName("Series")]
        [Category("Chart Option")]
        [Description("Description of Series")]
        public SeriesCollection Series
        {
            get
            {
                return this.m_chart.Series;
            }
            set
            {
            }
        }
        #endregion
    }
}
