using Core.Class;
using DevExpress.CodeParser;
using DevExpress.DataAccess.Native.Data;
using DevExpress.Schedule;
using DevExpress.XtraEditors;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraSpreadsheet.Import.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using DataTable = System.Data.DataTable;

namespace HARIM_FA_DOSING
{
    public partial class m_Note : DevExpress.XtraEditors.XtraForm
    {
        public string vResourceNo { get; set; }      // 품목
        public string vPlantCode { get; set; }      // 품목
        public string vNote { get; set; }           // 품목

        public string[] SelectedValue { get; set; }
        private string SQL = String.Empty;

        public m_Note(string sPlantCode, string sResourceNo, string sNote)
        {
            InitializeComponent();

            vPlantCode = sPlantCode;
            vResourceNo = sResourceNo;
            vNote = sNote;

            clsDevexpressGrid.ReadGridViewInit(bandViewNote, Properties.Settings.Default.FontSize);
        }

        private void m_Note_Load(object sender, EventArgs e)
        {
            try
            {
                XDetail_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "m_Note_Load", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridNote));
        }

        private void XDetail_Search()
        {
            SQL = $@"
            SELECT DISTINCT a.PLANT_CODE, a.RESOURCE_NO, NVL(d.RESOURCE_NO_3, a.IDNRK) AS IDNRK, NVL(e.DESCRIPTION, c.DESCRIPTION) AS IDNRK_DESC
            FROM SAP_IN_BOM_COND a
                INNER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.IDNRK
                LEFT JOIN SAP_IN_PRODUCT_CP D ON d.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO AND d.RESOURCE_NO_2 = a.IDNRK
                LEFT JOIN SAP_DI_PRODUCT E ON e.PLANT_CODE = d.PLANT_CODE AND e.RESOURCE_NO = d.RESOURCE_NO_3
            WHERE a.PLANT_CODE = '{vPlantCode}' AND a.RESOURCE_NO = '{vResourceNo}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);
            bandViewNote.OptionsView.ShowFooter = true;
            clsDevexpressGrid.BindGridControl(gridNote, bandViewNote, ds.Tables[0], false, true);


            SetConfirmNote();
        }

        private void SetConfirmNote()
        {
            DataSet ds1 = null;
            string sNote = string.Empty;
            
            DataTable DT = (DataTable)gridNote.DataSource;

            // 배치 컬럼 생성
            
            SQL = $@"
            SELECT a.RESOURCE_NO, c.DESCRIPTION, a.NOTE, d.RESOURCE_NO AS IDNRK, d.DESCRIPTION AS IDNRK_DESC, b.MENGE, b.MEINS
            FROM SAP_IN_BOM_CONM a
                INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE AND b.P_TYPE = a.P_TYPE
                INNER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                INNER JOIN SAP_DI_PRODUCT D ON d.PLANT_CODE = b.PLANT_CODE AND d.RESOURCE_NO = b.IDNRK
            WHERE a.PLANT_CODE = '{vPlantCode}' AND a.RESOURCE_NO = '{vResourceNo}' AND a.P_TYPE = '2' AND a.STLST = '2'
            ORDER BY a.NOTE DESC, d.RESOURCE_NO
            ";

            DataSet noteDs = Dbconn.conn.ExecutDataset(SQL);

            int distinctCount = noteDs.Tables[0].AsEnumerable()
            .Select(row => row.Field<string>("NOTE"))
            .Where(value => !string.IsNullOrEmpty(value)) // 필요 시 null/공백 제외
            .Distinct()
            .Count();

            string[] noteList = noteDs.Tables[0].AsEnumerable()
            .Select(row => row.Field<string>("NOTE"))
            .Where(value => !string.IsNullOrEmpty(value)) // (옵션) null 또는 공백 제외
            .Distinct()
            .ToArray();

            SetCreateBatch(noteList, DT);

            int temp_batch = 1;
            string filterEx = string.Empty;
            for (int i = 2; i < bandViewNote.Columns.Count; i++)
            {
                // 원료코드
                if (bandViewNote.Columns[i].FieldName.Contains("C"))
                {
                    for (int r = 0; r < bandViewNote.RowCount; r++)
                    {
                        sNote = bandViewNote.Columns[i].OwnerBand.Caption;
                        filterEx = $"IDNRK ='{bandViewNote.GetRowCellValue(r, "IDNRK")}' AND NOTE = '{sNote}'";

                        DataRow[] row = noteDs.Tables[0].Select(filterEx);

                        if (row.Length > 0)
                        {
                            bandViewNote.SetRowCellValue(r, bandViewNote.Columns[i], row[0]["IDNRK"]);
                        }
                    }
                }

                // 원료명
                if (bandViewNote.Columns[i].FieldName.Contains("D"))
                {
                    for (int r = 0; r < bandViewNote.RowCount; r++)
                    {
                        sNote = bandViewNote.Columns[i].OwnerBand.Caption;
                        filterEx = $"IDNRK ='{bandViewNote.GetRowCellValue(r, "IDNRK")}' AND NOTE = '{sNote}'";

                        DataRow[] row = noteDs.Tables[0].Select(filterEx);

                        if (row.Length > 0)
                        {
                            bandViewNote.SetRowCellValue(r, bandViewNote.Columns[i], row[0]["IDNRK_DESC"]);
                        }
                    }
                }

                // 비율
                if (bandViewNote.Columns[i].FieldName.Contains("M"))
                {
                    for (int r = 0; r < bandViewNote.RowCount; r++)
                    {
                        sNote = bandViewNote.Columns[i].OwnerBand.Caption;
                        filterEx = $"IDNRK ='{bandViewNote.GetRowCellValue(r, "IDNRK")}' AND NOTE = '{sNote}'";

                        DataRow[] row = noteDs.Tables[0].Select(filterEx);

                        if (row.Length > 0)
                        {
                            bandViewNote.SetRowCellValue(r, bandViewNote.Columns[i], row[0]["MENGE"]);
                        }
                    }
                }

                // 단위
                if (bandViewNote.Columns[i].FieldName.Contains("U"))
                {
                    for (int r = 0; r < bandViewNote.RowCount; r++)
                    {
                        sNote = bandViewNote.Columns[i].OwnerBand.Caption;
                        filterEx = $"IDNRK ='{bandViewNote.GetRowCellValue(r, "IDNRK")}' AND NOTE = '{sNote}'";

                        DataRow[] row = noteDs.Tables[0].Select(filterEx);

                        if (row.Length > 0)
                        {
                            bandViewNote.SetRowCellValue(r, bandViewNote.Columns[i], row[0]["MEINS"]);
                        }
                    }

                    temp_batch += 1;
                }
            }
        }

        /// <summary>
        /// 그리드 배치 컬럼 생성
        /// </summary>
        /// <param name="batch_cnt"></param>
        /// <param name="DT"></param>
        private void SetCreateBatch(string[] noteList, DataTable DT)
        {
            string sNote = string.Empty;
            GridBand band = null;

            for (int i = 0; i < (noteList.Length); i++)
            {
                if (sNote != noteList[i])
                {
                    sNote = noteList[i];

                    band = new GridBand();
                    band.Caption = sNote;
                    band.Visible = true;
                    
                }

                BandedGridColumn colBatch1 = new BandedGridColumn();
                colBatch1.Visible = true;
                colBatch1.Width = 30;
                colBatch1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None;
                colBatch1.Caption = "원료코드";
                colBatch1.FieldName = "C" + i.ToString();
                DT.Columns.Add("C" + i.ToString());
                colBatch1.OptionsColumn.AllowEdit = true;
                colBatch1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                colBatch1.OptionsColumn.ReadOnly = true;

                band.Columns.Add(colBatch1);
                bandViewNote.Bands.Add(band);
                bandViewNote.Columns.Add(colBatch1);

                band.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                //bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                //if (i % 2 == 0)
                //{
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(204, 229, 255);
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                //}
                //else
                //{
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                //}



                BandedGridColumn colBatch2 = new BandedGridColumn();
                colBatch2.Visible = true;
                colBatch2.Width = 160;
                colBatch2.Caption = "원료명";
                colBatch2.FieldName = "D" + i.ToString();
                DT.Columns.Add("D" + i.ToString());
                colBatch2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                colBatch2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None;
                colBatch2.OptionsColumn.AllowEdit = true;
                colBatch2.OptionsColumn.ReadOnly = true;
                colBatch2.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(153, 204, 255);

                band.Columns.Add(colBatch2);
                bandViewNote.Columns.Add(colBatch2);

                bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                //bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;


                //if (i % 2 == 0)
                //{
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(204, 229, 255);
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;

                //}
                //else
                //{
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                //}


                BandedGridColumn colBatch3 = new BandedGridColumn();
                colBatch3.Visible = true;
                colBatch3.Width = 100;
                colBatch3.DisplayFormat.FormatString = "{0:0.###}";
                colBatch3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                colBatch3.Caption = "비율";
                colBatch3.FieldName = "M" + i.ToString();
                DT.Columns.Add("M" + i.ToString());
                colBatch3.OptionsColumn.AllowEdit = true;
                colBatch3.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                colBatch3.OptionsColumn.ReadOnly = true;
                colBatch3.SummaryItem.DisplayFormat = "{0:0.###} %";
                colBatch3.SummaryItem.FieldName = "M" + i.ToString();
                colBatch3.SummaryItem.Mode = DevExpress.Data.SummaryMode.AllRows;
                colBatch3.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                band.Columns.Add(colBatch3);
                bandViewNote.Columns.Add(colBatch3);
                bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                //bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;


                //if (i % 2 == 0)
                //{
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(204, 229, 255);
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                //}
                //else
                //{
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                //}

                BandedGridColumn colBatch4 = new BandedGridColumn();
                colBatch4.Visible = true;
                colBatch4.Width = 140;
                colBatch4.Caption = "단위";
                colBatch4.FieldName = "U" + i.ToString();
                DT.Columns.Add("U" + i.ToString());
                colBatch4.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                colBatch4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None;
                colBatch4.OptionsColumn.AllowEdit = true;
                colBatch4.OptionsColumn.ReadOnly = true;
                colBatch4.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(153, 204, 255);

                band.Columns.Add(colBatch4);
                bandViewNote.Columns.Add(colBatch4);

                bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                //bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;


                //if (i % 2 == 0)
                //{
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(204, 229, 255);
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                //}
                //else
                //{
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                //    bandViewNote.Columns[bandViewNote.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                //}
            }

            gridNote.DataSource = DT;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dtDate_TextChanged(object sender, EventArgs e)
        {
            XDetail_Search();
        }
    }
}