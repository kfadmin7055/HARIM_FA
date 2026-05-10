using Core.Class;
using DevExpress.XtraEditors;
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
    public partial class frm_DosingWorkLog : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;
        DataSet dsMain = null;

        public frm_DosingWorkLog()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);

            InitControl();
        }

        private void InitControl()
        {
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboResourceNo, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 2, false, false, "KG", false, true), "품목을 선택 해주세요.", false);

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboLocation, clsCommon.GetLocation(clsCommon.PlantCode, ""));

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboUser, clsCommon.GetDO_INSA(clsCommon.PlantCode));

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboDOSING_YN, clsCommon.GetYn(null, new string[] { "Y", "N" }));

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboVINSPECTION, clsCommon.GetYn(new string[] { "O", "X" }, new string[] { "O", "X" }));
        }

        private void XMain_Search()
        {
            try
            {
                //clsDevexpressGrid.ItemLookUpEditSetup(gridCboRESOURCE_NO, clsCommon.GetResource("", "", $"'{clsCommon.GetResourceTypeCode("원재료")}', '{clsCommon.GetResourceTypeCode("반제품")}'"));

                SQL = $@"
                SELECT a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.NUM
                    , a.RESOURCE_NO, p.DESCRIPTION, a.BATCH, a.NOTE, a.START_TIME, a.END_TIME
                    , (SELECT SUM(CASE WHEN wo.BU_YN = 'Y' THEN wo.BATCH * wd.SET_VAL ELSE 0 END)
                     FROM WORK_ORDER wo
                     JOIN WORK_DETAIL wd 
                       ON wd.PLANT_CODE = wo.PLANT_CODE
                      AND wd.PROCESS_KEY = wo.PROCESS_KEY
                      AND wd.L_CODE = wo.L_CODE
                      AND wd.WORKDATE = wo.WORKDATE
                      AND wd.NUM = wo.NUM
                      AND wd.INGRED_CODE IN (
                           SELECT RESOURCE_NO_2 
                             FROM SAP_IN_PRODUCT_RC 
                            WHERE PLANT_CODE = wo.PLANT_CODE 
                              AND RESOURCE_NO = wo.RESOURCE_NO
                      )
                    WHERE wo.PLANT_CODE = a.PLANT_CODE
                      AND wo.PROCESS_KEY = a.PROCESS_KEY
                      AND wo.L_CODE = a.L_CODE
                      AND wo.WORKDATE = a.WORKDATE
                      AND wo.NUM = a.NUM
                  ) AS BBATCH_Q
                  , NVL(b.DOSING_YN, 'Y') AS DOSING_YN 
                  , NVL(b.VINSPECTION, 'O') AS VINSPECTION
                   , CASE WHEN bin.PROCESS_KEY = 'PLP010' THEN bin.LOCATION END AS PELLET_BIN
                  , CASE WHEN bin.PROCESS_KEY <> 'PLP010' THEN bin.LOCATION END AS BIN
                  , (SELECT SUM(wr.P_Q) 
                     FROM WORK_REMARK wr
                    WHERE wr.PLANT_CODE = a.PLANT_CODE
                      AND wr.PROCESS_KEY = 'SAP_' || a.PROCESS_KEY
                      AND wr.L_CODE = a.L_CODE
                      AND wr.WORKDATE = a.WORKDATE
                      AND wr.NUM = a.NUM
                  ) AS ERP_Q
                  , a.REMARK
                FROM WORK_ORDER a
                    LEFT JOIN WORK_ORDER_ADD b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE 
                                                AND b.WORKDATE = a.WORKDATE AND b.NUM = a.NUM
                    LEFT JOIN SAP_DI_PRODUCT P ON p.RESOURCE_NO = a.RESOURCE_NO
                    LEFT JOIN BIN bin ON bin.PLANT_CODE = a.PLANT_CODE AND bin.LOCATION IN (a.LOCATION_ED, a.LOCATION_ED2)
                WHERE a.PLANT_CODE = '{clsCommon.PlantCode}'
                    AND a.WORKDATE = '{Convert.ToDateTime(dtWork.EditValue).ToString("yyyyMMdd")}'
                    AND a.PROCESS_KEY = '{clsCommon.GetProcessKey("배합", clsCommon.PlantCode)}'
                    AND a.ERP_OSTATUS = 'Y'
                    AND NVL(a.DEL_FLAG, 'N') != 'Y'
                ORDER BY a.WORKDATE, a.NUM, a.RESOURCE_NO, a.END_TIME
                ";

                dsMain = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, dsMain.Tables[0], true);

                sValid = new string[] { "" };
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_DosingWorkLog_Load(object sender, EventArgs e)
        {
            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                dtWork.EditValue = DateTime.Today;

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_DosingWorkLog_Load", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_excelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    gridView.ExportToXlsx(sfd.FileName);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_excelExport_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    e.Info.DisplayText = (1 + e.RowHandle).ToString();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_CustomDrawRowIndicator", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void reflash_timer_Tick(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridViewAddRow(gridView);
                gridView.SetFocusedRowCellValue("SEQ", clsCommon.GetMaxSeq("WAP_SLIO_DOWN", "SEQ", new Dictionary<string, string> { { "IS_NO", $"" } }));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["IN_DAY"], DateTime.Now.ToString("yyyy-MM-dd"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["IN_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["OUT_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["SPIV_CAR_WEIGHT"], "0");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowAdd_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(gridView);
                clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("분쇄일지 정보를 저장하시겠습니까?"))
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

                if (DT == null)
                {
                    return;
                }


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

                    if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                    {
                        dr.SetColumnError("RESOURCE_NO", "원료를 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        /* 주원료투입공정 작업일지 */
                        INSERT INTO BIN_INGRED_INPUT(
                               PLANT_CODE
                             , SEQ
                             , INPUT_DATE
                             , INPUT_QTY
                             , I_TIME
                             , I_USER
                             , RESOURCE_NO
                             , INPUT_START_TIME
                             , INPUT_LOCATION
                             , REMARKS
                             , INPUT_USER
                             , INGRED_GUBUN
                             , B_W
                             , WEIGHT
                             , CAR_NO
                             , INPUT_END_TIME
                        )
                        VALUES (
                               '{dr["PLANT_CODE"]}'         /* PLANT_CODE */
                             , '{dr["SEQ"]}'                /* SEQ */
                             , '{dr["INPUT_DATE"]}'         /* INPUT_DATE */
                             , '{dr["INPUT_QTY"]}'          /* INPUT_QTY */
                             , SYSDATE                      /* I_TIME */
                             , '{dr["I_USER"]}'             /* I_USER */
                             , '{dr["RESOURCE_NO"]}'        /* RESOURCE_NO */
                             , '{dr["INPUT_START_TIME"]}'   /* INPUT_START_TIME */
                             , '{dr["INPUT_LOCATION"]}'     /* INPUT_LOCATION */
                             , '{dr["REMARKS"]}'            /* REMARKS */
                             , '{dr["INPUT_USER"]}'         /* INPUT_USER */
                             , '{dr["INGRED_GUBUN"]}'       /* INGRED_GUBUN */
                             , '{dr["B_W"]}'                /* B_W */
                             , '{dr["WEIGHT"]}'             /* WEIGHT */
                             , '{dr["CAR_NO"]}'             /* CAR_NO */
                             , '{dr["INPUT_END_TIME"]}'     /* INPUT_END_TIME */
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        if (!clsCommon.Auth_Form_Function(authDs, "W"))
                        {
                            ShowMessageBox.XtraShowInformation("권한이 없습니다");
                            return;
                        }

                        SQL = $@"
                        /* 주원료투입공정 작업일지 */
                        UPDATE BIN_INGRED_INPUT
                           SET INPUT_DATE        = '{dr["INPUT_DATE"]}'
                             , INPUT_QTY         = '{dr["INPUT_QTY"]}'
                             , I_TIME            = SYSDATE
                             , I_USER            = '{dr["I_USER"]}'
                             , RESOURCE_NO       = '{dr["RESOURCE_NO"]}'
                             , INPUT_START_TIME  = TO_DATE('{Convert.ToDateTime(dr["INPUT_START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                             , INPUT_LOCATION    = '{dr["INPUT_LOCATION"]}'
                             , REMARKS           = '{dr["REMARKS"]}'
                             , INPUT_USER        = '{dr["INPUT_USER"]}'
                             , INGRED_GUBUN      = '{dr["INGRED_GUBUN"]}'
                             , B_W               = '{dr["B_W"]}'
                             , WEIGHT            = '{dr["WEIGHT"]}'
                             , CAR_NO            = '{dr["CAR_NO"]}'
                             , INPUT_END_TIME    = TO_DATE('{Convert.ToDateTime(dr["INPUT_END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                         WHERE PLANT_CODE = '{clsCommon.PlantCode}'
                           AND SEQ        = '{dr["SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
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
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (gridView.GetSelectedRows().Length == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 데이터를 체크하여주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 하차내역 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in gridView.GetSelectedRows())
                {
                    DataRow sel_row = gridView.GetDataRow(id);
                    string seq = sel_row["SEQ"].ToString().Trim();

                    if (sel_row.RowState != DataRowState.Added)
                    {
                        SQL = $"UPDATE WAP_SLIO_DOWN SET DEL_FLAG = 'Y', U_TIME = GETDATE(), U_USER = '{clsCommon._strUserId}' WHERE SEQ = '{seq}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                            return;
                        }
                    }
                    else
                    {
                        gridView.DeleteRow(gridView.FocusedRowHandle);
                    }
                } //foeach

                Dbconn.conn.Commit();

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                Dbconn.conn.Rollback();
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
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void dateEdit_workStartDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workEndDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        /// <summary>
        /// 일지 프린터
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                clsPrintReport.PrintDosingWorkLog(dtWork.EditValue.ToString(), dsMain);
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