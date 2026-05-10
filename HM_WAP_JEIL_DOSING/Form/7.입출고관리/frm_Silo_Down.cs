using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class frm_Silo_Down : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_Silo_Down()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(viewControl, Properties.Settings.Default.FontSize);
        }

        private void frm_Silo_Down_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            dtInTime.EditValue = DateTime.Today;

            InitControl();

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void InitControl()
        {
            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboCust, clsCommon.GetCustomer());

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboResourceNo, clsCommon.GetResource(clsCommon.PlantCode, "", $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 2, true, false, "KG", false, true), "품목을 선택 해주세요.", false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboYn, clsCommon.GetYn(new string[] { "Y", "N" }));

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboOx, clsCommon.GetYn(new string[] { "O", "X" }, new string[] { "O", "X" }));
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT a.SEQ
                    , a.IS_NO                                -- 발급번호
                    , a.CUST_CODE                       -- 거래처코드
                    , a.CAR_FULL_NUM                -- 챠량번호
                    , a.SPCS                                     -- 모선
                    , a.LOCATION                         -- 목적빈
                    , a.RESOURCE_NO                  -- 제품코드
                    , a.SPIV_CAR_WEIGHT          -- 하차량
                    , a.IN_TIME                              -- 하차 시작 시간
                    , a.OUT_TIME                         -- 하차 종료 시간
                    , a.DEL_FLAG                         -- 삭제 여부
                    , a.AUTO_YN                          -- 자동 여부
                    , a.U_TIME                               -- 입력 일시
                    , a.U_USER                               -- 입력 사원
                    , a.BJ                                       -- 비중
                    , a.VINSPECTION                      -- 외관검사(Y,N)
                    , a.MFGDATE                              -- 제조일자
                    , a.EXPDATE                              -- 유효기간
                    , a.REMARKS                              -- 비고
                FROM WAP_SLIO_DOWN a
                WHERE a.IN_TIME >= TO_DATE('{Convert.ToDateTime(dtInTime.EditValue):yyyyMMdd}', 'YYYYMMDD')
                    AND a.IN_TIME < TO_DATE('{Convert.ToDateTime(dtInTime.EditValue).AddDays(1):yyyyMMdd}', 'YYYYMMDD')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, viewControl, ds.Tables[0], false);

                sValid = new string[] { "" };


                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("사일로 정보데이터를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(viewControl);
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

                Dbconn.conn.BeginTransaction();
                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewControl);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewControl.FocusedColumn = viewControl.Columns[rValid]; // 이동할 컬럼명
                        viewControl.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    //input check
                    if (string.IsNullOrEmpty(dr["PTMCD"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("PTMCD", "사일로코드를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("PTMCD"));
                        return;
                    }


                    if (string.IsNullOrEmpty(dr["PTMCDNM"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("PTMCDNM", "사일로명을 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("PTMCDNM"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["WEIGHT"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("WEIGHT", "사일로 무계를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("WEIGHT"));
                        return;
                    }


                    if (Convert.ToInt32(dr["WEIGHT"]) < 1)
                    {
                        dr.SetColumnError("WEIGHT", "0이상을 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("WEIGHT"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["DISPLAY_SEQ"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("DISPLAY_SEQ", "화면표시순서를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("DISPLAY_SEQ"));
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO WAP_SLIO_DOWN(SEQ
                                                , IS_NO
                                                , CUST_CODE
                                                , CAR_FULL_NUM
                                                , SPCS
                                                , LOCATION
                                                , RESOURCE_NO
                                                , SPIV_CAR_WEIGHT
                                                , IN_TIME
                                                , OUT_TIME
                                                , DEL_FLAG
                                                , AUTO_YN
                                                , U_TIME
                                                , U_USER
                                                , BJ
                                                , VINSPECTION
                                                , MFGDATE
                                                , EXPDATE
                                                , REMARKS)
                        VALUES ( '{dr["SEQ"]}'               /* 01 */
                               , '{dr["IS_NO"]}'             /* 02 */
                               , '{dr["CUST_CODE"]}'         /* 03 */
                               , '{dr["CAR_FULL_NUM"]}'      /* 04 */
                               , '{dr["SPCS"]}'              /* 05 */
                               , '{dr["LOCATION"]}'          /* 06 */
                               , '{dr["RESOURCE_NO"]}'       /* 07 */
                               , '{dr["SPIV_CAR_WEIGHT"]}'   /* 08 */
                               , '{dr["IN_TIME"]}'           /* 09 */
                               , '{dr["OUT_TIME"]}'          /* 10 */
                               , '{dr["DEL_FLAG"]}'          /* 11 */
                               , '{dr["AUTO_YN"]}'           /* 12 */
                               , SYSDATE                     /* 13 U_TIME */
                               , '{dr["U_USER"]}'            /* 14 */
                               , '{dr["BJ"]}'                /* 15 */
                               , '{dr["VINSPECTION"]}'       /* 16 */
                               , '{dr["MFGDATE"]}'           /* 17 */
                               , '{dr["EXPDATE"]}'           /* 18 */
                               , '{dr["REMARKS"]}'           /* 19 */
                        )";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE WAP_SLIO_DOWN
                            SET SEQ = '{dr["SEQ"]}'
                                , IS_NO = '{dr["IS_NO"]}'
                                , CUST_CODE = '{dr["CUST_CODE"]}'
                                , CAR_FULL_NUM = '{dr["CAR_FULL_NUM"]}'
                                , SPCS = '{dr["SPCS"]}'
                                , LOCATION = '{dr["LOCATION"]}'
                                , RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                                , SPIV_CAR_WEIGHT = '{dr["SPIV_CAR_WEIGHT"]}'
                                , IN_TIME = '{dr["IN_TIME"]}'
                                , OUT_TIME = '{dr["OUT_TIME"]}'
                                , DEL_FLAG = '{dr["DEL_FLAG"]}'
                                , AUTO_YN = '{dr["AUTO_YN"]}'
                                , U_TIME = SYSDATE
                                , U_USER = '{dr["U_USER"]}'
                                , BJ = '{dr["BJ"]}'
                                , VINSPECTION = '{dr["VINSPECTION"]}'
                                , MFGDATE = '{dr["MFGDATE"]}'
                                , EXPDATE = '{dr["EXPDATE"]}'
                                , REMARKS = '{dr["REMARKS"]}'
                            WHERE SEQ = '{dr["SEQ"]}'
                            AND IS_NO = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {

                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }


                    }

                    dr.AcceptChanges();

                } //foreach

                Dbconn.conn.Commit();

                viewControl.RefreshData();

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

                if (viewControl.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 사일로 정보를 선택하여 주세요");
                    return;
                }

                DataRow row = viewControl.GetDataRow(viewControl.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    viewControl.DeleteRow(viewControl.FocusedRowHandle);
                }
                else
                {
                    DialogResult result = ShowMessageBox.Confirm($"선택하신 사일로 정보 {clsDevexpressGrid.GetFocusedRowDisplayText(viewControl, "IS_NO")} 를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        SQL = $"DELETE FROM WAP_SLIO_DOWN WHERE IS_NO = '{clsDevexpressGrid.GetFocusedRowCellValue(viewControl, "IS_NO")}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                            return;
                        }

                        XMain_Search();

                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridViewAddRow(viewControl);

            viewControl.SetRowCellValue(viewControl.FocusedRowHandle, viewControl.Columns["SEQ"], 1);
            viewControl.SetRowCellValue(viewControl.FocusedRowHandle, viewControl.Columns["IS_NO"], DateTime.Now.ToString("yyyyMMddHHmmsss"));
            viewControl.SetRowCellValue(viewControl.FocusedRowHandle, viewControl.Columns["U_USER"], clsCommon.UserId);

            viewControl.ShowEditor();
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewControl);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewControl);
        }

        private void viewBland_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridBrand_KeyDown(object sender, KeyEventArgs e)
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
            viewControl.FocusedRowHandle = 0;
            viewControl.FocusedColumn = viewControl.VisibleColumns[0];
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}