using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DynaRAP.Data;
using DynaRAP.UTIL;
using log4net.Config;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace DynaRAP.UControl
{
    public partial class BinModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        BinTableData binTableResponse = null;
        List<ResponsePreset> presetList = null;
        List<PresetParamData> gridList = null;
        List<BinGridData> binGridData = null;
        List<ParamDatas> selectedParamDataList = new List<ParamDatas>();
        List<ParamDatas> paramListData = null;
        string binMetaSeq = null;

        public BinModuleControl()
        {
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void BinModuleControl_Load(object sender, EventArgs e)
        {

            //DateTime dtNow = DateTime.Now;
            //string strNow = string.Format("{0:yyyy-MM-dd}", dtNow);
            //dateScenario.Text = strNow;

            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.VerticalScroll.Visible = true;

            InitializeTreeDataList();

            GetBinTableList();

            panelParamCnt.Controls.Add(new BinParameterSelectControl(this, new List<ParamDatas>()));
            panelParamCnt.Controls.Add(new BinParameterSelectControl(this, new List<ParamDatas>()));
            //panelParamCnt1.Controls.Add(new BinParameterSelectControl(this, new List<ParamDatas>()));
        }

        private void InitializeTreeDataList()
        {
            //treeList1.Parent = this;
            //treeList1.Dock = DockStyle.Fill;
            treeList1.KeyFieldName = "ID";
            treeList1.ParentFieldName = "ParentID";
            treeList1.OptionsBehavior.PopulateServiceColumns = true;

            treeList1.DataSource = GetTreeList();
            treeList1.ForceInitialize();

            treeList1.RowHeight = 23;
            treeList1.OptionsView.ShowColumns = false;
            treeList1.OptionsView.ShowHorzLines = false;
            treeList1.OptionsView.ShowVertLines = false;
            treeList1.OptionsView.ShowIndicator = false;
            treeList1.OptionsView.ShowTreeLines = DevExpress.Utils.DefaultBoolean.False;
            treeList1.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            treeList1.OptionsView.ShowSummaryFooter = false;
            treeList1.OptionsView.AutoWidth = false;

            treeList1.OptionsFilter.AllowFilterEditor = false;

            //Access the automatically created columns.
            TreeListColumn colName = treeList1.Columns["TreeName"];
            TreeListColumn colCheck = treeList1.Columns["Check"];

            //Hide the key columns. An end-user can access them from the Customization Form.
            treeList1.Columns[treeList1.KeyFieldName].Visible = false;
            treeList1.Columns[treeList1.ParentFieldName].Visible = false;

            //Make the Project column read-only.
            colName.OptionsColumn.ReadOnly = true;
            colCheck.OptionsColumn.ReadOnly = false;

            colName.OptionsColumn.AllowEdit = false;

            //Sort data against the Project column
            colName.SortIndex = -1;// 0;

            repositoryItemCheckEdit1.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.CheckBox;
            repositoryItemCheckEdit1.EditValueChanged += repositoryItemCheckEdit1_EditValueChanged;
            //treeList1.OptionsView.ShowCheckBoxes = true; // 제일 앞에 checkBox 붙이는 옵션
            treeList1.CellValueChanged += treeList1_CellValueChanged;

            //treeList1.ExpandAll();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate
            {
                treeList1.BestFitColumns();
            }));



        }

        private void treeList1_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            foreach (TreeListNode node in e.Node.Nodes)
                node[e.Column] = e.Value;
            TreeListNode parent = e.Node.ParentNode;
            if (parent != null) // not a root node
            {
                bool checkedValue = false;
                if (e.Value != null)
                    checkedValue = (bool)e.Value;
                foreach (TreeListNode node in parent.Nodes)
                {
                    if ((bool)node[e.Column] != checkedValue)
                    {
                        parent[e.Column] = null;
                        break;
                    }
                    else
                        parent[e.Column] = checkedValue;
                }
            }
            List<ParamDatas> paramDatas = GetParamListByPartsShortblock();
            if (paramListData != null)
            {
                List<string> seqList1 = paramListData.Select(item => item.seq).ToList();
                List<string> seqList2 = paramDatas.Select(item => item.seq).ToList();


                //bool test = (Enumerable.SequenceEqual(seqList1, seqList2));
                if (!Enumerable.SequenceEqual(seqList1, seqList2))
                {
                    if (panelParamCnt.Controls.Count != 0)
                    {
                        MessageBox.Show("선택된 ShortBlock의 변경으로 \nparameter의 변경이 있어 선택된 parameter를 초기화 합니다.");
                    }
                    panelParamCnt.Controls.Clear();
                    panelParamCnt1.Controls.Clear();
                    selectedParamDataList.Clear();
                    paramListData = paramDatas;
                    panelParamCnt.Controls.Add(new BinParameterSelectControl(this, paramListData));
                    panelParamCnt.Controls.Add(new BinParameterSelectControl(this, paramListData));
                    if (chkUse3D.Checked)
                    {
                        panelParamCnt1.Controls.Add(new BinParameterSelectControl(this, paramListData));
                    }
                }
            }
            else
            {
                panelParamCnt.Controls.Clear();
                panelParamCnt1.Controls.Clear();
                selectedParamDataList.Clear();
                paramListData = paramDatas;
                panelParamCnt.Controls.Add(new BinParameterSelectControl(this, paramListData));
                panelParamCnt.Controls.Add(new BinParameterSelectControl(this, paramListData));
                if (chkUse3D.Checked)
                {
                    panelParamCnt1.Controls.Add(new BinParameterSelectControl(this, paramListData));
                }
            }
            //else
            //{
            //    panelParamCnt.Controls.Add(new BinParameterSelectControl(this, new List<ParamDatas>()));
            //    panelParamCnt.Controls.Add(new BinParameterSelectControl(this, new List<ParamDatas>()));
            //    panelParamCnt1.Controls.Add(new BinParameterSelectControl(this, new List<ParamDatas>()));
            //}
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            treeList1.PostEditor();
        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = e.RowHandle.ToString();
        }

        private void hyperlinkBrowseSB_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;

            mainForm.PanelBinSbList.Show();

        }

        private void btnCreateBIN_Click(object sender, EventArgs e)
        {
           
            List<PickUpParam> pickUpParamList = new List<PickUpParam>();
            foreach (BinParameterSelectControl control in panelParamCnt.Controls)
            {
                PickUpParam pickUpParam = control.SelectedParamLIst();
                if (pickUpParam != null)
                {
                    pickUpParamList.Add(pickUpParam);
                }
            }
            foreach (BinParameterSelectControl control in panelParamCnt1.Controls)
            {
                PickUpParam pickUpParam = control.SelectedParamLIst();
                if (pickUpParam != null)
                {
                    pickUpParamList.Add(pickUpParam);
                }
            }

            if (pickUpParamList.Count < 2)
            {
                MessageBox.Show("파라미터가 2개 이상 선택되어야 BIN테이블 생성이 가능합니다.");
                return;
            }
            BindingList<TreeData> treeDatas = treeList1.DataSource as BindingList<TreeData>;
            var treeList = treeDatas.ToList();
            List<TreeData> shortBlockList = treeList.FindAll(x => x.Check != false && x.Type == "shortblock");
            List<string> shorBlockSeqList = shortBlockList.Select(x => x.Seq).ToList();
            MainForm mainForm = this.ParentForm as MainForm;

            mainForm.PanelBinTable.Text = "BIN TABLE";

            //로직 확인 후 주석해제
            //string sendData = string.Format(@"
            //    {{
            //    ""command"":""clear-summary"",
            //    ""binMetaSeq"":""{0}""
            //    }}", binMetaSeq);
            //string responseData = Utils.GetPostData(System.Configuration.ConfigurationManager.AppSettings["UrlBINTable"], sendData);
            //if (responseData != null)
            //{
            //    JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
            //    if (result.code == 200)
            //    {

            //    }
            //}


            BinTableControl binTableCtrl = new BinTableControl(selectedParamDataList, pickUpParamList, shorBlockSeqList, mainForm, binMetaSeq);
            binTableCtrl.Dock = DockStyle.Fill;
            //mainForm.PanelBinTable.Controls.Clear();
            //mainForm.PanelBinTable.Controls.Add(binTableCtrl);
            //mainForm.PanelBinTable.Show();

            //mainForm.PanelBinTable.Hide();
            DockPanel panelChart = new DockPanel();
            panelChart = mainForm.DockManager1.AddPanel(DockingStyle.Float);
            panelChart.FloatLocation = new Point(400, 100);
            panelChart.FloatSize = new Size(1058, 528);
            panelChart.Name = "BIN TABLE";
            panelChart.Text = "BIN TABLE";
            //chartControl.Dock = DockStyle.Fill;
            panelChart.Controls.Add(binTableCtrl);
            //binTableCtrl.Show();
         
        }



        private void GetBinTableList()
        {
            string sendData = string.Format(@"
                {{
                ""command"":""list""
                }}");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlBINTable"], sendData);
            if (responseData != null)
            {
                binMetaSeq = null;
                panelParamCnt.Controls.Clear();
                panelParamCnt1.Controls.Clear();
                selectedParamDataList.Clear();
                binGridData = new List<BinGridData>();
                binTableResponse = new BinTableData();

                binTableResponse = JsonConvert.DeserializeObject<BinTableData>(responseData);
                if (binTableResponse.response != null && binTableResponse.response.Count() != 0)
                {
                    foreach (var list in binTableResponse.response)
                    {
                        list.metaName = Utils.base64StringDecoding(list.metaName);
                        BinGridData binData = new BinGridData();
                        binData.seq = list.seq;
                        binData.metaName = list.metaName;
                        binData.tags = list.dataProps.tags;
                        binGridData.Add(binData);
                    }
                }
                this.gridControl2.DataSource = binGridData;
                gridView2.RefreshData();
            }
        }

        private BindingList<TreeData> GetTreeList()
        {
            BindingList<TreeData> list = new BindingList<TreeData>();
            //list.Add(new FlyingData(1, 0, "Short Block", null));

            try
            {

                string sendData = string.Format(@"
                {{
                ""command"":""list"",
                ""registerUid"":"""",
                ""uploadSeq"":"""",
                ""pageNo"":1,
                ""pageSize"":3000
                }}");

                string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlPart"], sendData);
                if (responseData != null)
                {
                    PartListResponse partListResponse = JsonConvert.DeserializeObject<PartListResponse>(responseData);
                    if (partListResponse.response != null && partListResponse.response.Count() != 0)
                    {
                        int i = 1;
                        foreach (var partList in partListResponse.response)
                        {
                            byte[] byte64 = Convert.FromBase64String(partList.partName);
                            string decName = Encoding.UTF8.GetString(byte64);
                            TreeData data = new TreeData(i++,0,decName,partList.seq,"part",false);
                            //data.ParentID = 0;
                            //data.ID = i++;
                            //data.Seq = partList.seq;
                            //data.Type = 
                            ////Decoding

                            //data.TreeName = decName;
                            //data.Check = false;
                            list.Add(data);
                            sendData = string.Format(@"
                            {{
                            ""command"":""list"",
                            ""registerUid"":"""",
                            ""partSeq"":""{0}"",
                            ""pageNo"":1,
                            ""pageSize"":3000
                            }}", partList.seq );

                           string shortBlockData =  Utils.GetPostData(ConfigurationManager.AppSettings["UrlShortBlock"], sendData);
                            if (shortBlockData != null)
                            {
                                SBListResponse sbListReponse = JsonConvert.DeserializeObject<SBListResponse>(shortBlockData);
                                if (partListResponse.response != null && partListResponse.response.Count() != 0)
                                {
                                    foreach(var sb in sbListReponse.response)
                                    {
                                        TreeData sbData = new TreeData();
                                        sbData.ParentID = data.ID;
                                        sbData.ID = i++;
                                        sbData.Seq = sb.seq;
                                        sbData.partSeq = sb.partSeq;
                                        sbData.blockMetaSeq = sb.blockMetaSeq;
                                        sbData.Type = "shortblock";
                                        byte[] byteData = Convert.FromBase64String(sb.blockName);
                                        string decoName = Encoding.UTF8.GetString(byteData);

                                        sbData.TreeName = decoName;
                                        sbData.Check = false;
                                        list.Add(sbData);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

            return list;
        }

        private void btnListSave_Click(object sender, EventArgs e)
        {
            SaveBinTable("save");
        }

        private void SaveBinTable(string type)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            BindingList<TreeData> treeDatas = treeList1.DataSource as BindingList<TreeData>;
            var treeList = treeDatas.ToList();
            var shortBlockList = treeList.FindAll(x => x.Check != false && x.Type == "shortblock");
            var partList = treeList.FindAll(x => x.Check != false && x.Type == "part");
            BinGridData binGridData = (BinGridData)gridView2.GetFocusedRow();
            BinTableSaveRequest binSaveRequest = new BinTableSaveRequest();
            binSaveRequest.parts = new List<string>();
            binSaveRequest.selectedShortBlocks = new List<string>();
            binSaveRequest.dataProps = new DataProps();
            binSaveRequest.pickUpParams = new List<PickUpParam>();
            if (type == "update")
            {
                binSaveRequest.dataProps.tags = binGridData.tags;
                binSaveRequest.binMetaSeq = binGridData.seq ;
            }
            else
            {
                string tags = "";
                foreach(ButtonEdit btn in panelTag.Controls)
                {
                   tags += (btn.Text + "|");
                  
                }
                if (tags != "")
                {
                    tags = tags.Substring(0, tags.Length - 1);
                }
                binSaveRequest.dataProps.tags = tags;
            }

            binSaveRequest.dataProps.key = "value";
            binSaveRequest.dataProps.key2 = "value2";

            binSaveRequest.command = "save";
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(binName.Text);
            string encName = Convert.ToBase64String(basebyte);
            binSaveRequest.metaName = encName;
            foreach (var list in partList)
            {
                binSaveRequest.parts.Add(list.Seq);
            }
            foreach (var list in shortBlockList)
            {
                binSaveRequest.selectedShortBlocks.Add(list.Seq);
            }
            foreach(BinParameterSelectControl control in panelParamCnt.Controls)
            {
                PickUpParam pickUpParam = control.SelectedParamLIst();
                if(pickUpParam != null)
                {
                    binSaveRequest.pickUpParams.Add(pickUpParam);
                }
            }
            foreach (BinParameterSelectControl control in panelParamCnt1.Controls)
            {
                PickUpParam pickUpParam = control.SelectedParamLIst();
                if (pickUpParam != null)
                {
                    binSaveRequest.pickUpParams.Add(pickUpParam);
                }
            }
            var json = JsonConvert.SerializeObject(binSaveRequest);
            mainForm.ShowSplashScreenManager("BIN테이블을 저장 중입니다.. 잠시만 기다려주십시오.");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlBINTable"], json);
            mainForm.HideSplashScreenManager();
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    MessageBox.Show(type == "save" ? "저장 성공" : "수정 성공");
                    AllShortBlockUnChecked();
                    GetBinTableList();
                }
                else
                {
                    MessageBox.Show(type == "save" ? "저장 실패" : "수정 실패");
                }
            }
        }

        private void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("BIN테이블 정보를 불러오는 중입니다.. 잠시만 기다려주십시오.");
            BinGridData binGridData = (BinGridData)gridView2.GetFocusedRow();
            BindingList<TreeData> treeDatas = treeList1.DataSource as BindingList<TreeData>;
            binName.Text = binGridData.metaName;
            binMetaSeq = binGridData.seq;
            var selectData = binTableResponse.response.Find(x => x.seq == binGridData.seq);
            foreach (var treeData in treeDatas)
            {
                treeData.Check = false;
            }
            foreach (var list in selectData.selectedShortBlocks)
            {
                foreach(var treeData in treeDatas)
                {
                    if(treeData.Seq == list.refSeq && treeData.Type == "shortblock")
                    {
                        treeData.Check = true;
                    }
                }
            }

            foreach (var list in selectData.parts)
            {
                foreach (var treeData in treeDatas)
                {
                    if (treeData.Seq == list.refSeq && treeData.Type == "part")
                    {
                        treeData.Check = true;
                    }
                }
            }
            treeList1.DataSource = treeDatas;
            treeList1.Refresh();

            if (binGridData.tags != null && binGridData.tags != "")
            {
                string[] tagList = binGridData.tags.Split('|');

                panelTag.Controls.Clear();
                foreach (string value in tagList)
                {
                    addTag(value);
                }
            }
            else
            {
                panelTag.Controls.Clear();
            }

            selectedParamDataList.Clear();
            panelParamCnt.Controls.Clear();
            panelParamCnt1.Controls.Clear();
            paramListData = GetParamListByPartsShortblock();

            if (selectData.pickUpParams.Count != 0)
            {
                chkUse3D.Checked = false;
                foreach (var list in selectData.pickUpParams)
                {
                    BinParameterSelectControl ct = new BinParameterSelectControl(this, paramListData, list);
                    if (panelParamCnt.Controls.Count > 1)
                    {
                        chkUse3D.Checked = true;
                        panelParamCnt1.Controls.Clear();
                        panelParamCnt1.Controls.Add(ct);
                    }
                    else
                    {
                        panelParamCnt.Controls.Add(ct);
                    }
                }
            }
            else
            {
                panelParamCnt.Controls.Add(new BinParameterSelectControl(this, paramListData));
                panelParamCnt.Controls.Add(new BinParameterSelectControl(this, paramListData));
                if (chkUse3D.Checked)
                {
                    panelParamCnt1.Controls.Add(new BinParameterSelectControl(this, paramListData));
                }
            }
            mainForm.HideSplashScreenManager();
        }

        private void btnListModify_Click(object sender, EventArgs e)
        {
            SaveBinTable("update");
        }

        private void btnBinDelete_Click(object sender, EventArgs e)
        {
            if(binMetaSeq == null)
            {
                MessageBox.Show("선택된 BIN테이블이 없습니다. 선택 후 삭제해주세요.");
                return;
            }
            if(MessageBox.Show(binName.Text +  "을(를) 삭제하시겠습니까?", "삭제",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sendData = string.Format(@"
                {{
                ""command"":""remove"",
                ""binMetaSeq"":""{0}""
                }}", binMetaSeq);
                string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlBINTable"], sendData);
                if (responseData != null)
                {
                    JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                    if (result.code == 200)
                    {
                        MessageBox.Show("삭제 성공");
                        binMetaSeq = null;
                        binName.Text = "";
                        AllShortBlockUnChecked();
                        GetBinTableList();
                    }
                    else
                    {
                        MessageBox.Show("삭제 실패");
                    }
                }
            }
        }

        private void AllShortBlockUnChecked()
        {
            BindingList<TreeData> treeDataList = treeList1.DataSource as BindingList<TreeData>;
            foreach(var treeData in treeDataList)
            {
                treeData.Check = false;
            }
        }
        private void edtTag_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                BinGridData binGridData = (BinGridData)gridView2.GetFocusedRow();
                if (binGridData.tags != null && binGridData.tags != "")
                {
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "tags", binGridData.tags + "|" + me.Text);
                }
                else
                {
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "tags", me.Text);

                }
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }
        private void edtTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                BinGridData binGridData = (BinGridData)gridView2.GetFocusedRow();
                if (binGridData.tags != null && binGridData.tags != "")
                {
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "tags", binGridData.tags + "|" + me.Text);
                }
                else
                {
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "tags", me.Text);

                }
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }

        private void addTag(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            ButtonEdit btn = new ButtonEdit();
            btn.Properties.Buttons[0].Kind = ButtonPredefines.Close;
            btn.BorderStyle = BorderStyles.Simple;
            btn.ForeColor = Color.White;
            btn.Properties.Appearance.BorderColor = Color.White;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
            btn.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            //btn.ReadOnly = true;
            btn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btn.Properties.AllowFocused = false;
            btn.ButtonClick += removeTag_ButtonClick;
            btn.Text = name;
            panelTag.Controls.Add(btn);
        }
        public void removeControl(BinParameterSelectControl parameterSelectControl, string seq = null)
        {
            if(seq != null)
            {
                RemoveSelectedParams(seq);
            }
            panelParamCnt.Controls.Remove(parameterSelectControl);
        }
        private void removeTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit btn = sender as ButtonEdit;

            BinGridData binGridData = (BinGridData)gridView2.GetFocusedRow();

            List<string> tagList = new List<string>();
            if (binGridData.tags != null && binGridData.tags != "")
            {
                tagList = binGridData.tags.Split('|').ToList();
            }
            tagList.Remove(btn.Text);
            string tags = "";
            foreach (var tag in tagList)
            {
                tags += (tag + "|");
            }
            if (tags != "")
            {
                tags = tags.Substring(0, tags.Length - 1);
            }
            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "tags", tags);
            panelTag.Controls.Remove(btn);

        }

        private void btnParameterAdd_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            paramListData = GetParamListByPartsShortblock();
            if(paramListData.Count == 0)
            {
                MessageBox.Show("선택된 shortblock에 공통된 파라미터가 없습니다.");
                return;
            }
            BinParameterSelectControl ct = new BinParameterSelectControl(this, paramListData);
            panelParamCnt.Controls.Add(ct);
        }

        private List<ParamDatas> GetParamListByPartsShortblock()
        {
            //List<ParamDatas> responseParamList = new List<ParamDatas>();
            BindingList<TreeData> treeDatas = treeList1.DataSource as BindingList<TreeData>;

            var treeList = treeDatas.ToList();
            var shortBlockList = treeList.FindAll(x => x.Check != false && x.Type == "shortblock");
            var partList = treeList.FindAll(x => x.Check != false && x.Type == "part");
            List<string> blockMetaSeq = new List<string>();
            //foreach (var part in partList)
                //{
                //    string sendData = string.Format(@"
                //    {{
                //    ""command"":""param-list"",
                //    ""partSeq"":""{0}""
                //    }}", part.Seq);
                //    string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlPart"], sendData);
                //    if (responseData != null)
                //    {
                //        ResponseParamList responseParam = JsonConvert.DeserializeObject<ResponseParamList>(responseData);
                //        if (responseParam.code == 200)
                //        {
                //            foreach (var paramData in responseParam.response.paramData)
                //            {
                //                if (responseParamList.FindIndex(x => x.seq == paramData.seq) == -1)
                //                {
                //                    responseParamList.Add(paramData);
                //                }
                //            }
                //        }
                //    }
                //}
            Dictionary<ParamDatas, int> paramSeqDic = new Dictionary<ParamDatas, int>();
            int i = 0;
            foreach (var shortblock in shortBlockList)
            {
                if (!blockMetaSeq.Contains(shortblock.blockMetaSeq))
                {
                    blockMetaSeq.Add(shortblock.blockMetaSeq);
                    string sendData = string.Format(@"
                    {{
                    ""command"":""param-list"",
                    ""blockSeq"":""{0}""
                    }}", shortblock.Seq);
                    string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlShortBlock"], sendData);
                    if (responseData != null)
                    {
                        ResponseParamList responseParam = JsonConvert.DeserializeObject<ResponseParamList>(responseData);
                        if (responseParam.code == 200)
                        {
                            foreach (var paramData in responseParam.response.paramData)
                            {
                                if (paramSeqDic.Where(paramD => paramD.Key.seq == paramData.seq).Count() != 0)
                                {
                                    if (paramData.propInfo != null)
                                    {
                                        ParamDatas selectedParam = paramSeqDic.Where(paramD => paramD.Key.seq == paramData.seq).Select(paramD => paramD.Key).ToList()[0];
                                        paramSeqDic[selectedParam] = paramSeqDic[selectedParam] + 1;
                                    }
                                }
                                else
                                {
                                    paramSeqDic.Add(paramData, 1);
                                }
                                //if (responseParamList.FindIndex(x => x.seq == paramData.seq) == -1)
                                //{
                                //    responseParamList.Add(paramData);
                                //}
                            }
                        }
                        i++;
                    }
                }
            }

            return paramSeqDic.Where(paramD => paramD.Value == i).Select(paramD => paramD.Key).ToList();

        }

        public bool SetSelectedParams(ParamDatas paramDatas)
        {
            if (selectedParamDataList.FindIndex(x => x.seq == paramDatas.seq) == -1)
            {
                selectedParamDataList.Add(paramDatas);
                return true;
            }
            else
            {

                return false;
            }
        }
        
        public void RemoveSelectedParams(string seq)
        {
            selectedParamDataList.RemoveAt(selectedParamDataList.FindIndex(x => x.seq == seq));
        }

        private void chkUse3D_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUse3D.Checked)
            {
                panelParamCnt1.Controls.Clear();
                if (paramListData != null)
                {
                    panelParamCnt1.Controls.Add(new BinParameterSelectControl(this, paramListData));
                }
                else
                {
                    panelParamCnt1.Controls.Add(new BinParameterSelectControl(this, new List<ParamDatas>()));
                }
            }
            else
            {
                if (panelParamCnt1.Controls.Count != 0)
                {
                    BinParameterSelectControl control = (BinParameterSelectControl)panelParamCnt1.Controls[0];
                    if (control.beforeSeq != null)
                    {
                        RemoveSelectedParams(control.beforeSeq);
                    }
                }
                panelParamCnt1.Controls.Clear();
            }
        }
    }

    
}
