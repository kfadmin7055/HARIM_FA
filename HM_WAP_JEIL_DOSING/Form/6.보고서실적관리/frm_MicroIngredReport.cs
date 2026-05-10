using Core.Class;
using Core.Extension;
using DevExpress.XtraEditors;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraLayout;
using DevExpress.XtraPrinting.Export;
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
    public partial class frm_MicroIngredReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_MicroIngredReport()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void frm_MicroIngredReport_Load(object sender, EventArgs e)
        {
            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

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

        private void XMain_Search()
        {
            try
            {

                SQL = $@"
                SELECT PLANT_CODE, WORK_NUMBER, WORK_SEQ, RESOURCE_NO, LOCATION, INPUT_QTY, EMPLOYEE_NO, INPUT_TIME, I_USER, I_TIME
                FROM MICRO_INPUT_REPORT
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ORDER BY WORK_NUMBER, WORK_SEQ 
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false);

                sValid = new string[] { "" };


                gridScboResourceNO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboResourceNO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("반제품")}', '{clsCommon.GetResourceTypeCode("원재료")}'"));
                gridScboResourceNO.PopupFormMinSize = new Size(500, 600);


                //작업자정보
                gridScboEMPLOYEE_NO.NullValuePrompt = "";
                gridScboEMPLOYEE_NO.NullText = "";
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboEMPLOYEE_NO, clsCommon.GetDO_INSA(cboPlant_Code.EditValue?.ToString()));

                checkEdit_CHK_LIST1.Checked = false;
                textEdit_CHK_REMARK1.Text = string.Empty;
                checkEdit_CHK_LIST2.Checked = false;
                textEdit_CHK_REMARK2.Text = string.Empty;
                checkEdit_CHK_LIST3.Checked = false;
                textEdit_CHK_REMARK3.Text = string.Empty;
                checkEdit_CHK_LIST4.Checked = false;
                textEdit_CHK_REMARK4.Text = string.Empty;
                checkEdit_CHK_LIST5.Checked = false;
                textEdit_CHK_REMARK5.Text = string.Empty;

                SQL = $@"
                SELECT 
                    WORK_NUMBER, 
                    CHK_LIST1, CHK_REMARK1, 
                    CHK_LIST2, CHK_REMARK2, 
                    CHK_LIST3, CHK_REMARK3, 
                    CHK_LIST4, CHK_REMARK4, 
                    CHK_LIST5, CHK_REMARK5, 
                    I_TIME, 
                    I_USER  
                FROM 
                    MICRO_INPUT_CHK_LIST 
                WHERE 
                    PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                ";

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) > 0 )
                {
                    string chk_list1 = Dbconn.conn.getData(chkListDs, "CHK_LIST1", 0);
                    string chk_remark1 = Dbconn.conn.getData(chkListDs, "CHK_REMARK1", 0);
                    string chk_list2 = Dbconn.conn.getData(chkListDs, "CHK_LIST2", 0);
                    string chk_remark2 = Dbconn.conn.getData(chkListDs, "CHK_REMARK2", 0);
                    string chk_list3 = Dbconn.conn.getData(chkListDs, "CHK_LIST3", 0);
                    string chk_remark3 = Dbconn.conn.getData(chkListDs, "CHK_REMARK3", 0);
                    string chk_list4 = Dbconn.conn.getData(chkListDs, "CHK_LIST4", 0);
                    string chk_remark4 = Dbconn.conn.getData(chkListDs, "CHK_REMARK4", 0);
                    string chk_list5 = Dbconn.conn.getData(chkListDs, "CHK_LIST5", 0);
                    string chk_remark5 = Dbconn.conn.getData(chkListDs, "CHK_REMARK5", 0);

                    checkEdit_CHK_LIST1.Checked = chk_list1.Contains("Y") ? true : false;
                    checkEdit_CHK_LIST2.Checked = chk_list2.Contains("Y") ? true : false;
                    checkEdit_CHK_LIST3.Checked = chk_list3.Contains("Y") ? true : false;
                    checkEdit_CHK_LIST4.Checked = chk_list4.Contains("Y") ? true : false;
                    checkEdit_CHK_LIST5.Checked = chk_list5.Contains("Y") ? true : false;

                    textEdit_CHK_REMARK1.Text = chk_remark1;
                    textEdit_CHK_REMARK2.Text = chk_remark2;
                    textEdit_CHK_REMARK3.Text = chk_remark3;
                    textEdit_CHK_REMARK4.Text = chk_remark4;
                    textEdit_CHK_REMARK5.Text = chk_remark5;
                }

                ds.Dispose();


            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
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

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string workDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                clsPrintReport.PrintMicroReport(cboPlant_Code.EditValue?.ToString(), workDate);
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

        private void layoutControl_checkList_CustomDraw(object sender, ItemCustomDrawEventArgs e)
        {
            try
            {
                e.DefaultDraw();
                Pen pen = new Pen(Brushes.DarkGray, 1);
                e.Cache.DrawRectangle(pen, e.Bounds);
                pen.Dispose();
                e.Handled = true;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "layoutControl_checkList_CustomDraw", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
 
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboPlant_Code.EditValue?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("플랜트를 먼저 선택 조회 해주세요.");
                    cboPlant_Code.Focus();
                    return;
                }

                clsDevexpressGrid.GridViewAddRow(gridView);
                gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
                gridView.SetFocusedRowCellValue("L_CODE", clsCommon.GetProcessKey("마이크로").Merge(cboL_Code.EditValue?.ToString()));
                gridView.SetFocusedRowCellValue("WORK_NUMBER", DateTime.Now.ToString("yyyyMMdd"));

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["INPUT_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["INPUT_QTY"], "0");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["EMPLOYEE_NO"], clsCommon._strUserId);

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

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }


            if (DialogResult.Yes != ShowMessageBox.Confirm("마이크로원료 투입일지 정보를 저장하시겠습니까?"))
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
                        dr.SetColumnError("RESOURCE_NO", "투입원료정보를 선택하여 주세요");
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

                    if (string.IsNullOrEmpty(dr["INPUT_TIME"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("INPUT_TIME", "투입시간 정보를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("INPUT_TIME"));
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
                        INSERT INTO MICRO_INPUT_REPORT (
                             PLANT_CODE       -- 1
                            , WORK_NUMBER     -- 2
                            , WORK_SEQ        -- 3
                            , RESOURCE_NO     -- 4
                            , LOCATION        -- 5
                            , INPUT_QTY       -- 6
                            , EMPLOYEE_NO     -- 7
                            , INPUT_TIME      -- 8
                            , I_TIME          -- 9
                            , I_USER          -- 10
                        )
                        VALUES (
                             '{dr["PLANT_CODE"]}'       -- 1
                            , '{dr["WORK_NUMBER"]}'     -- 2
                            , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) FROM MICRO_INPUT_REPORT WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND WORK_NUMBER = '{dr["WORK_NUMBER"]}')        -- 3
                            , '{dr["RESOURCE_NO"]}'     -- 4
                            , '{dr["LOCATION"]}'        -- 5
                            , '{dr["INPUT_QTY"]}'       -- 6
                            , '{dr["EMPLOYEE_NO"]}'     -- 7
                            , TO_DATE('{Convert.ToDateTime(dr["INPUT_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')      -- 8
                            , SYSDATE                   -- 9
                            , '{dr["I_USER"]}'          -- 10
                        )
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
                        UPDATE MICRO_INPUT_REPORT
                        SET    RESOURCE_NO = '{dr["RESOURCE_NO"]}',
                               LOCATION    = '{dr["LOCATION"]}',
                               INPUT_QTY   = '{dr["INPUT_QTY"]}',
                               EMPLOYEE_NO = '{clsCommon._strUserId}',
                               INPUT_TIME  = TO_DATE('{Convert.ToDateTime(dr["INPUT_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                               I_TIME      = SYSDATE,
                               I_USER      = '{clsCommon._strUserId}'
                        WHERE  WORK_NUMBER = '{dr["WORK_NUMBER"]}'
                        AND    WORK_SEQ    = '{dr["WORK_SEQ"]}'
                        AND    PLANT_CODE  = '{dr["PLANT_CODE"]}'
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

                SQL = $"DELETE FROM MICRO_INPUT_CHK_LIST WHERE PLANT_CODE  = '{cboPlant_Code.EditValue}' AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                Dbconn.conn.SQLrun(SQL);


                SQL = $@"
                INSERT INTO MICRO_INPUT_CHK_LIST (
                   PLANT_CODE, WORK_NUMBER, CHK_LIST1, 
                   CHK_REMARK1, CHK_LIST2, CHK_REMARK2, 
                   CHK_LIST3, CHK_REMARK3, CHK_LIST4, 
                   CHK_REMARK4, CHK_LIST5, CHK_REMARK5, 
                   I_TIME, I_USER) 
                VALUES ( 
                    '{cboPlant_Code.EditValue}'
                  , '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                  , '{checkEdit_CHK_LIST1.EditValue}'
                  , '{textEdit_CHK_REMARK1.EditValue}'
                  , '{checkEdit_CHK_LIST2.EditValue}'
                  , '{textEdit_CHK_REMARK2.EditValue}'
                  , '{checkEdit_CHK_LIST3.EditValue}'
                  , '{textEdit_CHK_REMARK3.EditValue}'
                  , '{checkEdit_CHK_LIST4.EditValue}'
                  , '{textEdit_CHK_REMARK4.EditValue}'
                  , '{checkEdit_CHK_LIST5.EditValue}'
                  , '{textEdit_CHK_REMARK5.EditValue}'
                  , SYSDATE
                  , '{clsCommon._strUserId}' )
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("MICRO_INPUT_CHK_LIST 저장에 실패했습니다");
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
                DialogResult result = ShowMessageBox.Confirm("마이크로 투입정보를 빈투입내역정보에서 불러오시겠습니까?", "기존에 입력된 내역은 전부 삭제되고 빈투입내역정보 데이터로 새로 입력됩니다");
                if (result != DialogResult.Yes)
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                SQL = $"DELETE FROM MICRO_INPUT_REPORT WHERE PLANT_CODE  = '{cboPlant_Code.EditValue}' AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                Dbconn.conn.SQLrun(SQL);

                SQL = $"DELETE FROM MICRO_INPUT_CHK_LIST WHERE PLANT_CODE  = '{cboPlant_Code.EditValue}' AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' ";
                
                Dbconn.conn.SQLrun(SQL);

                SQL = $@"
                SELECT 
                  TO_CHAR(a.I_TIME, 'YYYYMMDD'),
                  ROW_NUMBER() OVER(ORDER BY a.I_TIME),
                  a.RESOURCE_NO,
                  a.LOCATION,
                  a.INPUT_QTY,
                  a.I_USER,
                  a.I_TIME,
                  SYSDATE,
                  'AUTO'
                FROM BIN_INGRED_INPUT a
                LEFT OUTER JOIN SAP_DI_PRODUCT b ON a.RESOURCE_NO = b.RESOURCE_NO
                LEFT OUTER JOIN DO_INSA c ON a.I_USER = c.EMPLOYEE_NO
                WHERE aPLANT_CODE  = '{cboPlant_Code.EditValue}'
                  AND a.LOCATION LIKE '9%'
                  AND TO_CHAR(a.I_TIME, 'YYYYMMDD') = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                ORDER BY a.I_TIME
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("내역을 불러오기가 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowInformation("마이크로 빈투입내역정보 불러오기가 완료되었습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_loadData_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }

        }

        private void b_delete_Click(object sender, EventArgs e)
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

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 마이크로 투입정보 데이터를 삭제하시겠습니까?"))
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
                        SQL = $"DELETE FROM MICRO_INPUT_REPORT WHERE WORK_NUMBER = '{work_num}' AND WORK_SEQ = '{work_seq}' ";

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

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, false);

            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboProcessKey_EditValueChanged(object sender, EventArgs e)
        {
            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);
        }
    }
}