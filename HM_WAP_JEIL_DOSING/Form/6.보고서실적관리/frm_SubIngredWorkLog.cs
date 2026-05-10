using Core.Class;
using DevExpress.XtraEditors;
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
    public partial class frm_SubIngredWorkLog : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        DataSet authDs;
        private string[] sValid = null;
        DataSet dsMain = null;

        public frm_SubIngredWorkLog()
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
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT bii.SEQ 
                    , bii.RESOURCE_NO                                                           -- 품명
                    , p.DESCRIPTION                                                             -- 품명   
                    , bii.INPUT_DATE                                                            -- 입고일
                    , TO_CHAR(bii.INPUT_START_TIME, 'HH24:MI:SS') AS INPUT_START_TIME           -- 투입시간
                    , TO_CHAR(bii.INPUT_END_TIME, 'HH24:MI:SS') AS INPUT_END_TIME               -- 투입시간
                    , bii.INPUT_LOCATION                                                        -- 투입장소
                    , sdl.DESCRIPTION AS LOCATION_DESC                                          -- 투입장소
                    , (bii.INPUT_EA * i.SM_INPUT) INPUT_QTY                                     -- 투입량
                    , bii.WEIGHT                                                                -- 중량
                    , bii.CAR_NO                                                                -- 차량번호
                    , bii.B_W                                                                   -- 비중
                    , bii.REMARKS                                                               -- 비고
                    , bii.I_USER                                                                -- 근무자
                    , d.NAME
                    , bii.INGRED_GUBUN
                FROM BIN_INGRED_INPUT bii
                    JOIN INGRED i ON i.PLANT_CODE = bii.PLANT_CODE AND NVL(i.MS_GUBUN, 'M') = 'S' AND i.RESOURCE_NO = bii.RESOURCE_NO 
                    LEFT JOIN SAP_DI_PRODUCT p ON p.PLANT_CODE = bii.PLANT_CODE AND p.RESOURCE_NO = bii.RESOURCE_NO
                    LEFT JOIN SAP_DI_LOCATION sdl ON sdl.PLANT_CODE = bii.PLANT_CODE AND sdl.LOCATION = bii.INPUT_LOCATION
                    LEFT JOIN DO_INSA d ON d.EMPLOYEE_NO = bii.I_USER
                WHERE bii.PLANT_CODE = '{clsCommon.PlantCode}'
                    AND bii.INPUT_DATE = '{Convert.ToDateTime(dtWork.EditValue).ToString("yyyyMMdd")}'
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

        private void frm_SubIngredWorkLog_Load(object sender, EventArgs e)
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
                clsLog.logSave(this, "frm_SubIngredWorkLog_Load", ex);
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
                        /* 부원료투입공정 작업일지 */
                        INSERT INTO BIN_INGRED_INPUT(
                               PLANT_CODE
                             , SEQ
                             , INPUT_DATE
                             , INPUT_QTY
                             , ERP_UP_YN
                             , I_TIME
                             , I_USER
                             , LOCATION
                             , RESOURCE_NO
                             , INPUT_DTTM
                             , INPUT_LOCATION
                             , REMARKS
                             , INPUT_USER
                             , INGRED_GUBUN
                             , B_W
                             , WEIGHT
                             , CAR_NO
                        )
                        VALUES (
                               '{dr["PLANT_CODE"]}'         /* PLANT_CODE */
                             , '{dr["SEQ"]}'                /* SEQ */
                             , '{dr["INPUT_DATE"]}'         /* INPUT_DATE */
                             , '{dr["INPUT_QTY"]}'          /* INPUT_QTY */
                             , '{dr["ERP_UP_YN"]}'          /* ERP_UP_YN */
                             , SYSDATE                      /* I_TIME */
                             , '{dr["I_USER"]}'             /* I_USER */
                             , '{dr["LOCATION"]}'           /* LOCATION */
                             , '{dr["RESOURCE_NO"]}'        /* RESOURCE_NO */
                             , '{dr["INPUT_DTTM"]}'         /* INPUT_DTTM */
                             , '{dr["INPUT_LOCATION"]}'     /* INPUT_LOCATION */
                             , '{dr["REMARKS"]}'            /* REMARKS */
                             , '{dr["INPUT_USER"]}'         /* INPUT_USER */
                             , '{dr["INGRED_GUBUN"]}'       /* INGRED_GUBUN */
                             , '{dr["B_W"]}'                /* B_W */
                             , '{dr["WEIGHT"]}'             /* WEIGHT */
                             , '{dr["CAR_NO"]}'             /* CAR_NO */
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
                           SET INPUT_DATE      = '{dr["INPUT_DATE"]}'
                             , INPUT_QTY       = '{dr["INPUT_QTY"]}'
                             , ERP_UP_YN       = '{dr["ERP_UP_YN"]}'
                             , I_TIME          = SYSDATE
                             , I_USER          = '{dr["I_USER"]}'
                             , LOCATION        = '{dr["LOCATION"]}'
                             , RESOURCE_NO     = '{dr["RESOURCE_NO"]}'
                             , INPUT_DTTM      = '{dr["INPUT_DTTM"]}'
                             , INPUT_LOCATION  = '{dr["INPUT_LOCATION"]}'
                             , REMARKS         = '{dr["REMARKS"]}'
                             , INPUT_USER      = '{dr["INPUT_USER"]}'
                             , INGRED_GUBUN    = '{dr["INGRED_GUBUN"]}'
                             , B_W             = '{dr["B_W"]}'
                             , WEIGHT          = '{dr["WEIGHT"]}'
                             , CAR_NO          = '{dr["CAR_NO"]}'
                         WHERE PLANT_CODE = '{dr["PLANT_CODE"]}'
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

                clsPrintReport.PrintSubIngredWorkLog(dtWork.EditValue.ToString(), dsMain);
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