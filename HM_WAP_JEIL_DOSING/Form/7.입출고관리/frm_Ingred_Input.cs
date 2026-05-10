using Core.Class;
using Core.Enum;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Ingred_Input : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;


        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                   'N' AS CHK, IS_NO, IV_NO, INGRED_CODE, 
                   SPEM_CAR_WEIGHT, SPTT_CAR_WEIGHT, SPWG_CAR_WEIGHT, 
                   N_WEIGHT, UNIT, SPIV_CAR_WEIGHT, 
                   EA, SPCS, LOT_NO, 
                   U_WEIGHT, PC_STATUS, USER_ID, 
                   QR_LOG, OFF_TIME, BEFORE_WEIGHT, 
                   BEFORE_WEIGHT_TIME, WEIGHT, WEIGHT_TIME, 
                   ORDER_NO, LINE_NO, CL_SER_NO, 
                   CL_LN_NO, ERP_LOCATION, ERP_WEIGHT, 
                   ERP_UP_YN, I_TIME
                FROM WAP_GOCAR
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                sValid = new string[] { "" };


                gridView.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

                // ERP 전송상태
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboTRANS_FLAG, clsCommon.GetTransFlag(), "", false, false);

                //사용여부
                repItemLkUpEdit_USEYN.NullText = "";
                repItemLkUpEdit_USEYN.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_USEYN, clsCommon.GetYn(null, new string[] {"전송", "미전송"}), "", false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource("cboPlant_Code.EditValue?.ToString()"), "", false);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion
        public frm_Ingred_Input()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Ingred_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            // ERP 진행여부
            clsDevexpressUtil.ItemLookUpEditSetup(cboERPUpLoad, clsCommon.GetTransFlag(), "", false, 0, true);

            XMain_Search();


            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void btn_reflash_Click(object sender, EventArgs e)
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("원료정보 데이터를 저장하시겠습니까?"))
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

                var topRowIndex = gridView.TopRowIndex;
                var focusedRowHandle = gridView.FocusedRowHandle;

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
                        INSERT INTO WAP_GOCAR (
                            IS_NO, IV_NO, INGRED_CODE, 
                            SPEM_CAR_WEIGHT, SPTT_CAR_WEIGHT, SPWG_CAR_WEIGHT, 
                            N_WEIGHT, UNIT, SPIV_CAR_WEIGHT, 
                            EA, SPCS, LOT_NO, 
                            U_WEIGHT, PC_STATUS, USER_ID, 
                            QR_LOG, OFF_TIME, BEFORE_WEIGHT, 
                            BEFORE_WEIGHT_TIME, WEIGHT, WEIGHT_TIME, 
                            ORDER_NO, LINE_NO, CL_SER_NO, 
                            CL_LN_NO, ERP_LOCATION, ERP_WEIGHT, 
                            ERP_UP_YN, I_TIME) 
                        VALUES ( 
                             , '{dr["IS_NO"]}'
                             , '{dr["IV_NO"]}'
                             , '{dr["INGRED_CODE"]}'
                             , '{dr["SPEM_CAR_WEIGHT"]}'
                             , '{dr["SPTT_CAR_WEIGHT"]}'
                             , '{dr["SPWG_CAR_WEIGHT"]}'
                             , '{dr["N_WEIGHT"]}'
                             , '{dr["UNIT"]}'
                             , '{dr["SPIV_CAR_WEIGHT"]}'
                             , '{dr["EA"]}'
                             , '{dr["SPCS"]}'
                             , '{dr["LOT_NO"]}'
                             , '{dr["U_WEIGHT"]}'
                             , '{dr["PC_STATUS"]}'
                             , '{dr["USER_ID"]}'
                             , '{dr["QR_LOG"]}'
                             , '{dr["OFF_TIME"]}'
                             , '{dr["BEFORE_WEIGHT"]}'
                             , '{dr["BEFORE_WEIGHT_TIME"]}'
                             , '{dr["WEIGHT"]}'
                             , '{dr["WEIGHT_TIME"]}'
                             , '{dr["ORDER_NO"]}'
                             , '{dr["LINE_NO"]}'
                             , '{dr["CL_SER_NO"]}'
                             , '{dr["CL_LN_NO"]}'
                             , '{dr["ERP_LOCATION"]}'
                             , '{dr["ERP_WEIGHT"]}'
                             , 'N'
                             , SYSDATE )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(SAP_DI_PRODUCT)데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        if (!clsCommon.Auth_Form_Function(authDs, "W"))
                        {
                            ShowMessageBox.XtraShowInformation("권한이 없습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE WAP_GOCAR
                        SET   IS_NO              = '{dr["IS_NO"]}'
                            , IV_NO              = '{dr["IV_NO"]}'
                            , INGRED_CODE        = '{dr["INGRED_CODE"]}'
                            , SPEM_CAR_WEIGHT    = '{dr["SPEM_CAR_WEIGHT"]}'
                            , SPTT_CAR_WEIGHT    = '{dr["SPTT_CAR_WEIGHT"]}'
                            , SPWG_CAR_WEIGHT    = '{dr["SPWG_CAR_WEIGHT"]}'
                            , N_WEIGHT           = '{dr["N_WEIGHT"]}'
                            , UNIT               = '{dr["UNIT"]}'
                            , SPIV_CAR_WEIGHT    = '{dr["SPIV_CAR_WEIGHT"]}'
                            , EA                 = '{dr["EA"]}'
                            , SPCS               = '{dr["SPCS"]}'
                            , LOT_NO             = '{dr["LOT_NO"]}'
                            , U_WEIGHT           = '{dr["U_WEIGHT"]}'
                            , PC_STATUS          = '{dr["PC_STATUS"]}'
                            , USER_ID            = '{dr["USER_ID"]}'
                            , QR_LOG             = '{dr["QR_LOG"]}'
                            , OFF_TIME           = '{dr["OFF_TIME"]}'
                            , BEFORE_WEIGHT      = '{dr["BEFORE_WEIGHT"]}'
                            , BEFORE_WEIGHT_TIME = '{dr["BEFORE_WEIGHT_TIME"]}'
                            , WEIGHT             = '{dr["WEIGHT"]}'
                            , WEIGHT_TIME        = '{dr["WEIGHT_TIME"]}'
                            , ORDER_NO           = '{dr["ORDER_NO"]}'
                            , LINE_NO            = '{dr["LINE_NO"]}'
                            , CL_SER_NO          = '{dr["CL_SER_NO"]}'
                            , CL_LN_NO           = '{dr["CL_LN_NO"]}'
                            , ERP_LOCATION       = '{dr["ERP_LOCATION"]}'
                            , ERP_WEIGHT         = '{dr["ERP_WEIGHT"]}'
                            , ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                            , ERP_ERR_CNT = 0
                            , I_TIME             = SYSDATE
                        WHERE IS_NO               = '{dr["IS_NO"]}'
                            AND IV_NO              = '{dr["IV_NO"]}'
                            AND INGRED_CODE        = '{dr["INGRED_CODE"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();
                    gridView.RefreshData();
                }

                XMain_Search();
                gridView.FocusedRowHandle = focusedRowHandle;
                gridView.TopRowIndex = topRowIndex;

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

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(gridView);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["USEYN"], "Y");
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    gridView.DeleteRow(gridView.FocusedRowHandle);
                }
                else
                {
                    string sRESOURCE_NO = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO");
                    string sRESOURCE_NO2 = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO_2");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 원료를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {

                        Dbconn.conn.BeginTransaction();

                        SQL = $"DELETE FROM SAP_IN_PRODUCT_SR_CON WHERE RESOURCE_NO = '{sRESOURCE_NO}' AND RESOURCE_NO_2 = '{sRESOURCE_NO2}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("원료 삭제에 실패했습니다");
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
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.RowCount == 0)
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
                clsDevexpressGrid.GridEndEdit(gridView);
                DataTable DT = (DataTable)gridControl.DataSource;

                DataTable copyTable = DT.Clone();

                foreach (DataRow row in DT.Select("CHK = 'Y'"))
                {
                    copyTable.ImportRow(row);
                }

                if (copyTable.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowWarning("복사 할 자재를 먼저 선택 해주세요.");
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (DataRow dr in copyTable.Rows)
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

                    SQL = $@"
                    SELECT 1
                    FROM WAP_GOCAR
                    WHERE IS_NO               = '{dr["IS_NO"]}'
                        AND IV_NO              = '{dr["IV_NO"]}'
                        AND INGRED_CODE        = '{dr["INGRED_CODE"]}'
                        AND ERP_UP_YN IN ('N', 'M', 'X', 'G')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                    UPDATE WAP_GOCAR
                    SET   ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) WHEN 'N' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                     ELSE TO_CHAR(ERP_UP_YN) END
                        , ERP_ERR_CNT = 0
                    WHERE IS_NO               = '{dr["IS_NO"]}'
                        AND IV_NO              = '{dr["IV_NO"]}'
                        AND INGRED_CODE        = '{dr["INGRED_CODE"]}'
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

        private void dtFrom_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dtTo_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboERPUpLoad_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}