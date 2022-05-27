namespace DynaRAP.UControl
{
    partial class MgmtLRPExtraControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MgmtLRPExtraControl));
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.edtKey = new DevExpress.XtraEditors.TextEdit();
            this.edtValue = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.edtKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.ImageOptions.Image")));
            this.btnDelete.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDelete.Location = new System.Drawing.Point(524, 1);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(17, 18);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.ToolTip = "삭제";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // edtKey
            // 
            this.edtKey.Location = new System.Drawing.Point(37, 0);
            this.edtKey.Name = "edtKey";
            this.edtKey.Size = new System.Drawing.Size(186, 22);
            this.edtKey.TabIndex = 3;
            // 
            // edtValue
            // 
            this.edtValue.Location = new System.Drawing.Point(308, 0);
            this.edtValue.Name = "edtValue";
            this.edtValue.Size = new System.Drawing.Size(186, 22);
            this.edtValue.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(4, 1);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(23, 15);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "KEY";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(261, 3);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(38, 15);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "VALUE";
            // 
            // MgmtLRPExtraControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.edtValue);
            this.Controls.Add(this.edtKey);
            this.Controls.Add(this.btnDelete);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MgmtLRPExtraControl";
            this.Size = new System.Drawing.Size(558, 22);
            this.Load += new System.EventHandler(this.MgmtLRPExtraControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edtKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.TextEdit edtKey;
        private DevExpress.XtraEditors.TextEdit edtValue;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
