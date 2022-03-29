using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1()
        {
            InitializeComponent();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            InitializeFuselageList();
        }

        private void InitializeFuselageList()
        {
            AddFuselage("형상 A/1호기");
            AddFuselage("형상 B/2호기");
            AddFuselage("형상 C/3호기");
            AddFuselage("형상 D/1호기");
            AddFuselage("형상 E/1호기");
            AddFuselage("형상 F/1호기");
            AddFuselage("형상 G/1호기");
            AddFuselage("형상 H/1호기");
            AddFuselage("형상 I/1호기");
            AddFuselage("형상 J/1호기");
            AddFuselage("형상 K/2호기");
            AddFuselage("형상 L/1호기");
            AddFuselage("형상 M/2호기");
            AddFuselage("형상 N/3호기");
            AddFuselage("형상 O/1호기");
            AddFuselage("형상 P/1호기");
            AddFuselage("형상 Q/1호기");
            AddFuselage("형상 R/1호기");
            AddFuselage("형상 S/1호기");
            AddFuselage("형상 T/1호기");
            AddFuselage("형상 U/1호기");
            AddFuselage("형상 V/2호기");
            AddFuselage("형상 W/1호기");
            AddFuselage("형상 X/2호기");
            AddFuselage("형상 Y/3호기");
            AddFuselage("형상 Z/1호기");
            AddFuselage("형상 A/1호기");
            AddFuselage("형상 B/2호기");
            AddFuselage("형상 C/3호기");
            AddFuselage("형상 D/1호기");
            AddFuselage("형상 E/1호기");
            AddFuselage("형상 F/1호기");
            AddFuselage("형상 G/1호기");
            AddFuselage("형상 H/1호기");
            AddFuselage("형상 I/1호기");
            AddFuselage("형상 J/1호기");
            AddFuselage("형상 K/2호기");
            AddFuselage("형상 L/1호기");
            AddFuselage("형상 M/2호기");
            AddFuselage("형상 N/3호기");
            AddFuselage("형상 O/1호기");
            AddFuselage("형상 P/1호기");
            AddFuselage("형상 Q/1호기");
            AddFuselage("형상 R/1호기");
            AddFuselage("형상 S/1호기");
            AddFuselage("형상 T/1호기");
            AddFuselage("형상 U/1호기");
            AddFuselage("형상 V/2호기");
            AddFuselage("형상 W/1호기");
            AddFuselage("형상 X/2호기");
            AddFuselage("형상 Y/3호기");
            AddFuselage("형상 Z/1호기");

        }

        private void AddFuselage(string name)
        {
            TextEdit edit = new TextEdit();
            edit.BorderStyle = BorderStyles.Simple;
            edit.ForeColor = Color.Gray;
            edit.Properties.Appearance.BorderColor = Color.Gray;
            edit.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            edit.ReadOnly = true;
            //edit.Properties.AllowFocused = false;
            edit.Text = name;
            edit.Click += Fuselage_Click;
            panelFuseLage.Controls.Add(edit);
        }

        private void Fuselage_Click(object sender, EventArgs e)

        {
            TextEdit edit = sender as TextEdit;
            if (edit != null)
            {

                // 하나의 비행기체만 선택되도록 한다.
                foreach (Control c in panelFuseLage.Controls)
                {
                    TextEdit ed = c as TextEdit;
                    ed.ForeColor = Color.Gray;
                    ed.Properties.Appearance.BorderColor = Color.Gray;
                    ed.Font = new Font(ed.Font, FontStyle.Regular);
                }
                edit.ForeColor = Color.White;
                edit.Properties.Appearance.BorderColor = Color.White;
                edit.Font = new Font(edit.Font, FontStyle.Bold);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

    }
}