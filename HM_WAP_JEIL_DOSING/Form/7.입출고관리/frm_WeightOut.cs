using Core.Class;
using Core.Enum;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using System;
using System.Collections;
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
    public partial class frm_WeightOut : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_WeightOut()
        {
            InitializeComponent();

            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(gridView_Detail, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(gridView_Pallet, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                /*                
                    SQL =
                    "SELECT a.IS_NO, a.CUST_CODE, a.CUST_CODE as CUST_NM,  a.INCAR_NO, a.CHKIN_TIME,   CEILING(a.IN_WEIGHT) as IN_WEIGHT,   " +
                    "CEILING(a.OUT_WEIGHT) as OUT_WEIGHT, a.CAR_TDETAIL, a.D_NAME, CONVERT(VARCHAR(12), a.INCAR_DATE, 20) as INCAR_DATE,   " +
                    "a.INCAR_TIME, CONVERT(VARCHAR(12), a.OUTCAR_DATE, 20) as OUTCAR_DATE , a.OUTCAR_TIME, ISNULL(b.SUM_PAADD, 0) AS SUM_PAADD,  " +
                    "CEILING(a.OUT_WEIGHT - a.IN_WEIGHT) AS WEIGHT, CEILING(a.OUT_WEIGHT - a.IN_WEIGHT - ISNULL(b.SUM_PAADD, 0)) AS REAL_WEIGHT,  " +
                    "a.D_HP, c.BULK_WEIGHT, a.PC_STATUS   " +
                    "FROM WAP_DECAR DECAR LEFT OUTER JOIN (  " +
                    "SELECT IN_ADD.IS_NO, SUM(PA_M.WEIGHT * IN_ADD.PD_QTY) as SUM_PAADD   " +
                    "FROM WAP_IN_ADD IN_ADD LEFT OUTER JOIN WAP_PA_MASTER PA_M  " +
                    "ON IN_ADD.PTMCD = PA_M.PTMCD   " +
                    "GROUP BY IN_ADD.IS_NO  " +
                    ") PAADD ON a.IS_NO = b.IS_NO  " +
                    "LEFT OUTER JOIN (  " +
                    "SELECT IS_NO, ISNULL(SUM(P_Q),0) AS BULK_WEIGHT FROM BULK_ORDER WHERE WORK_START_DATE BETWEEN '{0}' AND '{1}' " +
                    "GROUP BY IS_NO  " +
                    ") BULK_WORK  " +
                    "ON a.IS_NO = c.IS_NO  " +
                    "WHERE CONVERT(CHAR(8), CHKIN_TIME, 112) BETWEEN '{0}' AND '{1}' AND CAR_TDETAIL IN ('벌크','카고')     " +
                    "ORDER BY IS_NO   ";
                */


                ArrayList exRowList = new ArrayList();

                for (int i=0; i < gridView.RowCount; i++)
                {
                    if (gridView.GetMasterRowExpanded(i))
                    {
                        exRowList.Add(i);
                    }
                }

                var topRowIndex = gridView.TopRowIndex;
                var focusedRowHandle = gridView.FocusedRowHandle;

                SQL = $@"
                SELECT 
                    a.IS_NO,
                    a.CUST_CODE,
                    a.CUST_CODE AS CUST_NM,
                    a.INCAR_NO,
                    a.CHKIN_TIME,
                    CEIL(a.IN_WEIGHT) AS IN_WEIGHT,
                    CEIL(a.OUT_WEIGHT) AS OUT_WEIGHT,
                    a.CAR_TDETAIL,
                    a.D_NAME,
                    TO_CHAR(a.INCAR_DATE, 'YYYY-MM-DD') AS INCAR_DATE,
                    a.INCAR_TIME,
                    TO_CHAR(a.OUTCAR_DATE, 'YYYY-MM-DD') AS OUTCAR_DATE,
                    a.OUTCAR_TIME,
                    NVL(b.SUM_PAADD, 0) AS SUM_PAADD,
                    CEIL(a.OUT_WEIGHT - a.IN_WEIGHT) AS WEIGHT,
                    CEIL(a.OUT_WEIGHT - a.IN_WEIGHT - NVL(b.SUM_PAADD, 0) - NVL(d.P_WEIGHT, 0)) AS REAL_WEIGHT,
                    c.BULK_WEIGHT,
                    a.PC_STATUS,
                    NVL(d.P_WEIGHT, 0) AS P_WEIGHT,
                    CEIL(a.OUT_WEIGHT - a.IN_WEIGHT - NVL(b.SUM_PAADD, 0) - NVL(d.P_WEIGHT, 0)) - c.BULK_WEIGHT AS CHA_WEIGHT
                FROM 
                    WAP_DECAR a
                LEFT OUTER JOIN (
                    SELECT 
                        IN_ADD.IS_NO,
                        SUM(PA_M.WEIGHT * IN_ADD.PD_QTY) AS SUM_PAADD
                    FROM 
                        WAP_IN_ADD IN_ADD
                    LEFT OUTER JOIN 
                        WAP_PA_MASTER PA_M ON IN_ADD.PTMCD = PA_M.PTMCD
                    GROUP BY 
                        IN_ADD.IS_NO
                ) b ON a.IS_NO = b.IS_NO
                LEFT OUTER JOIN (
                    SELECT 
                        IS_NO,
                        NVL(SUM(P_Q), 0) AS BULK_WEIGHT
                    FROM 
                        BULK_ORDER
                    WHERE 
                        WORK_START_DATE BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                    GROUP BY 
                        IS_NO
                ) c ON a.IS_NO = c.IS_NO
                LEFT OUTER JOIN (
                    SELECT 
                        IS_NO,
                        SUM(a.P_WEIGHT) AS P_WEIGHT
                    FROM (
                        SELECT 
                            IS_NO,
                            CASE 
                                WHEN SUBSTR(RESOURCE_NO, -1) = '2' THEN 3 * (QUANTITY / 1000)
                                WHEN SUBSTR(RESOURCE_NO, -1) = '1' THEN 0.23 * QUANTITY
                                ELSE 0
                            END AS P_WEIGHT
                        FROM 
                            BULK_ORDER
                        WHERE 
                            IS_NO IS NOT NULL
                            AND C_CONDITION = '031004'
                            AND CAR_FULL_NUM IN (
                                SELECT VEHICLENO 
                                FROM TMS_INPUT_CARMASTER_CON 
                                WHERE VEHICLEGROUPCODE = '카고'
                            )
                    ) a
                    GROUP BY IS_NO
                ) d ON a.IS_NO = d.IS_NO
                WHERE 
                    TO_CHAR(CHKIN_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                    AND CAR_TDETAIL IN ('벌크','카고')
                ORDER BY 
                    IS_NO
                ";

                DataSet ds1 = Dbconn.conn.ExecutDataset(SQL, "in_table");

                SQL = $@"
                SELECT 
                    IS_NO,
                    WO_NUMBER,
                    WORK_SEQ,
                    ORDERNO,
                    ORDERLINENO,
                    CUST_NAME,
                    CAR_FULL_NUM,
                    RESOURCE_NO,
                    WORK_START_DATE,
                    BATCH,
                    BATCH_Q,
                    NVL(P_Q, 0) AS P_Q,
                    LOCATION,
                    LOCATION AS SCALE,
                    AUTO_YN,
                    START_TIME,
                    END_TIME,
                    QUANTITY,
                    C_CONDITION,
                    REMARK,
                    I_TIME,
                    PC_STATUS,
                    BEFORE_WEIGHT,
                    BEFORE_WEIGHT_TIME,
                    WEIGHT,
                    WEIGHT - BEFORE_WEIGHT AS REAL_WEIGHT,
                    CASE 
                        WHEN PROCESS_KEY = 'P06' AND C_CONDITION IN ('{clsCommon.GetPcStatusCode("진행")}','{clsCommon.PcStatus.Completed}')
                        THEN NVL(P_Q,0) - (NVL(WEIGHT,0) - NVL(BEFORE_WEIGHT,0)) 
                        ELSE 0 
                    END AS SANG_REAL_WEIGHT,
                    WEIGHT_TIME,
                    CASE 
                        WHEN SUBSTR(RESOURCE_NO, -1) = '2' THEN 3 * (QUANTITY / 1000)
                        WHEN SUBSTR(RESOURCE_NO, -1) = '1' THEN 0.23 * QUANTITY
                        ELSE 0 
                    END AS P_WEIGHT
                FROM 
                    BULK_ORDER
                WHERE 
                    WORK_START_DATE BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                    AND IS_NO IN (
                        SELECT IS_NO 
                        FROM WAP_DECAR 
                        WHERE TO_CHAR(CHKIN_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                        AND CAR_TDETAIL IN ('벌크', '카고')
                    )
                ORDER BY 
                    C_CONDITION, LOCATION, ORDERNO, WO_NUMBER, WORK_SEQ
                ";

                DataSet ds2 = Dbconn.conn.ExecutDataset(SQL, "in_detail");

                ds1.Merge(ds2);

                GridLevelNode gridLevelNode1 = new GridLevelNode();
                gridView_Detail.OptionsDetail.EnableMasterViewMode = true;
                gridLevelNode1.RelationName = "상차내역상세";
                gridLevelNode1.LevelTemplate = gridView_Detail;
                this.gridControl.LevelTree.Nodes.Add(gridLevelNode1);
                ds1.Relations.Add("상차내역상세", ds1.Tables["in_table"].Columns["IS_NO"], ds1.Tables["in_detail"].Columns["IS_NO"]);


                SQL = $@"
                SELECT 
                    INADD.IS_NO, 
                    PA.PTMCDNM, 
                    PA.WEIGHT, 
                    INADD.PD_QTY, 
                    INADD.I_TIME
                FROM 
                    WAP_IN_ADD INADD
                    LEFT OUTER JOIN WAP_PA_MASTER PA ON INADD.PTMCD = PA.PTMCD
                WHERE 
                    INADD.IS_NO IN (
                        SELECT IS_NO
                        FROM BULK_ORDER
                        WHERE WORK_START_DATE BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                        AND IS_NO IN (
                            SELECT IS_NO 
                            FROM WAP_DECAR 
                            WHERE TO_CHAR(CHKIN_TIME, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                            AND CAR_TDETAIL IN ('벌크', '카고')
                        )
                        AND IS_NO IS NOT NULL
                    )
                ORDER BY 
                    INADD.IS_NO
                ";

                DataSet ds3 = Dbconn.conn.ExecutDataset(SQL, "pa_detail");

                ds1.Merge(ds3);

                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds1.Tables["in_table"], true, true);

                sValid = new string[] { "" };


                GridLevelNode gridLevelNode2 = new GridLevelNode();
                //gridView_Pallet.OptionsDetail.EnableMasterViewMode = true;
                gridLevelNode2.RelationName = "파레트상세";
                gridLevelNode2.LevelTemplate = gridView_Pallet;
                this.gridControl.LevelTree.Nodes.Add(gridLevelNode2);
                ds1.Relations.Add("파레트상세", ds1.Tables["in_table"].Columns["IS_NO"], ds1.Tables["pa_detail"].Columns["IS_NO"]);

                
                //제품
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("제품")}', '{clsCommon.GetResourceTypeCode("상품")}'"));


                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[] {
                    new DataColumn("CODE"),
                    new DataColumn("NAME"),
                });

                dt.Rows.Add("0", "입차등록");
                dt.Rows.Add("1", "입차완료");
                dt.Rows.Add("2", "출차완료");
                dt.Rows.Add("9", "강제완료");

                repItemLkUpEdit_PC_STATUS.NullValuePrompt = "";
                repItemLkUpEdit_PC_STATUS.NullText = "";
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_PC_STATUS, dt);

                //작업계획
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_C_CONDITION, "03", "10");

                foreach(var item in exRowList)
                {
                    gridView.SetMasterRowExpanded(Convert.ToInt16(item), true);
                }

                gridView.FocusedRowHandle = focusedRowHandle;
                gridView.TopRowIndex = topRowIndex;

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "weightIn_search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void frm_WeightOut_Load(object sender, EventArgs e)
        {

            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView_Detail);

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            dateEdit_workStDate.EditValue = DateTime.Today;
            dateEdit_workEdDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();

            if (clsCommon._strUserId == "AD0001" ||
                clsCommon._strUserId == "kfirst" ||
                clsCommon._strUserId == "N20210101" ||  //음영준
                clsCommon._strUserId == "N20191001" ||  //곽종면 차장
                clsCommon._strUserId == "N20211001") //김미정
            {
                layoutControlItem_weightReset.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_weightPlc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_weightInput.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem_weightVal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem_weightReset.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_weightPlc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_weightInput.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem_weightVal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {

            if (DialogResult.Yes != ShowMessageBox.Confirm("계근제품출고 정보를 수정하시겠습니까?"))
            {
                return;
            }

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

                foreach (DataRow dr in DT.DataSet.Tables["in_table"].Rows)
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

                    
                    if (dr.RowState == DataRowState.Modified)
                    {

                        string uIncarTIme = string.Empty;
                        string uOutcarTime = string.Empty;


                        if (!string.IsNullOrEmpty(dr["INCAR_TIME"].ToString()))
                        {

                            uIncarTIme = "'" + Convert.ToDateTime(dr["INCAR_TIME"]).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                        else
                        {
                            uIncarTIme = "NULL";
                        }


                        if (!string.IsNullOrEmpty(dr["OUTCAR_TIME"].ToString()))
                        {

                            uOutcarTime = "'" + Convert.ToDateTime(dr["OUTCAR_TIME"]).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                        else
                        {
                            uOutcarTime = "NULL";
                        }

                        SQL = $@"
                        UPDATE WAP_DECAR
                        SET    INCAR_NO         = '{dr["INCAR_NO"]}',
                               VEHICLEGROUPCODE = '{dr["VEHICLEGROUPCODE"]}',
                               WEIGHT_KG        = '{dr["WEIGHT_KG"]}',
                               IN_WEIGHT        = '{dr["IN_WEIGHT"]}',
                               OUT_WEIGHT       = '{dr["OUT_WEIGHT"]}',
                               TR_YN            = '{dr["TR_YN"]}',
                               TR_WEIGHT        = '{dr["TR_WEIGHT"]}',
                               USER_ID          = '{dr["USER_ID"]}',
                               INCAR_DATE       = '{uIncarTIme}',
                               OUTCAR_DATE      = '{uOutcarTime}',
                               PC_STATUS        = '{dr["PC_STATUS"]}',
                               ERP_UP_YN        = '{dr["ERP_UP_YN"]}',
                               ERP_TNUMBER      = '{dr["ERP_TNUMBER"]}',
                               DEL_FLAG         = '{dr["DEL_FLAG"]}',
                               TEM_TYPE         = '{dr["TEM_TYPE"]}',
                               I_TIME           = '{dr["I_TIME"]}',
                               I_USER           = '{clsCommon.UserId}'
                        WHERE  IS_NO            = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            //dr.RejectChanges();
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this, "btn_update_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();

                } //foreach



                //detail update
                clsDevexpressGrid.GridDetailEndEdit(gridView_Detail);

                foreach (DataRow dr in DT.DataSet.Tables["in_detail"].Rows)
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

                    if (dr.RowState == DataRowState.Modified)
                    {

                        SQL = "SELECT ISNULL(ERP_UP_YN,'N') AS ERP_UP_YN  FROM BULK_ORDER WHERE DELIVERY_NO = '{0}' AND ORDER_NO = '{1}' AND ORDER_LINE_SKEY = '{2}' ";
                        SQL = string.Format(SQL,
                            dr["DELIVERY_NO"].ToString(),
                            dr["ORDER_NO"].ToString(),
                            dr["ORDER_LINE_SKEY"].ToString()
                            );
                        DataSet erpUpChkDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(erpUpChkDs) > 0)
                        {
                            string erpUpChk = Dbconn.conn.getData(erpUpChkDs, "ERP_UP_YN", 0);

                            if (erpUpChk == "Y")
                            {
                                dr.RowError = "ERP에 업로드 처리된 주문지시입니다";

                            }else
                            {
                                SQL = $@"
                                UPDATE BULK_ORDER
                                SET    PLANT_CODE          = '{dr["PLANT_CODE"]}',
                                        PROCESS_KEY        = '{dr["PROCESS_KEY"]}',
                                        WO_NUMBER          = '{dr["WO_NUMBER"]}',
                                        WORK_SEQ           = '{dr["WORK_SEQ"]}',
                                        ORDERLINENO        = '{dr["ORDERLINENO"]}',
                                        DISPATCHNO         = '{dr["DISPATCHNO"]}',
                                        ORDERNO            = '{dr["ORDERNO"]}',
                                        WORK_START_DATE    = '{dr["WORK_START_DATE"]}',
                                        BATCH              = '{dr["BATCH"]}',
                                        R_BATCH            = '{dr["R_BATCH"]}',
                                        BATCH_Q            = '{dr["BATCH_Q"]}',
                                        P_Q                = '{dr["P_Q"]}',
                                        QUANTITY           = '{dr["QUANTITY"]}',
                                        RESOURCE_NO        = '{dr["RESOURCE_NO"]}',
                                        PART_NAME          = '{dr["PART_NAME"]}',
                                        CUST_NO            = '{dr["CUST_NO"]}',
                                        CUST_NAME          = '{dr["CUST_NAME"]}',
                                        C_CONDITION        = '{dr["C_CONDITION"]}',
                                        AUTO_YN            = '{dr["AUTO_YN"]}',
                                        CAR_NO_REAL        = '{dr["CAR_NO_REAL"]}',
                                        CAR_FULL_NUM       = '{dr["CAR_FULL_NUM"]}',
                                        START_TIME         = '{dr["START_TIME"]}',
                                        END_TIME           = '{dr["END_TIME"]}',
                                        IS_NO              = '{dr["IS_NO"]}',
                                        PC_STATUS          = '{dr["PC_STATUS"]}',
                                        ERP_LOCATION       = '{dr["ERP_LOCATION"]}',
                                        BEFORE_WEIGHT      = '{dr["BEFORE_WEIGHT"]}',
                                        BEFORE_WEIGHT_TIME = '{Convert.ToDateTime(dr["BEFORE_WEIGHT_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}',
                                        WEIGHT             = '{dr["WEIGHT"]}',
                                        WEIGHT_TIME        = '{Convert.ToDateTime(dr["WEIGHT_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}',
                                        REMARK             = '{dr["REMARK"]}',
                                        U_TIME             = SYSDATE,
                                        U_USER             = '{clsCommon.UserId}',
                                        EVENT_LOG          = '{dr["EVENT_LOG"]}',
                                        LOCATION           = '{dr["LOCATION"]}'
                                WHERE  PLANT_CODE          = '{dr["PLANT_CODE"]}'
                                AND    PROCESS_KEY         = '{dr["PROCESS_KEY"]}'
                                AND    WO_NUMBER           = '{dr["WO_NUMBER"]}'
                                AND    WORK_SEQ            = '{dr["WORK_SEQ"]}'
                                ";

                                if (Dbconn.conn.SQLrun(SQL) < 0)
                                {
                                    Dbconn.conn.Rollback();
                                    clsLog.logSave(this, "btn_update_Click", SQL);
                                    ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                                    return;
                                }
                            }


                            if (dr["BEFORE_WEIGHT"].ToString() != "" && dr["WEIGHT"].ToString() != "")
                            {
                                SQL = "UPDATE BULK_ORDER SET PC_STATUS = '2' WHERE DELIVERY_NO = '{0}' AND ORDER_NO = '{1}' AND ORDER_LINE_SKEY = '{2}' ";
                                SQL = string.Format(SQL,
                                       dr["DELIVERY_NO"].ToString(),
                                       dr["ORDER_NO"].ToString(),
                                       dr["ORDER_LINE_SKEY"].ToString()
                                       );

                                   Dbconn.conn.SQLrun(SQL);

                            }

                        }

                    }

                    dr.AcceptChanges();

                } //foreach


                Dbconn.conn.Commit();

                XMain_Search();

            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_update_Click", ex);
                clsLog.logSave(this, "btn_update_Click", SQL);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (clsCommon._strUserId == "AD0001" ||
                    clsCommon._strUserId == "kfirst" ||
                    clsCommon._strUserId == "N20210101" ||  //음영준
                    clsCommon._strUserId == "N20191001" ||  //곽종면 차장
                    clsCommon._strUserId == "N20211001") //김미정
                {

                }else
                {
                    if (gridView.GetFocusedRowCellValue("PC_STATUS").ToString().Trim().Equals("2"))
                    {
                        e.Cancel = true;

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
                string pc_status = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["PC_STATUS"]).ToString();
                string erp_up_yn = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["ERP_UP_YN"]).ToString();

                /*                if (erp_up_yn == "Y")
                                {
                                    e.Appearance.BackColor = Color.LightCyan;
                                    e.Appearance.ForeColor = Color.Black;
                                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                                }
                                else
                                {*/

                if (e.RowHandle != this.gridView.FocusedRowHandle || e.Column.AbsoluteIndex == this.gridView.FocusedColumn.AbsoluteIndex)
                {
                    if (pc_status == "2" || pc_status == "99")
                    {
                        e.Appearance.BackColor = Color.LightGray;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }

                }

                //}
            }
            catch (Exception ex)
            {

            }
        }

        private void checkEdit_reflashSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit_reflashSearch.Checked)
            {
                reflash_timer.Interval = 5000;
                reflash_timer.Enabled = true;
            }
            else
            {
                reflash_timer.Enabled = false;
            }
            
        }

        private void reflash_timer_Tick(object sender, EventArgs e)
        {
            XMain_Search();

            //마지막로우로 스크롤바 이동
            //gridView.MakeRowVisible(gridView.RowCount - 1);
        }

        //계근량 초기화
        private void btn_weightReset_Click(object sender, EventArgs e)
        {
            try
            {
                int visibleDetailRelationIndex = gridView.GetVisibleDetailRelationIndex(gridView.FocusedRowHandle);

                GridView detailView = gridView.GetDetailView(gridView.FocusedRowHandle, visibleDetailRelationIndex) as GridView;

                if (detailView == null || string.IsNullOrEmpty(clsDevexpressGrid.GetFocusedRowCellValue(detailView, "DELIVERY_NO")))
                {
                    ShowMessageBox.XtraShowInformation("초기화 하실 계근세부사항을 선택하여 주세요");
                    return;
                }

                string delivery_no = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "DELIVERY_NO");
                string order_no = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "ORDER_NO");
                string order_line = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "ORDER_LINE_SKEY");
                string erp_up_yn = clsDevexpressGrid.GetFocusedRowCellValue(detailView, "ERP_UP_YN");

                if (erp_up_yn == "전송")
                {
                    ShowMessageBox.XtraShowInformation("ERP에 업로드 처리된 주문서는 초기화 하지 못합니다");
                    return;
                }

                DialogResult result = ShowMessageBox.Confirm("선택하신 배차번호" + delivery_no + "-" + order_no + "-" + order_line + " 계근사항을 초기화 하시겠습니까?");
                if (result != DialogResult.Yes)
                {
                    return;
                }

                SQL =
                "UPDATE BULK_ORDER  " +
                "SET ERP_LOCATION = NULL, BEFORE_WEIGHT = NULL, BEFORE_WEIGHT = NULL, BEFORE_WEIGHT_TIME = NULL,  " +
                "WEIGHT = NULL, WEIGHT_TIME = NULL, ERP_QTY = NULL, ERP_WEIGHT = NULL, PC_STATUS = '1', EVENT_LOG = '계근초기화/" + clsCommon.UserId + "' " +
                "WHERE DELIVERY_NO = '{0}' AND ORDER_NO = '{1}' AND ORDER_LINE_SKEY = '{2}' ";

                SQL = string.Format(SQL,
                    delivery_no,
                    order_no,
                    order_line
                    );

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    ShowMessageBox.XtraShowWarning("계근량 초기화에 실패했습니다");
                    return;
                }

                XMain_Search();

                ShowMessageBox.XtraShowInformation("계근량 초기화가 완료되었습니다");
            }
            catch (Exception ex) 
            {
                clsLog.logSave(ex.Message, 0);
                ShowMessageBox.XtraShowWarning("초기화를 하는 도중 에러가 발생했습니다");
            }
        }

        //엑셀내보내기(세부)
        private void btn_exportExcel_Click(object sender, EventArgs e)
        {
            gridView.OptionsPrint.ExpandAllDetails = true;
            gridView.OptionsPrint.PrintDetails = true;
            gridView.OptionsPrint.AutoWidth = true;
            XlsxExportOptionsEx o = new XlsxExportOptionsEx();
            o.ExportType = DevExpress.Export.ExportType.WYSIWYG;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                gridView.ExportToXlsx(sfd.FileName, o);
                System.Diagnostics.Process.Start(sfd.FileName);
            }
        }

        private void weightProcess(string gubun)
        {
            try
            {
                if (gridView.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("계근수동처리 하실 입차내역을 선택하여 주세요");
                    return;
                }

                string is_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "IS_NO");
                string incar_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "INCAR_NO");

                DialogResult result = ShowMessageBox.Confirm("선택하신 발급번호" + is_no + ", 차량번호" + incar_no + " 입차내역을 수동계량 하시겠습니까?");
                if (result != DialogResult.Yes)
                {
                    return;
                }

                string scale_weight = string.Empty;

                //if (gubun == "PLC")
                //{
                //    string weight_end_chk = string.Empty;
                //    try
                //    {
                //        weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
                //    }
                //    catch
                //    {
                //        weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
                //    }


                //    if (weight_end_chk != "1")
                //    {
                //        ShowMessageBox.XtraShowInformation("계량스케일 안정상태가 아닙니다\r\n차량정차 후 실행해주세요");
                //        return;
                //    }

                //    string temp_weight1 = string.Empty;
                //    string temp_weight2 = string.Empty;
                //    try
                //    {
                //        temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
                //        clsUtil.Delay(200);
                //        temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
                //    }
                //    catch
                //    {
                //        temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
                //        clsUtil.Delay(200);
                //        temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
                //    }

                //    try
                //    {
                //        scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
                //    }
                //    catch (Exception)
                //    {
                //        scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
                //    }
                //}
                //else if (gubun == "INPUT")
                //{
                //    string input_weight = XtraInputBox.Show("", "계량값을 입력해주세요", "");

                //    if (string.IsNullOrEmpty(input_weight))
                //    {
                //        ShowMessageBox.XtraShowInformation("계량값을 입력하지 않아 취소되었습니다");
                //        return;
                //    }

                //    scale_weight = input_weight;
                //}

                int intScaleValue = 0;

                bool isIntChk = int.TryParse(scale_weight, out intScaleValue);

                if (!isIntChk)
                {
                    ShowMessageBox.XtraShowInformation("계량값을 정확하지 않습니다\r\n다시 실행 바랍니다");
                    return;
                }

                string return_status = string.Empty;

                mInoutSelect mInoutSelect = new mInoutSelect();
                mInoutSelect.StartPosition = FormStartPosition.CenterScreen;
                DialogResult dResult = mInoutSelect.ShowDialog();

                if (dResult == DialogResult.Yes)
                {
                    return_status = clsCarProcess.InWeightProcess(incar_no, scale_weight, is_no);
                }
                else if (dResult == DialogResult.No)
                {

                    return_status = clsCarProcess.outChkProcess(incar_no, scale_weight, is_no);
                }
                else
                {
                    ShowMessageBox.XtraShowInformation("수동계량이 취소되었습니다\r\n입/출차 구분을 선택하여 주십쇼");
                    return;
                }


                if (return_status == "OK")
                {
                    if (gubun == "PLC")
                    {
/*                        try
                        {
                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0004", "1");
                        }
                        catch (Exception)
                        {
                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0004", "1");
                        }

                        clsUtil.Delay(4000);

                        try
                        {
                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");
                        }
                        catch (Exception)
                        {
                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");
                        }

                        XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");*/
                    }

                    ShowMessageBox.XtraShowInformation("계근수동계량처리가 완료되었습니다");
                    XMain_Search();
                }
                else
                {
                    ShowMessageBox.XtraShowInformation("계근수동계량처리가 실패했습니다\r\n실패사유: " + return_status);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "weightProcess", ex);
            }
        }

        //계근량 수동처리
        private void btn_weightSelf_Click(object sender, EventArgs e)
        {
            weightProcess("PLC");

        }

        private void btn_weightInput_Click(object sender, EventArgs e)
        {
            weightProcess("INPUT");
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("프린터 출력을 하실 입차내역을 선택하여 주세요");
                return;
            }

            string is_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "IS_NO");
            string car_tdetail = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "CAR_TDETAIL");

            bool print = false;
            if (car_tdetail == "카고")
            {
                print = true;
            }
            else
            {
                print = false;
            }
            mInOutPrintSel popForm = new mInOutPrintSel(is_no, print);
            popForm.StartPosition = FormStartPosition.CenterScreen;
            popForm.Show();
        }

        private void btn_weightTest_Click(object sender, EventArgs e)
        {
            string scale_weight = string.Empty;
            string temp_weight1 = string.Empty;
            string temp_weight2 = string.Empty;

            string weight_end_chk = string.Empty;
            //try
            //{
            //    weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
            //}
            //catch
            //{
            //    weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
            //}

            //try
            //{
            //    temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
            //    clsUtil.Delay(200);
            //    temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
            //}
            //catch
            //{
            //    temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
            //    clsUtil.Delay(200);
            //    temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
            //}

            try
            {
                scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
            }
            catch (Exception)
            {
                scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
            }

            string msgEndChk = string.Empty;
            if (weight_end_chk == "1")
            {
                msgEndChk = "(안정)";
            }else
            {
                msgEndChk = "(안정안됨)";
            }

            btn_weightTest.Text = scale_weight + "\r\n" + msgEndChk;
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void gridView_Detail_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView_Detail);
        }

        private void btn_gridExpAll_Click(object sender, EventArgs e)
        {
            for (int i=0; i < gridView.RowCount; i++)
            {
                gridView.SetMasterRowExpanded(Convert.ToInt16(i), true);
            }

        }

        private void btn_gridUnExpAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView.RowCount; i++)
            {
                gridView.SetMasterRowExpanded(Convert.ToInt16(i), false);
            }

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

            //// 저장
            //if (e.Control && e.KeyCode == Keys.S)
            //{
            //    XMain_Save();
            //}

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workStDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workEdDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}