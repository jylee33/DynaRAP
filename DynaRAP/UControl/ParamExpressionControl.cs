using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DynaRAP.Data;
using DynaRAP.UTIL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class ParamExpressionControl : DevExpress.XtraEditors.XtraUserControl
    {
        private string paramModuleSeq = null;
        private List<EquationGridData> eqGridDataList = null;
        ParameterModuleControl parameterModuleControl = null;
        private List<double> chartData = null;
        ChartControl chartControl = null;
        DataTable dt = null;
        DockPanel panelChart = null;

        public ParamExpressionControl(ParameterModuleControl parameterModuleControl)
        {
            this.parameterModuleControl = parameterModuleControl;
            InitializeComponent();
        }

        private void ParamExpressionControl_Load(object sender, EventArgs e)
        {
            InitializeGridControl();
        }
        private void InitializeGridControl()
        {
            GridColumn colEQNo = gridView1.Columns["eqNo"];
            colEQNo.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colEQNo.OptionsColumn.FixedWidth = true;
            colEQNo.Width = 80;
            colEQNo.OptionsColumn.ReadOnly = true;

            GridColumn colView = gridView1.Columns["View"];
            colView.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colView.OptionsColumn.FixedWidth = true;
            colView.Width = 40;
            colView.Caption = "보기";
            colView.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox1.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox1.Buttons[0].Visible = false;

            this.repositoryItemImageComboBox1.Click += RepositoryItemImageComboBox1_Click;



            GridColumn colDel = gridView1.Columns["Del"];
            colDel.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            colDel.OptionsColumn.FixedWidth = true;
            colDel.Width = 40;
            colDel.Caption = "삭제";
            colDel.OptionsColumn.ReadOnly = true;

            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(0, 0));
            this.repositoryItemImageComboBox2.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(1, 1));

            this.repositoryItemImageComboBox2.GlyphAlignment = HorzAlignment.Center;
            this.repositoryItemImageComboBox2.Buttons[0].Visible = false;
            this.repositoryItemImageComboBox2.Click += RepositoryItemImageComboBox2_Click;

            chartData = new List<double>();

        }
        private void RepositoryItemImageComboBox1_Click(object sender, EventArgs e)
        {
            EquationGridData selectGridData = (EquationGridData)gridView1.GetFocusedRow();
            eqGridDataList.Remove(selectGridData);
            if (selectGridData.Seq != null && selectGridData.Seq != "")
            {
                SaveEquationRequest("delOne");
            }
            this.gridControl1.DataSource = eqGridDataList;
            gridView1.RefreshData();

        }

        private void RepositoryItemImageComboBox2_Click(object sender, EventArgs e)
        {
            EquationGridData selectGridData = (EquationGridData)gridView1.GetFocusedRow();
            MainForm mainForm = this.ParentForm as MainForm;
            string sendData = string.Empty;

            if (selectGridData.Seq != null && selectGridData.Seq != "")
            {
                sendData = string.Format(@"
                {{
                ""command"":""eq-data"",
                ""moduleSeq"": ""{0}"",
                ""eqSeq"" : ""{1}""
                }}", paramModuleSeq, selectGridData.Seq);
            }
            else
            {
                sendData = string.Format(@"
                {{
                ""command"":""evaluation"",
                ""moduleSeq"": ""{0}"",
                ""equation"" : ""{1}""
                }}", paramModuleSeq, selectGridData.equation);
            }
            mainForm.ShowSplashScreenManager("수식을 데이터를 불러오는 중입니다. 잠시만 기다려주십시오.");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            mainForm.HideSplashScreenManager();
            if (responseData != null)
            {
                EvaluationResponse evaluationResponse = JsonConvert.DeserializeObject<EvaluationResponse>(responseData);
                if (evaluationResponse.response != null && evaluationResponse.response.Count() != 0)
                {
                    dt = GetChartValues(evaluationResponse.response);
                    AddChartData(selectGridData.eqName);
                }
            }
        }

        public void SetSelectDataSource(string paramModuleSeq)
        {
            this.paramModuleSeq = paramModuleSeq;
            GetSelectDataList(paramModuleSeq);
        }
        private void GetSelectDataList(string paramModuleSeq)
        {
            MainForm mainForm = this.ParentForm as MainForm;
            string sendData = string.Format(@"
                {{
                ""command"":""eq-list"",
                ""moduleSeq"": ""{0}""
                }}", paramModuleSeq);
            mainForm.ShowSplashScreenManager("수식을 불러오는 중입니다. 잠시만 기다려주십시오.");
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            mainForm.HideSplashScreenManager();
            if (responseData != null)
            {
                eqGridDataList = new List<EquationGridData>();
                EquationResponse equationResponse = JsonConvert.DeserializeObject<EquationResponse>(responseData);
                if (equationResponse.response != null && equationResponse.response.Count() != 0)
                {
                    foreach (var list in equationResponse.response)
                    {
                        list.eqName = Utils.base64StringDecoding(list.eqName);
                        eqGridDataList.Add(new EquationGridData(list));
                    }
                }
                this.gridControl1.DataSource = eqGridDataList;
                gridView1.RefreshData();
            }
        }

        private void btnAddEQ_Click(object sender, EventArgs e)
        {

            if (paramModuleSeq == null)
            {
                MessageBox.Show("파라미터모듈을 먼저 선택 후 진행해주세요.");
            }
            else
            {
                if (eqGridDataList == null)
                {
                    eqGridDataList = new List<EquationGridData>();
                }
                eqGridDataList.Add(new EquationGridData());
                this.gridControl1.DataSource = eqGridDataList;
                gridView1.RefreshData();
            }
        }

        private void edtTag_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonEdit me = sender as ButtonEdit;
            if (me != null)
            {
                EquationGridData selectGridData = (EquationGridData)gridView1.GetFocusedRow();
                if (selectGridData.tags != null && selectGridData.tags != "")
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", selectGridData.tags + "|" + me.Text);
                }
                else
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", me.Text);

                }
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
                EquationGridData selectGridData = (EquationGridData)gridView1.GetFocusedRow();
                if (selectGridData.tags != null && selectGridData.tags != "")
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", selectGridData.tags + "|" + me.Text);
                }
                else
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", me.Text);

                }
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
            //btn.ReadOnly = true;
            btn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btn.Properties.AllowFocused = false;
            btn.ButtonClick += removeTag_ButtonClick;
            btn.Text = name;
            panelTag.Controls.Add(btn);
        }

        private void removeTag_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit btn = sender as ButtonEdit;
            EquationGridData selectGridData = (EquationGridData)gridView1.GetFocusedRow();
            List<string> tagList = new List<string>();
            if (selectGridData.tags != null && selectGridData.tags != "")
            {
                tagList = selectGridData.tags.Split('|').ToList();
            }
            tagList.Remove(btn.Text);
            string tags = "";
            foreach (var tag in tagList)
            {
                tags += (tag + "|");
            }
            if (tags != "")
            {
                tags = tags.Substring(0, tags.Length - 1);
            }
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", tags);
            panelTag.Controls.Remove(btn);

        }
        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            EquationGridData selectGridData = (EquationGridData)gridView1.GetFocusedRow();
            if (selectGridData.tags != null && selectGridData.tags != "")
            {
                string[] tagList = selectGridData.tags.Split('|');

                panelTag.Controls.Clear();
                foreach (string value in tagList)
                {
                    addTag(value);
                }
            }
            else
            {
                panelTag.Controls.Clear();
            }
        }

        private void btnSaveExpression_Click(object sender, EventArgs e)
        {
            SaveEquationRequest("save");
        }
        private void SaveEquationRequest(string type)
        {
            EquationSaveRequest equationRequest = new EquationSaveRequest();
            equationRequest.command = "save-eq";
            equationRequest.moduleSeq = paramModuleSeq;
            equationRequest.equations = new List<Equations>();
            List<EquationGridData> gridDataList = (List<EquationGridData>)this.gridControl1.DataSource;
            MainForm mainForm = this.ParentForm as MainForm;

            foreach (var list in gridDataList)
            {
                if (list.eqName == null || list.eqName == "")
                {
                    MessageBox.Show("수식이름 중 비어있는 항목이 있습니다. \n수식이름 입력 후 저장해주세요.");
                    return;
                }
                byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(list.eqName);
                string encName = Convert.ToBase64String(basebyte);

                DataProps dataProp = new DataProps();
                dataProp.key = "value";
                dataProp.tags = list.tags;

                equationRequest.equations.Add(new Equations(encName,list.equation,list.julianStartAt,list.julianEndAt,list.offsetStartAt,list.offsetEndAt, dataProp,list.Seq));
            }

            if (gridDataList.Count == 0)
            {
                equationRequest.equations.Add(new Equations());
            }
            var json = JsonConvert.SerializeObject(equationRequest);
            string waitMsg = null;
            string successMsg = null;
            string failMsg = null;
            switch (type)
            {
                case "save":
                case "saveEq":
                    waitMsg = "수식을 저장 중입니다. 잠시만 기다려주십시오.";
                    successMsg = "저장 성공";
                    failMsg = "저장 실패";
                    break;
                case "deleteAll":
                    waitMsg = "수식을 삭제 중입니다. 잠시만 기다려주십시오.";
                    successMsg = "전체삭제 성공";
                    failMsg = "전체삭제 실패";
                    break;
                case "delOne":
                    waitMsg = "수식을 삭제 중입니다. 잠시만 기다려주십시오.";
                    successMsg = "삭제 성공";
                    failMsg = "삭제 실패"; 
                    break;
            }

            mainForm.ShowSplashScreenManager(waitMsg);

            //if (!splashScreenManager1.IsSplashFormVisible)
            //{
            //    splashScreenManager1.ShowWaitForm();
            //    splashScreenManager1.SetWaitFormCaption(waitMsg);
            //}   
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], json);
            mainForm.HideSplashScreenManager();
            //if(splashScreenManager1.IsSplashFormVisible)
            //    splashScreenManager1.CloseWaitForm();
            if (responseData != null)
            {
                JsonData result = JsonConvert.DeserializeObject<JsonData>(responseData);
                if (result.code == 200)
                {
                    if (type != "saveEq")
                    {
                        MessageBox.Show(successMsg);
                    }
                    GetSelectDataList(paramModuleSeq);
                    parameterModuleControl.SaveChangePlotFromEQ(paramModuleSeq);
                }
                else
                {
                    MessageBox.Show(failMsg);
                }
            }
        }

        private void btnInitAll_Click(object sender, EventArgs e)
        {
            if (eqGridDataList.Count() != 0 && MessageBox.Show("수식이 전체 삭제됩니다. 삭제하시겠습니까?", "전체삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                eqGridDataList.Clear();
                this.gridControl1.DataSource = eqGridDataList;
                //gridView2.DeleteRow(gridView2.FocusedRowHandle);
                gridView1.RefreshData();
                SaveEquationRequest("deleteAll");
            }
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;
            
            if (e.KeyCode == Keys.Enter && view.FocusedColumn.FieldName == "equation")
            {
                GetEvaluation(view.FocusedValue.ToString());
            }
            
            //ButtonEdit me = sender as ButtonEdit;
            //if (me != null)
            //{
            //    EquationGridData selectGridData = (EquationGridData)gridView1.GetFocusedRow();
            //    if (selectGridData.tags != null && selectGridData.tags != "")
            //    {
            //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", selectGridData.tags + "|" + me.Text);
            //    }
            //    else
            //    {
            //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "tags", me.Text);

            //    }
            //    addTag(me.Text);
            //    me.Text = String.Empty;
            //}
        }

        private void GetEvaluation(string equation)
        {
            string sendData = string.Format(@"
                {{
                ""command"":""evaluation"",
                ""moduleSeq"": ""{0}"",
                ""equation"" : ""{1}""
                }}", paramModuleSeq,equation);

            MainForm mainForm = this.ParentForm as MainForm;
            mainForm.ShowSplashScreenManager("수식을 계산 중입니다. 잠시만 기다려주십시오.");
            //if (!splashScreenManager1.IsSplashFormVisible)
            //{
            //    splashScreenManager1.ShowWaitForm();
            //    splashScreenManager1.SetWaitFormCaption("수식을 계산 중입니다. 잠시만 기다려주십시오.");
            //}
            string responseData = Utils.GetPostData(ConfigurationManager.AppSettings["UrlParamModule"], sendData);
            //if (splashScreenManager1.IsSplashFormVisible)
            //    splashScreenManager1.CloseWaitForm();
            mainForm.HideSplashScreenManager();
            if (responseData != null)
            {
                EvaluationResponse evaluationResponse = JsonConvert.DeserializeObject<EvaluationResponse>(responseData);
                if (evaluationResponse.response != null && evaluationResponse.response.Count() != 0)
                {
                    SaveEquationRequest("saveEq");
                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DataCnt", evaluationResponse.response.Count()) ;

                    //foreach (var list in equationResponse.response)
                    //{
                    //    list.eqName = Utils.base64StringDecoding(list.eqName);
                    //    eqGridDataList.Add(new EquationGridData(list));
                    //}
                }
                //this.gridControl1.DataSource = eqGridDataList;
                //gridView1.RefreshData();
            }
        }

        private void AddChartData(string keyValue)
        {
            MainForm mainForm = this.ParentForm as MainForm;

            if (chartControl != null)
            {
                chartControl.Dispose();
                chartControl = null;
            }

            chartControl = new ChartControl();

            chartControl.Series.Clear();

            Series series = new Series("Series1", ViewType.Line);
            chartControl.Series.Add(series);

            series.DataSource = dt;

            series.ArgumentScaleType = ScaleType.Auto;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            //((XYDiagram)chartControl.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chartControl.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            XYDiagram diagram = (XYDiagram)chartControl.Diagram;
            
            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;

            diagram.AxisX.Label.Visible = false;
            //this.rangeControl1.Client = chartControl;
            //rangeControl1.RangeChanged += RangeControl1_RangeChanged;
            //rangeControl1.ShowLabels = true;
            //rangeControl1.SelectedRange = new RangeControlRange(minValue, maxValue);

            if (panelChart == null)
            {
                panelChart = new DockPanel();
                panelChart = mainForm.DockManager1.AddPanel(DockingStyle.Float);
                panelChart.FloatLocation = new Point(500, 100);
                panelChart.FloatSize = new Size(1058, 528);
                panelChart.Name = keyValue;
                panelChart.Text = keyValue;
                chartControl.Dock = DockStyle.Fill;
                panelChart.Controls.Add(chartControl);
                //panelChart.ClosedPanel += PanelChart_ClosedPanel;
            }
            else
            {
                panelChart.Name = keyValue;
                panelChart.Text = keyValue;
                //panelChart.Controls.Clear();
                chartControl.Dock = DockStyle.Fill;
                panelChart.Controls.Add(chartControl);
                panelChart.Show();
                panelChart.Focus();
            }
        }

        private DataTable GetChartValues(List<double> dataArr)
        {
            // Create an empty table.
            DataTable table = new DataTable("Table1");

            // Add two columns to the table.
            table.Columns.Add("Argument", typeof(Int32));
            //table.Columns.Add("Argument", typeof(DateTime));
            table.Columns.Add("Value", typeof(double));

            DataRow row = null;
            chartData.Clear();
            int i = 0;
            foreach (double data in dataArr)
            {
                row = table.NewRow();
                
                chartData.Add(data);
                row["Value"] = data;
                row["Argument"] = i++;
                table.Rows.Add(row);

            }

            return table;
        }
    }
}
