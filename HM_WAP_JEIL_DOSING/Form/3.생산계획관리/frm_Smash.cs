using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Tab;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Smash : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;
        private string[] sValid = null;
        private string sWorkSeq = string.Empty;

        public frm_Smash()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewSmash, Properties.Settings.Default.FontSize);
        }

        private void viewSmash_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridSmash, viewSmash);
        }

        private void frm_Smash_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridSmash, viewSmash);

            authDs = clsSql.GetAuthDataSet(this.Name);

            dt_From_workDate.EditValue = DateTime.Today;
            dt_To_workDate.EditValue = DateTime.Today;

            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetGridLine(clsCommon.PlantCode, clsCommon.GetProcessKey("분쇄")), "", false, 1, true);

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "전체선택", false, 2, true);

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridSmash));
        }

        #region 작업순번생성
        private string workNumber_maker()
        {
            try
            {
                string SQL = $@"
                SELECT NVL(MAX(WORK_SEQ) + 1, 1) AS SEQ
                FROM SMASH_REPORT a
                WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                    AND a.PROCESS_KEY = '{clsCommon.GetProcessKey("분쇄")}'
                    AND a.L_CODE = '{cboL_Code.EditValue}'
                    AND a.WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dt_From_workDate.EditValue)}'
                ";

                DataSet Ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(Ds) == 0)
                {
                    clsLog.logSave("작업순번 생성에러 / SQL : " + SQL, 0);
                    return string.Empty;
                }

                string return_seq = Dbconn.conn.getData(Ds, "SEQ", 0);
                Ds.Dispose();

                return return_seq;

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "workNumber_maker", ex);
                return string.Empty;
            }
        }
        #endregion

        #region 작업지시 조회 함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 분쇄 작업지시
                SELECT 
                    a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, 
                    a.WORK_NUMBER, a.WORK_SEQ, a.RESOURCE_NO, 
                    a.ICM_CODE, a.EMPLOYEE_NO, a.C_CONDITION, 
                    TO_CHAR(a.RUN_ST, 'YYYY-MM-DD HH24:MI:SS') RUN_ST,
                    TO_CHAR(a.RUN_ET, 'YYYY-MM-DD HH24:MI:SS') RUN_ET,
                    a.SC_FORE, a.SC_BG, a.SCREEN, a.FEED_RATE, 
                    a.LOAD, a.GR_WV, a.GR_QTY, 
                    a.DUST_WV1, a.DUST_WV2, a.TH_GR_QTY, 
                    a.LOCATION_ST1, a.LOCATION_ST2, a.LOCATION_ED1, 
                    a.LOCATION_ED2, a.PRO_QTY, a.I_TIME, 
                    a.REMARK, a.DEL_FLAG, a.ERP_UP_YN, 
                    a.ERP_TNUMBER, a.PRODUCTIVITY, a.RUN_TIME, a.STOP_TIME
                FROM SMASH_REPORT a
                WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                    AND a.PROCESS_KEY = '{clsCommon.GetProcessKey("분쇄")}'
                    AND ('{cboL_Code.EditValue}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue}')
                    AND a.WORK_NUMBER BETWEEN '{string.Format("{0:yyyyMMdd}", dt_From_workDate.EditValue)}' AND '{string.Format("{0:yyyyMMdd}", dt_To_workDate.EditValue)}'
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                ORDER BY a.WORK_NUMBER, a.WORK_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridSmash, viewSmash, ds.Tables[0], true);

                //viewSmash.SetFixCol(new string[] {  "ERP_UP_YN"
                //    , "L_CODE"
                //    , "WORK_NUMBER"
                //    , "WORK_SEQ"
                //    , "RESOURCE_NO"});

                sValid = new string[] { "PLANT_CODE", "PROCESS_KEY", "L_CODE", "WORK_NUMBER", "RESOURCE_NO", "ICM_CODE", "EMPLOYEE_NO", "LOCATION_ST1", "LOCATION_ED1" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = viewSmash.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                viewSmash.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

                // ERP 전송구분
                clsDevexpressGrid.ItemLookUpEditSetup(gridSmaCboERPUpLoad, clsCommon.GetTransFlag(), "", false, false);

                // 라인코드
                clsDevexpressGrid.ItemLookUpEditSetup(gridSmaCboLCode, clsCommon.GetGridLine(clsCommon.PlantCode, clsCommon.GetProcessKey("분쇄")), "", false, false);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridSmaScboResourceNo, clsCommon.GetResource(clsCommon.PlantCode, clsCommon.GetProcessKey("분쇄"), $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 2, true, false, "KG", true), "품목을 선택 해주세요.", false);
                DevExpress.XtraGrid.Views.Grid.GridView view = gridSmaScboResourceNo.View as DevExpress.XtraGrid.Views.Grid.GridView;

                view.OptionsView.ColumnAutoWidth = false;

                // 원하는 컬럼 사이즈 지정
                view.Columns["CODE"].Width = 100;
                view.Columns["NAME"].Width = 260;

                // 교대조
                clsDevexpressGrid.ItemLookUpEditSetup(gridSmaCboICM, clsCommon.GetICM(), "", false, false);

                // 작업자
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridSmaScboEmployees, clsCommon.GetDO_INSA(clsCommon.PlantCode));

                // 진행 상태
                clsDevexpressGrid.ItemLookUpEditSetup(gridSmaCboPcStatus, clsCommon.GetPcStatus(), "", false, false);

                // 인출빈
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridSmaScboLocationSt, clsCommon.GetBin(clsCommon.PlantCode, clsCommon.GetProcessKey("분쇄"), cboL_Code.EditValue?.ToString()));

                // 목적빈
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridSmaScboLocationSt, clsCommon.GetBin(clsCommon.PlantCode, clsCommon.GetProcessKey("분쇄"), cboL_Code.EditValue?.ToString()));

                // 삭제여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridSmaCboDelFlag, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "", false, false);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "car_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XSub_Search()
        {
            try
            {
                SQL = $@"
                SELECT PLANT_CODE, PROCESS_KEY, L_CODE, WORK_NUMBER, WORK_SEQ
                    , PROD_DATE, START_TIME, END_TIME, RUN_TIME, GRINDER
                    , RESOURCE_NO, ORIGIN, GRINDER_SPEED, FEEDER_NAME, PROD_QTY
                    , PRODUCTIVITY, B_W, MESH_6, MESH_8, MESH_10
                    , MESH_18, MESH_35, MESH_PAN, REMARK, STOP_TIME
                FROM SMASH_REPORT_ADD
                WHERE PLANT_CODE = '{viewSmash.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewSmash.GetFocusedRowCellValue("PROCESS_KEY")}' AND L_CODE = '{viewSmash.GetFocusedRowCellValue("L_CODE")}'
                    AND WORKDATE = '{viewSmash.GetFocusedRowCellValue("WORKDATE")}' AND WORK_SEQ = '{viewSmash.GetFocusedRowCellValue("WORK_SEQ")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridReport, viewReport, ds.Tables[0], true, true);
                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "car_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 새로고침
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }
        #endregion

        #region 행추가
        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (cboL_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("공정 라인을 먼저 선택 조회 해주세요.");
                cboL_Code.Focus();
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewSmash);

            viewSmash.SetFocusedRowCellValue("PLANT_CODE", clsCommon.PlantCode);
            viewSmash.SetFocusedRowCellValue("PROCESS_KEY", clsCommon.GetProcessKey("분쇄"));
            viewSmash.SetFocusedRowCellValue("L_CODE", cboL_Code.EditValue?.ToString());
            viewSmash.SetFocusedRowCellValue("WORK_NUMBER", string.Format("{0:yyyyMMdd}", dt_From_workDate.EditValue));
            viewSmash.SetRowCellValue(viewSmash.FocusedRowHandle, viewSmash.Columns["EMPLOYEE_NO"], clsCommon.UserId);
            viewSmash.SetRowCellValue(viewSmash.FocusedRowHandle, viewSmash.Columns["GR_QTY"], 0);
            viewSmash.SetRowCellValue(viewSmash.FocusedRowHandle, viewSmash.Columns["C_CONDITION"], clsCommon.PcStatus.Plan);
            viewSmash.SetRowCellValue(viewSmash.FocusedRowHandle, viewSmash.Columns["DEL_FLAG"], "N");
        }
        #endregion

        #region 행삭제
        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewSmash);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewSmash);
        }
        #endregion

        #region 에디터활성 이벤트
        private void viewSmash_work_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (viewSmash.GetFocusedRowCellValue("C_CONDITION").Equals(clsCommon.GetPcStatusCode("진행")) ||     // 031003	진행
                    viewSmash.GetFocusedRowCellValue("C_CONDITION").Equals(clsCommon.PcStatus.Completed)
                    ) //작지가 진행,완료처리된것은 수정못하도록 에디트모드 off
                {
                    e.Cancel = true;

                }
                else
                {
                    e.Cancel = false;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "viewSmash_work_ShowingEditor", ex);
            }
        }
        #endregion

        private void btn_workEnd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (viewSmash.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("강제완료 하실 작업지시를 선택하여 주세요");
                    return;
                }

                DataRow row = viewSmash.GetDataRow(viewSmash.FocusedRowHandle);

                if (row.RowState != DataRowState.Added)
                {
                    string plantCode = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "PLANT_CODE");
                    string processKey = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "PROCESS_KEY");
                    string lCode = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "L_CODE");
                    string swork_number = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "WORK_NUMBER");
                    string work_seq = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "WORK_SEQ");
                    string run_st = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "RUN_ST");
                    string insert_st_time = string.Empty;

                    string con_st = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "C_CONDITION");

                    if (con_st == clsCommon.PcStatus.Completed)
                    {
                        ShowMessageBox.XtraShowInformation("이미 완료처리 된 작업지시 입니다");
                        return;
                    }

                    if (!SetPROTY(plantCode, processKey, lCode, swork_number, work_seq))
                    {
                        Dbconn.conn.Rollback();
                        ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                        return;
                    }

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업순번 " + work_seq + " 작업지시를 완료 하시겠습니까?");
                    if (result == DialogResult.Yes)
                    {
                        Dbconn.conn.BeginTransaction();

                        SQL = $@"
                        UPDATE SMASH_REPORT
                        SET C_CONDITION = '{clsCommon.PcStatus.Completed}', RUN_ET = SYSDATE
                        WHERE PLANT_CODE = '{clsCommon.PlantCode}'
                            AND PROCESS_KEY = '{clsCommon.GetProcessKey("분쇄")}'
                            AND L_CODE = '{cboL_Code.EditValue}'
                            AND WORK_NUMBER = '{swork_number}'
                            AND WORK_SEQ = '{work_seq}'
                        ";
                        
                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_workEnd_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }


                        Dbconn.conn.Commit();
                    }
                }
                else
                {
                    ShowMessageBox.XtraShowInformation("해당 작업지시는 저장을 완료하신후에 완료 하여 주시길 바랍니다");
                    return;
                }


                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_workEnd_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        /// <summary>
        /// 생산성 업데이트
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sProcessKey"></param>
        /// <param name="sLCode"></param>
        /// <param name="sWorkDate"></param>
        /// <param name="sWorkSeq"></param>
        private bool SetPROTY(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate, string sWorkSeq)
        {
            bool returnStatus = true;

            SQL = $@"
            SELECT RUN_ST, SYSDATE AS RUN_ET, PRO_QTY
            FROM SMASH_REPORT
                WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                        AND WORK_NUMBER = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{sWorkSeq}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
            {
                if (ds.Tables[0].Rows[0]["RUN_ST"]?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("작업 시작 먼저 해주세요.");
                    return !returnStatus;
                }

                if (ds.Tables[0].Rows[0]["RUN_ET"]?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("이미 종료된 작업입니다.");
                    return !returnStatus;
                }

                //if (ds.Tables[0].Rows[0]["QTY"]?.ToString() == "")
                //{
                //    ShowMessageBox.XtraShowWarning("생산량을 입력 해주세요.");
                //    return;
                //}

                DateTime runSt = Convert.ToDateTime(ds.Tables[0].Rows[0]["RUN_ST"]);
                DateTime runEt = Convert.ToDateTime(ds.Tables[0].Rows[0]["RUN_ET"]);

                // 시간 차이
                double diffHours = (runEt - runSt).TotalHours;

                //if (diffHours < 0.1)
                //{
                //    ShowMessageBox.XtraShowWarning("종료시간이 너무 빠릅니다.");
                //    return !returnStatus;
                //}

                // 분 차이
                double diffMinutes = (runEt - runSt).TotalMinutes;

                SQL = $@"
                /* 생산성 업데이트 */
                UPDATE SMASH_REPORT
                SET PRODUCTIVITY = CASE WHEN '{diffHours.ToString("F1")}' = 0 THEN 0 ELSE (PRO_QTY / 1000) / {diffHours.ToString("F1")} END, RUN_TIME = '{diffMinutes.ToString("F1")}'
                WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                        AND WORK_NUMBER = '{sWorkDate}' AND WORK_SEQ = '{sWorkSeq}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btnWorkStart_Click", SQL);
                    ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                    return !returnStatus;
                }
            }

            return returnStatus;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        #region 작업시작 버튼 클릭
        private void btn_workStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                sWorkSeq = viewSmash.GetFocusedRowCellValue("WORK_SEQ")?.ToString();

                if (string.IsNullOrEmpty(sWorkSeq))
                {
                    ShowMessageBox.XtraShowInformation("작업시작을 하실 작업지시를 선택하여 주세요");
                    return;
                }

                DataRow row = viewSmash.GetDataRow(viewSmash.FocusedRowHandle);

                if (row.RowState != DataRowState.Added)
                {
                    string work_seq = sWorkSeq;
                    string swork_number = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "WORK_NUMBER");
                    string run_st = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "RUN_ST");
                    string insert_st_time = string.Empty;

                    string con_st = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "C_CONDITION");

                    if (con_st == clsCommon.GetPcStatusCode("진행"))     // 031003	진행
                    {
                        ShowMessageBox.XtraShowInformation("이미 진행중인 작업지시 입니다");
                        return;
                    }

                    if (con_st == clsCommon.PcStatus.Completed)
                    {
                        ShowMessageBox.XtraShowInformation("이미 완료처리 된 작업지시 입니다");
                        return;
                    }

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업순번 " + work_seq + " 작업지시 진행하시겠습니까?");
                    if (result == DialogResult.Yes)
                    {


                        Dbconn.conn.BeginTransaction();

                        string SQL =
                        $@"UPDATE SMASH_REPORT
                           SET C_CONDITION = '{clsCommon.GetPcStatusCode("진행")}'
                             , RUN_ST = SYSDATE
                        WHERE PLANT_CODE = '{clsCommon.PlantCode}'
                            AND PROCESS_KEY = '{clsCommon.GetProcessKey("분쇄")}'
                            AND L_CODE = '{cboL_Code.EditValue}'
                            AND WORK_NUMBER = '{swork_number}'
                            AND WORK_SEQ    = '{work_seq}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_workStart_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }


                        Dbconn.conn.Commit();
                    }
                }
                else
                {
                    ShowMessageBox.XtraShowInformation("해당 작업지시는 저장을 완료하신후에 작업시작 하여 주시길 바랍니다");
                    return;
                }

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_workStart_Click", ex);
                ShowMessageBox.XtraShowError("작업을 시작하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 그리드 로우셀 클릭 이벤트
        private void viewSmash_work_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            string selData_work_seq = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "WORK_SEQ");

            if (!string.IsNullOrEmpty(selData_work_seq))
            {
                sWorkSeq = selData_work_seq;
            }
        }
        #endregion

        #region 저장 버튼 클릭
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("작업지시정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                DateTime dtRunSt = new DateTime();
                DateTime dtRunEd = new DateTime();
                clsDevexpressGrid.GridEndEdit(viewSmash);
                DataTable DT = (DataTable)gridSmash.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (DataRow dr in DT.Rows)
                {
                    dr.ClearErrors();

                    string sDtFrom = string.Empty;
                    string sDtTo = string.Empty;

                    if (!string.IsNullOrEmpty(dr["RUN_ST"].ToString()))
                    {
                        DateTime dtFrom = DateTime.Parse(dr["RUN_ST"].ToString());

                        sDtFrom = $"TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (!string.IsNullOrEmpty(dr["RUN_ET"].ToString()))
                    {
                        DateTime dtTo = DateTime.Parse(dr["RUN_ET"].ToString());

                        sDtTo = $"TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (!string.IsNullOrEmpty(dr["RUN_ST"].ToString()) && !string.IsNullOrEmpty(dr["RUN_ET"].ToString()))
                    {
                        int time_diff = Convert.ToDateTime(Convert.ToDateTime(dr["RUN_ET"]).ToString("yyyy-MM-dd HH:mm:ss")).CompareTo(Convert.ToDateTime(Convert.ToDateTime(dr["RUN_ST"]).ToString("yyyy-MM-dd HH:mm:ss")));
                        if (time_diff < 0)
                        {
                            ShowMessageBox.XtraShowInformation("종료시간이 시작시간보다 빠르거나 같습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        string swork_number = workNumber_maker();

                        if (string.IsNullOrEmpty(swork_number))
                        {
                            ShowMessageBox.XtraShowInformation("작업순번을 생성하는 도중 에러가 발생했습니다");
                            return;
                        }

                        SQL = $@"
                        INSERT INTO SMASH_REPORT (
                           PLANT_CODE, PROCESS_KEY, L_CODE , 
                           WORK_NUMBER, WORK_SEQ, RESOURCE_NO, 
                           ICM_CODE, EMPLOYEE_NO, C_CONDITION, 
                           RUN_ST, RUN_ET, SC_FORE, 
                           SC_BG, SCREEN, FEED_RATE, 
                           LOAD, GR_WV, GR_QTY, 
                           DUST_WV1, DUST_WV2, TH_GR_QTY, 
                           LOCATION_ST1, LOCATION_ST2, LOCATION_ED1, 
                           LOCATION_ED2, PRO_QTY, I_TIME, 
                           REMARK, DEL_FLAG, ERP_UP_YN, 
                           ERP_TNUMBER) 
                        VALUES (
                           '{dr["PLANT_CODE"]}'
                         , '{dr["PROCESS_KEY"]}'
                         , '{dr["L_CODE"]}'
                         , '{dr["WORK_NUMBER"]}'
                         , '{workNumber_maker()}'
                         , '{dr["RESOURCE_NO"]}'
                         , '{dr["ICM_CODE"]}'
                         , '{dr["EMPLOYEE_NO"]}'
                         , '{dr["C_CONDITION"]}'
                         , ({(string.IsNullOrEmpty(sDtFrom) ? "''" : $"{sDtFrom}")})
                         , ({(string.IsNullOrEmpty(sDtTo) ? "''" : $"{sDtTo}")})
                         , '{dr["SC_FORE"]}'
                         , '{dr["SC_BG"]}'
                         , '{dr["SCREEN"]}'
                         , '{dr["FEED_RATE"]}'
                         , '{dr["LOAD"]}'
                         , '{dr["GR_WV"]}'
                         , '{dr["GR_QTY"]}'
                         , '{dr["DUST_WV1"]}'
                         , '{dr["DUST_WV2"]}'
                         , '{dr["TH_GR_QTY"]}'
                         , '{dr["LOCATION_ST1"]}'
                         , '{dr["LOCATION_ST2"]}'
                         , '{dr["LOCATION_ED1"]}'
                         , '{dr["LOCATION_ED2"]}'
                         , '{dr["PRO_QTY"]}'
                         , SYSDATE
                         , '{dr["REMARK"]}'
                         , '{dr["DEL_FLAG"]}'
                         , 'N'
                         , '{dr["ERP_TNUMBER"]}'
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            viewSmash.HideLoadingPanel();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        INSERT INTO SMASH_REPORT_ADD  -- TB01 분쇄기 보고서 추가 테이블
                        (
                              PLANT_CODE
                            , PROCESS_KEY
                            , L_CODE
                            , WORK_NUMBER
                            , WORK_SEQ
                        )
                        VALUES (
                            '{dr["PLANT_CODE"]}'
                            , '{dr["PROCESS_KEY"]}'
                            , '{dr["L_CODE"]}'
                            , '{dr["WORK_NUMBER"]}'
                            , '{dr["WORK_SEQ"]}'
                        )       
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            viewSmash.HideLoadingPanel();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        -- 001
                        UPDATE SMASH_REPORT
                        SET    ICM_CODE        = '{dr["ICM_CODE"]}'   
                                , EMPLOYEE_NO  = '{dr["EMPLOYEE_NO"]}'
                                , C_CONDITION  = '{dr["C_CONDITION"]}'
                                , RUN_ST       = ({(string.IsNullOrEmpty(sDtFrom) ? "''" : $"{sDtFrom}")})
                                , RUN_ET       = ({(string.IsNullOrEmpty(sDtTo) ? "''" : $"{sDtTo}")})
                                , SC_FORE      = '{dr["SC_FORE"]}'      
                                , SC_BG        = '{dr["SC_BG"]}'        
                                , SCREEN       = '{dr["SCREEN"]}'       
                                , FEED_RATE    = '{dr["FEED_RATE"]}'    
                                , LOAD         = '{dr["LOAD"]}'         
                                , GR_WV        = '{dr["GR_WV"]}'        
                                , GR_QTY       = '{dr["GR_QTY"]}'       
                                , DUST_WV1     = '{dr["DUST_WV1"]}'     
                                , DUST_WV2     = '{dr["DUST_WV2"]}'     
                                , TH_GR_QTY    = '{dr["TH_GR_QTY"]}'    
                                , LOCATION_ST1 = '{dr["LOCATION_ST1"]}' 
                                , LOCATION_ST2 = '{dr["LOCATION_ST2"]}' 
                                , LOCATION_ED1 = '{dr["LOCATION_ED1"]}' 
                                , LOCATION_ED2 = '{dr["LOCATION_ED2"]}' 
                                , PRO_QTY      = '{dr["PRO_QTY"]}'      
                                , I_TIME       = SYSDATE                
                                , REMARK       = '{dr["REMARK"]}'       
                                , DEL_FLAG     = '{dr["DEL_FLAG"]}'     
                                , ERP_UP_YN    = 'N'    -- 024
                        WHERE  PLANT_CODE   = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY  = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE       = '{dr["L_CODE"]}'
                        AND    WORK_NUMBER  = '{dr["WORK_NUMBER"]}'
                        AND    WORK_SEQ     = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            viewSmash.HideLoadingPanel();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        //SQL = $@"
                        //UPDATE SMASH_REPORT_ADD  -- TB01 분쇄기 보고서 추가 테이블
                        //SET    B_W             = '{dr["B_W"]}'            -- 1 배치중량
                        //        , END_TIME        = '{dr["END_TIME"]}'       -- 2 종료시간
                        //        , FEEDER_NAME     = '{dr["FEEDER_NAME"]}'    -- 3 공급 Feeder
                        //        , GRINDER         = '{dr["GRINDER"]}'        -- 4 분쇄기명
                        //        , GRINDER_SPEED   = '{dr["GRINDER_SPEED"]}'  -- 5 분쇄기속도
                        //        , L_CODE          = '{dr["L_CODE"]}'         -- 6 라인코드
                        //        , MESH_10         = '{dr["MESH_10"]}'        -- 7 입자도 #10
                        //        , MESH_18         = '{dr["MESH_18"]}'        -- 8 입자도 #18
                        //        , MESH_35         = '{dr["MESH_35"]}'        -- 9 입자도 #35
                        //        , MESH_6          = '{dr["MESH_6"]}'         -- 10 입자도 #6
                        //        , MESH_8          = '{dr["MESH_8"]}'         -- 11 입자도 #8
                        //        , MESH_PAN        = '{dr["MESH_PAN"]}'       -- 12 PAN
                        //        , ORIGIN          = '{dr["ORIGIN"]}'         -- 13 원산지
                        //        , PROD_DATE       = '{dr["PROD_DATE"]}'      -- 14 생산일
                        //        , PROD_QTY        = '{dr["PROD_QTY"]}'       -- 15 생산량
                        //        , PRODUCTIVITY    = '{dr["PRODUCTIVITY"]}'   -- 16 생산성
                        //        , REMARK          = '{dr["REMARK"]}'         -- 17 비고
                        //        , RESOURCE_NO     = '{dr["RESOURCE_NO"]}'    -- 18 자원번호
                        //        , RUN_TIME        = '{dr["RUN_TIME"]}'       -- 19 가동시간
                        //        , START_TIME      = '{dr["START_TIME"]}'     -- 20 시작시간
                        //        , STOP_TIME       = '{dr["STOP_TIME"]}'      -- 21 중단시간
                        //        , I_TIME          = SYSDATE                  -- 22 입력시간
                        //WHERE  PLANT_CODE    = '{dr["PLANT_CODE"]}'
                        //AND    PROCESS_KEY   = '{dr["PROCESS_KEY"]}'
                        //AND    L_CODE        = '{dr["L_CODE"]}'
                        //AND    WORK_NUMBER   = '{dr["WORK_NUMBER"]}'
                        //AND    WORK_SEQ      = '{dr["WORK_SEQ"]}'
                        //";

                        //if (Dbconn.conn.SQLrun(SQL) < 1)
                        //{
                        //    Dbconn.conn.Rollback();
                        //    viewSmash.HideLoadingPanel();
                        //    clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        //    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                        //    return;
                        //}
                    }

                    dr.AcceptChanges();
                } //foreach

                ShowMessageBox.XtraShowInformation("저장 되었습니다.");

                viewSmash.RefreshData();

                XMain_Search();

            }
            catch (Exception ex)
            {
                viewSmash.HideLoadingPanel();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void repItemLkUpEdit_t1_RESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            viewSmash.SetRowCellValue(viewSmash.FocusedRowHandle, "RESOURCE_CD", textEditor.EditValue);
        }

        private void viewSmash_work_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (viewSmash.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 작업지시를 선택하여 주세요");
                    return;
                }

                DataRow row = viewSmash.GetDataRow(viewSmash.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewSmash.DeleteRow(viewSmash.FocusedRowHandle);
                }
                else
                {
                    //string condition = viewSmash.GetRowCellDisplayText(viewSmash.FocusedRowHandle, viewSmash.Columns["ERP_UP_YN"]);

                    /*                    if (!condition.Equals("계획"))
                                        {
                                            ShowMessageBox.XtraShowInformation("계획중인 작업지시만 삭제하실 수 있습니다");
                                            return;
                                        }
                    */

                    string condition = viewSmash.GetRowCellDisplayText(viewSmash.FocusedRowHandle, viewSmash.Columns["C_CONDITION"]);

                    if (condition.Equals("완료"))
                    {
                        ShowMessageBox.XtraShowInformation("완료된 작업지시는 삭제하실 수 없습니다");
                        return;
                    }


                    string sWORKDATE = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "WORK_NUMBER");
                    string sWORK_SEQ = clsDevexpressGrid.GetFocusedRowCellValue(viewSmash, "WORK_SEQ");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업순번 " + sWORK_SEQ + " 작업지시를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        Dbconn.conn.BeginTransaction();
                        //delete work num
                        SQL = $@"
                        UPDATE SMASH_REPORT SET DEL_FLAG = 'Y'
                        WHERE PLANT_CODE = '{clsCommon.PlantCode}'
                            AND PROCESS_KEY = '{viewSmash.GetFocusedRowCellValue("PROCESS_KEY")}'
                            AND L_CODE = '{viewSmash.GetFocusedRowCellValue("L_CODE")}'
                            AND WORK_NUMBER = '{viewSmash.GetFocusedRowCellValue("WORK_NUMBER")}'
                            AND WORK_SEQ = '{viewSmash.GetFocusedRowCellValue("WORK_SEQ")}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();

                        XMain_Search();

                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
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
            gridSmash.Focus();
            viewSmash.FocusedRowHandle = 0;
            viewSmash.FocusedColumn = viewSmash.VisibleColumns[0];
        }

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewSmash.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("전송 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 ERP로 전송 하시겠습니까?", "ERP의 기존 작업 내역은 삭제 후 현 작업 데이터를 재전송 합니다."))
            {
                return;
            }

            try
            {
                int[] selectedRows = viewSmash.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewSmash.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM SMASH_REPORT
                    WHERE  PLANT_CODE  = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE      = '{dr["L_CODE"]}'
                        AND    WORKDATE    = '{dr["WORKDATE"]}'
                        AND    WORK_SEQ    = '{dr["WORK_SEQ"]}'
                        AND ERP_UP_YN IN ('N', 'M', 'X', 'G')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE SMASH_REPORT
                        SET    ERP_UP_YN = CASE WHEN ERP_UP_YN = 'Y' THEN 'M' ELSE 'F' END
                            , ERP_ERR_CNT = 0
                        WHERE  PLANT_CODE  = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE      = '{dr["L_CODE"]}'
                        AND    WORKDATE    = '{dr["WORKDATE"]}'
                        AND    WORK_SEQ    = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_plcUpload_Click", ex.Message + "/" + ex.StackTrace);
            }
            finally
            {
            }

            ShowMessageBox.XtraShowInformation("선택된 정보가 전송 대기로 변경 되었습니다");
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }

        private void cboERPUpLoad_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboL_Code_EditValueChanged_1(object sender, EventArgs e)
        {
            gridSmash.DataSource = null;

            XMain_Search();
        }

        private void btnERPDelete_Click(object sender, EventArgs e)
        {
            //ShowMessageBox.XtraShowInformation("기능 추가 중입니다.");
            //return;
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewSmash.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("전송 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 ERP로 전송 하시겠습니까?", "ERP의 기존 작업 내역은 삭제 후 현 작업 데이터를 재전송 합니다."))
            {
                return;
            }

            try
            {
                int[] selectedRows = viewSmash.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                string pellet = string.Empty;

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewSmash.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM SMASH_REPORT
                     WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                        AND (ERP_OSTATUS IN ('Y')
                        OR ERP_ISTATUS IN ('Y'))
                    ";

                    DataSet dsPel = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(dsPel) > 0)
                    {
                        SQL = $@"
                        UPDATE SMASH_REPORT
                        SET   ERP_OSTATUS = CASE TO_CHAR(ERP_UP_YN) 
                                                    WHEN 'Y' THEN 'D'
                                                ELSE TO_CHAR(ERP_UP_YN) END
                            , ERP_OERR_CNT = 0
                            , ERP_ISTATUS = CASE TO_CHAR(ERP_ISTATUS) 
                                                WHEN 'Y' THEN 'D'
                                            ELSE TO_CHAR(ERP_ISTATUS) END
                            , ERP_IERR_CNT = 0
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.XtraShowInformation("전송이 완료된 작업만 취소할 수 있습니다.");
                        Dbconn.conn.Rollback();
                        return;
                    }
                }

                Dbconn.conn.Commit();

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_plcUpload_Click", ex.Message + "/" + ex.StackTrace);
            }

            ShowMessageBox.XtraShowInformation("선택된 정보가 삭제 전송 대기로 변경 되었습니다");
        }

        private void viewSmash_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            XSub_Search();
        }

        private void btn_RepoSave_Click(object sender, EventArgs e)
        {
            XSub_Save();
        }

        private void XSub_Save()
        {
            try
            {
                int iRowIdx = 0;
                string lCode = string.Empty;
                clsDevexpressGrid.GridEndEdit(viewReport);
                DataTable DT = (DataTable)gridReport.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (DataRow dr in DT.Rows)
                {
                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Modified)
                    {
                        string SQL = $@"
                        UPDATE SMASH_REPORT_ADD
                           SET PROD_DATE     = '{dr["PROD_DATE"]}'
                             , START_TIME    = '{dr["START_TIME"]}'
                             , END_TIME      = '{dr["END_TIME"]}'
                             , RUN_TIME      = '{dr["RUN_TIME"]}'
                             , GRINDER       = '{dr["GRINDER"]}'
                             , RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                             , ORIGIN        = '{dr["ORIGIN"]}'
                             , GRINDER_SPEED = '{dr["GRINDER_SPEED"]}'
                             , FEEDER_NAME   = '{dr["FEEDER_NAME"]}'
                             , PROD_QTY      = '{dr["PROD_QTY"]}'
                             , PRODUCTIVITY  = '{dr["PRODUCTIVITY"]}'
                             , B_W           = '{dr["B_W"]}'
                             , MESH_6        = '{dr["MESH_6"]}'
                             , MESH_8        = '{dr["MESH_8"]}'
                             , MESH_10       = '{dr["MESH_10"]}'
                             , MESH_18       = '{dr["MESH_18"]}'
                             , MESH_35       = '{dr["MESH_35"]}'
                             , MESH_PAN      = '{dr["MESH_PAN"]}'
                             , REMARK        = '{dr["REMARK"]}'
                             , STOP_TIME     = '{dr["STOP_TIME"]}'
                         WHERE PLANT_CODE   = '{dr["PLANT_CODE"]}'
                           AND PROCESS_KEY  = '{dr["PROCESS_KEY"]}'
                           AND L_CODE       = '{dr["L_CODE"]}'
                           AND WORK_NUMBER  = '{dr["WORK_NUMBER"]}'
                           AND WORK_SEQ     = '{dr["WORK_SEQ"]}'
                        "; 

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                ShowMessageBox.XtraShowInformation("작업일지가 저장 되었습니다.");

                Dbconn.conn.Commit();

                viewSmash.RefreshData();
                XMain_Search();

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }
    }
}