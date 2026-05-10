using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
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
    public partial class frm_EqTrouble : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_EqTrouble()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }


        private void XMain_Search()
        {
            try
            {

                SQL = $@"
                SELECT 
                    SEQ, 
                    WORKDATE, 
                    EQ_CODE, 
                    EQ_NAME, 
                    EQ_TRB_MSG, 
                    EQ_TRB_TASK_MSG, 
                    REMARK, 
                    IN_GUBUN, 
                    I_TIME, 
                    I_USER
                FROM 
                    EQ_TROUBLE
                WHERE 
                    TRUNC(WORKDATE) = TO_DATE('{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', 'YYYYMMDD')
                ORDER BY 
                    SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false);

                sValid = new string[] { "WORKDATE", "EQ_TRB_MSG" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = gridView.Columns[sValid[i]];
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
                clsLog.logSave(this, "silo_XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_EqTrouble_Load(object sender, EventArgs e)
        {
            try
            {
                dateEdit_workDate.EditValue = DateTime.Today;

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_EqTrouble_Load", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
/*                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }
*/
                clsDevexpressGrid.GridViewAddRow(gridView);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["DATE"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["I_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["GUBUN"], "수기입력");
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                /*                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                                {
                                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                                    return;
                                }*/

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
                        SQL = $"DELETE FROM EQ_TROUBLE WHERE SEQ = '{seq}'  ";

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

                ShowMessageBox.XtraShowInformation("삭제 되었습니다.");

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                Dbconn.conn.Rollback();
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != ShowMessageBox.Confirm("설비오류내역 정보를 저장하시겠습니까?"))
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

                    string sWorkDate = string.Empty;

                    if (!string.IsNullOrEmpty(dr["WORKDATE"].ToString()))
                    {
                        DateTime dtFrom = DateTime.Parse(dr["WORKDATE"].ToString());

                        sWorkDate = $"TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO EQ_TROUBLE (
                           SEQ, WORKDATE, EQ_CODE, 
                           EQ_NAME, EQ_TRB_MSG, EQ_TRB_TASK_MSG, 
                           REMARK, IN_GUBUN, I_TIME, 
                           I_USER) 
                        VALUES (
                            (SELECT NVL(MAX(SEQ) + 1, 1) FROM EQ_TROUBLE)
                          , ({(string.IsNullOrEmpty(sWorkDate) ? "''" : $"{sWorkDate}")})
                          , '{dr["EQ_CODE"]}'
                          , '{dr["EQ_NAME"]}'
                          , '{dr["EQ_TRB_MSG"]}'
                          , '{dr["EQ_TRB_TASK_MSG"]}'
                          , '{dr["REMARK"]}'
                          , '수기입력'
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
                        /*                        if (!clsCommon.Auth_Form_Function(authDs, "W"))
                                                {
                                                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                                                    return;
                                                }*/

                        SQL = $@"
                        UPDATE EQ_TROUBLE
                        SET    SEQ             = '{dr["SEQ"]}',
                               WORKDATE        = ({(string.IsNullOrEmpty(sWorkDate) ? "''" : $"{sWorkDate}")}),
                               EQ_CODE         = '{dr["EQ_CODE"]}',
                               EQ_NAME         = '{dr["EQ_NAME"]}',
                               EQ_TRB_MSG      = '{dr["EQ_TRB_MSG"]}',
                               EQ_TRB_TASK_MSG = '{dr["EQ_TRB_TASK_MSG"]}',
                               REMARK          = '{dr["REMARK"]}',
                               IN_GUBUN        = '{dr["IN_GUBUN"]}',
                               I_TIME          = SYSDATE,
                               I_USER          = '{clsCommon._strUserId}'
                        WHERE  SEQ             = '{dr["SEQ"]}'
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

                ShowMessageBox.XtraShowInformation("저장 되었습니다.");

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
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

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;
            DataRow dr = null;

            // 현재 행의 RowHandle 가져오기
            int rowHandle = view.FocusedRowHandle;
            string fieldName = view.FocusedColumn.FieldName;

            // DataRowView로부터 DataRow 얻기
            DataRowView drv = view.GetRow(rowHandle) as DataRowView;

            if (drv != null)
            {
                dr = drv.Row;

                if (dr.RowState != DataRowState.Added && sValid.Contains(fieldName))
                {
                    if (!view.IsNewItemRow(rowHandle))
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }
        }
    }
}