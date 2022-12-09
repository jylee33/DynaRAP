using DevExpress.Utils;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DynaRAP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class ChangeAxisInfoControl : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler DeleteBtnClicked;
        private PlotAxisInfo plotAxisInfo = null;
        DXChartControl dXChartControl = null;
        DockPanel panel = null;


        public ChangeAxisInfoControl(PlotAxisInfo plotAxisInfo, DXChartControl dXChartControl)
        {
            this.plotAxisInfo = plotAxisInfo;
            this.dXChartControl = dXChartControl;
            InitializeComponent();
        }

        private void ChangeAxisInfoControl_Load(object sender, EventArgs e)
        {
            cboValueType.Properties.NullText = "";
            cboValueType.Properties.Items.Add("");
            cboValueType.Properties.Items.Add("Index");
            cboValueType.Properties.Items.Add("Time");
            cboXGridAlign.Properties.NullText = "";
            cboXGridAlign.Properties.Items.Add("Millisecond");
            cboXGridAlign.Properties.Items.Add("Second"); 
            InitAxisValue();
        }

        private void InitAxisValue()
        {
            if (plotAxisInfo != null)
            {
                cboValueType.Text = plotAxisInfo.diagramType;
                edtXTitle.Text = plotAxisInfo.xTitle;
                edtXSpacing.Text = plotAxisInfo.xSpacing;
                cboXGridAlign.Text = plotAxisInfo.xGridAlign;
                //edtXGridAlign.Text = ;
                edtXMinRange.Text = plotAxisInfo.xMinRange;
                edtXMaxRange.Text = plotAxisInfo.xMaxRange;
                edtYTitle.Text = plotAxisInfo.yTitle;
                edtYSpacing.Text = plotAxisInfo.ySpacing;
                //edtYGridAlign.Text = plotAxisInfo.yGridAlign;
                edtYMinRange.Text = plotAxisInfo.yMinRange;
                edtYMaxRange.Text = plotAxisInfo.yMaxRange;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(cboValueType.Text == "Time")
            {
                DateTime minTime;
                DateTime.TryParse(edtXMinRange.Text, out minTime);
                DateTime maxTime;
                DateTime.TryParse(edtXMaxRange.Text, out maxTime);
                if (edtXMinRange.Text.Length != 15 || edtXMaxRange.Text.Length != 15 || edtXMinRange.Text.IndexOf(":") != 2 || edtXMinRange.Text.LastIndexOf(":") != 5 || edtXMinRange.Text.LastIndexOf(".") != 8 || edtXMaxRange.Text.IndexOf(":") != 2 || edtXMaxRange.Text.LastIndexOf(":") != 5 || edtXMaxRange.Text.LastIndexOf(".") != 8)
                {
                    MessageBox.Show(new Form { TopMost = true }, "X축 정보의 min, max의 형식이 잘못되었습니다. 다시 확인해주세요. \n날짜 형식 : HH:MM:DD.######");
                    return;
                }

                if(minTime> maxTime)
                {
                    MessageBox.Show(new Form { TopMost = true }, "X축 정보의 max값이 min의 값보다 작습니다. 다시 확인해주세요.");
                    return;
                }
            } 
            else
            {
                double minValue1 = -9999;
                double maxValue1 = -9999;
     
                if (!double.TryParse(edtXMinRange.Text, out minValue1))
                {
                    MessageBox.Show(new Form { TopMost = true }, "X축 정보의 min 형식이 잘못되었습니다. 다시 확인해주세요.");
                    return;
                }
                if (!double.TryParse(edtXMaxRange.Text, out maxValue1))
                {
                    MessageBox.Show(new Form { TopMost = true }, "X축 정보의 max 형식이 잘못되었습니다. 다시 확인해주세요.");
                    return;
                }
                if (minValue1 > maxValue1)
                {
                    MessageBox.Show(new Form { TopMost = true }, "X축 정보의 max값이 min의 값보다 작습니다. 다시 확인해주세요.");
                    return;
                }
            }
            double minValue = -9999;
            double maxValue = -9999;

            if (!double.TryParse(edtYMinRange.Text, out minValue))
            {
                MessageBox.Show(new Form { TopMost = true }, "Y축 정보의 min 형식이 잘못되었습니다. 다시 확인해주세요.");
                return;
            }
            if (!double.TryParse(edtYMaxRange.Text, out maxValue))
            {
                MessageBox.Show(new Form { TopMost = true }, "Y축 정보의 max 형식이 잘못되었습니다. 다시 확인해주세요.");
                return;
            }
            if (minValue > maxValue)
            {
                MessageBox.Show(new Form { TopMost = true }, "Y축 정보의 max값이 min의 값보다 작습니다. 다시 확인해주세요.");
                return;
            }
            if (!double.TryParse(edtXSpacing.Text, out maxValue))
            {
                MessageBox.Show(new Form { TopMost = true }, "X축 정보의 Spacing 형식이 잘못되었습니다. 다시 확인해주세요.");
                return;
            }
            if (!double.TryParse(edtYSpacing.Text, out maxValue))
            {
                MessageBox.Show(new Form { TopMost = true }, "Y축 정보의 Spacing 형식이 잘못되었습니다. 다시 확인해주세요.");
                return;
            }
            plotAxisInfo.diagramType = cboValueType.Text;
            plotAxisInfo.xTitle = edtXTitle.Text;
            plotAxisInfo.xSpacing = edtXSpacing.Text;
            plotAxisInfo.xGridAlign = cboXGridAlign.Text;
            plotAxisInfo.xMinRange = edtXMinRange.Text;
            plotAxisInfo.xMaxRange = edtXMaxRange.Text;
            plotAxisInfo.yTitle = edtYTitle.Text;
            plotAxisInfo.ySpacing = edtYSpacing.Text;
            //plotAxisInfo.yGridAlign = edtYGridAlign.Text;
            plotAxisInfo.yMinRange = edtYMinRange.Text;
            plotAxisInfo.yMaxRange = edtYMaxRange.Text;
            dXChartControl.SetPlotAxisInfo(plotAxisInfo);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dXChartControl.closeAxisInfoPanel();
        }

        private void cboValueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboValueType.Text == "Time")
            {
                cboXGridAlign.ReadOnly = false;
                cboXGridAlign.SelectedIndex = 0;
            }
            else
            {
                cboXGridAlign.Text = "";
                cboXGridAlign.ReadOnly = true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboValueType.Text = string.Empty;
            edtXTitle.Text = string.Empty;
            edtXSpacing.Text = string.Empty;
            cboXGridAlign.Text = string.Empty;
            edtXMinRange.Text = string.Empty;
            edtXMaxRange.Text = string.Empty;
            edtYTitle.Text = string.Empty;
            edtYSpacing.Text = string.Empty;
            edtYMinRange.Text = string.Empty;
            edtYMaxRange.Text = string.Empty;
        }
    }
}
