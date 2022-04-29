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
    public partial class MgmtPresetControl : DevExpress.XtraEditors.XtraUserControl
    {
        int focusedNodeId = 0;
        TreeListNode focusedNode = null;
        List<ResponsePreset> presetList = null;

        public MgmtPresetControl()
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

        private void InitializePresetList()
        {
            cboPresetList.Properties.Items.Clear();

            presetList = GetPresetList();

            foreach (ResponsePreset list in presetList)
            {
                cboPresetList.Properties.Items.Add(list.presetPack);
            }
            cboPresetList.SelectedIndex = -1;

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

                if (pool.dirType.Equals("folder") || pool.dirType.Equals("preset"))
                {
                    list.Add(new DirData(pool.seq, pool.parentDirSeq, pool.dirType, name, pool.refSeq, pool.refSubSeq));
                }
            }

            return list;

        }

        private List<ResponsePreset> GetPresetList()
        {
            string url = ConfigurationManager.AppSettings["UrlPreset"];
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
            ListPresetJsonData result = JsonConvert.DeserializeObject<ListPresetJsonData>(responseText);

            return result.response;

        }

        private bool AddModParameter(string opType, string presetPack = "")
        {
            ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(presetPack));
            string paramName = edtParamName.Text;

            if (string.IsNullOrEmpty(paramName))
            {
                lblMandatoryField.Visible = true;
                MessageBox.Show("Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                lblMandatoryField.Visible = false;
            }

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(paramName);
            string encName = Convert.ToBase64String(basebyte);

            string url = ConfigurationManager.AppSettings["UrlPreset"];
            string sendData = string.Format(@"
            {{""command"":""{0}"",
            ""seq"":"""",
            ""presetPack"":""{1}"",
            ""presetName"":""{2}"",
            ""presetPackFrom"":""""
            }}"
            , opType, presetPack, encName);

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
            AddPresetJsonData result = JsonConvert.DeserializeObject<AddPresetJsonData>(responseText);

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

        private bool RemoveParam(string presetPack)
        {
            string url = ConfigurationManager.AppSettings["UrlPreset"];
            string sendData = string.Format(@"
            {{
            ""command"":""remove"",
            ""presetPack"":""{0}""
            }}"
            , presetPack);

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
        private void MgmtPresetControl_Load(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = 250;
            cboPresetList.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboProperty.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboUnit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboPart.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboPartLocation.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboAirplane.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

            btnAddParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddParameter.Properties.AllowFocused = false;

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

            InitializePresetList();
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

            string presetName = Prompt.ShowDialog("Preset Name", "Add Preset");

            if (string.IsNullOrEmpty(presetName))
            {
                MessageBox.Show(Properties.Resources.InputParameterName, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(presetName);
            string name = Convert.ToBase64String(basebyte);

            bool bResult = false;
            if (node == null)
                bResult = AddDir("preset", name, "0");
            else
                bResult = AddDir("preset", name, node.GetValue("ID").ToString());

            if (bResult)
            {
                //TreeListNode newNode = treeList1.AppendNode(new object[] { "" }, node);
                //newNode.SetValue("DirName", presetName);

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

            // Create the Add Preset command.
            DXMenuItem menuItemAdd = new DXMenuItem("Add Preset", this.addParamMenuItemClick);
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

                ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(node.GetValue("RefSeq").ToString()));
                if (preset == null)
                    this.cboPresetList.SelectedIndex = -1;
                else
                {
                    this.cboPresetList.Text = preset.presetPack;
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

                ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(node.GetValue("RefSeq").ToString()));
                if (preset == null)
                    this.cboPresetList.SelectedIndex = -1;
                else
                {
                    this.cboPresetList.Text = preset.presetPack;
                }
            }
        }

        private void btnModifyPreset_Click(object sender, EventArgs e)
        {
            string presetKey = cboPresetList.Text;
            ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(presetKey));

            if (preset != null)
            {
                bool bResult = AddModParameter("modify", preset.presetPack);

                if (bResult)
                {
                    MessageBox.Show(Properties.Resources.SuccessModify, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializePresetList();
                    cboPresetList.Text = presetKey;
                }
            }
        }

        private void btnDeletePreset_Click(object sender, EventArgs e)
        {
            string msg = string.Format(Properties.Resources.StringDeletePreset, cboPresetList.Text);
            if (MessageBox.Show(msg, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(cboPresetList.Text));

            if (preset != null)
            {
                bool bResult = RemoveParam(preset.presetPack);

                if (bResult)
                {
                    MessageBox.Show(Properties.Resources.SuccessRemove, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializePresetList();
                }
            }

        }

        private void btnSaveAsNewPreset_Click(object sender, EventArgs e)
        {
            //string presetKey = Prompt.ShowDialog("Preset Key", "New Preset");

            //if (string.IsNullOrEmpty(presetKey))
            //{
            //    MessageBox.Show(Properties.Resources.InputParameterKey, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            bool bResult = AddModParameter("add", "");

            if (bResult)
            {
                MessageBox.Show(Properties.Resources.SuccessAdd, Properties.Resources.StringSuccess, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                InitializePresetList();
                //cboPresetList.Text = "";
                cboPresetList.SelectedIndex = -1;
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

            ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(cboPresetList.Text));
            if (preset != null)
            {
                paramPack = preset.presetPack;
                seq = preset.seq;
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

        private void cboPresetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = sender as ComboBoxEdit;

            lblMandatoryField.Visible = false;

            ResponsePreset preset = presetList.Find(x => x.presetPack.Equals(cbo.Text));

            string presetName = String.Empty;

            if (preset != null)
            {
                //Decoding
                byte[] byte64 = Convert.FromBase64String(preset.presetName);
                string decName = Encoding.UTF8.GetString(byte64);

                presetName = decName;
            }
            edtParamName.Text = presetName;
        }

        private void btnAddParameter_Click(object sender, EventArgs e)
        {

        }

        #endregion EventHandler

    }
}
