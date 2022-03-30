namespace DynaRAP.UControl
{
    partial class SelectSBControl
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
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.ID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.ParentID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.FlyingName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.Check = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.ID,
            this.ParentID,
            this.FlyingName,
            this.Check});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Name = "treeList1";
            this.treeList1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.treeList1.Size = new System.Drawing.Size(362, 544);
            this.treeList1.TabIndex = 1;
            // 
            // ID
            // 
            this.ID.Caption = "ID";
            this.ID.FieldName = "ID";
            this.ID.Name = "ID";
            // 
            // ParentID
            // 
            this.ParentID.Caption = "ParentID";
            this.ParentID.FieldName = "ParentID";
            this.ParentID.Name = "ParentID";
            // 
            // FlyingName
            // 
            this.FlyingName.Caption = "FlyingName";
            this.FlyingName.FieldName = "FlyingName";
            this.FlyingName.Name = "FlyingName";
            this.FlyingName.Visible = true;
            this.FlyingName.VisibleIndex = 0;
            // 
            // Check
            // 
            this.Check.Caption = "Check";
            this.Check.ColumnEdit = this.repositoryItemCheckEdit1;
            this.Check.FieldName = "Check";
            this.Check.Name = "Check";
            this.Check.Visible = true;
            this.Check.VisibleIndex = 1;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // SelectSBControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeList1);
            this.Name = "SelectSBControl";
            this.Size = new System.Drawing.Size(362, 544);
            this.Load += new System.EventHandler(this.SelectSBControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ParentID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn FlyingName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn Check;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}
