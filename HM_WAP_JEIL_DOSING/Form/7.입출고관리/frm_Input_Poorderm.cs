using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.Utils.Win.Hook;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraRichEdit.Fields;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Input_Poorderm : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string sEBELN = string.Empty;
        private string sEBELP = string.Empty;
        private string SQL = String.Empty;
        private string sIS_NO = string.Empty;
        private string[] sDeValid = null;
        private string[] sOutValid = null;
        private string[] sPaValid = null;

        // RS232 통신
        private SerialPortHelper _rs232;

        public frm_Input_Poorderm()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewList, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewPoor, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewPoorD, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewResult, Properties.Settings.Default.FontSize);

            clsDevexpressGrid.EditGridViewInit(viewDeCar, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewOut, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewPallet, Properties.Settings.Default.FontSize);
        }

        private void viewPoor_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridPoor, viewPoor);
        }

        private void viewPoorD_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridPoorD, viewPoorD);
        }

        private void viewResult_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridResult, viewResult);
        }

        private void viewDeCar_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridDeCar, viewDeCar);
        }

        private void viewOut_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridOut, viewOut);
        }

        private void frm_Input_Poorderm_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridList, viewList);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridPoor, viewPoor);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridPoorD, viewPoorD);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridResult, viewResult);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridDeCar, viewDeCar);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridOut, viewOut);

            authDs = clsSql.GetAuthDataSet(this.Name);

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dtFromDeliveryDate.EditValue = DateTime.Today;
            dtToDeliveryDate.EditValue = DateTime.Today.AddDays(1).AddSeconds(-1);

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "미삭제", "삭제" }), "", false, 2, true, true);

            layoutControlItem32.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            tabPage.SelectedTabPageIndex = 0;

            XList_Search();

            InitControl();

            //공급사
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutScboPARTNER, clsCommon.GetCustomer(), "");

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridList));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridPoor));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridPoorD));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridResult));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridDeCar));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridOut));
        }

        private void InitControl()
        {
            #region Main
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboBSART, clsCommon.GetESART());

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboLIFNR, clsCommon.GetInOutType(), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboMTYPE, clsCommon.GetMSGType(), "", false, false);
            #endregion

            #region 상차지시
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridPDscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridPDscboPLANT, clsCommon.GetPlant("", true));

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboUNIT, clsCommon.GetUnit(), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "", false, false);
            #endregion

            #region 실적
            clsDevexpressGrid.ItemLookUpEditSetup(gridResultCboESART, clsCommon.GetESART());

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutScboMATNR, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

            clsDevexpressGrid.ItemLookUpEditSetup(gridOutCboUnit, clsCommon.GetUnit(), "", false, false);

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "전체선택", false, 2, true);
            #endregion

            #region decar
            //ERP 전송상태
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboERP_UP_YN, clsCommon.GetTransFlag());

            //차량 입고 타입
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboCAR_TYPE, clsCommon.GetCarInputType(), "", false, false);

            //차량 그룹
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarCboVEHICLEGROUPCODE, clsCommon.getCarGroupType(), "", false, false);

            // 트레일러 무게
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarCboTR_WEIGHT, clsCommon.GetTR_WEIGHT(), "", false, false);

            // 진행 상태
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboPC_STATUS, clsCommon.GetCarStatus());

            // 차량
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridDecarscboINCAR_NO, clsCommon.GetCarMaster());

            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboYN, clsCommon.GetYn(), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "", false, false);

            #endregion

            #region 상차 실적
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboPlant, clsCommon.GetPlant());

            // 오더분류
            clsDevexpressGrid.ItemLookUpEditSetup(gridOutCboESART, clsCommon.GetESART());

            // 포장코드
            clsDevexpressGrid.ItemLookUpEditSetup(gridOutCboMAGRV, clsCommon.GetMAGRV(), "", false, false);

            // 구매조직
            clsDevexpressGrid.ItemLookUpEditSetup(gridOutCboEKORG, clsCommon.GetEKORG(), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridOutCboUnit, clsCommon.GetUnit(), "", false, false);

            // 하차 상태
            clsDevexpressGrid.ItemLookUpEditSetup(gridOutcboPC_STATUS, clsCommon.GetOutStatus());
            #endregion

            #region 파렛트
            Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "무게" }
                };

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridPascboPTMCD, clsCommon.getPallet(), "", true, parameterDict);
            #endregion

            if (clsCommon.BarcodeConnYn == "Y")
                SetBarcode();
        }

        private void SetBarcode()
        {
            _rs232 = new SerialPortHelper(Properties.Settings.Default.Com, Properties.Settings.Default.BaudRate, Properties.Settings.Default.DataBit, System.IO.Ports.Parity.None);
            _rs232.DataReceived += Rs232_DataReceived;
            _rs232.Open();
        }

        private void Rs232_DataReceived(string data)
        {
            // UI 컨트롤 업데이트는 Invoke 필요
            this.Invoke(new MethodInvoker(() =>
            {
                var sub = Application.OpenForms["frm_Input_Poorderm"] as frm_Input_Poorderm;

                if (sub != null && !sub.IsDisposed)   // 폼이 열려있으면
                {
                    clsUtil.Delay(500);
                    txtQRCode.Text = "";
                    txtQRCode.Text = data; // 바코드 값
                }
                else
                {
                    ShowMessageBox.XtraShowWarning("[구매 발주 정보] 메뉴가 열려있지 않습니다.");
                }
            }));
        }

        #region 조회 쿼리
        private void XList_Search()
        {
            try
            {
                SQL = $@"
                WITH BSART AS (
                    -- 오더분류
                    SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
                    FROM COMM_DIV a
                        INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                        INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                    WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '45'
                ), WEIGHT_TYPE AS (
                    SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
                    FROM COMM_DIV a
                        INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                        INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                    WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '04'
                )

                SELECT DISTINCT
                        derd.EINDT              -- 납품예정일
                    , derm.EBELN                -- 발주번호
                    , derd.EBELP                -- 발주 항번
                    , bsart.NAME AS BSART       -- PO구분
                    , plant.P_DESCRIPTION AS WERKS                -- 플랜트 코드
                    , derm.LIFNR                -- 부두업체코드
                    , derm.NAME1                -- 부두업체명
                    , TO_CHAR(TO_DATE(derm.BEDAT, 'YYYY-MM-DD'), 'YYYY-MM-DD') AS BEDAT     -- 발주일자
                    , derm.LIFN2                -- 운송업체
                    , derm.NAME2                -- 운송업체명
                    , derd.MATNR AS POMATNR     -- 구매자재번호
                    , derd.TXZ01                -- 구매자재명 또는 단가설명
                    , derd.CHARG                -- 모선코드
                    , derd.CHARG_TEXT           -- 모선명
                    , derd.RESLO                -- 출고 창고 부두 창고
                    , derd.LOGBE_ISSUU          -- 출고 창고명
                    , derd.LGORT                -- 입고 창고
                    , derd.LGOBE_RECV           -- 입고 창고명
                    , derd.MENGE                -- 주문 수량
                    , derd.MEINS                -- 주문 단위
                    , decar.IS_NO               -- 발급번호
                    , CASE TO_CHAR(decar.CAR_TYPE) 
                                            WHEN '001' THEN '원료입고' 
                                        END AS CAR_TYPE                                             -- 차량입고타입
                    , decar.INCAR_NO            -- 차량전체번호
                    , decar.WEIGHT_KG           -- 계근번호
                    , decar.IN_WEIGHT           -- 입차중량
                    , decar.OUT_WEIGHT          -- 출차중량
                    , decar.INCAR_DATE          -- 입차일시
                    , decar.OUTCAR_DATE         -- 출차일시
                    , decar.PC_STATUS           -- 진행상태
                    , gocar.ESART               -- 오더분류
                    , gocar.SRM_PREV_GR         -- 계획번호
                    , gocar.PARTNER             -- 공급사
                    , gocar.EKORG               -- 공급사명
                    , gocar.SHIP_NAME           -- 모선명
                    , gocar.INVOICE_WEIGHT      -- 송장량
                    , gocard.SEQ                -- ?
                    , gocard.MTART              -- 자재유형
                    , gocard.MATNR              -- 하차 자재
                    , product.DESCRIPTION       -- 하차 자재명
                    , gocard.MAGRV              -- 포장
                    , gocard.MEINS              -- 하차단위
                    , gocard.GR_QNTY            -- 납품수량
                    , gocard.GR_QNTY_EA         -- 납품수량(EA)
                    , gocard.GR_QNTY_BOX        -- 납품수량(BOX)
                    , gocard.GR_QNTY_KG         -- 납품수량(KG)
                    , gocard.R_GR_QNTY          -- 하차량
                    , CASE WHEN NVL(ingred.WEIGHT_TYPE, '01') = '02'
                            THEN CASE WHEN gocard.WERKS IN ('P101', 'P102', 'PJ01', 'PJ02', 'PJ04', 'PJ05') 
                                    THEN NVL(gocard.R_GR_QNTY, 0) - ROUND(NVL(inadd.WEIGHT, 0), -1) 
                                ELSE NVL(gocard.R_GR_QNTY, 0) - NVL(inadd.WEIGHT, 0) END
                            ELSE NVL(gocard.R_GR_QNTY, 0)  END TRAN_QNTY                -- 전송중량
                    , inadd.WEIGHT                      -- 파렛트 무게
                    , gocard.EA_PACK_UNIT               -- 제조일자
                    , gocard.REMARK                     -- 비고
                    , gocard.JJO                        -- 제조일자
                    , gocard.SBDAY                      -- 소비기한
                    , gocard.I_TIME
                    , gocard.I_USER
                    , wt.NAME AS WEIGHT_TYPE            -- 계근타입 
                    , decar.ERP_UP_YN                   -- ERP 전송상태 
                    , derd.ELIKZ                        -- 오더 클로징 여부
                    , derd.LOEKZ                        -- 삭제여부
                    , derm.MTYPE                        -- 메세지유형
                    , derm.MESSAGE                      -- 메시지
                FROM SAP_INPUT_POORDERM_CON derm
                    JOIN SAP_INPUT_POORDERD_CON derd ON derd.EBELN = derm.EBELN
                    LEFT JOIN SAP_DI_PLANT plant ON plant.PLANT_CODE = derd.WERKS
                    LEFT JOIN BSART bsart ON bsart.CODE = derm.BSART
                    LEFT JOIN WAP_GOCAR gocar ON gocar.EBELN = derm.EBELN
                    LEFT JOIN WAP_GOCARD gocard ON gocard.IS_NO = gocar.IS_NO AND gocard.EBELN = gocar.EBELN
                    LEFT JOIN (SELECT EBELN, EBELP, IS_NO, SUM(WEIGHT) * SUM(PD_QTY) AS WEIGHT
                                        FROM WAP_IN_ADD
                                        GROUP BY EBELN, EBELP, IS_NO) inadd ON inadd.EBELN = gocard.EBELN 
                                            AND inadd.EBELP = gocard.EBELP AND inadd.IS_NO = gocard.IS_NO
                    LEFT JOIN SAP_DI_PRODUCT product ON product.PLANT_CODE = gocard.WERKS AND product.RESOURCE_NO = gocard.MATNR
                    LEFT JOIN WAP_DECAR decar ON decar.IS_NO = gocar.IS_NO 
                                                        AND decar.CAR_TYPE IN ('001')
                    LEFT JOIN INGRED ingred ON ingred.PLANT_CODE = gocard.WERKS AND ingred.RESOURCE_NO = gocard.MATNR
                    LEFT JOIN WEIGHT_TYPE wt ON wt.CODE = NVL(ingred.WEIGHT_TYPE, '01')
                WHERE derd.WERKS = '{cboPlant_Code.EditValue}'
                    AND derd.EINDT BETWEEN TO_CHAR(TO_DATE('{DateTime.Parse(dtFromDeliveryDate.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD'), 'YYYYMMDD') 
                                    AND TO_CHAR(TO_DATE('{DateTime.Parse(dtToDeliveryDate.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD'), 'YYYYMMDD')
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(decar.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                ORDER BY decar.IS_NO DESC, derd.EINDT DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridList, viewList, ds.Tables[0], false, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "pack_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT DISTINCT
                      a.EBELN     -- 발주번호
                    , a.BSART     -- PO구분
                    , a.LIFNR     -- 내외자구분
                    , a.NAME1     -- 부두업체명
                    , TO_CHAR(TO_DATE(a.BEDAT, 'YYYY-MM-DD'), 'YYYY-MM-DD') AS BEDAT     -- 발주일자
                    , a.LIFN2     -- 운송업체
                    , a.NAME2     -- 운송업체명
                    , a.MTYPE     -- 메세지유형
                    , a.MESSAGE   -- 메시지
                    , b.EINDT     -- 납품예정일
                FROM SAP_INPUT_POORDERM_CON a
                    JOIN SAP_INPUT_POORDERD_CON b ON b.EBELN = a.EBELN
                WHERE b.WERKS = '{cboPlant_Code.EditValue}'
                    AND b.EINDT BETWEEN TO_CHAR(TO_DATE('{DateTime.Parse(dtFromDeliveryDate.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD'), 'YYYYMMDD') 
                                    AND TO_CHAR(TO_DATE('{DateTime.Parse(dtToDeliveryDate.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD'), 'YYYYMMDD') 
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridPoor, viewPoor, ds.Tables[0], false, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "pack_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XDetail_Search()
        {
            SQL = $@"
            SELECT 
                  EBELN         -- 발주번호 / 구매오더 번호
                , EBELP         -- 발주 항번
                , MATNR         -- 자재번호
                , TXZ01         -- 자재명 또는 단가설명
                , WERKS         -- 플랜트 코드
                , CHARG         -- 모선코드
                , CHARG_TEXT    -- 모선명
                , RESLO         -- 출고 창고 부두 창고
                , LOGBE_ISSUU   -- 출고 창고명
                , LGORT         -- 입고 창고
                , LGOBE_RECV    -- 입고 창고명
                , MENGE         -- 주문 수량
                , MEINS         -- 주문 단위
                , ELIKZ         -- 오더 클로징 여부
                , LOEKZ         -- 삭제여부
                , EINDT         -- 납품예정일
            FROM SAP_INPUT_POORDERD_CON
            WHERE EBELN = '{viewPoor.GetFocusedRowCellValue("EBELN")}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressGrid.BindGridControl(gridPoorD, viewPoorD, ds.Tables[0], false, true);
        }

        private void XResult_Select()
        {
            try
            {
                SQL = $@"
                SELECT 
                    a.IS_NO, a.EBELN, b.EBELP
                    , a.ESART, a.SRM_PREV_GR, a.PARTNER, a.EKORG
                    , a.SHIP_NAME, a.INVOICE_WEIGHT
                    , b.SEQ, b.MTART, b.MATNR
                    , b.MAGRV, b.MEINS, b.GR_QNTY
                    , b.GR_QNTY_EA, b.GR_QNTY_BOX, b.GR_QNTY_KG
                    , b.WERKS, b.EA_PACK_UNIT, b.REMARK
                    , b.I_TIME, b.I_USER
                FROM WAP_GOCAR a
                    INNER JOIN WAP_GOCARD b ON b.IS_NO = a.IS_NO AND b.EBELN = a.EBELN
                WHERE a.EBELN = '{viewPoorD.GetFocusedRowCellValue("EBELN")}' AND b.EBELP = '{viewPoorD.GetFocusedRowCellValue("EBELP")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridResult, viewResult, ds.Tables[0], true);



                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xDetail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XDeCar_Search()
        {
            try
            {
                DateTime dtFrom = dtFromDeliveryDate.DateTime.Date;
                DateTime dtTo = dtToDeliveryDate.DateTime.Date;

                SQL = $@"
                WITH GOCAR AS (
                    SELECT a.IS_NO, MIN(c.MATNR) AS RESOURCE_NO
                        , MAX(b.NAME1) AS NAME1, c.WERKS, NVL(SUM(CASE WHEN NVL(e.WEIGHT_TYPE, '01') = '02'
                                                                THEN CASE WHEN 'P201' IN ('P101', 'P102', 'PJ01', 'PJ02', 'PJ04', 'PJ05') 
                                                                        THEN NVL(c.R_GR_QNTY, 0) - ROUND(NVL(d.WEIGHT, 0), -1) 
                                                                    ELSE NVL(c.R_GR_QNTY, 0) - NVL(d.WEIGHT, 0) END
                                                                ELSE NVL(c.R_GR_QNTY, 0)  END), 0) TRAN_QTY
                    FROM WAP_GOCAR a
                        JOIN SAP_INPUT_POORDERM_CON b ON b.EBELN = a.EBELN
                        JOIN WAP_GOCARD c ON c.IS_NO = a.IS_NO AND c.EBELN = a.EBELN
                        LEFT JOIN (SELECT EBELN, EBELP, IS_NO, SUM(WEIGHT) * SUM(PD_QTY) AS WEIGHT
                                            FROM WAP_IN_ADD
                                            GROUP BY EBELN, EBELP, IS_NO) d ON d.EBELN = b.EBELN
                                                                AND d.EBELP = c.EBELP AND d.IS_NO = a.IS_NO
                        LEFT JOIN INGRED E ON e.PLANT_CODE = c.WERKS AND e.RESOURCE_NO = c.MATNR
                    GROUP BY a.IS_NO, c.WERKS
                )

                SELECT 
                     a.IS_NO              -- 발급번호
                   , b.RESOURCE_NO || ' : ' || c.DESCRIPTION AS RESOURCE_NO
                   , a.CAR_TYPE           -- 차량입고타입
                   , a.INCAR_NO           -- 차량전체번호
                   , a.VEHICLEGROUPCODE   -- 차량그룹코드
                   , a.WEIGHT_KG          -- 계근번호
                   , a.IN_WEIGHT          -- 입차중량
                   , a.OUT_WEIGHT         -- 출차중량
                   , (a.IN_WEIGHT - a.OUT_WEIGHT) AS R_GR_QNTY
                   , b.TRAN_QTY
                   , a.TR_YN              -- 트레일러유무
                   , a.TR_WEIGHT          -- 트레일러무게
                   , a.USER_ID            -- 확인관리자
                   , a.INCAR_DATE         -- 입차일시
                   , a.OUTCAR_DATE        -- 출차일시
                   , a.PC_STATUS          -- 진행상태
                   , a.ERP_UP_YN          -- ERP 전송상태 
                   , a.ERP_TNUMBER        -- ERP 전송일련번호
                   , a.DEL_FLAG           -- 삭제여부
                   , a.TEM_TYPE           -- 수동여부
                   , a.PRINT_YN           -- 프린터 여부
                   , a.I_TIME             -- 입력시간
                   , a.I_USER             -- 입력자
                   , a.ERR_MSG
                   , b.NAME1
                FROM WAP_DECAR a
                    LEFT JOIN GOCAR b ON b.IS_NO = a.IS_NO
                    LEFT JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = b.WERKS AND c.RESOURCE_NO = b.RESOURCE_NO
                WHERE a.INCAR_DATE BETWEEN TO_DATE('{dtFrom.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                                        AND TO_DATE('{dtTo.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                    AND a.CAR_TYPE = '{clsCommon.GetCarInputTypeCode("원료입고")}'
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                    AND (b.WERKS = '{cboPlant_Code.EditValue}' OR b.WERKS IS NULL)
                ORDER BY a.IS_NO DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDeCar, viewDeCar, ds.Tables[0], true);

                sDeValid = new string[] { "IS_NO" };

                viewDeCar.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void XOut_Select()
        {
            try
            {
                SQL = $@"
                WITH COMM AS (
                SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
                FROM COMM_DIV a
                    INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                    INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '04'
                )

                SELECT 
                    a.IS_NO, a.EBELN, b.EBELP
                    , a.ESART, a.SRM_PREV_GR, a.PARTNER, a.EKORG
                    , a.SHIP_NAME, a.INVOICE_WEIGHT
                    , b.SEQ, b.MTART, b.MATNR
                    , b.MAGRV, b.MEINS, b.GR_QNTY
                    , b.GR_QNTY_EA, b.GR_QNTY_BOX, b.GR_QNTY_KG
                    , b.R_GR_QNTY 
                    , CASE WHEN NVL(d.WEIGHT_TYPE, '01') = '02'
                            THEN CASE WHEN '{clsCommon.PlantCode}' IN ('P101', 'P102', 'PJ01', 'PJ02', 'PJ04', 'PJ05') 
                                    THEN NVL(b.R_GR_QNTY, 0) - ROUND(NVL(c.WEIGHT, 0), -1) 
                                ELSE NVL(b.R_GR_QNTY, 0) - NVL(c.WEIGHT, 0) END
                            ELSE NVL(b.R_GR_QNTY, 0)  END TRAN_QNTY
                    , b.WERKS, b.EA_PACK_UNIT, b.REMARK
                    , b.JJO, b.SBDAY, b.I_TIME, b.I_USER
                    , E.NAME AS WEIGHT_TYPE
                FROM WAP_GOCAR a
                    INNER JOIN WAP_GOCARD b ON b.IS_NO = a.IS_NO AND b.EBELN = a.EBELN
                    LEFT JOIN (SELECT EBELN, EBELP, IS_NO, SUM(WEIGHT) * SUM(PD_QTY) AS WEIGHT
                        FROM WAP_IN_ADD
                        GROUP BY EBELN, EBELP, IS_NO) c ON c.EBELN = a.EBELN AND c.EBELP = b.EBELP AND c.IS_NO = a.IS_NO
                    LEFT JOIN INGRED D ON D.PLANT_CODE = b.WERKS AND d.RESOURCE_NO = b.MATNR
                    LEFT JOIN COMM E ON e.CODE = NVL(d.WEIGHT_TYPE, '01')
                WHERE a.IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridOut, viewOut, ds.Tables[0], true);

                sOutValid = new string[] { "IS_NO", "EBELN" };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutScboMATNR, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xDetail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XPallet_Select()
        {
            try
            {
                SQL = $@"
                SELECT a.EBELN, a.EBELP, a.IS_NO                                -- 01. 발급번호
                   , a.PTMCD                                -- 02. 팔렛트코드
                   , b.WEIGHT                               -- 03. 중량
                   , a.PD_QTY                               -- 04. 수량
                   , (b.WEIGHT * a.PD_QTY) AS TWEIGHT       -- 05. 총중량
                   , a.I_TIME                               -- 06. 입력시간
                FROM WAP_IN_ADD a
                    LEFT JOIN WAP_PA_MASTER b ON b.PTMCD = a.PTMCD
                WHERE a.IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridPallet, viewPallet, ds.Tables[0], true);

                sPaValid = new string[] { "PTMCD", "WEIGHT" };

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xDetail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                if (tabPage.SelectedTabPageIndex == 0)
                    XList_Search();
                else if (tabPage.SelectedTabPageIndex == 1)
                    XMain_Search();
                else
                    XDeCar_Search();
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

        #region 구매 발주 이벤트
        private void frm_Shown(object sender, EventArgs e)
        {
            gridPoor.Focus();
            viewPoor.FocusedRowHandle = 0;
            viewPoor.FocusedColumn = viewPoor.VisibleColumns[0];
        }

        private void viewPoor_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            SetRowNum(e);
        }

        private void viewPoorD_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            SetRowNum(e);
        }

        private void viewPoorD_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //if (viewPoorD.GetFocusedRowCellValue("ERP_UP_YN").ToString().Trim().Equals("전송완료")) //작지가 완료처리된것은 수정못하도록 에디트모드 off
                //{
                //    e.Cancel = true;
                //}
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_detail_ShowingEditor", ex);
            }
        }

        private void tabPoor_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (tabPage.SelectedTabPageIndex == 0)
            {
                layoutControlItem32.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_workdate.Text = "납품예정일";
                XList_Search();
            }
            else if (tabPage.SelectedTabPageIndex == 1)
            {
                layoutControlItem32.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_workdate.Text = "납품예정일";
                XMain_Search();
            }
            else
            {
                layoutControlItem32.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_workdate.Text = "입차일자";
                XDeCar_Search();
            }
        }

        private void tabPoor_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void viewPoor_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridPoorD.DataSource = null;
                gridResult.DataSource = null;

                XDetail_Search();
                XResult_Select();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        private void viewPoorD_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridPoorD.DataSource = null;

                XResult_Select();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (tabPage.SelectedTabPageIndex == 0)
                XList_Search();
            else if (tabPage.SelectedTabPageIndex == 1)
                XMain_Search();
            else
                XDeCar_Search();
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            if (tabPage.SelectedTabPageIndex == 0)
                XList_Search();
            else if (tabPage.SelectedTabPageIndex == 1)
                XMain_Search();
            else
                XDeCar_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPoor);
            DataTable DT = (DataTable)gridPoor.DataSource;

            if (DT == null)
            {
                return;
            }

            int rowAddCnt = 0;
            foreach (DataRow dr in DT.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    rowAddCnt += 1;
                }

                if (rowAddCnt > 0)
                {
                    ShowMessageBox.XtraShowInformation("추가된 신규입력을 저장후 행추가 하세요");
                    return;
                }
            }

            clsDevexpressGrid.GridViewAddRow(viewPoor);


            viewPoor.SetRowCellValue(viewPoor.FocusedRowHandle, viewPoor.Columns["INCAR_TIME"], string.Format("{0:yyyy-MM-dd}", dtFromDeliveryDate.EditValue) + " " + DateTime.Now.ToString("HH:mm:ss.fff"));
            viewPoor.SetRowCellValue(viewPoor.FocusedRowHandle, viewPoor.Columns["OUTCAR_TIME"], string.Format("{0:yyyy-MM-dd}", dtFromDeliveryDate.EditValue) + " " + DateTime.Now.ToString("HH:mm:ss.fff"));

        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPoor);
            clsDevexpressGrid.GridViewAddRowDelete(viewPoor);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("구매 발주 상세 내역을 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewPoorD);
                DataTable DT = (DataTable)gridPoorD.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                string is_no = string.Empty;
                string vendor_no = string.Empty;

                Dbconn.conn.BeginTransaction();

                foreach (DataRow dr in DT.Rows)
                {
                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        string VEHICLEGROUPCODE = string.Empty;

                        VEHICLEGROUPCODE = "N";

                        SQL = "SELECT TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') AS SEQ FROM DUAL ";
                        using (DataSet isnoDs = Dbconn.conn.ExecutDataset(SQL))
                        {
                            if (Dbconn.conn.getRowCnt(isnoDs) > 0)
                            {
                                is_no = Dbconn.conn.getData(isnoDs, "SEQ", 0);
                            }
                        }

                        SQL = $@"
                        INSERT INTO SAP_INPUT_POORDERM_CON (
                                   EBELN                          -- 1
                                 , BSART                          -- 2
                                 , LIFNR                          -- 3
                                 , NAME1                          -- 4
                                 , BEDAT                          -- 5
                                 , LIFN2                          -- 6
                                 , NAME2                          -- 7
                                 , MTYPE                          -- 8
                                 , MESSAGE                        -- 9
                                 , I_TIME                         -- 10
                        )
                        VALUES (
                                   '{dr["EBELN"]}'                -- 1
                                 , '{dr["BSART"]}'                -- 2
                                 , '{dr["LIFNR"]}'                -- 3
                                 , '{dr["NAME1"]}'                -- 4
                                 , '{dr["BEDAT"]}'                -- 5
                                 , '{dr["LIFN2"]}'                -- 6
                                 , '{dr["NAME2"]}'                -- 7
                                 , '{dr["MTYPE"]}'                -- 8
                                 , '{dr["MESSAGE"]}'              -- 9
                                 , SYSDATE                        -- 10
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE SAP_INPUT_POORDERD_CON
                        SET
                                   MATNR       = '{dr["MATNR"]}'           -- 1
                                 , TXZ01       = '{dr["TXZ01"]}'           -- 2
                                 , WERKS       = '{dr["WERKS"]}'           -- 3
                                 , CHARG       = '{dr["CHARG"]}'           -- 4
                                 , CHARG_TEXT  = '{dr["CHARG_TEXT"]}'      -- 5
                                 , RESLO       = '{dr["RESLO"]}'           -- 6
                                 , LOGBE_ISSUU = '{dr["LOGBE_ISSUU"]}'     -- 7
                                 , LGORT       = '{dr["LGORT"]}'           -- 8
                                 , LGOBE_RECV  = '{dr["LGOBE_RECV"]}'      -- 9
                                 , MENGE       = '{dr["MENGE"]}'           -- 10
                                 , MEINS       = '{dr["MEINS"]}'           -- 11
                                 , ELIKZ       = '{dr["ELIKZ"]}'           -- 12
                                 , LOEKZ       = '{dr["LOEKZ"]}'           -- 13
                                 , EINDT       = '{dr["EINDT"]}'           -- 14
                                 , I_TIME      = SYSDATE                   -- 15
                        WHERE
                                   EBELN       = '{dr["EBELN"]}'
                        AND        EBELP       = '{dr["EBELP"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }

                    }

                    dr.AcceptChanges();

                } //foreach


                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowInformation("구매 발주 상세 내역을 저장했습니다.");

                XMain_Search();

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                clsLog.logSave(this, "btn_save_Click", SQL);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                if (viewPoor.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 차량입고정보를 선택하여 주세요");
                    return;
                }

                DataRow row = viewPoor.GetDataRow(viewPoor.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewPoor.DeleteRow(viewPoor.FocusedRowHandle);
                }
                else
                {
                    string IS_NO = viewPoor.GetRowCellValue(viewPoor.FocusedRowHandle, viewPoor.Columns["IS_NO"]).ToString();
                    string INCAR_NO = viewPoor.GetRowCellValue(viewPoor.FocusedRowHandle, viewPoor.Columns["INCAR_NO"]).ToString();

                    DialogResult result = ShowMessageBox.Confirm("선택하신 " + INCAR_NO + "차량 입고정보를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        SQL = $"DELETE FROM WAP_DECAR WHERE IS_NO = '{IS_NO}'  ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("차량 입고정보 삭제에 실패했습니다");
                            return;
                        }

                        SQL = $"DELETE FROM WAP_GOCAR WHERE IS_NO = '{IS_NO}'  ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("차량 입고정보 삭제에 실패했습니다");
                            return;
                        }

                        SQL = $"DELETE FROM WAP_GOCARD WHERE IS_NO = '{IS_NO}'  ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("차량 입고정보 삭제에 실패했습니다");
                            return;
                        }

                        gridPoorD.DataSource = null;

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

        #region 차량 정보 이벤트

        private void viewDeCar_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridOut.DataSource = null;
                gridPallet.DataSource = null;

                sIS_NO = clsDevexpressGrid.GetFocusedRowCellValue(viewDeCar, "IS_NO");

                if (sIS_NO == null)
                    return;

                XOut_Select();

                XPallet_Select();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "viewIngred_FocusedRowChanged", ex);
            }
        }

        private void viewDeCar_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            SetRowNum(e);
        }

        private void btn_DeCar_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewDeCar.AddNewRow();
            int newRowHandle = viewDeCar.FocusedRowHandle;

            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["I_TIME"], DateTime.Now);
            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["CAR_TYPE"], clsCommon.GetCarInputTypeCode("원료입고"));
            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["IS_NO"], DateTime.Now.ToString("yyyyMMddHHmmsss"));
            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["WEIGHT_KG"], DateTime.Now.ToString("yyyyMMddHHmmsss"));

            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["DEL_FLAG"], "N");
            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["TEM_TYPE"], "Y");

            viewDeCar.ShowEditor();
        }

        private void btn_DeCar_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewDeCar);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewDeCar);
        }

        private void btn_DeCar_save_Click(object sender, EventArgs e)
        {
            XDecar_Save();
        }

        private void XDecar_Save()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(viewDeCar);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridDeCar.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sDeValid, dr, viewDeCar);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewDeCar.FocusedColumn = viewDeCar.Columns[rValid]; // 이동할 컬럼명
                        viewDeCar.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    DateTime? incarDate = null;
                    string sIncarDate = string.Empty;
                    string sOutcarDate = string.Empty;

                    if (!string.IsNullOrEmpty(dr["INCAR_DATE"].ToString()))
                    {
                        DateTime dtFrom = DateTime.Parse(dr["INCAR_DATE"].ToString());

                        sIncarDate = $"TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (!string.IsNullOrEmpty(dr["INCAR_DATE"].ToString()) && !string.IsNullOrEmpty(dr["OUTCAR_DATE"].ToString()))
                    {
                        int time_diff = Convert.ToDateTime(Convert.ToDateTime(dr["OUTCAR_DATE"]).ToString("yyyy-MM-dd HH:mm:ss")).CompareTo(Convert.ToDateTime(Convert.ToDateTime(dr["INCAR_DATE"]).ToString("yyyy-MM-dd HH:mm:ss")));
                        if (time_diff < 0)
                        {
                            ShowMessageBox.XtraShowInformation("종료시간이 시작시간보다 빠르거나 같습니다");
                            return;
                        }

                        DateTime dtTo = DateTime.Parse(dr["OUTCAR_DATE"].ToString());

                        sOutcarDate = $"TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO WAP_DECAR (
                           IS_NO, CAR_TYPE, INCAR_NO, 
                           VEHICLEGROUPCODE, WEIGHT_KG, IN_WEIGHT, 
                           OUT_WEIGHT, TR_YN, TR_WEIGHT, 
                           USER_ID, INCAR_DATE, OUTCAR_DATE, 
                           PC_STATUS, ERP_UP_YN, ERP_TNUMBER, 
                           DEL_FLAG, TEM_TYPE, PRINT_YN, 
                           I_TIME, I_USER) 
                        VALUES (
                           '{dr["IS_NO"]}'
                         , '{dr["CAR_TYPE"]}'
                         , '{dr["INCAR_NO"]}'
                         , '{dr["VEHICLEGROUPCODE"]}'
                         , '{dr["WEIGHT_KG"]}'
                         , '{dr["IN_WEIGHT"]}'
                         , '{dr["OUT_WEIGHT"]}'
                         , '{dr["TR_YN"]}'
                         , '{dr["TR_WEIGHT"]}'
                         , '{dr["USER_ID"]}'
                         , ({(string.IsNullOrEmpty(sIncarDate) ? "''" : $"{sIncarDate}")})
                         , ({(string.IsNullOrEmpty(sOutcarDate) ? "''" : $"{sOutcarDate}")})
                         , '{dr["PC_STATUS"]}'
                         , 'N'
                         , '{dr["ERP_TNUMBER"]}'
                         , '{dr["DEL_FLAG"]}'
                         , '{dr["TEM_TYPE"]}'
                         , '{dr["PRINT_YN"]}'
                         , SYSDATE
                         , '{clsCommon.UserId}'
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE WAP_DECAR
                        SET    CAR_TYPE         = '{dr["CAR_TYPE"]}'
                             , INCAR_NO         = '{dr["INCAR_NO"]}'
                             , VEHICLEGROUPCODE = '{dr["VEHICLEGROUPCODE"]}'
                             , WEIGHT_KG        = '{dr["WEIGHT_KG"]}'
                             , IN_WEIGHT        = '{dr["IN_WEIGHT"]}'
                             , OUT_WEIGHT       = '{dr["OUT_WEIGHT"]}'
                             , TR_YN            = '{dr["TR_YN"]}'
                             , TR_WEIGHT        = '{dr["TR_WEIGHT"]}'
                             , USER_ID          = '{dr["USER_ID"]}'
                             , INCAR_DATE       = ({(string.IsNullOrEmpty(sIncarDate) ? "''" : $"{sIncarDate}")})
                             , OUTCAR_DATE      = ({(string.IsNullOrEmpty(sOutcarDate) ? "''" : $"{sOutcarDate}")})
                             , PC_STATUS        = '{dr["PC_STATUS"]}'
                             , ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                             , ERP_TNUMBER      = '{dr["ERP_TNUMBER"]}'
                             , DEL_FLAG         = '{dr["DEL_FLAG"]}'
                             , TEM_TYPE         = '{dr["TEM_TYPE"]}'
                             , PRINT_YN         = '{dr["PRINT_YN"]}'
                             , I_TIME           = SYSDATE
                             , I_USER           = '{clsCommon.UserId}'
                        WHERE IS_NO             = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("차량 정보를 저장 했습니다");

                XDeCar_Search();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_DeCar_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewDeCar.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewDeCar);

            if (new string[] { "Y", "D", "R" }.Contains(viewDeCar.GetFocusedRowCellValue("ERP_UP_YN")?.ToString()))
            {
                ShowMessageBox.XtraShowWarning("ERP 전송이 완료된 실적은 변경 할수 없습니다.");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 차량정보를 삭제하시겠습니까?"))
            {
                return;
            }

            XDecar_Delete();
        }

        private void XDecar_Delete()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                splashScreenManager.ShowWaitForm();

                SQL = $"UPDATE WAP_DECAR SET DEL_FLAG = 'Y' WHERE IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                XDeCar_Search();
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

        #region 상차 내역 이벤트
        private void viewOut_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            string erpUpYn = viewOut.GetFocusedRowCellValue("ERP_UP_YN")?.ToString();
            string ebeln = viewOut.GetFocusedRowCellValue("EBELN")?.ToString();
            string ebelp = viewOut.GetFocusedRowCellValue("EBELP")?.ToString();

            if (erpUpYn == "Y"
                || !string.IsNullOrEmpty(ebeln)
                || !string.IsNullOrEmpty(ebelp))
            return;

            if (e.Column.FieldName == "EBELN")
            {
                // 선택된 행의 데이터 가져오기

                // 새 폼에 데이터 전달
                using (m_SAP_INPUT_POORDERM_CON child = new m_SAP_INPUT_POORDERM_CON(clsDevexpressGrid.GetFocusedRowCellValue(viewOut, "IS_NO")?.ToString()))
                {
                    child.StartPosition = FormStartPosition.CenterParent;
                    if (child.ShowDialog() == DialogResult.OK)
                    {
                        viewOut.SetFocusedValue(child.vEBELN);
                        viewOut.SetFocusedRowCellValue("ESART", child.vBSART);
                    }
                }
            }

            if (e.Column.FieldName == "EBELP")
            {
                // 선택된 행의 데이터 가져오기
                string sEBELN = clsDevexpressGrid.GetFocusedRowCellValue(viewOut, "EBELN");

                // 새 폼에 데이터 전달
                using (m_SAP_INPUT_POORDERD_CON child = new m_SAP_INPUT_POORDERD_CON(cboPlant_Code.EditValue?.ToString(), sEBELN))
                {
                    child.StartPosition = FormStartPosition.CenterParent;
                    if (child.ShowDialog() == DialogResult.OK)
                    {
                        viewOut.SetFocusedValue(child.vEBELP);
                        viewOut.SetFocusedRowCellValue("MATNR", child.vResourceNo);
                    }
                }
            }
        }

        private void btn_Out_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewOut.AddNewRow();
            int newRowHandle = viewOut.FocusedRowHandle;

            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["IS_NO"], viewDeCar.GetFocusedRowCellValue("IS_NO"));
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["CH_YN"], DateTime.Now);
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["I_TIME"], DateTime.Now);
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["PLANT_CODE"], DateTime.Now);

            viewOut.ShowEditor();
        }

        private void btn_Out_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewOut);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewOut);
        }

        private void btn_Out_save_Click(object sender, EventArgs e)
        {
            XOut_Save();
        }

        private void XOut_Save()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(viewOut);

                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridOut.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sOutValid, dr, viewOut);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewOut.FocusedColumn = viewOut.Columns[rValid]; // 이동할 컬럼명
                        viewOut.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    double rGrQnty = 0;

                    if (dr.Table.Columns.Contains("R_GR_QNTY") &&
                        dr["R_GR_QNTY"] != DBNull.Value &&
                        !string.IsNullOrWhiteSpace(dr["R_GR_QNTY"].ToString()))
                    {
                        rGrQnty = Convert.ToDouble(dr["R_GR_QNTY"]);
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        string VEHICLEGROUPCODE = string.Empty;
                        VEHICLEGROUPCODE = "N";

                        SQL = $@"
                        SELECT 1
                        FROM WAP_GOCAR
                        WHERE IS_NO = '{dr["IS_NO"]}'
                            AND EBELN = '{dr["EBELN"]}'
                        ";

                        DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(ds) == 0)
                        {
                            SQL = $@"
                            INSERT INTO WAP_GOCAR (
                               IS_NO, EBELN, ESART, 
                               SRM_PREV_GR, PARTNER, EKORG, 
                               SHIP_NAME, INVOICE_WEIGHT) 
                            VALUES ( 
                               '{dr["IS_NO"]}'
                             , '{dr["EBELN"]}'
                             , '{dr["ESART"]}'
                             , '{dr["SRM_PREV_GR"]}'
                             , '{dr["PARTNER"]}'
                             , '{dr["EKORG"]}'
                             , '{dr["SHIP_NAME"]}'
                             , '{dr["INVOICE_WEIGHT"]}' )
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 0)
                            {
                                clsLog.logSave(this, "btn_save_Click", SQL);
                                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                                return;
                            }
                        }

                        SQL = $@"
                        INSERT INTO WAP_GOCARD (
                           IS_NO, EBELN, EBELP, 
                           SEQ, MTART, MATNR, 
                           MAGRV, MEINS, GR_QNTY,
                           GR_QNTY_EA, GR_QNTY_BOX, GR_QNTY_KG, 
                           R_EA_PACK_UNIT, R_GR_QNTY, R_GR_QNTY_EA, R_TIME,
                           WERKS, EA_PACK_UNIT, REMARK, 
                           I_TIME, I_USER) 
                        VALUES ( 
                           '{dr["IS_NO"]}'
                         , '{dr["EBELN"]}'
                         , '{dr["EBELP"]}'
                         , '{dr["SEQ"]}'
                         , '{dr["MTART"]}'
                         , '{dr["MATNR"]}'
                         , '{dr["MAGRV"]}'
                         , '{dr["MEINS"]}'
                         , '{dr["GR_QNTY"]}'
                         , '{dr["GR_QNTY_EA"]}'
                         , '{dr["GR_QNTY_BOX"]}'
                         , '{dr["GR_QNTY_KG"]}'
                         , '{dr["EA_PACK_UNIT"]}'
                         , '{rGrQnty}'
                         , '{dr["GR_QNTY_EA"]}'
                         , SYSDATE
                         , '{dr["WERKS"]}'
                         , '{dr["EA_PACK_UNIT"]}'
                         , '{dr["REMARK"]}'
                         , SYSDATE
                         , '{dr["I_USER"]}')
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE WAP_DECAR
                        SET    ERP_UP_YN         = 'N'
                        WHERE  IS_NO       = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("상차마감이 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE WAP_GOCARD
                        SET    SEQ          = '{dr["SEQ"]}',
                               MTART        = '{dr["MTART"]}',
                               MATNR        = '{dr["MATNR"]}',
                               MAGRV        = '{dr["MAGRV"]}',
                               MEINS        = '{dr["MEINS"]}',
                               GR_QNTY      = '{dr["GR_QNTY"]}',
                               GR_QNTY_EA   = '{dr["GR_QNTY_EA"]}',
                               GR_QNTY_BOX  = '{dr["GR_QNTY_BOX"]}',
                               GR_QNTY_KG   = '{dr["GR_QNTY_KG"]}',
                               R_EA_PACK_UNIT = '{dr["EA_PACK_UNIT"]}',
                               R_GR_QNTY_EA = '{dr["GR_QNTY_EA"]}',
                               R_GR_QNTY    = '{rGrQnty}',
                               R_TIME       = SYSDATE,
                               WERKS        = '{dr["WERKS"]}',
                               EA_PACK_UNIT = '{dr["EA_PACK_UNIT"]}',
                               REMARK       = '{dr["REMARK"]}',
                               I_TIME       = SYSDATE,
                               I_USER       = '{dr["I_USER"]}'
                        WHERE  IS_NO        = '{dr["IS_NO"]}'
                        AND    EBELN        = '{dr["EBELN"]}'
                        AND    EBELP        = '{dr["EBELP"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE WAP_DECAR
                        SET    ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                        WHERE  IS_NO       = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("상차마감이 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("원료 하차정보를 저장 했습니다");

                XOut_Select();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_Out_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewOut.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewOut);

            if (viewDeCar.GetFocusedRowCellValue("ERP_UP_YN")?.ToString() == "Y")
            {
                ShowMessageBox.XtraShowWarning("ERP 전송이 완료된 실적은 변경 할수 없습니다.");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 상차실적을 삭제하시겠습니까?"))
            {
                return;
            }

            XOut_Delete();
        }

        private void XOut_Delete()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                splashScreenManager.ShowWaitForm();

                SQL = $@"
                DELETE FROM WAP_GOCAR
                WHERE  IS_NO = '{viewOut.GetFocusedRowCellValue("IS_NO")}'
                    AND EBELN = '{viewOut.GetFocusedRowCellValue("EBELN")}'
                    AND EBELN = '{viewOut.GetFocusedRowCellValue("EBELP")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                SQL = $@"
                DELETE FROM WAP_GOCARD 
                WHERE IS_NO = '{viewOut.GetFocusedRowCellValue("IS_NO")}'
                    AND EBELN = '{viewOut.GetFocusedRowCellValue("EBELN")}'
                    AND EBELP = '{viewOut.GetFocusedRowCellValue("EBELP")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowWarning("원료 하차정보를 삭제 했습니다");

                XOut_Select();
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

        private static void SetRowNum(RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        #region 팔렛트 이벤트
        private void btnPalRowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewPallet.AddNewRow();
            int newRowHandle = viewPallet.FocusedRowHandle;

            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["IS_NO"], viewOut.GetFocusedRowCellValue("IS_NO"));
            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["EBELN"], viewOut.GetFocusedRowCellValue("EBELN"));
            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["EBELP"], viewOut.GetFocusedRowCellValue("EBELP"));
            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["I_TIME"], DateTime.Now);

            viewPallet.ShowEditor();
        }

        private void btnPalRowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPallet);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewPallet);
        }

        private void btnPalSave_Click(object sender, EventArgs e)
        {
            XPallet_Save();
        }

        private void XPallet_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewPallet);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridPallet.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sPaValid, dr, viewPallet);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewPallet.FocusedColumn = viewPallet.Columns[rValid]; // 이동할 컬럼명
                        viewPallet.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO WAP_IN_ADD (
                             EBELN
                            , EBELP
                            , IS_NO     -- 01. 입고번호
                            , PTMCD    -- 02. 항목코드
                            , WEIGHT   -- 03. 중량
                            , PD_QTY   -- 04. 수량
                            , I_TIME   -- 05. 입력일시
                        ) VALUES (
                            '{dr["EBELN"]}'
                            , '{dr["EBELP"]}'
                            , '{dr["IS_NO"]}'
                            , '{dr["PTMCD"]}'
                            , '{dr["WEIGHT"]}'
                            , '{dr["PD_QTY"]}'
                            , SYSDATE
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE WAP_IN_ADD
                        SET    EBELN  = '{dr["EBELN"]}'
                             , EBELP  = '{dr["EBELP"]}'
                             , I_TIME = SYSDATE
                             , IS_NO  = '{dr["IS_NO"]}'
                             , PD_QTY = '{dr["PD_QTY"]}'
                             , PTMCD  = '{dr["PTMCD"]}'
                             , WEIGHT = '{dr["WEIGHT"]}'
                        WHERE  IS_NO  = '{dr["IS_NO"]}'
                            AND PTMCD  = '{dr["PTMCD"]}'
                            AND EBELN  = '{dr["EBELN"]}'
                            AND EBELP  = '{dr["EBELP"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("파렛트 정보를 저장 했습니다");

                XPallet_Select();
                XOut_Select();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btnPalDelete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewPallet.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewPallet);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 팔렛트를 삭제하시겠습니까?"))
            {
                return;
            }

            XPallet_Delete();
        }

        private void XPallet_Delete()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                splashScreenManager.ShowWaitForm();

                SQL = $"DELETE FROM WAP_IN_ADD WHERE IS_NO = '{viewPallet.GetFocusedRowCellValue("IS_NO")}' AND PTMCD = '{viewPallet.GetFocusedRowCellValue("PTMCD")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                XPallet_Select();
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

        private void txtQRCode_KeyDown(object sender, KeyEventArgs e)
        {
            string colName = string.Empty;
            object oParts = null;

            int start = 10;
            int step = 15;
            int count = 0;
            int current = start;

            if (e.KeyCode == Keys.Enter)
            {
                //"SRMSYS.VER.001\t168\n4510000225\tNB\tT0102506000001\tZJ030001\tT010\t\t    \t         \t\n10\t10\tROH1\t1223030026\t\tKG\t2000.000\t2.000 \t0.000\t0.000\tP201\t1000.000\tdetail notice 6\t20250611\t20250625";
                //"SRMSYS.VER.001\t273\n4500000952\tNB\tJ0002507000054\t10067519\tJ000\t\t2000\t경남81가1234\n10\t10\tROH1\t1223010008\t\tKG\t1000.000\t20.000\t0.000\t0.000\tPJ04\t50.000  \tKPP형 13형 1개\t\t\n20\t20\tROH1\t1223010013\t\tKG\t1000.000\t20.000\t0.000\t0.000\tPJ04\t50.000\tKPP형 13형 1개\t\t";
                //txtQRCode.Text = txtQRCode.Text;

                // TextBox에 들어온 "깨진 글자"를 ISO-8859-1 기준으로 바이트화
                //byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(txtQRCode.Text);

                //// 그 바이트를 UTF-8로 복원
                //string fixedText = Encoding.UTF8.GetString(bytes);

                //txtQRCode.Text = fixedText;

                string raw = txtQRCode.Text;

                // 문자열에 들어있는 "\\t", "\\n"을 실제 문자로 변환
                raw = raw.Replace("\\t", "\t")
                         .Replace("\\n", "\t")
                         .Replace("\\T", "\t"); // 필요 시 역슬래시 제거

                // 줄바꿈 제거 (원한다면)
                //raw = raw.Replace("\r", "").Replace("\n", "");

                // 탭으로 분리
                string[] parts = raw.Split('\t');

                while (current <= parts.Length)
                {
                    current += step;
                    if (current > parts.Length)
                    {
                        if (count == 0)
                            count = 1;

                        break;
                    }
                    count++;
                }

                DataTable dtMaster = clsCommon.GetQRFomatMaster();

                DataTable dtDetail = clsCommon.GetQRFomatDetail();

                string dtCode = dtMaster.AsEnumerable()
                  .Where(row => row.Field<string>("NAME") == "INCAR_NO")
                  .Select(row => row.Field<string>("CODE"))
                  .FirstOrDefault();

                if (viewDeCar.GetFocusedRowCellValue("INCAR_NO")?.ToString() != parts[Convert.ToInt16(dtCode)])
                {
                    ShowMessageBox.XtraShowWarning("차량번호가 상이하여 실적을 생성 할수 없습니다.");
                    clsDevexpressGrid.GridEndEdit(viewOut);
                    clsDevexpressGrid.GridViewLastAddRowDelete(viewOut);
                    return;
                }

                for (int n = 0; n < count; n++)
                {
                    for (int i = 0; i < dtMaster.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            viewOut.AddNewRow();

                            viewOut.SetFocusedRowCellValue("IS_NO", viewDeCar.GetFocusedRowCellValue("IS_NO"));
                            viewOut.SetFocusedRowCellValue("CH_YN", DateTime.Now);
                            viewOut.SetFocusedRowCellValue("I_TIME", DateTime.Now);
                            viewOut.SetFocusedRowCellValue("PLANT_CODE", DateTime.Now);
                        }

                        if (viewOut.Columns[dtMaster.Rows[i]["NAME"].ToString()] != null)
                        {
                            if (parts[Convert.ToInt16(dtMaster.Rows[i]["CODE"])].Contains("."))
                                oParts = Convert.ToDecimal(parts[Convert.ToInt16(dtMaster.Rows[i]["CODE"])]);
                            else
                                oParts = parts[Convert.ToInt16(dtMaster.Rows[i]["CODE"])];

                            viewOut.SetFocusedRowCellValue(dtMaster.Rows[i]["NAME"].ToString(), oParts);
                        }
                    }

                    for (int i = 0; i < dtDetail.Rows.Count; i++)
                    {
                        if (viewOut.Columns[dtDetail.Rows[i]["NAME"].ToString()] != null)
                        {
                            if (parts[Convert.ToInt16(dtDetail.Rows[i]["CODE"]) + (n * 15)].Contains("."))
                                oParts = Convert.ToDecimal(parts[Convert.ToInt16(dtDetail.Rows[i]["CODE"]) + (n * 15)]);
                            else
                                oParts = parts[Convert.ToInt16(dtDetail.Rows[i]["CODE"]) + (n * 15)];

                            viewOut.SetFocusedRowCellValue(dtDetail.Rows[i]["NAME"].ToString(), oParts);
                        }
                    }
                }

                viewOut.ShowEditor();

                XOut_Save();

                txtQRCode.Clear();
            }
        }

        private void dtFromDeliveryDate_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }

        private void dtToDeliveryDate_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }

        private void gridPascboPTMCD_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            DataTable dt = clsCommon.getPallet();

            // 선택된 row에서 type 가져오기
            DataRow[] rows = dt.Select($"CODE = '{textEditor.EditValue.ToString()}'");
            if (rows.Length > 0)
            {
                string typeValue = rows[0]["type"].ToString();

                // 현재 행의 type 컬럼에 값 세팅
                viewPallet.SetFocusedRowCellValue("WEIGHT", typeValue);
            }
        }

        private void btnERPUpload1_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewDeCar.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("전송 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 ERP로 전송 하시겠습니까?", "ERP의 기존 작업 내역은 삭제 후 현 작업 데이터를 재전송 합니다."))
            {
                return;
            }

            try
            {
                int[] selectedRows = viewDeCar.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewDeCar.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM WAP_DECAR
                    WHERE IS_NO = '{dr["IS_NO"]}'
                        AND ERP_UP_YN IN ('N', 'M', 'X', 'G')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE WAP_DECAR
                            SET ERP_UP_YN =  CASE TO_CHAR(ERP_UP_YN) WHEN 'N' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                     ELSE TO_CHAR(ERP_UP_YN) END
                                , ERP_ERR_CNT = 0
                        WHERE IS_NO = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                XDeCar_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_plcUpload_Click", ex.Message + "/" + ex.StackTrace);
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            ShowMessageBox.XtraShowInformation("선택된 정보가 전송 대기로 변경 되었습니다");
        }

        /// <summary>
        /// ERP 입고 취소 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnERPDelete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewDeCar.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("전송 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 ERP로 전송 하시겠습니까?", "ERP의 기존 작업 내역이 입고 취소 됩니다. 취소 확정 후 신규 작업을 재 생성해서 진행 해주세요."))
            {
                return;
            }

            try
            {
                int[] selectedRows = viewDeCar.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewDeCar.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM WAP_DECAR
                    WHERE IS_NO = '{dr["IS_NO"]}'
                        AND ERP_UP_YN IN ('Y')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE WAP_DECAR
                            SET ERP_UP_YN =  CASE '{dr["ERP_UP_YN"]}'
                                                   WHEN 'Y' THEN 'D'
                                                   ELSE '{dr["ERP_UP_YN"]}'
                                               END
                                , ERP_ERR_CNT = 0
                        WHERE IS_NO = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                XDeCar_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_plcUpload_Click", ex.Message + "/" + ex.StackTrace);
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            ShowMessageBox.XtraShowInformation("선택된 정보가 전송 대기로 변경 되었습니다");
        }

        private void viewDeCar_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
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

                if (dr.RowState != DataRowState.Added && sDeValid.Contains(fieldName))
                {
                    if (!view.IsNewItemRow(rowHandle))
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }

            string val = view.GetFocusedRowCellValue("ERP_UP_YN")?.ToString();

            if (val == "Y" || val == "D" || val == "C" || val == "R")
            {
                e.Cancel = true;
            }

            if (viewDeCar.FocusedColumn.FieldName == "RESOURCE_NO" || viewDeCar.FocusedColumn.FieldName == "TRAN_QTY")
                e.Cancel = true;
        }

        private void viewOut_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
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

                if (dr.RowState != DataRowState.Added && sOutValid.Contains(fieldName))
                {
                    if (!view.IsNewItemRow(rowHandle))
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }

            string val = viewDeCar.GetFocusedRowCellValue("ERP_UP_YN")?.ToString();

            if (val == "Y" || val == "D" || val == "C" || val == "R")
            {
                e.Cancel = true;
            }
        }

        private void viewPallet_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
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

                if (dr.RowState != DataRowState.Added && sPaValid.Contains(fieldName))
                {
                    if (!view.IsNewItemRow(rowHandle))
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }
        }

        private void btnWorkCopy_Click(object sender, EventArgs e)
        {
            XDecar_Copy();
        }

        private void XDecar_Copy()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewDeCar);
                Dbconn.conn.BeginTransaction();

                string sOldIsNo = viewDeCar.GetFocusedRowCellValue("IS_NO")?.ToString();
                string sNewIsNo = DateTime.Now.ToString("yyyyMMddHHmmsss");

                SQL = $@"
                INSERT INTO WAP_DECAR (
                    IS_NO, CAR_TYPE, INCAR_NO, 
                    VEHICLEGROUPCODE, WEIGHT_KG, IN_WEIGHT, 
                    OUT_WEIGHT, TR_YN, TR_WEIGHT, 
                    USER_ID, INCAR_DATE, OUTCAR_DATE, 
                    PC_STATUS, ERP_UP_YN, ERP_TNUMBER, 
                    DEL_FLAG, TEM_TYPE, PRINT_YN, 
                    I_TIME, I_USER, ERP_ERR_CNT) 
                SELECT 
                '{sNewIsNo}', CAR_TYPE, INCAR_NO, 
                   VEHICLEGROUPCODE, '{sNewIsNo}', IN_WEIGHT, 
                   OUT_WEIGHT, TR_YN, TR_WEIGHT, 
                   USER_ID, INCAR_DATE, OUTCAR_DATE, 
                   PC_STATUS, 'N', NULL, 
                   'N', TEM_TYPE, PRINT_YN, 
                   SYSDATE, '{clsCommon.UserId}', ERP_ERR_CNT
                FROM WAP_DECAR
                WHERE IS_NO = '{sOldIsNo}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "XDecar_Copy()", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return;
                }

                SQL = $@"
                INSERT INTO WAP_GOCAR (
                    IS_NO, EBELN, ESART, 
                    SRM_PREV_GR, PARTNER, EKORG, 
                    SHIP_NAME, INVOICE_WEIGHT) 
                SELECT '{sNewIsNo}', EBELN, ESART, 
                   SRM_PREV_GR, PARTNER, EKORG, 
                   SHIP_NAME, INVOICE_WEIGHT
                FROM WAP_GOCAR
                WHERE IS_NO = '{sOldIsNo}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "XDecar_Copy()", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return;
                }

                SQL = $@"
                INSERT INTO WAP_GOCARD (
                    IS_NO, EBELN, EBELP, 
                    SEQ, MTART, MATNR, 
                    MAGRV, MEINS, GR_QNTY, 
                    GR_QNTY_EA, GR_QNTY_BOX, GR_QNTY_KG, 
                    R_EA_PACK_UNIT, R_GR_QNTY, R_GR_QNTY_EA, R_TIME,
                    WERKS, EA_PACK_UNIT, REMARK, 
                    JJO, SBDAY, I_TIME, I_USER) 
                SELECT '{sNewIsNo}', EBELN, EBELP, 
                   SEQ, MTART, MATNR, 
                   MAGRV, MEINS, GR_QNTY, 
                   GR_QNTY_EA, GR_QNTY_BOX, GR_QNTY_KG,
                   R_EA_PACK_UNIT, R_GR_QNTY, R_GR_QNTY_EA, R_TIME,
                   WERKS, EA_PACK_UNIT, REMARK, 
                   JJO, SBDAY, SYSDATE, '{clsCommon.UserId}'
                FROM WAP_GOCARD
                WHERE IS_NO = '{sOldIsNo}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "XDecar_Copy()", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("차량 정보를 저장 했습니다");

                XDeCar_Search();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_Input_Poorderm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (clsCommon.BarcodeConnYn == "Y")
                _rs232.Close();
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            PrintReport(1);
        }

        /// <summary>
        /// 검근표 출력
        /// iCarType : 0 : 벌크, 1 : 지대, 2 : 기타
        /// </summary>
        /// <param name="iGubun">1 : 전체출력, 2 : IS_NO 출력 </param>
        private void PrintReport(int iGubun)
        {
            DataSet ds = null;
            string isNo = string.Empty;
            int iCarType = 0;

            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                if (iGubun == 1)
                {
                    string sDISPATCHNO = viewDeCar.GetFocusedRowCellValue("IS_NO")?.ToString();

                    if (sDISPATCHNO.IsNullValue() == "")
                    {
                        ShowMessageBox.XtraShowWarning("선택된 상차지시를 배차한 차량이 없습니다.");
                        return;
                    }

                    SQL = $@"
                     SELECT 
                        a.IS_NO, a.EBELN, b.EBELP
                        , a.ESART, a.SRM_PREV_GR, a.PARTNER, a.EKORG
                        , a.SHIP_NAME, a.INVOICE_WEIGHT
                        , b.SEQ, b.MTART, b.MATNR
                        , b.MAGRV, b.MEINS, b.GR_QNTY
                        , b.GR_QNTY_EA, b.GR_QNTY_BOX, b.GR_QNTY_KG
                        , b.WERKS, b.EA_PACK_UNIT, b.REMARK
                        , b.I_TIME, b.I_USER
                    FROM WAP_GOCAR a
                        INNER JOIN WAP_GOCARD b ON b.IS_NO = a.IS_NO AND b.EBELN = a.EBELN
                    WHERE a.IS_NO = '{sDISPATCHNO}'
                    ";

                    ds = Dbconn.conn.ExecutDataset(SQL);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        sIS_NO = ds.Tables[0].Rows[0]["IS_NO"]?.ToString();

                        clsPrintReport.PrintWeighingSheet2(sIS_NO);

                    }
                    else return;
                }
                else
                {
                    sIS_NO = viewDeCar.GetFocusedRowCellValue("IS_NO")?.ToString();

                    clsPrintReport.PrintWeighingSheet2(sIS_NO);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_report_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void viewDeCar_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "ERP_UP_YN")
            {
                string iStatus = Convert.ToString(viewDeCar.GetRowCellValue(e.RowHandle, "ERP_UP_YN"));

                if (iStatus == "Y")
                {
                    e.Appearance.BackColor = Color.Black;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (iStatus == "M")
                {
                    e.Appearance.BackColor = Color.LightBlue;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (iStatus == "G" || iStatus == "X" || iStatus == "O" || iStatus == "L")
                {
                    e.Appearance.BackColor = Color.OrangeRed;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else
                {
                    e.Appearance.BackColor = Color.White;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        /// <summary>
        /// 파렛트 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewPallet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int iWEIGHT = 0;
            int iPD_QTY = 0;

            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            //지시량 자동계산 (배치수 * 배치량 = 지시량) 
            try
            {
                if (e.Column.FieldName == "WEIGHT" || e.Column.FieldName == "PD_QTY")
                {
                    iWEIGHT = Convert.ToInt32(view.GetFocusedRowCellValue("WEIGHT"));

                    iPD_QTY = Convert.ToInt32(view.GetFocusedRowCellValue("PD_QTY"));

                    view.SetFocusedRowCellValue("TWEIGHT", iPD_QTY * iWEIGHT);
                }
            }
            catch
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["TWEIGHT"], 0);     // 지시량
            }
        }

        /// <summary>
        /// 하차 실적
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewOut_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

        }

        private void viewOut_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "R_GR_QNTY" || e.Column.FieldName == "TRAN_QNTY")
            {
                if (e.Value == null || e.Value == DBNull.Value || string.IsNullOrWhiteSpace(e.Value.ToString())) return;

                decimal val = Convert.ToDecimal(e.Value);

                if (val == 0)
                    e.DisplayText = "-";
                else
                {
                    if (cboPlant_Code.EditValue?.ToString() == "P101")
                        e.DisplayText = val.ToString("n0");
                    else
                        e.DisplayText = val.ToString("n3");
                }
            }
        }

        #endregion

        //private void btnSelectCar_Click(object sender, EventArgs e)
        //{
        //    string sIS_NO = string.Empty;
        //    string sEBELN = string.Empty;

        //    using (m_Wap_Decar child = new m_Wap_Decar())
        //    {
        //        child.StartPosition = FormStartPosition.CenterParent;
        //        if (child.ShowDialog() == DialogResult.OK)
        //        {
        //            sIS_NO = child.vIS_NO;
        //            sEBELN = child.vEBELN;
        //        }
        //    }

        //    if (sIS_NO != null && sIS_NO.Length > 0)
        //    {
        //        clsDevexpressGrid.GridViewAddRow(viewMain);

        //        viewMain.SetRowCellValue(viewMain.FocusedRowHandle, viewMain.Columns["IS_NO"], sIS_NO);
        //        viewMain.SetRowCellValue(viewMain.FocusedRowHandle, viewMain.Columns["EBELN"], sEBELN);

        //    }
        //}
    }
}