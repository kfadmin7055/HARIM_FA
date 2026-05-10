using DevExpress.XtraEditors;

using DevExpress.XtraEditors.Repository;
using System;
using System.Data;
using System.Windows.Forms;
using Core.Class;
using Core.Extension;
using DevExpress.XtraEditors.Controls;
using System.Diagnostics;

namespace HARIM_FA_DOSING
{
    public partial class frm_WorkGroup : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        string vPlantCode = string.Empty;
        string vWorkDate = string.Empty;
        string vEmployee = string.Empty;
        string vICMCode = string.Empty;

        private string[] sMValid = null;
        private string[] sDValid = null;
        DataSet authDs;

        public frm_WorkGroup()
        {
            InitializeComponent();
            // 이벤트 핸들러를 직접 연결
            //gridView.OptionsBehavior.Editable = true;

            // 그리드 초기화 코드를 유지하고 싶은 그리드들에만 적용
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewWorkerDetail, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(gridView2, Properties.Settings.Default.FontSize);
        }

        private void frm_WorkGroup_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem24.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else
            {
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem24.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code1, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code1.EditValue = clsCommon.PlantCode;

            //작업일자
            WORK_START_DATE.EditValue = DateTime.Today;
            dateEdit_workDateStart.EditValue = DateTime.Today;
            dateEdit_workDateEnd.EditValue = DateTime.Today.AddDays(1);

            XMain_Search(WORK_START_DATE.DateTime.ToString("yyyyMMdd"));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XMain_Search(string vWorkDate, string icm_Code = "")
        {
            try
            {
                SQL = $@"
                -- 교대조 마스터
                SELECT PLANT_CODE,
                   TO_CHAR(TO_DATE(WORK_START_DATE, 'YYYYMMDD'), 'YYYY-MM-DD') AS WORK_START_DATE, ICM_CODE, ICM_DESC, 
                   ICM_ACTIVE, REASON_DESC, REMARK, 
                   FIX_CH_YN, I_TIME
                FROM SHIFT_DETAIL_D
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND WORK_START_DATE = '{vWorkDate}'
                    AND ('{icm_Code}' IS NULL OR ICM_CODE = '{icm_Code}') 
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true, true);

                sMValid = new string[] { "WORK_START_DATE", "ICM_CODE", "PLANT_CODE" };

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPlant_Code, clsCommon.GetPlant("", true));

                // 교대조
                gridcboICM_DESC.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboICM_DESC, clsCommon.GetICM(), "", false);

                // 사용유뮤
                gridcboYN.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "", false, false);

                // 초기화: 다른 그리드 데이터 제거
                //gridWorker.DataSource = null;

                ds.Dispose();

                XDetail_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "bin_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다: " + ex.Message);
            }
        }

        private void XDetail_Search()
        {
            try
            {
                SQL = $@"
                SELECT DISTINCT a.PLANT_CODE, a.WORK_START_DATE, a.ICM_CODE, a.PROCESS_RUN_ST, a.PROCESS_KEY, a.EMPLOYEE_NO
                    , FIX_CH_YN, '08:00' AS BASEWORKTIME
                    --, LPAD(EXTRACT(HOUR FROM NUMTODSINTERVAL(DAY_WORK_ET - DAY_WORK_ST, 'DAY')), 2, '0') 
                    --    || ':' || 
                    --    LPAD(EXTRACT(MINUTE FROM NUMTODSINTERVAL(DAY_WORK_ET - DAY_WORK_ST, 'DAY')), 2, '0') 
                    --    AS BASEWORKTIME
                    , TO_CHAR(a.DAY_WORK_ST, 'YYYY-MM-DD HH24:MI:SS') AS DAY_WORK_ST
                    , TO_CHAR(a.DAY_WORK_ET, 'YYYY-MM-DD HH24:MI:SS') AS DAY_WORK_ET
                    , LPAD(TRUNC(a.DAY_WORK_AT / 60), 2, '0') || ':' || LPAD(MOD(a.DAY_WORK_AT, 60), 2, '0') AS DAY_WORK_AT
                    , a.REASON_DESC, a.REMARK, a.I_TIME
                FROM SHIFT_DETAIL a
                WHERE a.PLANT_CODE = '{vPlantCode}' AND a.WORK_START_DATE = '{vWorkDate:yyyy-MM-dd}' AND a.ICM_CODE = '{vICMCode}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridWorkerDetail, viewWorkerDetail, ds.Tables[0], true, true);

                sDValid = new string[] { "PLANT_CODE", "WORK_START_DATE", "ICM_CODE", "EMPLOYEE_NO", "PROCESS_KEY" };

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboProcessKey, clsCommon.GetGridProcess(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString()), "", false, false);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboEmployeeNo, clsCommon.GetDO_INSA(cboPlant_Code.EditValue?.ToString()));
                
                gridchkFix.ValueChecked = "Y";
                gridchkFix.ValueUnchecked = "N";
                gridchkFix.NullStyle = StyleIndeterminate.Unchecked;
                gridchkFix.CheckStyle = CheckStyles.Standard;

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPC_STATUS, clsCommon.GetPcStatus(), "", false, false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Worker_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다: " + ex.Message);
            }
        }

        private void InputSearch()
        {
            try
            {
                SQL = $@"
                -- 기간별 작업조
                SELECT a.PLANT_CODE, a.WORK_START_DATE, a.ICM_CODE
                    , b.EMPLOYEE_NO
                    , CONCAT(SUM(TRUNC((c.JB_ETIME - c.JB_STIME) * 24 * 60)), '분') AS TOTAL_STANDARD_WORK_HOUR                  -- 총근무 배정 시간
                    , CONCAT(SUM(TRUNC((b.DAY_WORK_ET - b.DAY_WORK_ST) * 24 * 60)), '분') AS TOTAL_WORKING_MINUTES               -- 근무시간
                    , CONCAT(SUM(TRUNC((b.DAY_WORK_ET - b.DAY_WORK_ST) * 24 * 60) - TRUNC((c.JB_ETIME - c.JB_STIME) * 24 * 60)), '분') AS TOTAL_ADD_WORK_TIME        -- 추가근무시간
                    , COUNT(CASE WHEN b.DAY_WORK_AT IS NULL THEN 1 END) AS ADD_WORK_TIME_COUNT                                   -- 추가근무건수
                    , COUNT(TRIM(b.REASON_DESC)) AS REASON_DESC_COUNT                                                            -- 변경근무수
                FROM SHIFT_DETAIL_D a
                        INNER JOIN SHIFT_DETAIL b ON b.PLANT_CODE = a.PLANT_CODE AND b.WORK_START_DATE = a.WORK_START_DATE AND b.ICM_CODE = a.ICM_CODE
                        INNER JOIN SAP_PROCESS_DIVISION c ON c.PLANT_CODE = b.PLANT_CODE AND c.PROCESS_KEY = b.PROCESS_KEY
                WHERE a.PLANT_CODE = '{vPlantCode}' AND b.WORK_START_DATE BETWEEN '{dateEdit_workDateStart.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workDateEnd.DateTime.ToString("yyyyMMdd")}'
                GROUP BY a.PLANT_CODE, a.WORK_START_DATE, a.ICM_CODE, b.EMPLOYEE_NO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl2, gridView2, ds.Tables[0], true, true);

                clsDevexpressGrid.ItemLookUpEditSetup(grid2CboPLANT_CODE, clsCommon.GetPlant("", true));

                clsDevexpressGrid.ItemLookUpEditSetup(grid2ScboEmployeeNo, clsCommon.GetDO_INSA(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "bin_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void detail_search()
        {
            try
            {
                string argSql = string.Empty;
                // 날짜 범위 조건 추가
                string startDate = dateEdit_workDateStart.DateTime.ToString("yyyyMMdd");
                string endDate = dateEdit_workDateEnd.DateTime.ToString("yyyyMMdd");

                SQL = $@"
                -- 기간별 작업조 상세
                SELECT a.WORK_START_DATE, a.ICM_CODE
                    , a.DAY_WORK_ST, a.DAY_WORK_ET, CONCAT(a.DAY_WORK_AT, '분') AS ADD_WORK_TIME, a.REASON_DESC
                FROM SHIFT_DETAIL a
                    INNER JOIN SAP_PROCESS_DIVISION b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY
                WHERE a.PLANT_CODE = '{vPlantCode}' AND a.WORK_START_DATE BETWEEN '{startDate}' AND '{endDate}'
                    AND a.ICM_CODE = '{vICMCode}' AND a.EMPLOYEE_NO = '{vEmployee}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl_End, gridViewEnd, ds.Tables[0], true, true);

                clsDevexpressGrid.ItemLookUpEditSetup(grid2CboICM_CODE, clsCommon.GetICM(), "", false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "detail_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다: " + ex.Message);
            }
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search(WORK_START_DATE.DateTime.ToString("yyyyMMdd"));
        }

        private RepositoryItemComboBox CreateComboBoxEdit(string[] items)
        {
            RepositoryItemComboBox comboBox = new RepositoryItemComboBox();
            comboBox.Items.AddRange(items);
            return comboBox;
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                return;
            }

            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            gridView.AddNewRow();
            int newRowHandle = gridView.FocusedRowHandle;

            gridView.SetRowCellValue(newRowHandle, gridView.Columns["PLANT_CODE"], cboPlant_Code.EditValue);
            gridView.SetRowCellValue(newRowHandle, gridView.Columns["I_TIME"], DateTime.Now);

            gridView.ShowEditor();
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridControl.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sMValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    string sWORK_START_DATE = dr["WORK_START_DATE"].ToString();
                    //string sPROCESS_RUN_ST = dr["PROCESS_RUN_ST"].ToString();
                    string sICM_CODE = dr["ICM_CODE"].ToString();

                    if (string.IsNullOrEmpty(sWORK_START_DATE))
                    {
                        ShowMessageBox.XtraShowWarning("작업일자를 입력하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(sICM_CODE))
                    {
                        ShowMessageBox.XtraShowWarning("작업조를 입력하여 주세요");
                        return;
                    }

                    //if (string.IsNullOrEmpty(sPROCESS_RUN_ST))
                    //{
                    //    ShowMessageBox.XtraShowWarning("진행여부를 입력하여 주세요");
                    //    return;
                    //}

                    if (dr.RowState == DataRowState.Added)
                    {
                        string SQL = $@"
                        INSERT INTO SHIFT_DETAIL_D (
                            PLANT_CODE, WORK_START_DATE, ICM_CODE, 
                            ICM_ACTIVE, REASON_DESC, REMARK, 
                            FIX_CH_YN, I_TIME) 
                        VALUES ( 
                            '{dr["PLANT_CODE"]}',
                            '{string.Format("{0:yyyyMMdd}", DateTime.Parse(sWORK_START_DATE))}',
                            '{sICM_CODE}',
                            '{dr["ICM_ACTIVE"]}',
                            '{dr["REASON_DESC"]}',
                            '{dr["REMARK"]}',
                            '{dr["FIX_CH_YN"]}',
                            SYSDATE )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_input_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        string SQL = $@"
                        UPDATE SHIFT_DETAIL_D
                        SET    ICM_ACTIVE      = '{dr["ICM_ACTIVE"]}',
                               REASON_DESC     = '{dr["REASON_DESC"]}',
                               REMARK          = '{dr["REMARK"]}',
                               FIX_CH_YN       = '{dr["FIX_CH_YN"]}',
                               I_TIME          = SYSDATE
                        WHERE  WORK_START_DATE = '{string.Format("{0:yyyyMMdd}", DateTime.Parse(sWORK_START_DATE))}'
                        AND    PLANT_CODE = '{dr["PLANT_CODE"]}' AND ICM_CODE        = '{sICM_CODE}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_input_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    Dbconn.conn.Commit();

                    ShowMessageBox.XtraShowWarning("작업조를 추가 했습니다");

                    XMain_Search(WORK_START_DATE.DateTime.ToString("yyyyMMdd"));
                }
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_input_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search(WORK_START_DATE.DateTime.ToString("yyyyMMdd"));
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                string workDt = WORK_START_DATE.DateTime.ToString("yyyyMMdd");

                XMain_Search(workDt);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "notice_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                if (gridView.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 작업조를 선택하여 주세요");
                    return;
                }

                DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    gridView.DeleteRow(gridView.FocusedRowHandle);
                }
                else
                {
                    string ICM_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "ICM_CODE");
                    string WORK_START_DATE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORK_START_DATE");
                    string PLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PLANT_CODE");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업조를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        Dbconn.conn.BeginTransaction();

                        // Delete from SHIFT_DETAIL table
                        SQL = $"DELETE FROM SHIFT_DETAIL_D WHERE PLANT_CODE = '{PLANT_CODE}' AND WORK_START_DATE = '{WORK_START_DATE}' AND ICM_CODE = '{ICM_CODE}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업조 삭제에 실패했습니다");
                            return;
                        }


                        // Delete from DT_POST table
                        SQL = $"DELETE FROM SHIFT_DETAIL WHERE PLANT_CODE = '{PLANT_CODE}' AND WORK_START_DATE = '{WORK_START_DATE}' AND ICM_CODE = '{ICM_CODE}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업조 삭제에 실패했습니다");
                            return;
                        }

                        string from_loc = string.Empty;
                        string to_loc = string.Empty;

                        Dbconn.conn.Commit();

                        XMain_Search(WORK_START_DATE);

                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridWorkerDetail.DataSource = null;

            vPlantCode = gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
            vWorkDate = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORK_START_DATE").ToDate("");
            vICMCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "ICM_CODE");
            vEmployee = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "EMPLOYEE_NO");

            XDetail_Search();
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridControl_End.DataSource = null;

            vPlantCode = gridView2.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
            vWorkDate = gridView2.GetFocusedRowCellValue("WORK_START_DATE")?.ToString();
            vICMCode = gridView2.GetFocusedRowCellValue("ICM_CODE")?.ToString();
            vEmployee = gridView2.GetFocusedRowCellValue("EMPLOYEE_NO")?.ToString();

            detail_search();
        }

        private void btn_searchEnd_Click(object sender, EventArgs e)
        {
            InputSearch();
        }

        private void btn_rowAdd2_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewWorkerDetail.AddNewRow();
            int newRowHandle = viewWorkerDetail.FocusedRowHandle;

            viewWorkerDetail.SetFocusedRowCellValue("PLANT_CODE", gridView.GetFocusedRowCellValue("PLANT_CODE"));
            viewWorkerDetail.SetFocusedRowCellValue("WORK_START_DATE", Convert.ToDateTime(gridView.GetFocusedRowCellValue("WORK_START_DATE")).ToString("yyyyMMdd"));
            viewWorkerDetail.SetFocusedRowCellValue("PROCESS_KEY", gridView.GetFocusedRowCellValue("PROCESS_KEY"));
            viewWorkerDetail.SetFocusedRowCellValue("EMPLOYEE_NO", gridView.GetFocusedRowCellValue("EMPLOYEE_NO"));
            viewWorkerDetail.SetFocusedRowCellValue("PROCESS_RUN_ST", gridView.GetFocusedRowCellValue("PROCESS_RUN_ST"));
            viewWorkerDetail.SetFocusedRowCellValue("ICM_CODE", gridView.GetFocusedRowCellValue("ICM_CODE"));
            viewWorkerDetail.SetFocusedRowCellValue("BASEWORKTIME", "08:00");
            viewWorkerDetail.SetFocusedRowCellValue("I_TIME", DateTime.Now);

            viewWorkerDetail.ShowEditor();
        }

        private void btn_rowDel2_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewWorkerDetail);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewWorkerDetail);
        }

        private void btn_save2_Click(object sender, EventArgs e)
        {
            try
            {
                string sDAY_WORK_ST = string.Empty;
                string sDAY_WORK_ET = string.Empty;
                string sDAY_WORK_AT = string.Empty;
                string sREASON_DESC = string.Empty;

                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridWorkerDetail.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sDValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    sDAY_WORK_ST = dr["DAY_WORK_ST"].ToString();
                    sDAY_WORK_ET = dr["DAY_WORK_ET"].ToString();
                    sDAY_WORK_AT = dr["DAY_WORK_AT"].ToString();
                    sREASON_DESC = dr["REASON_DESC"].ToString();

                    DateTime dtFrom = DateTime.Parse(dr["DAY_WORK_ST"].ToString());
                    DateTime dtTo = DateTime.Parse(dr["DAY_WORK_ET"].ToString());
                    DateTime dt = DateTime.Now;

                    string timeStr = dr["DAY_WORK_AT"].ToString();
                    TimeSpan result;

                    if (!TimeSpan.TryParse(timeStr, out result))
                    {
                        result = TimeSpan.Zero;
                    }

                    TimeSpan time = result;

                    // 총 분 단위로 변환
                    int totalMinutes = (int)time.TotalMinutes;


                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SHIFT_DETAIL (
                           PLANT_CODE, WORK_START_DATE, PROCESS_KEY, ICM_CODE
                           , DAY_WORK_ST, DAY_WORK_ET, DAY_WORK_AT 
                           , FIX_CH_YN, PROCESS_RUN_ST, REASON_DESC
                           , REMARK, I_TIME, EMPLOYEE_NO) 
                        VALUES (
                            '{dr["PLANT_CODE"]}', '{string.Format("{0:yyyyMMdd}", dr["WORK_START_DATE"])}', '{dr["PROCESS_KEY"]}', '{dr["ICM_CODE"]}'
                            , TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'), TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'), '{totalMinutes}'
                            , '{dr["FIX_CH_YN"]}', '{dr["PROCESS_RUN_ST"]}', '{sREASON_DESC}'
                            , '', SYSDATE, '{dr["EMPLOYEE_NO"]}')
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save1_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                     {
                        SQL = $@"
                        UPDATE SHIFT_DETAIL
                        SET   I_TIME = SYSDATE
                            , DAY_WORK_ST = TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                            , DAY_WORK_ET = TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                            , DAY_WORK_AT = '{totalMinutes}'
                            , PROCESS_RUN_ST = '{dr["PROCESS_RUN_ST"]}'
                            , REASON_DESC = '{sREASON_DESC}'
                        WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                            AND WORK_START_DATE = '{string.Format("{0:yyyyMMdd}", dr["WORK_START_DATE"])}'
                            AND ICM_CODE = '{dr["ICM_CODE"]}'
                            AND PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save1_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();
                XDetail_Search();

                ShowMessageBox.XtraShowWarning("근무시간을 추가 했습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save1_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete2_Click(object sender, EventArgs e)
        {
            try
            {
                if (viewWorkerDetail.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 작업조를 선택하여 주세요");
                    return;
                }

                DataRow row = viewWorkerDetail.GetDataRow(viewWorkerDetail.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewWorkerDetail.DeleteRow(viewWorkerDetail.FocusedRowHandle);
                }
                else
                {
                    string ICM_CODE = clsDevexpressGrid.GetFocusedRowCellValue(viewWorkerDetail, "ICM_CODE");
                    string WORK_START_DATE = clsDevexpressGrid.GetFocusedRowCellValue(viewWorkerDetail, "WORK_START_DATE");
                    string vEmployee = clsDevexpressGrid.GetFocusedRowCellValue(viewWorkerDetail, "EMPLOYEE_NO");
                    string PROCESS_KEY = clsDevexpressGrid.GetFocusedRowCellValue(viewWorkerDetail, "PROCESS_KEY");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업조를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        Dbconn.conn.BeginTransaction();

                        // Delete from SHIFT_DETAIL table
                        SQL = $@"
                        DELETE FROM SHIFT_DETAIL
                        WHERE PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}'
                            AND WORK_START_DATE = '{WORK_START_DATE}'
                            AND ICM_CODE = '{ICM_CODE}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업조 삭제에 실패했습니다");
                            return;
                        }

                        string from_loc = string.Empty;
                        string to_loc = string.Empty;
                        Dbconn.conn.Commit();

                        XDetail_Search();
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        private void griddtWorkDate_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;
            gridView.SetRowCellValue(gridView.FocusedRowHandle, "WORK_START_DATE", DateTime.Parse(textEditor.EditValue.ToString()).ToString("yyyy-MM-dd"));
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search(WORK_START_DATE.DateTime.ToString("yyyyMMdd"));
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

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            //TextEdit textEditor = (TextEdit)sender;

            //cboPlant_Code1.EditValue = textEditor.EditValue?.ToString();

            //XMain_Search(WORK_START_DATE.DateTime.ToString("yyyyMMdd"));
        }

        private void cboPlant_Code1_EditValueChanged(object sender, EventArgs e)
        {
            //TextEdit textEditor = (TextEdit)sender;

            //cboPlant_Code.EditValue = textEditor.EditValue?.ToString();

            //InputSearch();
        }
    }
}