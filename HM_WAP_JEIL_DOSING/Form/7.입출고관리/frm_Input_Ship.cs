using Core.Class;
using Core.Extension;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTabbedMdi;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Input_Ship : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;

        private string sIS_NO = string.Empty;

        private string deltransFlag = clsCommon.GetTransFlagCode("삭제");

        public frm_Input_Ship()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewMain, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewDetail, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewResult, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridMain, viewMain);
        }

        private void viewDetail_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridDetail, viewDetail);
        }

        private void frm_SCR_MG_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridMain, viewMain);

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridDetail, viewDetail);

            authDs = clsSql.GetAuthDataSet(this.Name);

            //배송일자
            dtFromLFDAT.EditValue = DateTime.Today;
            dtToLFDAT.EditValue = DateTime.Today.AddDays(1);

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            clsDevexpressUtil.ItemLookUpEditSetup(cboZTM_CRE_FLAG, clsCommon.GetResultType(), "", false, 0, true);

            clsDevexpressUtil.ItemLookUpEditSetup(cboDOC_CATEGORY, clsCommon.GetSalesType(), "", false, 0, true);

            XMain_Search();
            InitControl();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridMain));

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridDetail));
        }

        private void InitControl()
        {
            // 전송구분
            //clsDevexpressGrid.ItemLookUpEditSetup(gridcboTRANS_FLAG, clsCommon.GetTransFlag());

            // 실적기준구분
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboZTM_CRE_FLAG, clsCommon.GetResultType());

            //플랜트
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboWERKS, clsCommon.GetPlant("", true));

            //저장소
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboLocation, clsCommon.GetLocation(cboPlant_Code.EditValue?.ToString()));

            // 납품처상세구분
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboKUNNR_RTYP, clsCommon.GetDeliveryType());

            // 운송비구분
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboTMS_COSTF, clsCommon.GetYn());

            // 납품유형
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboLFART, clsCommon.GetLFART());

            // 오더/납품 구분
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboDOC_CATEGORY, clsCommon.GetSalesType());

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()), "", false);

            clsDevexpressGrid.ItemLookUpEditSetup(griddcboYN, clsCommon.GetYn(), "N", false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(null, new string[] { "O", "X" }));

            // 상품
            clsDevexpressGrid.ItemLookUpEditSetup(gridDecarcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));
        }

        #region 조회 쿼리
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                   a.VBELN, a.TRANS_FLAG
                   , a.ZTM_CRE_FLAG, a.BUKRS, a.VKORG
                   , a.WERKS, a.LGORT, a.VSTEL
                   , TO_CHAR(TO_DATE(a.LFDAT, 'YYYYMMDD'), 'YYYY-MM-DD') AS LFDAT
                   , a.LFART, a.LFART_TEXT
                   , a.KUNNR_RTYP, a.PLANT_BP, a.PLANT_BP_NAME1
                   , CASE WHEN a.VBTYP = 'T' THEN '반품' ELSE '정상' END VBTYP
                   , a.KUNNR, a.KUNNR_NAME1
                   , a.KUNAG, a.KUNAG_NAME1, a.HEADER_REMARK
                   , a.SDABW, a.BEZEI, a.TMS_COSTF
                   , a.TKNUM
                   , CASE WHEN LENGTH(a.LDDAT) != 8 THEN NULL WHEN a.LDDAT = '00000000' THEN NULL ELSE TO_CHAR(TO_DATE(a.LDDAT, 'YYYYMMDD'), 'YYYY-MM-DD') END LDDAT
                   , CASE WHEN LENGTH(a.LDUHR) != 6 THEN NULL WHEN a.LDUHR = '000000' THEN NULL ELSE TO_CHAR(TO_DATE(a.LDUHR, 'HH24MISS'), 'HH24:MI:SS') END LDUHR
                   , CASE WHEN LENGTH(a.ERDAT) != 8 THEN NULL WHEN a.ERDAT = '00000000' THEN NULL ELSE TO_CHAR(TO_DATE(a.ERDAT, 'YYYYMMDD'), 'YYYY-MM-DD') END ERDAT
                   , CASE WHEN LENGTH(a.ERZET) != 6 THEN NULL WHEN a.ERZET = '000000' THEN NULL ELSE TO_CHAR(TO_DATE(a.ERZET, 'HH24MISS'), 'HH24:MI:SS') END ERZET
                   , a.ERNAM
                   , a.TON_FLAG, a.VBELN_IM, a.CUST_PICKUP
                   , a.DOC_CATEGORY, a.VGBEL_OLD, a.XDATS
                FROM SAP_INPUT_SHIP_ORDERM_CON a
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.WERKS = '{cboPlant_Code.EditValue}')
                    AND a.LFDAT BETWEEN '{dtFromLFDAT.DateTime.ToString("yyyyMMdd")}' AND '{dtToLFDAT.DateTime.ToString("yyyyMMdd")}'
                    AND a.TRANS_FLAG != '{clsCommon.GetTransFlagCode("작업 삭제")}'
                --        AND ('{cboZTM_CRE_FLAG.EditValue}' IS NULL OR b.ORDERTYPECODE = '{cboZTM_CRE_FLAG.EditValue}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                viewMain.SetFixCol(new string[] {  "TRANS_FLAG"
                                                , "VBELN"
                                                , "ZTM_CRE_FLAG"
                                                , "WERKS" });

                sValid = new string[] { "" };

                //viewMain.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

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
                   b.VBELN, 
                   b.POSNR, b.MATNR, b.LFIMG, b.VRKME, 
                   b.NTGEW, b.VOLUM, b.VGBEL, 
                   b.VGPOS, b.VKBUR, b.VKGRP, 
                   b.VTWEG, b.NETPR, b.NETWR, 
                   b.MWSBP, b.ITEM_TEXT1, b.ITEM_TEXT2, 
                   b.ITEM_TEXT3, b.ITEM_TEXT4, b.ITEM_REMARK
                FROM SAP_INPUT_SHIP_ORDERD_CON b
                WHERE b.VBELN = '{viewMain.GetFocusedRowCellValue("VBELN")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDetail, viewDetail, ds.Tables[0], true);

                sValid = new string[] { "" };

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
                   a.IS_NO, a.DISPATCHNO, a.ORDERNO, 
                   a.ORDERLINENO, a.PD_YN, a.RESOURCE_NO, 
                   a.ZERO_W, a.QTY, a.WEIGHT, 
                   a.CH_YN, a.I_TIME, a.PLANT_CODE
                FROM TMS_OUTPUT_RESULT a
                WHERE DISPATCHNO = '{viewMain.GetFocusedRowCellValue("TKNUM")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridResult, viewResult, ds.Tables[0], true);

                sValid = new string[] { "" };


                sIS_NO = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["IS_NO"].ToString() : "";

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Detail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        //private void XMain_DeCar_Search()
        //{
        //    DateTime dtFrom = dtFromLFDAT.DateTime.Date;
        //    DateTime dtTo = dtToLFDAT.DateTime.Date;

        //    try
        //    {
        //        SQL = $@"
        //        SELECT 
        //             IS_NO              -- 발급번호
        //           , CAR_TYPE           -- 차량입고타입
        //           , INCAR_NO           -- 차량전체번호
        //           , VEHICLEGROUPCODE   -- 차량그룹코드
        //           , WEIGHT_KG          -- 계근번호
        //           , IN_WEIGHT          -- 입차중량
        //           , OUT_WEIGHT         -- 출차중량
        //           , TR_YN              -- 트레일러유무
        //           , TR_WEIGHT          -- 트레일러무게
        //           , USER_ID            -- 확인관리자
        //           , INCAR_DATE         -- 입차일시
        //           , OUTCAR_DATE        -- 출차일시
        //           , PC_STATUS          -- 진행상태
        //           , ERP_UP_YN          -- ERP 전송상태 
        //           , ERP_TNUMBER        -- ERP 전송일련번호
        //           , DEL_FLAG           -- 삭제여부
        //           , TEM_TYPE           -- 수동여부
        //           , PRINT_YN           -- 프린터 여부
        //           , I_TIME             -- 입력시간
        //           , I_USER             -- 입력자
        //        FROM WAP_DECAR
        //        WHERE INCAR_DATE BETWEEN TO_DATE('{dtFrom.AddDays(-1).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
        //                                AND TO_DATE('{dtTo.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
        //            AND CAR_TYPE = '{clsCommon.GetCarInputTypeCode("제품출고, 이고(TMS)")}'
        //        ";

        //        DataSet ds = Dbconn.conn.ExecutDataset(SQL);
        //        clsDevexpressGrid.BindGridControl(gridDeCar, viewDeCar, ds.Tables[0], true);

        //        clsDevexpressGrid.ItemLookUpEditSetup(gridcboCAR_TYPE, clsCommon.GetCarInputType());

        //        clsDevexpressGrid.ItemLookUpEditSetup(gridcboPC_STATUS, clsCommon.GetCarStatus());

        //        clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "", false, false);

        //        clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboINCAR_NO, clsCommon.GetCarMaster());

        //        ds.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsLog.logSave(this, "XMain_DeCar_Search", ex);
        //        ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
        //    }

        //}

        //private void XOut_Select()
        //{
        //    try
        //    {
        //        SQL = $@"
        //        SELECT 
        //             RT_TYPE         -- 실적유형
        //           , IS_NO           -- 발급번호
        //           , DISPATCHNO      -- 배차번호
        //           , ORDERNO         -- 주문번호
        //           , ORDERLINENO     -- 라인번호
        //           , PD_YN           -- 상차확인여부
        //           , RESOURCE_NO     -- 품목코드
        //           , ZERO_W          -- 공차중량
        //           , QTY             -- 상차수량
        //           , WEIGHT          -- 상차중량
        //           , CH_YN           -- 확인일자
        //           , I_TIME          -- 계근일자
        //           , PLANT_CODE      -- 플랜트
        //        FROM TMS_OUTPUT_RESULT
        //        WHERE IS_NO = '{sIS_NO}'
        //        ";

        //        DataSet ds = Dbconn.conn.ExecutDataset(SQL);
        //        clsDevexpressGrid.BindGridControl(gridOut, viewOut, ds.Tables[0], true);

        //        ds.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsLog.logSave(this, "XOut_Select", ex);
        //        ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
        //    }
        //}
        #endregion

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        #region 버튼 이벤트
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewMain.AddNewRow();
            int newRowHandle = viewMain.FocusedRowHandle;

            viewMain.SetRowCellValue(newRowHandle, viewMain.Columns["TRANS_FLAG"], clsCommon.GetTransFlagCode("신규"));
            viewMain.SetRowCellValue(newRowHandle, viewMain.Columns["I_TIME"], DateTime.Now);

            viewMain.ShowEditor();
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewMain);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewMain);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridMain.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewMain);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewMain.FocusedColumn = viewMain.Columns[rValid]; // 이동할 컬럼명
                        viewMain.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_INPUT_SHIP_ORDERM (
                           XSEQNR, VBELN, 
                           TRANS_FLAG, ZTM_CRE_FLAG, BUKRS, 
                           VKORG, WERKS, LGORT, 
                           VSTEL, LFDAT, LFART, 
                           LFART_TEXT, KUNNR_RTYP, PLANT_BP, 
                           PLANT_BP_NAME1, VBTYP, KUNNR, 
                           KUNNR_NAME1, KUNAG, KUNAG_NAME1, 
                           HEADER_REMARK, SDABW, BEZEI, 
                           TMS_COSTF, TKNUM, LDDAT, 
                           LDUHR, ERDAT, ERZET, 
                           ERNAM, TON_FLAG, VBELN_IM, 
                           CUST_PICKUP, DOC_CATEGORY, VGBEL_OLD, 
                           XDATS, SEND_YN) 
                        VALUES ( 
                         , '{dr["XSEQNR"]}', '{dr["VBELN"]}'
                         , '{dr["TRANS_FLAG"]}', '{dr["ZTM_CRE_FLAG"]}', '{dr["BUKRS"]}'
                         , '{dr["VKORG"]}', '{dr["WERKS"]}', '{dr["LGORT"]}'
                         , '{dr["VSTEL"]}', '{dr["LFDAT"]}', '{dr["LFART"]}'
                         , '{dr["LFART_TEXT"]}', '{dr["KUNNR_RTYP"]}', '{dr["PLANT_BP"]}'
                         , '{dr["PLANT_BP_NAME1"]}', '{dr["VBTYP"]}', '{dr["KUNNR"]}'
                         , '{dr["KUNNR_NAME1"]}', '{dr["KUNAG"]}', '{dr["KUNAG_NAME1"]}'
                         , '{dr["HEADER_REMARK"]}', '{dr["SDABW"]}', '{dr["BEZEI"]}'
                         , '{dr["TMS_COSTF"]}', '{dr["TKNUM"]}', '{dr["LDDAT"]}'
                         , '{dr["LDUHR"]}', SYSDATE, '{clsCommon.UserId}'
                         , '{dr["ERNAM"]}', '{dr["TON_FLAG"]}', '{dr["VBELN_IM"]}'
                         , '{dr["CUST_PICKUP"]}', '{dr["DOC_CATEGORY"]}', '{dr["VGBEL_OLD"]}'
                         , '{dr["XDATS"]}', '{dr["SEND_YN"]}' )
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
                        UPDATE SAP_INPUT_SHIP_ORDERM
                        SET    TRANS_FLAG     = '{dr["TRANS_FLAG"]}'
                               ZTM_CRE_FLAG   = '{dr["ZTM_CRE_FLAG"]}'
                               BUKRS          = '{dr["BUKRS"]}'
                               VKORG          = '{dr["VKORG"]}'
                               WERKS          = '{dr["WERKS"]}'
                               LGORT          = '{dr["LGORT"]}'
                               VSTEL          = '{dr["VSTEL"]}'
                               LFDAT          = '{dr["LFDAT"]}'
                               LFART          = '{dr["LFART"]}'
                               LFART_TEXT     = '{dr["LFART_TEXT"]}'
                               KUNNR_RTYP     = '{dr["KUNNR_RTYP"]}'
                               PLANT_BP       = '{dr["PLANT_BP"]}'
                               PLANT_BP_NAME1 = '{dr["PLANT_BP_NAME1"]}'
                               VBTYP          = '{dr["VBTYP"]}'
                               KUNNR          = '{dr["KUNNR"]}'
                               KUNNR_NAME1    = '{dr["KUNNR_NAME1"]}'
                               KUNAG          = '{dr["KUNAG"]}'
                               KUNAG_NAME1    = '{dr["KUNAG_NAME1"]}'
                               HEADER_REMARK  = '{dr["HEADER_REMARK"]}'
                               SDABW          = '{dr["SDABW"]}'
                               BEZEI          = '{dr["BEZEI"]}'
                               TMS_COSTF      = '{dr["TMS_COSTF"]}'
                               TKNUM          = '{dr["TKNUM"]}'
                               LDDAT          = '{dr["LDDAT"]}'
                               LDUHR          = '{dr["LDUHR"]}'
                               ERDAT          = SYSDATE
                               ERZET          = '{clsCommon.UserId}'
                               ERNAM          = '{dr["ERNAM"]}'
                               TON_FLAG       = '{dr["TON_FLAG"]}'
                               VBELN_IM       = '{dr["VBELN_IM"]}'
                               CUST_PICKUP    = '{dr["CUST_PICKUP"]}'
                               DOC_CATEGORY   = '{dr["DOC_CATEGORY"]}'
                               VGBEL_OLD      = '{dr["VGBEL_OLD"]}'
                               XDATS          = '{dr["XDATS"]}'
                               SEND_YN        = '{dr["SEND_YN"]}'
                        WHERE  XSEQNR         = '{dr["XSEQNR"]}'
                               VBELN          = '{dr["VBELN"]}'
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

                ShowMessageBox.XtraShowWarning("작업자를 저장 했습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

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

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 차량정보를 삭제하시겠습니까?"))
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

                SQL = $@"UPDATE SAP_INPUT_SHIP_ORDERM_CON
                SET    TRANS_FLAG     = :'{clsCommon.GetTransFlagCode("삭제")}',
                       ERDAT          = :SYSDATE,
                       ERZET          = :'{clsCommon.UserId}'
                WHERE  VBELN          = '{clsDevexpressGrid.GetFocusedRowCellValue(viewMain, "VBELN")}'
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

        private void viewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
                try
                {
                    gridDetail.DataSource = null;

                    Detail_Select();
                    Result_Select();
                }
                catch (Exception ex)
                {
                    clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
                }
        }

        private void viewMain_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "TKNUM")
            {
                // 선택된 행의 데이터 가져오기
                string sDISPATCHNO = clsDevexpressGrid.GetFocusedRowCellValue(viewMain, "TKNUM");

                string sdtFrom = dtFromLFDAT.DateTime.ToString("yyyyMMdd");
                string sdtTo = dtToLFDAT.DateTime.ToString("yyyyMMdd");

                if (!sDISPATCHNO.Equals(""))
                {
                    // 새 폼에 데이터 전달
                    var frm_Input_Ploadm = new frm_Input_Ploadm(sDISPATCHNO, sdtFrom, sdtTo);
                    frm_Input_Ploadm.StartPosition = FormStartPosition.CenterParent;
                    frm_Input_Ploadm.ShowDialog();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewMain_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            // 데이터 수정시 전송구분을 수정으로 변경
            viewMain.SetFocusedRowCellValue("TRANS_FLAG", clsCommon.GetTransFlagCode("수정"));
        }

        private void viewMain_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(viewMain.GetFocusedRowCellValue("TRANS_FLAG").ToString().Trim()) 
                        && viewMain.GetFocusedRowCellValue("TRANS_FLAG").ToString().Trim().Equals("D"))
                {
                    if (viewMain.FocusedRowHandle == viewMain.FocusedRowHandle)
                    {
                        e.Cancel = true; // 편집 막기
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
            }
        }

        private void viewMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            // 현재 행의 Status 값을 가져옴
            string status = viewMain.GetRowCellValue(e.RowHandle, "TRANS_FLAG")?.ToString();

            if (status == deltransFlag)
            {
                // 빨간색 + 취소선 스타일 적용
                e.Appearance.ForeColor = Color.Red;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Strikeout);
            }

            if (e.Column.Equals("TKNUM"))
                 e.Appearance.ForeColor = Color.Blue;
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

        private void dtFromLFDAT_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }

        private void dtToLFDAT_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }

        private void cboZTM_CRE_FLAG_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }

        private void cboDOC_CATEGORY_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            if (cboPlant_Code.Properties.GetDisplayText(cboPlant_Code.EditValue).ToString().Contains("제일사료"))
            {
                dNETPR.Visible = false;
                dNETWR.Visible = false;
                dMWSBP.Visible = false;
            }
            else
            {
                dNETPR.Visible = true;
                dNETWR.Visible = true;
                dMWSBP.Visible = true;
            }
        }
    }
}