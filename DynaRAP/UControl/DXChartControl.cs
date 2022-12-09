using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraCharts;
using DynaRAP.Common;
using System.Drawing;
using DevExpress.Utils.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using DevExpress.XtraEditors;
using DynaRAP.Data;
using Microsoft.Scripting.Utils;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DynaRAP.UTIL;
using DevExpress.XtraBars.Docking;
using DynaRAP.Forms;

namespace DynaRAP.UControl
{
    public partial class DXChartControl : UserControl
    {
        #region enums
        public enum DrawTypes
        {
            DT_1D, DT_2D, DT_MINMAX, DT_POTATO, DT_UNKNOWN
        }
        #endregion

        #region private variables
        private SplitContainerControl m_spliter;
        private PropertyGrid m_propertyGrid;
        private ChartControl m_chart;
        private List<string> m_series;
        private string m_filename;
        private int m_pageIndex;
        private int m_pageSize;
        private int m_totalPages;
        private DataTable m_table;
        private DataTable max_table;
        private DrawTypes m_drawTypes;
        private List<Cluster> m_clusters = new List<Cluster>();
        private List<PotatoChartInfo> potatoChartInfoList = new List<PotatoChartInfo>();
        private List<PotatoChartInfo> crossChartInfoList = new List<PotatoChartInfo>();
        private int m_propertyGridWidth;

        private List<DLL_DATA> m_dllDatas;
        private Dictionary<string, List<SeriesPointData>> m_dicData;
        private Dictionary<string, PlotSeriesSource> plotSourceNameSeqDic;
        private List<dxGridData> dxGridDataList = new List<dxGridData>();
        private List<PlotPointData> selectPointList = new List<PlotPointData>();
        private PlotModuleControl plotModuleControl = null;
        private AdditionalResponse additionalResponse = null;
        private string tagValue = string.Empty;
        private List<string> timeKeyList = new List<string>();
        private List<string> allKeyList = new List<string>();
        DockPanel panel = null;
        DockPanel axisInfoPanel = null;

        private PlotAxisInfo plotAxisInfo = null;
        private string plotName = string.Empty;
        #endregion

        public DXChartControl()
        {
            InitializeComponent();

            m_drawTypes = DrawTypes.DT_UNKNOWN;

            m_series = new List<string>();
            m_filename = @"sampledata.xls";
            m_pageIndex = 0;
            m_pageSize = 100000;
            m_totalPages = 0;

            m_propertyGridWidth = 0;
        }

        public DXChartControl(AdditionalResponse additionalResponse ,PlotModuleControl plotModuleControl)
        {
            InitializeComponent();

            m_drawTypes = DrawTypes.DT_UNKNOWN;

            m_series = new List<string>();
            //m_filename = @"sampledata.xls";
            m_pageIndex = 0;
            m_pageSize = 100000;
            m_totalPages = 0;

            m_propertyGridWidth = 0;

            this.plotModuleControl = plotModuleControl;
            this.additionalResponse = additionalResponse;
        }
        
        public DXChartControl(AdditionalResponse additionalResponse, PlotModuleControl plotModuleControl, List<dxGridData> dxGridDataList, string tagValue, List<PlotPointData> selectPointList, PlotAxisInfo plotAxisInfo, string plotName)
        {
            InitializeComponent();

            m_drawTypes = DrawTypes.DT_UNKNOWN;

            m_series = new List<string>();
            //m_filename = @"sampledata.xls";
            m_pageIndex = 0;
            m_pageSize = 100000;
            m_totalPages = 0;

            m_propertyGridWidth = 0;

            this.plotModuleControl = plotModuleControl;
            this.additionalResponse = additionalResponse;
            this.dxGridDataList = dxGridDataList;
            this.tagValue = tagValue;
            this.selectPointList = selectPointList;
            this.plotAxisInfo = plotAxisInfo;
            this.plotName = plotName;
        }
        public DXChartControl DeepCopy(DXChartControl dXChartControl)
        {
            //InitializeComponent();
            DXChartControl newDXChartControl = new DXChartControl();
            //newDXChartControl.m_spliter = dXChartControl.m_spliter;
            //newDXChartControl.m_propertyGrid = dXChartControl.m_propertyGrid;
            //newDXChartControl.m_chart = dXChartControl.m_chart;
            //newDXChartControl.m_series = dXChartControl.m_series;
            newDXChartControl.m_filename = dXChartControl.m_filename;
            newDXChartControl.m_table = dXChartControl.m_table;
            newDXChartControl.max_table = dXChartControl.max_table;
            newDXChartControl.m_drawTypes = dXChartControl.m_drawTypes;
            newDXChartControl.m_clusters = dXChartControl.m_clusters;
            newDXChartControl.potatoChartInfoList = dXChartControl.potatoChartInfoList; 
            newDXChartControl.crossChartInfoList = dXChartControl.crossChartInfoList;
            newDXChartControl.m_propertyGridWidth = dXChartControl.m_propertyGridWidth;

            newDXChartControl.m_dllDatas = dXChartControl.m_dllDatas;
            newDXChartControl.m_dicData = dXChartControl.m_dicData;


            newDXChartControl.plotModuleControl = dXChartControl.plotModuleControl;
            newDXChartControl.additionalResponse = dXChartControl.additionalResponse;
            //dxGridData[] dxGridArray= new dxGridData[dXChartControl.dxGridDataList.Count];
            newDXChartControl.dxGridDataList = Utils.DeepClone(dXChartControl.dxGridDataList);
            //dXChartControl.dxGridDataList.CopyTo(dxGridArray);
            //newDXChartControl.dxGridDataList = dxGridArray.ToList() ;
            newDXChartControl.tagValue = Utils.DeepClone(dXChartControl.tagValue);

            newDXChartControl.plotAxisInfo = dXChartControl.plotAxisInfo.DeepCody();
        //newDXChartControl.m_chart = new ChartControl();
        //newDXChartControl.m_chart = (ChartControl)dXChartControl.m_chart.Clone();
        //newDXChartControl.m_chart.ContextMenuStrip = newDXChartControl.contextMenuStrip1;
        //newDXChartControl.m_chart.CustomPaint += newDXChartControl.Chart_CustomPaint;
        //newDXChartControl.m_chart.Dock = DockStyle.Fill;;
        //newDXChartControl.m_chart.CustomPaint += newDXChartControl.Chart_CustomPaint;

        //newDXChartControl.m_spliter = new SplitContainerControl();
        //newDXChartControl.m_spliter.MouseClick += newDXChartControl.Spliter_MouseClick;
        //newDXChartControl.m_spliter.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
        //newDXChartControl.m_spliter.Location = new Point(0, newDXChartControl.pnPaging.Location.Y + newDXChartControl.pnPaging.Height);
        //newDXChartControl.m_spliter.Size = new Size(newDXChartControl.ClientRectangle.Width, this.ClientRectangle.Height - newDXChartControl.pnPaging.Height);
        //newDXChartControl.m_spliter.CollapsePanel = SplitCollapsePanel.Panel2;
        //newDXChartControl.m_spliter.FixedPanel = SplitFixedPanel.Panel2;
        //newDXChartControl.m_spliter.SetPanelCollapsed(true);
        //newDXChartControl.m_spliter.SplitterPosition = 0;
        //newDXChartControl.Controls.Add(newDXChartControl.m_spliter);
        //newDXChartControl.m_chart.SelectionMode = (ElementSelectionMode)SelectionMode.MultiExtended;
        //newDXChartControl.m_spliter.Panel1.Controls.Add(newDXChartControl.m_chart);
            newDXChartControl.gridControl1.DataSource = newDXChartControl.dxGridDataList;
            newDXChartControl.gridView1.RefreshData();

            //this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            //this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
            //foreach (Series series in dXChartControl.m_chart.Series)
            //{
            //    //Series clonedSeries = (Series)Activator.CreateInstance(series.GetType());
            //    //clonedSeries.Points.AddRange(series.Points);
            //    ////clone.Diagram.Series.Add(clonedSeries);
            //    Series clonedSeries = (Series)series.Clone();
            //    newDXChartControl.m_chart.Series.Add(clonedSeries);

            //}

            //XYDiagram diagram = (XYDiagram)dXChartControl.m_chart.Diagram.Clone();

            //XYDiagram diagramNew = newDXChartControl.m_chart.Diagram as XYDiagram;

            //diagramNew = diagram;
            //newDXChartControl.m_chart.Series.AddRange(
            //dXChartControl.m_chart.Series.OfType<Series>().Select(s =>
            //{
            //    Series series = (Series)s.Clone();
            //    //series.Name += "Clone";
            //    //series.ChangeView(ViewType.Point);
            //    return series;
            //}).ToArray());

            //newDXChartControl.m_chart.Diagram = dXChartControl.m_chart.Diagram;

            //    newDXChartControl.Controls.Add(dXChartControl.m_spliter);

            //newDXChartControl.m_spliter.Panel1.Controls.Add(newDXChartControl.m_chart); 
            //newDXChartControl.m_spliter.Panel2.Controls.Add(newDXChartControl.m_propertyGrid);

            return newDXChartControl;
            //InitializeComponent();
        }


        private void chartControl_MouseDown(object sender, MouseEventArgs e)
        {
            ChartHitInfo hitInfo = this.m_chart.CalcHitInfo(e.Location);
            // if (hitInfo.InLegend && !hitInfo.InSeries) uncomment to select only by the labels click  
            {
                if (hitInfo.SeriesPoint == null)
                    return;


                if (this.selectPointList == null)
                {
                    this.selectPointList = new List<PlotPointData>();
                }
                dxGridData selectData = dxGridDataList.Find(x => x.seriesName == hitInfo.Series.ToString());
                if(selectData != null)
                {

                    PlotSeriesSource xData = null; 
                    PlotSeriesSource yData = null;
                    string sourceType = null;
                    if (selectData.xAxis != null)
                    {
                        this.plotSourceNameSeqDic.TryGetValue(selectData.xAxis, out xData);
                    }
                    this.plotSourceNameSeqDic.TryGetValue(selectData.yAxis, out yData);
                    PlotPointData plotPointData = null;
                    string xAxisDate = null;
                    if(hitInfo.SeriesPoint.DateTimeArgument != default(DateTime))
                    {
                        xAxisDate = Utils.GetJulianFromDate(hitInfo.SeriesPoint.DateTimeArgument);
                    }
                    else
                    {
                        xAxisDate = hitInfo.SeriesPoint.Argument.ToString();
                    }
                    switch (yData.sourceType)
                    {
                        case "shortblock":
                        case "parammodule":
                            sourceType = yData.sourceType.ToUpper();
                            break;
                        case "eq":
                            sourceType = "수식";
                            break;
                        case "part":
                            sourceType = "분할데이터";
                            break;
                        case "dll":
                            sourceType = "기준데이터";
                            break;
                        case "bintable":
                            sourceType = "BIN Table";
                            break;
                    }
                    int index = this.m_chart.Series[hitInfo.Series.ToString()].Points.IndexOf(hitInfo.SeriesPoint);
                    if (xData == null)
                    {
                        plotPointData = new PlotPointData(index, selectData.chartType, xAxisDate, null,null,null, null, hitInfo.SeriesPoint.Values[0].ToString() , yData.itemName,yData.seq, yData.originSeq, yData.sourceType, sourceType);
                    }
                    else
                    {
                        plotPointData = new PlotPointData(index, selectData.chartType, xAxisDate, xData.itemName, xData.seq, xData.originSeq,xData.sourceType, hitInfo.SeriesPoint.Values[0].ToString() , yData.itemName, yData.seq, yData.originSeq, yData.sourceType, sourceType);
                    }

                    if (plotPointData != null)
                    {
                        this.selectPointList.Add(plotPointData);
                        MainForm mainForm = this.ParentForm as MainForm;
                        mainForm.SelectPointTableControl.AddPlotPointData(plotPointData);
                        this.gridControl2.DataSource = this.selectPointList;
                        this.gridView2.RefreshData();
                    }
                }
                //dxGridData data = new dxGridData(hitInfo.SeriesPoint.Argument.ToString(), hitInfo.SeriesPoint.Values[0].ToString());
               


                this.m_chart.ClearSelection();
                //this.m_chart.SelectedItems.Add(this.m_chart.Series[0].Points.FirstOrDefault(r => r.ArgumentX.NumericalArgument == hitInfo.SeriesPoint.NumericalArgument));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //var temp = this.Size;
            this.Size = new Size(625, 300);
            m_spliter = new SplitContainerControl();

                m_spliter.MouseClick += Spliter_MouseClick;
                m_spliter.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                m_spliter.Location = new Point(this.tableLayoutPanel3.Location.X + this.tableLayoutPanel3.Width, this.pnPaging.Location.Y + this.pnPaging.Height);
                m_spliter.Size = new Size(this.ClientRectangle.Width - this.tableLayoutPanel3.Width, this.ClientRectangle.Height - this.pnPaging.Height);
                m_spliter.CollapsePanel = SplitCollapsePanel.Panel2;
                m_spliter.FixedPanel = SplitFixedPanel.Panel2;
                m_spliter.SetPanelCollapsed(true);
                m_spliter.SplitterPosition = 0;
                this.Controls.Add(m_spliter);
            if (this.m_chart == null)
            {
                this.m_chart = new ChartControl();

            }

            this.m_chart.Dock = DockStyle.Fill;
            this.m_chart.ContextMenuStrip = this.contextMenuStrip1;
            this.m_chart.CustomPaint += Chart_CustomPaint;
            this.m_chart.MouseDown += chartControl_MouseDown;
            this.m_chart.SelectionMode = (ElementSelectionMode)SelectionMode.MultiSimple;
            m_spliter.Panel1.Controls.Add(this.m_chart);
            m_propertyGrid = new PropertyGrid();

            m_propertyGrid.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            m_propertyGrid.Location = new Point(0, 0);
            m_propertyGrid.Size = new Size(m_spliter.Panel2.Width, m_spliter.Panel2.Height);
            m_propertyGrid.SelectedObject = this.m_chart;
            m_spliter.Panel2.Controls.Add(m_propertyGrid);
            this.cbSeries.Enabled = false;

            this.plotSourceNameSeqDic = new Dictionary<string, PlotSeriesSource>();
            this.m_dicData = ReadDataList4();
            //SetDllDatas();
            this.cbSeries.Enabled = true;

            //mnuDrawChart1D.Enabled =
            //    mnuDrawChart2D.Enabled =
            //    mnuDrawChartMinMax.Enabled =
            //    mnuDrawPotato.Enabled = this.cbSeries.Items.Count > 0;

            //toolStripMenuItem4.Enabled =
            //   toolStripMenuItem6.Enabled =
            //   toolStripMenuItem9.Enabled = this.cbSeries.Items.Count > 0;

            InitGridControl1();
            if(this.dxGridDataList != null && this.dxGridDataList.Count != 0)
            {
                if (this.dxGridDataList[0].chartType == "1D-Time History")
                {
                    this.repositoryItemComboBox2.Items.Clear();
                    this.repositoryItemComboBox2.Items.Add("");
                    this.repositoryItemComboBox2.Items.AddRange(timeKeyList);
                }
                if (this.dxGridDataList[0].chartType == "Potato Plot")
                {
                    GridColumn colYAxis = gridView1.Columns["xAxis"];
                    colYAxis.OptionsColumn.ReadOnly = false;
                }
                this.gridControl1.DataSource = this.dxGridDataList;
               
                this.gridView1.RefreshData();
                DrawSeries();
            }
            InitGridControl2();
            if(this.selectPointList != null && this.selectPointList.Count != 0)
            {
                this.gridControl2.DataSource = this.selectPointList;

                this.gridView2.RefreshData();
            }
        }

        private void InitGridControl1()
        {
            //this.repositoryItemComboBox1.PopupFormMinSize = new System.Drawing.Size(0, 200);

            this.repositoryItemComboBox1.Items.Add("1D-Time History");
            this.repositoryItemComboBox1.Items.Add("Cross Plot");
            this.repositoryItemComboBox1.Items.Add("Potato Plot");
            this.repositoryItemComboBox1.SelectedIndexChanged += RepositoryItemComboBox1_SelectedIndexChanged;

            //선타입
            this.repositoryItemComboBox3.Items.Add("Line");
            this.repositoryItemComboBox3.Items.Add("Dot");

            //선색상
            this.repositoryItemComboBox4.Items.Add("Red");
            this.repositoryItemComboBox4.Items.Add("Orange");
            this.repositoryItemComboBox4.Items.Add("Yellow");
            this.repositoryItemComboBox4.Items.Add("Green");
            this.repositoryItemComboBox4.Items.Add("Blue");
            this.repositoryItemComboBox4.Items.Add("Purple");
            this.repositoryItemComboBox4.Items.Add("Indigo");
            this.repositoryItemComboBox4.Items.Add("Black");
            //선굵기
            this.repositoryItemComboBox5.Items.Add("1");
            this.repositoryItemComboBox5.Items.Add("2");
            this.repositoryItemComboBox5.Items.Add("3");
            this.repositoryItemComboBox5.Items.Add("4");
            this.repositoryItemComboBox5.Items.Add("5");
            this.repositoryItemComboBox5.Items.Add("6");
            this.repositoryItemComboBox5.Items.Add("7");
            this.repositoryItemComboBox5.Items.Add("8");
            this.repositoryItemComboBox5.Items.Add("9");
            this.repositoryItemComboBox5.Items.Add("10");

            GridColumn colDelet = gridView1.Columns["delete"];
            colDelet.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDelet.OptionsColumn.FixedWidth = true;
            colDelet.Width = 40;
            colDelet.Caption = "삭제";
            colDelet.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;

        }


        private void InitGridControl2()
        {
          
            GridColumn colDelet = gridView2.Columns["delete"];
            colDelet.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDelet.OptionsColumn.FixedWidth = true;
            colDelet.Width = 40;
            colDelet.Caption = "삭제";
            colDelet.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox2.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox2.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox2.Click += RepositoryItemImageComboBox2_Click;

        }
        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
                dxGridData selectDxGridData = (dxGridData)gridView1.GetFocusedRow();
                dxGridDataList.Remove(selectDxGridData);
           
                this.gridControl1.DataSource = dxGridDataList;
                //gridView2.DeleteRow(gridView2.FocusedRowHandle);
                this.gridView1.RefreshData();
        }

        private void RepositoryItemImageComboBox2_Click(object sender, EventArgs e)
        {
            PlotPointData selectDxGridData = (PlotPointData)gridView2.GetFocusedRow();
            selectPointList.Remove(selectDxGridData);
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.SelectPointTableControl.RemovePlotPointData(selectDxGridData);
            
            this.gridControl2.DataSource = selectPointList;
            //gridView2.DeleteRow(gridView2.FocusedRowHandle);
            this.gridView2.RefreshData();
        }



        private void RepositoryItemComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxEdit;
            if (combo.SelectedIndex != -1)
            {
                string selectChartType = combo.SelectedItem as string;
                this.repositoryItemComboBox2.Items.Clear();
                if (selectChartType == "1D-Time History" )
                {
                    this.repositoryItemComboBox2.Items.Add("");
                    this.repositoryItemComboBox2.Items.AddRange(timeKeyList);
                    
                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxis", null);
                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxisSourceSeq", null);
                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxisSourceType", null);

                    GridColumn colYAxis = gridView1.Columns["xAxis"];
                    colYAxis.OptionsColumn.ReadOnly = false;
                }
                else if (selectChartType == "Potato Plot")
                {
                    this.repositoryItemComboBox2.Items.AddRange(allKeyList);

                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxis", null);
                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxisSourceSeq", null);
                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxisSourceType", null);

                    GridColumn colYAxis = gridView1.Columns["xAxis"];
                    colYAxis.OptionsColumn.ReadOnly = true;
                }
                else
                {
                    this.repositoryItemComboBox2.Items.AddRange(allKeyList);

                    GridColumn colYAxis = gridView1.Columns["xAxis"];
                    colYAxis.OptionsColumn.ReadOnly = false ;
                }
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "chartType", selectChartType);
            }
        }

        private void RepositoryItemComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxEdit;
            if (combo.SelectedIndex != -1)
            {
                PlotSeriesSource plotSeriesSource;
                this.plotSourceNameSeqDic.TryGetValue(combo.Text, out plotSeriesSource);
                if (plotSeriesSource != null)
                {
                    this.gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxisSourceSeq", plotSeriesSource.seq);
                    this.gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxisSourceType", plotSeriesSource.sourceType);
                }
                else
                {
                    this.gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxisSourceSeq", null);
                    this.gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "xAxisSourceType", null);
                }
            }
        }
        private void RepositoryItemComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxEdit;
            if (combo.SelectedIndex != -1)
            {
                PlotSeriesSource plotSeriesSource;
                this.plotSourceNameSeqDic.TryGetValue(combo.Text, out plotSeriesSource);
                if (plotSeriesSource != null)
                {
                   this.gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "yAxisSourceSeq", plotSeriesSource.seq);
                   this.gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "yAxisSourceType", plotSeriesSource.sourceType);
                }
            }
        }

        private void Spliter_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_spliter.SplitterPosition == 0)
                m_spliter.SplitterPosition = m_spliter.Width * 2 / 3;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (null == m_spliter || null == m_propertyGrid)
                return;
        }

        private void InitPropertyControl()
        {
            //Splitter splitter = new Splitter();
            //splitter.Width = 10;
            //splitter.Dock = DockStyle.Right;
            //splitter.Visible = false;
            //this.Controls.Add(splitter);

            //PropertyGrid propertyGrid = new PropertyGrid();
            //propertyGrid.SelectedObject = this.m_chart;
            //propertyGrid.Dock = DockStyle.Right;
            //propertyGrid.Width = 300;
            //propertyGrid.Visible = false;

            //this.Controls.Add(propertyGrid);
            //this.m_chart.BringToFront();
        }

        private void RefreshUI()
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(Splitter) || c.GetType() == typeof(PropertyGrid))
                    c.Visible = m_drawTypes.Equals(DrawTypes.DT_MINMAX) && propertyShowToolStripMenuItem.Checked;
            }
        }

        /// <summary>
        /// 개발을 위하여 샘플로 사용한 데이터
        /// 실 데이터는 파일을 읽어 오도록 작업 함.
        /// </summary>
        private void SetDllDatas()
        {
            m_dllDatas = new List<DLL_DATA>();

            var data = new DLL_DATA();
            data.Name = "LH Wing BL1870";
            data.ButtockLine = "1870";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW901P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW903P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW905P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            data = new DLL_DATA();
            data.Name = "LH Wing BL2955";
            data.ButtockLine = "2955";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW907P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW909P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW911P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            data = new DLL_DATA();
            data.Name = "LH Wing BL3405";
            data.ButtockLine = "3405";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW913P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW915P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW917P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            data = new DLL_DATA();
            data.Name = "LH Wing BL4385";
            data.ButtockLine = "4385";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW919P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW921P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW923P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            data = new DLL_DATA();
            data.Name = "LH Wing BL5160";
            data.ButtockLine = "5160";
            data.parameters = new List<DLL_DATA.Parameter>()
            {
                new DLL_DATA.Parameter() { Name = "SW925P", Type = "V", Unit = "N"},
                new DLL_DATA.Parameter() { Name = "SW927P", Type = "T", Unit = "N-m"},
                new DLL_DATA.Parameter() { Name = "SW929P", Type = "BM", Unit = "N-m"},
            };
            m_dllDatas.Add(data);

            //ReadDataTable(m_filename);



            foreach (DLL_DATA dll in m_dllDatas)
            {
                foreach (DLL_DATA.Parameter param in dll.parameters)
                {
                    foreach (string key in this.m_dicData.Keys)
                    {
                        if (key.Contains(param.Name))
                        {
                            List<SeriesPointData> spData;
                            this.m_dicData.TryGetValue(key, out spData);

                            if (null == param.data)
                            {
                                param.data = new DataTable();
                                param.data.Columns.Add("Series", typeof(string));
                                param.data.Columns.Add("Argument", typeof(int));
                                param.data.Columns.Add("Value", typeof(double));
                            }

                            foreach (SeriesPointData d in spData)
                            {
                                DataRow row = param.data.NewRow();
                                row["Series"] = d.SeriesName;
                                row["Argument"] = d.Argument;
                                row["Value"] = d.Value;
                                param.data.Rows.Add(row);

                                if (param.MinVal == 0)
                                    param.MinVal = d.Value;
                                else if (param.MinVal > d.Value)
                                    param.MinVal = d.Value;

                                if (param.MaxVal == 0)
                                    param.MaxVal = d.Value;
                                else if (param.MaxVal < d.Value)
                                    param.MaxVal = d.Value;
                            }

                            break;
                        }
                    }
                }
            }
        }

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt">데이터 테이블</param>
        /// <param name="drawTypes">그래프 타입/param>
        /// <param name="seriesName">시리즈명(1D, 2D만 해당 됨)</param>
        /// <param name="axisX">X축 타이틀</param>
        /// <param name="axisY">Y축 타이틀</param>
        /// <param name="pageIndex">Page 번호</param>
        /// <param name="pageSize">Page 당 표출 사이즈</param>
        public void DrawChart(
            DataTable dt,
            DrawTypes drawTypes = DrawTypes.DT_UNKNOWN,
            string seriesName = "",
            string axisX = "",
            string axisY = "",
            int pageIndex = 0,
            int pageSize = 50000)
        {
            this.m_table = dt;

            //if (this.m_chart.Series.Count > 0)
            //    this.m_chart.Series.Clear();

            //this.pnPaging.Visible = drawTypes.Equals(DrawTypes.DT_1D) || drawTypes.Equals(DrawTypes.DT_2D);
          
            switch (drawTypes)
            {
                case DrawTypes.DT_1D:
                    {
                        DrawChart_1D();
                    }
                    break;

                case DrawTypes.DT_2D:
                    {
                        DrawChart_2D();
                    }
                    break;

                case DrawTypes.DT_MINMAX:
                    {
                        DrawChart_MinMax();
                    }
                    break;

                case DrawTypes.DT_POTATO:
                    {
                        DrawChart_Potato();
                    }
                    break;
            }
            this.gridControl1.DataSource = dxGridDataList;
            this.gridView1.RefreshData();
        }

        //public void DrawChart(DataTable table, string seriesName = "Series1")
        //{
        //    if(null != table)
        //        m_table = table;

        //    this.pnPaging.Visible = true;

        //    if (this.m_chart.Series.Count > 0)
        //        this.m_chart.Series.Clear();

        //    this.m_chart.Series.Add(new Series(seriesName, ViewType.Line));
        //    this.m_chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

        //    DrawChart_1D();
        //}

        //public void DrawChart(DrawTypes type, string seriesStr, string chartTitle, List<SeriesPointData> points, string titleX = "", string titleY = "", int pageIndex = 0, int pageSize = 50000)
        //{
        //    this.pnPaging.Visible = false;

        //    //RefreshUI();

        //    if (this.m_chart.Series.Count > 0)
        //        this.m_chart.Series.Clear();

        //    //DrawChart(MakeTableData(points));

        //    //return;

        //    switch (type)
        //    {
        //        case DrawTypes.DT_1D:
        //            {
        //                this.pnPaging.Visible = true;
        //                this.m_chart.Series.Add(new Series(seriesStr, ViewType.Line));

        //                DrawChart_1D();
        //            }
        //            break;
        //        case DrawTypes.DT_2D:
        //            {
        //                this.pnPaging.Visible = true;
        //                this.m_chart.Series.Add(new Series(seriesStr, ViewType.Line));

        //                DrawChart_2D();
        //            }
        //            break;
        //        case DrawTypes.DT_MINMAX:
        //            {
        //                DrawChart_MinMax();
        //            }
        //            break;

        //        case DrawTypes.DT_POTATO:
        //            {
        //                DrawChart_Potato();
        //            }
        //            break;
        //    }
        //}
        #endregion

        #region 1D/2D

        private DataTable MakeTableData(List<SeriesPointData> points)
        {
            if (null == m_table)
                m_table = new DataTable();

            m_table.Columns.Clear();
            m_table.Rows.Clear();

            int index = 0;

            foreach (SeriesPointData point in points)
            {
                if (0 == index)
                {
                    m_table.Columns.Add("Argument", typeof(int));
                    m_table.Columns.Add("Value", typeof(double));
                }

                DataRow row = m_table.NewRow();
                row["Argument"] = point.Argument;
                row["Value"] = point.Value;
                m_table.Rows.Add(row);

                index++;
            }

            return m_table;
        }

        private DataTable MakeMaxTableData(List<SeriesPointData> points)
        {
            if (null == max_table)
                max_table = new DataTable();

            max_table.Columns.Clear();
            max_table.Rows.Clear();

            int index = 0;

            foreach (SeriesPointData point in points)
            {
                if (0 == index)
                {
                    max_table.Columns.Add("Argument", typeof(int));
                    max_table.Columns.Add("Value", typeof(double));
                }

                DataRow row = max_table.NewRow();
                row["Argument"] = point.Argument;
                row["Value"] = point.Value;
                max_table.Rows.Add(row);

                index++;
            }

            return max_table;
        }

        private void DrawChart_1D(string axisTitleX = "", int pageIndex = 0, int pageSize = 50000)
        {
            //if(this.m_chart.Series.Count == 0)
            //    this.m_chart.Series.Add(new Series("Series", ViewType.Line));
            Series series = new Series("Series", ViewType.Line);
            //var dataSource = this.m_table.AsEnumerable().Skip(m_pageSize * m_pageIndex).Take(this.m_pageSize);
            var dataSource = this.m_table.AsEnumerable().ToList();

            //this.m_chart.Series[0].Points.Clear();

            foreach (DataRow row in dataSource)
                series.Points.Add(new SeriesPoint(row["Argument"], row["Value"]));
            //this.m_chart.Series[0].Points.Add(new SeriesPoint(row["Argument"], row["Value"]));

            series.ArgumentScaleType = ScaleType.Auto;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            this.m_chart.Series.Add(series);
            this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
            //this.m_chart.Series[0].ArgumentScaleType = ScaleType.Auto;
            //this.m_chart.Series[0].ArgumentDataMember = "Argument";
            //this.m_chart.Series[0].ValueScaleType = ScaleType.Numerical;
            //this.m_chart.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });

            //this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            //this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
            //dxGridDataList.Add(new dxGridData("series", "1D", this.cbSeries.Text));

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

            diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            //diagram.AxisY.NumericScaleOptions.AutoGrid = false;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.WholeRange.SideMarginsValue = 0L;

            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.DateTimeScaleOptions.GridOffset = 1;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
            //diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffff}";

            //this.btnMoveFirst.Enabled = this.btnMoveLeft.Enabled = (this.m_pageIndex > 0);
            //this.btnMoveLast.Enabled = this.btnMoveRight.Enabled = (this.m_pageIndex < this.m_totalPages);

            //this.lblPages.Text = string.Format("{0} page of {1} pages.", this.m_pageIndex + 1, this.m_totalPages + 1);
            //m_pageIndex = pageIndex;
            //m_pageSize = pageSize;
        }

        private void DrawChart_2D(string axisTitleX = "", string axisTitleY = "", int pageIndex = 0, int pageSize = 50000)
        {
            //if (this.m_chart.Series.Count == 0)
            //    this.m_chart.Series.Add(new Series("Series", ViewType.Line));
            Series series = new Series("Series", ViewType.Line);
            //var dataSource = this.m_table.AsEnumerable().Skip(m_pageSize * m_pageIndex).Take(this.m_pageSize);
            var dataSource = this.m_table.AsEnumerable().ToList();
            //this.m_chart.Series[0].Points.Clear();

            //foreach (DataRow row in dataSource)
            //    this.m_chart.Series[0].Points.Add(new SeriesPoint(row["Argument"], row["Value"]));

            //this.m_chart.Series[0].ArgumentScaleType = ScaleType.Auto;
            //this.m_chart.Series[0].ArgumentDataMember = "Argument";
            //this.m_chart.Series[0].ValueScaleType = ScaleType.Numerical;
            //this.m_chart.Series[0].ValueDataMembers.AddRange(new string[] { "Value" });

            foreach (DataRow row in dataSource)
                series.Points.Add(new SeriesPoint(row["Argument"], row["Value"]));

            series.ArgumentScaleType = ScaleType.Auto;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            this.m_chart.Series.Add(series);
            this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
            //dxGridDataList.Add(new dxGridData("series", "2D", this.cbSeries.Text, this.cbSeries2.Text));

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

            diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.Default;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.WholeRange.SideMarginsValue = 0L;

            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.DateTimeScaleOptions.GridOffset = 1;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
            diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffff}";

            //this.btnMoveFirst.Enabled = this.btnMoveLeft.Enabled = (this.m_pageIndex > 0);
            //this.btnMoveLast.Enabled = this.btnMoveRight.Enabled = (this.m_pageIndex < this.m_totalPages);

            //this.lblPages.Text = string.Format("{0} page of {1} pages.", this.m_pageIndex + 1, this.m_totalPages + 1);

            //m_pageIndex = pageIndex;
            //m_pageSize = pageSize;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (this.m_drawTypes == DrawTypes.DT_1D)
            {
                if (string.IsNullOrEmpty(this.txtPageSize.Text))
                {
                    this.txtPageSize.Focus();
                    return;
                }                

                List<SeriesPointData> spDatas;
                this.m_dicData.TryGetValue(this.cbSeries.Text, out spDatas);

                DataTable dt = MakeTableData(spDatas);

                this.m_pageIndex = 0;
                this.m_pageSize = Convert.ToInt32(this.txtPageSize.Text);
                this.m_totalPages = dt.Rows.Count / this.m_pageSize;

                DrawChart(dt, this.m_drawTypes);
            }
        }

        private void btnMoveFirst_Click(object sender, EventArgs e)
        {
            this.m_pageIndex = 0;

            if (this.m_drawTypes == DrawTypes.DT_1D)
                DrawChart_1D();
            else if (m_drawTypes == DrawTypes.DT_2D)
                DrawChart_2D();
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            if (this.m_pageIndex > 0)
                this.m_pageIndex--;

            if (this.m_drawTypes == DrawTypes.DT_1D)
                DrawChart_1D();
            else if (m_drawTypes == DrawTypes.DT_2D)
                DrawChart_2D();
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            if (this.m_pageIndex < this.m_totalPages)
                this.m_pageIndex++;

            if (this.m_drawTypes == DrawTypes.DT_1D)
                DrawChart_1D();
            else if (m_drawTypes == DrawTypes.DT_2D)
                DrawChart_2D();
        }

        private void btnMoveLast_Click(object sender, EventArgs e)
        {
            this.m_pageIndex = this.m_totalPages;

            if (this.m_drawTypes == DrawTypes.DT_1D)
                DrawChart_1D();
            else if (m_drawTypes == DrawTypes.DT_2D)
                DrawChart_2D();
        }
        #endregion

        #region MinMax
        private DataTable MakeMinMaxData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Operation", typeof(string));
            table.Columns.Add("Argument", typeof(double));
            table.Columns.Add("Value", typeof(double));

            foreach (DLL_DATA d in m_dllDatas)
            {
                foreach (DLL_DATA.Parameter p in d.parameters)
                {
                    if (p.Type.Equals("V"))
                    {
                        DataRow row = table.NewRow();
                        row["Operation"] = "Minimum";
                        row["Argument"] = double.Parse(d.ButtockLine);
                        row["Value"] = p.MinVal;
                        table.Rows.Add(row);

                        row = table.NewRow();
                        row["Operation"] = "Maximum";
                        row["Argument"] = double.Parse(d.ButtockLine);
                        row["Value"] = p.MaxVal;
                        table.Rows.Add(row);
                    }
                }
            }

            return table;
        }
        //private void DrawChart_MinMax(string title = "", string axisTitleX = "", string axisTitleY = "")
        //{
        //    Dictionary<string, Series> seriesInfo = new Dictionary<string, Series>();
        //    Series series;
        //    foreach (DataRow row in m_table.Rows)
        //    {
        //        string operation = row["Operation"].ToString();
        //        if (seriesInfo.TryGetValue(operation, out series) == false)
        //        {
        //            series = new Series(operation, ViewType.Line);
        //            seriesInfo.Add(operation, series);

        //            series.ArgumentDataMember = "Argument";
        //            series.ArgumentScaleType = ScaleType.Numerical;
        //            series.ValueDataMembers.AddRange(new string[] { "Value" });
        //            series.ValueScaleType = ScaleType.Numerical;

        //            this.m_chart.Series.Add(series);
        //        }

        //        double parameter = double.Parse(row["Argument"].ToString());
        //        double minmax = double.Parse(row["Value"].ToString());
        //        SeriesPoint point = new SeriesPoint(parameter, minmax);
        //        series.Points.Add(point);
        //    }

        //    XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

        //    diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.Default;

        //    diagram.EnableAxisXScrolling = true;
        //    diagram.EnableAxisXZooming = true;
        //    diagram.AxisX.WholeRange.SideMarginsValue = 0L;

        //    diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Manual;
        //    diagram.AxisX.NumericScaleOptions.MeasureUnit = NumericMeasureUnit.Ones;
        //    diagram.AxisX.NumericScaleOptions.GridOffset = 1;
        //    diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Thousands;
        //    diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
        //    diagram.AxisX.Label.TextPattern = "{A}";
        //}
        #endregion

        private void DrawChart_MinMax(string title = "", string axisTitleX = "", string axisTitleY = "")
        {
            //this.m_chart.Series.Clear();
            //this.m_chart.Series.Add(new Series("Min", ViewType.Line));
            //this.m_chart.Series.Add(new Series("MaX", ViewType.Line));

            //var minDataSource = this.m_table.AsEnumerable().Skip(m_pageSize * m_pageIndex).Take(this.m_pageSize).ToList();
            //var maxDataSource = this.max_table.AsEnumerable().Skip(m_pageSize * m_pageIndex).Take(this.m_pageSize).ToList();

            Series series = new Series("Min-Max", ViewType.Line);

            var minDataSource = this.m_table.AsEnumerable().ToList();
            var maxDataSource = this.max_table.AsEnumerable().ToList();

            //this.m_chart.Series[0].Points.Clear();
            if(minDataSource.Count() != maxDataSource.Count())
            {
                MessageBox.Show(new Form { TopMost = true }, "Min,Max 데이터의 개수가 다릅니다.");
                return;
            }
            for(int i=0; i < minDataSource.Count(); i++)
            {
                series.Points.Add(new SeriesPoint(minDataSource[i]["Value"], maxDataSource[i]["Value"]));
            }
            //foreach (DataRow row in minDataSource)
            //    this.m_chart.Series[0].Points.Add(new SeriesPoint(row["Argument"], row["Value"]));

            series.ArgumentScaleType = ScaleType.Auto;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            this.m_chart.Series.Add(series);

            this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;

            //this.m_chart.Series[1].Points.Clear();

            //foreach (DataRow row in maxDataSource)
            //    this.m_chart.Series[1].Points.Add(new SeriesPoint(row["Argument"], row["Value"]));

            //this.m_chart.Series[1].ArgumentScaleType = ScaleType.Auto;
            //this.m_chart.Series[1].ArgumentDataMember = "Argument";
            //this.m_chart.Series[1].ValueScaleType = ScaleType.Numerical;
            //this.m_chart.Series[1].ValueDataMembers.AddRange(new string[] { "Value" });

            //this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            //this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;


            //Dictionary<string, Series> seriesInfo = new Dictionary<string, Series>();
            //Series series;

            //foreach (DataRow row in m_table.Rows)
            //{
            //    string operation = row["Operation"].ToString();
            //    if (seriesInfo.TryGetValue(operation, out series) == false)
            //    {
            //        series = new Series(operation, ViewType.Line);
            //        seriesInfo.Add(operation, series);

            //        series.ArgumentDataMember = "Argument";
            //        series.ArgumentScaleType = ScaleType.Numerical;
            //        series.ValueDataMembers.AddRange(new string[] { "Value" });
            //        series.ValueScaleType = ScaleType.Numerical;

            //        this.m_chart.Series.Add(series);
            //    }

            //    double parameter = double.Parse(row["Argument"].ToString());
            //    double minmax = double.Parse(row["Value"].ToString());
            //    SeriesPoint point = new SeriesPoint(parameter, minmax);
            //    series.Points.Add(point);
            //}
            //dxGridDataList.Add(new dxGridData("series", "Min-Max", this.cbSeries.Text, this.cbSeries2.Text));

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

            diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.Default;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.WholeRange.SideMarginsValue = 0L;

            diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Manual;
            diagram.AxisX.NumericScaleOptions.MeasureUnit = NumericMeasureUnit.Ones;
            diagram.AxisX.NumericScaleOptions.GridOffset = 1;
            diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Thousands;
            diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
            diagram.AxisX.Label.TextPattern = "{A}";
        }


        #region Potatao
        private void Chart_CustomPaint(object sender, CustomPaintEventArgs e)
        {
            //if (this.m_drawTypes != DrawTypes.DT_POTATO)
            //    return;

            DXCustomPaintEventArgs args = e as DXCustomPaintEventArgs;
            if (args == null)
                return;
            GraphicsCache g = args.Cache;
            //if (m_clusters.Count == 0)
            //{
            //    return;
            //}

            if (this.m_chart.Diagram == null)
            {
                return;
            }
            foreach(var potatoChart in potatoChartInfoList)
            {
                g.ClipInfo.SetClip(CalculateDiagramBounds());
                g.SmoothingMode = SmoothingMode.AntiAlias;

                DrawClusterPotato(potatoChart, g);
            }
            foreach(var crossChart in crossChartInfoList)
            {
                g.ClipInfo.SetClip(CalculateDiagramBounds());
                g.SmoothingMode = SmoothingMode.AntiAlias;

                DrawClusterCross(crossChart, g);

            }
            //for (int i = 0; i < m_clusters.Count; i++)
            //{
            //    DrawCluster(m_clusters[i], g, potatoColor, potatoColor);
            //}
        }

        private DataTable MakePotatoData(string seriesName)
        {
            var data = m_dllDatas.Find(f => f.ButtockLine.Equals(seriesName));
            var data_T = data.parameters.Find(f => f.Type.Equals("T"));
            var data_BM = data.parameters.Find(f => f.Type.Equals("BM"));

            DataTable table = new DataTable();
            table.Columns.Add("Operation", typeof(string));
            table.Columns.Add("Argument", typeof(double));
            table.Columns.Add("Value", typeof(double));

            for (int i = 0; i < data_T.data.Rows.Count; i++)
            {
                DataRow row = table.NewRow();
                row["Operation"] = seriesName;
                row["Argument"] = data_T.data.Rows[i]["Value"];
                row["Value"] = data_BM.data.Rows[i]["Value"];
                table.Rows.Add(row);
            }

            return table;
        }

        private DataTable MakePotatoDataNew(List<SeriesPointData> seriesX, List<SeriesPointData> seriesY)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Operation", typeof(string));
            table.Columns.Add("Argument", typeof(double));
            table.Columns.Add("Value", typeof(double));

            int index = seriesX.Count() < seriesY.Count() ? seriesX.Count() : seriesY.Count();
                Random random = new Random();

            for (int i = 0; i < index; i++)
            {
                DataRow row = table.NewRow();
                row["Operation"] = "test";
                //row["Argument"] = seriesX[i].Value;
                //row["Value"] = seriesY[i].Value;
                var t1 = random.Next(0, 500);
                var t2 = random.Next(0, 500); ;
                row["Argument"] = t1;
                row["Value"] = t2;
                table.Rows.Add(row);
            }
            DataRow row1 = table.NewRow();
            row1["Operation"] = "test1";
            row1["Argument"] = 250.0;
            row1["Value"] = 250.0;
            table.Rows.Add(row1);

            return table;
        }

        private void DrawChart_Potato(string title = "", string axisTitleX = "", string axisTitleY = "")
        {
            //dxGridDataList.Add(new dxGridData("series", "Potato", this.cbSeries.Text, this.cbSeries2.Text));

            Dictionary<string, Series> seriesInfo = new Dictionary<string, Series>();
            Series series;
            foreach (DataRow row in m_table.Rows)
            {
                string operation = row["Operation"].ToString();
                if (seriesInfo.TryGetValue(operation, out series) == false)
                {
                    series = new Series(operation, ViewType.Point);
                    seriesInfo.Add(operation, series);

                    series.ArgumentDataMember = "Argument";
                    series.ArgumentScaleType = ScaleType.Numerical;
                    series.ValueDataMembers.AddRange(new string[] { "Value" });
                    series.ValueScaleType = ScaleType.Numerical;

                    this.m_chart.Series.Add(series);
                }

                double parameter = double.Parse(row["Argument"].ToString());
                double minmax = double.Parse(row["Value"].ToString());
                SeriesPoint point = new SeriesPoint(parameter, minmax);
                series.Points.Add(point);
            }

            //PointSeriesView seriesView = this.m_chart.Series[0].View as PointSeriesView;
            //seriesView.PointMarkerOptions.Kind = MarkerKind.Circle;
            //seriesView.PointMarkerOptions.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            //seriesView.PointMarkerOptions.Size = 3;

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

            //diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.Default;

            //diagram.EnableAxisXScrolling = true;
            //diagram.EnableAxisXZooming = true;

            //diagram.AxisX.WholeRange.Auto = true;
            //diagram.AxisX.WholeRange.SideMarginsValue = 100;
            //diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Manual;
            //diagram.AxisX.NumericScaleOptions.MeasureUnit = NumericMeasureUnit.Tens;
            //diagram.AxisX.NumericScaleOptions.GridOffset = 0;
            //diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Thousands;
            //diagram.AxisX.NumericScaleOptions.GridSpacing = 10;
            //diagram.AxisX.Label.TextPattern = "{A}";

            //diagram.AxisY.WholeRange.Auto = true;
            //diagram.AxisY.NumericScaleOptions.GridOffset = 0;
            //diagram.AxisY.NumericScaleOptions.GridSpacing = 100;
            ////diagram.AxisY.NumericScaleOptions.MeasureUnit = NumericMeasureUnit.Thousands;
            //diagram.AxisY.NumericScaleOptions.GridAlignment = NumericGridAlignment.Thousands;
            //diagram.AxisY.VisualRange.SideMarginsValue = 100;

            diagram.AxisX.Title.Visibility = string.IsNullOrEmpty(axisTitleX) ? DevExpress.Utils.DefaultBoolean.False : DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisX.Title.Alignment = StringAlignment.Center;
            diagram.AxisX.Title.Text = axisTitleX;

            diagram.AxisY.Title.Visibility = string.IsNullOrEmpty(axisTitleX) ? DevExpress.Utils.DefaultBoolean.False : DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Alignment = StringAlignment.Center;
            diagram.AxisY.Title.Text = axisTitleY;

            //var title1 = new ChartTitle();
            //title1.Text = string.IsNullOrEmpty(title) ? m_chart.Series[0].Name : title;
            //m_chart.Titles.Add(title1);

            PointSeriesView seriesView = m_chart.Series[0].View as PointSeriesView;
            seriesView.PointMarkerOptions.Kind = MarkerKind.Circle;
            seriesView.PointMarkerOptions.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Hatch;
            seriesView.PointMarkerOptions.Size = 5;
            ProcessAutoClusters();
        }


        private void ProcessAutoClusterSet(SeriesPointCollection point, Color color, int lineTickness)
        {
            List<PointF>[] pointsLists = new List<PointF>[3] { new List<PointF>(), new List<PointF>(), new List<PointF>() };
            DiagramToPointHelper.CalculateClusters(point, pointsLists[0], pointsLists[1], pointsLists[2]);
            Cluster cluster = new Cluster();
            cluster.Calculate(pointsLists[0]);
            cluster.IsSelected = true;

            potatoChartInfoList.Add(new PotatoChartInfo(cluster, color, lineTickness));
        }


        private void ProcessAutoClusterSetCross(SeriesPointCollection point, Color color, int lineTickness)
        {
            List<PointF>[] pointsLists = new List<PointF>[3] { new List<PointF>(), new List<PointF>(), new List<PointF>() };
            DiagramToPointHelper.CalculateClusters(point, pointsLists[0], pointsLists[1], pointsLists[2]);
            Cluster cluster = new Cluster();
            cluster.CalculateCross(pointsLists[0]);
            cluster.IsSelected = true;

            crossChartInfoList.Add(new PotatoChartInfo(cluster, color, lineTickness));
        }

        private void ProcessAutoClusters()
        {
            List<PointF>[] pointsLists = new List<PointF>[3] { new List<PointF>(), new List<PointF>(), new List<PointF>() };
            DiagramToPointHelper.CalculateClusters(this.m_chart.Series[0].Points, pointsLists[0], pointsLists[1], pointsLists[2]);
            for (int i = 0; i < 3; i++)
            {
                Cluster cluster = new Cluster();
                cluster.Calculate(pointsLists[i]);
                m_clusters.Add(cluster);
                if (i == 0)
                {
                    cluster.IsSelected = true;
                }
            }
        }

        Rectangle CalculateDiagramBounds()
        {
            var rangeX = (this.m_chart.Diagram as XYDiagram).AxisX.VisualRange;
            var rangeY = (this.m_chart.Diagram as XYDiagram).AxisY.VisualRange;

            Point p1 = (this.m_chart.Diagram as XYDiagram)
                .DiagramToPoint(
                    (double)(this.m_chart.Diagram as XYDiagram).AxisX.WholeRange.MinValue,
                    (double)(this.m_chart.Diagram as XYDiagram).AxisY.WholeRange.MinValue)
                .Point;
            Point p2 = (this.m_chart.Diagram as XYDiagram)
                .DiagramToPoint(
                    (double)(this.m_chart.Diagram as XYDiagram).AxisX.WholeRange.MaxValue,
                    (double)(this.m_chart.Diagram as XYDiagram).AxisY.WholeRange.MaxValue)
                .Point;
            return DiagramToPointHelper.CreateRectangle(p1, p2);
        }
        Point[] GetScreenPoints(List<PointF> contourPoints)
        {
            Point[] screenPoints = new Point[contourPoints.Count];
            for (int i = 0; i < contourPoints.Count; i++)
                screenPoints[i] = (this.m_chart.Diagram as XYDiagram).DiagramToPoint(contourPoints[i].X, contourPoints[i].Y).Point;
            return screenPoints;
        }

        void DrawCluster(Cluster cluster, GraphicsCache g, Color color, Color borderColor)
        {
            Point[] screenPoints = GetScreenPoints(cluster.ContourPoints);
            if (screenPoints.Length > 0)
            {
                Color fillColor = Color.FromArgb(50, color.R, color.G, color.B);
                if (cluster.IsSelected)
                {
                    //g.FillPolygon(screenPoints, fillColor);
                    g.DrawPolygon(screenPoints, borderColor, 1);
                }
                else
                {
                    //g.FillPolygon(screenPoints, fillColor);
                    g.DrawPolygon(screenPoints, borderColor, 1);
                }
            }
        }

        void DrawClusterPotato(PotatoChartInfo potatoChartInfo, GraphicsCache g)
        {
            if (potatoChartInfo != null)
            {
                Point[] screenPoints = GetScreenPoints(potatoChartInfo.cluster.ContourPoints);
                if (screenPoints.Length > 0)
                {
                    g.DrawPolygon(screenPoints, potatoChartInfo.color, potatoChartInfo.lineTickness);
                }
            }
        }

        void DrawClusterCross(PotatoChartInfo potatoChartInfo, GraphicsCache g)
        {
            if (potatoChartInfo != null)
            {
                Point[] screenPoints = GetScreenPoints(potatoChartInfo.cluster.ContourPoints);
                if (screenPoints.Length > 0)
                {
                    g.DrawLines(screenPoints, potatoChartInfo.color, potatoChartInfo.lineTickness);
                }
            }
        }

        void SelectOrUnselectCluster(Cluster cluster, Point point)
        {
            if (cluster.Points.Count < 2)
                return;
            Point[] screenPoints = GetScreenPoints(cluster.ContourPoints);
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(screenPoints);
            cluster.IsSelected = path.IsVisible(point);
        }
        #endregion


        private void cbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.cbSeries.Enabled == false || this.m_drawTypes == DrawTypes.DT_UNKNOWN)
            //    return;

            //List<SeriesPointData> spDatas;
            //this.m_dicData.TryGetValue(this.cbSeries.Text, out spDatas);
            //DataTable dt = null;
            //if (this.m_drawTypes == DrawTypes.DT_1D || this.m_drawTypes == DrawTypes.DT_MINMAX)
            //{
            //    dt = MakeTableData(spDatas);
            //}
            //else if (this.m_drawTypes == DrawTypes.DT_POTATO)
            //{

            //    List<SeriesPointData> dataY;

            //    this.m_dicData.TryGetValue(this.cbSeries2.Text, out dataY);
            //    dt = MakePotatoDataNew(spDatas, dataY);
            //}

            //DrawChart(dt, this.m_drawTypes);
        }

        private void cbSeries2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.cbSeries2.Enabled == false || this.m_drawTypes == DrawTypes.DT_UNKNOWN)
            //    return;

            //List<SeriesPointData> minDatas;
            //List<SeriesPointData> maxDatas;

            //DataTable dt = null;

            //this.m_dicData.TryGetValue(this.cbSeries.Text, out minDatas);
            //this.m_dicData.TryGetValue(this.cbSeries2.Text, out maxDatas);

            //if (this.m_drawTypes == DrawTypes.DT_MINMAX)
            //{
            //    dt = MakeTableData(minDatas);
            //    this.max_table = MakeMaxTableData(maxDatas);
            //}
            //else if (this.m_drawTypes == DrawTypes.DT_POTATO)
            //{
            //    dt = MakePotatoDataNew(minDatas, maxDatas);
            //}

            //DrawChart(dt, this.m_drawTypes);
        }
        private void mnuFileRead_Click(object sender, EventArgs e)
        {
            this.cbSeries.Enabled = false;
            this.m_dicData = ReadDataList4();
            SetDllDatas();
            this.cbSeries.Enabled = true;

            mnuDrawChart1D.Enabled =
                mnuDrawChart2D.Enabled =
                mnuDrawChartMinMax.Enabled =
                mnuDrawPotato.Enabled = this.cbSeries.Items.Count > 0;
        }

        private void mnuDrawChart1D_Click(object sender, EventArgs e)
        {
            this.m_drawTypes = DrawTypes.DT_1D;
            mnuDrawChart1D.Checked = true;
            mnuDrawChart2D.Checked = false;
            mnuDrawChartMinMax.Checked = false;
            mnuDrawPotato.Checked = false;

            List<SeriesPointData> spDatas;
            this.m_dicData.TryGetValue(this.cbSeries.Text, out spDatas);

            DataTable dt = MakeTableData(spDatas);

            DrawChart(dt, this.m_drawTypes);
        }

        private void mnuDrawChart2D_Click(object sender, EventArgs e)
        {
            this.m_drawTypes = DrawTypes.DT_2D;
            mnuDrawChart1D.Checked = false;
            mnuDrawChart2D.Checked = true;
            mnuDrawChartMinMax.Checked = false;
            mnuDrawPotato.Checked = false;

            List<SeriesPointData> spDatas;
            this.m_dicData.TryGetValue(this.cbSeries.Text, out spDatas);

            DataTable dt = MakeTableData(spDatas);

            DrawChart(dt, this.m_drawTypes);
        }

        private void drawChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.cbSeries2.Text == null || this.cbSeries2.Text == "")
            {
                MessageBox.Show(new Form { TopMost = true }, "Max로 사용할 항목을 선택해 주세요.");
                return;
            }
            this.m_drawTypes = DrawTypes.DT_MINMAX;
            mnuDrawChart1D.Checked = false;
            mnuDrawChart2D.Checked = false;
            mnuDrawChartMinMax.Checked = true;
            mnuDrawPotato.Checked = false;


            List<SeriesPointData> minDatas;
            List<SeriesPointData> maxDatas;

            this.m_dicData.TryGetValue(this.cbSeries.Text, out minDatas);
            this.m_dicData.TryGetValue(this.cbSeries2.Text, out maxDatas);

            DataTable dt = MakeTableData(minDatas);
            this.max_table = MakeMaxTableData(maxDatas);

            DrawChart(dt, this.m_drawTypes);
        }

        private void propertyShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.m_drawTypes != DrawTypes.DT_MINMAX)
                return;

            propertyShowToolStripMenuItem.Checked = !propertyShowToolStripMenuItem.Checked;

            RefreshUI();
        }

        private void mnuDrawPotato_Click(object sender, EventArgs e)
        {
            this.m_drawTypes = DrawTypes.DT_POTATO;
            mnuDrawChart1D.Checked = false;
            mnuDrawChart2D.Checked = false;
            mnuDrawChartMinMax.Checked = false;
            mnuDrawPotato.Checked = true;

            List<SeriesPointData> dataX;
            List<SeriesPointData> dataY;

            this.m_dicData.TryGetValue(this.cbSeries.Text, out dataX);
            this.m_dicData.TryGetValue(this.cbSeries2.Text, out dataY);
            if(this.cbSeries2.Text == null || this.cbSeries2.Text == "")
            {
                MessageBox.Show(new Form { TopMost = true }, "Y축으로 사용할 항목을 선택해주세요.");
                return;
            }

            //DataTable dt = MakePotatoData("2955");
            DataTable dt = MakePotatoDataNew(dataX, dataY);
            
            DrawChart(dt, m_drawTypes);
        }

        private void mnuSaveChart_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "저장경로를 지정하세요.";
            sfd.OverwritePrompt = true;
            sfd.Filter = "SVG File(*.svg)|*.svg";

            if (sfd.ShowDialog() == DialogResult.OK)
                this.m_chart.ExportToSvg(sfd.FileName);
        }
        //private Dictionary<string, List<SeriesPointData>> SetDataList()
        //{
        //    Dictionary<string, List<SeriesPointData>> dicData = new Dictionary<string, List<SeriesPointData>>();
        //    try
        //    {
        //        using (FileStream fs = new FileStream(filename, FileMode.Open))
        //        using (StreamReader reader = new StreamReader(fs, Encoding.UTF8, false))
        //        {
        //            while (!reader.EndOfStream)
        //            {
        //                string line = reader.ReadLine();
        //                var values = line.Split(',');

        //                if (string.IsNullOrEmpty(values[0]))
        //                    continue;

        //                int i;
        //                if (values[0].Equals("DATE"))
        //                {
        //                    for (i = 1; i < values.Length; i++)
        //                    {
        //                        if (!string.IsNullOrEmpty(values[i]))
        //                        {
        //                            dicData.Add(values[i], new List<SeriesPointData>());
        //                        }
        //                    }
        //                    this.cbSeries.Items.AddRange(dicData.Keys.ToArray());
        //                    this.cbSeries.SelectedIndex = 0;
        //                    continue;
        //                }

        //                i = 0;
        //                foreach (string key in dicData.Keys)
        //                {
        //                    if (!string.IsNullOrEmpty(values[i]))
        //                    {
        //                        string[] splits = values[0].Split(':');
        //                        var temp = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);
        //                        var arg = DateTime.ParseExact(temp, "yyyy-MM-dd HH:mm:ss.ffffff", null);
        //                        var value = double.Parse(string.IsNullOrEmpty(values[i + 1]) ? "0" : values[i + 1]);
        //                        dicData[key].Add(new SeriesPointData(key, arg, value));
        //                        i++;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
        //    }

        //    return dicData;
        //}

        private void ReadDataTable(string filename)
        {
            int index = 1;

            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8, false))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var values = line.Split(',');

                        if (string.IsNullOrEmpty(values[0]))
                            continue;

                        if (values[0].Equals("DATE"))
                        {
                            for (int i = 1; i < values.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(values[i]))
                                {
                                    if (null == m_series)
                                        m_series = new List<string>();

                                    m_series.Add(values[i]);
                                }
                            }

                            continue;
                        }


                        foreach (DLL_DATA dll in m_dllDatas)
                        {
                            foreach (DLL_DATA.Parameter param in dll.parameters)
                            {
                                if (null == param.data)
                                {
                                    param.data = new DataTable();
                                    param.data.Columns.Add("Series", typeof(string));
                                    param.data.Columns.Add("Argument", typeof(DateTime));
                                    param.data.Columns.Add("Value", typeof(double));
                                }

                                index = m_series.FindIndex(f => f.Contains(param.Name));

                                string[] splits = values[0].Split(':');
                                var arg = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);
                                var argument = DateTime.ParseExact(arg, "yyyy-MM-dd HH:mm:ss.ffffff", null);
                                var value = double.Parse(values[index + 1]);

                                DataRow row = param.data.NewRow();
                                row["Series"] = param.Name;
                                row["Argument"] = argument;
                                row["Value"] = value;
                                param.data.Rows.Add(row);


                                if (param.MinVal == 0)
                                    param.MinVal = value;
                                else if (param.MinVal > value)
                                    param.MinVal = value;

                                if (param.MaxVal == 0)
                                    param.MaxVal = value;
                                else if (param.MaxVal < value)
                                    param.MaxVal = value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        //private List<SeriesPointData> ReadDataList(string filename, bool isFirst = false)
        //{
        //    int index = 1;
        //    List<SeriesPointData> data = new List<SeriesPointData>();
        //    try
        //    {
        //        using (FileStream fs = new FileStream(filename, FileMode.Open))
        //        using (StreamReader reader = new StreamReader(fs, Encoding.UTF8, false))
        //        {
        //            if (null == m_series)
        //                m_series = new List<string>();

        //            while (!reader.EndOfStream)
        //            {
        //                string line = reader.ReadLine();
        //                var values = line.Split(',');

        //                if (string.IsNullOrEmpty(values[0]))
        //                    continue;

        //                if (values[0].Equals("DATE"))
        //                {
        //                    if (isFirst)
        //                    {
        //                        for (int i = 1; i < values.Length; i++)
        //                        {
        //                            if (!string.IsNullOrEmpty(values[i]))
        //                            {
        //                                m_series.Add(values[i]);
        //                                this.cbSeries.Items.Add(values[i]);
        //                            }
        //                        }
        //                        this.cbSeries.SelectedIndex = 0;
        //                    }
        //                    else
        //                        index = m_series.FindIndex(f => f.Contains(this.cbSeries.Text));

        //                    continue;
        //                }

        //                if (!isFirst)
        //                {
        //                    string[] splits = values[0].Split(':');
        //                    var arg = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);
        //                    var argument = DateTime.ParseExact(arg, "yyyy-MM-dd HH:mm:ss.ffffff", null);
        //                    var value = double.Parse(values[index + 1]);
        //                    data.Add(new SeriesPointData(this.cbSeries.Text, argument, value));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
        //    }

        //    return data;
        //}

        //private List<SeriesPointData> ReadDataList2(string filename)
        //{
        //    List<SeriesPointData> data = new List<SeriesPointData>();
        //    try
        //    {
        //        using (FileStream fs = new FileStream(filename, FileMode.Open))
        //        using (StreamReader reader = new StreamReader(fs, Encoding.UTF8, false))
        //        {
        //            if (null == m_series)
        //                m_series = new List<string>();
        //            m_series.Clear();

        //            while (!reader.EndOfStream)
        //            {
        //                string line = reader.ReadLine();
        //                var values = line.Split(',');

        //                if (string.IsNullOrEmpty(values[0]))
        //                    continue;

        //                if (values[0].Equals("DATE"))
        //                {
        //                    for (int i = 1; i < values.Length; i++)
        //                    {
        //                        if (!string.IsNullOrEmpty(values[i]))
        //                        {
        //                            m_series.Add(values[i]);
        //                        }
        //                    }
        //                    this.cbSeries.Items.AddRange(m_series.ToArray());
        //                    this.cbSeries.SelectedIndex = 0;
        //                    continue;
        //                }

        //                for (int i = 0; i < m_series.Count; i++)
        //                {
        //                    string[] splits = values[0].Split(':');
        //                    var arg = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);
        //                    var argument = DateTime.ParseExact(arg, "yyyy-MM-dd HH:mm:ss.ffffff", null);
        //                    var value = double.Parse(string.IsNullOrEmpty(values[i + 1]) ? "0" : values[i + 1]);
        //                    var series = m_series[i];

        //                    //Debug.Print(string.Format("arg = {0}, value = {1}, series = {2}", argument, value, series));
        //                    data.Add(new SeriesPointData(series, argument, value));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
        //    }

        //    return data;
        //}

        private Dictionary<string, List<SeriesPointData>> ReadDataList3(string filename)
        {
            Dictionary<string, List<SeriesPointData>> dicData = new Dictionary<string, List<SeriesPointData>>();
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8, false))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var values = line.Split(',');

                        if (string.IsNullOrEmpty(values[0]))
                            continue;

                        int i;
                        if (values[0].Equals("DATE"))
                        {
                            for (i = 1; i < values.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(values[i]))
                                {
                                    dicData.Add(values[i], new List<SeriesPointData>());
                                }
                            }
                            this.cbSeries.Items.AddRange(dicData.Keys.ToArray());
                            this.cbSeries.SelectedIndex = 0;
                            continue;
                        }

                        i = 0;
                        foreach (string key in dicData.Keys)
                        {
                            if (!string.IsNullOrEmpty(values[i]))
                            {
                                //string[] splits = values[0].Split(':');
                                //var temp = string.Format("{0} {1}:{2}:{3}", new DateTime().AddYears(DateTime.Now.Year - 1).AddDays(double.Parse(splits[0])).ToString("yyyy-MM-dd"), splits[1], splits[2], splits[3]);
                                //var arg = DateTime.ParseExact(temp, "yyyy-MM-dd HH:mm:ss.ffffff", null);
                                var value = double.Parse(string.IsNullOrEmpty(values[i + 1]) ? "0" : values[i + 1]);
                                dicData[key].Add(new SeriesPointData(key, i, value));
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return dicData;
        }

        private Dictionary<string, List<SeriesPointData>> ReadDataList4()
        {
            Dictionary<string, List<SeriesPointData>> dicData = new Dictionary<string, List<SeriesPointData>>();
            foreach(var source in additionalResponse.sourceList)
            {
                string itemName = string.Format(@"{0}-{1}", Utils.base64StringDecoding(source.sourceName), source.paramKey);
                dicData.Add(itemName, new List<SeriesPointData>());
                if (source.timeSet != null && source.timeSet.Count != 0)
                {
                    if (source.data != null)
                    {
                        for (int i = 0; i < source.data.Count; i++)
                        {
                            dicData[itemName].Add(new SeriesPointData(itemName, source.timeSet[i], source.data[i]));
                        }
                    }
                }
                else
                {
                    if (source.data != null)
                    {
                        for (int i = 0; i < source.data.Count; i++)
                        {
                            dicData[itemName].Add(new SeriesPointData(itemName, i, source.data[i]));
                        }
                    }
                }
                plotSourceNameSeqDic.Add(itemName, new PlotSeriesSource(source.sourceType, source.seq,itemName, source.sourceSeq));
            }

            foreach (var equaction in additionalResponse.eqlist)
            {
                string itemName = Utils.base64StringDecoding(equaction.eqName);
                dicData.Add(itemName, new List<SeriesPointData>());
                //if (equaction.timeSet != null && equaction.timeSet.Count != 0)
                //{
                if (equaction.convhData != null && equaction.convhData.Count != 0)
                {
                    for (int i = 0; i < equaction.convhData.Count; i++)
                    {
                        dicData[itemName].Add(new SeriesPointData(itemName, i, equaction.convhData[i][0], equaction.convhData[i][1]));
                    }
                }
                else
                {
                    for (int i = 0; i < equaction.data.Count; i++)
                    {
                        if (equaction.timeSet != null && equaction.timeSet.Count != 0)
                        {
                            dicData[itemName].Add(new SeriesPointData(itemName, equaction.data[i].ToString(), 0));
                        }
                        else
                        {
                            dicData[itemName].Add(new SeriesPointData(itemName, i, Convert.ToDouble(equaction.data[i])));
                        }
                    }
                }
                //}
                //else
                //{
                //    for (int i = 0; i < source.data.Count; i++)
                //    {
                //        dicData[itemName].Add(new SeriesPointData(itemName, i, source.data[i]));
                //    }
                //}
                plotSourceNameSeqDic.Add(itemName, new PlotSeriesSource("eq", equaction.seq, itemName,equaction.moduleSeq));
            }
            //foreach (var source in plotGridSourceDataList)
            //{
            //    int index = -99999;
            //    index = plotDataResponse.additionalResponse.plotSourceList.FindIndex(x => x.sourceSeq == source.seq && x.sourceType == source.sourceType);
            //    int i = 0;
            //    if (index != -1)
            //    {
            //        dicData.Add(source.itemName, new List<SeriesPointData>());
            //        foreach (var plotData in plotDataResponse.response[index])
            //        {
            //            dicData[source.itemName].Add(new SeriesPointData(source.itemName, i, plotData));
            //            i++;
            //        }
            //    }
            //}

            this.timeKeyList = dicData.Where(x => x.Value.Count != 0 && x.Value[0].TimeValue != null).Select(dic => dic.Key).ToList();
            this.allKeyList = dicData.Keys.ToList();
            this.repositoryItemComboBox2.Items.AddRange(dicData.Keys.ToArray());

            this.repositoryItemComboBox2.NullText = "";
            this.repositoryItemComboBox2.SelectedIndexChanged += RepositoryItemComboBox2_SelectedIndexChanged;
            this.repositoryItemComboBox6.Items.AddRange(dicData.Keys.ToArray());

            this.repositoryItemComboBox6.NullText = "";
            this.repositoryItemComboBox6.SelectedIndexChanged += RepositoryItemComboBox6_SelectedIndexChanged;

            return dicData;
        }


        public void ChangeSourceList(AdditionalResponse newAdditionalResponse)
        {
            this.additionalResponse = newAdditionalResponse;
            plotSourceNameSeqDic.Clear();
            Dictionary<string, List<SeriesPointData>> dicData = new Dictionary<string, List<SeriesPointData>>();
            foreach (var source in additionalResponse.sourceList)
            {
                string itemName = string.Format(@"{0}-{1}", Utils.base64StringDecoding(source.sourceName), source.paramKey);
                dicData.Add(itemName, new List<SeriesPointData>());
                if (source.timeSet != null && source.timeSet.Count != 0)
                {
                    if (source.data != null)
                    {
                        for (int i = 0; i < source.data.Count; i++)
                        {
                            dicData[itemName].Add(new SeriesPointData(itemName, source.timeSet[i], source.data[i]));
                        }
                    }
                }
                else
                {
                    if (source.data != null)
                    {
                        for (int i = 0; i < source.data.Count; i++)
                        {
                            dicData[itemName].Add(new SeriesPointData(itemName, i, source.data[i]));
                        }
                    }
                }
                plotSourceNameSeqDic.Add(itemName, new PlotSeriesSource(source.sourceType, source.seq, itemName, source.sourceSeq));
            }

            foreach (var equaction in additionalResponse.eqlist)
            {
                string itemName = Utils.base64StringDecoding(equaction.eqName);
                dicData.Add(itemName, new List<SeriesPointData>());
                //if (equaction.timeSet != null && equaction.timeSet.Count != 0)
                //{
                if (equaction.convhData != null && equaction.convhData.Count != 0)
                {
                    for (int i = 0; i < equaction.convhData.Count; i++)
                    {
                        dicData[itemName].Add(new SeriesPointData(itemName, i, equaction.convhData[i][0], equaction.convhData[i][1]));
                    }
                }
                else
                {
                    for (int i = 0; i < equaction.data.Count; i++)
                    {
                        if (equaction.timeSet != null && equaction.timeSet.Count != 0)
                        {
                            dicData[itemName].Add(new SeriesPointData(itemName, equaction.data[i].ToString(), 0));
                        }
                        else
                        {
                            dicData[itemName].Add(new SeriesPointData(itemName, i, Convert.ToDouble(equaction.data[i])));
                        }
                    }
                }
                //}
                //else
                //{
                //    for (int i = 0; i < source.data.Count; i++)
                //    {
                //        dicData[itemName].Add(new SeriesPointData(itemName, i, source.data[i]));
                //    }
                //}
                plotSourceNameSeqDic.Add(itemName, new PlotSeriesSource("eq", equaction.seq, itemName, equaction.moduleSeq));
            }

            this.timeKeyList = dicData.Where(x => x.Value.Count != 0 && x.Value[0].TimeValue != null).Select(dic => dic.Key).ToList();
            this.allKeyList = dicData.Keys.ToList();
            this.repositoryItemComboBox2.Items.Clear();
            this.repositoryItemComboBox2.Items.AddRange(dicData.Keys.ToArray());
            this.repositoryItemComboBox6.Items.Clear();
            this.repositoryItemComboBox6.Items.AddRange(dicData.Keys.ToArray());


            if (this.dxGridDataList != null && this.dxGridDataList.Count != 0)
            {
                if (this.dxGridDataList[0].chartType == "1D-Time History")
                {
                    this.repositoryItemComboBox2.Items.Clear();
                    this.repositoryItemComboBox2.Items.Add("");
                    this.repositoryItemComboBox2.Items.AddRange(timeKeyList);
                }
                if (this.dxGridDataList[0].chartType == "Potato Plot")
                {
                    GridColumn colYAxis = gridView1.Columns["xAxis"];
                    colYAxis.OptionsColumn.ReadOnly = false;
                }
            }
            this.m_dicData =  dicData;
        }


        private void txtPageSize_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnReset_Click(null, null);
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            DXChartControl chartControl = new DXChartControl();
            chartControl = chartControl.DeepCopy(this);
            plotModuleControl.AddDocument(chartControl, plotModuleControl.GetDocumentName());
        }

        private void chartResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_chart.Series.Clear();
        }

        private void btn_AddSeries_Click(object sender, EventArgs e)
        {
            if(this.dxGridDataList == null)
            {
                this.dxGridDataList = new List<dxGridData>();
            }
            dxGridData data = new dxGridData();
            this.dxGridDataList.Add(data);
            this.gridControl1.DataSource = this.dxGridDataList;
            this.gridView1.RefreshData();
        }

        private void btn_SeriesApply_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("차트를 생성중입니다. 잠시만 기다려주십시오.");
            this.m_chart.Series.Clear();
            this.potatoChartInfoList.Clear();
            this.crossChartInfoList.Clear();
            DrawSeries();
            mainForm.HideSplashScreenManager();
        }

        private void btn_ResetSeries_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("전체 차트 및 추가한 Series 데이터가 초기화 됩니다. \n초기화 하시겠습니까?", "전체삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.m_chart.Series.Clear();
                this.dxGridDataList.Clear();
                this.gridControl1.DataSource = this.dxGridDataList;
                this.gridView1.RefreshData();
                this.selectPointList.Clear();
                this.gridControl2.DataSource = this.selectPointList;
                this.gridView2.RefreshData();
            }
        }

        private void DrawSeries()
        {
           foreach(var seriesInfo in dxGridDataList)
           {
                if (string.IsNullOrEmpty(seriesInfo.seriesName))
                {
                    MessageBox.Show(new Form { TopMost = true }, "Series이름이 비어있는 데이터가 있습니다. 다시확인해주세요.");
                    this.m_chart.Series.Clear();
                    return;
                }
                if (string.IsNullOrEmpty(seriesInfo.chartType))
                {
                    MessageBox.Show(new Form { TopMost = true }, "차트타입이 비어있는 데이터가 있습니다. 다시확인해주세요.");
                    this.m_chart.Series.Clear();
                    return;
                }
                if (string.IsNullOrEmpty(seriesInfo.lineBorder))
                {
                    MessageBox.Show(new Form { TopMost = true }, "선굵기가 비어있는 데이터가 있습니다. 다시확인해주세요.");
                    this.m_chart.Series.Clear();
                    return;
                }
                if (string.IsNullOrEmpty(seriesInfo.lineType))
                {
                    MessageBox.Show(new Form { TopMost = true }, "선타입이 비어있는 데이터가 있습니다. 다시확인해주세요.");
                    this.m_chart.Series.Clear();
                    return;
                }
                if (string.IsNullOrEmpty(seriesInfo.lineColor))
                {
                    MessageBox.Show(new Form { TopMost = true }, "선색상이 비어있는 데이터가 있습니다. 다시확인해주세요.");
                    this.m_chart.Series.Clear();
                    return;
                }
                if (string.IsNullOrEmpty(seriesInfo.yAxis))
                {
                    MessageBox.Show(new Form { TopMost = true }, "Y축이 비어있는 데이터가 있습니다. 다시확인해주세요.");
                    this.m_chart.Series.Clear();
                    return;
                }
                List<SeriesPointData> spXAxisData = null ;
                List<SeriesPointData> spYAxisData;
                DataTable dtX = null;
                DataTable dtY = null;

                this.m_dicData.TryGetValue(seriesInfo.yAxis, out spYAxisData);

                if (seriesInfo.xAxis != null)
                    this.m_dicData.TryGetValue(seriesInfo.xAxis, out spXAxisData);

                Color seriesColor = Color.White;
                ViewType viewType = ViewType.Line;

                switch (seriesInfo.lineType)
                {
                    case "Line":
                        viewType = ViewType.Line;
                        break;
                    case "Dot":
                        viewType = ViewType.Point;
                        break;
                }
                switch (seriesInfo.lineColor)
                {
                    case "Red":
                        seriesColor = Color.Red;
                        break;
                    case "Orange":
                        seriesColor = Color.Orange;
                        break;
                    case "Yellow":
                        seriesColor = Color.Yellow;
                        break;
                    case "Green":
                        seriesColor = Color.Green;
                        break;
                    case "Blue":
                        seriesColor = Color.Blue;
                        break;
                    case "Purple":
                        seriesColor = Color.Purple;
                        break;
                    case "Indigo":
                        seriesColor = Color.Indigo;
                        break;
                    case "Black":
                        seriesColor = Color.Black;
                        break;
                }
                int bordThickness = 0;
                Int32.TryParse(seriesInfo.lineBorder, out bordThickness);
                switch (seriesInfo.chartType)
                {
                    case "1D-Time History":
                        if (spYAxisData != null) 
                        { 
                            dtY = Make1DTimeTableData(spYAxisData);
                        }
                        if (spXAxisData != null)
                        {
                            dtX = Make1DTimeData(spXAxisData);
                            if(dtX.Rows.Count != dtY.Rows.Count)
                            {
                                MessageBox.Show(new Form { TopMost = true }, seriesInfo.seriesName + "시리즈의 X,Y축의 데이터 수가 다릅니다. 다시 확인해주세요.");
                                this.m_chart.Series.Clear();
                                return;
                            }
                            if (spXAxisData.Count == 0 || spXAxisData[0].TimeValue == null)
                            {
                                MessageBox.Show(new Form { TopMost = true }, seriesInfo.seriesName + "시리즈의 X축의 데이터의 시간값이 없습니다. 다시 확인해주세요.");
                                this.m_chart.Series.Clear();
                                return;
                            }
                            DrawChart_1DBasic(seriesInfo.seriesName, seriesColor, bordThickness, viewType, dtY, dtX);
                        }
                        else 
                        {
                            if (dtX == null && (spYAxisData == null || spYAxisData.Count == 0 || spYAxisData[0].TimeValue == null))
                            {
                                MessageBox.Show(new Form { TopMost = true }, seriesInfo.seriesName + "시리즈의 Y축의 데이터의 시간값이 없습니다. \nX축에 시간값을 추가하거나 다시 확인해주세요.");
                                this.m_chart.Series.Clear();
                                return;
                            }
                            DrawChart_1DBasic(seriesInfo.seriesName, seriesColor, bordThickness, viewType, dtY);
                        }
                        break;
                    case "Cross Plot":
                        if (seriesInfo.yAxis == null)
                        {
                            MessageBox.Show(new Form { TopMost = true }, "Y축이 설정되지 않은 데이터가 있습니다. 다시 확인해주세요.");
                            this.m_chart.Series.Clear();
                            return;
                        }
                        if (seriesInfo.xAxis == null || spXAxisData == null)
                        {
                            MessageBox.Show(new Form { TopMost = true }, "X축이 설정되지 않은 데이터가 있습니다. 다시 확인해주세요.");
                            this.m_chart.Series.Clear();
                            return;
                        }
                        dtX = Make1DTableData(spXAxisData);
                        dtY = Make1DTableData(spYAxisData);
                        if(dtX.Rows.Count != dtY.Rows.Count)
                        {
                            MessageBox.Show(new Form { TopMost = true }, "X,Y의 갯수가 다른 데이터가 있습니다. 다시 확인해주세요.");
                            this.m_chart.Series.Clear();
                            return;
                        }
                        DrawChart_CrossPlot(seriesInfo.seriesName, seriesColor, bordThickness, viewType, dtX,dtY); ;
                        break;
                    case "Potato Plot":
                        dtX = MakePotatoTableData(spYAxisData);
                        DrawChart_PotatoPlot(seriesInfo.seriesName, seriesColor, bordThickness, viewType, dtX);
                        break;
                }

           }
        }

        private DataTable Make1DTableData(List<SeriesPointData> points)
        {
            DataTable tableData = new DataTable();

            int index = 0;

            foreach (SeriesPointData point in points)
            {
                if (0 == index)
                {
                    tableData.Columns.Add("Argument", typeof(int));
                    tableData.Columns.Add("Value", typeof(double));
                }

                DataRow row = tableData.NewRow();
                row["Argument"] = point.Argument;
                row["Value"] = point.Value;
                tableData.Rows.Add(row);

                index++;
            }

            return tableData;
        }

        private DataTable Make1DTimeTableData(List<SeriesPointData> points)
        {
            DataTable tableData = new DataTable();

            int index = 0;

            foreach (SeriesPointData point in points)
            {
                if (0 == index)
                {
                    tableData.Columns.Add("Argument", typeof(DateTime));
                    tableData.Columns.Add("Value", typeof(double));
                }

                DataRow row = tableData.NewRow();
                if (point.TimeValue != null)
                {
                    DateTime dt = Utils.GetDateFromJulian(point.TimeValue);
                    row["Argument"] = dt;
                }
                row["Value"] = point.Value;
                tableData.Rows.Add(row);

                index++;
            }

            return tableData;
        }

        private DataTable Make1DTimeData(List<SeriesPointData> points)
        {
            DataTable tableData = new DataTable();

            int index = 0;

            foreach (SeriesPointData point in points)
            {
                if (point.TimeValue != null)
                {
                    if (0 == index)
                    {
                        tableData.Columns.Add("Argument", typeof(DateTime));
                    }

                    DataRow row = tableData.NewRow();

                    DateTime dt = Utils.GetDateFromJulian(point.TimeValue);
                    row["Argument"] = dt;
                    tableData.Rows.Add(row);

                    index++;
                }
            }

            return tableData;
        }

        private DataTable MakePotatoTableData(List<SeriesPointData> points)
        {
            DataTable tableData = new DataTable();

            int index = 0;

            foreach (SeriesPointData point in points)
            {
                if (0 == index)
                {
                    tableData.Columns.Add("Value", typeof(double));
                    tableData.Columns.Add("Value1", typeof(double));
                }

                DataRow row = tableData.NewRow();
                row["Value"] = point.Value;
                row["Value1"] = point.Value1;
                tableData.Rows.Add(row);

                index++;
            }

            return tableData;
        }
        private void DrawChart_1DBasic(string serieaName,Color seriesColor, int bordThickness, ViewType viewType, DataTable dt, DataTable dt1 = null)
        {
            if(dt == null)
            {
                MessageBox.Show(new Form { TopMost = true }, "데이터 소스를 찾지 못하는 시리즈가 있습니다. 다시 확인해주세요.");
                return;
            }
            Series series = new Series(serieaName, viewType);
            List<DataRow> dataSource1 = null;
            if (dt1 != null)
            {
                dataSource1 = dt1.AsEnumerable().ToList();
            }
            var dataSource = dt.AsEnumerable().ToList();
      
            
           
            series.View.Color = seriesColor;
            if (dataSource1 == null)
            {
                foreach (DataRow row in dataSource)
                    series.Points.Add(new SeriesPoint(row["Argument"], row["Value"]));
            }
            else
            {
                for (int i = 0; i < dataSource.Count(); i++)
                {
                    series.Points.Add(new SeriesPoint(dataSource1[i]["Argument"], dataSource[i]["Value"]));
                }
            }
            if (viewType == ViewType.Line)
            {
                ((LineSeriesView)series.View).LineStyle.Thickness = bordThickness;
            }
            series.ArgumentScaleType = ScaleType.DateTime;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            this.m_chart.Series.Add(series);
            this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

            diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            //타임으로 변경
            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            diagram.AxisX.WholeRange.SideMarginsValue = 0L;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.DateTimeScaleOptions.GridOffset = 1;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;

            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;

            diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.AutoGrid = false;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffffff}";
            diagram.AxisX.Title.Text = "";
            diagram.AxisY.Title.Text = "";

            diagram.RangeControlDateTimeGridOptions.GridMode = ChartRangeControlClientGridMode.Manual;
            diagram.RangeControlDateTimeGridOptions.GridOffset = 1;
            diagram.RangeControlDateTimeGridOptions.GridSpacing = 60;
            diagram.RangeControlDateTimeGridOptions.LabelFormat = "HH:mm:ss.fff";
            diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Millisecond;

            this.m_chart.Titles.Clear();
            ChartTitle chartTitle = new ChartTitle();
            chartTitle.Text = plotModuleControl.GetDocumentName() == null ? plotName : plotModuleControl.GetDocumentName();
            chartTitle.Alignment = StringAlignment.Center;
            chartTitle.Dock = ChartTitleDockStyle.Top;
            chartTitle.Font = new Font("Tahoma", 14, FontStyle.Bold);
            chartTitle.TextColor = Color.White;
            this.m_chart.Titles.Add(chartTitle);

            if (plotAxisInfo != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(plotAxisInfo.xTitle))
                    {
                        diagram.AxisX.Title.Text = plotAxisInfo.xTitle;
                        diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    }

                    if (!string.IsNullOrEmpty(plotAxisInfo.yTitle))
                    {
                        diagram.AxisY.Title.Text = plotAxisInfo.yTitle;
                        diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.diagramType))
                    {
                        switch (plotAxisInfo.diagramType)
                        {
                            case "Time":
                                diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                                diagram.AxisX.DateTimeScaleOptions.AutoGrid = false;
                                diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffffff}";

                                diagram.RangeControlDateTimeGridOptions.GridMode = ChartRangeControlClientGridMode.Manual;
                                diagram.RangeControlDateTimeGridOptions.GridOffset = 1;

                                diagram.RangeControlDateTimeGridOptions.LabelFormat = "HH:mm:ss.fff";
                                if (!string.IsNullOrEmpty(plotAxisInfo.xGridAlign))
                                {
                                    if (plotAxisInfo.xGridAlign == "Millisecond")
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
                                        diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;
                                        diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Millisecond;
                                    }
                                    if (plotAxisInfo.xGridAlign == "Second")
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
                                        diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
                                        diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Second;
                                    }
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMaxRange))
                                {
                                    try
                                    {
                                        DateTime maxTime;
                                        DateTime.TryParse(plotAxisInfo.xMaxRange, out maxTime);
                                        if (maxTime != default(DateTime))
                                        {
                                            DateTime nowMaxTime = (DateTime)diagram.AxisX.WholeRange.MaxValue;
                                            DateTime changeMaxTime = new DateTime(nowMaxTime.Year, nowMaxTime.Month, nowMaxTime.Day, maxTime.Hour, maxTime.Minute, maxTime.Second, maxTime.Millisecond);
                                            diagram.AxisX.WholeRange.SetMinMaxValues(diagram.AxisX.WholeRange.MinValue, changeMaxTime);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                                if (!string.IsNullOrEmpty(plotAxisInfo.xMinRange))
                                {

                                    try
                                    {
                                        DateTime minTime;
                                        DateTime.TryParse(plotAxisInfo.xMinRange, out minTime);
                                        if (minTime != default(DateTime))
                                        {
                                            DateTime nowMinTime = (DateTime)diagram.AxisX.WholeRange.MinValue;
                                            DateTime changeMinTime = new DateTime(nowMinTime.Year, nowMinTime.Month, nowMinTime.Day, minTime.Hour, minTime.Minute, minTime.Second, minTime.Millisecond);
                                            diagram.AxisX.WholeRange.SetMinMaxValues(changeMinTime, diagram.AxisX.WholeRange.MaxValue);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xSpacing))
                                {
                                    double xSpacing = 0;
                                    double.TryParse(plotAxisInfo.xSpacing, out xSpacing);
                                    if (xSpacing != 0)
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.GridSpacing = xSpacing;
                                        //diagram.RangeControlDateTimeGridOptions.GridSpacing = xSpacing;
                                    }
                                }


                                break;
                            case "Index":
                                diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Manual;
                                diagram.AxisX.NumericScaleOptions.AutoGrid = false;
                                diagram.AxisX.Label.TextPattern = "{A}";

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMaxRange))
                                {
                                    double xMaxRange = 0;
                                    double.TryParse(plotAxisInfo.xMaxRange, out xMaxRange);
                                    diagram.AxisX.WholeRange.MaxValue = xMaxRange;
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMinRange))
                                {
                                    double xMinRange = 0;
                                    double.TryParse(plotAxisInfo.xMinRange, out xMinRange);
                                    diagram.AxisX.WholeRange.MinValue = xMinRange;
                                }
                                if (!string.IsNullOrEmpty(plotAxisInfo.xSpacing))
                                {
                                    double xSpacing = 0;
                                    double.TryParse(plotAxisInfo.xSpacing, out xSpacing);
                                    if (xSpacing != 0)
                                    {
                                        diagram.AxisX.NumericScaleOptions.GridSpacing = xSpacing;
                                    }
                                }
                                break;
                        }
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.yMinRange))
                    {
                        double yMinRange = 0;
                        double.TryParse(plotAxisInfo.yMinRange, out yMinRange);
                        diagram.AxisY.WholeRange.MinValue = yMinRange;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.yMaxRange))
                    {
                        double yMaxRange = 0;
                        double.TryParse(plotAxisInfo.yMaxRange, out yMaxRange);
                        diagram.AxisY.WholeRange.MaxValue = yMaxRange;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.ySpacing))
                    {
                        double ySpacing = 0;
                        double.TryParse(plotAxisInfo.ySpacing, out ySpacing);
                        if (ySpacing != 0)
                        {
                            diagram.AxisY.NumericScaleOptions.AutoGrid = false;
                            diagram.AxisY.NumericScaleOptions.GridSpacing = ySpacing;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(new Form { TopMost = true }, plotName +" 의 축정보 설정 중 오류가 발생했습니다. 축정보를 확인해주세요.");
                }
            }

            //List<PointF> points = new List<PointF>();
            //foreach (SeriesPoint point in this.m_chart.Series)
            //    points.Add(new PointF((float)point.NumericalArgument, (float)point.Values[0]));

        }

        //private void DrawChart_CrossPlot(string serieaName, Color seriesColor, int bordThickness,ViewType viewType, DataTable dt, DataTable dt2)
        //{
        //    Series series = new Series(serieaName, viewType);
        //    series.View.Color = seriesColor;

        //    var minDataSource = dt.AsEnumerable().ToList();
        //    var maxDataSource = dt2.AsEnumerable().ToList();

        //    //this.m_chart.Series[0].Points.Clear();
        //    if (minDataSource.Count() != maxDataSource.Count())
        //    {
        //        MessageBox.Show(new Form { TopMost = true }, "CrossPlot 데이터의 개수가 다릅니다.");
        //        return;
        //    }
        //    for (int i = 0; i < minDataSource.Count(); i++)
        //    {
        //        series.Points.Add(new SeriesPoint(minDataSource[i]["Value"], maxDataSource[i]["Value"]));
        //    }
        //    if (viewType == ViewType.Line)
        //    {
        //        ((LineSeriesView)series.View).LineStyle.Thickness = bordThickness;
        //    }
        //    series.ArgumentScaleType = ScaleType.Auto;
        //    series.ArgumentDataMember = "Argument";
        //    series.ValueScaleType = ScaleType.Numerical;
        //    series.ValueDataMembers.AddRange(new string[] { "Value" });
        //    this.m_chart.Series.Add(series);

        //    this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
        //    this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;

        //    XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

        //    diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.Default;

        //    diagram.EnableAxisXScrolling = true;
        //    diagram.EnableAxisXZooming = true;
        //    diagram.AxisX.WholeRange.SideMarginsValue = 0L;
        //    diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Manual;
        //    diagram.AxisX.NumericScaleOptions.MeasureUnit = NumericMeasureUnit.Ones;
        //    diagram.AxisX.NumericScaleOptions.GridOffset = 1;
        //    diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Thousands;
        //    diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
        //    diagram.AxisX.Label.TextPattern = "{A}";
        //    diagram.AxisX.Title.Text = "";
        //    diagram.AxisY.Title.Text = "";
        //}

        private void DrawChart_CrossPlot(string serieaName, Color seriesColor, int bordThickness, ViewType viewType, DataTable dt, DataTable dt2)
        {
            Series series = new Series(serieaName, ViewType.Point);
            series.View.Color = seriesColor;

            var minDataSource = dt.AsEnumerable().ToList();
            var maxDataSource = dt2.AsEnumerable().ToList();

            //this.m_chart.Series[0].Points.Clear();
            if (minDataSource.Count() != maxDataSource.Count())
            {
                MessageBox.Show(new Form { TopMost = true }, "CrossPlot 데이터의 개수가 다릅니다.");
                return;
            }
            for (int i = 0; i < minDataSource.Count(); i++)
            {
                series.Points.Add(new SeriesPoint(minDataSource[i]["Value"], maxDataSource[i]["Value"]));
            }
       
            series.ArgumentScaleType = ScaleType.Auto;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            this.m_chart.Series.Add(series);

            this.m_chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            this.m_chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;

            diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.Default;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
            //diagram.AxisX.WholeRange.SideMarginsValue = 0L;
            //diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Manual;
            //diagram.AxisX.NumericScaleOptions.MeasureUnit = NumericMeasureUnit.Ones;
            //diagram.AxisX.NumericScaleOptions.GridOffset = 1;
            //diagram.AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Thousands;
            //diagram.AxisX.NumericScaleOptions.GridSpacing = 1;
            diagram.AxisX.Label.TextPattern = "{A}";
            diagram.AxisX.Title.Text = "";
            diagram.AxisY.Title.Text = "";


            this.m_chart.Titles.Clear();
            ChartTitle chartTitle = new ChartTitle();
            chartTitle.Text = plotModuleControl.GetDocumentName() == null ? plotName : plotModuleControl.GetDocumentName();
            chartTitle.Alignment = StringAlignment.Center;
            chartTitle.Dock = ChartTitleDockStyle.Top;
            chartTitle.Font = new Font("Tahoma", 14, FontStyle.Bold);
            chartTitle.TextColor = Color.White;
            this.m_chart.Titles.Add(chartTitle);

            if (plotAxisInfo != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(plotAxisInfo.xTitle))
                    {
                        diagram.AxisX.Title.Text = plotAxisInfo.xTitle;
                        diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    }

                    if (!string.IsNullOrEmpty(plotAxisInfo.yTitle))
                    {
                        diagram.AxisY.Title.Text = plotAxisInfo.yTitle;
                        diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.diagramType))
                    {
                        switch (plotAxisInfo.diagramType)
                        {
                            case "Time":
                                diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                                diagram.AxisX.DateTimeScaleOptions.AutoGrid = false;
                                diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffffff}";

                                diagram.RangeControlDateTimeGridOptions.GridMode = ChartRangeControlClientGridMode.Manual;
                                diagram.RangeControlDateTimeGridOptions.GridOffset = 1;

                                diagram.RangeControlDateTimeGridOptions.LabelFormat = "HH:mm:ss.fff";
                                if (!string.IsNullOrEmpty(plotAxisInfo.xGridAlign))
                                {
                                    if (plotAxisInfo.xGridAlign == "Millisecond")
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
                                        diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;
                                        diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Millisecond;
                                    }
                                    if (plotAxisInfo.xGridAlign == "Second")
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
                                        diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
                                        diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Second;
                                    }
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMaxRange))
                                {
                                    try
                                    {
                                        DateTime maxTime;
                                        DateTime.TryParse(plotAxisInfo.xMaxRange, out maxTime);
                                        if (maxTime != default(DateTime))
                                        {
                                            DateTime nowMaxTime = (DateTime)diagram.AxisX.WholeRange.MaxValue;
                                            DateTime changeMaxTime = new DateTime(nowMaxTime.Year, nowMaxTime.Month, nowMaxTime.Day, maxTime.Hour, maxTime.Minute, maxTime.Second, maxTime.Millisecond);
                                            diagram.AxisX.WholeRange.SetMinMaxValues(diagram.AxisX.WholeRange.MinValue, changeMaxTime);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                                if (!string.IsNullOrEmpty(plotAxisInfo.xMinRange))
                                {

                                    try
                                    {
                                        DateTime minTime;
                                        DateTime.TryParse(plotAxisInfo.xMinRange, out minTime);
                                        if (minTime != default(DateTime))
                                        {
                                            DateTime nowMinTime = (DateTime)diagram.AxisX.WholeRange.MinValue;
                                            DateTime changeMinTime = new DateTime(nowMinTime.Year, nowMinTime.Month, nowMinTime.Day, minTime.Hour, minTime.Minute, minTime.Second, minTime.Millisecond);
                                            diagram.AxisX.WholeRange.SetMinMaxValues(changeMinTime, diagram.AxisX.WholeRange.MaxValue);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xSpacing))
                                {
                                    double xSpacing = 0;
                                    double.TryParse(plotAxisInfo.xSpacing, out xSpacing);
                                    if (xSpacing != 0)
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.GridSpacing = xSpacing;
                                        //diagram.RangeControlDateTimeGridOptions.GridSpacing = xSpacing;
                                    }
                                }


                                break;
                            case "Index":
                                //diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Manual;
                                diagram.AxisX.NumericScaleOptions.AutoGrid = false;
                                diagram.AxisX.Label.TextPattern = "{A}";

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMaxRange))
                                {
                                    double xMaxRange = 0;
                                    double.TryParse(plotAxisInfo.xMaxRange, out xMaxRange);
                                    diagram.AxisX.WholeRange.MaxValue = xMaxRange;
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMinRange))
                                {
                                    double xMinRange = 0;
                                    double.TryParse(plotAxisInfo.xMinRange, out xMinRange);
                                    diagram.AxisX.WholeRange.MinValue = xMinRange;
                                }
                                if (!string.IsNullOrEmpty(plotAxisInfo.xSpacing))
                                {
                                    double xSpacing = 0;
                                    double.TryParse(plotAxisInfo.xSpacing, out xSpacing);
                                    if (xSpacing != 0)
                                    {
                                        diagram.AxisX.NumericScaleOptions.GridSpacing = xSpacing;
                                    }
                                }
                                break;
                        }
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.yMinRange))
                    {
                        double yMinRange = 0;
                        double.TryParse(plotAxisInfo.yMinRange, out yMinRange);
                        diagram.AxisY.WholeRange.MinValue = yMinRange;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.yMaxRange))
                    {
                        double yMaxRange = 0;
                        double.TryParse(plotAxisInfo.yMaxRange, out yMaxRange);
                        diagram.AxisY.WholeRange.MaxValue = yMaxRange;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.ySpacing))
                    {
                        double ySpacing = 0;
                        double.TryParse(plotAxisInfo.ySpacing, out ySpacing);
                        if (ySpacing != 0)
                        {
                            diagram.AxisY.NumericScaleOptions.AutoGrid = false;
                            diagram.AxisY.NumericScaleOptions.GridSpacing = ySpacing;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(new Form { TopMost = true }, plotName + " 의 축정보 설정 중 오류가 발생했습니다. 축정보를 확인해주세요.");
                }
            }
            PointSeriesView seriesView = m_chart.Series[0].View as PointSeriesView;
            seriesView.PointMarkerOptions.Kind = MarkerKind.Circle;
            seriesView.PointMarkerOptions.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Hatch;
            seriesView.PointMarkerOptions.Size = 5;

            if (viewType == ViewType.Line)
            {
                ProcessAutoClusterSetCross(series.Points, seriesColor, bordThickness);
            }
        }

        private void DrawChart_PotatoPlot(string serieaName,Color seriesColor, int bordThickness, ViewType viewType, DataTable dt)
        {
            //dxGridDataList.Add(new dxGridData("series", "Potato", this.cbSeries.Text, this.cbSeries2.Text));
            Dictionary<string, Series> seriesInfo = new Dictionary<string, Series>();
            Series series = new Series();
            foreach (DataRow row in dt.Rows)
            {
                if (seriesInfo.TryGetValue(serieaName, out series) == false)
                {
                    series = new Series(serieaName, ViewType.Point);
                    series.View.Color = seriesColor;
                    seriesInfo.Add(serieaName, series);

                    series.ArgumentDataMember = "Value";
                    series.ArgumentScaleType = ScaleType.Numerical;
                    series.ValueDataMembers.AddRange(new string[] { "Value1" });
                    series.ValueScaleType = ScaleType.Numerical;

                    this.m_chart.Series.Add(series);
                }

                double val1 = double.Parse(row["Value"].ToString());
                double val2 = double.Parse(row["Value1"].ToString());
                SeriesPoint point = new SeriesPoint(val1, val2);
                point.Color = seriesColor;
                series.Points.Add(point);
            }

            XYDiagram diagram = this.m_chart.Diagram as XYDiagram;
            diagram.AxisX.Title.Text = "";
            diagram.AxisY.Title.Text = "";

            //diagram.AxisX.Title.Visibility = string.IsNullOrEmpty(axisTitleX) ? DevExpress.Utils.DefaultBoolean.False : DevExpress.Utils.DefaultBoolean.True;
            //diagram.AxisX.Title.Alignment = StringAlignment.Center;
            //diagram.AxisX.Title.Text = axisTitleX;

            //diagram.AxisY.Title.Visibility = string.IsNullOrEmpty(axisTitleY) ? DevExpress.Utils.DefaultBoolean.False : DevExpress.Utils.DefaultBoolean.True;
            //diagram.AxisY.Title.Alignment = StringAlignment.Center;
            //diagram.AxisY.Title.Text = axisTitleY;

            //var title1 = new ChartTitle();
            //title1.Text = string.IsNullOrEmpty(title) ? m_chart.Series[0].Name : title;
            //m_chart.Titles.Add(title1);
            this.m_chart.Titles.Clear();
            ChartTitle chartTitle = new ChartTitle();
            chartTitle.Text = plotModuleControl.GetDocumentName() == null ? plotName : plotModuleControl.GetDocumentName();
            chartTitle.Alignment = StringAlignment.Center;
            chartTitle.Dock = ChartTitleDockStyle.Top;
            chartTitle.Font = new Font("Tahoma", 14, FontStyle.Bold);
            chartTitle.TextColor = Color.White;
            this.m_chart.Titles.Add(chartTitle);

            if (plotAxisInfo != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(plotAxisInfo.xTitle))
                    {
                        diagram.AxisX.Title.Text = plotAxisInfo.xTitle;
                        diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    }

                    if (!string.IsNullOrEmpty(plotAxisInfo.yTitle))
                    {
                        diagram.AxisY.Title.Text = plotAxisInfo.yTitle;
                        diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.diagramType))
                    {
                        switch (plotAxisInfo.diagramType)
                        {
                            case "Time":
                                diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                                diagram.AxisX.DateTimeScaleOptions.AutoGrid = false;
                                diagram.AxisX.Label.TextPattern = "{A:HH:mm:ss.ffffff}";

                                diagram.RangeControlDateTimeGridOptions.GridMode = ChartRangeControlClientGridMode.Manual;
                                diagram.RangeControlDateTimeGridOptions.GridOffset = 1;

                                diagram.RangeControlDateTimeGridOptions.LabelFormat = "HH:mm:ss.fff";
                                if (!string.IsNullOrEmpty(plotAxisInfo.xGridAlign))
                                {
                                    if (plotAxisInfo.xGridAlign == "Millisecond")
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
                                        diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Millisecond;
                                        diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Millisecond;
                                    }
                                    if (plotAxisInfo.xGridAlign == "Second")
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
                                        diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
                                        diagram.RangeControlDateTimeGridOptions.SnapAlignment = DateTimeGridAlignment.Second;
                                    }
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMaxRange))
                                {
                                    try
                                    {
                                        DateTime maxTime;
                                        DateTime.TryParse(plotAxisInfo.xMaxRange, out maxTime);
                                        if (maxTime != default(DateTime))
                                        {
                                            DateTime nowMaxTime = (DateTime)diagram.AxisX.WholeRange.MaxValue;
                                            DateTime changeMaxTime = new DateTime(nowMaxTime.Year, nowMaxTime.Month, nowMaxTime.Day, maxTime.Hour, maxTime.Minute, maxTime.Second, maxTime.Millisecond);
                                            diagram.AxisX.WholeRange.SetMinMaxValues(diagram.AxisX.WholeRange.MinValue, changeMaxTime);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                                if (!string.IsNullOrEmpty(plotAxisInfo.xMinRange))
                                {

                                    try
                                    {
                                        DateTime minTime;
                                        DateTime.TryParse(plotAxisInfo.xMinRange, out minTime);
                                        if (minTime != default(DateTime))
                                        {
                                            DateTime nowMinTime = (DateTime)diagram.AxisX.WholeRange.MinValue;
                                            DateTime changeMinTime = new DateTime(nowMinTime.Year, nowMinTime.Month, nowMinTime.Day, minTime.Hour, minTime.Minute, minTime.Second, minTime.Millisecond);
                                            diagram.AxisX.WholeRange.SetMinMaxValues(changeMinTime, diagram.AxisX.WholeRange.MaxValue);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xSpacing))
                                {
                                    double xSpacing = 0;
                                    double.TryParse(plotAxisInfo.xSpacing, out xSpacing);
                                    if (xSpacing != 0)
                                    {
                                        diagram.AxisX.DateTimeScaleOptions.GridSpacing = xSpacing;
                                        //diagram.RangeControlDateTimeGridOptions.GridSpacing = xSpacing;
                                    }
                                }


                                break;
                            case "Index":
                                diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Manual;
                                diagram.AxisX.NumericScaleOptions.AutoGrid = false;
                                diagram.AxisX.Label.TextPattern = "{A}";

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMaxRange))
                                {
                                    double xMaxRange = 0;
                                    double.TryParse(plotAxisInfo.xMaxRange, out xMaxRange);
                                    diagram.AxisX.WholeRange.MaxValue = xMaxRange;
                                }

                                if (!string.IsNullOrEmpty(plotAxisInfo.xMinRange))
                                {
                                    double xMinRange = 0;
                                    double.TryParse(plotAxisInfo.xMinRange, out xMinRange);
                                    diagram.AxisX.WholeRange.MinValue = xMinRange;
                                }
                                if (!string.IsNullOrEmpty(plotAxisInfo.xSpacing))
                                {
                                    double xSpacing = 0;
                                    double.TryParse(plotAxisInfo.xSpacing, out xSpacing);
                                    if (xSpacing != 0)
                                    {
                                        diagram.AxisX.NumericScaleOptions.GridSpacing = xSpacing;
                                    }
                                }
                                break;
                        }
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.yMinRange))
                    {
                        double yMinRange = 0;
                        double.TryParse(plotAxisInfo.yMinRange, out yMinRange);
                        diagram.AxisY.WholeRange.MinValue = yMinRange;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.yMaxRange))
                    {
                        double yMaxRange = 0;
                        double.TryParse(plotAxisInfo.yMaxRange, out yMaxRange);
                        diagram.AxisY.WholeRange.MaxValue = yMaxRange;
                    }
                    if (!string.IsNullOrEmpty(plotAxisInfo.ySpacing))
                    {
                        double ySpacing = 0;
                        double.TryParse(plotAxisInfo.ySpacing, out ySpacing);
                        if (ySpacing != 0)
                        {
                            diagram.AxisY.NumericScaleOptions.AutoGrid = false;
                            diagram.AxisY.NumericScaleOptions.GridSpacing = ySpacing;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(new Form { TopMost = true }, plotName + " 의 축정보 설정 중 오류가 발생했습니다. 축정보를 확인해주세요.");
                }
            }

            PointSeriesView seriesView = m_chart.Series[0].View as PointSeriesView;
            seriesView.PointMarkerOptions.Kind = MarkerKind.Circle;
            seriesView.PointMarkerOptions.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Hatch;
            seriesView.PointMarkerOptions.Size = 5;
            ProcessAutoClusterSet(series.Points, seriesColor, bordThickness);
        }

        private void btn_ChangePlotName_Click(object sender, EventArgs e)
        {
            plotModuleControl.GetDocument();
        }

        private void btn_AddTags_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            if(panel != null)
            {
                panel.Close();
            }
            panel = new DockPanel();
            TagControl tagControl = new TagControl(tagValue, this);
            panel = mainForm.DockManager1.AddPanel(DockingStyle.Float);
            panel.FloatLocation = new Point(500, 100);
            panel.FloatSize = new Size(450, 250);
            panel.Name = "태그";
            panel.Text = "태그";
            tagControl.Dock = DockStyle.Fill;

            panel.Controls.Add(tagControl);
        }

        public void setTagValue(string tagVal)
        {
            this.tagValue = tagVal;
            panel.Close();
        }
        public string getTagValue()
        {
            return this.tagValue;
        }
        public void closePanel()
        {
            panel.Close();
        }

        public void closeAxisInfoPanel()
        {
            axisInfoPanel.Close();
        }
        public List<dxGridData> getSeriesInfo()
        {
            List<dxGridData> gridDataList = (List<dxGridData>)this.gridControl1.DataSource;
            return gridDataList;
        }
        public List<PlotPointData> GetPlotPointDatas()
        {
            return selectPointList;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            if (axisInfoPanel != null)
            {
                axisInfoPanel.Close();
            }
            axisInfoPanel = new DockPanel();
            ChangeAxisInfoControl changeAxisInfoControl = new ChangeAxisInfoControl(plotAxisInfo, this);
            axisInfoPanel = mainForm.DockManager1.AddPanel(DockingStyle.Float);
            axisInfoPanel.FloatLocation = new Point(500, 100);
            axisInfoPanel.FloatSize = new Size(440, 320);
            axisInfoPanel.Name = "축 정보 변경";
            axisInfoPanel.Text = "축 정보 변경";
            changeAxisInfoControl.Dock = DockStyle.Fill;

            axisInfoPanel.Controls.Add(changeAxisInfoControl);
        }

        public PlotAxisInfo GetPlotAxisInfo()
        {
            return this.plotAxisInfo;
        }

        public void SetPlotAxisInfo(PlotAxisInfo plotAxisInfo)
        {
            this.plotAxisInfo = plotAxisInfo;
            axisInfoPanel.Close();
        }
    }

    #region 1D SeriesPoint Data
    public class SeriesPointData
    {
        public string SeriesName { get; private set; }
        //public DateTime Argument { get; private set; }
        public int Argument { get; private set; }
        public double Value { get; private set; }
        public double Value1 { get; private set; }
        public string TimeValue { get; private set; }

        public SeriesPointData(string name, int arg, double val)
        {
            SeriesName = name;
            Argument = arg;
            Value = val;
        }

        public SeriesPointData(string name, int arg, double val, double val1)
        {
            SeriesName = name;
            Argument = arg;
            Value = val;
            Value1 = val1;
        }
        public SeriesPointData(string name, string timeValue, double val)
        {
            SeriesName = name;
            TimeValue = timeValue;
            Value = val;
        }
    }
    #endregion

    #region For Potato
    public class DLL_DATA
    {
        public string Name { get; set; }
        public string ButtockLine { get; set; }
        public List<Parameter> parameters { get; set; }
        public class Parameter
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Unit { get; set; }
            public double MinVal { get; set; }
            public double MaxVal { get; set; }
            public DataTable data { get; set; }
        }
    }
    #endregion

    #region CustomPaint
    public class Cluster
    {
        List<PointF> points = new List<PointF>();
        List<PointF> sortedPoints = new List<PointF>();
        List<PointF> contourPoints = new List<PointF>();
        bool isClusterSelected = false;

        public List<PointF> Points { get { return points; } }
        public List<PointF> SortedPoints { get { return sortedPoints; } }
        public List<PointF> ContourPoints { get { return contourPoints; } }
        public bool IsSelected { get { return isClusterSelected; } set { isClusterSelected = value; } }

        public void Calculate(List<PointF> points)
        {
            this.points = points;
            sortedPoints = DiagramToPointHelper.Sort(points);
            contourPoints = DiagramToPointHelper.CreateClosedCircuit(sortedPoints);
            isClusterSelected = false;
        }

        public void CalculateCross(List<PointF> points)
        {
            this.points = points;
            sortedPoints = points;
            contourPoints = points;
            isClusterSelected = false;
        }
        public void Clear()
        {
            points.Clear();
            sortedPoints.Clear();
            contourPoints.Clear();
            isClusterSelected = false;
        }
    }


    static class DiagramToPointHelper
    {
        const double Epsilon = 0.001;

        static PointF CalcRandomPoint(Random random, int xCenter, int yCenter)
        {
            const int dispersion = 2;
            const int expectedSum = 6;
            PointF point = new PointF();
            double sum = 0;
            for (int i = 0; i < 12; i++)
                sum += random.NextDouble();
            double radius = (sum - expectedSum) * dispersion;
            double angle = random.Next(360) * Math.PI / 180;
            point.X = (float)(xCenter + radius * Math.Cos(angle));
            point.Y = (float)(yCenter + radius * Math.Sin(angle));
            return point;
        }
        static bool AreEqual(PointF point1, PointF point2)
        {
            return AreEqual(point1.X, point2.X) && AreEqual(point1.Y, point2.Y);
        }
        static bool AreEqual(double number1, double number2)
        {
            double difference = number1 - number2;
            if (Math.Abs(difference) <= Epsilon)
                return true;
            return false;
        }
        static PointF GetClusterCenter(List<PointF> cluster)
        {
            if (cluster.Count == 0)
                return PointF.Empty;
            float centerX = 0;
            float centerY = 0;
            foreach (PointF point in cluster)
            {
                centerX += point.X;
                centerY += point.Y;
            }
            centerX /= cluster.Count;
            centerY /= cluster.Count;
            return new PointF(centerX, centerY);
        }
        static void CreateUpperArc(List<PointF> cluster, List<PointF> sortedCluster)
        {
            for (int i = 1; i < cluster.Count; i++)
            {
                if (i + 1 == cluster.Count)
                {
                    sortedCluster.Add(cluster[i]);
                    break;
                }
                bool shouldAddPoint = false;
                float x0 = sortedCluster[sortedCluster.Count - 1].X;
                float y0 = sortedCluster[sortedCluster.Count - 1].Y;
                float x1 = cluster[i].X;
                float y1 = cluster[i].Y;
                if (x1 == x0)
                {
                    if (y0 < y1)
                        shouldAddPoint = true;
                }
                else
                    for (int j = i + 1; j < cluster.Count; j++)
                    {
                        if (cluster[j].Y >= (double)(cluster[j].X - x0) * (double)(y1 - y0) / (double)(x1 - x0) + y0)
                        {
                            shouldAddPoint = false;
                            break;
                        }
                        else
                            shouldAddPoint = true;
                    }
                if (shouldAddPoint)
                    sortedCluster.Add(cluster[i]);
            }
        }
        static void CreateBottomArc(List<PointF> cluster, List<PointF> sortedCluster)
        {
            for (int i = cluster.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    sortedCluster.Add(cluster[i]);
                    break;
                }
                bool shouldAddPoint = false;
                float x0 = sortedCluster[sortedCluster.Count - 1].X;
                float y0 = sortedCluster[sortedCluster.Count - 1].Y;
                float x1 = cluster[i].X;
                float y1 = cluster[i].Y;
                if (x1 == x0)
                {
                    if (y0 > y1)
                        shouldAddPoint = true;
                }
                else
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (cluster[j].Y <= (double)(cluster[j].X - x0) * (double)(y1 - y0) / (double)(x1 - x0) + y0)
                        {
                            shouldAddPoint = false;
                            break;
                        }
                        else
                            shouldAddPoint = true;
                    }
                if (shouldAddPoint)
                    sortedCluster.Add(cluster[i]);
            }
        }

        public static Rectangle CreateRectangle(Point corner1, Point corner2)
        {
            int x = corner1.X < corner2.X ? corner1.X : corner2.X;
            int y = corner1.Y < corner2.Y ? corner1.Y : corner2.Y;
            int width = Math.Abs(corner1.X - corner2.X);
            int height = Math.Abs(corner1.Y - corner2.Y);
            return new Rectangle(x, y, width, height);
        }
        public static RectangleF CreateRectangle(PointF corner1, PointF corner2)
        {
            float x = corner1.X < corner2.X ? corner1.X : corner2.X;
            float y = corner1.Y < corner2.Y ? corner1.Y : corner2.Y;
            float width = Math.Abs(corner1.X - corner2.X);
            float height = Math.Abs(corner1.Y - corner2.Y);
            return new RectangleF(x, y, width, height);
        }
        public static Point GetLastSelectionCornerPosition(Point p, Rectangle bounds)
        {
            if (p.X < bounds.Left)
                p.X = bounds.Left;
            else if (p.X > bounds.Right)
                p.X = bounds.Right - 1;
            if (p.Y < bounds.Top)
                p.Y = bounds.Top;
            else if (p.Y > bounds.Bottom)
                p.Y = bounds.Bottom - 1;
            return p;
        }
        public static SeriesPoint[] CalculatePoints(Random random, int count, int xCenter, int yCenter)
        {
            SeriesPoint[] seriesPoints = new SeriesPoint[count];
            for (int i = 0; i < count; i++)
            {
                PointF point = CalcRandomPoint(random, xCenter, yCenter);
                seriesPoints[i] = new SeriesPoint(point.X, new double[] { point.Y });
            }
            return seriesPoints;
        }
        public static void CalculateClusters(SeriesPointCollection seriesPoints, List<PointF> cluster1, List<PointF> cluster2, List<PointF> cluster3)
        {
            List<PointF> points = new List<PointF>();
            foreach (SeriesPoint point in seriesPoints)
                points.Add(new PointF((float)point.NumericalArgument, (float)point.Values[0]));
            //if (points.Count < 100)
            //    return;
            PointF nextCenter1 = points[0];
            //PointF nextCenter2 = points[50];
            //PointF nextCenter3 = points[100];
            PointF center1;
            //PointF center2;
            //PointF center3;
            do
            {
                center1 = nextCenter1;
                //center2 = nextCenter2;
                //center3 = nextCenter3;
                cluster1.Clear();
                //cluster2.Clear();
                //cluster3.Clear();
                foreach (PointF point in points)
                {
                    float x = point.X;
                    float y = point.Y;
                    double distance1 = Math.Sqrt((center1.X - x) * (center1.X - x) + (center1.Y - y) * (center1.Y - y));
                    cluster1.Add(point);
                    //double distance2 = Math.Sqrt((center2.X - x) * (center2.X - x) + (center2.Y - y) * (center2.Y - y));
                    //double distance3 = Math.Sqrt((center3.X - x) * (center3.X - x) + (center3.Y - y) * (center3.Y - y));
                    //if (distance1 <= distance2 && distance1 <= distance3)
                    //    cluster1.Add(point);
                    //else if (distance2 <= distance1 && distance2 <= distance3)
                    //    cluster2.Add(point);
                    //else
                    //    cluster3.Add(point);
                }
                nextCenter1 = GetClusterCenter(cluster1);
                //nextCenter2 = GetClusterCenter(cluster2);
                //nextCenter3 = GetClusterCenter(cluster3);
            } while (!AreEqual(center1, nextCenter1) /*|| !AreEqual(center2, nextCenter2) || !AreEqual(center3, nextCenter3)*/);
        }
        public static List<PointF> Sort(List<PointF> cluster)
        {
            List<PointF> sortedCluster = new List<PointF>();
            if (cluster.Count == 0)
                return sortedCluster;
            sortedCluster.Add(cluster[0]);
            for (int i = 1; i < cluster.Count; i++)
            {
                if (sortedCluster[0].X >= cluster[i].X)
                    sortedCluster.Insert(0, cluster[i]);
                else if (sortedCluster[sortedCluster.Count - 1].X <= cluster[i].X)
                    sortedCluster.Insert(sortedCluster.Count, cluster[i]);
                else
                    for (int j = 0; j < sortedCluster.Count - 1; j++)
                    {
                        if (sortedCluster[j].X <= cluster[i].X && sortedCluster[j + 1].X >= cluster[i].X)
                        {
                            sortedCluster.Insert(j + 1, cluster[i]);
                            break;
                        }
                    }
            }
            return sortedCluster;
        }
        public static List<PointF> CreateClosedCircuit(List<PointF> sortedCluster)
        {
            List<PointF> contourPoints = new List<PointF>();
            if (sortedCluster.Count == 0)
                return contourPoints;
            contourPoints.Add(sortedCluster[0]);
            CreateUpperArc(sortedCluster, contourPoints);
            CreateBottomArc(sortedCluster, contourPoints);
            return contourPoints;
        }
    }

    #endregion // CustomPaint
}
