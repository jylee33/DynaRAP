using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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

namespace DynaRAP.UControl
{
    public partial class ParamModuleSelectControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ListParamModule paramModuleData = null;
        ParameterModuleControl parameterModuleControl = null;

        private List<ListParamModule> paramModuleList = null;
        public ParamModuleSelectControl(ParameterModuleControl parameterModuleControl)
        {
            this.parameterModuleControl = parameterModuleControl;
            InitializeComponent();
        }

        private void ParamDataSelectControl_Load(object sender, EventArgs e)
        {
            SetListParamModuleList();
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void edtSearch_Click(object sender, ButtonPressedEventArgs e)
        {
            //if (comboBoxEdit1.Text.Equals("") || edtSearch.Text.Equals(""))
            //{
            //    MessageBox.Show("구분 값이나 검색어가 없습니다.");
            //    return;
            //}
            //if (e.Button.Kind.ToString() == "Search")
            //{
            //    string sendData = string.Format(@"
            //    {{
            //    ""command"":""search"",
            //    ""sourceType"": ""{0}"",
            //    ""keyword"": ""{1}""
            //    }}" , comboBoxEdit1.Text.ToLower(), edtSearch.Text);
            //    string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            //    if (responseData != null)
            //    {
            //        //ParamModuleResponse paramModuleResponse = JsonConvert.DeserializeObject<ParamModuleResponse>(responseData);
            //        //byte[] byte64 = Convert.FromBase64String(paramModuleResponse.response[0].partName);
            //        //string decName = Encoding.UTF8.GetString(byte64);
            //        MessageBox.Show(responseData);
            //    }
            //}
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {

        }

        private void btnListSave_Click(object sender, EventArgs e)
        {
            if(paramModuleData == null)
            {
                MessageBox.Show("수정할 파라미터모듈이 선택되지 않았습니다.");
            }
            else
            {
                ModifyParamModule();
            }
        }

        private void btnNewParamModuleSave_Click(object sender, EventArgs e)
        {
            if (edtModuleName.Text == "")
            {
                MessageBox.Show("파라미터모듈 이름을 입력하세요.");
            }
            else
            {
                SaveNewParamModule();
            }
        }

        private void btnDelParamModule_Click(object sender, EventArgs e)
        {
            if(paramModuleData == null)
            {
                MessageBox.Show("삭제할 파라미터모듈이 선택되지 않았습니다.");
            }
            else
            {
                DeleteParamModule();
            }
        }


        private void btnParamModuleCopy_Click(object sender, EventArgs e)
        {
            if (paramModuleData == null)
            {
                MessageBox.Show("복사할 파라미터모듈이 선택되지 않았습니다.");
            }
            else
            {
                CopyParamModule();
            }
        }


        private void edtTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
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

        private void removeTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit btn = sender as ButtonEdit;
            panelTag.Controls.Remove(btn);

        }

        private void SetListParamModuleList()
        {
            //moduleNameList.Properties.DataSource = null;

            string sendData = string.Format(@"
                {{
                ""command"":""list""
                }}");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null && responseData != "")
            {
                paramModuleList = new List<ListParamModule>();
                ListParamModuleResponse paramModuleResponse = JsonConvert.DeserializeObject<ListParamModuleResponse>(responseData);
                foreach (ListParamModule list in paramModuleResponse.response)
                {
                    byte[] byte64 = Convert.FromBase64String(list.moduleName);
                    string moduleName = Encoding.UTF8.GetString(byte64);
                    list.moduleName = moduleName;
                    //paramModuleList.Add(new ParamModuleData(list.seq, list.moduleName, list.copyFromSeq));
                }
                paramModuleList = paramModuleResponse.response;
                this.gridControl1.DataSource = paramModuleList;
            }
        }


        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            paramModuleData = (ListParamModule)gridView1.GetFocusedRow();
            //MessageBox.Show(paramModuleData.Seq+"/"+ paramModuleData.ModuleName +"/"+ paramModuleData.CopyFromSeq);
            edtModuleName.Text = paramModuleData.moduleName;
            string[] tagList = paramModuleData.dataProp.tags.Split('|');
            panelTag.Controls.Clear();
            foreach (string value in tagList)
            {
                addTag(value);
            }
        }
        private void SaveNewParamModule()
        {
            //moduleNameList.Properties.DataSource = null;
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(edtModuleName.Text);
            string encName = Convert.ToBase64String(basebyte);
            string tagValues = string.Empty;
            foreach (ButtonEdit btn in this.panelTag.Controls)
            {
                tagValues += btn.Text + "|";
            }
            if (tagValues != String.Empty)
            {
                tagValues = tagValues.Substring(0, tagValues.LastIndexOf("|"));
            }
            string sendData = string.Format(@"
                {{
                ""command"":""add"",
                ""moduleName"":""{0}"",
                ""dataProp"": {{
                               ""key"":""value"",
                               ""tags"":""{1}""
                              }}
                }}", encName, tagValues);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                JsonData result= JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    MessageBox.Show("저장 성공");
                    paramModuleData = null;
                    SetListParamModuleList();
                    parameterModuleControl.SetListParamModule();
                }
                else
                {
                    MessageBox.Show("저장 실패");
                }
            }
        }
        private void DeleteParamModule()
        {
            string sendData = string.Format(@"
                {{
                ""command"":""remove"",
                ""moduleSeq"":""{0}""
                }}", paramModuleData.seq);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    MessageBox.Show("삭제 성공");
                    paramModuleData = null;
                    SetListParamModuleList();
                    parameterModuleControl.SetListParamModule();
                }
                else
                {
                    MessageBox.Show("삭제 실패");
                }
            }
        }

        private void ModifyParamModule()
        {
            //moduleNameList.Properties.DataSource = null;
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(edtModuleName.Text);
            string encName = Convert.ToBase64String(basebyte);
            string tagValues = string.Empty;
            foreach (ButtonEdit btn in this.panelTag.Controls)
            {
                tagValues += btn.Text + "|";
            }
            if (tagValues != String.Empty)
            {
                tagValues = tagValues.Substring(0, tagValues.LastIndexOf("|"));
            }
            string sendData = string.Format(@"
                {{
                ""command"":""modify"",
                ""moduleSeq"":""{0}"",
                ""moduleName"":""{1}"",
                ""dataProp"": {{
                               ""key2"":""value!!!"",
                               ""tags"":""{2}""
                              }}
                }}", paramModuleData.seq, encName, tagValues);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    MessageBox.Show("수정 성공");
                    paramModuleData = null;
                    SetListParamModuleList();
                    parameterModuleControl.SetListParamModule();
                }
                else
                {
                    MessageBox.Show("수정 실패");
                }
            }
        }

        private void CopyParamModule()
        {
            string sendData = string.Format(@"
                {{
                ""command"":""copy"",
                ""moduleSeq"":""{0}""
                }}", paramModuleData.seq);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    MessageBox.Show("복사 성공");
                    paramModuleData = null;
                    SetListParamModuleList();
                    parameterModuleControl.SetListParamModule();
                }
                else
                {
                    MessageBox.Show("복사 실패");
                }
            }
        }


    }
}
