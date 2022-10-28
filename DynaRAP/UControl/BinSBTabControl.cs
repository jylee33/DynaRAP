using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DynaRAP.Data;
using DynaRAP.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class BinSBTabControl : DevExpress.XtraEditors.XtraUserControl
    {
        private string idxValue = string.Empty;
        List<string> shortBlockSeqList = null;
        string binTableName = string.Empty;
        string binMetaSeq = string.Empty;
        BinSBSummary sbSummaryControl = null;
        SummaryData selectSummaryData = null;
        string selectParamSeq = string.Empty;
        public string IdxValue
        {
            get { return idxValue; }
            set { idxValue = value; }
        }

        public BinSBTabControl(List<string> shortBlockSeqList,SummaryData selectSummaryData,string selectParamSeq, string binTableName, string binMetaSeq)
        {
            this.shortBlockSeqList = shortBlockSeqList;
            this.binTableName = binTableName;
            this.binMetaSeq = binMetaSeq;
            this.selectSummaryData = selectSummaryData;
            this.selectParamSeq = selectParamSeq;
            InitializeComponent();
        }

        private void BinSBTabControl_Load(object sender, EventArgs e)
        {
            //AddTabPage("SB1");
            //AddTabPage("SB2");
            //AddTabPage("SB3");
            //AddTabPage("SB4");
            //AddTabPage("SB5");
            if (selectSummaryData != null)
            {
                SetTabPage();
            }
        }

        private void SetTabPage()
        {
            AddTabSummaryPage();
            foreach (string shortblock in shortBlockSeqList)
            {
                string sendData = string.Format(@"
                {{
                ""command"":""info"",
                ""blockSeq"":""{0}""
                }}", shortblock);
                string responseData = Utils.GetPostData(System.Configuration.ConfigurationManager.AppSettings["UrlShortBlock"], sendData);
                if (responseData != null)
                {
                    SBInfoResponse responseParam = JsonConvert.DeserializeObject<SBInfoResponse>(responseData);
                    if (responseParam.code == 200)
                    {

                        byte[] byte64 = Convert.FromBase64String(responseParam.response.blockName);
                        string shorblockName = Encoding.UTF8.GetString(byte64);

                        AddTabPage(responseParam.response.partSeq,shorblockName, shortblock);


                    }
                }

                // sendData = string.Format(@"
                //{{
                //""command"":""param-list"",
                //""blockSeq"":""{0}""
                //}}", shortblock);
                //responseData = Utils.GetPostData(System.Configuration.ConfigurationManager.AppSettings["UrlShortBlock"], sendData);
                //if (responseData != null)
                //{
                //    ResponseParamList responseParam = JsonConvert.DeserializeObject<ResponseParamList>(responseData);
                //    if (responseParam.code == 200)
                //    {



                //    }
                //}
            }
            sbSummaryControl.initGridControl();
        }
        private void AddTabSummaryPage()
        {
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = "Summary";
            tabPage.Text = "Summary";

            sbSummaryControl = new BinSBSummary(selectSummaryData,binTableName,binMetaSeq);
            sbSummaryControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(sbSummaryControl);
        }
        private void AddTabPage(string partSeq, string tabName, string shortBlockSeq)
        {
            XtraTabPage tabPage = new XtraTabPage();
            this.xtraTabControl1.TabPages.Add(tabPage);
            tabPage.Name = tabName;
            tabPage.Text = tabName;

            BinSBInfoControl sbInfoControl = new BinSBInfoControl(partSeq, shortBlockSeq, selectParamSeq, tabName, sbSummaryControl);
            sbInfoControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(sbInfoControl);
        }
    }
}
