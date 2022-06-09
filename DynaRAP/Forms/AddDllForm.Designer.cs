namespace DynaRAP.Forms
{
    partial class AddDllForm
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.edtDataSetCode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.edtDataSetName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.edtDataSetVersion = new DevExpress.XtraEditors.TextEdit();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.edtDataSetCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtDataSetName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtDataSetVersion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "데이터셋 코드";
            // 
            // edtDataSetCode
            // 
            this.edtDataSetCode.Location = new System.Drawing.Point(110, 13);
            this.edtDataSetCode.Name = "edtDataSetCode";
            this.edtDataSetCode.Size = new System.Drawing.Size(288, 22);
            this.edtDataSetCode.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 44);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 15);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "데이터셋 이름";
            // 
            // edtDataSetName
            // 
            this.edtDataSetName.Location = new System.Drawing.Point(110, 41);
            this.edtDataSetName.Name = "edtDataSetName";
            this.edtDataSetName.Size = new System.Drawing.Size(288, 22);
            this.edtDataSetName.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 72);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(75, 15);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "데이터셋 버전";
            // 
            // edtDataSetVersion
            // 
            this.edtDataSetVersion.Location = new System.Drawing.Point(110, 69);
            this.edtDataSetVersion.Name = "edtDataSetVersion";
            this.edtDataSetVersion.Size = new System.Drawing.Size(288, 22);
            this.edtDataSetVersion.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(110, 112);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "추가";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(244, 112);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "취소";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddDllForm
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(407, 153);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.edtDataSetVersion);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.edtDataSetName);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.edtDataSetCode);
            this.Controls.Add(this.labelControl1);
            this.Name = "AddDllForm";
            this.Text = "기준데이터 추가";
            this.Load += new System.EventHandler(this.AddDllForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edtDataSetCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtDataSetName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtDataSetVersion.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit edtDataSetCode;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit edtDataSetName;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit edtDataSetVersion;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}