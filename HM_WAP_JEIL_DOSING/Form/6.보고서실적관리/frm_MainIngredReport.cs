using Core;
using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;
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
    public partial class frm_MainIngredReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_MainIngredReport()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                string SQL = $@"
                SELECT 
                    WORK_NUMBER,
                    WORK_SEQ,
                    RESOURCE_NO,
                    LOCATION,
                    INPUT_QTY,
                    TO_CHAR(START_TIME, 'YYYY-MM-DD HH24:MI:SS') AS START_TIME,
                    TO_CHAR(END_TIME, 'YYYY-MM-DD HH24:MI:SS') AS END_TIME,
                    INCAR_NO,
                    DRIVER_NAME,
                    WORK_NAME,
                    REMARK,
                    TO_CHAR(I_TIME, 'YYYY-MM-DD HH24:MI:SS') AS I_TIME,
                    I_USER
                FROM MAINMATERIALS_REPORT
                WHERE WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                ORDER BY WORK_NUMBER, WORK_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false);

                sValid = new string[] { "" };


                repositoryItemLookUpEdit_RESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                repositoryItemLookUpEdit_RESOURCE_NO.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch;
                clsDevexpressGrid.ItemLookUpEditSetup(repositoryItemLookUpEdit_RESOURCE_NO, clsCommon.GetResource("", "", $"'{clsCommon.GetResourceTypeCode("반제품")}', '{clsCommon.GetResourceTypeCode("원재료")}'"), "하차원료를 선택하여 주세요");
                repositoryItemLookUpEdit_RESOURCE_NO.PopupFormMinSize = new Size(500, 600);


                SQL = $"SELECT CHK_REMARK FROM MAINMATERIALS_CHK_LIST WHERE WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                memoEdit_Remark.Text = string.Empty;
                if (Dbconn.conn.getRowCnt(chkListDs) > 0)
                {
                    memoEdit_Remark.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK", 0);
                }

                chkListDs.Dispose();
                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_MainIngredReport_Load(object sender, EventArgs e)
        {
            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_MicroIngredReport_Load", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string workDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                clsPrintReport.PrintMainIngredReport(workDate);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_report_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridViewAddRow(gridView);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["START_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["END_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["INPUT_QTY"], "0");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowAdd_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);
                clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    e.Info.DisplayText = (1 + e.RowHandle).ToString();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_CustomDrawRowIndicator", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("주원료 하차일지 정보를 저장하시겠습니까?"))
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

                    //input check
                    if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("RESOURCE_NO", "하차원료정보를 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["LOCATION"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("LOCATION", "투입빈 정보를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["START_TIME"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("START_TIME", "시작시간 정보를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("START_TIME"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["END_TIME"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("END_TIME", "종료시간 정보를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("END_TIME"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["INPUT_QTY"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("INPUT_QTY", "하차양을 입력해주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("INPUT_QTY"));
                        return;
                    }


                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO MAINMATERIALS_REPORT (
                           WORK_NUMBER, WORK_SEQ, RESOURCE_NO, 
                           LOCATION, INPUT_QTY, START_TIME, 
                           END_TIME, INCAR_NO, DRIVER_NAME, 
                           WORK_NAME, REMARK, I_TIME, 
                           I_USER) 
                        VALUES ( 
                        '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                        , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) FROM MAINMATERIALS_REPORT WHERE WORK_NUMBER = '{dr["WORK_NUMBER"]}')
                        , '{dr["RESOURCE_NO"]}'
                        , '{dr["LOCATION"]}'
                        , '{dr["INPUT_QTY"]}'
                        , TO_DATE('{Convert.ToDateTime(dr["START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                        , TO_DATE('{Convert.ToDateTime(dr["END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                        , '{dr["INCAR_NO"]}'
                        , '{dr["DRIVER_NAME"]}'
                        , '{dr["WORK_NAME"]}'
                        , '{dr["REMARK"]}'
                        , SYSDATE
                        , '{clsCommon._strUserId}' )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE MAINMATERIALS_REPORT
                        SET    RESOURCE_NO = '{dr["RESOURCE_NO"]}',
                               LOCATION    = '{dr["LOCATION"]}',
                               INPUT_QTY   = '{dr["INPUT_QTY"]}',
                               START_TIME  = TO_DATE('{Convert.ToDateTime(dr["START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                               END_TIME    = TO_DATE('{Convert.ToDateTime(dr["END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                               INCAR_NO    = '{dr["INCAR_NO"]}',
                               DRIVER_NAME = '{dr["DRIVER_NAME"]}',
                               WORK_NAME   = '{dr["WORK_NAME"]}',
                               REMARK      = '{dr["REMARK"]}',
                               I_TIME      = SYSDATE,
                               I_USER      = '{clsCommon._strUserId}'
                        WHERE  WORK_NUMBER = '{dr["WORK_NUMBER"]}'
                        AND    WORK_SEQ    = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                SQL = $"DELETE FROM MAINMATERIALS_CHK_LIST WHERE WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("MAINMATERIALS_CHK_LIST 삭제에 실패했습니다");
                    return;
                }

                SQL = $@"
                INSERT INTO MAINMATERIALS_CHK_LIST
                (WORK_NUMBER, CHK_REMARK, I_TIME, I_USER)
                VALUES ('{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', '{memoEdit_Remark.Text}', SYSDATE, '{clsCommon._strUserId}')
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("MAINMATERIALS_CHK_LIST 저장에 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowInformation("저장되었습니다");

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                clsLog.logSave(this, "btn_save_Click", SQL);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_loadData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = ShowMessageBox.Confirm("주원료 하차정보를 하차정보에서 불러오시겠습니까?", "기존에 입력된 내역은 전부 삭제되고 하차정보 데이터로 새로 입력됩니다");
                if (result != DialogResult.Yes)
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                SQL = $"DELETE FROM MAINMATERIALS_REPORT WHERE WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                Dbconn.conn.SQLrun(SQL);

                SQL = $"DELETE FROM MAINMATERIALS_CHK_LIST WHERE WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                Dbconn.conn.SQLrun(SQL);

                SQL = $@"
                INSERT INTO MAINMATERIALS_REPORT (
                     WORK_NUMBER
                   , WORK_SEQ
                   , RESOURCE_NO
                   , LOCATION
                   , INPUT_QTY
                   , START_TIME
                   , END_TIME
                   , INCAR_NO
                   , DRIVER_NAME
                   , WORK_NAME
                   , REMARK
                   , I_TIME
                   , I_USER
                )
                SELECT  TO_CHAR(WS.IN_TIME, 'YYYYMMDD')           -- WORK_NUMBER
                      , ROW_NUMBER() OVER(ORDER BY WS.SEQ)        -- WORK_SEQ
                      , WS.RESOURCE_NO
                      , WS.LOCATION
                      , WS.SPIV_CAR_WEIGHT
                      , WS.IN_TIME
                      , WS.OUT_TIME
                      , WS.CAR_FULL_NUM
                      , ERP_CUST.NAME_ORG1         -- DRIVER_NAME
                      , NULL                                      -- WORK_NAME
                      , 'AUTO'                                    -- REMARK
                      , SYSDATE                                   -- I_TIME
                      , 'AUTO'                                    -- I_USER
                FROM WAP_SLIO_DOWN WS
                LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING
                   ON WS.RESOURCE_NO = ERP_ING.RESOURCE_NO
                LEFT OUTER JOIN SAP_DI_CUSTOMER ERP_CUST
                   ON WS.CUST_CODE = ERP_CUST.PARTNER
                WHERE TO_CHAR(WS.IN_TIME, 'YYYYMMDD') = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                  AND NVL(WS.DEL_FLAG, 'N') != 'Y'
                ORDER BY WS.SEQ
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("하차내역을 불러오기가 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowInformation("하차정보 불러오기가 완료되었습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_loadData_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
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
                if (gridView.GetSelectedRows().Length == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 데이터를 체크하여주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 주원료하차정보 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in gridView.GetSelectedRows())
                {
                    DataRow sel_row = gridView.GetDataRow(id);

                    string work_num = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                    string work_seq = sel_row["WORK_SEQ"].ToString().Trim();

                    if (sel_row.RowState != DataRowState.Added)
                    {
                        SQL = $"DELETE FROM MAINMATERIALS_REPORT WHERE WORK_NUMBER = '{work_num}' AND WORK_SEQ = '{work_seq}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                            return;
                        }
                    }
                    else
                    {
                        gridView.DeleteRow(gridView.FocusedRowHandle);
                    }
                } //foeach

                Dbconn.conn.Commit();

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                Dbconn.conn.Rollback();
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
    }
}