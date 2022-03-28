using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.StyleFormatConditions;
using DynaRAP.TEST;
using DynaRAP.UControl;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        string defaultLayoutFile = "Default.xml";
        string defaultWorkspaceName = "Default";
        string projectLayoutFile = "Project.xml";
        string projectWorkspaceName = "Project";

        DockPanel panelBottom = null;

        StartScreenControl startControl = null;
        ImportModuleControl importModuleControl = null;


        public MainForm()
        {
            InitializeComponent();
            // Handling the QueryControl event that will populate all automatically generated Documents
            this.tabbedView1.QueryControl += tabbedView1_QueryControl;

            LoadWorkspaces();
        }

        void LoadWorkspaces()
        {
            workspaceManager1.LoadWorkspace(defaultWorkspaceName, defaultLayoutFile);
            workspaceManager1.LoadWorkspace(projectWorkspaceName, projectLayoutFile);
        }

        void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            //if (e.Document == startScreenControlDocument)
            //    e.Control = new DynaRAP.UControl.StartScreenControl();
            //if (e.Document == projectListControlDocument)
            //    e.Control = new DynaRAP.UControl.ProjectListControl();
            //if (e.Document == userControl1Document)
            //    e.Control = new DynaRAP.UserControl1();
            //if (e.Document == userControl2Document)
            //    e.Control = new DynaRAP.UserControl2();
            if (e.Control == null)
                e.Control = new System.Windows.Forms.Control();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeWorkspace();

            tabbedView1.DocumentRemoved += TabbedView1_DocumentRemoved;

            startControl = new StartScreenControl();
            DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(startControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
            doc.Caption = "시작화면";
            tabbedView1.ActivateDocument(startControl);

        }

        private void TabbedView1_DocumentRemoved(object sender, DevExpress.XtraBars.Docking2010.Views.DocumentEventArgs e)
        {
            if (e.Document.Control is StartScreenControl)
            {
                startControl = null;
            }
            else if (e.Document.Control is ImportModuleControl)
            {
                importModuleControl = null;
            }
        }

        private void InitializeWorkspace()
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
            //if (workspaceManager1.LoadWorkspace(projectWorkspaceName, projectLayoutFile, true))
            //    workspaceManager1.ApplyWorkspace(projectWorkspaceName);

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Save DevExpress controls' layouts to a file
            workspaceManager1.CaptureWorkspace(projectWorkspaceName, true);
            workspaceManager1.SaveWorkspace(projectWorkspaceName, projectLayoutFile, true);
            
        }

        private void btnPythonTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptSource source = engine.CreateScriptSourceFromFile("Python/Calculator.py");
            ScriptScope scope = engine.CreateScope();
            source.Execute(scope);

            dynamic Calculator = scope.GetVariable("Calculator");
            dynamic calc = Calculator();
            int result = calc.add(4, 5);
            Console.WriteLine("result = {0}", result);
        }

        private void btnChartTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
        }

        private void btnLogin_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoginForm form = new LoginForm();
            form.Show();
        }

        private void btnPanel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //DockPanel panel = this.panelContainer1.AddPanel();
            //panel.Dock = DockingStyle.Fill;
            //panel.Location = new Point(0, 0);
            //panel.Name = "newPanel";
            //panel.Text = "New Panel";

            //DockPanel panel1 = dockManager1.AddPanel(DockingStyle.Bottom);
            //panel1.Text = "Panel 1";
            //// Add a new panel to panel1. This forms a split container that owns panel1 and panel2.
            //DockPanel panel2 = panel1.AddPanel();
            //panel2.Text = "Panel 2";
            //// Transform the split container into a tab container.
            //DockPanel container = panel1.ParentPanel;
            //container.Tabbed = true;



            //DockPanel panel = this.panelContainer2.AddPanel();
            //panel.Dock = DockingStyle.Fill;
            //panelContainer2.Tabbed = true;

            //DockPanel container = panel.ParentPanel;
            //if (container == null) return;
            // Transform the split container to a tab container.
            //container.Tabbed = true;

            //if(panelBottom == null)
            //{
            //    panelBottom = dockManager1.AddPanel(DockingStyle.Bottom);
            //    panelBottom.ClosedPanel += PanelBottom_ClosedPanel;
            //}
            //else
            //{
            //    DockPanel panel = panelBottom.AddPanel();
            //    panel.ClosedPanel += Panel_ClosedPanel;
            //    //DockPanel container = panel.ParentPanel;
            //    //if (container == null)
            //    //    return;
            //    //container.Tabbed = true;
            //}

            //아래에 panel 추가
            //DockPanel panel = panelContainer2.AddPanel();
            //panel.Name = "panel";
            //panel.Text = "addedPanel";
            //panelContainer2.Tabbed = true;
            //panelContainer2.ActiveChild = panel;

            if (importModuleControl == null)
            {
                importModuleControl = new ImportModuleControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(importModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "Import Module";
                tabbedView1.ActivateDocument(importModuleControl);
            }

            if (startControl != null)
            {
                tabbedView1.RemoveDocument(startControl);
            }

        }

        private void PanelBottom_ClosedPanel(object sender, DockPanelEventArgs e)
        {
            //panelBottom.ClosedPanel -= PanelBottom_ClosedPanel;
            //panelBottom = null;
        }

        private void Panel_ClosedPanel(object sender, DockPanelEventArgs e)
        {
            DockPanel panel = sender as DockPanel;
            DockPanel container = panel.ParentPanel;
            if (container == null)
                panelBottom = null;
        }

        private void toolTreeTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeTestForm form = new TreeTestForm();
            form.Show();
        }

        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (startControl == null)
            {
                startControl = new StartScreenControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(startControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "시작화면";
                tabbedView1.ActivateDocument(startControl);
            }
        }
    }

}