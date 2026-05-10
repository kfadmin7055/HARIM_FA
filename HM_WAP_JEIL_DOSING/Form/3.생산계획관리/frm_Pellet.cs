using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.CodeParser.CodeStyle.Formatting;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.PivotGrid.QueryMode;
using DevExpress.Schedule;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList.ViewInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Pellet : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;
        private string[] sValid = null;

        private string formname = string.Empty;
        private string vProcessName = string.Empty;

        private string vPlant_Code = string.Empty;
        private string vProcess_Code = string.Empty;
        private string vLine_Code = string.Empty;

        bool chk_version = false;
        decimal dNote_Per = 0;

        private bool isInitializing = false;

        public frm_Pellet(string plant_code, string process_key, string lcode, string formName, string sProcessName)
        {
            InitializeComponent();
            formname = formName;

            vPlant_Code = plant_code;
            vProcess_Code = process_key;
            vLine_Code = lcode;
            vProcessName = sProcessName;

            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewSub, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewList, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Pellet_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            isInitializing = true;

            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            dateEdit_workDate.EditValue = DateTime.Today;

            //if (clsCommon._strUserType == "010611")
            //{
            //    layoutControlItem_workDateEd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //}
            //else
            //{
            //layoutControlItem_workDateEd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            dateEdit_workDateEd.EditValue = DateTime.Today.AddDays(1);
            //}

            InitControl();

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void InitControl()
        {
            #region Main
            gridView.Columns["ERP_ISTATUS"].OptionsColumn.AllowEdit = false;
            gridView.Columns["ERP_OSTATUS"].OptionsColumn.AllowEdit = false;

            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(vPlant_Code, vProcess_Code), "", false, 0, true);
            cboL_Code.EditValue = vLine_Code;

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "전체선택", false, 2, true);

            // ERP 전송상태
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboTransFlag, clsCommon.GetTransFlag(), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboPROCESS_KEY, clsCommon.GetGridProcess(vPlant_Code, clsCommon.GetProcessTypeCode("펠렛")), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboBFProcessKey, clsCommon.GetGridProcess(vPlant_Code, clsCommon.GetProductTypeCode("배합")), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(vPlant_Code, vProcess_Code));

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboC_CONDITION, clsCommon.GetPcStatus());

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboResourceNo, clsCommon.GetResource(vPlant_Code, vProcess_Code, $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 2, true), "품목을 선택 해주세요.", false);
            DevExpress.XtraGrid.Views.Grid.GridView view = gridScboResourceNo.View as DevExpress.XtraGrid.Views.Grid.GridView;

            view.OptionsView.ColumnAutoWidth = false;

            // 원하는 컬럼 사이즈 지정
            view.Columns["CODE"].Width = 100;
            view.Columns["NAME"].Width = 260;

            Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "PER", "비율" }
                };

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboNOTE, clsCommon.getNote(vPlant_Code), "배합비 버전이 없습니다.", false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboLocation, clsCommon.GetBin(vPlant_Code, vProcess_Code), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboPROCESS_KEY, clsCommon.GetGridProcess(vPlant_Code, clsCommon.GetProcessTypeCode("펠렛")), "", false, false);

            //작업조
            gridcboICM_CODE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboICM_CODE, clsCommon.GetICM(), "", false, false);

            //작업자
            clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_EMPLOYEE_NO, clsCommon.GetDO_INSA(vPlant_Code));

            if (vProcess_Code.Contains(clsCommon.GetProcessKey("EP가공")))
            {
                string[] hiddenColumns = { "BF_PROCESS_KEY", "BF_L_CODE", "BF_WORKDATE", "BF_NUM" };
                foreach (string colName in hiddenColumns)
                {
                    if (gridView.Columns[colName] != null)
                        gridView.Columns[colName].Visible = false;
                }
            }

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboBF_PACK_TYPE, clsCommon.GetPackType(), "", false, false);
            #endregion

            subGridChk.ValueChecked = "Y";
            subGridChk.ValueUnchecked = "N";
            subGridChk.NullStyle = StyleIndeterminate.Unchecked;
            subGridChk.CheckStyle = CheckStyles.Standard;
        }

        private string workNumber_maker(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate)
        {
            try
            {
                string return_seq = string.Empty;

                string SQL = $@"
                SELECT NVL(MAX(WORK_SEQ) + 1, 1) AS SEQ
                FROM PELLET_REPORT
                WHERE PLANT_CODE = '{sPlantCode}'
                    AND PROCESS_KEY = '{sProcessKey}'
                    AND L_CODE = '{sLCode}'
                    AND WORKDATE = '{string.Format("{0:yyyyMMdd}", sWorkDate)}'
                ";

                using (DataSet Ds = Dbconn.conn.ExecutDataset(SQL))
                {
                    if (Dbconn.conn.getRowCnt(Ds) == 0)
                    {
                        clsLog.logSave("작업순번 생성에러 / SQL : " + SQL, 0);
                        return string.Empty;
                    }

                    return_seq = Dbconn.conn.getData(Ds, "SEQ", 0);
                    Ds.Dispose();

                    return return_seq;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "workNumber_maker", ex);
                return string.Empty;
            }
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                     a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE
                    , a.WORKDATE, a.WORK_SEQ, a.SEQ, P_TYPE
                    , a.RESOURCE_NO, a.NOTE, a.HALT_TIME
                    , a.RUN_ST, a.RUN_ET, a.BF_PLANT_CODE, a.BF_PROCESS_KEY, c.PROCESS_DESC AS BF_L_CODE
                    , a.BF_WORKDATE, a.BF_NUM, a.BF_QTY, a.BF_PACK_TYPE
                    , a.QTY, a.REMAIN_QTY, ROUND((a.RUN_ET - a.RUN_ST) * 24, 2) AS WORK_HOUR, a.PROTY
                    , a.TOTAL_OPER, a.DY_ST, a.DY_THICK
                    , a.WORK_START_DATE, a.ICM_CODE, a.EMPLOYEE_NO
                    , a.DY_SP, a.CURRENT_1, a.CURRENT_2
                    , a.FEEDER_RATE, a.CRUMBLE_YN, a.HARDNESS
                    , a.PDI, a.CLEAN_QTY, a.LOCATION_ST1
                    , a.LOCATION_ST2, a.LOCATION_ED1, a.LOCATION_ED2
                    , a.HZ, a.CD_TEMP, a.P_TEMP
                    , a.COL_TEMP, a.REMARK, a.DEL_FLAG
                    , a.ERP_UP_YN, a.ERP_TNUMBER, a.I_TIME, a.C_CONDITION
                    , a.ERP_ISTATUS, a.ERP_ITNUMBER, a.ERP_OSTATUS, a.ERP_OTNUMBER, a.ERR_MSG
                    , b.AMP_1ST, b.AMP_2ND, b.DURABILITY
                    , b.END_TIME, b.F_R_RATIO, b.GAP_VALUE
                    , b.MESH_10, b.MESH_18
                    , b.MESH_35, b.MESH_6, b.MESH_8 
                    , b.MESH_PAN
                    , b.PROD_TIME, b.ROLL_GAP, b.START_TIME, a.STOP_TIME
                    , b.STOP_TIME, b.TEMP_1ST, b.TEMP_2ND
                    , a.WORK_SEQ_COPY
                FROM PELLET_REPORT a
                    LEFT JOIN PELLET_REPORT_ADD b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE
                                                AND b.WORKDATE = a.WORKDATE AND b.WORK_SEQ = a.WORK_SEQ
                    LEFT JOIN SAP_PROCESS_LDIVISION c ON c.PLANT_CODE = a.BF_PLANT_CODE AND c.PROCESS_KEY = a.BF_PROCESS_KEY AND c.L_CODE = a.BF_L_CODE
                WHERE a.PLANT_CODE = '{vPlant_Code}'
                    AND a.PROCESS_KEY = '{vProcess_Code}'
                    AND ('{cboL_Code.EditValue?.ToString()}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue}')
                    AND a.WORKDATE BETWEEN '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' 
                                    AND '{string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)}'
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                ORDER BY a.WORKDATE DESC, a.RUN_ST, a.SEQ ASC, CASE WHEN a.WORK_SEQ_COPY = a.SEQ THEN 1
                                ELSE 0 END, a.WORK_SEQ_COPY DESC
                ";


                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true, true);
                ds.Dispose();

                gridView.SetFixCol(new string[] { "ERP_ISTATUS"
                                                , "ERP_OSTATUS"
                                                , "L_CODE"
                                                , "WORKDATE"
                                                , "WORK_SEQ"
                                                , "SEQ"
                                                , "RESOURCE_NO"
                                                });

                sValid = new string[] { "PLANT_CODE"
                                       , "PROCESS_KEY"
                                       , "L_CODE"
                                       , "P_TYPE"
                                       , "RESOURCE_NO"
                                       , "NOTE"
                                       , "BF_PROCESS_KEY"
                                       , "BF_WORKDATE"
                                       , "BF_NUM"
                                       , "ICM_CODE"
                                       , "EMPLOYEE_NO"
                                       , "LOCATION_ST1"
                                       , "LOCATION_ED1"
                                       };

                // 필수 항목 아이콘
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
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "car_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XSub_Search()
        {
            try
            {
                SQL = $@"
                SELECT PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, WORK_SEQ
                    , START_TIME, END_TIME, PROD_TIME, AMP_1ST, AMP_2ND
                    , F_R_RATIO, TEMP_1ST, TEMP_2ND, GAP_VALUE, ROLL_GAP
                    , DURABILITY, MESH_6, MESH_8, MESH_10, MESH_18
                    , MESH_35, MESH_PAN, STOP_TIME, DY_ST, DY_THICK
                    , WORK_START_DATE, DY_SP, CURRENT_1, CURRENT_2, FEEDER_RATE
                    , CRUMBLE_YN, HARDNESS, PDI, CLEAN_QTY, HZ
                    , CD_TEMP, P_TEMP, COL_TEMP, B_W
                FROM PELLET_REPORT_ADD
                WHERE PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{gridView.GetFocusedRowCellValue("PROCESS_KEY")}' AND L_CODE = '{gridView.GetFocusedRowCellValue("L_CODE")}'
                    AND WORKDATE = '{gridView.GetFocusedRowCellValue("WORKDATE")}' AND WORK_SEQ = '{gridView.GetFocusedRowCellValue("WORK_SEQ")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridSub, viewSub, ds.Tables[0], true, true);
                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "car_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XList_Search()
        {
            try
            {
                SQL = $@"
                WITH COMM AS (
                    SELECT a.WK_DIVCODE, b.COMM_CODE, c.COMM_DTCODE, c.COMM_DTNM
                    FROM COMM_DIV a
                        INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                        INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                )

                SELECT 
                     a.PLANT_CODE, a.PROCESS_KEY, c.PROCESS_DESC AS L_CODE
                    , a.WORKDATE, a.WORK_SEQ, a.SEQ, a.P_TYPE
                    , a.RESOURCE_NO || ' : ' || p.DESCRIPTION AS RESOURCE_NO, a.NOTE, a.HALT_TIME
                    , a.RUN_ST, a.RUN_ET, a.BF_PLANT_CODE, a.BF_PROCESS_KEY, c.PROCESS_DESC AS BF_L_CODE
                    , a.BF_WORKDATE, a.BF_NUM, a.BF_QTY, com.COMM_DTNM AS BF_PACK_TYPE
                    , a.QTY, a.REMAIN_QTY, a.PROTY, a.TOTAL_OPER, a.ICM_CODE, a.EMPLOYEE_NO, a.LOCATION_ST1
                    , a.LOCATION_ST2, a.LOCATION_ED1, a.LOCATION_ED2, a.REMARK, a.DEL_FLAG
                    , a.ERP_UP_YN, a.ERP_TNUMBER, a.I_TIME, a.C_CONDITION
                    , a.ERP_ISTATUS, a.ERP_ITNUMBER, a.ERP_OSTATUS, a.ERP_OTNUMBER, a.ERR_MSG
                    , b.START_TIME, b.END_TIME, b.PROD_TIME, b.AMP_1ST, b.AMP_2ND
                    , b.F_R_RATIO, b.TEMP_1ST, b.TEMP_2ND, b.GAP_VALUE, b.ROLL_GAP
                    , b.DURABILITY, b.MESH_6, b.MESH_8, b.MESH_10, b.MESH_18
                    , b.MESH_35, b.MESH_PAN, b.STOP_TIME, b.DY_ST, b.DY_THICK
                    , b.WORK_START_DATE, b.DY_SP, b.CURRENT_1, b.CURRENT_2, b.FEEDER_RATE
                    , b.CRUMBLE_YN, b.HARDNESS, b.PDI, b.CLEAN_QTY, b.HZ
                    , b.CD_TEMP, b.P_TEMP, b.COL_TEMP, b.B_W
                    , a.WORK_SEQ_COPY
                FROM PELLET_REPORT a
                    LEFT JOIN PELLET_REPORT_ADD b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE
                                                AND b.WORKDATE = a.WORKDATE AND b.WORK_SEQ = a.WORK_SEQ
                    LEFT JOIN SAP_PROCESS_LDIVISION c ON c.PLANT_CODE = a.PLANT_CODE AND c.PROCESS_KEY = a.PROCESS_KEY AND c.L_CODE = a.L_CODE
                    LEFT JOIN SAP_PROCESS_LDIVISION d ON d.PLANT_CODE = a.BF_PLANT_CODE AND d.PROCESS_KEY = a.BF_PROCESS_KEY AND d.L_CODE = a.BF_L_CODE
                    LEFT JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = a.PLANT_CODE AND p.RESOURCE_NO = a.RESOURCE_NO
                    LEFT JOIN COMM com ON com.WK_DIVCODE = '03' AND com.COMM_CODE = '14' AND com.COMM_DTCODE = a.BF_PACK_TYPE
                WHERE a.PLANT_CODE = '{vPlant_Code}'
                    AND a.PROCESS_KEY = '{vProcess_Code}'
                    AND ('{cboL_Code.EditValue?.ToString()}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue}')
                    AND a.WORKDATE BETWEEN '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' 
                                    AND '{string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)}'
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                    AND a.ERP_ISTATUS = 'Y'
                ORDER BY a.WORKDATE DESC, a.RUN_ST, a.SEQ ASC, CASE WHEN a.WORK_SEQ_COPY = a.SEQ THEN 1
                                ELSE 0 END, a.WORK_SEQ_COPY DESC
                ";


                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridList, viewList, ds.Tables[0], true, true);
                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "car_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
                XMain_Search();
            else
                XList_Search();
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

            if (cboL_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("공정 라인을 먼저 선택 조회 해주세요.");
                cboL_Code.Focus();
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);

            gridView.SetFocusedRowCellValue("PLANT_CODE", vPlant_Code);
            gridView.SetFocusedRowCellValue("PROCESS_KEY", vProcess_Code);
            gridView.SetFocusedRowCellValue("L_CODE", cboL_Code.EditValue?.ToString());
            gridView.SetFocusedRowCellValue("WORKDATE", string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
            gridView.SetFocusedRowCellValue("P_TYPE", "2");
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["ICM_CODE"], clsCommon.GetICMGubun());
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"], clsCommon.PcStatus.Plan);

            //gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["RUN_ST"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["RUN_ET"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            gridView.SetFocusedRowCellValue("STOP_TIME", 0);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["EMPLOYEE_NO"], clsCommon.UserId);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["BF_PROCESS_KEY"], clsCommon.GetProcessKey("배합"));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["BF_PACK_TYPE"], "031401");

        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                string lCode = string.Empty;
                clsDevexpressGrid.GridEndEdit(gridView);

                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (DialogResult.Yes != ShowMessageBox.Confirm("작업지시정보 데이터를 저장하시겠습니까?"))
                {
                    return;
                }

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

                    lCode = dr["L_CODE"].ToString().Trim().IsNullEmpty() ? " " : dr["L_CODE"].ToString();

                    string sDtFrom = string.Empty;
                    string sDtTo = string.Empty;

                    if (!string.IsNullOrEmpty(dr["RUN_ST"].ToString()))
                    {
                        DateTime dtFrom = DateTime.Parse(dr["RUN_ST"].ToString());

                        sDtFrom = $"TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (!string.IsNullOrEmpty(dr["RUN_ET"].ToString()))
                    {
                        DateTime dtTo = DateTime.Parse(dr["RUN_ET"].ToString());

                        sDtTo = $"TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (!string.IsNullOrEmpty(dr["RUN_ST"].ToString()) && !string.IsNullOrEmpty(dr["RUN_ET"].ToString()))
                    {
                        int time_diff = Convert.ToDateTime(Convert.ToDateTime(dr["RUN_ET"]).ToString("yyyy-MM-dd HH:mm:ss")).CompareTo(Convert.ToDateTime(Convert.ToDateTime(dr["RUN_ST"]).ToString("yyyy-MM-dd HH:mm:ss")));
                        if (time_diff < 0)
                        {
                            ShowMessageBox.XtraShowInformation("종료시간이 시작시간보다 빠르거나 같습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        if (!ValidationCheck(dr)) return;

                        string sWorkSeq = workNumber_maker(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["L_CODE"]?.ToString(), dr["WORKDATE"]?.ToString());

                        if (string.IsNullOrEmpty(sWorkSeq))
                        {
                            Dbconn.conn.Rollback();
                            ShowMessageBox.XtraShowInformation("작업순번을 생성하는 도중 에러가 발생했습니다");
                            return;
                        }

                        SQL = $@"
                        INSERT INTO PELLET_REPORT (
                             PLANT_CODE       -- 01
                            , PROCESS_KEY      -- 02
                            , L_CODE           -- 03
                            , WORKDATE         -- 04
                            , WORK_SEQ         -- 05
                            , P_TYPE           -- 06
                            , RESOURCE_NO      -- 07
                            , NOTE             -- 08
                            , HALT_TIME        -- 09
                            , RUN_ST           -- 10
                            , RUN_ET           -- 11
                            , BF_PROCESS_KEY   -- 12
                            , BF_WORKDATE      -- 13
                            , BF_NUM           -- 14
                            , BF_QTY           -- 15
                            , QTY              -- 16
                            , REMAIN_QTY       -- 17
                            , PROTY            -- 18
                            , TOTAL_OPER       -- 19
                            , DY_ST            -- 20
                            , DY_THICK         -- 21
                            , WORK_START_DATE  -- 22
                            , ICM_CODE         -- 23
                            , EMPLOYEE_NO      -- 24
                            , DY_SP            -- 25
                            , CURRENT_1        -- 26
                            , CURRENT_2        -- 27
                            , FEEDER_RATE      -- 28
                            , CRUMBLE_YN       -- 29
                            , HARDNESS         -- 30
                            , PDI              -- 31
                            , CLEAN_QTY        -- 32
                            , LOCATION_ST1     -- 33
                            , LOCATION_ST2     -- 34
                            , LOCATION_ED1     -- 35
                            , LOCATION_ED2     -- 36
                            , HZ               -- 37
                            , CD_TEMP          -- 38
                            , P_TEMP           -- 39
                            , COL_TEMP         -- 40
                            , REMARK           -- 41
                            , DEL_FLAG         -- 42
                            , ERP_UP_YN        -- 43
                            , ERP_TNUMBER      -- 44
                            , I_TIME           -- 45
                            , STOP_TIME
                            , BF_PACK_TYPE
                            , ERP_ISTATUS
                            , ERP_OSTATUS
                        )
                        VALUES (
                             '{dr["PLANT_CODE"]}'                                       -- 01
                            , '{dr["PROCESS_KEY"]}'      -- 02
                            , '{dr["L_CODE"]}'           -- 03
                            , '{dr["WORKDATE"]}'         -- 04
                            , '{sWorkSeq}'         -- 05
                            , '2'                        -- 06
                            , '{dr["RESOURCE_NO"]}'      -- 07
                            , '{dr["NOTE"]}'             -- 08
                            , '{dr["HALT_TIME"]}'        -- 09
                            , ({(string.IsNullOrEmpty(sDtFrom) ? "''" : $"{sDtFrom}")})
                            , ({(string.IsNullOrEmpty(sDtTo) ? "''" : $"{sDtTo}")})           -- 11
                            , '{dr["BF_PROCESS_KEY"]}'   -- 12
                            , '{(dr["BF_WORKDATE"] == DBNull.Value ? "" : Convert.ToDateTime(dr["BF_WORKDATE"]).ToString("yyyyMMdd"))}'      -- 13
                            , '{dr["BF_NUM"]}'           -- 14
                            , '{dr["BF_QTY"]}'           -- 15
                            , '{dr["QTY"]}'              -- 16
                            , '{dr["REMAIN_QTY"]}'       -- 17
                            , '{dr["PROTY"]}'            -- 18
                            , '{dr["TOTAL_OPER"]}'       -- 19
                            , '{dr["DY_ST"]}'            -- 20
                            , '{dr["DY_THICK"]}'         -- 21
                            , SYSDATE  -- 22
                            , '{dr["ICM_CODE"]}'         -- 23
                            , '{dr["EMPLOYEE_NO"]}'      -- 24
                            , '{dr["DY_SP"]}'            -- 25
                            , '{dr["CURRENT_1"]}'        -- 26
                            , '{dr["CURRENT_2"]}'        -- 27
                            , '{dr["FEEDER_RATE"]}'      -- 28
                            , '{dr["CRUMBLE_YN"]}'       -- 29
                            , '{dr["HARDNESS"]}'         -- 30
                            , '{dr["PDI"]}'              -- 31
                            , '{dr["CLEAN_QTY"]}'        -- 32
                            , '{dr["LOCATION_ST1"]}'     -- 33
                            , '{dr["LOCATION_ST2"]}'     -- 34
                            , '{dr["LOCATION_ED1"]}'     -- 35
                            , '{dr["LOCATION_ED2"]}'     -- 36
                            , '{dr["HZ"]}'               -- 37
                            , '{dr["CD_TEMP"]}'          -- 38
                            , '{dr["P_TEMP"]}'           -- 39
                            , '{dr["COL_TEMP"]}'         -- 40
                            , '{dr["REMARK"]}'           -- 41
                            , '{dr["DEL_FLAG"]}'         -- 42
                            , 'N'                        -- 43
                            , '{dr["ERP_TNUMBER"]}'      -- 44
                            , SYSDATE                    -- 45
                            , NVL('{dr["STOP_TIME"]}', 0)
                            , '{dr["BF_PACK_TYPE"]}'
                            , 'N'
                            , 'N'
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        /* TB01 : PELLET_REPORT_ADD */
                        INSERT INTO PELLET_REPORT_ADD (
                             L_CODE            /* 07 */
                           , PLANT_CODE        /* 14 */
                           , PROCESS_KEY       /* 15 */
                           , WORK_SEQ          /* 22 */
                           , WORKDATE          /* 23 */
                        ) VALUES (
                             '{dr["L_CODE"]}'           /* 07 */
                           , '{dr["PLANT_CODE"]}'       /* 14 */
                           , '{dr["PROCESS_KEY"]}'      /* 15 */
                           , '{sWorkSeq}'         /* 22 */
                           , '{dr["WORKDATE"]}'         /* 23 */
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();

                        //string erp_insert_chk = clsErpSql.InsertWorkOrder(clsCommon.pellet_process_code , string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), WORKDATE, dr, "I");
                        //if (erp_insert_chk != "OK")
                        //{
                        //    Dbconn.conn.Rollback();
                        //    ShowMessageBox.XtraShowWarning("작업지시 입력에 실패했습니다(ERP INPUT FAIL)");
                        //    return;
                        //}
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        if (!ValidationCheck(dr)) return;

                        SQL = $@"
                        UPDATE PELLET_REPORT
                        SET RESOURCE_NO      = '{dr["RESOURCE_NO"]}'      					          -- 02
                            , NOTE             = '{dr["NOTE"]}'             					          -- 03
                            , HALT_TIME        = '{dr["HALT_TIME"]}'        					          -- 04
                            , RUN_ST           = ({(string.IsNullOrEmpty(sDtFrom) ? "''" : $"{sDtFrom}")})
                            , RUN_ET           = ({(string.IsNullOrEmpty(sDtTo) ? "''" : $"{sDtTo}")})   					          -- 06
                            , BF_PROCESS_KEY   = '{dr["BF_PROCESS_KEY"]}'   					          -- 07
                            , BF_WORKDATE      = '{dr["BF_WORKDATE"]}'      					          -- 08
                            , BF_NUM           = '{dr["BF_NUM"]}'           					          -- 09
                            , BF_QTY           = '{dr["BF_QTY"]}'           					          -- 10
                            , QTY              = '{dr["QTY"]}'              					          -- 11
                            , REMAIN_QTY       = '{dr["REMAIN_QTY"]}'       					          -- 12
                            , PROTY            = '{dr["PROTY"]}'            					          -- 13
                            , TOTAL_OPER       = '{dr["TOTAL_OPER"]}'        					          -- 14
                            , DY_ST            = '{dr["DY_ST"]}'            					          -- 15
                            , DY_THICK         = '{dr["DY_THICK"]}'         					          -- 16
                            , WORK_START_DATE  = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'  					          -- 17
                            , ICM_CODE         = '{dr["ICM_CODE"]}'         					          -- 18
                            , EMPLOYEE_NO      = '{dr["EMPLOYEE_NO"]}'      					          -- 19
                            , DY_SP            = '{dr["DY_SP"]}'            					          -- 20
                            , CURRENT_1        = '{dr["CURRENT_1"]}'        					          -- 21
                            , CURRENT_2        = '{dr["CURRENT_2"]}'        					          -- 22
                            , FEEDER_RATE      = '{dr["FEEDER_RATE"]}'      					          -- 23
                            , CRUMBLE_YN       = '{dr["CRUMBLE_YN"]}'       					          -- 24
                            , HARDNESS         = '{dr["HARDNESS"]}'         					          -- 25
                            , PDI              = '{dr["PDI"]}'              					          -- 26
                            , CLEAN_QTY        = '{dr["CLEAN_QTY"]}'        					          -- 27
                            , LOCATION_ST1     = '{dr["LOCATION_ST1"]}'     					          -- 28
                            , LOCATION_ST2     = '{dr["LOCATION_ST2"]}'     					          -- 29
                            , LOCATION_ED1     = '{dr["LOCATION_ED1"]}'     					          -- 30
                            , LOCATION_ED2     = '{dr["LOCATION_ED2"]}'     					          -- 31
                            , HZ               = '{dr["HZ"]}'               					          -- 32
                            , CD_TEMP          = '{dr["CD_TEMP"]}'          					          -- 33
                            , P_TEMP           = '{dr["P_TEMP"]}'           					          -- 34
                            , COL_TEMP         = '{dr["COL_TEMP"]}'         					          -- 35
                            , REMARK           = '{dr["REMARK"]}'           					          -- 36
                            , DEL_FLAG         = '{dr["DEL_FLAG"]}'         					          -- 37
                            , I_TIME           = SYSDATE                    					          -- 40
                            , STOP_TIME        = '{dr["STOP_TIME"]}'
                            , BF_PACK_TYPE     = '{dr["BF_PACK_TYPE"]}'
                            , ERP_ERR_CNT = 0
                            , ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                             WHEN 'D' THEN 'X'
                                                             WHEN 'F' THEN 'N'
                                                             WHEN NULL THEN 'F'
                                                             ELSE TO_CHAR(ERP_OSTATUS) END
                            , ERP_ISTATUS = CASE TO_CHAR(ERP_ISTATUS) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                             WHEN 'D' THEN 'X'
                                                             WHEN 'F' THEN 'N'
                                                             WHEN NULL THEN 'F'
                                                             ELSE TO_CHAR(ERP_ISTATUS) END
                        WHERE PLANT_CODE       = '{dr["PLANT_CODE"]}'
                            AND  PROCESS_KEY      = '{dr["PROCESS_KEY"]}'
                            AND  L_CODE           = '{dr["L_CODE"]}'
                            AND  WORKDATE         = '{dr["WORKDATE"]}'
                            AND  WORK_SEQ         = '{dr["WORK_SEQ"]}'
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

                gridView.RefreshData();
                XMain_Search();

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private static bool ValidationCheck(DataRow dr)
        {
            if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
            {
                dr.SetColumnError("RESOURCE_NO", "제품을 선택하여 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                return false;
            }

            if (string.IsNullOrEmpty(dr["LOCATION_ST1"].ToString()))
            {
                dr.SetColumnError("LOCATION_ST1", "인출빈을 입력하여 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION_ST1"));
                return false;
            }

            if (string.IsNullOrEmpty(dr["LOCATION_ED1"].ToString()))
            {
                dr.SetColumnError("LOCATION_ED1", "제품빈을 입력하여 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION_ED1"));
                return false;
            }

            //if (string.IsNullOrEmpty(dr["QTY"].ToString()))
            //{
            //    dr.SetColumnError("QTY", "생산량을 입력하여주세요");
            //    ShowMessageBox.XtraShowWarning(dr.GetColumnError("QTY"));
            //    return false;
            //}

            if (string.IsNullOrEmpty(dr["BF_PROCESS_KEY"].ToString()))
            {
                dr.SetColumnError("BF_PROCESS_KEY", "이전 공정을 입력하여주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BF_PROCESS_KEY"));
                return false;
            }

            if (string.IsNullOrEmpty(dr["BF_WORKDATE"].ToString()))
            {
                dr.SetColumnError("BF_WORKDATE", "이전 작업일을 입력하여주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BF_WORKDATE"));
                return false;
            }

            if (string.IsNullOrEmpty(dr["BF_NUM"].ToString()))
            {
                dr.SetColumnError("BF_NUM", "이전 작업번호을 입력하여주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BF_NUM"));
                return false;
            }

            if (string.IsNullOrEmpty(dr["BF_QTY"].ToString()))
            {
                dr.SetColumnError("BF_QTY", "이전 생산량을 입력하여주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BF_QTY"));
                return false;
            }

            return true;
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

                if (gridView.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 작업지시를 선택하여 주세요");
                    return;
                }

                DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    gridView.DeleteRow(gridView.FocusedRowHandle);
                }
                else
                {
                    int[] selectedRows = gridView.GetSelectedRows();

                    if (selectedRows.Length == 0)
                    {
                        ShowMessageBox.XtraShowWarning("삭제할 정보를 선택 해주세요.");
                        return;
                    }

                    foreach (int rowHandle in selectedRows)
                    {
                        var dr = gridView.GetDataRow(rowHandle);

                        dr.ClearErrors();

                        string condition = gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["ICM_CODE"]);

                        if (condition.Equals("진행"))
                        {
                            ShowMessageBox.XtraShowInformation("진행중인 작업지시는 삭제하실 수 없습니다");
                            return;
                        }

                        if (condition.Equals("완료"))
                        {
                            ShowMessageBox.XtraShowInformation("완료된 작업지시는 삭제하실 수 없습니다");
                            return;
                        }

                        if (gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["ERP_ISTATUS"]).Equals("전송 완료"))
                        {
                            ShowMessageBox.XtraShowInformation("전송한 작업은 삭제 할수 없습니다");
                            return;
                        }

                        Dbconn.conn.BeginTransaction();
                        //delete work num
                        SQL = $@"
                        UPDATE PELLET_REPORT SET DEL_FLAG = 'Y'
                                                , ERP_OSTATUS = 'X'
                                                , ERP_ISTATUS = 'X'
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}'
                            AND PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                            AND L_CODE = '{dr["L_CODE"]}'
                            AND WORKDATE = '{dr["WORKDATE"]}' AND WORK_SEQ = '{dr["WORK_SEQ"]}' ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();
                    }
                }

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        private void btnERPDelete_Click(object sender, EventArgs e)
        {
            //ShowMessageBox.XtraShowInformation("기능 추가 중입니다.");
            //return;
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

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
                int[] selectedRows = gridView.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                string pellet = string.Empty;

                foreach (int rowHandle in selectedRows)
                {
                    var dr = gridView.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    clsCommon.GetAutoPellet(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), out pellet);

                    SQL = $@"
                    SELECT 1
                    FROM PELLET_REPORT
                     WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                        AND (ERP_OSTATUS IN ('Y')
                        OR ERP_ISTATUS IN ('Y'))
                    ";

                    DataSet dsPel = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(dsPel) > 0)
                    {
                        SQL = $@"
                        UPDATE PELLET_REPORT
                        SET   ERP_OSTATUS = CASE WHEN '{pellet}' IS NULL 
                                                THEN CASE TO_CHAR(ERP_UP_YN) 
                                                    WHEN 'Y' THEN 'D'
                                                ELSE TO_CHAR(ERP_UP_YN) END
                                            ELSE TO_CHAR(ERP_OSTATUS)
                                            END
                            , ERP_OERR_CNT = 0
                            , ERP_ISTATUS = CASE TO_CHAR(ERP_ISTATUS) 
                                                WHEN 'Y' THEN 'D'
                                            ELSE TO_CHAR(ERP_ISTATUS) END
                            , ERP_IERR_CNT = 0
                            , C_CONDITION = '{clsCommon.GetPcStatusCode("취소")}'
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
                            return;
                        }

                        if (!string.IsNullOrEmpty(pellet))
                        {
                            SQL = $@"
                            SELECT 1
                            FROM WORK_ORDER
                             WHERE PLANT_CODE = '{dr["BF_PLANT_CODE"]}' AND PROCESS_KEY = '{dr["BF_PROCESS_KEY"]}' AND L_CODE = '{dr["BF_L_CODE"]}'
                                        AND WORKDATE = TO_CHAR(TO_DATE('{dr["BF_WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{dr["BF_NUM"]}'
                                AND ERP_ISTATUS IN ('Y')
                            ";

                            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(ds) > 0)
                            {
                                SQL = $@"
                                UPDATE WORK_ORDER
                                SET  ERP_ISTATUS = 'P'
                                    , ERP_IERR_CNT = 0
                                WHERE PLANT_CODE = '{dr["BF_PLANT_CODE"]}' AND PROCESS_KEY = '{dr["BF_PROCESS_KEY"]}' AND L_CODE = '{dr["BF_L_CODE"]}'
                                        AND WORKDATE = TO_CHAR(TO_DATE('{dr["BF_WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{dr["BF_NUM"]}'
                                        AND DEL_FLAG != 'Y'
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
                    }
                    else
                    {
                        ShowMessageBox.XtraShowInformation("전송이 완료된 작업만 취소할 수 있습니다.");
                        Dbconn.conn.Rollback();
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

            ShowMessageBox.XtraShowInformation("선택된 정보가 삭제 전송 대기로 변경 되었습니다");
        }

        private void gridView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var targets = new[] { "Y", "C", "F" };

                for (int i = 0; i < gridView.RowCount; i++)
                {
                    if (targets.Contains(gridView.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                     || targets.Contains(gridView.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                    {
                        switch (gridView.FocusedColumn.FieldName)
                        {
                            case "RUN_ST":
                            case "RUN_ET":
                            case "STOP_TIME":
                                e.Cancel = false;
                                break;
                            default:
                                e.Cancel = true;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
            }
        }

        private void gridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "ERP_ISTATUS")
                {
                    string iStatus = Convert.ToString(gridView.GetRowCellValue(e.RowHandle, "ERP_ISTATUS"));

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
                    else if (iStatus == "G" || iStatus == "X" || iStatus == "O" || iStatus == "L" || iStatus == "C")
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

                if (e.Column.FieldName == "ERP_OSTATUS")
                {
                    string oStatus = Convert.ToString(gridView.GetRowCellValue(e.RowHandle, "ERP_OSTATUS"));

                    if (oStatus == "Y")
                    {
                        e.Appearance.BackColor = Color.Black;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (oStatus == "M")
                    {
                        e.Appearance.BackColor = Color.LightBlue;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (oStatus == "G" || oStatus == "X" || oStatus == "O" || oStatus == "L" || oStatus == "C")
                    {
                        e.Appearance.BackColor = Color.OrangeRed;
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
            catch (Exception ex)
            {

            }
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            //string fromDate = string.Empty;
            //string toDate = string.Empty;

            //double sumText = 0;

            //if (clsCommon._strUserType == "010611")
            //{
            //    fromDate = dateEdit_workDate.EditValue == null
            //                ? string.Empty
            //                : Convert.ToDateTime(dateEdit_workDate.EditValue).ToString("yyyyMMdd");
            //    toDate = dateEdit_workDate.EditValue == null
            //                ? string.Empty
            //                : Convert.ToDateTime(dateEdit_workDate.EditValue).ToString("yyyyMMdd");
            //}
            //else
            //{
            //    fromDate = dateEdit_workDate.EditValue == null
            //                ? string.Empty
            //                : Convert.ToDateTime(dateEdit_workDate.EditValue).ToString("yyyyMMdd");
            //    toDate = dateEdit_workDateEd.EditValue == null
            //                ? string.Empty
            //                : Convert.ToDateTime(dateEdit_workDateEd.EditValue).ToString("yyyyMMdd");
            //}

            //string SQL = $@"
            //    WITH REST AS (
            //        SELECT NVL(SUM(REST_MINUTES), 0) AS REST_MINUTES
            //        FROM REST_TIME
            //        WHERE PROCESS_KEY = '{clsCommon.GetProcessKey("펠렛")}'
            //            AND WORKDATE = '{fromDate}'
            //    )

                
            //    SELECT ROUND(AVG(a.PROTY), 2) AS AVG_PROTY, SUM(a.QTY) AS SUM_QTY
            //    FROM PELLET_REPORT a
            //    WHERE a.PLANT_CODE = '{vPlant_Code}'
            //        AND a.PROCESS_KEY = '{vProcess_Code}'
            //        AND ('{cboL_Code.EditValue?.ToString()}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue}')
            //        AND a.WORKDATE BETWEEN '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' 
            //                        AND '{string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)}'
            //        AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
            //        AND ERP_ISTATUS = 'Y'
            //        AND a.PROTY IS NOT NULL
            //    ";

            //DataSet proDs = Dbconn.conn.ExecutDataset(SQL);

            //if (e.Column.FieldName == "PROTY")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "AVG_PROTY", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "AVG_PROTY", 0));
            //    }

            //    e.Info.DisplayText = $"AVG :        {String.Format("{0:N1}", sumText)}";
            //}

            //if (e.Column.FieldName == "QTY")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "SUM_QTY", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "SUM_QTY", 0));
            //    }

            //    e.Info.DisplayText = $"{String.Format("{0:N0}", sumText)} KG";
            //}
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

        private void gridScboResourceNo_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            string sResourceNo = textEditor.EditValue.ToString();
            string sBatchQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "BATCH_Q");

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "RESOURCE_NO", textEditor.EditValue);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "P_TYPE", 2);

            string sNOTE = clsCommon.getLastVersion(vPlant_Code, sResourceNo, out chk_version, out dNote_Per);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "NOTE", sNOTE);
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboERPUpLoad_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_ShownEditor(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (view.FocusedColumn.FieldName == "NOTE")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.getNote(vPlant_Code, view.GetFocusedRowCellValue("RESOURCE_NO")?.ToString(), "2"), "", false, 0, false, true, false);
                edit.Properties.PopupFormMinSize = new Size(200, 300);
            }
        }

        private void btnWorkStart_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("전송 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 시작 하시겠습니까?"))
            {
                return;
            }

            try
            {
                int[] selectedRows = gridView.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("시작할 작업을 선택 해주세요.");
                    return;
                }
                else if (selectedRows.Length > 1)
                {
                    ShowMessageBox.XtraShowWarning("작업은 한개만 시작 할수 있습니다.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = gridView.GetDataRow(rowHandle);

                    if (!ValidationCheck(dr)) return;

                    dr.ClearErrors();
                    SQL = $@"
                    SELECT RUN_ST
                    FROM PELLET_REPORT
                     WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {

                        if (ds.Tables[0].Rows[0]["RUN_ST"]?.ToString() != "")
                        {
                            ShowMessageBox.XtraShowWarning("이미 시작된 작업입니다.");
                            return;
                        }


                        SQL = $@"
                        /* 작업 시작시간 업데이트 */
                        UPDATE PELLET_REPORT
                        SET RUN_ST = SYSDATE, C_CONDITION = '{clsCommon.PcStatus.InProgress}'
                            , ERP_ERR_CNT = 0
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btnWorkStart_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업 시작이 실패했습니다");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btnWorkStart_Click", ex.Message + "/" + ex.StackTrace);
            }

            XMain_Search();

            ShowMessageBox.XtraShowInformation("선택된 작업이 시작 되었습니다.");
        }

        private void btnWorkEnd_Click(object sender, EventArgs e)
        {
            string pellet = string.Empty;

            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("전송 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 종료 하시겠습니까?"))
            {
                return;
            }

            try
            {
                int[] selectedRows = gridView.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("종료할 작업을 선택 해주세요.");
                    return;
                }
                else if (selectedRows.Length > 1)
                {
                    ShowMessageBox.XtraShowWarning("작업은 한개만 종료 할수 있습니다.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = gridView.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT RUN_ST, RUN_ET, QTY
                    FROM PELLET_REPORT
                     WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        if (ds.Tables[0].Rows[0]["QTY"]?.ToString() == "")
                        {
                            ShowMessageBox.XtraShowWarning("생산량을 먼저 저장 후 종료 해주세요.");
                            return;
                        }

                        if (ds.Tables[0].Rows[0]["RUN_ST"]?.ToString() == "")
                        {
                            ShowMessageBox.XtraShowWarning("작업 시작 먼저 해주세요.");
                            return;
                        }

                        if (ds.Tables[0].Rows[0]["RUN_ET"]?.ToString() != "")
                        {
                            ShowMessageBox.XtraShowWarning("이미 종료된 작업입니다.");
                            return;
                        }

                        Dbconn.conn.BeginTransaction();

                        if (!SetPROTY(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["L_CODE"]?.ToString(), dr["WORKDATE"]?.ToString(), dr["WORK_SEQ"]?.ToString()))
                        {
                            Dbconn.conn.Rollback();
                            ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                            return;
                        }

                        clsCommon.GetAutoPellet(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), out pellet);

                        SQL = $@"
                        /* 작업 종료시간 업데이트 */
                        UPDATE PELLET_REPORT
                        SET RUN_ET = SYSDATE, C_CONDITION = '{clsCommon.PcStatus.Completed}'
                            , ERP_OSTATUS = CASE WHEN '{pellet}' IS NULL 
                                                THEN CASE WHEN '{clsCommon.GetTransAuto(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString())}' = 'Y' 
                                                        THEN 'F' 
                                                        ELSE 'N' 
                                                    END 
                                                ELSE 'B'
                                            END
                            , ERP_ISTATUS = CASE WHEN '{clsCommon.GetTransAuto(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString())}' = 'Y' THEN 'F' ELSE 'N' END
                            , ERP_OERR_CNT = 0
                            , ERP_IERR_CNT = 0
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                            AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btnWorkStart_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btnWorkStart_Click", ex.Message + "/" + ex.StackTrace);
            }

            XMain_Search();

            ShowMessageBox.XtraShowInformation("선택된 작업이 종료 되었습니다.");
        }

        /// <summary>
        /// 생산성 업데이트
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sProcessKey"></param>
        /// <param name="sLCode"></param>
        /// <param name="sWorkDate"></param>
        /// <param name="sWorkSeq"></param>
        private bool SetPROTY(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate, string sWorkSeq)
        {
            bool returnStatus = true;

            SQL = $@"
            SELECT a.RUN_ST, SYSDATE AS RUN_ET, a.QTY, a.STOP_TIME
            FROM PELLET_REPORT a
            WHERE a.PLANT_CODE = '{sPlantCode}' AND a.PROCESS_KEY = '{sProcessKey}' AND a.L_CODE = '{sLCode}'
                    AND a.WORKDATE = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND a.WORK_SEQ = '{sWorkSeq}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
            {
                if (ds.Tables[0].Rows[0]["RUN_ST"]?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("작업 시작 먼저 해주세요.");
                    return !returnStatus;
                }

                if (ds.Tables[0].Rows[0]["RUN_ET"]?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("이미 종료된 작업입니다.");
                    return !returnStatus;
                }

                //if (ds.Tables[0].Rows[0]["QTY"]?.ToString() == "")
                //{
                //    ShowMessageBox.XtraShowWarning("생산량을 입력 해주세요.");
                //    return;
                //}

                DateTime runSt = Convert.ToDateTime(ds.Tables[0].Rows[0]["RUN_ST"]);
                DateTime runEt = Convert.ToDateTime(ds.Tables[0].Rows[0]["RUN_ET"]);

                double stopT = 0;
                double.TryParse(
                    ds.Tables[0].Rows[0]["STOP_TIME"]?.ToString(),
                    out stopT
                );

                // 시간 차이
                double diffHours = (runEt - runSt - TimeSpan.FromMinutes(stopT)).TotalHours;

                //if (diffHours < 0.1)
                //{
                //    ShowMessageBox.XtraShowWarning("종료시간이 너무 빠릅니다.");
                //    return !returnStatus;
                //}

                // 분 차이
                double diffMinutes = (runEt - runSt - TimeSpan.FromMinutes(stopT)).TotalMinutes;

                SQL = $@"
                /* 생산성 업데이트 */
                UPDATE PELLET_REPORT
                SET PROTY = CASE WHEN '{diffHours.ToString("F1")}' = 0 THEN 0 ELSE (QTY / 1000) / {diffHours.ToString("F1")} END, TOTAL_OPER = '{diffMinutes.ToString("F1")}'
                WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                        AND WORKDATE = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{sWorkSeq}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btnWorkStart_Click", SQL);
                    ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                    return !returnStatus;
                }
            }

            return returnStatus;
        }

        private void gridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName;

            string sPlantCode = gridView.GetRowCellValue(e.RowHandle, "PLANT_CODE")?.ToString();
            string sProcessKey = gridView.GetRowCellValue(e.RowHandle, "PROCESS_KEY")?.ToString();
            string sLCode = gridView.GetRowCellValue(e.RowHandle, "L_CODE")?.ToString();
            string sWorkDate = gridView.GetRowCellValue(e.RowHandle, "WORKDATE")?.ToString();
            string sWorkSeq = gridView.GetRowCellValue(e.RowHandle, "WORK_SEQ")?.ToString();

            double diffHours = 0;
            double diffMinutes = 0;

            object vRunSt = gridView.GetRowCellValue(e.RowHandle, "RUN_ST");

            DateTime? runSt = null;
            if (vRunSt != null && vRunSt != DBNull.Value && !string.IsNullOrWhiteSpace(vRunSt.ToString()))
            {
                runSt = Convert.ToDateTime(vRunSt);
            }

            object vRunEt = gridView.GetRowCellValue(e.RowHandle, "RUN_ET");

            DateTime? runEt = null;
            if (vRunEt != null && vRunEt != DBNull.Value && !string.IsNullOrWhiteSpace(vRunEt.ToString()))
            {
                runEt = Convert.ToDateTime(vRunEt);
            }

            double stopT = gridView.GetRowCellValue(e.RowHandle, "STOP_TIME") == DBNull.Value
                            ? 0
                            : Convert.ToDouble(gridView.GetRowCellValue(e.RowHandle, "STOP_TIME"));

            string sQty = gridView.GetRowCellValue(e.RowHandle, "QTY")?.ToString();

            if (string.IsNullOrWhiteSpace(sQty) || !runSt.HasValue || !runEt.HasValue)
            {
                return;
            }

            if (fieldName == "RUN_ST" || fieldName == "RUN_ET" || fieldName == "QTY" || fieldName == "STOP_TIME")
            {
                diffHours = (runEt - runSt - TimeSpan.FromMinutes(stopT)).Value.TotalHours;
                diffMinutes = (runEt - runSt - TimeSpan.FromMinutes(stopT)).Value.TotalMinutes;

                if (diffHours < 0.1)
                {
                    ShowMessageBox.XtraShowWarning("종료시간이 너무 빠릅니다.");
                    return;
                }

                gridView.SetFocusedRowCellValue("TOTAL_OPER", diffMinutes.ToString("F1"));

                double result = (Convert.ToDouble(sQty) / 1000) / diffHours;

                gridView.SetFocusedRowCellValue("PROTY", result.ToString("F1"));
            }
        }

        private void btnWorkCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                string scPlantCode = gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                string scProcesskey = gridView.GetFocusedRowCellValue("PROCESS_KEY")?.ToString();
                string scLCode = gridView.GetFocusedRowCellValue("L_CODE")?.ToString();
                string scBWorkDate = gridView.GetFocusedRowCellValue("WORKDATE").ToString().Replace("-", "");
                string scNum = gridView.GetFocusedRowCellValue("WORK_SEQ")?.ToString();
                string scResourceNo = gridView.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();

                m_WorkDateCopy child = new m_WorkDateCopy(scPlantCode, scProcesskey, scLCode, scBWorkDate, scNum);

                child.StartPosition = FormStartPosition.CenterParent;
                if (child.ShowDialog() == DialogResult.OK)
                {
                    string sAWorkDate = child.SelectedWorkDate;

                    string WORK_SEQ = workNumber_maker(scPlantCode, scProcesskey, scLCode, string.Format("{0:yyyyMMdd}", scBWorkDate));

                    if (string.IsNullOrEmpty(WORK_SEQ))
                    {
                        ShowMessageBox.XtraShowInformation("작업순번을 생성하는 도중 에러가 발생했습니다");
                        return;
                    }

                    string pellet = string.Empty;

                    clsCommon.GetAutoPellet(scPlantCode, scProcesskey, scResourceNo, out pellet);

                    SQL = $@"
                    INSERT INTO PELLET_REPORT 
                        (PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, WORK_SEQ                                    -- 1
                        , P_TYPE, RESOURCE_NO, NOTE, HALT_TIME, RUN_ST                                          -- 2
                        , RUN_ET, BF_PLANT_CODE, BF_PROCESS_KEY, BF_L_CODE, BF_WORKDATE                         -- 3
                        , BF_NUM, BF_QTY, QTY, REMAIN_QTY, PROTY                                                -- 4
                        , TOTAL_OPER, DY_ST, DY_THICK, WORK_START_DATE, ICM_CODE                                -- 5
                        , EMPLOYEE_NO, DY_SP, CURRENT_1, CURRENT_2, FEEDER_RATE                                 -- 6
                        , CRUMBLE_YN, HARDNESS, PDI, CLEAN_QTY, LOCATION_ST1                                    -- 7
                        , LOCATION_ST2, LOCATION_ED1, LOCATION_ED2, HZ                                          -- 8
                        , CD_TEMP, P_TEMP, COL_TEMP, REMARK, DEL_FLAG                                           -- 9
                        , ERP_UP_YN, ERP_TNUMBER, I_TIME, ERP_ERR_CNT, SEQ                                      -- 10
                        , ERP_ISTATUS, ERP_ITNUMBER, ERP_OSTATUS, ERP_OTNUMBER, ERP_IERR_CNT                    -- 11
                        , ERP_OERR_CNT, BF_PACK_TYPE, ERR_MSG, WORK_SEQ_COPY, C_CONDITION)                      -- 12
                    SELECT 
                        PLANT_CODE, PROCESS_KEY, L_CODE, '{sAWorkDate}', '{WORK_SEQ}'                                 -- 1
                        , P_TYPE, RESOURCE_NO, NOTE, HALT_TIME, RUN_ST                                          -- 2
                        , RUN_ET, BF_PLANT_CODE, BF_PROCESS_KEY, BF_L_CODE, BF_WORKDATE                         -- 3
                        , BF_NUM, BF_QTY, QTY, REMAIN_QTY, PROTY                                                -- 4
                        , TOTAL_OPER, DY_ST, DY_THICK, WORK_START_DATE, ICM_CODE                                -- 5
                        , '{clsCommon.UserId}', DY_SP, CURRENT_1, CURRENT_2, FEEDER_RATE                        -- 6
                        , CRUMBLE_YN, HARDNESS, PDI, CLEAN_QTY, LOCATION_ST1                                    -- 7
                        , LOCATION_ST2, LOCATION_ED1, LOCATION_ED2, HZ                                          -- 8
                        , CD_TEMP, P_TEMP, COL_TEMP, '※{scBWorkDate} {scNum} 작업복사(복사 내용 수정/삭제 금지)', 'N'                                           -- 9
                        , ERP_UP_YN, ERP_TNUMBER, SYSDATE, ERP_ERR_CNT, '{WORK_SEQ}'                                     -- 10
                        , 'W', NULL, CASE WHEN '{pellet}' IS NOT NULL THEN 'B' ELSE 'W' END, NULL, 0                                                               -- 11
                        , 0, BF_PACK_TYPE, NULL, WORK_SEQ, '{clsCommon.GetPcStatusCode("계획")}'                -- 12
                    FROM PELLET_REPORT
                    WHERE PLANT_CODE = '{scPlantCode}'
                        AND PROCESS_KEY = '{scProcesskey}'
                        AND L_CODE = '{scLCode}'
                        AND WORKDATE = '{scBWorkDate}'
                        AND WORK_SEQ = '{scNum}' 
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        clsLog.logSave(this.Text, "btnWorkCopy_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 복제에 실패했습니다");
                        return;
                    }

                    ShowMessageBox.XtraShowInformation("작업 복사가 완료 되었습니다.");
                    XMain_Search();
                }
            }
            catch
            {

            }
        }

        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

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
                int[] selectedRows = gridView.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                string pellet = string.Empty;

                foreach (int rowHandle in selectedRows)
                {
                    var dr = gridView.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    clsCommon.GetAutoPellet(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), out pellet);

                    SQL = $@"
                    UPDATE PELLET_REPORT
                    SET   ERP_OSTATUS = CASE WHEN '{pellet}' IS NULL 
                                            THEN CASE TO_CHAR(ERP_UP_YN) 
                                                    WHEN 'N' THEN 'F'
                                                    WHEN 'W' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                ELSE TO_CHAR(ERP_UP_YN) END
                                        ELSE 'B'
                                        END
                        , ERP_OERR_CNT = 0
                        , ERP_ISTATUS = CASE TO_CHAR(ERP_ISTATUS) 
                                            WHEN 'N' THEN 'F'
                                            WHEN 'W' THEN 'F'
                                            WHEN 'M' THEN 'U'
                                            WHEN 'X' THEN 'D'
                                            WHEN 'G' THEN 'F'
                                            WHEN 'L' THEN 'U'
                                            WHEN 'R' THEN 'D'
                                            WHEN NULL THEN 'N'
                                        ELSE TO_CHAR(ERP_ISTATUS) END
                        , ERP_IERR_CNT = 0
                    WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                            AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
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

            ShowMessageBox.XtraShowInformation("선택된 정보가 전송 대기로 변경 되었습니다");
        }

        private void btn_AddSave_Click(object sender, EventArgs e)
        {
            try
            {
                int iRowIdx = 0;
                string lCode = string.Empty;
                clsDevexpressGrid.GridEndEdit(viewSub);
                DataTable DT = (DataTable)gridSub.DataSource;

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

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE PELLET_REPORT_ADD
                           SET START_TIME      = '{dr["START_TIME"]}'               /* 06 */
                             , END_TIME        = '{dr["END_TIME"]}'                 /* 07 */
                             , PROD_TIME       = '{dr["PROD_TIME"]}'                /* 08 */
                             , AMP_1ST         = '{dr["AMP_1ST"]}'                  /* 09 */
                             , AMP_2ND         = '{dr["AMP_2ND"]}'                  /* 10 */
                             , F_R_RATIO       = '{dr["F_R_RATIO"]}'                /* 11 */
                             , TEMP_1ST        = '{dr["TEMP_1ST"]}'                 /* 12 */
                             , TEMP_2ND        = '{dr["TEMP_2ND"]}'                 /* 13 */
                             , GAP_VALUE       = '{dr["GAP_VALUE"]}'                /* 14 */
                             , ROLL_GAP        = '{dr["ROLL_GAP"]}'                 /* 15 */
                             , DURABILITY      = '{dr["DURABILITY"]}'               /* 16 */
                             , MESH_6          = '{dr["MESH_6"]}'                   /* 17 */
                             , MESH_8          = '{dr["MESH_8"]}'                   /* 18 */
                             , MESH_10         = '{dr["MESH_10"]}'                  /* 19 */
                             , MESH_18         = '{dr["MESH_18"]}'                  /* 20 */
                             , MESH_35         = '{dr["MESH_35"]}'                  /* 21 */
                             , MESH_PAN        = '{dr["MESH_PAN"]}'                 /* 22 */
                             , STOP_TIME       = '{dr["STOP_TIME"]}'                /* 23 */
                             , DY_ST           = '{dr["DY_ST"]}'                    /* 24 */
                             , DY_THICK        = '{dr["DY_THICK"]}'                 /* 25 */
                             , DY_SP           = '{dr["DY_SP"]}'                    /* 27 */
                             , CURRENT_1       = '{dr["CURRENT_1"]}'                /* 28 */
                             , CURRENT_2       = '{dr["CURRENT_2"]}'                /* 29 */
                             , FEEDER_RATE     = '{dr["FEEDER_RATE"]}'              /* 30 */
                             , CRUMBLE_YN      = '{dr["CRUMBLE_YN"]}'               /* 31 */
                             , HARDNESS        = '{dr["HARDNESS"]}'                 /* 32 */
                             , PDI             = '{dr["PDI"]}'                      /* 33 */
                             , CLEAN_QTY       = '{dr["CLEAN_QTY"]}'                /* 34 */
                             , HZ              = '{dr["HZ"]}'                       /* 35 */
                             , CD_TEMP         = '{dr["CD_TEMP"]}'                  /* 36 */
                             , P_TEMP          = '{dr["P_TEMP"]}'                   /* 37 */
                             , COL_TEMP        = '{dr["COL_TEMP"]}'                 /* 38 */
                             , B_W             = '{dr["B_W"]}'
                         WHERE PLANT_CODE  = '{dr["PLANT_CODE"]}'
                           AND PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                           AND L_CODE      = '{dr["L_CODE"]}'
                           AND WORKDATE    = '{dr["WORKDATE"]}'
                           AND WORK_SEQ    = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach

                ShowMessageBox.XtraShowInformation("작업일지가 저장 되었습니다.");

                Dbconn.conn.Commit();

                viewSub.RefreshData();
                XMain_Search();

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            XSub_Search();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
                XMain_Search();
            else
                XList_Search();
        }

        private decimal sumQty = 0;
        private decimal sumHour = 0;

        private void gridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            var item = e.Item as DevExpress.XtraGrid.GridSummaryItem;

            if (item == null || item.FieldName != "PROTY")
                return;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                sumQty = 0;
                sumHour = 0;
            }

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {
                int rowHandle = e.RowHandle;

                decimal qty = Convert.ToDecimal(view.GetRowCellValue(rowHandle, "QTY") ?? 0);
                object obj = view.GetRowCellValue(rowHandle, "WORK_HOUR");

                decimal hour = obj == DBNull.Value ? 0 : Convert.ToDecimal(obj);

                sumQty += qty;
                sumHour += hour;
            }

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                e.TotalValue = sumHour == 0 ? 0 : Math.Round((sumQty / 1000) / sumHour, 2);
            }
        }
    }
}