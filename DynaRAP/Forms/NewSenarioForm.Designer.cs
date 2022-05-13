namespace DynaRAP.Forms
{
    partial class NewSenarioForm
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
            this.buttonEdit1 = new DevExpress.XtraEditors.ButtonEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnCreate = new DevExpress.XtraEditors.SimpleButton();
            this.dropScenarioType = new DevExpress.XtraEditors.DropDownButton();
            this.edtScenarioName = new DevExpress.XtraEditors.ButtonEdit();
            this.dateScenario = new DevExpress.XtraEditors.DateEdit();
            this.edtTag = new DevExpress.XtraEditors.ButtonEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.cboScenarioType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panelTag = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtScenarioName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateScenario.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateScenario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboScenarioType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(486, 97);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close)});
            this.buttonEdit1.Size = new System.Drawing.Size(100, 20);
            this.buttonEdit1.TabIndex = 0;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(486, 72);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "checkEdit1";
            this.checkEdit1.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.checkEdit1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkEdit1.Size = new System.Drawing.Size(93, 19);
            this.checkEdit1.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(28, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 22);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "시나리오";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(28, 146);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(26, 22);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "태그";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(275, 309);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "Create";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // dropScenarioType
            // 
            this.dropScenarioType.Location = new System.Drawing.Point(426, 43);
            this.dropScenarioType.Name = "dropScenarioType";
            this.dropScenarioType.Size = new System.Drawing.Size(165, 23);
            this.dropScenarioType.TabIndex = 0;
            this.dropScenarioType.Text = "Import Module Scenario";
            this.dropScenarioType.Visible = false;
            // 
            // edtScenarioName
            // 
            this.edtScenarioName.Location = new System.Drawing.Point(28, 83);
            this.edtScenarioName.Name = "edtScenarioName";
            this.edtScenarioName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Close)});
            this.edtScenarioName.Size = new System.Drawing.Size(165, 20);
            this.edtScenarioName.TabIndex = 1;
            this.edtScenarioName.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.edtScenarioName_ClearButtonClick);
            // 
            // dateScenario
            // 
            this.dateScenario.EditValue = null;
            this.dateScenario.Location = new System.Drawing.Point(28, 110);
            this.dateScenario.Name = "dateScenario";
            this.dateScenario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateScenario.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateScenario.Size = new System.Drawing.Size(100, 20);
            this.dateScenario.TabIndex = 2;
            // 
            // edtTag
            // 
            this.edtTag.Location = new System.Drawing.Point(28, 174);
            this.edtTag.Name = "edtTag";
            this.edtTag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)});
            this.edtTag.Size = new System.Drawing.Size(301, 20);
            this.edtTag.TabIndex = 4;
            this.edtTag.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.edtTag_ButtonClick);
            this.edtTag.KeyUp += new System.Windows.Forms.KeyEventHandler(this.edtTag_KeyUp);
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(486, 124);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(100, 20);
            this.textEdit1.TabIndex = 9;
            this.textEdit1.Visible = false;
            // 
            // cboScenarioType
            // 
            this.cboScenarioType.Location = new System.Drawing.Point(28, 57);
            this.cboScenarioType.Name = "cboScenarioType";
            this.cboScenarioType.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.cboScenarioType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboScenarioType.Size = new System.Drawing.Size(206, 20);
            this.cboScenarioType.TabIndex = 0;
            this.cboScenarioType.SelectedIndexChanged += new System.EventHandler(this.cboScenarioType_SelectedIndexChanged);
            // 
            // panelTag
            // 
            this.panelTag.AutoScroll = true;
            this.panelTag.Location = new System.Drawing.Point(29, 201);
            this.panelTag.Name = "panelTag";
            this.panelTag.Size = new System.Drawing.Size(557, 90);
            this.panelTag.TabIndex = 5;
            // 
            // NewSenarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 343);
            this.Controls.Add(this.panelTag);
            this.Controls.Add(this.cboScenarioType);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.dateScenario);
            this.Controls.Add(this.edtTag);
            this.Controls.Add(this.edtScenarioName);
            this.Controls.Add(this.dropScenarioType);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.buttonEdit1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewSenarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Scenario...";
            this.Load += new System.EventHandler(this.NewSenarioForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtScenarioName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateScenario.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateScenario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboScenarioType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnCreate;
        private DevExpress.XtraEditors.DropDownButton dropScenarioType;
        private DevExpress.XtraEditors.ButtonEdit edtScenarioName;
        private DevExpress.XtraEditors.DateEdit dateScenario;
        private DevExpress.XtraEditors.ButtonEdit edtTag;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit cboScenarioType;
        private System.Windows.Forms.FlowLayoutPanel panelTag;
    }
}