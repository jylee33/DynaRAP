namespace DynaRAP
{
    partial class PlotForm
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
            this.rdoPart = new System.Windows.Forms.RadioButton();
            this.rdoSB = new System.Windows.Forms.RadioButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.rdoRaw = new System.Windows.Forms.RadioButton();
            this.rdoLPF = new System.Windows.Forms.RadioButton();
            this.rdoHPF = new System.Windows.Forms.RadioButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn1DTime = new DevExpress.XtraEditors.SimpleButton();
            this.btn1DMinMax = new DevExpress.XtraEditors.SimpleButton();
            this.btn2DPotato = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // rdoPart
            // 
            this.rdoPart.AutoSize = true;
            this.rdoPart.Location = new System.Drawing.Point(241, 51);
            this.rdoPart.Name = "rdoPart";
            this.rdoPart.Size = new System.Drawing.Size(88, 19);
            this.rdoPart.TabIndex = 0;
            this.rdoPart.Text = "분할 데이터";
            this.rdoPart.UseVisualStyleBackColor = true;
            // 
            // rdoSB
            // 
            this.rdoSB.AutoSize = true;
            this.rdoSB.Location = new System.Drawing.Point(420, 51);
            this.rdoSB.Name = "rdoSB";
            this.rdoSB.Size = new System.Drawing.Size(80, 19);
            this.rdoSB.TabIndex = 0;
            this.rdoSB.Text = "SB 데이터";
            this.rdoSB.UseVisualStyleBackColor = true;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(54, 53);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(63, 15);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "데이터 구분";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(54, 90);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(63, 15);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "필터링 구분";
            // 
            // rdoRaw
            // 
            this.rdoRaw.AutoSize = true;
            this.rdoRaw.Checked = true;
            this.rdoRaw.Location = new System.Drawing.Point(241, 88);
            this.rdoRaw.Name = "rdoRaw";
            this.rdoRaw.Size = new System.Drawing.Size(52, 19);
            this.rdoRaw.TabIndex = 2;
            this.rdoRaw.TabStop = true;
            this.rdoRaw.Text = "RAW";
            this.rdoRaw.UseVisualStyleBackColor = true;
            // 
            // rdoLPF
            // 
            this.rdoLPF.AutoSize = true;
            this.rdoLPF.Location = new System.Drawing.Point(420, 90);
            this.rdoLPF.Name = "rdoLPF";
            this.rdoLPF.Size = new System.Drawing.Size(47, 19);
            this.rdoLPF.TabIndex = 2;
            this.rdoLPF.Text = "LPF";
            this.rdoLPF.UseVisualStyleBackColor = true;
            // 
            // rdoHPF
            // 
            this.rdoHPF.AutoSize = true;
            this.rdoHPF.Location = new System.Drawing.Point(590, 90);
            this.rdoHPF.Name = "rdoHPF";
            this.rdoHPF.Size = new System.Drawing.Size(49, 19);
            this.rdoHPF.TabIndex = 2;
            this.rdoHPF.Text = "HPF";
            this.rdoHPF.UseVisualStyleBackColor = true;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(54, 129);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 15);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "파라미터";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(54, 151);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(199, 574);
            this.listBox1.TabIndex = 3;
            this.listBox1.Visible = false;
            // 
            // btn1DTime
            // 
            this.btn1DTime.Location = new System.Drawing.Point(308, 151);
            this.btn1DTime.Name = "btn1DTime";
            this.btn1DTime.Size = new System.Drawing.Size(192, 23);
            this.btn1DTime.TabIndex = 4;
            this.btn1DTime.Text = "1D Time PLOT";
            // 
            // btn1DMinMax
            // 
            this.btn1DMinMax.Location = new System.Drawing.Point(308, 192);
            this.btn1DMinMax.Name = "btn1DMinMax";
            this.btn1DMinMax.Size = new System.Drawing.Size(192, 23);
            this.btn1DMinMax.TabIndex = 4;
            this.btn1DMinMax.Text = "1D Min / Max PLOT";
            // 
            // btn2DPotato
            // 
            this.btn2DPotato.Location = new System.Drawing.Point(308, 234);
            this.btn2DPotato.Name = "btn2DPotato";
            this.btn2DPotato.Size = new System.Drawing.Size(192, 23);
            this.btn2DPotato.TabIndex = 4;
            this.btn2DPotato.Text = "Potato PLOT";
            // 
            // PlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 748);
            this.Controls.Add(this.btn2DPotato);
            this.Controls.Add(this.btn1DMinMax);
            this.Controls.Add(this.btn1DTime);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.rdoHPF);
            this.Controls.Add(this.rdoLPF);
            this.Controls.Add(this.rdoRaw);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.rdoSB);
            this.Controls.Add(this.rdoPart);
            this.Name = "PlotForm";
            this.Text = "PlotForm";
            this.Load += new System.EventHandler(this.PlotForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoPart;
        private System.Windows.Forms.RadioButton rdoSB;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.RadioButton rdoRaw;
        private System.Windows.Forms.RadioButton rdoLPF;
        private System.Windows.Forms.RadioButton rdoHPF;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.ListBox listBox1;
        private DevExpress.XtraEditors.SimpleButton btn1DTime;
        private DevExpress.XtraEditors.SimpleButton btn1DMinMax;
        private DevExpress.XtraEditors.SimpleButton btn2DPotato;
    }
}