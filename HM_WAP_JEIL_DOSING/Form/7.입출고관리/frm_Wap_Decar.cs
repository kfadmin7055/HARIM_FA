using Core.Class;
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
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Wap_Decar : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string vIS_NO = string.Empty;
        private string sIS_NO = string.Empty;
        private string[] sValid = null;


        private DateTime DeliveryFrom = DateTime.Today.AddDays(-7);
        private DateTime DeliveryTo = DateTime.Today;

        DataSet authDs;

        public frm_Wap_Decar()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewMain, Properties.Settings.Default.FontSize);
        }

        public frm_Wap_Decar(string IS_NO, string sdtFrom, string sdtTo)
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewMain, Properties.Settings.Default.FontSize);

            vIS_NO = IS_NO;
            DeliveryFrom = DateTime.ParseExact(sdtFrom, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            DeliveryTo = DateTime.ParseExact(sdtTo, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

            layoutControlGroup1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        private void XMain_Search()
        {
            try
            {
                DateTime dtFrom = dtFromDeliveryDate.DateTime.Date;
                DateTime dtTo = dtToDeliveryDate.DateTime.Date;

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
                FROM WAP_DECAR
                WHERE ('{vIS_NO}' IS NULL OR IS_NO = '{vIS_NO}') 
                       AND INCAR_DATE BETWEEN TO_DATE('{dtFrom.AddDays(-1).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                                        AND TO_DATE('{dtTo.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                sValid = new string[] { "" };


                viewMain.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboCAR_TYPE, clsCommon.GetCarInputType());

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPC_STATUS, clsCommon.GetCarStatus());

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "", false, false);

                //clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboINCAR_NO, clsCommon.GetCarMaster());

                Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "운송사" }
                };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboINCAR_NO, clsCommon.GetCarMaster(), "", true, parameterDict);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void xDetail_Select()
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
                WHERE IS_NO = '{sIS_NO}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDetail, viewDetail, ds.Tables[0], true);

                sValid = new string[] { "" };


                clsDevexpressGrid.ItemLookUpEditSetup(gridOutcboYN, clsCommon.GetYn(), "", false, false);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xDetail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_SCR_MG_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            //배송일자
            dtFromDeliveryDate.EditValue = DeliveryFrom;
            dtToDeliveryDate.EditValue = DeliveryTo;

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
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
                    xDetail_Select();
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

            viewMain.SetRowCellValue(newRowHandle, viewMain.Columns["IS_NO"], DateTime.Now.ToString("yyyyMMddHHmmss"));
            viewMain.SetRowCellValue(newRowHandle, viewMain.Columns["WEIGHT_KG"], DateTime.Now.ToString("yyyyMMddHHmmss"));

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
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridMain.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    DateTime dtIncar = DateTime.Parse(dr["INCAR_DATE"].ToString());
                    DateTime dtOutcar = DateTime.Parse(dr["OUTCAR_DATE"].ToString());

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
                         , TO_DATE('{dtIncar.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                         , TO_DATE('{dtOutcar.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
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
                             , INCAR_DATE       = TO_DATE('{dtIncar.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                             , OUTCAR_DATE      = TO_DATE('{dtOutcar.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                             , PC_STATUS        = '{dr["PC_STATUS"]}'
                             , ERP_UP_YN        = '{dr["ERP_UP_YN"]}'
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

                ShowMessageBox.XtraShowWarning("작업자를 저장 했습니다");

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

                SQL = $"DELETE FROM TMS_INPUT_PLOADM_CON WHERE XSEQNR = '{viewMain.GetFocusedRowCellValue("XSEQNR")}' AND INTERFACEID = '{viewMain.GetFocusedRowCellValue("INTERFACEID")}' AND DISPATCHNO = '{viewMain.GetFocusedRowCellValue("DISPATCHNO")}'";

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

            viewDetail.SetRowCellValue(viewDetail.FocusedRowHandle, viewDetail.Columns["IS_NO"], viewMain.GetFocusedRowCellValue("IS_NO"));

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
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO TMS_OUTPUT_RESULT ( 
                           RT_TYPE, IS_NO, DISPATCHNO, 
                           ORDERNO, ORDERLINENO, PD_YN, 
                           RESOURCE_NO, ZERO_W, QTY, 
                           WEIGHT, CH_YN, I_TIME, 
                           PLANT_CODE) 
                        VALUES (
                           '1'
                         , '{dr["IS_NO"]}'
                         , '{dr["DISPATCHNO"]}'
                         , '{dr["ORDERNO"]}'
                         , '{dr["ORDERLINENO"]}'
                         , '{dr["PD_YN"]}'
                         , '{dr["RESOURCE_NO"]}'
                         , '{dr["ZERO_W"]}'
                         , '{dr["QTY"]}'
                         , '{dr["WEIGHT"]}'
                         , '{dr["CH_YN"]}'
                         , SYSDATE
                         , '{dr["PLANT_CODE"]}'
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
                        UPDATE TMS_OUTPUT_RESULT
                        SET    RT_TYPE     = '1'
                             , IS_NO       = '{dr["IS_NO"]}'
                             , DISPATCHNO  = '{dr["DISPATCHNO"]}'
                             , ORDERNO     = '{dr["ORDERNO"]}'
                             , ORDERLINENO = '{dr["ORDERLINENO"]}'
                             , PD_YN       = '{dr["PD_YN"]}'
                             , RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                             , ZERO_W      = '{dr["ZERO_W"]}'
                             , QTY         = '{dr["QTY"]}'
                             , WEIGHT      = '{dr["WEIGHT"]}'
                             , CH_YN       = '{dr["CH_YN"]}'
                             , I_TIME      = SYSDATE
                             , PLANT_CODE  = '{dr["PLANT_CODE"]}'
                        WHERE  RT_TYPE     = '{dr["RT_TYPE"]}'
                        AND    IS_NO       = '{dr["IS_NO"]}'
                        AND    DISPATCHNO  = '{dr["DISPATCHNO"]}'
                        AND    ORDERNO     = '{dr["ORDERNO"]}'
                        AND    ORDERLINENO = '{dr["ORDERLINENO"]}'
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

                xDetail_Select();
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
    }
}