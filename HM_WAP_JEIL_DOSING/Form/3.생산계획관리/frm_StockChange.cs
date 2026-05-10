using Core.Class;
using Core.Enum;
using DevExpress.DataAccess.Native.Sql;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
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
    public partial class frm_StockChange : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;
        private string[] sValid = null;

        string Gubun = String.Empty;

        public frm_StockChange()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView1, Properties.Settings.Default.FontSize);
        }



        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                   WERKS, INTERFACEID, SEQ, 
                   BWART, PROC_TYPE, XSEQNR_C, 
                   MATNR, UMMAT, BLDAT, 
                   BUDAT, LGORT, UMLGO, 
                   MENGE, MEINS, KOSTL, 
                   LOTNO, U_LEASON, FLD01, 
                   FLD02, FLD03, FLD04, 
                   FLD05, ERP_UP_YN, ERP_TNUMBER
                FROM SAP_P_EXEC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl1, gridView1, ds.Tables[0], false);

                sValid = new string[] { "WERKS", "PROC_TYPE", "MATNR", "UMMAT", "LGORT", "UMLGO", "MENGE" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = gridView1.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                gridView1.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

                // ERP 전송상태
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboTransFlag, clsCommon.GetTransFlag(), "", false, false);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCEE_NO, clsCommon.GetResource("", "", $"'{clsCommon.GetResourceTypeCode("제품")}', '{clsCommon.GetResourceTypeCode("반제품")}'"), "제품을 선택 해주세요.", false);

                // 이동유형
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboBWART, clsCommon.GetMovType(), "", false);

                // 품목전환
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPROC_TYPE, clsCommon.GetProcType(), "", false);

                // 저장위치
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboLocation, clsCommon.GetLocation(clsCommon.PlantCode), "창고 위치를 선택 해주세요.");


                clsDevexpressGrid.ItemLookUpEditSetup(gridcboXSEQNR_C, clsCommon.GetYn(null, new string[] {"승인", "취소"}), "", false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_StockChange_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            dateEdit_workDate.EditValue = DateTime.Today;

            // ERP 진행여부
            clsDevexpressUtil.ItemLookUpEditSetup(cboERPUpLoad, clsCommon.GetTransFlag(), "", false, 0, true);

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl1));
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(gridView1);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["INTERFACE_STATUS"], 0);
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView1);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView1);
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (!gridView1.GetFocusedRowCellValue("INTERFACE_STATUS").ToString().Trim().Equals("0"))
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
            }
        }

        private void repItemLkUpEdit_RESOURCE_NO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //gridView.SetRowCellValue(gridView.FocusedRowHandle, "UOM_DESC", e.NewValue);
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
                clsDevexpressGrid.GridEndEdit(gridView1);

                DataTable DT = (DataTable)gridControl1.DataSource;

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
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView1);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView1.FocusedColumn = gridView1.Columns[rValid]; // 이동할 컬럼명
                        gridView1.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(dr["FROM_RESOURCE_NO"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("FROM 품목을 선택하여 주세요");
                        dr.SetColumnError("FROM_RESOURCE_NO", "FROM 품목을 선택하여 주세요");
                        return;
                    }


                    if (string.IsNullOrEmpty(dr["TO_RESOURCE_NO"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("TO 품목을 선택하여 주세요");
                        dr.SetColumnError("TO_RESOURCE_NO", "FROM 품목을 선택하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["FROM_QTY"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("FROM 수량을 입력하여 주세요");
                        dr.SetColumnError("FROM_QTY", "FROM 수량을 입력하여 주세요");
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["FROM_LOCATION"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("FROM 저장위치를 선택하여 주세요");
                        dr.SetColumnError("FROM_LOCATION", "FROM 저장위치를 선택하여 주세요");
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SAP_P_EXEC (
                           INTERFACEID, WERKS, SEQ, 
                           BWART, PROC_TYPE, XSEQNR_C, 
                           MATNR, UMMAT, BLDAT, 
                           BUDAT, LGORT, UMLGO, 
                           MENGE, MEINS, KOSTL, 
                           LOTNO, U_LEASON, FLD01, 
                           FLD02, FLD03, FLD04, 
                           FLD05, ERP_UP_YN, ERP_TNUMBER) 
                        VALUES ( 
                           '{dr["INTERFACEID"]}'
                         , '{dr["WERKS"]}'
                         , '{dr["SEQ"]}'
                         , '{dr["BWART"]}'
                         , '{dr["PROC_TYPE"]}'
                         , '{dr["XSEQNR_C"]}'
                         , '{dr["MATNR"]}'
                         , '{dr["UMMAT"]}'
                         , '{dr["BLDAT"]}'
                         , '{dr["BUDAT"]}'
                         , '{dr["LGORT"]}'
                         , '{dr["UMLGO"]}'
                         , '{dr["MENGE"]}'
                         , '{dr["MEINS"]}'
                         , '{dr["KOSTL"]}'
                         , '{dr["LOTNO"]}'
                         , '{dr["U_LEASON"]}'
                         , '{dr["FLD01"]}'
                         , '{dr["FLD02"]}'
                         , '{dr["FLD03"]}'
                         , '{dr["FLD04"]}'
                         , '{dr["FLD05"]}'
                         , 'N'
                         , '{dr["ERP_TNUMBER"]}'  )
                        ";
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE SAP_P_EXEC
                        SET    INTERFACEID = '{dr["INTERFACEID"]}'
                               , WERKS       = '{dr["WERKS"]}'
                               , SEQ         = '{dr["SEQ"]}'
                               , BWART       = '{dr["BWART"]}'
                               , PROC_TYPE   = '{dr["PROC_TYPE"]}'
                               , XSEQNR_C    = '{dr["XSEQNR_C"]}'
                               , MATNR       = '{dr["MATNR"]}'
                               , UMMAT       = '{dr["UMMAT"]}'
                               , BLDAT       = '{dr["BLDAT"]}'
                               , BUDAT       = '{dr["BUDAT"]}'
                               , LGORT       = '{dr["LGORT"]}'
                               , UMLGO       = '{dr["UMLGO"]}'
                               , MENGE       = '{dr["MENGE"]}'
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
                        WHERE  WERKS       = '{dr["WERKS"]}'
                        AND    SEQ         = '{dr["SEQ"]}'
                        ";
                    }

                    dr.AcceptChanges();
                    gridView1.RefreshData();

                    XMain_Search();

                }

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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView1.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(gridView1);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 " + gridView1.GetFocusedRowCellValue("LOCATION") + " 빈을 삭제하시겠습니까?"))
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

                SQL = $@"
                DELETE FROM SAP_P_EXEC
                WHERE WERKS = '{gridView1.GetFocusedRowCellValue("WERKS")}'
                    AND SEQ = '{gridView1.GetFocusedRowCellValue("SEQ")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowInformation("데이터를 삭제 했습니다.");

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
            gridControl1.Focus();
            gridView1.FocusedRowHandle = 0;
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
        }

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView1.RowCount == 0)
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
                int[] selectedRows = gridView1.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = gridView1.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM SAP_P_EXEC
                    WHERE  INTERFACEID = '{dr["INTERFACEID"]}'
                            AND    WERKS       = '{dr["WERKS"]}'
                            AND    SEQ         = '{dr["SEQ"]}'
                        AND ERP_UP_YN IN ('N', 'M', 'X', 'G')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE SAP_P_EXEC
                            SET ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) WHEN 'N' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                     ELSE TO_CHAR(ERP_UP_YN) END
                                , ERP_ERR_CNT = 0
                            WHERE  INTERFACEID = '{dr["INTERFACEID"]}'
                            AND    WERKS       = '{dr["WERKS"]}'
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

        private void cboERPUpLoad_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}