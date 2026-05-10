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
    public partial class frm_RestTime : DevExpress.XtraEditors.XtraForm
    {
        string SQL = string.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_RestTime()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                if (string.Format("{0:yyyyMMdd}", dateEdit_workdate.EditValue) == "")
                {
                    return;
                }

                SQL = $@"
                SELECT 
                     SEQ
                    , PLANT_CODE
                    , PROCESS_KEY
                    , L_CODE
                    , WORKDATE
                    , REST_CODE
                    , START_TIME
                    , END_TIME
                    , REST_MINUTES
                    , REMARK
                    , I_TIME
                    , U_TIME
                    , I_USER
                FROM REST_TIME
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND ('{cboProcess.EditValue}' IS NULL OR PROCESS_KEY = '{cboProcess.EditValue}')
                    AND ('{cboL_Code.EditValue}' IS NULL OR L_CODE = '{cboL_Code.EditValue}')
                    AND WORKDATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workdate.EditValue)}' 
                ORDER BY SEQ 
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false);

                sValid = new string[] { "" };


                //비가동사유
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_REST_CODE, clsCommon.GetRestTime());

                //작업자정보
                repItemLkUpEdit_EMPLOYEE_NO.NullValuePrompt = "";
                repItemLkUpEdit_EMPLOYEE_NO.NullText = "";

                ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_EMPLOYEE_NO, clsCommon.GetDO_INSA(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "exp_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_RestTime_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            dateEdit_workdate.EditValue = DateTime.Today;

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, false);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            if (clsCommon._strMainPlcConnYn == "Y" && clsCommon._strSubPlcConnYn == "Y")
            {
                layoutControlItem_workDateEd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else
            {

                layoutControlItem_workDateEd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                dateEdit_workDateEd.EditValue = DateTime.Today.AddDays(1);
            }

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                return;
            }

            if (cboProcess.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("공정을 먼저 조회 해주세요.");
                cboProcess.Focus();
                return;
            }

            if (cboL_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("라인을 먼저 조회 해주세요.");
                cboL_Code.Focus();
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);

            gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
            gridView.SetFocusedRowCellValue("PROCESS_KEY", cboProcess.EditValue);
            gridView.SetFocusedRowCellValue("L_CODE", cboL_Code.EditValue);
            gridView.SetFocusedRowCellValue("I_USER", clsCommon.UserId);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["WORKDATE"], string.Format("{0:yyyyMMdd}", dateEdit_workdate.EditValue));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["START_TIME"], string.Format("{0:yyyy-MM-dd}", dateEdit_workdate.EditValue) + " " + DateTime.Now.ToString("HH:mm:ss"));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["END_TIME"], string.Format("{0:yyyy-MM-dd}", dateEdit_workdate.EditValue) + " " + DateTime.Now.ToString("HH:mm:ss"));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["REST_MINUTES"], 0);
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("휴게시간 데이터를 저장하시겠습니까?"))
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
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO REST_TIME (
                             PLANT_CODE     -- 2
                            , PROCESS_KEY    -- 3
                            , L_CODE         -- 4
                            , WORKDATE       -- 5
                            , SEQ
                            , REST_CODE      -- 6
                            , START_TIME     -- 7
                            , END_TIME       -- 8
                            , REST_MINUTES   -- 9
                            , REMARK         -- 10
                            , I_TIME         -- 11
                            , U_TIME         -- 12
                            , I_USER         -- 13
                        )
                        VALUES (
                             '{dr["PLANT_CODE"]}'     -- 2
                            , '{dr["PROCESS_KEY"]}'    -- 3
                            , '{dr["L_CODE"]}'         -- 4
                            , '{dr["WORKDATE"]}'       -- 5
                            , (SELECT NVL(MAX(SEQ) + 1, 1) FROM REST_TIME WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}')      -- 6
                            , '{dr["REST_CODE"]}'
                            , TO_DATE('{Convert.ToDateTime(dr["START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')     -- 7
                            , TO_DATE('{Convert.ToDateTime(dr["END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')       -- 8
                            , '{dr["REST_MINUTES"]}'   -- 9
                            , '{dr["REMARK"]}'         -- 10
                            , SYSDATE                  -- 11
                            , '{dr["U_TIME"]}'         -- 12
                            , '{dr["I_USER"]}'         -- 13
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                         {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        TimeSpan dateDiff = Convert.ToDateTime(dr["END_TIME"]) - Convert.ToDateTime(dr["START_TIME"]);

                        SQL = $@"
                        UPDATE REST_TIME
                        SET REST_CODE    = '{dr["REST_CODE"]}'      -- 5
                            , START_TIME   = TO_DATE('{Convert.ToDateTime(dr["START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')     -- 6
                            , END_TIME     = TO_DATE('{Convert.ToDateTime(dr["END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')       -- 7
                            , REST_MINUTES = '{dr["REST_MINUTES"]}'   -- 8
                            , REMARK       = '{dr["REMARK"]}'         -- 9
                            , U_TIME       = SYSDATE                  -- 11
                            , I_USER       = '{dr["I_USER"]}'         -- 12
                        WHERE 
                            SEQ = '{dr["SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {

                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                Dbconn.conn.Commit();

                gridView.RefreshData();

                ShowMessageBox.XtraShowInformation("휴계시간 정보를 저장 했습니다.");

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workdate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void LookupEdit_pcSel_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
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
                    ShowMessageBox.XtraShowInformation("삭제하실 휴게정보를 선택하여 주세요");
                    return;
                }

                DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    gridView.DeleteRow(gridView.FocusedRowHandle);
                }
                else
                {
                    string seq = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "SEQ");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 휴게정보를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {

                        SQL = "DELETE FROM REST_TIME WHERE SEQ = '{0}' ";
                        SQL = string.Format(SQL, seq);

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning(" 휴게정보 삭제에 실패했습니다");
                            return;
                        }

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
            gridControl.DataSource = null;

            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcess, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, false);

            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess.EditValue?.ToString()), "", false, 0, true);
        }

        private void cboProcess_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess.EditValue?.ToString()), "", false, 0, true);
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            XMain_Search();
        }

        private void dateEdit_workDateEd_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridView.UpdateCurrentRow();

            if (e.Column.FieldName == "START_TIME" || e.Column.FieldName == "END_TIME")  // 수량이 변경된 경우
            {
                if (gridView.GetFocusedRowCellValue("START_TIME")?.ToString() == "")
                    return;

                if (gridView.GetFocusedRowCellValue("END_TIME")?.ToString() == "")
                    return;

                string sSTART_TIME = gridView.GetFocusedRowCellValue("START_TIME")?.ToString();
                string sEND_TIME = gridView.GetFocusedRowCellValue("END_TIME")?.ToString();

                TimeSpan dateDiff = Convert.ToDateTime(sEND_TIME) - Convert.ToDateTime(sSTART_TIME);

                gridView.SetFocusedRowCellValue("REST_MINUTES", dateDiff.TotalMinutes.ToString());
            }
        }
    }
}