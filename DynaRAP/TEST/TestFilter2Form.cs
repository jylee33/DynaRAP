using MathNet.Filtering;
using MathNet.Filtering.FIR;
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
    public partial class TestFilter2Form : Form
    {
        public TestFilter2Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            double fs = 1000; //sampling rate 
            double fw = 5; //signal frequency 
            double fn = 50; //noise frequency 
            double n = 10; //number of periods to show 
            double A = 1; //signal amplitude 
            double N = 0.1; //noise amplitude 
            int size = (int)(n * fs / fw); //sample size 

            var t = Enumerable.Range(1, size).Select(p => p * 1 / fs).ToArray();
            var y = t.Select(p => (A * Math.Sin(2 * Math.PI * fw * p)) + (N * Math.Sin(2 * Math.PI * fn * p))).ToArray(); //Original 

            //lowpass filter 
            double fc = 10; //cutoff frequency 
            var lowpass = OnlineFirFilter.CreateLowpass(ImpulseResponse.Finite, fs, fc);

            //bandpass filter 
            double fc1 = 0; //low cutoff frequency 
            double fc2 = 10; //high cutoff frequency 
            var bandpass = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);

            //narrow bandpass filter 
            fc1 = 3; //low cutoff frequency 
            fc2 = 7; //high cutoff frequency 
            var bandpassnarrow = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);

            double[] yf1 = lowpass.ProcessSamples(y); //Lowpass 
            double[] yf2 = bandpass.ProcessSamples(y); //Bandpass 
            double[] yf3 = bandpassnarrow.ProcessSamples(y); //Bandpass Narrow 

            Series originalSeries = chart1.Series.Add("Original");
            originalSeries.Enabled = true;
            originalSeries.ChartType = SeriesChartType.Line;
            for (int i = 1; i < y.Length; i++)
                originalSeries.Points.AddXY(t[i], y[i]);

            Series lowpassSeries = chart1.Series.Add("Lowpass");
            lowpassSeries.ChartType = SeriesChartType.Line;
            lowpassSeries.Enabled = this.checkBoxLowpass.Checked;
            for (int i = 1; i < y.Length; i++)
                lowpassSeries.Points.AddXY(t[i], yf1[i]);

            Series bandpassSeries = chart1.Series.Add("Bandpass");
            bandpassSeries.ChartType = SeriesChartType.Line;
            bandpassSeries.Enabled = this.checkBoxBandpass.Checked;
            for (int i = 1; i < y.Length; i++)
                bandpassSeries.Points.AddXY(t[i], yf2[i]);
            
            Series bandpassNarrowSeries = chart1.Series.Add("Bandpass Narrow");
            bandpassNarrowSeries.ChartType = SeriesChartType.Line;
            bandpassNarrowSeries.Enabled = this.checkBoxBandpassNarrow.Checked;
            for (int i = 1; i < y.Length; i++)
                bandpassNarrowSeries.Points.AddXY(t[i], yf3[i]);
        }

        private void checkBoxLowPass_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series["Lowpass"].Enabled = checkBoxLowpass.Checked;
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void checkBoxBandpass_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series["Bandpass"].Enabled = checkBoxBandpass.Checked;
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void checkBoxBandpassNarrow_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series["Bandpass Narrow"].Enabled = checkBoxBandpassNarrow.Checked;
            chart1.ChartAreas[0].RecalculateAxesScale();
        }
    }
}
