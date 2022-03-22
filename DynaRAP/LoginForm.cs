using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP
{
    public partial class LoginForm : DevExpress.XtraEditors.XtraForm
    {

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        TextEdit[] txtList;
        string strIdPlaceholder = Properties.Resources.IdPlaceholder;
        string strPwPlaceholder = Properties.Resources.PwPlaceholder;

        public LoginForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            //ID, Password TextEdit Placeholder 설정
            txtList = new TextEdit[] { edtId, edtPassword };
            foreach (var txt in txtList)
            {
                //처음 공백 Placeholder 지정
                txt.ForeColor = Color.DarkGray;
                if (txt == edtId) txt.Text = strIdPlaceholder;
                else if (txt == edtPassword) txt.Text = strPwPlaceholder;

                txt.Focus();

                //텍스트박스 커서 Focus 여부에 따라 이벤트 지정
                txt.GotFocus += RemovePlaceholder;
                txt.LostFocus += SetPlaceholder;
            }

        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            TextEdit txt = (TextEdit)sender;
            if (txt.Text == strIdPlaceholder | txt.Text == strPwPlaceholder)
            { //텍스트박스 내용이 사용자가 입력한 값이 아닌 Placeholder일 경우에만, 커서 포커스일때 빈칸으로 만들기
                txt.ForeColor = Color.Black; //사용자 입력 진한 글씨
                txt.Text = string.Empty;
                if (txt == edtPassword) edtPassword.Properties.PasswordChar = '*';
            }
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            TextEdit txt = (TextEdit)sender;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                //사용자 입력값이 하나도 없는 경우에 포커스 잃으면 Placeholder 적용해주기
                txt.ForeColor = Color.DarkGray; //Placeholder 흐린 글씨
                if (txt == edtId) txt.Text = strIdPlaceholder;
                else if (txt == edtPassword) { txt.Text = strPwPlaceholder; edtPassword.Properties.PasswordChar = default; }
            }
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void LoginForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void LoginForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}