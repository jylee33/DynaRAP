using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.StyleFormatConditions;
using DynaRAP.TEST;
using DynaRAP.UControl;
using IronPython.Hosting;
using log4net.Config;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string defaultLayoutFile = "Default.xml";
        string defaultWorkspaceName = "Default";
        string projectLayoutFile = "Project.xml";
        string projectWorkspaceName = "Project";

        DockPanel panelBottom = null;

        StartScreenControl startControl = null;
        ImportModuleControl importModuleControl = null;
        SBModuleControl sbModuleControl = null;
        BinModuleControl binModuleControl = null;
        MgmtParameterControl mgmtParameterControl = null;
        MgmtPresetControl mgmtPresetControl = null;
        MgmtPresetGroupControl mgmtPresetGroupControl = null;

        CsvTableControl csvTableControl = null;

        public DevExpress.XtraBars.Docking.DockManager DockManager1
        {
            get
            {
                return this.dockManager1;
            }
        }
       
        public DockPanel PanelBinTable
        {
            get { return this.panelBinTable; }
        }

        public DockPanel PanelBinSbList
        {
            get { return this.panelBinSbList; }
        }

        public DockPanel PanelImportViewCsv
        {
            get { return this.panelImportViewCsv; }
        }

        public DockPanel PanelScenario
        {
            get { return this.panelScenario; }
        }

        public DockPanel PanelOther
        {
            get { return this.panelOther; }
        }

        public DockPanel PanelProperties
        {
            get { return this.panelProperties; }
        }

        public CsvTableControl CsvTableControl 
        { 
            get => csvTableControl;
            set => csvTableControl = value; 
        }
        public StartScreenControl StartControl { get => startControl; set => startControl = value; }
        public ImportModuleControl ImportModuleControl { get => importModuleControl; set => importModuleControl = value; }
        public SBModuleControl SbModuleControl { get => sbModuleControl; set => sbModuleControl = value; }
        public BinModuleControl BinModuleControl { get => binModuleControl; set => binModuleControl = value; }
        public MgmtParameterControl MgmtParameterControl { get => mgmtParameterControl; set => mgmtParameterControl = value; }
        public MgmtPresetControl MgmtPresetControl { get => mgmtPresetControl; set => mgmtPresetControl = value; }
        public MgmtPresetGroupControl MgmtPresetGroupControl { get => mgmtPresetGroupControl; set => mgmtPresetGroupControl = value; }

        public MainForm()
        {
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));

            // Handling the QueryControl event that will populate all automatically generated Documents
            this.tabbedView1.QueryControl += tabbedView1_QueryControl;
            this.tabbedView1.DocumentRemoved += TabbedView1_DocumentRemoved;

            LoadWorkspaces();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeWorkspace();

#if DEBUG
#else
            bar2.Visible = false;
            Workspace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            barSubItemTest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnStart.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
#endif

            if (startControl == null)
            {
                startControl = new StartScreenControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(startControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "시작화면";
                tabbedView1.ActivateDocument(startControl);
            }
            else
            {
                tabbedView1.ActivateDocument(startControl);
            }

            if (importModuleControl == null)
            {
                importModuleControl = new ImportModuleControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(importModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "Import Module";
                tabbedView1.ActivateDocument(importModuleControl);
            }
            else
            {
                tabbedView1.ActivateDocument(importModuleControl);
            }

            if (sbModuleControl == null)
            {
                sbModuleControl = new SBModuleControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(sbModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "Short Block";
                tabbedView1.ActivateDocument(sbModuleControl);
            }
            else
            {
                tabbedView1.ActivateDocument(sbModuleControl);
            }

            if (binModuleControl == null)
            {
                binModuleControl = new BinModuleControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(binModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "BIN 구성";
                tabbedView1.ActivateDocument(binModuleControl);
            }
            else
            {
                tabbedView1.ActivateDocument(binModuleControl);
            }

            if (mgmtParameterControl == null)
            {
                mgmtParameterControl = new MgmtParameterControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(mgmtParameterControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "파라미터 관리";
                tabbedView1.ActivateDocument(mgmtParameterControl);
            }
            else
            {
                tabbedView1.ActivateDocument(mgmtParameterControl);
            }

            if (mgmtPresetControl == null)
            {
                mgmtPresetControl = new MgmtPresetControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(mgmtPresetControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "프리셋 관리";
                tabbedView1.ActivateDocument(mgmtPresetControl);
            }
            else
            {
                tabbedView1.ActivateDocument(mgmtPresetControl);
            }

            if (mgmtPresetGroupControl == null)
            {
                mgmtPresetGroupControl = new MgmtPresetGroupControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(mgmtPresetGroupControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "프리셋그룹 관리";
                tabbedView1.ActivateDocument(mgmtPresetGroupControl);
            }
            else
            {
                tabbedView1.ActivateDocument(mgmtPresetGroupControl);
            }

#if DEBUG
            tabbedView1.ActivateDocument(mgmtPresetControl);
#else
            tabbedView1.RemoveDocument(importModuleControl);
            tabbedView1.RemoveDocument(sbModuleControl);
            tabbedView1.RemoveDocument(binModuleControl);
            tabbedView1.RemoveDocument(mgmtParameterControl);
            tabbedView1.RemoveDocument(mgmtPresetControl);
            tabbedView1.RemoveDocument(mgmtPresetGroupControl);
            tabbedView1.ActivateDocument(startControl);
#endif

            //if (panelBinTable == null)
            //{
            //    panelBinTable = dockManager1.AddPanel(DockingStyle.Bottom);
            //}
            panelBinTable.Name = "panelBinTable";
            panelBinTable.Text = "BIN TABLE";
            BinTableControl binTableCtrl = new BinTableControl();
            binTableCtrl.Dock = DockStyle.Fill;
            panelBinTable.Controls.Add(binTableCtrl);
            panelBinTable.Hide();

            panelBinSbList.Name = "panelBinSbList";
            panelBinSbList.Text = "ImportModuleScenarioName";
            BinSelectSBControl binSbListCtrl = new BinSelectSBControl();
            binSbListCtrl.Dock = DockStyle.Fill;
            panelBinSbList.Controls.Add(binSbListCtrl);
            panelBinSbList.Hide();

            panelImportViewCsv.Name = "panelImportViewCsv";
            panelImportViewCsv.Text = "CSV TABLE";
            csvTableControl = new CsvTableControl();
            csvTableControl.Dock = DockStyle.Fill;
            panelImportViewCsv.Controls.Add(csvTableControl);
            panelImportViewCsv.Hide();

            panelScenario.Hide();
            panelOther.Hide();
            panelProperties.Hide();
        }

        void LoadWorkspaces()
        {
            workspaceManager1.LoadWorkspace(defaultWorkspaceName, defaultLayoutFile);
            workspaceManager1.LoadWorkspace(projectWorkspaceName, projectLayoutFile);
        }

        void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            //if (e.Document == startScreenControlDocument)
            //{
            //    e.Control = startControl;
            //}
            //else if (e.Document == importModuleControlDocument)
            //{
            //    e.Control = importModuleControl;
            //}
            //else if (e.Document == sBModuleControlDocument)
            //{
            //    e.Control = sbModuleControl;
            //}
            //else if (e.Document == binModuleControlDocument)
            //{
            //    e.Control = binModuleControl;
            //}
            //else if (e.Document == mgmtParameterControl)
            //{
            //    e.Control = mgmtParameterControl;
            //}
            //else if (e.Document == mgmtPresetControl)
            //{
            //    e.Control = mgmtPresetControl;
            //}
            //else if (e.Document == mgmtPresetGroupControl)
            //{
            //    e.Control = mgmtPresetGroupControl;
            //}
            if (e.Control == null)
            {
                e.Control = new System.Windows.Forms.Control();
            }
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
            else if (e.Document.Control is SBModuleControl)
            {
                sbModuleControl = null;
            }
            else if (e.Document.Control is BinModuleControl)
            {
                binModuleControl = null;
            }
            else if (e.Document.Control is MgmtParameterControl)
            {
                mgmtParameterControl = null;
            }
            else if (e.Document.Control is MgmtPresetControl)
            {
                mgmtPresetControl = null;
            }
            else if (e.Document.Control is MgmtPresetGroupControl)
            {
                mgmtPresetGroupControl = null;
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

        public void Server_LoginSucceed(object sender, EventArgs e)
        {

        }
        private void btnPythonTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //pythonTest1();
            pythonTest2();
            
        }

        private void pythonTest1()
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

        private void pythonTest2()
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            var paths = engine.GetSearchPaths();

            paths.Add(@"C:\Python37");
            paths.Add(@"C:\Python37\DLLs");
            paths.Add(@"C:\Python37\Lib");
            paths.Add(@"C:\Python37\Lib\site-packages");

            engine.SetSearchPaths(paths);

            ScriptSource source = engine.CreateScriptSourceFromFile("Python/filter.py");
            source.Execute(scope);
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
            else
            {
                tabbedView1.ActivateDocument(startControl);
            }

        }

        private void btnImportModule_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (importModuleControl == null)
            {
                importModuleControl = new ImportModuleControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(importModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "비행데이터 Import";
                tabbedView1.ActivateDocument(importModuleControl);
            }
            else
            {
                tabbedView1.ActivateDocument(importModuleControl);
            }

        }

        private void btnSBModule_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (sbModuleControl == null)
            {
                sbModuleControl = new SBModuleControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(sbModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "Short Block";
                tabbedView1.ActivateDocument(sbModuleControl);
            }
            else
            {
                tabbedView1.ActivateDocument(sbModuleControl);
            }
        }

        private void btnBinModule_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (binModuleControl == null)
            {
                binModuleControl = new BinModuleControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(binModuleControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "BIN 구성";
                tabbedView1.ActivateDocument(binModuleControl);
            }
            else
            {
                tabbedView1.ActivateDocument(binModuleControl);
            }

        }

        private void btnDLLImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.InitialDirectory = "C:\\";
            //dlg.Filter = "Excel files (*.xls, *.xlsx)|*.xls; *.xlsx|Comma Separated Value files (CSV)|*.csv|모든 파일 (*.*)|*.*";

            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    //dlg.FileName;
            //}
        }

        private void btnImportAnalysis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnLPF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TestFilterForm form = new TestFilterForm();
            form.Show();
        }

        private void btnWebTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TestWebForm form = new TestWebForm();
            form.Show();
        }

        private void btnMgmtParameter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (mgmtParameterControl == null)
            {
                mgmtParameterControl = new MgmtParameterControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(mgmtParameterControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "파라미터 관리";
                tabbedView1.ActivateDocument(mgmtParameterControl);
            }
            else
            {
                tabbedView1.ActivateDocument(mgmtParameterControl);
            }
        }

        private void btnMgmtPreset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (mgmtPresetControl == null)
            {
                mgmtPresetControl = new MgmtPresetControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(mgmtPresetControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "프리셋 관리";
                tabbedView1.ActivateDocument(mgmtPresetControl);
            }
            else
            {
                tabbedView1.ActivateDocument(mgmtPresetControl);
            }
        }

        private void btnMgmtPresetGroup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (mgmtPresetGroupControl == null)
            {
                mgmtPresetGroupControl = new MgmtPresetGroupControl();
                DevExpress.XtraBars.Docking2010.Views.Tabbed.Document doc = tabbedView1.AddDocument(mgmtPresetGroupControl) as DevExpress.XtraBars.Docking2010.Views.Tabbed.Document;
                doc.Caption = "프리셋그룹 관리";
                tabbedView1.ActivateDocument(mgmtPresetGroupControl);
            }
            else
            {
                tabbedView1.ActivateDocument(mgmtPresetGroupControl);
            }
        }

        private void btnTestChart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TestChart form = new TestChart();
            form.Show();
        }
    }

}