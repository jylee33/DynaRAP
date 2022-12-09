using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
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
    public partial class ImportDeleteControl : DevExpress.XtraEditors.XtraUserControl
    {
        private int beforeSelectIndex = -1;
        private List<UploadData> uploadDataList = null;

        public ImportDeleteControl()
        {
            InitializeComponent();
        }

        private void ImportDeleteControl_Load(object sender, EventArgs e)
        {
            InitGridControl1();
        }

        private void InitGridControl1()
        {

            GridColumn colType = gridView1.Columns["dataType"];
            colType.OptionsColumn.FixedWidth = true;
            colType.Width = 80;
            colType.Caption = "타입";
            colType.OptionsColumn.ReadOnly = true;

            GridColumn colDel = gridView1.Columns["Delete"];
            colDel.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.OptionsColumn.FixedWidth = true;
            colDel.Width = 40;
            colDel.Caption = "삭제";
            colDel.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;
            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;
            
            GetUploadData();

            
        }


        public void GetUploadData()
        {
            string sendData = string.Format(@"
                {{
                ""command"":""upload-list""
                }}");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlImport"], sendData);
            if (responseData != null)
            {
                UploadDataResponse uploadDataResponse = JsonConvert.DeserializeObject<UploadDataResponse>(responseData);
                if (uploadDataResponse.response != null)
                {
                    foreach (var list in uploadDataResponse.response)
                    {
                        list.Delete = 1;
                        list.uploadName = Utils.base64StringDecoding(list.uploadName);
                    }
                    uploadDataList = uploadDataResponse.response;
                }
            }
            this.gridControl1.DataSource = uploadDataList;
        }
        
        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            UploadData uploadData = (UploadData)gridView1.GetFocusedRow();

            string sendData = string.Format(@"
                {{
                ""command"":""remove-upload"",
                ""uploadSeq"":""{0}""
                }}", uploadData.seq);
            string responseData = Utils.GetPostDataNew(ConfigurationManager.AppSettings["UrlImport"], sendData);
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    MessageBox.Show("삭제 성공"); 
                    GetUploadData();
                }
            }
        }

    }
}
