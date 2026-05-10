using Core.Class;
using Core.Extension;
using DevExpress.Charts.Native;
using DevExpress.DataAccess.DataFederation;
using DevExpress.DataAccess.Native.Data;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Internal;
using DevExpress.Office.Printing;
using DevExpress.PivotGrid.QueryMode;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Extensions;
using DevExpress.Utils.Html.Internal;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Tab;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Printing;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static DevExpress.Utils.HashCodeHelper.Blob;
using DataColumn = System.Data.DataColumn;
using DataTable = System.Data.DataTable;

namespace HARIM_FA_DOSING
{
    public partial class frm_Pdetail : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        bool headerCheckState = false;
        string vPalntCode = string.Empty;
        private string[] sValid = null;
        string sTapIdx = "1";
        string vResourceNo = string.Empty;

        public frm_Pdetail()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewProduct, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewDictionary, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewBomMix, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewBom, Properties.Settings.Default.FontSize);
        }

        private void frm_Pdetail_Load(object sender, EventArgs e)
        {
            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                InitContorl();

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridProduct));
                // Application.AddMessageFilter(new GridMouseWheelFilter(gridDictionary));
                // Application.AddMessageFilter(new GridMouseWheelFilter(gridBomMix));
                // Application.AddMessageFilter(new GridMouseWheelFilter(gridBom));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_Pdetail_Load", ex);
            }
        }

        private void InitContorl()
        {
            //그리드 바로 첫클릭시 동작
            viewProduct.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            viewDictionary.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            //clsDevexpressUtil.ItemSearchLookUpEditSetup(scboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", 1));
            //clsDevexpressUtil.ItemLookUpEditSetup(cboSearchNote, clsCommon.getNote(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);

            clsDevexpressUtil.ItemLookUpEditSetup(cboUnit, clsCommon.GetUnit(), "", false, 0, true);

            if (new string[] { "PJ01", "PJ02", "PJ04", "PJ05", "P101" }.Contains(clsCommon.PlantCode))
            {
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 자재 마스터
                SELECT 
                      a.PLANT_CODE, a.RESOURCE_NO, a.DESCRIPTION, b.NOTE, a.UOM, a.RESOURCE_TYPE, a.RESOURCE_TYPE_DESC
                    , a.ZSPEC,a.P_TYPE, a.STD_LOT_SIZE, a.PACK_SIZE, a.ZDOMESTIC_IMPORT, a.LVORM
                    , a.I_TIME, a.WERKS, a.HAND_YN, a.BU_P, a.PLANNER, a.SOURCE_DESC
                    , a.HOME_LOCATION, a.COST_CENTER1, a.COST_CENTER3, a.COST_DIVISION
                    , a.POU, a.BUYER, a.MANF_ENGR, a.INSPECTOR, a.MAT_MSTR
                    , a.ITEM_GROUP_V, a.ITEM_GROUP_W, a.AP_CLOSE_TYPE
                    , MAX(b.DATUV) AS DATUV, MAX(b.DATUV_TO) AS DATUV_TO, MAX(b.BOM_TYPE) AS BOM_TYPE
                    , MAX(CASE 
                        WHEN TO_DATE(b.DATUV, 'YY/MM/DD') > TRUNC(SYSDATE)
                        THEN 'M' -- 효력 유효
                        WHEN TRUNC(SYSDATE) BETWEEN TO_DATE(b.DATUV, 'YY/MM/DD') AND TO_DATE(b.DATUV_TO, 'YY/MM/DD')
                        THEN 'Y' -- 효력 유효
                        ELSE CASE WHEN b.DATUV IS NULL THEN NULL ELSE 'N' END -- 효력 초과
                     END) AS CHECK_DATUV
                FROM SAP_DI_PRODUCT a
                        JOIN (
                           SELECT PLANT_CODE, RESOURCE_NO, NOTE, MAX(BOM_TYPE) AS BOM_TYPE, DATUV, DATUV_TO
                            FROM (SELECT DISTINCT PLANT_CODE, RESOURCE_NO, NOTE, P_TYPE AS BOM_TYPE, I_TIME
                                            , DATUV, DATUV_TO
                                            , ROW_NUMBER() OVER(PARTITION BY RESOURCE_NO ORDER BY NOTE DESC) AS rn
                                            FROM SAP_IN_BOM_CONM
                                            WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}')
                                            ORDER BY NOTE DESC)
                            WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}') AND rn = '1'
                            GROUP BY PLANT_CODE, RESOURCE_NO, NOTE, DATUV, DATUV_TO
                            ) b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND (a.DESCRIPTION LIKE '%{txtResource.EditValue}%' OR a.RESOURCE_NO LIKE '%{txtResource.EditValue}%')
                    AND a.RESOURCE_TYPE IN ('{clsCommon.GetResourceTypeCode("제품")}', '{clsCommon.GetResourceTypeCode("반제품")}')
                    AND b.NOTE LIKE '%{txtNote.EditValue}%'
                    AND ('{cboUnit.EditValue}' IS NULL OR a.UOM = '{cboUnit.EditValue}')
                GROUP BY a.PLANT_CODE, a.RESOURCE_NO, a.DESCRIPTION, b.NOTE, a.UOM, a.RESOURCE_TYPE, a.RESOURCE_TYPE_DESC
                    , a.ZSPEC,a.P_TYPE, a.STD_LOT_SIZE, a.PACK_SIZE, a.ZDOMESTIC_IMPORT, a.LVORM
                    , a.I_TIME, a.WERKS, a.HAND_YN, a.BU_P, a.PLANNER, a.SOURCE_DESC
                    , a.HOME_LOCATION, a.COST_CENTER1, a.COST_CENTER3, a.COST_DIVISION
                    , a.POU, a.BUYER, a.MANF_ENGR, a.INSPECTOR, a.MAT_MSTR
                    , a.ITEM_GROUP_V, a.ITEM_GROUP_W, a.AP_CLOSE_TYPE
                ORDER BY BOM_TYPE DESC, a.PLANT_CODE, a.RESOURCE_TYPE, a.RESOURCE_NO, a.UOM
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridProduct, viewProduct, ds.Tables[0], true);

                ds.Dispose();

                viewProduct.OptionsBehavior.Editable = true;
                viewProduct.OptionsSelection.MultiSelect = true;

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboPlant, clsCommon.GetPlant("", true));

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboResourceNo, clsCommon.GetResource("", "", "", "", 0, true));

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboNote, clsCommon.getNote());

                // 생산유형
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboRESOURCE_TYPE, clsCommon.GetResourceType());


                clsDevexpressGrid.ItemLookUpEditSetup(gridcboUOM, clsCommon.GetUnit());

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void xBom_Search(string plantCode, string resourceNo, string note)
        {
            DataSet ds = null;

            try
            {
                SQL = $@"
                -- 배합 설정정보
                SELECT MAX(PLANT_CODE)  AS PLANT_CODE
                    ,  MAX(RESOURCE_NO) AS RESOURCE_NO
                    ,  MAX(MIX_TIME)    AS MIX_TIME
                    ,  MAX(MIX_TIME2)   AS MIX_TIME2 
                    ,  MAX(DRY_TIME)    AS DRY_TIME
                    ,  MAX(MT_TIME)     AS MT_TIME
                    ,  MAX(FINAL_TIME)  AS FINAL_TIME
                    ,  MAX(LR_YN)       AS LR_YN
                    ,  MAX(CR_YN)       AS CR_YN
                    ,  MAX(REMARK_1)    AS REMARK_1
                    ,  MAX(REMARK_2)    AS REMARK_2
                FROM (
                    SELECT 
                        a.PLANT_CODE
                      , a.RESOURCE_NO
                      , a.MIX_TIME
                      , a.MIX_TIME2
                      , a.DRY_TIME
                      , a.MT_TIME
                      , a.FINAL_TIME
                      , a.LR_YN
                      , a.CR_YN
                      , a.REMARK_1
                      , a.REMARK_2
                    FROM SAP_IN_BOM_CONM a
                    WHERE a.PLANT_CODE = '{plantCode}'
                      AND a.RESOURCE_NO = '{resourceNo}'
                      AND a.NOTE = '{note}'
                      AND a.P_TYPE = '2'
                    UNION ALL
                    -- 2. a 테이블에 값이 없을 경우 b 테이블 조회
                    SELECT 
                        b.PLANT_CODE
                      , b.RESOURCE_NO
                      , b.MIX_TIME
                      , b.MIX_TIME2
                      , b.DRY_TIME
                      , b.MT_TIME
                      , b.FINAL_TIME
                      , b.LR_YN
                      , b.CR_YN
                      , b.REMARK_1
                      , b.REMARK_2
                    FROM SAP_DI_PRODUCT b
                    WHERE b.PLANT_CODE = '{plantCode}'
                      AND b.RESOURCE_NO = '{resourceNo}'
                    )
                ";

                ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridBom, viewBom, ds.Tables[0], true);

                sValid = new string[] { "MENGE" };

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "N");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Dicionary_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private string DictionaryQuery(string plantCode, string p_type, string resourceNo, string note)
        {
            string query = $@"
            -- 사전 배합비
            SELECT  a.PLANT_CODE, a.P_TYPE, a.RESOURCE_NO, a.NOTE, a.STLST
                , a.MIX_TIME, a.MIX_TIME2, a.DRY_TIME, a.FINAL_TIME, a.LR_YN
                , a.REMARK_1, a.CR_YN, a.REMARK_2, a.USE_YN, a.EMPLOYEE_NO
                , b.IDNRK, b.P_NOTE, b.STLAN, b.STLNR, b.STLAL, b.BMEIN
                , b.LKENZ, b.POSNR, b.MENGE, b.MEINS, b.AUSCH, b.KZKUP
                , b.ALPOS, b.ALPGR, b.EWAHR, b.SANFE, b.SANKA, b.BEIKZ
                , b.DATUV, b.DATUV_TO, b.AENNR, b.SORTF, b.LGORT, b.POTX1, b.SEQ, b.XSEQNR
                , CASE 
                    WHEN SYSDATE BETWEEN TO_DATE(b.DATUV, 'YY/MM/DD') AND TO_DATE(b.DATUV_TO, 'YY/MM/DD') 
                    THEN 'Y' -- 효력 유효
                    ELSE 'N' -- 효력 초과
                 END AS CHECK_DATUV
            FROM SAP_IN_BOM_CONM a
                INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.P_TYPE = a.P_TYPE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
            WHERE ('{plantCode}' IS NULL OR a.PLANT_CODE = '{plantCode}')
                AND ('{p_type}' IS NULL OR a.P_TYPE = '{p_type}') AND a.RESOURCE_NO = '{resourceNo}'
                AND ('{note}' IS NULL OR a.NOTE = '{note}')
            ORDER BY TO_NUMBER(b.SEQ)
            ";

            return query;
        }

        private string BomMixQuery(string plantCode, string p_type, string resourceNo, string note)
        {
            string query = $@"
            -- BOM 배합비
            SELECT  a.PLANT_CODE, a.P_TYPE, a.RESOURCE_NO, a.NOTE, a.STLST
                , a.MIX_TIME, a.MIX_TIME2, a.DRY_TIME, a.FINAL_TIME, a.LR_YN
                , a.REMARK_1, a.CR_YN, a.REMARK_2, a.USE_YN, a.EMPLOYEE_NO
                , b.IDNRK, b.P_NOTE, b.STLAN, b.STLNR, b.STLAL, b.BMEIN
                , b.LKENZ, b.POSNR
                , b.MENGE
                , b.MEINS, b.AUSCH, b.KZKUP
                , b.ALPOS, b.ALPGR, b.EWAHR, b.SANFE, b.SANKA, b.BEIKZ
                , b.DATUV, b.DATUV_TO, b.AENNR, b.SORTF, b.LGORT, b.POTX1, b.SEQ AS SEQ, b.XSEQNR
                , CASE 
                    WHEN SYSDATE BETWEEN TO_DATE(b.DATUV, 'YY/MM/DD') AND TO_DATE(b.DATUV_TO, 'YY/MM/DD') 
                    THEN 'Y' -- 효력 유효
                    ELSE 'N' -- 효력 초과
                 END AS CHECK_DATUV
                , c.USE_FLAG
            FROM SAP_IN_BOM_CONM a
                INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.P_TYPE = a.P_TYPE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                INNER JOIN  SAP_DI_PRODUCT c ON c.PLANT_CODE = b.PLANT_CODE AND c.RESOURCE_NO = b.IDNRK
            WHERE ('{plantCode}' IS NULL OR a.PLANT_CODE = '{plantCode}')
                AND ('{p_type}' IS NULL OR a.P_TYPE = '{p_type}') AND a.RESOURCE_NO = '{resourceNo}'
                AND ('{note}' IS NULL OR a.NOTE = '{note}')
            ORDER BY TO_NUMBER(b.SEQ)
            ";

            return query;
        }
        #endregion

        private void gridView_product_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void viewDictionary_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void viewBomMix_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridView_product_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridBom.DataSource = null;

                vPalntCode = viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                //InitEventControl();

                txtResourceNo.Text = viewProduct.GetFocusedRowCellDisplayText("RESOURCE_NO");
                vResourceNo = viewProduct.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();

                if (!vResourceNo.IsNullEmpty() && vResourceNo.Contains("P"))
                    layoutControlItem23.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                else
                    layoutControlItem23.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                clsDevexpressUtil.ItemLookUpEditSetup(cboNOTE, clsCommon.getNote(cboPlant_Code.EditValue?.ToString(), vResourceNo), "", false, 0);

                if (!string.IsNullOrEmpty(txtResourceNo.EditValue.ToString()))
                {
                    GetMixVersion(vPalntCode, vResourceNo);
                }

                xBom_Search(vPalntCode, vResourceNo, cboNOTE.EditValue?.ToString());
                //xMix_Search(vPalntCode, sTapIdx, vResourceNo.ToString(), cboNOTE.EditValue.ToString());
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        private void xMix_Search(string plantCode, string p_type, string resourceNo, string note)
        {
            DataSet ds = null;

            try
            {
                if (p_type.Equals("1"))
                {
                    SQL = DictionaryQuery(plantCode, p_type, resourceNo, note);

                    ds = Dbconn.conn.ExecutDataset(SQL);
                    clsDevexpressGrid.BindGridControl(gridDictionary, viewDictionary, ds.Tables[0], true);

                    viewDictionary.OptionsBehavior.Editable = true;
                    viewDictionary.OptionsSelection.MultiSelect = true;

                    clsDevexpressGrid.ItemSearchLookUpEditSetup(gridDicscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", "", 0, true));

                    // 단위
                    clsDevexpressGrid.ItemLookUpEditSetup(gridDiccboUnit, clsCommon.GetUnit());
                }
                else if (p_type.Equals("2"))
                {
                    SQL = BomMixQuery(plantCode, p_type, resourceNo, note);

                    ds = Dbconn.conn.ExecutDataset(SQL);
                    clsDevexpressGrid.BindGridControl(gridBomMix, viewBomMix, ds.Tables[0], true);

                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["STLST"].ToString() == "2")
                    {
                        btnBomMixConfirm.Text = "확정 취소";
                        //layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItem21.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItem17.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else
                    {
                        btnBomMixConfirm.Text = "배합비 확정";
                        //layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItem21.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItem17.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    }

                    gridBomcboNOTE.NullText = "";
                    clsDevexpressGrid.ItemSearchLookUpEditSetup(gridBomscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", "", 0, true));

                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[] {
                    new DataColumn("NAME"),
                    new DataColumn("CODE"),
                });

                    dt.Rows.Add("KG", "1");
                    dt.Rows.Add("EA", "2");

                    gridBomcboUnit.NullText = "";
                    gridBomcboUnit.NullValuePrompt = "";
                    clsDevexpressGrid.ItemLookUpEditSetup(gridBomcboUnit, dt);
                }

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Dicionary_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void InitEventControl()
        {
            gridDictionary.DataSource = null;
            gridDictionary.RefreshDataSource();
            gridBomMix.DataSource = null;
            gridBomMix.RefreshDataSource();
            gridBom.DataSource = null;
            gridBom.RefreshDataSource();
        }

        private void GetMixVersion(string sPlant_Code, string sResource_No)
        {
            SQL = $@"
                SELECT DISTINCT NOTE AS CODE, NOTE AS NAME, I_TIME
                FROM SAP_IN_BOM_CONM
                WHERE PLANT_CODE = '{sPlant_Code}' AND USE_YN = 'Y' AND RESOURCE_NO = '{sResource_No}'
                ORDER BY NOTE DESC
                ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            //clsDevexpressUtil.ItemLookUpEditSetup(cboNOTE, clsCommon.getNote("", sResource_No), "", false, 0);

            if (ds.Tables[0].Rows.Count > 0)
                cboNOTE.EditValue = ds.Tables[0].Rows[0]["CODE"].ToString();
            else
                cboNOTE.EditValue = "";

            ds.Dispose();
        }

        #region 메인 이벤트
        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        /// <summary>
        /// 탭 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabMix_MouseUp(object sender, MouseEventArgs e)
        {
            var tabControl = sender as DevExpress.XtraTab.XtraTabControl;
            DevExpress.XtraTab.ViewInfo.XtraTabHitInfo hitInfo = tabControl.CalcHitInfo(e.Location);

            if (hitInfo.Page != null && hitInfo.HitTest == DevExpress.XtraTab.ViewInfo.XtraTabHitTest.PageHeader)
            {
                sTapIdx = (tabMix.SelectedTabPageIndex + 1).ToString();

                xBom_Search(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), vResourceNo, cboNOTE.EditValue?.ToString());
                xMix_Search(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), sTapIdx.ToString(), vResourceNo, cboNOTE.EditValue?.ToString());
            }
        }
        #endregion

        #region 사전배합비 이벤트

        //private void btnAutoNote_Click(object sender, EventArgs e)
        //{
        //    txtNote.EditValue = DateTime.Now.ToString("yyyyMMddHHmmss");

        //    Thread.Sleep(700);
        //}

        /// <summary>
        /// 사전 배합비 복사
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBomMixCopy_Click(object sender, EventArgs e)
        {
            bool bPremixGubun = false;
            DataSet dsPremix = null;

            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowWarning("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("배합비 정보를 본 배합비로 복사 하시겠습니까?"))
            {
                return;
            }

            if (cboNOTE.EditValue.Equals(""))
            {
                ShowMessageBox.XtraShowWarning("배합비 버전이 없습니다. 배합비 버전을 먼저 생성해주세요.");
                return;
            }

            //if (viewProduct.GetFocusedRowCellValue("CHECK_DATUV").ToString() == "N")
            //{
            //    ShowMessageBox.XtraShowWarning("효력 유효일이 초과된 배합비는 사용할 수 없습니다.");
            //    return;
            //}

            try
            {
                if (!MixTimeChek(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), vResourceNo, cboNOTE.EditValue.ToString())) return;

                SQL = $@"
                SELECT 1
                FROM SAP_IN_BOM_COND
                WHERE P_TYPE = '2' AND PLANT_CODE = '{viewProduct.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND RESOURCE_NO = '{vResourceNo}' AND NOTE = '{cboNOTE.EditValue}'
                    AND ROWNUM = 1
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {
                    ShowMessageBox.Confirm("본 배합비가 존재 합니다.", "기존 본 배합비를 삭제 후 복사해주세요.");
                    return;
                }

                int[] selectedRows = viewDictionary.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("복사 할 자재를 먼저 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewDictionary.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(vResourceNo.ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("RESOURCE_NO", "제품을 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    SQL = $@"
                    SELECT 
                        PLANT_CODE
                        , PRESOURCE_NO
                        , PIDNRK
                        , I_USER 
                        , I_TIME
                    FROM PREMIX_MASTER
                    WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PRESOURCE_NO = 'P{vResourceNo}' AND PIDNRK = '{dr["IDNRK"]}'
                    ";

                    dsPremix = Dbconn.conn.ExecutDataset(SQL);

                    Dbconn.conn.BeginTransaction();

                    string aa = "";

                    if (dr["IDNRK"].ToString() == "1222170000")
                        aa = "";

                    // 프리믹스 마스터에 있는 원료면
                    if (Dbconn.conn.getRowCnt(dsPremix) > 0)
                    {
                        bPremixGubun = true;
                    }
                    else if (!CopyBomMix(dr))
                    {
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.AcceptChanges();

                } //foreach

                if (bPremixGubun)
                {
                    CreatePremix(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), vResourceNo, cboNOTE.EditValue?.ToString());

                    // 프리믹스 배합비를 본 배합비 추가
                    if (!SetBomMixCreate(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), vResourceNo, cboNOTE.EditValue?.ToString())) return;

                    // 프리믹스 배합비 100% 만들기
                    if (!SetPreMixRate(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), vResourceNo, cboNOTE.EditValue?.ToString())) return;

                    bPremixGubun = false;
                }

                // 제품별 품목(SAP_IN_UPPRODUCT_CP) 추가가 있으면 SAP_IN_BOM_COND merge 추가 실행
                SetResorce_CP(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), vResourceNo, cboNOTE.EditValue?.ToString());

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowInformation("배합비가 모두 복사 되었습니다.");

                viewDictionary.RefreshData();

                xBom_Search(vPalntCode, vResourceNo.ToString(), cboNOTE.EditValue.ToString());
                xMix_Search(vPalntCode, "2", vResourceNo.ToString(), cboNOTE.EditValue.ToString());

                tabMix.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btnBomMixCopy_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void CreatePremix(string sPlantCode, string sResourceNo, string sNote)
        {
            // 프리믹스 기준정보 만들기
            if (!PremixBaseDataCreate(sPlantCode, sResourceNo, sNote)) return;

            Dbconn.conn.BeginTransaction();

            // 프리믹스 배합비 생성
            if (!PreMixCreate(sPlantCode, sResourceNo, sNote))
            {
                Dbconn.conn.Rollback();
                return;
            }

            Dbconn.conn.Commit();
        }

        private bool PreMixCreate(string sPlantCode, string sResourceNo, string sNote)
        {
            SQL = $@"
            SELECT 1
            FROM SAP_IN_BOM_COND
            WHERE PLANT_CODE = '{sPlantCode}'
                AND RESOURCE_NO = 'P{sResourceNo}' AND NOTE = '{sNote}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
            {
                return true;
            }

            SQL = $@"
            MERGE INTO SAP_IN_BOM_COND D
            USING (
            SELECT DISTINCT a.PLANT_CODE, '1' AS P_TYPE, a.NOTE, a.IDNRK, 
                'P' || a.RESOURCE_NO AS RESOURCE_NO , a.STLAN, a.STLNR, 
                a.STLAL, a.BMEIN, a.LKENZ, 
                a.POSNR, a.MENGE, 
                '' AS P_NOTE, a.MEINS, a.AUSCH, a.KZKUP, 
                a.ALPOS, a.ALPGR, a.EWAHR, 
                a.SANFE, a.SANKA, a.BEIKZ, 
                a.DATUV, a.DATUV_TO, a.AENNR, a.SORTF, 
                a.LGORT, a.POTX1, a.SEQ, 
                a.XSEQNR, SYSDATE AS I_TIME
            FROM SAP_IN_BOM_COND a
                LEFT OUTER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.BU_P = a.IDNRK
                LEFT OUTER JOIN SAP_IN_PRODUCT_CP d ON d.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO AND d.RESOURCE_NO_2 = a.IDNRK
            WHERE a.PLANT_CODE = '{sPlantCode}' AND a.P_TYPE = '1'
                AND a.RESOURCE_NO = '{sResourceNo}'
                AND a.NOTE = '{sNote}'
                AND a.IDNRK IN (SELECT PIDNRK FROM PREMIX_MASTER WHERE PLANT_CODE = '{sPlantCode}' AND PRESOURCE_NO = 'P{sResourceNo}')
            UNION ALL
            SELECT DISTINCT a.PLANT_CODE, '2' AS P_TYPE, a.NOTE, NVL(d.RESOURCE_NO_3, NVL(c.RESOURCE_NO, a.IDNRK)) AS IDNRK, 
                'P' || a.RESOURCE_NO AS RESOURCE_NO, a.STLAN, a.STLNR, 
                a.STLAL, a.BMEIN, a.LKENZ, 
                a.POSNR, NVL((a.MENGE * (d.PART_P / 100)), a.MENGE) AS MENGE, 
                '' AS P_NOTE, a.MEINS, a.AUSCH, a.KZKUP, 
                a.ALPOS, a.ALPGR, a.EWAHR, 
                a.SANFE, a.SANKA, a.BEIKZ, 
                a.DATUV, a.DATUV_TO, a.AENNR, a.SORTF, 
                a.LGORT, a.POTX1, a.SEQ, 
                a.XSEQNR, SYSDATE + (1/24/60/60) AS I_TIME
            FROM SAP_IN_BOM_COND a
                LEFT OUTER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.BU_P = a.IDNRK
                LEFT OUTER JOIN SAP_IN_PRODUCT_CP d ON c.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO AND d.RESOURCE_NO_2 = a.IDNRK
            WHERE a.PLANT_CODE = '{sPlantCode}' AND a.P_TYPE = '1'
                AND a.RESOURCE_NO = '{sResourceNo}'
                AND a.NOTE = '{sNote}'
                AND a.IDNRK IN (SELECT PIDNRK FROM PREMIX_MASTER WHERE PLANT_CODE = '{sPlantCode}' AND PRESOURCE_NO = 'P{sResourceNo}')
            ORDER BY IDNRK) s
            ON
              (d.PLANT_CODE = s.PLANT_CODE AND
              d.P_TYPE = s.P_TYPE AND 
              d.NOTE = s.NOTE AND 
              d.RESOURCE_NO = s.RESOURCE_NO AND 
              d.IDNRK = s.IDNRK )
            --WHEN MATCHED
            --THEN
            --UPDATE SET
            --  d.STLAN = s.STLAN,
            --  d.STLNR = s.STLNR,
            --  d.STLAL = s.STLAL,
            --  d.BMEIN = s.BMEIN,
            --  d.LKENZ = s.LKENZ,
            --  d.POSNR = s.POSNR,
            --  d.MENGE = s.MENGE,
            --  d.P_NOTE = s.P_NOTE,
            --  d.MEINS = s.MEINS,
            --  d.AUSCH = s.AUSCH,
            --  d.KZKUP = s.KZKUP,
            --  d.ALPOS = s.ALPOS,
            --  d.ALPGR = s.ALPGR,
            --  d.EWAHR = s.EWAHR,
            --  d.SANFE = s.SANFE,
            --  d.SANKA = s.SANKA,
            --  d.BEIKZ = s.BEIKZ,
            --  d.DATUV = s.DATUV,
            --  d.DATUV_TO = s.DATUV_TO,
            --  d.AENNR = s.AENNR,
            --  d.SORTF = s.SORTF,
            --  d.LGORT = s.LGORT,
            --  d.POTX1 = s.POTX1,
            --  d.SEQ = s.SEQ,
            --  d.XSEQNR = s.XSEQNR,
            --  d.I_TIME = s.I_TIME
            WHEN NOT MATCHED
            THEN
            INSERT (
              PLANT_CODE, P_TYPE, NOTE, RESOURCE_NO,
              IDNRK, STLAN, STLNR,
              STLAL, BMEIN,
              LKENZ, POSNR, MENGE,
              P_NOTE, MEINS, AUSCH,
              KZKUP, ALPOS, ALPGR,
              EWAHR, SANFE, SANKA,
              BEIKZ, DATUV, DATUV_TO,
              AENNR, SORTF, LGORT,
              POTX1, SEQ, XSEQNR, I_TIME)
            VALUES (
              s.PLANT_CODE, s.P_TYPE, s.NOTE, s.RESOURCE_NO,
              s.IDNRK, s.STLAN, s.STLNR,
              s.STLAL, s.BMEIN,
              s.LKENZ, s.POSNR, s.MENGE,
              s.P_NOTE, s.MEINS, s.AUSCH,
              s.KZKUP, s.ALPOS, s.ALPGR,
              s.EWAHR, s.SANFE, s.SANKA,
              s.BEIKZ, s.DATUV, s.DATUV_TO,
              s.AENNR, s.SORTF, s.LGORT,
              s.POTX1, s.SEQ, s.XSEQNR, s.I_TIME)
            ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "PreMixCreate", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                return false;
            }

            return true;
        }

        private void SetResorce_CP(string sPlantCode, string sResourceNo, string sNote)
        {
            SQL = $@"
                SELECT 1
                FROM SAP_IN_UPPRODUCT_CP
                WHERE PLANT_CODE = '{sPlantCode}'
                        AND RESOURCE_NO = '{sResourceNo}'
                ";

            DataSet ds2 = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds2) > 0)
            {
                SQL = $@"
                MERGE INTO SAP_IN_BOM_COND D
                USING (
                SELECT 
                        '2'                           AS P_TYPE
                    , MAX(a.PLANT_CODE)              AS PLANT_CODE
                    , MAX(a.NOTE)                    AS NOTE
                    , b.RESOURCE_NO_2                AS IDNRK
                    , MAX(a.RESOURCE_NO)             AS RESOURCE_NO
                    , MAX(a.STLAN)                   AS STLAN
                    , MAX(a.STLNR)                   AS STLNR
                    , MAX(a.STLAL)                   AS STLAL
                    , MAX(a.BMEIN)                   AS BMEIN
                    , MAX(a.LKENZ)                   AS LKENZ
                    , MAX(a.POSNR) + 10              AS POSNR
                    , b.PART_P                       AS MENGE
                    , MAX(a.P_NOTE)                  AS P_NOTE
                    , MAX(a.MEINS)                   AS MEINS
                    , MAX(a.AUSCH)                   AS AUSCH
                    , MAX(a.KZKUP)                   AS KZKUP
                    , MAX(a.ALPOS)                   AS ALPOS
                    , MAX(a.ALPGR)                   AS ALPGR
                    , MAX(a.EWAHR)                   AS EWAHR
                    , MAX(a.SANFE)                   AS SANFE
                    , MAX(a.SANKA)                   AS SANKA
                    , MAX(a.BEIKZ)                   AS BEIKZ
                    , MIN(a.DATUV)                   AS DATUV
                    , MAX(a.DATUV_TO)                AS DATUV_TO
                    , MAX(a.AENNR)                   AS AENNR
                    , MAX(a.SORTF)                   AS SORTF
                    , MAX(a.LGORT)                   AS LGORT
                    , MAX(a.POTX1)                   AS POTX1
                    , MAX(TO_NUMBER(a.SEQ)) + 1      AS SEQ
                    , MAX(a.XSEQNR)                  AS XSEQNR
                    , MAX(a.I_TIME)                  AS I_TIME
                FROM SAP_IN_BOM_COND a
                    JOIN SAP_IN_UPPRODUCT_CP b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{sPlantCode}' AND a.P_TYPE = '1'
                    AND a.RESOURCE_NO = '{sResourceNo}'
                    AND a.NOTE = '{sNote}'
                    GROUP BY b.RESOURCE_NO_2, b.PART_P) s
                ON
                    (d.PLANT_CODE = s.PLANT_CODE AND
                    d.P_TYPE = s.P_TYPE AND 
                    d.NOTE = s.NOTE AND 
                    d.RESOURCE_NO = s.RESOURCE_NO AND 
                    d.IDNRK = s.IDNRK )
                -- WHEN MATCHED
                -- THEN
                -- UPDATE SET
                --   d.STLAN = s.STLAN,
                --   d.STLNR = s.STLNR,
                --   d.STLAL = s.STLAL,
                --   d.BMEIN = s.BMEIN,
                --   d.LKENZ = s.LKENZ,
                --   d.POSNR = s.POSNR,
                --   d.MENGE = s.MENGE,
                --   d.P_NOTE = s.P_NOTE,
                --   d.MEINS = s.MEINS,
                --   d.AUSCH = s.AUSCH,
                --   d.KZKUP = s.KZKUP,
                --   d.ALPOS = s.ALPOS,
                --   d.ALPGR = s.ALPGR,
                --   d.EWAHR = s.EWAHR,
                --   d.SANFE = s.SANFE,
                --   d.SANKA = s.SANKA,
                --   d.BEIKZ = s.BEIKZ,
                --   d.DATUV = s.DATUV,
                --   d.DATUV_TO = s.DATUV_TO,
                --   d.AENNR = s.AENNR,
                --   d.SORTF = s.SORTF,
                --   d.LGORT = s.LGORT,
                --   d.POTX1 = s.POTX1,
                --   d.SEQ = s.SEQ,
                --   d.XSEQNR = s.XSEQNR,
                --   d.I_TIME = s.I_TIME
                WHEN NOT MATCHED
                THEN
                INSERT (
                    PLANT_CODE, P_TYPE, NOTE, RESOURCE_NO,
                    IDNRK, STLAN, STLNR,
                    STLAL, BMEIN,
                    LKENZ, POSNR, MENGE,
                    P_NOTE, MEINS, AUSCH,
                    KZKUP, ALPOS, ALPGR,
                    EWAHR, SANFE, SANKA,
                    BEIKZ, DATUV, DATUV_TO,
                    AENNR, SORTF, LGORT,
                    POTX1, SEQ, XSEQNR, I_TIME)
                VALUES (
                    s.PLANT_CODE, s.P_TYPE, s.NOTE, s.RESOURCE_NO,
                    s.IDNRK, s.STLAN, s.STLNR,
                    s.STLAL, s.BMEIN,
                    s.LKENZ, s.POSNR, s.MENGE,
                    s.P_NOTE, s.MEINS, s.AUSCH,
                    s.KZKUP, s.ALPOS, s.ALPGR,
                    s.EWAHR, s.SANFE, s.SANKA,
                    s.BEIKZ, s.DATUV, s.DATUV_TO,
                    s.AENNR, s.SORTF, s.LGORT,
                    s.POTX1, s.SEQ, s.XSEQNR, s.I_TIME)
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "CopyBomMix", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                }
            }
        }

        private bool MixTimeChek(string vPlantCode, string vResourceNo, string vNote)
        {
            if (viewProduct.GetFocusedRowCellValue("UOM").ToString() == "EA")
                return true;

            SQL = $@"
            SELECT 1 FROM SAP_DI_ROUTING a WHERE a.ARBPL LIKE 'MXP%' AND a.RESOURCE_NO = '{vResourceNo}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
            {
                SQL = $@"
                SELECT b.MIX_TIME
                    , b.MIX_TIME2
                    , b.DRY_TIME
                    , b.MT_TIME
                    , b.FINAL_TIME
                    , b.LR_YN
                    , b.CR_YN
                    , b.REMARK_1
                    , b.REMARK_2
                FROM SAP_DI_PRODUCT b
                WHERE b.PLANT_CODE = '{vPlantCode}'
                    AND b.RESOURCE_NO = '{vResourceNo}'
                    AND NOT EXISTS (
                        SELECT 1
                        FROM SAP_IN_BOM_CONM a
                        WHERE a.PLANT_CODE = b.PLANT_CODE
                            AND a.RESOURCE_NO = b.RESOURCE_NO
                            AND a.NOTE = '{vNote}'
                            AND a.P_TYPE = '2'
                        )
                UNION ALL
                SELECT a.MIX_TIME
                    , a.MIX_TIME2
                    , a.DRY_TIME
                    , a.MT_TIME
                    , a.FINAL_TIME
                    , a.LR_YN
                    , a.CR_YN
                    , a.REMARK_1
                    , a.REMARK_2
                FROM SAP_IN_BOM_CONM a
                WHERE a.PLANT_CODE = '{vPlantCode}'
                    AND a.RESOURCE_NO = '{vResourceNo}'
                    AND a.NOTE = '{vNote}'
                    AND a.P_TYPE = '2'
                ";

                ds = Dbconn.conn.ExecutDataset(SQL);

                if ((ds.Tables[0].Rows.Count == 0 || (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["MIX_TIME"].ToString() == "")))
                {
                    ShowMessageBox.XtraShowWarning("배합시간이 없습니다. 저장 후 확정 해주세요.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 사전배합비에서 BOM배합비로 복사함
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool CopyBomMix(DataRow dr)
        {
            clsDevexpressGrid.GridEndEdit(viewBom);
            DataTable dt = (DataTable)gridBom.DataSource;

            DataRow drBom = dt.Rows[0];

            SQL = $@"
            MERGE INTO SAP_IN_BOM_CONM d
            USING (
              Select
                a.PLANT_CODE                              as PLANT_CODE,
                '2'                                       as P_TYPE,
                a.RESOURCE_NO                             as RESOURCE_NO,
                a.NOTE                                    as NOTE,
                '1'                                       as STLST,
                a.BMENG                                   as BMENG,
                '{drBom["MIX_TIME"]}'                     AS MIX_TIME,
                '{drBom["MIX_TIME2"]}'                    AS MIX_TIME2,
                '{drBom["DRY_TIME"]}'                     AS DRY_TIME,
                '{drBom["FINAL_TIME"]}'                   AS FINAL_TIME,
                '{drBom["LR_YN"]}'                        AS LR_YN,
                '{drBom["CR_YN"]}'                        AS CR_YN,
                '{drBom["MT_TIME"]}'                      AS MT_TIME,
                '{drBom["REMARK_1"]}'                     AS REMARK_1,
                '{drBom["REMARK_2"]}'                     AS REMARK_2,
                '{clsCommon.UserId}'                      AS EMPLOYEE_NO,
                a.H_YN                                    AS H_YN,
                SYSDATE                                   AS I_TIME,
                a.USE_YN                                  AS USE_YN,
                a.DATUV                                   AS DATUV,
                a.DATUV_TO                                AS DATUV_TO
            FROM SAP_IN_BOM_CONM a
                LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
              WHERE a.PLANT_CODE = '{dr["PLANT_CODE"]}' AND a.P_TYPE = '1' AND a.RESOURCE_NO = '{dr["RESOURCE_NO"]}' AND a.NOTE = '{dr["NOTE"]}'
                AND ROWNUM = 1) s
            ON
              (d.PLANT_CODE = s.PLANT_CODE AND
              d.P_TYPE = s.P_TYPE AND 
              d.RESOURCE_NO = s.RESOURCE_NO AND 
              d.NOTE = s.NOTE )
            WHEN MATCHED
            THEN
            UPDATE SET
              d.BMENG = s.BMENG,
              d.MIX_TIME = s.MIX_TIME,
              d.MIX_TIME2 = s.MIX_TIME2,
              d.DRY_TIME = s.DRY_TIME,
              d.FINAL_TIME = s.FINAL_TIME,
              d.LR_YN = s.LR_YN,
              d.CR_YN = s.CR_YN,
              d.MT_TIME = s.MT_TIME,
              d.REMARK_1 = s.REMARK_1,
              d.REMARK_2 = s.REMARK_2,
              d.EMPLOYEE_NO = s.EMPLOYEE_NO,
              d.H_YN = s.H_YN,
              d.I_TIME = s.I_TIME,
              d.USE_YN = s.USE_YN,
              d.DATUV = s.DATUV,
              d.DATUV_TO = s.DATUV_TO
            WHEN NOT MATCHED
            THEN
            INSERT (
              PLANT_CODE, P_TYPE, RESOURCE_NO, NOTE,
              BMENG, MIX_TIME, MIX_TIME2, DRY_TIME,
              FINAL_TIME, LR_YN, CR_YN, MT_TIME,
              REMARK_1, REMARK_2, EMPLOYEE_NO,
              H_YN, I_TIME, USE_YN,
              DATUV, DATUV_TO)
            VALUES (
              s.PLANT_CODE, s.P_TYPE, s.RESOURCE_NO, s.NOTE,
              s.BMENG, s.MIX_TIME, s.MIX_TIME2, s.DRY_TIME,
              s.FINAL_TIME, s.LR_YN, s.CR_YN, s.MT_TIME,
              s.REMARK_1, s.REMARK_2, s.EMPLOYEE_NO,
              s.H_YN, s.I_TIME, s.USE_YN,
              s.DATUV, s.DATUV_TO)
            ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "CopyBomMix", SQL);
                ShowMessageBox.XtraShowWarning("(SAP_IN_BOM_CONM)데이터 입력에 실패했습니다");
                return false;
            }

            SQL = $@"
            MERGE INTO SAP_IN_BOM_COND D
            USING (
            SELECT DISTINCT a.PLANT_CODE, '2' AS P_TYPE, a.NOTE, NVL(d.RESOURCE_NO_3, NVL(c.RESOURCE_NO, a.IDNRK)) AS IDNRK, 
                a.RESOURCE_NO, a.STLAN, a.STLNR, 
                a.STLAL, a.BMEIN, a.LKENZ, 
                a.POSNR, NVL((a.MENGE * (d.PART_P / 100)), a.MENGE) AS MENGE, 
                '' AS P_NOTE, a.MEINS, a.AUSCH, a.KZKUP, 
                a.ALPOS, a.ALPGR, a.EWAHR, 
                a.SANFE, a.SANKA, a.BEIKZ, 
                a.DATUV, a.DATUV_TO, a.AENNR, a.SORTF, 
                a.LGORT, a.POTX1, a.SEQ, 
                a.XSEQNR, SYSDATE AS I_TIME
            FROM SAP_IN_BOM_COND a
                LEFT OUTER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.BU_P = a.IDNRK
                LEFT OUTER JOIN SAP_IN_PRODUCT_CP d ON d.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO AND d.RESOURCE_NO_2 = a.IDNRK
            WHERE a.PLANT_CODE = '{dr["PLANT_CODE"]}' AND a.P_TYPE = '1'
                AND a.RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                AND a.NOTE = '{dr["NOTE"]}'
                AND a.IDNRK = '{dr["IDNRK"]}'
            ORDER BY IDNRK) s
            ON
              (d.PLANT_CODE = s.PLANT_CODE AND
              d.P_TYPE = s.P_TYPE AND 
              d.NOTE = s.NOTE AND 
              d.RESOURCE_NO = s.RESOURCE_NO AND 
              d.IDNRK = s.IDNRK )
            WHEN MATCHED
            THEN
            UPDATE SET
              d.STLAN = s.STLAN,
              d.STLNR = s.STLNR,
              d.STLAL = s.STLAL,
              d.BMEIN = s.BMEIN,
              d.LKENZ = s.LKENZ,
              d.POSNR = s.POSNR,
              d.MENGE = s.MENGE,
              d.P_NOTE = s.P_NOTE,
              d.MEINS = s.MEINS,
              d.AUSCH = s.AUSCH,
              d.KZKUP = s.KZKUP,
              d.ALPOS = s.ALPOS,
              d.ALPGR = s.ALPGR,
              d.EWAHR = s.EWAHR,
              d.SANFE = s.SANFE,
              d.SANKA = s.SANKA,
              d.BEIKZ = s.BEIKZ,
              d.DATUV = s.DATUV,
              d.DATUV_TO = s.DATUV_TO,
              d.AENNR = s.AENNR,
              d.SORTF = s.SORTF,
              d.LGORT = s.LGORT,
              d.POTX1 = s.POTX1,
              d.SEQ = s.SEQ,
              d.XSEQNR = s.XSEQNR,
              d.I_TIME = s.I_TIME
            WHEN NOT MATCHED
            THEN
            INSERT (
              PLANT_CODE, P_TYPE, NOTE, RESOURCE_NO,
              IDNRK, STLAN, STLNR,
              STLAL, BMEIN,
              LKENZ, POSNR, MENGE,
              P_NOTE, MEINS, AUSCH,
              KZKUP, ALPOS, ALPGR,
              EWAHR, SANFE, SANKA,
              BEIKZ, DATUV, DATUV_TO,
              AENNR, SORTF, LGORT,
              POTX1, SEQ, XSEQNR, I_TIME)
            VALUES (
              s.PLANT_CODE, s.P_TYPE, s.NOTE, s.RESOURCE_NO,
              s.IDNRK, s.STLAN, s.STLNR,
              s.STLAL, s.BMEIN,
              s.LKENZ, s.POSNR, s.MENGE,
              s.P_NOTE, s.MEINS, s.AUSCH,
              s.KZKUP, s.ALPOS, s.ALPGR,
              s.EWAHR, s.SANFE, s.SANKA,
              s.BEIKZ, s.DATUV, s.DATUV_TO,
              s.AENNR, s.SORTF, s.LGORT,
              s.POTX1, s.SEQ, s.XSEQNR, s.I_TIME)
            ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "CopyBomMix", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                return false;
            }

            return true;
        }
        #endregion

        #region Bom 배합비 이벤트
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            xMix_Search(vPalntCode, "2", vResourceNo.ToString(), cboNOTE.EditValue.ToString());
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string[] value = null;
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                using (m_Product child = new m_Product(viewProduct.GetFocusedRowCellValue("PLANT_CODE").ToString()))
                {
                    child.StartPosition = FormStartPosition.CenterParent;
                    if (child.ShowDialog() == DialogResult.OK)
                    {
                        value = child.SelectedValue;
                        //txtResult.Text = value; // 부모창에서 처리
                    }
                }

                if (value != null && value.Length > 0)
                {
                    clsDevexpressGrid.GridViewAddRow(viewBomMix);
                    //clsDevexpressGrid.GridViewAddRow(viewProduct);
                    //clsDevexpressGrid.GridViewAddRow(viewDictionary);
                    //clsDevexpressGrid.GridViewAddRow(viewBom);

                    viewBomMix.SetRowCellValue(viewBomMix.FocusedRowHandle, viewBomMix.Columns["PLANT_CODE"], viewProduct.GetFocusedRowCellValue("PLANT_CODE"));
                    viewBomMix.SetRowCellValue(viewBomMix.FocusedRowHandle, viewBomMix.Columns["IDNRK"], value[0]);
                    viewBomMix.SetRowCellValue(viewBomMix.FocusedRowHandle, viewBomMix.Columns["RESOURCE_NO"], viewProduct.GetFocusedRowCellValue("RESOURCE_NO"));
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowAdd_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewBomMix);
                clsDevexpressGrid.GridViewLastAddRowDelete(viewBomMix);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            DictionarySave();
        }

        private void DictionarySave()
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("본 배합비 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            try
            {
                clsDevexpressGrid.GridEndEdit(viewBomMix);
                DataTable DT = (DataTable)gridBomMix.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();
                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewBomMix);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewBomMix.FocusedColumn = viewBomMix.Columns[rValid]; // 이동할 컬럼명
                        viewBomMix.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(vResourceNo.ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("RESOURCE_NO", "제품을 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    if (!SetBomConUpsert(dr))
                    {
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.AcceptChanges();

                } //foreach

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowInformation("본 배합비를 저장 했습니다.");

                xMix_Search(vPalntCode, "2", vResourceNo.ToString(), cboNOTE.EditValue.ToString());
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "DictionarySave", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private bool SetBomConUpsert(DataRow dr)
        {
            if (string.IsNullOrEmpty(cboNOTE.EditValue?.ToString()))
            {
                dr.SetColumnError("NOTE", "배합비 버전을 선택해 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("NOTE"));
                return false;
            }

            if (string.IsNullOrEmpty(vResourceNo.ToString()))
            {
                dr.SetColumnError("RESOURCE_NO", "제품을 선택해 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                return false;
            }

            DateTime dtFrom = DateTime.MinValue;
            DateTime dtTo = DateTime.MaxValue;

            if (!dr["DATUV"].ToString().Equals("") && !dr["DATUV_TO"].ToString().Equals(""))
            {
                dtFrom = DateTime.Parse(dr["DATUV"].ToString());
                dtTo = DateTime.Parse(dr["DATUV_TO"].ToString());
            }
            else
            {
                dtFrom = DateTime.Today;
                dtTo = new DateTime(2099, 12, 31);
            }

            if (dr.RowState == DataRowState.Added)
            {
                SQL = $@"
                INSERT INTO SAP_IN_BOM_COND (
                    PLANT_CODE, P_TYPE, NOTE, 
                    RESOURCE_NO, IDNRK, STLAN, 
                    STLNR, STLAL, BMEIN, 
                    LKENZ, POSNR, MENGE, 
                    P_NOTE, MEINS, AUSCH, 
                    KZKUP, ALPOS, ALPGR, 
                    EWAHR, SANFE, SANKA, 
                    BEIKZ, DATUV, DATUV_TO, 
                    AENNR, SORTF, LGORT, 
                    POTX1, SEQ, XSEQNR) 
                VALUES ('{dr["PLANT_CODE"]}' ,'2', '{cboNOTE.EditValue}',
                    '{vResourceNo}', '{dr["IDNRK"]}', '{dr["STLAN"]}',
                    '{dr["STLNR"]}', '{dr["STLAL"]}', '{dr["BMEIN"]}', 
                    '{dr["LKENZ"]}', '{dr["POSNR"]}', '{dr["MENGE"]}', 
                    '{dr["P_NOTE"]}', '{dr["MEINS"]}', '{dr["AUSCH"]}',
                    '{dr["KZKUP"]}', '{dr["ALPOS"]}', '{dr["ALPGR"]}',
                    '{dr["EWAHR"]}', '{dr["SANFE"]}', '{dr["SANKA"]}',
                    '{dr["BEIKZ"]}', TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'), TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                    '{dr["AENNR"]}', '{dr["SORTF"]}', '{dr["LGORT"]}',
                    '{dr["POTX1"]}', '{dr["SEQ"]}', TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') )
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    clsLog.logSave(this.Text, "SetBomConUpsert", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return false;
                }
            }

            if (dr.RowState == DataRowState.Modified)
            {
                SQL = $@"
                UPDATE SAP_IN_BOM_COND
                SET    P_TYPE      = '{dr["P_TYPE"]}',
                       PLANT_CODE  = '{cboPlant_Code.EditValue}',
                       NOTE        = '{dr["NOTE"]}',
                       RESOURCE_NO = '{dr["RESOURCE_NO"]}',
                       IDNRK       = '{dr["IDNRK"]}',
                       STLAN       = '{dr["STLAN"]}',
                       STLNR       = '{dr["STLNR"]}',
                       STLAL       = '{dr["STLAL"]}',
                       BMEIN       = '{dr["BMEIN"]}',
                       LKENZ       = '{dr["LKENZ"]}',
                       POSNR       = '{dr["POSNR"]}',
                       MENGE       = '{dr["MENGE"]}',
                       P_NOTE      = '{dr["P_NOTE"]}',
                       MEINS       = '{dr["MEINS"]}',
                       AUSCH       = '{dr["AUSCH"]}',
                       KZKUP       = '{dr["KZKUP"]}',
                       ALPOS       = '{dr["ALPOS"]}',
                       ALPGR       = '{dr["ALPGR"]}',
                       EWAHR       = '{dr["EWAHR"]}',
                       SANFE       = '{dr["SANFE"]}',
                       SANKA       = '{dr["SANKA"]}',
                       BEIKZ       = '{dr["BEIKZ"]}',
                       DATUV       = TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                       DATUV_TO    = TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                       AENNR       = '{dr["AENNR"]}',
                       SORTF       = '{dr["SORTF"]}',
                       LGORT       = '{dr["LGORT"]}',
                       POTX1       = '{dr["POTX1"]}',
                       SEQ         = '{dr["SEQ"]}',
                       XSEQNR      = '{dr["XSEQNR"]}'
                WHERE  PLANT_CODE = '{cboPlant_Code.EditValue}' AND P_TYPE = '{dr["P_TYPE"]}' AND RESOURCE_NO = '{vResourceNo}' AND NOTE = '{cboNOTE.EditValue}'
                AND    IDNRK       = '{dr["IDNRK"]}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    clsLog.logSave(this.Text, "SetBomConUpsert", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// bom 배합비 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            int[] selectedRows = viewBomMix.GetSelectedRows();

            if (selectedRows.Length == 0)
            {
                ShowMessageBox.XtraShowWarning("삭제 할 자재를 먼저 선택 해주세요.");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewBomMix);

            if (DialogResult.Yes != ShowMessageBox.Confirm($"선택된 자재를 삭제하시겠습니까?"))
            {
                return;
            }

            try
            {
                Dbconn.conn.BeginTransaction();

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewBomMix.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    DELETE FROM SAP_IN_BOM_COND
                    WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND P_TYPE = '2'
                        AND RESOURCE_NO = '{viewBomMix.GetRowCellValue(rowHandle, "RESOURCE_NO")}'
                        AND IDNRK = '{viewBomMix.GetRowCellValue(rowHandle, "IDNRK")}'
                        AND NOTE = '{viewBomMix.GetRowCellValue(rowHandle, "NOTE")}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                        return;
                    }
                }

                SQL = $@"
                SELECT 1
                FROM SAP_IN_BOM_COND
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND P_TYPE = '2'
                        AND RESOURCE_NO = '{viewProduct.GetFocusedRowCellValue("RESOURCE_NO")}'
                        AND NOTE = '{cboNOTE.EditValue}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) == 0)
                {
                    SQL = $@"
                    DELETE FROM SAP_IN_BOM_COND
                    WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                        AND RESOURCE_NO = 'P{viewProduct.GetFocusedRowCellValue("RESOURCE_NO")}'
                        AND NOTE = '{cboNOTE.EditValue}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                        return;
                    }

                    SQL = $@"
                    DELETE FROM SAP_IN_BOM_CONM
                     WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND P_TYPE = '2'
                        AND RESOURCE_NO = '{viewProduct.GetFocusedRowCellValue("RESOURCE_NO")}'
                        AND NOTE = '{cboNOTE.EditValue}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                        return;
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowInformation("삭제 되었습니다.");

                xMix_Search(vPalntCode, "2", vResourceNo.ToString(), cboNOTE.EditValue.ToString());
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click()", ex);
                ShowMessageBox.XtraShowWarning("삭제를 실행하는 도중 에러가 발생했습니다");
            }
        }

        /// <summary>
        /// 배합비 확정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBomMixConfirm_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (!MixTimeChek(viewProduct.GetFocusedRowCellValue("PLANT_CODE").ToString(), vResourceNo, cboNOTE.EditValue.ToString())) return;

            clsDevexpressGrid.GridEndEdit(viewBomMix);

            DataTable dt = (DataTable)gridBomMix.DataSource;

            DataRow[] dr = dt.Select("USE_FLAG = 'N'");

            if (dr.Length > 0)
            {
                ShowMessageBox.XtraShowWarning($"[{clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()).Rows[0]["NAME"].ToString()}] 효력일이 초과 되었습니다.");
                return;
            }

            SQL = $@"
            UPDATE SAP_IN_BOM_CONM
            SET    STLST       = CASE WHEN STLST = '2' THEN '1' ELSE '2' END
            WHERE  P_TYPE      = '2'
                AND    NOTE        = '{cboNOTE.EditValue}'
                AND    RESOURCE_NO = '{vResourceNo}'
            ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "btnBomMixConfirm_Click", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                return;
            }

            //layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            ShowMessageBox.XtraShowInformation($"{btnBomMixConfirm.Text} 되었습니다.");

            xMix_Search(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), "2", vResourceNo, cboNOTE.EditValue?.ToString());
        }
        #endregion

        #region 배합비 설정 정보
        private void btn_rowAdd11_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridViewAddRow(viewBom);

                viewBom.SetRowCellValue(viewBom.FocusedRowHandle, viewBom.Columns["P_TYPE"], "2");
                viewBom.SetRowCellValue(viewBom.FocusedRowHandle, viewBom.Columns["RESOURCE_NO"], vResourceNo);
                viewBom.SetRowCellValue(viewBom.FocusedRowHandle, viewBom.Columns["NOTE"], cboNOTE.EditValue);
                viewBom.SetRowCellValue(viewBom.FocusedRowHandle, viewBom.Columns["USE_YN"], "Y");
                viewBom.SetRowCellValue(viewBom.FocusedRowHandle, viewBom.Columns["H_YN"], "M");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowAdd_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_rowDel11_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewBom);
                clsDevexpressGrid.GridViewLastAddRowDelete(viewBom);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_save11_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("배합설정정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            try
            {
                clsDevexpressGrid.GridEndEdit(viewBom);
                DataTable DT = (DataTable)gridBom.DataSource;

                if (viewBomMix.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("본 배합비가 없습니다.");
                    return;
                }

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();
                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewBom);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewBom.FocusedColumn = viewBom.Columns[rValid]; // 이동할 컬럼명
                        viewBom.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(vResourceNo.ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("RESOURCE_NO", "제품을 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    if (!SetBomUpsert(dr))
                    {
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.AcceptChanges();

                } //foreach

                Dbconn.conn.Commit();

                viewBom.RefreshData();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save11_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private bool SetBomUpsert(DataRow dr)
        {
            SQL = $@"
            UPDATE SAP_IN_BOM_CONM
            SET    BMENG       = '{dr["BMENG"]}',
                    MIX_TIME    = '{dr["MIX_TIME"]}',
                    MIX_TIME2    = '{dr["MIX_TIME2"]}',
                    DRY_TIME    = '{dr["DRY_TIME"]}',
                    FINAL_TIME  = '{dr["FINAL_TIME"]}',
                    LR_YN       = '{dr["LR_YN"]}',
                    CR_YN       = '{dr["CR_YN"]}',
                    REMARK_1    = '{dr["REMARK_1"]}',
                    REMARK_2    = '{dr["REMARK_2"]}',
                    EMPLOYEE_NO = '{clsCommon.UserId}',
                    I_TIME      = SYSDATE
            WHERE P_TYPE = '{dr["P_TYPE"]}' AND RESOURCE_NO = '{dr["RESOURCE_NO"]}' AND NOTE = '{dr["NOTE"]}'
            ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "SetBomUpsert", SQL);
                ShowMessageBox.XtraShowWarning("데이터  입력에 실패했습니다");
                return false;
            }

            ShowMessageBox.XtraShowWarning("배합비 설정정보를 저장 했습니다");

            return true;
        }

        private void btn_delete11_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void viewProduct_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (viewProduct == null)
                    return;

                string bomType = viewProduct.GetRowCellValue(e.RowHandle, viewProduct.Columns["BOM_TYPE"]).ToString();

                //if (bomType == "1") //완료
                //{
                //    e.Appearance.BackColor = Color.LightCyan;
                //    e.Appearance.ForeColor = Color.DarkGreen;
                //    e.Appearance.FontStyleDelta = FontStyle.Bold;
                //}

                if (bomType == "2") //완료
                {
                    e.Appearance.BackColor = Color.LightGreen;
                    e.Appearance.ForeColor = Color.DarkGreen;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }

                string checkDatuv = viewProduct.GetRowCellValue(e.RowHandle, viewDictionary.Columns["CHECK_DATUV"]).ToString();

                if (checkDatuv == "M") //완료
                {
                    e.Appearance.BackColor = Color.LightGray;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                //else if (checkDatuv == "N") //완료
                //{
                //    e.Appearance.BackColor = Color.Red;
                //    e.Appearance.ForeColor = Color.Black;
                //    e.Appearance.FontStyleDelta = FontStyle.Bold;
                //}

                if (e.RowHandle == viewProduct.FocusedRowHandle)
                {
                    e.Appearance.BackColor = Color.LightPink;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void viewDictionary_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            string condition = viewDictionary.GetRowCellValue(e.RowHandle, viewDictionary.Columns["CHECK_DATUV"]).ToString();

            if (condition == "N") //완료
            {
                //e.Appearance.BackColor = Color.Red;
                //e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
        }

        private void viewBomMix_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            string condition = viewBomMix.GetRowCellValue(e.RowHandle, viewBomMix.Columns["CHECK_DATUV"]).ToString();

            if (condition == "N") //완료
            {
                //e.Appearance.BackColor = Color.Red;
                //e.Appearance.ForeColor = Color.Black;
                //e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
        }

        /// <summary>
        /// 사전배합비 체크 시 효력 체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewDictionary_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            bool bProgressType = false;
            int[] selectedRows = viewDictionary.GetSelectedRows();

            foreach (int rowHandle in selectedRows)
            {
                var dr = viewDictionary.GetDataRow(rowHandle);

                dr.ClearErrors();

                //if (dr["CHECK_DATUV"].ToString() == "N")
                //{
                //    if (!bProgressType)
                //    {
                //        //if (DialogResult.Yes != ShowMessageBox.Confirm("유효효력이 종료된 자재가 있습니다. 계속 진행 하시겠습니까?"))
                //        //{
                //        //    viewDictionary.ClearSelection();
                //        //    return;
                //        //}

                //        bProgressType = true;
                //    }

                //    //ShowMessageBox.XtraShowWarning($"{clsCommon.GetResourceName(dr["IDNRK"].ToString())} 자재는 효력이 종료된 자재 입니다.");
                //    viewDictionary.UnselectRow(rowHandle);
                //}
            }
        }

        private void frm_Pdetail_KeyDown(object sender, KeyEventArgs e)
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
            gridDictionary.Focus();
            viewDictionary.FocusedRowHandle = 0;
            viewDictionary.FocusedColumn = viewDictionary.VisibleColumns[0];
        }

        private void gridDicscboRESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            clsDevexpressGrid.ItemLookUpEditSetup(gridDiccboP_NOTE, clsCommon.getNote(textEditor.EditValue?.ToString()));

            viewDictionary.SetRowCellValue(viewDictionary.FocusedRowHandle, "IDNRK", textEditor.EditValue);
        }

        #region 사전배합비 생성 
        ///// <summary>
        ///// 조회된 사전배합비 목록을 신규 배합비 버전으로 복사
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnDicCopy_Click(object sender, EventArgs e)
        //{
        //    //if (string.IsNullOrWhiteSpace(txtNote.EditValue?.ToString()))
        //    //{
        //    //    ShowMessageBox.XtraShowWarning("신규 배합비 버전을 생성하거나 입력해주세요.");

        //    //    return;
        //    //}

        //    if (!clsCommon.Auth_Form_Function(authDs, "W"))
        //    {
        //        ShowMessageBox.XtraShowInformation("권한이 없습니다");
        //        return;
        //    }

        //    if (DialogResult.Yes != ShowMessageBox.Confirm("신규 버전을 생성 하시겠습니까?"))
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        clsDevexpressGrid.GridEndEdit(viewDictionary);
        //        DataTable DT = (DataTable)gridDictionary.DataSource;

        //        if (DT.Rows.Count == 0)
        //        {
        //            ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
        //            return;
        //        }

        //        if (DT == null)
        //        {
        //            return;
        //        }

        //        Dbconn.conn.BeginTransaction();
        //        foreach (DataRow dr in DT.Rows)
        //        {
        //            if (dr["CHECK_DATUV"].ToString() == "N")
        //                continue;

        //            dr.ClearErrors();

        //            if (string.IsNullOrEmpty(vResourceNo.ToString()))
        //            {
        //                Dbconn.conn.Rollback();
        //                dr.SetColumnError("RESOURCE_NO", "제품을 선택하여 주세요");
        //                ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
        //                return;
        //            }

        //            if (!SetBomConCopy(dr))
        //            {
        //                Dbconn.conn.Rollback();
        //                return;
        //            }

        //            dr.AcceptChanges();

        //        } //foreach

        //        Dbconn.conn.Commit();

        //        ShowMessageBox.XtraShowInformation("신규 버전이 생성 되었습니다.");
        //        viewDictionary.RefreshData();

        //        GetMixVersion(cboPlant_Code.EditValue?.ToString(), vResourceNo);

        //        //cboNOTE.EditValue = txtNote.EditValue?.ToString();

        //        xMix_Search(vPalntCode, "1", vResourceNo, cboNOTE.EditValue?.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        clsLog.logSave(this, "btnDicCopy_Click", ex);
        //        ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
        //    }
        //}

        //private bool SetBomConCopy(DataRow dr)
        //{
        //    SQL = $@"
        //    MERGE INTO SAP_IN_BOM_CONM T  -- 대상 테이블
        //    USING (SELECT '{cboPlant_Code.EditValue}' AS PLANT_CODE
        //            , '1' AS P_TYPE
        //            , '{vResourceNo}' AS RESOURCE_NO
        //            , '{txtNote.EditValue}' AS NOTE
        //            FROM DUAL) S  -- 입력할 값
        //    ON (T.P_TYPE = S.P_TYPE AND T.PLANT_CODE = S.PLANT_CODE AND T.RESOURCE_NO = S.RESOURCE_NO AND T.NOTE = S.NOTE)  -- 키값 기준 비교
        //    WHEN NOT MATCHED THEN  
        //        INSERT (PLANT_CODE, P_TYPE, NOTE, USE_YN, RESOURCE_NO, EMPLOYEE_NO, H_YN, I_TIME)  
        //        VALUES (S.PLANT_CODE, S.P_TYPE, S.NOTE, 'Y', S.RESOURCE_NO, '{clsCommon.UserId}', 'M', SYSDATE)
        //    ";

        //    Dbconn.conn.SQLrun(SQL);

        //    SQL = $@"
        //    MERGE INTO SAP_IN_BOM_COND d
        //    USING (
        //      Select
        //        '{dr["P_TYPE"]}'        as P_TYPE,
        //        '{cboPlant_Code.EditValue}'    as PLANT_CODE,
        //        '{txtNote.EditValue}'   as NOTE,
        //        '{dr["RESOURCE_NO"]}'   as RESOURCE_NO,
        //        '{dr["IDNRK"]}'         as IDNRK,
        //        '{dr["STLAN"]}'         as STLAN,
        //        '{dr["STLNR"]}'         as STLNR,
        //        '{dr["STLAL"]}'         as STLAL,
        //        '{dr["BMEIN"]}'         as BMEIN,
        //        '{dr["LKENZ"]}'         as LKENZ,
        //        '{dr["POSNR"]}'         as POSNR,
        //        '{dr["MENGE"]}'         as MENGE,
        //        '{dr["P_NOTE"]}'        as P_NOTE,
        //        '{dr["MEINS"]}'         as MEINS,
        //        '{dr["AUSCH"]}'         as AUSCH,
        //        '{dr["KZKUP"]}'         as KZKUP,
        //        '{dr["ALPOS"]}'         as ALPOS,
        //        '{dr["ALPGR"]}'         as ALPGR,
        //        '{dr["EWAHR"]}'         as EWAHR,
        //        '{dr["SANFE"]}'         as SANFE,
        //        '{dr["SANKA"]}'         as SANKA,
        //        '{dr["BEIKZ"]}'         as BEIKZ,
        //        '{dr["DATUV"]}'         as DATUV,
        //        '{dr["DATUV_TO"]}'      as DATUV_TO,
        //        '{dr["AENNR"]}'         as AENNR,
        //        '{dr["SORTF"]}'         as SORTF,
        //        '{dr["LGORT"]}'         as LGORT,
        //        '{dr["POTX1"]}'         as POTX1,
        //        '{dr["SEQ"]}'           as SEQ,
        //        '{dr["XSEQNR"]}'        as XSEQNR
        //      From Dual) s
        //    ON
        //      (d.P_TYPE = s.P_TYPE and 
        //      d.PLANT_CODE = s.PLANT_CODE and 
        //      d.NOTE = s.NOTE and 
        //      d.RESOURCE_NO = s.RESOURCE_NO and 
        //      d.IDNRK = s.IDNRK )
        //    WHEN MATCHED
        //    THEN
        //    UPDATE SET
        //      d.STLAN = s.STLAN,
        //      d.STLNR = s.STLNR,
        //      d.STLAL = s.STLAL,
        //      d.BMEIN = s.BMEIN,
        //      d.LKENZ = s.LKENZ,
        //      d.POSNR = s.POSNR,
        //      d.MENGE = s.MENGE,
        //      d.P_NOTE = s.P_NOTE,
        //      d.MEINS = s.MEINS,
        //      d.AUSCH = s.AUSCH,
        //      d.KZKUP = s.KZKUP,
        //      d.ALPOS = s.ALPOS,
        //      d.ALPGR = s.ALPGR,
        //      d.EWAHR = s.EWAHR,
        //      d.SANFE = s.SANFE,
        //      d.SANKA = s.SANKA,
        //      d.BEIKZ = s.BEIKZ,
        //      d.DATUV = s.DATUV,
        //      d.DATUV_TO = s.DATUV_TO,
        //      d.AENNR = s.AENNR,
        //      d.SORTF = s.SORTF,
        //      d.LGORT = s.LGORT,
        //      d.POTX1 = s.POTX1,
        //      d.SEQ = s.SEQ,
        //      d.XSEQNR = s.XSEQNR
        //    WHEN NOT MATCHED
        //    THEN
        //    INSERT (
        //      P_TYPE, PLANT_CODE, NOTE,
        //      RESOURCE_NO, IDNRK, STLAN,
        //      STLNR, STLAL, BMEIN,
        //      LKENZ, POSNR, MENGE,
        //      P_NOTE, MEINS, AUSCH,
        //      KZKUP, ALPOS, ALPGR,
        //      EWAHR, SANFE, SANKA,
        //      BEIKZ, DATUV, DATUV_TO,
        //      AENNR, SORTF, LGORT,
        //      POTX1, SEQ, XSEQNR)
        //    VALUES (
        //      s.P_TYPE, s.PLANT_CODE, s.NOTE,
        //      s.RESOURCE_NO, s.IDNRK, s.STLAN,
        //      s.STLNR, s.STLAL, s.BMEIN,
        //      s.LKENZ, s.POSNR, s.MENGE,
        //      s.P_NOTE, s.MEINS, s.AUSCH,
        //      s.KZKUP, s.ALPOS, s.ALPGR,
        //      s.EWAHR, s.SANFE, s.SANKA,
        //      s.BEIKZ, s.DATUV, s.DATUV_TO,
        //      s.AENNR, s.SORTF, s.LGORT,
        //      s.POTX1, s.SEQ, s.XSEQNR)
        //    ";

        //    if (Dbconn.conn.SQLrun(SQL) < 1)
        //    {
        //        clsLog.logSave(this.Text, "SetBomConCopy", SQL);
        //        ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
        //        return false;
        //    }

        //    return true;
        //} 
        #endregion

        private void cboNOTE_EditValueChanged(object sender, EventArgs e)
        {
            gridDictionary.DataSource = null;
            gridBomMix.DataSource = null;

            TextEdit textEditor = (TextEdit)sender;

            xMix_Search(vPalntCode, sTapIdx, vResourceNo, textEditor.EditValue?.ToString());
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            //clsDevexpressUtil.ItemSearchLookUpEditSetup(scboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", 1));

            XMain_Search();
        }

        private void scboRESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            //clsDevexpressUtil.ItemLookUpEditSetup(cboSearchNote, clsCommon.getNote(cboPlant_Code.EditValue?.ToString(), scboRESOURCE_NO.EditValue?.ToString()), "", false, 0, true);

            XMain_Search();
        }

        /// <summary>
        /// 선택 배합비 확정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBomMixCon_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowWarning("권한이 없습니다");
                return;
            }

            if (!MixTimeChek(viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), vResourceNo, cboNOTE.EditValue.ToString())) return;

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택한 제품의 배합비 정보를 본 배합비로 확정 하시겠습니까?"))
            {
                return;
            }

            if (cboNOTE.EditValue.Equals(""))
            {
                ShowMessageBox.XtraShowWarning("배합비 버전이 없습니다. 배합비 버전을 먼저 생성해주세요.");
                return;
            }

            //if (viewProduct.GetFocusedRowCellValue("CHECK_DATUV").ToString() == "N")
            //{
            //    ShowMessageBox.XtraShowWarning("효력 유효일이 초과된 배합비는 사용할 수 없습니다.");
            //    return;
            //}

            try
            {
                int[] selectedRows = viewProduct.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("복사 할 자재를 먼저 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewProduct.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(vResourceNo.ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("RESOURCE_NO", "제품을 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    Dbconn.conn.BeginTransaction();

                    if (!SelectConBomMix(dr))
                    {
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.AcceptChanges();

                } //foreach

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowInformation("배합비가 모두 확정 되었습니다.");

                viewBomMix.RefreshData();

                XMain_Search();
                xBom_Search(vPalntCode, vResourceNo.ToString(), cboNOTE.EditValue.ToString());
                xMix_Search(vPalntCode, "2", vResourceNo.ToString(), cboNOTE.EditValue.ToString());

                tabMix.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btnBomMixCopy_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        /// <summary>
        /// 사전배합비에서 BOM배합비로 확정함
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool SelectConBomMix(DataRow dr)
        {
            if (dr["RESOURCE_NO"].ToString().Contains("P"))
                return true;

            SQL = $@"
            DELETE FROM SAP_IN_BOM_COND
            WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND RESOURCE_NO = 'P{dr["RESOURCE_NO"]}' AND NOTE = '{dr["NOTE"]}'
            ";

            Dbconn.conn.SQLrun(SQL);

            SQL = $@"
            DELETE FROM SAP_IN_BOM_COND
            WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND P_TYPE = '2' AND RESOURCE_NO = '{dr["RESOURCE_NO"]}' AND NOTE = '{dr["NOTE"]}'
            ";

            Dbconn.conn.SQLrun(SQL);

            SQL = $@"
            DELETE FROM SAP_IN_BOM_CONM
            WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND RESOURCE_NO = 'P{dr["RESOURCE_NO"]}' AND NOTE = '{dr["NOTE"]}'
            ";

            Dbconn.conn.SQLrun(SQL);

            SQL = $@"
            DELETE FROM SAP_IN_BOM_CONM
            WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND P_TYPE = '2' AND RESOURCE_NO = '{dr["RESOURCE_NO"]}' AND NOTE = '{dr["NOTE"]}'
            ";

            Dbconn.conn.SQLrun(SQL);

            Dbconn.conn.BeginTransaction();

            SQL = $@"
            MERGE INTO SAP_IN_BOM_CONM d
            USING (
              SELECT
                a.PLANT_CODE                              as PLANT_CODE,
                '2'                                     as P_TYPE,
                a.RESOURCE_NO                             as RESOURCE_NO,
                a.NOTE                                    as NOTE,
                '2'                                     as STLST,
                a.BMENG                                   as BMENG,
                b.MIX_TIME                                as MIX_TIME,
                b.MIX_TIME2                               as MIX_TIME2,
                b.DRY_TIME                                as DRY_TIME,
                b.FINAL_TIME                              as FINAL_TIME,
                b.LR_YN                                   as LR_YN,
                b.CR_YN                                   as CR_YN,
                b.REMARK_1                                as REMARK_1,
                b.REMARK_2                                as REMARK_2,
                '{clsCommon.UserId}'                    as EMPLOYEE_NO,
                a.H_YN                                    as H_YN,
                SYSDATE                                 as I_TIME,
                a.USE_YN                                  as USE_YN,
                a.DATUV                                   as DATUV,
                a.DATUV_TO                                as DATUV_TO
              FROM SAP_IN_BOM_CONM a
                LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
              WHERE a.PLANT_CODE = '{dr["PLANT_CODE"]}' AND a.P_TYPE = '1' AND a.RESOURCE_NO IN '{dr["RESOURCE_NO"]}' AND a.NOTE = '{dr["NOTE"]}'
                AND ROWNUM = 1) s
            ON
              (d.PLANT_CODE = s.PLANT_CODE AND
              d.P_TYPE = s.P_TYPE AND 
              d.RESOURCE_NO = s.RESOURCE_NO AND 
              d.NOTE = s.NOTE )
            --WHEN MATCHED
            --THEN
            --UPDATE SET
            --  d.BMENG = s.BMENG,
            --  d.MIX_TIME = s.MIX_TIME,
            --  d.DRY_TIME = s.DRY_TIME,
            --  d.FINAL_TIME = s.FINAL_TIME,
            --  d.LR_YN = s.LR_YN,
            --  d.CR_YN = s.CR_YN,
            --  d.REMARK_1 = s.REMARK_1,
            --  d.REMARK_2 = s.REMARK_2,
            --  d.EMPLOYEE_NO = s.EMPLOYEE_NO,
            --  d.H_YN = s.H_YN,
            --  d.I_TIME = s.I_TIME,
            --  d.USE_YN = s.USE_YN,
            --  d.DATUV = s.DATUV,
            --  d.DATUV_TO = s.DATUV_TO
            WHEN NOT MATCHED
            THEN
            INSERT (
              PLANT_CODE, P_TYPE, RESOURCE_NO, NOTE,
              STLST, BMENG, MIX_TIME, MIX_TIME2, DRY_TIME,
              FINAL_TIME, LR_YN, CR_YN,
              REMARK_1, REMARK_2, EMPLOYEE_NO,
              H_YN, I_TIME, USE_YN,
              DATUV, DATUV_TO)
            VALUES (
              s.PLANT_CODE, s.P_TYPE, s.RESOURCE_NO, s.NOTE,
              S.STLST, s.BMENG, s.MIX_TIME, s.MIX_TIME2, s.DRY_TIME,
              s.FINAL_TIME, s.LR_YN, s.CR_YN,
              s.REMARK_1, s.REMARK_2, s.EMPLOYEE_NO,
              s.H_YN, s.I_TIME, s.USE_YN,
              s.DATUV, s.DATUV_TO)
            ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this.Text, "SelectConBomMix", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                return false;
            }

            SQL = $@"
            MERGE INTO SAP_IN_BOM_COND D
            USING (
            SELECT a.PLANT_CODE, '2' AS P_TYPE, a.NOTE, NVL(d.RESOURCE_NO_3, NVL(c.RESOURCE_NO, a.IDNRK)) AS IDNRK, 
                a.RESOURCE_NO, a.STLAN, a.STLNR, 
                a.STLAL, a.BMEIN, a.LKENZ, 
                a.POSNR, NVL((a.MENGE * (d.PART_P / 100)), a.MENGE) AS MENGE, 
                '' AS P_NOTE, a.MEINS, a.AUSCH, a.KZKUP, 
                a.ALPOS, a.ALPGR, a.EWAHR, 
                a.SANFE, a.SANKA, a.BEIKZ, 
                a.DATUV, a.DATUV_TO, a.AENNR, a.SORTF, 
                a.LGORT, a.POTX1, a.SEQ, 
                a.XSEQNR, SYSDATE AS I_TIME
            FROM SAP_IN_BOM_COND a
                LEFT OUTER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.BU_P = a.IDNRK
                LEFT OUTER JOIN SAP_IN_PRODUCT_CP d ON d.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO AND d.RESOURCE_NO_2 = a.IDNRK
            WHERE a.PLANT_CODE = '{dr["PLANT_CODE"]}' AND a.P_TYPE = '1'
                AND a.RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                AND a.NOTE = '{dr["NOTE"]}'
                AND NOT EXISTS (
                                SELECT 1
                                FROM PREMIX_MASTER b
                                WHERE b.PLANT_CODE = a.PLANT_CODE AND b.PRESOURCE_NO = 'P{dr["RESOURCE_NO"]}' AND b.PIDNRK = a.IDNRK
                                )
            ORDER BY IDNRK) s
            ON
              (d.PLANT_CODE = s.PLANT_CODE AND
              d.P_TYPE = s.P_TYPE AND 
              d.NOTE = s.NOTE AND 
              d.RESOURCE_NO = s.RESOURCE_NO AND 
              d.IDNRK = s.IDNRK )
            --WHEN MATCHED
            --THEN
            --UPDATE SET
            --  d.STLAN = s.STLAN,
            --  d.STLNR = s.STLNR,
            --  d.STLAL = s.STLAL,
            --  d.BMEIN = s.BMEIN,
            --  d.LKENZ = s.LKENZ,
            --  d.POSNR = s.POSNR,
            --  d.MENGE = s.MENGE,
            --  d.P_NOTE = s.P_NOTE,
            --  d.MEINS = s.MEINS,
            --  d.AUSCH = s.AUSCH,
            --  d.KZKUP = s.KZKUP,
            --  d.ALPOS = s.ALPOS,
            --  d.ALPGR = s.ALPGR,
            --  d.EWAHR = s.EWAHR,
            --  d.SANFE = s.SANFE,
            --  d.SANKA = s.SANKA,
            --  d.BEIKZ = s.BEIKZ,
            --  d.DATUV = s.DATUV,
            --  d.DATUV_TO = s.DATUV_TO,
            --  d.AENNR = s.AENNR,
            --  d.SORTF = s.SORTF,
            --  d.LGORT = s.LGORT,
            --  d.POTX1 = s.POTX1,
            --  d.SEQ = s.SEQ,
            --  d.XSEQNR = s.XSEQNR,
            --  d.I_TIME = s.I_TIME
            WHEN NOT MATCHED
            THEN
            INSERT (
              PLANT_CODE, P_TYPE, NOTE, RESOURCE_NO,
              IDNRK, STLAN, STLNR,
              STLAL, BMEIN,
              LKENZ, POSNR, MENGE,
              P_NOTE, MEINS, AUSCH,
              KZKUP, ALPOS, ALPGR,
              EWAHR, SANFE, SANKA,
              BEIKZ, DATUV, DATUV_TO,
              AENNR, SORTF, LGORT,
              POTX1, SEQ, XSEQNR, I_TIME)
            VALUES (
              s.PLANT_CODE, s.P_TYPE, s.NOTE, s.RESOURCE_NO,
              s.IDNRK, s.STLAN, s.STLNR,
              s.STLAL, s.BMEIN,
              s.LKENZ, s.POSNR, s.MENGE,
              s.P_NOTE, s.MEINS, s.AUSCH,
              s.KZKUP, s.ALPOS, s.ALPGR,
              s.EWAHR, s.SANFE, s.SANKA,
              s.BEIKZ, s.DATUV, s.DATUV_TO,
              s.AENNR, s.SORTF, s.LGORT,
              s.POTX1, s.SEQ, s.XSEQNR, s.I_TIME)
            ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this.Text, "SelectConBomMix", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                return false;
            }

            Dbconn.conn.Commit();

            SQL = $@"
            SELECT 
                a.PLANT_CODE
                , a.PRESOURCE_NO
                , a.PIDNRK
                , a.I_USER 
                , a.I_TIME
            FROM PREMIX_MASTER a
            WHERE a.PRESOURCE_NO = 'P{dr["RESOURCE_NO"]}'
            ";

            DataSet dsPremix = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(dsPremix) > 0)
            {
                CreatePremix(dr["PLANT_CODE"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), dr["NOTE"]?.ToString());

                // 프리믹스 배합비 포함한 본 배합비 생성
                if (!SetBomMixCreate(dr["PLANT_CODE"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), dr["NOTE"]?.ToString())) return false;

                // 프리믹스 배합비 100% 만들기
                if (!SetPreMixRate(dr["PLANT_CODE"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), dr["NOTE"]?.ToString())) return false;
            }

            SetResorce_CP(dr["PLANT_CODE"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), dr["NOTE"]?.ToString());

            return true;
        }

        /// <summary>
        /// 프리믹스 배합비 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreMixDelete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            SQL = $@"
            SELECT 1
            FROM SAP_IN_BOM_COND
            WHERE P_TYPE = '2' AND PLANT_CODE = '{viewProduct.GetFocusedRowCellValue("PLANT_CODE")}'
                AND RESOURCE_NO = '{vResourceNo.Replace("P", "")}' AND NOTE = '{cboNOTE.EditValue}'
                AND ROWNUM = 1
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ShowMessageBox.XtraShowWarning("본 배합비가 존재 합니다. 삭제 후 진행 해주세요.");
                return;
            }

            SQL = $@"
            DELETE
            FROM SAP_IN_BOM_COND a
            WHERE a.PLANT_CODE = '{viewProduct.GetFocusedRowCellValue("PLANT_CODE")}'
                AND a.RESOURCE_NO  = '{vResourceNo}'
                AND a.NOTE = '{cboNOTE.EditValue}'
            ";

            if (Dbconn.conn.SQLrun(SQL) < 0)
            {
                clsLog.logSave(this.Text, "btnPreMixDelete_Click", SQL);
                ShowMessageBox.XtraShowWarning("프리믹스 레시피 삭제에 실패했습니다");
                return;
            }

            SQL = $@"
            DELETE
            FROM SAP_IN_BOM_CONM a
            WHERE a.PLANT_CODE = '{viewProduct.GetFocusedRowCellValue("PLANT_CODE")}'
                AND a.RESOURCE_NO  = '{vResourceNo}'
                AND a.NOTE = '{cboNOTE.EditValue}'
            ";

            if (Dbconn.conn.SQLrun(SQL) < 0)
            {
                clsLog.logSave(this.Text, "btnPreMixDelete_Click", SQL);
                ShowMessageBox.XtraShowWarning("프리믹스 배합비 삭제에 실패했습니다");
                return;
            }

            XMain_Search();
            xBom_Search(vPalntCode, vResourceNo.ToString(), cboNOTE.EditValue.ToString());
            xMix_Search(vPalntCode, "1", vResourceNo.ToString(), cboNOTE.EditValue.ToString());

            tabMix.SelectedTabPageIndex = 0;
        }

        /// <summary>
        /// 프리믹스 배합비 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreMixCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                int[] selectedRows = viewDictionary.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("프리믹스 배합 품목을 먼저 선택 해주세요.");
                    return;
                }

                string sPlantCode = viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                string sResourceNo = viewProduct.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();
                string sNote = cboNOTE.EditValue?.ToString();

                if (sResourceNo.Contains("P"))
                {
                    ShowMessageBox.XtraShowWarning("프리믹스 배합 품목입니다. 제품을 선택 해주세요.");
                    return;
                }

                CreatePremix(selectedRows[0], sPlantCode, sResourceNo, sNote);

                viewBomMix.RefreshData();

                XMain_Search();
                xBom_Search(vPalntCode, vResourceNo.ToString(), cboNOTE.EditValue.ToString());
                xMix_Search(vPalntCode, "2", vResourceNo.ToString(), cboNOTE.EditValue.ToString());

                tabMix.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btnPreMixCreate_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        /// <summary>
        /// 프리믹스 배합비 생성
        /// </summary>
        /// <param name="selectedRows"></param>
        /// <param name="sPlantCode"></param>
        /// <param name="sResourceNo"></param>
        /// <param name="sNote"></param>
        private void CreatePremix(int rowHandle, string sPlantCode, string sResourceNo, string sNote)
        {
            // 프리믹스 기준정보 만들기
            if (!PremixBaseDataCreate(sPlantCode, sResourceNo, sNote)) return;

            var dr = viewDictionary.GetDataRow(rowHandle);

            dr.ClearErrors();

            if (string.IsNullOrEmpty(vResourceNo.ToString()))
            {
                Dbconn.conn.Rollback();
                dr.SetColumnError("RESOURCE_NO", "제품을 선택하여 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                return;
            }

            Dbconn.conn.BeginTransaction();

            // 프리믹스 배합비 생성
            if (!PreMixCreate(dr))
            {
                Dbconn.conn.Rollback();
                return;
            }

            dr.AcceptChanges();

            Dbconn.conn.Commit();
        }

        /// <summary>
        /// 프리믹스 배합비를 포함한 본 배합비 생성
        /// </summary>
        /// <param name="selectedRows"></param>
        private bool SetBomMixCreate(string sPlantCode, string sResourceNo, string sNote)
        {
            Dbconn.conn.BeginTransaction();
            // 프리믹스 배합비를 본배합비에 추가
            SQL = $@"
                INSERT INTO SAP_IN_BOM_COND (
                        P_TYPE
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
                    , KZKUP
                    , ALPOS
                    , ALPGR
                    , EWAHR
                    , SANFE
                    , SANKA
                    , BEIKZ
                    , DATUV
                    , DATUV_TO
                    , AENNR
                    , SORTF
                    , LGORT
                    , POTX1
                    , SEQ
                    , XSEQNR
                    , I_TIME
                ) SELECT   '2'
                    , a.PLANT_CODE
                    , a.NOTE
                    , SUBSTR(a.RESOURCE_NO, 2) AS RESOURCE_NO
                    , a.RESOURCE_NO AS IDNRK
                    , a.STLAN
                    , a.STLNR
                    , a.STLAL
                    , a.BMEIN
                    , a.LKENZ
                    , '0010' AS POSNR
                    , SUM(a.MENGE) AS MENGE
                    , a.P_NOTE
                    , a.MEINS
                    , a.AUSCH
                    , a.KZKUP
                    , a.ALPOS
                    , a.ALPGR
                    , a.EWAHR
                    , a.SANFE
                    , a.SANKA
                    , a.BEIKZ
                    , MIN(a.DATUV) AS DATUV
                    , MAX(a.DATUV_TO) AS DATUV_TO
                    , '' AS AENNR
                    , a.SORTF
                    , a.LGORT
                    , a.POTX1
                    , '1'
                    , MAX(a.XSEQNR) AS XSEQNR
                    , SYSDATE
                FROM  SAP_IN_BOM_COND a
                    LEFT OUTER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.BU_P = a.IDNRK
                    LEFT OUTER JOIN SAP_IN_PRODUCT_CP d ON d.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO AND d.RESOURCE_NO_2 = a.IDNRK
                WHERE a.PLANT_CODE = '{sPlantCode}' AND a.RESOURCE_NO = 'P{sResourceNo}' AND a.NOTE = '{sNote}' AND a.P_TYPE = '1'
                GROUP BY a.P_TYPE
                    , a.PLANT_CODE
                    , a.NOTE
                    , SUBSTR(a.RESOURCE_NO, 2)
                    , a.RESOURCE_NO
                    , a.STLAN
                    , a.STLNR
                    , a.STLAL
                    , a.BMEIN
                    , a.LKENZ
                    , a.P_NOTE
                    , a.MEINS
                    , a.AUSCH
                    , a.KZKUP
                    , a.ALPOS
                    , a.ALPGR
                    , a.EWAHR
                    , a.SANFE
                    , a.SANKA
                    , a.BEIKZ
                    , a.SORTF
                    , a.LGORT
                    , a.POTX1";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this.Text, "PreMixCreate", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                Dbconn.conn.Rollback();
                return false;
            }

            Dbconn.conn.Commit();

            return true;
        }

        /// <summary>
        /// 프리믹스 기준 정보 생성
        /// </summary>
        /// <param name="plantCode"></param>
        /// <param name="resourceNo"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        private bool PremixBaseDataCreate(string plantCode, string resourceNo, string note)
        {
            SQL = $@"
            SELECT 1
            FROM SAP_DI_PRODUCT
            WHERE PLANT_CODE = '{plantCode}' AND RESOURCE_NO = 'P{resourceNo}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            Dbconn.conn.BeginTransaction();

            if (Dbconn.conn.getRowCnt(ds) == 0)
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
                    WHERE PLANT_CODE = '{plantCode}' AND RESOURCE_NO = '{resourceNo}'
                ) s
                ON (
                  d.PLANT_CODE  = s.PLANT_CODE AND d.RESOURCE_NO = s.RESOURCE_NO
                )
                --WHEN MATCHED THEN
                --UPDATE SET
                --    d.DESCRIPTION         = s.DESCRIPTION
                --  , d.UOM                 = s.UOM
                --  , d.RESOURCE_TYPE       = s.RESOURCE_TYPE
                --  , d.RESOURCE_TYPE_DESC  = s.RESOURCE_TYPE_DESC
                --  , d.ZSPEC               = s.ZSPEC
                --  , d.P_TYPE              = s.P_TYPE
                --  , d.STD_LOT_SIZE        = s.STD_LOT_SIZE
                --  , d.PACK_SIZE           = s.PACK_SIZE
                --  , d.ZDOMESTIC_IMPORT    = s.ZDOMESTIC_IMPORT
                --  , d.LVORM               = s.LVORM
                --  , d.I_TIME              = s.I_TIME
                --  , d.WERKS               = s.WERKS
                --  , d.HAND_YN             = s.HAND_YN
                --  , d.SU_CODE             = s.SU_CODE
                --  , d.PLANNER             = s.PLANNER
                --  , d.SOURCE_DESC         = s.SOURCE_DESC
                --  , d.HOME_LOCATION       = s.HOME_LOCATION
                --  , d.COST_CENTER1        = s.COST_CENTER1
                --  , d.COST_CENTER3        = s.COST_CENTER3
                --  , d.COST_DIVISION       = s.COST_DIVISION
                --  , d.POU                 = s.POU
                --  , d.BUYER               = s.BUYER
                --  , d.MANF_ENGR           = s.MANF_ENGR
                --  , d.INSPECTOR           = s.INSPECTOR
                --  , d.MAT_MSTR            = s.MAT_MSTR
                --  , d.ITEM_GROUP_V        = s.ITEM_GROUP_V
                --  , d.ITEM_GROUP_W        = s.ITEM_GROUP_W
                --  , d.AP_CLOSE_TYPE       = s.AP_CLOSE_TYPE
                --  , d.LABOR               = s.LABOR
                --  , d.MIX_TIME            = s.MIX_TIME
                --  , d.DRY_TIME            = s.DRY_TIME
                --  , d.FINAL_TIME          = s.FINAL_TIME
                --  , d.LR_YN               = s.LR_YN
                --  , d.CR_YN               = s.CR_YN
                --  , d.MT_TIME             = s.MT_TIME
                --  , d.REMARK_1            = s.REMARK_1
                --  , d.REMARK_2            = s.REMARK_2
                --  , d.LABOR_T             = s.LABOR_T
                --  , d.P_TYPE_T            = s.P_TYPE_T
                --  , d.B_P                 = s.B_P
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
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "PreMixCreate", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return false;
                }
            }

            SQL = $@"
            SELECT 1
            FROM SAP_IN_BOM_CONM
            WHERE PLANT_CODE = '{plantCode}' AND RESOURCE_NO = 'P{resourceNo}' AND NOTE = '{note}'
            ";

            ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) == 0)
            {
                SQL = $@"
                MERGE INTO SAP_IN_BOM_CONM d
                USING (
                  SELECT
                    a.PLANT_CODE                              as PLANT_CODE,
                    '1'                                       as P_TYPE,
                    'P' || a.RESOURCE_NO                      as RESOURCE_NO,
                    a.NOTE                                    as NOTE,
                    '1'                                       as STLST,
                    a.BMENG                                   as BMENG,
                    b.MIX_TIME                                AS MIX_TIME,
                    b.MIX_TIME2                               AS MIX_TIME2,
                    b.DRY_TIME                                AS DRY_TIME,
                    b.FINAL_TIME                              AS FINAL_TIME,
                    b.LR_YN                                   AS LR_YN,
                    b.CR_YN                                   AS CR_YN,
                    b.MT_TIME                                 AS MT_TIME,
                    a.REMARK_1                                AS REMARK_1,
                    a.REMARK_2                                AS REMARK_2,
                    '{clsCommon.UserId}'                    AS EMPLOYEE_NO,
                    a.H_YN                                    AS H_YN,
                    SYSDATE                                 AS I_TIME,
                    a.USE_YN                                  AS USE_YN,
                    a.DATUV                                   AS DATUV,
                    a.DATUV_TO                                AS DATUV_TO
                FROM SAP_IN_BOM_CONM a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{plantCode}' AND a.P_TYPE = '1' AND a.RESOURCE_NO = '{resourceNo}' AND a.NOTE = '{note}'
                    AND ROWNUM = 1
                UNION ALL 
                SELECT
                    a.PLANT_CODE                              as PLANT_CODE,
                    '2'                                       as P_TYPE,
                    'P' || a.RESOURCE_NO                      as RESOURCE_NO,
                    a.NOTE                                    as NOTE,
                    '1'                                       as STLST,
                    a.BMENG                                   as BMENG,
                    b.MIX_TIME                                AS MIX_TIME,
                    b.MIX_TIME2                               AS MIX_TIME2,
                    b.DRY_TIME                                AS DRY_TIME,
                    b.FINAL_TIME                              AS FINAL_TIME,
                    b.LR_YN                                   AS LR_YN,
                    b.CR_YN                                   AS CR_YN,
                    b.MT_TIME                                 AS MT_TIME,
                    a.REMARK_1                                AS REMARK_1,
                    a.REMARK_2                                AS REMARK_2,
                    '{clsCommon.UserId}'                    AS EMPLOYEE_NO,
                    a.H_YN                                    AS H_YN,
                    SYSDATE + (1/24/60/60)                    AS I_TIME,
                    a.USE_YN                                  AS USE_YN,
                    a.DATUV                                   AS DATUV,
                    a.DATUV_TO                                AS DATUV_TO
                FROM SAP_IN_BOM_CONM a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                  WHERE a.PLANT_CODE = '{plantCode}' AND a.P_TYPE = '1' AND a.RESOURCE_NO = '{resourceNo}' AND a.NOTE = '{note}'
                    AND ROWNUM = 1
                UNION ALL 
                SELECT
                    a.PLANT_CODE                              as PLANT_CODE,
                    '2'                                       as P_TYPE,
                    a.RESOURCE_NO                             as RESOURCE_NO,
                    a.NOTE                                    as NOTE,
                    '1'                                       as STLST,
                    a.BMENG                                   as BMENG,
                    b.MIX_TIME                                AS MIX_TIME,
                    b.MIX_TIME2                               AS MIX_TIME2,
                    b.DRY_TIME                                AS DRY_TIME,
                    b.FINAL_TIME                              AS FINAL_TIME,
                    b.LR_YN                                   AS LR_YN,
                    b.CR_YN                                   AS CR_YN,
                    b.MT_TIME                                 AS MT_TIME,
                    a.REMARK_1                                AS REMARK_1,
                    a.REMARK_2                                AS REMARK_2,
                    '{clsCommon.UserId}'                    AS EMPLOYEE_NO,
                    a.H_YN                                    AS H_YN,
                    SYSDATE + (1/24/60/60)                    AS I_TIME,
                    a.USE_YN                                  AS USE_YN,
                    a.DATUV                                   AS DATUV,
                    a.DATUV_TO                                AS DATUV_TO
                FROM SAP_IN_BOM_CONM a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                  WHERE a.PLANT_CODE = '{plantCode}' AND a.P_TYPE = '1' AND a.RESOURCE_NO = '{resourceNo}' AND a.NOTE = '{note}'
                    AND ROWNUM = 1) s
                ON
                  (d.PLANT_CODE = s.PLANT_CODE AND
                  d.P_TYPE = s.P_TYPE AND 
                  d.RESOURCE_NO = s.RESOURCE_NO AND 
                  d.NOTE = s.NOTE )
                --WHEN MATCHED
                --THEN
                --UPDATE SET
                --  d.BMENG = s.BMENG,
                --  d.MIX_TIME = s.MIX_TIME,
                --  d.DRY_TIME = s.DRY_TIME,
                --  d.FINAL_TIME = s.FINAL_TIME,
                --  d.LR_YN = s.LR_YN,
                --  d.CR_YN = s.CR_YN,
                --  d.MT_TIME = s.MT_TIME,
                --  d.REMARK_1 = s.REMARK_1,
                --  d.REMARK_2 = s.REMARK_2,
                --  d.EMPLOYEE_NO = s.EMPLOYEE_NO,
                --  d.H_YN = s.H_YN,
                --  d.I_TIME = s.I_TIME,
                --  d.USE_YN = s.USE_YN,
                --  d.DATUV = s.DATUV,
                --  d.DATUV_TO = s.DATUV_TO
                WHEN NOT MATCHED
                THEN
                INSERT (
                  PLANT_CODE, P_TYPE, RESOURCE_NO, NOTE,
                  BMENG, MIX_TIME, MIX_TIME2, DRY_TIME,
                  FINAL_TIME, LR_YN, CR_YN, MT_TIME,
                  REMARK_1, REMARK_2, EMPLOYEE_NO,
                  H_YN, I_TIME, USE_YN,
                  DATUV, DATUV_TO)
                VALUES (
                  s.PLANT_CODE, s.P_TYPE, s.RESOURCE_NO, s.NOTE,
                  s.BMENG, s.MIX_TIME, s.MIX_TIME2, s.DRY_TIME,
                  s.FINAL_TIME, s.LR_YN, s.CR_YN, s.MT_TIME,
                  s.REMARK_1, s.REMARK_2, s.EMPLOYEE_NO,
                  s.H_YN, s.I_TIME, s.USE_YN,
                  s.DATUV, s.DATUV_TO)
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "PreMixCreate", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return false;
                }
            }

            Dbconn.conn.Commit();

            return true;
        }

        /// <summary>
        /// 프리믹스 사전, 본 배합비 생성
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool PreMixCreate(DataRow dr)
        {
            SQL = $@"
                SELECT 1
                FROM SAP_IN_BOM_COND
                WHERE PLANT_CODE = '{dr["PLANT_CODE"]}'
                    AND RESOURCE_NO = 'P{dr["RESOURCE_NO"]}' AND NOTE = '{dr["NOTE"]}'
                ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
            {
                return true;
            }

            SQL = $@"
            MERGE INTO SAP_IN_BOM_COND D
            USING (
            SELECT DISTINCT a.PLANT_CODE, '1' AS P_TYPE, a.NOTE, a.IDNRK, 
                'P' || a.RESOURCE_NO AS RESOURCE_NO , a.STLAN, a.STLNR, 
                a.STLAL, a.BMEIN, a.LKENZ, 
                a.POSNR, a.MENGE, 
                '' AS P_NOTE, a.MEINS, a.AUSCH, a.KZKUP, 
                a.ALPOS, a.ALPGR, a.EWAHR, 
                a.SANFE, a.SANKA, a.BEIKZ, 
                a.DATUV, a.DATUV_TO, a.AENNR, a.SORTF, 
                a.LGORT, a.POTX1, a.SEQ, 
                a.XSEQNR, SYSDATE AS I_TIME
            FROM SAP_IN_BOM_COND a
                LEFT OUTER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.BU_P = a.IDNRK
                LEFT OUTER JOIN SAP_IN_PRODUCT_CP d ON d.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO AND d.RESOURCE_NO_2 = a.IDNRK
            WHERE a.PLANT_CODE = '{dr["PLANT_CODE"]}' AND a.P_TYPE = '1'
                AND a.RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                AND a.NOTE = '{dr["NOTE"]}'
                AND a.IDNRK = '{dr["IDNRK"]}'
            UNION ALL
            SELECT DISTINCT a.PLANT_CODE, '2' AS P_TYPE, a.NOTE, NVL(d.RESOURCE_NO_3, NVL(c.RESOURCE_NO, a.IDNRK)) AS IDNRK, 
                'P' || a.RESOURCE_NO AS RESOURCE_NO, a.STLAN, a.STLNR, 
                a.STLAL, a.BMEIN, a.LKENZ, 
                a.POSNR, NVL((a.MENGE * (d.PART_P / 100)), a.MENGE) AS MENGE, 
                '' AS P_NOTE, a.MEINS, a.AUSCH, a.KZKUP, 
                a.ALPOS, a.ALPGR, a.EWAHR, 
                a.SANFE, a.SANKA, a.BEIKZ, 
                a.DATUV, a.DATUV_TO, a.AENNR, a.SORTF, 
                a.LGORT, a.POTX1, a.SEQ, 
                a.XSEQNR, SYSDATE + (1/24/60/60) AS I_TIME
            FROM SAP_IN_BOM_COND a
                LEFT OUTER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.BU_P = a.IDNRK
                LEFT OUTER JOIN SAP_IN_PRODUCT_CP d ON c.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO AND d.RESOURCE_NO_2 = a.IDNRK
            WHERE a.PLANT_CODE = '{dr["PLANT_CODE"]}' AND a.P_TYPE = '1'
                AND a.RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                AND a.NOTE = '{dr["NOTE"]}'
                AND a.IDNRK = '{dr["IDNRK"]}'
            ORDER BY IDNRK) s
            ON
              (d.PLANT_CODE = s.PLANT_CODE AND
              d.P_TYPE = s.P_TYPE AND 
              d.NOTE = s.NOTE AND 
              d.RESOURCE_NO = s.RESOURCE_NO AND 
              d.IDNRK = s.IDNRK )
            --WHEN MATCHED
            --THEN
            --UPDATE SET
            --  d.STLAN = s.STLAN,
            --  d.STLNR = s.STLNR,
            --  d.STLAL = s.STLAL,
            --  d.BMEIN = s.BMEIN,
            --  d.LKENZ = s.LKENZ,
            --  d.POSNR = s.POSNR,
            --  d.MENGE = s.MENGE,
            --  d.P_NOTE = s.P_NOTE,
            --  d.MEINS = s.MEINS,
            --  d.AUSCH = s.AUSCH,
            --  d.KZKUP = s.KZKUP,
            --  d.ALPOS = s.ALPOS,
            --  d.ALPGR = s.ALPGR,
            --  d.EWAHR = s.EWAHR,
            --  d.SANFE = s.SANFE,
            --  d.SANKA = s.SANKA,
            --  d.BEIKZ = s.BEIKZ,
            --  d.DATUV = s.DATUV,
            --  d.DATUV_TO = s.DATUV_TO,
            --  d.AENNR = s.AENNR,
            --  d.SORTF = s.SORTF,
            --  d.LGORT = s.LGORT,
            --  d.POTX1 = s.POTX1,
            --  d.SEQ = s.SEQ,
            --  d.XSEQNR = s.XSEQNR,
            --  d.I_TIME = s.I_TIME
            WHEN NOT MATCHED
            THEN
            INSERT (
              PLANT_CODE, P_TYPE, NOTE, RESOURCE_NO,
              IDNRK, STLAN, STLNR,
              STLAL, BMEIN,
              LKENZ, POSNR, MENGE,
              P_NOTE, MEINS, AUSCH,
              KZKUP, ALPOS, ALPGR,
              EWAHR, SANFE, SANKA,
              BEIKZ, DATUV, DATUV_TO,
              AENNR, SORTF, LGORT,
              POTX1, SEQ, XSEQNR, I_TIME)
            VALUES (
              s.PLANT_CODE, s.P_TYPE, s.NOTE, s.RESOURCE_NO,
              s.IDNRK, s.STLAN, s.STLNR,
              s.STLAL, s.BMEIN,
              s.LKENZ, s.POSNR, s.MENGE,
              s.P_NOTE, s.MEINS, s.AUSCH,
              s.KZKUP, s.ALPOS, s.ALPGR,
              s.EWAHR, s.SANFE, s.SANKA,
              s.BEIKZ, s.DATUV, s.DATUV_TO,
              s.AENNR, s.SORTF, s.LGORT,
              s.POTX1, s.SEQ, s.XSEQNR, s.I_TIME)
            ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "PreMixCreate", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 프리믹스 배합비 100% 만들기
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sResourceNo"></param>
        /// <param name="sNote"></param>
        private bool SetPreMixRate(string sPlantCode, string sResourceNo, string sNote)
        {
            Dbconn.conn.BeginTransaction();

            SQL = $@"
                MERGE INTO SAP_IN_BOM_COND T
                USING (
                    SELECT
                        A.PLANT_CODE
                      , A.RESOURCE_NO
                      , A.NOTE
                      , A.IDNRK
                      , A.P_TYPE
                      , CASE
                            WHEN A.ROW_NUM = 1 THEN ROUND(A.PERCENT + B.SHORTFALL, 3)
                            ELSE A.PERCENT
                        END AS NEW_MENGE
                    FROM (
                        SELECT
                            S.PLANT_CODE
                          , S.RESOURCE_NO
                          , S.NOTE
                          , S.IDNRK
                          , S.P_TYPE
                          , S.MENGE
                          , ROUND(S.MENGE / T.TOTAL * 100, 3) AS PERCENT
                          , ROW_NUMBER() OVER (ORDER BY S.MENGE DESC, S.ROWID) AS ROW_NUM
                        FROM SAP_IN_BOM_COND S
                        CROSS JOIN (
                            SELECT SUM(MENGE) AS TOTAL
                            FROM SAP_IN_BOM_COND
                            WHERE PLANT_CODE = '{sPlantCode}'
                              AND RESOURCE_NO = 'P{sResourceNo}'
                              AND NOTE = '{sNote}'
                              AND P_TYPE = '2'
                        ) T
                        WHERE S.PLANT_CODE = '{sPlantCode}'
                          AND S.RESOURCE_NO = 'P{sResourceNo}'
                          AND S.NOTE = '{sNote}'
                          AND S.P_TYPE = '2'
                    ) A
                    CROSS JOIN (
                        SELECT
                            ROUND(100 - SUM(PERCENT), 3) AS SHORTFALL
                        FROM (
                            SELECT
                                ROUND(MENGE / T.TOTAL * 100, 3) AS PERCENT
                            FROM SAP_IN_BOM_COND S
                            CROSS JOIN (
                                SELECT SUM(MENGE) AS TOTAL
                                FROM SAP_IN_BOM_COND
                                WHERE PLANT_CODE = '{sPlantCode}'
                                  AND RESOURCE_NO = 'P{sResourceNo}'
                                  AND NOTE = '{sNote}'
                                  AND P_TYPE = '2'
                            ) T
                            WHERE S.PLANT_CODE = '{sPlantCode}'
                              AND S.RESOURCE_NO = 'P{sResourceNo}'
                              AND S.NOTE = '{sNote}'
                              AND S.P_TYPE = '2'
                        )
                    ) B
                ) s
                ON (t.PLANT_CODE = s.PLANT_CODE AND t.RESOURCE_NO = s.RESOURCE_NO AND t.NOTE = s.NOTE AND t.IDNRK = s.IDNRK AND t.P_TYPE = s.P_TYPE)
                WHEN MATCHED THEN
                UPDATE SET t.MENGE = s.NEW_MENGE
                ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave(this.Text, "PreMixCreate", SQL);
                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                Dbconn.conn.Rollback();
                return false;
            }

            Dbconn.conn.Commit();

            return true;
        }

        private void viewProduct_ShownEditor(object sender, EventArgs e)
        {
        }

        private void txtResource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }

        private void txtNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }

        private void btnNoteList_Click(object sender, EventArgs e)
        {
            string sPlantCode = viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
            string sResourceNo = viewProduct.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();
            string sNote = cboNOTE.EditValue?.ToString();

            // 새 폼에 데이터 전달
            using (m_Note child = new m_Note(sPlantCode, sResourceNo, sNote))
            {
                child.StartPosition = FormStartPosition.CenterParent;
                if (child.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }

        /// <summary>
        /// 배합비 복사(버전추가)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNoteCopy_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("배합버전을 증가하여 생성하시겠습니까?"))
            {
                return;
            }

            try
            {
                string sPlantCode = viewProduct.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                string sResourceNo = viewProduct.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();
                string oldNote = cboNOTE.EditValue?.ToString();
                string newNote = clsCommon.IncreaseLastNumber(oldNote);

                Dbconn.conn.BeginTransaction();

                SQL = $@"
                INSERT INTO SAP_IN_BOM_CONM
                (
                     P_TYPE
                    , PLANT_CODE
                    , RESOURCE_NO
                    , NOTE
                    , DATUV
                    , DATUV_TO
                    , BMENG
                    , MIX_TIME
                    , MIX_TIME2
                    , DRY_TIME
                    , FINAL_TIME
                    , LR_YN
                    , CR_YN
                    , MT_TIME
                    , STLST
                    , REMARK_1
                    , REMARK_2
                    , EMPLOYEE_NO
                    , H_YN
                    , I_TIME
                    , USE_YN
                )
                SELECT P_TYPE, PLANT_CODE, RESOURCE_NO,
                   '{newNote}', DATUV, DATUV_TO,
                   BMENG, MIX_TIME, MIX_TIME2, DRY_TIME,
                   FINAL_TIME, LR_YN, CR_YN,
                   MT_TIME, '1', REMARK_1, 
                   REMARK_2, EMPLOYEE_NO, H_YN, 
                   SYSDATE, 'Y'
                FROM SAP_IN_BOM_CONM
                WHERE PLANT_CODE = '{sPlantCode}' AND RESOURCE_NO = '{sResourceNo}' AND NOTE = '{oldNote}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    clsLog.logSave(this.Text, "btnNoteCopy_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    Dbconn.conn.Rollback();
                    return;
                }

                SQL = $@"
                INSERT INTO SAP_IN_BOM_COND
                SELECT P_TYPE, PLANT_CODE, '{newNote}',
                   RESOURCE_NO, IDNRK, STLAN,
                   STLNR, STLAL, BMEIN,
                   LKENZ, POSNR, MENGE,
                   P_NOTE, MEINS, AUSCH,
                   KZKUP, ALPOS, ALPGR,
                   EWAHR, SANFE, SANKA,
                   BEIKZ, DATUV, DATUV_TO,
                   AENNR, SORTF, LGORT,
                   POTX1, SEQ, XSEQNR,
                   SYSDATE
                FROM SAP_IN_BOM_COND
                WHERE PLANT_CODE = '{sPlantCode}' AND RESOURCE_NO = '{sResourceNo}' AND NOTE = '{oldNote}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    clsLog.logSave(this.Text, "btnNoteCopy_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    Dbconn.conn.Rollback();
                    return;
                }

                Dbconn.conn.Commit();

                clsDevexpressUtil.ItemLookUpEditSetup(cboNOTE, clsCommon.getNote(cboPlant_Code.EditValue?.ToString(), vResourceNo), "", false, 0);

                if (!string.IsNullOrEmpty(txtResourceNo.EditValue.ToString()))
                {
                    GetMixVersion(sPlantCode, sResourceNo);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btnNoteCopy_Click", ex);
            }
        }


    }
}