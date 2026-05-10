using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Employee : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_Employee()
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
                SELECT DISTINCT a.PLANT_CODE, a.EMPLOYEE_NO, '*********' as PASSWORD, a.NAME as EMPLOYEE_NAME , a.JUMIN_FIRST, a.JUMIN_LAST,
                    a.JUSO, a.TEL_NO, a.HPON_NO, a.MAIL, a.MANAGE_TYPE, a.I_TIME
                FROM DO_INSA a
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                ORDER BY a.I_TIME
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false,  true);

                sValid = new string[] { "PLANT_CODE", "EMPLOYEE_NO", "MANAGE_TYPE" };

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

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPlant_Code, clsCommon.GetPlant("", true));
                
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_MANAGE_TYPE, clsCommon.GetManageType());
                repItemLkUpEdit_MANAGE_TYPE.NullText = "관리타입을 선택해주세요";
                repItemLkUpEdit_MANAGE_TYPE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

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

        private void frm_Employee_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);
            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

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

                if (DialogResult.Yes != ShowMessageBox.Confirm("인사정보 데이터를 저장하시겠습니까?"))
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

                    if (string.IsNullOrEmpty(dr["EMPLOYEE_NO"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("사원코드를 선택하여 주세요\r\nERP 사원정보를 선택하도록 되어있습니다");
                        dr.SetColumnError("EMPLOYEE_NO", "사원코드를 선택하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["PASSWORD"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("패스워드를 입력하여 주세요");
                        dr.SetColumnError("PASSWORD", "패스워드를 입력하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["MANAGE_TYPE"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("관리타입을 선택하여 주세요");
                        dr.SetColumnError("MANAGE_TYPE", "관리타입을 선택하여 주세요");
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"SELECT * FROM DO_INSA WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND EMPLOYEE_NO = '{dr["EMPLOYEE_NO"]}' ";
                        
                        DataSet dukDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(dukDs) > 0)
                        {
                            ShowMessageBox.XtraShowWarning("사원정보가 중복됩니다.\r\n중복되지 않은 사원정보를 입력하여 주세요");
                            dr.RowError = "사원정보가 중복됩니다.\r\n중복되지 않은 사원정보를 입력하여 주세요";
                            return;
                        }

                        SQL = $@"
                        INSERT INTO DO_INSA(PLANT_CODE, EMPLOYEE_NO, NAME, PASSWORD, JUSO, TEL_NO , MANAGE_TYPE, I_TIME)
                        VALUES ('{dr["PLANT_CODE"]}', '{dr["EMPLOYEE_NO"]}', '{dr["EMPLOYEE_NAME"]}', '{clsEncryption.SHA256Hash(dr["PASSWORD"].ToString())}', '{dr["JUSO"]}', '{dr["TEL_NO"]}', '{dr["MANAGE_TYPE"]}', SYSDATE )
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
                        UPDATE DO_INSA
                        SET NAME= '{dr["EMPLOYEE_NAME"]}', JUSO = '{dr["JUSO"]}', TEL_NO = '{dr["TEL_NO"]}', MANAGE_TYPE = '{dr["MANAGE_TYPE"]}', I_TIME = SYSDATE
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND EMPLOYEE_NO = '{dr["EMPLOYEE_NO"]}'
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 인사데이터 " + gridView.GetFocusedRowCellValue("EMPLOYEE_NAME") + " 님을 삭제하시겠습니까?"))
            {
                return;
            }

            XMain_Delete();

        }

        private void XMain_Delete()
        {
            try
            {
                string emp_no = gridView.GetFocusedRowCellValue("EMPLOYEE_NO").ToString();

                if (emp_no == "AD0001")
                {
                    ShowMessageBox.XtraShowInformation("총괄관리자 계정은 삭제하실 수 없습니다");
                    return;
                }

                splashScreenManager.ShowWaitForm();

                SQL = $"DELETE FROM DO_INSA WHERE PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}' AND EMPLOYEE_NO = '{emp_no}' ";
                
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

        private void btn_passChange_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("비밀번호를 변경하실 사원정보를 클릭하여 주세요");
                return;
            }

            DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);
            if (row.RowState == DataRowState.Added)
            {
                ShowMessageBox.XtraShowInformation("사원정보를 저장하신 후에 비밀번호를 변경해주시길 바랍니다");
                return;
            }

            string emp_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "EMPLOYEE_NO");

            m_LoginPasswordChange mPassChange = new m_LoginPasswordChange(emp_no);
            mPassChange.StartPosition = FormStartPosition.CenterParent;
            DialogResult rslt = mPassChange.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                ShowMessageBox.XtraShowInformation("비밀번호가 변경되었습니다");
            }
        }

        private void gridView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(gridView.GetFocusedRowCellValue("I_TIME").ToString().Trim()))
                {
                    if (gridView.FocusedColumn.FieldName == "EMPLOYEE_NO" || gridView.FocusedColumn.FieldName == "PASSWORD")
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
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

        /// <summary>
        /// 비밀번호 초기화 "kfirst"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPWReset_Click(object sender, EventArgs e)
        {
            SQL = $@"
            UPDATE DO_INSA
            SET PASSWORD = '{clsEncryption.SHA256Hash("0000")}'
            WHERE PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
                AND EMPLOYEE_NO = '{gridView.GetFocusedRowCellValue("EMPLOYEE_NO")}'
            ";

            if (Dbconn.conn.SQLrun(SQL) < 0)
            {
                clsLog.logSave(this.Text, "btnPWReset_Click", SQL);
                ShowMessageBox.XtraShowWarning("패스워드 초기화에 실패 했습니다.");
                return;
            }

            ShowMessageBox.XtraShowInformation("패스워드가 0000 으로 변경 되었습니다. 패스워드 재변경 후 사용 바랍니다.");
        }
    }
}