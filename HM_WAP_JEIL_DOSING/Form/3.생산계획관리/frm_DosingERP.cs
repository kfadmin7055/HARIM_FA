using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Web.UI.WebControls;
using System.Windows.Forms;


namespace HARIM_FA_DOSING
{
    public partial class frm_DosingERP : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;

        string vPlantCode = string.Empty;
        string vProcessKey = string.Empty;
        string vLCode = string.Empty;
        string vWorkDate = string.Empty;
        string vNum = string.Empty;
        string vResourceNo = string.Empty;
        string vNote = string.Empty;
        private string[] sValid = null;
        string sMSG = string.Empty;

        private bool isInitializing = false;

        public frm_DosingERP()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewWork, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewIngred, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(viewUpload, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewPreview, Properties.Settings.Default.FontSize);
            viewWork.OptionsView.ShowGroupPanel = false;
        }

        public static string workNumMake(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate)
        {
            try
            {
                string SQL = $@"
                SELECT NVL(MAX(NUM) + 1, 1) AS SEQ
                FROM WORK_ORDER
                WHERE PLANT_CODE = '{sPlantCode}'
                    AND PROCESS_KEY = '{sProcessKey}'
                    AND L_CODE = '{sLCode}'
                    AND WORKDATE = '{sWorkDate}'
                ";

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
                clsLog.logSave("clsProcessDosing", "workNumMake", ex);
                return string.Empty;
            }
        }


        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridWork, viewWork);
        }

        #region 폼로드 이벤트
        private void frm_Bin_Load(object sender, EventArgs e)
        {
            viewWork.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridWork, viewWork);

            try
            {
                authDs = clsSql.GetAuthDataSet(this.Name);

                isInitializing = true;

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;
                dateEdit_workDateEd.EditValue = DateTime.Today.AddDays(1);

                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                // ERP 진행여부
                //clsDevexpressUtil.ItemLookUpEditSetup(cboERPUpLoad, clsCommon.GetTransFlag(), "", false, 0, true);



                // 삭제여부
                //clsDevexpressUtil.ItemLookUpEditSetup(cboDelFlag, clsCommon.GetYn(null, new string[] {"삭제", "미삭제"}), "", false, 0, true);

                InitControl();

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_Bin_Load", ex);
                ShowMessageBox.XtraShowWarning("화면을 불러오는 도중 에러가 발생했습니다");
            }

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridWork));
            // Application.AddMessageFilter(new GridMouseWheelFilter(gridIngred));
            // Application.AddMessageFilter(new GridMouseWheelFilter(gridUpload));
            // Application.AddMessageFilter(new GridMouseWheelFilter(gridPreview));
        }

        private void InitControl()
        {
            gridChk.ValueChecked = "Y";
            gridChk.ValueUnchecked = "N";
            gridChk.NullStyle = StyleIndeterminate.Unchecked;
            gridChk.CheckStyle = CheckStyles.Standard;

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboLCode, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()));

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboERP_OSTATUS, clsCommon.GetTransFlag());

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "전체선택", false, 2, true);
        }
        #endregion

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                DataSet ds = null;
                string sDescription = string.Empty;
                string sResourceNo = string.Empty;

                SQL = $@"
                -- 작업지시내역
                SELECT
                  CHK
                , PLANT_CODE
                , PROCESS_KEY
                , L_CODE
                , WORKDATE
                , NUM
                , P_TYPE
                , RESOURCE_NO
                , DESCRIPTION
                , NOTE
                , WORK_START_DATE
                , BATCH
                , R_BATCH
                , BATCH_Q
                , OR_Q
                , PRO_Q
                , ERP_Q
                , ROUND((END_TIME - START_TIME) * 24, 2) AS WORK_HOUR
                 , CASE 
                        WHEN (END_TIME - START_TIME) * 24 = 0 THEN 0
                        ELSE ROUND(
                                (NVL(ERP_Q, 0) / 1000) 
                                / ((END_TIME - START_TIME) * 24)
                            , 2)
                    END AS ERP_Q_PER_HOUR
                    , GUBUN
                    , LOCATION_ED
                    , LOCATION_ED2
                    , REMARK
                    , ICM_CODE
                    , C_CONDITION
                    , HALT_TIME
                    , START_TIME
                    , END_TIME
                    , BBATCH_Q
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
                    , ERP_ISTATUS
                    , ERP_ITNUMBER
                    , ERP_OSTATUS
                    , ERP_OTNUMBER
                    , DEL_FLAG
                    , I_TIME
                    , EMPLOYEE_NO
                    , TRANS_YN
                    , ERR_MSG
                    , NUM_COPY
                FROM (
                    SELECT 'N' AS CHK
                         , a.PLANT_CODE
                         , a.PROCESS_KEY
                         , a.L_CODE
                         , TO_CHAR(TO_DATE(a.WORKDATE, 'YYYYMMDD'), 'YYYY-MM-DD') AS WORKDATE
                         , a.NUM
                         , a.P_TYPE
                         , a.RESOURCE_NO
                         , a.RESOURCE_NO || ' : ' || b.DESCRIPTION AS DESCRIPTION
                         , a.NOTE
                         , a.WORK_START_DATE
                         , a.BATCH
                         , a.R_BATCH
                         , a.BATCH_Q
                         , a.OR_Q
                         , a.PRO_Q
                         , (SELECT SUM(wr.P_Q) 
                              FROM WORK_REMARK wr
                             WHERE wr.PLANT_CODE = a.PLANT_CODE
                               AND wr.PROCESS_KEY = 'SAP_' || a.PROCESS_KEY
                               AND wr.L_CODE = a.L_CODE
                               AND wr.WORKDATE = a.WORKDATE
                               AND wr.NUM = a.NUM
                           ) AS ERP_Q
                         , a.GUBUN
                         , a.LOCATION_ED
                         , a.LOCATION_ED2
                         , a.REMARK
                         , a.ICM_CODE
                         , a.C_CONDITION
                         , a.HALT_TIME
                         , a.START_TIME
                         , a.END_TIME
                         , (SELECT SUM(CASE WHEN wo.BU_YN = 'Y' THEN wo.BATCH * wd.SET_VAL ELSE 0 END)
                              FROM WORK_ORDER wo
                              JOIN WORK_DETAIL wd 
                                ON wd.PLANT_CODE = wo.PLANT_CODE
                               AND wd.PROCESS_KEY = wo.PROCESS_KEY
                               AND wd.L_CODE = wo.L_CODE
                               AND wd.WORKDATE = wo.WORKDATE
                               AND wd.NUM = wo.NUM
                               AND wd.INGRED_CODE IN (
                                    SELECT RESOURCE_NO_2 
                                      FROM SAP_IN_PRODUCT_RC 
                                     WHERE PLANT_CODE = wo.PLANT_CODE 
                                       AND RESOURCE_NO = wo.RESOURCE_NO
                               )
                             WHERE wo.PLANT_CODE = a.PLANT_CODE
                               AND wo.PROCESS_KEY = a.PROCESS_KEY
                               AND wo.L_CODE = a.L_CODE
                               AND wo.WORKDATE = a.WORKDATE
                               AND wo.NUM = a.NUM
                           ) AS BBATCH_Q
                         , a.BAD_CODE1, a.BAD_QTY1
                         , a.BAD_CODE2, a.BAD_QTY2
                         , a.BAD_CODE3, a.BAD_QTY3
                         , a.BAD_CODE4, a.BAD_QTY4
                         , a.BAD_CODE5, a.BAD_QTY5
                         , a.ERP_ISTATUS
                         , a.ERP_ITNUMBER
                         , a.ERP_OSTATUS
                         , a.ERP_OTNUMBER
                         , a.DEL_FLAG
                         , a.I_TIME
                         , a.EMPLOYEE_NO
                         , a.TRANS_YN
                         , a.ERR_MSG
                         , a.NUM_COPY
                    FROM WORK_ORDER a
                    LEFT JOIN SAP_DI_PRODUCT b 
                           ON b.PLANT_CODE = a.PLANT_CODE
                          AND b.RESOURCE_NO = a.RESOURCE_NO
                    WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                      AND ('{cboProcessKey.EditValue}' IS NULL OR a.PROCESS_KEY = '{cboProcessKey.EditValue}')
                      AND ('{cboL_Code.EditValue}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue?.ToString()}')
                      AND a.WORKDATE BETWEEN '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                                          AND '{string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)}'
                      AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
                      AND a.C_CONDITION IN ('{clsCommon.PcStatus.Completed}', '{clsCommon.PcStatus.ForceCompleted}', '{clsCommon.PcStatus.Canceled}')
                     ) 
                ORDER BY WORKDATE DESC, END_TIME DESC, NVL(NUM_COPY, NUM) DESC, CASE WHEN NUM_COPY IS NULL THEN 0 ELSE 1 END, NUM
                ";
                // AND ('{cboDelFlag.EditValue}' IS NULL OR DEL_FLAG = '{cboDelFlag.EditValue}')
                ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridWork, viewWork, ds.Tables[0], true, true);

                sValid = new string[] { "" };

                viewWork.Columns["ERP_OSTATUS"].OptionsColumn.AllowEdit = false;

                string argResType = string.Empty;
                string argProFilter = string.Empty;

                gridcboRESOURCE_NO.NullText = "";
                gridcboRESOURCE_NO.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                gridcboRESOURCE_NO.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch;
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));
                gridcboRESOURCE_NO.PopupFormMinSize = new Size(500, 600);

                ds.Dispose();

                xSearchWorkResult();
                xSearchERPPreview();
                xSearchERPUpload();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다" + SQL);
            }
        }

        private void xSearchWorkResult()
        {
            try
            {
                SQL = $@"
                -- 작업 진행 내역
                SELECT wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.RESOURCE_NO, wo.NOTE
                    , wr.BATCH, wd.LOCATION, wd.SEQ, wd.INGRED_CODE, wd.SET_VAL, wd.QTY_PCT, wr.P_Q
                FROM WORK_ORDER wo
                    JOIN WORK_DETAIL wd ON wo.PLANT_CODE = wd.PLANT_CODE AND wo.PROCESS_KEY = wd.PROCESS_KEY AND wd.L_CODE = wo.L_CODE
                                            AND wo.WORKDATE = wd.WORKDATE AND wo.NUM = wd.NUM
                    LEFT JOIN WORK_REMARK wr ON wo.PLANT_CODE = wr.PLANT_CODE AND wo.PROCESS_KEY = wr.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                            AND wo.WORKDATE = wr.WORKDATE AND wo.NUM = wr.NUM AND wr.LOCATION = wd.LOCATION AND wr.INGRED_LOT = wd.INGRED_CODE
                WHERE ('{txtBatch.EditValue}' IS NULL OR wr.BATCH = '{txtBatch.EditValue}')
                    AND wo.PLANT_CODE = '{vPlantCode}' AND wo.PROCESS_KEY = '{vProcessKey}' AND wo.L_CODE = '{vLCode}' AND wo.WORKDATE = '{vWorkDate}' AND wo.NUM = '{vNum}'
                ORDER BY wr.BATCH, wd.SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                clsDevexpressGrid.BindGridControl(gridIngred, viewIngred, ds.Tables[0], false, true);

                sValid = new string[] { "" };


                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridIngscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xSearchWorkResult()", ex);
                clsLog.logSave(this, "xSearchWorkResult()", ex.StackTrace);
                ShowMessageBox.XtraShowError("작업지시내역을 조회하는중 오류가 발생했습니다.");
            }
        }

        private void xSearchERPPreview()
        {
            try
            {
                SQL = $@"
                -- ERP 작업 진행 내역
                SELECT PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM, BATCH, LOCATION, SEQ, RESOURCE_NO, NOTE
                    , INGRED_LOT AS INGRED_CODE, SUM(P_Q) AS P_Q
                FROM (
                    SELECT wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, wr.LOCATION, wr.SEQ, wr.INGRED_LOT, wr.P_Q
                    FROM WORK_ORDER wo
                        LEFT JOIN WORK_REMARK wr ON wo.PLANT_CODE = wr.PLANT_CODE AND 'SAP_' || wo.PROCESS_KEY = wr.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                                AND wo.WORKDATE = wr.WORKDATE AND wo.NUM = wr.NUM
                    WHERE ('{txtBatch.EditValue}' IS NULL OR wr.BATCH = '{txtBatch.EditValue}')
                        AND wo.PLANT_CODE = '{vPlantCode}' AND wo.PROCESS_KEY = '{vProcessKey}' AND wo.L_CODE = '{vLCode}' AND wo.WORKDATE = '{vWorkDate}' AND wo.NUM = '{vNum}'
                    ORDER BY wr.BATCH, wr.SEQ
                )
                GROUP BY PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM, BATCH, LOCATION, SEQ, RESOURCE_NO, NOTE, SEQ, LOCATION, INGRED_LOT
                ORDER BY BATCH, SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                clsDevexpressGrid.BindGridControl(gridPreview, viewPreview, ds.Tables[0], false, true);

                sValid = new string[] { "PLANT_CODE", "PROCESS_KEY", "L_CODE", "BATCH", "P_Q" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = viewPreview.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xSearchERPPreview()", ex);
                clsLog.logSave(this, "xSearchERPPreview()", ex.StackTrace);
                ShowMessageBox.XtraShowError("ERP 전송 상세내역 조회하는중 오류가 발생했습니다.");
            }
        }

        private void xSearchERPUpload()
        {
            try
            {
                //SQL = $@"
                //-- ERP 업로드 예상내역
                //SELECT PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM, RESOURCE_NO, NOTE, SEQ, LOCATION
                //    , INGRED_CODE, SUM(P_Q) AS P_Q
                //FROM (
                //    SELECT PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM, BATCH, LOCATION, SEQ, RESOURCE_NO, NOTE
                //        , INGRED_LOT AS INGRED_CODE, SUM(P_Q) AS P_Q
                //    FROM (
                //        SELECT wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, wr.LOCATION, wr.SEQ, wr.INGRED_LOT, wr.P_Q
                //        FROM WORK_ORDER wo
                //            LEFT JOIN WORK_REMARK wr ON wo.PLANT_CODE = wr.PLANT_CODE AND 'SAP_' || wo.PROCESS_KEY = wr.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                //                                    AND wo.WORKDATE = wr.WORKDATE AND wo.NUM = wr.NUM
                //        WHERE ('{txtBatch.EditValue}' IS NULL OR wr.BATCH = '{txtBatch.EditValue}')
                //            AND wo.PLANT_CODE = '{vPlantCode}' AND wo.PROCESS_KEY = '{vProcessKey}' AND wo.L_CODE = '{vLCode}' AND wo.WORKDATE = '{vWorkDate}' AND wo.NUM = '{vNum}'
                //        ORDER BY wr.BATCH, wr.SEQ
                //    )
                //    GROUP BY PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM, BATCH, LOCATION, SEQ, RESOURCE_NO, NOTE, SEQ, LOCATION, INGRED_LOT
                //    ORDER BY BATCH, SEQ
                //)
                //GROUP BY PROCESS_KEY, L_CODE, WORKDATE, PLANT_CODE, NUM, RESOURCE_NO, NOTE, SEQ, LOCATION, INGRED_CODE
                //ORDER BY SEQ
                //";

                SQL = $@"
                -- ERP 업로드 예상내역
                SELECT wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.NUM, wo.RESOURCE_NO, wo.NOTE
                    , MAX(wr.LOCATION) AS LOCATION, MAX(wr.SEQ) AS SEQ
                    , wr.INGRED_LOT AS INGRED_CODE, p.DESCRIPTION AS INGRED_NAME, SUM(wr.P_Q) AS P_Q
                FROM WORK_ORDER wo
                    LEFT JOIN WORK_REMARK wr ON wo.PLANT_CODE = wr.PLANT_CODE AND 'SAP_' || wo.PROCESS_KEY = wr.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                            AND wo.WORKDATE = wr.WORKDATE AND wo.NUM = wr.NUM
                    LEFT JOIN SAP_DI_PRODUCT P ON p.PLANT_CODE = wo.PLANT_CODE AND p.RESOURCE_NO = wr.INGRED_LOT
                WHERE ('{txtBatch.EditValue}' IS NULL OR wr.BATCH = '{txtBatch.EditValue}')
                    AND wo.PLANT_CODE = '{vPlantCode}' AND wo.PROCESS_KEY = '{vProcessKey}' AND wo.L_CODE = '{vLCode}' AND wo.WORKDATE = '{vWorkDate}' AND wo.NUM = '{vNum}'
                GROUP BY wr.PLANT_CODE, wr.PROCESS_KEY, wr.L_CODE, wr.WORKDATE, wr.NUM, wo.RESOURCE_NO, wo.NOTE, wr.INGRED_LOT, p.DESCRIPTION
                ORDER BY SEQ
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                clsDevexpressGrid.BindGridControl(gridUpload, viewUpload, ds.Tables[0], false, true);

                sValid = new string[] { "" };


                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridUpscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xSearchERPUpload()", ex);
                clsLog.logSave(this, "xSearchERPUpload()", ex.StackTrace);
                ShowMessageBox.XtraShowError("ERP 전송내역 조회하는중 오류가 발생했습니다.");
            }
        }
        #endregion

        #region 새로고침 버튼클릭 이벤트
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }
        #endregion

        #region 그리드 값변경후 이벤트
        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            viewWork.UpdateCurrentRow();
        }
        #endregion

        #region 그리드 로우넘버 그리기
        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {

        }

        private void viewPreview_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void viewUpload_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }
        #endregion

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gridIngred.DataSource = null;
                gridPreview.DataSource = null;
                gridUpload.DataSource = null;

                vPlantCode = clsDevexpressGrid.GetFocusedRowCellValue(viewWork, "PLANT_CODE");
                vProcessKey = clsDevexpressGrid.GetFocusedRowCellValue(viewWork, "PROCESS_KEY");
                vLCode = clsDevexpressGrid.GetFocusedRowCellValue(viewWork, "L_CODE");

                var value = clsDevexpressGrid.GetFocusedRowCellValue(viewWork, "WORKDATE");

                if (value != null && DateTime.TryParse(value.ToString(), out DateTime dt))
                {
                    vWorkDate = dt.ToString("yyyyMMdd");
                    // 예: 20250513
                }

                vNum = clsDevexpressGrid.GetFocusedRowCellValue(viewWork, "NUM");
                vResourceNo = clsDevexpressGrid.GetFocusedRowCellValue(viewWork, "RESOURCE_NO");
                vNote = clsDevexpressGrid.GetFocusedRowCellValue(viewWork, "NOTE");


                if (!string.IsNullOrEmpty(vProcessKey) && !string.IsNullOrEmpty(vWorkDate) && !string.IsNullOrEmpty(vNote))
                {
                    xSearchWorkResult();
                    xSearchERPPreview();
                    xSearchERPUpload();
                }
            }
            catch (Exception)
            {

            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void viewIngred_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        /// <summary>
        /// ERP 전송
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnERPUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewWork.RowCount == 0)
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
                int[] selectedRows = viewWork.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewWork.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    if (dr["TRANS_YN"]?.ToString() == "Y")
                    {
                        ShowMessageBox.XtraShowWarning("전송 불가 제품입니다. 관리자에게 문의 하세요.");
                        return;
                    }

                    string pellet = string.Empty;

                    clsCommon.GetAutoPellet(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), out pellet);

                    SQL = $@"
                    UPDATE WORK_ORDER
                    SET   ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) 
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
                        , ERP_ISTATUS = CASE WHEN '{pellet}' IS NULL 
                                            THEN CASE TO_CHAR(ERP_ISTATUS) 
                                                    WHEN 'N' THEN 'F'
                                                    WHEN 'W' THEN 'F'
                                                    WHEN 'M' THEN 'U'
                                                    WHEN 'X' THEN 'D'
                                                    WHEN 'G' THEN 'F'
                                                    WHEN 'L' THEN 'U'
                                                    WHEN 'R' THEN 'D'
                                                    WHEN NULL THEN 'N'
                                                        ELSE TO_CHAR(ERP_ISTATUS) END
                                            ELSE 'P'
                                        END
                        , ERP_IERR_CNT = 0
                        , MIXINGYN = 'Y'
                    WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                            AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{dr["NUM"]}'
                            AND DEL_FLAG != 'Y' AND NVL(TRANS_YN, 'N') != 'Y'
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

        /// <summary>
        /// ERP 전송 취소 요청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnERPDelete_Click(object sender, EventArgs e)
        {
            //ShowMessageBox.XtraShowInformation("기능 추가 중입니다.");
            //return;
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (viewWork.RowCount == 0)
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
                int[] selectedRows = viewWork.GetSelectedRows();

                if (selectedRows.Length == 0)
                {
                    ShowMessageBox.XtraShowWarning("전송할 정보를 선택 해주세요.");
                    return;
                }

                foreach (int rowHandle in selectedRows)
                {
                    var dr = viewWork.GetDataRow(rowHandle);

                    dr.ClearErrors();

                    string pellet = string.Empty;

                    clsCommon.GetAutoPellet(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["RESOURCE_NO"]?.ToString(), out pellet);

                    SQL = $@"
                    SELECT 1
                    FROM WORK_ORDER
                     WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{dr["NUM"]}'
                        AND (ERP_OSTATUS IN ('Y')
                        OR ERP_ISTATUS IN ('Y'))
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        UPDATE WORK_ORDER
                        SET   ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) 
                                                WHEN 'Y' THEN 'D'
                                            ELSE TO_CHAR(ERP_OSTATUS) END
                            , ERP_OERR_CNT = 0
                            , ERP_ISTATUS = CASE WHEN '{pellet}' IS NULL 
                                                THEN CASE TO_CHAR(ERP_ISTATUS) 
                                                        WHEN 'Y' THEN 'D'
                                                    ELSE TO_CHAR(ERP_ISTATUS) END
                                                ELSE TO_CHAR(ERP_ISTATUS)
                                            END
                            , ERP_IERR_CNT = 0
                            , C_CONDITION = '{clsCommon.GetPcStatusCode("취소")}'
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = TO_CHAR(TO_DATE('{dr["WORKDATE"]}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{dr["NUM"]}'
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

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
            }

            //// 신규 행 추가
            //if (e.KeyCode == Keys.F3)
            //{
            //    btn_rowAdd_Click(sender, e);
            //}

            //// 행 삭제
            //if (e.KeyCode == Keys.Delete)
            //{
            //    btn_rowDel_Click(sender, e);
            //}

            // 저장
            if (e.Control && e.KeyCode == Keys.S)
            {
                XMain_Save();
            }

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridWork.Focus();
            viewWork.FocusedRowHandle = 0;
            viewWork.FocusedColumn = viewWork.VisibleColumns[0];
        }

        #region ERP 전송 데이터 가공 이벤트

        private void btn_sap_reflash_Click(object sender, EventArgs e)
        {
            xSearchERPPreview();
        }

        private void btn_sap_rowAdd_Click(object sender, EventArgs e)
        {
            var targets = new[] { "Y", "C", "F" };

            for (int i = 0; i < viewWork.RowCount; i++)
            {
                if (targets.Contains(viewWork.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                 || targets.Contains(viewWork.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                {
                    return;
                }
            }

            clsDevexpressGrid.GridViewAddRow(viewPreview);

            viewPreview.SetRowCellValue(viewPreview.FocusedRowHandle, viewPreview.Columns["PLANT_CODE"], vPlantCode);
            viewPreview.SetRowCellValue(viewPreview.FocusedRowHandle, viewPreview.Columns["PROCESS_KEY"], "SAP_".Merge(vProcessKey));
            viewPreview.SetRowCellValue(viewPreview.FocusedRowHandle, viewPreview.Columns["L_CODE"], vLCode);
            viewPreview.SetRowCellValue(viewPreview.FocusedRowHandle, viewPreview.Columns["WORKDATE"], vWorkDate);
            viewPreview.SetRowCellValue(viewPreview.FocusedRowHandle, viewPreview.Columns["NUM"], vNum);
            viewPreview.SetRowCellValue(viewPreview.FocusedRowHandle, viewPreview.Columns["RESOURCE_NO"], vResourceNo);
        }

        private void btn_sap_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(viewPreview);
            clsDevexpressGrid.GridViewLastAddRowDelete(viewPreview);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var targets = new[] { "Y", "C", "F" };

            for (int i = 0; i < viewWork.RowCount; i++)
            {
                if (targets.Contains(viewWork.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                 || targets.Contains(viewWork.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                {
                    return;
                }
            }

            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("전송 데이터를 저장 하시겠습니까?"))
            {
                return;
            }

            XMain_Save();

            xSearchERPPreview();
            xSearchERPUpload();
        }

        private void XMain_Save()
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(viewPreview);
                DataTable DT = (DataTable)gridPreview.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("변경된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                splashScreenManager.ShowWaitForm();

                foreach (DataRow dr in DT.Rows)
                {
                    string rValid = clsCommon.ValdationCheck(sValid, dr, viewPreview);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        viewPreview.FocusedColumn = viewPreview.Columns[rValid]; // 이동할 컬럼명
                        viewPreview.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        UPDATE WORK_ORDER
                        SET    ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) -- WHEN 'Y' THEN 'M' 
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
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"].ToString().Replace("SAP_", "")}' AND L_CODE = '{dr["L_CODE"]}'
                                AND WORKDATE = '{dr["WORKDATE"]}' AND NUM = '{dr["NUM"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(WORK_REMARK)데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        INSERT INTO WORK_REMARK
                        (   PLANT_CODE      -- 01
                          , PROCESS_KEY     -- 02
                          , L_CODE          -- 03
                          , WORKDATE        -- 04
                          , NUM             -- 05
                          , BATCH           -- 06
                          , SEQ             -- 07
                          , IO_GUBUN        -- 08
                          , LOCATION        -- 09
                          , P_Q             -- 10
                          , P_Q_TIME        -- 11
                          , IO_DATE         -- 12
                          , INGRED_LOT      -- 13
                          , RESOURCE_NO     -- 14
                          , NAME            -- 15
                          , P_TYPE          -- 16
                          , SEND_YN         -- 17
                          , R_YN            -- 18
                          , C_CONDITION     -- 19
                          , I_TIME          -- 20
                        )
                        VALUES
                        (   '{dr["PLANT_CODE"]}'      -- 01
                          , '{dr["PROCESS_KEY"]}'     -- 02
                          , '{dr["L_CODE"]}'          -- 03
                          , '{dr["WORKDATE"]}'        -- 04
                          , '{dr["NUM"]}'             -- 05
                          , '{dr["BATCH"]}'           -- 06
                          , '{dr["SEQ"]}'             -- 07
                          , 'I'        -- 08
                          , 'H'        -- 09
                          , '{dr["P_Q"]}'             -- 10
                          , 0                   -- 11
                          , SYSDATE                   -- 12
                          , '{dr["INGRED_CODE"]}'      -- 13
                          , '{dr["RESOURCE_NO"]}'     -- 14
                          , '{clsCommon.UserId}'      -- 15
                          , '2'                       -- 16
                          , 'Y'                       -- 17
                          , 'N'                       -- 18
                          , null                      -- 19
                          , SYSDATE                   -- 20
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("(WORK_REMARK)데이터 입력에 실패했습니다");
                            return;
                        }
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE WORK_REMARK
                        SET    P_Q         = '{dr["P_Q"]}',
                               I_TIME      = SYSDATE
                        WHERE  PLANT_CODE  = '{dr["PLANT_CODE"]}'
                            AND    PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                            AND    L_CODE      = '{dr["L_CODE"]}'
                            AND    WORKDATE    = '{dr["WORKDATE"]}'
                            AND    NUM         = '{dr["NUM"]}'
                            AND    BATCH       = '{dr["BATCH"]}'
                            AND    SEQ         = '{dr["SEQ"]}'
                            AND    INGRED_LOT = '{dr["INGRED_CODE"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    SQL = $@"
                    UPDATE WORK_ORDER
                    SET    ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) -- WHEN 'Y' THEN 'M' 
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
                    WHERE PLANT_CODE = '{viewPreview.GetFocusedRowCellValue("PLANT_CODE")}'
                        AND PROCESS_KEY = '{viewPreview.GetFocusedRowCellValue("PROCESS_KEY").ToString().Replace("SAP_", "")}'
                        AND L_CODE = '{viewPreview.GetFocusedRowCellValue("L_CODE")}'
                        AND WORKDATE = '{viewPreview.GetFocusedRowCellValue("WORKDATE")}'
                        AND NUM = '{viewPreview.GetFocusedRowCellValue("NUM")}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        clsLog.logSave(this.Text, "btn_save_Click", SQL);
                        ShowMessageBox.XtraShowWarning("(WORK_REMARK)데이터 입력에 실패했습니다");
                        return;
                    }

                    dr.AcceptChanges();
                    viewPreview.RefreshData();
                }

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void btn_sap_delete_Click(object sender, EventArgs e)
        {
            var targets = new[] { "Y", "C", "F" };

            for (int i = 0; i < viewWork.RowCount; i++)
            {
                if (targets.Contains(viewWork.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                 || targets.Contains(viewWork.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                {
                    return;
                }
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("전송 데이터를 삭제 하시겠습니까?"))
            {
                return;
            }

            try
            {
                splashScreenManager.ShowWaitForm();

                SQL = $@"
                UPDATE WORK_ORDER
                SET    ERP_OSTATUS = CASE TO_CHAR(ERP_OSTATUS) -- WHEN 'Y' THEN 'M' 
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
                WHERE PLANT_CODE = '{viewPreview.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND PROCESS_KEY = '{viewPreview.GetFocusedRowCellValue("PROCESS_KEY").ToString().Replace("SAP_", "")}'
                    AND L_CODE = '{viewPreview.GetFocusedRowCellValue("L_CODE")}'
                    AND WORKDATE = '{viewPreview.GetFocusedRowCellValue("WORKDATE")}'
                    AND NUM = '{viewPreview.GetFocusedRowCellValue("NUM")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    clsLog.logSave(this.Text, "btn_save_Click", SQL);
                    ShowMessageBox.XtraShowWarning("(WORK_REMARK)데이터 입력에 실패했습니다");
                    return;
                }

                SQL = $@"
                DELETE FROM WORK_REMARK
                WHERE      PLANT_CODE  = '{viewPreview.GetFocusedRowCellValue("PLANT_CODE")}'
                    AND    PROCESS_KEY = '{viewPreview.GetFocusedRowCellValue("PROCESS_KEY")}'
                    AND    L_CODE      = '{viewPreview.GetFocusedRowCellValue("L_CODE")}'
                    AND    WORKDATE    = '{viewPreview.GetFocusedRowCellValue("WORKDATE")}'
                    AND    NUM         = '{viewPreview.GetFocusedRowCellValue("NUM")}'
                    AND    BATCH       = '{viewPreview.GetFocusedRowCellValue("BATCH")}'
                    AND    SEQ         = '{viewPreview.GetFocusedRowCellValue("SEQ")}'
                    AND    INGRED_LOT  = '{viewPreview.GetFocusedRowCellValue("INGRED_CODE")}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_sap_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                ShowMessageBox.XtraShowInformation("선택 항목을 삭제 했습니다.");

                xSearchERPPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_sap_delete_Click()", ex);
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

        #endregion

        private void txtBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                xSearchWorkResult();
                xSearchERPPreview();
                e.Handled = true;
                e.SuppressKeyPress = true; // 삑 소리 방지
            }
        }

        private void viewIngred_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (viewIngred.RowCount == 0)
            {
                return;
            }

            string sBatch = viewIngred.GetFocusedRowCellValue("BATCH").ToString();
            string sSeq = viewIngred.GetFocusedRowCellValue("SEQ").ToString();
            string sIngred = viewIngred.GetFocusedRowCellValue("INGRED_CODE").ToString();

            for (int i = 0; i < viewPreview.RowCount; i++)
            {
                var batch = viewPreview.GetRowCellValue(i, "BATCH")?.ToString();
                var seq = viewPreview.GetRowCellValue(i, "SEQ")?.ToString();
                var ingred = viewPreview.GetRowCellValue(i, "INGRED_CODE")?.ToString();

                if (batch == sBatch && seq == sSeq && ingred == sIngred)
                {
                    viewPreview.FocusedRowHandle = i;
                    break;
                }
            }

            int rowHandle = viewUpload.LocateByValue(0, viewUpload.Columns["INGRED_CODE"], sIngred);

            viewUpload.FocusedRowHandle = rowHandle;
        }

        private void viewPreview_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (viewPreview.RowCount == 0)
            {
                return;
            }

            string sBatch = viewPreview.GetFocusedRowCellValue("BATCH").ToString();
            //string sSeq = viewPreview.GetFocusedRowCellValue("SEQ").ToString();
            string sIngred = viewPreview.GetFocusedRowCellValue("INGRED_CODE").ToString();

            for (int i = 0; i < viewIngred.RowCount; i++)
            {
                var batch = viewIngred.GetRowCellValue(i, "BATCH")?.ToString();
                //var seq = viewIngred.GetRowCellValue(i, "SEQ")?.ToString();
                var ingred = viewIngred.GetRowCellValue(i, "INGRED_CODE")?.ToString();

                if (batch == sBatch && ingred == sIngred)
                {
                    viewIngred.FocusedRowHandle = i;
                    break;
                }
            }

            int rowHandle = viewUpload.LocateByValue(0, viewUpload.Columns["INGRED_CODE"], sIngred);

            viewUpload.FocusedRowHandle = rowHandle;
        }

        private void viewUpload_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (viewUpload.RowCount == 0)
            {
                return;
            }

            string sIngred = viewUpload.GetFocusedRowCellValue("INGRED_CODE")?.ToString();

            int rowHandle = viewPreview.LocateByValue(0, viewPreview.Columns["INGRED_CODE"], sIngred);

            viewPreview.FocusedRowHandle = rowHandle;

            rowHandle = viewIngred.LocateByValue(0, viewIngred.Columns["INGRED_CODE"], sIngred);

            viewIngred.FocusedRowHandle = rowHandle;
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
                return;

            try
            {
                gridWork.DataSource = null;
                gridIngred.DataSource = null;
                gridPreview.DataSource = null;
                gridUpload.DataSource = null;

                // 공정
                clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessTypeCode("배합")), "", false, 0, false);
                var plant = cboPlant_Code.EditValue?.ToString();

                var processKey = clsCommon.GetProcessKey("배합", plant);

                if (processKey != "@")
                    cboProcessKey.EditValue = processKey;

                //라인
                //clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void cboProcessKey_EditValueChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
                return;

            try
            {
                gridWork.DataSource = null;
                gridIngred.DataSource = null;
                gridPreview.DataSource = null;
                gridUpload.DataSource = null;

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
                cboL_Code.EditValue = clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()).Rows[0][0]?.ToString();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
                return;

            gridWork.DataSource = null;
            gridIngred.DataSource = null;
            gridPreview.DataSource = null;
            gridUpload.DataSource = null;

            /*XMain_Search()*/
            ;
        }

        private void cboERPUpLoad_EditValueChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
                return;

            XMain_Search();
        }

        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
                return;

            XMain_Search();
        }

        private void viewPreview_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //switch (viewPreview.FocusedColumn.FieldName)
            //{
            //    case "INGRED_CODE":
            //    case "BATCH":
            //    case "SEQ":
            //        e.Cancel = true;
            //        break;

            //    default:
            //        e.Cancel = false;
            //        break;
            //}

            var targets = new[] { "Y", "C", "F" };

            for (int i = 0; i < viewWork.RowCount; i++)
            {
                if (targets.Contains(viewWork.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString())
                 || targets.Contains(viewWork.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString()))
                {
                    e.Cancel = true;        // 수정 불가                    
                }
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

                if (viewWork.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("복사 할 작업을 선택하여 주세요");
                    return;
                }

                if (viewWork.GetFocusedRowCellValue("ERP_ISTATUS")?.ToString() != "C" && viewWork.GetFocusedRowCellValue("ERP_OSTATUS")?.ToString() != "C")
                {
                    ShowMessageBox.XtraShowWarning("[취소 완료] 상태만 복사 할수 있습니다.");
                    return;
                }

                string scPlantCode = viewWork.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                string scProcesskey = viewWork.GetFocusedRowCellValue("PROCESS_KEY")?.ToString();
                string scLCode = viewWork.GetFocusedRowCellValue("L_CODE")?.ToString();
                string scBWorkDate = viewWork.GetFocusedRowCellValue("WORKDATE").ToString().Replace("-", "");
                string scNum = viewWork.GetFocusedRowCellValue("NUM")?.ToString();

                m_WorkDateCopy child = new m_WorkDateCopy(scPlantCode, scProcesskey, scLCode, scBWorkDate, scNum);

                child.StartPosition = FormStartPosition.CenterParent;
                if (child.ShowDialog() == DialogResult.OK)
                {
                    string sAWorkDate = child.SelectedWorkDate;


                    string sCurrentSeq = string.Empty;

                    string SQL = $@"
                    SELECT NVL(MAX(NUM) + 1, 1) AS SEQ
                    FROM WORK_ORDER
                    WHERE PLANT_CODE = '{scPlantCode}'
                        AND PROCESS_KEY = '{scProcesskey}'
                        AND L_CODE = '{scLCode}'
                        AND WORKDATE = '{sAWorkDate}'
                    ";

                    using (DataSet Ds = Dbconn.conn.ExecutDataset(SQL))
                    {
                        if (Dbconn.conn.getRowCnt(Ds) == 0)
                        {
                            clsLog.logSave("작업순번 생성에러 / SQL : " + SQL, 0);
                            return;
                        }

                        sCurrentSeq = Dbconn.conn.getData(Ds, "SEQ", 0);
                        Ds.Dispose();
                    }

                    Dbconn.conn.BeginTransaction();

                    SQL = $@"
                    INSERT INTO WORK_ORDER
                        (PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM
                        , P_TYPE, RESOURCE_NO, NOTE, WORK_START_DATE, BATCH
                        , R_BATCH, BATCH_Q, OR_Q, PRO_Q, BBATCH_Q
                        , GUBUN, LOCATION_ED, LOCATION_ED2, REMARK, ICM_CODE
                        , C_CONDITION, HALT_TIME, START_TIME, END_TIME, BU_YN
                        , BAD_CODE1, BAD_QTY1, BAD_CODE2, BAD_QTY2, BAD_CODE3
                        , BAD_QTY3, BAD_CODE4, BAD_QTY4, BAD_CODE5, BAD_QTY5
                        , ERP_ISTATUS, ERP_ITNUMBER, ERP_OSTATUS, ERP_OTNUMBER, DEL_FLAG
                        , I_TIME, EMPLOYEE_NO, ERP_IERR_CNT, ERP_OERR_CNT, PACK_TYPE
                        , ERR_MSG, NUM_COPY)
                    SELECT 
                        PLANT_CODE, PROCESS_KEY, L_CODE, '{sAWorkDate}', '{sCurrentSeq}'
                        , P_TYPE, RESOURCE_NO, NOTE, WORK_START_DATE, BATCH
                        , R_BATCH, BATCH_Q, OR_Q, PRO_Q, BBATCH_Q
                        , GUBUN, LOCATION_ED, LOCATION_ED2, '※{scBWorkDate} {scNum} 작업복사(복사 내용 수정/삭제 금지)', ICM_CODE
                        , '{clsCommon.PcStatus.Completed}', HALT_TIME, START_TIME, END_TIME, BU_YN
                        , BAD_CODE1, BAD_QTY1, BAD_CODE2, BAD_QTY2, BAD_CODE3
                        , BAD_QTY3, BAD_CODE4, BAD_QTY4, BAD_CODE5, BAD_QTY5
                        , 'W', NULL, 'W', NULL, 'N'
                        , SYSDATE, '{clsCommon.UserId}', 0, 0, PACK_TYPE
                        , NULL, NUM
                    FROM WORK_ORDER
                    WHERE PLANT_CODE = '{scPlantCode}'
                        AND PROCESS_KEY = '{scProcesskey}'
                        AND L_CODE = '{scLCode}'
                        AND WORKDATE = '{scBWorkDate}'
                        AND NUM = '{scNum}'
                        AND NVL(DEL_FLAG, 'N') != 'Y' 
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btnWorkCopy_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 복제에 실패했습니다");
                        return;
                    }

                    SQL = $@"
                    INSERT INTO WORK_DETAIL(
                            PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM
                        , INGRED_CODE, SCALE_CODE, LOCATION, QTY_PCT, SET_VAL
                        , RC_YN, BIN_TYPE, HR_ERR, SEQ, I_TIME
                        , EWREW, BIN_SEQ, PARENT_BIN
                    )
                    SELECT PLANT_CODE, PROCESS_KEY, L_CODE, '{sAWorkDate}', '{sCurrentSeq}'
                            , INGRED_CODE, SCALE_CODE, LOCATION, QTY_PCT, SET_VAL
                            , RC_YN, BIN_TYPE, HR_ERR, SEQ, SYSDATE
                            , EWREW, BIN_SEQ, PARENT_BIN
                    FROM WORK_DETAIL
                    WHERE PLANT_CODE = '{scPlantCode}'
                        AND PROCESS_KEY LIKE '%{scProcesskey}'
                        AND L_CODE = '{scLCode}'
                        AND WORKDATE = '{scBWorkDate}'
                        AND NUM = '{scNum}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btnWorkCopy_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 복제에 실패했습니다");
                        return;
                    }

                    SQL = $@"
                    INSERT INTO WORK_REMARK(
                          PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM
                        , BATCH, SEQ, IO_GUBUN, LOCATION, P_Q
                        , P_Q_TIME, IO_DATE, INGRED_LOT, RESOURCE_NO, NAME
                        , P_TYPE, SEND_YN, R_YN, C_CONDITION, I_TIME
                        , ERP_UP_YN
                    )
                    SELECT PLANT_CODE, PROCESS_KEY, L_CODE, '{sAWorkDate}', '{sCurrentSeq}'
                         , BATCH, SEQ, IO_GUBUN, LOCATION, P_Q
                         , P_Q_TIME, IO_DATE, INGRED_LOT, RESOURCE_NO, NAME
                         , P_TYPE, SEND_YN, R_YN, C_CONDITION, SYSDATE
                         , ERP_UP_YN
                    FROM WORK_REMARK
                    WHERE PLANT_CODE = '{scPlantCode}'
                        AND PROCESS_KEY LIKE '%{scProcesskey}'
                        AND L_CODE = '{scLCode}'
                        AND WORKDATE = '{scBWorkDate}'
                        AND NUM = '{scNum}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btnWorkCopy_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 복제에 실패했습니다");
                        return;
                    }

                    Dbconn.conn.Commit();

                    ShowMessageBox.XtraShowInformation("작업 복사가 완료 되었습니다.");

                    XMain_Search();
                }
            }
            catch
            {

            }
        }

        private void viewWork_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "ERP_Q")
            {
                double orQ = 0;
                double erpQ = 0;
                double resultQ = 0;

                double.TryParse(Convert.ToString(viewWork.GetRowCellValue(e.RowHandle, "OR_Q")), out orQ);
                double.TryParse(Convert.ToString(viewWork.GetRowCellValue(e.RowHandle, "ERP_Q")), out erpQ);

                resultQ = orQ - erpQ;

                if (orQ != erpQ)
                {
                    e.Appearance.BackColor = Color.CadetBlue;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

            if (e.Column.FieldName == "ERP_ISTATUS")
            {
                string iStatus = Convert.ToString(viewWork.GetRowCellValue(e.RowHandle, "ERP_ISTATUS"));

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
                string oStatus = Convert.ToString(viewWork.GetRowCellValue(e.RowHandle, "ERP_OSTATUS"));

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
                else if (oStatus == "F" || oStatus == "D" || oStatus == "U")
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
        }

        private void viewWork_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewWork.FocusedColumn.FieldName != "TRANS_YN")      // 031003	진행
            {
                e.Cancel = true;
            }
        }

        private void viewPreview_RowStyle(object sender, RowStyleEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (e.RowHandle < 0)
                return;

            object val = view.GetRowCellValue(e.RowHandle, "P_Q");
            if (val == null || val == DBNull.Value)
                return;

            decimal p_q;
            if (!decimal.TryParse(val.ToString(), out p_q))
                return;

            if (p_q <= 0)
            {
                e.Appearance.BackColor = Color.Red;
                e.Appearance.ForeColor = Color.White;
                e.HighPriority = true;   // ⭐ 중요 (다른 스타일보다 우선)
            }
        }

        private void viewWork_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            //string fromDate = string.Empty;
            //string toDate = string.Empty;

            //double sumText = 0;

            //fromDate = dateEdit_workDate.EditValue == null
            //            ? string.Empty
            //            : Convert.ToDateTime(dateEdit_workDate.EditValue).ToString("yyyyMMdd");
            //toDate = dateEdit_workDateEd.EditValue == null
            //            ? string.Empty
            //            : Convert.ToDateTime(dateEdit_workDateEd.EditValue).ToString("yyyyMMdd");

            //string SQL = $@"
            //SELECT SUM(a.BATCH) AS BATCH
            //    , SUM((SELECT SUM(CASE WHEN wo.BU_YN = 'Y' THEN wo.BATCH * wd.SET_VAL ELSE 0 END)
            //                FROM WORK_ORDER wo
            //                JOIN WORK_DETAIL wd 
            //                ON wd.PLANT_CODE = wo.PLANT_CODE
            //                AND wd.PROCESS_KEY = wo.PROCESS_KEY
            //                AND wd.L_CODE = wo.L_CODE
            //                AND wd.WORKDATE = wo.WORKDATE
            //                AND wd.NUM = wo.NUM
            //                AND wd.INGRED_CODE IN (
            //                    SELECT RESOURCE_NO_2 
            //                        FROM SAP_IN_PRODUCT_RC 
            //                        WHERE PLANT_CODE = wo.PLANT_CODE 
            //                        AND RESOURCE_NO = wo.RESOURCE_NO
            //                )
            //                WHERE wo.PLANT_CODE = a.PLANT_CODE
            //                AND wo.PROCESS_KEY = a.PROCESS_KEY
            //                AND wo.L_CODE = a.L_CODE
            //                AND wo.WORKDATE = a.WORKDATE
            //                AND wo.NUM = a.NUM
            //            )) AS BBATCH_Q
            //    , SUM(a.R_BATCH) AS R_BATCH
            //    , SUM(a.BATCH_Q) AS BATCH_Q
            //    , SUM(a.OR_Q) AS OR_Q
            //    , SUM(a.PRO_Q) AS PRO_Q
            //    , SUM((SELECT SUM(wr.P_Q) 
            //        FROM WORK_REMARK wr
            //        WHERE wr.PLANT_CODE = a.PLANT_CODE
            //        AND wr.PROCESS_KEY = 'SAP_' || a.PROCESS_KEY
            //        AND wr.L_CODE = a.L_CODE
            //        AND wr.WORKDATE = a.WORKDATE
            //        AND wr.NUM = a.NUM
            //    )) AS ERP_Q
            //    , ROUND(
            //            (SUM((SELECT SUM(wr.P_Q) 
            //                FROM WORK_REMARK wr
            //                WHERE wr.PLANT_CODE = a.PLANT_CODE
            //                AND wr.PROCESS_KEY = 'SAP_' || a.PROCESS_KEY
            //                AND wr.L_CODE = a.L_CODE
            //                AND wr.WORKDATE = a.WORKDATE
            //                AND wr.NUM = a.NUM
            //            )  / 1000) / SUM(NULLIF((a.END_TIME - a.START_TIME) * 24, 0)))
            //        , 1
            //        ) AS ERP_Q_PER_HOUR
            //FROM WORK_ORDER a
            //WHERE a.PLANT_CODE = '{vPlantCode}'
            //    AND a.PROCESS_KEY = '{vProcessKey}'
            //    AND ('{vLCode}' IS NULL OR a.L_CODE = '{vLCode}')
            //    AND a.WORKDATE BETWEEN '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}' 
            //                    AND '{string.Format("{0:yyyyMMdd}", dateEdit_workDateEd.EditValue)}'
            //    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG, 'N') = '{cboDelYn.EditValue}')
            //    AND a.ERP_OSTATUS = 'Y'
            //";

            //DataSet proDs = Dbconn.conn.ExecutDataset(SQL);

            //if (e.Column.FieldName == "BATCH")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "BATCH", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "BATCH", 0));
            //    }

            //    e.Info.DisplayText = $"{String.Format("{0:N3}", sumText)}";
            //}

            //if (e.Column.FieldName == "BBATCH_Q")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "BBATCH_Q", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "BBATCH_Q", 0));
            //    }

            //    e.Info.DisplayText = $"{String.Format("{0:N0}", sumText)}";
            //}

            //if (e.Column.FieldName == "R_BATCH")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "R_BATCH", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "R_BATCH", 0));
            //    }

            //    e.Info.DisplayText = $"{String.Format("{0:N0}", sumText)}";
            //}

            //if (e.Column.FieldName == "BATCH_Q")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "BATCH_Q", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "BATCH_Q", 0));
            //    }

            //    e.Info.DisplayText = $"{String.Format("{0:N3}", sumText)}";
            //}

            //if (e.Column.FieldName == "OR_Q")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "OR_Q", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "OR_Q", 0));
            //    }

            //    e.Info.DisplayText = $"{String.Format("{0:N3}", sumText)}";
            //}

            //if (e.Column.FieldName == "PRO_Q")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "PRO_Q", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "PRO_Q", 0));
            //    }

            //    e.Info.DisplayText = $"{String.Format("{0:N3}", sumText)}";
            //}

            //if (e.Column.FieldName == "ERP_Q")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "ERP_Q", 0) != "")
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "ERP_Q", 0));
            //    }

            //    e.Info.DisplayText = $"{String.Format("{0:N3}", sumText)}";
            //}

            //if (e.Column.FieldName == "ERP_Q_PER_HOUR")
            //{
            //    if (Dbconn.conn.getRowCnt(proDs) > 0 && Dbconn.conn.getData(proDs, "ERP_Q_PER_HOUR", 0) != "" && Dbconn.conn.getData(proDs, "ERP_Q_PER_HOUR", 0) != null)
            //    {
            //        sumText = Convert.ToDouble(Dbconn.conn.getData(proDs, "ERP_Q_PER_HOUR", 0));
            //    }

            //    e.Info.DisplayText = $"AVG :{String.Format("{0:N1}", sumText)}";
            //}
        }

        private decimal sumQty = 0;
        private decimal sumHour = 0;

        private void viewWork_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            var item = e.Item as DevExpress.XtraGrid.GridSummaryItem;

            if (item == null || item.FieldName != "ERP_Q_PER_HOUR")
                return;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                sumQty = 0;
                sumHour = 0;
            }

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {
                int rowHandle = e.RowHandle;

                decimal qty = Convert.ToDecimal(view.GetRowCellValue(rowHandle, "ERP_Q") ?? 0);
                decimal hour = Convert.ToDecimal(view.GetRowCellValue(rowHandle, "WORK_HOUR") ?? 0);

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
