using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_IngredDayUseResult : DevExpress.XtraEditors.XtraForm
    {
        private string[] sValid = null;

        public frm_IngredDayUseResult()
        {
            InitializeComponent();
        }

        private void frm_IngredDayUseResult_Load(object sender, EventArgs e)
        {
            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dateEdit_workStartDate.EditValue = DateTime.Today.AddDays(-7);
            dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();
        }

        private void XMain_Search()
        {
            pivotGridControl.DataSource = null;

            pivotGridControl.BeginUpdate();

            //string SQL = $@"
            //SELECT 
            //    wo.PLANT_CODE, wo.PROCESS_KEY, wo.L_CODE
            //    , SUBSTR(wo.WORKDATE, 1, 4) || '-' || SUBSTR(wo.WORKDATE, 5, 2) || '-' || SUBSTR(wo.WORKDATE, 7, 2) AS WORK_START_DATE,
            //    wo.WORKDATE,
            //    remark.INGRED_LOT,
            //    TRIM(prod.DESCRIPTION) AS NAME,
            //    TRIM(prod.RESOURCE_TYPE_DESC) AS TYPE_DESC,
            //    SUM(detail.SET_VAL) AS SET_VAL,
            //    SUM(remark.P_Q) AS P_Q,
            //    SUM(detail.SET_VAL) - SUM(remark.P_Q) AS DIV_VAL,
            //    prod.UOM
            //FROM WORK_ORDER wo
            //     JOIN WORK_REMARK remark ON remark.PLANT_CODE = wo.PLANT_CODE AND remark.PROCESS_KEY = wo.PROCESS_KEY AND remark.L_CODE = wo.L_CODE
            //                                AND remark.WORKDATE = wo.WORKDATE AND remark.NUM = wo.NUM
            //     JOIN WORK_DETAIL detail ON detail.PLANT_CODE = wo.PLANT_CODE AND detail.PROCESS_KEY = wo.PROCESS_KEY AND detail.L_CODE = wo.L_CODE
            //                                AND detail.WORKDATE = wo.WORKDATE AND detail.NUM = wo.NUM AND detail.INGRED_CODE = remark.INGRED_LOT
            //    JOIN SAP_DI_PRODUCT prod ON prod.PLANT_CODE = wo.PLANT_CODE AND prod.RESOURCE_NO = remark.INGRED_LOT
            //WHERE wo.PLANT_CODE = '{cboPlant_Code.EditValue}'
            //    AND wo.WORKDATE BETWEEN TO_DATE('{dateEdit_workStartDate.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') AND TO_DATE('{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') + 1 - (1/86400)
            //    AND ('{cboProcess_Key.EditValue}' IS NULL OR wo.PROCESS_KEY = '{cboProcess_Key.EditValue}')
            //    AND ('{cboL_Code.EditValue}' IS NULL OR wo.L_CODE = '{cboL_Code.EditValue}')
            //    AND wo.ERP_OSTATUS = 'Y'
            //    AND NVL(wo.DEL_FLAG, 'N') != 'Y'
            //    AND CASE WHEN detail.PARENT_BIN IS NOT NULL THEN  0 ELSE remark.P_Q END > 0
            //GROUP BY 
            //    wo.PLANT_CODE, wo.PROCESS_KEY, wo.L_CODE,
            //    wo.WORKDATE,
            //    remark.INGRED_LOT,
            //    prod.DESCRIPTION,
            //    prod.RESOURCE_TYPE_DESC,
            //    prod.UOM
            //ORDER BY SUM(remark.P_Q) DESC
            //";

            //string SQL = $@"
            //WITH RESOURCETYPE AS (
            //    SELECT c.COMM_DTCODE AS CODE
            //         , c.COMM_DTNM  AS NAME
            //    FROM COMM_DIV a
            //    JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
            //    JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE  = b.COMM_CODE
            //    WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE  = '02'
            //), WREMARK AS (
            //    SELECT wr.PLANT_CODE
            //         , wr.L_CODE
            //         , wr.PROCESS_KEY
            //         , wr.WORKDATE
            //         , wr.NUM
            //         , wr.I_TIME AS WORK_START_DATE
            //         , wr.NAME
            //         , wr.INGRED_LOT
            //         , wr.BATCH
            //         , SUM(wr.P_Q) AS P_Q
            //    FROM WORK_ORDER wo
            //        JOIN WORK_REMARK wr ON wr.PLANT_CODE  = wo.PLANT_CODE AND wr.PROCESS_KEY = wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
            //                                                AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM
            //        JOIN WORK_DETAIL wd ON wd.PLANT_CODE  = wo.PLANT_CODE AND wd.PROCESS_KEY = wo.PROCESS_KEY AND wd.L_CODE      = wo.L_CODE
            //                                            AND wd.WORKDATE = wo.WORKDATE AND wd.NUM = wo.NUM AND wd.INGRED_CODE = wr.INGRED_LOT
            //    WHERE wo.PLANT_CODE = '{cboPlant_Code.EditValue}'
            //        AND wo.WORKDATE BETWEEN TO_DATE('{dateEdit_workStartDate.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') AND TO_DATE('{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') + 1 - (1/86400)
            //        AND ('{cboProcess_Key.EditValue}' IS NULL OR wo.PROCESS_KEY = '{cboProcess_Key.EditValue}')
            //        AND ('{cboL_Code.EditValue}' IS NULL OR wo.L_CODE = '{cboL_Code.EditValue}')
            //        AND wo.ERP_OSTATUS = 'Y'
            //        AND NVL(wo.DEL_FLAG, 'N') != 'Y'
            //        AND CASE WHEN wd.PARENT_BIN IS NOT NULL THEN  0 ELSE wr.P_Q END > 0
            //    GROUP BY wr.PLANT_CODE
            //           , wr.L_CODE
            //           , wr.PROCESS_KEY
            //           , wr.WORKDATE
            //           , wr.NUM
            //           , wr.I_TIME
            //           , wr.NAME
            //           , wr.INGRED_LOT
            //           , wr.BATCH
            //), WDETAIL AS (
            //    SELECT wd.PLANT_CODE, wd.PROCESS_KEY, wd.L_CODE, wd.WORKDATE, wd.INGRED_CODE
            //         , SUM(wd.SET_VAL * wo.R_BATCH) AS SET_VAL
            //    FROM WORK_ORDER wo
            //        JOIN WORK_DETAIL wd ON wd.PLANT_CODE  = wo.PLANT_CODE AND wd.PROCESS_KEY = wo.PROCESS_KEY AND wd.L_CODE = wo.L_CODE
            //                                            AND wd.WORKDATE = wo.WORKDATE AND wd.NUM = wo.NUM
            //    WHERE wo.PLANT_CODE = '{cboPlant_Code.EditValue}'
            //        AND wo.WORKDATE BETWEEN TO_DATE('{dateEdit_workStartDate.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') AND TO_DATE('{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') + 1 - (1/86400)
            //        AND ('{cboProcess_Key.EditValue}' IS NULL OR wo.PROCESS_KEY = '{cboProcess_Key.EditValue}')
            //        AND ('{cboL_Code.EditValue}' IS NULL OR wo.L_CODE = '{cboL_Code.EditValue}')
            //        AND wo.ERP_OSTATUS = 'Y'
            //        AND NVL(wo.DEL_FLAG, 'N') != 'Y'
            //    GROUP BY wd.PLANT_CODE, wd.PROCESS_KEY, wd.L_CODE, wd.WORKDATE, wd.INGRED_CODE
            //)

            //SELECT 
            //      WE.PLANT_CODE
            //    , WE.PROCESS_KEY
            //    , WE.L_CODE
            //    , SUBSTR(WE.WORKDATE, 1, 4) || '-' || SUBSTR(WE.WORKDATE, 5, 2) || '-' || SUBSTR(WE.WORKDATE, 7, 2) AS WORK_START_DATE
            //    , WE.WORKDATE
            //    , WE.INGRED_LOT
            //    , P.DESCRIPTION AS NAME
            //    , WE.NAME
            //    , TRIM(rt.NAME) AS TYPE_DESC
            //    , WDS.SET_VAL AS SET_VAL
            //    , SUM(WE.P_Q) AS P_Q
            //    , p.UOM
            //    , (WDS.SET_VAL - SUM(WE.P_Q)) * -1 AS DIV_VAL
            //FROM WREMARK WE
            //    JOIN SAP_DI_PRODUCT P ON WE.PLANT_CODE = p.PLANT_CODE AND WE.INGRED_LOT = p.RESOURCE_NO
            //    JOIN RESOURCETYPE rt ON rt.CODE = p.RESOURCE_TYPE
            //    JOIN WDETAIL WDS ON WDS.PLANT_CODE  = WE.PLANT_CODE AND WDS.PROCESS_KEY = WE.PROCESS_KEY AND WDS.L_CODE = WE.L_CODE
            //                                            AND WDS.WORKDATE = WE.WORKDATE AND WDS.INGRED_CODE = WE.INGRED_LOT
            //GROUP BY 
            //      WE.PLANT_CODE, WE.PROCESS_KEY, WE.L_CODE, WE.WORKDATE,
            //      WE.INGRED_LOT, WE.NAME, P.DESCRIPTION,
            //      rt.NAME, p.UOM, WDS.SET_VAL
            //ORDER BY P_Q DESC
            //";

            string SQL = $@"
            WITH RESOURCETYPE AS (
                SELECT c.COMM_DTCODE AS CODE
                     , c.COMM_DTNM  AS NAME
                FROM COMM_DIV a
                JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE  = b.COMM_CODE
                WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE  = '02'
            ), WORDER AS (
                SELECT *
                FROM WORK_ORDER wo
                WHERE wo.PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND wo.WORKDATE BETWEEN TO_DATE('{dateEdit_workStartDate.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') AND TO_DATE('{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') + 1 - (1/86400)
                    AND ('{cboProcess_Key.EditValue}' IS NULL OR wo.PROCESS_KEY = '{cboProcess_Key.EditValue}')
                    AND ('{cboL_Code.EditValue}' IS NULL OR wo.L_CODE = '{cboL_Code.EditValue}')
                    AND wo.ERP_OSTATUS = 'Y'
                    AND NVL(wo.DEL_FLAG, 'N') != 'Y'
            ), WDETAIL AS (
                SELECT 
                    wd.PLANT_CODE
                    , wd.PROCESS_KEY
                    , wd.L_CODE
                    , wd.WORKDATE
                    , wd.NUM
                    , wd.INGRED_CODE
                    , (wd.SET_VAL * wo.BATCH) AS SET_VAL
                    , wd.PARENT_BIN
                FROM WORDER wo
                    JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wo.PLANT_CODE AND wd.PROCESS_KEY = wo.PROCESS_KEY AND wd.L_CODE = wo.L_CODE
                        AND wd.WORKDATE = wo.WORKDATE AND wd.NUM = wo.NUM
                WHERE wd.PARENT_BIN IS NULL
            ),WREMARK AS (
                    SELECT wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.NUM
                        ,  NVL(wd.PARENT_BIN, wr.LOCATION) AS LOCATION, SUM(wr.P_Q) AS P_Q
                        , wr.INGRED_LOT, wr.RESOURCE_NO
                    FROM WORDER wo
                        JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wo.PLANT_CODE AND wd.PROCESS_KEY = wo.PROCESS_KEY AND wd.L_CODE = wo.L_CODE
                            AND wd.WORKDATE = wo.WORKDATE AND wd.NUM = wo.NUM
                        JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY = wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                            AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM AND wr.LOCATION = wd.LOCATION AND wr.INGRED_LOT = wd.INGRED_CODE
                    -- WHERE wr.INGRED_LOT = '1222010000'
                    GROUP BY wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.NUM
                                , NVL(wd.PARENT_BIN, wr.LOCATION)
                                , wr.INGRED_LOT, wr.RESOURCE_NO
            ) 
                
            SELECT 
                  wd.PLANT_CODE
                , wd.PROCESS_KEY
                , wd.L_CODE
                , SUBSTR(wd.WORKDATE, 1, 4) || '-' || SUBSTR(wd.WORKDATE, 5, 2) || '-' || SUBSTR(wd.WORKDATE, 7, 2) AS WORK_START_DATE
                , wd.WORKDATE
                , wd.INGRED_CODE AS INGRED_LOT
                , P.DESCRIPTION AS NAME
                , TRIM(rt.NAME) AS TYPE_DESC
                , SUM(wd.SET_VAL) AS SET_VAL
                , SUM(wr.P_Q) AS P_Q
                , p.UOM
                , (SUM(wd.SET_VAL) - SUM(wr.P_Q)) * -1 AS DIV_VAL
            FROM WDETAIL wd
                JOIN SAP_DI_PRODUCT P ON wd.PLANT_CODE = p.PLANT_CODE AND wd.INGRED_CODE = p.RESOURCE_NO
                JOIN WREMARK wr ON wr.PLANT_CODE = wd.PLANT_CODE AND wr.PROCESS_KEY = wd.PROCESS_KEY AND wr.L_CODE = wd.L_CODE
                                                    AND wr.WORKDATE = wd.WORKDATE AND wr.NUM = wd.NUM AND wr.INGRED_LOT = wd.INGRED_CODE
                JOIN RESOURCETYPE rt ON rt.CODE = p.RESOURCE_TYPE
            GROUP BY 
                  wd.PLANT_CODE, wd.PROCESS_KEY, wd.L_CODE, wd.WORKDATE,
                  wd.INGRED_CODE, P.DESCRIPTION,
                  rt.NAME, p.UOM
            ORDER BY P_Q DESC
            ";

            DataSet XMain_SearchDs = Dbconn.conn.ExecutDataset(SQL);

            pivotGridControl.DataSource = XMain_SearchDs.Tables[0];

            pivotGridControl.EndUpdate();
            pivotGridControl.OptionsView.ShowFilterHeaders = false;

            pivotGridControl.Appearance.Cell.Font = new Font(pivotGridControl.Appearance.Cell.Font.FontFamily, Properties.Settings.Default.FontSize);

            pivotGridControl.Appearance.FieldValue.Font =
                new Font(pivotGridControl.Appearance.FieldValue.Font.FontFamily, Properties.Settings.Default.FontSize);

            pivotGridControl.Appearance.FieldHeader.Font =
                new Font(pivotGridControl.Appearance.FieldHeader.Font.FontFamily, Properties.Settings.Default.FontSize);

            pivotGridControl.Appearance.GrandTotalCell.Font =
                new Font(pivotGridControl.Appearance.GrandTotalCell.Font.FontFamily, Properties.Settings.Default.FontSize);

            pivotGridControl.Appearance.TotalCell.Font =
                new Font(pivotGridControl.Appearance.TotalCell.Font.FontFamily, Properties.Settings.Default.FontSize);

            pivotGridControl.BestFit();

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void pivotGridControl1_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            if (e.Value == null)
                e.Value = 0;
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