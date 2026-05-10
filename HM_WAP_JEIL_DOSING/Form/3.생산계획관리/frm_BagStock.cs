using Core.Class;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraSplashScreen;
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
    public partial class frm_BagStock : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_BagStock()
        {
            InitializeComponent();
        }

        private void frm_BagStock_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            DateTime ym = DateTime.Today;
            DateTime firstDay = new DateTime(ym.Year, ym.Month, 1);

            dtClose.EditValue = firstDay;

            InitControl();

            XMain_Search();
            
        }

        private void InitControl()
        {
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("제품")}', '{clsCommon.GetResourceTypeCode("상품")}'", "", 2, true, false), "품목을 선택 해주세요.", false, null, "CODE", "NAME");
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                WITH BAG_STOCK AS (
                    SELECT a.EMPLOYEE_NO                -- 01 마감사원
                        , SUM(a.END_Q) AS END_Q        -- 02 마감수량
                        , NVL(SUM(bag.PRO_QTY) - (SUM(NVL(bag.F_Q, 0)) + SUM(NVL(bag.E_Q, 0)) + SUM(NVL(bag.BAD_QTY1, 0)) + SUM(NVL(bag.BAD_QTY2, 0))), 0) AS INPUT_Q
                        , 0 AS OUTPUT_Q
                        , a.END_YM                     -- 03 마감연월
                        , a.PLANT_CODE                 -- 04 플랜트
                        , a.RESOURCE_NO                -- 05 마감품목
                        , b.DESCRIPTION                -- 06 품목명
                    FROM   BAG_C_STOCK a    -- TB01
                        JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                        LEFT JOIN BAG_ORDER bag ON bag.PLANT_CODE = a.PLANT_CODE AND bag.RESOURCE_NO = a.RESOURCE_NO
                            AND bag.C_CONDITION = '{clsCommon.GetPcStatusCode("완료")}'                    
                    WHERE a.PLANT_CODE = '{clsCommon.PlantCode}' AND a.END_YM = '{dtClose.DateTime.ToString("yyyyMM")}'
                        AND (b.DESCRIPTION LIKE '%{txtResource.EditValue}%' OR a.RESOURCE_NO LIKE '%{txtResource.EditValue}%')
                    GROUP BY a.EMPLOYEE_NO, a.END_YM, a.PLANT_CODE, a.RESOURCE_NO, b.DESCRIPTION
                    ORDER BY a.END_YM, END_Q DESC
                )

                SELECT a.EMPLOYEE_NO, a.END_YM
                    , a.END_Q, a.INPUT_Q, a.OUTPUT_Q, a.END_Q + a.INPUT_Q - a.OUTPUT_Q AS STOCK_Q
                    , a.PLANT_CODE, a.RESOURCE_NO, a.DESCRIPTION
                FROM BAG_STOCK a
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false, true);

                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

            // 신규 행 추가
            if (e.KeyCode == Keys.F3)
            {
                btn_rowAdd_Click(sender, e);
            }

            // 행 삭제
            if (e.KeyCode == Keys.Delete)
            {
                btn_rowDel_Click(sender, e);
            }

            // 저장
            if (e.Control && e.KeyCode == Keys.S)
            {
                XMain_Save();
            }

            // 삭제
            if (e.Control && e.KeyCode == Keys.D)
            {
                //XMain_Delete();
            }
        }

        private void frm_Shown(object sender, EventArgs e)
        {
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            m_BagStock child = new m_BagStock(clsCommon.PlantCode);

            child.StartPosition = FormStartPosition.CenterParent;
            if (child.ShowDialog() == DialogResult.OK)
            {
                XMain_Search();
            }
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            //if (!clsCommon.Auth_Form_Function(authDs, "W"))
            //{
            //    ShowMessageBox.XtraShowInformation("권한이 없습니다");
            //    return;
            //}

            if (DialogResult.Yes != ShowMessageBox.Confirm("재고정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);

                DataTable DT = (DataTable)gridControl.DataSource;

                if (DT == null)
                {
                    return;
                }

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                splashScreenManager.ShowWaitForm();

                foreach (DataRow dr in DT.Rows)
                {
                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO BAG_C_STOCK
                        (
                            PLANT_CODE        /* 01 */
                          , RESOURCE_NO       /* 02 */
                          , END_YM            /* 03 */
                          , END_Q             /* 04 */
                          , EMPLOYEE_NO       /* 05 */
                        )
                        VALUES
                        (
                            '{dr["PLANT_CODE"]}'      /* 01 */
                          , '{dr["RESOURCE_NO"]}'     /* 02 */
                          , '{dr["END_YM"]}'          /* 03 */
                          , '{dr["END_Q"]}'           /* 04 */
                          , '{dr["EMPLOYEE_NO"]}'     /* 05 */
                        )";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 입력에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE BAG_C_STOCK
                           SET END_Q        = '{dr["END_Q"]}'
                             , EMPLOYEE_NO  = '{dr["EMPLOYEE_NO"]}'
                         WHERE PLANT_CODE   = '{dr["PLANT_CODE"]}'
                           AND RESOURCE_NO  = '{dr["RESOURCE_NO"]}'
                           AND END_YM       = '{dr["END_YM"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 입력에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboLocation_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            
        }

        private void btnStockAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            m_BagStock child = new m_BagStock(clsCommon.PlantCode);

            child.StartPosition = FormStartPosition.CenterParent;
            if (child.ShowDialog() == DialogResult.OK)
            {
                XMain_Search();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("전월 기말재고를 생성 하시겠습니까?"))
                {
                    return;
                }

                splashScreenManager.ShowWaitForm();

                var dt = Convert.ToDateTime(dtClose.EditValue);

                // 전전달 1일
                string firstDate = new DateTime(
                                        dt.Year,
                                        dt.Month - 2,
                                        1
                                    ).ToString("yyyyMMdd");

                // 전전달 마지막일
                string LastDate = dt.AddMonths(-1).AddDays(-1).ToString("yyyyMMdd");

                // 시작 일
                string fromDate = new DateTime(
                                        dt.Year,
                                        dt.Month - 1,
                                        1
                                    ).ToString("yyyyMMdd");
                // 종료 일
                string toDate = dt.AddDays(-1).ToString("yyyyMMdd");

                // 마감월
                string closeDate = dt.AddMonths(-2).ToString("yyyyMM");

                SQL = $@"
                INSERT INTO STOCK_CLOSING
                WITH AADJ AS ( 
                    -- 검색 일자 조정값
                    SELECT  a.PLANT_CODE, a.RESOURCE_NO
                        , SUM(NVL(a.ADJUST_QTY, 0)) AS ADJUST_QTY, SUM(NVL(a.LOSS_QTY, 0))  AS LOSS_QTY, SUM(NVL(a.RETURN_QTY, 0)) AS RETURN_QTY
                    FROM ADJUST a
                    WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                        AND a.ADJUST_DATE  BETWEEN '{fromDate}' AND '{toDate}'
                    GROUP BY a.PLANT_CODE, a.RESOURCE_NO    
                )
                , TADJ AS ( 
                    -- 검색 전일까지 조정값
                    SELECT  a.PLANT_CODE, a.RESOURCE_NO
                        , SUM(NVL(a.ADJUST_QTY, 0)) AS ADJUST_QTY, SUM(NVL(a.LOSS_QTY, 0))  AS LOSS_QTY, SUM(NVL(a.RETURN_QTY, 0)) AS RETURN_QTY
                    FROM ADJUST a
                    WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                        AND a.ADJUST_DATE  BETWEEN '{firstDate}' AND '{LastDate}'
                    GROUP BY a.PLANT_CODE, a.RESOURCE_NO    
                )
                , BOM AS (
                    -- 배합비
                    SELECT DISTINCT b.PLANT_CODE, b.MENGE, b.RESOURCE_NO, b.NOTE, a.BMENG
                    FROM SAP_IN_BOM_CONM a
                        JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                                                                    AND b.NOTE = a.NOTE
                    WHERE SUBSTR(NVL(b.IDNRK, '1'), 1, 1) = '1'  
                            AND SUBSTR(b.RESOURCE_NO, 1, 1) = '1'
                            AND b.P_TYPE = '2'
                ), PORDER AS (
                    -- 검색 일자 포장
                    SELECT a.PLANT_CODE, a.RESOURCE_NO, wp.DESCRIPTION, SUM(a.OR_QTY) AS OR_QTY, SUM(b.MENGE * a.PRO_QTY / b.BMENG) AS P_Q
                    FROM PACK_ORDER a
--                        JOIN PACK_REMARK b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE
--                                                                    AND b.WORKDATE = a.WORKDATE AND b.WORK_SEQ = a.WORK_SEQ
                        LEFT OUTER JOIN BOM b ON b.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = b.RESOURCE_NO AND b.NOTE = a.NOTE
                        LEFT JOIN SAP_DI_PRODUCT wp ON wp.PLANT_CODE = a.PLANT_CODE AND wp.RESOURCE_NO = a.RESOURCE_NO
                    WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                            AND a.WORKDATE BETWEEN '{fromDate}' AND '{toDate}'
                             AND a.ERP_OSTATUS = 'Y'
                            AND NVL(a.DEL_FLAG, 'N') != 'Y'
                    GROUP BY a.PLANT_CODE, a.RESOURCE_NO, wp.DESCRIPTION
                )
                , TWORDER AS (
                    -- 검색 일자 배합
                    SELECT a.PLANT_CODE, a.RESOURCE_NO, SUM(a.OR_Q) AS OR_Q, SUM(a.PRO_Q) AS PRO_Q, SUM(a.BATCH * a.BATCH_Q * b.PART_P / 100) AS BU_QTY, -NVL(c.P_Q, 0) AS PACK_Q
                    FROM WORK_ORDER a
                        LEFT JOIN SAP_DI_PRODUCT wp ON wp.PLANT_CODE = a.PLANT_CODE AND wp.RESOURCE_NO = a.RESOURCE_NO
                        LEFT JOIN SAP_IN_PRODUCT_RC b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND a.BU_YN = 'Y'
                        LEFT JOIN PORDER c ON c.PLANT_CODE = a.PLANT_CODE AND (SUBSTR(c.DESCRIPTION, 1, INSTR(c.DESCRIPTION, '(C') - 1) = SUBSTR(wp.DESCRIPTION, 1, INSTR(wp.DESCRIPTION, '(C') - 1)
                                                                            OR SUBSTR(c.DESCRIPTION, 1, INSTR(c.DESCRIPTION, '(P') - 1) = SUBSTR(wp.DESCRIPTION, 1, INSTR(wp.DESCRIPTION, '(P') - 1))
                    WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                            AND a.WORKDATE BETWEEN '{fromDate}' AND '{toDate}'
                            AND a.ERP_OSTATUS = 'Y'
                            AND NVL(a.DEL_FLAG, 'N') != 'Y'
                    GROUP BY a.PLANT_CODE, a.RESOURCE_NO, c.P_Q
                )
                , DECAR AS (
                    -- 검색 일자 입차 차량
                    SELECT b.RESOURCE_NO, SUM(b.WEIGHT) AS WEIGHT, SUM(b.QTY) AS QTY, SUM(c.PLANQTY) AS PLANQTY
                    FROM WAP_DECAR a
                        INNER JOIN TMS_OUTPUT_RESULT b ON b.IS_NO = a.IS_NO
                        INNER JOIN TMS_INPUT_PLOADD_CON c ON c.ORDERNO = b.ORDERNO AND c.ORDERLINENO = b.ORDERLINENO AND c.ORDERTYPECODE != 'ZLR1'
                    WHERE a.OUTCAR_DATE IS NOT NULL
                        AND a.OUTCAR_DATE BETWEEN TO_DATE('{fromDate}', 'YYYY-MM-DD HH24:MI:SS')
                                                        AND TO_DATE('{toDate}', 'YYYY-MM-DD HH24:MI:SS') + 1 - (1/86400)
                        AND a.CAR_TYPE = '002'
                        AND NVL(a.DEL_FLAG, 'N') != 'Y'
                    GROUP BY b.RESOURCE_NO
                )
                , FPORDER AS (
                    -- 검색 일자 이전 포장
                    SELECT a.PLANT_CODE, a.RESOURCE_NO, wp.DESCRIPTION, SUM(a.OR_QTY) AS OR_QTY, SUM(b.MENGE * a.PRO_QTY / b.BMENG) AS P_Q
                    FROM PACK_ORDER a
                    --    JOIN PACK_REMARK pr ON pr.PLANT_CODE = a.PLANT_CODE AND pr.PROCESS_KEY = a.PROCESS_KEY AND pr.L_CODE = a.L_CODE
                    --                                               AND pr.WORKDATE = a.WORKDATE AND pr.WORK_SEQ = a.WORK_SEQ
                        LEFT JOIN SAP_DI_PRODUCT wp ON wp.PLANT_CODE = a.PLANT_CODE AND wp.RESOURCE_NO = a.RESOURCE_NO
                        LEFT OUTER JOIN BOM b ON b.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = b.RESOURCE_NO AND b.NOTE = a.NOTE
                    WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                             AND a.WORKDATE BETWEEN '{firstDate}' AND '{LastDate}'
                             AND a.ERP_OSTATUS = 'Y'
                            AND NVL(a.DEL_FLAG, 'N') != 'Y'
                    GROUP BY a.PLANT_CODE, a.RESOURCE_NO, wp.DESCRIPTION
                )
                , FTWORDER AS (
                    -- 검색 일자 이전 배합
                    SELECT a.PLANT_CODE, a.RESOURCE_NO, SUM(a.OR_Q) AS OR_Q, SUM(a.PRO_Q) AS PRO_Q, SUM(a.BATCH * a.BATCH_Q * b.PART_P / 100) AS BU_QTY, -NVL(c.P_Q, 0) AS PACK_Q
                    FROM WORK_ORDER a
                        LEFT JOIN SAP_DI_PRODUCT wp ON wp.PLANT_CODE = a.PLANT_CODE AND wp.RESOURCE_NO = a.RESOURCE_NO
                        LEFT JOIN SAP_IN_PRODUCT_RC b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND a.BU_YN = 'Y'
                        LEFT JOIN FPORDER c ON c.PLANT_CODE = a.PLANT_CODE AND (SUBSTR(c.DESCRIPTION, 1, INSTR(c.DESCRIPTION, '(C') - 1) = SUBSTR(wp.DESCRIPTION, 1, INSTR(wp.DESCRIPTION, '(C') - 1)
                                                                            OR SUBSTR(c.DESCRIPTION, 1, INSTR(c.DESCRIPTION, '(P') - 1) = SUBSTR(wp.DESCRIPTION, 1, INSTR(wp.DESCRIPTION, '(P') - 1))
                    WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                            AND a.WORKDATE BETWEEN '{firstDate}' AND '{LastDate}'
                             AND a.ERP_OSTATUS = 'Y'
                            AND NVL(a.DEL_FLAG, 'N') != 'Y'
                    GROUP BY a.PLANT_CODE, a.RESOURCE_NO, c.P_Q
                )
                , FDECAR AS (
                    -- 검색일자 이전 입차 차량
                    SELECT b.RESOURCE_NO, SUM(b.WEIGHT) AS WEIGHT, SUM(b.QTY) AS QTY, SUM(c.PLANQTY) AS PLANQTY
                    FROM WAP_DECAR a
                        INNER JOIN TMS_OUTPUT_RESULT b ON b.IS_NO = a.IS_NO
                        INNER JOIN TMS_INPUT_PLOADD_CON c ON c.ORDERNO = b.ORDERNO AND c.ORDERLINENO = b.ORDERLINENO AND c.ORDERTYPECODE != 'ZLR1'
                    WHERE a.OUTCAR_DATE IS NOT NULL
                        AND a.OUTCAR_DATE BETWEEN TO_DATE('{firstDate}', 'YYYY-MM-DD HH24:MI:SS')
                                                        AND TO_DATE('{LastDate}', 'YYYY-MM-DD HH24:MI:SS') + 1 - (1/86400)
                        AND a.CAR_TYPE = '002'
                        AND NVL(a.DEL_FLAG, 'N') != 'Y'
                    GROUP BY b.RESOURCE_NO
                )
                , PRODUCT AS (
                    -- 결과
                    SELECT sc.PLANT_CODE, sc.RESOURCE_NO, tp.DESCRIPTION
                        , NVL(CASE WHEN SUBSTR('{fromDate}', 7, 2) = '01'
                             THEN sc.CLOSE_QTY
                            ELSE (
                                NVL(sc.CLOSE_QTY, 0) + (NVL(ftw.OR_Q, 0) + NVL(tad.LOSS_QTY, 0)) + NVL(NVL(ftw.PACK_Q, fpo.P_Q), 0) + NVL(tad.ADJUST_QTY, 0) - (NVL(fdcar.WEIGHT, 0) - NVL(tad.RETURN_QTY, 0))
                            ) END, 0) CLOSE_QTY
                        , NVL(tw.OR_Q, 0) - NVL(tw.BU_QTY, 0) + NVL(aad.LOSS_QTY, 0) AS PRO_Q, NVL(tw.BU_QTY, 0) AS BU_QTY
                        , NVL(NVL(tw.PACK_Q, po.P_Q), 0) AS PACK_QTY, NVL(dcar.WEIGHT, 0) AS WEIGHT, NVL(aad.ADJUST_QTY, 0) AS ADJUST_QTY
                        , NVL(aad.RETURN_QTY, 0) AS RETURN_QTY
                    FROM STOCK_CLOSING sc 
                        LEFT JOIN TWORDER tw ON tw.PLANT_CODE = sc.PLANT_CODE AND tw.RESOURCE_NO = sc.RESOURCE_NO
                        JOIN SAP_DI_PRODUCT tp ON tp.PLANT_CODE = sc.PLANT_CODE AND tp.RESOURCE_NO = sc.RESOURCE_NO
                        LEFT JOIN PORDER po ON po.PLANT_CODE = sc.PLANT_CODE AND po.RESOURCE_NO = sc.RESOURCE_NO
                        LEFT JOIN SAP_DI_PRODUCT pp ON pp.PLANT_CODE = sc.PLANT_CODE AND pp.RESOURCE_NO = sc.RESOURCE_NO 
                        LEFT JOIN DECAR dcar ON dcar.RESOURCE_NO = sc.RESOURCE_NO
                        LEFT JOIN FTWORDER ftw ON ftw.PLANT_CODE = sc.PLANT_CODE AND ftw.RESOURCE_NO = sc.RESOURCE_NO
                        LEFT JOIN FPORDER fpo ON fpo.PLANT_CODE = sc.PLANT_CODE AND fpo.RESOURCE_NO = sc.RESOURCE_NO
                        LEFT JOIN FDECAR fdcar ON fdcar.RESOURCE_NO = sc.RESOURCE_NO
                        LEFT JOIN AADJ aad ON aad.PLANT_CODE = sc.PLANT_CODE AND aad.RESOURCE_NO = sc.RESOURCE_NO
                        LEFT JOIN TADJ tad ON tad.PLANT_CODE = sc.PLANT_CODE AND tad.RESOURCE_NO = sc.RESOURCE_NO
                    WHERE sc.CLOSE_YYYYMM = '{dt.AddMonths(-2).ToString("yyyyMM")}'
                    ORDER BY sc.VIEW_SEQ
                )
                SELECT a.PLANT_CODE
                    , a.RESOURCE_NO
                    , '{dt.ToString("yyyyMM")}'
                    , NVL((a.CLOSE_QTY + a.PRO_Q + a.BU_QTY + a.PACK_QTY + a.ADJUST_QTY) - (a.WEIGHT - a.RETURN_QTY), 0) AS LAST_QTY 
                    , 'kfirst'           -- 05 등록자
                    , SYSDATE           -- 06 등록일시
                    , NULL           -- 07 수정자
                    , NULL           -- 08 수정일시
                    , NULL
                    , b.VIEW_SEQ
                FROM PRODUCT a
                    LEFT JOIN STOCK_CLOSING b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.CLOSE_YYYYMM = '{dt.AddMonths(-1).ToString("yyyyMM")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btnCopy_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 복사에 실패했습니다");
                    return;
                }

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click()", ex);
                ShowMessageBox.XtraShowWarning("복사를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }
    }
}