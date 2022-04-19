namespace DynaRAP.TEST
{
    partial class TestFilterForm
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
            DevExpress.XtraCharts.XYDiagram xyDiagram3 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView3 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.XYDiagram xyDiagram4 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series4 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView4 = new DevExpress.XtraCharts.LineSeriesView();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.cboParameter = new DevExpress.XtraEditors.ComboBoxEdit();
            this.chartControl2 = new DevExpress.XtraCharts.ChartControl();
            this.btnFilter = new DevExpress.XtraEditors.SimpleButton();
            this.radioLPF = new System.Windows.Forms.RadioButton();
            this.radioHPF = new System.Windows.Forms.RadioButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.edtSampling = new DevExpress.XtraEditors.TextEdit();
            this.edtCutoff = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboParameter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtSampling.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtCutoff.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            xyDiagram3.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram3.AxisY.VisibleInPanesSerializable = "-1";
            this.chartControl1.Diagram = xyDiagram3;
            this.chartControl1.Location = new System.Drawing.Point(12, 45);
            this.chartControl1.Name = "chartControl1";
            series3.Name = "Series 1";
            series3.View = lineSeriesView3;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3};
            this.chartControl1.Size = new System.Drawing.Size(946, 220);
            this.chartControl1.TabIndex = 0;
            // 
            // cboParameter
            // 
            this.cboParameter.Location = new System.Drawing.Point(12, 12);
            this.cboParameter.Name = "cboParameter";
            this.cboParameter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboParameter.Properties.DropDownRows = 15;
            this.cboParameter.Size = new System.Drawing.Size(262, 20);
            this.cboParameter.TabIndex = 0;
            // 
            // chartControl2
            // 
            xyDiagram4.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram4.AxisY.VisibleInPanesSerializable = "-1";
            this.chartControl2.Diagram = xyDiagram4;
            this.chartControl2.Location = new System.Drawing.Point(12, 359);
            this.chartControl2.Name = "chartControl2";
            series4.Name = "Series 1";
            series4.View = lineSeriesView4;
            this.chartControl2.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series4};
            this.chartControl2.Size = new System.Drawing.Size(946, 200);
            this.chartControl2.TabIndex = 3;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(370, 306);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(383, 23);
            this.btnFilter.TabIndex = 5;
            this.btnFilter.Text = "Filter";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // radioLPF
            // 
            this.radioLPF.AutoSize = true;
            this.radioLPF.Checked = true;
            this.radioLPF.Location = new System.Drawing.Point(93, 311);
            this.radioLPF.Name = "radioLPF";
            this.radioLPF.Size = new System.Drawing.Size(44, 18);
            this.radioLPF.TabIndex = 3;
            this.radioLPF.TabStop = true;
            this.radioLPF.Text = "LPF";
            this.radioLPF.UseVisualStyleBackColor = true;
            // 
            // radioHPF
            // 
            this.radioHPF.AutoSize = true;
            this.radioHPF.Location = new System.Drawing.Point(255, 311);
            this.radioHPF.Name = "radioHPF";
            this.radioHPF.Size = new System.Drawing.Size(46, 18);
            this.radioHPF.TabIndex = 4;
            this.radioHPF.Text = "HPF";
            this.radioHPF.UseVisualStyleBackColor = true;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 283);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "sampling rate";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(229, 283);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(92, 14);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "cutoff frequency";
            // 
            // edtSampling
            // 
            this.edtSampling.EditValue = "1000";
            this.edtSampling.Location = new System.Drawing.Point(93, 280);
            this.edtSampling.Name = "edtSampling";
            this.edtSampling.Size = new System.Drawing.Size(100, 20);
            this.edtSampling.TabIndex = 1;
            // 
            // edtCutoff
            // 
            this.edtCutoff.EditValue = "10";
            this.edtCutoff.Location = new System.Drawing.Point(338, 280);
            this.edtCutoff.Name = "edtCutoff";
            this.edtCutoff.Size = new System.Drawing.Size(100, 20);
            this.edtCutoff.TabIndex = 2;
            // 
            // TestFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 580);
            this.Controls.Add(this.edtCutoff);
            this.Controls.Add(this.edtSampling);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.radioHPF);
            this.Controls.Add(this.radioLPF);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.chartControl2);
            this.Controls.Add(this.cboParameter);
            this.Controls.Add(this.chartControl1);
            this.Name = "TestFilterForm";
            this.Text = "TestFilterForm";
            this.Load += new System.EventHandler(this.TestFilterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboParameter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtSampling.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtCutoff.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cboParameter;
        private DevExpress.XtraCharts.ChartControl chartControl2;
        private DevExpress.XtraEditors.SimpleButton btnFilter;
        private System.Windows.Forms.RadioButton radioLPF;
        private System.Windows.Forms.RadioButton radioHPF;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit edtSampling;
        private DevExpress.XtraEditors.TextEdit edtCutoff;
    }
}