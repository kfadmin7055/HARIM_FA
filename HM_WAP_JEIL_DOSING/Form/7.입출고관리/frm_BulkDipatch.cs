using Core.Class;
using DevExpress.Data.NetCompatibility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class frm_BulkDipatch : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_BulkDipatch()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewMain, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewSub, Properties.Settings.Default.FontSize);
        }

        private void frm_BulkDipatch_Load(object sender, EventArgs e)
        {
            //배송일자
            fromDtDelivery.EditValue = DateTime.Today;
            toDtDelivery.EditValue = DateTime.Today;

            rdoCarType.SelectedIndex = 0;
            rdoInOut.SelectedIndex = 0;

            XMain_Search();
            XSub_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridMain));
            // Application.AddMessageFilter(new GridMouseWheelFilter(gridSub));
        }

        private void XMain_Search()
        {
            try
            {
                string sORDERTYPECODE1 = rdoInOut.EditValue?.ToString() == "1" ? "NL"
                                                                            : rdoInOut.EditValue?.ToString() == "2" ? "ZLF" : "";
                string sORDERTYPECODE2 = rdoInOut.EditValue?.ToString() == "1" ? "ZLO"
                                                                            : rdoInOut.EditValue?.ToString() == "2" ? "ZLF" : "";

                SQL = $@"
                -- 배차번호, 배송일자, 차량번호, 배송처코드(농장), 상품(제품명), 계획수량, 출차량, 편차량
                WITH CUSTOMER AS (
                    SELECT 
                    PARTNER AS CODE, NAME_ORG1 AS NAME
                    FROM SAP_DI_CUSTOMER a
                    UNION ALL
                    SELECT 
                    LOCATION AS CODE, DESCRIPTION AS NAME
                    FROM SAP_DI_LOCATION
                    WHERE USE_FLAG = 'Y'
                )
                , DECAR AS (
                    SELECT a.CAR_TYPE, b.DISPATCHNO, b.ORDERNO, b.ORDERLINENO, b.RESOURCE_NO, b.WEIGHT, b.QTY, c.PLANQTY
                    FROM WAP_DECAR a
                        INNER JOIN TMS_OUTPUT_RESULT b ON b.IS_NO = a.IS_NO
                        INNER JOIN TMS_INPUT_PLOADD_CON c ON c.ORDERNO = b.ORDERNO AND c.ORDERLINENO = b.ORDERLINENO
                    WHERE a.OUTCAR_DATE IS NOT NULL
                )
                , SUMMARY AS (
                    SELECT DISTINCT TO_CHAR(TO_DATE(om.LFDAT, 'YYYYMMDD'), 'YYYY-MM-DD') AS DELIVERYDATE, od.MATNR AS ITEMCODE, op.DESCRIPTION
                        , SUM(od.LFIMG) AS LFIMG, SUM(NVL(pd.PLANQTY, 0)) AS PLANQTY
                        , SUM(CASE WHEN UPPER(pm.VEHICLETONCODE) LIKE '%BK%' THEN de.PLANQTY ELSE de.QTY END) AS WEIGHT
                        , NVL(op.STD_LOT_SIZE, (marm.UMREN / marm.UMREZ)) AS STD_LOT_SIZE
                    FROM SAP_INPUT_SHIP_ORDERM_CON om
                        JOIN SAP_INPUT_SHIP_ORDERD_CON od ON od.VBELN = om.VBELN
                        JOIN SAP_DI_PRODUCT op ON op.PLANT_CODE = om.WERKS AND op.RESOURCE_NO = od.MATNR
                        LEFT JOIN TMS_INPUT_PLOADM_CON pm ON pm.DISPATCHNO = om.TKNUM
                        LEFT JOIN TMS_INPUT_PLOADD_CON pd ON pd.DISPATCHNO = pm.DISPATCHNO AND pd.ORDERNO = od.VBELN AND pd.ORDERLINENO = od.POSNR
                        LEFT JOIN TMS_INPUT_CARMASTER_CON car ON car.VEHICLECODE = pm.VEHICLECODE
                        LEFT JOIN CUSTOMER cus ON cus.CODE = pd.TOLOCATIONCODE
                        LEFT JOIN DECAR de ON de.DISPATCHNO = pd.DISPATCHNO AND de.ORDERNO = pd.ORDERNO AND de.ORDERLINENO = pd.ORDERLINENO
                        LEFT JOIN SAP_MARM marm ON marm.MATNR = op.RESOURCE_NO AND marm.MEINH = 'KG'
                    WHERE om.WERKS = '{clsCommon.PlantCode}'
                        AND TO_DATE(om.LFDAT, 'YYYYMMDD')
                            BETWEEN TO_CHAR(TO_DATE('{DateTime.Parse(fromDtDelivery.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD'), 'YYYYMMDD') 
                                AND TO_CHAR(TO_DATE('{DateTime.Parse(toDtDelivery.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD') + 1 - 1/86400, 'YYYYMMDD')
                        AND ('{rdoCarType.EditValue}' is NULL OR op.UOM = '{rdoCarType.EditValue}')
                        AND ('{rdoInOut.EditValue}' IS NULL OR (om.LFART LIKE '{sORDERTYPECODE1}%' OR om.LFART LIKE '{sORDERTYPECODE2}%'))
                    GROUP BY om.LFDAT, od.MATNR, op.DESCRIPTION, NVL(op.STD_LOT_SIZE, (marm.UMREN / marm.UMREZ))
                )

                SELECT  DELIVERYDATE, ITEMCODE, DESCRIPTION, STD_LOT_SIZE, LFIMG, (LFIMG * STD_LOT_SIZE) AS LFIMG_Q, PLANQTY, (PLANQTY * STD_LOT_SIZE) AS PLANQTY_Q
                    , NVL(WEIGHT, 0) AS WEIGHT, (NVL(WEIGHT, 0) * STD_LOT_SIZE) AS WEIGHT_Q, (PLANQTY - NVL(WEIGHT, 0)) AS REMAINQTY, ((PLANQTY - NVL(WEIGHT, 0)) * STD_LOT_SIZE) AS REMAINQTY_Q
                FROM SUMMARY
                ORDER BY ITEMCODE, ((PLANQTY - NVL(WEIGHT, 0)) * STD_LOT_SIZE)
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], false);

                string[] plant = { "PJ01", "PJ02", "PJ04", "PJ05" };

                if (plant.Any(w => clsCommon.PlantCode.Contains(w)))
                {
                    viewMain.Columns["STD_LOT_SIZE"].Visible = true;
                    viewMain.Columns["LFIMG_Q"].Visible = true;
                    viewMain.Columns["PLANQTY_Q"].Visible = true;
                    viewMain.Columns["WEIGHT_Q"].Visible = true;
                    viewMain.Columns["REMAINQTY_Q"].Visible = true;

                    viewMain.Columns["LFIMG"].Caption = "출고지시량(EA)";
                    viewMain.Columns["PLANQTY"].Caption = "계획량(EA)";
                    viewMain.Columns["WEIGHT"].Caption = "출차량(EA)";
                    viewMain.Columns["REMAINQTY"].Caption = "남은량(EA)";
                }
                else
                {
                    viewMain.Columns["STD_LOT_SIZE"].Visible = false;
                    viewMain.Columns["LFIMG_Q"].Visible = false;
                    viewMain.Columns["PLANQTY_Q"].Visible = false;
                    viewMain.Columns["WEIGHT_Q"].Visible = false;
                    viewMain.Columns["REMAINQTY_Q"].Visible = false;

                    viewMain.Columns["LFIMG"].Caption = "출고지시량";
                    viewMain.Columns["PLANQTY"].Caption = "계획중량";
                    viewMain.Columns["WEIGHT"].Caption = "출차중량";
                    viewMain.Columns["REMAINQTY"].Caption = "남은중량";
                }

                sValid = new string[] { "" };

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void XSub_Search()
        {
            try
            {
                string check = chkDelYn.Checked ? "Y" : "";

                string sORDERTYPECODE1 = rdoInOut.EditValue?.ToString() == "1" ? "NL"
                                                                            : rdoInOut.EditValue?.ToString() == "2" ? "ZLF" : "";
                string sORDERTYPECODE2 = rdoInOut.EditValue?.ToString() == "1" ? "ZLO"
                                                                            : rdoInOut.EditValue?.ToString() == "2" ? "ZLF" : "";
                SQL = $@"
                -- 배차번호, 배송일자, 차량번호, 배송처코드(농장), 상품(제품명), 계획수량, 출차량, 편차량
                WITH CUSTOMER AS (
                    SELECT 
                    PARTNER AS CODE, NAME_ORG1 AS NAME
                    FROM SAP_DI_CUSTOMER a
                    UNION ALL
                    SELECT 
                    LOCATION AS CODE, DESCRIPTION AS NAME
                    FROM SAP_DI_LOCATION
                    WHERE USE_FLAG = 'Y'
                ), DECAR AS (
                    SELECT a.CAR_TYPE, b.DISPATCHNO, b.ORDERNO, b.ORDERLINENO, b.RESOURCE_NO, b.WEIGHT, b.QTY
                    FROM WAP_DECAR a
                        INNER JOIN TMS_OUTPUT_RESULT b ON b.IS_NO = a.IS_NO
                    WHERE a.OUTCAR_DATE IS NOT NULL
                )

                SELECT pm.DISPATCHNO, TO_CHAR(TO_DATE(pm.DELIVERYDATE, 'YYYYMMDD'), 'YYYY-MM-DD') AS DELIVERYDATE
                    , pm.VEHICLECODE, car.VEHICLENO
                    , pd.TOLOCATIONCODE, cus.NAME AS TOLOCATIONNAME, pd.ITEMCODE, item.DESCRIPTION
                    , pd.PLANQTY, pd.PLANQTY * NVL(item.STD_LOT_SIZE, (marm.UMREN / marm.UMREZ)) AS PLANQTY_Q
                    , CASE WHEN UPPER(pm.VEHICLETONCODE) LIKE '%BK%' THEN de.WEIGHT ELSE de.QTY END WEIGHT                    
                    , CASE WHEN UPPER(pm.VEHICLETONCODE) LIKE '%BK%' THEN de.WEIGHT ELSE de.QTY END * NVL(item.STD_LOT_SIZE, (marm.UMREN / marm.UMREZ)) AS WEIGHT_Q
                    , NVL(item.STD_LOT_SIZE, (marm.UMREN / marm.UMREZ)) AS STD_LOT_SIZE
                FROM TMS_INPUT_PLOADM_CON pm
                    INNER JOIN TMS_INPUT_PLOADD_CON pd ON pd.DISPATCHNO = pm.DISPATCHNO
                    INNER JOIN SAP_INPUT_SHIP_ORDERM_CON om ON om.WERKS =  pd.PLANTCODE
                            AND om.VBELN = pd.ORDERNO
                    LEFT JOIN TMS_INPUT_CARMASTER_CON car ON car.VEHICLECODE = pm.VEHICLECODE
                    LEFT JOIN CUSTOMER cus ON cus.CODE = pd.TOLOCATIONCODE
                    LEFT JOIN SAP_DI_PRODUCT item ON item.RESOURCE_NO = pd.ITEMCODE
                    LEFT JOIN DECAR de ON de.DISPATCHNO = pd.DISPATCHNO AND de.ORDERNO = pd.ORDERNO AND de.ORDERLINENO = pd.ORDERLINENO
                    LEFT JOIN SAP_MARM marm ON marm.MATNR = item.RESOURCE_NO AND marm.MEINH = 'KG'
                WHERE pm.DELIVERYDATE BETWEEN TO_CHAR(TO_DATE('{DateTime.Parse(fromDtDelivery.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD'), 'YYYYMMDD') 
                            AND TO_CHAR(TO_DATE('{DateTime.Parse(toDtDelivery.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD') + 1 - 1/86400, 'YYYYMMDD') 
                    AND ('{check}' IS NULL OR de.DISPATCHNO IS NULL)
                    AND ('{rdoCarType.EditValue}' is NULL OR item.UOM = '{rdoCarType.EditValue}')
                    AND ('{rdoInOut.EditValue}' IS NULL OR (pd.ORDERTYPECODE LIKE '{sORDERTYPECODE1}%' OR pd.ORDERTYPECODE LIKE '{sORDERTYPECODE2}%'))
                 ORDER BY pm.DISPATCHNO, car.VEHICLENO, pd.ITEMCODE
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridSub, viewSub, ds.Tables[0], false);

                string[] plant = { "PJ01", "PJ02", "PJ04", "PJ05" };

                if (plant.Any(w => clsCommon.PlantCode.Contains(w)))
                {
                    viewSub.Columns["PLANQTY_Q"].Visible = true;
                    viewSub.Columns["WEIGHT_Q"].Visible = true;

                    viewSub.Columns["PLANQTY"].Caption = "계획량(EA)";
                    viewSub.Columns["WEIGHT"].Caption = "출차량(EA)";
                }
                else
                {
                    viewSub.Columns["PLANQTY_Q"].Visible = false;
                    viewSub.Columns["WEIGHT_Q"].Visible = false;

                    viewSub.Columns["PLANQTY"].Caption = "계획중량";
                    viewSub.Columns["WEIGHT"].Caption = "출차중량";
                }

                sValid = new string[] { "" };


                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
            }
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridMain.Focus();
            viewMain.FocusedRowHandle = 0;
            viewMain.FocusedColumn = viewMain.VisibleColumns[0];
        }

        private void fromDtDelivery_EditValueChanged(object sender, EventArgs e)
        {
            toDtDelivery.EditValue = fromDtDelivery.EditValue;
        }

        private void chkDelYn_CheckedChanged(object sender, EventArgs e)
        {
            XMain_Search();
            XSub_Search();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
            XSub_Search();
        }

        private void viewMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {

        }

        /// <summary>
        /// 차량구분 라디오
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoCarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            XMain_Search();
            XSub_Search();
        }

        /// <summary>
        /// 판매구분 라디오
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoInOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            XMain_Search();
            XSub_Search();
        }
    }
}