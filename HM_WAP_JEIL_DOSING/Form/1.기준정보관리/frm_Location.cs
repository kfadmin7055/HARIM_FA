using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Location : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_Location()
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
                -- 저장소 마스터
                SELECT 
                   PLANT_CODE, LOCATION, DESCRIPTION, PARENT_SKEY, 
                   PARENT_DESCRIPTION, STOCK_TYPE, SPEC_TYPE, 
                   USE_FLAG
                FROM SAP_DI_LOCATION
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND ('{txtDESCRIPTION.EditValue}' IS NULL OR DESCRIPTION LIKE '%{txtDESCRIPTION.EditValue}%')
                    AND ('{txtPARENT_DESCRIPTION.EditValue}' IS NULL OR PARENT_DESCRIPTION LIKE '%{txtPARENT_DESCRIPTION.EditValue}%')
                    AND ('{cboYN.EditValue}' IS NULL OR USE_FLAG = '{cboYN.EditValue}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false,  true);

                sValid = new string[] { "LOCATION", "PLANT_CODE" };

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

        private void frm_Location_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

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

            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);
            gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
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
                        INSERT INTO SAP_DI_LOCATION (
                           PLANT_CODE, LOCATION, DESCRIPTION, PARENT_SKEY, 
                           PARENT_DESCRIPTION, STOCK_TYPE, SPEC_TYPE, 
                           USE_FLAG) 
                        VALUES ( 
                           '{dr["PLANT_CODE"]}'
                         , '{dr["LOCATION"]}'
                         , '{dr["DESCRIPTION"]}'
                         , '{dr["PARENT_SKEY"]}'
                         , '{dr["PARENT_DESCRIPTION"]}'
                         , '{dr["STOCK_TYPE"]}'
                         , '{dr["SPEC_TYPE"]}'
                         , '{dr["USE_FLAG"]}' )
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
                        UPDATE SAP_DI_LOCATION
                        SET    LOCATION           = '{dr["LOCATION"]}',
                               DESCRIPTION        = '{dr["DESCRIPTION"]}',
                               PARENT_SKEY        = '{dr["PARENT_SKEY"]}',
                               PARENT_DESCRIPTION = '{dr["PARENT_DESCRIPTION"]}',
                               STOCK_TYPE         = '{dr["STOCK_TYPE"]}',
                               SPEC_TYPE          = '{dr["SPEC_TYPE"]}',
                               USE_FLAG           = '{dr["USE_FLAG"]}'
                        WHERE  PLANT_CODE         = '{dr["PLANT_CODE"]}'
                           AND LOCATION           = '{dr["LOCATION"]}'
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