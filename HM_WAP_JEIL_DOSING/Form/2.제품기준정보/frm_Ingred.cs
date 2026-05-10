using Core.Class;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Ingred : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sMValid = null;
        private string[] sDValid = null;
        DataTable searchDt = null;

        string resourceNo = string.Empty;

        public frm_Ingred()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewIngred, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewBOM, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridIngred, viewIngred);
        }

        private void frm_Ingred_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridIngred, viewIngred);

            authDs = clsSql.GetAuthDataSet(this.Name);

            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridIngred));
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 원료 마스터
                SELECT DISTINCT
                    a.PLANT_CODE,
                    a.RESOURCE_NO,
                    b.DESCRIPTION,
                    a.H_ERROR,
                    a.L_ERROR,
                    a.SM_INPUT,
                    a.B_W,
                    a.MI_USE,
                    a.MR_USE,
                    a.USEYN,
                    a.M_INGRED_CODE,
                    a.PAY_YN,
                    a.GRI_YN,
                    a.ST_STOCK,
                    a.PR_DATE,
                    a.WEIGHT_TYPE,
                    a.I_TIME,
                    a.MANUALYN,
                    CASE WHEN c.RESOURCE_NO IS NULL THEN 'N' ELSE 'Y' END CON,
                    NVL(a.MS_GUBUN, 'M') AS MS_GUBUN
                FROM INGRED a
                    JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = b.RESOURCE_NO
                    LEFT JOIN SAP_IN_PRODUCT_CH c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = b.RESOURCE_NO
                WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND (a.RESOURCE_NO LIKE '%{txtIngred.EditValue}%' OR b.DESCRIPTION LIKE '%{txtIngred.EditValue}%')
                ORDER BY a.PLANT_CODE, a.RESOURCE_NO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridIngred, viewIngred, ds.Tables[0], true);
                searchDt = ds.Tables[0];

                sMValid = new string[] { "PLANT_CODE", "RESOURCE_NO", "DESCRIPTION" };


                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPlant_Code, clsCommon.GetPlant("", true));

                //gridcboRESOURCE_NO.NullText = "";
                //gridcboRESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //gridcboRESOURCE_NO.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSuggest;
                //gridcboRESOURCE_NO.CaseSensitiveSearch = true;
                //clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("원재료")}', '{clsCommon.GetResourceTypeCode("부재료")}', '{clsCommon.GetResourceTypeCode("반제품")}'"));
                //gridcboRESOURCE_NO.PopupFormMinSize = new Size(200, 300);

                //계량 유형
                gridcboMI_USE.NullText = "";
                gridcboMI_USE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboMI_USE, clsCommon.GetMI_USE(), "", false, false);

                gridcboUSEYN.NullText = "";
                gridcboUSEYN.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboUSEYN, clsCommon.GetYn(), "", false, false);

                // 계근방식
                gridcboWEIGHT_TYPE.NullText = "";
                gridcboWEIGHT_TYPE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboWEIGHT_TYPE, clsCommon.GetWeightType(), "", false, false);

                gridChk.ValueChecked = "Y";
                gridChk.ValueUnchecked = "N";
                gridChk.NullStyle = StyleIndeterminate.Unchecked;
                gridChk.CheckStyle = CheckStyles.Standard;

                gridCboMS_Gubun.NullText = "";
                gridCboMS_Gubun.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboMS_Gubun, clsCommon.GetIngredType(), "", false, false);

                XPartSearch(resourceNo);
                XBomSearch(resourceNo);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XPartSearch(string resourceNo)
        {
            try
            {
                SQL = $@"
                SELECT PLANT_CODE, RESOURCE_NO, RESOURCE_NO_2, PART_P, 
                   EMPLOYEE_NO
                FROM SAP_IN_PRODUCT_CH
                WHERE RESOURCE_NO = '{resourceNo}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridPart, viewPart, ds.Tables[0], true);

                sDValid = new string[] { "PLANT_CODE", "RESOURCE_NO", "RESOURCE_NO_2", "PART_P" };


                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("원재료")}', '{clsCommon.GetResourceTypeCode("부재료")}', '{clsCommon.GetResourceTypeCode("반제품")}', '{clsCommon.GetResourceTypeCode("부산물")}'", "", 0, true), "품목을 선택 해주세요.");

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XPartSearch", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XBomSearch(string sIdnrk)
        {
            try
            {
                SQL = $@"
                SELECT DISTINCT a.PLANT_CODE, a.RESOURCE_NO, c.DESCRIPTION AS RESOURCE_NAME, a.NOTE, a.DATUV, a.DATUV_TO
                    , b.IDNRK, D.DESCRIPTION AS IDNRK_NAME, b.MENGE
                FROM (SELECT DISTINCT PLANT_CODE, RESOURCE_NO, NOTE, P_TYPE AS BOM_TYPE, I_TIME
                                , DATUV, DATUV_TO
                                , ROW_NUMBER() OVER(PARTITION BY RESOURCE_NO ORDER BY I_TIME DESC) AS rn
                                FROM SAP_IN_BOM_CONM
                                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                                AND P_TYPE = '2' AND STLST = '2'
                                ORDER BY I_TIME DESC) a
                    JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE AND b.P_TYPE = a.BOM_TYPE
                    JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                    JOIN SAP_DI_PRODUCT D ON d.PLANT_CODE = b.PLANT_CODE AND d.RESOURCE_NO = b.IDNRK
                WHERE a.rn = '1' AND b.IDNRK = '{sIdnrk}'
                ORDER BY a.PLANT_CODE, a.RESOURCE_NO, b.IDNRK
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridBOM, viewBOM, ds.Tables[0], true);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XPartSearch", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewIngred);

                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("원료정보 데이터를 저장하시겠습니까?"))
                {
                    return;
                }

                DataTable DT = (DataTable)gridIngred.DataSource;

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

                var topRowIndex = viewIngred.TopRowIndex;
                var focusedRowHandle = viewIngred.FocusedRowHandle;

                Dbconn.conn.BeginTransaction();

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sMValid, dr, viewIngred);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewIngred.FocusedColumn = viewIngred.Columns[rValid]; // 이동할 컬럼명
                        viewIngred.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_DI_PRODUCT (
                           PLANT_CODE, RESOURCE_NO, DESCRIPTION, RESOURCE_TYPE) 
                        VALUES ( 
                           '{dr["PLANT_CODE"]}', '{dr["RESOURCE_NO"]}', '{dr["DESCRIPTION"]}', '{clsCommon.GetResourceTypeCode("원재료")}')
                        ";

                        Dbconn.conn.SQLrun(SQL);

                        SQL = $@"
                        INSERT INTO INGRED (
                             PLANT_CODE      -- 01. 공장코드
                            , RESOURCE_NO    -- 02. 자재코드
                            , H_ERROR        -- 03. 상한오차
                            , L_ERROR        -- 04. 하한오차
                            , SM_INPUT       -- 05. 소량투입여부
                            , B_W            -- 06. 단위
                            , MI_USE         -- 07. 주배합 사용여부
                            , MR_USE         -- 08. 보충배합 사용여부
                            , USEYN          -- 09. 사용여부
                            , M_INGRED_CODE  -- 10. 기준자재코드
                            , PAY_YN         -- 11. 유상여부
                            , GRI_YN         -- 12. 계량여부
                            , ST_STOCK       -- 13. 기준재고
                            , PR_DATE        -- 14. 기준일자
                            , WEIGHT_TYPE    -- 15. 중량형태
                            , MANUALYN       --
                            , MS_GUBUN
                            , I_TIME         -- 16. 입력일시
                        ) VALUES (
                             '{dr["PLANT_CODE"]}'
                            , '{dr["RESOURCE_NO"]}'
                            , '{dr["H_ERROR"]}'
                            , '{dr["L_ERROR"]}'
                            , '{dr["SM_INPUT"]}'
                            , '{dr["B_W"]}'
                            , '{dr["MI_USE"]}'
                            , '{dr["MR_USE"]}'
                            , '{dr["USEYN"]}'
                            , '{dr["M_INGRED_CODE"]}'
                            , '{dr["PAY_YN"]}'
                            , '{dr["GRI_YN"]}'
                            , '{dr["ST_STOCK"]}'
                            , '{dr["PR_DATE"]}'
                            , '{dr["WEIGHT_TYPE"]}'
                            , '{dr["MANUALYN"]}'
                            , '{dr["MS_GUBUN"]}'
                            , SYSDATE
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(INGRED)데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE SAP_DI_PRODUCT
                        SET   DESCRIPTION = '{dr["DESCRIPTION"]}'
                        WHERE  PLANT_CODE    = '{dr["PLANT_CODE"]}'
                        AND    RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(SAP_DI_PRODUCT)데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE INGRED
                        SET    H_ERROR       = '{dr["H_ERROR"]}'
                                , L_ERROR       = '{dr["L_ERROR"]}'
                                , SM_INPUT      = '{dr["SM_INPUT"]}'
                                , B_W           = '{dr["B_W"]}'
                                , MI_USE        = '{dr["MI_USE"]}'
                                , MR_USE        = '{dr["MR_USE"]}'
                                , USEYN         = '{dr["USEYN"]}'
                                , M_INGRED_CODE = '{dr["M_INGRED_CODE"]}'
                                , PAY_YN        = '{dr["PAY_YN"]}'
                                , GRI_YN        = '{dr["GRI_YN"]}'
                                , ST_STOCK      = '{dr["ST_STOCK"]}'
                                , PR_DATE       = '{dr["PR_DATE"]}'
                                , WEIGHT_TYPE   = '{dr["WEIGHT_TYPE"]}'
                                , MANUALYN      = '{dr["MANUALYN"]}'
                                , MS_GUBUN      = '{dr["MS_GUBUN"]}'
                                , I_TIME        = SYSDATE
                        WHERE  PLANT_CODE    = '{dr["PLANT_CODE"]}'
                        AND    RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();
                    viewIngred.RefreshData();
                }

                Dbconn.conn.Commit();

                XMain_Search();
                viewIngred.FocusedRowHandle = focusedRowHandle;
                viewIngred.TopRowIndex = topRowIndex;

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

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                clsDevexpressGrid.GridViewLastAddRowDelete(viewIngred);
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewIngred);
            viewIngred.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
            viewIngred.SetRowCellValue(viewIngred.FocusedRowHandle, viewIngred.Columns["USEYN"], "Y");
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewIngred);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewIngred);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                DataRow row = viewIngred.GetDataRow(viewIngred.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewIngred.DeleteRow(viewIngred.FocusedRowHandle);
                }
                else
                {
                    string sRESOURCE_NO = clsDevexpressGrid.GetFocusedRowCellValue(viewIngred, "RESOURCE_NO");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 원료를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {

                        Dbconn.conn.BeginTransaction();

                        SQL = $"DELETE FROM SAP_DI_PRODUCT WHERE PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}' AND RESOURCE_NO = '{sRESOURCE_NO}'";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("원료 삭제에 실패했습니다");
                            return;
                        }

                        SQL = $"DELETE FROM INGRED WHERE PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}' AND RESOURCE_NO = '{sRESOURCE_NO}'";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("원료 삭제에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();

                        XMain_Search();
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
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
            gridIngred.Focus();
            viewIngred.FocusedRowHandle = 0;
            viewIngred.FocusedColumn = viewIngred.VisibleColumns[0];
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridPart.DataSource = null;

                resourceNo = clsDevexpressGrid.GetFocusedRowCellValue(viewIngred, "RESOURCE_NO");

                XPartSearch(resourceNo);
                XBomSearch(resourceNo);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        private void btn_reflash1_Click(object sender, EventArgs e)
        {

        }

        private void btn_rowAdd1_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(viewPart);

            viewPart.SetRowCellValue(viewPart.FocusedRowHandle, viewPart.Columns["PLANT_CODE"], clsDevexpressGrid.GetFocusedRowCellValue(viewIngred, "PLANT_CODE"));
            viewPart.SetRowCellValue(viewPart.FocusedRowHandle, viewPart.Columns["RESOURCE_NO"], resourceNo);
        }

        private void btn_rowDel1_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPart);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewPart);
        }

        private void btn_save1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(viewPart);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridPart.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sDValid, dr, viewPart);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewPart.FocusedColumn = viewPart.Columns[rValid]; // 이동할 컬럼명
                        viewPart.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_IN_PRODUCT_CH (
                           PLANT_CODE, RESOURCE_NO, RESOURCE_NO_2, PART_P, 
                           EMPLOYEE_NO, I_TIME) 
                        VALUES ( 
                            '{dr["PLANT_CODE"]}', '{dr["RESOURCE_NO"]}', '{dr["RESOURCE_NO_2"]}', '{dr["PART_P"]}',
                            '{dr["EMPLOYEE_NO"]}', SYSDATE )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_Save_Part_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE SAP_IN_PRODUCT_CH
                        SET    PART_P        = '{dr["PART_P"]}',
                               EMPLOYEE_NO   = '{dr["EMPLOYEE_NO"]}',
                               I_TIME        = SYSDATE
                        WHERE  PLANT_CODE = '{dr["PLANT_CODE"]}'
                            AND RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                            AND RESOURCE_NO_2 = '{dr["RESOURCE_NO_2"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_Save_Part_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.Confirm("대체 비율을 저장 했습니다");

                XPartSearch(resourceNo);
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_Save_Part_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (viewPart.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 대체 항목을 선택하여 주세요");
                    return;
                }
                DataRow row = viewPart.GetDataRow(viewPart.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewPart.DeleteRow(viewPart.FocusedRowHandle);
                }
                else
                {
                    DialogResult result = ShowMessageBox.Confirm("선택하신 대체 항목을 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        Dbconn.conn.BeginTransaction();

                        // Delete from SHIFT_DETAIL table
                        SQL = $@"
                        DELETE FROM SAP_IN_PRODUCT_CH
                        WHERE RESOURCE_NO = '{resourceNo}' 
                            AND RESOURCE_NO_2 = '{viewPart.GetFocusedRowCellValue("RESOURCE_NO_2")}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("대체비율 삭제에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();

                        ShowMessageBox.Confirm("선택하신 대체 비율을 삭제 했습니다");
                    }

                    XPartSearch(resourceNo);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        private void viewIngred_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (viewIngred == null)
                    return;

                string con = viewIngred.GetRowCellValue(e.RowHandle, viewIngred.Columns["CON"]).ToString();

                if (con == "Y")
                {
                    e.Appearance.BackColor = Color.LightCyan;
                    e.Appearance.ForeColor = Color.DarkGreen;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            //clsDevexpressUtil.ItemSearchLookUpEditSetup(scboINGRED, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), $"'{clsCommon.GetResourceTypeCode("원재료")}', '{clsCommon.GetResourceTypeCode("부재료")}'"), "원료를 선택 해주세요.");

            XMain_Search();

        }

        private void scboINGRED_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void viewPart_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewPart.GetFocusedRowCellValue("RESOURCE_NO_2")?.ToString() == "")
            {
                return;
            }

            // 기존 행에서 특정 컬럼만 편집 허용 (예: "Quantity" 컬럼만 수정 가능)
            if (viewPart.FocusedColumn.FieldName != "PART_P")
            {
                e.Cancel = true; // 편집 막기
            }
        }

        private void txtIngred_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }
    }
}