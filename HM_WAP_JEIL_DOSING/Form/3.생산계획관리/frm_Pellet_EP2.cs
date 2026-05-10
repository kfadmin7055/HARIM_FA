using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser.CodeStyle.Formatting;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.Schedule;
using DevExpress.XtraEditors;
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
using System.Linq;
using System.Reflection.Emit;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Pellet_EP2 : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;
        private string[] sValid = null;

        private string formname = string.Empty;
        private string vProcessName = string.Empty;

        private string vPlant_Code = string.Empty;
        private string vProcess_Code = string.Empty;
        private string vLine_Code = string.Empty;

        string sPLANT_CODE = string.Empty;
        string sProcessKey = string.Empty;
        string sLcode = string.Empty;
        string resourceNo = string.Empty;
        string note = string.Empty;
        string sWorkDate = string.Empty;
        string sWorkSeq = string.Empty;

        bool chk_version = false;
        decimal dNote_Per = 0;

        private bool isInitializing = false;

        public frm_Pellet_EP2()
        {
            InitializeComponent();

            vPlant_Code = clsCommon.PlantCode;
            vProcess_Code = clsCommon.GetProcessKey("EP가공");

            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
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

        public frm_Pellet_EP2(string plant_code, string process_key, string formName, string sProcessName)
        {
            InitializeComponent();
            formname = formName;

            vPlant_Code = plant_code;
            vProcess_Code = process_key;
            vProcessName = sProcessName;

            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Pellet_EP2_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            authDs = clsSql.GetAuthDataSet(this.Name);

            isInitializing = true;

            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            // ERP 진행여부
            //clsDevexpressUtil.ItemLookUpEditSetup(cboERPUpLoad, clsCommon.GetTransFlag(), "", false, 0, true);

            dateEdit_workDate.EditValue = DateTime.Today;
            dateEdit_workDateEd.EditValue = DateTime.Today.AddDays(1);

            InitControl();

            XMain_Search();
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
            #endregion
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                    PLANT_CODE, PROCESS_KEY, L_CODE, 
                    WORKDATE, WORK_SEQ, SEQ, P_TYPE, 
                    RESOURCE_NO, NOTE, HALT_TIME, 
                    RUN_ST, RUN_ET, BF_PLANT_CODE, BF_PROCESS_KEY, BF_L_CODE,
                    BF_WORKDATE, BF_NUM, BF_QTY, 
                    QTY, REMAIN_QTY, PROTY, 
                    TOTAL_OPER, DY_ST, DY_THICK, 
                    WORK_START_DATE, ICM_CODE, EMPLOYEE_NO, 
                    DY_SP, CURRENT_1, CURRENT_2, 
                    FEEDER_RATE, CRUMBLE_YN, HARDNESS, 
                    PDI, CLEAN_QTY, LOCATION_ST1, 
                    LOCATION_ST2, LOCATION_ED1, LOCATION_ED2, 
                    HZ, CD_TEMP, P_TEMP, 
                    COL_TEMP, REMARK, DEL_FLAG, 
                    ERP_UP_YN, ERP_TNUMBER, I_TIME
                    , ERP_ISTATUS
                    , ERP_ITNUMBER
                    , ERP_OSTATUS
                    , ERP_OTNUMBER
                    , ERR_MSG
                FROM PELLET_REPORT
                WHERE PLANT_CODE = '{vPlant_Code}'
                    AND PROCESS_KEY = '{vProcess_Code}'
                    AND ('{cboL_Code.EditValue?.ToString()}' IS NULL OR L_CODE = '{cboL_Code.EditValue}')
                    AND WORKDATE BETWEEN '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' 
                                    AND '{string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)}'
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                ORDER BY ERP_UP_YN DESC,WORKDATE DESC, RUN_ST, SEQ ASC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true, true);

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

        private void GetPelletRecipe()
        {
            try
            {
                SQL = $@"
                -- EP 펠렛 배합 정보
                SELECT 'N' AS CHK, a.PLANT_CODE, a.P_TYPE, a.RESOURCE_NO, a.NOTE
                    , a.MIX_TIME, a.DRY_TIME, a.FINAL_TIME, a.LR_YN, a.STLST
                    , a.REMARK_1, a.CR_YN, a.REMARK_2, a.USE_YN, a.EMPLOYEE_NO
                    , b.IDNRK || ' : ' || c.DESCRIPTION AS IDNRK, b.STLAN, b.STLNR, b.STLAL, b.BMEIN, b.LKENZ, b.POSNR
                    , b.MENGE, b.MEINS, b.AUSCH, b.KZKUP
                    , b.ALPOS, b.ALPGR, b.EWAHR, b.SANFE, b.SANKA, b.BEIKZ
                    , b.DATUV, b.DATUV_TO, b.AENNR, b.SORTF, b.LGORT, b.POTX1, b.SEQ, b.XSEQNR
                FROM SAP_IN_BOM_CONM a
                    JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.P_TYPE = a.P_TYPE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                    JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = b.IDNRK
                WHERE a.PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}' AND a.P_TYPE = '2' AND a.RESOURCE_NO = '{gridView.GetFocusedRowCellValue("RESOURCE_NO")}'
                    AND ('{gridView.GetFocusedRowCellValue("NOTE")}' IS NULL OR a.NOTE = '{gridView.GetFocusedRowCellValue("NOTE")}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridIngred, viewIngred, ds.Tables[0], false, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "GetPelletRecipe(string workdate, string num)", ex);
                clsLog.logSave(this, "GetPelletRecipe(string workdate, string num)", ex.StackTrace);
                ShowMessageBox.XtraShowError("작업지시내역을 조회하는중 오류가 발생했습니다.");
            }
        }

        private void GetPelletERPResult()
        {
            try
            {
                SQL = $@"
                -- ERP 실적
                SELECT 
                   a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.WORK_SEQ  
                   , a.RESOURCE_NO || ' : ' || c.DESCRIPTION AS RESOURCE_NO, a.P_Q, a.I_TIME
                FROM PELLET_REMARK a
                    JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND a.PROCESS_KEY = 'SAP_{gridView.GetFocusedRowCellValue("PROCESS_KEY")}'
                    AND a.L_CODE = '{gridView.GetFocusedRowCellValue("L_CODE")}'
                    AND a.WORKDATE = '{gridView.GetFocusedRowCellValue("WORKDATE")}'
                    AND a.WORK_SEQ = '{gridView.GetFocusedRowCellValue("WORK_SEQ")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                clsDevexpressGrid.BindGridControl(gridERPPack, viewERPPack, ds.Tables[0], false, true);

                sValid = new string[] { "" };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridErpScboResourceNo, clsCommon.GetResource(vPlant_Code));

                // 단위
                clsDevexpressGrid.ItemLookUpEditSetup(gridErpCboPCStatus, clsCommon.GetPcStatus());
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "work_result(string workdate, string num)", ex);
                clsLog.logSave(this, "work_result(string workdate, string num)", ex.StackTrace);
                ShowMessageBox.XtraShowError("작업지시내역을 조회하는중 오류가 발생했습니다.");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
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

            //gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["RUN_ST"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["RUN_ET"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["EMPLOYEE_NO"], clsCommon.UserId);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["BF_PROCESS_KEY"], clsCommon.GetProcessKey("배합"));

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
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("작업지시정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
                string lCode = string.Empty;
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

                        DateTime dtFrom = DateTime.Parse(dr["RUN_ST"].ToString());
                        DateTime dtTo = DateTime.Parse(dr["RUN_ET"].ToString());

                        sDtFrom = $"TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                        sDtTo = $"TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
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
                            , ERP_ISTATUS
                            , ERP_OSTATUS
                        )
                        VALUES (
                             '{dr["PLANT_CODE"]}'                                       -- 01
                            , '{dr["PROCESS_KEY"]}'      -- 02
                            , '{dr["L_CODE"]}'           -- 03
                            , '{dr["WORKDATE"]}'         -- 04
                            , '{sWorkSeq}'               -- 05
                            , '2'                        -- 06
                            , '{dr["RESOURCE_NO"]}'      -- 07
                            , '{dr["NOTE"]}'             -- 08
                            , '{dr["HALT_TIME"]}'        -- 09
                            , ({(string.IsNullOrEmpty(sDtFrom) ? "''" : $"{sDtFrom}")})
                            , ({(string.IsNullOrEmpty(sDtTo) ? "''" : $"{sDtTo}")})           -- 11
                            , '{dr["BF_PROCESS_KEY"]}'   -- 12
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
                        WHERE
                                PLANT_CODE       = '{dr["PLANT_CODE"]}'
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

                        //string erp_insert_chk = clsErpSql.InsertWorkOrder(clsCommon.pellet_process_code, dr["WORKDATE"].ToString(), dr["WORK_SEQ"].ToString(), dr, "U");
                        //if (erp_insert_chk != "OK")
                        //{
                        //    Dbconn.conn.Rollback();
                        //    ShowMessageBox.XtraShowWarning("작업지시 수정에 실패했습니다(ERP INPUT FAIL)");
                        //    return;
                        //}

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
                        ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                        return;
                    }

                    foreach (int rowHandle in selectedRows)
                    {
                        var dr = gridView.GetDataRow(rowHandle);

                        dr.ClearErrors();

                        string condition = gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["ICM_CODE"]);

                        if (condition.Equals("완료"))
                        {
                            ShowMessageBox.XtraShowInformation("완료된 작업지시는 삭제하실 수 없습니다");
                            return;
                        }

                        Dbconn.conn.BeginTransaction();
                        //delete work num
                        SQL = $@"
                        UPDATE PELLET_REPORT SET DEL_FLAG = 'Y', ERP_UP_YN = 'X'
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
                        e.Cancel = true;        // 수정 불가
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
            if (e.Column.FieldName == "PROTY")
            {
                int sumText = 0;

                SQL =
                "SELECT FLOOR(NVL( ( SUM(A.PRO_Q) / (SUM(A.MIN_SUM)   " +
                "- (SELECT NVL(SUM(REST_MINUTES),0) FROM REST_TIME WHERE PROCESS_KEY = '{0}' AND WORKDATE BETWEEN '{1}' AND '{2}')) " +
                ")  " +
                "* 60 , 0)) AS PROVITY " +
                " FROM (  " +
                "SELECT   " +
                "WORK_START_DATE, SUM(QTY) AS PRO_Q,  DATEDIFF(MINUTE, MIN(RUN_ST), MAX(RUN_ET)) AS MIN_SUM  " +
                "FROM PELLET_REPORT   " +
                "WHERE NVL(DEL_FLAG,'N') != 'Y'     " +
                "AND WORK_START_DATE BETWEEN '{1}' AND '{2}'  " +
                "AND DATEDIFF(MINUTE, RUN_ST, RUN_ET) > 0 " +
                "GROUP BY WORK_START_DATE  " +
                ") A  ";

                if (clsCommon._strUserType == "010611")
                {
                    SQL = string.Format(SQL,
                         "clsCommon.pellet_process_code",
                         string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),
                         string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)
                     );

                }
                else
                {
                    SQL = string.Format(SQL,
                         "clsCommon.pellet_process_code",
                         string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue),
                         string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)
                     );
                }


                DataSet proDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(proDs) > 0)
                {

                    sumText = Convert.ToInt32(Dbconn.conn.getData(proDs, "PROVITY", 0));


                }

                e.Info.DisplayText = String.Format("{0:#,###}", sumText);
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
                        SET RUN_ST = SYSDATE
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
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("종료 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 종료(완료) 하시겠습니까?"))
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

                    Dbconn.conn.BeginTransaction();

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

                        //if (ds.Tables[0].Rows[0]["RUN_ET"]?.ToString() != "")
                        //{
                        //    ShowMessageBox.XtraShowWarning("이미 종료된 작업입니다.");
                        //    return;
                        //}

                        SQL = $@"
                        /* 작업 종료시간 업데이트 */
                        UPDATE PELLET_REPORT
                        SET RUN_ET = SYSDATE
                            , ERP_ERR_CNT = 0
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                            AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btnWorkStart_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                            return;
                        }

                        SetPROTY(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["L_CODE"]?.ToString(), dr["WORKDATE"]?.ToString(), dr["WORK_SEQ"]?.ToString());

                        SQL = $@"
                        MERGE INTO PELLET_REMARK D
                        USING (SELECT 
                                    a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.WORK_SEQ  
                                    , b.IDNRK, a.BF_QTY * b.MENGE AS P_Q
                                FROM PELLET_REPORT a
                                    JOIN (SELECT PLANT_CODE, RESOURCE_NO, IDNRK, (SUM(MENGE) / 100) AS MENGE
                                            FROM SAP_IN_BOM_COND
                                            WHERE P_TYPE = '2'
                                                AND NOTE = '{dr["NOTE"]}'
                                            GROUP BY PLANT_CODE, RESOURCE_NO, IDNRK) b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                                WHERE a.PLANT_CODE = '{dr["PLANT_CODE"]}'
                                    AND a.PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                                    AND a.L_CODE = '{dr["L_CODE"]}'
                                    AND a.WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD')
                                    AND a.WORK_SEQ = '{dr["WORK_SEQ"]}'
                                UNION ALL
                                SELECT 
                                    a.PLANT_CODE, 'SAP_' || a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.WORK_SEQ  
                                    , b.IDNRK, a.BF_QTY * b.MENGE AS P_Q
                                FROM PELLET_REPORT a
                                    JOIN (SELECT PLANT_CODE, RESOURCE_NO, IDNRK, (SUM(MENGE) / 100) AS MENGE
                                            FROM SAP_IN_BOM_COND
                                            WHERE P_TYPE = '2'
                                                AND NOTE = '{dr["NOTE"]}'
                                            GROUP BY PLANT_CODE, RESOURCE_NO, IDNRK) b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                                WHERE a.PLANT_CODE = '{dr["PLANT_CODE"]}'
                                    AND a.PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                                    AND a.L_CODE = '{dr["L_CODE"]}'
                                    AND a.WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD')
                                    AND a.WORK_SEQ = '{dr["WORK_SEQ"]}'
                             ) s
                        ON (s.PLANT_CODE = D.PLANT_CODE
                            AND s.PROCESS_KEY = D.PROCESS_KEY
                            AND s.L_CODE      = D.L_CODE     
                            AND s.WORKDATE    = D.WORKDATE   
                            AND s.WORK_SEQ    = D.WORK_SEQ   
                        )WHEN MATCHED
                                THEN
                                    UPDATE SET d.I_TIME = SYSDATE
                                            , d.P_Q = s.P_Q
                                            , d.RESOURCE_NO = s.IDNRK
                            WHEN NOT MATCHED
                            THEN
                                INSERT     (I_TIME
                                          , L_CODE
                                          , P_Q
                                          , PLANT_CODE
                                          , PROCESS_KEY
                                          , RESOURCE_NO
                                          , WORK_SEQ
                                          , WORKDATE)
                                    VALUES (
                                            SYSDATE
                                          , s.L_CODE
                                          , s.P_Q
                                          , s.PLANT_CODE
                                          , s.PROCESS_KEY
                                          , s.IDNRK
                                          , s.WORK_SEQ
                                          , s.WORKDATE
                                           )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("실적 생성에 실패했습니다.");
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

            ShowMessageBox.XtraShowInformation("선택된 작업이 종료(완료) 되었습니다.");
        }

        private void SetPROTY(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate, string sWorkSeq)
        {
            SQL = $@"
            SELECT RUN_ST, SYSDATE AS RUN_ET, QTY
            FROM PELLET_REPORT
                WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                        AND WORKDATE = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{sWorkSeq}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
            {
                if (ds.Tables[0].Rows[0]["RUN_ST"]?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("작업 시작 먼저 해주세요.");
                    return;
                }

                if (ds.Tables[0].Rows[0]["RUN_ET"]?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("이미 종료된 작업입니다.");
                    return;
                }

                if (ds.Tables[0].Rows[0]["QTY"]?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("생산량을 입력 해주세요.");
                    return;
                }

                DateTime runSt = Convert.ToDateTime(ds.Tables[0].Rows[0]["RUN_ST"]);
                DateTime runEt = Convert.ToDateTime(ds.Tables[0].Rows[0]["RUN_ET"]);

                // 시간 차이
                double diffHours = (runEt - runSt).TotalHours;

                // 분 차이
                double diffMinutes = (runEt - runSt).TotalMinutes;

                if (diffHours > 0.1)
                {
                    SQL = $@"
                    /* 생산성 업데이트 */
                    UPDATE PELLET_REPORT
                    SET PROTY = (QTY / 1000) / {diffHours.ToString("F1")}, TOTAL_OPER = '{diffMinutes.ToString("F1")}'
                    WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                            AND WORKDATE = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND WORK_SEQ = '{sWorkSeq}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        clsLog.logSave(this.Text, "btnWorkStart_Click", SQL);
                        ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                        return;
                    }
                }
            }
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

            string sQty = gridView.GetRowCellValue(e.RowHandle, "QTY")?.ToString();

            if (string.IsNullOrWhiteSpace(sQty) || !runSt.HasValue || !runEt.HasValue)
            {
                return;
            }

            if (fieldName == "RUN_ST" || fieldName == "RUN_ET" || fieldName == "QTY")
            {
                diffHours = (runEt - runSt).Value.TotalHours;
                diffMinutes = (runEt - runSt).Value.TotalMinutes;

                gridView.SetFocusedRowCellValue("TOTAL_OPER", diffMinutes.ToString("F1"));

                double result = (Convert.ToDouble(sQty) / 1000) / diffHours;

                gridView.SetFocusedRowCellValue("PROTY", result.ToString("F1"));
            }
        }

        private void gridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            gridIngred.DataSource = null;
            gridERPPack.DataSource = null;

            GetPelletRecipe();
            GetPelletERPResult();
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

                string pellet = string.Empty;
                string WORK_SEQ = workNumber_maker(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), gridView.GetFocusedRowCellValue("PROCESS_KEY")?.ToString(), gridView.GetFocusedRowCellValue("L_CODE")?.ToString(), gridView.GetFocusedRowCellValue("WORKDATE")?.ToString());

                if (string.IsNullOrEmpty(WORK_SEQ))
                {
                    ShowMessageBox.XtraShowInformation("작업순번을 생성하는 도중 에러가 발생했습니다");
                    return;
                }

                clsCommon.GetAutoPellet(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), gridView.GetFocusedRowCellValue("PROCESS_KEY")?.ToString(), gridView.GetFocusedRowCellValue("RESOURCE_NO")?.ToString(), out pellet);

                SQL = $@"
                INSERT INTO PELLET_REPORT 
                    (PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, WORK_SEQ                                                -- 1
                    , P_TYPE, RESOURCE_NO, NOTE, HALT_TIME, RUN_ST                                                      -- 2
                    , RUN_ET, BF_PLANT_CODE, BF_PROCESS_KEY, BF_L_CODE, BF_WORKDATE                                     -- 3
                    , BF_NUM, BF_QTY, QTY, REMAIN_QTY, PROTY                                                            -- 4
                    , TOTAL_OPER, DY_ST, DY_THICK, WORK_START_DATE, ICM_CODE                                            -- 5
                    , EMPLOYEE_NO, DY_SP, CURRENT_1, CURRENT_2, FEEDER_RATE                                             -- 6
                    , CRUMBLE_YN, HARDNESS, PDI, CLEAN_QTY, LOCATION_ST1                                                -- 7
                    , LOCATION_ST2, LOCATION_ED1, LOCATION_ED2, HZ                                                      -- 8
                    , CD_TEMP, P_TEMP, COL_TEMP, REMARK, DEL_FLAG                                                       -- 9
                    , ERP_UP_YN, ERP_TNUMBER, I_TIME, ERP_ERR_CNT, SEQ                                                  -- 10
                    , ERP_ISTATUS, ERP_ITNUMBER, ERP_OSTATUS, ERP_OTNUMBER, ERP_IERR_CNT                                -- 11
                    , ERP_OERR_CNT, BF_PACK_TYPE, ERR_MSG, WORK_SEQ_COPY, C_CONDITION)                                  -- 12
                SELECT 
                    PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, '{WORK_SEQ}'                                             -- 1
                    , P_TYPE, RESOURCE_NO, NOTE, HALT_TIME, RUN_ST                                                      -- 2
                    , RUN_ET, BF_PLANT_CODE, BF_PROCESS_KEY, BF_L_CODE, BF_WORKDATE                                     -- 3
                    , BF_NUM, BF_QTY, QTY, REMAIN_QTY, PROTY                                                            -- 4
                    , TOTAL_OPER, DY_ST, DY_THICK, WORK_START_DATE, ICM_CODE                                            -- 5
                    , '{clsCommon.UserId}', DY_SP, CURRENT_1, CURRENT_2, FEEDER_RATE                                    -- 6
                    , CRUMBLE_YN, HARDNESS, PDI, CLEAN_QTY, LOCATION_ST1                                                -- 7
                    , LOCATION_ST2, LOCATION_ED1, LOCATION_ED2, HZ                                                      -- 8
                    , CD_TEMP, P_TEMP, COL_TEMP, REMARK, DEL_FLAG                                                       -- 9
                    , ERP_UP_YN, ERP_TNUMBER, SYSDATE, ERP_ERR_CNT, SEQ                                                 -- 10
                    , 'W', NULL, CASE WHEN '{pellet}' IS NOT NULL THEN TO_CHAR(ERP_OSTATUS) ELSE 'W' END, NULL, 0       -- 11
                    , 0, BF_PACK_TYPE, NULL, WORK_SEQ, '{clsCommon.GetPcStatusCode("계획")}'                             -- 12
                FROM PELLET_REPORT
                WHERE PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND PROCESS_KEY = '{gridView.GetFocusedRowCellValue("PROCESS_KEY")}'
                    AND L_CODE = '{gridView.GetFocusedRowCellValue("L_CODE")}'
                    AND WORKDATE = '{gridView.GetFocusedRowCellValue("WORKDATE")}'
                    AND WORK_SEQ = '{gridView.GetFocusedRowCellValue("WORK_SEQ")}' 
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
            catch
            {

            }
        }
    }
}