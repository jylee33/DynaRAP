using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.TEST
{
    public partial class TestWebForm : DevExpress.XtraEditors.XtraForm
    {
        public TestWebForm()
        {
            InitializeComponent();
        }

        private void TestWebForm_Load(object sender, EventArgs e)
        {
            //test1();
            //test2();
            //test3();
        }

        private void test1()
        {
            string url = "https://httpbin.org/get";  //테스트 사이트
            string responseText = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 30 * 1000; // 30초
            request.Headers.Add("Authorization", "BASIC SGVsbG8="); // 헤더 추가 방법

            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                HttpStatusCode status = resp.StatusCode;
                Console.WriteLine(status);  // 정상이면 "OK"

                Stream respStream = resp.GetResponseStream();
                using (StreamReader sr = new StreamReader(respStream))
                {
                    responseText = sr.ReadToEnd();
                }
            }

            Console.WriteLine(responseText);
        }

        private void test2()
        {
            string url = "https://httpbin.org/post";
            string data = "{ \"id\": \"101\", \"name\" : \"Alex\" }";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 30 * 1000;
            //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

            // POST할 데이타를 Request Stream에 쓴다
            byte[] bytes = Encoding.ASCII.GetBytes(data);
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
        }

        private void test3()
        {
            string url = "http://httpbin.org/image/png";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            // Response 바이너리 데이타 처리. 이미지 파일로 저장.                        
            using (WebResponse resp = request.GetResponse())
            {
                var buff = new byte[1024];
                int pos = 0;
                int count;

                using (Stream stream = resp.GetResponseStream())
                {
                    using (var fs = new FileStream("image.png", FileMode.Create))
                    {
                        do
                        {
                            count = stream.Read(buff, pos, buff.Length);
                            fs.Write(buff, 0, count);
                        } while (count > 0);
                    }
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            txtResponse.Text = String.Empty;
           
            string url = txtUrl.Text;
            string data = txtRequest.Text;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = radioPOST.Checked ? "POST" : "GET";

            if(radioPOST.Checked)
                request.ContentType = "application/json";

            request.Timeout = 30 * 1000;
            //request.Headers.Add("Authorization", "BASIC SGVsbG8=");

            if (radioPOST.Checked)
            {
                // POST할 데이타를 Request Stream에 쓴다
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                request.ContentLength = bytes.Length; // 바이트수 지정

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }
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
            txtResponse.Text = responseText;
        }
    }
}