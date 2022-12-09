using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
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
    public partial class SelectCsvTableControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string sourceSeq = string.Empty;
        string sourceType = string.Empty;


        public SelectCsvTableControl()
        {
            InitializeComponent();
        }

        private void SelectCsvTableControl_Load(object sender, EventArgs e)
        {
            //if(File.Exists(csvFilePath))
            //    FillGrid();
        }

        public void FillGridPartShortBlock(string seq, string type, List<string> selectTime)
        {
            if (selectTime != null)
            {
                selectTime = selectTime.Distinct().ToList();
                selectTime.Sort();
                sourceSeq = seq;
                sourceType = type;

                gridControl1.DataSource = null;
                GC.Collect();
                DataTable dt = GetTableData(seq);
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                gridView1.OptionsView.ShowColumnHeaders = true;
                gridView1.OptionsView.ShowGroupPanel = false;
                gridView1.OptionsView.ShowIndicator = false;
                gridView1.OptionsView.ColumnAutoWidth = false;

                gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;

                gridView1.OptionsSelection.MultiSelect = true;
                gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
                bool firstFlag = true;
                foreach (var time in selectTime)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = gridView1.GetDataRow(i);
                        if (row["DATE"].ToString() == time)
                        {
                            gridView1.SelectRow(i);
                            if (firstFlag)
                            {
                                if (i != 0)
                                {
                                    gridView1.FocusedRowHandle = i - 1;
                                }
                                else
                                {
                                    gridView1.FocusedRowHandle = 0;
                                }
                                firstFlag = false;
                            }
                        }
                    }
                }
                for (int i = 0; i < gridView1.Columns.Count; i++)
                {
                    gridView1.Columns[i].Width = 100;
                }
            }
        }
        private DataTable GetTableData(string blockSeq)
        {
            DataTable dt = new DataTable();
            try
            {
                string url = string.Empty;
                string sendData = string.Empty;
                switch (sourceType)
                {
                    case "shortblock":
                        url = ConfigurationManager.AppSettings["UrlShortBlock"];
                        sendData = string.Format(@"
                        {{
                        ""command"":""row-data"",
                        ""blockSeq"":""{0}"",
                        ""julianRange"":["""", """"],
                        ""filterType"": ""N""
                        }}", blockSeq);
                        break;
                    case "part":
                        url = ConfigurationManager.AppSettings["UrlPart"];
                        sendData = string.Format(@"
                        {{
                        ""command"":""row-data"",
                        ""partSeq"":""{0}"",
                        ""julianRange"":["""", """"],
                        ""filterType"": ""N""
                        }}", blockSeq);
                        break;
                }
                //url = ConfigurationManager.AppSettings["UrlShortBlock"];

                //sendData = string.Format(@"
                //{{
                //""command"":""row-data"",
                //""blockSeq"":""{0}"",
                //""julianRange"":["""", """"],
                //""filterType"": ""N""
                //}}"
                //, blockSeq);
                url += "/d";
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
                using (WebResponse resp = request.GetResponse())
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        string[] headers = sr.ReadLine().Split(',');
                        foreach (string header in headers)
                        {
                            if(header == "")
                            {
                                continue;
                            }
                            if (dt.Columns.Contains(header))
                            {
                                dt.Columns.Add(header+"_1");
                            }
                            else
                            {
                                dt.Columns.Add(header);
                            }
                        }
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(',');
                            DataRow dr = dt.NewRow();
                            int length = rows.Length;
                            if (rows.Length != dr.ItemArray.Length)
                            {
                                length = dr.ItemArray.Length < rows.Length ? dr.ItemArray.Length : rows.Length;
                            }
                            for (int i = 0; i < length; i++)
                            {
                                if (rows[i] == "")
                                {
                                    continue ;
                                }
                                dr[i] = rows[i];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        private void GetTableSave(string blockSeq, List<string> selectDateTime)
        {
            MainForm mainForm = this.ParentForm as MainForm;

            mainForm.ShowSplashScreenManager("저장할 데이터를 불러오는 중입니다.. 잠시만 기다려주십시오.");
            try
            {
                int selectCount = selectDateTime.Count-1;
                int cnt = 0;
                string url = string.Empty;
                string sendData = string.Empty;
                switch (sourceType)
                {
                    case "shortblock":
                        url = ConfigurationManager.AppSettings["UrlShortBlock"];
                        sendData = string.Format(@"
                        {{
                        ""command"":""row-data"",
                        ""blockSeq"":""{0}"",
                        ""julianRange"":["""", """"],
                        ""filterType"": ""N""
                        }}", blockSeq);
                        break;
                    case "part":
                        url = ConfigurationManager.AppSettings["UrlPart"];
                        sendData = string.Format(@"
                        {{
                        ""command"":""row-data"",
                        ""partSeq"":""{0}"",
                        ""julianRange"":["""", """"],
                        ""filterType"": ""N""
                        }}", blockSeq);
                        break;
                }
                //string url = ConfigurationManager.AppSettings["UrlShortBlock"];

                //string sendData = string.Format(@"
                //{{
                //""command"":""row-data"",
                //""blockSeq"":""{0}"",
                //""julianRange"":["""", """"],
                //""filterType"": ""N""
                //}}"
                //, blockSeq);
                url += "/d";
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

                string responseText = string.Empty;
                if (request != null)
                {
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(bytes, 0, bytes.Length);
                    }
                    // Response 처리
                    using (WebResponse resp = request.GetResponse())
                    {
                        Stream respStream = resp.GetResponseStream();
                        using (StreamReader sr = new StreamReader(respStream))
                        {
                            responseText += sr.ReadLine();
                            responseText += "\n" + sr.ReadLine();
                            while (!sr.EndOfStream)
                            {
                                string streamData = sr.ReadLine();
                                if (streamData.Contains(selectDateTime[cnt]))
                                {
                                    responseText += "\n" + streamData;
                                    if (cnt == selectCount)
                                    {
                                        break;
                                    }
                                    cnt++;
                                }
                            }
                        }
                    }
                }

                mainForm.HideSplashScreenManager();
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = string.Format("{0}", "Plot_Select_Point");
                dlg.Filter = "Comma Separated Value files (CSV)|*.csv";
                dlg.Title = "Save a CSV File";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    mainForm.ShowSplashScreenManager("선택된 데이터를 저장 중입니다.. 잠시만 기다려주십시오.");
                    string fileName = dlg.FileName;

                    FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(responseText);
                    sw.Close();
                    fs.Close();
                    mainForm.HideSplashScreenManager();
                }

                //Console.WriteLine(responseText);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                mainForm.HideSplashScreenManager();
            }
        }


        private DataTable LoadCSV(string path, bool hasHeader)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(path))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }


            return dt;
        }

        private void btn_DownCsvFile_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("저장할 데이터를 불러오는 중입니다.. 잠시만 기다려주십시오.");
            var selectedRows = gridView1.GetSelectedRows();
            if (selectedRows != null)
            {
                List<string> selectedDate = new List<string>();
                foreach (var index in selectedRows)
                {
                    DataRow row = gridView1.GetDataRow(index);
                    if (row != null)
                    {
                        selectedDate.Add(row["DATE"].ToString());
                    }
                }
                mainForm.HideSplashScreenManager();
                GetTableSave(sourceSeq, selectedDate);
            }
        }

        public void CrearDataTable()
        {
            gridControl1.DataSource = null;
        }
    }
}
