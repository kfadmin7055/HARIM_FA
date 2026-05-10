using Core.Class;
using DevExpress.CodeParser;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraTreeList.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_ProductDayUseResult : DevExpress.XtraEditors.XtraForm
    {
        private string[] sValid = null;

        public frm_ProductDayUseResult()
        {
            InitializeComponent();
        }

        private void frm_ProductDayUseResult_Load(object sender, EventArgs e)
        {
            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", false, 0, false);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dateEdit_workStartDate.EditValue = DateTime.Today.AddDays(-7);
            dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();
        }

        private void XMain_Search()
        {
            pivotGridControl.DataSource = null;
            pivotGridControl.BeginUpdate();

            string SQL = $@"
            SELECT PLANT_CODE, PROCESS_KEY, L_CODE, WORK_START_DATE, RESOURCE_NO, DESCRIPTION, SUM(OR_Q) AS OR_Q, SUM(PRO_Q) AS PRO_Q, LOCATION_ED
            FROM (
                SELECT 
                    WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE
                    , SUBSTR(WO.WORK_START_DATE, 1, 4) || '-' || SUBSTR(WO.WORK_START_DATE, 5, 2) || '-' || SUBSTR(WO.WORK_START_DATE, 7, 2) AS WORK_START_DATE,
                    WO.RESOURCE_NO,
                    TRIM(b.DESCRIPTION) AS DESCRIPTION,
                    SUM(WO.OR_Q) AS OR_Q,
                    SUM(WO.PRO_Q) AS PRO_Q,
                    WO.LOCATION_ED
                FROM WORK_ORDER WO
                LEFT OUTER JOIN SAP_DI_PRODUCT b ON WO.PLANT_CODE = b.PLANT_CODE AND WO.RESOURCE_NO = b.RESOURCE_NO
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR WO.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND ('{cboProcess_Key.EditValue}' IS NULL OR WO.PROCESS_KEY = '{cboProcess_Key.EditValue}')
                    AND ('{cboL_Code.EditValue}' IS NULL OR WO.L_CODE = '{cboL_Code.EditValue}')
                    AND WO.WORK_START_DATE BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                    AND wo.ERP_OSTATUS = 'Y'
                    AND NVL(WO.DEL_FLAG, ' ') != 'Y'
                    AND wo.ERP_OSTATUS = 'Y'
                GROUP BY 
                    WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE,
                    WO.WORK_START_DATE,
                    WO.RESOURCE_NO,
                    b.DESCRIPTION,
                    WO.LOCATION_ED
                ORDER BY SUM(WO.PRO_Q) DESC
                )
            GROUP BY PLANT_CODE, PROCESS_KEY, L_CODE, WORK_START_DATE, RESOURCE_NO, DESCRIPTION, LOCATION_ED
            ORDER BY PRO_Q DESC
            ";

            DataSet XMain_SearchDs = Dbconn.conn.ExecutDataset(SQL);

            pivotGridControl.DataSource = XMain_SearchDs.Tables[0];

            pivotGridControl.EndUpdate();
            pivotGridControl.OptionsView.ShowFilterHeaders = false;

            pivotGridControl.BestFit();

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_excelExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pivotGridControl.ExportToXlsx(sfd.FileName);
                System.Diagnostics.Process.Start(sfd.FileName);
            }
        }

        private void pivotGridControl_CustomCellValue(object sender, DevExpress.XtraPivotGrid.PivotCellValueEventArgs e)
        {
            if (e.Value == null)
                e.Value = 0;
        }

        private void frm_ProductDayUseResult_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
            }

            //// 신규 행 추가
            //if (e.KeyCode == Keys.F3)
            //{
            //    btn_rowAdd_Click(sender, e);
            //}

            //// 행 삭제
            //if (e.KeyCode == Keys.Delete)
            //{
            //    btn_rowDel_Click(sender, e);
            //}

            //// 저장
            //if (e.Control && e.KeyCode == Keys.S)
            //{
            //    XMain_Save();
            //}

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
            }

            //// 신규 행 추가
            //if (e.KeyCode == Keys.F3)
            //{
            //    btn_rowAdd_Click(sender, e);
            //}

            //// 행 삭제
            //if (e.KeyCode == Keys.Delete)
            //{
            //    btn_rowDel_Click(sender, e);
            //}

            //// 저장
            //if (e.Control && e.KeyCode == Keys.S)
            //{
            //    XMain_Save();
            //}

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            pivotGridControl.Focus();
        }

        private void cboProcess_Key_EditValueChanged(object sender, EventArgs e)
        {
            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess_Key.EditValue?.ToString()), "", false, 0, true);
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcess_Key, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);

            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess_Key.EditValue?.ToString()), "", false, 0, true);
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workStartDate_EditValueChanged(object sender, EventArgs e)
        {
            DateTime? startDate = dateEdit_workStartDate.EditValue as DateTime?;
            DateTime? endDate = dateEdit_workEndDate.EditValue as DateTime?;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (endDate.Value < startDate.Value)
                {
                    dateEdit_workEndDate.EditValue = startDate.Value;
                }
            }
        }

        private void dateEdit_workEndDate_EditValueChanged(object sender, EventArgs e)
        {
            DateTime? startDate = dateEdit_workStartDate.EditValue as DateTime?;
            DateTime? endDate = dateEdit_workEndDate.EditValue as DateTime?;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (endDate.Value < startDate.Value)
                {
                    dateEdit_workStartDate.EditValue = endDate.Value;
                }
            }
        }
    }
}