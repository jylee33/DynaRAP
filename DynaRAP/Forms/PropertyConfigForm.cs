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
    public partial class PropertyConfigForm : DevExpress.XtraEditors.XtraForm
    {
        List<ResponsePropList> propList = new List<ResponsePropList>();
        string selPropertyType = string.Empty;

        public PropertyConfigForm()
        {
            InitializeComponent();
        }

        private void PropertyConfigForm_Load(object sender, EventArgs e)
        {
            cboPropertyType.SelectedIndexChanged += CboPropertyType_SelectedIndexChanged;
            cboPropertyCode.SelectedIndexChanged += CboPropertyCode_SelectedIndexChanged;

            InitializeProperty();

        }

        private void CboPropertyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit combo = sender as ComboBoxEdit;

            if (combo != null)
            {
                foreach (ResponsePropList prop in propList)
                {
                    if (prop.propType.Equals(selPropertyType) == false)
                    {
                        continue;
                    }
                    if (prop.propCode.Equals(combo.Text))
                    {
                        cboUnit.Text = prop.paramUnit;
                    }
                }
            }
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string propType = cboPropertyType.Text;
            string propCode = cboPropertyCode.Text;
            string unit = cboUnit.Text;

            if(string.IsNullOrEmpty(propType) || string.IsNullOrEmpty(propCode) || string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("항목을 입력하세요");
                return;
            }

            bool bResult = PropAdd(propType, propCode, unit);

            if(bResult)
            {
                MessageBox.Show("추가 성공");
                InitializeProperty();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            string propType = cboPropertyType.Text;
            string propCode = cboPropertyCode.Text;
            string unit = cboUnit.Text;
            string seq = string.Empty;

            if (string.IsNullOrEmpty(propType) || string.IsNullOrEmpty(propCode) || string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("항목을 입력하세요");
                return;
            }

            ResponsePropList prop = propList.Find(x => x.propType.Equals(propType) && x.propCode.Equals(propCode));
            if (prop == null)
            {
                MessageBox.Show("수정할 항목이 없습니다.");
                return;
            }
            else
            {
                seq = prop.seq;
            }

            bool bResult = PropModify(seq, propType, propCode, unit);

            if (bResult)
            {
                MessageBox.Show("수정 성공");
                InitializeProperty();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string propType = cboPropertyType.Text;
            string propCode = cboPropertyCode.Text;
            string unit = cboUnit.Text;
            string seq = string.Empty;

            if (string.IsNullOrEmpty(propType) || string.IsNullOrEmpty(propCode) || string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("항목을 입력하세요");
                return;
            }

            ResponsePropList prop = propList.Find(x => x.propType.Equals(propType) && x.propCode.Equals(propCode));
            if (prop == null)
            {
                MessageBox.Show("삭제할 항목이 없습니다.");
                return;
            }
            else
            {
                seq = prop.seq;
            }

            bool bResult = PropModify(seq, propType, propCode, unit, true);

            if (bResult)
            {
                MessageBox.Show("삭제 성공");
                InitializeProperty();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeProperty()
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
            cboPropertyType.SelectedIndex = -1;
        }

        private List<ResponsePropList> GetPropertyInfo()
        {
            PropListResponse result = null;

            try
            {
                BindingList<DirData> list = new BindingList<DirData>();

                string url = ConfigurationManager.AppSettings["UrlParam"];
                string sendData = @"
                {
                ""command"":""prop-list"",
                ""propType"":""""
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
                result = JsonConvert.DeserializeObject<PropListResponse>(responseText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result.response;

        }

        private bool PropAdd(string propType, string propCode, string unit)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlParam"];
                string sendData = string.Format(@"
                {{ ""command"":""prop-add"",
                    ""propType"":""{0}"",
                    ""propCode"":""{1}"",
                    ""paramUnit"":""{2}""
                }}"
                , propType, propCode, unit);

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
                PropAddResponse result = JsonConvert.DeserializeObject<PropAddResponse>(responseText);

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
                MessageBox.Show(ex.Message);
            }

            return true;
        }

        private bool PropModify(string seq, string propType, string propCode, string unit, bool deleted = false)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["UrlParam"];
                string sendData = string.Format(@"
                {{ ""command"":""prop-modify"",
                ""seq"":""{0}"",
                ""propType"":""{1}"",
                ""propCode"":""{2}"",
                ""paramUnit"":""{3}"",
                ""deleted"":{4}
                }}"
                , seq, propType, propCode, unit, deleted);

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
                PropAddResponse result = JsonConvert.DeserializeObject<PropAddResponse>(responseText);

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
                MessageBox.Show(ex.Message);
            }

            return true;
        }

    }
}