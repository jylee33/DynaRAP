using DevExpress.XtraEditors;
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

namespace DynaRAP.Forms
{
    public partial class ChangeConfigForm : DevExpress.XtraEditors.XtraForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        DllResponse response = null;
        DllData dllData = null;
        public DllResponse Response { get => response; set => response = value; }

        public ChangeConfigForm()
        {
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void ChangeConfigForm_Load(object sender, EventArgs e)
        {
            try
            {
                edtIP.Text = ConfigurationManager.AppSettings["ServerIP"];
                edtPort.Text = ConfigurationManager.AppSettings["ServerPort"];
            }
            catch
            {
                edtIP.Text = "";
                edtPort.Text = "";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtIP.Text))
            {
                MessageBox.Show("IP 가 비어 있습니다.");
                return;
            }
            if (string.IsNullOrEmpty(edtPort.Text))
            {
                MessageBox.Show("Port 가 비어 있습니다.");
                return;
            }

            try
            {
                string url = edtIP.Text +":"+ edtPort.Text;
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings["ServerIP"] == null)
                {
                    settings.Add("ServerIP", edtIP.Text);
                }
                else
                {
                    settings["ServerIP"].Value = edtIP.Text;
                }

                if (settings["ServerPort"] == null)
                {
                    settings.Add("ServerPort", edtPort.Text);
                }
                else
                {
                    settings["ServerPort"].Value = edtPort.Text;
                }   

                if (settings["UrlDir"] == null)
                {
                    settings.Add("UrlDir", url + "/api/0.0.1/dir");
                }
                else
                {
                    settings["UrlDir"].Value = url + "/api/0.0.1/dir";
                }

                if (settings["UrlParam"] == null)
                {
                    settings.Add("UrlParam", url + "/api/0.0.1/param");
                }
                else
                {
                    settings["UrlParam"].Value = url + "/api/0.0.1/param";
                }

                if (settings["UrlPreset"] == null)
                {
                    settings.Add("UrlPreset", url + "/api/0.0.1/preset");
                }
                else
                {
                    settings["UrlPreset"].Value = url + "/api/0.0.1/preset";
                }


                if (settings["UrlImport"] == null)
                {
                    settings.Add("UrlImport", url + "/api/0.0.1/raw");
                }
                else
                {
                    settings["UrlImport"].Value = url + "/api/0.0.1/raw";
                }

                if (settings["UrlPart"] == null)
                {
                    settings.Add("UrlPart", url + "/api/0.0.1/part");
                }
                else
                {
                    settings["UrlPart"].Value = url + "/api/0.0.1/part";
                }

                if (settings["UrlShortBlock"] == null)
                {
                    settings.Add("UrlShortBlock", url + "/api/0.0.1/shortblock");
                }
                else
                {
                    settings["UrlShortBlock"].Value = url + "/api/0.0.1/shortblock";
                }

                if (settings["UrlDLL"] == null)
                {
                    settings.Add("UrlDLL", url + "/api/0.0.1/dll");
                }
                else
                {
                    settings["UrlDLL"].Value = url + "/api/0.0.1/dll";
                }

                if (settings["UrlDataProp"] == null)
                {
                    settings.Add("UrlDataProp", url + "/api/0.0.1/data-prop");
                }
                else
                {
                    settings["UrlDataProp"].Value = url + "/api/0.0.1/data-prop";
                }

                if (settings["UrlParamModule"] == null)
                {
                    settings.Add("UrlParamModule", url + "/api/0.0.1/param-module");
                }
                else
                {
                    settings["UrlParamModule"].Value = url + "/api/0.0.1/param-module";
                }

                if (settings["UrlBINTable"] == null)
                {
                    settings.Add("UrlBINTable", url + "/api/0.0.1/bin-table");
                }
                else
                {
                    settings["UrlBINTable"].Value = url + "/api/0.0.1/bin-table";
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                this.DialogResult = DialogResult.OK;
            }
            catch (ConfigurationErrorsException)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}