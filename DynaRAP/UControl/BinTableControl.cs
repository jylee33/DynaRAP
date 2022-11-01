using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using DynaRAP.Data;
using DynaRAP.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class BinTableControl : DevExpress.XtraEditors.XtraUserControl
    {
        DockPanel binSBTabPanel = null;
        BinSBTabControl binSBTabCtrl = null;
        List<ParamDatas> allParamList = null;
        List<ParamDatas> paramDataList = null;
        List<PickUpParam> pickUpParamList = null;
        List<string> shortBlockSeqList = null;
        List<BINParamCombo> paramComboList = null;
        Dictionary<string,string> firstColNameList= null;
        List<ResponseParamList> responseParamList = null;
        MainForm mainForm = null;
        string binMetaSeq = null;
        string binTableName = null;
        Dictionary<string, Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, BINMetaData>>> rangeDic = new Dictionary<string, Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, BINMetaData>>>();
        Dictionary<int[], dynamic> pointDic = new Dictionary<int[], dynamic>();
        double maxValue = 0;

        public BinTableControl(List<ParamDatas> allParamList , List<ParamDatas> paramDataList, List<PickUpParam> pickUpParamList, List<string> shortBlockSeqList, MainForm mainForm, string binTableName, string binMetaSeq)
        {
            this.paramDataList = paramDataList;
            this.pickUpParamList = pickUpParamList;
            this.shortBlockSeqList = shortBlockSeqList;
            this.mainForm = mainForm;
            this.binMetaSeq = binMetaSeq;
            this.binTableName = binTableName;
            this.allParamList = new List<ParamDatas>(); ;
            foreach (var paramData in allParamList)
            {
                if (paramData.propInfo != null)
                {
                    if (paramData.propInfo.propType != "FLIGHT")
                    {
                        this.allParamList.Add(paramData);
                    }
                }
            }

            InitializeComponent();
        }

        private void BinTableControl_Load(object sender, EventArgs e)
        {
            mainForm.ShowSplashScreenManager("BIN테이블을 생성중입니다. 잠시만 기다려주십시오.");
            InitComboBoxList();
            firstColNameList = new Dictionary<string, string>();

            if (pickUpParamList.Count == 3)
            {
                string valueName = paramDataList[0].paramKey + paramDataList[2].paramKey;
                //DataTable dt = GetDataTable(paramDataList[i - 1], paramDataList[i]);
                GetShortBlockParamList();
                foreach (var paramData in pickUpParamList[2].userParamTable.Select((value, index) => new { value, index }))
                {
                    string showName = string.Format("{0}-{1}({2}-{3})", paramDataList[0].propInfo.paramUnit, paramDataList[2].propInfo.paramUnit, paramData.value.min, paramData.value.max);
                    AddTabPage(showName, valueName + paramData.value.min, paramDataList[1], paramDataList[0], pickUpParamList[2].paramSeq, paramData.value, paramData.index);
                }
            } else
            {
                string valueName = paramDataList[0].paramKey + paramDataList[1].paramKey;
                string showName = string.Format("{0}-{1}", paramDataList[0].propInfo.paramUnit, paramDataList[1].propInfo.paramUnit);
                GetShortBlockParamList();
                AddTabPage2D(showName, valueName, paramDataList[0], paramDataList[1]);
            }
            //for (int i = 0; i < paramDataList.Count() - 1; i++)
            //{
            //    for (int j = i + 1; j < paramDataList.Count(); j++)
            //    {
            //        string showName = string.Format("{0}-{1}", paramDataList[i].propInfo.paramUnit, paramDataList[j].propInfo.paramUnit);
            //        string valueName = paramDataList[i].paramKey + paramDataList[j].paramKey;
            //        //DataTable dt = GetDataTable(paramDataList[i - 1], paramDataList[i]);
            //        GetShortBlockParamList();
            //        AddTabPage(showName, valueName, paramDataList[i], paramDataList[j]);
            //    }
            //}
           
            mainForm.HideSplashScreenManager();


        }

        private void ChangeTabPage()
        {
            mainForm.ShowSplashScreenManager("BIN테이블 표시 내용을 변경 중입니다. 잠시만 기다려주십시오.");
            int selectedIndex = this.xtraTabControl1.SelectedTabPageIndex;
            firstColNameList = new Dictionary<string, string>();
            maxValue = 0;

            rangeDic.Clear();
            this.xtraTabControl1.TabPages.Clear();
            if (pickUpParamList.Count == 3)
            {

                string valueName = paramDataList[0].paramKey + paramDataList[2].paramKey;
                foreach (var paramData in pickUpParamList[2].userParamTable.Select((value, index) => new { value, index }))
                {
                    string showName = string.Format("{0}-{1}({2}-{3})", paramDataList[0].propInfo.paramUnit, paramDataList[2].propInfo.paramUnit, paramData.value.min, paramData.value.max);
                    AddTabPage(showName, valueName + paramData.value.min, paramDataList[1], paramDataList[0], pickUpParamList[2].paramSeq, paramData.value, paramData.index);
                }
                this.xtraTabControl1.SelectedTabPageIndex = selectedIndex;
            }
            else
            {
                string valueName = paramDataList[0].paramKey + paramDataList[1].paramKey;
                string showName = string.Format("{0}-{1}", paramDataList[0].propInfo.paramUnit, paramDataList[1].propInfo.paramUnit);
                AddTabPage2D(showName, valueName, paramDataList[0], paramDataList[1]);
            }
            //for (int i = 0; i < paramDataList.Count() - 1; i++)
            //{
            //    for (int j = i + 1; j < paramDataList.Count(); j++)
            //    {
            //        string showName = string.Format("{0}-{1}", paramDataList[i].propInfo.paramUnit, paramDataList[j].propInfo.paramUnit);
            //        string valueName = paramDataList[i].paramKey + paramDataList[j].paramKey;
            //        //DataTable dt = GetDataTable(paramDataList[i - 1], paramDataList[i]);
            //        GetShortBlockParamList();
            //        AddTabPage(showName, valueName, paramDataList[i], paramDataList[j]);
            //    }
            //}

            mainForm.HideSplashScreenManager();
        }

        private void InitComboBoxList()
        {
            cboParam.Properties.DisplayMember = "paramKey";
            cboParam.Properties.ValueMember = "seq";
            cboParam.Properties.NullText = "";

            if (paramComboList == null)
            {
                paramComboList = new List<BINParamCombo>();
            }
            if(allParamList != null)
            {
                cboParam.Properties.DataSource = null;

                foreach (var paramData in allParamList)
                {
                    BINParamCombo combo = new BINParamCombo();
                    combo.seq = paramData.paramValueMap.seq;
                    combo.paramKey = paramData.paramKey;
                    paramComboList.Add(combo);
                }
                cboParam.Properties.DataSource = paramComboList;

                cboParam.Properties.PopulateColumns();
                cboParam.Properties.ShowHeader = false;
                cboParam.Properties.Columns["seq"].Visible = false;
                cboParam.Properties.ShowFooter = false;
                cboParam.Properties.PopulateColumns();
                cboParam.Properties.Columns["paramKey"].Width = 800;

                cboParam.EditValue = allParamList[0].paramValueMap.seq;
                //cboParam.Properties.
            }
            cboType.Properties.Items.Add("평균 RMS크기");
            cboType.Properties.Items.Add("평균 대표주파수");
            cboType.Properties.Items.Add("최대 버스트값");
            cboType.Properties.Items.Add("최대 하중/가속도 예측값");
            cboType.SelectedIndex = 0;
            cboParam.EditValueChanged += cboParamAndType_EditValueChanged;
            cboType.EditValueChanged += cboParamAndType_EditValueChanged;
        }
        private void GetShortBlockParamList()
        {
            if(responseParamList == null)
            {
                responseParamList = new List<ResponseParamList>();
            }
            responseParamList.Clear();
            //SBProgressForm form = new SBProgressForm(shortBlockSeqList.Count()-1);
            //if (form.ShowDialog() == DialogResult.Cancel)
            //{
            //}
            foreach (var shortblock in shortBlockSeqList.Select((value, index) => new { value, index }))
            {

                string sendData = string.Format(@"
                {{
                ""command"":""param-list"",
                ""blockSeq"":""{0}""
                }}", shortblock.value);
                string responseData = Utils.GetPostData(System.Configuration.ConfigurationManager.AppSettings["UrlShortBlock"], sendData);
                if (responseData != null)
                {
                    responseData = responseData.Replace(@"""[", "[");
                    responseData = responseData.Replace(@"]""", "]");
                    ResponseParamList responseParam = JsonConvert.DeserializeObject<ResponseParamList>(responseData);
                    if (responseParam.code == 200)
                    {
                        responseParamList.Add(responseParam);
                    }
                }
            }
        }

        private DataTable GetDataTable(string keyName, ParamDatas header, ParamDatas row, string paramSeq, UserParamTable minMaxData, int indexZ)
        {

            string selectParamSeq = String.Empty;
            if (cboParam.GetColumnValue("seq") != null)
                selectParamSeq = cboParam.GetColumnValue("seq").ToString();

            Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, BINMetaData>> countSeqDic = new Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, BINMetaData>>();

            DataTable dt = new DataTable();
            foreach (var list in pickUpParamList.Find(x => x.paramSeq == row.seq).userParamTable)
            {
                Dictionary<MinMaxRagne, BINMetaData> headerDic = new Dictionary<MinMaxRagne, BINMetaData>();
                foreach (var list1 in pickUpParamList.Find(x => x.paramSeq == header.seq).userParamTable)
                {
                    headerDic.Add(new MinMaxRagne(list1.min,list1.max), new BINMetaData());
                }
                countSeqDic.Add(new MinMaxRagne(list.min,list.max), headerDic);

            }

            firstColNameList.Add(keyName, header.propInfo.paramUnit);
            dt.Columns.Add(header.propInfo.paramUnit);
            foreach (var list in pickUpParamList.Find(x => x.paramSeq == header.seq).userParamTable)
            {
                dt.Columns.Add(string.Format("{0}-{1}", list.min, list.max));
            }

            foreach(var responseParam in responseParamList)
            {
                var paramDataRow = responseParam.response.paramData.Find(x => x.seq == row.seq);
                var paramDataHeader = responseParam.response.paramData.Find(x => x.seq == header.seq);
                if (paramDataRow != null && paramDataHeader != null)
                {
                    var rowValue = countSeqDic.Where(dic => (dic.Key.max > paramDataRow.paramValueMap.blockAvg) && (dic.Key.min <= paramDataRow.paramValueMap.blockAvg)).Select(x => x.Key).ToList();
                    if (rowValue.Count != 0)
                    {
                        var headerValue = countSeqDic[rowValue[0]].Where(dic => (dic.Key.max > paramDataHeader.paramValueMap.blockAvg) && (dic.Key.min <= paramDataHeader.paramValueMap.blockAvg)).Select(x => x.Key).ToList();
                        if (headerValue.Count != 0)
                        {
                            var paramStand = responseParam.response.paramData.Find(x => x.seq == paramSeq);
                            if (paramStand.paramValueMap.blockAvg < minMaxData.max && paramStand.paramValueMap.blockAvg >= minMaxData.min)
                            {
                                countSeqDic[rowValue[0]][headerValue[0]].shortblcokSeqList.Add(responseParam.response.paramData[0].paramValueMap.blockSeq);
                            }
                        }
                    }
                }
            }
            //dt.Columns.Add(header.propInfo.paramUnit);
            foreach (var list in countSeqDic.Keys.Select((value, index) => new { value, index }))
            {
                DataRow dataRow = dt.NewRow();
                dataRow[header.propInfo.paramUnit] = list.value.range;
                foreach (var dic2 in countSeqDic[list.value].Keys.Select((value, index) => new { value, index }))
                {
                    if (countSeqDic[list.value][dic2.value].shortblcokSeqList.Count() != 0)
                    {
                        dynamic calulateData = CalculateSBData(list.index, dic2.index, indexZ, countSeqDic[list.value][dic2.value].shortblcokSeqList);
                        countSeqDic[list.value][dic2.value].jsonResult = calulateData;
                        if (calulateData != null)
                        {
                            Summary summaryData = JsonConvert.DeserializeObject<Summary>(calulateData[selectParamSeq].ToString());
                            double viewValue = 0;
                            switch (cboType.Text)
                            {
                                case "평균 RMS크기":
                                    viewValue = summaryData.bpf.avg_rms;
                                    break;
                                case "평균 대표주파수":
                                    viewValue = summaryData.bpf.avg_n0;
                                    break;
                                case "최대 버스트값":
                                    viewValue = summaryData.bpf.burstFactor;
                                    break;
                                case "최대 하중/가속도 예측값":
                                    viewValue = summaryData.bpf.maxLoadAccel;
                                    break;
                            }

                            dataRow[dic2.value.range] = viewValue;
                            if (maxValue < viewValue)
                            {
                                maxValue = viewValue;
                            }
                        }
                    }
                    else
                    {
                        dataRow[dic2.value.range] = countSeqDic[list.value][dic2.value].shortblcokSeqList.Count();
                    }
                }
                //dt.Rows.Add(, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
                //countSeqDic
                //dt.Columns.Add(list.range);
                dt.Rows.Add(dataRow);
            }

            rangeDic.Add(keyName, countSeqDic);

            dt.AcceptChanges();

            return dt;
        }

        private dynamic CalculateSBData(int indexX, int indexY, int indexZ, List<string> selectValue)
        {
            dynamic returnData = null;
            if (selectValue.Count != 0)
            {
                string shortBlock = "";
                foreach (var selectData in selectValue)
                {
                    if (shortBlock == "")
                    {
                        shortBlock = string.Format(@"{0}""{1}""", shortBlock, selectData);
                    }
                    else
                    {
                        shortBlock = string.Format(@"{0},""{1}""", shortBlock, selectData);
                    }
                }
                string sendData = string.Format(@"
                {{
                ""command"":""calculate"",
                ""binMetaSeq"":""{0}"",
                ""shortBlocks"" : [{1}],
                ""factorIndexes"" : [{2},{3},{4}]
                }}", binMetaSeq, shortBlock, indexX, indexY, indexZ);
                string responseData = Utils.GetPostData(System.Configuration.ConfigurationManager.AppSettings["UrlBINTable"], sendData);
                if (responseData != null)
                {
                    dynamic result = JsonConvert.DeserializeObject(responseData);
                    if (result["code"] == 200)
                    {
                        returnData = result["response"]["summary"];
                    }
                }
            }
            return returnData;
        }


        private DataTable GetDataTable2D(string keyName, ParamDatas header, ParamDatas row)
        {
            Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, BINMetaData>> countSeqDic = new Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, BINMetaData>>();

            string selectParamSeq = String.Empty;
            if (cboParam.GetColumnValue("seq") != null)
                selectParamSeq = cboParam.GetColumnValue("seq").ToString();

            DataTable dt = new DataTable();
            foreach (var list in pickUpParamList.Find(x => x.paramSeq == row.seq).userParamTable)
            {
                Dictionary<MinMaxRagne, BINMetaData> headerDic = new Dictionary<MinMaxRagne, BINMetaData>();
                foreach (var list1 in pickUpParamList.Find(x => x.paramSeq == header.seq).userParamTable)
                {
                    headerDic.Add(new MinMaxRagne(list1.min, list1.max), new BINMetaData());
                }
                countSeqDic.Add(new MinMaxRagne(list.min, list.max), headerDic);

            }

            firstColNameList.Add(keyName, header.propInfo.paramUnit);
            dt.Columns.Add(header.propInfo.paramUnit);
            foreach (var list in pickUpParamList.Find(x => x.paramSeq == header.seq).userParamTable)
            {
                dt.Columns.Add(string.Format("{0}-{1}", list.min, list.max));
            }

            foreach (var responseParam in responseParamList)
            {
                var paramDataRow = responseParam.response.paramData.Find(x => x.seq == row.seq);
                var paramDataHeader = responseParam.response.paramData.Find(x => x.seq == header.seq);
                if (paramDataRow != null && paramDataHeader != null)
                {
                    var rowValue = countSeqDic.Where(dic => (dic.Key.max > paramDataRow.paramValueMap.blockAvg) && (dic.Key.min <= paramDataRow.paramValueMap.blockAvg)).Select(x => x.Key).ToList();
                    if (rowValue.Count != 0)
                    {
                        var headerValue = countSeqDic[rowValue[0]].Where(dic => (dic.Key.max > paramDataHeader.paramValueMap.blockAvg) && (dic.Key.min <= paramDataHeader.paramValueMap.blockAvg)).Select(x => x.Key).ToList();
                        if (headerValue.Count != 0)
                        {
                            countSeqDic[rowValue[0]][headerValue[0]].shortblcokSeqList.Add(responseParam.response.paramData[0].paramValueMap.blockSeq);
                        }
                    }
                }
            }

            int indexZ = this.xtraTabControl1.SelectedTabPageIndex;

            foreach (var list in countSeqDic.Keys.Select((value, index) => new { value, index }))
            {
                DataRow dataRow = dt.NewRow();
                dataRow[header.propInfo.paramUnit] = list.value.range;
                foreach (var dic2 in countSeqDic[list.value].Keys.Select((value, index) => new { value, index }))
                {
                    dataRow[dic2.value.range] = countSeqDic[list.value][dic2.value].shortblcokSeqList.Count();
                    if (countSeqDic[list.value][dic2.value].shortblcokSeqList.Count() != 0)
                    {
                        dynamic calulateData = CalculateSBData(list.index, dic2.index, indexZ, countSeqDic[list.value][dic2.value].shortblcokSeqList);
                        countSeqDic[list.value][dic2.value].jsonResult = calulateData;
                        Summary summaryData = JsonConvert.DeserializeObject<Summary>(calulateData[selectParamSeq].ToString());
                        double viewValue = 0;
                        switch (cboType.Text)
                        {
                            case "평균 RMS크기":
                                viewValue = summaryData.bpf.avg_rms;
                                break;
                            case "평균 대표주파수":
                                viewValue = summaryData.bpf.avg_n0;
                                break;
                            case "최대 버스트값":
                                viewValue = summaryData.bpf.burstFactor;
                                break;
                            case "최대 하중/가속도 예측값":
                                viewValue = summaryData.bpf.maxLoadAccel;
                                break;
                        }

                        dataRow[dic2.value.range] = viewValue;
                        if(maxValue < viewValue)
                        {
                            maxValue = viewValue;
                        }
                    }
                    else
                    {
                        dataRow[dic2.value.range] = countSeqDic[list.value][dic2.value].shortblcokSeqList.Count();
                    }
                }
                //dt.Rows.Add(, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
                //countSeqDic
                //dt.Columns.Add(list.range);
                dt.Rows.Add(dataRow);
            }



            rangeDic.Add(keyName, countSeqDic);

            dt.AcceptChanges();

            return dt;
        }
        //private DataTable GetDataTable()
        //{
        //    DataTable dt = new DataTable();

        //    dt.Columns.Add("AOA");
        //    dt.Columns.Add("Column1");
        //    dt.Columns.Add("Column2");
        //    dt.Columns.Add("Column3");
        //    dt.Columns.Add("Column4");
        //    dt.Columns.Add("Column5");
        //    dt.Columns.Add("Column6");
        //    dt.Columns.Add("Column7");
        //    dt.Columns.Add("Column8");
        //    dt.Columns.Add("Column9");
        //    dt.Columns.Add("Column10");
        //    dt.Columns.Add("Column11");
        //    dt.Columns.Add("Column12");
        //    dt.Columns.Add("Column13");
        //    dt.Columns.Add("Column14");
        //    dt.Columns.Add("Column15");
        //    dt.Columns.Add("Column16");
        //    dt.Columns.Add("Column17");
        //    dt.Columns.Add("Column18");
        //    dt.Columns.Add("Column19");
        //    dt.Columns.Add("Column20");

        //    dt.Rows.Add(10, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        //    dt.Rows.Add(11, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        //    dt.Rows.Add(12, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        //    dt.Rows.Add(13, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        //    dt.Rows.Add(14, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        //    dt.Rows.Add(15, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        //    dt.Rows.Add(16, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        //    dt.Rows.Add(17, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        //    dt.Rows.Add(18, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);

        //    dt.AcceptChanges();

        //    return dt;
        //}

        private void AddTabPage(string tabName, string tabValue, ParamDatas header, ParamDatas row, string paramSeq, UserParamTable minMaxData,int indexZ)
        {
            DataTable dt = GetDataTable(tabValue, header, row,  paramSeq,  minMaxData, indexZ);
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = tabValue;
            tabPage.Text = tabName;

            GridControl gridControl = new GridControl();
            gridControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(gridControl);

            BandedGridView bandedGridView = new BandedGridView();

            gridControl.ViewCollection.Add(bandedGridView);

            gridControl.MainView = bandedGridView;
            bandedGridView.GridControl = gridControl;
            bandedGridView.Name = "gridView1";

            GridBand gridBand1 = new GridBand();
            //GridBand gridBand2 = new GridBand();

            bandedGridView.Bands.Clear();
            bandedGridView.Bands.Add(gridBand1);
            //bandedGridView.Bands.Add(gridBand2);

            //bandedGridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            bandedGridView.OptionsView.ShowColumnHeaders = true;
            bandedGridView.OptionsView.ShowGroupPanel = false;
            bandedGridView.OptionsView.ShowIndicator = false;
            //bandedGridView.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //bandedGridView.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            bandedGridView.OptionsView.ColumnAutoWidth = false;

            bandedGridView.OptionsBehavior.ReadOnly = true;
            bandedGridView.OptionsBehavior.Editable = false;

            //bandedGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            //bandedGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            bandedGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
            
            bandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandedGridView.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            // 아래처럼 LookAndFeel 제거를 해야 원하는 컬러로 세팅이 가능하다.
            //gridControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            //gridControl.LookAndFeel.UseDefaultLookAndFeel = false;
            //bandedGridView.Appearance.HeaderPanel.Options.UseBackColor = true;
            //bandedGridView.Appearance.HeaderPanel.BackColor = Color.Gray;

            bandedGridView.RowCellStyle += BandedGridView_RowCellStyle;
            bandedGridView.RowCellClick += BandedGridView_RowCellClick;
            bandedGridView.RowCellStyle += BandedGridView_RowCellStyle1;

            //bandedGridView.ColumnPanelRowHeight = 40;
            //bandedGridView.IndicatorWidth = 100;

            gridControl.DataSource = dt;

            gridBand1.Caption = row.propInfo.paramUnit;
            gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gridBand1.AppearanceHeader.BackColor = Color.Gray;
            //gridBand1.Columns.Clear();

            BandedGridColumn colIndex = bandedGridView.Columns[header.propInfo.paramUnit];
            //BandedGridColumn colIndex = bandedGridView.Columns["Q"];
            colIndex.OptionsColumn.FixedWidth = true;
            colIndex.Width = 80;
            //gridBand1.Columns.Add(colIndex);
            //colIndex.AppearanceHeader.Options.UseBackColor = true;
            //colIndex.AppearanceHeader.BackColor = Color.Gray;
            //colIndex.AppearanceCell.BackColor = Color.Gray;
            //colIndex.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //colIndex.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            colIndex.OptionsColumn.AllowFocus = false;


            //gridBand2.Caption = "AOS";
            //gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gridBand2.Columns.Clear();
            //gridBand2.Columns.Add(bandedGridView.Columns["Column1"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column2"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column3"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column4"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column5"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column6"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column7"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column8"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column9"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column10"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column11"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column12"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column13"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column14"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column15"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column16"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column17"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column18"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column19"]);
            //gridBand2.Columns.Add(bandedGridView.Columns["Column20"]);


        }

        private void AddTabPage2D(string tabName, string tabValue, ParamDatas header, ParamDatas row)
        {
            DataTable dt = GetDataTable2D(tabValue, header, row);
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = tabValue;
            tabPage.Text = tabName;

            GridControl gridControl = new GridControl();
            gridControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(gridControl);

            BandedGridView bandedGridView = new BandedGridView();

            gridControl.ViewCollection.Add(bandedGridView);

            gridControl.MainView = bandedGridView;
            bandedGridView.GridControl = gridControl;
            bandedGridView.Name = "gridView1";

            GridBand gridBand1 = new GridBand();

            bandedGridView.Bands.Clear();
            bandedGridView.Bands.Add(gridBand1);

            bandedGridView.OptionsView.ShowColumnHeaders = true;
            bandedGridView.OptionsView.ShowGroupPanel = false;
            bandedGridView.OptionsView.ShowIndicator = false;
            bandedGridView.OptionsView.ColumnAutoWidth = false;

            bandedGridView.OptionsBehavior.ReadOnly = true;
            bandedGridView.OptionsBehavior.Editable = false;
            bandedGridView.OptionsSelection.EnableAppearanceFocusedRow = false;

            bandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandedGridView.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            bandedGridView.RowCellStyle += BandedGridView_RowCellStyle;
            bandedGridView.RowCellClick += BandedGridView_RowCellClick;
            bandedGridView.RowCellStyle += BandedGridView_RowCellStyle1;

            gridControl.DataSource = dt;

            gridBand1.Caption = row.propInfo.paramUnit;
            gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            BandedGridColumn colIndex = bandedGridView.Columns[header.propInfo.paramUnit];
            colIndex.OptionsColumn.FixedWidth = true;
            colIndex.Width = 80;
            colIndex.OptionsColumn.AllowFocus = false;


        }

        private void BandedGridView_RowCellStyle1(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            var nowTabPage = this.xtraTabControl1.SelectedTabPage;
            if (e.Column == view.Columns[firstColNameList[nowTabPage.Name]])
                return;

            //if (Convert.ToInt32(view.GetRowCellValue(e.RowHandle, view.Columns["Column11"])) > 10)
            //if (Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "Column11")) > 10)
            //if (Convert.ToInt32(view.GetRowCellValue(e.RowHandle, e.Column.FieldName)) > 10)
            //{
            //    e.Appearance.BackColor = Color.Red;
            //}

            double val = 0;
            double.TryParse(view.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString(), out val);
            var percent = 0.0;
            if (!Double.IsNaN(val / maxValue))
            {
                percent = val / maxValue;
            }
            e.Appearance.BackColor = Color.FromArgb(0, Convert.ToInt32(percent * 128), 0);
        
            //if (val < 5)
            //{
            //    Color.FromArgb(255,0, 0);
            //e.Appearance.BackColor = Color.Green;
            //}
            //else if (val < 8)
            //{
            //    e.Appearance.BackColor = Color.Yellow;
            //    e.Appearance.ForeColor = Color.Black;
            //}
            //else if (val < 12)
            //{
            //    e.Appearance.BackColor = Color.Orange;
            //}
            //else
            //{
            //    e.Appearance.BackColor = Color.Red;
            //}

        }

        private void BandedGridView_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            BandedGridView gridView = sender as BandedGridView;
            var nowTabPage = this.xtraTabControl1.SelectedTabPage;

            string selectParamSeq = String.Empty;
            if (cboParam.GetColumnValue("seq") != null)
                selectParamSeq = cboParam.GetColumnValue("seq").ToString();


            if (e.Column.FieldName == firstColNameList[nowTabPage.Name])
            {
                return;
            }
            mainForm.ShowSplashScreenManager("ShorBlock 정보를 가져오는 중입니다. 잠시만 기다려주십시오.");
            string rowKey = gridView.GetRowCellValue(e.RowHandle, firstColNameList[nowTabPage.Name]).ToString();

            string colKey = gridView.FocusedColumn.FieldName;

            List<string> selectValue = null;
            SummaryData selectSummaryData = null;
            var rowValue = rangeDic[nowTabPage.Name].Where(dic => dic.Key.range == rowKey).Select(x => x.Key).ToList();
            if (rowValue.Count != 0)
            {
                var headerValue = rangeDic[nowTabPage.Name][rowValue[0]].Where(dic => dic.Key.range == colKey).Select(x => x.Key).ToList();
                if (headerValue.Count != 0)
                {
                    selectValue = rangeDic[nowTabPage.Name][rowValue[0]][headerValue[0]].shortblcokSeqList;
                    if (selectValue.Count != 0)
                    {
                        var temp = rangeDic[nowTabPage.Name][rowValue[0]][headerValue[0]].jsonResult[selectParamSeq].ToString();
                        Summary summaryData = JsonConvert.DeserializeObject<Summary>(temp);
                        selectSummaryData = summaryData.bpf;
                    }
                }
            }

            // panel 추가
            if (binSBTabPanel == null)
            {
                binSBTabPanel = mainForm.DockManager1.AddPanel(DockingStyle.Float);
                binSBTabPanel.FloatLocation = new Point(500, 100);
                binSBTabPanel.FloatSize = new Size(466, 620);
                binSBTabPanel.Name = "ShortBlock Panel";
                binSBTabPanel.Text = "ShortBlock Panel";
                binSBTabCtrl = new BinSBTabControl(selectValue, selectSummaryData, selectParamSeq, binTableName, binMetaSeq);
                binSBTabCtrl.IdxValue = rowKey;
                binSBTabCtrl.Dock = DockStyle.Fill;
                binSBTabPanel.Controls.Add(binSBTabCtrl);
                binSBTabPanel.ClosedPanel += BinSBTabPanel_ClosedPanel;
            }
            else
            {
                binSBTabCtrl.IdxValue = rowKey;
                binSBTabPanel.Show();
            }
            mainForm.HideSplashScreenManager();
        }

        private void BinSBTabPanel_ClosedPanel(object sender, DockPanelEventArgs e)
        {
            this.binSBTabPanel = null;
            this.binSBTabCtrl = null;
        }

        private void BandedGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            BandedGridView gridView = sender as BandedGridView;
            //e.Appearance.BackColor = Color.Black;

            if (e.Column.FieldName == "Customer")
            {
                bool value = Convert.ToBoolean(gridView.GetRowCellValue(e.RowHandle, "Flag_Customer"));
                if (value)
                    e.Appearance.BackColor = Color.Red;
            }
            if (e.Column.FieldName == "Vendor")
            {
                bool value = Convert.ToBoolean(gridView.GetRowCellValue(e.RowHandle, "Flat_Vendor"));
                if (value)
                    e.Appearance.BackColor = Color.Red;
            }
        }

        private void cboParamAndType_EditValueChanged(object sender, EventArgs e)
        {
            ChangeTabPage();
        }

    }
}
