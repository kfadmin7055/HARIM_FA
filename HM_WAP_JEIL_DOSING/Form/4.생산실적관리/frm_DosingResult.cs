using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class frm_DosingResult : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        private bool isInitializing = false;

        public frm_DosingResult()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_DosingResult_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", false, 0, false);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dateEdit_workStartDate.EditValue = DateTime.Today;
            dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();

            isInitializing = true;

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XMain_Search()
        {
            try
            {

                //if (splashScreenManager.IsSplashFormVisible)
                //{
                //    splashScreenManager.CloseWaitForm();
                //}


                //splashScreenManager.ShowWaitForm();
                //splashScreenManager.SetWaitFormCaption("데이터를 불러오는 중입니다");

                //SQL = $@"
                ///* =========================
                //   1. 상세 행
                //   ========================= */
                //SELECT a.PLANT_CODE
                //     , a.PROCESS_KEY
                //     , a.L_CODE
                //     , a.WORKDATE
                //     , a.BATCH
                //     , a.NUM
                //     , TO_CHAR(a.NOTE) AS NOTE
                //     , a.RESOURCE_NO
                //     , TO_CHAR(a.DESCRIPTION) AS DESCRIPTION
                //     , a.LOCATION
                //     , a.LOCATION_ED
                //     , CASE a.SCALE_NO 
                //           WHEN 11 THEN 'L'
                //           ELSE TO_CHAR(a.SCALE_NO)
                //       END AS SCALE_NO
                //     , a.INGRED_LOT
                //     , a.INGDESC
                //     , CASE 
                //           WHEN SUBSTR(a.SCALE_NAME, 1, 1) = 'M'
                //           THEN '**********'
                //           ELSE TO_CHAR(a.NAME)
                //       END AS NAME
                //     , a.SET_VAL
                //     , a.P_Q
                //     , a.P_CHA
                //     , NVL(a.HR_ERR, 0) AS HR_ERR
                //     , ROUND(ABS(a.P_CHA / NULLIF(a.SET_VAL, 0) * 100), 2) AS HR_PER
                //     , CASE 
                //           WHEN a.P_Q > a.SET_VAL + NVL(a.HR_ERR, 0) THEN '상한이탈'
                //           WHEN a.P_Q < a.SET_VAL - NVL(a.HR_ERR, 0) THEN '하한이탈'
                //           ELSE '정상'
                //       END AS HR_STATUS
                //     , 'D' AS ROW_TYPE          -- 상세
                //FROM ( SELECT 
                //        WO.PLANT_CODE
                //      , WO.PROCESS_KEY
                //      , WO.L_CODE
                //      , SUBSTR(WO.WORKDATE, 1, 4) || '-' || SUBSTR(WO.WORKDATE, 5, 2) || '-' || SUBSTR(WO.WORKDATE, 7, 2) AS WORKDATE
                //      , WR.BATCH
                //      , WO.NUM
                //      , WO.NOTE
                //      , WO.RESOURCE_NO
                //      , TRIM(PRO.DESCRIPTION) AS DESCRIPTION
                //      , WR.LOCATION
                //      , WO.LOCATION_ED
                //      , NVL(SC.SCALE_NO, 11) AS SCALE_NO
                //      , SC.SCALE_NAME
                //      , WR.INGRED_LOT
                //      , PROD.DESCRIPTION AS INGDESC
                //      , TRIM(WR.NAME) AS NAME
                //      , WD.SET_VAL
                //      , WR.P_Q
                //      , (WD.SET_VAL - WR.P_Q) * -1 AS P_CHA
                //      , WD.HR_ERR
                //    FROM WORK_REMARK WR
                //    JOIN WORK_DETAIL WD 
                //        ON WD.PLANT_CODE   = WR.PLANT_CODE
                //       AND WD.PROCESS_KEY = WR.PROCESS_KEY 
                //       AND WD.L_CODE      = WR.L_CODE
                //       AND WD.WORKDATE    = WR.WORKDATE 
                //       AND WD.NUM         = WR.NUM 
                //       AND WD.INGRED_CODE = WR.INGRED_LOT
                //    JOIN WORK_ORDER WO 
                //        ON WO.PLANT_CODE   = WR.PLANT_CODE
                //       AND WO.PROCESS_KEY = WR.PROCESS_KEY 
                //       AND WO.L_CODE      = WR.L_CODE
                //       AND WO.WORKDATE    = WR.WORKDATE 
                //       AND WO.NUM         = WR.NUM
                //    LEFT OUTER JOIN BIN B 
                //        ON WR.PLANT_CODE   = B.PLANT_CODE
                //       AND WR.PROCESS_KEY = B.PROCESS_KEY 
                //       AND WR.L_CODE      = B.L_CODE
                //       AND WR.LOCATION    = B.LOCATION
                //    LEFT OUTER JOIN SCALE SC 
                //        ON WD.PLANT_CODE   = SC.PLANT_CODE
                //       AND WD.PROCESS_KEY = SC.PROCESS_KEY 
                //       AND WD.L_CODE      = SC.L_CODE
                //       AND WD.SCALE_CODE  = SC.SCALE_CODE
                //    LEFT OUTER JOIN SAP_DI_PRODUCT PRO 
                //        ON WO.PLANT_CODE   = PRO.PLANT_CODE
                //       AND WO.RESOURCE_NO = PRO.RESOURCE_NO
                //    LEFT OUTER JOIN SAP_DI_PRODUCT PROD 
                //        ON WO.PLANT_CODE   = PROD.PLANT_CODE
                //       AND WR.INGRED_LOT  = PROD.RESOURCE_NO
                //    WHERE WR.PLANT_CODE = '{cboPlant_Code.EditValue}'
                //        AND WR.PROCESS_KEY = '{cboProcessKey.EditValue}'
                //        AND WR.L_CODE = '{cboL_Code.EditValue}'
                //        AND WR.WORKDATE BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}') a

                //UNION ALL

                ///* =========================
                //   2. 소계 행
                //   ========================= */
                //SELECT a.PLANT_CODE
                //     , a.PROCESS_KEY
                //     , a.L_CODE
                //     , a.WORKDATE
                //     , NULL AS BATCH
                //     , NULL AS NUM
                //     , NULL AS NOTE
                //     , a.RESOURCE_NO
                //     , '소계'  AS DESCRIPTION
                //     , NULL AS LOCATION
                //     , NULL AS LOCATION_ED
                //     , NULL AS SCALE_NO
                //     , NULL AS INGRED_LOT
                //     , NULL AS INGDESC
                //     , NULL AS NAME
                //     , SUM(a.SET_VAL) AS SET_VAL
                //     , SUM(a.P_Q)     AS P_Q
                //     , SUM(a.P_CHA)   AS P_CHA
                //     , NULL AS HR_ERR
                //     , NULL AS HR_PER
                //     , NULL AS HR_STATUS
                //     , 'S' AS ROW_TYPE          -- 소계
                //FROM ( SELECT 
                //        WO.PLANT_CODE
                //      , WO.PROCESS_KEY
                //      , WO.L_CODE
                //      , SUBSTR(WO.WORKDATE, 1, 4) || '-' || SUBSTR(WO.WORKDATE, 5, 2) || '-' || SUBSTR(WO.WORKDATE, 7, 2) AS WORKDATE
                //      , WR.BATCH
                //      , WO.NUM
                //      , WO.NOTE
                //      , WO.RESOURCE_NO
                //      , TRIM(PRO.DESCRIPTION) AS DESCRIPTION
                //      , WR.LOCATION
                //      , WO.LOCATION_ED
                //      , NVL(SC.SCALE_NO, 11) AS SCALE_NO
                //      , SC.SCALE_NAME
                //      , WR.INGRED_LOT
                //      , PROD.DESCRIPTION AS INGDESC
                //      , TRIM(WR.NAME) AS NAME
                //      , WD.SET_VAL
                //      , WR.P_Q
                //      , (WD.SET_VAL - WR.P_Q) * -1 AS P_CHA
                //      , WD.HR_ERR
                //    FROM WORK_REMARK WR
                //    JOIN WORK_DETAIL WD 
                //        ON WD.PLANT_CODE   = WR.PLANT_CODE
                //       AND WD.PROCESS_KEY = WR.PROCESS_KEY 
                //       AND WD.L_CODE      = WR.L_CODE
                //       AND WD.WORKDATE    = WR.WORKDATE 
                //       AND WD.NUM         = WR.NUM 
                //       AND WD.INGRED_CODE = WR.INGRED_LOT
                //    JOIN WORK_ORDER WO 
                //        ON WO.PLANT_CODE   = WR.PLANT_CODE
                //       AND WO.PROCESS_KEY = WR.PROCESS_KEY 
                //       AND WO.L_CODE      = WR.L_CODE
                //       AND WO.WORKDATE    = WR.WORKDATE 
                //       AND WO.NUM         = WR.NUM
                //    LEFT OUTER JOIN BIN B 
                //        ON WR.PLANT_CODE   = B.PLANT_CODE
                //       AND WR.PROCESS_KEY = B.PROCESS_KEY 
                //       AND WR.L_CODE      = B.L_CODE
                //       AND WR.LOCATION    = B.LOCATION
                //    LEFT OUTER JOIN SCALE SC 
                //        ON WD.PLANT_CODE   = SC.PLANT_CODE
                //       AND WD.PROCESS_KEY = SC.PROCESS_KEY 
                //       AND WD.L_CODE      = SC.L_CODE
                //       AND WD.SCALE_CODE  = SC.SCALE_CODE
                //    LEFT OUTER JOIN SAP_DI_PRODUCT PRO 
                //        ON WO.PLANT_CODE   = PRO.PLANT_CODE
                //       AND WO.RESOURCE_NO = PRO.RESOURCE_NO
                //    LEFT OUTER JOIN SAP_DI_PRODUCT PROD 
                //        ON WO.PLANT_CODE   = PROD.PLANT_CODE
                //       AND WR.INGRED_LOT  = PROD.RESOURCE_NO
                //    WHERE WR.PLANT_CODE = '{cboPlant_Code.EditValue}'
                //        AND WR.PROCESS_KEY = '{cboProcessKey.EditValue}'
                //        AND WR.L_CODE = '{cboL_Code.EditValue}'
                //        AND WR.WORKDATE BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}' ) a
                //GROUP BY a.PLANT_CODE
                //       , a.PROCESS_KEY
                //       , a.L_CODE
                //       , a.WORKDATE
                //       , a.RESOURCE_NO

                //ORDER BY WORKDATE
                //       , RESOURCE_NO
                //       , ROW_TYPE          -- D → S (소계가 아래로)
                //       , NUM
                //";

                //SQL = $@"
                //SELECT a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE
                //    , a.WORKDATE,
                //    a.BATCH,
                //    a.NUM,
                //    a.NOTE,
                //    a.RESOURCE_NO,
                //    a.DESCRIPTION,
                //    a.LOCATION,
                //    a.LOCATION_ED,
                //    CASE a.SCALE_NO 
                //        WHEN 11 THEN 'L' 
                //        ELSE TO_CHAR(a.SCALE_NO) 
                //    END AS SCALE_NO,
                //    a.INGRED_LOT,
                //    a.INGDESC,
                //    CASE TO_CHAR(SUBSTR(a.SCALE_NAME, 1, 1))
                //        WHEN 'M' THEN '**********' 
                //        ELSE TO_CHAR(a.NAME)
                //    END AS NAME,
                //    a.SET_VAL,
                //    a.P_Q,
                //    a.P_CHA,
                //    NVL(a.HR_ERR, 0) AS HR_ERR,
                //    ROUND(ABS(a.P_CHA / NULLIF(a.SET_VAL, 0) * 100), 2) AS HR_PER,
                //    CASE 
                //        WHEN a.P_Q > a.SET_VAL + NVL(a.HR_ERR, 0) THEN '상한이탈'
                //        WHEN a.P_Q < a.SET_VAL - NVL(a.HR_ERR, 0) THEN '하한이탈'
                //        ELSE '정상' 
                //    END AS HR_STATUS
                //FROM (
                //    SELECT 
                //        WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE
                //        , SUBSTR(WO.WORKDATE, 1, 4) || '-' || SUBSTR(WO.WORKDATE, 5, 2) || '-' || SUBSTR(WO.WORKDATE, 7, 2) AS WORKDATE,
                //        WR.BATCH,
                //        WO.NUM,
                //        WO.NOTE,
                //        WO.RESOURCE_NO,
                //        TRIM(PRO.DESCRIPTION) AS DESCRIPTION,
                //        WR.LOCATION,
                //        NVL(SC.SCALE_NO, 11) AS SCALE_NO,
                //        SC.SCALE_NAME,
                //        WR.INGRED_LOT,
                //        PROD.DESCRIPTION AS INGDESC,
                //        TRIM(WR.NAME) AS NAME,
                //        WD.SET_VAL,
                //        WR.P_Q,
                //        (WD.SET_VAL - WR.P_Q) * -1 AS P_CHA,
                //        WD.HR_ERR,
                //        WO.LOCATION_ED
                //    FROM WORK_REMARK WR
                //    JOIN WORK_DETAIL WD 
                //        ON WD.PLANT_CODE = WR.PLANT_CODE
                //        AND WD.PROCESS_KEY = WR.PROCESS_KEY 
                //        AND WD.L_CODE = WR.L_CODE
                //        AND WD.WORKDATE = WR.WORKDATE 
                //        AND WD.NUM = WR.NUM 
                //        AND WD.INGRED_CODE = WR.INGRED_LOT
                //    JOIN WORK_ORDER WO 
                //        ON WO.PLANT_CODE = WR.PLANT_CODE
                //        AND WO.PROCESS_KEY = WR.PROCESS_KEY 
                //        AND WO.L_CODE = WR.L_CODE
                //        AND WO.WORKDATE = WR.WORKDATE 
                //        AND WO.NUM = WR.NUM
                //    LEFT OUTER JOIN BIN B ON WR.PLANT_CODE = B.PLANT_CODE
                //                        AND WR.PROCESS_KEY = B.PROCESS_KEY 
                //                        AND WR.L_CODE = B.L_CODE
                //                        AND WR.LOCATION = B.LOCATION
                //    LEFT OUTER JOIN SCALE SC ON WD.PLANT_CODE = SC.PLANT_CODE
                //                        AND WD.PROCESS_KEY = SC.PROCESS_KEY 
                //                        AND WD.L_CODE = SC.L_CODE
                //                        AND WD.SCALE_CODE = SC.SCALE_CODE
                //    LEFT OUTER JOIN SAP_DI_PRODUCT PRO ON WO.PLANT_CODE = PRO.PLANT_CODE
                //                                AND WO.RESOURCE_NO = PRO.RESOURCE_NO
                //    LEFT OUTER JOIN SAP_DI_PRODUCT PROD ON WO.PLANT_CODE = PROD.PLANT_CODE
                //                                AND WR.INGRED_LOT = PROD.RESOURCE_NO
                //    WHERE WR.PLANT_CODE = '{cboPlant_Code.EditValue}'
                //        AND WR.PROCESS_KEY = '{cboProcessKey.EditValue}'
                //        AND WR.L_CODE = '{cboL_Code.EditValue}'
                //        AND WR.WORKDATE BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                //) a
                //ORDER BY a.WORKDATE, NUM, BATCH, NVL(SCALE_NO, 11), P_Q DESC
                //";

                //SQL = $@"
                //SELECT a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE
                //    , a.WORKDATE,
                //    a.BATCH,
                //    a.NUM,
                //    a.NOTE,
                //    a.RESOURCE_NO,
                //    a.DESCRIPTION,
                //    a.LOCATION,
                //    a.LOCATION_ED,
                //    CASE a.SCALE_NO 
                //        WHEN 11 THEN 'L' 
                //        ELSE TO_CHAR(a.SCALE_NO) 
                //    END AS SCALE_NO,
                //    a.INGRED_LOT,
                //    a.INGDESC,
                //    CASE TO_CHAR(SUBSTR(a.SCALE_NAME, 1, 1))
                //        WHEN 'M' THEN '**********' 
                //        ELSE TO_CHAR(a.NAME)
                //    END AS NAME,
                //    a.SET_VAL,
                //    a.P_Q,
                //    a.P_CHA,
                //    NVL(a.HR_ERR, 0) AS HR_ERR,
                //    ROUND(ABS(a.P_CHA / NULLIF(a.SET_VAL, 0) * 100), 2) AS HR_PER,
                //    CASE 
                //        WHEN a.P_Q > a.SET_VAL + NVL(a.HR_ERR, 0) THEN '상한이탈'
                //        WHEN a.P_Q < a.SET_VAL - NVL(a.HR_ERR, 0) THEN '하한이탈'
                //        ELSE '정상' 
                //    END AS HR_STATUS
                //FROM (
                //    SELECT WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE
                //        , SUBSTR(WO.WORKDATE, 1, 4) || '-' || SUBSTR(WO.WORKDATE, 5, 2) || '-' || SUBSTR(WO.WORKDATE, 7, 2) AS WORKDATE,
                //        WR.BATCH,
                //        WO.NUM,
                //        WO.NOTE,
                //        WO.RESOURCE_NO,
                //        TRIM(PRO.DESCRIPTION) AS DESCRIPTION,
                //        MAX(WR.LOCATION) AS LOCATION,
                //        NVL(SC.SCALE_NO, 11) AS SCALE_NO,
                //        SC.SCALE_NAME,
                //        WR.INGRED_LOT,
                //        PROD.DESCRIPTION AS INGDESC,
                //        TRIM(WR.NAME) AS NAME,
                //        MAX(CASE WHEN WD.PARENT_BIN IS NOT NULL THEN  0 ELSE WD.SET_VAL END) SET_VAL,
                //        SUM(WR.P_Q) AS P_Q,
                //        SUM((WD.SET_VAL - WR.P_Q) * -1) AS P_CHA,
                //        MAX(WD.HR_ERR) AS HR_ERR,
                //        WO.LOCATION_ED
                //    FROM WORK_ORDER WO 
                //        JOIN WORK_DETAIL WD 
                //            ON WD.PLANT_CODE = WO.PLANT_CODE
                //            AND WD.PROCESS_KEY = WO.PROCESS_KEY 
                //            AND WD.L_CODE = WO.L_CODE
                //            AND WD.WORKDATE = WO.WORKDATE 
                //            AND WD.NUM = WO.NUM
                //        JOIN WORK_REMARK WR
                //            ON WO.PLANT_CODE = WR.PLANT_CODE
                //            AND WO.PROCESS_KEY = WR.PROCESS_KEY 
                //            AND WO.L_CODE = WR.L_CODE
                //            AND WO.WORKDATE = WR.WORKDATE 
                //            AND WO.NUM = WR.NUM
                //            AND WD.INGRED_CODE = WR.INGRED_LOT
                //        LEFT OUTER JOIN BIN B ON WR.PLANT_CODE = B.PLANT_CODE
                //                            AND WR.PROCESS_KEY = B.PROCESS_KEY 
                //                            AND WR.L_CODE = B.L_CODE
                //                            AND WR.LOCATION = B.LOCATION
                //        LEFT OUTER JOIN SCALE SC ON WD.PLANT_CODE = SC.PLANT_CODE
                //                            AND WD.PROCESS_KEY = SC.PROCESS_KEY 
                //                            AND WD.L_CODE = SC.L_CODE
                //                            AND WD.SCALE_CODE = SC.SCALE_CODE
                //        LEFT OUTER JOIN SAP_DI_PRODUCT PRO ON WO.PLANT_CODE = PRO.PLANT_CODE
                //                                    AND WO.RESOURCE_NO = PRO.RESOURCE_NO
                //        LEFT OUTER JOIN SAP_DI_PRODUCT PROD ON WO.PLANT_CODE = PROD.PLANT_CODE
                //                                    AND WR.INGRED_LOT = PROD.RESOURCE_NO
                //    WHERE WR.PLANT_CODE = '{cboPlant_Code.EditValue}'
                //        AND WR.PROCESS_KEY = '{cboProcessKey.EditValue}'
                //        AND WR.L_CODE = '{cboL_Code.EditValue}'
                //        AND WR.WORKDATE BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                //        AND CASE WHEN WD.PARENT_BIN IS NOT NULL THEN  0 ELSE WR.P_Q END > 0
                //        AND wo.ERP_OSTATUS = 'Y'
                //        AND NVL(wo.DEL_FLAG, 'N') != 'Y'
                //    GROUP BY WO.PLANT_CODE, WO.PROCESS_KEY, WO.L_CODE,
                //        SUBSTR(WO.WORKDATE, 1, 4) || '-' || SUBSTR(WO.WORKDATE, 5, 2) || '-' || SUBSTR(WO.WORKDATE, 7, 2),
                //        WR.BATCH,
                //        WO.NUM,
                //        WO.NOTE,
                //        WO.RESOURCE_NO,
                //        TRIM(PRO.DESCRIPTION),
                //        NVL(SC.SCALE_NO, 11),
                //        SC.SCALE_NAME,
                //        WR.INGRED_LOT,
                //        PROD.DESCRIPTION,
                //        TRIM(WR.NAME),
                //        WO.LOCATION_ED
                //) a
                //ORDER BY a.WORKDATE, NUM, BATCH, NVL(SCALE_NO, 11), P_Q DESC
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
                        AND wo.WORKDATE BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                        AND ('{cboProcessKey.EditValue}' IS NULL OR wo.PROCESS_KEY = '{cboProcessKey.EditValue}')
                        AND ('{cboL_Code.EditValue}' IS NULL OR wo.L_CODE = '{cboL_Code.EditValue}')
                        AND wo.ERP_OSTATUS = 'Y'
                        AND NVL(wo.DEL_FLAG, 'N') != 'Y'
                --                    and wo.RESOURCE_NO = '1211030018'
                --                    AND wo.NUM = '10'
                ), WDETAIL AS (
                    SELECT 
                        wd.PLANT_CODE
                        , wd.PROCESS_KEY
                        , wd.L_CODE
                        , wd.WORKDATE
                        , wd.NUM
                        , wo.RESOURCE_NO
                        , wo.NOTE
                        , wd.INGRED_CODE
                        , wd.SET_VAL
                        , wd.PARENT_BIN
                        , WD.SCALE_CODE
                        , wo.LOCATION_ED
                        , wd.HR_ERR
                    FROM WORDER wo
                        JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wo.PLANT_CODE AND wd.PROCESS_KEY = wo.PROCESS_KEY AND wd.L_CODE = wo.L_CODE
                            AND wd.WORKDATE = wo.WORKDATE AND wd.NUM = wo.NUM
                    WHERE wd.PARENT_BIN IS NULL
                --                    AND wd.INGRED_code = '1222010000'
                ),WREMARK AS (
                        SELECT wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.NUM
                            ,  NVL(wd.PARENT_BIN, wr.LOCATION) AS LOCATION, SUM(wr.P_Q) AS P_Q
                            , wr.INGRED_LOT, WR.BATCH, WR.NAME
                        FROM WORDER wo
                            JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wo.PLANT_CODE AND wd.PROCESS_KEY = wo.PROCESS_KEY AND wd.L_CODE = wo.L_CODE
                                AND wd.WORKDATE = wo.WORKDATE AND wd.NUM = wo.NUM
                            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY = wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM AND wr.LOCATION = wd.LOCATION AND wr.INGRED_LOT = wd.INGRED_CODE
                --                     WHERE wr.INGRED_LOT = '1222010000'
                        GROUP BY wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.NUM
                                    , NVL(wd.PARENT_BIN, wr.LOCATION)
                                    , wr.INGRED_LOT, WR.BATCH, WR.NAME
                )

                SELECT a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE
                    , a.WORKDATE,
                    a.BATCH,
                    a.NUM,
                    a.NOTE,
                    a.RESOURCE_NO,
                    a.DESCRIPTION,
                    a.LOCATION,
                    a.LOCATION_ED,
                    CASE a.SCALE_NO 
                        WHEN 11 THEN 'L' 
                        ELSE TO_CHAR(a.SCALE_NO) 
                    END AS SCALE_NO,
                    a.INGRED_LOT,
                    a.INGDESC,
                    CASE TO_CHAR(SUBSTR(a.SCALE_NAME, 1, 1))
                        WHEN 'M' THEN '**********' 
                        ELSE TO_CHAR(a.NAME)
                    END AS NAME,
                    a.SET_VAL,
                    a.P_Q,
                    a.P_CHA,
                    NVL(a.HR_ERR, 0) AS HR_ERR,
                    ROUND(ABS(a.P_CHA / NULLIF(a.SET_VAL, 0) * 100), 2) AS HR_PER,
                    CASE 
                        WHEN a.P_Q > a.SET_VAL + NVL(a.HR_ERR, 0) THEN '상한이탈'
                        WHEN a.P_Q < a.SET_VAL - NVL(a.HR_ERR, 0) THEN '하한이탈'
                        ELSE '정상' 
                    END AS HR_STATUS
                FROM (
                    SELECT wd.PLANT_CODE, wd.PROCESS_KEY, wd.L_CODE
                        , SUBSTR(wd.WORKDATE, 1, 4) || '-' || SUBSTR(wd.WORKDATE, 5, 2) || '-' || SUBSTR(wd.WORKDATE, 7, 2) AS WORKDATE,
                        wr.BATCH,
                        wd.NUM,
                        wd.NOTE,
                        wd.RESOURCE_NO,
                        TRIM(PRO.DESCRIPTION) AS DESCRIPTION,
                        MAX(WR.LOCATION) AS LOCATION,
                        NVL(SC.SCALE_NO, 11) AS SCALE_NO,
                        SC.SCALE_NAME,
                        WR.INGRED_LOT,
                        PROD.DESCRIPTION AS INGDESC,
                        TRIM(WR.NAME) AS NAME,
                        MAX(CASE WHEN WD.PARENT_BIN IS NOT NULL THEN  0 ELSE WD.SET_VAL END) SET_VAL,
                        SUM(WR.P_Q) AS P_Q,
                        SUM((wd.SET_VAL - WR.P_Q) * -1) AS P_CHA,
                        MAX(wd.HR_ERR) AS HR_ERR,
                        wd.LOCATION_ED
                    FROM WDETAIL wd
                        JOIN WREMARK wr ON wr.PLANT_CODE = wd.PLANT_CODE AND wr.PROCESS_KEY = wd.PROCESS_KEY AND wr.L_CODE = wd.L_CODE
                                                                    AND wr.WORKDATE = wd.WORKDATE AND wr.NUM = wd.NUM AND wr.INGRED_LOT = wd.INGRED_CODE
                        LEFT OUTER JOIN BIN B ON WR.PLANT_CODE = B.PLANT_CODE
                                            AND WR.PROCESS_KEY = B.PROCESS_KEY 
                                            AND WR.L_CODE = B.L_CODE
                                            AND WR.LOCATION = B.LOCATION
                        LEFT OUTER JOIN SCALE SC ON WD.PLANT_CODE = SC.PLANT_CODE
                                            AND WD.PROCESS_KEY = SC.PROCESS_KEY 
                                            AND WD.L_CODE = SC.L_CODE
                                            AND WD.SCALE_CODE = SC.SCALE_CODE
                        LEFT OUTER JOIN SAP_DI_PRODUCT PRO ON wd.PLANT_CODE = PRO.PLANT_CODE
                                                    AND wd.RESOURCE_NO = PRO.RESOURCE_NO
                        LEFT OUTER JOIN SAP_DI_PRODUCT PROD ON wd.PLANT_CODE = PROD.PLANT_CODE
                                                    AND WR.INGRED_LOT = PROD.RESOURCE_NO
                    GROUP BY wd.PLANT_CODE, wd.PROCESS_KEY, wd.L_CODE,
                        SUBSTR(wd.WORKDATE, 1, 4) || '-' || SUBSTR(wd.WORKDATE, 5, 2) || '-' || SUBSTR(wd.WORKDATE, 7, 2),
                        WR.BATCH,
                        wd.NUM,
                        wd.NOTE,
                        wd.RESOURCE_NO,
                        TRIM(PRO.DESCRIPTION),
                        TRIM(WR.NAME),
                        NVL(SC.SCALE_NO, 11),
                        SC.SCALE_NAME,
                        WR.INGRED_LOT,
                        PROD.DESCRIPTION,
                        wd.LOCATION_ED
                ) a
                ORDER BY a.WORKDATE, NUM, BATCH, NVL(SCALE_NO, 11), P_Q DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                sValid = new string[] { "" };


                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "HR_STATUS")
            {
                if (e.CellValue.ToString() == "상한이탈")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (e.CellValue.ToString() == "하한이탈")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void btn_excelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    gridView.ExportToXlsx(sfd.FileName);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_excelExport_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
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

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessTypeCode("배합")), "", false, 0, false);

            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);
        }

        private void cboProcess_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            if (isInitializing)
            {
                gridControl.DataSource = null;
                XMain_Search();
            }
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