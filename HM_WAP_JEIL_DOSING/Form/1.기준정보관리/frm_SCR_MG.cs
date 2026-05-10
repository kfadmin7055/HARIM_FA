using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_SCR_MG : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_SCR_MG()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT PROGRAM_ID, MENU_ID, SCR_ID
                    , SCR_NM, PROGRAM_DESC, FORM_NAME
                    , USE_YN, DISPLAY_SEQ
                FROM SCR_MG
                WHERE ('{cboMenu.EditValue}' IS NULL OR MENU_ID LIKE '%{cboMenu.EditValue}%')
                    AND ('{txtSCR_NM.EditValue}' IS NULL OR SCR_NM LIKE '%{txtSCR_NM.EditValue}%')
                ORDER BY PROGRAM_ID, MENU_ID, DISPLAY_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                sValid = new string[] { "PROGRAM_ID", "MENU_ID", "SCR_ID" };

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

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboProgram, clsCommon.GetProgram());

                SQL = $"SELECT MENU_ID AS CODE, MENU_NM AS NAME FROM MENU_MG";
                ds = Dbconn.conn.ExecutDataset(SQL);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboMenu, ds.Tables[0]);

                gridcboCHK.ValueChecked = "Y";
                gridcboCHK.ValueUnchecked = "N";
                gridcboCHK.NullStyle = StyleIndeterminate.Unchecked;
                gridcboCHK.CheckStyle = CheckStyles.Standard;

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_SCR_MG_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            SQL = $"SELECT MENU_ID AS CODE, MENU_NM AS NAME FROM MENU_MG";
            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            clsDevexpressUtil.ItemLookUpEditSetup(cboMenu, ds.Tables[0], "", false, 0, true);

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        #region 버튼 이벤트
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            gridView.AddNewRow();
            int newRowHandle = gridView.FocusedRowHandle;

            gridView.SetRowCellValue(newRowHandle, gridView.Columns["PROGRAM_ID"], "P001");
            gridView.SetRowCellValue(newRowHandle, gridView.Columns["USE_YN"], "Y");
            gridView.SetRowCellValue(newRowHandle, gridView.Columns["I_TIME"], DateTime.Now);

            gridView.ShowEditor();
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridControl.DataSource;

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

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SCR_MG (
                           PROGRAM_ID, MENU_ID, SCR_ID
                           , SCR_NM, PROGRAM_DESC, FORM_NAME
                           , USE_YN, DISPLAY_SEQ) 
                        VALUES ( 
                           '{dr["PROGRAM_ID"]}'
                         , '{dr["MENU_ID"]}'
                         , '{dr["SCR_ID"]}'
                         , '{dr["SCR_NM"]}'
                         , '{dr["PROGRAM_DESC"]}'
                         , '{dr["FORM_NAME"]}'
                         , '{dr["USE_YN"]}'
                         , '{dr["DISPLAY_SEQ"]}' )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE SCR_MG
                        SET    PROGRAM_ID   = '{dr["PROGRAM_ID"]}'
                            , MENU_ID       = '{dr["MENU_ID"]}'
                            , SCR_ID        = '{dr["SCR_ID"]}'
                            , SCR_NM        = '{dr["SCR_NM"]}'
                            , PROGRAM_DESC  = '{dr["PROGRAM_DESC"]}'
                            , FORM_NAME     = '{dr["FORM_NAME"]}'
                            , USE_YN        = '{dr["USE_YN"]}'
                            , DISPLAY_SEQ   = '{dr["DISPLAY_SEQ"]}'
                        WHERE  PROGRAM_ID   = '{dr["PROGRAM_ID"]}'
                        AND    MENU_ID      = '{dr["MENU_ID"]}'
                        AND    SCR_ID       = '{dr["SCR_ID"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowWarning("메뉴를 저장 했습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(gridView);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 메뉴를 삭제하시겠습니까?"))
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

                SQL = $"DELETE FROM SCR_MG WHERE PROGRAM_ID = '{gridView.GetFocusedRowCellValue("PROGRAM_ID")}' AND MENU_ID = '{gridView.GetFocusedRowCellValue("MENU_ID")}' AND SCR_ID = '{gridView.GetFocusedRowCellValue("SCR_ID")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

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