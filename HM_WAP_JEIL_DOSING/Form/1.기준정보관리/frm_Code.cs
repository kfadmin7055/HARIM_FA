using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using GridView = DevExpress.XtraGrid.Views.Grid.GridView;

namespace HARIM_FA_DOSING
{
    public partial class frm_Code : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;
        private int lFocusRow = 0;
        private int mFocusRow = 0;
        private int sFocusRow = 0;

        public frm_Code()
        {
            InitializeComponent();

            clsDevexpressGrid.EditGridViewInit(gridView_l, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(gridView_m, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(gridView_s, Properties.Settings.Default.FontSize);
        }

        private void frm_Code_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);
            comm_large_search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl_large));
            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl_meddle));
            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl_small));
        }

        #region 대분류 조회이벤트
        private void comm_large_search(bool fixFocus = false)
        {
            try
            {
                SQL = $@"
                SELECT WK_DIVCODE, CODENM, DISPLAY_SEQ, REMARK
                FROM COMM_DIV
                ORDER BY DISPLAY_SEQ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl_large, gridView_l, ds.Tables[0], false, fixFocus);

                sValid = new string[] { "WK_DIVCODE" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = gridView_l.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "comm_large_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 대분류 재조회버튼 클릭이벤트
        private void btn_l_reflash_Click(object sender, EventArgs e)
        {
            comm_large_search(false);
        }
        #endregion

        #region 대분류 새행초기화 이벤트
        private void gridView_l_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                view.SetRowCellValue(e.RowHandle, view.Columns["DISPLAY_SEQ"], gridView_l.RowCount.ToString());
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_l_InitNewRow", ex);
                ShowMessageBox.XtraShowWarning("새행을 초기화하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 대분류 행추가버튼 클릭이벤트
        private void btn_l_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView_l);
        }
        #endregion

        #region 대분류 행삭제버튼 클릭이벤트
        private void btn_l_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView_l);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView_l);
        }
        #endregion

        #region 대분류 저장버튼 클릭 이벤트
        private void btn_l_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("대분류 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            try
            {
                clsDevexpressGrid.GridEndEdit(gridView_l);

                splashScreenManager.ShowWaitForm();
                DataTable DT = (DataTable)gridControl_large.DataSource;

                lFocusRow = gridView_l.FocusedRowHandle;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView_l);

                    if (string.IsNullOrWhiteSpace(rValid) && rValid != "")
                    {
                        gridView_l.FocusedColumn = gridView_l.Columns[rValid]; // 이동할 컬럼명
                        gridView_l.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(dr["WK_DIVCODE"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("대분류코드를 입력하여 주세요");
                        dr.SetColumnError("WK_DIVCODE", "대분류코드를 입력하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["CODENM"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("대분류명을 입력하여 주세요");
                        dr.SetColumnError("CODENM", "대분류명을 입력하여 주세요");
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {

                        SQL = "SELECT * FROM COMM_DIV WHERE WK_DIVCODE = '{0}' ";
                        SQL = string.Format(SQL,
                            dr["WK_DIVCODE"].ToString()
                            );

                        DataSet dukDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(dukDs) == 1)
                        {
                            ShowMessageBox.XtraShowWarning("대분류코드가 중복됩니다.\r\n중복되지 않은 유일한 코드를 입력하여 주세요");
                            dr.RowError = "대분류코드가 중복됩니다.\r\n중복되지 않은 유일한 코드를 입력하여 주세요";
                            return;
                        }

                        string seq = "";
                        if (string.IsNullOrEmpty(dr["DISPLAY_SEQ"].ToString().Trim()))
                        {
                            SQL = "SELECT NVL(MAX(DISPLAY_SEQ),0) + 1 as DIS_SEQ FROM COMM_DIV ";
                            DataSet SEQ_DS = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(SEQ_DS) > 0)
                            {
                                seq = Dbconn.conn.getData(SEQ_DS, "DIS_SEQ", 0);
                            }
                        }
                        else
                        {
                            seq = dr["DISPLAY_SEQ"].ToString();
                        }

                        SQL = $@"
                        INSERT INTO COMM_DIV (
                           WK_DIVCODE
                         , CODENM
                         , DISPLAY_SEQ
                         , REMARK
                        )
                        VALUES (
                           '{dr["WK_DIVCODE"]}'
                         , '{dr["CODENM"]}'
                         , '{seq}'
                         , '{dr["REMARK"]}'
                        )
                        ";
                            
                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_l_save_Click", SQL);
                            dr.RowError = "데이터 입력에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = "UPDATE COMM_DIV SET CODENM = '{1}', DISPLAY_SEQ = '{2}', REMARK = '{3}' WHERE WK_DIVCODE = '{0}' ";
                        SQL = string.Format(SQL,
                            dr["WK_DIVCODE"].ToString(),
                            dr["CODENM"].ToString(),
                            dr["DISPLAY_SEQ"].ToString(),
                            dr["REMARK"].ToString()
                        );

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_l_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();
                    gridView_l.RefreshData();
                    gridControl_meddle.DataSource = null;
                    gridControl_small.DataSource = null;

                    comm_large_search(true);

                    gridView_l.FocusedRowHandle = lFocusRow;
                    gridView_l.SelectRow(lFocusRow);
                }

            }catch (Exception ex)
            {
                clsLog.logSave(this, "btn_l_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }
        #endregion

        #region 대분류 삭제버튼 클릭 이벤트
        private void btn_l_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView_l.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            if (clsDevexpressGrid.GridDeleteCheck(gridView_l) == false)
            {
                gridView_l.DeleteRow(gridView_l.FocusedRowHandle);
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 대분류코드 "+ gridView_l.GetFocusedRowCellValue("WK_DIVCODE") + " 데이터를 삭제하시겠습니까?", "삭제하실 경우 대분류코드의 하위코드가 전부 삭제가 됩니다"))
            {
                return;
            }

            try
            {
                splashScreenManager.ShowWaitForm();

                Dbconn.conn.BeginTransaction();

                SQL = "DELETE FROM COMM_DIV WHERE WK_DIVCODE = '{0}' ";
                SQL = string.Format(SQL,
                    gridView_l.GetFocusedRowCellValue("WK_DIVCODE")
                    );

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btn_l_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다 -1");
                    return;
                }

                SQL = "DELETE FROM COMM_CODE WHERE WK_DIVCODE = '{0}' ";
                SQL = string.Format(SQL,
                    gridView_l.GetFocusedRowCellValue("WK_DIVCODE")
                    );

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btn_l_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다 -2");
                    return;
                }

                SQL = "DELETE FROM COMM_DTCODE WHERE WK_DIVCODE = '{0}' ";
                SQL = string.Format(SQL,
                        gridView_l.GetFocusedRowCellValue("WK_DIVCODE")
                        );

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btn_l_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다 -3");
                    return;
                }

                Dbconn.conn.Commit();

                gridControl_meddle.DataSource = null;
                gridControl_small.DataSource = null;

                comm_large_search(true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_l_delete_Click()", ex);
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

        private void gridView_l_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string selData = clsDevexpressGrid.GetFocusedRowCellValue(gridView_l, "WK_DIVCODE");
            if (!string.IsNullOrEmpty(selData))
            {
                comm_meddle_search(selData);

                //gridView_m.ClearSelection();
                //gridView_m.FocusedRowHandle = 0;
                //gridView_m.SelectRow(0);
            }
        }

        #region 중분류 조회이벤트
        private void comm_meddle_search(string wkcode, bool fixFocus = false)
        {
            try
            {
                SQL = $@"
                SELECT WK_DIVCODE, COMM_CODE, COMM_NM, DISPLAY_SEQ, REMARK
                FROM COMM_CODE 
                WHERE WK_DIVCODE = '{wkcode}' 
                ORDER BY DISPLAY_SEQ 
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl_meddle, gridView_m, ds.Tables[0], false, fixFocus);

                sValid = new string[] { "COMM_CODE", "WK_DIVCODE", "COMM_NM", "DISPLAY_SEQ" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = gridView_m.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                ds.Dispose();

                //gridControl_small.DataSource = null;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "comm_meddle_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 중분류 재조회버튼 클릭이벤트
        private void btn_m_reflash_Click(object sender, EventArgs e)
        {
            int selCnt = clsDevexpressGrid.GetSelectRowCount(gridView_l);

            if (selCnt > 1)
            {
                ShowMessageBox.XtraShowInformation("대분류 항목선택 정보가 없습니다");
                return;
            }

            string selData = clsDevexpressGrid.GetFocusedRowCellValue(gridView_l, "WK_DIVCODE");
            if (!string.IsNullOrEmpty(selData))
            {
                comm_meddle_search(selData);
            }
        }
        #endregion

        #region 중분류 새행초기화 이벤트
        private void gridView_m_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                view.SetRowCellValue(e.RowHandle, view.Columns["WK_DIVCODE"], gridView_l.GetFocusedRowCellValue("WK_DIVCODE").ToString().Trim());
                view.SetRowCellValue(e.RowHandle, view.Columns["DISPLAY_SEQ"], gridView_m.RowCount.ToString());
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_m_InitNewRow", ex);
                ShowMessageBox.XtraShowWarning("새행을 초기화하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 중분류 행추가버튼 클릭이벤트
        private void btn_m_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView_l.SelectedRowsCount == 0)
            {
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView_m);
        }
        #endregion

        #region 중분류 행삭제버튼 클릭이벤트
        private void btn_m_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView_m);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView_m);
        }
        #endregion

        #region 중분류 저장버튼 클릭 이벤트
        private void btn_m_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }


            if (DialogResult.Yes != ShowMessageBox.Confirm("중분류 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            if (gridView_l.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("대분류 선택정보가 없습니다");
                return;
            }

            try
            {
                clsDevexpressGrid.GridEndEdit(gridView_m);
                DataTable DT = (DataTable)gridControl_meddle.DataSource;

                mFocusRow = gridView_m.FocusedRowHandle;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                splashScreenManager.ShowWaitForm();

                string wk_divcode = gridView_l.GetFocusedRowCellValue("WK_DIVCODE").ToString().Trim();

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView_m);

                    if (string.IsNullOrWhiteSpace(rValid) && rValid != "")
                    {
                        gridView_m.FocusedColumn = gridView_m.Columns[rValid]; // 이동할 컬럼명
                        gridView_m.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(dr["COMM_CODE"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("중분류코드를 입력하여 주세요");
                        dr.SetColumnError("COMM_CODE", "중분류코드를 입력하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["COMM_NM"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("중분류명을 입력하여 주세요");
                        dr.SetColumnError("COMM_NM", "중분류명을 입력하여 주세요");
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = "SELECT * FROM COMM_CODE WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}'  ";
                        SQL = string.Format(SQL,
                            wk_divcode,
                            dr["COMM_CODE"].ToString()
                            );

                        DataSet dukDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(dukDs) == 1)
                        {
                            ShowMessageBox.XtraShowWarning("중분류코드가 중복됩니다.\r\n중복되지 않은 유일한 코드를 입력하여 주세요");
                            dr.RowError = "중분류코드가 중복됩니다.\r\n중복되지 않은 유일한 코드를 입력하여 주세요";
                            return;
                        }

                        string seq = "";
                        if (string.IsNullOrEmpty(dr["DISPLAY_SEQ"].ToString().Trim()))
                        {
                            SQL = "SELECT NVL(MAX(DISPLAY_SEQ),0) + 1 as DIS_SEQ FROM COMM_CODE WHERE WK_DIVCODE = '{0}' ";
                            SQL = String.Format(SQL, wk_divcode);

                            DataSet SEQ_DS = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(SEQ_DS) > 0)
                            {
                                seq = Dbconn.conn.getData(SEQ_DS, "DIS_SEQ", 0);
                            }
                        }
                        else
                        {
                            seq = dr["DISPLAY_SEQ"].ToString();
                        }

                        SQL = " INSERT INTO COMM_CODE(WK_DIVCODE, COMM_CODE, COMM_NM, DISPLAY_SEQ, REMARK) " +
                              " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}') ";
                        SQL = string.Format(SQL,
                            wk_divcode,
                            dr["COMM_CODE"].ToString(),
                            dr["COMM_NM"].ToString(),
                            seq,
                            dr["REMARK"].ToString()
                            );

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_m_save_Click", SQL);
                            dr.RowError = "데이터 입력에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = "UPDATE COMM_CODE SET COMM_NM = '{2}', DISPLAY_SEQ = '{3}', REMARK = '{4}' WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}' ";
                        SQL = string.Format(SQL,
                            wk_divcode,
                            dr["COMM_CODE"].ToString(),
                            dr["COMM_NM"].ToString(),
                            dr["DISPLAY_SEQ"].ToString(),
                            dr["REMARK"].ToString()
                        );

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_m_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();
                    gridView_m.RefreshData();
                    gridControl_small.DataSource = null;

                    comm_meddle_search(wk_divcode, true);

                    gridView_m.FocusedRowHandle = mFocusRow;
                    gridView_m.SelectRow(mFocusRow);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_m_save_Click", ex);
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
        #endregion

        #region 중분류 삭제버튼 클릭 이벤트
        private void btn_m_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView_m.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            if (clsDevexpressGrid.GridDeleteCheck(gridView_m) == false)
            {
                gridView_m.DeleteRow(gridView_m.FocusedRowHandle);
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 중분류코드 " + gridView_m.GetFocusedRowCellValue("COMM_CODE") + " 데이터를 삭제하시겠습니까?", "삭제하실 경우 중분류코드의 하위코드가 전부 삭제가 됩니다"))
            {
                return;
            }

            String wk_divcode = String.Empty;
            try
            {
                splashScreenManager.ShowWaitForm();

                Dbconn.conn.BeginTransaction();

                wk_divcode = gridView_m.GetFocusedRowCellValue("WK_DIVCODE").ToString();

                SQL = "DELETE FROM COMM_CODE WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}' ";
                SQL = String.Format(SQL, 
                    gridView_m.GetFocusedRowCellValue("WK_DIVCODE"),
                    gridView_m.GetFocusedRowCellValue("COMM_CODE")
                    );
                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btn_m_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다 -1");
                    return;
                }

                SQL = "DELETE FROM COMM_DTCODE WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}' ";
                SQL = String.Format(SQL, 
                    gridView_m.GetFocusedRowCellValue("WK_DIVCODE"),
                    gridView_m.GetFocusedRowCellValue("COMM_CODE")
                    );
                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btn_m_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다 -2");
                    return;
                }

                Dbconn.conn.Commit();

                gridControl_small.DataSource = null;

                comm_meddle_search(wk_divcode, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_m_delete_Click()", ex);
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

        private void gridView_m_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string selData_wdcode = gridView_m.GetFocusedRowCellValue("WK_DIVCODE")?.ToString();
            string selData_cmcode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_m, "COMM_CODE");

            if (!string.IsNullOrEmpty(selData_wdcode) && !string.IsNullOrEmpty(selData_cmcode))
            {
                comm_small_search(selData_wdcode, selData_cmcode);

                //gridView_s.ClearSelection();
                //gridView_s.FocusedRowHandle = 0;
                //gridView_s.SelectRow(0);
            }
            else
                gridControl_small.DataSource = null;
        }

        #region 소분류 조회이벤트
        private void comm_small_search(string wkcode, string cmcode, bool fixFocus = false)
        {
            try
            {
                SQL = $@"
                SELECT COMM_DTCODE, COMM_DTNM, COMM_CODE,REF_1, REF_2, WK_DIVCODE, DISPLAY_SEQ, USE_YN
                FROM COMM_DTCODE
                WHERE WK_DIVCODE = '{wkcode}' AND COMM_CODE = '{cmcode}'  ORDER BY DISPLAY_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl_small, gridView_s, ds.Tables[0], false, fixFocus);

                sValid = new string[] { "WK_DIVCODE", "COMM_CODE", "COMM_DTCODE" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = gridView_s.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                ds.Dispose();

                repItemLkUpEdit_s_use_yn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_s_use_yn, clsCommon.GetYn(), "", false, false);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "comm_small_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 소분류 재조회버튼 클릭이벤트
        private void btn_s_reflash_Click(object sender, EventArgs e)
        {
            int selCnt = clsDevexpressGrid.GetSelectRowCount(gridView_m);

            if (selCnt > 1)
            {
                ShowMessageBox.XtraShowInformation("중분류 항목선택 정보가 없습니다");
                return;
            }

            string selData_wdcode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_m, "WK_DIVCODE");
            string selData_cmcode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_m, "COMM_CODE");

            if (!string.IsNullOrEmpty(selData_wdcode) && !string.IsNullOrEmpty(selData_cmcode))
            {
                comm_small_search(selData_wdcode, selData_cmcode);
            }
        }



        #endregion

        #region 소분류 새행초기화 이벤트
        private void gridView_s_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                view.SetRowCellValue(e.RowHandle, view.Columns["WK_DIVCODE"], gridView_m.GetFocusedRowCellValue("WK_DIVCODE").ToString().Trim());
                view.SetRowCellValue(e.RowHandle, view.Columns["COMM_CODE"], gridView_m.GetFocusedRowCellValue("COMM_CODE").ToString().Trim());
                view.SetRowCellValue(e.RowHandle, view.Columns["DISPLAY_SEQ"], gridView_s.RowCount.ToString());
                view.SetRowCellValue(e.RowHandle, view.Columns["USE_YN"], "Y");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_s_InitNewRow", ex);
                ShowMessageBox.XtraShowWarning("새행을 초기화하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 소분류 행추가버튼 클릭이벤트
        private void btn_s_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView_m.SelectedRowsCount == 0)
            {
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView_s);
        }


        #endregion

        #region 소분류 행삭제버튼 클릭이벤트
        private void btn_s_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView_s);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView_s);
        }
        #endregion

        #region 소분류 저장버튼 클릭 이벤트
        private void btn_s_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("소분류 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            if (gridView_m.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("중분류 선택정보가 없습니다");
                return;
            }

            try
            {
                clsDevexpressGrid.GridEndEdit(gridView_s);

                DataTable DT = (DataTable)gridControl_small.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                splashScreenManager.ShowWaitForm();

                string wk_divcode = gridView_m.GetFocusedRowCellValue("WK_DIVCODE").ToString().Trim();
                string comm_code = gridView_m.GetFocusedRowCellValue("COMM_CODE").ToString().Trim();

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView_s);

                    if (string.IsNullOrWhiteSpace(rValid) && rValid != "")
                    {
                        gridView_s.FocusedColumn = gridView_s.Columns[rValid]; // 이동할 컬럼명
                        gridView_s.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(dr["COMM_DTCODE"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("소분류코드를 입력하여 주세요");
                        dr.SetColumnError("COMM_DTCODE", "소분류코드를 입력하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["COMM_DTNM"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("소분류명을 입력하여 주세요");
                        dr.SetColumnError("COMM_DTNM", "소분류명을 입력하여 주세요");
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = "SELECT * FROM COMM_DTCODE WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}' AND COMM_DTCODE = '{2}'  ";
                        SQL = string.Format(SQL,
                            dr["WK_DIVCODE"].ToString(),
                            dr["COMM_CODE"].ToString(),
                            dr["COMM_DTCODE"].ToString()
                            );

                        DataSet dukDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(dukDs) == 1)
                        {
                            ShowMessageBox.XtraShowWarning("소분류코드가 중복됩니다.\r\n중복되지 않은 유일한 코드를 입력하여 주세요");
                            dr.RowError = "소중분류코드가 중복됩니다.\r\n중복되지 않은 유일한 코드를 입력하여 주세요";
                            return;
                        }

                        string seq = "";
                        if (string.IsNullOrEmpty(dr["DISPLAY_SEQ"].ToString().Trim()))
                        {
                            SQL = "SELECT NVL(MAX(DISPLAY_SEQ),0) + 1 as DIS_SEQ FROM COMM_DTCODE WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}' ";
                            SQL = String.Format(SQL,
                                dr["WK_DIVCODE"].ToString(),
                                dr["COMM_CODE"].ToString()
                                );

                            DataSet SEQ_DS = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(SEQ_DS) > 0)
                            {
                                seq = Dbconn.conn.getData(SEQ_DS, "DIS_SEQ", 0);
                            }
                        }
                        else
                        {
                            seq = dr["DISPLAY_SEQ"].ToString();
                        }

                        SQL = $@"
                        INSERT INTO COMM_DTCODE (
                           WK_DIVCODE, COMM_CODE, COMM_DTCODE, 
                           COMM_DTNM, REF_1, REF_2, DISPLAY_SEQ, 
                           USE_YN, I_TIME) 
                        VALUES ( '{dr["WK_DIVCODE"]}', '{dr["COMM_CODE"]}', '{dr["COMM_DTCODE"]}', 
                           '{dr["COMM_DTNM"]}', '{dr["REF_1"]}', '{dr["REF_2"]}', '{dr["DISPLAY_SEQ"]}',
                           '{dr["USE_YN"]}', SYSDATE )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_s_save_Click", SQL);
                            dr.RowError = "데이터 입력에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = "UPDATE COMM_DTCODE SET COMM_DTNM = '{3}', REF_1 = '{4}', REF_2 = '{5}', DISPLAY_SEQ = '{6}', USE_YN = '{7}', I_TIME = SYSDATE WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}' AND COMM_DTCODE = '{2}' ";
                        SQL = string.Format(SQL,
                            dr["WK_DIVCODE"].ToString(),
                            dr["COMM_CODE"].ToString(),
                            dr["COMM_DTCODE"].ToString(),
                            dr["COMM_DTNM"].ToString(),
                            dr["REF_1"].ToString(),
                            dr["REF_2"].ToString(),
                            dr["DISPLAY_SEQ"].ToString(),
                            dr["USE_YN"].ToString()
                        );

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_s_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();
                    gridView_s.RefreshData();

                    comm_small_search(wk_divcode, comm_code, true);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_m_save_Click", ex);
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

        #endregion

        #region 소분류 삭제버튼 클릭 이벤트
        private void btn_s_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView_s.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            if (clsDevexpressGrid.GridDeleteCheck(gridView_s) == false)
            {
                gridView_s.DeleteRow(gridView_s.FocusedRowHandle);
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 소분류코드 " + gridView_s.GetFocusedRowCellValue("COMM_DTCODE") + " 데이터를 삭제하시겠습니까?"))
            {
                return;
            }

            string wk_divcode = string.Empty;
            string comm_code = string.Empty;

            try
            {
                splashScreenManager.ShowWaitForm();

                wk_divcode = gridView_s.GetFocusedRowCellValue("WK_DIVCODE").ToString();
                comm_code = gridView_s.GetFocusedRowCellValue("COMM_CODE").ToString();


                SQL = "DELETE FROM COMM_DTCODE WHERE WK_DIVCODE = '{0}' AND COMM_CODE = '{1}' AND COMM_DTCODE = '{2}' ";
                SQL = String.Format(SQL,
                    wk_divcode,
                    comm_code,
                    gridView_s.GetFocusedRowCellValue("COMM_DTCODE")
                    );
                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_s_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                comm_small_search(wk_divcode, comm_code, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_s_delete_Click()", ex);
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

        #region 그리드 로우넘버

        private void gridView_l_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridView_m_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridView_s_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }


        #endregion
    }
}