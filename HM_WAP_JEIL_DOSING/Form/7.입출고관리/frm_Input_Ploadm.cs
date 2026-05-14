using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.DataAccess.DataFederation;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static DevExpress.Data.Filtering.Helpers.SubExprHelper.ThreadHoppingFiltering;

namespace HARIM_FA_DOSING
{
    public partial class frm_Input_Ploadm : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string sDISPATCHNO = String.Empty;
        private string sIS_NO = string.Empty;
        private string[] sDeValid = null;
        private string[] sOutValid = null;
        private string[] sPaValid = null;


        private DateTime DeliveryFrom = DateTime.Today;
        private DateTime DeliveryTo = DateTime.Today.AddDays(1);

        DataSet authDs;

        public frm_Input_Ploadm()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewList, Properties.Settings.Default.FontSize);

            clsDevexpressGrid.ReadGridViewInit(viewMain, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewDetail, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewResult, Properties.Settings.Default.FontSize);

            clsDevexpressGrid.EditGridViewInit(viewDeCar, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewOut, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewPallet, Properties.Settings.Default.FontSize);
        }

        public frm_Input_Ploadm(string DISPATCHNO, string sdtFrom, string sdtTo)
        {
            InitializeComponent();

            sDISPATCHNO = DISPATCHNO;
            DeliveryFrom = DateTime.ParseExact(sdtFrom, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            DeliveryTo = DateTime.ParseExact(sdtTo, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

            layoutControlGroup1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        private void viewMain_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridMain, viewMain);
        }

        private void viewDetail_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridDetail, viewDetail);
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

        private void frm_SCR_MG_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridList, viewList);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridMain, viewMain);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridDetail, viewDetail);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridResult, viewResult);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridDeCar, viewDeCar);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridOut, viewOut);

            authDs = clsSql.GetAuthDataSet(this.Name);

            tabPage.SelectedTabPageIndex = 0;

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            //배송일자
            dtFromDeliveryDate.EditValue = DeliveryFrom;
            dtToDeliveryDate.EditValue = DeliveryTo;

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "전체선택", false, 2, true);

            InitControl();

            XList_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridList));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridMain));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridDetail));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridResult));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridDeCar));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridOut));
        }

        private void InitControl()
        {
            //////////////////////// 상차 지시 Main////////////////////
            // 상차 마감여부
            clsDevexpressGrid.ItemLookUpEditSetup(gridMcboPDEYN, clsCommon.GetPDYn(), "", false, false);

            // 납품유형
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboLFART, clsCommon.GetLFART());

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboVEHICLETON, clsCommon.GetVEHICLETON(), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboVEHICLEGROUP, clsCommon.GetVEHICLEGROUP(), "", false, false);

            // SAP 전송여부
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboTRANS_FLAG, clsCommon.GetTransFlag());

            // 차량그룹
            clsDevexpressGrid.ItemLookUpEditSetup(gridcbovehicleGroupCode, clsCommon.GetCarGroup());

            //////////////////////// 상차 실적 decar////////////////////

            //ERP 전송상태
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboERP_UP_YN, clsCommon.GetTransFlag());

            //차량 입고 타입
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboCAR_TYPE, clsCommon.GetCarInputType(new string[] { "제품출고", "반품" }), "", false, false);

            //차량 그룹
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarCboVEHICLEGROUPCODE, clsCommon.getCarGroupType(), "", false, false);

            // 진행 상태
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboPC_STATUS, clsCommon.GetCarStatus());

            // 차량
            //clsDevexpressGrid.ItemSearchLookUpEditSetup(gridDecarscboINCAR_NO, clsCommon.GetCarMaster());

            Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "운송사" }
                };

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridDecarscboINCAR_NO, clsCommon.GetCarMaster("",  true), "", true, parameterDict);

            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboYN, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "", false, false);

            ///////////////////////////////// out
            // 상차확인여부
            clsDevexpressGrid.ItemLookUpEditSetup(gridOutcboout_PD_YN, clsCommon.GetYn(null, new string[] { "상차완료", "상차중" }), "", false, false);

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridoutscboResourceNo, clsCommon.GetResource());

            // 주문번호
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutscboORDERNO, clsCommon.GetORDERNO(), "", true, null, "CODE", "CODE");

            // 배차번호
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutscboDISPATCHNO, clsCommon.GetDISPATCHNO(), "", true, null, "CODE", "CODE");
        }

        #region 조회 쿼리

        private void XList_Search()
        {
            try
            {
                string fromDate = Convert.ToDateTime(dtFromDeliveryDate.EditValue).ToString("yyyyMMdd");
                string toDate = Convert.ToDateTime(dtToDeliveryDate.EditValue).ToString("yyyyMMdd");

                SQL = $@"
                WITH CUSTOMER AS (
                    SELECT PARTNER AS CODE, NAME_ORG1 AS NAME
                    FROM SAP_DI_CUSTOMER
                    UNION ALL
                    SELECT LOCATION AS CODE, DESCRIPTION AS NAME
                    FROM SAP_DI_LOCATION
                    WHERE USE_FLAG = 'Y'
                ), LFART AS (
                    SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
                    FROM COMM_DIV a
                        JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                        JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE
                                         AND c.COMM_CODE = b.COMM_CODE
                    WHERE c.WK_DIVCODE = '05'
                      AND c.COMM_CODE = '30'
                ), TRANS AS (
                    -- 전송구분
                    SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
                    FROM COMM_DIV a
                        INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                        INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                    WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '11'
                )


                SELECT ploadm.DISPATCHNO                                         -- 배차번호
                    , ploadd.ORDERNO                                                -- 주문번호
                    , ploadd.ORDERLINENO                                        -- 주문라인번호 
                    , ploadm.VEHICLEGROUPNAME                           -- 차량그룹                          
                    , car.VEHICLECODE                                               -- 차량코드
                    , car.VEHICLENO                                                    -- 차량번호                                                             
                    , ploadd.ITEMCODE                                                 -- 주문 품목
                    , pldprd.DESCRIPTION AS ITEMNAME                    -- 주문 품목명
                    , result.PD_YN                                                          -- 상차여부
                    , decar.IS_NO                                                           -- 발급번호
                    , CASE TO_CHAR(decar.CAR_TYPE) 
                            WHEN '003' THEN '반품' ELSE '제품출고' 
                        END AS CAR_TYPE                                             -- 차량입고타입
                    , decar.INCAR_DATE                                              -- 입차일시                                                
                    , decar.OUTCAR_DATE                                            -- 출차일시
                    , decar.IN_WEIGHT                                                   -- 입차무게
                    , decar.OUT_WEIGHT                                                 -- 출차무게
                    , decar.PC_STATUS                                                   -- 진행상태                                                   
                    , result.ZERO_W                                                        -- 공차무게
                    , pldprd.UOM                                                           -- 품목단위
                    , marm.UMREN / marm.UMREZ AS CH_WEIGHT    -- 환산중량
                    , ploadd.PLANQTY AS ORIPLANQTY
                    , CASE WHEN pldprd.UOM = 'EA' THEN (marm.UMREN / marm.UMREZ) * ploadd.PLANQTY ELSE ploadd.PLANQTY END PLANQTY                                                    -- 계획수량
                    , result.QTY                                                               -- 상차수량
                    , result.WEIGHT                                                        -- 상차중량
                    , result.RESOURCE_NO
                    , product.DESCRIPTION
                    , ploadm.DELIVERYDATE
                    , ploadd.TOLOCATIONCODE                                     -- 배송처 코드
                    , tlocast.NAME AS TOLOCATIONNAME                    -- 배송처 명
                    , ploadd.FROMLOCATIONCODE                               -- 출하센터
                    , flocast.NAME AS FROMLOCATIONNAME              -- 출하센터명
                    , lfart.CODE AS ORDERTYPECODE                           -- 오더유형코드
                    , lfart.NAME AS ORDERTYPENAME                          -- 오더유형명
                    , ploadd.SOLDTOCODE                                             -- 거래처코드
                    , cust.NAME AS SOLDTONAME                               -- 거래처명
                    , decar.DEL_FLAG                                                    -- 삭제여부
                    , decar.TEM_TYPE                                                      -- 수동여부
                    , decar.PRINT_YN                                                       -- 프린터여부
                    , decar.I_TIME
                    , decar.I_USER
                    , ordrd.ITEM_TEXT1
                    , etrans.NAME AS ERP_UP_YN
                    , ttrans.NAME AS TMS_UP_YN
                    , ploadm.ERR_MSG
                FROM TMS_INPUT_PLOADM_CON ploadm
                    JOIN TMS_INPUT_PLOADD_CON ploadd ON ploadd.DISPATCHNO = ploadm.DISPATCHNO
                    JOIN SAP_INPUT_SHIP_ORDERD_CON ordrd ON ordrd.VBELN = ploadd.ORDERNO AND ordrd.POSNR = ploadd.ORDERLINENO
                    LEFT JOIN SAP_DI_PRODUCT pldprd ON pldprd.PLANT_CODE = ploadd.PLANTCODE AND pldprd.RESOURCE_NO = ploadd.ITEMCODE
                    LEFT JOIN CUSTOMER cust ON cust.CODE = ploadd.SOLDTOCODE
                    LEFT JOIN CUSTOMER tlocast ON tlocast.CODE = ploadd.TOLOCATIONCODE
                    LEFT JOIN CUSTOMER flocast ON flocast.CODE = ploadd.FROMLOCATIONCODE
                    LEFT JOIN LFART lfart ON lfart.CODE = ploadd.ORDERTYPECODE
                    LEFT JOIN TRANS etrans ON etrans.CODE = ploadm.ERP_UP_YN
                    LEFT JOIN TRANS ttrans ON ttrans.CODE = ploadm.TMS_UP_YN
                    LEFT JOIN SAP_MARM marm ON marm.MATNR = ploadd.ITEMCODE AND marm.MEINH = 'KG'
                    LEFT JOIN TMS_INPUT_CARMASTER_CON car ON car.VEHICLECODE = ploadm.VEHICLECODE
                    LEFT JOIN TMS_OUTPUT_RESULT result ON result.DISPATCHNO = ploadm.DISPATCHNO AND result.ORDERNO = ploadd.ORDERNO AND result.ORDERLINENO = ploadd.ORDERLINENO
                    LEFT JOIN WAP_DECAR decar ON decar.IS_NO = result.IS_NO AND decar.CAR_TYPE IN ('{clsCommon.GetCarInputTypeCode("제품출고")}', '{clsCommon.GetCarInputTypeCode("반품")}')
                    LEFT JOIN ( SELECT IS_NO, SUM(WEIGHT * PD_QTY) AS WEIGHT
                                FROM WAP_OUT_ADD
                                GROUP BY IS_NO
                            ) outa ON outa.IS_NO = result.IS_NO
                    LEFT JOIN SAP_DI_PRODUCT product ON product.PLANT_CODE = result.PLANT_CODE AND product.RESOURCE_NO = result.RESOURCE_NO
                WHERE ploadm.DELIVERYDATE BETWEEN '{fromDate}' AND '{toDate}'
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(decar.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                    AND (car.VEHICLECODE LIKE '%{txtVEHICLENO.EditValue}%' OR car.VEHICLENO LIKE '%{txtVEHICLENO.EditValue}%')
                ORDER BY decar.IS_NO DESC, ploadm.DELIVERYDATE DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridList, viewList, ds.Tables[0], true);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XList_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XMain_Search()
        {
            try
            {
                string fromDate = Convert.ToDateTime(dtFromDeliveryDate.EditValue).ToString("yyyyMMdd");
                string toDate = Convert.ToDateTime(dtToDeliveryDate.EditValue).ToString("yyyyMMdd");

                SQL = $@"
                SELECT DISTINCT
                     a.DISPATCHNO                 -- 배차번호
                   , a.VEHICLECODE                -- 차량코드
                   , a.TMSDIVISIONCODE            -- 디비전코드
                   , a.TMSLOGISTICGROUP           -- 물류운영그룹코드
                   , a.LFART                      -- 납품유형
                   , a.DELIVERYDATE               -- 배송일자
                   , a.CARRIERCODE                -- 운송사코드
                   , a.CARRIERNAME                -- 운송사명
                   , a.VEHICLENO                  -- 차량번호
                   , a.VEHICLETONCODE             -- 차량톤급
                   , a.VEHICLETONNAME             -- 차량톤급명칭
                   , a.VEHICLEGROUPCODE           -- 차량그룹코드
                   , a.VEHICLEGROUPNAME           -- 차량그룹명칭
                   , a.DRIVERNAME                 -- 기사이름
                   , a.DRIVERMOBILE               -- 휴대폰
                   , a.ROTATIONNUMBER             -- 회전수
                   , a.DISPATCHMEMO               -- 배차메모사항
                   , a.REGISTERAT                            
                   , a.REGISTERBY
                   , a.PDE_YN                     -- 상차마감여부
                   , a.ERP_UP_YN
                   , a.ERP_TNUMBER
                   , a.TMS_UP_YN
                   , a.TMS_TNUMBER
                   , a.ERR_MSG
                FROM TMS_INPUT_PLOADM_CON a
                    INNER JOIN TMS_INPUT_PLOADD_CON b ON b.DISPATCHNO = a.DISPATCHNO
                    LEFT JOIN SAP_INPUT_SHIP_ORDERM_CON c ON c.WERKS = '{cboPlant_Code.EditValue}' AND c.VBELN = b.ORDERNO
                    LEFT JOIN SAP_INPUT_SHIP_ORDERD_CON D ON d.VBELN = c.VBELN AND d.POSNR = b.ORDERLINENO
                WHERE ('{sDISPATCHNO}' IS NOT NULL AND a.DISPATCHNO = '{sDISPATCHNO}') OR
                       ('{sDISPATCHNO}' IS NULL AND a.DELIVERYDATE BETWEEN '{fromDate}' AND '{toDate}')
                    AND a.VEHICLENO LIKE '%{txtVEHICLENO.EditValue}%'
                ORDER BY DECODE(a.ERP_UP_YN, 'N', 1, 'M', 2, 'C', 3, 'F', 4, 'U', 5, 'D', 6, '', 7, 8)
                        , DECODE(a.TMS_UP_YN, 'N', 1, 'M', 2, 'C', 3, 'F', 4, 'U', 5, 'D', 6, '', 7, 8)
                        ASC, a.DISPATCHNO DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void Detail_Select()
        {
            try
            {
                SQL = $@"
                -- 거래처와 저장소 리스트
                WITH CUSTOMER AS (
                    SELECT 
                    PARTNER AS CODE, NAME_ORG1 AS NAME
                    FROM SAP_DI_CUSTOMER a
                    UNION ALL
                    SELECT 
                    LOCATION AS CODE, DESCRIPTION AS NAME
                    FROM SAP_DI_LOCATION
                    WHERE USE_FLAG = 'Y'
                ), LFART AS (
                    SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
                    FROM COMM_DIV a
                        INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                        INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                    WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '30'
                )

                SELECT DISTINCT
                     ploadd.DISPATCHNO         -- 배차번호
                   , ploadd.ORDERNO            -- 주문번호
                   , ploadd.ORDERLINENO        -- 주문라인번호
                   , ploadd.TOLOCATIONCODE     -- 도착지 코드
                   , tlocast.NAME AS TOLOCATIONNAME
                   , ploadd.FROMLOCATIONCODE   -- 출발지 코드
                   , flocast.NAME AS FROMLOCATIONNAME
                   , lfart.NAME AS ORDERTYPECODE    -- 주문유형 코드
                   , ploadd.SOLDTOCODE         -- 거래처 코드
                   , cust.NAME AS SOLDTONAME
                   , ploadd.DELIVERYSEQUENCE   -- 납품순번
                   , ploadd.ITEMCODE           -- 품목코드
                   , ploadd.ITEMCODE || ' : ' || prod.DESCRIPTION AS RESOURCE_NO
                   , ploadd.PLANQTY            -- 계획수량
                   , out.PD_YN              -- 상차확인 여부
                   , out.QTY                -- 상차수량
                   , out.WEIGHT             -- 상차중량
                   , ordrd.ITEM_TEXT1   -- 타이콘백 여부
                FROM TMS_INPUT_PLOADM_CON a
                    INNER JOIN TMS_INPUT_PLOADD_CON ploadd ON ploadd.DISPATCHNO = a.DISPATCHNO
-- INNER JOIN SAP_INPUT_SHIP_ORDERM_CON ordrm ON ordrm.TKNUM = TO_CHAR(ploadd.DISPATCHNO) AND ordrm.VBELN = ploadd.ORDERNO  AND ordrm.ZTM_CRE_FLAG = ploadd.ORDERLINENO
                    INNER JOIN SAP_INPUT_SHIP_ORDERD_CON  ordrd ON ordrd.VBELN = ploadd.ORDERNO AND ordrd.POSNR = ploadd.ORDERLINENO
                    LEFT JOIN TMS_OUTPUT_RESULT OUT ON TO_CHAR(out.DISPATCHNO) = TO_CHAR(ploadd.DISPATCHNO) AND out.ORDERNO = ploadd.ORDERNO AND out.ORDERLINENO = ploadd.ORDERLINENO
                    LEFT JOIN CUSTOMER cust ON cust.CODE = ploadd.SOLDTOCODE
                    LEFT JOIN CUSTOMER tlocast ON tlocast.CODE = ploadd.TOLOCATIONCODE
                    LEFT JOIN CUSTOMER flocast ON flocast.CODE = ploadd.FROMLOCATIONCODE
                    LEFT JOIN SAP_DI_PRODUCT prod ON prod.PLANT_CODE = ploadd.PLANTCODE AND prod.RESOURCE_NO = ploadd.ITEMCODE
                    LEFT JOIN LFART lfart ON lfart.CODE = ploadd.ORDERTYPECODE
                WHERE ('{viewMain.GetFocusedRowCellValue("DISPATCHNO")}' IS NULL OR a.DISPATCHNO = '{viewMain.GetFocusedRowCellValue("DISPATCHNO")}')
                ORDER BY ploadd.ORDERNO, ploadd.ORDERLINENO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDetail, viewDetail, ds.Tables[0], true);

                gridCHK.ValueChecked = "Y";
                gridCHK.ValueUnchecked = "N";
                gridCHK.NullStyle = StyleIndeterminate.Unchecked;
                gridCHK.CheckStyle = CheckStyles.Standard;

                //출하센터
                //clsDevexpressGrid.ItemLookUpEditSetup(gridcboCUSTOMER, clsCommon.GetLoCustomer(), "센터명이 없습니다.", false);

                //clsDevexpressGrid.ItemLookUpEditSetup(gridCboORDERTYPECODE, clsCommon.GetLFART(), "", false, false);

                // 프린트 여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn());

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Detail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void Result_Select()
        {
            try
            {
                SQL = $@"
                SELECT 
                     a.IS_NO         -- 발급번호
                    , a.DISPATCHNO   -- 배차번호
                    , a.ORDERNO      -- 주문번호
                    , a.ORDERLINENO  -- 주문라인번호
                    , a.PD_YN        -- 상차확인여부
                    , a.RESOURCE_NO || ' : ' || prod.DESCRIPTION AS RESOURCE_NO  -- 자재코
                    , a.ZERO_W       -- 공차중량
                    , a.QTY          -- 상차 수량
                    , a.WEIGHT       -- 상차 중량
                    , a.CH_YN        -- 확인 일자
                    , a.I_TIME       -- 입력 시간
                    , a.PLANT_CODE   -- 플랜트 코드
                FROM TMS_OUTPUT_RESULT a
                    LEFT JOIN SAP_DI_PRODUCT prod ON prod.PLANT_CODE = a.PLANT_CODE AND prod.RESOURCE_NO = a.RESOURCE_NO
                WHERE ('{viewMain.GetFocusedRowCellValue("DISPATCHNO")}' IS NULL OR a.DISPATCHNO = '{viewMain.GetFocusedRowCellValue("DISPATCHNO")}')
                ORDER BY a.DISPATCHNO, a.ORDERNO, a.ORDERLINENO
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridResult, viewResult, ds.Tables[0], true);

                sIS_NO = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["IS_NO"].ToString() : "";

                gridCHK.ValueChecked = "Y";
                gridCHK.ValueUnchecked = "N";
                gridCHK.NullStyle = StyleIndeterminate.Unchecked;
                gridCHK.CheckStyle = CheckStyles.Standard;

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(null, new string[] { "O", "X" }));


                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Detail_Select", ex);
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
                SELECT DISTINCT
                      a.IS_NO              -- 발급번호
                    , a.CAR_TYPE           -- 차량입고타입
                    , a.INCAR_NO           -- 차량전체번호
                    , a.VEHICLEGROUPCODE   -- 차량그룹코드
                    , a.WEIGHT_KG          -- 계근번호
                    , a.IN_WEIGHT          -- 입차중량
                    , a.OUT_WEIGHT         -- 출차중량
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
                    , c.ERP_UP_YN
                    , c.TMS_UP_YN
                FROM WAP_DECAR a
                    LEFT JOIN TMS_OUTPUT_RESULT b ON b.IS_NO = a.IS_NO
                    LEFT JOIN TMS_INPUT_PLOADM_CON c ON c.DISPATCHNO = b.DISPATCHNO
                WHERE a.INCAR_DATE BETWEEN TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                                       AND TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS') + 1 - (1/86400)
                    AND a.CAR_TYPE IN ('{clsCommon.GetCarInputTypeCode("제품출고")}', 
                                     '{clsCommon.GetCarInputTypeCode("반품")}')
                    --AND ('{txtVEHICLENO.EditValue}' IS NULL OR c.VEHICLENO LIKE '%{txtVEHICLENO.EditValue}%')
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                ORDER BY a.IS_NO DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDeCar, viewDeCar, ds.Tables[0], true);

                sDeValid = new string[] { "IS_NO", "INCAR_DATE" };

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
                SELECT 
                     a.RT_TYPE                -- 실적유형
                   , a.IS_NO                  -- 발급번호
                   , a.DISPATCHNO             -- 배차번호
                   , a.ORDERNO                -- 주문번호
                   , a.ORDERLINENO            -- 라인번호
                   , a.PD_YN                  -- 상차확인여부
                   , a.RESOURCE_NO            -- 품목코드
                   , a.ZERO_W                 -- 공차중량(중간계근 중량)
                   , a.QTY                    -- 상차수량
                   , a.WEIGHT                 -- 상차중량
                   , CASE WHEN NVL(d.WEIGHT_TYPE, '01') = '02' THEN CASE WHEN '{clsCommon.PlantCode}' IN ('P101', 'P102') THEN ROUND(NVL(a.WEIGHT, 0) - NVL(c.WEIGHT, 0), -1) ELSE NVL(a.WEIGHT, 0) - NVL(c.WEIGHT, 0) END ELSE NVL(a.WEIGHT, 0)  END TRAN_QNTY
                   , a.BAG_WEIGHT             -- 톤백 무게
                   , a.BEFORE_WEIGHT          -- 이전 계근 무게
                   , a.BEFORE_WEIGHT_TIME     -- 이전 계근 시간
                   , a.WEIGHT_TIME            -- 현재 계근 시간
                   , a.CH_YN                  -- 확인일자
                   , a.I_TIME                 -- 계근일자
                   , a.PLANT_CODE             -- 플랜트
                   , a.BI_NUM                 -- 봉인번호
                FROM TMS_OUTPUT_RESULT a
                    LEFT JOIN (SELECT IS_NO, SUM(WEIGHT) * SUM(PD_QTY) AS WEIGHT
                        FROM WAP_OUT_ADD
                        GROUP BY IS_NO) c ON c.IS_NO = a.IS_NO
                    LEFT JOIN INGRED D ON D.PLANT_CODE = a.PLANT_CODE AND d.RESOURCE_NO = a.RESOURCE_NO
                    LEFT JOIN TMS_INPUT_PLOADM_CON e ON e.DISPATCHNO = a.DISPATCHNO
                WHERE a.IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridOut, viewOut, ds.Tables[0], true);

                sOutValid = new string[] { "RT_TYPE", "IS_NO", "DISPATCHNO", "ORDERNO", "ORDERLINENO" };

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
                SELECT a.DISPATCHNO, a.PARTNER, a.IS_NO                                -- 01. 발급번호
                   , a.PTMCD                                -- 02. 팔렛트코드
                   , b.WEIGHT                               -- 03. 중량
                   , a.PD_QTY                               -- 04. 수량
                   , (b.WEIGHT * a.PD_QTY) AS TWEIGHT       -- 05. 총중량
                   , a.I_TIME                               -- 06. 입력시간
                FROM WAP_OUT_ADD a
                    LEFT JOIN WAP_PA_MASTER b ON b.PTMCD = a.PTMCD
                WHERE a.IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridPallet, viewPallet, ds.Tables[0], true);

                sPaValid = new string[] { "PTMCD", "WEIGHT" };


                Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "무게" }
                };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridPascboPTMCD, clsCommon.getPallet(), "", true, parameterDict);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xDetail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void Xbrand_Select()
        {
            try
            {
                SQL = $@"
                SELECT a.RT_TYPE
                        , a.IS_NO
                        , a.DISPATCHNO
                        , a.ORDERNO
                        , a.ORDERLINENO
                        , a.RESOURCE_NO
                        , a.RESOURCE_NO || ' : ' || b.DESCRIPTION AS DESCRIPTION
                        , a.WEIGHT
                        , a.I_TIME
                FROM TMS_OUTPUT_RESULT_B a
                    INNER JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridBrand, viewBland, ds.Tables[0], true);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xDetail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void viewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridDetail.DataSource = null;
                gridResult.DataSource = null;


                Detail_Select();

                Result_Select();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        #region 상차지시 메인 버튼 이벤트
        #endregion

        #region 상차지시 상세 버튼 이벤트
        private void btn_reflash1_Click(object sender, EventArgs e)
        {
            Detail_Select();

            Result_Select();
        }

        private void btn_rowAdd1_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewDetail.AddNewRow();
            int newRowHandle = viewDetail.FocusedRowHandle;

            viewDetail.ShowEditor();
        }

        private void btn_rowDel1_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewDetail);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewDetail);
        }

        private void btn_save1_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewDetail);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridDetail.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO TMS_INPUT_PLOADD_CON (
                           DISPATCHNO, ORDERNO, ORDERLINENO, 
                           TOLOCATIONCODE, FROMLOCATIONCODE, ORDERTYPECODE, 
                           SOLDTOCODE, DELIVERYSEQUENCE, ITEMCODE, 
                           PLANQTY, ERP_UP_YN, ERP_TNUMBER, 
                           TMS_UP_YN, TMS_TNUMBER) 
                        VALUES ( 
                         , '{dr["DISPATCHNO"]}'
                         , '{dr["ORDERNO"]}'
                         , '{dr["ORDERLINENO"]}'
                         , '{dr["TOLOCATIONCODE"]}'
                         , '{dr["FROMLOCATIONCODE"]}'
                         , '{dr["ORDERTYPECODE"]}'
                         , '{dr["SOLDTOCODE"]}'
                         , '{dr["DELIVERYSEQUENCE"]}'
                         , '{dr["ITEMCODE"]}'
                         , '{dr["PLANQTY"]}'
                         , '{dr["ERP_UP_YN"]}'
                         , '{dr["ERP_TNUMBER"]}'
                         , '{dr["TMS_UP_YN"]}'
                         , '{dr["TMS_TNUMBER"]}' )
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
                        UPDATE TMS_INPUT_PLOADD_CON
                        SET    DISPATCHNO       = '{dr["DISPATCHNO"]},
                               ORDERNO          = '{dr["ORDERNO"]},
                               ORDERLINENO      = '{dr["ORDERLINENO"]},
                               TOLOCATIONCODE   = '{dr["TOLOCATIONCODE"]},
                               FROMLOCATIONCODE = '{dr["FROMLOCATIONCODE"]},
                               ORDERTYPECODE    = '{dr["ORDERTYPECODE"]},
                               SOLDTOCODE       = '{dr["SOLDTOCODE"]},
                               DELIVERYSEQUENCE = '{dr["DELIVERYSEQUENCE"]},
                               ITEMCODE         = '{dr["ITEMCODE"]},
                               PLANQTY          = '{dr["PLANQTY"]},
                               ERP_UP_YN        = '{dr["ERP_UP_YN"]},
                               ERP_TNUMBER      = '{dr["ERP_TNUMBER"]},
                               TMS_UP_YN        = '{dr["TMS_UP_YN"]},
                               TMS_TNUMBER      = '{dr["TMS_TNUMBER"]}
                        WHERE  DISPATCHNO       = '{dr["DISPATCHNO"]}
                        AND    ORDERNO          = '{dr["ORDERNO"]}
                        AND    ORDERLINENO      = '{dr["ORDERLINENO"]}
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("작업자를 저장 했습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 상차 실적 버튼 이벤트
        private void btn_reflash2_Click(object sender, EventArgs e)
        {
            Detail_Select();

            Result_Select();
        }

        private void btn_rowAdd2_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewResult.AddNewRow();
            int newRowHandle = viewResult.FocusedRowHandle;

            viewResult.ShowEditor();
        }

        private void btn_rowDel2_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewResult);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewResult);
        }

        private void btn_save2_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewMain);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridMain.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO TMS_OUTPUT_RESULT (
                           IS_NO, DISPATCHNO, ORDERNO, 
                           ORDERLINENO, PD_YN, RESOURCE_NO, 
                           ZERO_W, QTY, WEIGHT, ERP_UP_YN, TMS_UP_YN
                           PRINT_YN, CH_YN, I_TIME) 
                        VALUES (
                         , '{dr["IS_NO"]}'
                         , '{dr["DISPATCHNO"]}'
                         , '{dr["ORDERNO"]}'
                         , '{dr["ORDERLINENO"]}'
                         , '{dr["PD_YN"]}'
                         , '{dr["RESOURCE_NO"]}'
                         , '{dr["ZERO_W"]}'
                         , '{dr["QTY"]}'
                         , '{dr["WEIGHT"]}'
                         , 'N'
                         , 'N'
                         , '{dr["PRINT_YN"]}'
                         , SYSDATE )
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
                        UPDATE TMS_OUTPUT_RESULT
                        SET    IS_NO       = '{dr["IS_NO"]}',
                               DISPATCHNO  = '{dr["DISPATCHNO"]}',
                               ORDERNO     = '{dr["ORDERNO"]}',
                               ORDERLINENO = '{dr["ORDERLINENO"]}',
                               PD_YN       = '{dr["PD_YN"]}',
                               RESOURCE_NO = '{dr["RESOURCE_NO"]}',
                               ZERO_W      = '{dr["ZERO_W"]}',
                               QTY         = '{dr["QTY"]}',
                               WEIGHT      = '{dr["WEIGHT"]}',
                               PRINT_YN    = '{dr["PRINT_YN"]}',
                               ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END,
                               TMS_UP_YN = CASE TO_CHAR(TMS_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(TMS_UP_YN) END,
                               I_TIME      = SYSDATE,
                        WHERE  IS_NO       = '{dr["IS_NO"]}'
                            AND DISPATCHNO  = '{dr["DISPATCHNO"]}'
                            AND ORDERNO     = '{dr["ORDERNO"]}'
                            AND ORDERLINENO = '{dr["ORDERLINENO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("작업자를 저장 했습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        private void lblPallet_Click(object sender, EventArgs e)
        {
            // 선택된 행의 데이터 가져오기
            //string sIS_NO = clsDevexpressGrid.GetFocusedRowCellValue(viewMain, "PALLET");

            if (!sIS_NO.Equals(""))
            {
                // 새 폼에 데이터 전달
                var m_WapInAdd = new m_WapInAdd(sIS_NO);
                m_WapInAdd.StartPosition = FormStartPosition.CenterParent;
                m_WapInAdd.ShowDialog();
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
                btn_DeCar_rowAdd_Click(sender, e);
            }

            // 행 삭제
            if (e.KeyCode == Keys.Delete)
            {
                btn_DeCar_rowDel_Click(sender, e);
            }

            // 저장
            if (e.Control && e.KeyCode == Keys.S)
            {
                XDecar_Save();
            }

            // 삭제
            if (e.Control && e.KeyCode == Keys.D)
            {
                XDecar_Delete();
            }
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridMain.Focus();
            viewMain.FocusedRowHandle = 0;
            viewMain.FocusedColumn = viewMain.VisibleColumns[0];
        }

        #region 차량 정보 이벤트
        private void btn_DeCar_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewDeCar.AddNewRow();
            int newRowHandle = viewDeCar.FocusedRowHandle;
            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["I_TIME"], DateTime.Now);
            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["CAR_TYPE"], clsCommon.GetCarInputTypeCode("제품출고"));
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
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            XDecar_Save();
        }

        private void XDecar_Save()
        {
            try
            {
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

                    if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified)
                    {
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
                             , ERP_UP_YN = CASE WHEN ERP_UP_YN = 'Y' THEN 'M' ELSE 'N' END
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

                        SQL = $@"
                        SELECT 1 FROM TMS_OUTPUT_RESULT WHERE  IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'
                        ";

                        DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(ds) > 0)
                        {
                            SQL = $@"
                            UPDATE TMS_INPUT_PLOADM_CON
                            SET    PDE_YN           = 'M'
                                , ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                                , TMS_UP_YN = CASE TO_CHAR(TMS_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(TMS_UP_YN) END
                            WHERE  DISPATCHNO IN (SELECT DISPATCHNO FROM TMS_OUTPUT_RESULT WHERE  IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}')
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
                splashScreenManager.ShowWaitForm();

                SQL = $"UPDATE WAP_DECAR SET DEL_FLAG = 'Y' WHERE IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                SQL = $@"
                UPDATE TMS_INPUT_PLOADM_CON
                SET   ERP_UP_YN = 'X'
                    , TMS_UP_YN = 'X'
                WHERE  DISPATCHNO IN (SELECT DISPATCHNO FROM TMS_OUTPUT_RESULT WHERE  IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}')
                ";

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

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (tabPage.SelectedTabPageIndex == 0)
                XList_Search();
            else if (tabPage.SelectedTabPageIndex == 1)
                XMain_Search();
            else
                XDeCar_Search();

            XOut_Select();
            XPallet_Select();
            Xbrand_Select();
        }

        private void tabPage_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (tabPage.SelectedTabPageIndex == 0)
            {
                layoutControlItem24.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_workdate.Text = "배송일자";
                layoutControlItem26.Enabled = true;

                if (viewList.RowCount == 0)
                {
                    XList_Search();
                }
            }
            else if (tabPage.SelectedTabPageIndex == 1)
            {
                layoutControlItem24.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_workdate.Text = "배송일자";
                layoutControlItem26.Enabled = true;

                if (viewMain.RowCount == 0)
                {
                    XMain_Search();
                }
            }
            else
            {
                layoutControlItem24.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem22.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_workdate.Text = "입차일자";
                layoutControlItem26.Enabled = false;

                if (viewDeCar.RowCount == 0)
                {
                    XDeCar_Search();
                }
            }
        }

        #region 상차 내역 이벤트
        private void btn_Out_rowAdd_Click(object sender, EventArgs e)
        {
            string sBEFORE_WEIGHT_TIME = string.Empty;
            string sZERO_W = string.Empty;
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewOut.AddNewRow();
            int newRowHandle = viewOut.FocusedRowHandle;

            string sIsNo = viewDeCar.GetFocusedRowCellValue("IS_NO")?.ToString();

            SQL = $@"
            SELECT 
                CASE 
                    WHEN EXISTS (
                        SELECT 1 
                        FROM TMS_OUTPUT_RESULT 
                        WHERE IS_NO = '{sIsNo}'
                    )
                    THEN (
                        SELECT TO_CHAR(WEIGHT_TIME, 'YYYY-MM-DD HH24:MI:SS')
                        FROM (
                            SELECT WEIGHT_TIME
                            FROM TMS_OUTPUT_RESULT
                            WHERE IS_NO = '{sIsNo}'
                            ORDER BY WEIGHT_TIME DESC
                        )
                        WHERE ROWNUM = 1
                    )
                    ELSE (
                        SELECT TO_CHAR(INCAR_DATE, 'YYYY-MM-DD HH24:MI:SS')
                        FROM WAP_DECAR
                        WHERE IS_NO = '{sIsNo}'
                        AND ROWNUM = 1
                    )
                END AS BEFORE_WEIGHT_TIME,
    
                CASE 
                    WHEN EXISTS (
                        SELECT 1 
                        FROM TMS_OUTPUT_RESULT 
                        WHERE IS_NO = '{sIsNo}'
                    )
                    THEN (
                        SELECT ZERO_W
                        FROM (
                            SELECT ZERO_W
                            FROM TMS_OUTPUT_RESULT
                            WHERE IS_NO = '{sIsNo}'
                            ORDER BY WEIGHT_TIME DESC
                        )
                        WHERE ROWNUM = 1
                    )
                    ELSE (
                        SELECT IN_WEIGHT
                        FROM WAP_DECAR
                        WHERE IS_NO = '{sIsNo}'
                        AND ROWNUM = 1
                    )
                END AS ZERO_W
            FROM DUAL
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
            {
                sBEFORE_WEIGHT_TIME = Dbconn.conn.getData(ds, "BEFORE_WEIGHT_TIME", 0);
                sZERO_W = Dbconn.conn.getData(ds, "ZERO_W", 0);
            }

            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["IS_NO"], sIsNo);
            viewOut.SetFocusedRowCellValue("RT_TYPE", clsCommon.GetCarInputTypeCode("제품출고"));
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["CH_YN"], DateTime.Now);
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["I_TIME"], DateTime.Now);
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["PLANT_CODE"], clsCommon.PlantCode);
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["BEFORE_WEIGHT_TIME"], sBEFORE_WEIGHT_TIME);
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["BEFORE_WEIGHT"], sZERO_W);

            viewOut.ShowEditor();
        }

        private void btn_Out_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewOut);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewOut);
        }

        private void btn_Out_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            XOut_Save();
        }

        private void XOut_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewOut);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridOut.DataSource;

                DateTime dtFrom = DateTime.Today;
                DateTime dtBEFORE_WEIGHT_TIME = DateTime.Today;

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

                    if (!dr["I_TIME"].ToString().Equals(""))
                    {
                        dtFrom = DateTime.Parse(dr["I_TIME"].ToString());
                    }

                    if (!dr["BEFORE_WEIGHT_TIME"].ToString().Equals(""))
                    {
                        dtBEFORE_WEIGHT_TIME = DateTime.Parse(dr["BEFORE_WEIGHT_TIME"].ToString());
                    }

                    string weightTime = DateTime.TryParse(dr["WEIGHT_TIME"]?.ToString(), out DateTime dt)
                                                                ? dt.ToString("yyyy-MM-dd HH:mm:ss")
                                                                : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO TMS_OUTPUT_RESULT (
                            RT_TYPE, IS_NO, DISPATCHNO, 
                            ORDERNO, ORDERLINENO, PD_YN, 
                            RESOURCE_NO, ZERO_W, QTY, 
                            BEFORE_WEIGHT, BEFORE_WEIGHT_TIME, WEIGHT_TIME,
                            WEIGHT, BAG_WEIGHT, I_TIME, BI_NUM,
                            PLANT_CODE, ERP_UP_YN, TMS_UP_YN
                        ) VALUES (
                            '{dr["RT_TYPE"]}', '{dr["IS_NO"]}', '{dr["DISPATCHNO"]}', 
                            '{dr["ORDERNO"]}', '{dr["ORDERLINENO"]}', '{dr["PD_YN"]}', 
                            '{dr["RESOURCE_NO"]}', '{dr["ZERO_W"]}', '{dr["QTY"]}', 
                            '{dr["BEFORE_WEIGHT"]}', TO_DATE('{dtBEFORE_WEIGHT_TIME.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'), TO_DATE('{weightTime}', 'YYYY-MM-DD HH24:MI:SS'),
                            '{dr["WEIGHT"]}', '{dr["BAG_WEIGHT"]}', TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'), '{dr["BI_NUM"]}',
                            '{clsCommon.GetPlantCode(clsCommon.PlantName)}', 'N', 'N'
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE TMS_INPUT_PLOADM_CON
                        SET PDE_YN           = 'M'
                            , ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                            , TMS_UP_YN = CASE TO_CHAR(TMS_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(TMS_UP_YN) END
                        WHERE  DISPATCHNO       = '{dr["DISPATCHNO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("상차마감이 실패했습니다");
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


                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE TMS_OUTPUT_RESULT
                        SET
                            RT_TYPE             = '{dr["RT_TYPE"]}',
                            IS_NO               = '{dr["IS_NO"]}',
                            DISPATCHNO          = '{dr["DISPATCHNO"]}',
                            ORDERNO             = '{dr["ORDERNO"]}',
                            ORDERLINENO         = '{dr["ORDERLINENO"]}',
                            PD_YN               = '{dr["PD_YN"]}',
                            RESOURCE_NO         = '{dr["RESOURCE_NO"]}',
                            ZERO_W              = '{dr["ZERO_W"]}',
                            QTY                 = '{dr["QTY"]}',
                            WEIGHT              = '{dr["WEIGHT"]}',
                            BAG_WEIGHT          = '{dr["BAG_WEIGHT"]}',
                            BEFORE_WEIGHT       = '{dr["BEFORE_WEIGHT"]}',
                            BEFORE_WEIGHT_TIME  = TO_DATE('{dtBEFORE_WEIGHT_TIME.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS'),
                            WEIGHT_TIME         = TO_DATE('{weightTime}', 'YYYY-MM-DD HH24:MI:SS'),
                            I_TIME              = SYSDATE,
                            BI_NUM              = '{dr["BI_NUM"]}',
                            PLANT_CODE          = '{dr["PLANT_CODE"]}',
                            ERP_UP_YN           = CASE WHEN ERP_UP_YN = 'Y' THEN 'M' ELSE 'N' END,
                            TMS_UP_YN           = CASE WHEN TMS_UP_YN = 'Y' THEN 'M' ELSE 'N' END
                        WHERE
                            RT_TYPE     = '{dr["RT_TYPE"]}'
                            AND IS_NO       = '{dr["IS_NO"]}'
                            AND DISPATCHNO  = '{dr["DISPATCHNO"]}'
                            AND ORDERNO     = '{dr["ORDERNO"]}'
                            AND ORDERLINENO = '{dr["ORDERLINENO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE TMS_INPUT_PLOADM_CON
                        SET    PDE_YN           = 'M'
                            , ERP_UP_YN = CASE WHEN ERP_UP_YN = 'Y' THEN 'M' ELSE 'N' END
                            , TMS_UP_YN = CASE WHEN TMS_UP_YN = 'Y' THEN 'M' ELSE 'N' END
                        WHERE  DISPATCHNO       = '{dr["DISPATCHNO"]}'
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

                ShowMessageBox.XtraShowWarning("상차 정보를 저장 했습니다");

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
                splashScreenManager.ShowWaitForm();

                SQL = $@"
                DELETE FROM TMS_OUTPUT_RESULT
                WHERE  IS_NO = '{viewOut.GetFocusedRowCellValue("IS_NO")}'
                    AND DISPATCHNO = '{viewOut.GetFocusedRowCellValue("DISPATCHNO")}'
                    AND ORDERNO = '{viewOut.GetFocusedRowCellValue("ORDERNO")}'
                    AND ORDERLINENO = '{viewOut.GetFocusedRowCellValue("ORDERLINENO")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowWarning("상차 정보를 삭제 했습니다");

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

        #region 팔렛트 이벤트
        private void btn_Pallet_RowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewPallet.AddNewRow();
            int newRowHandle = viewPallet.FocusedRowHandle;

            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["IS_NO"], viewOut.GetFocusedRowCellValue("IS_NO"));
            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["DISPATCHNO"], viewOut.GetFocusedRowCellValue("DISPATCHNO"));
            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["PARTNER"], viewOut.GetFocusedRowCellValue("ORDERNO"));
            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["I_TIME"], DateTime.Now);

            viewPallet.ShowEditor();
        }

        private void btn_Pallet_RowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPallet);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewPallet);
        }

        private void btn_Pallet_Save_Click(object sender, EventArgs e)
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
                        INSERT INTO WAP_OUT_ADD (
                             IS_NO     -- 01. 입고번호
                            , DISPATCHNO
                            , PARTNER
                            , PTMCD    -- 02. 항목코드
                            , WEIGHT   -- 03. 중량
                            , PD_QTY   -- 04. 수량
                            , I_TIME   -- 05. 입력일시
                        ) VALUES (
                              '{dr["IS_NO"]}'
                            , '{dr["DISPATCHNO"]}'
                            , (SELECT FROMLOCATIONCODE FROM TMS_INPUT_PLOADD_CON WHERE DISPATCHNO = '{dr["DISPATCHNO"]}' AND ORDERNO = '{dr["PARTNER"]}' )
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
                        UPDATE WAP_OUT_ADD
                        SET    I_TIME = SYSDATE
                             , PD_QTY = '{dr["PD_QTY"]}'
                             , PTMCD  = '{dr["PTMCD"]}'
                             , WEIGHT = '{dr["WEIGHT"]}'
                        WHERE  IS_NO  = '{dr["IS_NO"]}'
                            AND PTMCD  = '{dr["PTMCD"]}'
                            AND DISPATCHNO  = '{dr["DISPATCHNO"]}'
                            AND PARTNER  = '{dr["PARTNER"]}'
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
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_Pallet_Delete_Click(object sender, EventArgs e)
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
                splashScreenManager.ShowWaitForm();

                SQL = $"DELETE FROM WAP_IN_ADD WHERE IS_NO = '{viewPallet.GetFocusedRowCellValue("IS_NO")}' AND PTMCD = '{viewPallet.GetFocusedRowCellValue("PTMCD")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowInformation("파렛트가 삭제 되었습니다.");

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
        #endregion

        private void viewDeCar_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridOut.DataSource = null;
                gridPallet.DataSource = null;

                XOut_Select();
                XPallet_Select();
                Xbrand_Select();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "viewDeCar_FocusedRowChanged", ex);
            }
        }

        private void viewOut_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridPallet.DataSource = null;

                XPallet_Select();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "viewOut_FocusedRowChanged", ex);
            }
        }

        private void viewOut_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "DISPATCHNO")
            {
                // 선택된 행의 데이터 가져오기

                string sCarNo = viewDeCar.GetFocusedRowCellDisplayText("INCAR_NO")?.ToString();

                // 새 폼에 데이터 전달
                using (m_TMS_INPUT_PLOADM_CON child = new m_TMS_INPUT_PLOADM_CON(dtFromDeliveryDate.EditValue, dtToDeliveryDate.EditValue, sCarNo))
                {
                    child.StartPosition = FormStartPosition.CenterParent;
                    if (child.ShowDialog() == DialogResult.OK)
                    {
                        viewOut.SetFocusedValue(child.vDISPATCHNO);
                    }
                }
            }

            if (e.Column.FieldName == "ORDERNO")
            {
                // 선택된 행의 데이터 가져오기
                string sDISPATCHNO = clsDevexpressGrid.GetFocusedRowCellValue(viewOut, "DISPATCHNO");

                // 새 폼에 데이터 전달
                using (m_TMS_INPUT_PLOADD_CON child = new m_TMS_INPUT_PLOADD_CON(clsCommon.GetPlantCode(clsCommon.PlantName), sDISPATCHNO))
                {
                    child.StartPosition = FormStartPosition.CenterParent;
                    if (child.ShowDialog() == DialogResult.OK)
                    {
                        viewOut.SetFocusedValue(child.vORDERNO);
                        viewOut.SetFocusedRowCellValue("ORDERLINENO", child.vORDERLINENO);
                        viewOut.SetFocusedRowCellValue("RESOURCE_NO", child.vResourceNo);
                    }
                }
            }
        }

        private void dtFromDeliveryDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dtToDeliveryDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewMain.RowCount == 0)
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
                int[] selectedRows = viewMain.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewMain.GetDataRow(rowHandle);

                    if (!new string[] { "N", "M", "X", "G" }.Contains(dr["ERP_UP_YN"]))
                    {
                        ShowMessageBox.XtraShowWarning("ERP 전송을 할수 있는 상태가 아닙니다.");
                        return;
                    }

                    if (!new string[] { "N", "M", "X", "G" }.Contains(dr["TMS_UP_YN"]))
                    {
                        ShowMessageBox.XtraShowWarning("ERP 전송을 할수 있는 상태가 아닙니다.");
                        return;
                    }

                    if (!new string[] { "Y" }.Contains(dr["PDE_YN"]))
                    {
                        ShowMessageBox.XtraShowWarning("상차 마감이 되지 않아 ERP 전송을 할수 없습니다.");
                        return;
                    }

                    SQL = $@"
                    UPDATE TMS_INPUT_PLOADM_CON
                    SET    ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) WHEN 'N' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                     ELSE TO_CHAR(ERP_UP_YN) END
                        , ERP_ERR_CNT = 0
                        , TMS_UP_YN = CASE TO_CHAR(TMS_UP_YN) WHEN 'N' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                     ELSE TO_CHAR(TMS_UP_YN) END
                        , TMS_ERR_CNT = 0
                    WHERE  DISPATCHNO = '{dr["DISPATCHNO"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("ERP 차량정보 전송 상태 수정이 실패했습니다");
                        return;
                    }
                }

                Dbconn.conn.Commit();

                XMain_Search();
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

        private void btnPDE_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewMain.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("상차 마감 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 정보를 상차 마감 하시겠습니까?"))
            {
                return;
            }

            try
            {
                int[] selectedRows = viewMain.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("상차 마감할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewMain.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT *
                    FROM TMS_OUTPUT_RESULT
                    WHERE DISPATCHNO = '{dr["DISPATCHNO"]}' AND PD_YN != 'Y'
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ShowMessageBox.XtraShowWarning("상차 마감이 완료되지 않았습니다. 확인 후 진행해주세요.");
                        return;
                    }

                    SQL = $@"
                    UPDATE TMS_INPUT_PLOADM_CON
                    SET    PDE_YN           = 'Y'
                        , ERP_UP_YN = CASE TO_CHAR(NVL(ERP_UP_YN, '@')) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN '@' THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                        , TMS_UP_YN = CASE TO_CHAR(NVL(TMS_UP_YN, '@')) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN '@' THEN 'F'
                                                                ELSE TO_CHAR(TMS_UP_YN) END
                    WHERE  DISPATCHNO       = '{dr["DISPATCHNO"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("상차마감이 실패했습니다");
                        return;
                    }
                }

                Dbconn.conn.Commit();

                XMain_Search();
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

            ShowMessageBox.XtraShowInformation("상차마감이 완료 되었습니다.");
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
                    if (!view.IsNewItemRow(rowHandle) && fieldName != "INCAR_DATE")
                        e.Cancel = true;
                    else
                    {
                        //if (viewDeCar.GetFocusedRowCellValue("ERP_ISTATUS").ToString().Trim().Equals("Y") && viewDeCar.GetFocusedRowCellValue("ERP_OSTATUS").ToString().Trim().Equals("Y"))
                        //{
                        //    e.Cancel = true;        // 수정 불가
                        //}
                        //else
                        e.Cancel = false;
                    }
                }
            }
        }

        /// <summary>
        /// 입차 차량 단위 검근표 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridDecarBtnPrint_Click(object sender, EventArgs e)
        {
            PrintReport(2);
        }

        /// <summary>
        /// 상차 지시 단위 검근표 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            string sIS_NO = string.Empty;
            int iCarType = 0;
            string sDISPATCHNO = string.Empty;
            string sORDERNO = string.Empty;
            string sORDERLINENO = string.Empty;

            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                if (iGubun == 1)
                {
                    sDISPATCHNO = viewMain.GetFocusedRowCellValue("DISPATCHNO")?.ToString();

                    if (sDISPATCHNO.IsNullValue() == "")
                    {
                        ShowMessageBox.XtraShowWarning("선택된 상차지시 요청에 상차 이력이 없습니다.");
                        return;
                    }

                    SQL = $@"
                    SELECT 
                         a.IS_NO         -- 발급번호
                        , a.DISPATCHNO   -- 배차번호
                        , a.ORDERNO      -- 주문번호
                        , a.ORDERLINENO  -- 주문라인번호
                        , a.PD_YN        -- 상차확인여부
                        , a.RESOURCE_NO  -- 자재코
                        , a.ZERO_W       -- 공차중량
                        , a.QTY          -- 상차 수량
                        , a.WEIGHT       -- 상차 중량
                        , a.CH_YN        -- 확인 일자
                        , a.I_TIME       -- 입력 시간
                        , a.PLANT_CODE   -- 플랜트 코드
                    FROM 
                        TMS_OUTPUT_RESULT a
                    WHERE ('{sDISPATCHNO}' IS NULL OR a.DISPATCHNO = '{sDISPATCHNO}')
                    ";

                    ds = Dbconn.conn.ExecutDataset(SQL);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        iCarType = viewMain.GetFocusedRowCellValue("VEHICLEGROUPCODE").ToString() == "10" ? 0 : viewMain.GetFocusedRowCellValue("VEHICLEGROUPCODE").ToString() == "20" ? 1 : 2;
                        sIS_NO = ds.Tables[0].Rows[0]["IS_NO"]?.ToString();

                        if (clsCarUtil.returnCarGubun2(sIS_NO) == "벌크")
                        {
                            //PJ01, PJ04 대전,함안 제일사료는 검근표 추가발행
                            if (clsCommon.PlantCode == "PJ01" || clsCommon.PlantCode == "PJ04")
                            {
                                clsPrintExcel.PrintWeighingSheetCopy(sIS_NO);

                                clsUtil.Delay(1500);
                            }

                            clsPrintExcel.PrintWeighingSheet(sIS_NO, true);
                        }
                        else if (clsCarUtil.returnCarGubun2(sIS_NO) == "카고")
                        {
                            //대전 제일사료는 카고차량 검근표 발행
                            if (clsCommon.PlantCode == "PJ01")
                            {
                                clsPrintExcel.PrintWeighingSheet2(sIS_NO);
                                clsUtil.Delay(1000);
                            }

                            //제품이고, 출고명세서 발행
                            if (clsCommon.PlantCode != "PJ01")
                            {
                                clsPrintExcel.PrintChulgoSheet(sIS_NO);
                            }

                            //함안 제일사료는 출고명세서,검근표 합산출력물 추가 발행
                            if (clsCommon.PlantCode == "PJ04")
                            {
                                clsUtil.Delay(1000);
                                clsPrintExcel.PrintChulgoAllSheet(sIS_NO);
                            }
                        }
                    }
                    else return;
                }
                else
                {
                    iCarType = viewDeCar.GetFocusedRowCellValue("VEHICLEGROUPCODE").ToString() == "10" ? 0 : viewDeCar.GetFocusedRowCellValue("VEHICLEGROUPCODE").ToString() == "20" ? 1 : 2;
                    sIS_NO = viewDeCar.GetFocusedRowCellValue("IS_NO")?.ToString();

                    clsPrintExcel.PrintWeighingSheet(sIS_NO, true);
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

        private void viewOut_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int prevRowHandle = e.RowHandle - 1; // 바로 위 행 핸들

            if (prevRowHandle < 0)
                prevRowHandle = 0;

            if (prevRowHandle >= 0)
            {
                foreach (GridColumn col in view.Columns)
                {
                    object val = view.GetRowCellValue(prevRowHandle, col);
                    view.SetRowCellValue(e.RowHandle, col, val);
                }
            }
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

            if (viewMain.RowCount == 0)
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
                int[] selectedRows = viewMain.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewMain.GetDataRow(rowHandle);

                    SQL = $@"
                    UPDATE TMS_INPUT_PLOADM_CON
                    SET    ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) WHEN 'Y' THEN 'D'
                                                ELSE '{dr["ERP_UP_YN"]}'
                                            END
                        , ERP_ERR_CNT = 0
                        , TMS_UP_YN = CASE TO_CHAR(TMS_UP_YN)  WHEN 'Y' THEN 'D'
                                                ELSE '{dr["TMS_UP_YN"]}'
                                            END
                        , TMS_ERR_CNT = 0
                    WHERE  DISPATCHNO = '{dr["DISPATCHNO"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
                        return;
                    }
                }

                Dbconn.conn.Commit();

                XMain_Search();
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

        private void viewMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "ERP_UP_YN")
            {
                string iStatus = Convert.ToString(viewMain.GetRowCellValue(e.RowHandle, "ERP_UP_YN"));

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
                else if (iStatus == "F" || iStatus == "D" || iStatus == "U")
                {
                    e.Appearance.BackColor = Color.LightGreen;
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

            if (e.Column.FieldName == "TMS_UP_YN")
            {
                string iStatus = Convert.ToString(viewMain.GetRowCellValue(e.RowHandle, "TMS_UP_YN"));

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
                else if (iStatus == "F" || iStatus == "D" || iStatus == "U")
                {
                    e.Appearance.BackColor = Color.LightGreen;
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

        private void viewDeCar_ShownEditor(object sender, EventArgs e)
        {
            //        DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            //        if (view.FocusedColumn.FieldName != "INCAR_NO")
            //            return;

            //        SearchLookUpEdit edit = view.ActiveEditor as SearchLookUpEdit;

            //        if (edit == null)
            //            return;

            //        object currentValue = edit.EditValue;

            //        Dictionary<string, string> parameterDict = new Dictionary<string, string>
            //{
            //    { "TYPE", "운송사" }
            //};

            //        clsDevexpressUtil.ItemSearchLookUpEditSetup(
            //            edit,
            //            clsCommon.GetCarMaster("", true),
            //            "",
            //            true,
            //            parameterDict
            //        );

            //        edit.Properties.PopupFormMinSize = new Size(200, 300);

            //        edit.Properties.PopupFilterMode = PopupFilterMode.Contains;
            //        edit.Properties.TextEditStyle = TextEditStyles.Standard;
            //        edit.Properties.ImmediatePopup = true;
            //        edit.Properties.PopupSizeable = true;
            //        edit.Properties.NullText = "";

            //        DevExpress.XtraGrid.Views.Grid.GridView popupView = edit.Properties.View;

            //        popupView.OptionsView.ColumnAutoWidth = false;

            //        popupView.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            //        popupView.OptionsSelection.EnableAppearanceFocusedCell = false;

            //        popupView.OptionsBehavior.AllowIncrementalSearch = true;

            //        popupView.OptionsFind.AlwaysVisible = true;

            //        popupView.OptionsFind.FindMode = FindMode.FindClick;

            //        popupView.OptionsFind.HighlightFindResults = true;

            //        popupView.OptionsFind.FindFilterColumns = "CODE;NAME;TYPE";

            //        popupView.BestFitColumns();

            //        if (popupView.Columns["CODE"] != null)
            //            popupView.Columns["CODE"].Width = 120;

            //        if (popupView.Columns["NAME"] != null)
            //            popupView.Columns["NAME"].Width = 120;

            //        if (popupView.Columns["TYPE"] != null)
            //            popupView.Columns["TYPE"].Width = 120;

            //        // 중요
            //        edit.QueryPopUp -= Edit_QueryPopUp;
            //        edit.QueryPopUp += Edit_QueryPopUp;

            //        edit.EditValue = currentValue;
        }

        private void Edit_QueryPopUp(object sender, CancelEventArgs e)
        {
            SearchLookUpEdit edit = sender as SearchLookUpEdit;

            if (edit == null)
                return;

            DevExpress.XtraGrid.Views.Grid.GridView popupView = edit.Properties.View;

            popupView.ClearColumnsFilter();

            popupView.ActiveFilterString = "";

            popupView.ApplyFindFilter("");

            popupView.FindFilterText = "";

            popupView.FocusedRowHandle = 0;
        }

        private void txtVEHICLENO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tabPage.SelectedTabPageIndex == 0)
                    XList_Search();
                else if (tabPage.SelectedTabPageIndex == 1)
                    XMain_Search();
                else
                    XDeCar_Search();
            }
        }
    }
}