using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DynaRAP.Data;
using DynaRAP.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class SelectPointTableControl : DevExpress.XtraEditors.XtraUserControl
    {
        List<PlotPointData> plotPointDataList = new List<PlotPointData>();
        public SelectPointTableControl()
        {
            InitializeComponent();
        }


        private void SelectPointTableControl_Load(object sender, EventArgs e)
        {
            InitGridControl1();
            //if(File.Exists(csvFilePath))
            //FillGrid();
        }

        public void InitGridControl1()
        {
            //gridView1.Columns.Clear();
            gridControl1.DataSource = null;
         
            gridView1.OptionsView.ShowColumnHeaders = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            //gridView1.OptionsView.ColumnAutoWidth = false;

            gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;

        }

        public void AddPlotPointData(PlotPointData plotPointData)
        {
            if (plotPointData != null)
            {
                plotPointDataList.Add(plotPointData);
                gridControl1.DataSource = plotPointDataList;
                gridView1.RefreshData();
            }
        }

        public void RemovePlotPointData(PlotPointData plotPointData)
        {
            if (plotPointData != null)
            {
                if (plotPointDataList != null) { 
                plotPointDataList.Remove(plotPointData);
                gridControl1.DataSource = plotPointDataList;
                gridView1.RefreshData();
                }
            }
        }

        public void ClearPlotPointData()
        {
            plotPointDataList.Clear();
            gridControl1.DataSource = plotPointDataList;
            gridView1.RefreshData();
        }
        private void btn_ShowCsv_Click(object sender, EventArgs e)
        {
            int[] selectedRowIndex = gridView1.GetSelectedRows();
            if(selectedRowIndex.Count() == 0)
            {
                MessageBox.Show("선택된 Point가 없습니다. 다시 확인해주세요.");
            }
            string originSeq = null;
            string originType = null;
            string sourceType = null;
            List<string> selectTime = new List<string>();
            List<PlotPointData> eqPlotPointDataList = new List<PlotPointData>() ;
            foreach (var index in selectedRowIndex)
            {
                PlotPointData plotPointData = (PlotPointData)gridView1.GetRow(index);
                selectTime.Add(plotPointData.xAxis);
                sourceType = plotPointData.ySourceType;
                if(plotPointData.ySourceType == "eq")
                {
                    eqPlotPointDataList.Add(plotPointData);
                    if (originSeq == null)
                    {
                        originSeq = plotPointData.ySourceSeq;
                        originType = plotPointData.ySourceType;
                        continue;
                    }
                    if (originSeq != plotPointData.ySourceSeq || originType != plotPointData.ySourceType)
                    {
                        MessageBox.Show("선택된 Point중 다른 데이터의 값이 있습니다. 다시 확인해주세요.");
                        return;
                    }
                }
                else
                {
                    if (originSeq == null)
                    {
                        originSeq = plotPointData.yOriginSeq;
                        originType = plotPointData.ySourceType;
                        if (plotPointData.ySourceType == "eq")
                        {
                            eqPlotPointDataList.Add(plotPointData);
                        }
                        continue;
                    }
                    if (originSeq != plotPointData.yOriginSeq || originType != plotPointData.ySourceType)
                    {
                        MessageBox.Show("선택된 Point중 다른 데이터의 값이 있습니다. 다시 확인해주세요.");
                        return;
                    }
                }
               
            }
            switch (sourceType)
            {
                case "shortblock" :
                case "part":
                    partShortblockCsvShow(originSeq, sourceType, selectTime);
                    break;
                case "eq":
                    eqShortBlockCsvShow(eqPlotPointDataList);
                    break;
            }
        }

        private void partShortblockCsvShow(string seq, string sourceType, List<string> selectTime)
        {

            if (seq == null)
            {
                MessageBox.Show("선택된 데이터의 원본 데이터가 없거나 찾을 수 없습니다.");

                return;
            }
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("선택된 데이터를 불러오는 중입니다.. 잠시만 기다려주십시오.");
            mainForm.PanelSelectPointTable.Show();
            mainForm.SelectCsvTableControl.CrearDataTable();
            mainForm.SelectCsvTableControl.FillGridPartShortBlock(seq, sourceType, selectTime);
            mainForm.HideSplashScreenManager();
        }

        private void eqShortBlockCsvShow(List<PlotPointData> eqPlotPointData)
        {

            string seq = string.Empty;
            string sourceType = string.Empty;
            List<string> selectTime = new List<string>();
            
            foreach(PlotPointData eqPlotPoint in eqPlotPointData)
            {
                if (eqPlotPoint.ySourceSeq == null || eqPlotPoint.yOriginSeq == null)
                {
                    MessageBox.Show("선택된 데이터의 원본 데이터가 없거나 찾을 수 없습니다.");
                    return;
                }

                FindSource findSource = FindSourceData(eqPlotPoint);
                if (findSource == null)
                {
                    return;
                }
                if(findSource.timeSet != null)
                {
                    foreach(string time in findSource.timeSet)
                    {
                        if (selectTime.Contains(time) == false)
                        {
                            selectTime.Add(time);
                        }
                    }
                }
                if (findSource.partSeq != null && findSource.partSeq != "")
                {
                    seq = findSource.partSeq;
                    sourceType = "part";
                }
                if (findSource.blockSeq != null && findSource.blockSeq != "")
                {
                    seq = findSource.blockSeq;
                    sourceType = "shortblock";
                }
            }
            
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("선택된 데이터를 불러오는 중입니다.. 잠시만 기다려주십시오.");
            mainForm.PanelSelectPointTable.Show();
            mainForm.SelectCsvTableControl.CrearDataTable();
            mainForm.SelectCsvTableControl.FillGridPartShortBlock(seq, sourceType, selectTime);
            mainForm.HideSplashScreenManager();
        }

        private FindSource FindSourceData(PlotPointData eqPlotPointData)
        {
            FindSourceRequest findSourceRequest = new FindSourceRequest(eqPlotPointData);
            if (findSourceRequest.chartType == "Cross Plot")
            {
                findSourceRequest.chartType = "CrossPlot";
            }
            else if (findSourceRequest.chartType == "Potato Plot")
            {
                findSourceRequest.chartType = "Potato";
            }
            else
            {
                MessageBox.Show("수식데이터가 1D-Time History에 사용된 경우\n Point 찾기를 지원하지 않습니다. 다시 확인해주세요.");
                return null;
            }
            var json = JsonConvert.SerializeObject(findSourceRequest);
            string responseData = Utils.GetPostDataNew(ConfigurationManager.AppSettings["UrlParamModule"], json);
            if (responseData != null)
            {
                FindSourceResponse result = JsonConvert.DeserializeObject<FindSourceResponse>(responseData);
                if (result.code == 200)
                {
                    return result.response;
                }
            }

            return null;
        }
    }
}
