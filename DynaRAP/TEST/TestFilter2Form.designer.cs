namespace DynaRAP.TEST
{
    partial class TestFilter2Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBoxLowpass = new System.Windows.Forms.CheckBox();
            this.checkBoxBandpass = new System.Windows.Forms.CheckBox();
            this.checkBoxBandpassNarrow = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(12, 35);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(776, 403);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // checkBoxLowpass
            // 
            this.checkBoxLowpass.AutoSize = true;
            this.checkBoxLowpass.Location = new System.Drawing.Point(12, 12);
            this.checkBoxLowpass.Name = "checkBoxLowpass";
            this.checkBoxLowpass.Size = new System.Drawing.Size(68, 17);
            this.checkBoxLowpass.TabIndex = 1;
            this.checkBoxLowpass.Text = "Lowpass";
            this.checkBoxLowpass.UseVisualStyleBackColor = true;
            this.checkBoxLowpass.CheckedChanged += new System.EventHandler(this.checkBoxLowPass_CheckedChanged);
            // 
            // checkBoxBandpass
            // 
            this.checkBoxBandpass.AutoSize = true;
            this.checkBoxBandpass.Location = new System.Drawing.Point(100, 12);
            this.checkBoxBandpass.Name = "checkBoxBandpass";
            this.checkBoxBandpass.Size = new System.Drawing.Size(73, 17);
            this.checkBoxBandpass.TabIndex = 2;
            this.checkBoxBandpass.Text = "Bandpass";
            this.checkBoxBandpass.UseVisualStyleBackColor = true;
            this.checkBoxBandpass.CheckedChanged += new System.EventHandler(this.checkBoxBandpass_CheckedChanged);
            // 
            // checkBoxBandpassNarrow
            // 
            this.checkBoxBandpassNarrow.AutoSize = true;
            this.checkBoxBandpassNarrow.Location = new System.Drawing.Point(183, 12);
            this.checkBoxBandpassNarrow.Name = "checkBoxBandpassNarrow";
            this.checkBoxBandpassNarrow.Size = new System.Drawing.Size(110, 17);
            this.checkBoxBandpassNarrow.TabIndex = 3;
            this.checkBoxBandpassNarrow.Text = "Bandpass Narrow";
            this.checkBoxBandpassNarrow.UseVisualStyleBackColor = true;
            this.checkBoxBandpassNarrow.CheckedChanged += new System.EventHandler(this.checkBoxBandpassNarrow_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.checkBoxBandpassNarrow);
            this.Controls.Add(this.checkBoxBandpass);
            this.Controls.Add(this.checkBoxLowpass);
            this.Controls.Add(this.chart1);
            this.Name = "Form1";
            this.Text = "Simple testing [mathnet-filtering]";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox checkBoxLowpass;
        private System.Windows.Forms.CheckBox checkBoxBandpass;
        private System.Windows.Forms.CheckBox checkBoxBandpassNarrow;
    }
}

