using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;
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
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraLayout;
using DevExpress.XtraSplashScreen;
using Core.Class;
using Core.Extension;
using Core.Enum;
using Core;

namespace HARIM_FA_DOSING
{
    public partial class frm_BagReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;
        DataSet authDs;

        public frm_BagReport()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void resetControl()
        {
            foreach (var item in layoutControl.Items)
            {
                if (item is LayoutControlItem && (item as LayoutControlItem).Control is TextEdit)
                {
                    if (typeof(DevExpress.XtraEditors.TextEdit) == (item as LayoutControlItem).Control.GetType() ||
                        typeof(DevExpress.XtraEditors.MemoEdit) == (item as LayoutControlItem).Control.GetType()
                        )
                    {
                        (item as LayoutControlItem).Control.Text = string.Empty;
                    }

                }
            }
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_BagReport_Load(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

                authDs = clsSql.GetAuthDataSet(this.Name);

                dateEdit_workDate.EditValue = DateTime.Today;

                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                // 라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("타이콘")), "", false, 0, false);

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_BagReport_Load", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                   PLANT_CODE, PROCESS_KEY, L_CODE , 
                   WORK_NUMBER, WORK_SEQ, RESOURCE_NO, 
                   LOCATION_ST, BAG_NUMBER, RUN_ST, 
                   RUN_ET, RUN_TOTAL, WAIT_TIME, 
                   OR_QTY, PRO_QTY1, PRO_QTY2, 
                   USE_END_QTY, END_QTY, F_Q, 
                   E_Q, TOTAL_Q, SAMPLPE_TLY, 
                   I_TIME, I_USER
                FROM BAG_REPORT
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND PROCESS_KEY = '{clsCommon.GetProcessKey("타이콘")}'
                    AND L_CODE = '{cboL_Code.EditValue}'
                    AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' 
                ORDER BY WORK_NUMBER
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], false);

                sValid = new string[] { "" };


                repItemLkUpEdit_RESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                repItemLkUpEdit_RESOURCE_NO.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSuggest;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_RESOURCE_NO, clsCommon. GetResource(cboPlant_Code.EditValue?.ToString(), $"'{clsCommon.GetResourceTypeCode("제품")}', '{clsCommon.GetResourceTypeCode("반제품")}', '{clsCommon.GetResourceTypeCode("상품")}'"));
                repItemLkUpEdit_RESOURCE_NO.PopupFormMinSize = new Size(500, 600);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("타이콘")));

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboBin, clsCommon.GetBin(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("타이콘"), cboL_Code.EditValue?.ToString()));

                resetControl();

                SQL = $@"
                SELECT 
                   PLANT_CODE, WORK_NUMBER, PROCESS_KEY, 
                   L_CODE, REAL_CHK1, REAL_CHK2, 
                   REAL_CHK3, MAG_CLEAN_1_1, MAG_CLEAN_1_2, 
                   MAG_CLEAN_2_1, MAG_CLEAN_2_2, MAG_CLEAN_3_1, 
                   MAG_CLEAN_3_2, MAG_CLEAN_4_1, MAG_CLEAN_4_2, 
                   MAG_CLEAN_5_1, MAG_CLEAN_5_2, MAG_CLEAN_6_1, 
                   MAG_CLEAN_6_2, MAG_CLEAN_7_1, MAG_CLEAN_7_2, 
                   MAG_CLEAN_8, BIN_CHK_1_1, BIN_CHK_1_2, 
                   BIN_CHK_1_3, BIN_CHK_1_4, BIN_CHK_2_1, 
                   BIN_CHK_2_2, BIN_CHK_2_3, BIN_CHK_2_4, 
                   BIN_CHK_3_1, BIN_CHK_3_2, BIN_CHK_3_3, 
                   BIN_CHK_3_4, DUCK_CHK_TIME1, DUCK_CHK_KG1, 
                   DUCK_CHK_TIME2, DUCK_CHK_KG2, DUCK_CHK_TIME3, 
                   DUCK_CHK_KG3, DUCK_CHK_TIME4, DUCK_CHK_KG4, 
                   REMARK1, REMARK2, I_TIME, 
                   I_USER
                FROM BAG_CHK_LIST
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND PROCESS_KEY = '{cboProcessKey.EditValue}'
                    AND L_CODE = '{cboL_Code.EditValue}'
                    AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' 
                ";

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) == 1)
                {
                    txt_REAL_CHK1.Text = Dbconn.conn.getData(chkListDs, "REAL_CHK1", 0);
                    txt_REAL_CHK2.Text = Dbconn.conn.getData(chkListDs, "REAL_CHK2", 0);
                    txt_REAL_CHK3.Text = Dbconn.conn.getData(chkListDs, "REAL_CHK3", 0);
                    txt_MAG_CLEAN_1_1.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_1_1", 0);
                    txt_MAG_CLEAN_1_2.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_1_2", 0);
                    txt_MAG_CLEAN_2_1.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_2_1", 0);
                    txt_MAG_CLEAN_2_2.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_2_2", 0);
                    txt_MAG_CLEAN_3_1.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_3_1", 0);
                    txt_MAG_CLEAN_3_2.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_3_2", 0);
                    txt_MAG_CLEAN_4_1.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_4_1", 0);
                    txt_MAG_CLEAN_4_2.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_4_2", 0);
                    txt_MAG_CLEAN_5_1.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_5_1", 0);
                    txt_MAG_CLEAN_5_2.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_5_2", 0);
                    txt_MAG_CLEAN_6_1.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_6_1", 0);
                    txt_MAG_CLEAN_6_2.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_6_2", 0);
                    txt_MAG_CLEAN_7_1.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_7_1", 0);
                    txt_MAG_CLEAN_7_2.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_7_2", 0);
                    txt_MAG_CLEAN_8.Text = Dbconn.conn.getData(chkListDs, "MAG_CLEAN_8", 0);

                    txt_BIN_CHK_1_1.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_1_1", 0);
                    txt_BIN_CHK_1_2.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_1_2", 0);
                    txt_BIN_CHK_1_3.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_1_3", 0);
                    txt_BIN_CHK_1_4.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_1_4", 0);
                    txt_BIN_CHK_2_1.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_2_1", 0);
                    txt_BIN_CHK_2_2.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_2_2", 0);
                    txt_BIN_CHK_2_3.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_2_3", 0);
                    txt_BIN_CHK_2_4.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_2_4", 0);
                    txt_BIN_CHK_3_1.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_3_1", 0);
                    txt_BIN_CHK_3_2.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_3_2", 0);
                    txt_BIN_CHK_3_3.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_3_3", 0);
                    txt_BIN_CHK_3_4.Text = Dbconn.conn.getData(chkListDs, "BIN_CHK_3_4", 0);

                    txt_DUCK_CHK_TIME1.Text = Dbconn.conn.getData(chkListDs, "DUCK_CHK_TIME1", 0);
                    txt_DUCK_CHK_TIME2.Text = Dbconn.conn.getData(chkListDs, "DUCK_CHK_TIME2", 0);
                    txt_DUCK_CHK_TIME3.Text = Dbconn.conn.getData(chkListDs, "DUCK_CHK_TIME3", 0);
                    txt_DUCK_CHK_TIME4.Text = Dbconn.conn.getData(chkListDs, "DUCK_CHK_TIME4", 0);
                    txt_DUCK_CHK_KG1.Text = Dbconn.conn.getData(chkListDs, "DUCK_CHK_KG1", 0);
                    txt_DUCK_CHK_KG2.Text = Dbconn.conn.getData(chkListDs, "DUCK_CHK_KG2", 0);
                    txt_DUCK_CHK_KG3.Text = Dbconn.conn.getData(chkListDs, "DUCK_CHK_KG3", 0);
                    txt_DUCK_CHK_KG4.Text = Dbconn.conn.getData(chkListDs, "DUCK_CHK_KG4", 0);

                    memoEdit_REMARK1.Text = Dbconn.conn.getData(chkListDs, "REMARK1", 0);
                    memoEdit_REMARK2.Text = Dbconn.conn.getData(chkListDs, "REMARK2", 0);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        
        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void layoutControl_CustomDraw(object sender, DevExpress.XtraLayout.ItemCustomDrawEventArgs e)
        {
            try
            {
                if (e.Item.Tag != null && e.Item.Tag.ToString() == "LINE")
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

        private void btn_loadData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = ShowMessageBox.Confirm("타이콘 작업내역을 불러오시겠습니까?", "기존에 입력된 내역은 전부 삭제되고 타이콘 작업 데이터로 새로 입력됩니다");
                if (result != DialogResult.Yes)
                {
                    return;
                }


                Dbconn.conn.BeginTransaction();

                SQL = $@"
                DELETE FROM BAG_REPORT
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND PROCESS_KEY = '{clsCommon.GetProcessKey("타이콘")}' AND L_CODE = '{cboL_Code.EditValue}'
                AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'";
                
                Dbconn.conn.SQLrun(SQL);


                SQL = $@"
                INSERT INTO BAG_REPORT (
                   PLANT_CODE, PROCESS_KEY, L_CODE , 
                   WORK_NUMBER, WORK_SEQ, RESOURCE_NO, 
                   LOCATION_ST, RUN_ST, RUN_ET, RUN_TOTAL,  
                   OR_QTY, PRO_QTY1,
                   USE_END_QTY, END_QTY, F_Q, 
                   E_Q, SAMPLPE_TLY, 
                   I_TIME, I_USER) 
                SELECT 
                PLANT_CODE, PROCESS_KEY, L_CODE , 
                   WORKDATE, WORK_SEQ, RESOURCE_NO,  
                   LOCATION, RUN_ST, RUN_ET, (RUN_ET - RUN_ST) * 24 * 60,
                   (OR_QTY/1000), PRO_QTY,
                   USE_END_QTY, END_QTY, F_Q, 
                   E_Q, SAMPLE_TLY, 
                   SYSDATE, '{clsCommon.UserId}'
                FROM BAG_ORDER
                WHERE WORKDATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                    AND NVL(DEL_FLAG,'N') != 'Y'
                    AND C_CONDITION = '{clsCommon.PcStatus.Completed}'
                ORDER BY WORKDATE, WORK_SEQ
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("내역을 불러오기가 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowInformation("타이콘 작업정보 불러오기가 완료되었습니다");
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_loadData_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
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
                if (gridView.GetSelectedRows().Length == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 데이터를 체크하여주세요");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 타이콘일지 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in gridView.GetSelectedRows())
                {
                    DataRow sel_row = gridView.GetDataRow(id);

                    string work_num = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                    string work_seq = sel_row["WORK_SEQ"].ToString().Trim();

                    if (sel_row.RowState != DataRowState.Added)
                    {
                        SQL = $@"
                        DELETE FROM BAG_REPORT
                        WHERE  PLANT_CODE = '{cboPlant_Code.EditValue}' AND PROCESS_KEY = '{clsCommon.GetProcessKey("타이콘")}' AND L_CODE = '{cboL_Code.EditValue}'
                        AND WORK_NUMBER = '{work_num}' AND WORK_SEQ = '{work_seq}'";
                        
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

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboPlant_Code.EditValue?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                    cboPlant_Code.Focus();
                    clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
                    return;
                }

                if (cboL_Code.EditValue?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("공정 라인을 먼저 선택 조회 해주세요.");
                    cboL_Code.Focus();
                    return;
                }

                clsDevexpressGrid.GridViewAddRow(gridView);
                gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
                gridView.SetFocusedRowCellValue("PROCESS_KEY", clsCommon.GetProcessKey("타이콘"));
                gridView.SetFocusedRowCellValue("L_CODE", clsCommon.GetProcessKey("타이콘").Merge(cboL_Code.EditValue?.ToString()));
                gridView.SetFocusedRowCellValue("WORK_NUMBER", DateTime.Now.ToString("yyyyMMdd"));

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["RUN_ST"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["RUN_ET"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
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

            if (DialogResult.Yes != ShowMessageBox.Confirm("타이콘 작업일지 정보를 저장하시겠습니까?"))
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

                Dbconn.conn.BeginTransaction();

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

                    //input check
                    if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                    {
                        Dbconn.conn.Rollback();
                        dr.SetColumnError("RESOURCE_NO", "포장제품정보를 선택하여 주세요");
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
                        SQL = $@"
                        INSERT INTO BAG_REPORT (
                           PLANT_CODE, PROCESS_KEY, L_CODE , 
                           WORK_NUMBER, WORK_SEQ, RESOURCE_NO, 
                           LOCATION_ST, BAG_NUMBER, RUN_ST, 
                           RUN_ET, RUN_TOTAL, WAIT_TIME, 
                           OR_QTY, PRO_QTY1, PRO_QTY2, 
                           USE_END_QTY, END_QTY, F_Q, 
                           E_Q, TOTAL_Q, SAMPLPE_TLY, 
                           I_TIME, I_USER) 
                        VALUES (
                           '{dr["PLANT_CODE"]}'
                         , '{dr["PROCESS_KEY"]}'
                         , '{dr["L_CODE"]}'
                         , '{dr["WORK_NUMBER"]}'
                         , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) FROM BAG_REPORT WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}' AND WORK_NUMBER = '{dr["WORK_NUMBER"]}')
                         , '{dr["RESOURCE_NO"]}'
                         , '{dr["LOCATION_ST"]}'
                         , '{dr["BAG_NUMBER"]}'
                         , TO_DATE('{Convert.ToDateTime(dr["RUN_ST"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                         , TO_DATE('{Convert.ToDateTime(dr["RUN_ET"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                         , '{dr["RUN_TOTAL"]}'
                         , '{dr["WAIT_TIME"]}'
                         , '{dr["OR_QTY"]}'
                         , '{dr["PRO_QTY1"]}'
                         , '{dr["PRO_QTY2"]}'
                         , '{dr["USE_END_QTY"]}'
                         , '{dr["END_QTY"]}'
                         , '{dr["F_Q"]}'
                         , '{dr["E_Q"]}'
                         , '{dr["TOTAL_Q"]}'
                         , '{dr["SAMPLPE_TLY"]}'
                         , SYSDATE
                         , '{clsCommon._strUserId}'
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE BAG_REPORT
                        SET    RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                             , LOCATION_ST = '{dr["LOCATION_ST"]}'
                             , BAG_NUMBER  = '{dr["BAG_NUMBER"]}'
                             , RUN_ST      = '{Convert.ToDateTime(dr["RUN_ST"]).ToString("yyyy-MM-dd HH:mm:ss")}'
                             , RUN_ET      = '{Convert.ToDateTime(dr["RUN_ET"]).ToString("yyyy-MM-dd HH:mm:ss")}'
                             , RUN_TOTAL   = '{dr["RUN_TOTAL"]}'
                             , WAIT_TIME   = '{dr["WAIT_TIME"]}'
                             , OR_QTY      = '{dr["OR_QTY"]}'
                             , PRO_QTY1    = '{dr["PRO_QTY1"]}'
                             , PRO_QTY2    = '{dr["PRO_QTY2"]}'
                             , USE_END_QTY = '{dr["USE_END_QTY"]}'
                             , END_QTY     = '{dr["END_QTY"]}'
                             , F_Q         = '{dr["F_Q"]}'
                             , E_Q         = '{dr["E_Q"]}'
                             , TOTAL_Q     = '{dr["TOTAL_Q"]}'
                             , SAMPLPE_TLY = '{dr["SAMPLPE_TLY"]}'
                             , I_TIME      = SYSDATE
                             , I_USER      = '{clsCommon._strUserId}'
                        WHERE  PLANT_CODE  = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE      = '{dr["L_CODE"]}'
                        AND    WORK_NUMBER = '{dr["WORK_NUMBER"]}'
                        AND    WORK_SEQ    = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                SQL = $@"
                DELETE FROM BAG_CHK_LIST
                WHERE PLANT_CODE  = '{cboPlant_Code.EditValue}'
                    AND PROCESS_KEY = '{cboProcessKey.EditValue}'
                    AND L_CODE = '{cboL_Code.EditValue}'
                    AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'";

                Dbconn.conn.SQLrun(SQL);

                SQL = $@"
                INSERT INTO BAG_CHK_LIST (
                   PLANT_CODE, WORK_NUMBER, PROCESS_KEY, L_CODE ,
                   REAL_CHK1, REAL_CHK2, REAL_CHK3,
                   MAG_CLEAN_1_1, MAG_CLEAN_1_2, 
                   MAG_CLEAN_2_1, MAG_CLEAN_2_2, MAG_CLEAN_3_1, 
                   MAG_CLEAN_3_2, MAG_CLEAN_4_1, MAG_CLEAN_4_2, 
                   MAG_CLEAN_5_1, MAG_CLEAN_5_2, MAG_CLEAN_6_1, 
                   MAG_CLEAN_6_2, MAG_CLEAN_7_1, MAG_CLEAN_7_2, 
                   MAG_CLEAN_8, BIN_CHK_1_1, BIN_CHK_1_2, 
                   BIN_CHK_1_3, BIN_CHK_1_4, BIN_CHK_2_1, 
                   BIN_CHK_2_2, BIN_CHK_2_3, BIN_CHK_2_4, 
                   BIN_CHK_3_1, BIN_CHK_3_2, BIN_CHK_3_3, 
                   BIN_CHK_3_4, DUCK_CHK_TIME1, DUCK_CHK_KG1, 
                   DUCK_CHK_TIME2, DUCK_CHK_KG2, DUCK_CHK_TIME3, 
                   DUCK_CHK_KG3, DUCK_CHK_TIME4, DUCK_CHK_KG4, 
                   REMARK1, REMARK2, I_TIME, 
                   I_USER) 
                VALUES ( 
                   '{cboPlant_Code.EditValue}', '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', '{clsCommon.GetProcessKey("타이콘")}', '{cboL_Code.EditValue}'
                 , '{txt_REAL_CHK1.Text}'
                 , '{txt_REAL_CHK2.Text}'
                 , '{txt_REAL_CHK3.Text}'
                 , '{txt_MAG_CLEAN_1_1.Text}'
                 , '{txt_MAG_CLEAN_1_2.Text}'
                 , '{txt_MAG_CLEAN_2_1.Text}'
                 , '{txt_MAG_CLEAN_2_2.Text}'
                 , '{txt_MAG_CLEAN_3_1.Text}'
                 , '{txt_MAG_CLEAN_3_2.Text}'
                 , '{txt_MAG_CLEAN_4_1.Text}'
                 , '{txt_MAG_CLEAN_4_2.Text}'
                 , '{txt_MAG_CLEAN_5_1.Text}'
                 , '{txt_MAG_CLEAN_5_2.Text}'
                 , '{txt_MAG_CLEAN_6_1.Text}'
                 , '{txt_MAG_CLEAN_6_2.Text}'
                 , '{txt_MAG_CLEAN_7_1.Text}'
                 , '{txt_MAG_CLEAN_7_2.Text}'
                 , '{txt_MAG_CLEAN_8.Text}'
                 , '{txt_BIN_CHK_1_1.Text}'
                 , '{txt_BIN_CHK_1_2.Text}'
                 , '{txt_BIN_CHK_1_3.Text}'
                 , '{txt_BIN_CHK_1_4.Text}'
                 , '{txt_BIN_CHK_2_1.Text}'
                 , '{txt_BIN_CHK_2_2.Text}'
                 , '{txt_BIN_CHK_2_3.Text}'
                 , '{txt_BIN_CHK_2_4.Text}'
                 , '{txt_BIN_CHK_3_1.Text}'
                 , '{txt_BIN_CHK_3_2.Text}'
                 , '{txt_BIN_CHK_3_3.Text}'
                 , '{txt_BIN_CHK_3_4.Text}'
                 , '{txt_DUCK_CHK_TIME1.Text}'
                 , '{txt_DUCK_CHK_KG1.Text}'
                 , '{txt_DUCK_CHK_TIME2.Text}'
                 , '{txt_DUCK_CHK_KG2.Text}'
                 , '{txt_DUCK_CHK_TIME3.Text}'
                 , '{txt_DUCK_CHK_KG3.Text}'
                 , '{txt_DUCK_CHK_TIME4.Text}'
                 , '{txt_DUCK_CHK_KG4.Text}'
                 , '{memoEdit_REMARK1.Text}'
                 , '{memoEdit_REMARK2.Text}'
                 , SYSDATE
                 , '{clsCommon._strUserId}' )
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("BAG_CHK_LIST 저장에 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                XMain_Search();

                ShowMessageBox.XtraShowInformation("저장되었습니다");

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                clsLog.logSave(this, "btn_save_Click", SQL);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string workDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);
                clsPrintReport.PrintBagReport(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString(), cboL_Code.EditValue?.ToString(), workDate);
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

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // 공정
                clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);
                cboProcessKey.EditValue = clsCommon.GetProcessKey("타이콘");

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboProcessKey_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridControl.DataSource = null;

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }
    }
}