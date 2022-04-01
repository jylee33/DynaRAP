using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP
{
    public class MainStarter
    {
        MainForm mainForm = null;
        LoginForm loginForm = new LoginForm();
        //LoginForm2 loginForm = new LoginForm2();

        public MainStarter()
        {

        }

        public void Run()
        {
            if (Login() < 0)
            {
                return;
            }

            mainForm = new MainForm();
            //loginForm.LoginResultReceived += mainForm.Server_LoginSucceed;
            Application.Run(mainForm);
        }

        private int Login()
        {
            DialogResult dResult;
            string userId;
            dResult = loginForm.ShowDialog();
            userId = loginForm.UserId;

            if (dResult == DialogResult.OK)
            {

                //AccountManager accountManager = AccountManager.Instance;

                //accountManager.Initialize(OamServiceProvider.OamServer.DbConnection);
                //accountManager.SetLoginedUser(userId);
            }
            else
            {
                return -1;
            }

            return 0;
        }

    }
}
