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
        private int beforeSelectIndex = -1;

        public ParameterModuleControl()
        {
            InitializeComponent();

            dataSelectionControl = new ParamDataSelectControl();
            expressionControl = new ParamExpressionControl();
            plotControl = new ParamPlotControl();
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
                if (beforeSelectIndex == -1 || MessageBox.Show("저장되지 않은 선택 데이터는 삭제됩니다. 파라미터를 변경하시겠습니까?", "데이터선택", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    parammoduleSeq = moduleNameList.GetColumnValue("Seq").ToString();
                    dataSelectionControl.SetSelectDataSource(parammoduleSeq);
                    beforeSelectIndex = moduleNameList.ItemIndex;
                }
                else
                {
                    moduleNameList.ItemIndex = beforeSelectIndex;
                }
            }
        }
    }
}
