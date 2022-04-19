namespace DynaRAP.TEST
{
    partial class TestWebForm
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
            this.txtUrl = new DevExpress.XtraEditors.TextEdit();
            this.radioGET = new System.Windows.Forms.RadioButton();
            this.radioPOST = new System.Windows.Forms.RadioButton();
            this.btnSend = new DevExpress.XtraEditors.SimpleButton();
            this.txtRequest = new System.Windows.Forms.RichTextBox();
            this.txtResponse = new System.Windows.Forms.RichTextBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtUrl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(3, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(25, 15);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "URL";
            // 
            // txtUrl
            // 
            this.txtUrl.EditValue = "https://httpbin.org/post";
            this.txtUrl.Location = new System.Drawing.Point(67, 4);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(831, 22);
            this.txtUrl.TabIndex = 0;
            // 
            // radioGET
            // 
            this.radioGET.AutoSize = true;
            this.radioGET.Location = new System.Drawing.Point(67, 185);
            this.radioGET.Name = "radioGET";
            this.radioGET.Size = new System.Drawing.Size(49, 19);
            this.radioGET.TabIndex = 2;
            this.radioGET.Text = "GET";
            this.radioGET.UseVisualStyleBackColor = true;
            // 
            // radioPOST
            // 
            this.radioPOST.AutoSize = true;
            this.radioPOST.Checked = true;
            this.radioPOST.Location = new System.Drawing.Point(139, 185);
            this.radioPOST.Name = "radioPOST";
            this.radioPOST.Size = new System.Drawing.Size(57, 19);
            this.radioPOST.TabIndex = 3;
            this.radioPOST.TabStop = true;
            this.radioPOST.Text = "POST";
            this.radioPOST.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(213, 183);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(97, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(67, 33);
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(831, 144);
            this.txtRequest.TabIndex = 1;
            this.txtRequest.Text = "{\n\"id\": \"101\",\n\"name\" : \"Alex\"\n}";
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(67, 212);
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new System.Drawing.Size(831, 270);
            this.txtResponse.TabIndex = 5;
            this.txtResponse.Text = "";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(3, 36);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(47, 15);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "Request";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(3, 215);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(56, 15);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Response";
            // 
            // TestWebForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 493);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.radioPOST);
            this.Controls.Add(this.radioGET);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "TestWebForm";
            this.Text = "TestWebForm";
            this.Load += new System.EventHandler(this.TestWebForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtUrl.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtUrl;
        private System.Windows.Forms.RadioButton radioGET;
        private System.Windows.Forms.RadioButton radioPOST;
        private DevExpress.XtraEditors.SimpleButton btnSend;
        private System.Windows.Forms.RichTextBox txtRequest;
        private System.Windows.Forms.RichTextBox txtResponse;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}