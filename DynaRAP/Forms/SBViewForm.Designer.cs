namespace DynaRAP.Forms
{
    partial class SBViewForm
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
            this.dxChartControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dxChartControl1.Name = "dxChartControl1";
            this.dxChartControl1.Size = new System.Drawing.Size(793, 454);
            this.dxChartControl1.TabIndex = 0;
            // 
            // SBViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 454);
            this.Controls.Add(this.dxChartControl1);
            this.Name = "SBViewForm";
            this.Text = "SBViewForm";
            this.Load += new System.EventHandler(this.SBViewForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UControl.DXChartControl dxChartControl1;
    }
}