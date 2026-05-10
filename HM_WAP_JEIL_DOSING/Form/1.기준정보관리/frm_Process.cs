using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Process : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string sProcessKey = String.Empty;
        private string sPlantCode = string.Empty;
        DataSet authDs;
        private string[] sMValid = null;
        private string[] sDValid = null;


        public frm_Process()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewProcess, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewLine, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewPLC, Properties.Settings.Default.FontSize);

        }

        private void frm_Process_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboProcessKey, clsCommon.GetGridProcess(viewProcess.GetFocusedRowCellValue("PLANT_CODE")?.ToString()));

            gridChk.ValueChecked = "Y";
            gridChk.ValueUnchecked = "N";
            gridChk.NullStyle = StyleIndeterminate.Unchecked;
            gridChk.CheckStyle = CheckStyles.Standard;

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridProcess));
        }

        #region 조회 쿼리
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 공정 마스터
                SELECT 
                   PLANT_CODE, PROCESS_KEY, PROCESS_DESC, 
                   JB_STIME, JB_ETIME, TRANS_GUBUN,
                   CHANGED_BY, DATE_CHANGED, PROCESS_TYPE
                FROM SAP_PROCESS_DIVISION
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}')
                 AND PROCESS_DESC LIKE '%{txtProcess.EditValue}%'
                ORDER BY PLANT_CODE, PROCESS_KEY
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridProcess, viewProcess, ds.Tables[0], false, true);

                sMValid = new string[] { "PLANT_CODE", "PROCESS_KEY" };


                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboPlant, clsCommon.GetPlant("", true));

                // 공정 타입
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPROCESS_TYPE, clsCommon.GetProcessType());

                ds.Dispose();

                XDetail_Search();
                XPLC_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회(XMain_Search())를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XDetail_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                PLANT_CODE, PROCESS_KEY, L_CODE , 
                   PROCESS_DESC, SBNO, CHANGED_BY, 
                   DATE_CHANGED
                FROM SAP_PROCESS_LDIVISION
                WHERE PLANT_CODE = '{viewProcess.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewProcess.GetFocusedRowCellValue("PROCESS_KEY")}'
                ORDER BY PLANT_CODE, PROCESS_KEY, L_CODE
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridLine, viewLine, ds.Tables[0], false, true);

                sDValid = new string[] { "PLANT_CODE", "PROCESS_KEY", "L_CODE" };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridlscboPlant_Code, clsCommon.GetPlant("", true));
                clsDevexpressGrid.ItemLookUpEditSetup(gridlcboPROCESS_KEY, clsCommon.GetGridProcess());
                clsDevexpressGrid.ItemLookUpEditSetup(gridlcboSBNO, clsCommon.GetSBNo());

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회(XDetail_Search())를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XPLC_Search()
        {
            try
            {
                SQL = $@"
                SELECT PLANT_CODE, PROCESS_KEY, USE_PROCESS_KEY, PLC_NO, PLC_TYPE, 
                   IP, N_NO, PORT, T_OUT, CHANGED_BY, DATE_CHANGED
                FROM SAP_PROCESS_PLC
                WHERE PLANT_CODE = '{viewProcess.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewProcess.GetFocusedRowCellValue("PROCESS_KEY")}'
                ORDER BY PLANT_CODE, PROCESS_KEY, TO_NUMBER(PLC_NO)
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridPLC, viewPLC, ds.Tables[0], false, true);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPlcType, clsCommon.GetPlcType(), "", false, false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회(XDetail_Search())를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 공정 정보 버튼 이벤트

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                clsDevexpressGrid.GridViewLastAddRowDelete(viewProcess);
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewProcess);
            viewProcess.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewProcess);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewProcess);
        }

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
                clsDevexpressGrid.GridEndEdit(viewProcess);

                DataTable DT = (DataTable)gridProcess.DataSource;

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
                    string rValid = clsCommon.ValdationCheck(sMValid, dr, viewProcess);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewProcess.FocusedColumn = viewProcess.Columns[rValid]; // 이동할 컬럼명
                        viewProcess.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_PROCESS_DIVISION (
                           PLANT_CODE, PROCESS_KEY, PROCESS_DESC, 
                           JB_STIME, JB_ETIME, TRANS_GUBUN, CHANGED_BY, 
                           DATE_CHANGED, PROCESS_TYPE) 
                        VALUES ( 
                            '{dr["PLANT_CODE"]}'
                          , '{dr["PROCESS_KEY"]}'
                          , '{dr["PROCESS_DESC"]}'
                          , '{dr["JB_STIME"]}'
                          , '{dr["JB_ETIME"]}'
                          , '{dr["TRANS_GUBUN"]}'
                          , '{clsCommon.UserId}'
                          , SYSDATE
                          , '{dr["PROCESS_TYPE"]}' )
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
                        UPDATE SAP_PROCESS_DIVISION
                        SET    PROCESS_DESC = '{dr["PROCESS_DESC"]}',
                               JB_STIME     = '{dr["JB_STIME"]}',
                               JB_ETIME     = '{dr["JB_ETIME"]}',
                               TRANS_GUBUN  = '{dr["TRANS_GUBUN"]}',
                               CHANGED_BY   = '{clsCommon.UserId}',
                               DATE_CHANGED = SYSDATE,
                               PROCESS_TYPE = '{dr["PROCESS_TYPE"]}'
                        WHERE  PLANT_CODE = '{dr["PLANT_CODE"]}'
                            AND PROCESS_KEY  = '{dr["PROCESS_KEY"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }
                }
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

            ShowMessageBox.XtraShowInformation("공정 정보를 저장 했습니다.");

            XMain_Search();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewProcess.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewProcess);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 공정데이터 " + viewProcess.GetFocusedRowCellValue("PROCESS_DESC") + "을 삭제하시겠습니까?"))
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

                SQL = $"DELETE FROM SAP_PROCESS_DIVISION WHERE PLANT_CODE = '{viewProcess.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewProcess.GetFocusedRowCellValue("PROCESS_KEY")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                SQL = $"DELETE FROM SAP_PROCESS_LDIVISION WHERE PLANT_CODE = '{viewProcess.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewProcess.GetFocusedRowCellValue("PROCESS_KEY")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                SQL = $"DELETE FROM SAP_PROCESS_PLC WHERE PLANT_CODE = '{viewLine.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewLine.GetFocusedRowCellValue("PROCESS_KEY")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowInformation("삭제 되었습니다.");

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
        #endregion

        #region 라인 정보 이벤트
        private void btn_l_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewLine);

            viewLine.SetFocusedRowCellValue("PLANT_CODE", viewProcess.GetFocusedRowCellValue("PLANT_CODE"));
            viewLine.SetFocusedRowCellValue("PROCESS_KEY", viewProcess.GetFocusedRowCellValue("PROCESS_KEY"));
        }

        private void btn_l_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewLine);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewLine);
        }

        private void btn_l_save_Click(object sender, EventArgs e)
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

            XDetail_Save();
        }

        private void XDetail_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewLine);

                DataTable DT = (DataTable)gridLine.DataSource;

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
                    string rValid = clsCommon.ValdationCheck(sDValid, dr, viewLine);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewLine.FocusedColumn = viewLine.Columns[rValid]; // 이동할 컬럼명
                        viewLine.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_PROCESS_LDIVISION (
                             PLANT_CODE
                           , PROCESS_KEY
                           , L_CODE
                           , PROCESS_DESC
                           , CHANGED_BY
                           , DATE_CHANGED
                        )
                        VALUES (
                             '{dr["PLANT_CODE"]}'
                           , '{dr["PROCESS_KEY"]}'
                           , '{dr["L_CODE"]}'
                           , '{dr["PROCESS_DESC"]}'
                           , '{clsCommon.UserId}'
                           , SYSDATE
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
                        UPDATE SAP_PROCESS_LDIVISION
                        SET    PROCESS_DESC = '{dr["PROCESS_DESC"]}'
                             , CHANGED_BY   = '{clsCommon.UserId}'
                             , DATE_CHANGED = SYSDATE
                        WHERE  PLANT_CODE   = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY  = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE       = '{dr["L_CODE"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }
                }

                ShowMessageBox.XtraShowInformation("공정 정보를 저장 했습니다.");

                XDetail_Search();
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

        private void btn_l_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewProcess.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewProcess);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 공정을 삭제하시겠습니까?"))
            {
                return;
            }

            XDetail_Delete();
        }

        private void XDetail_Delete()
        {
            try
            {
                splashScreenManager.ShowWaitForm();

                SQL = $"DELETE FROM SAP_PROCESS_LDIVISION WHERE PLANT_CODE = '{viewLine.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewLine.GetFocusedRowCellValue("PROCESS_KEY")}' AND L_CODE = '{viewLine.GetFocusedRowCellValue("L_CODE")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                XDetail_Search();
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

        #region PLC 정보 이벤트
        private void btn_p_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewPLC);

            viewPLC.SetFocusedRowCellValue("PLANT_CODE", viewProcess.GetFocusedRowCellValue("PLANT_CODE"));
            viewPLC.SetFocusedRowCellValue("PROCESS_KEY", viewProcess.GetFocusedRowCellValue("PROCESS_KEY"));
        }

        private void btn_p_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPLC);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewPLC);
        }

        private void btn_p_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(viewPLC);

                DataTable DT = (DataTable)gridPLC.DataSource;

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
                        string SQL = $@"
                        INSERT INTO SAP_PROCESS_PLC (
                             PLANT_CODE
                           , PROCESS_KEY
                           , USE_PROCESS_KEY
                           , PLC_NO
                           , PLC_TYPE
                           , IP
                           , N_NO
                           , PORT
                           , T_OUT
                           , CHANGED_BY
                           , DATE_CHANGED
                        ) VALUES (
                             '{dr["PLANT_CODE"]}'
                           , '{dr["PROCESS_KEY"]}'
                           , '{dr["USE_PROCESS_KEY"]}'
                           , '{dr["PLC_NO"]}'
                           , '{dr["PLC_TYPE"]}'
                           , '{dr["IP"]}'
                           , '{dr["N_NO"]}'
                           , '{dr["PORT"]}'
                           , '{dr["T_OUT"]}'
                           , '{clsCommon.UserId}'
                           , SYSDATE
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
                        string SQL = $@"
                        UPDATE SAP_PROCESS_PLC
                           SET USE_PROCESS_KEY  = '{dr["USE_PROCESS_KEY"]}' 
                             ,PLC_TYPE          = '{dr["PLC_TYPE"]}'
                             , IP               = '{dr["IP"]}'
                             , N_NO             = '{dr["N_NO"]}'
                             , PORT             = '{dr["PORT"]}'
                             , T_OUT            = '{dr["T_OUT"]}'
                             , CHANGED_BY       = '{clsCommon.UserId}'
                             , DATE_CHANGED     = SYSDATE
                        WHERE PLANT_CODE        = '{dr["PLANT_CODE"]}'
                           AND PROCESS_KEY      = '{dr["PROCESS_KEY"]}'
                           AND PLC_NO           = '{dr["PLC_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }
                }

                ShowMessageBox.XtraShowInformation("PLC 정보를 저장 했습니다.");

                XPLC_Search();
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

        private void btn_p_delete_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();

                SQL = $"DELETE FROM SAP_PROCESS_PLC WHERE PLANT_CODE = '{viewPLC.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewPLC.GetFocusedRowCellValue("PROCESS_KEY")}' AND PLC_NO = '{viewPLC.GetFocusedRowCellValue("PLC_NO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                XPLC_Search();
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

        #region 그리드 이벤트

        private void gridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void repItemLkUpEdit_EMPLOYEE_NO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            DataRow row = viewProcess.GetDataRow(viewProcess.FocusedRowHandle);

            if (row.RowState != DataRowState.Added)
            {
                e.Cancel = true;
                viewProcess.CancelUpdateCurrentRow();
                ShowMessageBox.XtraShowInformation("입력된 사번정보는 수정하실 수 없습니다.\r\n사원정보를 삭제 후 추가해주세요");
            }

        }

        #endregion

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
            gridProcess.Focus();
            viewProcess.FocusedRowHandle = 0;
            viewProcess.FocusedColumn = viewProcess.VisibleColumns[0];
        }

        private void viewProcess_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                XDetail_Search();
                XPLC_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }
    }
}