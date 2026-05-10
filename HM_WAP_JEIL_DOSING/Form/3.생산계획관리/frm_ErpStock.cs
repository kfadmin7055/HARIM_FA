using Core.Class;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_ErpStock : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_ErpStock()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void frm_ErpStock_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            //저장위치
            clsDevexpressUtil.ItemLookUpEditSetup(cboLocation, clsCommon.GetLocation(cboPlant_Code.EditValue?.ToString()), "", true, 0, true, true);
            cboLocation.Properties.PopupFormMinSize = new Size(200, 300);

            lblRemark.Text = "";

            XMain_Search();
            gridView.ShowFindPanel();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XMain_Search()
        {
            try
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
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR WERKS = '{cboPlant_Code.EditValue}')
                    AND ('{cboLocation.EditValue}' IS NULL OR LGORT = '{cboLocation.EditValue}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true, true);

                sValid = new string[] { "WERKS" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = gridView.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", "", 0, true));

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboLocation, clsCommon.GetLocation(cboPlant_Code.EditValue?.ToString()));

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPLANT, clsCommon.GetPlant("", true));

                clsDevexpressGrid.ItemLookUpEditSetup(gridUNIT, clsCommon.GetUnit());

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

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

            // 신규 행 추가
            if (e.KeyCode == Keys.F3)
            {
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
                //XMain_Delete();
            }
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("ERP 재고를 가져오는 중입니다.");

                //string sPlantCode = gridView.GetFocusedRowCellValue("WERKS").ToString();

                SQL = $@"
                SELECT REQ
                FROM SAP_STOCK_REQ
                WHERE WERKS = '{clsCommon.PlantCode}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (ds != null)
                {
                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
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
                    else
                    {
                        SQL = $@"
                        INSERT INTO SAP_STOCK_REQ (
                           WERKS, REQ, I_TIME) 
                        VALUES ( '{clsCommon.PlantCode}', 'Y', SYSDATE)
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("재고 다운로드 요청에 실패했습니다");
                            return;
                        }
                    }
                }

                //ShowMessageBox.XtraShowInformation("재고정보를 ERP에 다운로드 요청 했습니다.");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click()", ex);
                ShowMessageBox.XtraShowWarning("수정를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                timerRemark.Start();
            }
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                return;
            }

            if (cboLocation.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("저장위치를 먼저 조회 해주세요.");
                cboLocation.Focus();
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);

            gridView.SetFocusedRowCellValue("WERKS", cboPlant_Code.EditValue);
            gridView.SetFocusedRowCellValue("LGORT", cboLocation.EditValue);
            gridView.SetFocusedRowCellValue("I_TIME", DateTime.Now);
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            //if (!clsCommon.Auth_Form_Function(authDs, "W"))
            //{
            //    ShowMessageBox.XtraShowInformation("권한이 없습니다");
            //    return;
            //}

            if (DialogResult.Yes != ShowMessageBox.Confirm("재고정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);
                DataTable DT = (DataTable)gridControl.DataSource;

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
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_STOCK_MASTER (
                                   WERKS                          -- 1
                                 , BKLAS                          -- 2
                                 , SEQ
                                 , MATNR                          -- 3
                                 , LGORT                          -- 4
                                 , CHARG                          -- 5
                                 , LABST                          -- 6
                                 , INSME                          -- 7
                                 , SPEME                          -- 8
                                 , MEINS                          -- 9
                                 , I_TIME                         -- 10
                        )
                        VALUES (
                                   '{dr["WERKS"]}'                -- 1
                                 , '{dr["BKLAS"]}'                -- 2
                                 , (SELECT NVL(MAX(SEQ) + 1, 1) FROM SAP_STOCK_MASTER WHERE WERKS = '{dr["WERKS"]}' AND BKLAS = '{dr["BKLAS"]}')
                                 , '{dr["MATNR"]}'                -- 3
                                 , '{dr["LGORT"]}'                -- 4
                                 , '{dr["CHARG"]}'                -- 5
                                 , '{dr["LABST"]}'                -- 6
                                 , '{dr["INSME"]}'                -- 7
                                 , '{dr["SPEME"]}'                -- 8
                                 , '{dr["MEINS"]}'                -- 9
                                 , SYSDATE                        -- 10
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(SAP_STOCK_MASTER)데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        //string bin_stock = string.Empty;

                        ////빈원료가 바뀌었을 경우 재고 초기화
                        //SQL = "SELECT RESOURCE_NO FROM BIN WHERE LOCATION = '{0}'";
                        //SQL = string.Format(SQL, dr["LOCATION"].ToString());

                        //using (DataSet binResDs = Dbconn.conn.ExecutDataset(SQL))
                        //{
                        //    if (Dbconn.conn.getRowCnt(binResDs) > 0)
                        //    {
                        //        if (!dr["RESOURCE_NO"].ToString().Equals(Dbconn.conn.getData(binResDs, "RESOURCE_NO", 0)))
                        //        {
                        //            bin_stock = "0";
                        //        }
                        //        else
                        //        {
                        //            bin_stock = dr["STOCK"].ToString();
                        //        }
                        //    }
                        //}

                        //if (string.IsNullOrEmpty(bin_stock))
                        //{
                        //    bin_stock = dr["STOCK"].ToString();
                        //}


                        SQL = $@"
                        UPDATE SAP_STOCK_MASTER
                        SET
                                   CHARG  = '{dr["CHARG"]}'           -- 1
                                 , LABST  = '{dr["LABST"]}'           -- 2
                                 , INSME  = '{dr["INSME"]}'           -- 3
                                 , SPEME  = '{dr["SPEME"]}'           -- 4
                                 , MEINS  = '{dr["MEINS"]}'           -- 5
                                 , I_TIME = SYSDATE          -- 6
                        WHERE
                                   WERKS  = '{dr["WERKS"]}'
                        AND        BKLAS  = '{dr["BKLAS"]}'
                        AND        MATNR  = '{dr["MATNR"]}'
                        AND        LGORT  = '{dr["LGORT"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning(dr.RowError);
                            return;
                        }


                        //int bin_seq = 0;
                        //if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                        //{
                        //    bin_seq = 0;

                        //}
                        //else
                        //{
                        //    SQL = $@"
                        //    SELECT SEQ
                        //    FROM BIN
                        //    WHERE PLANT_CODE    = '{dr["PLANT_CODE"]}'
                        //        AND PROCESS_KEY =  '{dr["PROCESS_KEY"]}'
                        //        AND L_CODE      =  '{dr["L_CODE"]}'
                        //        AND LOCATION    =  '{dr["LOCATION"]}'
                        //        AND SCALE_CODE  = '{dr["SCALE_CODE"]}' AND RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                        //    ";

                        //    bin_seq = Dbconn.conn.getRowCnt(Dbconn.conn.ExecutDataset(SQL));
                        //}

                        //SQL = $@"
                        //UPDATE BIN SET SEQ = '{bin_seq}'
                        //WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY =  '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                        //    AND LOCATION = '{dr["LOCATION"]}'
                        //";

                        ////스케일에 원료 1개인 빈은 순번 1로 조정
                        //SQL = $@"
                        //UPDATE BIN SET SEQ = 1 WHERE RESOURCE_NO IN
                        //(
                        //SELECT A.RESOURCE_NO FROM(
                        //SELECT  RESOURCE_NO FROM BIN
                        //WHERE SCALE_CODE IS NOT NULL
                        //AND PLANT_CODE    = '{dr["PLANT_CODE"]}'
                        //        AND PROCESS_KEY =  '{clsCommon.GetProcessKey("배합")}'
                        //        AND bin.LOCATION <> '315'
                        //        AND SCALE_CODE is not null
                        //GROUP BY RESOURCE_NO
                        //HAVING  COUNT(LOCATION) = 1 AND MAX(SEQ) > 1
                        //  ) A
                        //) ";

                        Dbconn.conn.SQLrun(SQL);

                    }
                    dr.AcceptChanges();
                    gridView.RefreshData();
                }

                ShowMessageBox.XtraShowInformation("재고정보를 저장 했습니다.");

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                string sPLANT_CODE = gridView.GetFocusedRowCellValue("WERKS").ToString();
                string sBKLAS = gridView.GetFocusedRowCellValue("BKLAS").ToString();
                string sMATNR = gridView.GetFocusedRowCellValue("MATNR").ToString();
                string sLGORT = gridView.GetFocusedRowCellValue("LGORT").ToString();

                splashScreenManager.ShowWaitForm();

                SQL = $@"
                DELETE SAP_STOCK_MASTER
                WHERE WERKS  = '{sPLANT_CODE}'
                    AND BKLAS  = '{sBKLAS}'
                    AND MATNR  = '{sMATNR}'
                    AND LGORT  = '{sLGORT}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowInformation("재고정보를 삭제 했습니다.");

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

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboLocation_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                //string sPlantCode = gridView.GetFocusedRowCellValue("WERKS").ToString();

                SQL = $@"
                SELECT REQ
                FROM SAP_STOCK_REQ
                WHERE WERKS = '{clsCommon.PlantCode}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (ds != null)
                {
                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
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
                    else
                    {
                        SQL = $@"
                        INSERT INTO SAP_STOCK_REQ (
                           WERKS, REQ, I_TIME) 
                        VALUES ( '{clsCommon.PlantCode}', 'Y', SYSDATE)
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("재고 다운로드 요청에 실패했습니다");
                            return;
                        }
                    }
                }

                ShowMessageBox.XtraShowInformation("재고정보를 ERP에 다운로드 요청 했습니다.");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click()", ex);
                ShowMessageBox.XtraShowWarning("수정를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {

                timerRemark.Start();  // 재가동
            }
        }

        private void timerRemark_Tick(object sender, EventArgs e)
        {
            //string sPlantCode = gridView.GetFocusedRowCellValue("WERKS").ToString();

            try
            {
                SQL = $@"
                SELECT REQ
                FROM SAP_STOCK_REQ
                WHERE REQ = 'Y'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                string sReq = Dbconn.conn.getData(ds, "REQ", 0);

                if (sReq == "Y")
                    lblRemark.Text = "ERP에 재고 정보 갱신을 요청 중입니다.";
                else
                {
                    lblRemark.Text = "";
                    timerRemark.Stop();  // 재가동
                    XMain_Search();

                    if (splashScreenManager.IsSplashFormVisible)
                    {
                        splashScreenManager.CloseWaitForm();
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "timerRemark_Tick()", ex);
                ShowMessageBox.XtraShowWarning("수정를 실행하는 도중 에러가 발생했습니다");
            }
        }
    }
}