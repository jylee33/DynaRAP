namespace DynaRAP.UControl
{
    partial class MgmtPresetControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MgmtPresetControl));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnSaveAsNewParameter = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnDeleteParameter = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            this.btnLink = new DevExpress.XtraEditors.SimpleButton();
            this.btnModifyParameter = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl37 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.lblMandatoryField = new DevExpress.XtraEditors.LabelControl();
            this.edtParamName = new DevExpress.XtraEditors.TextEdit();
            this.cboUnit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboPresetList = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            this.separatorControl2 = new DevExpress.XtraEditors.SeparatorControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            this.btnAddParameter = new DevExpress.XtraEditors.ButtonEdit();
            this.sharedImageCollection1 = new DevExpress.Utils.SharedImageCollection(this.components);
            this.RefSubSeq = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.RefSeq = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.DirType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.DirName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.ParentID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.ID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.edtParamName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPresetList.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.xtraScrollableControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddParameter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sharedImageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sharedImageCollection1.ImageSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(107, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(65, 17);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "프리셋 구성";
            // 
            // btnSaveAsNewParameter
            // 
            this.btnSaveAsNewParameter.Location = new System.Drawing.Point(490, 574);
            this.btnSaveAsNewParameter.Name = "btnSaveAsNewParameter";
            this.btnSaveAsNewParameter.Size = new System.Drawing.Size(110, 23);
            this.btnSaveAsNewParameter.TabIndex = 20;
            this.btnSaveAsNewParameter.Text = "새 프리셋으로 저장";
            this.btnSaveAsNewParameter.Click += new System.EventHandler(this.btnSaveAsNewPreset_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(107, 57);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(116, 15);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "프리셋 구성 파라미터";
            // 
            // btnDeleteParameter
            // 
            this.btnDeleteParameter.Location = new System.Drawing.Point(369, 574);
            this.btnDeleteParameter.Name = "btnDeleteParameter";
            this.btnDeleteParameter.Size = new System.Drawing.Size(110, 23);
            this.btnDeleteParameter.TabIndex = 19;
            this.btnDeleteParameter.Text = "프리셋 삭제";
            this.btnDeleteParameter.Click += new System.EventHandler(this.btnDeletePreset_Click);
            // 
            // labelControl19
            // 
            this.labelControl19.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl19.Appearance.Options.UseFont = true;
            this.labelControl19.Location = new System.Drawing.Point(107, 199);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(116, 15);
            this.labelControl19.TabIndex = 0;
            this.labelControl19.Text = "프리셋 구성 파라미터";
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(1, 335);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(91, 87);
            this.btnLink.TabIndex = 21;
            this.btnLink.Text = "<= 연결 =>";
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // btnModifyParameter
            // 
            this.btnModifyParameter.Location = new System.Drawing.Point(242, 574);
            this.btnModifyParameter.Name = "btnModifyParameter";
            this.btnModifyParameter.Size = new System.Drawing.Size(110, 23);
            this.btnModifyParameter.TabIndex = 18;
            this.btnModifyParameter.Text = "프리셋 저장";
            this.btnModifyParameter.Click += new System.EventHandler(this.btnModifyPreset_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(107, 84);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(55, 13);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "파라미터명";
            // 
            // labelControl37
            // 
            this.labelControl37.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.labelControl37.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl37.Appearance.Options.UseFont = true;
            this.labelControl37.Appearance.Options.UseForeColor = true;
            this.labelControl37.Location = new System.Drawing.Point(242, 108);
            this.labelControl37.Name = "labelControl37";
            this.labelControl37.Size = new System.Drawing.Size(195, 13);
            this.labelControl37.TabIndex = 0;
            this.labelControl37.Text = "구성을 구분할 특징적인 이름을 입력하세요.";
            // 
            // labelControl17
            // 
            this.labelControl17.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelControl17.Appearance.Options.UseFont = true;
            this.labelControl17.Location = new System.Drawing.Point(107, 134);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(80, 13);
            this.labelControl17.TabIndex = 0;
            this.labelControl17.Text = "프리셋 카테고리";
            // 
            // lblMandatoryField
            // 
            this.lblMandatoryField.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.lblMandatoryField.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblMandatoryField.Appearance.Options.UseFont = true;
            this.lblMandatoryField.Appearance.Options.UseForeColor = true;
            this.lblMandatoryField.Location = new System.Drawing.Point(481, 89);
            this.lblMandatoryField.Name = "lblMandatoryField";
            this.lblMandatoryField.Size = new System.Drawing.Size(137, 13);
            this.lblMandatoryField.TabIndex = 0;
            this.lblMandatoryField.Text = "프리셋 이름은 필수 입니다.";
            this.lblMandatoryField.Visible = false;
            // 
            // edtParamName
            // 
            this.edtParamName.EditValue = "";
            this.edtParamName.Location = new System.Drawing.Point(242, 84);
            this.edtParamName.Name = "edtParamName";
            this.edtParamName.Size = new System.Drawing.Size(222, 22);
            this.edtParamName.TabIndex = 1;
            // 
            // cboUnit
            // 
            this.cboUnit.EditValue = "";
            this.cboUnit.Location = new System.Drawing.Point(242, 129);
            this.cboUnit.Name = "cboUnit";
            this.cboUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboUnit.Size = new System.Drawing.Size(100, 22);
            this.cboUnit.TabIndex = 8;
            // 
            // cboPresetList
            // 
            this.cboPresetList.Location = new System.Drawing.Point(107, 25);
            this.cboPresetList.Name = "cboPresetList";
            this.cboPresetList.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPresetList.Size = new System.Drawing.Size(357, 22);
            this.cboPresetList.TabIndex = 0;
            this.cboPresetList.SelectedIndexChanged += new System.EventHandler(this.cboPresetList_SelectedIndexChanged);
            // 
            // labelControl18
            // 
            this.labelControl18.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.labelControl18.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl18.Appearance.Options.UseFont = true;
            this.labelControl18.Appearance.Options.UseForeColor = true;
            this.labelControl18.Location = new System.Drawing.Point(242, 156);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(109, 13);
            this.labelControl18.TabIndex = 0;
            this.labelControl18.Text = "센서 위치를 선택합니다.";
            // 
            // separatorControl1
            // 
            this.separatorControl1.BackColor = System.Drawing.Color.White;
            this.separatorControl1.Location = new System.Drawing.Point(107, 78);
            this.separatorControl1.Name = "separatorControl1";
            this.separatorControl1.Size = new System.Drawing.Size(561, 1);
            this.separatorControl1.TabIndex = 1;
            // 
            // separatorControl2
            // 
            this.separatorControl2.BackColor = System.Drawing.Color.White;
            this.separatorControl2.Location = new System.Drawing.Point(107, 224);
            this.separatorControl2.Name = "separatorControl2";
            this.separatorControl2.Size = new System.Drawing.Size(561, 1);
            this.separatorControl2.TabIndex = 1;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.xtraScrollableControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(711, 865);
            this.panelControl1.TabIndex = 6;
            // 
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Controls.Add(this.btnAddParameter);
            this.xtraScrollableControl1.Controls.Add(this.labelControl1);
            this.xtraScrollableControl1.Controls.Add(this.btnSaveAsNewParameter);
            this.xtraScrollableControl1.Controls.Add(this.labelControl2);
            this.xtraScrollableControl1.Controls.Add(this.btnDeleteParameter);
            this.xtraScrollableControl1.Controls.Add(this.labelControl19);
            this.xtraScrollableControl1.Controls.Add(this.btnLink);
            this.xtraScrollableControl1.Controls.Add(this.btnModifyParameter);
            this.xtraScrollableControl1.Controls.Add(this.labelControl3);
            this.xtraScrollableControl1.Controls.Add(this.labelControl37);
            this.xtraScrollableControl1.Controls.Add(this.labelControl17);
            this.xtraScrollableControl1.Controls.Add(this.lblMandatoryField);
            this.xtraScrollableControl1.Controls.Add(this.edtParamName);
            this.xtraScrollableControl1.Controls.Add(this.cboUnit);
            this.xtraScrollableControl1.Controls.Add(this.cboPresetList);
            this.xtraScrollableControl1.Controls.Add(this.labelControl18);
            this.xtraScrollableControl1.Controls.Add(this.separatorControl1);
            this.xtraScrollableControl1.Controls.Add(this.separatorControl2);
            this.xtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraScrollableControl1.Location = new System.Drawing.Point(2, 2);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Size = new System.Drawing.Size(707, 861);
            this.xtraScrollableControl1.TabIndex = 0;
            // 
            // btnAddParameter
            // 
            this.btnAddParameter.EditValue = "파라미터 추가";
            this.btnAddParameter.Location = new System.Drawing.Point(554, 196);
            this.btnAddParameter.Name = "btnAddParameter";
            this.btnAddParameter.Properties.Appearance.Options.UseTextOptions = true;
            this.btnAddParameter.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.btnAddParameter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)});
            this.btnAddParameter.Properties.ReadOnly = true;
            this.btnAddParameter.Size = new System.Drawing.Size(114, 22);
            this.btnAddParameter.TabIndex = 22;
            this.btnAddParameter.Click += new System.EventHandler(this.btnAddParameter_Click);
            // 
            // sharedImageCollection1
            // 
            // 
            // 
            // 
            this.sharedImageCollection1.ImageSource.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("sharedImageCollection1.ImageSource.ImageStream")));
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(0, "folder.png");
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(1, "file.png");
            this.sharedImageCollection1.ParentControl = this;
            // 
            // RefSubSeq
            // 
            this.RefSubSeq.Caption = "RefSubSeq";
            this.RefSubSeq.FieldName = "RefSubSeq";
            this.RefSubSeq.Name = "RefSubSeq";
            this.RefSubSeq.Visible = true;
            this.RefSubSeq.VisibleIndex = 5;
            // 
            // RefSeq
            // 
            this.RefSeq.Caption = "RefSeq";
            this.RefSeq.FieldName = "RefSeq";
            this.RefSeq.Name = "RefSeq";
            this.RefSeq.Visible = true;
            this.RefSeq.VisibleIndex = 4;
            // 
            // DirType
            // 
            this.DirType.Caption = "DirType";
            this.DirType.FieldName = "DirType";
            this.DirType.Name = "DirType";
            this.DirType.Visible = true;
            this.DirType.VisibleIndex = 3;
            // 
            // DirName
            // 
            this.DirName.Caption = "파라미터 정보";
            this.DirName.FieldName = "DirName";
            this.DirName.Name = "DirName";
            this.DirName.Visible = true;
            this.DirName.VisibleIndex = 0;
            // 
            // ParentID
            // 
            this.ParentID.Caption = "ParentID";
            this.ParentID.FieldName = "ParentID";
            this.ParentID.Name = "ParentID";
            this.ParentID.Visible = true;
            this.ParentID.VisibleIndex = 2;
            // 
            // ID
            // 
            this.ID.Caption = "ID";
            this.ID.FieldName = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = true;
            this.ID.VisibleIndex = 1;
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.ID,
            this.ParentID,
            this.DirName,
            this.DirType,
            this.RefSeq,
            this.RefSubSeq});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.ImageIndexFieldName = "DirName";
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Name = "treeList1";
            this.treeList1.Size = new System.Drawing.Size(320, 865);
            this.treeList1.StateImageList = this.sharedImageCollection1;
            this.treeList1.TabIndex = 2;
            this.treeList1.GetStateImage += new DevExpress.XtraTreeList.GetStateImageEventHandler(this.treeList1_GetStateImage);
            this.treeList1.GetSelectImage += new DevExpress.XtraTreeList.GetSelectImageEventHandler(this.treeList1_GetSelectImage);
            this.treeList1.RowClick += new DevExpress.XtraTreeList.RowClickEventHandler(this.treeList1_RowClick);
            this.treeList1.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList1_FocusedNodeChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeList1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1035, 865);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 1;
            // 
            // MgmtPresetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MgmtPresetControl";
            this.Size = new System.Drawing.Size(1035, 865);
            this.Load += new System.EventHandler(this.MgmtPresetControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edtParamName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPresetList.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.xtraScrollableControl1.ResumeLayout(false);
            this.xtraScrollableControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddParameter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sharedImageCollection1.ImageSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sharedImageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnSaveAsNewParameter;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnDeleteParameter;
        private DevExpress.XtraEditors.LabelControl labelControl19;
        private DevExpress.XtraEditors.SimpleButton btnLink;
        private DevExpress.XtraEditors.SimpleButton btnModifyParameter;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl37;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.LabelControl lblMandatoryField;
        private DevExpress.XtraEditors.TextEdit edtParamName;
        private DevExpress.XtraEditors.ComboBoxEdit cboUnit;
        private DevExpress.XtraEditors.ComboBoxEdit cboPresetList;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        private DevExpress.XtraEditors.SeparatorControl separatorControl1;
        private DevExpress.XtraEditors.SeparatorControl separatorControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
        private DevExpress.Utils.SharedImageCollection sharedImageCollection1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ParentID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn DirName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn DirType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn RefSeq;
        private DevExpress.XtraTreeList.Columns.TreeListColumn RefSubSeq;
        private DevExpress.XtraEditors.ButtonEdit btnAddParameter;
    }
}
