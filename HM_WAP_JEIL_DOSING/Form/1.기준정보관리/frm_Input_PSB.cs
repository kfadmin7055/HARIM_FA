using Core.Class;
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
    public partial class frm_Input_PSB : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_Input_PSB()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Input_PSB_Load(object sender, EventArgs e)
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

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 성분표
                SELECT 
                   a.PLANT_CODE, a.RESOURCE_NO, b.DESCRIPTION, a.RESOURCE_NO || ' : ' || b.DESCRIPTION AS RESOURCE_DESC, a.FIELD_FLAG, 
                   a.FIELD_NAME, a.SEQ_NO, a.FIELD_VALUE01, 
                   a.FIELD_VALUE02, a.ERDAT, a.ERZET, 
                   a.AEDAT, a.AEZET, a.TRANS_DATE
                FROM SAP_IN_PLA a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND (b.DESCRIPTION LIKE '%{txtResource.EditValue}%' OR a.RESOURCE_NO LIKE '%{txtResource.EditValue}%')
                ORDER BY a.PLANT_CODE, a.RESOURCE_NO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false,  true);

                gridView.Columns["PLANT_CODE"].Fixed = FixedStyle.Left;
                gridView.Columns["RESOURCE_DESC"].Fixed = FixedStyle.Left;

                sValid = new string[] { "PLANT_CODE", "RESOURCE_DESC", "FIELD_FLAG", "FIELD_NAME", "SEQ_NO" };

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

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPLANT_CODE, clsCommon.GetPlant("", true));

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboFIELD_FLAG, clsCommon.GetFieldFlag());

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

            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);
            gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
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
                        INSERT INTO SAP_IN_PLA (
                           PLANT_CODE, RESOURCE_NO, FIELD_FLAG, 
                           FIELD_NAME, SEQ_NO, FIELD_VALUE01, 
                           FIELD_VALUE02, ERDAT, ERZET, 
                           AEDAT, AEZET, TRANS_DATE) 
                        VALUES ( '{dr["PLANT_CODE"]}'
                           , '{dr["RESOURCE_NO"]}'
                           , '{dr["FIELD_FLAG"]}'
                           , '{dr["FIELD_NAME"]}'
                           , '{dr["SEQ_NO"]}'
                           , '{dr["FIELD_VALUE01"]}'
                           , '{dr["FIELD_VALUE02"]}'
                           , '{dr["ERDAT"]}'
                           , '{dr["ERZET"]}'
                           , '{dr["AEDAT"]}'
                           , '{dr["AEZET"]}'
                           , '{dr["TRANS_DATE"]}' )
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
                        UPDATE SAP_IN_PLA
                        SET    FIELD_VALUE01 = '{dr["FIELD_VALUE01"]}',
                               FIELD_VALUE02 = '{dr["FIELD_VALUE02"]}',
                               ERDAT         = '{dr["ERDAT"]}',
                               ERZET         = '{dr["ERZET"]}',
                               AEDAT         = '{dr["AEDAT"]}',
                               AEZET         = '{dr["AEZET"]}',
                               TRANS_DATE    = '{dr["TRANS_DATE"]}'
                        WHERE  PLANT_CODE    = '{dr["PLANT_CODE"]}'
                            AND    RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                            AND    FIELD_FLAG    = '{dr["FIELD_FLAG"]}'
                            AND    FIELD_NAME    = '{dr["FIELD_NAME"]}'
                            AND    SEQ_NO        = '{dr["SEQ_NO"]}'
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 성분표를 삭제하시겠습니까?"))
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
                DELETE FROM SAP_IN_PLA 
                WHERE   PLANT_CODE    = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND RESOURCE_NO   = '{gridView.GetFocusedRowCellValue("RESOURCE_NO")}'
                    AND FIELD_FLAG    = '{gridView.GetFocusedRowCellValue("FIELD_FLAG")}'
                    AND FIELD_NAME    = '{gridView.GetFocusedRowCellValue("FIELD_NAME")}'
                    AND SEQ_NO        = '{gridView.GetFocusedRowCellValue("SEQ_NO")}'
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
                string[] editableColumns = new[] { "PLANT_CODE", "RESOURCE_NO", "FIELD_FLAG", "FIELD_NAME", "SEQ_NO" };

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