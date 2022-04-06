
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
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btnPythonTest = new DevExpress.XtraBars.BarButtonItem();
            this.toolTreeTest = new DevExpress.XtraBars.BarButtonItem();
            this.btnLogin = new DevExpress.XtraBars.BarButtonItem();
            this.btnChartTest = new DevExpress.XtraBars.BarButtonItem();
            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
            this.btnImportModule = new DevExpress.XtraBars.BarButtonItem();
            this.btnSBModule = new DevExpress.XtraBars.BarButtonItem();
            this.btnBinModule = new DevExpress.XtraBars.BarButtonItem();
            this.btnDLLImport = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.Workspace = new DevExpress.XtraBars.BarWorkspaceMenuItem();
            this.workspaceManager1 = new DevExpress.Utils.WorkspaceManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.hideContainerLeft = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.panelContainer4 = new DevExpress.XtraBars.Docking.DockPanel();
            this.panelScenario = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.projectListControl = new DynaRAP.UControl.ProjectListControl();
            this.dockPanelOther = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer2 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.hideContainerRight = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.panelPlot = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelProperties = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel3_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.hideContainerBottom = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.panelContainer2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.panelLogs2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel4_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelLogs = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.hideContainerLeft.SuspendLayout();
            this.panelContainer4.SuspendLayout();
            this.panelScenario.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.dockPanelOther.SuspendLayout();
            this.hideContainerRight.SuspendLayout();
            this.panelPlot.SuspendLayout();
            this.panelProperties.SuspendLayout();
            this.hideContainerBottom.SuspendLayout();
            this.panelContainer2.SuspendLayout();
            this.panelLogs2.SuspendLayout();
            this.panelLogs.SuspendLayout();
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
            this.btnImportModule,
            this.Workspace,
            this.barSubItem1,
            this.toolTreeTest,
            this.btnStart,
            this.btnSBModule,
            this.btnBinModule,
            this.btnDLLImport});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 17;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnChartTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnStart),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnImportModule),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSBModule),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnBinModule),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnDLLImport)});
            resources.ApplyResources(this.bar1, "bar1");
            // 
            // barSubItem1
            // 
            resources.ApplyResources(this.barSubItem1, "barSubItem1");
            this.barSubItem1.Id = 11;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPythonTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.toolTreeTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnLogin)});
            this.barSubItem1.Name = "barSubItem1";
            this.barSubItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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
            this.toolTreeTest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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
            this.btnChartTest.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnChartTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChartTest_ItemClick);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Id = 13;
            this.btnStart.Name = "btnStart";
            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStart_ItemClick);
            // 
            // btnImportModule
            // 
            resources.ApplyResources(this.btnImportModule, "btnImportModule");
            this.btnImportModule.Id = 8;
            this.btnImportModule.Name = "btnImportModule";
            this.btnImportModule.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnImportModule_ItemClick);
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
            // btnDLLImport
            // 
            resources.ApplyResources(this.btnDLLImport, "btnDLLImport");
            this.btnDLLImport.Id = 16;
            this.btnDLLImport.Name = "btnDLLImport";
            this.btnDLLImport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDLLImport_ItemClick);
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Workspace)});
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
            this.dockManager1.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerLeft,
            this.hideContainerRight,
            this.hideContainerBottom});
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
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
            // hideContainerLeft
            // 
            this.hideContainerLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.hideContainerLeft.Controls.Add(this.panelContainer4);
            resources.ApplyResources(this.hideContainerLeft, "hideContainerLeft");
            this.hideContainerLeft.Name = "hideContainerLeft";
            // 
            // panelContainer4
            // 
            this.panelContainer4.ActiveChild = this.panelScenario;
            this.panelContainer4.Controls.Add(this.panelScenario);
            this.panelContainer4.Controls.Add(this.dockPanelOther);
            this.panelContainer4.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.panelContainer4.ID = new System.Guid("85ae2567-3dea-475c-af9d-da3f4e7f08f9");
            resources.ApplyResources(this.panelContainer4, "panelContainer4");
            this.panelContainer4.Name = "panelContainer4";
            this.panelContainer4.OriginalSize = new System.Drawing.Size(301, 200);
            this.panelContainer4.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.panelContainer4.SavedIndex = 2;
            this.panelContainer4.Tabbed = true;
            this.panelContainer4.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // panelScenario
            // 
            this.panelScenario.Controls.Add(this.dockPanel1_Container);
            this.panelScenario.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelScenario.ID = new System.Guid("ac197e9b-7668-43be-9d4c-dece8fdffa98");
            resources.ApplyResources(this.panelScenario, "panelScenario");
            this.panelScenario.Name = "panelScenario";
            this.panelScenario.OriginalSize = new System.Drawing.Size(294, 571);
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.projectListControl);
            resources.ApplyResources(this.dockPanel1_Container, "dockPanel1_Container");
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            // 
            // projectListControl
            // 
            resources.ApplyResources(this.projectListControl, "projectListControl");
            this.projectListControl.Name = "projectListControl";
            // 
            // dockPanelOther
            // 
            this.dockPanelOther.Controls.Add(this.controlContainer2);
            this.dockPanelOther.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelOther.ID = new System.Guid("a0efe638-b924-4a0a-aef0-81a62aa06a39");
            resources.ApplyResources(this.dockPanelOther, "dockPanelOther");
            this.dockPanelOther.Name = "dockPanelOther";
            this.dockPanelOther.OriginalSize = new System.Drawing.Size(294, 571);
            // 
            // controlContainer2
            // 
            resources.ApplyResources(this.controlContainer2, "controlContainer2");
            this.controlContainer2.Name = "controlContainer2";
            // 
            // hideContainerRight
            // 
            this.hideContainerRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.hideContainerRight.Controls.Add(this.panelPlot);
            this.hideContainerRight.Controls.Add(this.panelProperties);
            resources.ApplyResources(this.hideContainerRight, "hideContainerRight");
            this.hideContainerRight.Name = "hideContainerRight";
            // 
            // panelPlot
            // 
            this.panelPlot.Controls.Add(this.dockPanel2_Container);
            this.panelPlot.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelPlot.ID = new System.Guid("bbd25a6d-0250-46ba-9429-84d2ef47bbbc");
            resources.ApplyResources(this.panelPlot, "panelPlot");
            this.panelPlot.Name = "panelPlot";
            this.panelPlot.OriginalSize = new System.Drawing.Size(200, 218);
            this.panelPlot.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelPlot.SavedIndex = 0;
            this.panelPlot.SavedParent = this.panelProperties;
            this.panelPlot.SavedSizeFactor = 1.0023D;
            this.panelPlot.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel2_Container
            // 
            resources.ApplyResources(this.dockPanel2_Container, "dockPanel2_Container");
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.dockPanel3_Container);
            this.panelProperties.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelProperties.ID = new System.Guid("2633c3b8-4ee7-4e73-b180-a7e2da9a6929");
            resources.ApplyResources(this.panelProperties, "panelProperties");
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelProperties.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelProperties.SavedIndex = 1;
            this.panelProperties.SavedSizeFactor = 0.9977D;
            this.panelProperties.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel3_Container
            // 
            resources.ApplyResources(this.dockPanel3_Container, "dockPanel3_Container");
            this.dockPanel3_Container.Name = "dockPanel3_Container";
            // 
            // hideContainerBottom
            // 
            this.hideContainerBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.hideContainerBottom.Controls.Add(this.panelContainer2);
            resources.ApplyResources(this.hideContainerBottom, "hideContainerBottom");
            this.hideContainerBottom.Name = "hideContainerBottom";
            // 
            // panelContainer2
            // 
            this.panelContainer2.ActiveChild = this.panelLogs2;
            this.panelContainer2.Controls.Add(this.panelLogs);
            this.panelContainer2.Controls.Add(this.panelLogs2);
            this.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer2.FloatVertical = true;
            this.panelContainer2.ID = new System.Guid("7f89a891-9919-48b6-a684-d80605a70d4a");
            resources.ApplyResources(this.panelContainer2, "panelContainer2");
            this.panelContainer2.Name = "panelContainer2";
            this.panelContainer2.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelContainer2.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer2.SavedIndex = 0;
            this.panelContainer2.Tabbed = true;
            this.panelContainer2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // panelLogs2
            // 
            this.panelLogs2.Controls.Add(this.dockPanel4_Container);
            this.panelLogs2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelLogs2.FloatVertical = true;
            this.panelLogs2.ID = new System.Guid("6dfc02a1-a41a-4100-a459-234398a2ca56");
            resources.ApplyResources(this.panelLogs2, "panelLogs2");
            this.panelLogs2.Name = "panelLogs2";
            this.panelLogs2.Options.ShowCloseButton = false;
            this.panelLogs2.OriginalSize = new System.Drawing.Size(1012, 171);
            // 
            // dockPanel4_Container
            // 
            resources.ApplyResources(this.dockPanel4_Container, "dockPanel4_Container");
            this.dockPanel4_Container.Name = "dockPanel4_Container";
            // 
            // panelLogs
            // 
            this.panelLogs.Controls.Add(this.controlContainer1);
            this.panelLogs.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelLogs.ID = new System.Guid("d9a0ca3c-ea60-4ccb-b13a-157b5c5f38f6");
            resources.ApplyResources(this.panelLogs, "panelLogs");
            this.panelLogs.Name = "panelLogs";
            this.panelLogs.Options.ShowCloseButton = false;
            this.panelLogs.OriginalSize = new System.Drawing.Size(1012, 171);
            // 
            // controlContainer1
            // 
            resources.ApplyResources(this.controlContainer1, "controlContainer1");
            this.controlContainer1.Name = "controlContainer1";
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
            this.Controls.Add(this.hideContainerBottom);
            this.Controls.Add(this.hideContainerLeft);
            this.Controls.Add(this.hideContainerRight);
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
            this.hideContainerLeft.ResumeLayout(false);
            this.panelContainer4.ResumeLayout(false);
            this.panelScenario.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.dockPanelOther.ResumeLayout(false);
            this.hideContainerRight.ResumeLayout(false);
            this.panelPlot.ResumeLayout(false);
            this.panelProperties.ResumeLayout(false);
            this.hideContainerBottom.ResumeLayout(false);
            this.panelContainer2.ResumeLayout(false);
            this.panelLogs2.ResumeLayout(false);
            this.panelLogs.ResumeLayout(false);
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
        private DevExpress.XtraBars.Docking.DockPanel panelPlot;
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
        private DevExpress.XtraBars.BarButtonItem btnImportModule;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel panelLogs;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer2;
        private DevExpress.XtraBars.Docking.DockPanel panelLogs2;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel4_Container;
        private DevExpress.XtraBars.BarWorkspaceMenuItem Workspace;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem toolTreeTest;
        private UControl.ProjectListControl projectListControl;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerLeft;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer4;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelOther;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer2;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerRight;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerBottom;
        private DevExpress.XtraBars.BarButtonItem btnStart;
        private DevExpress.XtraBars.BarButtonItem btnSBModule;
        private DevExpress.XtraBars.BarButtonItem btnBinModule;
        private DevExpress.XtraBars.BarButtonItem btnDLLImport;
    }
}