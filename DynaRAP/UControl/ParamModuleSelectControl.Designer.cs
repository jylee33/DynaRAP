namespace DynaRAP.UControl
{
    partial class ParamModuleSelectControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParamModuleSelectControl));
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.btnListSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnNewParamSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelParamModule = new DevExpress.XtraEditors.SimpleButton();
            this.imageCollection2 = new DevExpress.Utils.ImageCollection(this.components);
            this.imageCollection3 = new DevExpress.Utils.ImageCollection(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.edtModuleName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblTag = new DevExpress.XtraEditors.LabelControl();
            this.edtTag = new DevExpress.XtraEditors.ButtonEdit();
            this.panelTag = new System.Windows.Forms.FlowLayoutPanel();
            this.btnParamModuleCopy = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection3)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtModuleName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTag.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(4, 62);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(101, 17);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "파라미터모듈 목록";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(4, 85);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.repositoryItemImageComboBox1});
            this.gridControl1.Size = new System.Drawing.Size(750, 367);
            this.gridControl1.TabIndex = 43;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseUp;
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "파라미터모듈이름";
            this.gridColumn1.ColumnEdit = this.repositoryItemComboBox1;
            this.gridColumn1.FieldName = "moduleName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.ReadOnly = true;
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // repositoryItemImageComboBox1
            // 
            this.repositoryItemImageComboBox1.AutoHeight = false;
            this.repositoryItemImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1";
            this.repositoryItemImageComboBox1.SmallImages = this.imageCollection1;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "none.png");
            this.imageCollection1.Images.SetKeyName(1, "plus.png");
            // 
            // btnListSave
            // 
            this.btnListSave.Location = new System.Drawing.Point(4, 636);
            this.btnListSave.Name = "btnListSave";
            this.btnListSave.Size = new System.Drawing.Size(115, 23);
            this.btnListSave.TabIndex = 44;
            this.btnListSave.Text = "파라미터모듈 수정";
            this.btnListSave.Click += new System.EventHandler(this.btnListSave_Click);
            // 
            // btnNewParamSave
            // 
            this.btnNewParamSave.Location = new System.Drawing.Point(258, 636);
            this.btnNewParamSave.Name = "btnNewParamSave";
            this.btnNewParamSave.Size = new System.Drawing.Size(137, 23);
            this.btnNewParamSave.TabIndex = 44;
            this.btnNewParamSave.Text = "새 파라미터모듈 등록";
            this.btnNewParamSave.Click += new System.EventHandler(this.btnNewParamModuleSave_Click);
            // 
            // btnDelParamModule
            // 
            this.btnDelParamModule.Location = new System.Drawing.Point(125, 636);
            this.btnDelParamModule.Name = "btnDelParamModule";
            this.btnDelParamModule.Size = new System.Drawing.Size(127, 23);
            this.btnDelParamModule.TabIndex = 44;
            this.btnDelParamModule.Text = "파라미터모듈 삭제";
            this.btnDelParamModule.Click += new System.EventHandler(this.btnDelParamModule_Click);
            // 
            // imageCollection2
            // 
            this.imageCollection2.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection2.ImageStream")));
            this.imageCollection2.Images.SetKeyName(0, "none.png");
            this.imageCollection2.Images.SetKeyName(1, "delete.png");
            // 
            // imageCollection3
            // 
            this.imageCollection3.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection3.ImageStream")));
            this.imageCollection3.Images.SetKeyName(0, "none.png");
            this.imageCollection3.Images.SetKeyName(1, "view.png");
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.labelControl1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.edtModuleName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelControl3, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(544, 51);
            this.tableLayoutPanel2.TabIndex = 45;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(3, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 17);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "모듈 이름";
            // 
            // edtModuleName
            // 
            this.edtModuleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edtModuleName.Location = new System.Drawing.Point(103, 3);
            this.edtModuleName.Name = "edtModuleName";
            this.edtModuleName.Size = new System.Drawing.Size(438, 22);
            this.edtModuleName.TabIndex = 16;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl3.Location = new System.Drawing.Point(103, 28);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(276, 15);
            this.labelControl3.TabIndex = 18;
            this.labelControl3.Text = "지정한 이름으로 파라미터 모듈을 저장, 수정 합니다.";
            // 
            // lblTag
            // 
            this.lblTag.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblTag.Appearance.Options.UseFont = true;
            this.lblTag.Location = new System.Drawing.Point(4, 464);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(26, 22);
            this.lblTag.TabIndex = 46;
            this.lblTag.Text = "태그";
            // 
            // edtTag
            // 
            this.edtTag.Location = new System.Drawing.Point(4, 492);
            this.edtTag.Name = "edtTag";
            this.edtTag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)});
            this.edtTag.Size = new System.Drawing.Size(263, 22);
            this.edtTag.TabIndex = 47;
            this.edtTag.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.removeTag_ButtonClick);
            this.edtTag.KeyUp += new System.Windows.Forms.KeyEventHandler(this.edtTag_KeyUp);
            // 
            // panelTag
            // 
            this.panelTag.AutoScroll = true;
            this.panelTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTag.Location = new System.Drawing.Point(4, 520);
            this.panelTag.Name = "panelTag";
            this.panelTag.Size = new System.Drawing.Size(750, 96);
            this.panelTag.TabIndex = 48;
            // 
            // btnParamModuleCopy
            // 
            this.btnParamModuleCopy.Location = new System.Drawing.Point(401, 636);
            this.btnParamModuleCopy.Name = "btnParamModuleCopy";
            this.btnParamModuleCopy.Size = new System.Drawing.Size(137, 23);
            this.btnParamModuleCopy.TabIndex = 49;
            this.btnParamModuleCopy.Text = "선택 파라미터모듈 복사";
            this.btnParamModuleCopy.Click += new System.EventHandler(this.btnParamModuleCopy_Click);
            // 
            // ParamModuleSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnParamModuleCopy);
            this.Controls.Add(this.lblTag);
            this.Controls.Add(this.edtTag);
            this.Controls.Add(this.panelTag);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.btnDelParamModule);
            this.Controls.Add(this.btnNewParamSave);
            this.Controls.Add(this.btnListSave);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.labelControl2);
            this.Name = "ParamModuleSelectControl";
            this.Size = new System.Drawing.Size(770, 694);
            this.Load += new System.EventHandler(this.ParamDataSelectControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection3)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtModuleName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTag.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.SimpleButton btnListSave;
        private DevExpress.XtraEditors.SimpleButton btnNewParamSave;
        private DevExpress.XtraEditors.SimpleButton btnDelParamModule;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.Utils.ImageCollection imageCollection2;
        private DevExpress.Utils.ImageCollection imageCollection3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit edtModuleName;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl lblTag;
        private DevExpress.XtraEditors.ButtonEdit edtTag;
        private System.Windows.Forms.FlowLayoutPanel panelTag;
        private DevExpress.XtraEditors.SimpleButton btnParamModuleCopy;
    }
}
