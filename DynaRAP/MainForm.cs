using DevExpress.XtraEditors;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
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
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {

        string file = "layout.xml";
        string workspaceName1 = "Default Layout";

        public MainForm()
        {
            InitializeComponent();
            // Handling the QueryControl event that will populate all automatically generated Documents
            this.tabbedView1.QueryControl += tabbedView1_QueryControl;

            LoadWorkspaces();
        }

        void LoadWorkspaces()
        {
            workspaceManager1.LoadWorkspace(workspaceName1, file);
            workspaceManager1.LoadWorkspace("jylee", @"jylee.xml");
        }

        void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            if (e.Document == userControl1Document)
                e.Control = new DynaRAP.UserControl1();
            if (e.Document == userControl2Document)
                e.Control = new DynaRAP.UserControl2();
            if (e.Control == null)
                e.Control = new System.Windows.Forms.Control();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Use the WorkspaceManager to handle the layout of DevExpress controls that reside within the current form.
            workspaceManager1.TargetControl = this;

            // Save & restore the form's size, position and state along with DevExpress controls' layouts.
            workspaceManager1.SaveTargetControlSettings = true;

            // Disable layout load animation effects 
            workspaceManager1.AllowTransitionAnimation = DevExpress.Utils.DefaultBoolean.False;

            // Disable (de)serialization for the following controls (if required):
            //WorkspaceManager.SetSerializationEnabled(gaugeControl1, false);
            //WorkspaceManager.SetSerializationEnabled(accordionControl1, false);

            // When restoring layouts of controls in a Form.Load event handler,
            // you may need to call the controls' ForceInitialize methods to finish their initialization before restoring their layouts.
            //gridControl1.ForceInitialize();
            //dockManager1.ForceInitialize();
            //barManager1.ForceInitialize();
            //...

            //Load DevExpress controls' layouts from a file
            if (workspaceManager1.LoadWorkspace(workspaceName1, file, true))
                workspaceManager1.ApplyWorkspace(workspaceName1);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Save DevExpress controls' layouts to a file
            workspaceManager1.CaptureWorkspace(workspaceName1, true);
            workspaceManager1.SaveWorkspace(workspaceName1, file, true);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptSource source = engine.CreateScriptSourceFromFile("../../../Calculator.py");
            ScriptScope scope = engine.CreateScope();
            source.Execute(scope);

            dynamic Calculator = scope.GetVariable("Calculator");
            dynamic calc = Calculator();
            int result = calc.add(4, 5);
            Console.WriteLine("result = {0}", result);
        }
    }
}