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
            this.label1 = new System.Windows.Forms.Label();
            this.txtPageSize = new System.Windows.Forms.TextBox();
            this.lblPages = new System.Windows.Forms.Label();
            this.btnMoveFirst = new System.Windows.Forms.Button();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.btnMoveLast = new System.Windows.Forms.Button();
            this.mnuDrawPotato = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDrawChartMinMax = new System.Windows.Forms.ToolStripMenuItem();
            this.drawChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyShowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDrawChart2D = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDrawChart1D = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileRead = new System.Windows.Forms.ToolStripMenuItem();
            this.pnPaging = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSeries2 = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSaveChart = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.chartResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.pnPaging.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(128, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Series";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSeries
            // 
            this.cbSeries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSeries.FormattingEnabled = true;
            this.cbSeries.Location = new System.Drawing.Point(92, 215);
            this.cbSeries.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cbSeries.Name = "cbSeries";
            this.cbSeries.Size = new System.Drawing.Size(210, 24);
            this.cbSeries.TabIndex = 3;
            this.cbSeries.SelectedIndexChanged += new System.EventHandler(this.cbSeries_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(407, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "PageSize";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // txtPageSize
            // 
            this.txtPageSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPageSize.Location = new System.Drawing.Point(433, 178);
            this.txtPageSize.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtPageSize.Name = "txtPageSize";
            this.txtPageSize.Size = new System.Drawing.Size(115, 23);
            this.txtPageSize.TabIndex = 5;
            this.txtPageSize.Visible = false;
            this.txtPageSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPageSize_KeyDown);
            // 
            // lblPages
            // 
            this.lblPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPages.AutoEllipsis = true;
            this.lblPages.Location = new System.Drawing.Point(423, 2);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(200, 20);
            this.lblPages.TabIndex = 4;
            this.lblPages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMoveFirst
            // 
            this.btnMoveFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveFirst.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMoveFirst.ForeColor = System.Drawing.Color.Black;
            this.btnMoveFirst.Location = new System.Drawing.Point(507, 177);
            this.btnMoveFirst.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnMoveFirst.Name = "btnMoveFirst";
            this.btnMoveFirst.Size = new System.Drawing.Size(49, 24);
            this.btnMoveFirst.TabIndex = 3;
            this.btnMoveFirst.Text = "<<";
            this.btnMoveFirst.UseVisualStyleBackColor = false;
            this.btnMoveFirst.Visible = false;
            this.btnMoveFirst.Click += new System.EventHandler(this.btnMoveFirst_Click);
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMoveLeft.ForeColor = System.Drawing.Color.Black;
            this.btnMoveLeft.Location = new System.Drawing.Point(526, 177);
            this.btnMoveLeft.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(49, 24);
            this.btnMoveLeft.TabIndex = 2;
            this.btnMoveLeft.Text = "<";
            this.btnMoveLeft.UseVisualStyleBackColor = false;
            this.btnMoveLeft.Visible = false;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveRight.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMoveRight.ForeColor = System.Drawing.Color.Black;
            this.btnMoveRight.Location = new System.Drawing.Point(562, 177);
            this.btnMoveRight.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(49, 24);
            this.btnMoveRight.TabIndex = 1;
            this.btnMoveRight.Text = ">";
            this.btnMoveRight.UseVisualStyleBackColor = false;
            this.btnMoveRight.Visible = false;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // btnMoveLast
            // 
            this.btnMoveLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveLast.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMoveLast.ForeColor = System.Drawing.Color.Black;
            this.btnMoveLast.Location = new System.Drawing.Point(581, 177);
            this.btnMoveLast.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnMoveLast.Name = "btnMoveLast";
            this.btnMoveLast.Size = new System.Drawing.Size(49, 24);
            this.btnMoveLast.TabIndex = 0;
            this.btnMoveLast.Text = ">>";
            this.btnMoveLast.UseVisualStyleBackColor = false;
            this.btnMoveLast.Visible = false;
            this.btnMoveLast.Click += new System.EventHandler(this.btnMoveLast_Click);
            // 
            // mnuDrawPotato
            // 
            this.mnuDrawPotato.Enabled = false;
            this.mnuDrawPotato.Name = "mnuDrawPotato";
            this.mnuDrawPotato.Size = new System.Drawing.Size(151, 22);
            this.mnuDrawPotato.Text = "DrawPotato";
            this.mnuDrawPotato.Click += new System.EventHandler(this.mnuDrawPotato_Click);
            // 
            // mnuDrawChartMinMax
            // 
            this.mnuDrawChartMinMax.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawChartToolStripMenuItem,
            this.propertyShowToolStripMenuItem});
            this.mnuDrawChartMinMax.Enabled = false;
            this.mnuDrawChartMinMax.Name = "mnuDrawChartMinMax";
            this.mnuDrawChartMinMax.Size = new System.Drawing.Size(151, 22);
            this.mnuDrawChartMinMax.Text = "MIN/MAX";
            // 
            // drawChartToolStripMenuItem
            // 
            this.drawChartToolStripMenuItem.Name = "drawChartToolStripMenuItem";
            this.drawChartToolStripMenuItem.ShowShortcutKeys = false;
            this.drawChartToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.drawChartToolStripMenuItem.Text = "Chart Draw";
            this.drawChartToolStripMenuItem.Click += new System.EventHandler(this.drawChartToolStripMenuItem_Click);
            // 
            // propertyShowToolStripMenuItem
            // 
            this.propertyShowToolStripMenuItem.Name = "propertyShowToolStripMenuItem";
            this.propertyShowToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.propertyShowToolStripMenuItem.Text = "Property Show";
            this.propertyShowToolStripMenuItem.Click += new System.EventHandler(this.propertyShowToolStripMenuItem_Click);
            // 
            // mnuDrawChart2D
            // 
            this.mnuDrawChart2D.Enabled = false;
            this.mnuDrawChart2D.Name = "mnuDrawChart2D";
            this.mnuDrawChart2D.Size = new System.Drawing.Size(151, 22);
            this.mnuDrawChart2D.Text = "DrawChart 2D";
            this.mnuDrawChart2D.Click += new System.EventHandler(this.mnuDrawChart2D_Click);
            // 
            // mnuDrawChart1D
            // 
            this.mnuDrawChart1D.Enabled = false;
            this.mnuDrawChart1D.Name = "mnuDrawChart1D";
            this.mnuDrawChart1D.Size = new System.Drawing.Size(151, 22);
            this.mnuDrawChart1D.Text = "DrawChart 1D";
            this.mnuDrawChart1D.Click += new System.EventHandler(this.mnuDrawChart1D_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(148, 6);
            // 
            // mnuFileRead
            // 
            this.mnuFileRead.Name = "mnuFileRead";
            this.mnuFileRead.Size = new System.Drawing.Size(151, 22);
            this.mnuFileRead.Text = "File Read";
            this.mnuFileRead.Click += new System.EventHandler(this.mnuFileRead_Click);
            // 
            // pnPaging
            // 
            this.pnPaging.BackColor = System.Drawing.Color.Transparent;
            this.pnPaging.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnPaging.Controls.Add(this.tableLayoutPanel1);
            this.pnPaging.Controls.Add(this.lblPages);
            this.pnPaging.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnPaging.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnPaging.Location = new System.Drawing.Point(0, 0);
            this.pnPaging.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.pnPaging.Name = "pnPaging";
            this.pnPaging.Size = new System.Drawing.Size(630, 94);
            this.pnPaging.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.85714F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.14286F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 380F));
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.44444F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.55556F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(628, 92);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoEllipsis = true;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(71, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Y";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(250, 3);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.repositoryItemImageComboBox1});
            this.tableLayoutPanel1.SetRowSpan(this.gridControl1, 3);
            this.gridControl1.Size = new System.Drawing.Size(375, 86);
            this.gridControl1.TabIndex = 44;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseUp;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "시리즈명";
            this.gridColumn1.ColumnEdit = this.repositoryItemComboBox1;
            this.gridColumn1.FieldName = "seriesName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "X축";
            this.gridColumn2.FieldName = "xAxis";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Y축";
            this.gridColumn3.FieldName = "yAxis";
            this.gridColumn3.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "차트타입";
            this.gridColumn4.FieldName = "chartType";
            this.gridColumn4.MinWidth = 80;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 80;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "선택구간(끝)";
            this.gridColumn5.FieldName = "SelectionEnd";
            this.gridColumn5.MinWidth = 80;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Width = 80;
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
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(71, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "X";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbSeries2
            // 
            this.cbSeries2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeries2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSeries2.FormattingEnabled = true;
            this.cbSeries2.Location = new System.Drawing.Point(92, 181);
            this.cbSeries2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cbSeries2.Name = "cbSeries2";
            this.cbSeries2.Size = new System.Drawing.Size(210, 24);
            this.cbSeries2.TabIndex = 10;
            this.cbSeries2.SelectedIndexChanged += new System.EventHandler(this.cbSeries2_SelectedIndexChanged);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Transparent;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReset.ForeColor = System.Drawing.Color.Black;
            this.btnReset.Location = new System.Drawing.Point(516, 177);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(63, 24);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
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
            this.contextMenuStrip.Size = new System.Drawing.Size(152, 148);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 6);
            // 
            // mnuSaveChart
            // 
            this.mnuSaveChart.Name = "mnuSaveChart";
            this.mnuSaveChart.Size = new System.Drawing.Size(151, 22);
            this.mnuSaveChart.Text = "Save Chart";
            this.mnuSaveChart.Click += new System.EventHandler(this.mnuSaveChart_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripMenuItem4,
            this.toolStripMenuItem6,
            this.toolStripMenuItem9,
            this.toolStripSeparator2,
            this.toolStripMenuItem10,
            this.chartResetToolStripMenuItem,
            this.toolStripMenuItem11});
            this.contextMenuStrip1.Name = "contextMenuStrip";
            this.contextMenuStrip1.Size = new System.Drawing.Size(152, 148);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(148, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Enabled = false;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItem4.Text = "DrawChart 1D";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.mnuDrawChart1D_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Enabled = false;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItem6.Text = "MIN/MAX";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.drawChartToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Enabled = false;
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItem9.Text = "DrawPotato";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.mnuDrawPotato_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(148, 6);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItem10.Text = "Save Chart";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.mnuSaveChart_Click);
            // 
            // chartResetToolStripMenuItem
            // 
            this.chartResetToolStripMenuItem.Name = "chartResetToolStripMenuItem";
            this.chartResetToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.chartResetToolStripMenuItem.Text = "Chart Reset";
            this.chartResetToolStripMenuItem.Click += new System.EventHandler(this.chartResetToolStripMenuItem_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(151, 22);
            this.toolStripMenuItem11.Text = "Copy Chart";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem11_Click);
            // 
            // DXChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pnPaging);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSeries);
            this.Controls.Add(this.btnMoveLast);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cbSeries2);
            this.Controls.Add(this.btnMoveRight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMoveLeft);
            this.Controls.Add(this.txtPageSize);
            this.Controls.Add(this.btnMoveFirst);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DXChartControl";
            this.Size = new System.Drawing.Size(630, 245);
            this.pnPaging.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSeries;
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
        private System.Windows.Forms.ToolStripMenuItem drawChartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertyShowToolStripMenuItem;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ComboBox cbSeries2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem chartResetToolStripMenuItem;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
