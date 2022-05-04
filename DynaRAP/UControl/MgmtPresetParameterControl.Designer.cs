namespace DynaRAP.UControl
{
    partial class MgmtPresetParameterControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MgmtPresetParameterControl));
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.cboParamType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.edtX = new DevExpress.XtraEditors.TextEdit();
            this.edtY = new DevExpress.XtraEditors.TextEdit();
            this.edtZ = new DevExpress.XtraEditors.TextEdit();
            this.cboParamKey = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cboParamType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboParamKey.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.ImageOptions.Image")));
            this.btnDelete.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDelete.Location = new System.Drawing.Point(531, 1);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(17, 18);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.ToolTip = "삭제";
            // 
            // cboParamType
            // 
            this.cboParamType.Location = new System.Drawing.Point(0, 0);
            this.cboParamType.Name = "cboParamType";
            this.cboParamType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboParamType.Size = new System.Drawing.Size(100, 22);
            this.cboParamType.TabIndex = 2;
            // 
            // edtX
            // 
            this.edtX.Location = new System.Drawing.Point(324, 0);
            this.edtX.Name = "edtX";
            this.edtX.Size = new System.Drawing.Size(60, 22);
            this.edtX.TabIndex = 3;
            // 
            // edtY
            // 
            this.edtY.Location = new System.Drawing.Point(390, 0);
            this.edtY.Name = "edtY";
            this.edtY.Size = new System.Drawing.Size(60, 22);
            this.edtY.TabIndex = 3;
            // 
            // edtZ
            // 
            this.edtZ.Location = new System.Drawing.Point(456, 0);
            this.edtZ.Name = "edtZ";
            this.edtZ.Size = new System.Drawing.Size(60, 22);
            this.edtZ.TabIndex = 3;
            // 
            // cboParamKey
            // 
            this.cboParamKey.Location = new System.Drawing.Point(107, 0);
            this.cboParamKey.Name = "cboParamKey";
            this.cboParamKey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboParamKey.Size = new System.Drawing.Size(211, 22);
            this.cboParamKey.TabIndex = 2;
            // 
            // MgmtPresetParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.edtZ);
            this.Controls.Add(this.edtY);
            this.Controls.Add(this.edtX);
            this.Controls.Add(this.cboParamKey);
            this.Controls.Add(this.cboParamType);
            this.Controls.Add(this.btnDelete);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MgmtPresetParameterControl";
            this.Size = new System.Drawing.Size(560, 22);
            this.Load += new System.EventHandler(this.MgmtPresetParameterControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cboParamType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboParamKey.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.ComboBoxEdit cboParamType;
        private DevExpress.XtraEditors.TextEdit edtX;
        private DevExpress.XtraEditors.TextEdit edtY;
        private DevExpress.XtraEditors.TextEdit edtZ;
        private DevExpress.XtraEditors.ComboBoxEdit cboParamKey;
    }
}
