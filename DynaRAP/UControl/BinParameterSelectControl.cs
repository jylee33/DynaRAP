using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DynaRAP.Data;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRAP.UControl
{
    public partial class BinParameterSelectControl : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PickUpParam pickUpParam = new PickUpParam();
        BinModuleControl moduleControl = null;
        List<ParamDatas> paramDataList = null;
        bool initParamList = false;
        string beforeSeq = null;
        public BinParameterSelectControl(BinModuleControl moduleControl, List<ParamDatas> paramDataList)
        {
            this.moduleControl = moduleControl;
            this.paramDataList = paramDataList;
            InitializeComponent();

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
        }

        private void SelectSBControl_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = pickUpParam.userParamTable;
            propTypeInit();

        }
        private void propTypeInit()
        {
            propTypeList.Properties.DataSource = null;

            List<propTypesCombo> comboList= new List<propTypesCombo>();
            foreach(var combo in paramDataList)
            {
                if (combo.propInfo != null)
                {
                    string temp = string.Format("{0}({1})", combo.propInfo.propType, combo.propInfo.paramUnit);
                    if (comboList.FindIndex(x => x.viewName == temp) == -1)
                    {
                        comboList.Add(new propTypesCombo(combo.propInfo.paramUnit, "",combo.propInfo.propType, temp));
                    }
                }
            }

            propTypeList.Properties.DisplayMember = "viewName";
            //propTypeList.Properties.ValueMember = "seq";
            propTypeList.Properties.NullText = "";

            paramList.Properties.NullText = "";

            propTypeList.Properties.DataSource = comboList;
            propTypeList.Properties.PopulateColumns();

            propTypeList.Properties.Columns["paramUnit"].Visible = false;
            propTypeList.Properties.Columns["propType"].Visible = false;
            propTypeList.Properties.Columns["seq"].Visible = false;
            propTypeList.Properties.Columns["paramKey"].Visible = false;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            moduleControl.removeControl(this);
        }

        private void btnAddNewRow_Click(object sender, EventArgs e)
        {

            if (pickUpParam.userParamTable == null)
            {
                pickUpParam.userParamTable = new List<UserParamTable>();
            }
            pickUpParam.userParamTable.Add(new UserParamTable());
            this.gridControl1.DataSource = pickUpParam.userParamTable;
            gridView1.RefreshData();
        }

        private void propTypeList_EditValueChanged(object sender, EventArgs e)
        {
            List<propTypesCombo> comboList = new List<propTypesCombo>();
            foreach (var combo in paramDataList)
            {
                if (combo.propInfo != null)
                {
                    if( combo.propInfo.propType == propTypeList.GetColumnValue("propType").ToString() && combo.propInfo.paramUnit == propTypeList.GetColumnValue("paramUnit").ToString())
                    {
                        if (comboList.FindIndex(x => x.paramKey == combo.paramKey) == -1)
                        {
                            comboList.Add(new propTypesCombo(combo.paramKey, combo.seq));
                        }
                    }
                   
                }
            }

            paramList.Properties.DataSource = comboList;

            paramList.Properties.DisplayMember = "paramKey";
            paramList.Properties.NullText = "";

            paramList.Properties.PopulateColumns();
            paramList.Properties.Columns["seq"].Visible = false;
            paramList.Properties.Columns["paramUnit"].Visible = false;
            paramList.Properties.Columns["propType"].Visible = false;
            paramList.Properties.Columns["viewName"].Visible = false;
        }

        private void paramList_EditValueChanged(object sender, EventArgs e)
        {
            if (!initParamList)
            {
                ParamDatas paramDatas = paramDataList.Find(x => x.seq == paramList.GetColumnValue("seq").ToString());

                if (moduleControl.SetSelectedParams(paramDatas))
                {
                    if (beforeSeq != null)
                    {
                        moduleControl.RemoveSelectedParams(beforeSeq);
                    }
                    beforeSeq = paramDatas.seq;
                }
                else
                {
                    initParamList = true;
                    paramList.EditValue = null;
                    if (beforeSeq != null)
                    {
                        moduleControl.RemoveSelectedParams(beforeSeq);
                        beforeSeq = null;
                    }
                    MessageBox.Show("파라미터 선택에 중복되는 값이 있습니다.");
                }
            }
            else
            {
                initParamList = false;
            }
        }

        public PickUpParam SelectedParamLIst()
        {
            ParamDatas paramDatas = paramDataList.Find(x => x.seq == paramList.GetColumnValue("seq").ToString());
            if(paramDatas != null)
            {
                PickUpParam pickUpParam = new PickUpParam(paramDatas);
                pickUpParam.userParamTable = (List<UserParamTable>)this.gridControl1.DataSource;

                return pickUpParam;
            }
            return null;
        }
    }
}
