using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
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

namespace DynaRAP.UControl
{
    public partial class MgmtParameterControl : DevExpress.XtraEditors.XtraUserControl
    {
        int focusedNodeId = 0;
        TreeListNode focusedNode = null;
        List<ResponseParam> paramList = null;

        public MgmtParameterControl()
        {
            InitializeComponent();
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
            treeList1.OptionsView.ShowIndicator = false;
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
            cboParamList.Properties.Items.Clear();

            paramList = GetParamList();

            foreach (ResponseParam list in paramList)
            {
                cboParamList.Properties.Items.Add(list.paramKey);
            }
            cboParamList.SelectedIndex = -1;

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
            string url = ConfigurationManager.AppSettings["UrlDir"];
            string sendData = string.Format(@"
            {{ ""command"":""add"",
            ""seq"":""{0}"",
            ""parentDirSeq"":""{1}"",
            ""dirName"":""{2}"",
            ""dirType"":""{3}"",
            ""dirIcon"":"""",
            ""refSeq"":""0"",
            ""refSubSeq"":""0""
            }}"
            , 1, pid, name, dirType);

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
            return true;
        }

        private bool ModifyDir(string dirType, string id, string pid, string name, string paramPack, string seq)
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
            return true;
        }

        private bool RemoveDir(string id)
        {
            string url = ConfigurationManager.AppSettings["UrlDir"];
            string sendData = string.Format(@"
            {{""command"":""remove"",
            ""seq"":""{0}""
            }}
            ", id);

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
            return true;
        }
        
        private BindingList<DirData> GetDirList()
        {
            BindingList<DirData> list = new BindingList<DirData>();

            string url = ConfigurationManager.AppSettings["UrlDir"];
            string sendData = "{ \"command\": \"list\" }";

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

            return list;

        }

        private List<ResponseParam> GetParamList()
        {
            string url = ConfigurationManager.AppSettings["UrlParam"];
            string sendData = @"
            {
            ""command"":""list"",
            ""pageNo"":1,
            ""pageSize"":3000
            }";

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
            ListParamJsonData result = JsonConvert.DeserializeObject<ListParamJsonData>(responseText);

            return result.response;

        }

        private bool AddModParameter(string opType, string paramKey, string paramPack = "")
        {
            ResponseParam param = paramList.Find(x => x.paramKey.Equals(paramKey));
            if(param == null || opType.Equals("modify"))
            {
                lblDuplicateKey.Visible = false;
            }
            else
            {
                lblDuplicateKey.Visible = true;
                MessageBox.Show("Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(edtParamName.Text);
            string encName = Convert.ToBase64String(basebyte);

            string url = ConfigurationManager.AppSettings["UrlParam"];
            string sendData = string.Format(@"
            {{
            ""command"":""{0}"",
            ""seq"":"""",
            ""paramPack"":""{1}"",
            ""paramGroupSeq"":"""",
            ""paramName"":""{2}"",
            ""paramKey"":""{3}"",
            ""paramSpec"":""{4}"",
            ""adamsKey"":""{5}"",
            ""zaeroKey"":""{6}"",
            ""grtKey"":""{7}"",
            ""fltpKey"":""{8}"",
            ""fltsKey"":""{9}"",
            ""partInfo"":""{10}"",
            ""partInfoSub"":""{11}"",
            ""lrpX"":""{12}"",
            ""lrpY"":""{13}"",
            ""lrpZ"":""{14}"",
            ""paramUnit"":""{15}"",
            ""domainMin"":""{16}"",
            ""domainMax"":""{17}"",
            ""specified"":""{18}"",
            ""paramVal"":null
            }}"
            , opType, paramPack, encName, paramKey, cboProperty.Text, edtAdams.Text
            , edtZaero.Text, edtGrt.Text, edtFltp.Text, edtFlts.Text
            , cboPart.Text, cboPartLocation.Text
            , edtLrpX.Text, edtLrpY.Text, edtLrpZ.Text
            , cboUnit.Text, edtMinumum.Text, edtMaximum.Text
            , edtSpecialValue.Text);

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
            return true;
        }

        private bool RemoveParam(string paramPack)
        {
            string url = ConfigurationManager.AppSettings["UrlParam"];
            string sendData = string.Format(@"
            {{
            ""command"":""remove"",
            ""paramPack"":""{0}""
            }}"
            , paramPack);

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
            return true;
        }

        #endregion Method

        #region EventHandler
        private void MgmtParameterControl_Load(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = 250;
            cboParamList.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboProperty.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboUnit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboPart.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboPartLocation.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboAirplane.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

#if DEBUG
            //edtKey.Text = "Bending, LH Wing BL1870";
            //edtAdams.Text = "112102";
            //edtZaero.Text = "112102";
            //edtGrt.Text = "SW921P";
            //edtFltp.Text = "SW921P";
            //edtFlts.Text = "SW921S";
            //cboProperty.Text = "BM";
            //cboUnit.Text = "N-mm";
            //cboPart.Text = "Wing";
            //cboPartLocation.Text = "Left";
            //cboAirplane.Text = "A2/3/6";
#endif

            InitializeParamList();
            InitializeDirDataList();
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
                    this.cboParamList.SelectedIndex = -1;
                else
                {
                    this.cboParamList.Text = param.paramKey;
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
                    this.cboParamList.SelectedIndex = -1;
                else
                {
                    this.cboParamList.Text = param.paramKey;
                }
            }
        }

        private void btnModifyParameter_Click(object sender, EventArgs e)
        {
            string paramKey = cboParamList.Text;
            ResponseParam param = paramList.Find(x => x.paramKey.Equals(paramKey));

            if (param != null)
            {
                bool bResult = AddModParameter("modify", paramKey, param.paramPack);

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
            string msg = string.Format(Properties.Resources.StringDeleteParameter, cboParamList.Text);
            if (MessageBox.Show(msg, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ResponseParam param = paramList.Find(x => x.paramKey.Equals(cboParamList.Text));

            if(param != null)
            {
                bool bResult = RemoveParam(param.paramPack);

                if (bResult)
                {
                    MessageBox.Show(Properties.Resources.SuccessRemove, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeParamList();
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
            bool bResult = AddModParameter("add", paramKey);

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

            ResponseParam param = paramList.Find(x => x.paramKey.Equals(cboParamList.Text));
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

            string paramName = String.Empty;
            string adamsKey = String.Empty;
            string zaeroKey = String.Empty;
            string grtKey = String.Empty;
            string fltpKey = String.Empty;
            string fltsKey = String.Empty;
            string paramSpec = String.Empty;
            string paramUnit = String.Empty;
            string partInfo = String.Empty;
            string partInfoSub = String.Empty;
            string lrpX = String.Empty;
            string lrpY = String.Empty;
            string lrpZ = String.Empty;
            string airplane = String.Empty;
            string domainMax = String.Empty;
            string domainMin = String.Empty;
            string specified = String.Empty;

            if (param != null)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(param.paramName);
                string decName = Encoding.UTF8.GetString(byte64);

                paramName =decName;
                adamsKey = param.adamsKey;
                zaeroKey = param.zaeroKey;
                grtKey = param.grtKey;
                fltpKey = param.fltpKey;
                fltsKey = param.fltsKey;
                paramSpec = param.paramSpec;
                paramUnit = param.paramUnit;
                partInfo = param.partInfo;
                partInfoSub = param.partInfoSub;
                lrpX = param.lrpX.ToString();
                lrpY = param.lrpY.ToString();
                lrpZ = param.lrpZ.ToString();
                airplane = "";
                domainMax = param.domainMax.ToString();
                domainMin = param.domainMin.ToString();
                specified = param.specified.ToString();
            }
            edtParamName.Text = paramName;
            edtAdams.Text = adamsKey;
            edtZaero.Text = zaeroKey;
            edtGrt.Text = grtKey;
            edtFltp.Text = fltpKey;
            edtFlts.Text = fltsKey;
            cboProperty.Text = paramSpec;
            cboUnit.Text = paramUnit;
            cboPart.Text = partInfo;
            cboPartLocation.Text = partInfoSub;
            edtLrpX.Text = lrpX;
            edtLrpY.Text = lrpY;
            edtLrpZ.Text = lrpZ;
            cboAirplane.Text = airplane;
            edtMaximum.Text = domainMax;
            edtMinumum.Text = domainMin;
            edtSpecialValue.Text = specified;
        }

        #endregion EventHandler

    }
}
