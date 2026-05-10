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
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_ProductInventoryLedger : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        private bool isInitializing = false;

        public frm_ProductInventoryLedger()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Stock_Info_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", false, 0, false);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            fromDtWorkDate.EditValue = DateTime.Today;
            toDtWorkDate.EditValue = DateTime.Today.AddDays(1);

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

                // 매달 1일
                var dt = Convert.ToDateTime(fromDtWorkDate.EditValue);

                string firstDate = dt.Day == 1
                    ? new DateTime(dt.Year, dt.Month, 1).AddMonths(-1).ToString("yyyyMMdd")
                    : new DateTime(dt.Year, dt.Month, 1).ToString("yyyyMMdd");

                // 시작 일
                string fromFirstDate = dt.AddDays(-1).ToString("yyyyMMdd");

                // 시작 일
                string fromDate = dt.ToString("yyyyMMdd");
                // 종료 일
                string toDate = Convert.ToDateTime(toDtWorkDate.EditValue).ToString("yyyyMMdd");

                // 마감월
                string closeDate = dt.AddMonths(-1).ToString("yyyyMM");

                SQL = $@"
                WITH AADJ AS ( 
                    -- 검색 일자 조정값
                    SELECT  a.PLANT_CODE, a.RESOURCE_NO
                        , SUM(NVL(a.ADJUST_QTY, 0)) AS ADJUST_QTY, SUM(NVL(a.LOSS_QTY, 0))  AS LOSS_QTY, SUM(NVL(a.RETURN_QTY, 0)) AS RETURN_QTY
                    FROM ADJUST a
                    WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue}'
                        AND a.ADJUST_DATE  BETWEEN '{fromDate}' AND '{toDate}'
                    GROUP BY a.PLANT_CODE, a.RESOURCE_NO    
                )
                , TADJ AS ( 
                    -- 검색 전일까지 조정값
                    SELECT  a.PLANT_CODE, a.RESOURCE_NO
                        , SUM(NVL(a.ADJUST_QTY, 0)) AS ADJUST_QTY, SUM(NVL(a.LOSS_QTY, 0))  AS LOSS_QTY, SUM(NVL(a.RETURN_QTY, 0)) AS RETURN_QTY
                    FROM ADJUST a
                    WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue}'
                        AND a.ADJUST_DATE  BETWEEN '{firstDate}' AND '{fromFirstDate}'
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
                    WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue}'
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
                    WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue}'
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
                    WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue}'
                             AND a.WORKDATE BETWEEN '{firstDate}' AND '{fromFirstDate}'
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
                    WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue}'
                            AND a.WORKDATE BETWEEN '{firstDate}' AND '{fromFirstDate}'
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
                                                        AND TO_DATE('{fromFirstDate}', 'YYYY-MM-DD HH24:MI:SS') + 1 - (1/86400)
                        AND a.CAR_TYPE = '002'
                        AND NVL(a.DEL_FLAG, 'N') != 'Y'
                    GROUP BY b.RESOURCE_NO
                )
                , PRODUCT AS (
                    -- 결과
                    SELECT sc.RESOURCE_NO, tp.DESCRIPTION
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
                    WHERE sc.CLOSE_YYYYMM = '{closeDate}'
                    ORDER BY sc.VIEW_SEQ
                )

                -- 제품코드, 제품명, 기초재고, 배합량, 부산물량, 포장대체, 조정량, 생산합계, 판매출고, 기말재고
                SELECT RESOURCE_NO, DESCRIPTION, CLOSE_QTY, PRO_Q AS PRO_Q, BU_QTY, PACK_QTY, ADJUST_QTY, NVL((PRO_Q + BU_QTY + PACK_QTY + ADJUST_QTY), 0) AS PRO_SUM
                    , WEIGHT, NVL((CLOSE_QTY + PRO_Q + BU_QTY + PACK_QTY + ADJUST_QTY) - (WEIGHT - RETURN_QTY), 0) AS LAST_QTY, RETURN_QTY, (WEIGHT - RETURN_QTY) AS WEIGHTSUM 
                FROM PRODUCT
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
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
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
                    //gridView.ExportToXlsx(sfd.FileName);
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
            //gridView.FocusedRowHandle = 0;
            //gridView.FocusedColumn = gridView.VisibleColumns[0];
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
            toDtWorkDate.EditValue = fromDtWorkDate.EditValue;

            if (isInitializing)
                XMain_Search();
        }

        private void dateEdit_workEndDate_EditValueChanged(object sender, EventArgs e)
        {
            DateTime? startDate = fromDtWorkDate.EditValue as DateTime?;
            DateTime? endDate = toDtWorkDate.EditValue as DateTime?;

            if (startDate.HasValue && endDate.HasValue)
            {
                if (endDate.Value < startDate.Value)
                {
                    fromDtWorkDate.EditValue = endDate.Value;
                }
            }
        }
    }
}