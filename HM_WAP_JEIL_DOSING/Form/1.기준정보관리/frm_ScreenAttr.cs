using Core.Class;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HARIM_FA_DOSING
{
    public partial class frm_ScreenAttr : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_ScreenAttr()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewManageType, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewAttr, Properties.Settings.Default.FontSize);

            viewAttr.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Default;
            viewAttr.OptionsBehavior.Editable = true;
        }

        #region 관리타입 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = "SELECT COMM_DTCODE, COMM_DTNM FROM COMM_DTCODE WHERE COMM_CODE = '06' AND WK_DIVCODE = '01' ORDER BY DISPLAY_SEQ ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridManageType, viewManageType, ds.Tables[0], false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void groupControl_mList_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            XMain_Search();
        }

        #region 화면권한 조회함수
        private void attr_search(string type)
        {
            try
            {
                SQL = $@"
                SELECT a.PROGRAM_ID, a.MENU_ID, a.MENU_NM
                    , b.SCR_ID, b.SCR_NM, b.USE_YN
                    , NVL(c.READ_ATT, 'N') AS READ_ATT, NVL(c.WRITE_ATT, 'N') AS WRITE_ATT
                    , NVL(c.DELETE_ATT, 'N') AS DELETE_ATT, NVL(c.UPDATE_ATT, 'N') AS UPDATE_ATT
                    , c.I_TIME
                FROM MENU_MG a
                    LEFT OUTER JOIN SCR_MG b ON a.PROGRAM_ID = b.PROGRAM_ID AND a.MENU_ID = b.MENU_ID AND b.USE_YN = 'Y'
                    LEFT OUTER JOIN SC_ATTRIBUTION c ON c.MANAGE_TYPE = '{type}'
                            AND a.PROGRAM_ID = c.PROGRAM_ID AND a.MENU_ID = c.MENU_ID AND b.SCR_ID = c.SCR_ID
                ORDER BY a.DISPLAY_SEQ, b.DISPLAY_SEQ 
                ";
                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridAttr, viewAttr, ds.Tables[0], false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "attr_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void frm_ScreenAttr_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);
            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridAttr));
        }

        private void gridView_manageType_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            string selData_manageType = clsDevexpressGrid.GetFocusedRowCellValue(viewManageType, "COMM_DTCODE");
            if (!string.IsNullOrEmpty(selData_manageType))
            {
                attr_search(selData_manageType);
            }
        }

        private void groupControl_Attr_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("화면권한 정보를 이대로 저장하시겠습니까?"))
            {
                return;
            }

            try
            {
                clsDevexpressGrid.GridEndEdit(viewAttr);

                DataTable DT = (DataTable)gridAttr.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                string selData_manageType = clsDevexpressGrid.GetFocusedRowCellValue(viewManageType, "COMM_DTCODE");

                if (string.IsNullOrEmpty(selData_manageType))
                {
                    ShowMessageBox.XtraShowInformation("수정하실 관리타입을 선택하여 주세요");
                }

                splashScreenManager.ShowWaitForm();

                foreach (DataRow dr in DT.Rows)
                {
                    //string rValid = clsCommon.ValdationCheck(sValid, dr, viewManageType);

                    //if (!string.IsNullOrWhiteSpace(rValid))
                    //{
                    //    viewManageType.FocusedColumn = viewManageType.Columns[rValid]; // 이동할 컬럼명
                    //    viewManageType.ShowEditor(); // 편집 모드 진입 (선택)
                    //    Dbconn.conn.Rollback();
                    //    return;
                    //}

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = "SELECT * FROM SC_ATTRIBUTION WHERE PROGRAM_ID = '{0}' AND MENU_ID = '{1}' AND SCR_ID = '{2}' AND MANAGE_TYPE = '{3}' ";
                        SQL = string.Format(SQL,
                            dr["PROGRAM_ID"].ToString(),
                            dr["MENU_ID"].ToString(),
                            dr["SCR_ID"].ToString(),
                            selData_manageType
                            );

                        DataSet chkAttrDurDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(chkAttrDurDs) == 0)
                        {
                            SQL = "INSERT INTO SC_ATTRIBUTION (PROGRAM_ID, MENU_ID, SCR_ID, MANAGE_TYPE, READ_ATT, WRITE_ATT, DELETE_ATT, UPDATE_ATT, I_TIME) " +
                                " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',SYSDATE) ";
                            SQL = string.Format(SQL,
                                dr["PROGRAM_ID"].ToString(),
                                dr["MENU_ID"].ToString(),
                                dr["SCR_ID"].ToString(),
                                selData_manageType,
                                dr["READ_ATT"].ToString(),
                                dr["WRITE_ATT"].ToString(),
                                dr["DELETE_ATT"].ToString(),
                                dr["UPDATE_ATT"].ToString()
                            );

                        }
                        else if (Dbconn.conn.getRowCnt(chkAttrDurDs) == 1)
                        {
                            SQL = "UPDATE SC_ATTRIBUTION SET READ_ATT = '{4}', WRITE_ATT = '{5}', DELETE_ATT = '{6}', UPDATE_ATT = '{7}', I_TIME = SYSDATE  WHERE PROGRAM_ID = '{0}' AND MENU_ID = '{1}' AND SCR_ID = '{2}' AND MANAGE_TYPE = '{3}' ";
                            SQL = string.Format(SQL,
                                dr["PROGRAM_ID"].ToString(),
                                dr["MENU_ID"].ToString(),
                                dr["SCR_ID"].ToString(),
                                selData_manageType,
                                dr["READ_ATT"].ToString(),
                                dr["WRITE_ATT"].ToString(),
                                dr["DELETE_ATT"].ToString(),
                                dr["UPDATE_ATT"].ToString()
                            );
                        }

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "groupControl_Attr_CustomButtonClick", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning(dr.RowError);
                            return;
                        }
                    }
                    dr.AcceptChanges();
                    viewAttr.RefreshData();
                }


                if (!string.IsNullOrEmpty(selData_manageType))
                {
                    attr_search(selData_manageType);
                }

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

        private void viewManageType_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                //InitEventControl();
                string menuNM = clsDevexpressGrid.GetFocusedRowCellValue(viewManageType, "COMM_DTCODE");

                if (!string.IsNullOrEmpty(menuNM))
                {
                    attr_search(menuNM);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "viewManageType_FocusedRowChanged", ex);
            }
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
            }

            //// 신규 행 추가
            //if (e.KeyCode == Keys.F3)
            //{
            //    btn_rowAdd_Click(sender, e);
            //}

            //// 행 삭제
            //if (e.KeyCode == Keys.Delete)
            //{
            //    btn_rowDel_Click(sender, e);
            //}

            //// 저장
            //if (e.Control && e.KeyCode == Keys.S)
            //{
            //    XMain_Save();
            //}

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridAttr.Focus();
            viewAttr.FocusedRowHandle = 0;
            viewAttr.FocusedColumn = viewAttr.VisibleColumns[0];
        }

        private void viewAttr_MouseDown(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);

            // 헤더 클릭 && 대상 컬럼이 CHK일 때
            if (hitInfo.InColumn && hitInfo.Column.FieldName == "READ_ATT"
                || hitInfo.InColumn && hitInfo.Column.FieldName == "WRITE_ATT"
                || hitInfo.InColumn && hitInfo.Column.FieldName == "DELETE_ATT"
                || hitInfo.InColumn && hitInfo.Column.FieldName == "UPDATE_ATT")
            {
                string sFieldName = hitInfo.Column.FieldName;

                // 현재 체크 상태 확인
                bool foundUnchecked = false;

                for (int i = 0; i < view.RowCount; i++)
                {
                    object val = view.GetRowCellValue(i, sFieldName);

                    view.SetRowCellValue(i, sFieldName, val.ToString() == "N" ? "Y" : "N");
                }

                // 강제로 헤더 다시 그림
                view.InvalidateColumnHeader(view.Columns["CHK"]);
            }
        }
    }
}