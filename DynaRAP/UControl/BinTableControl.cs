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
        List<ParamDatas> paramDataList = null;
        List<PickUpParam> pickUpParamList = null;
        List<string> shortBlockSeqList = null;
        Dictionary<string,string> firstColNameList= null;
        List<ResponseParamList> responseParamList = null;
        MainForm mainForm = null;
        Dictionary<string, Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, List<string>>>> rangeDic = new Dictionary<string, Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, List<string>>>>();
        public BinTableControl(List<ParamDatas> paramDataList, List<PickUpParam> pickUpParamList, List<string> shortBlockSeqList, MainForm mainForm)
        {
            this.paramDataList = paramDataList;
            this.pickUpParamList = pickUpParamList;
            this.shortBlockSeqList = shortBlockSeqList;
            this.mainForm = mainForm;
            InitializeComponent();
        }

        private void BinTableControl_Load(object sender, EventArgs e)
        {
            firstColNameList = new Dictionary<string, string>();
            int j = 1;
            for(int i=j; i< paramDataList.Count();i++)
            {
               string showName = string.Format("{0}-{1}", paramDataList[i - 1].propInfo.paramUnit, paramDataList[i].propInfo.paramUnit);
               string valueName = paramDataList[i - 1].paramKey + paramDataList[i].paramKey;
                //DataTable dt = GetDataTable(paramDataList[i - 1], paramDataList[i]);
               GetShortBlockParamList();
               AddTabPage(showName, valueName, paramDataList[i - 1], paramDataList[i]);

               if (i == paramDataList.Count() - 1)
               {
                    j++; 
               }
            }
            //DataTable dt = GetDataTable(paramDataList[0], paramDataList[1]);
            //DataTable dt1 = GetDataTable();
            //AddTabPage("AOA-Q", dt);
            //AddTabPage("AOA-AOS", dt1);
            //AddTabPage("Q-AOS", dt);

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
                    ResponseParamList responseParam = JsonConvert.DeserializeObject<ResponseParamList>(responseData);
                    if (responseParam.code == 200)
                    {
                        responseParamList.Add(responseParam);
                    }
                }
            }
        }

        private DataTable GetDataTable(string keyName, ParamDatas header, ParamDatas row)
        {
          Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, List<string>>> countSeqDic = new Dictionary<MinMaxRagne, Dictionary<MinMaxRagne, List<string>>>();

            DataTable dt = new DataTable();
            foreach (var list in pickUpParamList.Find(x => x.paramSeq == row.seq).userParamTable)
            {
                Dictionary<MinMaxRagne, List<string>> headerDic = new Dictionary<MinMaxRagne, List<string>>();
                foreach (var list1 in pickUpParamList.Find(x => x.paramSeq == header.seq).userParamTable)
                {
                    headerDic.Add(new MinMaxRagne(list1.min,list1.max), new List<string>());
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
                            countSeqDic[rowValue[0]][headerValue[0]].Add(responseParam.response.paramData[0].paramValueMap.blockSeq);
                        }
                    }
                }
            }
            rangeDic.Add(keyName, countSeqDic);
            //dt.Columns.Add(header.propInfo.paramUnit);
            foreach(var list in countSeqDic.Keys)
            {
                DataRow dataRow = dt.NewRow();
                dataRow[header.propInfo.paramUnit] = list.range;
                foreach (var dic2 in countSeqDic[list].Keys)
                {
                    dataRow[dic2.range] = countSeqDic[list][dic2].Count();
                }
                //dt.Rows.Add(, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
                //countSeqDic
                //dt.Columns.Add(list.range);
                dt.Rows.Add(dataRow);
            }
            
            //dt.Columns.Add("Column1");
            //dt.Columns.Add("Column2");
            //dt.Columns.Add("Column3");
            //dt.Columns.Add("Column4");
            //dt.Columns.Add("Column5");
            //dt.Columns.Add("Column6");
            //dt.Columns.Add("Column7");
            //dt.Columns.Add("Column8");
            //dt.Columns.Add("Column9");
            //dt.Columns.Add("Column10");
            //dt.Columns.Add("Column11");
            //dt.Columns.Add("Column12");
            //dt.Columns.Add("Column13");
            //dt.Columns.Add("Column14");
            //dt.Columns.Add("Column15");
            //dt.Columns.Add("Column16");
            //dt.Columns.Add("Column17");
            //dt.Columns.Add("Column18");
            //dt.Columns.Add("Column19");
            //dt.Columns.Add("Column20");

            //dt.Rows.Add(10, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            //dt.Rows.Add(11, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            //dt.Rows.Add(12, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            //dt.Rows.Add(13, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            //dt.Rows.Add(14, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            //dt.Rows.Add(15, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            //dt.Rows.Add(16, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            //dt.Rows.Add(17, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
            //dt.Rows.Add(18, 0, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);

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

        private void AddTabPage(string tabName, string tabValue, ParamDatas header, ParamDatas row)
        {
            DataTable dt = GetDataTable(tabValue, header, row);
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

            int val = 0;
            int.TryParse(view.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString(), out val);
            if (val < 5)
            {
                e.Appearance.BackColor = Color.Green;
            }
            else if (val < 8)
            {
                e.Appearance.BackColor = Color.Yellow;
                e.Appearance.ForeColor = Color.Black;
            }
            else if (val < 12)
            {
                e.Appearance.BackColor = Color.Orange;
            }
            else
            {
                e.Appearance.BackColor = Color.Red;
            }

        }

        private void BandedGridView_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            BandedGridView gridView = sender as BandedGridView;
            var nowTabPage = this.xtraTabControl1.SelectedTabPage;

            if (e.Column.FieldName == firstColNameList[nowTabPage.Name])
            {
                return;
            }
            string rowKey = gridView.GetRowCellValue(e.RowHandle, firstColNameList[nowTabPage.Name]).ToString();

            string colKey = gridView.FocusedColumn.FieldName;

            List<string> selectValue = null;
            var rowValue = rangeDic[nowTabPage.Name].Where(dic => dic.Key.range == rowKey).Select(x => x.Key).ToList();
            if (rowValue.Count != 0)
            {
                var headerValue = rangeDic[nowTabPage.Name][rowValue[0]].Where(dic => dic.Key.range == colKey).Select(x => x.Key).ToList();
                if (headerValue.Count != 0)
                {
                    selectValue = rangeDic[nowTabPage.Name][rowValue[0]][headerValue[0]];
                }
            }


            //MainForm mainForm = this.ParentForm as MainForm;

            // panel 추가
            if (binSBTabPanel == null)
            {
                binSBTabPanel = mainForm.DockManager1.AddPanel(DockingStyle.Float);
                binSBTabPanel.FloatLocation = new Point(500, 100);
                binSBTabPanel.FloatSize = new Size(466, 620);
                binSBTabPanel.Name = "ShortBlock Panel";
                binSBTabPanel.Text = "ShortBlock Panel";
                binSBTabCtrl = new BinSBTabControl(selectValue);
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
    }
}
