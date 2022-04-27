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

        public MgmtParameterControl()
        {
            InitializeComponent();
        }

        private void MgmtParameterControl_Load(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = 250;
            cboProperty.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboUnit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboPart.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboPartLocation.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cboAirplane.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;


            InitializeParameterDataList();
        }

        private void InitializeParameterDataList()
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
            treeList1.DataSource = GetParamList();
            //The treelist automatically creates columns for the public fields found in the data source. 
            //You do not need to call the TreeList.PopulateColumns method unless the treeList1.OptionsBehavior.AutoPopulatefColumns option is disabled.
            treeList1.ForceInitialize();

            treeList1.RowHeight = 23;
            treeList1.OptionsView.ShowColumns = false;
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

            //Hide the key columns. An end-user can access them from the Customization Form.
            treeList1.Columns[treeList1.KeyFieldName].Visible = false;
            treeList1.Columns[treeList1.ParentFieldName].Visible = false;
            treeList1.Columns["DirType"].Visible = false;

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
                DXMenuItem menuItemModify = new DXMenuItem("Modify Name", this.modifyNodeMenuItemClick);
                menuItemModify.Tag = node;
                e.Menu.Items.Add(menuItemModify);

                // Create the Delete Node command.
                DXMenuItem menuItemDelete = new DXMenuItem("Delete Node", this.deleteNodeMenuItemClick);
                menuItemDelete.Tag = node;
                e.Menu.Items.Add(menuItemDelete);
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
            if(node == null)
                bResult = CreateParameter("folder", name, "0");
            else
                bResult = CreateParameter("folder", name, node.GetValue("ID").ToString());

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
            if(node == null)
                bResult = CreateParameter("param", name, "0");
            else
                bResult = CreateParameter("param", name, node.GetValue("ID").ToString());

            if (bResult)
            {
                //TreeListNode newNode = treeList1.AppendNode(new object[] { "" }, node);
                //newNode.SetValue("DirName", paramName);

                ////treeList1.ExpandAll();
                //node.Expand();
                ////treeList1.Selection.Set(newNode);
                //treeList1.FocusedNode = newNode;

                RefreshTree();
                treeList1.FocusedNode = treeList1.FindNodeByFieldValue("ID", this.focusedNodeId);
            }
        }

        private void RefreshTree()
        {
            treeList1.DataSource = null;
            treeList1.DataSource = GetParamList();
            //The treelist automatically creates columns for the public fields found in the data source. 
            //You do not need to call the TreeList.PopulateColumns method unless the treeList1.OptionsBehavior.AutoPopulatefColumns option is disabled.
            treeList1.ForceInitialize();

            treeList1.ExpandAll();
        }

        private void modifyNodeMenuItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null) return;
            TreeListNode node = item.Tag as TreeListNode;
            if (node == null) return;

            string dirName = Prompt.ShowDialog("New Name", "Modify Name");

            if (string.IsNullOrEmpty(dirName))
            {
                MessageBox.Show(Properties.Resources.InputParameterName, Properties.Resources.StringWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Encoding
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(dirName);
            string name = Convert.ToBase64String(basebyte);

            bool bResult = ModifyParameter(node.GetValue("DirType").ToString(), node.GetValue("ID").ToString(), node.GetValue("ParentID").ToString(), name);
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

            if(MessageBox.Show(Properties.Resources.StringDelete, Properties.Resources.StringConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            bool bResult = DeleteParameter(node.GetValue("ID").ToString());
            if (bResult)
            {
                RefreshTree();
            }
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            treeList1.PostEditor();
        }

        private bool CreateParameter(string dirType, string name, string pid)
        {
            string url = ConfigurationManager.AppSettings["UrlParameter"];
            string sendData = string.Format(@"
            {{ ""command"":""add"",
            ""seq"":""1"",
            ""parentDirSeq"":""{0}"",
            ""dirName"":""{1}"",
            ""dirType"":""{2}"",
            ""dirIcon"":"""",
            ""refSeq"":"""",
            ""refSubSeq"":""""
            }}"
            , pid, name, dirType);

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
            ParameterJsonData result = JsonConvert.DeserializeObject<ParameterJsonData>(responseText);

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

        private bool ModifyParameter(string dirType, string id, string pid, string name)
        {
            string url = ConfigurationManager.AppSettings["UrlParameter"];
            string sendData = string.Format(@"
            {{ ""command"":""modify"",
            ""seq"":""{0}"",
            ""parentDirSeq"":""{1}"",
            ""dirName"":""{2}"",
            ""dirType"":""{3}"",
            ""dirIcon"":"""",
            ""refSeq"":"""",
            ""refSubSeq"":""""
            }}"
            , id, pid, name, dirType);

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
            ParameterJsonData result = JsonConvert.DeserializeObject<ParameterJsonData>(responseText);

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

        private bool DeleteParameter(string id)
        {
            string url = ConfigurationManager.AppSettings["UrlParameter"];
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
            ParameterJsonData result = JsonConvert.DeserializeObject<ParameterJsonData>(responseText);

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
        private BindingList<ParameterData> GetParamList()
        {
            BindingList<ParameterData> list = new BindingList<ParameterData>();

            string url = ConfigurationManager.AppSettings["UrlParameter"];
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
            ParameterJsonData result = JsonConvert.DeserializeObject<ParameterJsonData>(responseText);
            //object result = JsonConvert.DeserializeObject(responseText);

            foreach (Pool pool in result.response.pools)
            {
                byte[] byte64 = Convert.FromBase64String(pool.dirName);
                string name = Encoding.UTF8.GetString(byte64);

                list.Add(new ParameterData(pool.seq, pool.parentDirSeq, pool.dirType, name));
            }

            return list;

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

            if(e.Node.GetValue("DirType").ToString().Equals("folder"))
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

    }
}
