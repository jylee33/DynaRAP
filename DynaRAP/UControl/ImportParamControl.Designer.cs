namespace DynaRAP.UControl
{
    partial class ImportParamControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.cboParameter = new DevExpress.XtraEditors.ComboBoxEdit();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cboParameter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboParameter
            // 
            this.cboParameter.Location = new System.Drawing.Point(5, 1);
            this.cboParameter.Name = "cboParameter";
            this.cboParameter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboParameter.Size = new System.Drawing.Size(262, 20);
            this.cboParameter.TabIndex = 1;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(5, 28);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(523, 91);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(290, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "labelControl1";
            // 
            // ImportParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.cboParameter);
            this.Name = "ImportParamControl";
            this.Size = new System.Drawing.Size(533, 124);
            this.Load += new System.EventHandler(this.ImportParamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cboParameter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cboParameter;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
