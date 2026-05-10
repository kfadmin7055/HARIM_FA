using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Stock_Info : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        private bool isInitializing = false;

        public frm_Stock_Info()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewIngred, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_ProductInventoryLedger_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridIngred, viewIngred);

            XIngred_Search();

            isInitializing = true;

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XIngred_Search()
        {
            try
            {
                SQL = $@"
                -- 원료 재고
                SELECT b.PLANT_CODE, b.RESOURCE_NO, P.DESCRIPTION AS RESOURCE_NAME, b.STOCK
                FROM BIN b 
                    JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = b.PLANT_CODE AND p.RESOURCE_NO = b.RESOURCE_NO
                WHERE  b.PLANT_CODE = '{clsCommon.PlantCode}'
                    AND b.PROCESS_KEY IN (SELECT p.PROCESS_KEY FROM SAP_PROCESS_DIVISION P WHERE P.PLANT_CODE = b.PLANT_CODE AND p.PROCESS_TYPE = '{clsCommon.GetProcessTypeCode("배합")}')
                    AND (b.RESOURCE_NO LIKE '%{txtResource.EditValue}%' OR p.DESCRIPTION LIKE '%{txtResource.EditValue}%')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridIngred, viewIngred, ds.Tables[0], false);

                sValid = new string[] { "" };


                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void XSack_Search()
        {
            try
            {
                SQL = $@"
                -- 지대 재고    
                SELECT s.WERKS AS PLANT_CODE, s.MATNR AS RESOURCE_NO, P.DESCRIPTION AS RESOURCE_NAME, s.LABST AS STOCK
                FROM SAP_STOCK_MASTER s
                    JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = S.WERKS AND p.RESOURCE_NO = s.MATNR AND p.UOM = 'EA'
                WHERE s.WERKS = '{clsCommon.PlantCode}'
                    AND (s.MATNR LIKE '%{txtResource.EditValue}%' OR p.DESCRIPTION LIKE '%{txtResource.EditValue}%')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridSack, viewSack, ds.Tables[0], false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void XBulk_Search()
        {
            try
            {
                SQL = $@"
                -- 벌크 재고
                SELECT b.PLANT_CODE, b.RESOURCE_NO, P.DESCRIPTION AS RESOURCE_NAME, b.STOCK
                FROM BIN b 
                    JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = b.PLANT_CODE AND p.RESOURCE_NO = b.RESOURCE_NO
                WHERE  b.PLANT_CODE = '{clsCommon.PlantCode}'
                    AND b.PROCESS_KEY = '{clsCommon.GetProcessKey("벌크", clsCommon.PlantCode)}'
                    AND (b.RESOURCE_NO LIKE '%{txtResource.EditValue}%' OR p.DESCRIPTION LIKE '%{txtResource.EditValue}%')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridBulk, viewBulk, ds.Tables[0], false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void XBag_Search()
        {
            try
            {
                SQL = $@"
                WITH IN_PO AS (
                    -- 입고 매입
                    SELECT pd.WERKS AS PLANT_CODE, pd.MATNR AS RESOURCE_NO,  SUM(gd.R_GR_QNTY) AS I_PO_WEIGHT
                    FROM SAP_INPUT_POORDERD_CON pd
                        JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = pd.WERKS AND P.RESOURCE_NO = pd.MATNR AND P.RESOURCE_TYPE = 'HAWA' AND p.UOM = 'KG'
                        JOIN WAP_GOCARD gd ON gd.WERKS = pd.WERKS AND gd.EBELP = pd.EBELP
                    GROUP BY pd.WERKS, pd.MATNR
                ), IN_IGO AS (
                    -- 입고 이고
                    SELECT result.PLANT_CODE, result.RESOURCE_NO, SUM(result.WEIGHT) AS I_IGO_WEIGHT
                    FROM WAP_DECAR decar
                        JOIN TMS_OUTPUT_RESULT result ON result.IS_NO = decar.IS_NO
                        JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = result.PLANT_CODE AND P.RESOURCE_NO = result.RESOURCE_NO AND p.UOM = 'KG'
                    WHERE decar.CAR_TYPE IN ('004', '005')
                    GROUP BY result.PLANT_CODE, result.RESOURCE_NO
                ), IN_PACK AS (
                    --타이콘 포장
                    SELECT bo.PLANT_CODE, bo.RESOURCE_NO, SUM(bo.P_Q) AS I_P_Q
                    FROM BULK_ORDER bo
                    WHERE bo.CAR_NO_REAL = '9999'
                    GROUP BY bo.PLANT_CODE, bo.RESOURCE_NO
                ), OUT_IGO AS (
                    -- 출고 이고
                    SELECT result.PLANT_CODE, result.RESOURCE_NO, SUM(result.WEIGHT) AS O_IGO_WEIGHT
                    FROM WAP_DECAR decar
                        JOIN TMS_OUTPUT_RESULT result ON result.IS_NO = decar.IS_NO
                        JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = result.PLANT_CODE AND P.RESOURCE_NO = result.RESOURCE_NO AND p.UOM = 'KG'
                        JOIN TMS_INPUT_PLOADD_CON ipd ON ipd.DISPATCHNO = result.DISPATCHNO AND ipd.ORDERNO = result.ORDERNO AND ipd.ORDERLINENO = result.ORDERLINENO AND ipd.ORDERTYPECODE LIKE 'ZLO%'
                    --WHERE decar.CAR_TYPE IN ('004', '005')
                    GROUP BY result.PLANT_CODE, result.RESOURCE_NO
                ), OUT_SALE AS (
                    -- 출고 판매
                    SELECT result.PLANT_CODE, result.RESOURCE_NO, SUM(result.WEIGHT) AS O_SALE_WEIGHT
                    FROM WAP_DECAR decar
                        JOIN TMS_OUTPUT_RESULT result ON result.IS_NO = decar.IS_NO
                    --    JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = result.PLANT_CODE AND P.RESOURCE_NO = result.RESOURCE_NO AND p.UOM = 'KG'
                        JOIN SAP_INPUT_SHIP_ORDERD_CON sod ON sod.VBELN = result.ORDERNO AND sod.POSNR = result.ORDERLINENO AND sod.ITEM_TEXT1 = 'Y'
                    GROUP BY result.PLANT_CODE, result.RESOURCE_NO
                ), OUT_B_B AS (
                    -- 벌크->타이콘
                    SELECT result.PLANT_CODE, result.RESOURCE_NO, SUM(result.BAG_WEIGHT) AS O_BAG_WEIGHT
                    FROM WAP_DECAR decar
                        JOIN TMS_OUTPUT_RESULT result ON result.IS_NO = decar.IS_NO
                    --    JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = result.PLANT_CODE AND P.RESOURCE_NO = result.RESOURCE_NO AND p.UOM = 'KG'
                        JOIN SAP_INPUT_SHIP_ORDERD_CON sod ON sod.VBELN = result.ORDERNO AND sod.POSNR = result.ORDERLINENO AND sod.ITEM_TEXT1 = 'Y'
                    GROUP BY result.PLANT_CODE, result.RESOURCE_NO
                )

                SELECT stock.PLANT_CODE, stock.RESOURCE_NO, p.DESCRIPTION AS RESOURCE_NAME, stock.END_YM, stock.END_Q
                    , bo.PRO_KG, stock.END_Q AS STOCK_KG
                    , iop.I_PO_WEIGHT, iig.I_IGO_WEIGHT, ip.I_P_Q, oig.O_IGO_WEIGHT, os.O_SALE_WEIGHT, obb.O_BAG_WEIGHT
                FROM BAG_C_STOCK stock
                    JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = stock.PLANT_CODE AND P.RESOURCE_NO = stock.RESOURCE_NO
                    LEFT JOIN BAG_ORDER bo ON bo.PLANT_CODE = stock.PLANT_CODE AND bo.RESOURCE_NO = stock.RESOURCE_NO
                    LEFT JOIN IN_PO iop ON iop.PLANT_CODE = stock.PLANT_CODE AND iop.RESOURCE_NO = stock.RESOURCE_NO
                    LEFT JOIN IN_IGO iig ON iig.PLANT_CODE = stock.PLANT_CODE AND iig.RESOURCE_NO = stock.RESOURCE_NO
                    LEFT JOIN IN_PACK ip ON ip.PLANT_CODE = stock.PLANT_CODE AND ip.RESOURCE_NO = stock.RESOURCE_NO
                    LEFT JOIN OUT_IGO oig ON oig.PLANT_CODE = stock.PLANT_CODE AND oig.RESOURCE_NO = stock.RESOURCE_NO
                    LEFT JOIN OUT_SALE os ON os.PLANT_CODE = stock.PLANT_CODE AND os.RESOURCE_NO = stock.RESOURCE_NO
                    LEFT JOIN OUT_B_B obb ON obb.PLANT_CODE = stock.PLANT_CODE AND obb.RESOURCE_NO = stock.RESOURCE_NO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridBag, bandBag, ds.Tables[0], false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (tabStock.SelectedTabPage == tIngred)
                XIngred_Search();
            else if (tabStock.SelectedTabPage == tSack)
            {
                XSack_Search();
                XBulk_Search();
            }
            else if (tabStock.SelectedTabPage == tBag)
            {
                XBag_Search();
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {

        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XIngred_Search();
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
            gridIngred.Focus();
            //gridView.FocusedRowHandle = 0;
            //gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            if (isInitializing)
            {
                gridIngred.DataSource = null;
                XIngred_Search();
            }
        }

        private void txtResource_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabStock.SelectedTabPage == tIngred)
                XIngred_Search();
            else if (tabStock.SelectedTabPage == tSack)
            {
                XSack_Search();
                XBulk_Search();
            }
            else if (tabStock.SelectedTabPage == tBag)
            {
                XBag_Search();
            }
        }

        private void tabStock_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (tabStock.SelectedTabPage == tIngred)
                XIngred_Search();
            else if (tabStock.SelectedTabPage == tSack)
            {
                XSack_Search();
                XBulk_Search();
            }
            else if (tabStock.SelectedTabPage == tBag)
            {
                XBag_Search();
                //ShowMessageBox.XtraShowWarning("개발중입니다. 완료되면 공지 해드리겠습니다.");
                //tabStock.SelectedTabPage = tIngred;
                //return;
            }
        }
    }
}