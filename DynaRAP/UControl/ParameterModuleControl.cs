using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DynaRAP.Data;
using DynaRAP.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class ParameterModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        ParamDataSelectControl dataSelectionControl = null;
        ParamExpressionControl expressionControl = null;
        ParamPlotControl plotControl = null;
        ParamModuleSelectControl paramModuleControl = null;

        private ListParamModuleResponse paramModuleList = null;
        private List<ParamModuleData> paramModuleCombo = null;
        private List<ParamDataSelectionData> selectDataList = null;
        private int beforeSelectIndex = -1;

        public ParameterModuleControl()
        {
            InitializeComponent();

            dataSelectionControl = new ParamDataSelectControl(this);
            expressionControl = new ParamExpressionControl(this);
            plotControl = new ParamPlotControl(this);
            paramModuleControl = new ParamModuleSelectControl(this);
        }

        private void ParameterModuleControl_Load(object sender, EventArgs e)
        {
            //xtraTabControl1.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover;
            //xtraTabControl1.CloseButtonClick += XtraTabControl1_CloseButtonClick;

            AddTabPage("ParamModuleSelect", "파라미터모듈선택", paramModuleControl);
            AddTabPage("ParamDataSelect", "데이터 선택", dataSelectionControl);
            AddTabPage("ParamExpression", "수식 관리", expressionControl);
            AddTabPage("ParamPlot", "PLOT 구성", plotControl);


            moduleNameList.Properties.DisplayMember = "ModuleName";
            moduleNameList.Properties.ValueMember = "Seq";
            moduleNameList.Properties.NullText = "";
            SetListParamModule();
        }

        private void InitializeParamModuleDataList()
        {

        }

        private void XtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
        }

        private void AddTabPage(string tabName, string tabTitle, XtraUserControl ctrl)
        {
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = tabName;
            tabPage.Text = tabTitle;
            //tabPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;

            ctrl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(ctrl);
        }
        public void SetListParamModule()
        {
            moduleNameList.Properties.DataSource = null;
            string sendData = string.Format(@"
                {{
                ""command"":""list""
                }}");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null && responseData != "")
            {
                paramModuleList = new ListParamModuleResponse();
                paramModuleCombo = new List<ParamModuleData>();

                paramModuleList = JsonConvert.DeserializeObject<ListParamModuleResponse>(responseData);
                foreach (ListParamModule list in paramModuleList.response)
                {
                    byte[] byte64 = Convert.FromBase64String(list.moduleName);
                    string moduleName = Encoding.UTF8.GetString(byte64);
                    list.moduleName = moduleName;
                    paramModuleCombo.Add(new ParamModuleData(list.seq, list.moduleName, list.copyFromSeq));
                }
                moduleNameList.Properties.DataSource = paramModuleCombo;
                moduleNameList.Properties.PopulateColumns();
                moduleNameList.Properties.Columns["ModuleName"].Width = 800;

                moduleNameList.Properties.PopulateColumns();
                moduleNameList.Properties.ShowHeader = false;
                moduleNameList.Properties.Columns["Seq"].Visible = false;
                moduleNameList.Properties.Columns["CopyFromSeq"].Visible = false;
                moduleNameList.Properties.ShowFooter = false;
            }
        }

        public void moduleNameList_EditValueChanged(object sender, EventArgs e)
        {
            string parammoduleSeq = null;
            if (moduleNameList.GetColumnValue("Seq") != null && moduleNameList.ItemIndex != beforeSelectIndex)
            {
                if (beforeSelectIndex == -1)
                {
                    parammoduleSeq = moduleNameList.GetColumnValue("Seq").ToString();
                    GetSelectDataList(parammoduleSeq);
                    dataSelectionControl.SetSelectDataSource(parammoduleSeq);
                    expressionControl.SetSelectDataSource(parammoduleSeq);
                    string moduleName = moduleNameList.GetColumnValue("ModuleName").ToString();
                    plotControl.SetSelectDataSource(parammoduleSeq, moduleName);
                    beforeSelectIndex = moduleNameList.ItemIndex;
                }
                else
                {
                    DialogResult result = MessageBox.Show("저장되지 않은 선택 데이터 및 수식, PLOT은 삭제됩니다. \n파라미터를 저장 후 변경하시겠습니까?", "파라미터모듈 변경", MessageBoxButtons.YesNoCancel);
                    bool resultFlag = true;
                    if (result == DialogResult.Yes)
                    {
                        resultFlag = dataSelectionControl.SelectDataSaveRequest();
                        plotControl.PlotSaveRequest();
                        if (!resultFlag)
                        {
                            moduleNameList.ItemIndex = beforeSelectIndex;
                            MessageBox.Show("저장에 실패했습니다.");
                            return;
                        }
                        parammoduleSeq = moduleNameList.GetColumnValue("Seq").ToString();
                        GetSelectDataList(parammoduleSeq);
                        dataSelectionControl.SetSelectDataSource(parammoduleSeq);
                        expressionControl.SetSelectDataSource(parammoduleSeq);

                        string moduleName = moduleNameList.GetColumnValue("ModuleName").ToString();
                        plotControl.SetSelectDataSource(parammoduleSeq, moduleName);
                        beforeSelectIndex = moduleNameList.ItemIndex;
                    }
                    else if (result == DialogResult.No)
                    {
                        parammoduleSeq = moduleNameList.GetColumnValue("Seq").ToString();
                        GetSelectDataList(parammoduleSeq);
                        dataSelectionControl.SetSelectDataSource(parammoduleSeq);
                        expressionControl.SetSelectDataSource(parammoduleSeq);

                        string moduleName = moduleNameList.GetColumnValue("ModuleName").ToString();
                        plotControl.SetSelectDataSource(parammoduleSeq, moduleName);
                        beforeSelectIndex = moduleNameList.ItemIndex;
                    }
                    else
                    {
                        moduleNameList.ItemIndex = beforeSelectIndex;
                    }
                }
            }
        }

        public void GetSelectDataList(string paramModuleSeq)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            string sendData = string.Format(@"
                {{
                ""command"":""source-list"",
                ""moduleSeq"": ""{0}""
                }}", paramModuleSeq);
            mainForm.ShowSplashScreenManager("파라미터 정보를 불러오는 중입니다. 잠시만 기다려주십시오.");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            mainForm.HideSplashScreenManager();

            if (responseData != null)
            {
                selectDataList = new List<ParamDataSelectionData>();
                SaveParamModuleSelectDataResponse paramModuleResponse = JsonConvert.DeserializeObject<SaveParamModuleSelectDataResponse>(responseData);
                if (paramModuleResponse.response != null && paramModuleResponse.response.Count() != 0)
                {
                    foreach (var list in paramModuleResponse.response)
                    {
                        string sourceType = null;
                        switch (list.sourceType)
                        {
                            case "shortblock":
                            case "parammodule":
                                sourceType = list.sourceType.ToUpper();
                                break;
                            case "part":
                                sourceType = "분할데이터";
                                break;
                            case "dll":
                                sourceType = "기준데이터";
                                break;
                        }

                        if(list.sourceType == "parammodule")
                        {
                            list.paramKey = list.paramKey.Substring(0, list.paramKey.IndexOf('_'));
                        }
                        selectDataList.Add(new ParamDataSelectionData(sourceType, sourceType, Utils.base64StringDecoding(list.sourceName), string.Format("{0}_{1}", list.sourceNo, list.paramKey), list.julianStartAt, list.julianEndAt, list.dataCount, list.sourceSeq, list.useTime, list.seq, list.sourceType == "parammodule" ? 0 : 1, list.paramSeq));
                    }
                }
                this.gridControl1.DataSource = selectDataList;

                //MessageBox.Show(responseData);
            }
        }
        public void SaveChangePlotFromEQ(string paramModuleSeq)
        {
            string moduleName = moduleNameList.GetColumnValue("ModuleName").ToString();
            plotControl.SetSelectDataSource(paramModuleSeq, moduleName) ;
        }
    }
}
