using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Security.AccessControl;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using GridView = DevExpress.XtraGrid.Views.Grid.GridView;

namespace HARIM_FA_DOSING
{
    public partial class frm_Product : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        string resourceNo = string.Empty;
        string suGubun = string.Empty;
        private string[] sValid = null;
        private string[] sPatValid = null;
        private string[] sUpCPValid = null;
        private string[] sResValid = null;
        private bool isInitializing = false;

        public frm_Product()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewProduct, Properties.Settings.Default.FontSize);
        }

        private void frm_Ingred_Load(object sender, EventArgs e)
        {

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridProduct, viewProduct);

            viewProduct.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            authDs = clsSql.GetAuthDataSet(this.Name);

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            InitContorl();

            XMain_Search();

            isInitializing = true;

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridProduct));
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridProduct, viewProduct);
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                string sAllProdutct = chkAllProduct.Checked == true ? "" : "@";

                SQL = $@"
                -- 품목 마스터
                SELECT DISTINCT
                     a.PLANT_CODE, a.RESOURCE_NO, a.DESCRIPTION, a.UOM
                   , a.RESOURCE_TYPE, a.RESOURCE_TYPE_DESC
                   , a.ZSPEC, a.P_TYPE, NVL(a.STD_LOT_SIZE, (d.UMREN / d.UMREZ)) AS STD_LOT_SIZE
                   , NVL(a.PACK_SIZE, d.MEINH) AS PACK_SIZE, a.ZDOMESTIC_IMPORT, a.LVORM
                   , a.HAND_YN, a.I_TIME, CASE WHEN a.SU_CODE IS NOT NULL THEN TO_CHAR(a.SU_CODE || ' : ' || f.DESCRIPTION) ELSE '' END SU_CODE
                   , a.PLANNER, a.SOURCE_DESC, a.HOME_LOCATION, a.COST_CENTER1, a.COST_CENTER3
                   , a.COST_DIVISION, a.POU, a.BUYER, a.MANF_ENGR, a.INSPECTOR, a.MAT_MSTR
                   , a.ITEM_GROUP_V, a.ITEM_GROUP_W, a.AP_CLOSE_TYPE
                   , a.LABOR, a.MIX_TIME, a.MIX_TIME2, a.DRY_TIME, a.FINAL_TIME, a.LR_YN, a.CR_YN, a.MT_TIME, a.REMARK_1, a.REMARK_2
                   , CASE WHEN a.B_P = 'X' THEN '브랜드' ELSE '일반' END B_P_DESC
                   , a.BU_P, a.SMORDERYN, a.DEL_YN, a.BI_YN 
                   , CASE TO_CHAR(a.JD_GUBUN) WHEN 'E' THEN '생산'
                                            WHEN 'F' THEN '출하' 
                                    END JD_GUBUN_DESC
                   , CASE WHEN b.RESOURCE_NO IS NULL THEN 'N' ELSE 'Y' END CP
                   , CASE WHEN c.RESOURCE_NO IS NULL THEN 'N' ELSE 'Y' END RC
                   , CASE WHEN e.RESOURCE_NO IS NULL THEN 'N' ELSE 'Y' END UPCP
                   , NVL(a.USE_YN, 'Y') AS USE_YN
                FROM SAP_DI_PRODUCT a
                    LEFT JOIN SAP_IN_PRODUCT_CP b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                    LEFT JOIN SAP_IN_PRODUCT_RC c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                    LEFT JOIN SAP_IN_UPPRODUCT_CP e ON e.PLANT_CODE = a.PLANT_CODE AND e.RESOURCE_NO = a.RESOURCE_NO
                    LEFT JOIN SAP_MARM d ON d.MATNR = a.RESOURCE_NO AND d.MEINH = 'KG'
                    LEFT JOIN SAP_DI_PRODUCT f ON f.PLANT_CODE = a.PLANT_CODE AND f.RESOURCE_NO = a.SU_CODE
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND (a.RESOURCE_NO LIKE '%{txtResource.EditValue}%' OR a.DESCRIPTION LIKE '%{txtResource.EditValue}%')
                    {suGubun}
                    AND ('{sAllProdutct}' IS NULL OR (a.JD_GUBUN = 'E' OR (a.RESOURCE_NO IN (SELECT RESOURCE_NO FROM SAP_IN_BOM_CONM WHERE PLANT_CODE = a.PLANT_CODE))))
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_YN, 'N') = '{cboDelYn.EditValue}')
                ORDER BY a.PLANT_CODE, a.RESOURCE_TYPE, a.DESCRIPTION
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridProduct, viewProduct, ds.Tables[0], true);

                sValid = new string[] { "PLANT_CODE", "RESOURCE_NO" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = viewProduct.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                ds.Dispose();

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboPlant, clsCommon.GetPlant(), "", false, false);

                // 제품유형
                gridCboRESOURCE_TYPE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboRESOURCE_TYPE, clsCommon.GetResourceType(), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboP_TYPE, clsCommon.GetProductType(), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboLABOR, clsCommon.GetLabor(), "", false, false);

                // 부산물 마스터
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboBU_P, clsCommon.GetResource(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("부산물")}'"));

                // 단위
                repoCombo_Unit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(repoCombo_Unit, clsCommon.GetUnit(), "", false, false);

                // 저장위치
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboHOME_LOCATION, clsCommon.GetLocation(cboPlant_Code.EditValue?.ToString()));

                // 수동기입여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboHAND_YN, clsCommon.GetYn(null, new string[] { "자동", "수동" }));

                // 사용안함
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboUse, clsCommon.GetYn(null, new string[] { "사용", "사용안함" }), "N", false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboYn, clsCommon.GetYn(null, new string[] { "Y", "N" }), "", false, false);

                XPartSearch();
                XUpCPSearch();
                XResSearch();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XPartSearch()
        {
            try
            {
                SQL = $@"
                SELECT PLANT_CODE, RESOURCE_NO, RESOURCE_NO_2, RESOURCE_NO_3, PART_P, 
                   EMPLOYEE_NO
                FROM SAP_IN_PRODUCT_CP
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}' AND RESOURCE_NO = '{viewProduct.GetFocusedRowCellValue(RESOURCE_NO)}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridPart, viewPart, ds.Tables[0], true);

                sPatValid = new string[] { "PLANT_CODE", "RESOURCE_NO", "RESOURCE_NO_2", "RESOURCE_NO_3", "PART_P" };

                /*
                    BYPD	부산물
                    HALB	반제품
                    ROH	원재료
                    ROH1	부재료
                    UNBW	비평가자재

                */

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("부산물")}', '{clsCommon.GetResourceTypeCode("반제품")}', '{clsCommon.GetResourceTypeCode("원재료")}', '{clsCommon.GetResourceTypeCode("부재료")}', '{clsCommon.GetResourceTypeCode("비평가자재")}'", "", 0, true), "품목을 선택 해주세요.");

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XPartSearch", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XUpCPSearch()
        {
            try
            {
                SQL = $@"
                SELECT 
                PLANT_CODE, RESOURCE_NO, RESOURCE_NO_2, 
                   PART_P, SAP_UP_YN, EMPLOYEE_NO, I_TIME
                FROM SAP_IN_UPPRODUCT_CP
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}' AND RESOURCE_NO = '{viewProduct.GetFocusedRowCellValue(RESOURCE_NO)}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridUpCP, viewUpCP, ds.Tables[0], true);

                sUpCPValid = new string[] { "RESOURCE_NO", "RESOURCE_NO_2", "PART_P" };

                gridUpCPCheck.ValueChecked = "Y";
                gridUpCPCheck.ValueUnchecked = "N";
                gridUpCPCheck.NullStyle = StyleIndeterminate.Unchecked;
                gridUpCPCheck.CheckStyle = CheckStyles.Standard;

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridUpCPScboResourceNo, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", "", 0, true), "품목을 선택 해주세요.");

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XResSearch", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XResSearch()
        {
            try
            {
                SQL = $@"
                SELECT PLANT_CODE,
                RESOURCE_NO, RESOURCE_NO_2, PART_P
                   , SAP_UP_YN, EMPLOYEE_NO, I_TIME
                FROM SAP_IN_PRODUCT_RC
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}' AND RESOURCE_NO = '{viewProduct.GetFocusedRowCellValue(RESOURCE_NO)}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridResidue, viewResidue, ds.Tables[0], true);

                sResValid = new string[] { "RESOURCE_NO", "RESOURCE_NO_2", "PART_P" };

                for (int i = 0; i < sResValid.Length; i++)
                {
                    GridColumn col = viewResidue.Columns[sResValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                gridRcCheck.ValueChecked = "Y";
                gridRcCheck.ValueUnchecked = "N";
                gridRcCheck.NullStyle = StyleIndeterminate.Unchecked;
                gridRcCheck.CheckStyle = CheckStyles.Standard;

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO_2, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", "", 0, true), "품목을 선택 해주세요.");

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XResSearch", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void InitContorl()
        {
            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] {"삭제", "미삭제"}), "전체선택", false, 2, true);

            gridChk.ValueChecked = "Y";
            gridChk.ValueUnchecked = "N";
            gridChk.NullStyle = StyleIndeterminate.Unchecked;
            gridChk.CheckStyle = CheckStyles.Standard;

            //clsDevexpressUtil.ItemSearchLookUpEditSetup(scboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()), "품목을 선택 해주세요.");
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "RECEIVING")
            {
                int iReceiving = Convert.ToInt32(viewProduct.GetRowCellValue(e.RowHandle, viewProduct.Columns["RECEIVING"]));
                int iStock = Convert.ToInt32(viewProduct.GetRowCellValue(e.RowHandle, viewProduct.Columns["STOCK"]));
                int iSum = 0;

                if (iReceiving > 0)
                {
                    iSum = iReceiving + iStock;
                }

                viewProduct.SetRowCellValue(e.RowHandle, viewProduct.Columns["STOCK"], iSum);
            }
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                resourceNo = clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "RESOURCE_NO");

                string cp = clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "CP");
                string rc = clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "RC");
                string upcp = clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "UPCP");

                gridPart.DataSource = null;
                gridUpCP.DataSource = null;
                gridResidue.DataSource = null;

                XPartSearch();
                XUpCPSearch();
                XResSearch();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        #region Product 버튼 이벤트
        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                clsDevexpressGrid.GridViewLastAddRowDelete(viewProduct);
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewProduct);
            viewProduct.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
            viewProduct.SetFocusedRowCellValue("HAND_YN", "N");
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewProduct);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewProduct);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != ShowMessageBox.Confirm("원료정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            try
            {
                XMain_Save();
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

        private void XMain_Save()
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewProduct);
            DataTable DT = (DataTable)gridProduct.DataSource;

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

            var topRowIndex = viewProduct.TopRowIndex;
            var focusedRowHandle = viewProduct.FocusedRowHandle;

            foreach (DataRow dr in DT.Rows)
            {
                string rValid = clsCommon.ValdationCheck(sValid, dr, viewProduct);

                if (!string.IsNullOrWhiteSpace(rValid))
                {
                    viewProduct.FocusedColumn = viewProduct.Columns[rValid]; // 이동할 컬럼명
                    viewProduct.ShowEditor(); // 편집 모드 진입 (선택)
                    Dbconn.conn.Rollback();
                    return;
                }

                dr.ClearErrors();

                if (dr.RowState == DataRowState.Added)
                {
                    SQL = $@"
                    INSERT INTO SAP_DI_PRODUCT (
                       PLANT_CODE
                     , RESOURCE_NO
                     , DESCRIPTION
                     , UOM
                     , RESOURCE_TYPE
                     , RESOURCE_TYPE_DESC
                     , ZSPEC
                     , P_TYPE
                     , STD_LOT_SIZE
                     , PACK_SIZE
                     , BU_P
                     , SMORDERYN
                     , ZDOMESTIC_IMPORT
                     , LVORM
                     , I_TIME
                     , HAND_YN
                     , PLANNER
                     , SOURCE_DESC
                     , HOME_LOCATION
                     , COST_CENTER1
                     , COST_CENTER3
                     , COST_DIVISION
                     , POU
                     , BUYER
                     , MANF_ENGR
                     , INSPECTOR
                     , MAT_MSTR
                     , ITEM_GROUP_V
                     , ITEM_GROUP_W
                     , AP_CLOSE_TYPE
                     , LABOR
                     , MIX_TIME
                     , MIX_TIME2
                     , DRY_TIME
                     , FINAL_TIME
                     , LR_YN
                     , CR_YN
                     , MT_TIME
                     , REMARK_1
                     , REMARK_2
                     , JD_GUBUN
                     , BI_YN
                     , USE_YN
                    ) 
                    VALUES (
                       '{dr["PLANT_CODE"]}'
                     , '{dr["RESOURCE_NO"]}'
                     , '{dr["DESCRIPTION"]}'
                     , '{dr["UOM"]}'
                     , '{dr["RESOURCE_TYPE"]}'
                     , '{dr["RESOURCE_TYPE_DESC"]}'
                     , '{dr["ZSPEC"]}'
                     , '{dr["P_TYPE"]}'
                     , '{dr["STD_LOT_SIZE"]}'
                     , '{dr["PACK_SIZE"]}'
                     , '{dr["BU_P"]}'
                     , '{dr["SMORDERYN"]}'
                     , '{dr["ZDOMESTIC_IMPORT"]}'
                     , '{dr["LVORM"]}'
                     , SYSDATE
                     , '{dr["HAND_YN"]}'
                     , '{dr["PLANNER"]}'
                     , '{dr["SOURCE_DESC"]}'
                     , '{dr["HOME_LOCATION"]}'
                     , '{dr["COST_CENTER1"]}'
                     , '{dr["COST_CENTER3"]}'
                     , '{dr["COST_DIVISION"]}'
                     , '{dr["POU"]}'
                     , '{dr["BUYER"]}'
                     , '{dr["MANF_ENGR"]}'
                     , '{dr["INSPECTOR"]}'
                     , '{dr["MAT_MSTR"]}'
                     , '{dr["ITEM_GROUP_V"]}'
                     , '{dr["ITEM_GROUP_W"]}'
                     , '{dr["AP_CLOSE_TYPE"]}'
                     , '{dr["LABOR"]}'
                     , '{dr["MIX_TIME"]}'
                     , '{dr["MIX_TIME2"]}'
                     , '{dr["DRY_TIME"]}'
                     , '{dr["FINAL_TIME"]}'
                     , '{dr["LR_YN"]}'
                     , '{dr["CR_YN"]}'
                     , '{dr["MT_TIME"]}'
                     , '{dr["REMARK_1"]}'
                     , '{dr["REMARK_2"]}'
                     , 'E'
                     , '{dr["BI_YN"]}'
                     , '{dr["USE_YN"]}'
                    )
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("(SAP_DI_PRODUCT)데이터 입력에 실패했습니다");
                        return;
                    }

                    if (dr["RESOURCE_TYPE"].ToString() == clsCommon.GetResourceTypeCode("원재료") || dr["RESOURCE_TYPE"].ToString() == clsCommon.GetResourceTypeCode("부재료"))
                    {
                        SQL = $@"
                        INSERT INTO INGRED (
                            PLANT_CODE, RESOURCE_NO, I_TIME) 
                        VALUES ('{dr["PLANT_CODE"]}', '{dr["RESOURCE_NO"]}', SYSDATE)
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(INGRED)데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }
                else if (dr.RowState == DataRowState.Modified)
                {
                    SQL = $@"
                    UPDATE SAP_DI_PRODUCT
                    SET    DESCRIPTION        = '{dr["DESCRIPTION"]}'
                         , UOM                = '{dr["UOM"]}'
                         , RESOURCE_TYPE      = '{dr["RESOURCE_TYPE"]}'
                         , RESOURCE_TYPE_DESC = '{dr["RESOURCE_TYPE_DESC"]}'
                         , ZSPEC              = '{dr["ZSPEC"]}'
                         , P_TYPE             = '{dr["P_TYPE"]}'
                         , BU_P               = '{dr["BU_P"]}'
                         , SMORDERYN          = '{dr["SMORDERYN"]}'
                         , STD_LOT_SIZE       = '{dr["STD_LOT_SIZE"]}'
                         , PACK_SIZE          = '{dr["PACK_SIZE"]}'
                         , ZDOMESTIC_IMPORT   = '{dr["ZDOMESTIC_IMPORT"]}'
                         , LVORM              = '{dr["LVORM"]}'
                         , I_TIME             = SYSDATE
                         , HAND_YN            = '{dr["HAND_YN"]}'
                         , PLANNER            = '{dr["PLANNER"]}'
                         , SOURCE_DESC        = '{dr["SOURCE_DESC"]}'
                         , HOME_LOCATION      = '{dr["HOME_LOCATION"]}'
                         , COST_CENTER1       = '{dr["COST_CENTER1"]}'
                         , COST_CENTER3       = '{dr["COST_CENTER3"]}'
                         , COST_DIVISION      = '{dr["COST_DIVISION"]}'
                         , POU                = '{dr["POU"]}'
                         , BUYER              = '{dr["BUYER"]}'
                         , MANF_ENGR          = '{dr["MANF_ENGR"]}'
                         , INSPECTOR          = '{dr["INSPECTOR"]}'
                         , MAT_MSTR           = '{dr["MAT_MSTR"]}'
                         , ITEM_GROUP_V       = '{dr["ITEM_GROUP_V"]}'
                         , ITEM_GROUP_W       = '{dr["ITEM_GROUP_W"]}'
                         , AP_CLOSE_TYPE      = '{dr["AP_CLOSE_TYPE"]}'
                         , LABOR              = '{dr["LABOR"]}'
                         , MIX_TIME           = '{dr["MIX_TIME"]}'
                         , MIX_TIME2           = '{dr["MIX_TIME2"]}'
                         , DRY_TIME           = '{dr["DRY_TIME"]}'
                         , FINAL_TIME         = '{dr["FINAL_TIME"]}'
                         , LR_YN              = '{dr["LR_YN"]}'
                         , CR_YN              = '{dr["CR_YN"]}'
                         , MT_TIME            = '{dr["MT_TIME"]}'
                         , REMARK_1           = '{dr["REMARK_1"]}'
                         , REMARK_2           = '{dr["REMARK_2"]}'
                         , BI_YN              = '{dr["BI_YN"]}'
                         , USE_YN             = '{dr["USE_YN"]}'
                    WHERE  PLANT_CODE         = '{dr["PLANT_CODE"]}'
                    AND    RESOURCE_NO        = '{dr["RESOURCE_NO"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        dr.RowError = "데이터 수정에 실패했습니다";
                        ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                        return;
                    }
                }

                dr.AcceptChanges();
                viewProduct.RefreshData();
            }

            ShowMessageBox.XtraShowInformation("품목을 저장 했습니다.");

            XMain_Search();
            viewProduct.FocusedRowHandle = focusedRowHandle;
            viewProduct.TopRowIndex = topRowIndex;
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                DataRow row = viewProduct.GetDataRow(viewProduct.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewProduct.DeleteRow(viewProduct.FocusedRowHandle);
                }
                else
                {
                    string sRESOURCE_NO = clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "RESOURCE_NO");
                    string sPLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "PLANT_CODE");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 원료를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {

                        Dbconn.conn.BeginTransaction();

                        SQL = $"DELETE FROM SAP_IN_BOM_CONM WHERE PLANT_CODE = '{sPLANT_CODE}' AND RESOURCE_NO = '{sRESOURCE_NO}' ";

                        Dbconn.conn.SQLrun(SQL);

                        SQL = $"DELETE FROM SAP_IN_BOM_COND WHERE PLANT_CODE = '{sPLANT_CODE}' AND RESOURCE_NO = '{sRESOURCE_NO}' ";

                        Dbconn.conn.SQLrun(SQL);

                        SQL = $@"
                        UPDATE SAP_DI_PRODUCT
                        SET DEL_YN = 'Y'
                        WHERE PLANT_CODE = '{sPLANT_CODE}' AND RESOURCE_NO = '{sRESOURCE_NO}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("원료 삭제에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE INGRED
                        SET DEL_YN = 'Y'
                        WHERE PLANT_CODE = '{sPLANT_CODE}' AND RESOURCE_NO = '{sRESOURCE_NO}' ";

                        Dbconn.conn.SQLrun(SQL);

                        Dbconn.conn.Commit();

                        ShowMessageBox.XtraShowInformation("품목을 삭제 했습니다.");

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
        #endregion

        #region Part 버튼 이벤트
        private void btn_rowAdd_Part_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(viewPart);

            viewPart.SetRowCellValue(viewPart.FocusedRowHandle, viewPart.Columns["PLANT_CODE"], clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "PLANT_CODE"));
            viewPart.SetRowCellValue(viewPart.FocusedRowHandle, viewPart.Columns["RESOURCE_NO"], resourceNo);
        }

        private void btn_rowDel_Part_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPart);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewPart);
        }

        private void btn_Save_Part_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewPart);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridPart.DataSource;

                string plantCode = viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString();

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewPart);

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
                        INSERT INTO SAP_IN_PRODUCT_CP (
                           PLANT_CODE, RESOURCE_NO, RESOURCE_NO_2, RESOURCE_NO_3, PART_P, 
                           EMPLOYEE_NO, I_TIME) 
                        VALUES ( 
                            '{plantCode}', '{dr["RESOURCE_NO"]}', '{dr["RESOURCE_NO_2"]}', '{dr["RESOURCE_NO_3"]}', '{dr["PART_P"]}',
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
                        UPDATE SAP_IN_PRODUCT_CP
                        SET    PART_P        = '{dr["PART_P"]}',
                               EMPLOYEE_NO   = '{dr["EMPLOYEE_NO"]}',
                               I_TIME        = SYSDATE
                        WHERE  PLANT_CODE = '{plantCode}' 
                            AND RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                            AND RESOURCE_NO_2 = '{dr["RESOURCE_NO_2"]}'
                            AND RESOURCE_NO_3 = '{dr["RESOURCE_NO_3"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_Save_Part_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    Dbconn.conn.Commit();
                }

                ShowMessageBox.XtraShowInformation("대체 비율을 저장 했습니다");

                XPartSearch();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_Save_Part_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete_Part_Click(object sender, EventArgs e)
        {
            try
            {
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
                        DELETE FROM SAP_IN_PRODUCT_CP
                        WHERE RESOURCE_NO = '{resourceNo}' 
                            AND RESOURCE_NO_2 = '{viewPart.GetFocusedRowCellValue("RESOURCE_NO_2")}'
                            AND RESOURCE_NO_3 = '{viewPart.GetFocusedRowCellValue("RESOURCE_NO_3")}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("부산물 삭제에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();

                        ShowMessageBox.Confirm("선택하신 대체 비율을 삭제 했습니다");
                    }

                    XPartSearch();
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 제품품목 추가
        private void btn_rowAdd_UpCP_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(viewUpCP);

            viewUpCP.SetRowCellValue(viewUpCP.FocusedRowHandle, viewUpCP.Columns["PLANT_CODE"], clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "PLANT_CODE"));
            viewUpCP.SetRowCellValue(viewUpCP.FocusedRowHandle, viewUpCP.Columns["RESOURCE_NO"], resourceNo);
        }

        private void btn_rowDel_UpCP_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewUpCP);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewUpCP);
        }

        private void btn_Save_UpCP_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewUpCP);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridUpCP.DataSource;

                string plantCode = viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString();

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewUpCP);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewUpCP.FocusedColumn = viewUpCP.Columns[rValid]; // 이동할 컬럼명
                        viewUpCP.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_IN_UPPRODUCT_CP (
                           PLANT_CODE
                         , RESOURCE_NO
                         , RESOURCE_NO_2
                         , PART_P
                         , SAP_UP_YN
                         , EMPLOYEE_NO
                         , I_TIME
                        ) 
                        VALUES (
                           '{dr["PLANT_CODE"]}'
                         , '{dr["RESOURCE_NO"]}'
                         , '{dr["RESOURCE_NO_2"]}'
                         , '{dr["PART_P"]}'
                         , '{dr["SAP_UP_YN"]}'
                         , '{dr["EMPLOYEE_NO"]}'
                         , SYSDATE
                        )
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
                        UPDATE SAP_IN_UPPRODUCT_CP
                        SET    PART_P      = '{dr["PART_P"]}'
                             , SAP_UP_YN   = '{dr["SAP_UP_YN"]}'
                             , EMPLOYEE_NO = '{dr["EMPLOYEE_NO"]}'
                             , I_TIME      = SYSDATE
                        WHERE  PLANT_CODE    = '{dr["PLANT_CODE"]}'
                        AND    RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                        AND    RESOURCE_NO_2 = '{dr["RESOURCE_NO_2"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_Save_Part_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    Dbconn.conn.Commit();
                }

                ShowMessageBox.XtraShowInformation("대체 비율을 저장 했습니다");

                XUpCPSearch();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_Save_Part_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete_UpCP_Click(object sender, EventArgs e)
        {
            try
            {
                if (viewUpCP.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 품목을 선택하여 주세요");
                    return;
                }
                DataRow row = viewUpCP.GetDataRow(viewUpCP.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewUpCP.DeleteRow(viewUpCP.FocusedRowHandle);
                }
                else
                {
                    DialogResult result = ShowMessageBox.Confirm("선택하신 대체 항목을 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        Dbconn.conn.BeginTransaction();

                        // Delete from SHIFT_DETAIL table
                        SQL = $@"
                        DELETE FROM SAP_IN_UPPRODUCT_CP
                        WHERE RESOURCE_NO = '{resourceNo}' 
                            AND RESOURCE_NO = '{viewUpCP.GetFocusedRowCellValue("RESOURCE_NO")}'
                            AND RESOURCE_NO_2 = '{viewUpCP.GetFocusedRowCellValue("RESOURCE_NO_2")}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("부산물 삭제에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();

                        ShowMessageBox.XtraShowInformation("선택하신 대체 비율을 삭제 했습니다");
                    }

                    XUpCPSearch();
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region Residue 버튼 이벤트
        private void btn_rowAdd_Res_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(viewResidue);

            viewResidue.SetRowCellValue(viewResidue.FocusedRowHandle, viewResidue.Columns["RESOURCE_NO"], resourceNo);
            viewResidue.SetRowCellValue(viewResidue.FocusedRowHandle, viewResidue.Columns["PLANT_CODE"], clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "PLANT_CODE"));
        }

        private void btn_rowDel_Res_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewResidue);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewResidue);
        }

        private void btn_Save_Res_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewResidue);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridResidue.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewResidue);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewResidue.FocusedColumn = viewResidue.Columns[rValid]; // 이동할 컬럼명
                        viewResidue.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_IN_PRODUCT_RC (
                            PLANT_CODE, RESOURCE_NO, RESOURCE_NO_2, PART_P, SAP_UP_YN,
                            EMPLOYEE_NO, I_TIME) 
                        VALUES ( 
                           '{dr["PLANT_CODE"]}', '{dr["RESOURCE_NO"]}', '{dr["RESOURCE_NO_2"]}', '{dr["PART_P"]}', '{dr["SAP_UP_YN"]}',
                            '{clsCommon.UserId}', SYSDATE )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save1_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE SAP_IN_PRODUCT_RC
                        SET    PART_P        = '{dr["PART_P"]}',
                               EMPLOYEE_NO   = '{clsCommon.UserId}',
                               SAP_UP_YN     = '{dr["SAP_UP_YN"]}',
                               I_TIME        = SYSDATE
                        WHERE  PLANT_CODE = '{cboPlant_Code.EditValue}' 
                           AND RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                           AND RESOURCE_NO_2 = '{dr["RESOURCE_NO_2"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save1_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("부산물을 저장 했습니다");

                XResSearch();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save1_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete_Res_Click(object sender, EventArgs e)
        {
            try
            {
                if (viewResidue.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 부산물을 선택하여 주세요");
                    return;
                }

                DataRow row = viewResidue.GetDataRow(viewResidue.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewResidue.DeleteRow(viewResidue.FocusedRowHandle);
                }
                else
                {
                    DialogResult result = ShowMessageBox.Confirm("선택하신 부산물을 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        Dbconn.conn.BeginTransaction();

                        // Delete from SHIFT_DETAIL table
                        SQL = $@"
                        DELETE FROM SAP_IN_PRODUCT_RC
                        WHERE RESOURCE_NO = '{resourceNo}' 
                            AND RESOURCE_NO_2 = '{viewResidue.GetFocusedRowCellValue("RESOURCE_NO_2")}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("부산물 삭제에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();

                        ShowMessageBox.Confirm("선택하신 부산물을 삭제 했습니다");

                        XResSearch();
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void viewProduct_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (viewProduct == null)
                    return;

                string cp = viewProduct.GetRowCellValue(e.RowHandle, viewProduct.Columns["CP"]).ToString();
                string rc = viewProduct.GetRowCellValue(e.RowHandle, viewProduct.Columns["RC"]).ToString();
                string upcp = viewProduct.GetRowCellValue(e.RowHandle, viewProduct.Columns["UPCP"]).ToString();


                if (cp != "Y" && rc != "Y" && upcp != "Y")
                    return;

                if (cp == "Y")
                {
                    e.Appearance.BackColor = Color.LightCyan;
                    e.Appearance.ForeColor = Color.DarkGreen;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }

                if (upcp == "Y")
                {
                    e.Appearance.BackColor = Color.LightCyan;
                    e.Appearance.ForeColor = Color.DarkBlue;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }

                if (rc == "Y")
                {
                    e.Appearance.BackColor = Color.LightCyan;
                    e.Appearance.ForeColor = Color.DarkOrange;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
            catch (Exception ex)
            {

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
            gridProduct.Focus();
            viewProduct.FocusedRowHandle = 0;
            viewProduct.FocusedColumn = viewProduct.VisibleColumns[0];
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            //clsDevexpressUtil.ItemSearchLookUpEditSetup(scboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

            if (isInitializing)
                XMain_Search();
        }

        private void cboRESOURCE_TYPE_EditValueChanged(object sender, EventArgs e)
        {
            if (isInitializing)
                XMain_Search();
        }

        private void scboRESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            if (isInitializing)
                XMain_Search();
        }

        private void viewResidue_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewResidue.GetFocusedRowCellValue("RESOURCE_NO_2")?.ToString() == "")
            {
                return;
            }

            // 기존 행에서 특정 컬럼만 편집 허용 (예: "Quantity" 컬럼만 수정 가능)
            if (viewResidue.FocusedColumn.FieldName == "RESOURCE_NO_2")
            {
                e.Cancel = true; // 편집 막기
            }
        }

        private void txtResource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }

        private void viewProduct_ShownEditor(object sender, EventArgs e)
        {
            //GridView view = sender as GridView;

            //if (view.FocusedColumn.FieldName == "BU_P")
            //{
            //    LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
            //    edit.ShowPopup();
            //    edit.ClosePopup();

            //    // 여기
            //    clsDevexpressGrid.ItemLookUpEditSetup(gridCboBU_P, clsCommon.GetResource(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), $"'{clsCommon.GetResourceTypeCode("부산물")}'"));
            //    edit.Properties.PopupFormMinSize = new Size(200, 300);
            //}
        }

        private void viewPart_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void chkAllProduct_CheckedChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}