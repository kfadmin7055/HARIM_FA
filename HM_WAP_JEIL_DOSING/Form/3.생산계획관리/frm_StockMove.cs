using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.Schedule;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_StockMove : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;
        string resource_Type = "";
        string toResource_Type = "";
        string resource_Uom = "";
        string toResource_Uom = "";

        public frm_StockMove()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewExec, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridExec, viewExec);
        }

        private void frm_StockMove_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridExec, viewExec);

            viewExec.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            authDs = clsSql.GetAuthDataSet(this.Name);

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dateEdit_workStartDate.EditValue = DateTime.Today;
            dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

            // ERP 진행여부
            clsDevexpressUtil.ItemLookUpEditSetup(cboERPUpLoad, clsCommon.GetTransFlag(), "", false, 0, true);

            clsDevexpressUtil.ItemSearchLookUpEditSetup(scboLocation, clsCommon.GetLocation(cboPlant_Code.EditValue?.ToString()));

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridExec));
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT a.WERKS
                     , a.INTERFACEID
                     , a.SEQ
                     , a.BWART
                     , a.PROC_TYPE
                     , a.XSEQNR_C
                     , a.MATNR
                     , a.MATNR || ' : ' || b.DESCRIPTION AS MATNR_NAME
                     , NVL(D.LABST, 0) AS LABST
                     , a.UMMAT
                     , a.UMMAT || ' : ' || c.DESCRIPTION AS UMMAT_NAME
                     , a.BLDAT
                     , a.BUDAT
                     , a.LGORT
                     , a.UMLGO
                     , a.MENGE
                     , a.CH_WEIGHT
                     , b.UOM AS UNIT
                     , a.MEINS
                     , a.KOSTL
                     , a.LOTNO
                     , a.U_LEASON
                     , a.FLD01
                     , a.FLD02
                     , a.FLD03
                     , a.FLD04
                     , a.FLD05
                     , a.ERP_UP_YN
                     , a.ERP_TNUMBER
                FROM SAP_P_EXEC a
                     INNER JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.WERKS AND b.RESOURCE_NO = a.MATNR
                     INNER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.WERKS AND c.RESOURCE_NO = a.UMMAT
                     LEFT JOIN SAP_STOCK_MASTER D ON d.WERKS = a.WERKS AND d.MATNR = a.MATNR AND D.LGORT = a.LGORT
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.WERKS = '{cboPlant_Code.EditValue}')
                    AND a.BLDAT BETWEEN '{dateEdit_workStartDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEndDate.DateTime.ToString("yyyyMMdd")}'
                    AND (('{scboLocation.EditValue}' IS NULL OR a.LGORT = '{scboLocation.EditValue}') 
                        OR ('{scboLocation.EditValue}' IS NULL OR a.UMLGO = '{scboLocation.EditValue}'))
                ORDER BY a.BLDAT
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridExec, viewExec, ds.Tables[0], false);

                //viewExec.SetFixCol(new string[] {  "ERP_UP_YN"
                //                                , "PROC_TYPE"
                //                                , "XSEQNR_C"
                //                                , "MATNR_NAME"
                //                                , "UMMAT_NAME"});

                sValid = new string[] { "WERKS", "PROC_TYPE", "MATNR", "UMMAT", "LGORT", "UMLGO", "MENGE" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = viewExec.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                // ERP 전송상태
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboTransFlag, clsCommon.GetTransFlag(), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboXSEQNR_C, clsCommon.GetYn(), "", false);

                // 품목전환
                //clsDevexpressGrid.ItemLookUpEditSetup(gridcboBWART, clsCommon.GetMovType(), "", false);

                // 이동유형
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboPROC_TYPE, clsCommon.GetMovType(), "", false);

                // 저장위치
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboLocation, clsCommon.GetLocation(cboPlant_Code.EditValue?.ToString()), "창고 위치를 선택 해주세요.");

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboXSEQNR_C, clsCommon.GetYn(null, new string[] { "취소", "정상" }), "", false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboUnit, clsCommon.GetUnit(), "", false, false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                clsDevexpressGrid.GridViewLastAddRowDelete(viewExec);
                return;
            }

            clsDevexpressGrid.GridViewAddRow(viewExec);
            viewExec.SetFocusedRowCellValue("WERKS", cboPlant_Code.EditValue);
            viewExec.SetFocusedRowCellValue("SEQ", clsCommon.GetMaxSeq("SAP_P_EXEC", "SEQ", new Dictionary<string, string> { { "WERKS", $"{cboPlant_Code.EditValue}" } }));
            viewExec.SetFocusedRowCellValue("XSEQNR_C", "N");
            viewExec.SetRowCellValue(viewExec.FocusedRowHandle, viewExec.Columns["INTERFACE_STATUS"], 0);

            SQL = $@"
            UPDATE SAP_STOCK_REQ
            SET    REQ    = 'Y',
                    I_TIME = SYSDATE
            WHERE WERKS = '{clsCommon.PlantCode}'
            ";

            if (Dbconn.conn.SQLrun(SQL) < 0)
            {
                clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                ShowMessageBox.XtraShowWarning("재고 다운로드 요청에 실패했습니다");
                return;
            }
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewExec);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewExec);
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (viewExec.GetFocusedRowCellValue("ERP_UP_YN").ToString().Trim() == "Y" || viewExec.GetFocusedRowCellValue("ERP_UP_YN").ToString().Trim() == "C")
                {
                    switch (viewExec.FocusedColumn.FieldName)
                    {
                        case "U_LEASON":
                            e.Cancel = false;
                            break;
                        default:
                            e.Cancel = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("자재이동 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewExec);
                DataTable DT = (DataTable)gridExec.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                splashScreenManager.ShowWaitForm();

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewExec);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewExec.FocusedColumn = viewExec.Columns[rValid]; // 이동할 컬럼명
                        viewExec.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(dr["LGORT"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("FROM 저장위치를 선택하여 주세요");
                        dr.SetColumnError("LGORT", "FROM 저장위치를 선택하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["UMLGO"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("TO 저장위치를 선택하여 주세요");
                        dr.SetColumnError("UMLGO", "TO 저장위치를 선택하여 주세요");
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {

                        SQL = $@"
                        INSERT INTO SAP_P_EXEC (
                           WERKS, INTERFACEID, SEQ, 
                           BWART, PROC_TYPE, XSEQNR_C, 
                           MATNR, UMMAT, BLDAT, 
                           BUDAT, LGORT, UMLGO, 
                           MENGE, CH_WEIGHT, MEINS, KOSTL, 
                           LOTNO, U_LEASON, FLD01, 
                           FLD02, FLD03, FLD04, 
                           FLD05, ERP_UP_YN, ERP_TNUMBER) 
                        VALUES ( 
                           '{dr["WERKS"]}', '{dr["INTERFACEID"]}', (SELECT NVL(MAX(SEQ) + 1, 1) FROM SAP_P_EXEC WHERE WERKS = '{dr["WERKS"]}')
                         , '{dr["BWART"]}', '{dr["PROC_TYPE"]}', '{dr["XSEQNR_C"]}'
                         , '{dr["MATNR"]}', '{dr["UMMAT"]}', TO_CHAR(TO_DATE('{dr["BLDAT"]}', 'YYYY-MM-DD AM HH12:MI:SS'), 'YYYYMMDD')
                         , TO_CHAR(TO_DATE('{dr["BUDAT"]}', 'YYYY-MM-DD AM HH12:MI:SS'), 'YYYYMMDD'), '{dr["LGORT"]}', '{dr["UMLGO"]}'
                         , '{dr["MENGE"]}', '{dr["CH_WEIGHT"]}', '{dr["MEINS"]}', '{dr["KOSTL"]}'
                         , '{dr["LOTNO"]}', '{dr["U_LEASON"]}', '{dr["FLD01"]}'
                         , '{dr["FLD02"]}', '{dr["FLD03"]}', '{dr["FLD04"]}'
                         , '{dr["FLD05"]}', 'N', '{dr["ERP_TNUMBER"]}')
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(SAP_P_EXEC)데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {

                        SQL = $@"
                        UPDATE SAP_P_EXEC
                        SET      INTERFACEID = '{dr["INTERFACEID"]}'
                               , BWART       = '{dr["BWART"]}'
                               , PROC_TYPE   = '{dr["PROC_TYPE"]}'
                               , XSEQNR_C    = '{dr["XSEQNR_C"]}'
                               , MATNR       = '{dr["MATNR"]}'
                               , UMMAT       = '{dr["UMMAT"]}'
                               , BLDAT       = TO_CHAR(TO_DATE('{dr["BLDAT"]}', 'YYYY-MM-DD AM HH12:MI:SS'), 'YYYYMMDD')
                               , BUDAT       = TO_CHAR(TO_DATE('{dr["BUDAT"]}', 'YYYY-MM-DD AM HH12:MI:SS'), 'YYYYMMDD')
                               , LGORT       = '{dr["LGORT"]}'
                               , UMLGO       = '{dr["UMLGO"]}'
                               , MENGE       = '{dr["MENGE"]}'
                               , CH_WEIGHT   = '{dr["CH_WEIGHT"]}'
                               , MEINS       = '{dr["MEINS"]}'
                               , KOSTL       = '{dr["KOSTL"]}'
                               , LOTNO       = '{dr["LOTNO"]}'
                               , U_LEASON    = '{dr["U_LEASON"]}'
                               , FLD01       = '{dr["FLD01"]}'
                               , FLD02       = '{dr["FLD02"]}'
                               , FLD03       = '{dr["FLD03"]}'
                               , FLD04       = '{dr["FLD04"]}'
                               , FLD05       = '{dr["FLD05"]}'
                               , ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                               , ERP_TNUMBER = '{dr["ERP_TNUMBER"]}'
                        WHERE   WERKS       = '{dr["WERKS"]}'
                        AND     SEQ         = '{dr["SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(SAP_P_EXEC)데이터 수정에 실패했습니다");
                            return;
                        }
                    }
                    dr.AcceptChanges();
                }

                viewExec.RefreshData();

                ShowMessageBox.XtraShowWarning("품목대체를 저장 했습니다");

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_l_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
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
            XMain_Search();
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
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

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridExec.Focus();
            viewExec.FocusedRowHandle = 0;
            viewExec.FocusedColumn = viewExec.VisibleColumns[0];
        }


        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewExec.RowCount == 0)
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
                int[] selectedRows = viewExec.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewExec.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM SAP_P_EXEC
                    WHERE  WERKS       = '{dr["WERKS"]}'
                            AND    SEQ         = '{dr["SEQ"]}'
                        AND ERP_UP_YN IN ('N', 'M', 'X', 'G')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE SAP_P_EXEC
                            SET  ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) WHEN 'N' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                     ELSE TO_CHAR(ERP_UP_YN) END
                                , ERP_ERR_CNT = 0
                            WHERE  WERKS       = '{dr["WERKS"]}'
                            AND    SEQ         = '{dr["SEQ"]}'
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
            }

            ShowMessageBox.XtraShowInformation("선택된 정보가 전송 대기로 변경 되었습니다");
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                if (viewExec.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제 할 작업을 선택하여 주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 삭제 하시겠습니까?"))
                {
                    return;
                }

                int[] selectedRows = viewExec.GetSelectedRows();

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewExec.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    DELETE FROM SAP_P_EXEC
                    WHERE WERKS = '{dr["WERKS"]}'
                        AND SEQ = '{dr["SEQ"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
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

            ShowMessageBox.XtraShowInformation("데이터를 삭제 했습니다.");
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "MENGE")
            {
                SQL = $@"
                SELECT 
                MATNR, MEINH, UMREZ, 
                   UMREN, LVORM
                FROM SAP_MARM
                WHERE MATNR = '{viewExec.GetFocusedRowCellValue("MATNR")}'
                    AND MEINH = '{viewExec.GetFocusedRowCellValue("MEINS")}'
                    AND ROWNUM = 1
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                double menge = Convert.ToDouble(viewExec.GetFocusedRowCellValue("MENGE"));
                double labst = Convert.ToDouble(viewExec.GetFocusedRowCellValue("LABST"));

                if (labst < menge)
                {
                    viewExec.SetFocusedRowCellValue("MENGE", labst);
                    ShowMessageBox.XtraShowWarning("생산 재고보다 많이 입력 할 수 없습니다. 생산 재고를 확인 해주세요.");
                    return;
                }


                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {                    
                    double umrez = Convert.ToDouble(ds.Tables[0].Rows[0]["UMREZ"]);
                    double umren = Convert.ToDouble(ds.Tables[0].Rows[0]["UMREN"]);

                    double result = 0;

                    if (umren != 0)
                        result = umren / umrez;

                    viewExec.SetFocusedRowCellValue("CH_WEIGHT", menge * result);
                }
                else
                {
                    ShowMessageBox.XtraShowWarning("환산 정보가 없습니다. 환산중량(KG) 를 직접 입력해주세요.");
                }
            }

            if (e.Column.FieldName == "LGORT")
            {
                SetProdStock();
            }
        }

        private void SetProdStock()
        {
            SQL = $@"
                SELECT 
                    WERKS           -- 플랜트
                    , BKLAS         -- 평가클래스
                    , MATNR         -- 품목코드
                    , CHARG         -- 배치번호
                    , LGORT         -- 저장위치
                    , LABST         -- 가용재고
                    , INSME         -- 품질중재고
                    , SPEME         -- 보류재고
                    , MEINS         -- 기본단위
                    , TO_CHAR(I_TIME, 'YYYY-MM-DD HH24:MI:SS') AS I_TIME        -- 등록일자
                FROM SAP_STOCK_MASTER
                WHERE WERKS = '{cboPlant_Code.EditValue}'
                    AND MATNR = '{viewExec.GetFocusedRowCellValue("MATNR")}'
                    AND LGORT = '{viewExec.GetFocusedRowCellValue("LGORT")}'
                ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
                viewExec.SetFocusedRowCellValue("LABST", Dbconn.conn.getData(ds, "LABST", 0));
            else
                viewExec.SetFocusedRowCellValue("LABST", 0);
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }

        private void dateEdit_workEndDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void viewExec_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            string[] value = null;

            if (viewExec.GetFocusedRowCellValue("PROC_TYPE").ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("품목전환 유형을 선택 해주세요.");
                return;
            }

            if (viewExec.GetFocusedRowCellValue("ERP_UP_YN").ToString() == "Y" || viewExec.GetFocusedRowCellValue("ERP_UP_YN").ToString() == "C")
            {
                //ShowMessageBox.XtraShowWarning("전송이 완료된 데이터는 변경 할 수 없습니다.");
                return;
            }

            try
            {
                if (e.Column.FieldName == "MATNR_NAME")
                {
                    using (m_Product child = new m_Product(viewExec.GetFocusedRowCellValue("WERKS").ToString(), resource_Type, resource_Uom))
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
                        for (int i = 0; i < value.Length; i++)
                        {
                            SQL = $@"
                            SELECT RESOURCE_NO, RESOURCE_NO || ' : ' || DESCRIPTION AS DESCRIPTION, UOM
                            FROM SAP_DI_PRODUCT
                            WHERE PLANT_CODE = '{viewExec.GetFocusedRowCellValue("WERKS")}' AND RESOURCE_NO = '{value[0]}' 
                            ";

                            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                            viewExec.SetFocusedRowCellValue("MATNR", ds.Tables[0].Rows[0]["RESOURCE_NO"]?.ToString());
                            viewExec.SetFocusedRowCellValue("MATNR_NAME", ds.Tables[0].Rows[0]["DESCRIPTION"]?.ToString());
                            viewExec.SetFocusedRowCellValue("UNIT", ds.Tables[0].Rows[0]["UOM"]?.ToString());
                            SetProdStock();

                            if (viewExec.GetFocusedRowCellValue("PROC_TYPE").ToString() == "C2")
                            {
                                SQL = $@"
                                SELECT a.IDNRK, a.IDNRK || ' : ' || b.DESCRIPTION AS DESCRIPTION, a.MEINS
                                FROM SAP_IN_BOM_COND a
                                    INNER JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.IDNRK
                                WHERE a.PLANT_CODE = '{viewExec.GetFocusedRowCellValue("WERKS")}' AND a.RESOURCE_NO = '{value[0]}' AND a.MEINS = 'KG'
                                ";

                                ds = Dbconn.conn.ExecutDataset(SQL);

                                if (Dbconn.conn.getRowCnt(ds) > 0)
                                {
                                    viewExec.SetFocusedRowCellValue("UMMAT", ds.Tables[0].Rows[0]["IDNRK"]?.ToString());
                                    viewExec.SetFocusedRowCellValue("UMMAT_NAME", ds.Tables[0].Rows[0]["DESCRIPTION"]?.ToString());
                                    viewExec.SetFocusedRowCellValue("MEINS", ds.Tables[0].Rows[0]["MEINS"]?.ToString());
                                }
                            }

                            //viewBomMix.SetRowCellValue(viewBomMix.FocusedRowHandle, viewBomMix.Columns["PLANT_CODE"], viewProduct.GetFocusedRowCellValue("PLANT_CODE"));
                            //viewBomMix.SetRowCellValue(viewBomMix.FocusedRowHandle, viewBomMix.Columns["IDNRK"], value[i]);
                            //viewBomMix.SetRowCellValue(viewBomMix.FocusedRowHandle, viewBomMix.Columns["RESOURCE_NO"], viewProduct.GetFocusedRowCellValue("RESOURCE_NO"));
                        }

                    }
                }

                if (e.Column.FieldName == "UMMAT_NAME")
                {
                    using (m_Product child = new m_Product(viewExec.GetFocusedRowCellValue("WERKS").ToString(), toResource_Type, toResource_Uom))
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
                        for (int i = 0; i < value.Length; i++)
                        {
                            SQL = $@"
                            SELECT RESOURCE_NO, RESOURCE_NO || ' : ' || DESCRIPTION AS DESCRIPTION, UOM
                            FROM SAP_DI_PRODUCT
                            WHERE PLANT_CODE = '{viewExec.GetFocusedRowCellValue("WERKS")}' AND RESOURCE_NO = '{value[0]}' 
                            ";

                            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                            viewExec.SetFocusedRowCellValue("UMMAT", ds.Tables[0].Rows[0]["RESOURCE_NO"]?.ToString());
                            viewExec.SetFocusedRowCellValue("UMMAT_NAME", ds.Tables[0].Rows[0]["DESCRIPTION"]?.ToString());
                            viewExec.SetFocusedRowCellValue("MEINS", ds.Tables[0].Rows[0]["UOM"]?.ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowAdd_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void gridCboPROC_TYPE_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = sender as LookUpEdit;

            if (editor != null)
            {
                viewExec.SetFocusedRowCellValue("MATNR", "");
                viewExec.SetFocusedRowCellValue("MATNR_NAME", "");
                viewExec.SetFocusedRowCellValue("UMMAT", "");
                viewExec.SetFocusedRowCellValue("UMMAT_NAME", "");
                resource_Uom = string.Empty;
                toResource_Uom = string.Empty;
                //LGORT, UMLGO
                if (editor.EditValue?.ToString() == "C1")
                {
                    resource_Type = $"'{clsCommon.GetResourceTypeCode("제품")}', '{clsCommon.GetResourceTypeCode("반제품")}'";

                    toResource_Type = $"'{clsCommon.GetResourceTypeCode("부산물")}'";

                    viewExec.SetFocusedRowCellValue("LGORT", "5000");
                    viewExec.SetFocusedRowCellValue("UMLGO", "4000");
                }
                else
                {
                    resource_Type = $"'{clsCommon.GetResourceTypeCode("제품")}'";
                    resource_Uom = "EA";
                    toResource_Type = $"'{clsCommon.GetResourceTypeCode("제품")}'";
                    toResource_Uom = "KG";

                    viewExec.SetFocusedRowCellValue("LGORT", "5000");
                    viewExec.SetFocusedRowCellValue("UMLGO", "5000");
                }
            }
        }

        private void btnERPDelete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewExec.RowCount == 0)
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
                int[] selectedRows = viewExec.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewExec.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM SAP_P_EXEC
                    WHERE  WERKS       = '{dr["WERKS"]}'
                            AND    SEQ         = '{dr["SEQ"]}'
                        AND ERP_UP_YN IN ('Y')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE SAP_P_EXEC
                            SET ERP_UP_YN =  CASE '{dr["ERP_UP_YN"]}'
                                                   WHEN 'Y' THEN 'D'
                                                   ELSE '{dr["ERP_UP_YN"]}'
                                               END
                                , ERP_ERR_CNT = 0
                            WHERE  WERKS       = '{dr["WERKS"]}'
                            AND    SEQ         = '{dr["SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("ERP 삭제요청에 실패했습니다");
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

            ShowMessageBox.XtraShowInformation("선택된 정보가 삭제요청으로 변경 되었습니다");
        }

        private void viewExec_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "ERP_UP_YN")
            {
                string iStatus = Convert.ToString(viewExec.GetRowCellValue(e.RowHandle, "ERP_UP_YN"));

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
