
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
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            DevExpress.Utils.Animation.PushTransition pushTransition1 = new DevExpress.Utils.Animation.PushTransition();
            this.documentGroup1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
            this.userControl1Document = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.userControl2Document = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnPythonTest = new DevExpress.XtraBars.BarButtonItem();
            this.btnChartTest = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.hideContainerLeft = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.hideContainerRight = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.dockPanel2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanel3 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel3_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel5 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel5_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanel4 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel4_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanel6 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel6_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.workspaceManager1 = new DevExpress.Utils.WorkspaceManager(this.components);
            this.btnLogin = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userControl1Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userControl2Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.hideContainerLeft.SuspendLayout();
            this.dockPanel1.SuspendLayout();
            this.hideContainerRight.SuspendLayout();
            this.dockPanel2.SuspendLayout();
            this.dockPanel3.SuspendLayout();
            this.panelContainer1.SuspendLayout();
            this.dockPanel5.SuspendLayout();
            this.dockPanel4.SuspendLayout();
            this.dockPanel6.SuspendLayout();
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
            this.btnLogin});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 8;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPythonTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnChartTest),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnLogin)});
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
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
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
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
            this.barDockControlTop.Size = new System.Drawing.Size(1090, 47);
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
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 47);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 636);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1090, 47);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 636);
            // 
            // dockManager1
            // 
            this.dockManager1.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerLeft,
            this.hideContainerRight});
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.panelContainer1});
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
            this.hideContainerLeft.Controls.Add(this.dockPanel1);
            this.hideContainerLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.hideContainerLeft.Location = new System.Drawing.Point(0, 47);
            this.hideContainerLeft.Name = "hideContainerLeft";
            this.hideContainerLeft.Size = new System.Drawing.Size(36, 636);
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.ID = new System.Guid("ac197e9b-7668-43be-9d4c-dece8fdffa98");
            this.dockPanel1.Location = new System.Drawing.Point(36, 46);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.SavedIndex = 0;
            this.dockPanel1.Size = new System.Drawing.Size(200, 637);
            this.dockPanel1.Text = "dockPanel1";
            this.dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Location = new System.Drawing.Point(3, 25);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(193, 609);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // hideContainerRight
            // 
            this.hideContainerRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.hideContainerRight.Controls.Add(this.dockPanel2);
            this.hideContainerRight.Controls.Add(this.dockPanel3);
            this.hideContainerRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.hideContainerRight.Location = new System.Drawing.Point(1054, 47);
            this.hideContainerRight.Name = "hideContainerRight";
            this.hideContainerRight.Size = new System.Drawing.Size(36, 636);
            // 
            // dockPanel2
            // 
            this.dockPanel2.Controls.Add(this.dockPanel2_Container);
            this.dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel2.ID = new System.Guid("bbd25a6d-0250-46ba-9429-84d2ef47bbbc");
            this.dockPanel2.Location = new System.Drawing.Point(854, 46);
            this.dockPanel2.Name = "dockPanel2";
            this.dockPanel2.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel2.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel2.SavedIndex = 0;
            this.dockPanel2.SavedParent = this.dockPanel3;
            this.dockPanel2.Size = new System.Drawing.Size(200, 637);
            this.dockPanel2.Text = "dockPanel2";
            this.dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Location = new System.Drawing.Point(4, 25);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(193, 609);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // dockPanel3
            // 
            this.dockPanel3.Controls.Add(this.dockPanel3_Container);
            this.dockPanel3.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel3.ID = new System.Guid("2633c3b8-4ee7-4e73-b180-a7e2da9a6929");
            this.dockPanel3.Location = new System.Drawing.Point(854, 46);
            this.dockPanel3.Name = "dockPanel3";
            this.dockPanel3.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel3.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel3.SavedIndex = 0;
            this.dockPanel3.Size = new System.Drawing.Size(200, 637);
            this.dockPanel3.Text = "dockPanel3";
            this.dockPanel3.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel3_Container
            // 
            this.dockPanel3_Container.Location = new System.Drawing.Point(4, 25);
            this.dockPanel3_Container.Name = "dockPanel3_Container";
            this.dockPanel3_Container.Size = new System.Drawing.Size(193, 609);
            this.dockPanel3_Container.TabIndex = 0;
            // 
            // panelContainer1
            // 
            this.panelContainer1.ActiveChild = this.dockPanel5;
            this.panelContainer1.Controls.Add(this.dockPanel4);
            this.panelContainer1.Controls.Add(this.dockPanel5);
            this.panelContainer1.Controls.Add(this.dockPanel6);
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer1.ID = new System.Guid("a1fdf501-3845-4e92-8538-24821d750144");
            this.panelContainer1.Location = new System.Drawing.Point(36, 483);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelContainer1.Size = new System.Drawing.Size(1018, 200);
            this.panelContainer1.Tabbed = true;
            this.panelContainer1.Text = "panelContainer1";
            // 
            // dockPanel5
            // 
            this.dockPanel5.Controls.Add(this.dockPanel5_Container);
            this.dockPanel5.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel5.FloatVertical = true;
            this.dockPanel5.ID = new System.Guid("13182def-b1d0-41ec-b4bd-1afc4384c06f");
            this.dockPanel5.Location = new System.Drawing.Point(3, 26);
            this.dockPanel5.Name = "dockPanel5";
            this.dockPanel5.OriginalSize = new System.Drawing.Size(1012, 145);
            this.dockPanel5.Size = new System.Drawing.Size(1012, 145);
            this.dockPanel5.Text = "dockPanel5";
            // 
            // dockPanel5_Container
            // 
            this.dockPanel5_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel5_Container.Name = "dockPanel5_Container";
            this.dockPanel5_Container.Size = new System.Drawing.Size(1012, 145);
            this.dockPanel5_Container.TabIndex = 0;
            // 
            // dockPanel4
            // 
            this.dockPanel4.Controls.Add(this.dockPanel4_Container);
            this.dockPanel4.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel4.ID = new System.Guid("e9028b11-7301-4291-9df1-611c12f7e6e9");
            this.dockPanel4.Location = new System.Drawing.Point(3, 26);
            this.dockPanel4.Name = "dockPanel4";
            this.dockPanel4.OriginalSize = new System.Drawing.Size(1012, 145);
            this.dockPanel4.Size = new System.Drawing.Size(1012, 145);
            this.dockPanel4.Text = "dockPanel4";
            // 
            // dockPanel4_Container
            // 
            this.dockPanel4_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel4_Container.Name = "dockPanel4_Container";
            this.dockPanel4_Container.Size = new System.Drawing.Size(1012, 145);
            this.dockPanel4_Container.TabIndex = 0;
            // 
            // dockPanel6
            // 
            this.dockPanel6.Controls.Add(this.dockPanel6_Container);
            this.dockPanel6.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel6.FloatVertical = true;
            this.dockPanel6.ID = new System.Guid("7f7fc57f-b438-478b-aace-bbcbed29d3e6");
            this.dockPanel6.Location = new System.Drawing.Point(3, 26);
            this.dockPanel6.Name = "dockPanel6";
            this.dockPanel6.OriginalSize = new System.Drawing.Size(1012, 145);
            this.dockPanel6.Size = new System.Drawing.Size(1012, 145);
            this.dockPanel6.Text = "dockPanel6";
            // 
            // dockPanel6_Container
            // 
            this.dockPanel6_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel6_Container.Name = "dockPanel6_Container";
            this.dockPanel6_Container.Size = new System.Drawing.Size(1012, 145);
            this.dockPanel6_Container.TabIndex = 0;
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
            // workspaceManager1
            // 
            this.workspaceManager1.TargetControl = this;
            this.workspaceManager1.TransitionType = pushTransition1;
            // 
            // btnLogin
            // 
            this.btnLogin.Caption = "Login";
            this.btnLogin.Id = 7;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLogin_ItemClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 705);
            this.Controls.Add(this.panelContainer1);
            this.Controls.Add(this.hideContainerLeft);
            this.Controls.Add(this.hideContainerRight);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "MainForm";
            this.Text = "DynaRAP";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userControl1Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userControl2Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.hideContainerLeft.ResumeLayout(false);
            this.dockPanel1.ResumeLayout(false);
            this.hideContainerRight.ResumeLayout(false);
            this.dockPanel2.ResumeLayout(false);
            this.dockPanel3.ResumeLayout(false);
            this.panelContainer1.ResumeLayout(false);
            this.dockPanel5.ResumeLayout(false);
            this.dockPanel4.ResumeLayout(false);
            this.dockPanel6.ResumeLayout(false);
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
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel5;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel5_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel4;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel4_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel6;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel6_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel3;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel3_Container;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerLeft;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerRight;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel2;
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
    }
}