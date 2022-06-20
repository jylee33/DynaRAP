using DevExpress.XtraEditors;
using DynaRAP.Data;
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

namespace DynaRAP.Forms
{
    public partial class AddDllForm : DevExpress.XtraEditors.XtraForm
    {
        DllResponse response = null;

        public DllResponse Response { get => response; set => response = value; }

        public AddDllForm()
        {
            InitializeComponent();
        }

        private void AddDllForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            response = AddDll();

            if (response.code == 200)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(response.message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private DllResponse AddDll()
        {
            DllResponse result = null;
            try
            {
                //Encoding
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(edtDataSetName.Text);
                string encName = Convert.ToBase64String(basebyte);

                string url = ConfigurationManager.AppSettings["UrlDLL"];

                string sendData = string.Format(@"
                {{
                ""command"":""add"",
                ""dataSetCode"":""{0}"",
                ""dataSetName"":""{1}"",
                ""dataVersion"":""{2}""
                }}"
                , edtDataSetCode.Text, encName, edtDataSetVersion.Text);

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
                result = JsonConvert.DeserializeObject<DllResponse>(responseText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }
    }
}