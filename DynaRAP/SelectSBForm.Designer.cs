namespace DynaRAP
{
    partial class SelectSBForm
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
            this.selectSBControl1 = new DynaRAP.UControl.SelectSBControl();
            this.SuspendLayout();
            // 
            // selectSBControl1
            // 
            this.selectSBControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectSBControl1.Location = new System.Drawing.Point(0, 0);
            this.selectSBControl1.Name = "selectSBControl1";
            this.selectSBControl1.Size = new System.Drawing.Size(392, 645);
            this.selectSBControl1.TabIndex = 0;
            // 
            // SelectSBForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 645);
            this.Controls.Add(this.selectSBControl1);
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectSBForm";
            this.Load += new System.EventHandler(this.SelectSBForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UControl.SelectSBControl selectSBControl1;
    }
}