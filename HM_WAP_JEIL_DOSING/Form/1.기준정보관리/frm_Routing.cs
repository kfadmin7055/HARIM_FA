using Core.Class;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Routing : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_Routing()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void frm_Routing_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);
            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            XMain_Search();

            InitControl();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void InitControl()
        {
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboProcessKey, clsCommon.GetProcess(clsCommon.PlantCode));

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 0, true, false), "품목을 선택 해주세요.", false, null, "CODE", "NAME");

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboYn, clsCommon.GetYn(null, new string[] { "자동", "수동" }));
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 라우팅 마스터
                SELECT  a.ARBPL        -- 1 작업장
                     ,  a.I_TIME       -- 2 입력시간
                     ,  CASE WHEN a.INPUT_MODE = 'Y' THEN '자동' ELSE '수동' END INPUT_MODE    -- 3 입력모드(자동/수동 등)
                     ,  a.LTXA1        -- 4 작업 설명
                     ,  a.PLANT_CODE   -- 5 공장코드
                     ,  a.RESOURCE_NO  -- 6 자원번호
                     ,  a.VORNR        -- 7 공정순서
                FROM   SAP_DI_ROUTING a  -- TB01 라우팅 테이블
                    JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                    AND (a.RESOURCE_NO LIKE '%{txtResource.EditValue}%' OR b.DESCRIPTION LIKE '%{txtResource.EditValue}%')
                ORDER BY a.RESOURCE_NO, a.VORNR, a.ARBPL
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false,  true);

                ds.Dispose();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }
        #endregion

        #region 행추가버튼 클릭이벤트
        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);
            gridView.SetFocusedRowCellValue("PLANT_CODE", clsCommon.PlantCode);
            gridView.SetFocusedRowCellValue("INPUT_MODE", "N");

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
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("저장하시겠습니까?"))
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

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                splashScreenManager.ShowWaitForm();

                foreach (DataRow dr in DT.Rows)
                {
                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_DI_ROUTING  -- TB01 라우팅 테이블
                        (
                            ARBPL        -- 1 작업장
                          , I_TIME       -- 2 입력시간
                          , INPUT_MODE   -- 3 입력모드(자동/수동 등)
                          , LTXA1        -- 4 작업 설명
                          , PLANT_CODE   -- 5 공장코드
                          , RESOURCE_NO  -- 6 자원번호
                          , VORNR        -- 7 공정순서
                        )
                        VALUES (
                            '{dr["ARBPL"]}'
                          , SYSDATE
                          , '{dr["INPUT_MODE"]}'
                          , '{dr["LTXA1"]}'
                          , '{dr["PLANT_CODE"]}'
                          , '{dr["RESOURCE_NO"]}'
                          , '{dr["VORNR"]}'
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 품목을 삭제하시겠습니까?"))
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
                DELETE FROM SAP_DI_ROUTING 
                WHERE   PLANT_CODE    = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND RESOURCE_NO   = '{gridView.GetFocusedRowCellValue("RESOURCE_NO")}'
                    AND VORNR   = '{gridView.GetFocusedRowCellValue("VORNR")}'
                ";

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

                // 신규 입력 중인 행인지 확인
                string[] editableColumns = new[] { "PLANT_CODE", "RESOURCE_NO", "ARBPL", "VORNR" };

                if (dr.RowState != DataRowState.Added && editableColumns.Contains(fieldName))
                {
                    if (!view.IsNewItemRow(rowHandle))
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }
        }

        private void gridView_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (e.FocusedColumn == null)
                return;
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

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;
            XMain_Search();
        }

        private void txtResource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }
    }
}