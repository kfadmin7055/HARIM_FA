using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
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
    public partial class frm_SubIngredReceivingInsp : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;
        DataSet dsMain = null;
        DataSet dsSub = null;

        public frm_SubIngredReceivingInsp()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewMain, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewSub, Properties.Settings.Default.FontSize);

            InitControl();
        }

        private void InitControl()
        {
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboResourceNo, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 2, false, false, "KG", false, true), "품목을 선택 해주세요.", false);

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboLocation, clsCommon.GetLocation(clsCommon.PlantCode, ""));

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboUser, clsCommon.GetDO_INSA(clsCommon.PlantCode));
            
            clsDevexpressGrid.ItemLookUpEditSetup(gridMCboVINSPECTION, clsCommon.GetYn(new string[] { "O", "X" }, new string[] { "O", "X" }), "", false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridSCboVINSPECTION, clsCommon.GetYn(new string[] { "O", "X" }, new string[] { "O", "X" }), "", false);
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT wgd.MATNR, p.DESCRIPTION, wg.PARTNER, c.NAME_ORG1 AS PARTNER_DESC, wgd.R_GR_QNTY, wd.INCAR_NO
                    , NVL(wgi.VINSPECTION, 'O') AS VINSPECTION
                    , wgd.JJO AS MFGDATE
                    , wgd.SBDAY AS EXPDATE
                    , wgi.REMARKS
                    , NVL(wgi.BJ, 0) AS B_W
                    , wd.IS_NO
                    , derd.CHARG                -- 모선코드
                    , derd.CHARG_TEXT           -- 모선명
                FROM WAP_DECAR wd
                    JOIN WAP_GOCAR wg ON wg.IS_NO = wd.IS_NO
                    JOIN WAP_GOCARD wgd ON wgd.IS_NO = wg.IS_NO AND wgd.EBELN = wg.EBELN
                    JOIN SAP_INPUT_POORDERM_CON derm ON derm.EBELN = wgd.EBELN
                    JOIN SAP_INPUT_POORDERD_CON derd ON derd.EBELN = derm.EBELN
                    JOIN INGRED i ON NVL(i.MS_GUBUN, 'M') = 'M' AND i.RESOURCE_NO = wgd.MATNR
                    LEFT JOIN WAP_SLIO_DOWN wgi ON wgi.IS_NO = wg.IS_NO
                    LEFT JOIN SAP_DI_PRODUCT p ON p.RESOURCE_NO = wgd.MATNR
                    LEFT JOIN SAP_DI_CUSTOMER c ON c.PARTNER = wg.PARTNER
                WHERE TRUNC(wd.INCAR_DATE) = TO_DATE('{dtWork.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD')
                ";

                dsMain = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, dsMain.Tables[0], true);

                sValid = new string[] { "" };
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XSub_Search()
        {
            try
            {
                SQL = $@"
                SELECT wgd.MATNR, p.DESCRIPTION, wg.PARTNER, c.NAME_ORG1 AS PARTNER_DESC, wgd.R_GR_QNTY, wd.INCAR_NO
                    , NVL(wgi.VINSPECTION, 'O') AS VINSPECTION, wgi.MFGDATE, wgi.EXPDATE, wgi.REMARKS, wgi.B_W
                    , wd.IS_NO
                    , derd.CHARG                -- 모선코드
                    , derd.CHARG_TEXT           -- 모선명
                FROM WAP_DECAR wd
                    JOIN WAP_GOCAR wg ON wg.IS_NO = wd.IS_NO
                    JOIN WAP_GOCARD wgd ON wgd.IS_NO = wg.IS_NO AND wgd.EBELN = wg.EBELN
                    JOIN SAP_INPUT_POORDERM_CON derm ON derm.EBELN = wgd.EBELN
                    JOIN SAP_INPUT_POORDERD_CON derd ON derd.EBELN = derm.EBELN
                    JOIN INGRED i ON NVL(i.MS_GUBUN, 'M') = 'S' AND i.RESOURCE_NO = wgd.MATNR
                    LEFT JOIN WAP_GOCAR_INSP wgi ON wgi.IS_NO = wg.IS_NO
                    LEFT JOIN SAP_DI_PRODUCT p ON p.RESOURCE_NO = wgd.MATNR
                    LEFT JOIN SAP_DI_CUSTOMER c ON c.PARTNER = wg.PARTNER
                WHERE TRUNC(wd.INCAR_DATE) = TO_DATE('{dtWork.DateTime.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD')
                ";

                dsSub = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridSub, viewSub, dsSub.Tables[0], true);

                sValid = new string[] { "" };
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_SubIngredReceivingInsp_Load(object sender, EventArgs e)
        {
            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                dtWork.EditValue = DateTime.Today;

                XMain_Search();
                XSub_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridMain));
                // Application.AddMessageFilter(new GridMouseWheelFilter(gridSub));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_SubIngredReceivingInsp_Load", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
            XSub_Search();
        }

        private void btn_excelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    viewSub.ExportToXlsx(sfd.FileName);
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
            XSub_Search();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewSub);
                DataTable DT = (DataTable)gridMain.DataSource;


                if (DT == null)
                {
                    return;
                }


                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewSub);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewSub.FocusedColumn = viewSub.Columns[rValid]; // 이동할 컬럼명
                        viewSub.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    
                    if (!clsCommon.Auth_Form_Function(authDs, "W"))
                    {
                        ShowMessageBox.XtraShowInformation("권한이 없습니다");
                        return;
                    }

                    SQL = $@"
                    SELECT 1
                    FROM WAP_GOCAR_INSP
                    WHERE IS_NO = '{dr["IS_NO"]}'
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        SQL = $@"
                        /* 원료 입고 검사 */
                        UPDATE WAP_GOCAR_INSP
                            SET VINSPECTION = '{dr["VINSPECTION"]}'
                                , REMARKS     = '{dr["REMARKS"]}'
                                , B_W         = '{dr["B_W"]}'
                            WHERE IS_NO = '{dr["IS_NO"]}'
                        ";
                    }
                    else
                    {
                        SQL = $@"
                        /* 원료 입고 검사 */
                        INSERT INTO WAP_GOCAR_INSP(
                                IS_NO
                                , VINSPECTION
                                , REMARKS
                                , B_W
                        )
                        VALUES (
                                '{dr["IS_NO"]}'           /* IS_NO */
                                , '{dr["VINSPECTION"]}'     /* VINSPECTION */
                                , '{dr["REMARKS"]}'         /* REMARKS */
                                , '{dr["B_W"]}'
                        )
                        ";
                    }                        

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                        return;
                    }

                    dr.AcceptChanges();

                } //foreach

                XMain_Search();
                XSub_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XSub_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewSub);
                DataTable DT = (DataTable)gridSub.DataSource;


                if (DT == null)
                {
                    return;
                }


                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewSub);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewSub.FocusedColumn = viewSub.Columns[rValid]; // 이동할 컬럼명
                        viewSub.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        /* 부원료 입고 검사 */
                        INSERT INTO WAP_GOCAR_INSP(
                                IS_NO
                                , VINSPECTION
                                , MFGDATE
                                , EXPDATE
                                , REMARKS
                        )
                        VALUES (
                                '{dr["IS_NO"]}'           /* IS_NO */
                                , '{dr["VINSPECTION"]}'     /* VINSPECTION */
                                , '{dr["MFGDATE"]}'         /* MFGDATE */
                                , '{dr["EXPDATE"]}'         /* EXPDATE */
                                , '{dr["REMARKS"]}'         /* REMARKS */
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
                        SELECT 1
                        FROM WAP_GOCAR_INSP
                        WHERE IS_NO = '{dr["IS_NO"]}'
                        ";

                        DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            SQL = $@"
                            /* 부원료 입고 검사 */
                            UPDATE WAP_GOCAR_INSP
                               SET VINSPECTION = '{dr["VINSPECTION"]}'
                                 , MFGDATE     = TO_DATE('{Convert.ToDateTime(dr["MFGDATE"]).ToString("yyyy-MM-dd")}', 'YYYY-MM-DD')
                                 , EXPDATE     = TO_DATE('{Convert.ToDateTime(dr["EXPDATE"]).ToString("yyyy-MM-dd")}', 'YYYY-MM-DD')
                                 , REMARKS     = '{dr["REMARKS"]}'
                             WHERE IS_NO = '{dr["IS_NO"]}'
                            ";
                        }
                        else
                        {
                            SQL = $@"
                            /* 부원료 입고 검사 */
                            INSERT INTO WAP_GOCAR_INSP(
                                    IS_NO
                                    , VINSPECTION
                                    , MFGDATE
                                    , EXPDATE
                                    , REMARKS
                            )
                            VALUES (
                                    '{dr["IS_NO"]}'           /* IS_NO */
                                    , '{dr["VINSPECTION"]}'     /* VINSPECTION */
                                    , TO_DATE('{Convert.ToDateTime(dr["MFGDATE"]).ToString("yyyy-MM-dd")}', 'YYYY-MM-DD')         /* MFGDATE */
                                    , TO_DATE('{Convert.ToDateTime(dr["EXPDATE"]).ToString("yyyy-MM-dd")}', 'YYYY-MM-DD')         /* EXPDATE */
                                    , '{dr["REMARKS"]}'         /* REMARKS */
                            )
                            ";
                        }

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
                XSub_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
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

                if (viewSub.GetSelectedRows().Length == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 데이터를 체크하여주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 하차내역 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in viewSub.GetSelectedRows())
                {
                    DataRow sel_row = viewSub.GetDataRow(id);
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
                        viewSub.DeleteRow(viewSub.FocusedRowHandle);
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

        private void XSub_Delete()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (viewSub.GetSelectedRows().Length == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 데이터를 체크하여주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 하차내역 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in viewSub.GetSelectedRows())
                {
                    DataRow sel_row = viewSub.GetDataRow(id);
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
                        viewSub.DeleteRow(viewSub.FocusedRowHandle);
                    }
                } //foeach

                Dbconn.conn.Commit();

                XSub_Search();
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
                XSub_Search();
            }

            // 신규 행 추가
            if (e.KeyCode == Keys.F3)
            {
                e.Handled = true;
            }

            // 행 삭제
            if (e.KeyCode == Keys.Delete)
            {
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
            gridSub.Focus();
            viewSub.FocusedRowHandle = 0;
            viewSub.FocusedColumn = viewSub.VisibleColumns[0];
        }

        private void dateEdit_workStartDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
            XSub_Search();
        }

        /// <summary>
        /// 일지 프린트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                clsPrintReport.PrintSubIngredReceivingInsp(dtWork.EditValue.ToString(), dsMain, dsSub);
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

        private void btn_MainRowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridViewAddRow(viewSub);
                viewSub.SetFocusedRowCellValue("SEQ", clsCommon.GetMaxSeq("WAP_SLIO_DOWN", "SEQ", new Dictionary<string, string> { { "IS_NO", $"" } }));
                viewSub.SetRowCellValue(viewSub.FocusedRowHandle, viewSub.Columns["IN_DAY"], DateTime.Now.ToString("yyyy-MM-dd"));
                viewSub.SetRowCellValue(viewSub.FocusedRowHandle, viewSub.Columns["IN_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                viewSub.SetRowCellValue(viewSub.FocusedRowHandle, viewSub.Columns["OUT_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                viewSub.SetRowCellValue(viewSub.FocusedRowHandle, viewSub.Columns["SPIV_CAR_WEIGHT"], "0");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowAdd_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_MainRowDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(viewSub);
                clsDevexpressGrid.GridViewLastAddRowDelete(viewSub);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_MainSave_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("원료 입고 검사 정보를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void btn_MainDelete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }

        private void btn_SubRowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridViewAddRow(viewSub);
                viewSub.SetFocusedRowCellValue("SEQ", clsCommon.GetMaxSeq("WAP_SLIO_DOWN", "SEQ", new Dictionary<string, string> { { "IS_NO", $"" } }));
                viewSub.SetRowCellValue(viewSub.FocusedRowHandle, viewSub.Columns["IN_DAY"], DateTime.Now.ToString("yyyy-MM-dd"));
                viewSub.SetRowCellValue(viewSub.FocusedRowHandle, viewSub.Columns["IN_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                viewSub.SetRowCellValue(viewSub.FocusedRowHandle, viewSub.Columns["OUT_TIME"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                viewSub.SetRowCellValue(viewSub.FocusedRowHandle, viewSub.Columns["SPIV_CAR_WEIGHT"], "0");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowAdd_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_SubRowDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "D"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(viewSub);
                clsDevexpressGrid.GridViewLastAddRowDelete(viewSub);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_SubSave_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("부원료 입고 검사 정보를 저장하시겠습니까?"))
            {
                return;
            }

            XSub_Save();
        }

        private void btn_SubDelete_Click(object sender, EventArgs e)
        {
            XMain_Delete();
        }
    }
}