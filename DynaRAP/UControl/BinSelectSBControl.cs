using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DynaRAP.Data;
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
    public partial class BinSelectSBControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public BinSelectSBControl()
        {
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void SelectSBControl_Load(object sender, EventArgs e)
        {
            InitializeFlyingDataList();
        }

        private void InitializeFlyingDataList()
        {
            //TreeList treeList1 = new TreeList();
            treeList1.Parent = this;
            treeList1.Dock = DockStyle.Fill;
            //Specify the fields that arrange underlying data as a hierarchy.
            treeList1.KeyFieldName = "ID";
            treeList1.ParentFieldName = "ParentID";
            //Allow the treelist to create columns bound to the fields the KeyFieldName and ParentFieldName properties specify.
            treeList1.OptionsBehavior.PopulateServiceColumns = true;

            //Specify the data source.
            //treeList1.DataSource = null;
            treeList1.DataSource = GetSBList();
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
            treeList1.OptionsView.AutoWidth = false;

            treeList1.OptionsFilter.AllowFilterEditor = false;

            //Access the automatically created columns.
            TreeListColumn colName = treeList1.Columns["FlyingName"];
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

            treeList1.ExpandAll();

            //Calculate the optimal column widths after the treelist is shown.
            this.BeginInvoke(new MethodInvoker(delegate
            {
                treeList1.BestFitColumns();
            }));



        }

        private void treeList1_CellValueChanged(object sender, CellValueChangedEventArgs e)
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
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            treeList1.PostEditor();
        }

        private BindingList<FlyingData> GetSBList()
        {
            BindingList<FlyingData> list = new BindingList<FlyingData>();
            ////list.Add(new FlyingData(0, -1, "ImportModuleScenarioName", null));
            //list.Add(new FlyingData(1, 0, "비행데이터", null));
            //list.Add(new FlyingData(2, 0, "버펫팅데이터", null));
            //list.Add(new FlyingData(3, 0, "Short Block", null));
            list.Add(new FlyingData(1, 0, "Short Block", null));

            try
            {
                string url = ConfigurationManager.AppSettings["UrlShortBlock"];

                string sendData = string.Format(@"
                {{
                ""command"":""list"",
                ""registerUid"":"""",
                ""partSeq"":"""",
                ""pageNo"":1,
                ""pageSize"":3000
                }}"
                );

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
                SBListResponse result = JsonConvert.DeserializeObject<SBListResponse>(responseText);

                if (result != null)
                {
                    if (result.code != 200)
                    {
                        return null;
                    }
                    else
                    {
                        foreach (ResponseSBList sbList in result.response)
                        {
                            FlyingData data = new FlyingData();
                            data.ParentID = 1;

                            //Decoding
                            byte[] byte64 = Convert.FromBase64String(sbList.blockName);
                            string decName = Encoding.UTF8.GetString(byte64);

                            data.FlyingName = decName;
                            data.Check = false;
                            list.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return list;
        }
    }
}
