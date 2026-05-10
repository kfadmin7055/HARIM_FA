using Core;
using Core.Class;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static DevExpress.XtraEditors.Mask.MaskSettings;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace HARIM_FA_DOSING
{
    public partial class frm_SubIngredReport : DevExpress.XtraEditors.XtraForm
    {
        string SQL = string.Empty;
        private string[] sValid = null;
        DataSet authDs;


        public frm_SubIngredReport()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(bandedGridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT
                    PLANT_CODE,
                    WORK_NUMBER, 
                    WORK_SEQ, 
                    RESOURCE_NO, 
                    SPCS, 
                    LOCATION, 
                    INPUT_QTY, 
                    START_TIME, 
                    END_TIME, 
                    CHK_YN1, 
                    CHK_YN2, 
                    CHK_YN3, 
                    CHK_YN4, 
                    CHK_END_DATE
                FROM 
                    SUBMATERIALS_REPORT
                WHERE 
                    ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                ORDER BY 
                    WORK_NUMBER, WORK_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, bandedGridView, ds.Tables[0], false, true);

                sValid = new string[] { "" };


                SQL = $"SELECT CHK_MEMO1, CHK_MEMO2, CHK_REMARK FROM SUBMATERIALS_CHK_LIST WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                txt_CHK_MEMO1.Text = string.Empty;
                txt_CHK_MEMO2.Text = string.Empty;
                memoEdit_CHK_REMARK.Text = string.Empty;

                if (Dbconn.conn.getRowCnt(chkListDs) > 0)
                {
                    txt_CHK_MEMO1.Text = Dbconn.conn.getData(chkListDs, "CHK_MEMO1", 0);
                    txt_CHK_MEMO2.Text = Dbconn.conn.getData(chkListDs, "CHK_MEMO2", 0);
                    memoEdit_CHK_REMARK.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK", 0);
                }

                repositoryItemLookUpEdit_RESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                repositoryItemLookUpEdit_RESOURCE_NO.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch;
                clsDevexpressGrid.ItemLookUpEditSetup(repositoryItemLookUpEdit_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("제품")}', '{clsCommon.GetResourceTypeCode("반제품")}', '{clsCommon.GetResourceTypeCode("원재료")}'"));
                repositoryItemLookUpEdit_RESOURCE_NO.PopupFormMinSize = new Size(500, 600);

                chkListDs.Dispose();
                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            } 
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void bandedGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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

        private void frm_SubIngredReport_Load(object sender, EventArgs e)
        {
            try
            {
                //플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_SubIngredReport_Load", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("부원료 투입일지 정보를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(bandedGridView);
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
                    string rValid = clsCommon.ValdationCheck(sValid, dr, bandedGridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        bandedGridView.FocusedColumn = bandedGridView.Columns[rValid]; // 이동할 컬럼명
                        bandedGridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    //input check
                    if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("RESOURCE_NO", "투입원료정보를 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["INPUT_QTY"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("INPUT_QTY", "투입양을 입력해주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("INPUT_QTY"));
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SUBMATERIALS_REPORT (
                           PLANT_CODE, WORK_NUMBER, WORK_SEQ,
                           RESOURCE_NO, SPCS, LOCATION, 
                           INPUT_QTY, START_TIME, END_TIME, 
                           CHK_YN1, CHK_YN2, CHK_YN3, 
                           CHK_YN4, CHK_END_DATE, I_TIME, 
                           I_USER) 
                        VALUES ( 
                            '{dr["PLANT_CODE"]}'
                            , '{dr["WORK_NUMBER"]}'
                            , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) FROM SUBMATERIALS_REPORT WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND WORK_NUMBER = '{dr["WORK_NUMBER"]}')
                            , '{dr["RESOURCE_NO"]}'
                            , '{dr["SPCS"]}'
                            , '{dr["LOCATION"]}'
                            , '{dr["INPUT_QTY"]}'
                            , TO_DATE('{Convert.ToDateTime(dr["START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                            , TO_DATE('{Convert.ToDateTime(dr["END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                            , '{dr["CHK_YN1"]}'
                            , '{dr["CHK_YN2"]}'
                            , '{dr["CHK_YN3"]}'
                            , '{dr["CHK_YN4"]}'
                            , '{dr["CHK_END_DATE"]}'
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
                        UPDATE SUBMATERIALS_REPORT
                        SET    RESOURCE_NO  = '{dr["RESOURCE_NO"]}',
                               SPCS         = '{dr["SPCS"]}',
                               LOCATION     = '{dr["LOCATION"]}',
                               INPUT_QTY    = '{dr["INPUT_QTY"]}',
                               START_TIME   = TO_DATE('{Convert.ToDateTime(dr["START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                               END_TIME     = TO_DATE('{Convert.ToDateTime(dr["END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                               CHK_YN1      = '{dr["CHK_YN1"]}',
                               CHK_YN2      = '{dr["CHK_YN2"]}',
                               CHK_YN3      = '{dr["CHK_YN3"]}',
                               CHK_YN4      = '{dr["CHK_YN4"]}',
                               CHK_END_DATE = '{dr["CHK_END_DATE"]}',
                               I_TIME       = SYSDATE,
                               I_USER       = '{clsCommon._strUserId}'
                        WHERE  PLANT_CODE   = '{dr["PLANT_CODE"]}'
                        AND    WORK_NUMBER  = '{dr["WORK_NUMBER"]}'
                        AND    WORK_SEQ     = '{dr["WORK_SEQ"]}'
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

                SQL = $"DELETE FROM SUBMATERIALS_CHK_LIST WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("SUBMATERIALS_CHK_LIST 삭제에 실패했습니다");
                    return;
                }

                if (!string.IsNullOrEmpty(txt_CHK_MEMO1.Text) || !string.IsNullOrEmpty(txt_CHK_MEMO2.Text) || !string.IsNullOrEmpty(memoEdit_CHK_REMARK.Text))
                {
                    SQL = $@"
                    INSERT INTO SUBMATERIALS_CHK_LIST (
                       PLANT_CODE, WORK_NUMBER, CHK_MEMO1, 
                       CHK_MEMO2, CHK_REMARK, I_TIME, 
                       I_USER) 
                    VALUES ( 
                        '{cboPlant_Code.EditValue}',
                        '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}',
                        '{txt_CHK_MEMO1.Text}',
                        '{txt_CHK_MEMO2.Text}',
                        '{memoEdit_CHK_REMARK.Text}',
                        SYSDATE,
                        '{clsCommon._strUserId}' )
                    ";
                }

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("SUBMATERIALS_CHK_LIST 저장에 실패했습니다");
                    return;
                }

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

        private void layoutControl_CustomDraw(object sender, ItemCustomDrawEventArgs e)
        {
            try
            {
                if (e.Item.Tag != null && e.Item.Tag.ToString() == "LINE")
                {
                    e.DefaultDraw();
                    Pen pen = new Pen(Brushes.DarkGray, 1);
                    e.Cache.DrawRectangle(pen, e.Bounds);
                    pen.Dispose();
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "layoutControl_CustomDraw", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string workDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                clsPrintReport.PrintSubIngredReport(cboPlant_Code.EditValue?.ToString(), workDate);
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
                clsDevexpressGrid.GridViewAddRow(bandedGridView);
                
                bandedGridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue?.ToString());
                bandedGridView.SetFocusedRowCellValue("WORK_NUMBER", string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
                bandedGridView.SetRowCellValue(bandedGridView.FocusedRowHandle, bandedGridView.Columns["START_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                bandedGridView.SetRowCellValue(bandedGridView.FocusedRowHandle, bandedGridView.Columns["END_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                bandedGridView.SetRowCellValue(bandedGridView.FocusedRowHandle, bandedGridView.Columns["CHK_YN1"], "Y");
                bandedGridView.SetRowCellValue(bandedGridView.FocusedRowHandle, bandedGridView.Columns["CHK_YN2"], "Y");
                bandedGridView.SetRowCellValue(bandedGridView.FocusedRowHandle, bandedGridView.Columns["CHK_YN3"], "Y");
                bandedGridView.SetRowCellValue(bandedGridView.FocusedRowHandle, bandedGridView.Columns["CHK_YN4"], "Y");
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
                clsDevexpressGrid.GridEndEdit(bandedGridView);
                clsDevexpressGrid.GridViewLastAddRowDelete(bandedGridView);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_loadData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = ShowMessageBox.Confirm("부원료 투입정보를 빈투입내역정보에서 불러오시겠습니까?", "기존에 입력된 내역은 전부 삭제되고 빈투입내역정보 데이터로 새로 입력됩니다");
                if (result != DialogResult.Yes)
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                SQL = $"DELETE FROM SUBMATERIALS_REPORT WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                Dbconn.conn.SQLrun(SQL);

                SQL = $"DELETE FROM SUBMATERIALS_CHK_LIST WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                Dbconn.conn.SQLrun(SQL);

                SQL = $@"
                INSERT INTO SUBMATERIALS_REPORT (
                     PLANT_CODE
                   , WORK_NUMBER
                   , WORK_SEQ
                   , RESOURCE_NO
                   , SPCS
                   , LOCATION
                   , INPUT_QTY
                   , START_TIME
                   , END_TIME
                   , CHK_YN1
                   , CHK_YN2
                   , CHK_YN3
                   , CHK_YN4
                   , CHK_END_DATE
                   , I_TIME
                   , I_USER
                )
                SELECT  a.PLANT_CODE
                      , TO_CHAR(a.I_TIME, 'YYYYMMDD')                -- WORK_NUMBER
                      , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) FROM SUBMATERIALS_REPORT WHERE PLANT_CODE = a.PLANT_CODE AND WORK_NUMBER = TO_CHAR(a.I_TIME, 'YYYYMMDD'))         -- WORK_SEQ
                      , b.RESOURCE_NO
                      , NULL                                              -- SPCS (값 누락되어 NULL 처리)
                      , a.LOCATION
                      , a.INPUT_QTY
                      , a.I_TIME
                      , a.I_TIME
                      , 'Y'
                      , 'Y'
                      , 'Y'
                      , 'Y'
                      , SYSDATE                                           -- CHK_END_DATE
                      , SYSDATE                                           -- I_TIME
                      , 'AUTO'                                            -- I_USER
                FROM BIN_INGRED_INPUT a
                        INNER JOIN BIN b ON b.PLANT_CODE = a.PLANT_CODE AND b.LOCATION = a.LOCATION
                LEFT OUTER JOIN SAP_DI_PRODUCT c ON b.RESOURCE_NO = c.RESOURCE_NO
                LEFT OUTER JOIN DO_INSA INSA
                   ON a.I_USER = INSA.EMPLOYEE_NO
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}') AND a.LOCATION < 400
                  AND TO_CHAR(a.I_TIME, 'YYYYMMDD') = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                ORDER BY a.I_TIME
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("투입정보 내역을 불러오기가 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowInformation("부원료 투입정보 불러오기가 완료되었습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_loadData_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
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
                if (bandedGridView.GetSelectedRows().Length == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 데이터를 체크하여주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 부원료 투입정보 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in bandedGridView.GetSelectedRows())
                {
                    DataRow sel_row = bandedGridView.GetDataRow(id);

                    string work_num = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                    string work_seq = sel_row["WORK_SEQ"].ToString().Trim();

                    if (sel_row.RowState != DataRowState.Added)
                    {
                        SQL = $"DELETE FROM SUBMATERIALS_REPORT WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND WORK_NUMBER = '{work_num}' AND WORK_SEQ = '{work_seq}' ";

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
                        bandedGridView.DeleteRow(bandedGridView.FocusedRowHandle);
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
            bandedGridView.FocusedRowHandle = 0;
            bandedGridView.FocusedColumn = bandedGridView.VisibleColumns[0];
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }
    }
}