using Core.Class;
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

namespace HARIM_FA_DOSING
{
    public partial class frm_IngredUseResult : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_IngredUseResult()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void frm_IngredUseResult_Load(object sender, EventArgs e)
        {
            cboGubun.Properties.Items.Add("작업일자");
            cboGubun.Properties.Items.Add("투입일자");
            cboGubun.SelectedIndex = 0;

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", false, 0, false);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dtFrom.EditValue = DateTime.Today;
            dtTo.EditValue = DateTime.Today.AddDays(1);
            XMain_Search();

            gridView.ShowFindPanel();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XMain_Search()
        {
            try
            {
                string column = cboGubun.EditValue.ToString() == "1" ? "wo.WORK_START_DATE" : "wr.I_TIME";

                //var quotedArray = lookUpEdit_pcSel.EditValue.ToString()
                //.Split(',')
                //.Select(x => $"'{x.Trim()}'")
                //.ToArray();

                //string process = string.Join(", ", quotedArray);

                //SQL = $@"
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
                //    WHERE wo.PLANT_CODE  = '{cboPlant_Code.EditValue}'
                //      AND wo.PROCESS_KEY = '{cboProcess_Key.EditValue}'
                //      AND wo.L_CODE      = '{cboL_Code.EditValue}'
                //      AND NVL(wo.DEL_FLAG, 'N') != 'Y'
                //      AND wo.WORKDATE BETWEEN TO_DATE('{dtFrom.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') AND TO_DATE('{dtTo.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') + 1 - (1/86400)
                //      AND wo.ERP_OSTATUS = 'Y'
                //      AND CASE WHEN wd.PARENT_BIN IS NOT NULL THEN  0 ELSE wr.P_Q END > 0
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
                //    SELECT wd.INGRED_CODE, wd.PARENT_BIN
                //         , SUM(wd.SET_VAL * wo.R_BATCH) AS SET_VAL
                //    FROM WORK_ORDER wo
                //        JOIN WORK_DETAIL wd ON wd.PLANT_CODE  = wo.PLANT_CODE AND wd.PROCESS_KEY = wo.PROCESS_KEY AND wd.L_CODE      = wo.L_CODE
                //                                            AND wd.WORKDATE = wo.WORKDATE AND wd.NUM = wo.NUM
                //    WHERE wo.PLANT_CODE  = '{cboPlant_Code.EditValue}'
                //      AND wo.PROCESS_KEY = '{cboProcess_Key.EditValue}'
                //      AND wo.L_CODE      = '{cboL_Code.EditValue}'
                //      AND NVL(wo.DEL_FLAG, 'N') != 'Y'
                //      AND wo.WORKDATE BETWEEN TO_DATE('{dtFrom.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') AND TO_DATE('{dtTo.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') + 1 - (1/86400)
                //      AND wo.ERP_OSTATUS = 'Y'

                //    GROUP BY wd.INGRED_CODE, wd.PARENT_BIN
                //)

                //SELECT 
                //      WE.PLANT_CODE
                //    , WE.PROCESS_KEY
                //    , WE.L_CODE
                //    , WE.INGRED_LOT
                //    , P.DESCRIPTION AS INGRED_NAME
                //    , WE.NAME
                //    , TRIM(rt.NAME) AS TYPE_DESC
                //    , SUM(WE.P_Q) AS P_Q
                //    , p.UOM
                //    , WDS.SET_VAL
                //    , (WDS.SET_VAL - SUM(WE.P_Q)) * -1 AS DIV_VAL
                //    , ROUND(
                //                    ((WDS.SET_VAL - SUM(WE.P_Q)) * -1
                //                     / NULLIF(WDS.SET_VAL, 0)
                //                    ) * 100
                //                , 3) AS DIFFRATE
                //FROM WREMARK WE
                //    JOIN SAP_DI_PRODUCT P ON WE.PLANT_CODE = p.PLANT_CODE AND WE.INGRED_LOT = p.RESOURCE_NO
                //    JOIN RESOURCETYPE rt ON rt.CODE = p.RESOURCE_TYPE
                //    JOIN WDETAIL WDS ON WDS.INGRED_CODE = WE.INGRED_LOT
                //WHERE CASE WHEN WDS.PARENT_BIN IS NOT NULL THEN  0 ELSE WE.P_Q END > 0
                //GROUP BY 
                //      WE.PLANT_CODE, WE.PROCESS_KEY, WE.L_CODE,
                //      WE.INGRED_LOT, WE.NAME, P.DESCRIPTION,
                //      rt.NAME, p.UOM, WDS.SET_VAL
                //ORDER BY SUM(WE.P_Q) DESC
                //";

                SQL = $@"
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
                    AND wo.WORKDATE BETWEEN TO_DATE('{dtFrom.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') AND TO_DATE('{dtTo.DateTime.ToString("yyyyMMdd")}', 'YYYY-MM-DD') + 1 - (1/86400)
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
                    , wd.INGRED_CODE AS INGRED_LOT
                    , P.DESCRIPTION AS INGRED_NAME
                    , TRIM(rt.NAME) AS TYPE_DESC
                    , SUM(wr.P_Q) AS P_Q
                    , p.UOM
                    , SUM(wd.SET_VAL) AS SET_VAL
                    , (SUM(wd.SET_VAL) - SUM(wr.P_Q)) * -1 AS DIV_VAL
                    , ROUND(
                                    ((SUM(wd.SET_VAL) - SUM(wr.P_Q)) * -1
                                        / NULLIF(SUM(wd.SET_VAL), 0)
                                    ) * 100
                                , 3) AS DIFFRATE
                FROM WDETAIL wd
                    JOIN SAP_DI_PRODUCT P ON wd.PLANT_CODE = p.PLANT_CODE AND wd.INGRED_CODE = p.RESOURCE_NO
                    JOIN WREMARK wr ON wr.PLANT_CODE = wd.PLANT_CODE AND wr.PROCESS_KEY = wd.PROCESS_KEY AND wr.L_CODE = wd.L_CODE
                                                        AND wr.WORKDATE = wd.WORKDATE AND wr.NUM = wd.NUM AND wr.INGRED_LOT = wd.INGRED_CODE
                    JOIN RESOURCETYPE rt ON rt.CODE = p.RESOURCE_TYPE
                GROUP BY 
                        wd.PLANT_CODE, wd.PROCESS_KEY, wd.L_CODE,
                        wd.INGRED_CODE, P.DESCRIPTION, TRIM(rt.NAME),
                        p.UOM
                ORDER BY P_Q DESC
                ";
                
                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                sValid = new string[] { "" };


                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
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
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void cboProcess_Key_EditValueChanged(object sender, EventArgs e)
        {
            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess_Key.EditValue?.ToString()), "", false, 0, false);

            XMain_Search();
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            // 공정
             clsDevexpressUtil.ItemLookUpEditSetup(cboProcess_Key, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessTypeCode("배합")), "", false, 0, false);

            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess_Key.EditValue?.ToString()), "", false, 0, false);

            XMain_Search();
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }

        private void dtFrom_EditValueChanged(object sender, EventArgs e)
        {
            DateTime? startDate = dtFrom.EditValue as DateTime?;
            DateTime? endDate = dtTo.EditValue as DateTime?;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (endDate.Value < startDate.Value)
                {
                    dtTo.EditValue = startDate.Value;
                }
            }
        }

        private void dtTo_EditValueChanged(object sender, EventArgs e)
        {
            DateTime? startDate = dtFrom.EditValue as DateTime?;
            DateTime? endDate = dtTo.EditValue as DateTime?;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (endDate.Value < startDate.Value)
                {
                    dtFrom.EditValue = endDate.Value;
                }
            }
        }
    }
}