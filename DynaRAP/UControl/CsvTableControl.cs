using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DynaRAP.Common;
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
    public partial class CsvTableControl : DevExpress.XtraEditors.XtraUserControl
    {
        string csvFilePath = string.Empty;
        ImportType importType = ImportType.FLYING;
        public string CsvFilePath
        {
            set { csvFilePath = value; }
        }
        public ImportType ImportType
        {
            set { importType = value; }
        }

        public CsvTableControl()
        {
            InitializeComponent();
        }

        public CsvTableControl(string csvFilePath) : this()
        {
            this.csvFilePath = csvFilePath;
        }

        public CsvTableControl(ImportType importType) : this()
        {
            this.importType = importType;
        }

        private void CsvTableControl_Load(object sender, EventArgs e)
        {
            if(File.Exists(csvFilePath))
                FillGrid();
        }

        public void FillGrid()
        {
            gridControl1.DataSource = null;
            GC.Collect();
            DataTable dt = LoadCSV(this.csvFilePath, true);

            gridView1.Columns.Clear();

            gridControl1.DataSource = dt;

            //gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            gridView1.OptionsView.ShowColumnHeaders = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            //gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            //gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ColumnAutoWidth = false;

            //gridView1.OptionsBehavior.ReadOnly = true;
            //gridView1.OptionsBehavior.Editable = false;

            gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            //gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            //gridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
            //gridView1.OptionsSelection.EnableAppearanceHideSelection = false;

            //gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            //gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;

            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;

            //GridColumn colType = gridView1.Columns[0];
            //colType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            //colType.OptionsColumn.FixedWidth = true;
            //colType.Width = 120;
            for(int i=0;i < gridView1.Columns.Count; i++)
            {
                gridView1.Columns[i].Width = 100;
            }
        }

        private DataTable LoadCSV(string path, bool hasHeader)
        {
            DataTable dt = new DataTable();

            Dictionary<string, List<string>> dicData = new Dictionary<string, List<string>>();

            if (importType == ImportType.FLYING) // 비행데이터 import
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        dt.Rows.Add(dr);
                    }

                }
            }
            else // 해석데이터 import
            {
                using (StreamReader sr = new StreamReader(path))
                {

                    Dictionary<string, List<string>> tempData = new Dictionary<string, List<string>>();

                    // 스트림의 끝까지 읽기
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        line = line.Trim();
                        string[] data = line.Split(' ');

                        if (string.IsNullOrEmpty(data[0]))
                            continue;

                        double dVal;
                        bool isNumber = double.TryParse(data[0], out dVal);
                        int i = 0;

                        if (isNumber == false)
                        {
                            foreach (string key in tempData.Keys)
                            {
                                if (dicData.ContainsKey(key) == false)
                                {
                                    dicData.Add(key, tempData[key]);
                                }
                            }

                            if (data[0].Equals("UNITS"))
                            {
                                tempData.Clear();

                                if (tempData.ContainsKey("DATE") == false)
                                {
                                    tempData.Add("DATE", new List<string>());
                                }
                                for (i = 1; i < data.Length; i++)
                                {
                                    if (tempData.ContainsKey(data[i]) == false)
                                    {
                                        if (string.IsNullOrEmpty(data[i]) == false)
                                        {
                                            tempData.Add(data[i], new List<string>());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            data = data.Where((source, index) => string.IsNullOrEmpty(source) == false).ToArray();

                            if (data[0].StartsWith("-"))
                            {
                                continue;
                            }

                            i = 0;
                            foreach (string key in tempData.Keys)
                            {
                                if (tempData.ContainsKey(key))
                                {
                                    if (string.IsNullOrEmpty(data[i]) == false)
                                        tempData[key].Add(data[i++]);
                                }
                            }
                        }
                    }
                    dt.Columns.Clear();
                    foreach (var key in dicData.Keys)
                    {
                        if (dt.Columns.Contains(key))
                        {
                            dt.Columns.Add(key+"_1");
                        }else
                        {
                            dt.Columns.Add(key);
                        }
                    }
                    for(int i =0; i < dicData[dicData.Keys.ToList()[0]].Count; i++)
                    {
                        DataRow dr = dt.NewRow();

                        int j = 0;
                        foreach (var key in dicData.Keys)
                        {
                            dr[j] = dicData[key][i];
                            j++;
                        }
                        dt.Rows.Add(dr);
                    }
                    //임시주석
                    //while (!sr.EndOfStream)
                    //{
                    //    string line = sr.ReadLine();
                    //    line = line.Trim();
                    //    string[] data = line.Split(' ');

                    //    if (string.IsNullOrEmpty(data[0]))
                    //        continue;

                    //    double dVal;
                    //    bool isNumber = double.TryParse(data[0], out dVal);
                    //    int i = 0;

                    //    if (isNumber == false)
                    //    {

                    //        if (data[0].Equals("UNITS"))
                    //        {
                    //            dt.Columns.Clear();

                    //            if (dt.Columns.Contains("DATE") == false)
                    //            {
                    //                dt.Columns.Add("DATE");
                    //            }
                    //            for (i = 1; i < data.Length; i++)
                    //            {
                    //                if (dt.Columns.Contains(data[i]) == false)
                    //                { 
                    //                    if (string.IsNullOrEmpty(data[i]) == false)
                    //                    {
                    //                        dt.Columns.Add(data[i]);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        data = data.Where((source, index) => string.IsNullOrEmpty(source) == false).ToArray();

                    //        if (data[0].StartsWith("-"))
                    //        {
                    //            continue;
                    //        }
                    //        i = 0;

                    //        DataRow dr = dt.NewRow();
                    //        foreach (string valueData in data)
                    //        {
                    //            dr[i] = valueData;
                    //            i++;
                    //        }

                    //        dt.Rows.Add(dr);
                    //    }
                    //}
                }


            }


            return dt;
        }





    }
}
