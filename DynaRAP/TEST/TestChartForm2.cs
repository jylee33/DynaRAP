using System.Data;
using System.Windows.Forms;

namespace DynaRAP.TEST
{
    public partial class TestChartForm2 : Form
    {
        public TestChartForm2()
        {
            InitializeComponent();
        }

        public TestChartForm2(DataTable dt)
        {
            InitializeComponent();

            this.dxChartControl1.DrawChart(dt);
        }
    }
}
