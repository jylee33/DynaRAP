
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
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            this.documentGroup1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
            this.userControl1Document = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.userControl2Document = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btnPythonTest = new DevExpress.XtraBars.BarButtonItem();
            this.btnChartTest = new DevExpress.XtraBars.BarButtonItem();
            this.btnLogin = new DevExpress.XtraBars.BarButtonItem();
            this.btnPanel = new DevExpress.XtraBars.BarButtonItem();
            this.toolTreeTest = new DevExpress.XtraBars.BarButtonItem();
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
            this.panelLogs2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel4_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelLogs = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelContainer3 = new DevExpress.XtraBars.Docking.DockPanel();
            this.panelPlot = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelProperties = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel3_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelScenario = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.projectListControl = new DynaRAP.UControl.ProjectListControl();
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userControl1Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userControl2Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.panelContainer2.SuspendLayout();
            this.panelLogs2.SuspendLayout();
            this.panelLogs.SuspendLayout();
            this.panelContainer3.SuspendLayout();
            this.panelPlot.SuspendLayout();
            this.panelProperties.SuspendLayout();
            this.panelScenario.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentGroup1
            // 
            this.documentGroup1.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document[] {
            this.userControl1Document,
            this.userControl2Document});
            // 
            // userControl1Document
            // 
            this.userControl1Document.Caption = "UserControl1";
            this.userControl1Document.ControlName = "UserControl1";
            this.userControl1Document.ControlTypeName = "DynaRAP.UserControl1";
            // 
            // userControl2Document
            // 
            this.userControl2Document.Caption = "UserControl2";
            this.userControl2Document.ControlName = "UserControl2";
            this.userControl2Document.ControlTypeName = "DynaRAP.UserControl2";
            this.userControl2Document.FloatLocation = new System.Drawing.Point(205, 110);
            this.userControl2Document.FloatSize = new System.Drawing.Size(1012, 608);
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
            this.btnPanel,
            this.Workspace,
            this.barSubItem1,
            this.toolTreeTest});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 13;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1)});
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "TEST";
            this.barSubItem1.Id = 11;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPythonTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnChartTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnLogin),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPanel),
            new DevExpress.XtraBars.LinkPersistInfo(this.toolTreeTest)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btnPythonTest
            // 
            this.btnPythonTest.Caption = "Python";
            this.btnPythonTest.Id = 5;
            this.btnPythonTest.Name = "btnPythonTest";
            this.btnPythonTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPythonTest_ItemClick);
            // 
            // btnChartTest
            // 
            this.btnChartTest.Caption = "Chart";
            this.btnChartTest.Id = 6;
            this.btnChartTest.Name = "btnChartTest";
            this.btnChartTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChartTest_ItemClick);
            // 
            // btnLogin
            // 
            this.btnLogin.Caption = "Login";
            this.btnLogin.Id = 7;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLogin_ItemClick);
            // 
            // btnPanel
            // 
            this.btnPanel.Caption = "Test";
            this.btnPanel.Id = 8;
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPanel_ItemClick);
            // 
            // toolTreeTest
            // 
            this.toolTreeTest.Caption = "Tree";
            this.toolTreeTest.Id = 12;
            this.toolTreeTest.Name = "toolTreeTest";
            this.toolTreeTest.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.toolTreeTest_ItemClick);
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
            this.bar2.Text = "Main menu";
            // 
            // Workspace
            // 
            this.Workspace.Caption = "Workspace";
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
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1090, 48);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 683);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1090, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 48);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 635);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1090, 48);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 635);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.panelContainer2,
            this.panelContainer3,
            this.panelScenario});
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
            this.panelContainer2.ActiveChild = this.panelLogs2;
            this.panelContainer2.Controls.Add(this.panelLogs);
            this.panelContainer2.Controls.Add(this.panelLogs2);
            this.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer2.FloatVertical = true;
            this.panelContainer2.ID = new System.Guid("7f89a891-9919-48b6-a684-d80605a70d4a");
            this.panelContainer2.Location = new System.Drawing.Point(0, 483);
            this.panelContainer2.Name = "panelContainer2";
            this.panelContainer2.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelContainer2.Size = new System.Drawing.Size(1090, 200);
            this.panelContainer2.Tabbed = true;
            this.panelContainer2.Text = "panelContainer2";
            // 
            // panelLogs2
            // 
            this.panelLogs2.Controls.Add(this.dockPanel4_Container);
            this.panelLogs2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelLogs2.FloatVertical = true;
            this.panelLogs2.ID = new System.Guid("6dfc02a1-a41a-4100-a459-234398a2ca56");
            this.panelLogs2.Location = new System.Drawing.Point(3, 26);
            this.panelLogs2.Name = "panelLogs2";
            this.panelLogs2.Options.ShowCloseButton = false;
            this.panelLogs2.OriginalSize = new System.Drawing.Size(684, 145);
            this.panelLogs2.Size = new System.Drawing.Size(1084, 145);
            this.panelLogs2.Text = "Logs";
            // 
            // dockPanel4_Container
            // 
            this.dockPanel4_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel4_Container.Name = "dockPanel4_Container";
            this.dockPanel4_Container.Size = new System.Drawing.Size(1084, 145);
            this.dockPanel4_Container.TabIndex = 0;
            // 
            // panelLogs
            // 
            this.panelLogs.Controls.Add(this.controlContainer1);
            this.panelLogs.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelLogs.ID = new System.Guid("d9a0ca3c-ea60-4ccb-b13a-157b5c5f38f6");
            this.panelLogs.Location = new System.Drawing.Point(3, 26);
            this.panelLogs.Name = "panelLogs";
            this.panelLogs.Options.ShowCloseButton = false;
            this.panelLogs.OriginalSize = new System.Drawing.Size(684, 145);
            this.panelLogs.Size = new System.Drawing.Size(1084, 145);
            this.panelLogs.Text = "Logs";
            // 
            // controlContainer1
            // 
            this.controlContainer1.Location = new System.Drawing.Point(0, 0);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(1084, 145);
            this.controlContainer1.TabIndex = 0;
            // 
            // panelContainer3
            // 
            this.panelContainer3.Controls.Add(this.panelPlot);
            this.panelContainer3.Controls.Add(this.panelProperties);
            this.panelContainer3.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelContainer3.ID = new System.Guid("83ff3d44-760d-489a-942b-9dce38a34268");
            this.panelContainer3.Location = new System.Drawing.Point(890, 48);
            this.panelContainer3.Name = "panelContainer3";
            this.panelContainer3.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelContainer3.Size = new System.Drawing.Size(200, 435);
            this.panelContainer3.Text = "panelContainer3";
            // 
            // panelPlot
            // 
            this.panelPlot.Controls.Add(this.dockPanel2_Container);
            this.panelPlot.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelPlot.ID = new System.Guid("bbd25a6d-0250-46ba-9429-84d2ef47bbbc");
            this.panelPlot.Location = new System.Drawing.Point(0, 0);
            this.panelPlot.Name = "panelPlot";
            this.panelPlot.OriginalSize = new System.Drawing.Size(200, 318);
            this.panelPlot.Size = new System.Drawing.Size(200, 218);
            this.panelPlot.Text = "Plot";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Location = new System.Drawing.Point(4, 25);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(193, 189);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.dockPanel3_Container);
            this.panelProperties.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelProperties.ID = new System.Guid("2633c3b8-4ee7-4e73-b180-a7e2da9a6929");
            this.panelProperties.Location = new System.Drawing.Point(0, 218);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.OriginalSize = new System.Drawing.Size(200, 317);
            this.panelProperties.Size = new System.Drawing.Size(200, 217);
            this.panelProperties.Text = "Properties";
            // 
            // dockPanel3_Container
            // 
            this.dockPanel3_Container.Location = new System.Drawing.Point(4, 25);
            this.dockPanel3_Container.Name = "dockPanel3_Container";
            this.dockPanel3_Container.Size = new System.Drawing.Size(193, 189);
            this.dockPanel3_Container.TabIndex = 0;
            // 
            // panelScenario
            // 
            this.panelScenario.Controls.Add(this.dockPanel1_Container);
            this.panelScenario.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.panelScenario.ID = new System.Guid("ac197e9b-7668-43be-9d4c-dece8fdffa98");
            this.panelScenario.Location = new System.Drawing.Point(0, 48);
            this.panelScenario.Name = "panelScenario";
            this.panelScenario.OriginalSize = new System.Drawing.Size(301, 200);
            this.panelScenario.Size = new System.Drawing.Size(301, 435);
            this.panelScenario.Text = "Scenario";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.projectListControl);
            this.dockPanel1_Container.Location = new System.Drawing.Point(3, 25);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(294, 407);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // projectListControl
            // 
            this.projectListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectListControl.Location = new System.Drawing.Point(0, 0);
            this.projectListControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.projectListControl.Name = "projectListControl";
            this.projectListControl.Size = new System.Drawing.Size(294, 407);
            this.projectListControl.TabIndex = 0;
            // 
            // tabbedView1
            // 
            this.tabbedView1.DocumentGroups.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup[] {
            this.documentGroup1});
            this.tabbedView1.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.userControl1Document,
            this.userControl2Document});
            dockingContainer1.Element = this.documentGroup1;
            this.tabbedView1.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            dockingContainer1});
            // 
            // panelContainer1
            // 
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelContainer1.FloatVertical = true;
            this.panelContainer1.ID = new System.Guid("d6e9e7ff-7efe-4068-9a25-5e74b2c23de9");
            this.panelContainer1.Location = new System.Drawing.Point(36, 483);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelContainer1.Size = new System.Drawing.Size(1018, 200);
            this.panelContainer1.Text = "panelContainer1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 705);
            this.Controls.Add(this.panelScenario);
            this.Controls.Add(this.panelContainer3);
            this.Controls.Add(this.panelContainer2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "MainForm";
            this.Text = "DynaRAP";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userControl1Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userControl2Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.panelContainer2.ResumeLayout(false);
            this.panelLogs2.ResumeLayout(false);
            this.panelLogs.ResumeLayout(false);
            this.panelContainer3.ResumeLayout(false);
            this.panelPlot.ResumeLayout(false);
            this.panelProperties.ResumeLayout(false);
            this.panelScenario.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document userControl1Document;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document userControl2Document;
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
        private DevExpress.XtraBars.BarButtonItem btnPanel;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel panelLogs;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer2;
        private DevExpress.XtraBars.Docking.DockPanel panelLogs2;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel4_Container;
        private DevExpress.XtraBars.BarWorkspaceMenuItem Workspace;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer3;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem toolTreeTest;
        private UControl.ProjectListControl projectListControl;
    }
}