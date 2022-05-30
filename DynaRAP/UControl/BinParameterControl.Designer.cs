namespace DynaRAP.UControl
{
    partial class BinParameterControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinParameterControl));
            this.cboType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.chkParameter = new System.Windows.Forms.CheckBox();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cboType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cboType
            // 
            this.cboType.Location = new System.Drawing.Point(23, 0);
            this.cboType.Name = "cboType";
            this.cboType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboType.Size = new System.Drawing.Size(185, 22);
            this.cboType.TabIndex = 1;
            // 
            // cboName
            // 
            this.cboName.Location = new System.Drawing.Point(214, 0);
            this.cboName.Name = "cboName";
            this.cboName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboName.Size = new System.Drawing.Size(211, 22);
            this.cboName.TabIndex = 1;
            // 
            // chkParameter
            // 
            this.chkParameter.AutoSize = true;
            this.chkParameter.Location = new System.Drawing.Point(4, 4);
            this.chkParameter.Name = "chkParameter";
            this.chkParameter.Size = new System.Drawing.Size(15, 14);
            this.chkParameter.TabIndex = 2;
            this.chkParameter.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.ImageOptions.Image")));
            this.btnDelete.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDelete.Location = new System.Drawing.Point(447, 1);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(17, 18);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // BinParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkParameter);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.cboName);
            this.Controls.Add(this.cboType);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BinParameterControl";
            this.Size = new System.Drawing.Size(482, 22);
            this.Load += new System.EventHandler(this.BinParameterControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cboType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.ComboBoxEdit cboType;
        private DevExpress.XtraEditors.ComboBoxEdit cboName;
        private System.Windows.Forms.CheckBox chkParameter;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
    }
}
