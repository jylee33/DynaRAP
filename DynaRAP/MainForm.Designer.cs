
using DevExpress.XtraBars.Docking2010.Views.Tabbed;

namespace DynaRAP
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.Animation.PushTransition pushTransition1 = new DevExpress.Utils.Animation.PushTransition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barSubItemTest = new DevExpress.XtraBars.BarSubItem();
            this.btnPythonTest = new DevExpress.XtraBars.BarButtonItem();
            this.toolTreeTest = new DevExpress.XtraBars.BarButtonItem();
            this.btnLogin = new DevExpress.XtraBars.BarButtonItem();
            this.btnChartTest = new DevExpress.XtraBars.BarButtonItem();
            this.btnWebTest = new DevExpress.XtraBars.BarButtonItem();
            this.btnLPF = new DevExpress.XtraBars.BarButtonItem();
            this.btnTestChart = new DevExpress.XtraBars.BarButtonItem();
            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.btnImportDLL = new DevExpress.XtraBars.BarButtonItem();
            this.btnImportFlying = new DevExpress.XtraBars.BarButtonItem();
            this.btnImportAnalysis = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.btnMgmtParameter = new DevExpress.XtraBars.BarButtonItem();
            this.btnMgmtPreset = new DevExpress.XtraBars.BarButtonItem();
            this.btnMgmtPresetGroup = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btnSBModule = new DevExpress.XtraBars.BarButtonItem();
            this.btnBinModule = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem4 = new DevExpress.XtraBars.BarSubItem();
            this.btnChartLine2d = new DevExpress.XtraBars.BarButtonItem();
            this.btnChartMinMax = new DevExpress.XtraBars.BarButtonItem();
            this.btnChartPotato = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.Workspace = new DevExpress.XtraBars.BarWorkspaceMenuItem();
            this.workspaceManager1 = new DevExpress.Utils.WorkspaceManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.panelContainer2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.panelImportViewCsv = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelBinTable = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel4_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelContainer3 = new DevExpress.XtraBars.Docking.DockPanel();
            this.panelBinSbList = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelProperties = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel3_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelContainer4 = new DevExpress.XtraBars.Docking.DockPanel();
            this.panelScenario = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelOther = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer2 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.panelContainer2.SuspendLayout();
            this.panelImportViewCsv.SuspendLayout();
            this.panelBinTable.SuspendLayout();
            this.panelContainer3.SuspendLayout();
            this.panelBinSbList.SuspendLayout();
            this.panelProperties.SuspendLayout();
            this.panelContainer4.SuspendLayout();
            this.panelScenario.SuspendLayout();
            this.panelOther.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.MenuManager = this.barManager1;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnPythonTest,
            this.btnChartTest,
            this.btnLogin,
            this.btnImportFlying,
            this.Workspace,
            this.barSubItemTest,
            this.toolTreeTest,
            this.btnStart,
            this.btnSBModule,
            this.btnBinModule,
            this.btnImportDLL,
            this.barSubItem2,
            this.btnImportAnalysis,
            this.barSubItem3,
            this.btnMgmtParameter,
            this.btnMgmtPreset,
            this.btnMgmtPresetGroup,
            this.barSubItem1,
            this.btnLPF,
            this.btnWebTest,
            this.btnTestChart,
            this.barSubItem4,
            this.btnChartLine2d,
            this.btnChartMinMax,
            this.btnChartPotato});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 32;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItemTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnStart),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem4)});
            resources.ApplyResources(this.bar1, "bar1");
            // 
            // barSubItemTest
            // 
            resources.ApplyResources(this.barSubItemTest, "barSubItemTest");
            this.barSubItemTest.Id = 11;
            this.barSubItemTest.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPythonTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.toolTreeTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnLogin),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnChartTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnWebTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnLPF),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnTestChart)});
            this.barSubItemTest.Name = "barSubItemTest";
            // 
            // btnPythonTest
            // 
            resources.ApplyResources(this.btnPythonTest, "btnPythonTest");
            this.btnPythonTest.Id = 5;
            this.btnPythonTest.Name = "btnPythonTest";
            this.btnPythonTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPythonTest_ItemClick);
            // 
            // toolTreeTest
            // 
            resources.ApplyResources(this.toolTreeTest, "toolTreeTest");
            this.toolTreeTest.Id = 12;
            this.toolTreeTest.Name = "toolTreeTest";
            this.toolTreeTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.toolTreeTest_ItemClick);
            // 
            // btnLogin
            // 
            resources.ApplyResources(this.btnLogin, "btnLogin");
            this.btnLogin.Id = 7;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLogin_ItemClick);
            // 
            // btnChartTest
            // 
            resources.ApplyResources(this.btnChartTest, "btnChartTest");
            this.btnChartTest.Id = 6;
            this.btnChartTest.Name = "btnChartTest";
            this.btnChartTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChartTest_ItemClick);
            // 
            // btnWebTest
            // 
            resources.ApplyResources(this.btnWebTest, "btnWebTest");
            this.btnWebTest.Id = 26;
            this.btnWebTest.Name = "btnWebTest";
            this.btnWebTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnWebTest_ItemClick);
            // 
            // btnLPF
            // 
            resources.ApplyResources(this.btnLPF, "btnLPF");
            this.btnLPF.Id = 24;
            this.btnLPF.Name = "btnLPF";
            this.btnLPF.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLPF_ItemClick);
            // 
            // btnTestChart
            // 
            resources.ApplyResources(this.btnTestChart, "btnTestChart");
            this.btnTestChart.Id = 27;
            this.btnTestChart.Name = "btnTestChart";
            this.btnTestChart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTestChart_ItemClick);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Id = 13;
            this.btnStart.Name = "btnStart";
            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStart_ItemClick);
            // 
            // barSubItem2
            // 
            resources.ApplyResources(this.barSubItem2, "barSubItem2");
            this.barSubItem2.Id = 17;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnImportDLL),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnImportFlying),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnImportAnalysis)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // btnImportDLL
            // 
            resources.ApplyResources(this.btnImportDLL, "btnImportDLL");
            this.btnImportDLL.Id = 16;
            this.btnImportDLL.Name = "btnImportDLL";
            this.btnImportDLL.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDLLImport_ItemClick);
            // 
            // btnImportFlying
            // 
            resources.ApplyResources(this.btnImportFlying, "btnImportFlying");
            this.btnImportFlying.Id = 8;
            this.btnImportFlying.Name = "btnImportFlying";
            this.btnImportFlying.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnImportModule_ItemClick);
            // 
            // btnImportAnalysis
            // 
            resources.ApplyResources(this.btnImportAnalysis, "btnImportAnalysis");
            this.btnImportAnalysis.Id = 18;
            this.btnImportAnalysis.Name = "btnImportAnalysis";
            this.btnImportAnalysis.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnImportAnalysis_ItemClick);
            // 
            // barSubItem3
            // 
            resources.ApplyResources(this.barSubItem3, "barSubItem3");
            this.barSubItem3.Id = 19;
            this.barSubItem3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnMgmtParameter),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnMgmtPreset),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnMgmtPresetGroup)});
            this.barSubItem3.Name = "barSubItem3";
            // 
            // btnMgmtParameter
            // 
            resources.ApplyResources(this.btnMgmtParameter, "btnMgmtParameter");
            this.btnMgmtParameter.Id = 20;
            this.btnMgmtParameter.Name = "btnMgmtParameter";
            this.btnMgmtParameter.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMgmtParameter_ItemClick);
            // 
            // btnMgmtPreset
            // 
            resources.ApplyResources(this.btnMgmtPreset, "btnMgmtPreset");
            this.btnMgmtPreset.Id = 21;
            this.btnMgmtPreset.Name = "btnMgmtPreset";
            this.btnMgmtPreset.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMgmtPreset_ItemClick);
            // 
            // btnMgmtPresetGroup
            // 
            resources.ApplyResources(this.btnMgmtPresetGroup, "btnMgmtPresetGroup");
            this.btnMgmtPresetGroup.Id = 22;
            this.btnMgmtPresetGroup.Name = "btnMgmtPresetGroup";
            this.btnMgmtPresetGroup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnMgmtPresetGroup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMgmtPresetGroup_ItemClick);
            // 
            // barSubItem1
            // 
            resources.ApplyResources(this.barSubItem1, "barSubItem1");
            this.barSubItem1.Id = 23;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSBModule),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnBinModule)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btnSBModule
            // 
            resources.ApplyResources(this.btnSBModule, "btnSBModule");
            this.btnSBModule.Id = 14;
            this.btnSBModule.Name = "btnSBModule";
            this.btnSBModule.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSBModule_ItemClick);
            // 
            // btnBinModule
            // 
            resources.ApplyResources(this.btnBinModule, "btnBinModule");
            this.btnBinModule.Id = 15;
            this.btnBinModule.Name = "btnBinModule";
            this.btnBinModule.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBinModule_ItemClick);
            // 
            // barSubItem4
            // 
            resources.ApplyResources(this.barSubItem4, "barSubItem4");
            this.barSubItem4.Id = 28;
            this.barSubItem4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnChartLine2d),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnChartMinMax),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnChartPotato)});
            this.barSubItem4.Name = "barSubItem4";
            // 
            // btnChartLine2d
            // 
            resources.ApplyResources(this.btnChartLine2d, "btnChartLine2d");
            this.btnChartLine2d.Id = 29;
            this.btnChartLine2d.Name = "btnChartLine2d";
            this.btnChartLine2d.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChartLine2d_ItemClick);
            // 
            // btnChartMinMax
            // 
            resources.ApplyResources(this.btnChartMinMax, "btnChartMinMax");
            this.btnChartMinMax.Id = 30;
            this.btnChartMinMax.Name = "btnChartMinMax";
            this.btnChartMinMax.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChartMinMax_ItemClick);
            // 
            // btnChartPotato
            // 
            resources.ApplyResources(this.btnChartPotato, "btnChartPotato");
            this.btnChartPotato.Id = 31;
            this.btnChartPotato.Name = "btnChartPotato";
            this.btnChartPotato.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChartPotato_ItemClick);
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Workspace)});
            this.bar2.OptionsBar.AutoPopupMode = DevExpress.XtraBars.BarAutoPopupMode.None;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            resources.ApplyResources(this.bar2, "bar2");
            // 
            // Workspace
            // 
            resources.ApplyResources(this.Workspace, "Workspace");
            this.Workspace.Id = 10;
            this.Workspace.Name = "Workspace";
            this.Workspace.ShowSaveLoadCommands = true;
            this.Workspace.WorkspaceManager = this.workspaceManager1;
            // 
            // workspaceManager1
            // 
            this.workspaceManager1.TargetControl = this;
            this.workspaceManager1.TransitionType = pushTransition1;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            resources.ApplyResources(this.bar3, "bar3");
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            resources.ApplyResources(this.barDockControlTop, "barDockControlTop");
            this.barDockControlTop.Manager = this.barManager1;
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            resources.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
            this.barDockControlBottom.Manager = this.barManager1;
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            resources.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
            this.barDockControlLeft.Manager = this.barManager1;
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            resources.ApplyResources(this.barDockControlRight, "barDockControlRight");
            this.barDockControlRight.Manager = this.barManager1;
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.panelContainer2,
            this.panelContainer3,
            this.panelContainer4});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // panelContainer2
            // 
            this.panelContainer2.ActiveChild = this.panelImportViewCsv;
            this.panelContainer2.Controls.Add(this.panelImportViewCsv);
            this.panelContainer2.Controls.Add(this.panelBinTable);
            this.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer2.FloatVertical = true;
            this.panelContainer2.ID = new System.Guid("84cb71cc-a810-4c21-8fb6-2b9bfc8242e7");
            resources.ApplyResources(this.panelContainer2, "panelContainer2");
            this.panelContainer2.Name = "panelContainer2";
            this.panelContainer2.OriginalSize = new System.Drawing.Size(1012, 211);
            this.panelContainer2.Tabbed = true;
            // 
            // panelImportViewCsv
            // 
            this.panelImportViewCsv.Controls.Add(this.controlContainer1);
            this.panelImportViewCsv.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelImportViewCsv.ID = new System.Guid("d9a0ca3c-ea60-4ccb-b13a-157b5c5f38f6");
            resources.ApplyResources(this.panelImportViewCsv, "panelImportViewCsv");
            this.panelImportViewCsv.Name = "panelImportViewCsv";
            this.panelImportViewCsv.OriginalSize = new System.Drawing.Size(1084, 170);
            // 
            // controlContainer1
            // 
            resources.ApplyResources(this.controlContainer1, "controlContainer1");
            this.controlContainer1.Name = "controlContainer1";
            // 
            // panelBinTable
            // 
            this.panelBinTable.Controls.Add(this.dockPanel4_Container);
            this.panelBinTable.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelBinTable.ID = new System.Guid("6dfc02a1-a41a-4100-a459-234398a2ca56");
            resources.ApplyResources(this.panelBinTable, "panelBinTable");
            this.panelBinTable.Name = "panelBinTable";
            this.panelBinTable.OriginalSize = new System.Drawing.Size(1084, 170);
            // 
            // dockPanel4_Container
            // 
            resources.ApplyResources(this.dockPanel4_Container, "dockPanel4_Container");
            this.dockPanel4_Container.Name = "dockPanel4_Container";
            // 
            // panelContainer3
            // 
            this.panelContainer3.Controls.Add(this.panelBinSbList);
            this.panelContainer3.Controls.Add(this.panelProperties);
            this.panelContainer3.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelContainer3.ID = new System.Guid("77ebbe2f-c44d-4f1b-90ba-84ae38c7d40f");
            resources.ApplyResources(this.panelContainer3, "panelContainer3");
            this.panelContainer3.Name = "panelContainer3";
            this.panelContainer3.OriginalSize = new System.Drawing.Size(254, 218);
            // 
            // panelBinSbList
            // 
            this.panelBinSbList.Controls.Add(this.dockPanel2_Container);
            this.panelBinSbList.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelBinSbList.ID = new System.Guid("bbd25a6d-0250-46ba-9429-84d2ef47bbbc");
            resources.ApplyResources(this.panelBinSbList, "panelBinSbList");
            this.panelBinSbList.Name = "panelBinSbList";
            this.panelBinSbList.OriginalSize = new System.Drawing.Size(254, 229);
            // 
            // dockPanel2_Container
            // 
            resources.ApplyResources(this.dockPanel2_Container, "dockPanel2_Container");
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.dockPanel3_Container);
            this.panelProperties.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelProperties.ID = new System.Guid("2633c3b8-4ee7-4e73-b180-a7e2da9a6929");
            resources.ApplyResources(this.panelProperties, "panelProperties");
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.OriginalSize = new System.Drawing.Size(254, 228);
            // 
            // dockPanel3_Container
            // 
            resources.ApplyResources(this.dockPanel3_Container, "dockPanel3_Container");
            this.dockPanel3_Container.Name = "dockPanel3_Container";
            // 
            // panelContainer4
            // 
            this.panelContainer4.ActiveChild = this.panelScenario;
            this.panelContainer4.Controls.Add(this.panelScenario);
            this.panelContainer4.Controls.Add(this.panelOther);
            this.panelContainer4.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.panelContainer4.ID = new System.Guid("85ae2567-3dea-475c-af9d-da3f4e7f08f9");
            resources.ApplyResources(this.panelContainer4, "panelContainer4");
            this.panelContainer4.Name = "panelContainer4";
            this.panelContainer4.OriginalSize = new System.Drawing.Size(301, 200);
            this.panelContainer4.Tabbed = true;
            this.panelContainer4.TabsPosition = DevExpress.XtraBars.Docking.TabsPosition.Left;
            // 
            // panelScenario
            // 
            this.panelScenario.Controls.Add(this.dockPanel1_Container);
            this.panelScenario.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelScenario.ID = new System.Guid("ac197e9b-7668-43be-9d4c-dece8fdffa98");
            resources.ApplyResources(this.panelScenario, "panelScenario");
            this.panelScenario.Name = "panelScenario";
            this.panelScenario.OriginalSize = new System.Drawing.Size(267, 429);
            // 
            // dockPanel1_Container
            // 
            resources.ApplyResources(this.dockPanel1_Container, "dockPanel1_Container");
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            // 
            // panelOther
            // 
            this.panelOther.Controls.Add(this.controlContainer2);
            this.panelOther.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelOther.ID = new System.Guid("a0efe638-b924-4a0a-aef0-81a62aa06a39");
            resources.ApplyResources(this.panelOther, "panelOther");
            this.panelOther.Name = "panelOther";
            this.panelOther.OriginalSize = new System.Drawing.Size(267, 429);
            // 
            // controlContainer2
            // 
            resources.ApplyResources(this.controlContainer2, "controlContainer2");
            this.controlContainer2.Name = "controlContainer2";
            // 
            // panelContainer1
            // 
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelContainer1.FloatVertical = true;
            this.panelContainer1.ID = new System.Guid("d6e9e7ff-7efe-4068-9a25-5e74b2c23de9");
            resources.ApplyResources(this.panelContainer1, "panelContainer1");
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.OriginalSize = new System.Drawing.Size(200, 200);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContainer4);
            this.Controls.Add(this.panelContainer3);
            this.Controls.Add(this.panelContainer2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.panelContainer2.ResumeLayout(false);
            this.panelImportViewCsv.ResumeLayout(false);
            this.panelBinTable.ResumeLayout(false);
            this.panelContainer3.ResumeLayout(false);
            this.panelBinSbList.ResumeLayout(false);
            this.panelProperties.ResumeLayout(false);
            this.panelContainer4.ResumeLayout(false);
            this.panelScenario.ResumeLayout(false);
            this.panelOther.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking.DockPanel panelProperties;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel3_Container;
        private DevExpress.XtraBars.Docking.DockPanel panelScenario;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking.DockPanel panelBinSbList;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.Utils.WorkspaceManager workspaceManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnPythonTest;
        private DevExpress.XtraBars.BarButtonItem btnChartTest;
        private DevExpress.XtraBars.BarButtonItem btnLogin;
        private DevExpress.XtraBars.BarButtonItem btnImportFlying;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel panelImportViewCsv;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraBars.Docking.DockPanel panelBinTable;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel4_Container;
        private DevExpress.XtraBars.BarWorkspaceMenuItem Workspace;
        private DevExpress.XtraBars.BarSubItem barSubItemTest;
        private DevExpress.XtraBars.BarButtonItem toolTreeTest;
        private UControl.ProjectListControl projectListControl;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer4;
        private DevExpress.XtraBars.Docking.DockPanel panelOther;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer2;
        private DevExpress.XtraBars.BarButtonItem btnStart;
        private DevExpress.XtraBars.BarButtonItem btnSBModule;
        private DevExpress.XtraBars.BarButtonItem btnBinModule;
        private DevExpress.XtraBars.BarButtonItem btnImportDLL;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer2;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer3;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarButtonItem btnImportAnalysis;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraBars.BarButtonItem btnMgmtParameter;
        private DevExpress.XtraBars.BarButtonItem btnMgmtPreset;
        private DevExpress.XtraBars.BarButtonItem btnMgmtPresetGroup;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem btnLPF;
        private DevExpress.XtraBars.BarButtonItem btnWebTest;
        private DevExpress.XtraBars.BarButtonItem btnTestChart;
        private DevExpress.XtraBars.BarSubItem barSubItem4;
        private DevExpress.XtraBars.BarButtonItem btnChartLine2d;
        private DevExpress.XtraBars.BarButtonItem btnChartMinMax;
        private DevExpress.XtraBars.BarButtonItem btnChartPotato;

        public TabbedView TabbedView1 { get => tabbedView1; set => tabbedView1 = value; }
    }
}