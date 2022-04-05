using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DynaRAP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class ImportModuleControl : DevExpress.XtraEditors.XtraUserControl
    {
        string selectedFuselage = string.Empty;
        Dictionary<string, List<string>> dicData = new Dictionary<string, List<string>>();

        DockPanel csvTablePanel = null;
        CsvTableControl csvTableCtrl = null;
        string csvFilePath = string.Empty;

        public ImportModuleControl()
        {
            InitializeComponent();
        }

        private void ImportModuleControl_Load(object sender, EventArgs e)
        {
            InitializeFuselageList();
            //InitializeSplittedRegionList();

            DateTime dtNow = DateTime.Now;
            string strNow = string.Format("{0:yyyy-MM-dd}", dtNow);
            dateScenario.Text = strNow;

            panelData.AutoScroll = true;
            panelData.WrapContents = false;
            panelData.HorizontalScroll.Visible = false;
            panelData.VerticalScroll.Visible = true;

            btnViewData.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddParameter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnAddSplittedInterval.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnSaveSplittedInterval.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            btnViewData.Properties.AllowFocused = false;
            btnAddParameter.Properties.AllowFocused = false;
            btnAddSplittedInterval.Properties.AllowFocused = false;
            btnSaveSplittedInterval.Properties.AllowFocused = false;

        }

        private void InitializeFuselageList()
        {
            AddFuselage("형상 A/1호기");
            AddFuselage("형상 A/2호기");
            AddFuselage("형상 A/3호기");
            AddFuselage("형상 B/1호기");
            AddFuselage("형상 C/1호기");
            AddFuselage("형상 D/1호기");
            AddFuselage("형상 E/1호기");
            AddFuselage("형상 F/1호기");
            AddFuselage("형상 G/1호기");
            AddFuselage("형상 H/1호기");
            AddFuselage("형상 Z/2호기");

        }

        private void AddFuselage(string name)
        {
            ButtonEdit edit = new ButtonEdit();
            edit.BorderStyle = BorderStyles.Simple;
            edit.ForeColor = Color.Gray;
            edit.Properties.Appearance.BorderColor = Color.Gray;
            edit.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            //edit.ReadOnly = true;
            edit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            edit.Properties.AllowFocused = false;
            edit.Properties.Buttons[0].Visible = false;
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
                if (selectedFuselage.Equals(edit.Text))
                {
                    return;
                }

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
                selectedFuselage = edit.Text;
            }
        }

        //private void InitializeSplittedRegionList()
        //{
        //    List<SplittedInterval> list = new List<SplittedInterval>();
        //    DateTime dtNow = DateTime.Now;
        //    string strNow = string.Format("{0:HH:mm:ss}", dtNow);

        //    list.Add(new SplittedInterval("비행구간#1", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#2", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#3", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#4", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#5", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#1", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#2", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#3", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#4", strNow, strNow, 1));
        //    list.Add(new SplittedInterval("비행구간#5", strNow, strNow, 1));

        //    this.gridControl1.DataSource = list;

        //    gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

        //    gridView1.OptionsView.ShowColumnHeaders = true;
        //    gridView1.OptionsView.ShowGroupPanel = false;
        //    gridView1.OptionsView.ShowIndicator = false;
        //    gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
        //    gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
        //    gridView1.OptionsView.ColumnAutoWidth = false;

        //    gridView1.OptionsBehavior.ReadOnly = true;
        //    //gridView1.OptionsBehavior.Editable = false;

        //    gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
        //    gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

        //    GridColumn colName = gridView1.Columns["IntervalName"];
        //    colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
        //    colName.OptionsColumn.FixedWidth = true;
        //    colName.Width = 130;
        //    colName.Caption = "구간이름";

        //    GridColumn colStartTime = gridView1.Columns["StartTime"];
        //    colStartTime.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
        //    colStartTime.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
        //    colStartTime.OptionsColumn.FixedWidth = true;
        //    colStartTime.Width = 130;
        //    colStartTime.Caption = "시작시간";

        //    GridColumn colEndTime = gridView1.Columns["EndTime"];
        //    colEndTime.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
        //    colEndTime.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
        //    colEndTime.OptionsColumn.FixedWidth = true;
        //    colEndTime.Width = 130;
        //    colEndTime.Caption = "종료시간";

        //    GridColumn colView = gridView1.Columns["View"];
        //    colView.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
        //    colView.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
        //    colView.OptionsColumn.FixedWidth = true;
        //    colView.Width = 130;
        //    colView.Caption = "보기";

        //    this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
        //    this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

        //    this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
        //    this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

        //    this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;

        //}

        //private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        //{
        //    RepositoryItemImageComboBox combo = sender as RepositoryItemImageComboBox;

        //}

        private void edtScenarioName_ClearButtonClick(object sender, ButtonPressedEventArgs e)
        {
            edtScenarioName.Text = String.Empty;

        }

        private void edtTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }

        private void edtTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                addTag(me.Text);
                me.Text = String.Empty;
            }
        }

        private void addTag(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            ButtonEdit btn = new ButtonEdit();
            btn.Properties.Buttons[0].Kind = ButtonPredefines.Close;
            btn.BorderStyle = BorderStyles.Simple;
            btn.ForeColor = Color.White;
            btn.Properties.Appearance.BorderColor = Color.White;
            btn.Font = new Font(btn.Font, FontStyle.Bold);
            btn.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            btn.ReadOnly = true;
            btn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btn.Properties.AllowFocused = false;
            btn.ButtonClick += removeTag_ButtonClick;
            btn.Text = name;
            panelTag.Controls.Add(btn);
        }

        private void removeTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit btn = sender as ButtonEdit;
            panelTag.Controls.Remove(btn);

        }

        private void btnViewData_ButtonClick(object sender, EventArgs e)
        {
            if (File.Exists(csvFilePath) == false)
            {
                MessageBox.Show(Properties.Resources.FileNotExist);
                return;
            }

            MainForm mainForm = this.ParentForm as MainForm;

            //아래에 panel 추가
            if (csvTablePanel == null)
            {
                csvTablePanel = mainForm.DockManager1.AddPanel(DockingStyle.Bottom);
                csvTablePanel.Name = "panelCSVTable";
                csvTablePanel.Text = "CSV TABLE";
                csvTablePanel.Height = 240;
                csvTableCtrl = new CsvTableControl(csvFilePath);
                csvTableCtrl.Dock = DockStyle.Fill;
                csvTablePanel.Controls.Add(csvTableCtrl);
                csvTablePanel.ClosedPanel += CsvTablePanel_ClosedPanel;
            }
            else
            {
                csvTableCtrl.CsvFilePath = csvFilePath;
                csvTableCtrl.FillGrid();
                csvTablePanel.Show();
            }
        }

        private void CsvTablePanel_ClosedPanel(object sender, DockPanelEventArgs e)
        {
            this.csvTablePanel = null;
            this.csvTableCtrl = null;
        }

        private void btnAddParameter_ButtonClick(object sender, EventArgs e)
        {
            AddParameter();
        }

        int paramIndex = 23;

        private void AddParameter()
        {
            ImportParamControl ctrl = new ImportParamControl();
            ctrl.Title = "Parameter " + paramIndex.ToString();
            ctrl.DicData = dicData;
            panelData.Controls.Add(ctrl);
            panelData.Controls.SetChildIndex(ctrl, paramIndex++);

        }

        private void btnAddSplittedInterval_ButtonClick(object sender, EventArgs e)
        {
            AddSplittedInterval();
        }

        int intervalIndex = 6;

        private void AddSplittedInterval()
        {
            ImportIntervalControl ctrl = new ImportIntervalControl();
            ctrl.Title = "비행구간#" + (paramIndex + intervalIndex).ToString();
            panelData.Controls.Add(ctrl);
            panelData.Controls.SetChildIndex(ctrl, paramIndex + intervalIndex);
            intervalIndex++;

        }

        private void btnSaveSplittedInterval_ButtonClick(object sender, ButtonPressedEventArgs e)
        {

        }

        private void btnSaveSplittedInterval_ButtonClick(object sender, EventArgs e)
        {

        }

        private void lblFlyingData_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "C:\\";
            dlg.Filter = "Excel files (*.xls, *.xlsx)|*.xls; *.xlsx|Comma Separated Value files (CSV)|*.csv|모든 파일 (*.*)|*.*";
            //dlg.Filter = "Comma Separated Value files (CSV)|*.csv";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                csvFilePath = dlg.FileName;
                lblFlyingData.Text = csvFilePath;
                StreamReader sr = new StreamReader(dlg.FileName);

                int idx = 0;

                // 스트림의 끝까지 읽기
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] data = line.Split(',');

                    if (string.IsNullOrEmpty(data[0]))
                        continue;

                    int i = 0;
                    if (idx == 0)
                    {
                        dicData.Clear();
                        for (i = 0; i < data.Length; i++)
                        {
                            if (dicData.ContainsKey(data[i]) == false)
                            {
                                if (string.IsNullOrEmpty(data[i]) == false)
                                    dicData.Add(data[i], new List<string>());
                            }
                        }
                        idx++;
                        continue;
                    }

                    i = 0;
                    foreach (string key in dicData.Keys)
                    {
                        if (dicData.ContainsKey(key))
                        {
                            if (string.IsNullOrEmpty(data[i]) == false)
                                dicData[key].Add(data[i++]);
                        }
                    }
                }

                //foreach (KeyValuePair<string, List<string>> kv in dicData)
                //{
                //    Console.Write("{0} : ", kv.Key);
                //    foreach (string val in kv.Value)
                //    {
                //        Console.Write("{0} ", val);
                //    }
                //    Console.WriteLine();
                //}

            }
        }
    }

    
}
