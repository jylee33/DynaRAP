namespace DynaRAP.TEST
{
    partial class TestChart
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
            this.myChartControl1 = new DynaRAP.UControl.MyChartControl();
            this.SuspendLayout();
            // 
            // myChartControl1
            // 
            this.myChartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myChartControl1.Location = new System.Drawing.Point(0, 0);
            this.myChartControl1.Name = "myChartControl1";
            this.myChartControl1.PageSize = 0;
            this.myChartControl1.Size = new System.Drawing.Size(800, 450);
            this.myChartControl1.TabIndex = 0;
            // 
            // TestChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.myChartControl1);
            this.Name = "TestChart";
            this.Text = "TestChart";
            this.ResumeLayout(false);

        }

        #endregion

        private UControl.MyChartControl myChartControl1;
    }
}