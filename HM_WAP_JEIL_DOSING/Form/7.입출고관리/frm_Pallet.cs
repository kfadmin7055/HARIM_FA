using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class frm_Pallet : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_Pallet()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                SQL =
                    "SELECT PTMCD, PTMCDNM, WEIGHT, DISPLAY_SEQ, USER_ID, I_TIME FROM WAP_PA_MASTER " +
                    "ORDER BY DISPLAY_SEQ ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false);

                sValid = new string[] { "" };


                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void frm_Pallet_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("파렛트 정보데이터를 저장하시겠습니까?"))
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
                    if (string.IsNullOrEmpty(dr["PTMCD"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("PTMCD", "파렛트코드를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("PTMCD"));
                        return;
                    }


                    if (string.IsNullOrEmpty(dr["PTMCDNM"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("PTMCDNM", "파렛트명을 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("PTMCDNM"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["WEIGHT"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("WEIGHT", "파렛트 무계를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("WEIGHT"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["DISPLAY_SEQ"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("DISPLAY_SEQ", "화면표시순서를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("DISPLAY_SEQ"));
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO WAP_PA_MASTER (
                           PTMCD, PTMCDNM, WEIGHT, 
                           DISPLAY_SEQ, USER_ID,
                           I_TIME) 
                        VALUES (
                             '{dr["PTMCD"]}'
                           , '{dr["PTMCDNM"]}'
                           , '{dr["WEIGHT"]}'
                           , '{dr["DISPLAY_SEQ"]}'
                           , '{dr["USER_ID"]}'
                           , SYSDATE )
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
                        SQL = $@"
                        UPDATE WAP_PA_MASTER
                        SET    PTMCD       = '{dr["PTMCD"]}',
                               PTMCDNM     = '{dr["PTMCDNM"]}',
                               WEIGHT      = '{dr["WEIGHT"]}',
                               DISPLAY_SEQ = '{dr["DISPLAY_SEQ"]}',
                               USER_ID     = '{clsCommon.UserId}',
                               I_TIME      = SYSDATE
                        WHERE  PTMCD       = '{dr["PTMCD"]}'
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

                if (gridView.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 파렛트정보를 선택하여 주세요");
                    return;
                }

                DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    gridView.DeleteRow(gridView.FocusedRowHandle);
                }
                else
                {
                    DialogResult result = ShowMessageBox.Confirm($"선택하신 파렛트정보 {clsDevexpressGrid.GetFocusedRowDisplayText(gridView, "PTMCDNM")} 를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        SQL = $"DELETE FROM WAP_PA_MASTER WHERE PTMCD = '{clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PTMCD")}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
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

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(gridView);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["USER_ID"], clsCommon.UserId);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["DISPLAY_SEQ"], gridView.RowCount.ToString());
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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
    }
}