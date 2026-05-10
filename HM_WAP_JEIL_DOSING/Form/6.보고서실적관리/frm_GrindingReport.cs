using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.XtraEditors;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid.Views.BandedGrid;
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

namespace HARIM_FA_DOSING
{
    public partial class frm_GrindingReport : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = string.Empty;
        private string[] sValid = null;

        public frm_GrindingReport()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void resetControls()
        { 
            txtCHK_WORK1.Text = string.Empty;
            cboCHK_RESULT1.EditValue = "1";
            dtCHK_CHDAY1.EditValue = DateTime.Today;
            txtCHK_REMARK1.Text = string.Empty;
            txtCHK_WORK2.Text = string.Empty;
            cboCHK_RESULT2.EditValue = "1";
            dtCHK_CHDAY2.EditValue = DateTime.Today;
            txtCHK_REMARK2.Text = string.Empty;
            txtCHK_WORK3.Text = string.Empty;
            cboCHK_RESULT3.EditValue = "1";
            dtCHK_CHDAY3.EditValue = DateTime.Today;
            txtCHK_REMARK3.Text = string.Empty;
            txtCHK_WORK4.Text = string.Empty;
            cboCHK_RESULT4.EditValue = "1";
            dtCHK_CHDAY4.EditValue = DateTime.Today;
            txtCHK_REMARK4.Text = string.Empty;
            txtCHK_WORK5.Text = string.Empty;
            cboCHK_RESULT5.EditValue = "1";
            dtCHK_CHDAY5.EditValue = DateTime.Today;
            txtCHK_REMARK5.Text = string.Empty;
            txtCHK_WORK6.Text = string.Empty;
            cboCHK_RESULT6.EditValue = "1";
            dtCHK_CHDAY6.EditValue = DateTime.Today;
            txtCHK_REMARK6.Text = string.Empty;
            txtCHK_WORK7.Text = string.Empty;
            cboCHK_RESULT7.EditValue = "1";
            dtCHK_CHDAY7.EditValue = DateTime.Today;
            txtCHK_REMARK7.Text = string.Empty;
            txtCHK_WORK8.Text = string.Empty;
            cboCHK_RESULT8.EditValue = "1";
            dtCHK_CHDAY8.EditValue = DateTime.Today;
            txtCHK_REMARK8.Text = string.Empty;
            txtCHK_WORK9.Text = string.Empty;
            cboCHK_RESULT9.EditValue = "1";
            dtCHK_CHDAY9.EditValue = DateTime.Today;
            txtCHK_REMARK9.Text = string.Empty;
            txtCHK_WORK10.Text = string.Empty;
            cboCHK_RESULT10.EditValue = "1";
            dtCHK_CHDAY10.EditValue = DateTime.Today;
            txtCHK_REMARK10.Text = string.Empty;
            txtCHK_WORK11.Text = string.Empty;
            cboCHK_RESULT11.EditValue = "1";
            dtCHK_CHDAY11.EditValue = DateTime.Today;
            txtCHK_REMARK11.Text = string.Empty;
        }

        private void layoutControl_checkList_CustomDraw(object sender, DevExpress.XtraLayout.ItemCustomDrawEventArgs e)
        {
            if (e.Item.Tag?.ToString() == "LINE")
            {
                e.DefaultDraw();
                Pen pen = new Pen(Brushes.DarkGray, 1);
                e.Cache.DrawRectangle(pen, e.Bounds);
                pen.Dispose();
                e.Handled = true;
            }
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 분쇄일지
                SELECT 
                PLANT_CODE, PROCESS_KEY, L_CODE , 
                   WORK_NUMBER, WORK_SEQ, RESOURCE_NO, 
                   ICM_CODE, EMPLOYEE_NO, C_CONDITION, 
                   RUN_ST, RUN_ET, SC_FORE, 
                   SC_BG, SCREEN, FEED_RATE, 
                   LOAD, GR_WV, GR_QTY, 
                   DUST_WV1, DUST_WV2, TH_GR_QTY, 
                   LOCATION_ST1, LOCATION_ST2, LOCATION_ED1, 
                   LOCATION_ED2, PRO_QTY, I_TIME, 
                   REMARK, DEL_FLAG, ERP_UP_YN, 
                   ERP_TNUMBER
                FROM SMASH_REPORT
                WHERE 
                    PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND PROCESS_KEY = '{clsCommon.GetProcessKey("분쇄")}'
                    AND L_CODE = '{cboL_Code.EditValue}'
                    AND WORK_NUMBER BETWEEN '{string.Format("{0:yyyyMMdd}", dt_From_workDate.EditValue)}' AND '{string.Format("{0:yyyyMMdd}", dt_To_workDate.EditValue)}'
                ORDER BY 
                    WORK_NUMBER,
                    WORK_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                sValid = new string[] { "" };

                gridcboRESOURCE_NO.NullText = "";
                gridcboRESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                gridcboRESOURCE_NO.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSuggest;
                gridcboRESOURCE_NO.CaseSensitiveSearch = true;
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("반제품")}','{clsCommon.GetResourceTypeCode("원재료")}'"));

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPROCESS_KEY, clsCommon.GetGridProcess());

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_Code, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("분쇄")), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboICM_CODE, clsCommon.GetICM(), "교대조를 선택 해주세요.", false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboLocation, clsCommon.GetBin(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("분쇄"), cboL_Code.EditValue?.ToString()), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboC_CONDITION, clsCommon.GetPcStatus());

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboEMPLOYEE_NO, clsCommon.GetDO_INSA(cboPlant_Code.EditValue?.ToString()));

                gridcboRESOURCE_NO.PopupFormMinSize = new Size(400, 0);


                resetControls();

                InitControl();

                SQL = $@"
                SELECT 
                PLANT_CODE, WORK_NUMBER, L_CODE, 
                   CHK_WORK1, CHK_RESULT1, CHK_CHDAY1, 
                   CHK_REMARK1, CHK_WORK2, CHK_RESULT2, 
                   CHK_CHDAY2, CHK_REMARK2, CHK_WORK3, 
                   CHK_RESULT3, CHK_CHDAY3, CHK_REMARK3, 
                   CHK_WORK4, CHK_RESULT4, CHK_CHDAY4, 
                   CHK_REMARK4, CHK_WORK5, CHK_RESULT5, 
                   CHK_CHDAY5, CHK_REMARK5, CHK_WORK6, 
                   CHK_RESULT6, CHK_CHDAY6, CHK_REMARK6, 
                   CHK_WORK7, CHK_RESULT7, CHK_CHDAY7, 
                   CHK_REMARK7, CHK_WORK8, CHK_RESULT8, 
                   CHK_CHDAY8, CHK_REMARK8, CHK_WORK9, 
                   CHK_RESULT9, CHK_CHDAY9, CHK_REMARK9, 
                   CHK_WORK10, CHK_RESULT10, CHK_CHDAY10, 
                   CHK_REMARK10, CHK_WORK11, CHK_RESULT11, 
                   CHK_CHDAY11, CHK_REMARK11, I_TIME, 
                   I_USER
                FROM SMASH_CHK_LIST
                WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND L_CODE = '{cboL_Code.EditValue}'
                    AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dt_From_workDate.EditValue)}'
                ";

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) == 1 )
                {
                    txtCHK_WORK1.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK1", 0);
                    cboCHK_RESULT1.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT1", 0);
                    dtCHK_CHDAY1.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY1", 0);
                    txtCHK_REMARK1.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK1", 0);

                    txtCHK_WORK2.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK2", 0);
                    cboCHK_RESULT2.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT2", 0);
                    dtCHK_CHDAY2.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY2", 0);
                    txtCHK_REMARK2.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK2", 0);

                    txtCHK_WORK3.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK3", 0);
                    cboCHK_RESULT3.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT3", 0);
                    dtCHK_CHDAY3.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY3", 0);
                    txtCHK_REMARK3.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK3", 0);

                    txtCHK_WORK4.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK4", 0);
                    cboCHK_RESULT4.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT4", 0);
                    dtCHK_CHDAY4.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY4", 0);
                    txtCHK_REMARK4.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK4", 0);

                    txtCHK_WORK5.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK5", 0);
                    cboCHK_RESULT5.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT5", 0);
                    dtCHK_CHDAY5.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY5", 0);
                    txtCHK_REMARK5.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK5", 0);

                    txtCHK_WORK6.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK6", 0);
                    cboCHK_RESULT6.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT6", 0);
                    dtCHK_CHDAY6.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY6", 0);
                    txtCHK_REMARK6.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK6", 0);

                    txtCHK_WORK7.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK7", 0);
                    cboCHK_RESULT7.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT7", 0);
                    dtCHK_CHDAY7.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY7", 0);
                    txtCHK_REMARK7.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK7", 0);

                    txtCHK_WORK8.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK8", 0);
                    cboCHK_RESULT8.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT8", 0);
                    dtCHK_CHDAY8.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY8", 0);
                    txtCHK_REMARK8.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK8", 0);

                    txtCHK_WORK9.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK9", 0);
                    cboCHK_RESULT9.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT9", 0);
                    dtCHK_CHDAY9.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY9", 0);
                    txtCHK_REMARK9.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK9", 0);

                    txtCHK_WORK10.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK10", 0);
                    cboCHK_RESULT10.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT10", 0);
                    dtCHK_CHDAY10.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY10", 0);
                    txtCHK_REMARK10.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK10", 0);

                    txtCHK_WORK11.Text = Dbconn.conn.getData(chkListDs, "CHK_WORK11", 0);
                    cboCHK_RESULT11.Text = Dbconn.conn.getData(chkListDs, "CHK_RESULT11", 0);
                    dtCHK_CHDAY11.Text = Dbconn.conn.getData(chkListDs, "CHK_CHDAY11", 0);
                    txtCHK_REMARK11.Text = Dbconn.conn.getData(chkListDs, "CHK_REMARK11", 0);

                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_GrindingReport_Load(object sender, EventArgs e)
        {
            try
            {
                //작업일자
                dt_From_workDate.EditValue = DateTime.Today;
                dt_To_workDate.EditValue = DateTime.Today;

                //플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                // 라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("분쇄")), "", false, 0, false);

                // ERP 진행여부
                clsDevexpressUtil.ItemLookUpEditSetup(cboERPUpLoad, clsCommon.GetTransFlag(), "", false, 0, true);

                InitControl();

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_GrindingReport_Load", ex);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void InitControl()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {

                    new DataColumn("NAME"),
                    new DataColumn("CODE"),

                });

            dt.Rows.Add("양호", "양호");
            dt.Rows.Add("폐기", "폐기");

            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT1, dt, "", false, 0, false);
            cboCHK_RESULT1.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT2, dt, "", false, 0, false);
            cboCHK_RESULT2.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT3, dt, "", false, 0, false);
            cboCHK_RESULT3.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT4, dt, "", false, 0, false);
            cboCHK_RESULT4.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT5, dt, "", false, 0, false);
            cboCHK_RESULT5.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT6, dt, "", false, 0, false);
            cboCHK_RESULT6.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT7, dt, "", false, 0, false);
            cboCHK_RESULT7.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT8, dt, "", false, 0, false);
            cboCHK_RESULT8.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT9, dt, "", false, 0, false);
            cboCHK_RESULT9.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT10, dt, "", false, 0, false);
            cboCHK_RESULT10.EditValue = "양호";
            clsDevexpressUtil.ItemLookUpEditSetup(cboCHK_RESULT11, dt, "", false, 0, false);
            cboCHK_RESULT11.EditValue = "양호";
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboPlant_Code.EditValue?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("플랜트를 먼저 선택 조회 해주세요.");
                    cboPlant_Code.Focus();
                    return;
                }

                if (cboL_Code.EditValue?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("공정 라인을 먼저 선택 조회 해주세요.");
                    cboL_Code.Focus();
                    return;
                }

                clsDevexpressGrid.GridViewAddRow(gridView);
                gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue?.ToString());
                gridView.SetFocusedRowCellValue("PROCESS_KEY", clsCommon.GetProcessKey("분쇄"));
                gridView.SetFocusedRowCellValue("L_CODE", clsCommon.GetProcessKey("분쇄").Merge(cboL_Code.EditValue?.ToString()));
                gridView.SetFocusedRowCellValue("WORK_NUMBER", DateTime.Now.ToString("yyyyMMdd"));

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["ICM_CODE"], clsCommon.GetICMGubun());
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["GR_QTY"], 0);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["RUN_ST"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["RUN_ET"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_rowAdd_Click", ex); 
                ShowMessageBox.XtraShowError(ex.Message.ToString());
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
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

                    if (string.IsNullOrEmpty(dr["LOCATION_ST1"].ToString()))
                    {
                        dr.SetColumnError("LOCATION_ST1", "인출빈1을 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION_ST"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["LOCATION_ED1"].ToString()))
                    {
                        dr.SetColumnError("LOCATION_ED1", "투입빈1을 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION_ED"));
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SMASH_REPORT (
                             PLANT_CODE
                           , PROCESS_KEY
                           , L_CODE
                           , WORK_NUMBER
                           , WORK_SEQ
                           , RESOURCE_NO
                           , ICM_CODE
                           , EMPLOYEE_NO
                           , C_CONDITION
                           , RUN_ST
                           , RUN_ET
                           , SC_FORE
                           , SC_BG
                           , SCREEN
                           , FEED_RATE
                           , LOAD
                           , GR_WV
                           , GR_QTY
                           , DUST_WV1
                           , DUST_WV2
                           , TH_GR_QTY
                           , LOCATION_ST1
                           , LOCATION_ST2
                           , LOCATION_ED1
                           , LOCATION_ED2
                           , PRO_QTY
                           , I_TIME
                           , REMARK
                           , DEL_FLAG
                           , ERP_UP_YN
                           , ERP_TNUMBER
                        )
                        VALUES (
                             '{dr["PLANT_CODE"]}'
                           , '{dr["PROCESS_KEY"]}'
                           , '{dr["L_CODE"]}'
                           , '{dr["WORK_NUMBER"]}'
                           , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) FROM SMASH_REPORT WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}' AND WORK_NUMBER = '{dr["WORK_NUMBER"]}')
                           , '{dr["RESOURCE_NO"]}'
                           , '{dr["ICM_CODE"]}'
                           , '{dr["EMPLOYEE_NO"]}'
                           , '{dr["C_CONDITION"]}'
                           , TO_DATE('{Convert.ToDateTime(dr["RUN_ST"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                           , TO_DATE('{Convert.ToDateTime(dr["RUN_ET"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                           , '{dr["SC_FORE"]}'
                           , '{dr["SC_BG"]}'
                           , '{dr["SCREEN"]}'
                           , '{dr["FEED_RATE"]}'
                           , '{dr["LOAD"]}'
                           , '{dr["GR_WV"]}'
                           , '{dr["GR_QTY"]}'
                           , '{dr["DUST_WV1"]}'
                           , '{dr["DUST_WV2"]}'
                           , '{dr["TH_GR_QTY"]}'
                           , '{dr["LOCATION_ST1"]}'
                           , '{dr["LOCATION_ST2"]}'
                           , '{dr["LOCATION_ED1"]}'
                           , '{dr["LOCATION_ED2"]}'
                           , '{dr["PRO_QTY"]}'
                           ,  SYSDATE
                           , '{dr["REMARK"]}'
                           , 'N'
                           , 'N'
                           , '{dr["ERP_TNUMBER"]}'
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
                        SQL = $@"
                        UPDATE SMASH_REPORT
                        SET    RESOURCE_NO   = '{dr["RESOURCE_NO"]}'
                             , ICM_CODE      = '{dr["ICM_CODE"]}'
                             , EMPLOYEE_NO   = '{dr["EMPLOYEE_NO"]}'
                             , C_CONDITION   = '{dr["C_CONDITION"]}'
                             , RUN_ST        = TO_DATE('{Convert.ToDateTime(dr["RUN_ST"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                             , RUN_ET        = TO_DATE('{Convert.ToDateTime(dr["RUN_ET"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                             , SC_FORE       = '{dr["SC_FORE"]}'
                             , SC_BG         = '{dr["SC_BG"]}'
                             , SCREEN        = '{dr["SCREEN"]}'
                             , FEED_RATE     = '{dr["FEED_RATE"]}'
                             , LOAD          = '{dr["LOAD"]}'
                             , GR_WV         = '{dr["GR_WV"]}'
                             , GR_QTY        = '{dr["GR_QTY"]}'
                             , DUST_WV1      = '{dr["DUST_WV1"]}'
                             , DUST_WV2      = '{dr["DUST_WV2"]}'
                             , TH_GR_QTY     = '{dr["TH_GR_QTY"]}'
                             , LOCATION_ST1  = '{dr["LOCATION_ST1"]}'
                             , LOCATION_ST2  = '{dr["LOCATION_ST2"]}'
                             , LOCATION_ED1  = '{dr["LOCATION_ED1"]}'
                             , LOCATION_ED2  = '{dr["LOCATION_ED2"]}'
                             , PRO_QTY       = '{dr["PRO_QTY"]}'
                             , I_TIME        = SYSDATE
                             , REMARK        = '{dr["REMARK"]}'
                             , DEL_FLAG      = '{dr["DEL_FLAG"]}'
                             , ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                             , ERP_TNUMBER   = '{dr["ERP_TNUMBER"]}'
                             , ERP_ERR_CNT = 0
                        WHERE  PLANT_CODE    = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY   = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE        = '{dr["L_CODE"]}'
                        AND    WORK_NUMBER   = '{dr["WORK_NUMBER"]}'
                        AND    WORK_SEQ      = '{dr["WORK_SEQ"]}'
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

                SQL = $"DELETE FROM SMASH_CHK_LIST WHERE PLANT_CODE = '{cboPlant_Code.EditValue}' AND L_CODE = '{cboL_Code.EditValue}' AND WORK_NUMBER = '{string.Format("{0:yyyyMMdd}", dt_From_workDate.EditValue)}'";

                Dbconn.conn.SQLrun(SQL);

                SQL = $@"
                INSERT INTO SMASH_CHK_LIST (
                   PLANT_CODE, WORK_NUMBER, L_CODE, 
                   CHK_WORK1, CHK_RESULT1, CHK_CHDAY1, 
                   CHK_REMARK1, CHK_WORK2, CHK_RESULT2, 
                   CHK_CHDAY2, CHK_REMARK2, CHK_WORK3, 
                   CHK_RESULT3, CHK_CHDAY3, CHK_REMARK3, 
                   CHK_WORK4, CHK_RESULT4, CHK_CHDAY4, 
                   CHK_REMARK4, CHK_WORK5, CHK_RESULT5, 
                   CHK_CHDAY5, CHK_REMARK5, CHK_WORK6, 
                   CHK_RESULT6, CHK_CHDAY6, CHK_REMARK6, 
                   CHK_WORK7, CHK_RESULT7, CHK_CHDAY7, 
                   CHK_REMARK7, CHK_WORK8, CHK_RESULT8, 
                   CHK_CHDAY8, CHK_REMARK8, CHK_WORK9, 
                   CHK_RESULT9, CHK_CHDAY9, CHK_REMARK9, 
                   CHK_WORK10, CHK_RESULT10, CHK_CHDAY10, 
                   CHK_REMARK10, CHK_WORK11, CHK_RESULT11, 
                   CHK_CHDAY11, CHK_REMARK11, I_TIME, 
                   I_USER) 
                VALUES ( 
                    '{cboPlant_Code.EditValue}'
                  , '{string.Format("{0:yyyyMMdd}", dt_From_workDate.EditValue)}'
                  , '{cboL_Code.EditValue}'
                  , '{txtCHK_WORK1.EditValue}'
                  , '{cboCHK_RESULT1.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY1.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK1.EditValue}'
                  , '{txtCHK_WORK2.EditValue}'
                  , '{cboCHK_RESULT2.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY2.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK2.EditValue}'
                  , '{txtCHK_WORK3.EditValue}'
                  , '{cboCHK_RESULT3.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY3.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK3.EditValue}' 
                  , '{txtCHK_WORK4.EditValue}'
                  , '{cboCHK_RESULT4.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY4.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK4.EditValue}'
                  , '{txtCHK_WORK5.EditValue}'
                  , '{cboCHK_RESULT5.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY5.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK5.EditValue}'
                  , '{txtCHK_WORK6.EditValue}'
                  , '{cboCHK_RESULT6.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY6.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK6.EditValue}'
                  , '{txtCHK_WORK7.EditValue}'
                  , '{cboCHK_RESULT7.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY7.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK7.EditValue}'
                  , '{txtCHK_WORK8.EditValue}'
                  , '{cboCHK_RESULT8.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY8.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK8.EditValue}'
                  , '{txtCHK_WORK9.EditValue}'
                  , '{cboCHK_RESULT9.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY9.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK9.EditValue}'
                  , '{txtCHK_WORK10.EditValue}'
                  , '{cboCHK_RESULT10.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY10.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK10.EditValue}'
                  , '{txtCHK_WORK11.EditValue}'
                  , '{cboCHK_RESULT11.EditValue}'
                  , TO_DATE('{((DateTime)dtCHK_CHDAY11.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                  , '{txtCHK_REMARK11.EditValue}'
                  , SYSDATE
                  , '{clsCommon._strUserId}' )
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    clsLog.logSave(this.Text, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("SMASH_CHK_LIST 저장에 실패했습니다");
                    return;
                }

                XMain_Search();

                ShowMessageBox.XtraShowInformation("저장되었습니다");

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("미리보기창이 실행중입니다\r\n잠시만 기다려주세요");

                string workDate = string.Format("{0:yyyyMMdd}", dt_From_workDate.EditValue);
                clsPrintReport.PrintGrindingReport(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString(), cboL_Code.EditValue?.ToString(), workDate);
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

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 분쇄정보 데이터를 삭제하시겠습니까?"))
                {
                    return;
                }

                Dbconn.conn.BeginTransaction();

                foreach (int id in gridView.GetSelectedRows())
                {
                    DataRow dr = gridView.GetDataRow(id);

                    string sPLANT_CODE = dr["PLANT_CODE"].ToString().Trim();
                    string sPROCESS_KEY = dr["PROCESS_KEY"].ToString().Trim();
                    string sL_CODE = dr["L_CODE"].ToString().Trim();
                    string sWORK_NUMBER = dr["WORK_NUMBER"].ToString().Trim();
                    string sWORK_SEQ = dr["WORK_SEQ"].ToString().Trim();

                    if (dr.RowState != DataRowState.Added)
                    {
                        SQL = $@"
                        DELETE FROM SMASH_REPORT
                        WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                            AND PROCESS_KEY = '{clsCommon.GetProcessKey("분쇄")}'
                            AND L_CODE = '{cboL_Code.EditValue}'
                            AND WORK_NUMBER = '{sWORK_NUMBER}' AND WORK_SEQ = '{sWORK_SEQ}'
                        ";

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

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            if (gridView.RowCount == 0)
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
                Dbconn.conn.BeginTransaction();
                int[] selectedRows = gridView.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = gridView.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM SMASH_REPORT
                    WHERE  PLANT_CODE  = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE      = '{dr["L_CODE"]}'
                        AND    WORKDATE    = '{dr["WORKDATE"]}'
                        AND    WORK_SEQ    = '{dr["WORK_SEQ"]}'
                        AND ERP_UP_YN IN ('N', 'M', 'X', 'G')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE SMASH_REPORT
                        SET    ERP_UP_YN = CASE WHEN ERP_UP_YN = 'Y' THEN 'M' ELSE 'F' END
                            , ERP_ERR_CNT = 0
                        WHERE  PLANT_CODE  = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE      = '{dr["L_CODE"]}'
                        AND    WORKDATE    = '{dr["WORKDATE"]}'
                        AND    WORK_SEQ    = '{dr["WORK_SEQ"]}'
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

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridControl.DataSource = null;

                // 공정
                clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);
                cboProcessKey.EditValue = clsCommon.GetProcessKey("분쇄");

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