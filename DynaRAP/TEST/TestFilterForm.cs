using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DynaRAP.Data;
using DynaRAP.UTIL;
using MathNet.Filtering;
using MathNet.Filtering.FIR;
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

namespace DynaRAP.TEST
{
    public partial class TestFilterForm : DevExpress.XtraEditors.XtraForm
    {
        Dictionary<string, List<string>> dicData = new Dictionary<string, List<string>>();
        string selKey = String.Empty;
        List<double> listData = new List<double>();
        
        double fs = 1000; //sampling rate
        double fc = 10; //cutoff frequency

        public TestFilterForm()
        {
            InitializeComponent();
        }

        private void TestFilterForm_Load(object sender, EventArgs e)
        {
            ReadCsvFile();
            InitComboList();
            InitChart();
        }

        private void ReadCsvFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "C:\\";
            dlg.Filter = "Excel files (*.xls, *.xlsx)|*.xls; *.xlsx|Comma Separated Value files (CSV)|*.csv|모든 파일 (*.*)|*.*";
            //dlg.Filter = "Comma Separated Value files (CSV)|*.csv";

#if DEBUG
#else
            if (dlg.ShowDialog() == DialogResult.OK)
#endif
            {
#if DEBUG
                StreamReader sr = new StreamReader(@"C:\32063_20220314_180351_SL30_02_2nd_ALCM_FT_Full_Sample_1.xls");
#else
                StreamReader sr = new StreamReader(dlg.FileName);
#endif

                int idx = 0;

                // 스트림의 끝까지 읽기
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] data = line.Split(',');

                    if (string.IsNullOrEmpty(data[0]))
                        continue;

                    int i = 0;
                    if (idx == 0)
                    {
                        dicData.Clear();
                        for (i = 0; i < data.Length; i++)
                        {
                            if (dicData.ContainsKey(data[i]) == false)
                            {
                                if (string.IsNullOrEmpty(data[i]) == false)
                                    dicData.Add(data[i], new List<string>());
                            }
                        }
                        idx++;
                        continue;
                    }

                    i = 0;
                    foreach (string key in dicData.Keys)
                    {
                        if (dicData.ContainsKey(key))
                        {
                            if (string.IsNullOrEmpty(data[i]) == false)
                                dicData[key].Add(data[i++]);
                        }
                    }
                }


            }
        }

        private void InitComboList()
        {
            cboParameter.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboParameter.SelectedIndexChanged += cboParameter_SelectedIndexChanged;

            List<string> paramList = dicData.Keys.ToList();
            cboParameter.Properties.Items.AddRange(paramList);
            cboParameter.Properties.Items.Remove("DATE");

            cboParameter.SelectedIndex = 0;
        }

        private void InitChart()
        {
            //chartControl1.BackColor = Color.FromArgb(45, 45, 48);
            //chartControl1.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;

            //chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            //XYDiagram diagram = chartControl1.Diagram as XYDiagram;
            //AxisX axisX = diagram.AxisX;
            //AxisY axisY = diagram.AxisY;
            //axisX.Visibility = DevExpress.Utils.DefaultBoolean.False;
            //axisY.Visibility = DevExpress.Utils.DefaultBoolean.False;

            //// Customize axes.
            //diagram.AxisX.Label.TextPattern = "{A:MMM, d (HH:mm)}";
            //diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
            //diagram.AxisX.DateTimeScaleOptions.GridSpacing = 9;
            //diagram.AxisX.WholeRange.SideMarginsValue = 0.5;
            //diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;


        }

        private void cboParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboParameter.EditValue != null)
            {
                selKey = cboParameter.EditValue.ToString();
                AddChartData(selKey);
            }
        }

        private void AddChartData(string strKey)
        {
            chartControl1.Series.Clear();

            Series series = new Series("Series1", ViewType.Line);
            chartControl1.Series.Add(series);

            //series.DataSource = CreateChartData(50);
            series.DataSource = GetChartValues(strKey);

            series.ArgumentScaleType = ScaleType.Numerical;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            ((XYDiagram)chartControl1.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

        }

        private DataTable GetChartValues(string strKey)
        {
            // Create an empty table.
            DataTable table = new DataTable("Table1");

            // Add two columns to the table.
            table.Columns.Add("Argument", typeof(Int32));
            table.Columns.Add("Value", typeof(double));

            DataRow row = null;
            int i = 0;
            listData.Clear();
            foreach (string value in dicData[selKey])
            {
                row = table.NewRow();
                string day = dicData["DATE"][i];
                DateTime dt = Utils.GetDateFromJulian(day);
                double data = double.Parse(value);
                listData.Add(data);
                row["Argument"] = i;
                row["Value"] = data;
                table.Rows.Add(row);
                i++;
            }

            return table;
        }

        private DataTable CreateChartData(int rowCount)
        {
            // Create an empty table.
            DataTable table = new DataTable("Table1");

            // Add two columns to the table.
            table.Columns.Add("Argument", typeof(Int32));
            table.Columns.Add("Value", typeof(Int32));

            // Add data rows to the table.
            Random rnd = new Random();
            DataRow row = null;
            for (int i = 0; i < rowCount; i++)
            {
                row = table.NewRow();
                row["Argument"] = i;
                row["Value"] = rnd.Next(100);
                table.Rows.Add(row);
            }

            return table;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(edtSampling.Text, out fs))
            {
            }
            else
            {
                MessageBox.Show("Sampling Rate 비정상");
            }

            if (Double.TryParse(edtCutoff.Text, out fc))
            {
            }
            else
            {
                MessageBox.Show("Cutoff Frequency 비정상");
            }

            if (radioLPF.Checked)
            {
                var lowpass = OnlineFirFilter.CreateLowpass(ImpulseResponse.Finite, fs, fc);
                double[] lpf = lowpass.ProcessSamples(listData.ToArray());
                AddChart2Data(lpf);
            }
            else
            {
                var highpass = OnlineFirFilter.CreateHighpass(ImpulseResponse.Finite, fs, fc);
                double[] hpf = highpass.ProcessSamples(listData.ToArray());
                AddChart2Data(hpf);
            }
        }

        private void AddChart2Data(double[] lpf)
        {
            chartControl2.Series.Clear();

            Series series = new Series("Series1", ViewType.Line);
            chartControl2.Series.Add(series);

            //series.DataSource = CreateChartData(50);
            series.DataSource = GetChart2Values(lpf);

            series.ArgumentScaleType = ScaleType.Numerical;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            ((XYDiagram)chartControl2.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chartControl2.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

        }

        private DataTable GetChart2Values(double[] lpf)
        {
            // Create an empty table.
            DataTable table = new DataTable("Table1");

            // Add two columns to the table.
            table.Columns.Add("Argument", typeof(Int32));
            table.Columns.Add("Value", typeof(double));

            DataRow row = null;
            int i = 0;
            listData.Clear();
            for(i = 0; i< lpf.Length; i++)
            {
                row = table.NewRow();
                //string day = dicData["DATE"][i];
                //DateTime dt = Utils.GetDateFromJulian(day);
                row["Argument"] = i;
                row["Value"] = lpf[i];
                table.Rows.Add(row);
            }

            return table;
        }
    }
}