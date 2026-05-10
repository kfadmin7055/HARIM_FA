using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.PivotGrid.OLAP;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList.ViewInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_OUTPUT_SMORDER2 : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string vIS_NO = string.Empty;
        private string sIS_NO = string.Empty;
        private string[] sMValid = null;
        private string[] sDValid = null;
        private string[] sPValid = null;

        DataSet authDs;

        #region 작업순선생성
        private string workNumber_maker(string dtVDATU)
        {
            try
            {
                string return_seq = string.Empty;
                string SQL = $@"
                SELECT NVL(MAX(POSNR) + 10, 10) AS SEQ
                FROM SAP_OUTPUT_SMORDER
                WHERE VDATU = '{DateTime.ParseExact(dtVDATU, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")}'
                ";

                using (DataSet Ds = Dbconn.conn.ExecutDataset(SQL))
                {
                    if (Dbconn.conn.getRowCnt(Ds) == 0)
                    {
                        clsLog.logSave("작업순번 생성에러 / SQL : " + SQL, 0);
                        return string.Empty;
                    }

                    return_seq = Dbconn.conn.getData(Ds, "SEQ", 0);
                    Ds.Dispose();

                    return return_seq;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "workNumber_maker", ex);
                return string.Empty;
            }
        }
        #endregion

        public frm_OUTPUT_SMORDER2()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewMain, Properties.Settings.Default.FontSize);
        }

        private void frm_SCR_MG_Load(object sender, EventArgs e)
        {
            viewMain.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridMain, viewMain);

            authDs = clsSql.GetAuthDataSet(this.Name);

            //배송일자
            dtFromDeliveryDate.EditValue = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
            dtToDeliveryDate.EditValue = DateTime.Today.ToString("yyyy-MM-dd");

            XMain_Search();

            InitControl();

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboLocation, clsCommon.GetCustomer());
        }

        private void XMain_Search()
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
                   , b.ERP_UP_YN          -- ERP 전송상태 
                   , a.ERP_TNUMBER        -- ERP 전송일련번호
                   , a.DEL_FLAG           -- 삭제여부
                   , a.TEM_TYPE           -- 수동여부
                   , a.PRINT_YN           -- 프린터 여부
                   , a.I_TIME             -- 입력시간
                   , a.I_USER             -- 입력자
                FROM WAP_DECAR a
                    LEFT JOIN SAP_OUTPUT_SMORDER b ON b.IS_NO = a.IS_NO
                WHERE ('{vIS_NO}' IS NULL OR a.IS_NO = '{vIS_NO}') 
                    AND a.INCAR_DATE BETWEEN TO_DATE('{dtFrom.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                                        AND TO_DATE('{dtTo.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                    AND a.CAR_TYPE = '{clsCommon.GetCarInputTypeCode("홍성물류")}'
                ORDER BY DECODE(b.ERP_UP_YN, '', 1, 'N', 2, 'M', 3, 'C', 4, 'F', 5, 'U', 6, 'D', 7, 8) ASC, a.IS_NO DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                sMValid = new string[] { "" };


                viewMain.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void InitControl()
        {
            // ERP 전송 여부
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboTransFlag, clsCommon.GetTransFlag());

            // 차량타입
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboCAR_TYPE, clsCommon.GetCarInputType(), "", false, false);

            // 차량번호
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboVEHICLEGROUPCODE, clsCommon.getCarGroupType(), "", false, false);

            // 진행상태
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboPC_STATUS, clsCommon.GetCarStatus(), "", false, false);

            //YN
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "", false, false);



            clsDevexpressGrid.ItemLookUpEditSetup(gridcboTEM_TYPE, clsCommon.GetYn(null, new string[] { "자동", "수동" }), "", false, false);

            //clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboINCAR_NO, clsCommon.GetCarMaster());

            Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "운송사" }
                };

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboINCAR_NO, clsCommon.GetCarMaster(), "", true, parameterDict);
        }

        private void xDetail_Select()
        {
            try
            {
                SQL = $@"
                SELECT 
                     VDATU         -- 01. 요청일자
                   , VKORG         -- 02. 영업조직
                   , VTWEG         -- 03. 유통경로
                   , SPART         -- 04. 제품군
                   , KUNNR_SP      -- 05. 판매처
                   , KUNNR_SH      -- 06. 납품처
                   , POSNR         -- 07. 품목번호
                   , AUART         -- 08. 오더유형
                   , PRSDT         -- 09. 가격결정일
                   , AUGRU         -- 10. 오더사유
                   , BSTKD         -- 11. 스마트오더 주문번호
                   , HEADTEXT      -- 12. 헤더특이사항
                   , MATNR         -- 13. 자재코드
                   , KWMENG        -- 14. 오더수량
                   , VRKME         -- 15. 단위
                   , IS_NO         -- 16. 발급번호
                   , ERP_UP_YN     -- 17. ERP 전송 여부
                   , ERP_TNUMBER   -- 18. ERP 전송 일련번호
                FROM SAP_OUTPUT_SMORDER
                WHERE IS_NO = '{sIS_NO}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDetail, viewDetail, ds.Tables[0], true);

                sDValid = new string[] { "VDATU", "VKORG", "VTWEG", "SPART", "KUNNR_SP", "KUNNR_SH", "AUART", "PRSDT", "AUGRU", "BSTKD" };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutscboRESOURCE_NO, clsCommon.GetResource("PJ01"), "", true);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboAUART, clsCommon.GetOrderType(), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboUNIT, clsCommon.GetUnit(), "", false, false);

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
                WHERE a.IS_NO = '{sIS_NO}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridPallet, viewPallet, ds.Tables[0], true);

                sPValid = new string[] { "" };


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
                    gridPallet.DataSource = null;

                    xDetail_Select();
                    XPallet_Select();
                }
                catch (Exception ex)
                {
                    clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
                }
        }

        #region 상차지시 메인 버튼 이벤트
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

            viewMain.SetRowCellValue(newRowHandle, viewMain.Columns["I_TIME"], DateTime.Now);

            viewMain.SetRowCellValue(newRowHandle, viewMain.Columns["IS_NO"], "SM".Merge(DateTime.Now.ToString("yyyyMMddHHmmss")));
            viewMain.SetRowCellValue(newRowHandle, viewMain.Columns["WEIGHT_KG"], DateTime.Now.ToString("yyyyMMddHHmmss"));
            viewMain.SetFocusedRowCellValue("CAR_TYPE", clsCommon.GetCarInputTypeCode("홍성물류"));
            viewMain.SetFocusedRowCellValue("TEM_TYPE", "N");
            viewMain.SetFocusedRowCellValue("DEL_FLAG", "N");
            viewMain.SetFocusedRowCellValue("PRINT_YN", "N");
            viewMain.SetFocusedRowCellValue("ERP_UP_YN", "N");

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
                    string rValid = clsCommon.ValdationCheck(sMValid, dr, viewPallet);

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
                         , '{dr["ERP_UP_YN"]}'
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

                ShowMessageBox.XtraShowWarning("스마트 오더를 저장 했습니다");

                XMain_Search();
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 스마트오더를 삭제하시겠습니까?"))
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

                SQL = $"DELETE FROM WAP_DECAR WHERE IS_NO = '{viewMain.GetFocusedRowCellValue("IS_NO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                SQL = $"DELETE FROM SAP_OUTPUT_SMORDER WHERE IS_NO = '{viewMain.GetFocusedRowCellValue("IS_NO")}'";

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

        #region 상차지시 상세 버튼 이벤트
        private void btn_reflash1_Click(object sender, EventArgs e)
        {
            xDetail_Select();
        }

        private void btn_rowAdd1_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewDetail.AddNewRow();

            viewDetail.SetFocusedRowCellValue("BSTKD", $"SM{viewMain.GetFocusedRowCellValue("IS_NO")}");
            viewDetail.SetFocusedRowCellValue("IS_NO", viewMain.GetFocusedRowCellValue("IS_NO"));

            viewDetail.SetFocusedRowCellValue("HEADTEXT", viewMain.GetFocusedRowCellDisplayText("INCAR_NO"));
            //viewDetail.SetFocusedRowCellValue("VDATU", DateTime.Now);
            //viewDetail.SetFocusedRowCellValue("PRSDT", DateTime.Now);
            //workNumber_maker
            viewDetail.SetFocusedRowCellValue("VKORG", "J000");
            viewDetail.SetFocusedRowCellValue("VTWEG", "10");
            viewDetail.SetFocusedRowCellValue("SPART", "10");
            viewDetail.SetFocusedRowCellValue("AUGRU", "A01");
            viewDetail.SetFocusedRowCellValue("AUART", "ZO01");
            viewDetail.SetFocusedRowCellValue("ERP_UP_YN", "N");

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
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridDetail.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sDValid, dr, viewDetail);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewDetail.FocusedColumn = viewDetail.Columns[rValid]; // 이동할 컬럼명
                        viewDetail.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_OUTPUT_SMORDER (
                                   VDATU                          -- 1
                                 , VKORG                          -- 2
                                 , VTWEG                          -- 3
                                 , SPART                          -- 4
                                 , KUNNR_SP                       -- 5
                                 , KUNNR_SH                       -- 6
                                 , POSNR                          -- 7
                                 , AUART                          -- 8
                                 , PRSDT                          -- 9
                                 , AUGRU                          -- 10
                                 , BSTKD                          -- 11
                                 , HEADTEXT                       -- 12
                                 , MATNR                          -- 13
                                 , KWMENG                         -- 14
                                 , VRKME                          -- 15
                                 , IS_NO                          -- 16
                                 , ERP_UP_YN                      -- 17
                                 , ERP_TNUMBER                    -- 18
                        )
                        VALUES (
                                   '{dr["VDATU"]}'                -- 1
                                 , '{dr["VKORG"]}'                -- 2
                                 , '{dr["VTWEG"]}'                -- 3
                                 , '{dr["SPART"]}'                -- 4
                                 , '{dr["KUNNR_SP"]}'             -- 5
                                 , '{dr["KUNNR_SH"]}'             -- 6
                                 , (SELECT NVL(MAX(POSNR) + 10, 10) FROM SAP_OUTPUT_SMORDER WHERE BSTKD = '{dr["BSTKD"]}')     -- 7
                                 , '{dr["AUART"]}'                -- 8
                                 , '{dr["PRSDT"].ToString()}'     -- 9
                                 , '{dr["AUGRU"]}'                -- 10
                                 , '{dr["BSTKD"]}'                -- 11
                                 , '{dr["HEADTEXT"]}'             -- 12
                                 , '{dr["MATNR"]}'                -- 13
                                 , '{dr["KWMENG"]}'               -- 14
                                 , '{dr["VRKME"]}'                -- 15
                                 , '{dr["IS_NO"]}'                -- 16
                                 , 'N'            -- 17
                                 , '{dr["ERP_TNUMBER"]}'          -- 18
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
                        UPDATE SAP_OUTPUT_SMORDER
                        SET
                                   HEADTEXT    = '{dr["HEADTEXT"]}'             -- 1
                                 , MATNR       = '{dr["MATNR"]}'                -- 2
                                 , KWMENG      = '{dr["KWMENG"]}'               -- 3
                                 , VRKME       = '{dr["VRKME"]}'                -- 4
                                 , AUART       = '{dr["AUART"]}'
                                 , ERP_UP_YN   = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END           -- 6
                        WHERE
                                   VDATU       = '{dr["VDATU"]}'
                        AND        VKORG       = '{dr["VKORG"]}'
                        AND        VTWEG       = '{dr["VTWEG"]}'
                        AND        SPART       = '{dr["SPART"]}'
                        AND        KUNNR_SP    = '{dr["KUNNR_SP"]}'
                        AND        KUNNR_SH    = '{dr["KUNNR_SH"]}'
                        AND        POSNR       = '{dr["POSNR"]}'
                        AND        PRSDT       = '{dr["PRSDT"]}'
                        AND        AUGRU       = '{dr["AUGRU"]}'
                        AND        BSTKD       = '{dr["BSTKD"]}'
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

                ShowMessageBox.XtraShowWarning("스마트오더 상차정보를 저장 했습니다");

                XMain_Search();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete1_Click(object sender, EventArgs e)
        {

        }


        #endregion

        #region 상차 실적 버튼 이벤트
        private void btn_reflash2_Click(object sender, EventArgs e)
        {
            xDetail_Select();
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

        private void btnDispatch_Click(object sender, EventArgs e)
        {
            string sDISPATCHNO = string.Empty;    // 배차번호
            string sORDERNO = string.Empty;       // 주문번호
            string[] sORDERLINENO = null;   // 라인번호
            string[] sRESOURCE_NO = null;   // 품목코드

            string sINCAR_NO = viewMain.GetFocusedRowCellDisplayText("INCAR_NO").ToString();
            sIS_NO = viewMain.GetFocusedRowCellValue("IS_NO").ToString();
            string sIN_WEIGHT = viewMain.GetFocusedRowCellValue("IN_WEIGHT").ToString();

            using (m_Input_Ploadm child = new m_Input_Ploadm("PJ01", sINCAR_NO))
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
                        clsDevexpressGrid.GridViewAddRow(viewDetail);

                        viewDetail.SetRowCellValue(viewDetail.FocusedRowHandle, viewDetail.Columns["IS_NO"], sIS_NO);
                        viewDetail.SetRowCellValue(viewDetail.FocusedRowHandle, viewDetail.Columns["DISPATCHNO"], sDISPATCHNO);
                        viewDetail.SetRowCellValue(viewDetail.FocusedRowHandle, viewDetail.Columns["ORDERNO"], sORDERNO);
                        viewDetail.SetRowCellValue(viewDetail.FocusedRowHandle, viewDetail.Columns["ORDERLINENO"], sORDERLINENO[i]);
                        viewDetail.SetRowCellValue(viewDetail.FocusedRowHandle, viewDetail.Columns["RESOURCE_NO"], sRESOURCE_NO[i]);
                        viewDetail.SetRowCellValue(viewDetail.FocusedRowHandle, viewDetail.Columns["PD_YN"], 'Y');
                        viewDetail.SetRowCellValue(viewDetail.FocusedRowHandle, viewDetail.Columns["ZERO_W"], sIN_WEIGHT);
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

        private void viewDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string temp = string.Empty;
            viewPallet.UpdateCurrentRow();

            if (e.Column.FieldName == "VDATU" && e.Value.ToString().Length > 10)
            {
                if (e.Value.ToString() == "")
                    return;

                temp = DateTime.Parse(e.Value.ToString().Replace("오전", "AM").Replace("오후", "PM")).ToString("yyyy-MM-dd");

                viewDetail.SetFocusedRowCellValue("VDATU", temp);

                viewDetail.SetFocusedRowCellValue("PRSDT", temp);
            }
        }

        #region 팔렛트 이벤트
        private void btn_Pallet_RowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            viewPallet.AddNewRow();
            int newRowHandle = viewPallet.FocusedRowHandle;

            viewPallet.SetRowCellValue(newRowHandle, viewPallet.Columns["IS_NO"], viewMain.GetFocusedRowCellValue("IS_NO"));
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
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridPallet.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sPValid, dr, viewPallet);

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
                            NVL('{dr["EBELN"]}', '@')
                            , NVL('{dr["EBELP"]}', '@')
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

        private void viewDetail_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "VDATU" && e.Value is DateTime dt)
            {
                e.DisplayText = dt.ToString("yyyy-MM-dd");
            }
        }

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
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
                    SELECT 1
                    FROM SAP_OUTPUT_SMORDER
                    WHERE IS_NO = '{dr["IS_NO"]}'
                        AND ERP_UP_YN IN ('N', 'M', 'X', 'G')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE SAP_OUTPUT_SMORDER
                            SET ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) WHEN 'N' THEN 'F'
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

        private void gridDtVDATU_EditValueChanged(object sender, EventArgs e)
        {
            //e.ToString()
            //viewDetail.SetFocusedRowCellValue("POSNR", workNumber_maker());
        }

        private void gridOutscboRESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            DataTable dt = clsCommon.GetResource("PJ01");

            // 선택된 row에서 type 가져오기
            DataRow[] rows = dt.Select($"CODE = '{textEditor.EditValue.ToString()}'");
            if (rows.Length > 0)
            {
                string typeValue = rows[0]["UOM"].ToString();

                // 현재 행의 type 컬럼에 값 세팅
                viewDetail.SetFocusedRowCellValue("VRKME", typeValue);
            }
        }

        private void gridScboLocation_EditValueChanged(object sender, EventArgs e)
        {
            //TextEdit textEditor = (TextEdit)sender;

            //clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutscboRESOURCE_NO, clsCommon.GetResource("PJ01"), "", true);
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            PrintReport();
        }

        /// <summary>
        /// 검근표 출력
        /// iCarType : 0 : 벌크, 1 : 지대, 2 : 기타
        /// </summary>
        /// <param name="iGubun">1 : 전체출력, 2 : IS_NO 출력 </param>
        private void PrintReport()
        {
            DataSet ds = null;
            string isNo = string.Empty;
            int iCarType = 0;

            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string sIsNo = viewMain.GetFocusedRowCellValue("IS_NO")?.ToString();

                if (sIsNo.IsNullValue() == "")
                {
                    ShowMessageBox.XtraShowWarning("선택된 상차지시를 배차한 차량이 없습니다.");
                    return;
                }

                SQL = $@"
                SELECT 
                     VDATU         -- 01. 요청일자
                   , VKORG         -- 02. 영업조직
                   , VTWEG         -- 03. 유통경로
                   , SPART         -- 04. 제품군
                   , KUNNR_SP      -- 05. 판매처
                   , KUNNR_SH      -- 06. 납품처
                   , POSNR         -- 07. 품목번호
                   , AUART         -- 08. 오더유형
                   , PRSDT         -- 09. 가격결정일
                   , AUGRU         -- 10. 오더사유
                   , BSTKD         -- 11. 스마트오더 주문번호
                   , HEADTEXT      -- 12. 헤더특이사항
                   , MATNR         -- 13. 자재코드
                   , KWMENG        -- 14. 오더수량
                   , VRKME         -- 15. 단위
                   , IS_NO         -- 16. 발급번호
                   , ERP_UP_YN     -- 17. ERP 전송 여부
                   , ERP_TNUMBER   -- 18. ERP 전송 일련번호
                FROM SAP_OUTPUT_SMORDER
                WHERE IS_NO = '{sIsNo}'
                ";

                ds = Dbconn.conn.ExecutDataset(SQL);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    iCarType = viewMain.GetFocusedRowCellValue("VEHICLEGROUPCODE").ToString() == "10" ? 0 : viewMain.GetFocusedRowCellValue("VEHICLEGROUPCODE").ToString() == "20" ? 1 : 2;
                }
                else return;

                clsPrintReport.PrinWeighingReport_HS(sIsNo);
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
    }
}
