namespace DynaRAP.TEST
{
    partial class TreeTestForm
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
            this.treeListProject = new DevExpress.XtraTreeList.TreeList();
            ((System.ComponentModel.ISupportInitialize)(this.treeListProject)).BeginInit();
            this.SuspendLayout();
            // 
            // treeListProject
            // 
            this.treeListProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListProject.Location = new System.Drawing.Point(0, 0);
            this.treeListProject.Name = "treeListProject";
            this.treeListProject.Size = new System.Drawing.Size(809, 593);
            this.treeListProject.TabIndex = 0;
            // 
            // TreeTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 593);
            this.Controls.Add(this.treeListProject);
            this.Name = "TreeTestForm";
            this.Text = "TreeTestForm";
            this.Load += new System.EventHandler(this.TreeTestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeListProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeListProject;
    }
}