namespace DynaRAP.UControl
{
    partial class PlotModuleControl
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddPlot = new DevExpress.XtraEditors.SimpleButton();
            this.btnPlotSave = new DevExpress.XtraEditors.SimpleButton();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnAddPlot);
            this.flowLayoutPanel1.Controls.Add(this.btnPlotSave);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(908, 42);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnAddPlot
            // 
            this.btnAddPlot.Location = new System.Drawing.Point(3, 3);
            this.btnAddPlot.Name = "btnAddPlot";
            this.btnAddPlot.Size = new System.Drawing.Size(96, 30);
            this.btnAddPlot.TabIndex = 55;
            this.btnAddPlot.Text = "PLOT추가";
            this.btnAddPlot.Click += new System.EventHandler(this.btnAddPlot_Click);
            // 
            // btnPlotSave
            // 
            this.btnPlotSave.Location = new System.Drawing.Point(105, 3);
            this.btnPlotSave.Name = "btnPlotSave";
            this.btnPlotSave.Size = new System.Drawing.Size(96, 30);
            this.btnPlotSave.TabIndex = 56;
            this.btnPlotSave.Text = "PLOT저장";
            this.btnPlotSave.Click += new System.EventHandler(this.btnPlotSave_Click);
            // 
            // PlotModuleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "PlotModuleControl";
            this.Size = new System.Drawing.Size(908, 752);
            this.Load += new System.EventHandler(this.PlotModuleControl_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnAddPlot;
        private DevExpress.XtraEditors.SimpleButton btnPlotSave;
    }
}
