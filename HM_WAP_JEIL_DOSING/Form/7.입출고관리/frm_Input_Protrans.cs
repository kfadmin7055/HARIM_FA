using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.Xpo.Helpers;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Input_Protrans : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string sIS_NO = string.Empty;
        private string sTKNUM = String.Empty;
        private string[] sDeValid = null;
        private string[] sOutValid = null;
        private string[] sPaValid = null;


        private DateTime DeliveryFrom = DateTime.Today;
        private DateTime DeliveryTo = DateTime.Today.AddDays(1);

        DataSet authDs;

        public frm_Input_Protrans()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewMain, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewResult, Properties.Settings.Default.FontSize);

            clsDevexpressGrid.EditGridViewInit(viewDeCar, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewOut, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewPallet, Properties.Settings.Default.FontSize);
        }

        public frm_Input_Protrans(string DISPATCHNO, string sdtFrom, string sdtTo)
        {
            InitializeComponent();

            sTKNUM = DISPATCHNO;
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
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridMain, viewMain);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridDetail, viewDetail);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridResult, viewResult);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridDeCar, viewDeCar);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridOut, viewOut);

            authDs = clsSql.GetAuthDataSet(this.Name);

            layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, true, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            //배송일자
            dateFrom.EditValue = DateTime.Today;
            dateTo.EditValue = DateTime.Today.AddDays(1);

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "전체선택", false, 2, true);

            tabPage.SelectedTabPageIndex = 0;

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridList));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridMain));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridDetail));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridResult));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridDeCar));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridOut));
        }

        #region 조회 쿼리

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (tabPage.SelectedTabPageIndex == 0)
                XList_Search();
            else if (tabPage.SelectedTabPageIndex == 1)
                XMain_Search();
            else
                XDeCar_Search();
        }

        private void XList_Search()
        {
            try
            {
                SQL = $@"
                SELECT TO_CHAR(TO_DATE(sm.TRANS_DATE, 'YYYY-MM-DD'), 'YYYY-MM-DD') AS TRANS_DATE        -- 이고 출고일
                    , sm.TKNUM                                      -- 배차번호
                    , sd.VBELN                                      -- 02. 출고지시서
                    , sd.POSNR                                      -- 03. 출고지시 항번
                    , CASE TO_CHAR(decar.CAR_TYPE)
                        WHEN '004' THEN '이고입고' ELSE '이고반품'
                    END AS CAR_TYPE                                 -- 차량입고타입
                    , decar.INCAR_NO                                -- 차량코드
                    , sm.TRAID                                      -- 차량번호
                    , decar.VEHICLEGROUPCODE                        -- 차량그룹코드
                    , gi.P_DESCRIPTION AS WERKS_GI                                   -- 출고 플랜트
                    , gr.P_DESCRIPTION AS WERKS_GR                                   -- 입고 플랜트
                    , sd.MATNR                                      -- 04. 품목코드
                    , product.DESCRIPTION
                    , sd.LFIMG                                      -- 05. 입고예정수량
                    , sd.VRKME                                      -- 06. 단위
                    , result.IS_NO                                  -- 발급번호
                    , result.ORDERLINENO                            -- 주문라인번호
                    , result.PD_YN                                  -- 상차확인여부
                    , result.RESOURCE_NO                            -- 자재코
                    , result.ZERO_W                                 -- 공차중량
                    , result.QTY                                    -- 상차 수량
                    , result.WEIGHT                                 -- 상차 중량
                    , result.I_TIME                                 -- 입력 시간
                    , decar.WEIGHT_KG                               -- 계근번호
                    , decar.IN_WEIGHT                               -- 입차중량
                    , decar.OUT_WEIGHT                              -- 출차중량
                    , decar.INCAR_DATE                              -- 입차일시
                    , decar.OUTCAR_DATE                             -- 출차일시
                    , decar.PC_STATUS                               -- 진행상태
                    , decar.I_TIME                                  -- 입력시간
                    , decar.I_USER                                  -- 입력자
                    , sm.ERP_UP_YN 
                    --, sm.ERR_MSG
                FROM SAP_INPUT_PROTRANSM sm
                    JOIN SAP_INPUT_PROTRANSD sd ON sd.TKNUM = sm.TKNUM
                    LEFT JOIN SAP_DI_PLANT gi ON gi.PLANT_CODE = sm.WERKS_GI
                    LEFT JOIN SAP_DI_PLANT gr ON gr.PLANT_CODE = sm.WERKS_GR
                    LEFT JOIN SAP_DI_PRODUCT product ON product.PLANT_CODE = '{cboPlant_Code.EditValue}' AND product.RESOURCE_NO = sd.MATNR
                    LEFT JOIN TMS_OUTPUT_RESULT result ON result.DISPATCHNO = sd.TKNUM AND result.ORDERNO = sd.VBELN AND result.ORDERLINENO = sd.POSNR
                    LEFT JOIN WAP_DECAR decar ON decar.IS_NO = result.IS_NO AND decar.CAR_TYPE IN ('{clsCommon.GetCarInputTypeCode("이고입고")}', '{clsCommon.GetCarInputTypeCode("이고반품")}')
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR (sm.WERKS_GI = '{cboPlant_Code.EditValue}' OR sm.WERKS_GR = '{cboPlant_Code.EditValue}'))
                    AND TO_CHAR(TO_DATE(sm.TRANS_DATE, 'YYYY-MM-DD'), 'YYYYMMDD') BETWEEN TO_CHAR(TO_DATE('{DateTime.Parse(dateFrom.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD') , 'YYYYMMDD') 
                                    AND TO_CHAR(TO_DATE('{DateTime.Parse(dateTo.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD'), 'YYYYMMDD') 
                ORDER BY sm.TRANS_DATE DESC
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
                SELECT 
                TKNUM, TRAID, WERKS_GI, 
                   WERKS_GR, TO_CHAR(TO_DATE(a.TRANS_DATE, 'YYYY-MM-DD'), 'YYYY-MM-DD') AS TRANS_DATE, ERP_UP_YN, 
                   ERP_TNUMBER, CASE WHEN b.EBELN IS NULL THEN 'N' ELSE 'Y' END TRANSYN
                FROM SAP_INPUT_PROTRANSM a
                    LEFT JOIN WAP_GOCAR b ON b.EBELN = a.TKNUM
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR (WERKS_GI = '{cboPlant_Code.EditValue}' OR WERKS_GR = '{cboPlant_Code.EditValue}'))
                    AND TO_CHAR(TO_DATE(a.TRANS_DATE, 'YYYY-MM-DD'), 'YYYYMMDD') BETWEEN TO_CHAR(TO_DATE('{DateTime.Parse(dateFrom.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD') , 'YYYYMMDD') 
                                    AND TO_CHAR(TO_DATE('{DateTime.Parse(dateTo.EditValue.ToString()).ToString("yyyyMMdd")}', 'YYYYMMDD'), 'YYYYMMDD') 
                ORDER BY DECODE(a.ERP_UP_YN, 'N', 1, 'M', 2, 'C', 3, 'F', 4, 'U', 5, 'D', 6, '', 7)
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                // 플랜트
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboPLANT, clsCommon.GetPlant("", true));

                // 플랜트
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboTransFlag, clsCommon.GetTransFlag());

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
                SELECT 
                     TKNUM   -- 01. 배차번호
                   , VBELN   -- 02. 출고지시서
                   , POSNR   -- 03. 출고지시 항번
                   , MATNR   -- 04. 품목코드
                   , LFIMG   -- 05. 입고예정수량
                   , VRKME   -- 06. 단위
                FROM SAP_INPUT_PROTRANSD
                WHERE TKNUM = '{viewMain.GetFocusedRowCellValue("TKNUM")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDetail, viewDetail, ds.Tables[0], true);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridDcboScboResourceNo, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

                clsDevexpressGrid.ItemLookUpEditSetup(gridDcboVRKME, clsCommon.GetUnit());

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
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
                    , a.RESOURCE_NO  -- 자재코
                    , a.ZERO_W       -- 공차중량
                    , a.QTY          -- 상차 수량
                    , a.WEIGHT       -- 상차 중량
                    , a.CH_YN        -- 확인 일자
                    , a.I_TIME       -- 입력 시간
                    , a.PLANT_CODE   -- 플랜트 코드
                FROM 
                    TMS_OUTPUT_RESULT a
                WHERE ('{viewDetail.GetFocusedRowCellValue("TKNUM")}' IS NULL OR a.DISPATCHNO = '{viewDetail.GetFocusedRowCellValue("TKNUM")}')
                    AND RT_TYPE IN ('{clsCommon.GetCarInputTypeCode("이고입고")}', '{clsCommon.GetCarInputTypeCode("이고반품")}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridResult, viewResult, ds.Tables[0], true);

                sIS_NO = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["IS_NO"].ToString() : "";

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(null, new string[] { "O", "X" }));

                // 상품
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

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
            DateTime dtFrom = dateFrom.DateTime.Date;
            DateTime dtTo = dateTo.DateTime.Date;

            try
            {
                SQL = $@"
                SELECT 
                     IS_NO              -- 발급번호
                   , CAR_TYPE           -- 차량입고타입
                   , INCAR_NO           -- 차량전체번호
                   , VEHICLEGROUPCODE   -- 차량그룹코드
                   , WEIGHT_KG          -- 계근번호
                   , IN_WEIGHT          -- 입차중량
                   , OUT_WEIGHT         -- 출차중량
                   , TR_YN              -- 트레일러유무
                   , TR_WEIGHT          -- 트레일러무게
                   , USER_ID            -- 확인관리자
                   , INCAR_DATE         -- 입차일시
                   , OUTCAR_DATE        -- 출차일시
                   , PC_STATUS          -- 진행상태
                   , ERP_UP_YN          -- ERP 전송상태 
                   , ERP_TNUMBER        -- ERP 전송일련번호
                   , DEL_FLAG           -- 삭제여부
                   , TEM_TYPE           -- 수동여부
                   , PRINT_YN           -- 프린터 여부
                   , I_TIME             -- 입력시간
                   , I_USER             -- 입력자
                   , ERR_MSG
                FROM WAP_DECAR
                WHERE INCAR_DATE BETWEEN TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                                        AND TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS') + 1 - (1/86400)
                    AND CAR_TYPE IN ('{clsCommon.GetCarInputTypeCode("이고입고")}', '{clsCommon.GetCarInputTypeCode("이고반품")}')
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                ORDER BY IS_NO DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDeCar, viewDeCar, ds.Tables[0], true);

                sDeValid = new string[] { "IS_NO", "CAR_TYPE", "INCAR_NO", "IN_WEIGHT" };

                viewDeCar.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

                //ERP 전송상태
                clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboERP_UP_YN, clsCommon.GetTransFlag());

                // 차량입고타입
                clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboCAR_TYPE, clsCommon.GetCarInputType(new string[] { "이고입고", "이고반품" }));

                //차량 그룹
                clsDevexpressGrid.ItemLookUpEditSetup(gridDecarCboVEHICLEGROUPCODE, clsCommon.getCarGroupType(), "", false, false);

                // 진행상태
                clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboPC_STATUS, clsCommon.GetCarStatus());

                // 삭제여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboDEL_FLAG, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "", false, false);

                // 수동여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboTEM_TYPE, clsCommon.GetYn(null, new string[] { "자동", "수동" }), "", false, false);

                // 프린트 여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboPRINT_YN, clsCommon.GetYn(), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboTEM_TYPE, clsCommon.GetYn(), "", false, false);

                Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "운송사" }
                };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridDecarcboINCAR_NO, clsCommon.GetCarMaster(), "", true, parameterDict);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XDeCar_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void XOut_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                     RT_TYPE         -- 실적유형
                   , IS_NO           -- 발급번호
                   , DISPATCHNO      -- 배차번호
                   , ORDERNO         -- 주문번호
                   , ORDERLINENO     -- 라인번호
                   , PD_YN           -- 상차확인여부
                   , RESOURCE_NO     -- 품목코드
                   , ZERO_W          -- 공차중량
                   , QTY             -- 상차수량
                   , WEIGHT          -- 상차중량
                   , CH_YN           -- 확인일자
                   , I_TIME          -- 계근일자
                   , PLANT_CODE      -- 플랜트
                FROM TMS_OUTPUT_RESULT
                WHERE IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridOut, viewOut, ds.Tables[0], true);

                sOutValid = new string[] { "RT_TYPE", "IS_NO", "DISPATCHNO", "ORDERNO", "ORDERLINENO" };

                // 실적유형
                clsDevexpressGrid.ItemLookUpEditSetup(gridOutCboRT_TYPE, clsCommon.GetRTType(), "", false, false);

                // 상차확인여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridOutcboout_PD_YN, clsCommon.GetYn(null, new string[] { "상차완료", "상차중" }), "", false, false);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridoutscboResourceNo, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

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

                Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "무게" }
                };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridPalScboPTMCD, clsCommon.getPallet(), "", true, parameterDict);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xDetail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

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

        #region 입고 예정 정보 상세 이벤트
        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewDetail.AddNewRow();
            int newRowHandle = viewDetail.FocusedRowHandle;

            viewDetail.SetRowCellValue(newRowHandle, viewDetail.Columns["I_TIME"], DateTime.Now);

            viewDetail.SetRowCellValue(newRowHandle, viewDetail.Columns["IS_NO"], DateTime.Now.ToString("yyyyMMddHHmmsss"));
            viewDetail.SetRowCellValue(newRowHandle, viewDetail.Columns["WEIGHT_KG"], DateTime.Now.ToString("yyyyMMddHHmmsss"));

            viewDetail.ShowEditor();
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewDetail);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewDetail);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(viewDetail);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridDetail.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_INPUT_PROTRANSD (
                                   TKNUM                          -- 1
                                 , VBELN                          -- 2
                                 , POSNR                          -- 3
                                 , MATNR                          -- 4
                                 , LFIMG                          -- 5
                                 , VRKME                          -- 6
                                 , I_TIME                         -- 7
                        )
                        VALUES (
                                   '{dr["TKNUM"]}'                -- 1
                                 , '{dr["VBELN"]}'                -- 2
                                 , '{dr["POSNR"]}'                -- 3
                                 , '{dr["MATNR"]}'                -- 4
                                 , '{dr["LFIMG"]}'                -- 5
                                 , '{dr["VRKME"]}'                -- 6
                                 , SYSDATE                        -- 7
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
                        UPDATE SAP_INPUT_PROTRANSD
                        SET
                                   MATNR = '{dr["MATNR"]}'
                                 , LFIMG = '{dr["LFIMG"]}'
                                 , VRKME = '{dr["VRKME"]}'
                        WHERE   TKNUM = '{dr["TKNUM"]}'
                        AND     VBELN = '{dr["VBELN"]}'
                        AND     POSNR = '{dr["POSNR"]}'
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

                ShowMessageBox.XtraShowWarning("입고예정 상세내역을 저장 했습니다");

                XMain_Search();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion

        #region 입차 정보 이벤트
        private void viewDeCar_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
                try
                {
                    gridOut.DataSource = null;
                    gridPallet.DataSource = null;

                    XOut_Search();

                    XPallet_Select();
                }
                catch (Exception ex)
                {
                    clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
                }
        }

        private void btn_DeCar_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewDeCar.AddNewRow();
            int newRowHandle = viewDeCar.FocusedRowHandle;

            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["I_TIME"], DateTime.Now);
            viewDeCar.SetRowCellValue(newRowHandle, viewDeCar.Columns["CAR_TYPE"], clsCommon.GetCarInputTypeCode("이고입고"));
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
                    string rValid = clsCommon.ValdationCheck(sDeValid, dr, viewPallet);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewPallet.FocusedColumn = viewPallet.Columns[rValid]; // 이동할 컬럼명
                        viewPallet.ShowEditor(); // 편집 모드 진입 (선택)
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

                ShowMessageBox.XtraShowWarning("차량정보를 저장 했습니다");

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

                SQL = $"DELETE FROM WAP_DECAR WHERE IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                SQL = $"DELETE FROM TMS_OUTPUT_RESULT WHERE  IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                SQL = $"DELETE FROM WAP_IN_ADD WHERE IS_NO = '{viewDeCar.GetFocusedRowCellValue("IS_NO")}'";

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

        #region 상차 정보 이벤트

        private void btnDispatch_Click(object sender, EventArgs e)
        {
            string sDISPATCHNO = string.Empty;    // 배차번호
            string sORDERNO = string.Empty;       // 주문번호
            string[] sORDERLINENO = null;   // 라인번호
            string[] sRESOURCE_NO = null;   // 품목코드

            string sINCAR_NO = viewDeCar.GetFocusedRowCellDisplayText("INCAR_NO").ToString();
            sIS_NO = viewDeCar.GetFocusedRowCellValue("IS_NO").ToString();
            string sIN_WEIGHT = viewDeCar.GetFocusedRowCellValue("IN_WEIGHT").ToString();

            using (m_Input_Ploadm child = new m_Input_Ploadm(cboPlant_Code.EditValue?.ToString(), sINCAR_NO))
            {
                child.StartPosition = FormStartPosition.CenterParent;

                if (child.ShowDialog() == DialogResult.OK)
                {
                    sDISPATCHNO = child.vDISPATCHNO;
                    sORDERNO = child.vORDERNO;
                    sORDERLINENO = child.vORDERLINENO;
                    sRESOURCE_NO = child.vRESOURCE_NO;
                }

                if (sRESOURCE_NO != null && sRESOURCE_NO.Length > 0)
                {
                    for (int i = 0; i < sRESOURCE_NO.Length; i++)
                    {
                        clsDevexpressGrid.GridViewAddRow(viewOut);

                        viewOut.SetRowCellValue(viewOut.FocusedRowHandle, viewOut.Columns["IS_NO"], sIS_NO);
                        viewOut.SetRowCellValue(viewOut.FocusedRowHandle, viewOut.Columns["DISPATCHNO"], sDISPATCHNO);
                        viewOut.SetRowCellValue(viewOut.FocusedRowHandle, viewOut.Columns["ORDERNO"], sORDERNO);
                        viewOut.SetRowCellValue(viewOut.FocusedRowHandle, viewOut.Columns["ORDERLINENO"], sORDERLINENO[i]);
                        viewOut.SetRowCellValue(viewOut.FocusedRowHandle, viewOut.Columns["RESOURCE_NO"], sRESOURCE_NO[i]);
                        viewOut.SetRowCellValue(viewOut.FocusedRowHandle, viewOut.Columns["PD_YN"], 'Y');
                        viewOut.SetRowCellValue(viewOut.FocusedRowHandle, viewOut.Columns["ZERO_W"], sIN_WEIGHT);
                    }
                }
            }
        }

        private void btn_Out_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewOut.AddNewRow();
            int newRowHandle = viewOut.FocusedRowHandle;

            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["I_TIME"], DateTime.Now);

            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["IS_NO"], clsDevexpressGrid.GetFocusedRowCellValue(viewDeCar, "IS_NO"));
            viewOut.SetFocusedRowCellValue("RT_TYPE", clsDevexpressGrid.GetFocusedRowCellValue(viewDeCar, "CAR_TYPE"));
            viewOut.SetRowCellValue(newRowHandle, viewOut.Columns["ZERO_W"], (Convert.ToDouble(viewDeCar.GetFocusedRowCellValue("IN_WEIGHT") ?? 0)) - Convert.ToDouble(viewDeCar.GetFocusedRowCellValue("OUT_WEIGHT") ?? 0));

            viewOut.ShowEditor();
        }

        private void btn_Out_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewOut);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewOut);
        }

        private void btn_Out_save_Click(object sender, EventArgs e)
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
                    if (dr.RowState == DataRowState.Added)
                    {
                        string rValid = clsCommon.ValdationCheck(sOutValid, dr, viewOut);

                        if (!string.IsNullOrWhiteSpace(rValid))
                        {
                            viewOut.FocusedColumn = viewOut.Columns[rValid]; // 이동할 컬럼명
                            viewOut.ShowEditor(); // 편집 모드 진입 (선택)
                            Dbconn.conn.Rollback();
                            return;
                        }

                        SQL = $@"
                        INSERT INTO TMS_OUTPUT_RESULT (
                           RT_TYPE, IS_NO, DISPATCHNO, ORDERNO, 
                           ORDERLINENO, PD_YN, RESOURCE_NO, 
                           ZERO_W, QTY, WEIGHT, 
                           CH_YN, I_TIME
                           , PLANT_CODE
                           , ERP_UP_YN
                           , TMS_UP_YN
                        ) 
                        VALUES (
                           '{dr["RT_TYPE"]}'
                         , '{dr["IS_NO"]}'
                         , '{dr["DISPATCHNO"]}'
                         , '{dr["ORDERNO"]}'
                         , '{dr["ORDERLINENO"]}'
                         , '{dr["PD_YN"]}'
                         , '{dr["RESOURCE_NO"]}'
                         , '{dr["ZERO_W"]}'
                         , '{dr["QTY"]}'
                         , '{dr["WEIGHT"]}'
                         , 'Y'
                         , SYSDATE
                         , '{clsCommon.GetPlantCode(clsCommon.PlantName)}'
                         , 'N', 'N'
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
                        UPDATE SAP_INPUT_PROTRANSM
                        SET    ERP_UP_YN         = 'N'
                        WHERE  TKNUM       = '{dr["DISPATCHNO"]}'
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
                        SET    IS_NO       = '{dr["IS_NO"]}',
                               DISPATCHNO  = '{dr["DISPATCHNO"]}',
                               ORDERNO     = '{dr["ORDERNO"]}',
                               ORDERLINENO = '{dr["ORDERLINENO"]}',
                               PD_YN       = '{dr["PD_YN"]}',
                               RESOURCE_NO = '{dr["RESOURCE_NO"]}',
                               ZERO_W      = '{dr["ZERO_W"]}',
                               QTY         = '{dr["QTY"]}',
                               WEIGHT      = '{dr["WEIGHT"]}',
                               PRINT_YN    = 'Y',
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

                        SQL = $@"
                        UPDATE SAP_INPUT_PROTRANSM
                        SET    ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                        WHERE  TKNUM       = '{dr["DISPATCHNO"]}'
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

                ShowMessageBox.XtraShowWarning("데이터를 저장 했습니다");

                XOut_Search();
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

                ShowMessageBox.XtraShowWarning("데이터를 삭제 했습니다");

                XOut_Search();
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
        private void btn_Pallet_rowAdd_Click(object sender, EventArgs e)
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

        private void btn_Pallet_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPallet);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewPallet);
        }

        private void btn_Pallet_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

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
                        SET    EBELN  = {dr["EBELN"]}
                             , EBELP  = {dr["EBELP"]}
                             , I_TIME = SYSDATE
                             , IS_NO  = {dr["IS_NO"]}
                             , PD_QTY = {dr["PD_QTY"]}
                             , PTMCD  = {dr["PTMCD"]}
                             , WEIGHT = {dr["WEIGHT"]}
                        WHERE  IS_NO  = {dr["IS_NO"]}
                            AND PTMCD  = {dr["PTMCD"]}
                            AND EBELN  = {dr["EBELN"]}
                            AND EBELP  = {dr["EBELP"]}
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

        /// <summary>
        /// 그리드 단축키 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                //XSave_DeCar_Search();
            }

            // 삭제
            if (e.Control && e.KeyCode == Keys.D)
            {
                XDeCar_Search();
            }
        }

        /// <summary>
        /// 그리드 단축키가 디폴트로 적용될수 있게 폼 로드시 그리드 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_Shown(object sender, EventArgs e)
        {
            gridMain.Focus();
            viewMain.FocusedRowHandle = 0;
            viewMain.FocusedColumn = viewMain.VisibleColumns[0];
        }


        private void TabPage_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (tabPage.SelectedTabPageIndex == 0)
            {
                layoutControlItem19.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_workdate.Text = "이고 출고일";
                layoutControlItem60.Enabled = true;
                XList_Search();
            }
            else if (tabPage.SelectedTabPageIndex == 1)
            {
                layoutControlItem19.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_workdate.Text = "이고 출고일";
                layoutControlItem60.Enabled = true;
                XMain_Search();
            }
            else
            {
                layoutControlItem19.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_workdate.Text = "입차일자";
                layoutControlItem60.Enabled = false;
                XDeCar_Search();
            }
        }

        private void viewOut_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "DISPATCHNO")
            {
                // 선택된 행의 데이터 가져오기

                // 새 폼에 데이터 전달
                using (m_SAP_INPUT_PROTRANSM child = new m_SAP_INPUT_PROTRANSM(cboPlant_Code.EditValue?.ToString()))
                {
                    child.StartPosition = FormStartPosition.CenterParent;
                    if (child.ShowDialog() == DialogResult.OK)
                    {
                        viewOut.SetFocusedValue(child.vTKNUM);
                    }
                }
            }

            if (e.Column.FieldName == "ORDERNO" || e.Column.FieldName == "ORDERLINENO")
            {
                // 선택된 행의 데이터 가져오기
                string sDISPATCHNO = clsDevexpressGrid.GetFocusedRowCellValue(viewOut, "DISPATCHNO");

                // 새 폼에 데이터 전달
                using (m_SAP_INPUT_PROTRANSD child = new m_SAP_INPUT_PROTRANSD(cboPlant_Code.EditValue?.ToString(), sDISPATCHNO))
                {
                    child.StartPosition = FormStartPosition.CenterParent;
                    if (child.ShowDialog() == DialogResult.OK)
                    {
                        viewOut.SetFocusedValue(child.vVBELN);
                        viewOut.SetFocusedRowCellValue("ORDERLINENO", child.vPOSNR);
                        viewOut.SetFocusedRowCellValue("RESOURCE_NO", child.vResourceNo);
                    }
                }
            }
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {

            //XMain_Search();

        }

        private void dateFrom_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }

        private void dateTo_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }

        private void cboTransFlag_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
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

                    dr.ClearErrors();

                    SQL = $@"
                    UPDATE SAP_INPUT_PROTRANSM
                    SET    ERP_UP_YN       = CASE TO_CHAR(ERP_UP_YN)
                                            WHEN 'N' THEN 'F'
                                            WHEN 'M' THEN 'U'
                                            WHEN 'X' THEN 'D'
                                            WHEN 'G' THEN 'F'
                                            WHEN 'L' THEN 'U'
                                            WHEN 'E' THEN 'D'
                                            ELSE '{dr["ERP_UP_YN"]}'
                                        END
                        , ERP_ERR_CNT = 0
                    WHERE  TKNUM = '{dr["TKNUM"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("실적 업데이트가 실패했습니다");
                        return;
                    }

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

        private void gridPalScboPTMCD_EditValueChanged(object sender, EventArgs e)
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

        private void btnPDE_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewMain.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("하차 마감 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 정보를 하차 마감 하시겠습니까?"))
            {
                return;
            }

            try
            {
                int[] selectedRows = viewMain.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("하차 마감할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewMain.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    UPDATE SAP_INPUT_PROTRANSM
                    SET    ERP_UP_YN       = CASE TO_CHAR(ERP_UP_YN)
                                             WHEN 'Y' THEN 'M'
                                            ELSE 'N'
                                        END
                        , ERP_ERR_CNT = 0
                    WHERE  TKNUM = '{dr["TKNUM"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("하차마감이 실패했습니다");
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

            ShowMessageBox.XtraShowInformation("하차마감이 완료 되었습니다.");
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
                    string sIS_NO = viewDeCar.GetFocusedRowCellValue("IS_NO")?.ToString();

                    if (sIS_NO.IsNullValue() == "")
                    {
                        ShowMessageBox.XtraShowWarning("선택된 상차지시를 배차한 차량이 없습니다.");
                        return;
                    }

                    //clsPrintReport.PrintChulgoAllSheet(sDISPATCHNO);
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
                            clsPrintExcel.PrintChulgoAllSheet(sIS_NO);
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
        }
    }
}