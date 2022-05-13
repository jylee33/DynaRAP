namespace DynaRAP.TEST
{
    partial class TestChartForm2
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
            this.dxChartControl1 = new DynaRAP.UControl.DXChartControl();
            this.SuspendLayout();
            // 
            // dxChartControl1
            // 
            this.dxChartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dxChartControl1.Location = new System.Drawing.Point(0, 0);
            this.dxChartControl1.Name = "dxChartControl1";
            this.dxChartControl1.Size = new System.Drawing.Size(800, 450);
            this.dxChartControl1.TabIndex = 0;
            // 
            // TestChartForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dxChartControl1);
            this.Name = "TestChartForm2";
            this.Text = "TestChartForm2";
            this.ResumeLayout(false);

        }

        #endregion

        private UControl.DXChartControl dxChartControl1;
    }
}