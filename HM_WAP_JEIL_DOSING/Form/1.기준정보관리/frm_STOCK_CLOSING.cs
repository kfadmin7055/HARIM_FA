using Core.Class;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_STOCK_CLOSING : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_STOCK_CLOSING()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void frm_STOCK_CLOSING_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            DateTime ym = DateTime.Today;
            DateTime firstDay = new DateTime(ym.Year, ym.Month, 1);

            dtClose.EditValue = firstDay;

            dtFromDate.EditValue = DateTime.Today;
            dtToDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();

            InitControl();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void InitControl()
        {
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("제품")}'", "", 2, true, false), "품목을 선택 해주세요.", false, null, "CODE", "NAME");

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridSCboResourceNo, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("제품")}'", "", 2, true, false), "품목을 선택 해주세요.", false, null, "CODE", "NAME");
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 재고 마감
                SELECT a.PLANT_CODE      -- 01 공장코드
                    , a.RESOURCE_NO      -- 02 자재코드
                    , a.CLOSE_YYYYMM     -- 03 마감년월
                    , a.CLOSE_QTY        -- 04 마감수량
                    , a.VIEW_SEQ         -- 보기 순서
                    , a.I_USER           -- 05 등록자
                    , a.I_TIME           -- 06 등록일시
                    , a.U_USER           -- 07 수정자
                    , a.U_TIME           -- 08 수정일시
                    , a.ADJUST_QTY       -- 09 조정수량
                FROM STOCK_CLOSING a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{clsCommon.PlantCode}' 
                    AND a.CLOSE_YYYYMM = '{dtClose.DateTime.ToString("yyyyMM")}'
                    AND (b.DESCRIPTION LIKE '%{txtResource.EditValue}%' OR a.RESOURCE_NO LIKE '%{txtResource.EditValue}%')
                ORDER BY a.VIEW_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false,  true);

                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XQty_Search()
        {
            try
            {
                SQL = $@"
                SELECT a.PLANT_CODE
                    , a.RESOURCE_NO
                    , TO_CHAR(TO_DATE(a.ADJUST_DATE, 'YYYYMMDD'), 'YYYY-MM-DD') AS ADJUST_DATE
                    , a.I_USER
                    , a.U_USER
                    , a.U_TIME
                    , a.ADJUST_QTY
                    , a.LOSS_QTY
                    , a.RETURN_QTY
                    , a.I_TIME
                FROM ADJUST a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE  a.PLANT_CODE = '{clsCommon.PlantCode}' 
                    AND a.ADJUST_DATE BETWEEN '{dtFromDate.DateTime.ToString("yyyyMMdd")}' AND '{dtToDate.DateTime.ToString("yyyyMMdd")}'
                    AND (b.DESCRIPTION LIKE '%{txtResource.EditValue}%' OR a.RESOURCE_NO LIKE '%{txtResource.EditValue}%')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridQty, viewQty, ds.Tables[0], false, true);

                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XQty_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 행추가버튼 클릭이벤트
        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);
            gridView.SetFocusedRowCellValue("PLANT_CODE", clsCommon.PlantCode);
            gridView.SetFocusedRowCellValue("CLOSE_YYYYMM", dtClose.DateTime.ToString("yyyyMM"));
        }

        private void btn_QrowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewQty);
            viewQty.SetFocusedRowCellValue("PLANT_CODE", clsCommon.PlantCode);
        }

        #endregion

        #region 새행초기화 이벤트
        private void gridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["MANAGE_TYPE"], "010603");
        }

        #endregion

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_QrowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewQty);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewQty);
        }

        #region 저장버튼 클릭이벤트
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("저장하시겠습니까?"))
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
                        INSERT INTO STOCK_CLOSING (
                                PLANT_CODE
                            , RESOURCE_NO
                            , CLOSE_YYYYMM
                            , VIEW_SEQ
                            , I_USER
                            , U_USER
                            , U_TIME
                            , ADJUST_QTY
                            , CLOSE_QTY
                            , I_TIME
                        )
                        VALUES (
                                '{dr["PLANT_CODE"]}'     /* 01 공장코드 */
                            , '{dr["RESOURCE_NO"]}'    /* 02 자재코드 */
                            , '{dr["CLOSE_YYYYMM"]}'   /* 03 마감년월 (YYYYMM) */
                            , '{dr["VIEW_SEQ"]}'
                            , '{clsCommon.UserId}'         /* 04 등록자 */
                            , '{dr["U_USER"]}'         /* 05 수정자 */
                            , '{dr["U_TIME"]}'         /* 06 수정일시 */
                            , '{dr["ADJUST_QTY"]}'     /* 07 조정수량 */
                            , '{dr["CLOSE_QTY"]}'      /* 08 마감수량 */
                            , SYSDATE                  /* 09 등록일시 */
                        )
                        ";

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
                        UPDATE STOCK_CLOSING
                        SET U_USER     = '{clsCommon.UserId}'
                            , U_TIME     = SYSDATE
                            , ADJUST_QTY = '{dr["ADJUST_QTY"]}'
                            , CLOSE_QTY  = '{dr["CLOSE_QTY"]}'
                            , VIEW_SEQ = '{dr["VIEW_SEQ"]}'
                        WHERE PLANT_CODE   = '{dr["PLANT_CODE"]}'
                        AND RESOURCE_NO  = '{dr["RESOURCE_NO"]}'
                        AND CLOSE_YYYYMM = '{dr["CLOSE_YYYYMM"]}'
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

        private void btn_Qsave_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("저장하시겠습니까?"))
            {
                return;
            }

            XQty_Save();
        }

        private void XQty_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewQty);

                DataTable DT = (DataTable)gridQty.DataSource;

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
                        INSERT INTO ADJUST
                        (
                              PLANT_CODE
                            , RESOURCE_NO
                            , ADJUST_DATE
                            , I_USER
                            , U_USER
                            , U_TIME
                            , ADJUST_QTY
                            , LOSS_QTY
                            , RETURN_QTY
                            , I_TIME
                        )
                        VALUES
                        (
                              '{dr["PLANT_CODE"]}'        /* 01 PLANT_CODE */
                            , '{dr["RESOURCE_NO"]}'       /* 02 RESOURCE_NO */
                            , '{Convert.ToDateTime(dr["ADJUST_DATE"]).ToString("yyyyMMdd")}'       /* 03 ADJUST_DATE */
                            , '{dr["I_USER"]}'            /* 04 I_USER */
                            , '{dr["U_USER"]}'            /* 05 U_USER */
                            , '{dr["U_TIME"]}'            /* 06 U_TIME */
                            , '{dr["ADJUST_QTY"]}'        /* 07 ADJUST_QTY */
                            , '{dr["LOSS_QTY"]}'          /* 08 LOSS_QTY */
                            , '{dr["RETURN_QTY"]}'        /* 09 RETURN_QTY */
                            , SYSDATE                     /* 10 I_TIME */
                        )
                        ";

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
                        UPDATE ADJUST
                        SET RESOURCE_NO  = '{dr["RESOURCE_NO"]}'
                            , I_USER       = '{dr["I_USER"]}'
                             , U_USER       = '{dr["U_USER"]}'
                             , U_TIME       = '{dr["U_TIME"]}'
                             , ADJUST_QTY   = '{dr["ADJUST_QTY"]}'
                             , LOSS_QTY     = '{dr["LOSS_QTY"]}'
                             , RETURN_QTY   = '{dr["RETURN_QTY"]}'
                             , I_TIME       = SYSDATE
                         WHERE PLANT_CODE   = '{dr["PLANT_CODE"]}'
                           AND RESOURCE_NO  = '{dr["RESOURCE_NO"]}'
                           AND ADJUST_DATE  = '{Convert.ToDateTime(dr["ADJUST_DATE"]).ToString("yyyyMMdd")}'
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

                ShowMessageBox.XtraShowInformation("저장이 완료 되었습니다.");

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

        #endregion

        #region 삭제버튼 클릭이벤트
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(gridView);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 품목을 삭제하시겠습니까?"))
            {
                return;
            }

            XMain_Delete();

        }

        private void XMain_Delete()
        {
            try
            {
                splashScreenManager.ShowWaitForm();

                SQL = $@"
                DELETE FROM STOCK_CLOSING 
                WHERE   PLANT_CODE    = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND CLOSE_YYYYMM  = '{gridView.GetFocusedRowCellValue("CLOSE_YYYYMM")}'
                    AND RESOURCE_NO   = '{gridView.GetFocusedRowCellValue("RESOURCE_NO")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click()", ex);
                ShowMessageBox.XtraShowWarning("삭제를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void btn_Qdelete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewQty);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 품목을 삭제하시겠습니까?"))
            {
                return;
            }

            ShowMessageBox.XtraShowInformation("삭제 되었습니다.");

            XQty_Delete();
        }

        private void XQty_Delete()
        {
            try
            {
                splashScreenManager.ShowWaitForm();

                SQL = $@"
                DELETE FROM ADJUST 
                WHERE   PLANT_CODE    = '{viewQty.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND ADJUST_DATE  = '{viewQty.GetFocusedRowCellValue("ADJUST_DATE").ToString().Replace("-", "")}'
                    AND RESOURCE_NO   = '{viewQty.GetFocusedRowCellValue("RESOURCE_NO")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                XQty_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click()", ex);
                ShowMessageBox.XtraShowWarning("삭제를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }
        #endregion

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void repItemLkUpEdit_EMPLOYEE_NO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);

            if (row.RowState != DataRowState.Added)
            {
                e.Cancel = true;
                gridView.CancelUpdateCurrentRow();
                ShowMessageBox.XtraShowInformation("입력된 사번정보는 수정하실 수 없습니다.\r\n사원정보를 삭제 후 추가해주세요");
            }

        }

        private void gridView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridView view = sender as GridView;
            DataRow dr = null;

            // 현재 행의 RowHandle 가져오기
            int rowHandle = view.FocusedRowHandle;
            string fieldName = view.FocusedColumn.FieldName;

            // DataRowView로부터 DataRow 얻기
            DataRowView drv = view.GetRow(rowHandle) as DataRowView;

            if (drv != null)
            {
                dr = drv.Row;

                // 신규 입력 중인 행인지 확인
                string[] editableColumns = new[] { "PLANT_CODE", "RESOURCE_NO" };

                if (dr.RowState != DataRowState.Added && editableColumns.Contains(fieldName))
                {
                    if (!view.IsNewItemRow(rowHandle))
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }

                if (fieldName.Contains("CLOSE_YYYYMM"))
                {
                    e.Cancel = true;
                }
            }
        }

        private void gridView_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (e.FocusedColumn == null)
                return;
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
                e.Handled = true;
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
                XMain_Delete();
            }
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_Qsearch_Click(object sender, EventArgs e)
        {
            XQty_Search();
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;
            XMain_Search();
        }

        private void txtResource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }

        private void dtClose_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
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

                if (DialogResult.Yes != ShowMessageBox.Confirm("전월 기말재고를 복사 하시겠습니까?"))
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