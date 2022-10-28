using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DynaRAP.Data;
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
    public partial class TagControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler DeleteBtnClicked;
        public string tagValue = null;
        DXChartControl dXChartControl = null;
        DockPanel panel = null;


        public TagControl(string tagValue, DXChartControl dXChartControl)
        {
            this.tagValue = tagValue;
            this.dXChartControl = dXChartControl;
            this.panel = panel;
            InitializeComponent();
        }

        private void BinParameterControl_Load(object sender, EventArgs e)
        {
            InitTagSet(tagValue);
        }

        private void InitTagSet(string tagVal)
        {
            string[] tagList = tagVal.Split('|');
            panelTag.Controls.Clear();
            foreach (string value in tagList)
            {
                addTag(value);
            }
        }
        private void edtTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }

        private void edtTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }

        private void addTag(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            ButtonEdit btn = new ButtonEdit();
            btn.Properties.Buttons[0].Kind = ButtonPredefines.Close;
            btn.BorderStyle = BorderStyles.Simple;
            btn.ForeColor = Color.White;
            btn.Properties.Appearance.BorderColor = Color.White;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
            btn.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            //btn.ReadOnly = true;
            btn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btn.Properties.AllowFocused = false;
            btn.ButtonClick += removeTag_ButtonClick;
            btn.Text = name;
            panelTag.Controls.Add(btn);
        }

        private void removeTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit btn = sender as ButtonEdit;
            panelTag.Controls.Remove(btn);

        }

        private void btnTagSave_Click(object sender, EventArgs e)
        {
            string tagValues = string.Empty;
            foreach (ButtonEdit btn in this.panelTag.Controls)
            {
                tagValues += btn.Text + "|";
            }
            if (tagValues != String.Empty)
            {
                tagValues = tagValues.Substring(0, tagValues.LastIndexOf("|"));
            }
            dXChartControl.setTagValue(tagValues);
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            dXChartControl.closePanel();
        }
    }
}
