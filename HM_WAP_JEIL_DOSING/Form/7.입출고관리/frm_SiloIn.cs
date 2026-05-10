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
    public partial class frm_SiloIn : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_SiloIn()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_RESOURCE_NO, clsCommon.GetResource("", "", $"'{clsCommon.GetResourceTypeCode("원재료")}', '{clsCommon.GetResourceTypeCode("반제품")}'"));

                SQL = $@"
                SELECT 
                    a.SEQ, 
                    a.IN_TIME AS IN_DAY, 
                    b.IS_NO, 
                    a.CUST_CODE, 
                    a.SPCS, 
                    a.RESOURCE_NO,  
                    a.CAR_FULL_NUM, 
                    b.INCAR_DATE, 
                    a.LOCATION, 
                    a.SPIV_CAR_WEIGHT, 
                    a.IN_TIME, 
                    a.OUT_TIME, 
                    b.OUTCAR_DATE
                FROM 
                    WAP_SLIO_DOWN a
                    LEFT JOIN WAP_DECAR b ON a.IS_NO = b.IS_NO
                    LEFT JOIN WAP_GOCAR c ON b.IS_NO = c.IS_NO
                WHERE 
                    TO_CHAR(a.IN_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                     AND NVL(a.DEL_FLAG, 'N') != 'Y'
                ORDER BY b.INCAR_DATE
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
        }

        private void frm_SiloIn_Load(object sender, EventArgs e)
        {
            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                dateEdit_workStartDate.EditValue = DateTime.Today;
                dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_SiloIn_Load", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_excelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    gridView.ExportToXlsx(sfd.FileName);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_excelExport_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
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

        private void checkEdit_reflashSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit_reflashSearch.Checked)
            {
                reflash_timer.Interval = 5000;
                reflash_timer.Enabled = true;
            }
            else
            {
                reflash_timer.Enabled = false;
            }
        }

        private void reflash_timer_Tick(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridViewAddRow(gridView);
                gridView.SetFocusedRowCellValue("SEQ", clsCommon.GetMaxSeq("WAP_SLIO_DOWN", "SEQ", new Dictionary<string, string> { { "IS_NO", $"" } }));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["IN_DAY"], DateTime.Now.ToString("yyyy-MM-dd"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["IN_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["OUT_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["SPIV_CAR_WEIGHT"], "0");
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
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

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

            if (DialogResult.Yes != ShowMessageBox.Confirm("분쇄일지 정보를 저장하시겠습니까?"))
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

                    if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                    {
                        dr.SetColumnError("RESOURCE_NO", "원료를 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO WAP_SLIO_DOWN (
                           IS_NO, SEQ, CUST_CODE, 
                           CAR_FULL_NUM, SPCS, LOCATION, 
                           RESOURCE_NO, SPIV_CAR_WEIGHT, IN_TIME, 
                           OUT_TIME, DEL_FLAG, AUTO_YN, 
                           U_TIME, U_USER) 
                        VALUES ( 
                             '{dr["IS_NO"]}'
                           , (SELECT NVL(MAX(SEQ) + 1, 1) FROM WAP_SLIO_DOWN WHERE IS_NO = '{dr["IS_NO"]}')
                           , '{dr["CUST_CODE"]}'
                           , '{dr["CAR_FULL_NUM"]}'
                           , '{dr["SPCS"]}'
                           , '{dr["LOCATION"]}'
                           , '{dr["RESOURCE_NO"]}'
                           , '{dr["SPIV_CAR_WEIGHT"]}'
                           , ''{Convert.ToDateTime(dr["IN_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}'
                           , '{Convert.ToDateTime(dr["OUT_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}'
                           , '{dr["DEL_FLAG"]}'
                           , 'N'
                           , SYSDATE
                           , '{clsCommon._strUserId}' )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        if (!clsCommon.Auth_Form_Function(authDs, "W"))
                        {
                            ShowMessageBox.XtraShowInformation("권한이 없습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE WAP_SLIO_DOWN
                        SET    SEQ             = '{dr["SEQ"]}',
                               IS_NO           = '{dr["IS_NO"]}',
                               CUST_CODE       = '{dr["CUST_CODE"]}',
                               CAR_FULL_NUM    = '{dr["CAR_FULL_NUM"]}',
                               SPCS            = '{dr["SPCS"]}',
                               LOCATION        = '{dr["LOCATION"]}',
                               RESOURCE_NO     = '{dr["RESOURCE_NO"]}',
                               SPIV_CAR_WEIGHT = '{dr["SPIV_CAR_WEIGHT"]}',
                               IN_TIME         = '{Convert.ToDateTime(dr["IN_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}',
                               OUT_TIME        = '{Convert.ToDateTime(dr["OUT_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}',
                               DEL_FLAG        = '{dr["DEL_FLAG"]}',
                               AUTO_YN         = '{dr["AUTO_YN"]}',
                               U_TIME          = '{dr["U_TIME"]}',
                               U_USER          = '{clsCommon._strUserId}'
                        WHERE  SEQ             = '{dr["SEQ"]}'
                        AND    IS_NO           = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
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
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (gridView.GetSelectedRows().Length == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 데이터를 체크하여주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 하차내역 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in gridView.GetSelectedRows())
                {
                    DataRow sel_row = gridView.GetDataRow(id);
                    string seq = sel_row["SEQ"].ToString().Trim();

                    if (sel_row.RowState != DataRowState.Added)
                    {
                        SQL = $"UPDATE WAP_SLIO_DOWN SET DEL_FLAG = 'Y', U_TIME = GETDATE(), U_USER = '{clsCommon._strUserId}' WHERE SEQ = '{seq}' ";

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

        private void dateEdit_workStartDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workEndDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}