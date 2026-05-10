using Core.Class;
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
    public partial class frm_InOutCarMaster : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_InOutCarMaster()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewInOutCar, Properties.Settings.Default.FontSize);
        }

        private void frm_InCarMaster_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            dtFromDeliveryDate.EditValue = DateTime.Today.AddDays(-1);
            dtToDeliveryDate.EditValue = DateTime.Today;

            SQL = $@"
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '13'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            clsDevexpressUtil.ItemLookUpEditSetup(cboCar_Type, ds.Tables[0], "", false, 0, true);

            XMain_Search();


            // Application.AddMessageFilter(new GridMouseWheelFilter(gridInOutCar));
        }

        private void XMain_Search()
        {
            try
            {
                DateTime dtFrom = dtFromDeliveryDate.DateTime.Date;
                DateTime dtTo = dtToDeliveryDate.DateTime.Date;

                SQL = $@"
                WITH CUSTOMER AS (
                    SELECT PARTNER AS CODE, NAME_ORG1 AS NAME
                    FROM SAP_DI_CUSTOMER a
                    UNION ALL
                    SELECT LOCATION AS CODE, DESCRIPTION AS NAME
                    FROM SAP_DI_LOCATION
                    WHERE USE_FLAG = 'Y'
                ), CAR_TYPE AS (
                    SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
                    FROM COMM_DIV a
                        INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                        INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                    WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '13'
                )

                SELECT decar.INCAR_NO, NVL(car.VEHICLENO, decar.INCAR_NO) AS VEHICLENO, decar.CAR_TYPE, ct.NAME AS CAR_TYPE_DESC, decar.ETC_DETAIL
                    , TO_CHAR(decar.INCAR_DATE, 'yyyy-MM-dd HH24:mi:ss') AS INCAR_DATE
                    , TO_CHAR(decar.OUTCAR_DATE, 'yyyy-MM-dd HH24:mi:ss') AS OUTCAR_DATE
                    , decar.IN_WEIGHT, decar.OUT_WEIGHT
                    , CASE WHEN decar.CAR_TYPE IN ('002') THEN (decar.OUT_WEIGHT - decar.IN_WEIGHT) ELSE (decar.IN_WEIGHT - decar.OUT_WEIGHT) END REAL_WEIGHT
                    , goc.INVOICE_WEIGHT, NVL(oresult.RESOURCE_NO, god.MATNR) AS RESOURCE_NO, NVL(ordp.DESCRIPTION, godp.DESCRIPTION) AS DESCRIPTION
                    , NVL(plod.PLANQTY, poord.MENGE) AS PLANQTY
                    , NVL(plod.TOLOCATIONCODE, poorm.LIFNR) AS TOLOCATIONCODE, NVL(plocm.NAME, poorcm.NAME) AS TOLOCATION
                    , NVL(TO_CHAR(TO_DATE(plom.DELIVERYDATE, 'yyyymmdd'), 'yyyy-MM-dd'), TO_CHAR(TO_DATE(poord.EINDT, 'yyyymmdd'), 'yyyy-MM-dd')) AS DELIVERYDATE
                    , poord.CHARG, poord.CHARG_TEXT
                FROM WAP_DECAR decar
                    LEFT JOIN TMS_INPUT_CARMASTER_CON car ON car.VEHICLECODE = decar.INCAR_NO
                    LEFT JOIN TMS_OUTPUT_RESULT oresult ON oresult.IS_NO = decar.IS_NO 
                    LEFT JOIN SAP_DI_PRODUCT ordp ON ordp.PLANT_CODE = oresult.PLANT_CODE AND ordp.RESOURCE_NO = oresult.RESOURCE_NO
                    LEFT JOIN TMS_INPUT_PLOADD_CON plod ON plod.DISPATCHNO = oresult.DISPATCHNO AND plod.ORDERNO = oresult.ORDERNO AND plod.ORDERLINENO = oresult.ORDERLINENO
                    LEFT JOIN TMS_INPUT_PLOADM_CON plom ON plom.DISPATCHNO = plod.DISPATCHNO
                    LEFT JOIN CUSTOMER plocm ON plocm.CODE = plod.TOLOCATIONCODE
                    LEFT JOIN CAR_TYPE ct ON ct.CODE = decar.CAR_TYPE
                    LEFT JOIN WAP_GOCAR goc ON goc.IS_NO = decar.IS_NO
                    LEFT JOIN WAP_GOCARD god ON god.IS_NO = goc.IS_NO AND god.EBELN = goc.EBELN
                    LEFT JOIN SAP_DI_PRODUCT godp ON godp.PLANT_CODE = god.WERKS AND godp.RESOURCE_NO = god.MATNR
                    LEFT JOIN SAP_INPUT_POORDERD_CON poord ON poord.EBELN = god.EBELN
                    LEFT JOIN SAP_INPUT_POORDERM_CON poorm ON poorm.EBELN = poord.EBELN
                    LEFT JOIN CUSTOMER poorcm ON poorcm.CODE = poorm.LIFNR
                WHERE decar.INCAR_DATE BETWEEN TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                                       AND TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS') + 1 - (1/86400)
                    AND ('{cboCar_Type.EditValue}' IS NULL OR decar.CAR_TYPE = '{cboCar_Type.EditValue}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridInOutCar, viewInOutCar, ds.Tables[0], false);

                sValid = new string[] { "" };


                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void gridBrand_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
            }
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridInOutCar.Focus();
            viewInOutCar.FocusedRowHandle = 0;
            viewInOutCar.FocusedColumn = viewInOutCar.VisibleColumns[0];
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void viewInOutCar_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridInOutCar_Click(object sender, EventArgs e)
        {

        }
    }
}