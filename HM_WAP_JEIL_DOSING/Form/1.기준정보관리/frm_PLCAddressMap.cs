using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_PLCAddressMap : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_PLCAddressMap()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- PLC ADDRESS 관리
                SELECT PLC_ID, PLC_LOCATION, ADDRESS_ID, 
                   ADDRESS_SEQ, DEVICE_TYPE, ADDRESS, 
                   ADDRESS_COUNT, ADDRESS_UNIT, DATA_TYPE, 
                   RW_FLAG, DESCRIPTION, USE_YN, 
                   I_TIME, I_USER
                FROM PLC_ADDRESS_MAP
                ORDER BY PLC_ID, PLC_LOCATION, ADDRESS_ID, ADDRESS_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false,  true);

                sValid = new string[] { "" };


                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboPlant, clsCommon.GetPlant("", true));

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "", false, false);

                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }
        #endregion

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_PLCAddressMap_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            clsDevexpressUtil.ItemLookUpEditSetup(cboYN, clsCommon.GetYn(), "", false, 1, true);

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        #region 행추가버튼 클릭이벤트
        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }


            clsDevexpressGrid.GridViewAddRow(gridView);
            gridView.SetFocusedRowCellValue("USE_FLAG", "Y");
        }
        #endregion

        #region 새행초기화 이벤트
        private void gridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["MANAGE_TYPE"], "010603");
        }

        #endregion

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        #region 저장버튼 클릭이벤트
        private void btn_save_Click(object sender, EventArgs e)
        {
            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);

                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("저장하시겠습니까?"))
                {
                    return;
                }

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
                        INSERT INTO PLC_ADDRESS_MAP (
                             PLC_ID           -- 1
                           , PLC_LOCATION     -- 2
                           , ADDRESS_ID       -- 3
                           , ADDRESS_SEQ      -- 4
                           , DEVICE_TYPE      -- 5
                           , ADDRESS          -- 6
                           , ADDRESS_COUNT    -- 7
                           , ADDRESS_UNIT     -- 8
                           , DATA_TYPE        -- 9
                           , RW_FLAG          -- 10
                           , DESCRIPTION      -- 11
                           , USE_YN           -- 12
                           , I_TIME           -- 13
                           , I_USER           -- 14
                        )
                        VALUES (
                             '{dr["PLC_ID"]}'           -- 1
                           , '{dr["PLC_LOCATION"]}'     -- 2
                           , '{dr["ADDRESS_ID"]}'       -- 3
                           , '{dr["ADDRESS_SEQ"]}'      -- 4
                           , '{dr["DEVICE_TYPE"]}'      -- 5
                           , '{dr["ADDRESS"]}'          -- 6
                           , '{dr["ADDRESS_COUNT"]}'    -- 7
                           , '{dr["ADDRESS_UNIT"]}'     -- 8
                           , '{dr["DATA_TYPE"]}'        -- 9
                           , '{dr["RW_FLAG"]}'          -- 10
                           , '{dr["DESCRIPTION"]}'      -- 11
                           , '{dr["USE_YN"]}'           -- 12
                           , SYSDATE                    -- 13
                           , '{dr["I_USER"]}'           -- 14
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
                        UPDATE PLC_ADDRESS_MAP
                        SET    DEVICE_TYPE      = '{dr["DEVICE_TYPE"]}'      -- 1
                             , ADDRESS          = '{dr["ADDRESS"]}'          -- 2
                             , ADDRESS_COUNT    = '{dr["ADDRESS_COUNT"]}'    -- 3
                             , ADDRESS_UNIT     = '{dr["ADDRESS_UNIT"]}'     -- 4
                             , DATA_TYPE        = '{dr["DATA_TYPE"]}'        -- 5
                             , RW_FLAG          = '{dr["RW_FLAG"]}'          -- 6
                             , DESCRIPTION      = '{dr["DESCRIPTION"]}'      -- 7
                             , USE_YN           = '{dr["USE_YN"]}'           -- 8
                             , I_TIME           = SYSDATE                    -- 9
                             , I_USER           = '{dr["I_USER"]}'           -- 10
                        WHERE  PLC_ID           = '{dr["PLC_ID"]}'
                        AND    PLC_LOCATION     = '{dr["PLC_LOCATION"]}'
                        AND    ADDRESS_ID       = '{dr["ADDRESS_ID"]}'
                        AND    ADDRESS_SEQ      = '{dr["ADDRESS_SEQ"]}'
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

                XMain_Search();

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
        #endregion

        #region 삭제버튼 클릭이벤트
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 저장소를 삭제하시겠습니까?"))
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

                SQL = $@"
                DELETE FROM SAP_DI_LOCATION
                WHERE PLANT_CODE         = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}' 
                   AND LOCATION          = '{gridView.GetFocusedRowCellValue("LOCATION")}' 
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowInformation("저장소를 삭제 했습니다.");

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

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void repItemLkUpEdit_EMPLOYEE_NO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);

            if (row.RowState != DataRowState.Added)
            {
                e.Cancel = true;
                gridView.CancelUpdateCurrentRow();
                ShowMessageBox.XtraShowInformation("입력된 사번정보는 수정하실 수 없습니다.\r\n사원정보를 삭제 후 추가해주세요");
            }

        }

        private void gridView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void gridView_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (e.FocusedColumn == null)
                return;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
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
            gridControl.DataSource = null;

            XMain_Search();
        }

        private void cboYN_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}