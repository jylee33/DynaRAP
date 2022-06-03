namespace DynaRAP.UControl
{
    partial class DXChartControl
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
            this.mnuDrawPotato = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDrawChartMinMax = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDrawChart2D = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDrawChart1D = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileRead = new System.Windows.Forms.ToolStripMenuItem();
            this.pnPaging = new System.Windows.Forms.Panel();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSaveChart = new System.Windows.Forms.ToolStripMenuItem();
            this.pnPaging.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Series";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSeries
            // 
            this.cbSeries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSeries.FormattingEnabled = true;
            this.cbSeries.Location = new System.Drawing.Point(6, 22);
            this.cbSeries.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbSeries.Name = "cbSeries";
            this.cbSeries.Size = new System.Drawing.Size(185, 28);
            this.cbSeries.TabIndex = 3;
            this.cbSeries.SelectedIndexChanged += new System.EventHandler(this.cbSeries_SelectedIndexChanged);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(325, 22);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(216, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "PageSize";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPageSize
            // 
            this.txtPageSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPageSize.Location = new System.Drawing.Point(219, 22);
            this.txtPageSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPageSize.Multiline = true;
            this.txtPageSize.Name = "txtPageSize";
            this.txtPageSize.Size = new System.Drawing.Size(100, 28);
            this.txtPageSize.TabIndex = 5;
            // 
            // lblPages
            // 
            this.lblPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPages.AutoEllipsis = true;
            this.lblPages.Location = new System.Drawing.Point(423, 3);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(175, 15);
            this.lblPages.TabIndex = 4;
            this.lblPages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMoveFirst
            // 
            this.btnMoveFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveFirst.Location = new System.Drawing.Point(423, 22);
            this.btnMoveFirst.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.btnMoveLeft.Location = new System.Drawing.Point(462, 22);
            this.btnMoveLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.btnMoveRight.Location = new System.Drawing.Point(526, 22);
            this.btnMoveRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.btnMoveLast.Location = new System.Drawing.Point(565, 22);
            this.btnMoveLast.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMoveLast.Name = "btnMoveLast";
            this.btnMoveLast.Size = new System.Drawing.Size(33, 28);
            this.btnMoveLast.TabIndex = 0;
            this.btnMoveLast.Text = ">>";
            this.btnMoveLast.UseVisualStyleBackColor = true;
            this.btnMoveLast.Click += new System.EventHandler(this.btnMoveLast_Click);
            // 
            // mnuDrawPotato
            // 
            this.mnuDrawPotato.Enabled = false;
            this.mnuDrawPotato.Name = "mnuDrawPotato";
            this.mnuDrawPotato.Size = new System.Drawing.Size(189, 22);
            this.mnuDrawPotato.Text = "DrawPotato";
            this.mnuDrawPotato.Click += new System.EventHandler(this.mnuDrawPotato_Click);
            // 
            // mnuDrawChartMinMax
            // 
            this.mnuDrawChartMinMax.Enabled = false;
            this.mnuDrawChartMinMax.Name = "mnuDrawChartMinMax";
            this.mnuDrawChartMinMax.Size = new System.Drawing.Size(189, 22);
            this.mnuDrawChartMinMax.Text = "DrawChart MIN/MAX";
            this.mnuDrawChartMinMax.Click += new System.EventHandler(this.mnuDrawChartMinMax_Click);
            // 
            // mnuDrawChart2D
            // 
            this.mnuDrawChart2D.Enabled = false;
            this.mnuDrawChart2D.Name = "mnuDrawChart2D";
            this.mnuDrawChart2D.Size = new System.Drawing.Size(189, 22);
            this.mnuDrawChart2D.Text = "DrawChart 2D";
            this.mnuDrawChart2D.Click += new System.EventHandler(this.mnuDrawChart2D_Click);
            // 
            // mnuDrawChart1D
            // 
            this.mnuDrawChart1D.Enabled = false;
            this.mnuDrawChart1D.Name = "mnuDrawChart1D";
            this.mnuDrawChart1D.Size = new System.Drawing.Size(189, 22);
            this.mnuDrawChart1D.Text = "DrawChart 1D";
            this.mnuDrawChart1D.Click += new System.EventHandler(this.mnuDrawChart1D_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(186, 6);
            // 
            // mnuFileRead
            // 
            this.mnuFileRead.Name = "mnuFileRead";
            this.mnuFileRead.Size = new System.Drawing.Size(189, 22);
            this.mnuFileRead.Text = "File Read";
            this.mnuFileRead.Click += new System.EventHandler(this.mnuFileRead_Click);
            // 
            // pnPaging
            // 
            this.pnPaging.BackColor = System.Drawing.Color.White;
            this.pnPaging.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.pnPaging.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnPaging.Location = new System.Drawing.Point(0, 0);
            this.pnPaging.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnPaging.Name = "pnPaging";
            this.pnPaging.Size = new System.Drawing.Size(603, 56);
            this.pnPaging.TabIndex = 6;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileRead,
            this.toolStripMenuItem1,
            this.mnuDrawChart1D,
            this.mnuDrawChart2D,
            this.mnuDrawChartMinMax,
            this.mnuDrawPotato,
            this.toolStripMenuItem2,
            this.mnuSaveChart});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(190, 170);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(186, 6);
            // 
            // mnuSaveChart
            // 
            this.mnuSaveChart.Name = "mnuSaveChart";
            this.mnuSaveChart.Size = new System.Drawing.Size(189, 22);
            this.mnuSaveChart.Text = "Save Chart";
            this.mnuSaveChart.Click += new System.EventHandler(this.mnuSaveChart_Click);
            // 
            // DXChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnPaging);
            this.Name = "DXChartControl";
            this.Size = new System.Drawing.Size(603, 150);
            this.pnPaging.ResumeLayout(false);
            this.pnPaging.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem mnuDrawPotato;
        private System.Windows.Forms.ToolStripMenuItem mnuDrawChartMinMax;
        private System.Windows.Forms.ToolStripMenuItem mnuDrawChart2D;
        private System.Windows.Forms.ToolStripMenuItem mnuDrawChart1D;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileRead;
        private System.Windows.Forms.Panel pnPaging;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveChart;
    }
}
