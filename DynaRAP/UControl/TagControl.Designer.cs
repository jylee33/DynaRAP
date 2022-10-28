namespace DynaRAP.UControl
{
    partial class TagControl
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
            this.lblTag = new DevExpress.XtraEditors.LabelControl();
            this.edtTag = new DevExpress.XtraEditors.ButtonEdit();
            this.panelTag = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTagSave = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.edtTag.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTag
            // 
            this.lblTag.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblTag.Appearance.Options.UseFont = true;
            this.lblTag.Location = new System.Drawing.Point(3, 3);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(26, 22);
            this.lblTag.TabIndex = 54;
            this.lblTag.Text = "태그";
            // 
            // edtTag
            // 
            this.edtTag.Location = new System.Drawing.Point(3, 28);
            this.edtTag.Name = "edtTag";
            this.edtTag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)});
            this.edtTag.Size = new System.Drawing.Size(184, 22);
            this.edtTag.TabIndex = 55;
            this.edtTag.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.edtTag_ButtonClick);
            this.edtTag.KeyUp += new System.Windows.Forms.KeyEventHandler(this.edtTag_KeyUp);
            // 
            // panelTag
            // 
            this.panelTag.AutoScroll = true;
            this.panelTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTag.Location = new System.Drawing.Point(3, 58);
            this.panelTag.Name = "panelTag";
            this.panelTag.Size = new System.Drawing.Size(444, 109);
            this.panelTag.TabIndex = 56;
            // 
            // btnTagSave
            // 
            this.btnTagSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTagSave.Location = new System.Drawing.Point(281, 3);
            this.btnTagSave.Name = "btnTagSave";
            this.btnTagSave.Size = new System.Drawing.Size(80, 27);
            this.btnTagSave.TabIndex = 45;
            this.btnTagSave.Text = "저장";
            this.btnTagSave.Click += new System.EventHandler(this.btnTagSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panelTag, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.edtTag, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTag, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(450, 200);
            this.tableLayoutPanel1.TabIndex = 57;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnTagSave);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 170);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(450, 30);
            this.flowLayoutPanel1.TabIndex = 58;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Location = new System.Drawing.Point(367, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 27);
            this.btnCancel.TabIndex = 46;
            this.btnCancel.Text = "취소";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // TagControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TagControl";
            this.Size = new System.Drawing.Size(450, 200);
            this.Load += new System.EventHandler(this.BinParameterControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edtTag.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblTag;
        private DevExpress.XtraEditors.ButtonEdit edtTag;
        private System.Windows.Forms.FlowLayoutPanel panelTag;
        private DevExpress.XtraEditors.SimpleButton btnTagSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}
