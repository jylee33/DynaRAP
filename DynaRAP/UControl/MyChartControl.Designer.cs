namespace DynaRAP.UControl
{
    partial class MyChartControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.RangeControlRange rangeControlRange1 = new DevExpress.XtraEditors.RangeControlRange();
            this.rangeControl = new DevExpress.XtraEditors.RangeControl();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSeries = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPageSize = new System.Windows.Forms.TextBox();
            this.lblPages = new System.Windows.Forms.Label();
            this.btnMoveFirst = new System.Windows.Forms.Button();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.btnMoveLast = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.chartControl = new DevExpress.XtraCharts.ChartControl();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fileOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getXAxisRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnPaging = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.rangeControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.pnPaging.SuspendLayout();
            this.SuspendLayout();
            // 
            // rangeControl
            // 
            this.rangeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rangeControl.Location = new System.Drawing.Point(0, 0);
            this.rangeControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rangeControl.Name = "rangeControl";
            rangeControlRange1.Maximum = new System.DateTime(2022, 5, 11, 0, 0, 0, 0);
            rangeControlRange1.Minimum = new System.DateTime(2022, 5, 2, 0, 0, 0, 0);
            rangeControlRange1.Owner = this.rangeControl;
            this.rangeControl.SelectedRange = rangeControlRange1;
            this.rangeControl.ShowLabels = false;
            this.rangeControl.Size = new System.Drawing.Size(767, 85);
            this.rangeControl.TabIndex = 2;
            this.rangeControl.Text = "rangeControl1";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Series";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSeries
            // 
            this.cbSeries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeries.FormattingEnabled = true;
            this.cbSeries.Location = new System.Drawing.Point(57, 6);
            this.cbSeries.Name = "cbSeries";
            this.cbSeries.Size = new System.Drawing.Size(121, 20);
            this.cbSeries.TabIndex = 3;
            this.cbSeries.SelectedIndexChanged += new System.EventHandler(this.cbSeries_SelectedIndexChanged);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(386, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(55, 28);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(232, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "PageSize";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPageSize
            // 
            this.txtPageSize.Location = new System.Drawing.Point(306, 6);
            this.txtPageSize.Name = "txtPageSize";
            this.txtPageSize.Size = new System.Drawing.Size(74, 21);
            this.txtPageSize.TabIndex = 5;
            // 
            // lblPages
            // 
            this.lblPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPages.AutoEllipsis = true;
            this.lblPages.Location = new System.Drawing.Point(532, 0);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(154, 34);
            this.lblPages.TabIndex = 4;
            this.lblPages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMoveFirst
            // 
            this.btnMoveFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveFirst.Location = new System.Drawing.Point(454, 3);
            this.btnMoveFirst.Name = "btnMoveFirst";
            this.btnMoveFirst.Size = new System.Drawing.Size(33, 28);
            this.btnMoveFirst.TabIndex = 3;
            this.btnMoveFirst.Text = "<<";
            this.btnMoveFirst.UseVisualStyleBackColor = true;
            this.btnMoveFirst.Click += new System.EventHandler(this.btnMoveFirst_Click);
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveLeft.Location = new System.Drawing.Point(493, 3);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(33, 28);
            this.btnMoveLeft.TabIndex = 2;
            this.btnMoveLeft.Text = "<";
            this.btnMoveLeft.UseVisualStyleBackColor = true;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveRight.Location = new System.Drawing.Point(692, 3);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(33, 28);
            this.btnMoveRight.TabIndex = 1;
            this.btnMoveRight.Text = ">";
            this.btnMoveRight.UseVisualStyleBackColor = true;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // btnMoveLast
            // 
            this.btnMoveLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveLast.Location = new System.Drawing.Point(731, 3);
            this.btnMoveLast.Name = "btnMoveLast";
            this.btnMoveLast.Size = new System.Drawing.Size(33, 28);
            this.btnMoveLast.TabIndex = 0;
            this.btnMoveLast.Text = ">>";
            this.btnMoveLast.UseVisualStyleBackColor = true;
            this.btnMoveLast.Click += new System.EventHandler(this.btnMoveLast_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer.Panel1.Controls.Add(this.chartControl);
            this.splitContainer.Panel1.Controls.Add(this.pnPaging);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rangeControl);
            this.splitContainer.Size = new System.Drawing.Size(767, 388);
            this.splitContainer.SplitterDistance = 302;
            this.splitContainer.SplitterWidth = 1;
            this.splitContainer.TabIndex = 4;
            // 
            // chartControl
            // 
            this.chartControl.ContextMenuStrip = this.contextMenuStrip;
            this.chartControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Right;
            this.chartControl.Location = new System.Drawing.Point(0, 0);
            this.chartControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chartControl.Name = "chartControl";
            this.chartControl.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControl.Size = new System.Drawing.Size(767, 265);
            this.chartControl.TabIndex = 3;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileOpenToolStripMenuItem,
            this.getXAxisRangeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(168, 48);
            // 
            // fileOpenToolStripMenuItem
            // 
            this.fileOpenToolStripMenuItem.Name = "fileOpenToolStripMenuItem";
            this.fileOpenToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.fileOpenToolStripMenuItem.Text = "File Open";
            this.fileOpenToolStripMenuItem.Click += new System.EventHandler(this.fileOpenToolStripMenuItem_Click);
            // 
            // getXAxisRangeToolStripMenuItem
            // 
            this.getXAxisRangeToolStripMenuItem.Name = "getXAxisRangeToolStripMenuItem";
            this.getXAxisRangeToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.getXAxisRangeToolStripMenuItem.Text = "Get X-Axis Range";
            this.getXAxisRangeToolStripMenuItem.Click += new System.EventHandler(this.getXAxisRangeToolStripMenuItem_Click);
            // 
            // pnPaging
            // 
            this.pnPaging.BackColor = System.Drawing.Color.Transparent;
            this.pnPaging.Controls.Add(this.label2);
            this.pnPaging.Controls.Add(this.cbSeries);
            this.pnPaging.Controls.Add(this.btnReset);
            this.pnPaging.Controls.Add(this.label1);
            this.pnPaging.Controls.Add(this.txtPageSize);
            this.pnPaging.Controls.Add(this.lblPages);
            this.pnPaging.Controls.Add(this.btnMoveFirst);
            this.pnPaging.Controls.Add(this.btnMoveLeft);
            this.pnPaging.Controls.Add(this.btnMoveRight);
            this.pnPaging.Controls.Add(this.btnMoveLast);
            this.pnPaging.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnPaging.Location = new System.Drawing.Point(0, 265);
            this.pnPaging.Name = "pnPaging";
            this.pnPaging.Size = new System.Drawing.Size(767, 37);
            this.pnPaging.TabIndex = 1;
            // 
            // MyChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "MyChartControl";
            this.Size = new System.Drawing.Size(767, 388);
            ((System.ComponentModel.ISupportInitialize)(this.rangeControl)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartControl)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.pnPaging.ResumeLayout(false);
            this.pnPaging.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSeries;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPageSize;
        private System.Windows.Forms.Label lblPages;
        private System.Windows.Forms.Button btnMoveFirst;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button btnMoveRight;
        private System.Windows.Forms.Button btnMoveLast;
        private System.Windows.Forms.SplitContainer splitContainer;
        private DevExpress.XtraCharts.ChartControl chartControl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getXAxisRangeToolStripMenuItem;
        private System.Windows.Forms.Panel pnPaging;
        private DevExpress.XtraEditors.RangeControl rangeControl;
    }
}
