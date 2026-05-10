using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.Schedule;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Bag : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = string.Empty;
        private string[] sValid = null;

        bool chk_version = false;
        decimal dNote_Per = 0;

        public frm_Bag()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Bag_Load(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

                authDs = clsSql.GetAuthDataSet(this.Name);

                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                // ERP 진행여부
                clsDevexpressUtil.ItemLookUpEditSetup(cboERPUpLoad, clsCommon.GetTransFlag(), "", false, 0, true);

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;


                if (clsCommon._strUserType == "010608")
                {
                    layoutControlItem_workDateEd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
                else
                {

                    layoutControlItem_workDateEd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    dateEdit_workDateEd.EditValue = DateTime.Today.AddDays(1);
                }

                //작업조회
                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_Bag_Load", ex);
            }
        }

        private string workNumber_maker()
        {
            try
            {
                string return_seq = string.Empty;
                string SQL =
                "SELECT NVL(MAX(WORK_SEQ) + 1, 1) AS SEQ  " +
                "FROM BAG_ORDER WHERE WORKDATE = '{0}' ";

                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));

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

        #region 작업지시조회
        private void XMain_Search()
        {
            try
            {

                SQL = $@"
                SELECT a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, 
                    a.WORKDATE, a.WORK_SEQ, a.P_TYPE, 
                    a.RESOURCE_NO, a.NOTE, a.ICM_CODE, 
                    a.WORK_START_DATE, a.OR_QTY, a.HALT_TIME, 
                    a.RUN_ST, a.RUN_ET, a.EXP_QTY, 
                    a.PRO_QTY, a.PRO_KG,
                    a.E_Q, a.USE_END_QTY, 
                    a.END_QTY, a.LOCATION, a.F_Q, 
                    a.SAMPLE_TLY, a.BAD_CODE1, a.BAD_QTY1, 
                    a.BAD_CODE2, a.BAD_QTY2, a.BAD_CODE3, 
                    a.BAD_QTY3, a.BAD_CODE4, a.BAD_QTY4, 
                    a.BAD_CODE5, a.BAD_QTY5, a.C_CONDITION, 
                    a.DEL_FLAG, a.I_TIME, a.ERP_UP_YN, 
                    a.EMPLOYEE_NO,
                        CASE TO_CHAR(a.ERP_UP_YN) 
                            WHEN 'Y' THEN '전송완료' 
                            WHEN 'N' THEN '미전송' 
                            ELSE '미전송' 
                        END AS ERP_UP_YN,
                        CASE 
                            WHEN (a.RUN_ET - a.RUN_ST) * 1440 > 0 
                            THEN FLOOR((a.PRO_KG / ((a.RUN_ET - a.RUN_ST) * 1440)) * 60) 
                            ELSE 0 
                        END AS PROVITY,
                    a.REMARKS
                FROM BAG_ORDER a
                WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue}'
                    AND a.PROCESS_KEY = '{cboProcessKey.EditValue}'
                    AND a.L_CODE = '{cboL_Code.EditValue}'
                    AND a.WORKDATE BETWEEN TO_DATE('{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}', 'YYYYMMDD') 
                                    AND TO_DATE('{string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)}', 'YYYYMMDD')
                    AND NVL(a.DEL_FLAG, 'N') != 'Y'
                ORDER BY a.C_CONDITION DESC, a.WORKDATE, a.WORK_SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true);

                gridView.SetFixCol(new string[] {  "WORKDATE"
                                                , "WORK_SEQ"
                                                , "RESOURCE_NO"});

                sValid = new string[] { "PLANT_CODE", "PROCESS_KEY", "L_CODE", "WORKDATE", "ICM_CODE", "WORK_START_DATE", "LOCATION", "EMPLOYEE_NO" };

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

                //gridView.Columns["ERP_UP_YN"].OptionsColumn.AllowEdit = false;

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("타이콘")));

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[] {
                    new DataColumn("NAME"),
                    new DataColumn("CODE"),
                });

                string process_cd = string.Empty;

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboResourceNo, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString(), $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 0, true, false, "KG"), "품목을 선택 해주세요.", false);
                gridScboResourceNo.PopupFormSize = new Size(400, 400); // 가로 500, 세로 300
                DevExpress.XtraGrid.Views.Grid.GridView popupView = gridScboResourceNo.View as DevExpress.XtraGrid.Views.Grid.GridView;
                popupView.Columns["NAME"].Width = 180;

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboNOTE, clsCommon.getNote(cboPlant_Code.EditValue?.ToString(), "", "2"), "배합비 버전이 없습니다.", false);

                repItemLkUpEdit_LOCATION.NullText = "";
                repItemLkUpEdit_LOCATION.NullValuePrompt = "";
                repItemLkUpEdit_LOCATION.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                repItemLkUpEdit_LOCATION.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSuggest;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_LOCATION, clsCommon.GetBin("", "F"));

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboICM_CODE, clsCommon.GetICM(), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_EMPLOYEE_NO, clsCommon.GetDO_INSA(cboPlant_Code.EditValue?.ToString()));

                //작업계획
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_C_CONDITION, "03", "10");

                ////불량내역
                //repItemLkUpEdit_BAD_CODE.NullValuePrompt = "";
                //repItemLkUpEdit_BAD_CODE.NullText = "";
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
        #endregion

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

            clsDevexpressGrid.GridViewAddRow(gridView);

            string WORK_SEQ = workNumber_maker();

            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["PLANT_CODE"], cboPlant_Code.EditValue?.ToString());
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["PROCESS_KEY"], cboProcessKey.EditValue?.ToString());
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["L_CODE"], cboL_Code.EditValue.ToString());

            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["WORKDATE"], string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["WORK_START_DATE"], string.Format("{0:yyyyMMdd}", DateTime.Today));
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["WORK_SEQ"], WORK_SEQ);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["ICM_CODE"], clsCommon.GetICMGubun());

            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["EMPLOYEE_NO"], clsCommon.UserId);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["OR_QTY"], 0);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["PRO_QTY"], 0);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["PRO_KG"], 0);
            gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"], clsCommon.PcStatus.Plan);
            gridView.SetFocusedRowCellValue("ERP_UP_YN", clsCommon.GetTransFlagCode("신규"));
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
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

                DataRow dr = gridView.GetDataRow(gridView.FocusedRowHandle);

                if (dr.RowState == DataRowState.Added)
                {
                    gridView.DeleteRow(gridView.FocusedRowHandle);
                }
                else
                {
                    string condition = gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"]);

                    if (condition.Equals("완료"))
                    {
                        ShowMessageBox.XtraShowInformation("완료된 작업지시는 삭제하실 수 없습니다");
                        return;
                    }

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업지시를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        Dbconn.conn.BeginTransaction();

                        int[] selectedRows = gridView.GetSelectedRows();

                        if (selectedRows.Length == 0)
                        {
                            ShowMessageBox.XtraShowWarning("복사 할 자재를 먼저 선택 해주세요.");
                            return;
                        }

                        foreach (int rowHandle in selectedRows)
                        {
                            var chkDr = gridView.GetDataRow(rowHandle);

                            chkDr.ClearErrors();

                            SQL = $@"
                            UPDATE BAG_ORDER
                                SET    DEL_FLAG        = 'Y'
                            WHERE  PLANT_CODE      = '{chkDr["PLANT_CODE"]}'
                                AND    PROCESS_KEY     = '{chkDr["PROCESS_KEY"]}'
                                AND    L_CODE          = '{chkDr["L_CODE"]}'
                                AND    WORKDATE        = '{chkDr["WORKDATE"]}'
                                AND    WORK_SEQ        = '{chkDr["WORK_SEQ"]}'
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 1)
                            {
                                Dbconn.conn.Rollback();
                                clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                                ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                                return;
                            }
                        }

                        Dbconn.conn.Commit();
                        XMain_Search();
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

            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            XMain_Save();
        }

        private void XMain_Save()
        {
            try
            {
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
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

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
                        string WORK_SEQ = workNumber_maker();

                        if (string.IsNullOrEmpty(WORK_SEQ))
                        {
                            Dbconn.conn.Rollback();
                            ShowMessageBox.XtraShowInformation("작업순번을 생성하는 도중 에러가 발생했습니다");
                            return;
                        }

                        SQL = $@"
                        INSERT INTO BAG_ORDER (
                              PLANT_CODE        -- 01
                            , PROCESS_KEY       -- 02
                            , L_CODE            -- 03
                            , WORKDATE          -- 04
                            , WORK_SEQ          -- 05
                            , P_TYPE            -- 06
                            , RESOURCE_NO       -- 07
                            , NOTE              -- 08
                            , ICM_CODE          -- 09
                            , WORK_START_DATE   -- 10
                            , OR_QTY            -- 11
                            , HALT_TIME         -- 12
                            , RUN_ST            -- 13
                            , RUN_ET            -- 14
                            , EXP_QTY           -- 15
                            , PRO_QTY           -- 16
                            , PRO_KG -- 17     
                            , E_Q               -- 17
                            , USE_END_QTY       -- 18
                            , END_QTY           -- 19
                            , LOCATION          -- 20
                            , F_Q               -- 21
                            , SAMPLE_TLY        -- 22
                            , BAD_CODE1         -- 23
                            , BAD_QTY1          -- 24
                            , BAD_CODE2         -- 25
                            , BAD_QTY2          -- 26
                            , BAD_CODE3         -- 27
                            , BAD_QTY3          -- 28
                            , BAD_CODE4         -- 29
                            , BAD_QTY4          -- 30
                            , BAD_CODE5         -- 31
                            , BAD_QTY5          -- 32
                            , C_CONDITION       -- 33
                            , DEL_FLAG          -- 34
                            , I_TIME            -- 35
                            , ERP_UP_YN         -- 36
                            , EMPLOYEE_NO       -- 37
                            , REMARKS           --        
                        )
                        VALUES (
                                '{dr["PLANT_CODE"]}'        -- 01
                            , '{dr["PROCESS_KEY"]}'       -- 02
                            , '{dr["L_CODE"]}'            -- 03
                            , '{dr["WORKDATE"]}'          -- 04
                            , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) FROM BAG_ORDER WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}' AND WORKDATE = '{dr["WORKDATE"]}')          -- 05
                            , '2'            -- 06
                            , '{dr["RESOURCE_NO"]}'       -- 07
                            , '{dr["NOTE"]}'              -- 08
                            , '{dr["ICM_CODE"]}'          -- 09
                            , '{dr["WORK_START_DATE"]}'   -- 10
                            , '{dr["OR_QTY"]}'            -- 11
                            , '{dr["HALT_TIME"]}'         -- 12
                            , {sDtFrom}            -- 13
                            , {sDtTo}            -- 14
                            , '{dr["EXP_QTY"]}'           -- 15
                            , '{dr["PRO_QTY"]}'           -- 16
                            , '{dr["PRO_KG"]}'            -- 17
                            , '{dr["E_Q"]}'               -- 17
                            , '{dr["USE_END_QTY"]}'       -- 18
                            , '{dr["END_QTY"]}'           -- 19
                            , '{dr["LOCATION"]}'          -- 20
                            , '{dr["F_Q"]}'               -- 21
                            , '{dr["SAMPLE_TLY"]}'        -- 22
                            , '{dr["BAD_CODE1"]}'         -- 23
                            , '{dr["BAD_QTY1"]}'          -- 24
                            , '{dr["BAD_CODE2"]}'         -- 25
                            , '{dr["BAD_QTY2"]}'          -- 26
                            , '{dr["BAD_CODE3"]}'         -- 27
                            , '{dr["BAD_QTY3"]}'          -- 28
                            , '{dr["BAD_CODE4"]}'         -- 29
                            , '{dr["BAD_QTY4"]}'          -- 30
                            , '{dr["BAD_CODE5"]}'         -- 31
                            , '{dr["BAD_QTY5"]}'          -- 32
                            , '{dr["C_CONDITION"]}'       -- 33
                            , '{dr["DEL_FLAG"]}'          -- 34
                            , SYSDATE                     -- 35
                            , '{dr["ERP_UP_YN"]}'         -- 36
                            , '{dr["EMPLOYEE_NO"]}'       -- 37
                            , '{dr["REMARKS"]}'           --
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }



                        //string erp_insert_chk = clsErpSql.InsertWorkOrder(clsCommon.bag_process_code, string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), WORKDATE, dr, "I");
                        //if (erp_insert_chk != "OK")
                        //{
                        //    Dbconn.conn.Rollback();
                        //    ShowMessageBox.XtraShowWarning("작업지시 입력에 실패했습니다(ERP INPUT FAIL)");
                        //    return;
                        //}
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE BAG_ORDER
                        SET    P_TYPE          = '{dr["P_TYPE"]}'          -- 01
                             , RESOURCE_NO     = '{dr["RESOURCE_NO"]}'     -- 02
                             , NOTE            = '{dr["NOTE"]}'            -- 03
                             , ICM_CODE        = '{dr["ICM_CODE"]}'        -- 04
                             , WORK_START_DATE = '{dr["WORK_START_DATE"]}' -- 05
                             , OR_QTY          = '{dr["OR_QTY"]}'          -- 06
                             , HALT_TIME       = '{dr["HALT_TIME"]}'       -- 07
                             , RUN_ST          = {sDtFrom}          -- 08
                             , RUN_ET          = {sDtTo}          -- 09
                             , EXP_QTY         = '{dr["EXP_QTY"]}'         -- 10
                             , PRO_QTY         = '{dr["PRO_QTY"]}'         -- 11
                             , PRO_KG          = '{dr["PRO_KG"]}'         -- 11
                             , E_Q             = '{dr["E_Q"]}'             -- 12
                             , USE_END_QTY     = '{dr["USE_END_QTY"]}'     -- 13
                             , END_QTY         = '{dr["END_QTY"]}'         -- 14
                             , LOCATION        = '{dr["LOCATION"]}'        -- 15
                             , F_Q             = '{dr["F_Q"]}'             -- 16
                             , SAMPLE_TLY      = '{dr["SAMPLE_TLY"]}'      -- 17
                             , BAD_CODE1       = '{dr["BAD_CODE1"]}'       -- 18
                             , BAD_QTY1        = '{dr["BAD_QTY1"]}'        -- 19
                             , BAD_CODE2       = '{dr["BAD_CODE2"]}'       -- 20
                             , BAD_QTY2        = '{dr["BAD_QTY2"]}'        -- 21
                             , BAD_CODE3       = '{dr["BAD_CODE3"]}'       -- 22
                             , BAD_QTY3        = '{dr["BAD_QTY3"]}'        -- 23
                             , BAD_CODE4       = '{dr["BAD_CODE4"]}'       -- 24
                             , BAD_QTY4        = '{dr["BAD_QTY4"]}'        -- 25
                             , BAD_CODE5       = '{dr["BAD_CODE5"]}'       -- 26
                             , BAD_QTY5        = '{dr["BAD_QTY5"]}'        -- 27
                             , C_CONDITION     = '{dr["C_CONDITION"]}'     -- 28
                             , DEL_FLAG        = '{dr["DEL_FLAG"]}'        -- 29
                             , I_TIME          = SYSDATE                   -- 30
                             , ERP_UP_YN       = '{dr["ERP_UP_YN"]}'       -- 31
                             , EMPLOYEE_NO     = '{dr["EMPLOYEE_NO"]}'     -- 32
                             , REMARKS         = '{dr["REMARKS"]}'         --
                        WHERE  PLANT_CODE      = '{dr["PLANT_CODE"]}'
                        AND    PROCESS_KEY     = '{dr["PROCESS_KEY"]}'
                        AND    L_CODE          = '{dr["L_CODE"]}'
                        AND    WORKDATE        = '{dr["WORKDATE"]}'
                        AND    WORK_SEQ        = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {

                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        //string erp_insert_chk = clsErpSql.InsertWorkOrder(clsCommon.bag_process_code, string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), dr["WORK_SEQ"].ToString(), dr, "U");
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

                ShowMessageBox.XtraShowInformation("작업지시를 저장 했습니다.");

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void repItemLkUpEdit_RESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (gridView.FocusedColumn.FieldName.Contains("REMARK"))
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
                string plantCode = gridView.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                string processKey = gridView.GetFocusedRowCellValue("PROCESS_KEY")?.ToString();
                string lCode = gridView.GetFocusedRowCellValue("L_CODE")?.ToString();
                string work_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORKDATE");
                string work_seq = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORK_SEQ");
                string L_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "L_CODE");

                string con_st = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "C_CONDITION");

                string resource_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "RESOURCE_NO");


                //if (con_st == clsCommon.PcStatus.Completed)
                //{
                //    ShowMessageBox.XtraShowInformation("이미 완료처리 된 작업지시 입니다");
                //    return;
                //}

                m_bagEnd mBagEnd = new m_bagEnd(plantCode, processKey, lCode, work_num, work_seq, L_CODE);
                mBagEnd.StartPosition = FormStartPosition.CenterScreen;
                DialogResult rlt = mBagEnd.ShowDialog();

                if (rlt == DialogResult.OK)
                {
                    Dbconn.conn.BeginTransaction();

                    SQL = $@"
                    SELECT *
                    FROM BAG_ORDER
                    WHERE PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}'
                        AND PROCESS_KEY = '{cboProcessKey.EditValue}'
                        AND L_CODE = '{cboL_Code.EditValue}'
                        AND WORKDATE = '{work_num}' AND WORK_SEQ = '{work_seq}' AND L_CODE = '{L_CODE}' ";

                    DataSet work_ds = Dbconn.conn.ExecutDataset(SQL);
                    DataRow row = work_ds.Tables[0].Rows[0];

                    SQL = $@"
                    UPDATE BAG_ORDER
                        SET C_CONDITION = '{clsCommon.PcStatus.Completed}'
                    WHERE  PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}'
                        AND PROCESS_KEY = '{cboProcessKey.EditValue}'
                        AND L_CODE = '{cboL_Code.EditValue}'
                        AND WORKDATE = '{work_num}' AND WORK_SEQ = '{work_seq}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        Dbconn.conn.Rollback();
                        ShowMessageBox.XtraShowWarning("작업지시 강제완료에 실패했습니다");
                        return;
                    }

                    Dbconn.conn.Commit();

                    //SQL = $"SELECT NOTE FROM SAP_IN_BOM_CONM WHERE P_TYPE = '2' AND RESOURCE_NO = '{resource_no}' ORDER BY NOTE DESC";

                    //DataSet note_ds = Dbconn.conn.ExecutDataset(SQL);

                    //SQL = $" UPDATE BAG_ORDER SET NOTE = '{Dbconn.conn.getData(note_ds, "NOTE", 0).Trim()}' WHERE WORKDATE = '{work_num}' AND WORK_SEQ = '{work_seq}'";

                    //Dbconn.conn.SQLrun(SQL);

                    XMain_Search();

                    ShowMessageBox.XtraShowInformation("강제완료처리가 완료되었습니다");
                }
            }
            else
            {
                Dbconn.conn.Rollback();
                ShowMessageBox.XtraShowInformation("해당 작업지시는 저장을 완료하신후에 강제완료 하여 주시길 바랍니다");
                return;
            }
        }

        private void gridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (gridView == null)
                    return;

                string condition = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["C_CONDITION"]).ToString();

                if (condition == clsCommon.PcStatus.Completed) //완료
                {
                    e.Appearance.BackColor = Color.LightGray;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
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


        private void lookUpEdit_sbno_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "C_CONDITION")
            {
                int sumText = 0;

                SQL = $@"
                SELECT 
                    FLOOR(
                        NVL(
                            ( A.PRO_Q / (A.MIN_SUM - NVL(RT.REST_MINUTES, 0)) ) * 60, 
                        0)
                    ) AS PROVITY
                FROM (
                    SELECT 
                        SUM(PRO_QTY) AS PRO_Q,
                        SUM((RUN_ET - RUN_ST) * 24 * 60) AS MIN_SUM
                    FROM BAG_ORDER
                    WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                        AND PROCESS_KEY = '{cboProcessKey.EditValue}'
                        AND L_CODE = '{cboL_Code.EditValue}'
                        AND WORK_START_DATE BETWEEN '{dateEdit_workDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workDateEd.DateTime.ToString("yyyyMMdd")}'
                        AND C_CONDITION = '{clsCommon.PcStatus.Completed}'
                        AND (RUN_ET - RUN_ST) IS NOT NULL
                ) A
                LEFT JOIN (
                    SELECT 
                        SUM(REST_MINUTES) AS REST_MINUTES
                    FROM REST_TIME 
                    WHERE PLANT_CODE = '{cboPlant_Code.EditValue}'
                        AND PROCESS_KEY = '{cboProcessKey.EditValue}'
                        AND L_CODE = '{cboL_Code.EditValue}'
                        AND WORKDATE BETWEEN '{dateEdit_workDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workDateEd.DateTime.ToString("yyyyMMdd")}'
                ) RT ON 1=1
                ";

                DataSet proDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(proDs) > 0)
                {
                    if (Dbconn.conn.getData(proDs, "PROVITY", 0) != "")
                    {
                        sumText = Convert.ToInt32(Dbconn.conn.getData(proDs, "PROVITY", 0));
                    }

                }

                e.Info.DisplayText = "생산성 : " + String.Format("{0:#,###}", sumText);
            }
        }

        private void gridView_ShownEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if (view.FocusedColumn.FieldName == "NOTE")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.getNote(cboPlant_Code.EditValue?.ToString(), view.GetFocusedRowCellValue("RESOURCE_NO")?.ToString()), "", false, 0, false, true, false);
                edit.Properties.PopupFormMinSize = new Size(200, 300);
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
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
                    FROM BAG_ORDER
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
                        UPDATE BAG_ORDER
                        SET    ERP_UP_YN   = CASE TO_CHAR(ERP_UP_YN) WHEN 'N' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                     ELSE TO_CHAR(ERP_UP_YN) END
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

        private void cboERPUpLoad_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);
            cboProcessKey.EditValue = clsCommon.GetProcessKey("타이콘");

            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessKey("타이콘")), "", false, 0, false);

            XMain_Search();
        }

        private void gridScboResourceNo_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            string sResourceNo = textEditor.EditValue.ToString();
            string sBatchQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "BATCH_Q");

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "RESOURCE_NO", textEditor.EditValue);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "P_TYPE", 2);

            string sNOTE = clsCommon.getLastVersion(cboPlant_Code.EditValue?.ToString(), sResourceNo, out chk_version, out dNote_Per);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "NOTE", sNOTE);
        }
    }
}