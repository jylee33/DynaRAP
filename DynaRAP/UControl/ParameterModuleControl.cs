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
                    plotControl.SetSelectDataSource(parammoduleSeq);
                    beforeSelectIndex = moduleNameList.ItemIndex;
                }
                else
                {
                    DialogResult result = MessageBox.Show("저장되지 않은 선택 데이터 및 수식, PLOT은 삭제됩니다. \n파라미터를 저장 후 변경하시겠습니까?", "파라미터모듈 변경", MessageBoxButtons.YesNoCancel);
                    bool resultFlag = true;
                    if (result == DialogResult.Yes)
                    {
                        resultFlag = dataSelectionControl.SelectDataSaveRequest();
                        resultFlag = plotControl.PlotSaveRequest();
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
                        plotControl.SetSelectDataSource(parammoduleSeq);
                        beforeSelectIndex = moduleNameList.ItemIndex;
                    }
                    else if (result == DialogResult.No)
                    {
                        parammoduleSeq = moduleNameList.GetColumnValue("Seq").ToString();
                        GetSelectDataList(parammoduleSeq);
                        dataSelectionControl.SetSelectDataSource(parammoduleSeq);
                        expressionControl.SetSelectDataSource(parammoduleSeq);
                        plotControl.SetSelectDataSource(parammoduleSeq);
                        beforeSelectIndex = moduleNameList.ItemIndex;
                    }
                    else
                    {
                        moduleNameList.ItemIndex = beforeSelectIndex;
                    }
                }
            }
        }

        private void GetSelectDataList(string paramModuleSeq)
        {
            string sendData = string.Format(@"
                {{
                ""command"":""source-list"",
                ""moduleSeq"": ""{0}""
                }}", paramModuleSeq);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                selectDataList = new List<ParamDataSelectionData>();
                SaveParamModuleSelectDataResponse paramModuleResponse = JsonConvert.DeserializeObject<SaveParamModuleSelectDataResponse>(responseData);
                if (paramModuleResponse.response != null && paramModuleResponse.response.Count() != 0)
                {
                    foreach (var list in paramModuleResponse.response)
                    {
                        selectDataList.Add(new ParamDataSelectionData(list.sourceType, Utils.base64StringDecoding(list.sourceName), list.paramKey, list.julianStartAt, list.julianEndAt, list.dataCount, list.sourceSeq, list.useTime, list.seq, list.sourceType == "parammodule" ? 0 : 1, list.paramSeq));
                    }
                }
                this.gridControl1.DataSource = selectDataList;

                //MessageBox.Show(responseData);
            }
        }
        public void SaveChangePlotFromEQ(string paramModuleSeq)
        {
            plotControl.SetSelectDataSource(paramModuleSeq);
        }
    }
}
