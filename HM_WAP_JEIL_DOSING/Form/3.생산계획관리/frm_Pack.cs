using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.DataAccess.Native.Json;
using DevExpress.PivotGrid.QueryMode;
using DevExpress.Schedule;
using DevExpress.Utils.Serializing.Helpers;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraSpreadsheet.Import.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Formats.Asn1.AsnWriter;

namespace HARIM_FA_DOSING
{
    public partial class frm_Pack : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;
        private string[] sValid = null;
        private string[] sDValid = null;

        private string formname = string.Empty;
        private string vPlant_Code = string.Empty;
        private string vProcess_Code = string.Empty;
        private string vLine_Code = string.Empty;
        private string vProcessName = string.Empty;

        string sPLANT_CODE = string.Empty;
        string sProcessKey = string.Empty;
        string sLcode = string.Empty;
        string resourceNo = string.Empty;
        string note = string.Empty;
        string sWorkDate = string.Empty;
        string sWorkSeq = string.Empty;

        private bool isInitializing = false;

        bool chk_version = false;
        decimal dNote_Per = 0;

        public frm_Pack(string plant_code, string process_key, string lcode, string formName, string sProcessName)
        {
            InitializeComponent();
            formname = formName;

            vPlant_Code = plant_code;
            vProcess_Code = process_key;
            vLine_Code = lcode;
            vProcessName = sProcessName;

            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewIngred, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewERPPack, Properties.Settings.Default.FontSize);
            this.vProcessName = vProcessName;
        }

        #region 작업순선생성
        private string workNumber_maker(string sPlant, string sProcessKey, string sLCode, string sWorkDate)
        {
            try
            {
                string return_seq = string.Empty;

                string SQL = $@"
                SELECT NVL(MAX(WORK_SEQ) + 1, 1) AS SEQ FROM PACK_ORDER WHERE PLANT_CODE = '{sPlant}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}' AND WORKDATE = '{sWorkDate}'
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
        #endregion

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Pack_Load(object sender, EventArgs e)
        {
            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                isInitializing = true;         // 초기화 완료

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(vPlant_Code, vProcess_Code), "", false, 0, true);
                cboL_Code.EditValue = vLine_Code;

                // ERP 진행여부
                //clsDevexpressUtil.ItemLookUpEditSetup(cboERPUpLoad, clsCommon.GetTransFlag(), "", false, 0, true);

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                //if (clsCommon._strUserType == "010607")
                //{
                //    layoutControlItem_workDateEd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //}
                //else
                //{

                    layoutControlItem_workDateEd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    dateEdit_workDateEd.EditValue = DateTime.Today.AddDays(1);
                //}

                InitControl();

                //작업조회
                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_Pack_Load", ex);
            }
        }

        private void InitControl()
        {
            #region Main
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboTransFlag, clsCommon.GetTransFlag());

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(vPlant_Code, vProcess_Code));

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "전체선택", false, 2, true);

            //인출빈설정
            repItemLkUpEdit_LOCATION.NullText = "";
            repItemLkUpEdit_LOCATION.NullValuePrompt = "";
            repItemLkUpEdit_LOCATION.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            repItemLkUpEdit_LOCATION.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSuggest;
            clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_LOCATION, clsCommon.GetBin(vPlant_Code, vProcess_Code, vLine_Code));

            //작업조
            gridcboICM_CODE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboICM_CODE, clsCommon.GetICM(), "작업조 선택", false, false);

            //작업자
            repItemLkUpEdit_EMPLOYEE_NO.NullText = "";
            repItemLkUpEdit_EMPLOYEE_NO.NullValuePrompt = "";
            clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_EMPLOYEE_NO, clsCommon.GetDO_INSA(vPlant_Code));

            //작업계획
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboPCStatus, clsCommon.GetPcStatus(), "", false, false);

            //불량내역
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboBAD_CODE, clsCommon.GetPackBad(), "", false);
            #endregion

            clsDevexpressGrid.ItemLookUpEditSetup(gridcbo_Ingred_Resource, clsCommon.GetResource(vPlant_Code, "", "", "", 0, true));

            // 단위
            clsDevexpressGrid.ItemLookUpEditSetup(gridcbo_Ingred_Unit, clsCommon.GetUnit());

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboYn, clsCommon.GetYn(null, new string[] {"삭제", "미삭제"}));
        }

        #region 작업지시조회
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                WITH BOM AS (
                    SELECT DISTINCT b.PLANT_CODE, b.MENGE, b.RESOURCE_NO, b.NOTE, a.BMENG
                    FROM SAP_IN_BOM_CONM a
                        JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                                                                    AND b.NOTE = a.NOTE
                    WHERE SUBSTR(NVL(b.IDNRK, '1'), 1, 1) = '1'  
                            AND SUBSTR(b.RESOURCE_NO, 1, 1) = '1'
                            AND b.P_TYPE = '2'
                )

                -- 포장 작업지시 정보
                SELECT DISTINCT
                     a.PLANT_CODE
                    , a.PROCESS_KEY
                    , a.L_CODE
                    , a.WORKDATE
                    , a.WORK_SEQ
                    , a.P_TYPE
                    , a.RESOURCE_NO
                    , a.NOTE
                    , a.WORK_START_DATE
                    , a.ICM_CODE
                    , a.OR_QTY 
                    , a.LOCATION
                    , a.DOS_Q
                    , b.BMENG
                    , a.EXP_QTY
                    , a.PRO_QTY
                    , b.MENGE * a.PRO_QTY / b.BMENG AS PRO_KG
                    , b.MENGE
                    , a.HALT_TIME
                    , a.RUN_ST
                    , a.RUN_ET
                    , a.REAL_QTY
                    , NVL(a.F_Q, 0) AS F_Q
                    , NVL(a.E_Q, 0) AS E_Q
                    , NVL(a.PA_Q, 0) AS PA_Q
                    , NVL(a.USE_PA_Q, 0) AS USE_PA_Q
                    , NVL(a.USE_EMP_PACK, 0) AS USE_EMP_PACK
                    , a.DIFF
                    , a.SAMPLE_TLY
                    , a.DAN1
                    , a.DAN2
                    , a.DAN3
                    , a.C_CONDITION
                    , a.BAD_CODE1
                    , a.BAD_QTY1
                    , a.BAD_CODE2
                    , a.BAD_QTY2
                    , a.BAD_CODE3
                    , a.BAD_QTY3
                    , a.BAD_CODE4
                    , a.BAD_QTY4
                    , a.BAD_CODE5
                    , a.BAD_QTY5
                    , a.DEL_FLAG
                    , a.I_TIME
                    , a.EMPLOYEE_NO
                    , CASE WHEN a.RUN_ST IS NOT NULL 
                                AND a.RUN_ET IS NOT NULL 
                                AND (a.RUN_ET - a.RUN_ST) > 0 
                            THEN FLOOR((b.MENGE * a.PRO_QTY) / ((a.RUN_ET - a.RUN_ST) * 1440) * 60) 
                            ELSE 0 
                        END AS PROVITY
                    , ROUND((a.RUN_ET - a.RUN_ST) * 24, 1) AS WORK_HOUR
                    , ROUND(
                           ((b.MENGE * a.PRO_QTY / b.BMENG)  / 1000) / NULLIF((a.RUN_ET - a.RUN_ST) * 24, 0)
                        , 1
                        ) AS PROTY
                    , a.ERP_ISTATUS
                    , a.ERP_ITNUMBER
                    , a.ERP_OSTATUS
                    , a.ERP_OTNUMBER
                    , c.B_W, c.END_TIME, c.PROD_QTY
                    , c.PROD_TIME, c.START_TIME
                    , c.STOP_TIME
                    , a.ERR_MSG
                    , a.REMARK
                    , a.WORK_SEQ_COPY
                FROM PACK_ORDER a
                    LEFT OUTER JOIN BOM b ON b.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = b.RESOURCE_NO AND b.NOTE = a.NOTE
                    LEFT JOIN PACK_ORDER_ADD c ON c.PLANT_CODE = a.PLANT_CODE AND c.PROCESS_KEY = a.PROCESS_KEY AND c.L_CODE = a.L_CODE
                                        AND c.WORKDATE = a.WORKDATE AND c.WORK_SEQ = a.WORK_SEQ
                WHERE a.PLANT_CODE = '{vPlant_Code}'
                    AND a.PROCESS_KEY = '{vProcess_Code}'
                    AND ('{cboL_Code.EditValue}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue}')
                    AND a.WORKDATE BETWEEN '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                                    AND '{string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)}'
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                ORDER BY CASE WHEN WORK_SEQ_COPY IS NULL OR WORK_SEQ_COPY = '' THEN WORK_SEQ
                                ELSE WORK_SEQ_COPY END, a.WORKDATE, a.C_CONDITION DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                //gridView.SetFixCol(new string[] {  "ERP_UP_YN"
                //    , "L_CODE"
                //    , "WORKDATE"
                //    , "WORK_SEQ"
                //    , "RESOURCE_NO"
                //    , "NOTE"});

                sValid = new string[] { "PLANT_CODE", "PROCESS_KEY", "L_CODE", "WORKDATE", "P_TYPE", "RESOURCE_NO", "NOTE", "WORK_START_DATE", "ICM_CODE", "LOCATION", "EMPLOYEE_NO" };

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

                gridView.Columns["ERP_OSTATUS"].OptionsColumn.AllowEdit = false;
                gridView.Columns["ERP_ISTATUS"].OptionsColumn.AllowEdit = false;

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(vPlant_Code, vProcess_Code, $"'{clsCommon.GetResourceTypeCode("제품")}','{clsCommon.GetResourceTypeCode("반제품")}'", "", 2, true, false, "EA"), "품목을 선택 해주세요.", false);
                gridscboRESOURCE_NO.PopupFormSize = new Size(400, 400); // 가로 500, 세로 300
                DevExpress.XtraGrid.Views.Grid.GridView popupView = gridscboRESOURCE_NO.View as DevExpress.XtraGrid.Views.Grid.GridView;
                popupView.Columns["NAME"].Width = 180;

                // 생산버전
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboNOTE, clsCommon.getNote(vPlant_Code));

                //불량내역
                //repItemLkUpEdit_BAD_CODE.NullText = "";
                //repItemLkUpEdit_BAD_CODE.NullValuePrompt = "";
                //SQL = $"SELECT ERROR_NO, TRIM(DESCRIPTION) AS DESCRIPTION FROM ERP_DBLINK.{clsCommon.erp_dosing_db_name}.DBO.V_MES_ATG_110_1";
                //ds = Dbconn.conn.ExecutDataset(SQL);
                //clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_BAD_CODE, ds.Tables[0], "DESCRIPTION", "ERROR_NO");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void work_result()
        {
            try
            {
                SQL = $@"
                -- 포장 배합 정보
                SELECT 'N' AS CHK, a.PLANT_CODE, a.P_TYPE, a.RESOURCE_NO, a.NOTE
                    , a.MIX_TIME, a.DRY_TIME, a.FINAL_TIME, a.LR_YN, a.STLST
                    , a.REMARK_1, a.CR_YN, a.REMARK_2, a.USE_YN, a.EMPLOYEE_NO
                    , b.IDNRK, b.STLAN, b.STLNR, b.STLAL, b.BMEIN, b.LKENZ, b.POSNR
                    , b.MENGE, b.MEINS, b.AUSCH, b.KZKUP
                    , b.ALPOS, b.ALPGR, b.EWAHR, b.SANFE, b.SANKA, b.BEIKZ
                    , b.DATUV, b.DATUV_TO, b.AENNR, b.SORTF, b.LGORT, b.POTX1, b.SEQ, b.XSEQNR
                FROM SAP_IN_BOM_CONM a
                    INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.P_TYPE = a.P_TYPE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                WHERE a.PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}' AND a.P_TYPE = '2' AND a.RESOURCE_NO = '{gridView.GetFocusedRowCellValue("RESOURCE_NO")}'
                    AND ('{gridView.GetFocusedRowCellValue("NOTE")}' IS NULL OR a.NOTE = '{gridView.GetFocusedRowCellValue("NOTE")}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridIngred, viewIngred, ds.Tables[0], false, true);

                sDValid = new string[] { "" };
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "work_result(string workdate, string num)", ex);
                clsLog.logSave(this, "work_result(string workdate, string num)", ex.StackTrace);
                ShowMessageBox.XtraShowError("작업지시내역을 조회하는중 오류가 발생했습니다.");
            }
        }

        private void Work_ERP_Result()
        {
            try
            {
                SQL = $@"
                -- ERP 실적
                SELECT 
                      a.PLANT_CODE
                    , a.PROCESS_KEY
                    , a.L_CODE
                    , a.WORKDATE
                    , a.WORK_SEQ
                    , a.RESOURCE_NO
                    , a.P_Q
                    , a.P_Q_TIME
                    , a.IO_DATE
                    , a.SEND_YN
                    , a.R_YN
                    , a.C_CONDITION
                    , a.I_TIME
                FROM 
                    PACK_REMARK a
                WHERE   a.PLANT_CODE = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
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
        #endregion

        #region 새로고침
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }
        #endregion

        #region 행추가
        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            clsDevexpressGrid.GridViewAddRow(gridView);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["PLANT_CODE"], vPlant_Code);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["PROCESS_KEY"], vProcess_Code);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["L_CODE"], cboL_Code.EditValue?.ToString());
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["WORKDATE"], string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["WORK_START_DATE"], string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["EMPLOYEE_NO"], clsCommon.UserId);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["OR_QTY"], 0);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["PRO_QTY"], 0);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["P_TYPE"], "2");
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["ICM_CODE"], clsCommon.GetICMGubun());
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"], clsCommon.PcStatus.Plan);

            if (vPlant_Code == "P101")
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["LOCATION"], "605");

            //if (gridView.GetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["SBNO"]).ToString() == "2")
            //{
            //}

        }
        #endregion

        #region 행삭제
        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }
        #endregion

        #region 조회 버튼 클릭
        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if(gridView.FocusedColumn.FieldName.Contains("REMARK"))
                {
                    e.Cancel = false;
                    return;
                }

                // 031004	완료
                if (gridView.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.Completed)
                    || gridView.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.ForceCompleted)
                    || gridView.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.Canceled)) //작지가 완료처리된것은 수정못하도록 에디트모드 off
                {
                    if (clsCommon.Auth_Form_Function(authDs, "U"))
                    {
                        switch (gridView.FocusedColumn.FieldName)
                        {
                            case "ICM_CODE":
                            case "LOCATION":
                            case "DOS_Q":
                            case "RUN_ST":
                            case "RUN_ET":
                                e.Cancel = false;
                                break;
                            default:
                                e.Cancel = true;
                                break;
                        }
                    }
                    else
                        if (gridView.FocusedColumn.FieldName != "TRANS_YN")
                        e.Cancel = true;        // 수정 불가

                    return;
                }
                else if (gridView.GetFocusedRowCellValue("C_CONDITION").Equals(clsCommon.GetPcStatusCode("진행")) && !gridView.FocusedColumn.FieldName.Contains("REMARK"))      // 031003	진행
                     //작지가 진행,완료처리된것은 수정못하도록 에디트모드 off
                {
                    e.Cancel = true;
                }
                else
                {
                    //if (gridView.FocusedColumn.FieldName.Contains("DEL_FLAG"))
                    //{
                    //    e.Cancel = true;        // 수정 불가
                    //}
                    //else
                        e.Cancel = false;
                }

                var targets = new[] { "Y", "C", "F" };

                for (int i = 0; i < gridView.RowCount; i++)
                {
                    if (targets.Contains(gridView.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                     || targets.Contains(gridView.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                    {
                        if (gridView.FocusedColumn.FieldName.Contains("DEL_FLAG"))
                        {
                            e.Cancel = false;        // 수정 가능
                        }
                        else
                            e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
            }
        }
        #endregion

        
        #region 저장하기

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
                clsDevexpressGrid.GridEndEdit(gridView);
                DataTable DT = (DataTable)gridControl.DataSource;

                DateTime dtRunSt;
                DateTime dtRunEd;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                foreach (DataRow dr in DT.Rows)
                {
                    dr.ClearErrors();

                    if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))
                    {
                        dr.SetColumnError("RESOURCE_NO", "제품을 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                        return;
                    }

                    if (string.IsNullOrEmpty(dr["LOCATION"].ToString()))
                    {
                        dr.SetColumnError("LOCATION", "인출빈을 선택하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION"));
                        return;
                    }

                    if (Convert.ToInt32(dr["OR_QTY"]) < 1)
                    {
                        dr.SetColumnError("OR_QTY", "0이상을 입력하여 주세요");
                        ShowMessageBox.XtraShowWarning(dr.GetColumnError("OR_QTY"));
                        return;
                    }

                    string sDtFrom = string.Empty;
                    string sDtTo = string.Empty;

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
                    else
                    {
                        sDtFrom = "''";
                        sDtTo = "''";
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                        if (!string.IsNullOrWhiteSpace(rValid))
                        {
                            gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                            gridView.ShowEditor(); // 편집 모드 진입 (선택)
                            Dbconn.conn.Rollback();
                            return;
                        }

                        string WORK_SEQ = workNumber_maker(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["L_CODE"]?.ToString(), dr["WORKDATE"]?.ToString());

                        if (string.IsNullOrEmpty(WORK_SEQ))
                        {
                            ShowMessageBox.XtraShowInformation("작업순번을 생성하는 도중 에러가 발생했습니다");
                            return;
                        }

                        SQL = $@"
                        INSERT INTO PACK_ORDER (
                             PLANT_CODE
                            , PROCESS_KEY
                            , L_CODE
                            , WORKDATE
                            , WORK_SEQ
                            , P_TYPE
                            , RESOURCE_NO
                            , NOTE
                            , WORK_START_DATE
                            , ICM_CODE
                            , OR_QTY
                            , LOCATION
                            , DOS_Q
                            , EXP_QTY
                            , PRO_QTY
                            , HALT_TIME
                            , REAL_QTY
                            , F_Q
                            , E_Q
                            , PA_Q
                            , USE_PA_Q
                            , USE_EMP_PACK
                            , DIFF
                            , SAMPLE_TLY
                            , DAN1
                            , DAN2
                            , DAN3
                            , C_CONDITION
                            , BAD_CODE1
                            , BAD_QTY1
                            , BAD_CODE2
                            , BAD_QTY2
                            , BAD_CODE3
                            , BAD_QTY3
                            , BAD_CODE4
                            , BAD_QTY4
                            , BAD_CODE5
                            , BAD_QTY5
                            , REMARK
                            , DEL_FLAG
                            , I_TIME
                            , EMPLOYEE_NO
                            , ERP_ISTATUS
                            , ERP_OSTATUS
                        )
                        VALUES (
                             '{dr["PLANT_CODE"]}'					--		PLANT_CODE
                            , '{dr["PROCESS_KEY"]}'					--		PROCESS_KEY
                            , '{dr["L_CODE"]}'						--		L_CODE
                            , '{dr["WORKDATE"]}'					--		WORKDATE
                            , '{WORK_SEQ}'					        --		WORK_SEQ
                            , '2'						            --		P_TYPE
                            , '{dr["RESOURCE_NO"]}'					--		RESOURCE_NO
                            , '{dr["NOTE"]}'						--		NOTE
                            , '{dr["WORK_START_DATE"]}'			    --		WORK_START_DATE
                            , '{dr["ICM_CODE"]}'					--		 ICM_CODE
                            , '{dr["OR_QTY"]}'						--		OR_QTY
                            , '{dr["LOCATION"]}'					--		LOCATION
                            , '{dr["DOS_Q"]}'						--		DOS_Q
                            , '{dr["EXP_QTY"]}'						--		EXP_QTY
                            , '0'						            --		PRO_QTY
                            , '{dr["HALT_TIME"]}'					--		HALT_TIME
                            , '{dr["REAL_QTY"]}'					--		REAL_QTY
                            , '{dr["F_Q"]}'							--		F_Q
                            , '{dr["E_Q"]}'							--		E_Q
                            , '{dr["PA_Q"]}'						--		PA_Q
                            , '{dr["USE_PA_Q"]}'					--		USE_PA_Q
                            , '{dr["USE_EMP_PACK"]}'
                            , '{dr["DIFF"]}'						--		DIFF
                            , '{dr["SAMPLE_TLY"]}'					--		SAMPLE_TLY
                            , '{dr["DAN1"]}'						--		DAN1
                            , '{dr["DAN2"]}'						--		DAN2
                            , '{dr["DAN3"]}'						--		DAN3
                            , '{dr["C_CONDITION"]}'				    --		C_CONDITION
                            , '{dr["BAD_CODE1"]}'					--		BAD_CODE1
                            , '{dr["BAD_QTY1"]}'					--		BAD_QTY1
                            , '{dr["BAD_CODE2"]}'					--		BAD_CODE2
                            , '{dr["BAD_QTY2"]}'					--		BAD_QTY2
                            , '{dr["BAD_CODE3"]}'					--		BAD_CODE3
                            , '{dr["BAD_QTY3"]}'					--		BAD_QTY3
                            , '{dr["BAD_CODE4"]}'					--		BAD_CODE4
                            , '{dr["BAD_QTY4"]}'					--		BAD_QTY4
                            , '{dr["BAD_CODE5"]}'					--		BAD_CODE5
                            , '{dr["BAD_QTY5"]}'					--		BAD_QTY5
                            , '{dr["REMARK"]}'                      --      REMARK
                            , 'N'					                --		DEL_FLAG
                            , SYSDATE								--		I_TIME
                            , '{dr["EMPLOYEE_NO"]}'				    --		EMPLOYEE_NO
                            , 'N'
                            , 'N'
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
                        string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                        if (!string.IsNullOrWhiteSpace(rValid))
                        {
                            gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                            gridView.ShowEditor(); // 편집 모드 진입 (선택)
                            Dbconn.conn.Rollback();
                            return;
                        }

                        SQL = $@"
                        UPDATE PACK_ORDER
                        SET
                              RESOURCE_NO     = '{dr["RESOURCE_NO"]}'     -- 02
                            , NOTE            = '{dr["NOTE"]}'            -- 03
                            , WORK_START_DATE = '{dr["WORK_START_DATE"]}' -- 04
                            , ICM_CODE        = '{dr["ICM_CODE"]}'        -- 05
                            , OR_QTY          = '{dr["OR_QTY"]}'          -- 06
                            , LOCATION        = '{dr["LOCATION"]}'        -- 07
                            , DOS_Q           = '{dr["DOS_Q"]}'           -- 08
                            , EXP_QTY         = '{dr["EXP_QTY"]}'         -- 09
                            , PRO_QTY         = '{dr["PRO_QTY"]}'         -- 10
                            , REAL_QTY        = '{dr["REAL_QTY"]}'        -- 14
                            , F_Q             = '{dr["F_Q"]}'             -- 15
                            , E_Q             = '{dr["E_Q"]}'             -- 16
                            , PA_Q            = '{dr["PA_Q"]}'            -- 17
                            , USE_PA_Q        = '{dr["USE_PA_Q"]}'        -- 18
                            , RUN_ST          = {sDtFrom}
                            , RUN_ET          = {sDtTo}
                            , USE_EMP_PACK    = '{dr["USE_EMP_PACK"]}'
                            , DIFF            = '{dr["DIFF"]}'            -- 19
                            , SAMPLE_TLY      = '{dr["SAMPLE_TLY"]}'      -- 20
                            , DAN1            = '{dr["DAN1"]}'            -- 21
                            , DAN2            = '{dr["DAN2"]}'            -- 22
                            , DAN3            = '{dr["DAN3"]}'            -- 23
                            , C_CONDITION     = '{dr["C_CONDITION"]}'     -- 24
                            , BAD_CODE1       = '{dr["BAD_CODE1"]}'       -- 25
                            , BAD_QTY1        = '{dr["BAD_QTY1"]}'        -- 26
                            , BAD_CODE2       = '{dr["BAD_CODE2"]}'       -- 27
                            , BAD_QTY2        = '{dr["BAD_QTY2"]}'        -- 28
                            , BAD_CODE3       = '{dr["BAD_CODE3"]}'       -- 29
                            , BAD_QTY3        = '{dr["BAD_QTY3"]}'        -- 30
                            , BAD_CODE4       = '{dr["BAD_CODE4"]}'       -- 31
                            , BAD_QTY4        = '{dr["BAD_QTY4"]}'        -- 32
                            , BAD_CODE5       = '{dr["BAD_CODE5"]}'       -- 33
                            , BAD_QTY5        = '{dr["BAD_QTY5"]}'        -- 34
                            , REMARK          = '{dr["REMARK"]}'
                            , DEL_FLAG        = '{dr["DEL_FLAG"]}'
                            , I_TIME          = SYSDATE                   -- 38
                            , EMPLOYEE_NO     = '{dr["EMPLOYEE_NO"]}'     -- 39
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
                        WHERE PLANT_CODE      = '{dr["PLANT_CODE"]}'      -- 40
                        AND  PROCESS_KEY     = '{dr["PROCESS_KEY"]}'     -- 41
                        AND  L_CODE          = '{dr["L_CODE"]}'          -- 42
                        AND  WORKDATE        = '{dr["WORKDATE"]}'        -- 43
                        AND  WORK_SEQ        = '{dr["WORK_SEQ"]}'        -- 44
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach


                gridView.RefreshData();
                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        #endregion

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
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

                var targets = new[] { "Y", "C", "F" };

                for (int i = 0; i < gridView.RowCount; i++)
                {
                    if (targets.Contains(gridView.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                     || targets.Contains(gridView.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                    {
                        ShowMessageBox.XtraShowInformation("ERP 전송 완료된 작업지시는 삭제하실 수 없습니다");
                        return;
                    }
                }

                int[] selectedRows = gridView.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("삭제 할 작업지시를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = gridView.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        gridView.DeleteRow(rowHandle);
                    }
                    else
                    {
                        DialogResult result = ShowMessageBox.Confirm($"선택하신 작업순번 {dr["WORK_SEQ"]} 작업지시를 삭제하시겠습니까?");

                        if (result == DialogResult.Yes)
                        {
                            splashScreenManager.ShowWaitForm();

                            string condition = gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"]);

                            if (condition.Equals("진행"))
                            {
                                ShowMessageBox.XtraShowInformation("진행중인 작업지시는 삭제하실 수 없습니다");
                                return;
                            }

                            if (clsCommon.PlantCode.Contains("PJ01") && condition.Equals("완료"))
                            {
                                ShowMessageBox.XtraShowInformation("완료된 작업지시는 삭제하실 수 없습니다");
                                return;
                            }

                            Dbconn.conn.BeginTransaction();
                            //delete work num
                            SQL = $@"
                            UPDATE PACK_ORDER SET DEL_FLAG = 'Y'
                                                , ERP_OSTATUS = 'X'
                                                , ERP_ISTATUS = 'X'
                            WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}' 
                                AND WORKDATE = '{dr["WORKDATE"]}' AND WORK_SEQ = '{dr["WORK_SEQ"]}'
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 1)
                            {
                                Dbconn.conn.Rollback();
                                clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                                ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                                return;
                            }

                            Dbconn.conn.Commit();

                            XMain_Search();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void btn_workEnd_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("강제완료 하실 작업지시를 선택하여 주세요");
                return;
            }

            DataRow sel_row = gridView.GetDataRow(gridView.FocusedRowHandle);

            if (sel_row.RowState != DataRowState.Added)
            {
                string sPLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PLANT_CODE");
                string sPROCESS_KEY = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PROCESS_KEY");
                string sL_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "L_CODE");
                string sWORKDATE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORKDATE");
                string sWORK_SEQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORK_SEQ");
                string location = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "LOCATION");
                string resource_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO");


                string con_st = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "C_CONDITION");

                if (clsDevexpressGrid.GetFocusedRowCellValue(gridView, "ERP_OSTATUS") == "Y" && clsDevexpressGrid.GetFocusedRowCellValue(gridView, "ERP_ISTATUS") == "Y")
                {
                    ShowMessageBox.XtraShowInformation("이미 전송 된 작업지시 입니다");
                    return;
                }

                m_packEnd mPackEnd = new m_packEnd(sPLANT_CODE, sPROCESS_KEY, sL_CODE, sWORKDATE, sWORK_SEQ, resource_no);
                mPackEnd.StartPosition = FormStartPosition.CenterScreen;
                DialogResult rlt = mPackEnd.ShowDialog();

                if (rlt == DialogResult.OK)
                {
                    Dbconn.conn.BeginTransaction();

                    SQL = $@"
                    SELECT * FROM PACK_ORDER
                    WHERE PLANT_CODE = '{sPLANT_CODE}' AND PROCESS_KEY = '{sPROCESS_KEY}' AND L_CODE = '{sL_CODE}'
                        AND WORKDATE = '{sWORKDATE}' AND WORK_SEQ = '{sWORK_SEQ}'";

                    DataSet work_ds = Dbconn.conn.ExecutDataset(SQL);
                    DataRow row = work_ds.Tables[0].Rows[0];

                    SQL = $@"
                    DELETE
                    FROM PACK_REMARK
                    WHERE PLANT_CODE = '{sPLANT_CODE}'
                        AND PROCESS_KEY LIKE '%{sPROCESS_KEY}'
                        AND L_CODE = '{sL_CODE}'
                        AND WORKDATE = '{sWORKDATE}'
                        AND WORK_SEQ = '{sWORK_SEQ}'
                    ";

                    Dbconn.conn.SQLrun(SQL);

                    SQL = $@"
                    INSERT INTO PACK_REMARK
                    SELECT a.PLANT_CODE
                         , a.PROCESS_KEY AS PROCESS_KEY
                         , a.L_CODE
                         , a.WORKDATE
                         , a.WORK_SEQ
                         , b.IDNRK
                         --, NVL(a.F_Q, 0)
                         --, NVL(a.E_Q, 0)
                         --, NVL(a.BAD_QTY1, 0)
                         --, NVL(a.BAD_QTY2, 0)
                         --, NVL(a.PRO_QTY, 0)
                         --, MEINS
                         , TRUNC(CASE WHEN MEINS = 'EA' THEN b.MENGE * (NVL(a.F_Q, 0) + NVL(a.E_Q, 0) + NVL(a.BAD_QTY1, 0) + NVL(a.BAD_QTY2, 0) + NVL(a.PRO_QTY, 0) + NVL(a.USE_EMP_PACK, 0)) / b.BMENG  -- 공지대
                                 ELSE ((b.MENGE * (a.PRO_QTY + (NVL(a.F_Q, 0) + NVL(a.E_Q, 0)                        -- 벌크
                                                 + CASE WHEN a.PLANT_CODE IN ('PJ01', 'PJ02', 'PJ04', 'PJ05') 
                                                                     THEN NVL(a.BAD_QTY1, 0) + NVL(a.BAD_QTY2, 0) 
                                                 ELSE 0 END)))  + NVL(a.PA_Q, 0))  / b.BMENG
                             END) PRO_QTY
                         , NULL
                         , SYSDATE
                         , 'N'
                         , 'N'
                         , ''
                         , SYSDATE
                    FROM PACK_ORDER a
                         INNER JOIN (SELECT DISTINCT a.PLANT_CODE, a.P_TYPE, a.RESOURCE_NO, a.NOTE, a.BMENG
                                            , c.BU_P, a.MIX_TIME, a.DRY_TIME, a.FINAL_TIME, a.LR_YN, a.STLST
                                            , a.REMARK_1, a.CR_YN, a.REMARK_2, a.USE_YN, a.EMPLOYEE_NO
                                            , b.IDNRK, b.STLAN, b.STLNR, b.STLAL, b.BMEIN, b.LKENZ, b.POSNR
                                            , b.MENGE, b.MEINS, b.AUSCH, b.KZKUP
                                            , b.ALPOS, b.ALPGR, b.EWAHR, b.SANFE, b.SANKA, b.BEIKZ
                                            , b.DATUV, b.DATUV_TO, b.AENNR, b.SORTF, b.LGORT, b.POTX1, b.SEQ, b.XSEQNR
                                    FROM SAP_IN_BOM_CONM a
                                            INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.P_TYPE = a.P_TYPE
                                                AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                                            INNER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                                    WHERE b.P_TYPE = '2' AND a.PLANT_CODE = '{sPLANT_CODE}'
                                        AND a.RESOURCE_NO = '{resource_no}') b ON b.PLANT_CODE = a.PLANT_CODE
                                                                        AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                    WHERE a.PLANT_CODE = '{sPLANT_CODE}'
                          AND a.PROCESS_KEY = '{sPROCESS_KEY}'
                          AND a.L_CODE = '{sL_CODE}'
                          AND a.WORKDATE = '{sWORKDATE}'
                          AND a.WORK_SEQ = '{sWORK_SEQ}'
                    UNION ALL
                    SELECT a.PLANT_CODE
                         , a.PROCESS_KEY AS PROCESS_KEY
                         , a.L_CODE
                         , a.WORKDATE
                         , a.WORK_SEQ
                         , NVL(b.BU_P, b.IDNRK) AS IDNRK
                         , TRUNC(((b.MENGE * (NVL(a.F_Q, 0) + NVL(a.E_Q, 0) 
                                + CASE WHEN a.PLANT_CODE IN ('PJ01', 'PJ02', 'PJ04', 'PJ05') 
                                    THEN NVL(a.BAD_QTY1, 0) + NVL(a.BAD_QTY2, 0) ELSE 0 END))
                                + a.PA_Q) / b.BMENG)    -- 부산물
                         , NULL
                         , SYSDATE
                         , 'N'
                         , CASE WHEN F_Q > 0 OR E_Q > 0 THEN 'Y' ELSE 'N' END
                         , ''
                         , SYSDATE
                    FROM PACK_ORDER a
                         INNER JOIN (SELECT DISTINCT a.PLANT_CODE, a.P_TYPE, a.RESOURCE_NO, a.NOTE, a.BMENG
                                         , c.BU_P, a.MIX_TIME, a.DRY_TIME, a.FINAL_TIME, a.LR_YN, a.STLST
                                         , a.REMARK_1, a.CR_YN, a.REMARK_2, a.USE_YN, a.EMPLOYEE_NO
                                         , b.IDNRK, b.STLAN, b.STLNR, b.STLAL, b.BMEIN, b.LKENZ, b.POSNR
                                         , b.MENGE, b.MEINS, b.AUSCH, b.KZKUP
                                         , b.ALPOS, b.ALPGR, b.EWAHR, b.SANFE, b.SANKA, b.BEIKZ
                                         , b.DATUV, b.DATUV_TO, b.AENNR, b.SORTF, b.LGORT, b.POTX1, b.SEQ, b.XSEQNR
                                    FROM SAP_IN_BOM_CONM a
                                         INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.P_TYPE = a.P_TYPE
                                                AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                                         INNER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                                    WHERE b.P_TYPE = '2' AND a.PLANT_CODE = '{sPLANT_CODE}' AND a.RESOURCE_NO = '{resource_no}'
                                    ) b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                    WHERE a.PLANT_CODE = '{sPLANT_CODE}'
                          AND a.PROCESS_KEY = '{sPROCESS_KEY}'
                          AND a.L_CODE = '{sL_CODE}'
                          AND a.WORKDATE = '{sWORKDATE}'
                          AND a.WORK_SEQ = '{sWORK_SEQ}'
                          AND (b.MENGE * (NVL(a.F_Q, 0) + NVL(a.E_Q, 0) + CASE WHEN a.PLANT_CODE IN ('PJ01', 'PJ02', 'PJ04', 'PJ05') THEN NVL(a.BAD_QTY1, 0) + NVL(a.BAD_QTY2, 0) ELSE 0 END)) + a.PA_Q > 0
                          AND (a.F_Q > 0 OR a.E_Q > 0 OR a.BAD_QTY1 > 0 OR a.BAD_QTY2 > 0 OR a.PA_Q > 0) AND ROWNUM = 1
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("실적 생성에 실패했습니다.");
                        return;
                    }

                    SQL = $@"
                    INSERT INTO PACK_REMARK
                    SELECT a.PLANT_CODE
                         , 'SAP_' || a.PROCESS_KEY AS PROCESS_KEY
                         , a.L_CODE
                         , a.WORKDATE
                         , a.WORK_SEQ
                         , b.IDNRK
                         --, NVL(a.F_Q, 0)
                         --, NVL(a.E_Q, 0)
                         --, NVL(a.BAD_QTY1, 0)
                         --, NVL(a.BAD_QTY2, 0)
                         --, NVL(a.PRO_QTY, 0)
                         , TRUNC(CASE WHEN MEINS = 'EA' THEN b.MENGE * (NVL(a.F_Q, 0) + NVL(a.E_Q, 0) + NVL(a.BAD_QTY1, 0) + NVL(a.BAD_QTY2, 0) + NVL(a.PRO_QTY, 0) + NVL(a.USE_EMP_PACK, 0)) / b.BMENG  -- 공지대
                                 ELSE ((b.MENGE * (a.PRO_QTY + (NVL(a.F_Q, 0) + NVL(a.E_Q, 0)                        -- 벌크
                                                 + CASE WHEN a.PLANT_CODE IN ('PJ01', 'PJ02', 'PJ04', 'PJ05') 
                                                                     THEN NVL(a.BAD_QTY1, 0) + NVL(a.BAD_QTY2, 0) 
                                                 ELSE 0 END)))  + NVL(a.PA_Q, 0))  / b.BMENG
                             END) PRO_QTY
                         , NULL
                         , SYSDATE
                         , 'N'
                         , 'N'
                         , ''
                         , SYSDATE
                    FROM PACK_ORDER a
                         INNER JOIN (SELECT DISTINCT a.PLANT_CODE, a.P_TYPE, a.RESOURCE_NO, a.NOTE, a.BMENG
                                     , c.BU_P, a.MIX_TIME, a.DRY_TIME, a.FINAL_TIME, a.LR_YN, a.STLST
                                     , a.REMARK_1, a.CR_YN, a.REMARK_2, a.USE_YN, a.EMPLOYEE_NO
                                     , b.IDNRK, b.STLAN, b.STLNR, b.STLAL, b.BMEIN, b.LKENZ, b.POSNR
                                     , b.MENGE, b.MEINS, b.AUSCH, b.KZKUP
                                     , b.ALPOS, b.ALPGR, b.EWAHR, b.SANFE, b.SANKA, b.BEIKZ
                                     , b.DATUV, b.DATUV_TO, b.AENNR, b.SORTF, b.LGORT, b.POTX1, b.SEQ, b.XSEQNR
                                FROM SAP_IN_BOM_CONM a
                                     INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.P_TYPE = a.P_TYPE
                                            AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                                     INNER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                                WHERE b.P_TYPE = '2' AND a.PLANT_CODE = '{sPLANT_CODE}' AND a.RESOURCE_NO = '{resource_no}')
                                    b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                    WHERE a.PLANT_CODE = '{sPLANT_CODE}'
                          AND a.PROCESS_KEY = '{sPROCESS_KEY}'
                          AND a.L_CODE = '{sL_CODE}'
                          AND a.WORKDATE = '{sWORKDATE}'
                          AND a.WORK_SEQ = '{sWORK_SEQ}'
                    UNION ALL
                    SELECT a.PLANT_CODE
                         , 'SAP_' || a.PROCESS_KEY AS PROCESS_KEY
                         , a.L_CODE
                         , a.WORKDATE
                         , a.WORK_SEQ
                         , NVL(b.BU_P, b.IDNRK) AS IDNRK
                         , TRUNC(((b.MENGE * (NVL(a.F_Q, 0) + NVL(a.E_Q, 0) 
                                + CASE WHEN a.PLANT_CODE IN ('PJ01', 'PJ02', 'PJ04', 'PJ05') 
                                    THEN NVL(a.BAD_QTY1, 0) + NVL(a.BAD_QTY2, 0) ELSE 0 END))
                                + a.PA_Q) / b.BMENG)    -- 부산물
                         , NULL
                         , SYSDATE
                         , 'N'
                         , CASE WHEN F_Q > 0 OR E_Q > 0 THEN 'Y' ELSE 'N' END
                         , ''
                         , SYSDATE
                    FROM PACK_ORDER a
                         INNER JOIN (SELECT DISTINCT a.PLANT_CODE, a.P_TYPE, a.RESOURCE_NO, a.NOTE, a.BMENG
                                         , c.BU_P, a.MIX_TIME, a.DRY_TIME, a.FINAL_TIME, a.LR_YN, a.STLST
                                         , a.REMARK_1, a.CR_YN, a.REMARK_2, a.USE_YN, a.EMPLOYEE_NO
                                         , b.IDNRK, b.STLAN, b.STLNR, b.STLAL, b.BMEIN, b.LKENZ, b.POSNR
                                         , b.MENGE, b.MEINS, b.AUSCH, b.KZKUP
                                         , b.ALPOS, b.ALPGR, b.EWAHR, b.SANFE, b.SANKA, b.BEIKZ
                                         , b.DATUV, b.DATUV_TO, b.AENNR, b.SORTF, b.LGORT, b.POTX1, b.SEQ, b.XSEQNR
                                    FROM SAP_IN_BOM_CONM a
                                         INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.P_TYPE = a.P_TYPE
                                                AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                                         INNER JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                                    WHERE b.P_TYPE = '2' AND a.PLANT_CODE = '{sPLANT_CODE}' AND a.RESOURCE_NO = '{resource_no}'
                                    ) b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE
                    WHERE a.PLANT_CODE = '{sPLANT_CODE}'
                          AND a.PROCESS_KEY = '{sPROCESS_KEY}'
                          AND a.L_CODE = '{sL_CODE}'
                          AND a.WORKDATE = '{sWORKDATE}'
                          AND a.WORK_SEQ = '{sWORK_SEQ}'
                          AND (b.MENGE * (NVL(a.F_Q, 0) + NVL(a.E_Q, 0) + CASE WHEN a.PLANT_CODE IN ('PJ01', 'PJ02', 'PJ04', 'PJ05') THEN NVL(a.BAD_QTY1, 0) + NVL(a.BAD_QTY2, 0) ELSE 0 END)) + a.PA_Q > 0
                          AND (a.F_Q > 0 OR a.E_Q > 0 OR a.BAD_QTY1 > 0 OR a.BAD_QTY2 > 0 OR a.PA_Q > 0) AND ROWNUM = 1
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("실적 생성에 실패했습니다.");
                        return;
                    }

                    SQL = $@"
                    UPDATE PACK_ORDER
                    SET C_CONDITION = '{clsCommon.PcStatus.Completed}'
                            , ERP_OSTATUS = CASE WHEN '{mPackEnd.IsAuto}' = 'True' AND '{clsCommon.GetTransAuto(sPLANT_CODE, sPROCESS_KEY)}' = 'Y' THEN 'F' ELSE 'N' END 
                            , ERP_ISTATUS = CASE WHEN '{mPackEnd.IsAuto}' = 'True' AND '{clsCommon.GetTransAuto(sPLANT_CODE, sPROCESS_KEY)}' = 'Y' THEN 'F' ELSE 'N' END
                            , ERP_OERR_CNT = 0
                            , ERP_IERR_CNT = 0
                    WHERE PLANT_CODE = '{sPLANT_CODE}'
                        AND PROCESS_KEY = '{vProcess_Code}'
                        AND L_CODE = '{sL_CODE}'
                        AND WORKDATE = '{sWORKDATE}' AND WORK_SEQ = '{sWORK_SEQ}'";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        Dbconn.conn.Rollback();
                        ShowMessageBox.XtraShowWarning("작업지시 강제완료에 실패했습니다");
                        return;
                    }

                    Dbconn.conn.Commit();

                    ShowMessageBox.XtraShowInformation("강제완료처리가 완료되었습니다");
                }

            }
            else
            {
                Dbconn.conn.Rollback();
                ShowMessageBox.XtraShowInformation("해당 작업지시는 저장을 완료하신후에 강제완료 하여 주시길 바랍니다");
                return;
            }

            XMain_Search();
        }

        private void lookUpEdit_sbno_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                string condition = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["C_CONDITION"]).ToString();

                if (condition == clsCommon.PcStatus.Completed) //완료
                {
                    e.Appearance.BackColor = Color.LightGray;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;

                }

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

        private void gridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "C_CONDITION")
            {
                int sumText = 0;

                //string process_cd = string.Empty;
                //if (lookUpEdit_sbno.EditValue.Equals("1"))
                //{
                //    process_cd = clsCommon.pack_process_code;
                //}
                //else if (lookUpEdit_sbno.EditValue.Equals("2"))
                //{
                //    process_cd = clsCommon.Excon_pack_process_code;
                //}

                SQL = $@"
                SELECT FLOOR(NVL(
                    SUM(A.PRO_Q) / 
                    (SUM(A.MIN_SUM) 
                        - NVL((SELECT SUM(REST_MINUTES) 
                               FROM REST_TIME 
                               WHERE PROCESS_KEY = '{vProcess_Code}' 
                                 AND WORK_START_DATE BETWEEN TO_DATE('{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', 'YYYY-MM-DD') 
                                                  AND TO_DATE('{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', 'YYYY-MM-DD') 
                              ), 0)
                    ) * 60, 0)
                ) AS PROVITY
                FROM (
                    SELECT a.PLANT_CODE, a.WORK_START_DATE,  a.RESOURCE_NO,
                           SUM(b.MENGE * a.PRO_QTY) AS PRO_Q, 
                           (MAX(a.RUN_ET) - MIN(a.RUN_ST)) * 1440 AS MIN_SUM
                    FROM PACK_ORDER a
                    LEFT OUTER JOIN (
                        SELECT * 
                        FROM SAP_IN_BOM_COND b
                        WHERE SUBSTR(NVL(b.IDNRK, '1'), 1, 1) = '1'  
                              AND SUBSTR(b.RESOURCE_NO, -1, 1) = '1' 
                    ) b ON a.PLANT_CODE = b.PLANT_CODE AND a.RESOURCE_NO = b.RESOURCE_NO
                    WHERE a.PLANT_CODE = '{vPlant_Code}' AND a.PROCESS_KEY = '{vProcess_Code}' AND a.L_CODE = '{vLine_Code}'
                          AND a.WORK_START_DATE BETWEEN TO_DATE('{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', 'YYYY-MM-DD') 
                                           AND TO_DATE('{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', 'YYYY-MM-DD') 
                          AND NVL(a.DEL_FLAG, 'N') != 'Y' 
                          AND (a.RUN_ET - a.RUN_ST) * 1440 > 0
                    GROUP BY a.PLANT_CODE, a.WORK_START_DATE,  a.RESOURCE_NO
                ) A
                GROUP BY WORK_START_DATE   
                ";

                DataSet proDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(proDs) > 0)
                {

                    sumText = Convert.ToInt32(Dbconn.conn.getData(proDs, "PROVITY", 0));

                }

                e.Info.DisplayText = "생산성 : " + String.Format("{0:#,###}", sumText);
            }
        }

        private void gridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            gridIngred.DataSource = null;
            gridERPPack.DataSource = null;

            work_result();
            Work_ERP_Result();
        }

        private void GridscboRESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            string sPlantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PLANT_CODE");
            string sProcessKey = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PROCESS_KEY");
            string sLCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "L_CODE");
            string sLocation = string.Empty;
            string sScale = string.Empty;

            string sResourceNo = textEditor.EditValue?.ToString();

            string sNOTE = clsCommon.getLastVersion(sPlantCode, sResourceNo, out chk_version, out dNote_Per);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "NOTE", sNOTE);

            clsCommon.getSelectBin(sPlantCode, sProcessKey, sLCode, sResourceNo, out sLocation, out sScale);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "SCALE_CODE", sScale);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "LOCSTION", sLocation);

            work_result();
        }

        private void gridcboNOTE_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;
            string sNote = textEditor.EditValue?.ToString();

            string sResourceNo = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO");

            work_result();
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

            string condition = gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"]);

            var targets = new[] { "Y", "C", "F" };

            for (int i = 0; i < gridView.RowCount; i++)
            {
                if (targets.Contains(gridView.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                 || targets.Contains(gridView.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                {
                    ShowMessageBox.XtraShowInformation("ERP 전송 완료된 작업지시는 재전송 하실 수 없습니다");
                    return;
                }
            }

            try
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

                    SQL = $@"
                    UPDATE PACK_ORDER
                    SET ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) 
                                        WHEN 'N' THEN 'F'
                                        WHEN 'W' THEN 'F'
                                        WHEN 'M' THEN 'U'
                                        WHEN 'X' THEN 'D'
                                        WHEN 'G' THEN 'F'
                                        WHEN 'L' THEN 'U'
                                        WHEN 'R' THEN 'D'
                                        WHEN NULL THEN 'N'
                                    ELSE TO_CHAR(ERP_OSTATUS) END
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
                    WHERE PLANT_CODE        = '{dr["PLANT_CODE"]}' 
                        AND PROCESS_KEY     = '{dr["PROCESS_KEY"]}'
                        AND L_CODE          = '{dr["L_CODE"]}'     
                        AND WORKDATE        = '{dr["WORKDATE"]}'   
                        AND WORK_SEQ        = '{dr["WORK_SEQ"]}'   
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
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            ShowMessageBox.XtraShowInformation("선택된 정보가 전송 대기로 변경 되었습니다");
        }

        private void btnERPDelete_Click_1(object sender, EventArgs e)
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
                ShowMessageBox.XtraShowInformation("전송 취소 할 작업을 선택하여 주세요");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 작업을 ERP로 전송 하시겠습니까?", "ERP의 기존 작업 내역이 입고/투입 취소 됩니다."))
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

                foreach (int rowHandle in selectedRows)
                {
                    var dr = gridView.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    SQL = $@"
                    SELECT 1
                    FROM PACK_ORDER
                    WHERE PLANT_CODE      = '{dr["PLANT_CODE"]}'
                        AND  PROCESS_KEY     = '{dr["PROCESS_KEY"]}'
                        AND  L_CODE          = '{dr["L_CODE"]}'
                        AND  WORKDATE        = '{dr["WORKDATE"]}'
                        AND  WORK_SEQ        = '{dr["WORK_SEQ"]}'
                        AND (ERP_OSTATUS IN ('Y')
                        OR ERP_ISTATUS IN ('Y'))
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE PACK_ORDER
                        SET   ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) 
                                                WHEN 'Y' THEN 'D'
                                            ELSE TO_CHAR(ERP_OSTATUS) END
                            , ERP_OERR_CNT = 0
                            , ERP_ISTATUS = CASE TO_CHAR(ERP_ISTATUS) 
                                                WHEN 'Y' THEN 'D'
                                            ELSE TO_CHAR(ERP_ISTATUS) END
                            , ERP_IERR_CNT = 0
                            , C_CONDITION = '{clsCommon.GetPcStatusCode("취소")}'
                        WHERE PLANT_CODE      = '{dr["PLANT_CODE"]}'
                            AND  PROCESS_KEY     = '{dr["PROCESS_KEY"]}'
                            AND  L_CODE          = '{dr["L_CODE"]}'
                            AND  WORKDATE        = '{dr["WORKDATE"]}'
                            AND  WORK_SEQ        = '{dr["WORK_SEQ"]}'
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
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            ShowMessageBox.XtraShowInformation("선택된 정보가 삭제 전송 대기로 변경 되었습니다");
        }

        private void btnERPRowAdd_Click(object sender, EventArgs e)
        {
            var targets = new[] { "Y", "C", "F" };

            for (int i = 0; i < gridView.RowCount; i++)
            {
                if (targets.Contains(gridView.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                 || targets.Contains(gridView.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                {
                    ShowMessageBox.XtraShowInformation("ERP 전송 완료된 작업지시는 추가하실 수 없습니다");
                    return;
                }
            }

            viewERPPack.AddNewRow();
            viewERPPack.ShowEditor();

            sPLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PLANT_CODE");
            sProcessKey = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PROCESS_KEY");
            sLcode = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "L_CODE");
            sWorkDate = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORKDATE");
            sWorkSeq = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORK_SEQ");

            viewERPPack.SetFocusedRowCellValue("PLANT_CODE", sPLANT_CODE);
            viewERPPack.SetFocusedRowCellValue("PROCESS_KEY", "SAP_".Merge(sProcessKey));
            viewERPPack.SetFocusedRowCellValue("L_CODE", sLcode);
            viewERPPack.SetFocusedRowCellValue("WORKDATE", sWorkDate);
            viewERPPack.SetFocusedRowCellValue("WORK_SEQ", sWorkSeq);
        }

        private void btnERPRowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewERPPack);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewERPPack);
        }

        private void btnERPSave_Click(object sender, EventArgs e)
        {
            try
            {
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridERPPack.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    string condition = gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"]);

                    var targets = new[] { "Y", "C", "F" };

                    for (int i = 0; i < gridView.RowCount; i++)
                    {
                        if (targets.Contains(gridView.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                         || targets.Contains(gridView.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                        {
                            ShowMessageBox.XtraShowInformation("ERP 전송 완료된 작업지시는 수정하실 수 없습니다");
                            return;
                        }
                    }

                    string rValid = clsCommon.ValdationCheck(sDValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO PACK_REMARK (
                           PLANT_CODE        -- 01
                         , PROCESS_KEY       -- 02
                         , L_CODE            -- 03
                         , WORKDATE          -- 04
                         , WORK_SEQ          -- 05
                         , RESOURCE_NO       -- 06
                         , P_Q               -- 07
                         , P_Q_TIME          -- 08
                         , IO_DATE           -- 09
                         , SEND_YN           -- 10
                         , R_YN              -- 11
                         , C_CONDITION       -- 12
                         , I_TIME            -- 13
                        )
                        VALUES (
                           '{dr["PLANT_CODE"]}'        -- 01
                         , '{dr["PROCESS_KEY"]}'       -- 02
                         , '{dr["L_CODE"]}'            -- 03
                         , '{dr["WORKDATE"]}'          -- 04
                         , '{dr["WORK_SEQ"]}'          -- 05
                         , '{dr["RESOURCE_NO"]}'       -- 06
                         , '{dr["P_Q"]}'               -- 07
                         , '{dr["P_Q_TIME"]}'          -- 08
                         , SYSDATE                     -- 09
                         , '{dr["SEND_YN"]}'           -- 10
                         , '{dr["R_YN"]}'              -- 11
                         , '{dr["C_CONDITION"]}'       -- 12
                         , SYSDATE                     -- 13
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
                        UPDATE PACK_ORDER
                        SET ERP_ISTATUS       = CASE TO_CHAR(ERP_ISTATUS) -- WHEN 'Y' THEN 'M' 
                                                    WHEN 'M' THEN 'U' WHEN 'X' THEN 'D' ELSE 'N' END
                            , ERP_ERR_CNT = 0
                            , ERP_OSTATUS       = CASE TO_CHAR(ERP_OSTATUS) -- WHEN 'Y' THEN 'M' 
                                                    WHEN 'M' THEN 'U' WHEN 'X' THEN 'D' ELSE 'N' END
                        WHERE PLANT_CODE      = '{dr["PLANT_CODE"]}'      -- 40
                            AND  PROCESS_KEY     = '{dr["PROCESS_KEY"].ToString().Replace("SAP_", "")}'     -- 41
                            AND  L_CODE          = '{dr["L_CODE"]}'          -- 42
                            AND  WORKDATE        = '{dr["WORKDATE"]}'        -- 43
                            AND  WORK_SEQ        = '{dr["WORK_SEQ"]}'        -- 44
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("ERP 전송 상태 수정이 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE PACK_REMARK
                        SET    P_Q         = '{dr["P_Q"]}'         -- 01
                             , P_Q_TIME    = '{dr["P_Q_TIME"]}'    -- 02
                             , IO_DATE     = SYSDATE     -- 03
                             , SEND_YN     = '{dr["SEND_YN"]}'     -- 04
                             , R_YN        = '{dr["R_YN"]}'        -- 05
                             , C_CONDITION = '{dr["C_CONDITION"]}' -- 06
                             , I_TIME      = SYSDATE               -- 07
                        WHERE  PLANT_CODE  = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE      = '{dr["L_CODE"]}'
                        AND    WORKDATE    = '{dr["WORKDATE"]}'
                        AND    WORK_SEQ    = '{dr["WORK_SEQ"]}'
                        AND    RESOURCE_NO = '{dr["RESOURCE_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        UPDATE PACK_ORDER
                        SET ERP_ISTATUS       = CASE TO_CHAR(ERP_ISTATUS) -- WHEN 'Y' THEN 'M' 
                                                WHEN 'X' THEN 'D' ELSE 'N' END
                            , ERP_IERR_CNT = 0
                            , ERP_OSTATUS       = CASE TO_CHAR(ERP_OSTATUS) -- WHEN 'Y' THEN 'M' 
                                                WHEN 'X' THEN 'D' ELSE 'N' END
                            , ERP_OERR_CNT = 0
                        WHERE PLANT_CODE      = '{dr["PLANT_CODE"]}'      -- 40
                            AND  PROCESS_KEY     = '{dr["PROCESS_KEY"].ToString().Replace("SAP_", "")}'     -- 41
                            AND  L_CODE          = '{dr["L_CODE"]}'          -- 42
                            AND  WORKDATE        = '{dr["WORKDATE"]}'        -- 43
                            AND  WORK_SEQ        = '{dr["WORK_SEQ"]}'        -- 44
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

                ShowMessageBox.XtraShowWarning("포장 실적을 저장 했습니다");

                Work_ERP_Result();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btnERPDelete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            string condition = gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"]);

            var targets = new[] { "Y", "C", "F" };

            for (int i = 0; i < gridView.RowCount; i++)
            {
                if (targets.Contains(gridView.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                 || targets.Contains(gridView.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                {
                    ShowMessageBox.XtraShowInformation("ERP 전송 완료된 작업지시는 삭제하실 수 없습니다");
                    return;
                }
            }

            if (viewERPPack.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(viewERPPack);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 포장 정보를 삭제하시겠습니까?"))
            {
                return;
            }

            XERP_Delete();
        }

        private void XERP_Delete()
        {
            try
            {
                splashScreenManager.ShowWaitForm();                

                SQL = $"DELETE FROM PACK_REMARK WHERE PLANT_CODE = '{viewERPPack.GetFocusedRowCellValue("PLANT_CODE")}' AND PROCESS_KEY = '{viewERPPack.GetFocusedRowCellValue("PROCESS_KEY")}' AND L_CODE = '{viewERPPack.GetFocusedRowCellValue("L_CODE")}' AND WORKDATE = '{viewERPPack.GetFocusedRowCellValue("WORKDATE")}' AND WORK_SEQ = '{viewERPPack.GetFocusedRowCellValue("WORK_SEQ")}' AND RESOURCE_NO = '{viewERPPack.GetFocusedRowCellValue("RESOURCE_NO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowWarning("포장 실적을 삭제 했습니다");

                Work_ERP_Result();
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

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!isInitializing)
                    return;

                gridControl.DataSource = null;
                gridIngred.DataSource = null;
                gridERPPack.DataSource = null;

                vLine_Code = cboL_Code.EditValue?.ToString();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_ShownEditor(object sender, EventArgs e)
        {
            if (gridView.FocusedColumn.FieldName == "NOTE")
            {
                LookUpEdit edit = (LookUpEdit)gridView.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.getNote(vPlant_Code, gridView.GetFocusedRowCellValue("RESOURCE_NO")?.ToString(), "2"), "", false, 0, false, true, false);
                edit.Properties.PopupFormMinSize = new Size(200, 300);
            }
        }

        /// <summary>
        /// 작업 복사
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWorkCopy_Click(object sender, EventArgs e)
        {
            //ShowMessageBox.XtraShowInformation("기능 추가 중입니다.");
            //return;
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }
            try
            {
                string scPlantCode = gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                string scProcesskey = gridView.GetFocusedRowCellValue("PROCESS_KEY")?.ToString();
                string scLCode = gridView.GetFocusedRowCellValue("L_CODE")?.ToString();
                string scBWorkDate = gridView.GetFocusedRowCellValue("WORKDATE").ToString().Replace("-", "");
                string scNum = gridView.GetFocusedRowCellValue("WORK_SEQ")?.ToString();

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

                    SQL = $@"
                    -- 포장 작업지시 정보
                    INSERT INTO PACK_ORDER 
                        (BAD_CODE1, BAD_CODE2, BAD_CODE3, BAD_CODE4, BAD_CODE5                              -- 1
                        , BAD_QTY1, BAD_QTY2, BAD_QTY3, BAD_QTY4, BAD_QTY5                                  -- 2
                        , BAG_STANDARD, C_CONDITION, DAN1, DAN2, DAN3                                       -- 3
                        , DEL_FLAG, DIFF, DOS_Q, E_Q, EMPLOYEE_NO                                           -- 4
                        , ERP_ERR_CNT, ERP_IERR_CNT, ERP_ISTATUS, ERP_ITNUMBER, ERP_OERR_CNT                -- 5
                        , ERP_OSTATUS, ERP_OTNUMBER, ERP_TNUMBER, ERP_UP_YN, ERR_MSG                        -- 6
                        , EXP_QTY, F_Q, HALT_TIME, I_TIME, ICM_CODE                                         -- 7
                        , L_CODE, LOCATION, NOTE, OR_QTY, P_TYPE                                            -- 8
                        , PA_Q, PLANT_CODE, PRO_QTY, PROCESS_KEY, REAL_QTY                                  -- 9
                        , RESOURCE_NO, RUN_ET, RUN_ST, SAMPLE_TLY, USE_EMP_PACK                             -- 10
                        , USE_PA_Q, WORK_SEQ, WORK_START_DATE, WORKDATE, WORK_SEQ_COPY)                     -- 11
                    SELECT 
                        BAD_CODE1, BAD_CODE2, BAD_CODE3, BAD_CODE4, BAD_CODE5                               -- 1
                        , BAD_QTY1, BAD_QTY2, BAD_QTY3, BAD_QTY4, BAD_QTY5                                  -- 2
                        , BAG_STANDARD, '{clsCommon.GetPcStatusCode("계획")}', DAN1, DAN2, DAN3             -- 3
                        , 'N', DIFF, DOS_Q, E_Q, '{clsCommon.UserId}'                                       -- 4
                        , ERP_ERR_CNT, ERP_IERR_CNT, 'W', '', ERP_OERR_CNT                                  -- 5
                        , 'W', '', ERP_TNUMBER, ERP_UP_YN, ''                                               -- 6
                        , EXP_QTY, F_Q, HALT_TIME, SYSDATE, ICM_CODE                                        -- 7
                        , L_CODE, LOCATION, NOTE, OR_QTY, P_TYPE                                            -- 8
                        , PA_Q, PLANT_CODE, PRO_QTY, PROCESS_KEY, REAL_QTY                                  -- 9
                        , RESOURCE_NO, RUN_ET, RUN_ST, SAMPLE_TLY, USE_EMP_PACK                             -- 10
                        , USE_PA_Q, '{WORK_SEQ}', WORK_START_DATE, '{string.Format("{0:yyyyMMdd}", sAWorkDate)}', WORK_SEQ                       -- 11
                    FROM PACK_ORDER
                    WHERE PLANT_CODE = '{scPlantCode}'
                        AND PROCESS_KEY = '{scProcesskey}'
                        AND L_CODE = '{scLCode}'
                        AND WORKDATE = '{scBWorkDate}'
                        AND WORK_SEQ = '{scNum}'
                        AND NVL(DEL_FLAG, 'N') != 'Y'
                    ORDER BY C_CONDITION DESC, WORKDATE, WORK_SEQ
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

        private void viewERPPack_ShowingEditor(object sender, CancelEventArgs e)
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

        private void gridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            int iDosQ = 0;
            int iProQ = 0;

            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            //지시량 자동계산 (배치수 * 배치량 = 지시량) 
            try
            {
                if (e.Column.FieldName == "DOS_Q")        // 배치량
                {
                    SQL = $@"
                    SELECT b.MENGE
                    FROM SAP_IN_BOM_CONM a
                        JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                                                                    AND b.NOTE = a.NOTE
                    WHERE b.RESOURCE_NO = '{view.GetFocusedRowCellValue("RESOURCE_NO")}'
                            AND SUBSTR(NVL(b.IDNRK, '1'), 1, 1) = '1'  
                            AND SUBSTR(b.RESOURCE_NO, 1, 1) = '1'
                            AND b.P_TYPE = '2' AND a.STLST = '2'
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        iDosQ = Convert.ToInt32(e.Value.ToString());

                        iProQ = iDosQ / Convert.ToInt32(Dbconn.conn.getData(ds, "MENGE", 0));

                        view.SetFocusedRowCellValue("OR_QTY", iProQ);
                    }
                }

                if (e.Column.FieldName == "LOCATION")        // 배치량
                {
                    SQL = $@"
                    -- 빈 리스트
                    SELECT DISTINCT b.RESOURCE_NO, b.DESCRIPTION
                    FROM BIN a 
                        LEFT JOIN SAP_DI_PRODUCT b on a.RESOURCE_NO = b.RESOURCE_NO
                    WHERE a.PLANT_CODE = '{view.GetFocusedRowCellValue("PLANT_CODE")}'
                        --AND a.PROCESS_KEY = '{view.GetFocusedRowCellValue("PROCESS_KEY")}'
                        --AND a.L_CODE = '{view.GetFocusedRowCellValue("L_CODE")}'
                        AND a.LOCATION = '{view.GetFocusedRowCellValue("LOCATION")}'
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    string sResourceNo = (ds != null &&
                                            ds.Tables.Count > 0 &&
                                            ds.Tables[0].Rows.Count > 0 &&
                                            ds.Tables[0].Columns.Contains("RESOURCE_NO"))
                                        ? Convert.ToString(ds.Tables[0].Rows[0]["RESOURCE_NO"])
                                        : string.Empty;


                    SQL = $@"
                    SELECT DISTINCT a.RESOURCE_NO, c.DESCRIPTION
                    FROM SAP_IN_BOM_CONM a
                        JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                                                                    AND b.NOTE = a.NOTE
                        LEFT JOIN SAP_DI_PRODUCT c on c.RESOURCE_NO = a.RESOURCE_NO
                    WHERE b.IDNRK = '{sResourceNo}'
                            AND SUBSTR(NVL(b.IDNRK, '1'), 1, 1) = '1'  
                            AND SUBSTR(b.RESOURCE_NO, 1, 1) = '1'
                            AND b.P_TYPE = '2' AND a.STLST = '2'
                    ";

                    ds = Dbconn.conn.ExecutDataset(SQL);

                    sResourceNo = (ds != null &&
                                            ds.Tables.Count > 0 &&
                                            ds.Tables[0].Rows.Count > 0 &&
                                            ds.Tables[0].Columns.Contains("RESOURCE_NO"))
                                        ? Convert.ToString(ds.Tables[0].Rows[0]["RESOURCE_NO"])
                                        : sResourceNo;

                    view.SetFocusedRowCellValue("RESOURCE_NO", sResourceNo);

                    //view.UpdateCurrentRow();
                    //view.CloseEditor();
                }

                if (e.Column.FieldName == "RESOURCE_NO")
                {
                    string sNOTE = clsCommon.getLastVersion(gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), e.Value?.ToString(), out chk_version, out dNote_Per);

                    gridView.SetRowCellValue(gridView.FocusedRowHandle, "NOTE", sNOTE);
                }
            }
            catch
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["OR_QTY"], 0);     // 지시량
            }
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

                decimal qty = Convert.ToDecimal(view.GetRowCellValue(rowHandle, "PRO_KG") ?? 0);
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