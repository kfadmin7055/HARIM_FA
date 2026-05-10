using Core.Class;
using DevExpress.XtraEditors;
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
using System.Windows.Forms;

namespace HM_WAP_JEIL_DOSING
{
    public partial class frm_PackReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        public frm_PackReport()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(bandedGridView);
        }

        private string workNumMake(string WORK_START_DATE, string sbno)
        {
            try
            {
                string SQL =
                "SELECT ISNULL(MAX(WORK_SEQ) + 1, 1) AS SEQ  " +
                "FROM PACK_REPORT WHERE WORK_NUMBER = '{0}'";
                SQL = string.Format(SQL, WORK_START_DATE, sbno);

                using (DataSet Ds = Dbconn.conn.ExecutDataset(SQL))
                {
                    if (Dbconn.conn.getRowCnt(Ds) == 0)
                    {
                        clsLog.logSave("작업순번 생성에러 / SQL : " + SQL, 0);
                        return string.Empty;
                    }

                    string return_seq = Dbconn.conn.getData(Ds, "SEQ", 0);
                    Ds.Dispose();

                    return return_seq;
                }

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "workNumMake", ex);
                return string.Empty;
            }
        }

        private void XMain_Search()
        {
            try
            {
                repItemLkUpEdit_RESOURCE_NO.NullText = "";
                repItemLkUpEdit_RESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                repItemLkUpEdit_RESOURCE_NO.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSuggest;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_RESOURCE_NO, clsCommon.GetResource("'S','F','G'"));
                repItemLkUpEdit_RESOURCE_NO.PopupFormMinSize = new Size(500, 600);


                SQL = $@"   
                SELECT
                    PLANT_CODE, PROCESS_KEY, L_CODE, 
                    a.WORK_NUMBER,
                    a.WORK_SEQ,
                    a.SBNO,
                    a.RESOURCE_NO,
                    b.DESCRIPTION,
                    a.RUN_ST,
                    a.RUN_ET,
                    (CAST(a.RUN_ET AS DATE) - CAST(a.RUN_ST AS DATE)) * 24 * 60 AS RUN_TOTAL,
                    a.LOCATION_ST,
                    a.PRO_Q,
                    a.OR_QTY,
                    a.PRO_QTY,
                    a.F_Q,
                    a.E_Q,
                    a.PA_Q,
                    a.DIFF,
                    a.SAMPLE_TLY,
                    a.DAN1,
                    a.DAN2,
                    a.DAN3,
                    a.WORK_CHK
                FROM PACK_REPORT a
                LEFT JOIN SAP_DI_PRODUCT b
                    ON a.RESOURCE_NO = b.RESOURCE_NO
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}
                    AND a.WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                    AND a.SBNO = '{lookUpEdit_sbno.EditValue}'
                ORDER BY a.WORK_NUMBER, a.WORK_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, bandedGridView, ds.Tables[0], false);


                txt_CHK_MEMO1.Text = string.Empty;
                txt_CHK_MEMO2.Text = string.Empty;
                txt_CHK_MEMO3.Text = string.Empty;
                txt_CHK_MEMO4.Text = string.Empty;
                memoEdit_REMARK1.Text = string.Empty;
                memoEdit_REMARK2.Text = string.Empty;

                SQL =
                "SELECT CHK_LIST1, CHK_LIST2, CHK_LIST3, CHK_LIST4, REMARK1, REMARK2, I_TIME, I_USER  " +
                "FROM PACK_CHK_LIST " +
                "WHERE WORK_NUMBER = '{0}' AND SBNO = '{1}' ";
                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),
                    lookUpEdit_sbno.EditValue.ToString()
                    );

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) == 1)
                {
                    txt_CHK_MEMO1.Text = Dbconn.conn.getData(chkListDs, "CHK_LIST1", 0);
                    txt_CHK_MEMO2.Text = Dbconn.conn.getData(chkListDs, "CHK_LIST2", 0);
                    txt_CHK_MEMO3.Text = Dbconn.conn.getData(chkListDs, "CHK_LIST3", 0);
                    txt_CHK_MEMO4.Text = Dbconn.conn.getData(chkListDs, "CHK_LIST4", 0);
                    memoEdit_REMARK1.Text = Dbconn.conn.getData(chkListDs, "REMARK1", 0);
                    memoEdit_REMARK2.Text = Dbconn.conn.getData(chkListDs, "REMARK2", 0);

                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생하였습니다");
            }
        }


        private void frm_PackReport_Load(object sender, EventArgs e)
        {
            try
            {
                //플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant());

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                DataTable gubun_dt = new DataTable();
                gubun_dt.Columns.AddRange(new DataColumn[] {

                    new DataColumn("NAME"),
                    new DataColumn("CODE"),

                });

                gubun_dt.Rows.Add("포장공정", "1");
                gubun_dt.Rows.Add("대용유포장공정", "2");
                lookUpEdit_sbno.Properties.NullText = "";
                lookUpEdit_sbno.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressUtil.ItemLookUpEditSetup(lookUpEdit_sbno, gubun_dt);

                lookUpEdit_sbno.EditValue = "1";

                //작업조회
                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_PackReport_Load", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void layoutControl_CustomDraw(object sender, DevExpress.XtraLayout.ItemCustomDrawEventArgs e)
        {
            try
            {
                if (e.Item.Tag == "LINE")
                {
                    e.DefaultDraw();
                    Pen pen = new Pen(Brushes.DarkGray, 1);
                    e.Cache.DrawRectangle(pen, e.Bounds);
                    pen.Dispose();
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "layoutControl_CustomDraw", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string workDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                string sbno = lookUpEdit_sbno.EditValue.ToString();
                clsPrintReport.PrintPackReport(workDate, sbno);
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
        private void btn_loadData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = ShowMessageBox.Confirm("포장작업내역을 불러오시겠습니까?", "기존에 입력된 내역은 전부 삭제되고 포장작업 데이터로 새로 입력됩니다");
                if (result != DialogResult.Yes)
                {
                    return;
                }


                Dbconn.conn.BeginTransaction();

                SQL = "DELETE FROM PACK_REPORT WHERE WORK_NUMBER = '{0}' AND SBNO = '{1}' ";


                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), 
                    lookUpEdit_sbno.EditValue.ToString()
                    );

                Dbconn.conn.SQLrun(SQL);


                SQL = "DELETE FROM PACK_CHK_LIST WHERE WORK_NUMBER = '{0}' AND SBNO = '{1}' ";
                SQL = string.Format(SQL,
                        string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),
                        lookUpEdit_sbno.EditValue.ToString()
                        );
                Dbconn.conn.SQLrun(SQL);


                SQL =
                    "INSERT INTO PACK_REPORT " +
                    "(WORK_NUMBER, WORK_SEQ, SBNO, RESOURCE_NO, RUN_ST, RUN_ET, LOCATION_ST, PRO_Q, OR_QTY, PRO_QTY, F_Q, E_Q, PA_Q, DIFF, SAMPLE_TLY, DAN1, DAN2, DAN3, I_TIME, I_USER)  " +
                    "SELECT '{0}',  " +
                    "ROW_NUMBER() OVER(ORDER BY PO.WORK_SEQ),  " +
                    "PO.SBNO, " +
                    "PO.RESOURCE_NO, PO.RUN_ST, PO.RUN_ET, PO.LOCATION_ST, ERP_NOTE.QTY_PCT * PO.PRO_QTY, PO.OR_QTY, PO.PRO_QTY, PO.F_Q, PO.E_Q, PO.PA_Q, " +
                    "PO.DIFF, PO.SAMPLE_TLY, PO.DAN1, PO.DAN2, PO.DAN3, GETDATE(), '{2}' " +
                    "FROM PACK_ORDER PO LEFT OUTER JOIN (  " +
                    "select * From  ERP_DBLINK.ATL_MFG.DBO.V_MES_ATG_101_3  ERP_NOTE  " +
                    "  where SUBSTRING(ISNULL(ERP_NOTE.RESOURCE_USED,1),1,1) = '1'  " +
                    "and RIGHT(ERP_NOTE.RESOURCE_NO,1) = '1' " +
                    ") ERP_NOTE  " +
                    "ON  PO.RESOURCE_NO = ERP_NOTE.RESOURCE_NO " +
                    "where PO.WORKDATE = '{0}' " +
                    "AND PO.SBNO = '{1}'  AND ISNULL(PO.DEL_FLAG,'N') <> 'D' " +
                    $"AND PO.C_CONDITION = '{clsCommon.GetCOMM_DTNAME("03", "10", "완료")}' " +
                    "ORDER BY PO.WORKDATE, PO.WORK_SEQ ";

                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),
                    lookUpEdit_sbno.EditValue.ToString(),
                    clsCommon._strUserId
                    );


                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("내역을 불러오기가 실패하였습니다");
                    return;
                }

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowInformation("포장작업정보 불러오기가 완료되었습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_loadData_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.GridViewAddRow(bandedGridView);

                bandedGridView.SetRowCellValue(bandedGridView.FocusedRowHandle, bandedGridView.Columns["RUN_ST"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                bandedGridView.SetRowCellValue(bandedGridView.FocusedRowHandle, bandedGridView.Columns["RUN_ET"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
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
                clsDevexpressGrid.GridEndEdit(bandedGridView);
                clsDevexpressGrid.GridViewLastAddRowDelete(bandedGridView);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowDel_Click", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != ShowMessageBox.Confirm("포장 작업일지 정보를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(bandedGridView);
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
                    dr.ClearErrors();

                    //input check
                    if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("RESOURCE_NO", "하차원료정보를 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["LOCATION_ST"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("LOCATION_ST", "인출빈 정보를 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION"));
                        return;
                    }


                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL =
                            "INSERT INTO PACK_REPORT " +
                            "(WORK_NUMBER, WORK_SEQ, SBNO, RESOURCE_NO, RUN_ST, RUN_ET, LOCATION_ST, PRO_Q, OR_QTY, PRO_QTY, " +
                            "F_Q, E_Q, PA_Q, DIFF, SAMPLE_TLY, DAN1, DAN2, DAN3, WORK_CHK, I_TIME, I_USER)  " +
                            "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', " +
                            "'{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', getdate(), '{19}') ";

                        SQL = string.Format(SQL,
                            string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),  //1
                            workNumMake(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), lookUpEdit_sbno.EditValue.ToString()),
                            lookUpEdit_sbno.EditValue.ToString(),
                            dr["RESOURCE_NO"].ToString(),
                            Convert.ToDateTime(dr["RUN_ST"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            Convert.ToDateTime(dr["RUN_ET"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            dr["LOCATION_ST"].ToString(),
                            dr["PRO_Q"].ToString(),
                            dr["OR_QTY"].ToString(),
                            dr["PRO_QTY"].ToString(), //10
                            dr["F_Q"].ToString(),  //11
                            dr["E_Q"].ToString(),
                            dr["PA_Q"].ToString(),
                            dr["DIFF"].ToString(),
                            dr["SAMPLE_TLY"].ToString(),
                            dr["DAN1"].ToString(),
                            dr["DAN2"].ToString(),
                            dr["DAN3"].ToString(),
                            dr["WORK_CHK"].ToString(),
                            clsCommon._strUserId
                            );

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패하였습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL =
                            "UPDATE PACK_REPORT  " +
                            "SET RESOURCE_NO = '{3}', RUN_ST = '{4}', RUN_ET = '{5}', LOCATION_ST = '{6}', PRO_Q = '{7}', OR_QTY = '{8}', PRO_QTY = '{9}', " +
                            "F_Q = '{10}', E_Q = '{11}', PA_Q = '{12}', DIFF = '{13}', SAMPLE_TLY = '{14}', DAN1 = '{15}', DAN2 = '{16}', DAN3 = '{17}', " +
                            "WORK_CHK = '{18}', I_TIME = getdate(), I_USER = '{19}'  " +
                            "WHERE WORK_NUMBER = '{0}'  AND WORK_SEQ = '{1}' AND SBNO = '{2}'  ";

                        SQL = string.Format(SQL,
                            dr["WORK_NUMBER"].ToString(),  //1
                            dr["WORK_SEQ"].ToString(),
                            lookUpEdit_sbno.EditValue.ToString(),
                            dr["RESOURCE_NO"].ToString(),
                            Convert.ToDateTime(dr["RUN_ST"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            Convert.ToDateTime(dr["RUN_ET"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            dr["LOCATION_ST"].ToString(),
                            dr["PRO_Q"].ToString(),
                            dr["OR_QTY"].ToString(),
                            dr["PRO_QTY"].ToString(), //10
                            dr["F_Q"].ToString(),  //11
                            dr["E_Q"].ToString(),
                            dr["PA_Q"].ToString(),
                            dr["DIFF"].ToString(),
                            dr["SAMPLE_TLY"].ToString(),
                            dr["DAN1"].ToString(),
                            dr["DAN2"].ToString(),
                            dr["DAN3"].ToString(),
                            dr["WORK_CHK"].ToString(),
                            clsCommon._strUserId  //20
                            );

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패하였습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                SQL = "DELETE FROM PACK_CHK_LIST WHERE WORK_NUMBER = '{0}' AND SBNO = '{1}' ";
                SQL = string.Format(SQL,
                        string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),
                        lookUpEdit_sbno.EditValue.ToString()
                        );
                Dbconn.conn.SQLrun(SQL);

                SQL =
                   "INSERT INTO PACK_CHK_LIST " +
                   "(WORK_NUMBER, SBNO, CHK_LIST1, CHK_LIST2, CHK_LIST3, CHK_LIST4, REMARK1, REMARK2, I_TIME, I_USER)  " +
                   "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', getdate(), '{8}') ";

                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), //1
                    lookUpEdit_sbno.EditValue.ToString(),
                    txt_CHK_MEMO1.Text,
                    txt_CHK_MEMO2.Text,
                    txt_CHK_MEMO3.Text,
                    txt_CHK_MEMO4.Text, //6
                    memoEdit_REMARK1.Text,
                    memoEdit_REMARK2.Text,
                    clsCommon._strUserId
                    );

                Dbconn.conn.SQLrun(SQL);

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowInformation("저장되었습니다");

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                clsLog.logSave(this, "btn_save_Click", SQL);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생하였습니다");
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
                if (bandedGridView.GetSelectedRows().Length == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 데이터를 체크하여주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 포장일지 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in bandedGridView.GetSelectedRows())
                {
                    DataRow sel_row = bandedGridView.GetDataRow(id);

                    string work_num = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                    string work_seq = sel_row["WORK_SEQ"].ToString().Trim();
                    string sbno = lookUpEdit_sbno.EditValue.ToString();

                    if (sel_row.RowState != DataRowState.Added)
                    {
                        SQL = "DELETE FROM PACK_REPORT WHERE WORK_NUMBER = '{0}' AND WORK_SEQ = '{1}' AND SBNO = '{2}'  ";
                        SQL = string.Format(SQL, work_num, work_seq, sbno);

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 삭제에 실패하였습니다");
                            return;
                        }
                    }
                    else
                    {
                        bandedGridView.DeleteRow(bandedGridView.FocusedRowHandle);
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

        private void bandedGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    e.Info.DisplayText = (1 + e.RowHandle).ToString();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "bandedGridView_CustomDrawRowIndicator", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            if (dateEdit_workDate.EditValue != null && lookUpEdit_sbno.EditValue != null)
            {
                XMain_Search();
            }
        }

        private void lookUpEdit_sbno_EditValueChanged(object sender, EventArgs e)
        {
            if (dateEdit_workDate.EditValue != null && lookUpEdit_sbno.EditValue != null)
            {
                XMain_Search();
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
            bandedGridView.FocusedRowHandle = 0;
            bandedGridView.FocusedColumn = bandedGridView.VisibleColumns[0];
        }
    }
}