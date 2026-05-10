using Core.Class;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_BOM_Create : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        public frm_BOM_Create()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewMain, Properties.Settings.Default.FontSize);
        }

        private void frm_BOM_Create_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboPresourceNo, clsCommon.GetPremix(clsCommon.PlantCode), "품목을 선택 해주세요.", false, true);

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboIdnrk, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("원재료")}', '{clsCommon.GetResourceTypeCode("부재료")}', '{clsCommon.GetResourceTypeCode("반제품")}', '{clsCommon.GetResourceTypeCode("제품")}', '{clsCommon.GetResourceTypeCode("상품")}', '{clsCommon.GetResourceTypeCode("비평가자재")}'", "", 0, true, false), "품목을 선택 해주세요.", false);

            //viewSub.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridMain));
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT P_TYPE
                     , PLANT_CODE
                     , RESOURCE_NO
                     , NOTE
                     , DATUV
                     , DATUV_TO
                     , BMENG
                     , STLST
                     , REMARK_1
                     , REMARK_2
                     , EMPLOYEE_NO
                     , I_TIME
                     , USE_YN
                FROM SAP_IN_BOM_CONM
                WHERE PLANT_CODE = '{clsCommon.PlantCode}'
                    AND (('{txtResource.EditValue}' IS NULL AND RESOURCE_NO LIKE 'F%') OR RESOURCE_NO LIKE 'F{txtResource.EditValue}%')
                    AND P_TYPE = '1'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], false, true);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "F", 0, true, false), "품목을 선택 해주세요.", false);

                ds.Dispose();

                XSub_Search(viewMain.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), viewMain.GetFocusedRowCellValue("RESOURCE_NO")?.ToString(), viewMain.GetFocusedRowCellValue("NOTE")?.ToString());
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XSub_Search(string sPlantCode, string sResourceNo, string sNote)
        {
            try
            {
                SQL = $@"
                SELECT P_TYPE
                    , PLANT_CODE
                    , NOTE
                    , RESOURCE_NO
                    , IDNRK
                    , STLAN
                    , STLNR
                    , STLAL
                    , BMEIN
                    , LKENZ
                    , POSNR
                    , MENGE
                    , P_NOTE
                    , MEINS
                    , AUSCH
                    , EWAHR
                    , SANFE
                    , SANKA
                    , DATUV
                    , DATUV_TO
                    , AENNR
                    , SEQ
                    , XSEQNR
                    , I_TIME
                FROM SAP_IN_BOM_COND
                WHERE PLANT_CODE = '{sPlantCode}'
                    AND RESOURCE_NO = '{sResourceNo}'
                    AND NOTE = '{sNote}'
                    AND P_TYPE = '1'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridSub, viewSub, ds.Tables[0], false, true);

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

            clsDevexpressGrid.GridViewAddRow(viewMain);
            viewMain.SetFocusedRowCellValue("PLANT_CODE", clsCommon.PlantCode);
        }
        #endregion

        #region 새행초기화 이벤트
        private void gridView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["MANAGE_TYPE"], "010603");
        }

        #endregion

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewMain);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewMain);
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
                clsDevexpressGrid.GridEndEdit(viewMain);

                DataTable DT = (DataTable)gridMain.DataSource;

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

                    string res = dr["RESOURCE_NO"].ToString().StartsWith("F") ? dr["RESOURCE_NO"].ToString() : "F" + dr["RESOURCE_NO"].ToString();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_IN_BOM_CONM /*TB01*/
                             ( P_TYPE          /* 01 */
                             , PLANT_CODE      /* 02 */
                             , RESOURCE_NO     /* 03 */
                             , NOTE            /* 04 */
                             , DATUV           /* 06 */
                             , DATUV_TO        /* 19 */
                             , I_TIME          /* 20 */
                             , USE_YN          /* 21 */
                             )
                        VALUES
                             ( '1'                      /* 01 */
                             , '{dr["PLANT_CODE"]}'     /* 02 */
                             , '{res}'                  /* 03 */
                             , '{dr["NOTE"]}'           /* 04 */
                             , '{dr["DATUV"]}'          /* 06 */
                             , '{dr["DATUV_TO"]}'       /* 19 */
                             , SYSDATE                  /* 20 — I_TIME 자동 */
                             , 'Y'         /* 21 */
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
                    //else if (dr.RowState == DataRowState.Modified)
                    //{
                    //    SQL = $@"
                    //    /* SAP_IN_BOM_CONM */
                    //    UPDATE SAP_IN_BOM_CONM 
                    //    SET BMENG       = '{dr["BMENG"]}'
                    //        , MIX_TIME    = '{dr["MIX_TIME"]}'
                    //        , DRY_TIME    = '{dr["DRY_TIME"]}'
                    //        , FINAL_TIME  = '{dr["FINAL_TIME"]}'
                    //        , LR_YN       = '{dr["LR_YN"]}'
                    //        , CR_YN       = '{dr["CR_YN"]}'
                    //        , MT_TIME     = '{dr["MT_TIME"]}'
                    //        , STLST       = '{dr["STLST"]}'
                    //        , REMARK_1    = '{dr["REMARK_1"]}'
                    //        , REMARK_2    = '{dr["REMARK_2"]}'
                    //        , EMPLOYEE_NO = '{dr["EMPLOYEE_NO"]}'
                    //        , H_YN        = '{dr["H_YN"]}'
                    //        , I_TIME      = SYSDATE
                    //        , USE_YN      = '{dr["USE_YN"]}'
                    //        , MIX_TIME2   = '{dr["MIX_TIME2"]}'
                    //    WHERE P_TYPE      = '{dr["P_TYPE"]}'
                    //    AND PLANT_CODE  = '{dr["PLANT_CODE"]}'
                    //    AND RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                    //    AND NOTE        = '{dr["NOTE"]}'
                    //    ";

                    //    if (Dbconn.conn.SQLrun(SQL) < 0)
                    //    {
                    //        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                    //        dr.RowError = "데이터 수정에 실패했습니다";
                    //        ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                    //        return;
                    //    }
                    //}
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

        private void SetCreatePremixProduct(string sPlantCode, string sResourceNo)
        {
            SQL = $@"
                -- 프리믹스 반제품 생성
                MERGE INTO SAP_DI_PRODUCT D
                USING (
                    SELECT 
                        PLANT_CODE, 'P' || RESOURCE_NO AS RESOURCE_NO, 'P)' || DESCRIPTION AS DESCRIPTION, 
                       UOM, 'HALB' AS RESOURCE_TYPE, '반제품' AS RESOURCE_TYPE_DESC, 
                       ZSPEC, P_TYPE, STD_LOT_SIZE, 
                       PACK_SIZE, ZDOMESTIC_IMPORT, LVORM, 
                       I_TIME, WERKS, HAND_YN, 
                       BU_P, PLANNER, SOURCE_DESC, 
                       HOME_LOCATION, COST_CENTER1, COST_CENTER3, 
                       COST_DIVISION, POU, BUYER, 
                       MANF_ENGR, INSPECTOR, MAT_MSTR, 
                       ITEM_GROUP_V, ITEM_GROUP_W, AP_CLOSE_TYPE, 
                       LABOR, MIX_TIME, MIX_TIME2, DRY_TIME, 
                       FINAL_TIME, LR_YN, CR_YN, 
                       MT_TIME, REMARK_1, REMARK_2, 
                       LABOR_T, P_TYPE_T, B_P
                    FROM SAP_DI_PRODUCT
                    WHERE PLANT_CODE = '{sPlantCode}' AND RESOURCE_NO = '{sResourceNo}'
                ) s
                ON (
                  d.PLANT_CODE  = s.PLANT_CODE AND d.RESOURCE_NO = s.RESOURCE_NO
                )
                WHEN NOT MATCHED THEN
                INSERT (
                    PLANT_CODE, RESOURCE_NO, DESCRIPTION
                  , UOM, RESOURCE_TYPE, RESOURCE_TYPE_DESC
                  , ZSPEC, P_TYPE, STD_LOT_SIZE
                  , PACK_SIZE, ZDOMESTIC_IMPORT, LVORM
                  , I_TIME, WERKS, HAND_YN
                  , BU_P, PLANNER, SOURCE_DESC
                  , HOME_LOCATION, COST_CENTER1, COST_CENTER3
                  , COST_DIVISION, POU, BUYER
                  , MANF_ENGR, INSPECTOR, MAT_MSTR
                  , ITEM_GROUP_V, ITEM_GROUP_W, AP_CLOSE_TYPE
                  , LABOR, MIX_TIME, MIX_TIME2, DRY_TIME
                  , FINAL_TIME, LR_YN, CR_YN
                  , MT_TIME, REMARK_1, REMARK_2
                  , LABOR_T, P_TYPE_T, B_P
                ) VALUES (
                    s.PLANT_CODE, s.RESOURCE_NO, s.DESCRIPTION
                  , s.UOM, s.RESOURCE_TYPE, s.RESOURCE_TYPE_DESC
                  , s.ZSPEC, s.P_TYPE, s.STD_LOT_SIZE
                  , s.PACK_SIZE, s.ZDOMESTIC_IMPORT, s.LVORM
                  , s.I_TIME, s.WERKS, s.HAND_YN
                  , s.BU_P, s.PLANNER, s.SOURCE_DESC
                  , s.HOME_LOCATION, s.COST_CENTER1, s.COST_CENTER3
                  , s.COST_DIVISION, s.POU, s.BUYER
                  , s.MANF_ENGR, s.INSPECTOR, s.MAT_MSTR
                  , s.ITEM_GROUP_V, s.ITEM_GROUP_W, s.AP_CLOSE_TYPE
                  , s.LABOR, s.MIX_TIME, s.MIX_TIME2, s.DRY_TIME
                  , s.FINAL_TIME, s.LR_YN, s.CR_YN
                  , s.MT_TIME, s.REMARK_1, s.REMARK_2
                  , s.LABOR_T, s.P_TYPE_T, s.B_P
                )
                ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "PreMixCreate", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                return;
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

            if (viewMain.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewMain);

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
                DELETE FROM SAP_IN_BOM_CONM 
                WHERE   PLANT_CODE    = '{viewMain.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND RESOURCE_NO   = '{viewMain.GetFocusedRowCellValue("RESOURCE_NO")}'
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

        private void viewSub_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void repItemLkUpEdit_EMPLOYEE_NO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            DataRow row = viewMain.GetDataRow(viewMain.FocusedRowHandle);

            if (row.RowState != DataRowState.Added)
            {
                e.Cancel = true;
                viewMain.CancelUpdateCurrentRow();
                ShowMessageBox.XtraShowInformation("입력된 사번정보는 수정하실 수 없습니다.\r\n사원정보를 삭제 후 추가해주세요");
            }

        }

        private void gridView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
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
                string[] editableColumns = new[] { "PLANT_CODE", "RESOURCE_NO", "PARTNER", "VEHICLENO" };

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
            gridMain.Focus();
            viewMain.FocusedRowHandle = 0;
            viewMain.FocusedColumn = viewMain.VisibleColumns[0];
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridMain.DataSource = null;
            XMain_Search();
        }

        private void txtResource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }

        private void btnSubAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewSub);
            viewSub.SetFocusedRowCellValue("PLANT_CODE", clsCommon.PlantCode);
            viewSub.SetFocusedRowCellValue("RESOURCE_NO", viewMain.GetFocusedRowCellValue("RESOURCE_NO"));
            viewSub.SetFocusedRowCellValue("NOTE", viewMain.GetFocusedRowCellValue("NOTE"));
        }

        private void btnSubDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewSub);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewSub);
        }

        private void btnSubSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != ShowMessageBox.Confirm("저장하시겠습니까?"))
            {
                return;
            }

            XSub_Save();
        }

        private void XSub_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewSub);

                DataTable DT = (DataTable)gridSub.DataSource;

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

                    if (dr["IDNRK"] == DBNull.Value)
                        continue;

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                         /*TB01*/
                        INSERT INTO SAP_IN_BOM_COND
                             ( P_TYPE       /* 01 */
                             , PLANT_CODE   /* 02 */
                             , NOTE         /* 03 */
                             , RESOURCE_NO  /* 04 */
                             , IDNRK        /* 05 */
                             , POSNR        /* 11 */
                             , MENGE        /* 12 */
                             , I_TIME       /* 31 */
                             )
                        VALUES
                             ( '1'        /* 01 */
                             , '{dr["PLANT_CODE"]}'    /* 02 */
                             , '{dr["NOTE"]}'          /* 03 */
                             , '{dr["RESOURCE_NO"]}'   /* 04 */
                             , '{dr["IDNRK"]}'         /* 05 */
                             , '{dr["POSNR"]}'         /* 11 */
                             , '{dr["MENGE"]}'         /* 12 */
                             , SYSDATE                 /* 31 — I_TIME 자동 */
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
                         UPDATE SAP_IN_BOM_COND /*TB01*/
                         SET MENGE    = '{dr["MENGE"]}'
                         WHERE P_TYPE     = '{dr["P_TYPE"]}'
                           AND PLANT_CODE = '{dr["PLANT_CODE"]}'
                           AND NOTE       = '{dr["NOTE"]}'
                           AND RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                           AND IDNRK      = '{dr["IDNRK"]}'
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

        private void btnSubDelete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewMain.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewMain);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 원료를 삭제하시겠습니까?"))
            {
                return;
            }

            XSub_Delete();

        }

        private void XSub_Delete()
        {
            try
            {
                splashScreenManager.ShowWaitForm();

                SQL = $@"
                DELETE FROM SAP_IN_BOM_COND  
                WHERE   PLANT_CODE    = '{viewSub.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND PRESOURCE_NO   = '{viewSub.GetFocusedRowCellValue("PRESOURCE_NO")}'
                    AND PIDNRK        = '{viewSub.GetFocusedRowCellValue("PIDNRK")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }
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

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                XSub_Search(viewMain.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), viewMain.GetFocusedRowCellValue("RESOURCE_NO")?.ToString(), viewMain.GetFocusedRowCellValue("NOTE")?.ToString());
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        private void viewMain_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            viewMain.UpdateCurrentRow();

            try
            {
                if (e.Column.FieldName == "RESOURCE_NO")  // 수량이 변경된 경우
                {
                    string sResourceNo = viewMain.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();

                    SQL = $@"
                    SELECT RESOURCE_NO
                    FROM SAP_DI_PRODUCT
                    WHERE PLANT_CODE = '{clsCommon.PlantCode}' AND RESOURCE_NO = 'P{sResourceNo}'
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        viewMain.SetFocusedRowCellValue("PRESOURCE_NO", $"P{sResourceNo}");
                        viewMain.RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {

                clsLog.logSave(this, "viewMain_CellValueChanged", ex);
                ShowMessageBox.XtraShowWarning("품목을 변경하는중 도중 에러가 발생했습니다");
            }
        }


    }
}