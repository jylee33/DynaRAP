using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DynaRAP.Data.ParamDataSelectionData;

namespace DynaRAP.UControl
{
    public partial class ParamDataSelectControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ParamDataSelectionData> paramDataList = null;
        private List<ParamDataSelectionData> selectParamDataList = null;
        private Dictionary<string, RepositoryItemComboBox> comboDic = null;
        private Dictionary<string, RepositoryItemComboBox> selectComboDic = null;
        private Dictionary<string, List<GridParam>> gridPramDic = new Dictionary<string, List<GridParam>>();
         
        public ParamDataSelectControl()
        {
            InitializeComponent();
        }

        private void ParamDataSelectControl_Load(object sender, EventArgs e)
        {
            //comboBoxEdit1.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            comboBoxEdit1.Properties.Items.Add("PART");
            comboBoxEdit1.Properties.Items.Add("SHORTBLOCK");
            comboBoxEdit1.Properties.Items.Add("DLL");
            comboBoxEdit1.Properties.Items.Add("PARAMMODULE");

            selectParamDataList = new List<ParamDataSelectionData>();
            selectComboDic = new Dictionary<string, RepositoryItemComboBox>();

            InitializeGridControl1();
            InitializeGridControl2();
        }
        private void InitializeGridControl1()
        {
            GridColumn colStart = gridView1.Columns["SelectionStart"];
            colStart.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colStart.OptionsColumn.FixedWidth = true;
            colStart.Width = 140;
            colStart.OptionsColumn.ReadOnly = true;

            GridColumn colEnd = gridView1.Columns["SelectionEnd"];
            colEnd.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colEnd.OptionsColumn.FixedWidth = true;
            colEnd.Width = 140;
            colEnd.OptionsColumn.ReadOnly = true;

            GridColumn colAdd = gridView1.Columns["Add"];
            colAdd.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colAdd.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colAdd.OptionsColumn.FixedWidth = true;
            colAdd.Width = 40;
            colAdd.Caption = "추가";
            colAdd.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;
            
            gridView1.CustomRowCellEditForEditing += (sender, e) =>
            {
                GridView view = sender as GridView;

                if (e.Column.FieldName == "ParamsCombo")
                {
                   
                    ParamDataSelectionData paramData = (ParamDataSelectionData)gridView1.GetFocusedRow();
                    //this.gridColumn3.ColumnEdit = comboDic[paramData.SourceSeq];
                    e.RepositoryItem = comboDic[paramData.SourceSeq];

                    //방법2
                    //RepositoryItemLookUpEdit riLookup1 = new RepositoryItemLookUpEdit();

                    //riLookup1.DisplayMember = "paramKey";

                    //riLookup1.ValueMember = "paramKey";

                    //riLookup1.DataSource = gridPramDic[paramData.SourceSeq];

                    //e.RepositoryItem = riLookup1;
                }

            };
        }
        private void InitializeGridControl2()
        {
            GridColumn colStart = gridView2.Columns["SelectionStart"];
            colStart.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colStart.OptionsColumn.FixedWidth = true;
            colStart.Width = 140;
            colStart.OptionsColumn.ReadOnly = true;

            GridColumn colEnd = gridView2.Columns["SelectionEnd"];
            colEnd.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colEnd.OptionsColumn.FixedWidth = true;
            colEnd.Width = 140;
            colEnd.OptionsColumn.ReadOnly = true;

            GridColumn colDel = gridView2.Columns["Del"];
            colDel.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.OptionsColumn.FixedWidth = true;
            colDel.Width = 40;
            colDel.Caption = "삭제";
            colDel.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox2.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox2.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox2.Click += RepositoryItemImageComboBox2_Click;

            GridColumn colView = gridView2.Columns["View"];
            colView.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colView.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            colView.OptionsColumn.FixedWidth = true;
            colView.Width = 40;
            colView.Caption = "보기";
            colView.OptionsColumn.ReadOnly = true;
            this.repositoryItemImageComboBox3.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox3.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox3.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox3.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox3.Click += RepositoryItemImageComboBox3_Click;

            gridView2.CustomRowCellEditForEditing += (sender, e) =>
            {

                GridView view = sender as GridView;

                if (e.Column.FieldName == "ParamsCombo")
                {
                    ParamDataSelectionData paramData = (ParamDataSelectionData)gridView2.GetFocusedRow();
                    e.RepositoryItem = selectComboDic[paramData.SourceSeq];
                }
            };
        }

        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            ParamDataSelectionData selectData = (ParamDataSelectionData)gridView1.GetFocusedRow();
            //selectParamDataList.Find(selectData)
            if (selectParamDataList.FindIndex(x => x.SourceSeq == selectData.SourceSeq) != -1)
            {
                MessageBox.Show("이미 추가된 데이터 입니다.");
            }
            else 
            {
                selectParamDataList.Add(selectData);

                //RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
                //foreach (var parameter in selectData.Parameter)
                //{
                //    repositoryItemComboBox.Items.Add(parameter.paramKey);
                //}
                //selectComboDic.Add(selectData.SourceSeq, repositoryItemComboBox);
                //this.gridControl2.DataSource = selectParamDataList;
                gridView2.RefreshData();
            }//this.gridView
           
            //     (ListParamModule)gridView1.GetFocusedRow()
            ////gridView2.DeleteRow(gridView2.FocusedRowHandle);
            ////lblSplitCount.Text = string.Format(Properties.Resources.StringSplitCount, intervalList.Count);
            //MessageBox.Show("test");
        }

        private void RepositoryItemImageComboBox2_Click(object sender, EventArgs e)
        {
            selectParamDataList.Remove((ParamDataSelectionData)gridView2.GetFocusedRow());
            this.gridControl2.DataSource = selectParamDataList;
            //gridView2.DeleteRow(gridView2.FocusedRowHandle);
            gridView2.RefreshData();
        }
        private void RepositoryItemImageComboBox3_Click(object sender, EventArgs e)
        {

        }

        //RepositoryItemComboBox UserCombo = new RepositoryItemComboBox();
        //UserCombo.Items.Add("1");
        //    UserCombo.Items.Add("2");
            
        //    UserCombo.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
        //    gridView1.Columns["Id"].ColumnEdit = UserCombo;
        private void edtSearch_Click(object sender, ButtonPressedEventArgs e)
        {
            if (comboBoxEdit1.Text.Equals("") || edtSearch.Text.Equals(""))
            {
                MessageBox.Show("구분 값이나 검색어가 없습니다.");
                return;
            }
            if (e.Button.Kind.ToString() == "Search")
            {
                string sendData = string.Format(@"
                {{
                ""command"":""search"",
                ""sourceType"": ""{0}"",
                ""keyword"": ""{1}""
                }}" , comboBoxEdit1.Text.ToLower(), edtSearch.Text);
                string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
                if (responseData != null)
                {
                    SearchParamModuleResponse paramModuleResponse = JsonConvert.DeserializeObject<SearchParamModuleResponse>(responseData);
                    comboDic = new Dictionary<string, RepositoryItemComboBox>();
                    paramDataList = new List<ParamDataSelectionData>();
                    switch (comboBoxEdit1.Text)
                    {
                        case "PART": 
                            foreach(var list in paramModuleResponse.response)
                            {
                                List<GridParam> paramList = new List<GridParam>();
                                DateTime? startTime = null;
                                DateTime? endTime = null;
                                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
                                repositoryItemComboBox.SelectedIndexChanged += RepositoryItemComboBox_SelectedIndexChanged;
                                repositoryItemComboBox.ReadOnly = false;
                                repositoryItemComboBox.ReadOnly = false;
                                repositoryItemComboBox.AllowDropDownWhenReadOnly = DefaultBoolean.True;
                                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
                                foreach (var parameter in list.@params)
                                {
                                    GridParam param = new GridParam();
                                    param.seq = parameter.seq;
                                    param.paramKey = parameter.paramKey;
                                    paramList.Add(param);
                                    repositoryItemComboBox.Items.Add(param.paramKey);
                                }
                                gridPramDic.Add(list.seq, paramList);
                                gridControl1.RepositoryItems.Add(repositoryItemComboBox);
                                comboDic.Add(list.seq, repositoryItemComboBox);
                                if(list.julianStartAt != "")
                                {
                                    startTime = Utils.GetDateFromJulian(list.julianStartAt);
                                }
                                if(list.julianEndAt != "")
                                {
                                    endTime = Utils.GetDateFromJulian(list.julianEndAt);
                                }
                                paramDataList.Add(new ParamDataSelectionData("part", Utils.base64StringDecoding(list.partName), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", startTime), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", endTime) , "",list.seq,1, paramList));
                            }
                            break;
                        case "SHORTBLOCK":
                            foreach (var list in paramModuleResponse.response)
                            {
                                List<GridParam> paramList = new List<GridParam>();
                                DateTime? startTime = null;
                                DateTime? endTime = null;
                                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
                                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
                                foreach (var parameter in list.@params)
                                {
                                    GridParam param = new GridParam();
                                    param.seq = parameter.seq;
                                    param.paramKey = parameter.paramKey;
                                    paramList.Add(param);
                                    repositoryItemComboBox1.Items.Add(param.paramKey);
                                }
                                comboDic.Add(list.seq, repositoryItemComboBox);

                                if (list.julianStartAt != "")
                                {
                                    startTime = Utils.GetDateFromJulian(list.julianStartAt);
                                }
                                if (list.julianEndAt != "")
                                {
                                    endTime = Utils.GetDateFromJulian(list.julianEndAt);
                                }
                                paramDataList.Add(new ParamDataSelectionData("shortblock", Utils.base64StringDecoding(list.blockName), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", startTime), string.Format("{0:yyyy-MM-dd hh:mm:ss.ffffff}", endTime), "", list.seq,1, paramList));
                            }
                            break;
                        case "DLL":
                            foreach (var list in paramModuleResponse.response)
                            {
                                paramDataList.Add(new ParamDataSelectionData("shortblock", Utils.base64StringDecoding(list.dataSetName),"","", "", list.seq,0));
                            }
                            break;
                        case "PARAMMODULE":
                            foreach (var list in paramModuleResponse.response)
                            {
                                paramDataList.Add(new ParamDataSelectionData("parammodule", Utils.base64StringDecoding(list.moduleName),"","", "", list.seq,0));
                            }
                            break;
                    }
                    this.gridControl1.DataSource = paramDataList;
                }
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            if (paramDataList != null)
            {
                foreach (var param in paramDataList)
                {
                    selectParamDataList.Add(param);
                }
            }
            this.gridControl2.DataSource = selectParamDataList;
            gridView2.RefreshData();
        }

        private void btnListSave_Click(object sender, EventArgs e)
        {
            //string sendData = string.Format(@"
            //    {{
            //    ""command"":""save-source"",
            //    ""moduleSeq"": ""{0}"",
            //    ""keyword"": ""{1}""
            //    }}", comboBoxEdit1.Text.ToLower(), edtSearch.Text);

            //string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            //if (responseData != null)
            //{
            //    SearchParamModuleResponse paramModuleResponse = JsonConvert.DeserializeObject<SearchParamModuleResponse>(responseData);
            //    byte[] byte64 = Convert.FromBase64String(paramModuleResponse.response[0].partName);
            //    string decName = Encoding.UTF8.GetString(byte64);
            //    MessageBox.Show(decName);
            //}

        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {

        }

        private void RepositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ParamDataSelectionData temp = (ParamDataSelectionData)gridView2.GetFocusedRow();
            //comboDic[temp.SourceSeq] = comboDic[temp.SourceSeq].
            //gridView1.Columns["Parameter"].ColumnEdit = "tesst";
            //Columns.ColumnByFieldName(sColumnNameReplace).ColumnEdit = Temp
            var combo = sender as ComboBoxEdit;
            if (combo.SelectedIndex != -1)
            {
                string paramKey = combo.SelectedItem as string;
                var test = comboDic;
                //gridView1.Columns.
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DataCnt", paramKey);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ParamsCombo", paramKey);
                //gridView1.RefreshData();
            }
            //gridView1.Columns["Parameter"].ColumnEdit = combo;
        }
    }
}
