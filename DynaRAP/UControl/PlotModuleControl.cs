using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking2010.Views.Widget;
using DevExpress.XtraEditors;
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
    public partial class PlotModuleControl : DevExpress.XtraEditors.XtraUserControl
    {

        public PlotModuleControl()
        {
            InitializeComponent();
        }

        private void PlotModuleControl_Load(object sender, EventArgs e)
        {
            AddDocumentManager();
            //for (int i = 0; i < 4; i++)
            //{
            //    AddDocuments();
            //}
            ////Adding Documents to group1 is not necessary, since all newly created Documents are automatically placed in the first StackGroup.
            //group1.Items.AddRange(new Document[] { view.Documents[0] as Document, view.Documents[1] as Document });
            //view.Controller.Dock(view.Documents[2] as Document, group2);
            //view.Controller.Dock(view.Documents[3] as Document, group3);
        }

        WidgetView view;
        StackGroup group1, group2, group3;
        void AddDocumentManager()
        {
            DocumentManager dM = new DocumentManager(components);
            view = new WidgetView();
            dM.View = view;
            view.AllowDocumentStateChangeAnimation = DevExpress.Utils.DefaultBoolean.True;
            group1 = new StackGroup();
            group2 = new StackGroup();
            group3 = new StackGroup();
            group1.Length.UnitType = LengthUnitType.Star;
            group1.Length.UnitValue = 1;
            view.StackGroups.AddRange(new StackGroup[] { group1, group2, group3 });
            dM.ContainerControl = this;
        }

        int count = 0;
        void AddDocuments()
        {
            Document document = view.AddDocument(new MyLineChart()) as Document;
            document.MaximizedControl = new MyLineChart();
            count++;
        }

        public void AddDocument(UserControl ctrl)
        {
            Document document = view.AddDocument(ctrl) as Document;
            //document.MaximizedControl = new MyLineChart();
            document.Caption = count.ToString();

            switch (count % view.StackGroups.Count)
            {
                case 0:
                    view.Controller.Dock(document);
                    break;
                case 1:
                    view.Controller.Dock(document, group2);
                    break;
                case 2:
                    view.Controller.Dock(document, group3);
                    break;
                default:
                    view.Controller.Dock(document);
                    break;
            }

            count++;
        }
    }
}
