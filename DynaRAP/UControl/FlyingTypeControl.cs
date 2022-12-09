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
    public partial class FlyingTypeControl : DevExpress.XtraEditors.XtraUserControl
    {
        private int beforeSelectIndex = -1;
        private List<FlyingType> flyingTypeList = null;

        public FlyingTypeControl()
        {
            InitializeComponent();
        }

        private void FlyingTypeControl_Load(object sender, EventArgs e)
        {
            InitGridControl1();
        }

        private void InitGridControl1()
        {

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
            
            GetFlyingType();

           
        }


        public void GetFlyingType()
        {
            string sendData = string.Format(@"
                {{
                ""command"":""flight-type""
                }}");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlImport"], sendData);
            if (responseData != null)
            {
                FlyingTpyeResponse flyingTpyeResponse = JsonConvert.DeserializeObject<FlyingTpyeResponse>(responseData);
                if (flyingTpyeResponse.response != null)
                {
                    foreach (var list in flyingTpyeResponse.response)
                    {
                        list.Delete = 1;
                        list.typeName = Utils.base64StringDecoding(list.typeName);
                    }
                    flyingTypeList = flyingTpyeResponse.response;
                }
            }
            if(flyingTypeList == null)
            {
                flyingTypeList = new List<FlyingType>();
            }
            this.gridControl1.DataSource = flyingTypeList;
        }
        
        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            FlyingType flyingType = (FlyingType)gridView1.GetFocusedRow();
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(flyingType.typeName);
            string encName = Convert.ToBase64String(basebyte);

            string sendData = string.Format(@"
                {{
                ""command"":""remove-flight-type"",
                ""typeCode"" : ""{0}"",
                ""typeName"" : ""{1}""
                }}" , flyingType.typeCode, encName);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlImport"], sendData);
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    MessageBox.Show("삭제 성공");
                    GetFlyingType();
                }
                else
                {
                    MessageBox.Show("삭제 실패");
                }
            }
        }

        private void btnListSave_Click(object sender, EventArgs e)
        {
            FlyingTpyeRequest flyingTpyeRequest = new FlyingTpyeRequest();
            flyingTpyeRequest.command = "save-flight-type";
            List<FlyingType> flyingTypeList = (List<FlyingType>)this.gridControl1.DataSource;
            foreach (FlyingType flyingType in flyingTypeList)
            {
                flyingType.typeCode = flyingType.typeName;
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(flyingType.typeName);
                string encName = Convert.ToBase64String(basebyte);
                flyingType.typeName = encName;
            }
            flyingTpyeRequest.flightTypes = flyingTypeList;
            
            var json = JsonConvert.SerializeObject(flyingTpyeRequest);
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlImport"], json);
            if (responseData != null)
            {
                FlyingTpyeResponse flyingTpyeResponse = JsonConvert.DeserializeObject<FlyingTpyeResponse>(responseData);
                if (flyingTpyeResponse.code == 200)
                {
                    MessageBox.Show("저장 성공");
                    GetFlyingType();
                }
                else
                {
                    MessageBox.Show("저장 실패");
                }
            }
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            FlyingType flyingType = (FlyingType)gridView1.GetFocusedRow();
            flyingTypeList.Remove(flyingType);
            this.gridControl1.DataSource = flyingTypeList;
            this.gridView1.RefreshData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FlyingType flyingType = new FlyingType();
            flyingType.Delete = 1;
            if(flyingTypeList == null)
            {
                flyingTypeList = new List<FlyingType>();
            }
            flyingTypeList.Add(flyingType);
            this.gridControl1.DataSource = flyingTypeList;
            this.gridView1.RefreshData();
        }
    }
}
