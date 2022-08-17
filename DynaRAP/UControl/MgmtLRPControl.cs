using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DynaRAP.Common;
using DynaRAP.Data;
using DynaRAP.Forms;
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
    public partial class MgmtLRPControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        int focusedNodeId = 0;
        TreeListNode focusedNode = null;
        List<ResponseParam> paramList = null;
        List<ResponsePropList> propList = new List<ResponsePropList>();
        string selPropertyType = string.Empty;
        List<MgmtLRPExtraControl> extraControlList = new List<MgmtLRPExtraControl>();

        public MgmtLRPControl()
        {
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        #region Method
        private void InitializeDirDataList()
        {
            //TreeList treeList1 = new TreeList();
            treeList1.Parent = this.splitContainer1.Panel1;
            treeList1.Dock = DockStyle.Fill;
            //Specify the fields that arrange underlying data as a hierarchy.
            treeList1.KeyFieldName = "ID";
            treeList1.ParentFieldName = "ParentID";
            //Allow the treelist to create columns bound to the fields the KeyFieldName and ParentFieldName properties specify.
            treeList1.OptionsBehavior.PopulateServiceColumns = true;

            //Specify the data source.
            //treeList1.DataSource = null;
            treeList1.DataSource = GetDirList();
            //The treelist automatically creates columns for the public fields found in the data source. 
            //You do not need to call the TreeList.PopulateColumns method unless the treeList1.OptionsBehavior.AutoPopulatefColumns option is disabled.
            treeList1.ForceInitialize();

            treeList1.RowHeight = 23;
            treeList1.OptionsView.ShowHorzLines = false;
            treeList1.OptionsView.ShowVertLines = false;
            treeList1.OptionsView.ShowIndicator = true;
            treeList1.OptionsView.ShowTreeLines = DevExpress.Utils.DefaultBoolean.False;
            treeList1.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            treeList1.OptionsView.ShowSummaryFooter = false;
            treeList1.OptionsView.AutoWidth = true;
            treeList1.OptionsView.ShowAutoFilterRow = true;

            treeList1.OptionsFilter.AllowFilterEditor = false;

            treeList1.OptionsSelection.MultiSelect = false;

            treeList1.OptionsNavigation.AutoFocusNewNode = true;
            treeList1.OptionsNavigation.AutoMoveRowFocus = true;

#if !DEBUG
            treeList1.OptionsView.ShowColumns = false;
            //Hide the key columns. An end-user can access them from the Customization Form.
            treeList1.Columns[treeList1.KeyFieldName].Visible = false;
            treeList1.Columns[treeList1.ParentFieldName].Visible = false;
            treeList1.Columns["DirType"].Visible = false;
            treeList1.Columns["RefSeq"].Visible = false;
            treeList1.Columns["RefSubSeq"].Visible = false;
#endif
            //Access the automatically created columns.
            TreeListColumn colName = treeList1.Columns["DirName"];

            colName.MinWidth = 200;

            //Make the Project column read-only.
            colName.OptionsColumn.ReadOnly = true;
            colName.OptionsColumn.AllowEdit = false;

            //Sort data against the Project column
            colName.SortIndex = -1;// 0;

            //treeList1.OptionsView.ShowCheckBoxes = true; // 제일 앞에 checkBox 붙이는 옵션
            treeList1.PopupMenuShowing += TreeList1_PopupMenuShowing;

            treeList1.ExpandAll();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate
            {
                treeList1.BestFitColumns();
            }));



        }

        private void InitializeParamList()
        {
            /*cboParamList.Properties.Items.Clear();

            paramList = GetParamList();

            foreach (ResponseParam list in paramList)
            {
                cboParamList.Properties.Items.Add(list.paramKey);
            }
            cboParamList.SelectedIndex = -1;*/

            paramList = GetParamList();

            //TreeList treeList2 = new TreeList();
            //treeList2.Parent = this.splitContainer1.Panel1;
            //treeList2.Dock = DockStyle.Fill;
            //Specify the fields that arrange underlying data as a hierarchy.
            treeList2.KeyFieldName = "ID";
            treeList2.ParentFieldName = "ParentID";
            //Allow the treelist to create columns bound to the fields the KeyFieldName and ParentFieldName properties specify.
            treeList2.OptionsBehavior.PopulateServiceColumns = true;

            //Specify the data source.
            //treeList2.DataSource = null;
            treeList2.DataSource = paramList; //GetDirList();
            //The treelist automatically creates columns for the public fields found in the data source. 
            //You do not need to call the TreeList.PopulateColumns method unless the treeList2.OptionsBehavior.AutoPopulatefColumns option is disabled.
            treeList2.ForceInitialize();

            treeList2.RowHeight = 23;
            treeList2.OptionsView.ShowHorzLines = false;
            treeList2.OptionsView.ShowVertLines = false;
            treeList2.OptionsView.ShowIndicator = true;
            treeList2.OptionsView.ShowTreeLines = DevExpress.Utils.DefaultBoolean.False;
            treeList2.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            treeList2.OptionsView.ShowSummaryFooter = false;
            treeList2.OptionsView.AutoWidth = true;
            treeList2.OptionsView.ShowAutoFilterRow = true;

            treeList2.OptionsFilter.AllowFilterEditor = false;

            treeList2.OptionsSelection.MultiSelect = false;

            treeList2.OptionsNavigation.AutoFocusNewNode = true;
            treeList2.OptionsNavigation.AutoMoveRowFocus = true;

#if !DEBUG
            treeList2.OptionsView.ShowColumns = false;
            //Hide the key columns. An end-user can access them from the Customization Form.
            treeList2.Columns[treeList2.KeyFieldName].Visible = false;
            treeList2.Columns[treeList2.ParentFieldName].Visible = false;
#endif
            //Access the automatically created columns.
            TreeListColumn colName = treeList2.Columns["paramKey"];

            colName.MinWidth = 200;

            //Make the Project column read-only.
            colName.OptionsColumn.ReadOnly = true;
            colName.OptionsColumn.AllowEdit = false;

            //Sort data against the Project column
            colName.SortIndex = -1;// 0;

            //treeList2.OptionsView.ShowCheckBoxes = true; // 제일 앞에 checkBox 붙이는 옵션
            //treeList2.PopupMenuShowing += treeList2_PopupMenuShowing;

            treeList2.ExpandAll();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate
            {
                treeList2.BestFitColumns();
            }));

        }

        private void RefreshTree()
        {
            treeList1.DataSource = null;
            treeList1.DataSource = GetDirList();
            //The treelist automatically creates columns for the public fields found in the data source. 
            //You do not need to call the TreeList.PopulateColumns method unless the treeList1.OptionsBehavior.AutoPopulatefColumns option is disabled.
            treeList1.ForceInitialize();

            treeList1.ExpandAll();
        }

        private bool AddDir(string dirType, string name, string pid)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlDir"];
                string sendData = string.Format(@"
            {{ ""command"":""add"",
            ""seq"":""{0}"",
            ""parentDirSeq"":""{1}"",
            ""dirName"":""{2}"",
            ""dirType"":""{3}"",
            ""dirIcon"":"""",
            ""refSeq"":null,
            ""refSubSeq"":null
            }}"
                , 1, pid, name, dirType);

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                DirJsonData result = JsonConvert.DeserializeObject<DirJsonData>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    this.focusedNodeId = result.response.seq;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private bool ModifyDir(string dirType, string id, string pid, string name, string paramPack, string seq)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlDir"];
                string sendData = string.Format(@"
            {{ ""command"":""modify"",
            ""seq"":""{0}"",
            ""parentDirSeq"":""{1}"",
            ""dirName"":""{2}"",
            ""dirType"":""{3}"",
            ""dirIcon"":"""",
            ""refSeq"":""{4}"",
            ""refSubSeq"":""{5}""
            }}"
                , id, pid, name, dirType, paramPack, seq);

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                DirJsonData result = JsonConvert.DeserializeObject<DirJsonData>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    this.focusedNodeId = result.response.seq;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private bool RemoveDir(string id)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlDir"];
                string sendData = string.Format(@"
            {{""command"":""remove"",
            ""seq"":""{0}""
            }}
            ", id);

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                DirJsonData result = JsonConvert.DeserializeObject<DirJsonData>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }
        
        private BindingList<DirData> GetDirList()
        {
            BindingList<DirData> list = new BindingList<DirData>();

            try
            {
                string url = ConfigurationManager.AppSettings["UrlDir"];
                string sendData = "{ \"command\": \"list\" }";

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                DirJsonData result = JsonConvert.DeserializeObject<DirJsonData>(responseText);
                //object result = JsonConvert.DeserializeObject(responseText);

                foreach (Pool pool in result.response.pools)
                {
                    byte[] byte64 = Convert.FromBase64String(pool.dirName);
                    string name = Encoding.UTF8.GetString(byte64);

                    if (pool.dirType.Equals("folder") || pool.dirType.Equals("param"))
                    {
                        list.Add(new DirData(pool.seq, pool.parentDirSeq, pool.dirType, name, pool.refSeq, pool.refSubSeq));
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

            return list;

        }

        private List<ResponseParam> GetParamList()
        {
            ListParamJsonData result = null;

            try
            {
                string url = ConfigurationManager.AppSettings["UrlParam"];
                //string sendData = @"
                //{
                //""command"":""list"",
                //""pageNo"":1,
                //""pageSize"":3000,
                //""resultDataType"": ""map""
                //}";
                string sendData = @"
            {
            ""command"":""list"",
            ""pageNo"":1,
            ""pageSize"":3000
            }";

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                result = JsonConvert.DeserializeObject<ListParamJsonData>(responseText);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

            return result.response;

        }

        private bool AddModParameter(string opType, string paramKey, string tags, string extras, string seq, string propSeq, string paramPack = "")
        {
            try
            {
                ResponseParam param = paramList.Find(x => x.paramKey.Equals(paramKey));
                if (param == null || opType.Equals("modify"))
                {
                    lblDuplicateKey.Visible = false;
                }
                else
                {
                    lblDuplicateKey.Visible = true;
                    MessageBox.Show("Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string url = ConfigurationManager.AppSettings["UrlParam"];
                string sendData = string.Format(@"
            {{
            ""command"":""{0}"",
            ""seq"":""{15}"",
            ""paramPack"":""{1}"",
            ""propSeq"":""{16}"",
            ""paramKey"":""{2}"",
            ""adamsKey"":""{3}"",
            ""zaeroKey"":""{4}"",
            ""grtKey"":""{5}"",
            ""fltpKey"":""{6}"",
            ""fltsKey"":""{7}"",
            ""partInfo"":""{8}"",
            ""partInfoSub"":""{9}"",
            ""lrpX"":""{10}"",
            ""lrpY"":""{11}"",
            ""lrpZ"":""{12}"",
            ""tags"":""{13}"",
            ""extras"":{14}
            }}"
                , opType, paramPack, paramKey, edtAdams.Text
                , edtZaero.Text, edtGrt.Text, edtFltp.Text, edtFlts.Text
                , cboPart.Text, cboPartLocation.Text
                , edtLrpX.Text, edtLrpY.Text, edtLrpZ.Text, tags, extras
                , seq, propSeq);

                if(opType.Equals("add"))
                {
                    // add 의 경우에는 seq, paramPack, propSeq 를 null 로 주거나 파라미터를 빼야 한다.
                    sendData = string.Format(@"
                    {{
                    ""command"":""{0}"",
                    ""seq"":null,
                    ""paramPack"":null,
                    ""propSeq"":null,
                    ""paramKey"":""{2}"",
                    ""adamsKey"":""{3}"",
                    ""zaeroKey"":""{4}"",
                    ""grtKey"":""{5}"",
                    ""fltpKey"":""{6}"",
                    ""fltsKey"":""{7}"",
                    ""partInfo"":""{8}"",
                    ""partInfoSub"":""{9}"",
                    ""lrpX"":""{10}"",
                    ""lrpY"":""{11}"",
                    ""lrpZ"":""{12}"",
                    ""tags"":""{13}"",
                    ""extras"":{14}
                    }}"
                    , opType, paramPack, paramKey, edtAdams.Text
                    , edtZaero.Text, edtGrt.Text, edtFltp.Text, edtFlts.Text
                    , cboPart.Text, cboPartLocation.Text
                    , edtLrpX.Text, edtLrpY.Text, edtLrpZ.Text, tags, extras
                    , seq, propSeq);
                }

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                Console.WriteLine(responseText);
                AddParamJsonData result = JsonConvert.DeserializeObject<AddParamJsonData>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    //this.focusedNodeId = result.response.seq;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private bool RemoveParam(string paramPack)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlParam"];
                string sendData = string.Format(@"
            {{
            ""command"":""remove"",
            ""paramPack"":""{0}""
            }}"
                , paramPack);

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                DirJsonData result = JsonConvert.DeserializeObject<DirJsonData>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        MessageBox.Show(result.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void InitializeProperty()
        {
            try
            {
                propList.Clear();
                propList = GetPropertyInfo();

                foreach (ResponsePropList prop in propList)
                {
                    if (cboPropertyType.Properties.Items.Contains(prop.propType) == false && prop.deleted == false)
                    {
                        cboPropertyType.Properties.Items.Add(prop.propType);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private List<ResponsePropList> GetPropertyInfo()
        {
            try
            {
                BindingList<DirData> list = new BindingList<DirData>();

                string url = ConfigurationManager.AppSettings["UrlParam"];
                string sendData = @"
            {
            ""command"":""prop-list"",
            ""propType"":""""
            }";

                log.Info("url : " + url);
                log.Info(sendData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 30 * 1000;
                //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }

                // Response 처리
                string responseText = string.Empty;
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }

                //Console.WriteLine(responseText);
                PropListResponse result = JsonConvert.DeserializeObject<PropListResponse>(responseText);

                return result.response;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        private void InitializePropertyCode(string text)
        {
            cboPropertyCode.Properties.Items.Clear();
            cboUnit.Properties.Items.Clear();

            cboPropertyCode.Text = String.Empty;
            cboUnit.Text = String.Empty;

            foreach (ResponsePropList prop in propList)
            {
                if (prop.propType.Equals(text))
                {
                    if (cboPropertyCode.Properties.Items.Contains(prop.propCode) == false)
                        cboPropertyCode.Properties.Items.Add(prop.propCode);
                    if (cboUnit.Properties.Items.Contains(prop.paramUnit) == false)
                        cboUnit.Properties.Items.Add(prop.paramUnit);
                }
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

        const int START_EXTRA_INDEX = 0;
        const int EXTRA_HEIGHT = 22;
        const int FLOWLAYOUTPANEL1_HEIGHT = 22;
        const int MAX_EXTRA_CNT = 10;
        int extraIndex = START_EXTRA_INDEX;

        private void AddExtra(string extraKey = "", string extraVal = "")
        {
            MgmtLRPExtraControl ctrl = new MgmtLRPExtraControl(extraKey, extraVal);
            ctrl.Title = "Parameter " + extraIndex.ToString();
            ctrl.DeleteBtnClicked += new EventHandler(ExtraControl_DeleteBtnClicked);
            //ctrl.Dock = DockStyle.Fill;
            flowLayoutPanel1.Controls.Add(ctrl);
            flowLayoutPanel1.Controls.SetChildIndex(ctrl, extraIndex++);
            extraControlList.Add(ctrl);

            if (extraIndex <= MAX_EXTRA_CNT)
            {
                flowLayoutPanel1.Height += EXTRA_HEIGHT;
                lblTag.Location = new Point(lblTag.Location.X, lblTag.Location.Y + EXTRA_HEIGHT);
                edtTag.Location = new Point(edtTag.Location.X, edtTag.Location.Y + EXTRA_HEIGHT);
                panelTag.Location = new Point(panelTag.Location.X, panelTag.Location.Y + EXTRA_HEIGHT);
                btnModifyParameter.Location = new Point(btnModifyParameter.Location.X, btnModifyParameter.Location.Y + EXTRA_HEIGHT);
                btnDeleteParameter.Location = new Point(btnDeleteParameter.Location.X, btnDeleteParameter.Location.Y + EXTRA_HEIGHT);
                btnSaveAsNewParameter.Location = new Point(btnSaveAsNewParameter.Location.X, btnSaveAsNewParameter.Location.Y + EXTRA_HEIGHT);
            }
        }

        void ExtraControl_DeleteBtnClicked(object sender, EventArgs e)
        {
            MgmtLRPExtraControl ctrl = sender as MgmtLRPExtraControl;
            flowLayoutPanel1.Controls.Remove(ctrl);
            extraControlList.Remove(ctrl);
            ctrl.Dispose();

            extraIndex--;

            if (extraIndex < MAX_EXTRA_CNT)
            {
                //flowLayoutPanel1.Height -= EXTRA_HEIGHT;
                flowLayoutPanel1.Height = extraIndex * EXTRA_HEIGHT;

                lblTag.Location = new Point(lblTag.Location.X, lblTag.Location.Y - EXTRA_HEIGHT);
                edtTag.Location = new Point(edtTag.Location.X, edtTag.Location.Y - EXTRA_HEIGHT);
                panelTag.Location = new Point(panelTag.Location.X, panelTag.Location.Y - EXTRA_HEIGHT);
                btnModifyParameter.Location = new Point(btnModifyParameter.Location.X, btnModifyParameter.Location.Y - EXTRA_HEIGHT);
                btnDeleteParameter.Location = new Point(btnDeleteParameter.Location.X, btnDeleteParameter.Location.Y - EXTRA_HEIGHT);
                btnSaveAsNewParameter.Location = new Point(btnSaveAsNewParameter.Location.X, btnSaveAsNewParameter.Location.Y - EXTRA_HEIGHT);
            }
        }

        #endregion Method

        #region EventHandler
        private void MgmtLRPControl_Load(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = 250;
            cboParamList.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboPropertyType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboPropertyCode.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboUnit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboPart.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboPartLocation.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboAirplane.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            cboPropertyType.SelectedIndexChanged += CboPropertyType_SelectedIndexChanged;
            cboPropertyCode.SelectedIndexChanged += CboPropertyCode_SelectedIndexChanged;

            btnAddExtra.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddExtra.Properties.AllowFocused = false;

            flowLayoutPanel1.HorizontalScroll.Enabled = false;

            InitializeProperty();
            InitializeParamList();
            InitializeDirDataList();
        }

        private void CboPropertyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                selPropertyType = combo.Text;
                InitializePropertyCode(selPropertyType);
            }
        }

        private void CboPropertyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                foreach(ResponsePropList prop in propList)
                {
                    if(prop.propType.Equals(selPropertyType) == false)
                    {
                        continue;
                    }
                    if(prop.propCode.Equals(combo.Text))
                    {
                        cboUnit.Text = prop.paramUnit;
                    }
                }
            }
        }


        private void addFolderMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            TreeListNode node = item.Tag as TreeListNode;
            //if (node == null) return;

            string folderName = Prompt.ShowDialog("Folder Name", "Add Folder");

            if (string.IsNullOrEmpty(folderName))
            {
                MessageBox.Show(Properties.Resources.InputFolderName, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(folderName);
            string name = Convert.ToBase64String(basebyte);

            bool bResult = false;
            if (node == null)
                bResult = AddDir("folder", name, "0");
            else
                bResult = AddDir("folder", name, node.GetValue("ID").ToString());

            if (bResult)
            {
                //TreeListNode newNode = treeList1.AppendNode(new object[] { "" }, node);
                //newNode.SetValue("DirName", folderName);

                ////treeList1.ExpandAll();
                //node.Expand();
                ////treeList1.Selection.Set(newNode);
                //treeList1.FocusedNode = newNode;

                RefreshTree();
                treeList1.FocusedNode = treeList1.FindNodeByFieldValue("ID", this.focusedNodeId);
            }
        }

        private void addParamMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            TreeListNode node = item.Tag as TreeListNode;
            //if (node == null) return;

            string paramName = Prompt.ShowDialog("Parameter Name", "Add Parameter");

            if (string.IsNullOrEmpty(paramName))
            {
                MessageBox.Show(Properties.Resources.InputParameterName, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(paramName);
            string name = Convert.ToBase64String(basebyte);

            bool bResult = false;
            if (node == null)
                bResult = AddDir("param", name, "0");
            else
                bResult = AddDir("param", name, node.GetValue("ID").ToString());

            if (bResult)
            {
                //TreeListNode newNode = treeList1.AppendNode(new object[] { "" }, node);
                //newNode.SetValue("DirName", paramName);

                ////treeList1.ExpandAll();
                //node.Expand();
                ////treeList1.Selection.Set(newNode);
                //treeList1.FocusedNode = newNode;

                RefreshTree();
                focusedNode = treeList1.FindNodeByFieldValue("ID", this.focusedNodeId);
                if (focusedNode != null)
                {
                    treeList1.FocusedNode = focusedNode;

                    //bResult = AddPameter(focusedNode.GetValue("RefSeq").ToString(), focusedNode.GetValue("RefSubSeq").ToString());
                }
            }
        }


        private void TreeList1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            // Check if a node's indicator cell is clicked.
            TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(e.Point);
            TreeListNode node = null;
            //if (hitInfo.HitInfoType == HitInfoType.RowIndicator)
            {
                node = hitInfo.Node;
            }
            //if (node == null) return;

            if (node != null)
                treeList1.FocusedNode = node;

            // Create the Add Folder command.
            DXMenuItem menuItemAddFolder = new DXMenuItem("Add Folder", this.addFolderMenuItemClick);
            menuItemAddFolder.Tag = node;
            e.Menu.Items.Add(menuItemAddFolder);

            // Create the Add Parameter command.
            DXMenuItem menuItemAdd = new DXMenuItem("Add Parameter", this.addParamMenuItemClick);
            menuItemAdd.Tag = node;
            e.Menu.Items.Add(menuItemAdd);

            if (node != null)
            {
                // Create Modity Node command.
                DXMenuItem menuItemModify = new DXMenuItem("Rename", this.modifyNodeMenuItemClick);
                menuItemModify.Tag = node;
                e.Menu.Items.Add(menuItemModify);

                // Create the Delete Node command.
                DXMenuItem menuItemDelete = new DXMenuItem("Delete Node", this.deleteNodeMenuItemClick);
                menuItemDelete.Tag = node;
                e.Menu.Items.Add(menuItemDelete);
            }

            // Refresh Node
            DXMenuItem menuItemRefresh = new DXMenuItem("Refresh Node", this.refreshNodeMenuItemClick);
            menuItemRefresh.Tag = node;
            e.Menu.Items.Add(menuItemRefresh);
        }

        private void modifyNodeMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            TreeListNode node = item.Tag as TreeListNode;
            if (node == null) return;

            string dirName = Prompt.ShowDialog("New Name", "Rename");

            if (string.IsNullOrEmpty(dirName))
            {
                MessageBox.Show(Properties.Resources.InputParameterName, Properties.Resources.StringWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(dirName);
            string name = Convert.ToBase64String(basebyte);

            bool bResult = ModifyDir(node.GetValue("DirType").ToString(), node.GetValue("ID").ToString(), node.GetValue("ParentID").ToString(), name, "0", "0");
            if (bResult)
            {
                RefreshTree();
                treeList1.FocusedNode = treeList1.FindNodeByFieldValue("ID", this.focusedNodeId);
            }
        }

        private void deleteNodeMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            TreeListNode node = item.Tag as TreeListNode;
            if (node == null) return;

            //node.TreeList.DeleteNode(node);

            if (MessageBox.Show(Properties.Resources.StringDelete, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            bool bResult = RemoveDir(node.GetValue("ID").ToString());
            if (bResult)
            {
                RefreshTree();
            }
        }

        private void refreshNodeMenuItemClick(object sender, EventArgs e)
        {
            RefreshTree();
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            treeList1.PostEditor();
        }

        private void treeList1_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            //if (e.Node.ParentNode == null)
            //{
            //    //   e.Node.SelectImageIndex = 1;
            //    //   e.Node.ImageIndex = 1;
            //    e.NodeImageIndex = 1;
            //}
            //else
            //{
            //    e.NodeImageIndex = 2;
            //}

            if (e.Node == null || e.Node.GetValue("DirType") == null)
                return;

            if (e.Node.GetValue("DirType").ToString().Equals("folder"))
            {
                e.NodeImageIndex = 0;
            }
            else
            {
                e.NodeImageIndex = 1;
            }
        }

        private void treeList1_GetSelectImage(object sender, GetSelectImageEventArgs e)
        {
            if (e.Node.ParentNode != null)
            {
                e.Node.SelectImageIndex = 1;
                e.Node.ImageIndex = 1;
            }
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = e.Node;

            if (node != null && node.GetValue("DirType") != null)
            {
                if (node.GetValue("DirType").ToString().Equals("folder"))
                {
                    this.btnLink.Visible = false;
                }
                else
                {
                    this.btnLink.Visible = true;
                }
                focusedNode = node;

                ResponseParam param = paramList.Find(x => x.paramPack.Equals(node.GetValue("RefSeq").ToString()));
                if (param == null)
                {
                    //this.cboParamList.SelectedIndex = -1;
                    treeList2.FocusedNode = null;
                }
                else
                {
                    //this.cboParamList.Text = param.paramKey;
                    treeList2.FocusedNode = treeList2.FindNodeByFieldValue("paramKey", param.paramKey);
                }
            }
        }

        private void treeList1_RowClick(object sender, RowClickEventArgs e)
        {
            TreeListNode node = e.Node;

            if (node != null)
            {
                if (node.GetValue("DirType").ToString().Equals("folder"))
                {
                    this.btnLink.Visible = false;
                }
                else
                {
                    this.btnLink.Visible = true;
                }
                focusedNode = node;

                ResponseParam param = paramList.Find(x => x.paramPack.Equals(node.GetValue("RefSeq").ToString()));
                if (param == null)
                {
                    //this.cboParamList.SelectedIndex = -1;
                    treeList2.FocusedNode = null;
                }
                else
                {
                    //this.cboParamList.Text = param.paramKey;
                    treeList2.FocusedNode = treeList2.FindNodeByFieldValue("paramKey", param.paramKey);
                }
            }
        }

        private void btnModifyParameter_Click(object sender, EventArgs e)
        {
            //string paramKey = cboParamList.Text;
            string paramKey = treeList2.FocusedNode.GetValue("paramKey").ToString();
            ResponseParam param = paramList.Find(x => x.paramKey.Equals(paramKey));

            if (param != null)
            {
                string tags = string.Empty;
                foreach(ButtonEdit tag in panelTag.Controls)
                {
                    tags += tag.Text + "|";
                }
                if (tags.Length > 0)
                {
                    tags = tags.Substring(0, tags.LastIndexOf("|"));
                }

                //Encoding
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(tags);
                string encTags = Convert.ToBase64String(basebyte);

                string extras = string.Empty;
                Dictionary<string, string> dicExtra = new Dictionary<string, string>();
                foreach (MgmtLRPExtraControl ctrl in flowLayoutPanel1.Controls)
                {
                    if (dicExtra.ContainsKey(ctrl.ExtraKey) == false)
                    {
                        dicExtra.Add(ctrl.ExtraKey, ctrl.ExtraVal);
                    }
                }

                extras = JsonConvert.SerializeObject(dicExtra);

                string propSeq = string.Empty;

                ResponsePropList prop = propList.Find(x => x.propType.Equals(cboPropertyType.Text) && x.propCode.Equals(cboPropertyCode.Text));
                if(prop != null)
                {
                    propSeq = prop.seq;
                }

                bool bResult = AddModParameter("modify", paramKey, encTags, extras, param.seq, propSeq, param.paramPack);

                if (bResult)
                {
                    MessageBox.Show(Properties.Resources.SuccessModify, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeParamList();
                    //cboParamList.Text = paramKey;
                }
            }
        }

        private void btnDeleteParameter_Click(object sender, EventArgs e)
        {
            string paramKey = treeList2.FocusedNode.GetValue("paramKey").ToString();
            string msg = string.Format(Properties.Resources.StringDeleteParameter, paramKey);
            if (MessageBox.Show(msg, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ResponseParam param = paramList.Find(x => x.paramKey.Equals(paramKey));

            if(param != null)
            {
                bool bResult = RemoveParam(param.paramPack);

                if (bResult)
                {
                    MessageBox.Show(Properties.Resources.SuccessRemove, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeParamList();
                    //cboParamList_SelectedIndexChanged(cboParamList, null);
                    //treeList2_RowClick(treeList2, null);
                }
            }

        }

        private void btnSaveAsNewParameter_Click(object sender, EventArgs e)
        {
            string paramKey = Prompt.ShowDialog("Parameter Key", "New Parameter");

            if (string.IsNullOrEmpty(paramKey))
            {
                MessageBox.Show(Properties.Resources.InputParameterKey, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tags = string.Empty;
            foreach (ButtonEdit tag in panelTag.Controls)
            {
                tags += tag.Text + "|";
            }
            if (tags.Length > 0)
            {
                tags = tags.Substring(0, tags.LastIndexOf("|"));
            }

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(tags);
            string encTags = Convert.ToBase64String(basebyte);

            string extras = string.Empty;
            Dictionary<string, string> dicExtra = new Dictionary<string, string>();
            foreach (MgmtLRPExtraControl ctrl in flowLayoutPanel1.Controls)
            {
                if(dicExtra.ContainsKey(ctrl.ExtraKey) == false)
                {
                    dicExtra.Add(ctrl.ExtraKey, ctrl.ExtraVal);
                }
            }

            extras = JsonConvert.SerializeObject(dicExtra);

            bool bResult = AddModParameter("add", paramKey, encTags, extras, "0", "0");

            if (bResult)
            {
                MessageBox.Show(Properties.Resources.SuccessAdd, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                InitializeParamList();
                //cboParamList.Text = paramKey;
            }
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            string dirName = focusedNode.GetValue("DirName").ToString();
            string dirType = focusedNode.GetValue("DirType").ToString();
            string id = focusedNode.GetValue("ID").ToString();
            string pid = focusedNode.GetValue("ParentID").ToString();
            string paramPack = string.Empty;
            string seq = string.Empty;
            string paramKey = treeList2.FocusedNode.GetValue("paramKey").ToString();

            ResponseParam param = paramList.Find(x => x.paramKey.Equals(paramKey));
            if (param != null)
            {
                paramPack = param.paramPack;
                seq = param.seq;
            }

            //Encoding
            byte[] basebyte2 = System.Text.Encoding.UTF8.GetBytes(dirName);
            string name = Convert.ToBase64String(basebyte2);

            bool bResult = ModifyDir(dirType, id, pid, name, paramPack, seq);
            if (bResult)
            {
                RefreshTree();
                treeList1.FocusedNode = treeList1.FindNodeByFieldValue("ID", this.focusedNodeId);
            }
        }

        private void cboParamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = sender as ComboBoxEdit;

            lblDuplicateKey.Visible = false;
                
            ResponseParam param = paramList.Find(x => x.paramKey.Equals(cbo.Text));

            string adamsKey = String.Empty;
            string zaeroKey = String.Empty;
            string grtKey = String.Empty;
            string fltpKey = String.Empty;
            string fltsKey = String.Empty;
            string propType = String.Empty;
            string propCode = String.Empty;
            string paramUnit = String.Empty;
            string partInfo = String.Empty;
            string partInfoSub = String.Empty;
            string lrpX = String.Empty;
            string lrpY = String.Empty;
            string lrpZ = String.Empty;

            if (param != null)
            {
                adamsKey = param.adamsKey;
                zaeroKey = param.zaeroKey;
                grtKey = param.grtKey;
                fltpKey = param.fltpKey;
                fltsKey = param.fltsKey;
                if (param.propInfo != null)
                {
                    propType = param.propInfo.propType;
                    propCode = param.propInfo.propCode;
                    paramUnit = param.propInfo.paramUnit;
                }
                partInfo = param.partInfo;
                partInfoSub = param.partInfoSub;
                lrpX = param.lrpX.ToString();
                lrpY = param.lrpY.ToString();
                lrpZ = param.lrpZ.ToString();
            }
            edtAdams.Text = adamsKey;
            edtZaero.Text = zaeroKey;
            edtGrt.Text = grtKey;
            edtFltp.Text = fltpKey;
            edtFlts.Text = fltsKey;
            cboPropertyType.Text = propType;
            cboPropertyCode.Text = propCode;
            cboUnit.Text = paramUnit;
            cboPart.Text = partInfo;
            cboPartLocation.Text = partInfoSub;
            edtLrpX.Text = lrpX;
            edtLrpY.Text = lrpY;
            edtLrpZ.Text = lrpZ;

            int height = EXTRA_HEIGHT * flowLayoutPanel1.Controls.Count;
            lblTag.Location = new Point(lblTag.Location.X, lblTag.Location.Y - height);
            edtTag.Location = new Point(edtTag.Location.X, edtTag.Location.Y - height);
            panelTag.Location = new Point(panelTag.Location.X, panelTag.Location.Y - height);
            btnModifyParameter.Location = new Point(btnModifyParameter.Location.X, btnModifyParameter.Location.Y - height);
            btnDeleteParameter.Location = new Point(btnDeleteParameter.Location.X, btnDeleteParameter.Location.Y - height);
            btnSaveAsNewParameter.Location = new Point(btnSaveAsNewParameter.Location.X, btnSaveAsNewParameter.Location.Y - height);
            flowLayoutPanel1.Height = FLOWLAYOUTPANEL1_HEIGHT;

            extraIndex = START_EXTRA_INDEX;

            MgmtLRPExtraControl[] controls = new MgmtLRPExtraControl[flowLayoutPanel1.Controls.Count];
            extraControlList.CopyTo(controls);
            foreach (MgmtLRPExtraControl ctrl in controls)
            {
                flowLayoutPanel1.Controls.Remove(ctrl);
                ctrl.Dispose();
                extraControlList.Remove(ctrl);
            }

            if (param != null)
            {
                foreach (KeyValuePair<string, string> pair in param.extras)
                {
                    AddExtra(pair.Key, pair.Value);
                }
            }

            ButtonEdit[] controlArray = new ButtonEdit[panelTag.Controls.Count];
            panelTag.Controls.CopyTo(controlArray, 0);

            foreach (ButtonEdit btn in controlArray)
            {
                panelTag.Controls.Remove(btn);
            }

            if (param != null)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(param.tags);
                string decName = Encoding.UTF8.GetString(byte64);

                string[] tags = decName.Split('|');
                foreach (string tag in tags)
                {
                    addTag(tag);
                }
            }
        }

        private void btnPropertyConfig_Click(object sender, EventArgs e)
        {
            PropertyConfigForm form = new PropertyConfigForm();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();

            InitializeProperty();
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
        private void removeTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit btn = sender as ButtonEdit;
            panelTag.Controls.Remove(btn);

        }

        private void btnAddExtra_ButtonClick(object sender, EventArgs e)
        {
            AddExtra();
        }
        private void treeList2_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = e.Node;
            lblDuplicateKey.Visible = false;

            if (node != null && node.GetValue("paramKey") != null)
            {
                string paramKey = node.GetValue("paramKey").ToString();
                edtParamKey.Text = paramKey;
                ResponseParam param = paramList.Find(x => x.paramKey.Equals(paramKey));

                string adamsKey = String.Empty;
                string zaeroKey = String.Empty;
                string grtKey = String.Empty;
                string fltpKey = String.Empty;
                string fltsKey = String.Empty;
                string propType = String.Empty;
                string propCode = String.Empty;
                string paramUnit = String.Empty;
                string partInfo = String.Empty;
                string partInfoSub = String.Empty;
                string lrpX = String.Empty;
                string lrpY = String.Empty;
                string lrpZ = String.Empty;

                if (param != null)
                {
                    adamsKey = param.adamsKey;
                    zaeroKey = param.zaeroKey;
                    grtKey = param.grtKey;
                    fltpKey = param.fltpKey;
                    fltsKey = param.fltsKey;
                    if (param.propInfo != null)
                    {
                        propType = param.propInfo.propType;
                        propCode = param.propInfo.propCode;
                        paramUnit = param.propInfo.paramUnit;
                    }
                    partInfo = param.partInfo;
                    partInfoSub = param.partInfoSub;
                    lrpX = param.lrpX.ToString();
                    lrpY = param.lrpY.ToString();
                    lrpZ = param.lrpZ.ToString();
                }
                edtAdams.Text = adamsKey;
                edtZaero.Text = zaeroKey;
                edtGrt.Text = grtKey;
                edtFltp.Text = fltpKey;
                edtFlts.Text = fltsKey;
                cboPropertyType.Text = propType;
                cboPropertyCode.Text = propCode;
                cboUnit.Text = paramUnit;
                cboPart.Text = partInfo;
                cboPartLocation.Text = partInfoSub;
                edtLrpX.Text = lrpX;
                edtLrpY.Text = lrpY;
                edtLrpZ.Text = lrpZ;

                int height = EXTRA_HEIGHT * flowLayoutPanel1.Controls.Count;
                lblTag.Location = new Point(lblTag.Location.X, lblTag.Location.Y - height);
                edtTag.Location = new Point(edtTag.Location.X, edtTag.Location.Y - height);
                panelTag.Location = new Point(panelTag.Location.X, panelTag.Location.Y - height);
                btnModifyParameter.Location = new Point(btnModifyParameter.Location.X, btnModifyParameter.Location.Y - height);
                btnDeleteParameter.Location = new Point(btnDeleteParameter.Location.X, btnDeleteParameter.Location.Y - height);
                btnSaveAsNewParameter.Location = new Point(btnSaveAsNewParameter.Location.X, btnSaveAsNewParameter.Location.Y - height);
                flowLayoutPanel1.Height = FLOWLAYOUTPANEL1_HEIGHT;

                extraIndex = START_EXTRA_INDEX;

                MgmtLRPExtraControl[] controls = new MgmtLRPExtraControl[flowLayoutPanel1.Controls.Count];
                extraControlList.CopyTo(controls);
                foreach (MgmtLRPExtraControl ctrl in controls)
                {
                    flowLayoutPanel1.Controls.Remove(ctrl);
                    ctrl.Dispose();
                    extraControlList.Remove(ctrl);
                }

                if (param != null)
                {
                    foreach (KeyValuePair<string, string> pair in param.extras)
                    {
                        AddExtra(pair.Key, pair.Value);
                    }
                }

                ButtonEdit[] controlArray = new ButtonEdit[panelTag.Controls.Count];
                panelTag.Controls.CopyTo(controlArray, 0);

                foreach (ButtonEdit btn in controlArray)
                {
                    panelTag.Controls.Remove(btn);
                }

                if (param != null)
                {
                    //Decoding
                    byte[] byte64 = Convert.FromBase64String(param.tags);
                    string decName = Encoding.UTF8.GetString(byte64);

                    string[] tags = decName.Split('|');
                    foreach (string tag in tags)
                    {
                        addTag(tag);
                    }
                }
            }
        }

        private void treeList2_RowClick(object sender, RowClickEventArgs e)
        {
            TreeListNode node = e.Node;
            lblDuplicateKey.Visible = false;

            if (node != null && node.GetValue("paramKey") != null)
            {
                string paramKey = node.GetValue("paramKey").ToString();
                edtParamKey.Text = paramKey;
                ResponseParam param = paramList.Find(x => x.paramKey.Equals(paramKey));
           
                string adamsKey = String.Empty;
                string zaeroKey = String.Empty;
                string grtKey = String.Empty;
                string fltpKey = String.Empty;
                string fltsKey = String.Empty;
                string propType = String.Empty;
                string propCode = String.Empty;
                string paramUnit = String.Empty;
                string partInfo = String.Empty;
                string partInfoSub = String.Empty;
                string lrpX = String.Empty;
                string lrpY = String.Empty;
                string lrpZ = String.Empty;

                if (param != null)
                {
                    adamsKey = param.adamsKey;
                    zaeroKey = param.zaeroKey;
                    grtKey = param.grtKey;
                    fltpKey = param.fltpKey;
                    fltsKey = param.fltsKey;
                    if (param.propInfo != null)
                    {
                        propType = param.propInfo.propType;
                        propCode = param.propInfo.propCode;
                        paramUnit = param.propInfo.paramUnit;
                    }
                    partInfo = param.partInfo;
                    partInfoSub = param.partInfoSub;
                    lrpX = param.lrpX.ToString();
                    lrpY = param.lrpY.ToString();
                    lrpZ = param.lrpZ.ToString();
                }
                edtAdams.Text = adamsKey;
                edtZaero.Text = zaeroKey;
                edtGrt.Text = grtKey;
                edtFltp.Text = fltpKey;
                edtFlts.Text = fltsKey;
                cboPropertyType.Text = propType;
                cboPropertyCode.Text = propCode;
                cboUnit.Text = paramUnit;
                cboPart.Text = partInfo;
                cboPartLocation.Text = partInfoSub;
                edtLrpX.Text = lrpX;
                edtLrpY.Text = lrpY;
                edtLrpZ.Text = lrpZ;

                int height = EXTRA_HEIGHT * flowLayoutPanel1.Controls.Count;
                lblTag.Location = new Point(lblTag.Location.X, lblTag.Location.Y - height);
                edtTag.Location = new Point(edtTag.Location.X, edtTag.Location.Y - height);
                panelTag.Location = new Point(panelTag.Location.X, panelTag.Location.Y - height);
                btnModifyParameter.Location = new Point(btnModifyParameter.Location.X, btnModifyParameter.Location.Y - height);
                btnDeleteParameter.Location = new Point(btnDeleteParameter.Location.X, btnDeleteParameter.Location.Y - height);
                btnSaveAsNewParameter.Location = new Point(btnSaveAsNewParameter.Location.X, btnSaveAsNewParameter.Location.Y - height);
                flowLayoutPanel1.Height = FLOWLAYOUTPANEL1_HEIGHT;

                extraIndex = START_EXTRA_INDEX;

                MgmtLRPExtraControl[] controls = new MgmtLRPExtraControl[flowLayoutPanel1.Controls.Count];
                extraControlList.CopyTo(controls);
                foreach (MgmtLRPExtraControl ctrl in controls)
                {
                    flowLayoutPanel1.Controls.Remove(ctrl);
                    ctrl.Dispose();
                    extraControlList.Remove(ctrl);
                }

                if (param != null)
                {
                    foreach (KeyValuePair<string, string> pair in param.extras)
                    {
                        AddExtra(pair.Key, pair.Value);
                    }
                }

                ButtonEdit[] controlArray = new ButtonEdit[panelTag.Controls.Count];
                panelTag.Controls.CopyTo(controlArray, 0);

                foreach (ButtonEdit btn in controlArray)
                {
                    panelTag.Controls.Remove(btn);
                }

                if (param != null)
                {
                    //Decoding
                    byte[] byte64 = Convert.FromBase64String(param.tags);
                    string decName = Encoding.UTF8.GetString(byte64);

                    string[] tags = decName.Split('|');
                    foreach (string tag in tags)
                    {
                        addTag(tag);
                    }
                }
            }

        }

        #endregion EventHandler

    }
}
