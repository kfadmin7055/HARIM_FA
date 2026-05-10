using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HARIM_FA_DOSING
{
    public partial class frm_BulkOrder : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;
        private string[] sValid = null;
        private string vBulkProcessKey = string.Empty;
        private string vBagProcessKey = string.Empty;

        bool chk_version = false;
        decimal dNote_Per = 0;

        public frm_BulkOrder()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(gridView_Detail, Properties.Settings.Default.FontSize);
        }

        private string workNumber_maker(string sPlantCode, string sProcesskey, string sWorkDate)
        {
            try
            {
                string return_seq = string.Empty;
                string SQL = $@"
                SELECT NVL(MAX(WORK_SEQ) + 1, 1) AS SEQ FROM BULK_ORDER WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcesskey}' AND WO_NUMBER = '{sWorkDate}'
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

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_BulkOrder_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            authDs = clsSql.GetAuthDataSet(this.Name);

            //플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            vBulkProcessKey = clsCommon.GetProcessKey("벌크");
            vBagProcessKey = clsCommon.GetProcessKey("타이콘");

            //작업일자
            dtFromDelivery.EditValue = DateTime.Today;
            dtToDelivery.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();

            Initcontrol();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void Initcontrol()
        {
            clsDevexpressGrid.ItemLookUpEditSetup(gridcbo_PLANT_CODE, clsCommon.GetPlant(), "", false, false);

            // 라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), vBulkProcessKey), "", false, 0, true);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), vBulkProcessKey), "라인을 선택 해주세요.", false);

            Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "PER", "비율" }
                };

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboNote, clsCommon.getNote(cboPlant_Code.EditValue?.ToString()), "배합비 버전이 없습니다.", false);

            //clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboVEHICLENO, clsCommon.GetCarMaster("10"));

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboScale, clsCommon.GetScale(cboPlant_Code.EditValue?.ToString(), vBulkProcessKey, cboL_Code.EditValue?.ToString()), "", true, true, null, null, "CODE", "CODE");

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboLocation, clsCommon.GetBin(cboPlant_Code.EditValue?.ToString(), vBulkProcessKey, cboL_Code.EditValue?.ToString()), "", true, true, null, null, "CODE", "CODE");

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboCUST_NO, clsCommon.GetCustomer(false));

            // 납품유형
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboLFART, clsCommon.GetLFART());

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboYn, clsCommon.GetYn(null, new string[] { "확정", "미확정" }));

            //작업계획
            clsDevexpressGrid.ItemLookUpEditSetup(gridCboC_CONDITION, clsCommon.GetPcStatus(), "", false, false);

            gridChk.ValueChecked = "Y";
            gridChk.ValueUnchecked = "N";
            gridChk.NullStyle = StyleIndeterminate.Unchecked;
            gridChk.CheckStyle = CheckStyles.Standard;
        }



        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT a.PLANT_CODE
                 , a.PROCESS_KEY
                 , a.L_CODE
                 , a.WO_NUMBER
                 , a.WORK_SEQ
                 , a.DISPATCHNO
                 , a.ORDERNO
                 , a.ORDERLINENO
                 , a.WORK_START_DATE
                 , a.BATCH
                 , a.R_BATCH
                 , a.BATCH_Q
                 , a.ORDER_QTY
                 , a.P_Q
                 , a.QUANTITY
                 , a.RESOURCE_NO
                 , a.RESOURCE_NO1
                 , a.NOTE
                 , a.PART_NAME
                 , a.CUST_NO
                 , a.CUST_NAME
                 , a.C_CONDITION
                 , a.AUTO_YN
                 , a.CAR_NO_REAL
                 , a.CAR_FULL_NUM
                 , a.START_TIME
                 , a.END_TIME
                 , a.IS_NO
                 , a.PC_STATUS
                 , a.ERP_LOCATION
                 , a.BEFORE_WEIGHT
                 , a.BEFORE_WEIGHT_TIME
                 , a.WEIGHT
                 , a.WEIGHT_TIME
                 , a.REMARK
                 , a.I_TIME
                 , a.U_TIME
                 , a.U_USER
                 , a.EVENT_LOG
                 , a.LOCATION
                 , a.LFART
                 , a.SCALE_CODE
                 , a.DELIVERYDATE
                 , a.CARRIERCODE
                 , a.CARRIERNAME
            FROM BULK_ORDER a
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND a.PROCESS_KEY IN ('{vBulkProcessKey}', '{vBagProcessKey}') 
                    AND ('{cboL_Code.EditValue}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue}')
                    AND (a.WO_NUMBER BETWEEN '{dtFromDelivery.DateTime.ToString("yyyyMMdd")}' AND '{dtToDelivery.DateTime.ToString("yyyyMMdd")}')
                ORDER BY 
                    INSTR('{clsCommon.PcStatus.Plan},{clsCommon.PcStatus.InProgress},{clsCommon.PcStatus.Completed},{clsCommon.PcStatus.ForceCompleted}', a.C_CONDITION) ASC, 
                    --a.ORDERNO, 
                    a.WO_NUMBER DESC, 
                    a.WORK_SEQ DESC
                ";

                DataSet searchDs1 = Dbconn.conn.ExecutDataset(SQL, "bulkTable");

                SQL = $@"
                WITH BULK AS (
                SELECT PLANT_CODE, PROCESS_KEY, L_CODE, 
                   WO_NUMBER, WORK_SEQ
                FROM BULK_ORDER
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND PROCESS_KEY IN ('{vBulkProcessKey}', '{vBagProcessKey}') 
                    AND ('{cboL_Code.EditValue}' IS NULL OR L_CODE = '{cboL_Code.EditValue}')
                    AND DELIVERYDATE BETWEEN '{dtFromDelivery.DateTime.ToString("yyyyMMdd")}' AND '{dtToDelivery.DateTime.ToString("yyyyMMdd")}'
                )

                SELECT a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE
                   , a.WO_NUMBER, a.WORK_SEQ, a.SEQ
                   , a.IS_NO, a.BATCH, a.PRO_Q
                   , a.GUBUN, a.START_TIME, a.END_TIME
                   , a.C_CONDITION, a.REMARK, a.I_TIME
                   , a.LOCATION
                FROM WAP_BULK_ORDER a
                    INNER JOIN BULK b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE
                                    AND b.WO_NUMBER = a.WO_NUMBER AND b.WORK_SEQ = a.WORK_SEQ
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR a.PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND a.PROCESS_KEY IN ('{vBulkProcessKey}', '{vBagProcessKey}') 
                    AND ('{cboL_Code.EditValue}' IS NULL OR a.L_CODE = '{cboL_Code.EditValue}')
                ";

                DataSet searchDs2 = Dbconn.conn.ExecutDataset(SQL, "gridView_Detail");

                searchDs1.Merge(searchDs2);

                DataColumn[] keyColumn = {
                    searchDs1.Tables["bulkTable"].Columns["PLANT_CODE"]
                    , searchDs1.Tables["bulkTable"].Columns["PROCESS_KEY"]
                    , searchDs1.Tables["bulkTable"].Columns["WO_NUMBER"]
                    , searchDs1.Tables["bulkTable"].Columns["WORK_SEQ"]
                    };

                DataColumn[] foreignKeyColumn = {
                    searchDs1.Tables["gridView_Detail"].Columns["PLANT_CODE"]
                    , searchDs1.Tables["gridView_Detail"].Columns["PROCESS_KEY"]
                    , searchDs1.Tables["gridView_Detail"].Columns["WO_NUMBER"]
                    , searchDs1.Tables["gridView_Detail"].Columns["WORK_SEQ"]
                    };

                searchDs1.Relations.Add("벌크상차 상세내역", keyColumn, foreignKeyColumn, false);

                clsDevexpressGrid.BindGridControl(gridControl, gridView, searchDs1.Tables["bulkTable"], true, true);

                sValid = new string[] { "" };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridScboResourceNo, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("제품")}'", "", 0, false, false, "KG"));
                DevExpress.XtraGrid.Views.Grid.GridView view = gridScboResourceNo.View as DevExpress.XtraGrid.Views.Grid.GridView;

                view.OptionsView.ColumnAutoWidth = false;

                // 원하는 컬럼 사이즈 지정
                view.Columns["CODE"].Width = 100;
                view.Columns["NAME"].Width = 260;

                gridControl.LevelTree.Nodes.Add("벌크상차 상세내역", gridView_Detail);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }
        #endregion


        #region 조회버튼 클릭 이벤트
        private void btn_search_Click(object sender, EventArgs e)
        {
            gridView.ClearColumnsFilter();   // 모든 컬럼 필터 초기화
            gridView.ActiveFilter.Clear();   // ActiveFilter 초기화
            gridView.ClearSorting();         // 정렬 초기화
            gridView.OptionsCustomization.AllowSort = true; // (정렬 비활성화된 경우를 대비해)
            gridView.RefreshData();
            XMain_Search();
        }
        #endregion

        private void repItemLkUpEdit_LOCATION_EditValueChanged(object sender, EventArgs e)
        {
            this.gridView.PostEditor();
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {

                if (clsCommon.PlantCode == "P201" && gridView.FocusedColumn.FieldName == "RESOURCE_NO")
                {
                    if (string.IsNullOrEmpty(gridView.GetFocusedRowCellValue("RESOURCE_NO").ToString().Trim()))
                        e.Cancel = false;
                    else
                        e.Cancel = true;
                }

                // 031004	완료
                if (gridView.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.Completed)
                    || gridView.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.ForceCompleted)
                    || gridView.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.Canceled)) //작지가 완료처리된것은 수정못하도록 에디트모드 off
                {
                    //if (clsCommon.Auth_Form_Function(authDs, "U"))
                    //{
                    switch (gridView.FocusedColumn.FieldName)
                    {
                        case "RESOURCE_NO":
                        case "LOCATION_ED":
                        case "SCALE_CODE":
                            e.Cancel = true;
                            break;
                        default:
                            e.Cancel = false;

                            break;
                    }
                    //}
                }

                //if (gridView.GetFocusedRowCellValue("C_CONDITION").Equals(clsCommon.GetPcStatusCode("진행")) ||
                //    gridView.GetFocusedRowCellValue("C_CONDITION").Equals(clsCommon.PcStatus.Completed) ||
                //    gridView.GetFocusedRowCellValue("C_CONDITION").Equals(clsCommon.PcStatus.ForceCompleted))
                //{

                //    e.Cancel = true;

                //}


                //else if (!gridView.GetFocusedRowCellValue("CAR_NO_REAL").Equals("9999"))
                //{
                //    if (gridView.FocusedColumn.FieldName == "LOCATION" || gridView.FocusedColumn.FieldName == "BATCH")
                //    {
                //        e.Cancel = false;
                //    }
                //    else
                //    {
                //        e.Cancel = true;
                //    }
                //}
                //else if (gridView.GetFocusedRowCellValue("CAR_NO_REAL").Equals("9999"))
                //{
                //    e.Cancel = false;
                //}
                //else if (gridView.GetFocusedRowCellValue("PROCESS_KEY").Equals(vBagProcessKey))
                //{
                //    e.Cancel = true;
                //}
                //else
                //{
                //    e.Cancel = false;
                //}
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_ShowingEditor", ex);
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

                if (DialogResult.Yes != ShowMessageBox.Confirm("데이터를 저장하시겠습니까?"))
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

                    string startTime;
                    string endTime;
                    string deliveryDate;

                    if (dr["START_TIME"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["START_TIME"].ToString()))
                    {
                        startTime = "''";   // 날짜 값이 없을 경우
                    }
                    else
                    {
                        startTime = $"TO_DATE('{Convert.ToDateTime(dr["START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (dr["END_TIME"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["END_TIME"].ToString()))
                    {
                        endTime = "''";   // 날짜 값이 없을 경우
                    }
                    else
                    {
                        endTime = $"TO_DATE('{Convert.ToDateTime(dr["END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (dr["DELIVERYDATE"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["DELIVERYDATE"].ToString()))
                    {
                        deliveryDate = "";   // 날짜 값이 없을 경우
                    }
                    else if (dr["DELIVERYDATE"].ToString().Contains("-"))
                    {
                        deliveryDate = $"{Convert.ToDateTime(dr["DELIVERYDATE"]).ToString("yyyyMMdd")}";
                    }
                    else
                    {
                        deliveryDate = dr["DELIVERYDATE"]?.ToString();
                    }

                    int intBatchQ = 0;
                    if (dr["BATCH_Q"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["BATCH_Q"].ToString()))
                    {
                        intBatchQ = Convert.ToInt16(dr["BATCH_Q"]);
                    }

                    int intBatch = 0;
                    if (dr["BATCH"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["BATCH"].ToString()))
                    {
                        intBatch = Convert.ToInt16(dr["BATCH"]);
                    }

                    int intBatchWorkQty = 0;

                    if (intBatchQ > 0 && intBatch > 0)
                        intBatchWorkQty = intBatchQ / intBatch;

                    //if (intBatchWorkQty > 2000)
                    //{
                    //    ShowMessageBox.XtraShowWarning("한배치량이 2,000KG를 넘습니다. 배치수를 다시 조절 바랍니다\r\n현재 1배치량 : " + intBatchWorkQty.ToString());
                    //    return;
                    //}

                    if (dr.RowState == DataRowState.Added)
                    {
                        ValidationCheck(dr);

                        SQL = $@"
                        /* 벌크 작업지시 저장 */
                        INSERT INTO BULK_ORDER (
                            PLANT_CODE
                            , PROCESS_KEY
                            , L_CODE
                            , WO_NUMBER
                            , WORK_SEQ
                            , DISPATCHNO
                            , ORDERNO
                            , ORDERLINENO
                            , WORK_START_DATE
                            , BATCH
                            , R_BATCH
                            , BATCH_Q
                            , ORDER_QTY
                            , P_Q
                            , QUANTITY
                            , RESOURCE_NO
                            , PART_NAME
                            , CUST_NO
                            , CUST_NAME
                            , C_CONDITION
                            , AUTO_YN
                            , CAR_NO_REAL
                            , CAR_FULL_NUM
                            , START_TIME
                            , END_TIME
                            , IS_NO
                            , PC_STATUS
                            , ERP_LOCATION
                            , BEFORE_WEIGHT
                            , BEFORE_WEIGHT_TIME
                            , WEIGHT
                            , WEIGHT_TIME
                            , REMARK
                            , I_TIME
                            , U_TIME
                            , U_USER
                            , EVENT_LOG
                            , LOCATION
                            , LFART
                            , DELIVERYDATE
                            , NOTE
                            , SCALE_CODE
                            , CARRIERCODE
                            , CARRIERNAME
                            , RESOURCE_NO1
                        ) VALUES (
                            '{dr["PLANT_CODE"]}'
                            , '{dr["PROCESS_KEY"]}'
                            , '{dr["L_CODE"]}'
                            , '{dr["WO_NUMBER"]}'
                            , '{workNumber_maker(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["WO_NUMBER"]?.ToString())}'
                            , '{dr["DISPATCHNO"]}'
                            , '{dr["ORDERNO"]}'
                            , '{dr["ORDERLINENO"]}'
                            , '{dr["WORK_START_DATE"]}'
                            , '{intBatch}'
                            , '{dr["R_BATCH"]}'
                            , '{intBatchQ}'
                            , '{dr["ORDER_QTY"]}'
                            , '{dr["P_Q"]}'
                            , '{dr["QUANTITY"]}'
                            , '{dr["RESOURCE_NO"]}'
                            , '{dr["PART_NAME"]}'
                            , '{dr["CUST_NO"]}'
                            , '{dr["CUST_NAME"]}'
                            , '{dr["C_CONDITION"]}'
                            , '{dr["AUTO_YN"]}'
                            , '{dr["CAR_NO_REAL"]}'
                            , '{dr["CAR_FULL_NUM"]}'
                            , {startTime}
                            , {endTime}
                            , '{dr["IS_NO"]}'
                            , '{dr["PC_STATUS"]}'
                            , '{dr["ERP_LOCATION"]}'
                            , '{dr["BEFORE_WEIGHT"]}'
                            , '{dr["BEFORE_WEIGHT_TIME"]}'
                            , '{dr["WEIGHT"]}'
                            , '{dr["WEIGHT_TIME"]}'
                            , '{dr["REMARK"]}'
                            , SYSDATE
                            , SYSDATE
                            , '{dr["U_USER"]}'
                            , '{dr["EVENT_LOG"]}'
                            , '{dr["LOCATION"]}'
                            , '{dr["LFART"]}'
                            , '{deliveryDate}'
                            , '{dr["NOTE"]}'
                            , '{dr["SCALE_CODE"]}'
                            , '{dr["CARRIERCODE"]}'
                            , '{dr["CARRIERNAME"]}'
                            , '{dr["RESOURCE_NO1"]}'
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
                        /* 벌크 작업 지시 수정 */
                        UPDATE BULK_ORDER
                        SET    L_CODE             = '{dr["L_CODE"]}'
                            , PROCESS_KEY        = '{dr["PROCESS_KEY"]}'
                            , WO_NUMBER          = '{dr["WO_NUMBER"]}'
                            , WORK_SEQ           = '{dr["WORK_SEQ"]}'
                            , DISPATCHNO         = '{dr["DISPATCHNO"]}'
                            , ORDERNO            = '{dr["ORDERNO"]}'
                            , ORDERLINENO        = '{dr["ORDERLINENO"]}'
                            , WORK_START_DATE    = '{dr["WORK_START_DATE"]}'
                            , BATCH              = '{intBatch}'
                            , R_BATCH            = '{dr["R_BATCH"]}'
                            , BATCH_Q            = '{intBatchQ}'
                            , ORDER_QTY          = '{dr["ORDER_QTY"]}'
                            , P_Q                = '{dr["P_Q"]}'
                            , QUANTITY           = '{dr["QUANTITY"]}'
                            , RESOURCE_NO        = '{dr["RESOURCE_NO"]}'
                            , PART_NAME          = '{dr["PART_NAME"]}'
                            , CUST_NO            = '{dr["CUST_NO"]}'
                            , CUST_NAME          = '{dr["CUST_NAME"]}'
                            , C_CONDITION        = '{dr["C_CONDITION"]}'
                            , AUTO_YN            = '{dr["AUTO_YN"]}'
                            , CAR_NO_REAL        = '{dr["CAR_NO_REAL"]}'
                            , CAR_FULL_NUM       = '{dr["CAR_FULL_NUM"]}'
                            , START_TIME         = {startTime}
                            , END_TIME           = {endTime}
                            , IS_NO              = '{dr["IS_NO"]}'
                            , PC_STATUS          = '{dr["PC_STATUS"]}'
                            , ERP_LOCATION       = '{dr["ERP_LOCATION"]}'
                            , BEFORE_WEIGHT      = '{dr["BEFORE_WEIGHT"]}'
                            , BEFORE_WEIGHT_TIME = '{dr["BEFORE_WEIGHT_TIME"]}'
                            , WEIGHT             = '{dr["WEIGHT"]}'
                            , WEIGHT_TIME        = '{dr["WEIGHT_TIME"]}'
                            , REMARK             = '{dr["REMARK"]}'
                            , I_TIME             = SYSDATE
                            , U_USER             = '{clsCommon.UserId}'
                            , EVENT_LOG          = '{dr["EVENT_LOG"]}'
                            , LOCATION           = '{dr["LOCATION"]}'
                            , LFART              = '{dr["LFART"]}'
                            , DELIVERYDATE       = '{deliveryDate}'
                            , NOTE               = '{dr["NOTE"]}'
                            , SCALE_CODE         = '{dr["SCALE_CODE"]}'
                            , CARRIERCODE        = '{dr["CARRIERCODE"]}'
                            , CARRIERNAME        = '{dr["CARRIERNAME"]}'
                            , RESOURCE_NO1       = '{dr["RESOURCE_NO1"]}'
                        WHERE  PLANT_CODE         = '{dr["PLANT_CODE"]}'
                            AND    PROCESS_KEY        = '{dr["PROCESS_KEY"]}'
                            AND    WO_NUMBER          = '{dr["WO_NUMBER"]}'
                            AND    WORK_SEQ           = '{dr["WORK_SEQ"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    dr.AcceptChanges();
                    gridView.RefreshData();
                } //foreach

                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btnbinSave_Click(object sender, EventArgs e)
        {
            DialogResult result = ShowMessageBox.Confirm("인출빈을 수정 하시겠습니까?", "인출빈만 수정되고 다른정보는 수정되지 않습니다.");

            if (result == DialogResult.Yes)
            {
                try
                {
                    clsDevexpressGrid.GridEndEdit(gridView);

                    if (!clsCommon.Auth_Form_Function(authDs, "W"))
                    {
                        ShowMessageBox.XtraShowInformation("권한이 없습니다");
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

                        string startTime;
                        string endTime;
                        string deliveryDate;

                        if (dr["START_TIME"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["START_TIME"].ToString()))
                        {
                            startTime = "''";   // 날짜 값이 없을 경우
                        }
                        else
                        {
                            startTime = $"TO_DATE('{Convert.ToDateTime(dr["START_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                        }

                        if (dr["END_TIME"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["END_TIME"].ToString()))
                        {
                            endTime = "''";   // 날짜 값이 없을 경우
                        }
                        else
                        {
                            endTime = $"TO_DATE('{Convert.ToDateTime(dr["END_TIME"]).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                        }

                        if (dr["DELIVERYDATE"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["DELIVERYDATE"].ToString()))
                        {
                            deliveryDate = "";   // 날짜 값이 없을 경우
                        }
                        else if (dr["DELIVERYDATE"].ToString().Contains("-"))
                        {
                            deliveryDate = $"{Convert.ToDateTime(dr["DELIVERYDATE"]).ToString("yyyyMMdd")}";
                        }
                        else
                        {
                            deliveryDate = dr["DELIVERYDATE"]?.ToString();
                        }

                        int intBatchQ = 0;
                        if (dr["BATCH_Q"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["BATCH_Q"].ToString()))
                        {
                            intBatchQ = Convert.ToInt16(dr["BATCH_Q"]);
                        }

                        int intBatch = 0;
                        if (dr["BATCH"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["BATCH"].ToString()))
                        {
                            intBatch = Convert.ToInt16(dr["BATCH"]);
                        }

                        int intBatchWorkQty = 0;

                        if (intBatchQ > 0 && intBatch > 0)
                            intBatchWorkQty = intBatchQ / intBatch;

                        //if (intBatchWorkQty > 2000)
                        //{
                        //    ShowMessageBox.XtraShowWarning("한배치량이 2,000KG를 넘습니다. 배치수를 다시 조절 바랍니다\r\n현재 1배치량 : " + intBatchWorkQty.ToString());
                        //    return;
                        //}

                        if (dr.RowState == DataRowState.Modified)
                        {
                            SQL = $@"
                            /* 벌크 작업 지시 수정 */
                            UPDATE BULK_ORDER
                            SET LOCATION           = '{dr["LOCATION"]}'
                                , SCALE_CODE         = '{dr["SCALE_CODE"]}'
                                , L_CODE         = '{dr["L_CODE"]}'
                            WHERE  PLANT_CODE         = '{dr["PLANT_CODE"]}'
                                AND    PROCESS_KEY        = '{dr["PROCESS_KEY"]}'
                                AND    WO_NUMBER          = '{dr["WO_NUMBER"]}'
                                AND    WORK_SEQ           = '{dr["WORK_SEQ"]}'
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 1)
                            {
                                clsLog.logSave(this.Text, "btn_save_Click", SQL);
                                ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                                return;
                            }
                        }

                        dr.AcceptChanges();
                        gridView.RefreshData();
                    } //foreach

                    XMain_Search();

                }
                catch (Exception ex)
                {
                    clsLog.logSave(this, "btn_save_Click", ex);
                    ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
                }
            }
        }

        private void ValidationCheck(DataRow dr)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (string.IsNullOrEmpty(dr["LOCATION"].ToString()))
            {
                dr.SetColumnError("LOCATION", "인출빈을 선택하여 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION"));
                return;
            }

            if (string.IsNullOrEmpty(dr["BATCH_Q"].ToString()))
            {
                dr.SetColumnError("BATCH_Q", "배치량을 입력하여 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                return;
            }

            if (string.IsNullOrEmpty(dr["BATCH"].ToString()))
            {
                dr.SetColumnError("BATCH", "배치를 입력하여 주세요");
                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH"));
                return;
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_reflash_Click(object sender, EventArgs e)
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
                    ShowMessageBox.XtraShowInformation("삭제하실 출하주문지시를 선택하여 주세요");
                    return;
                }

                DataRow row = gridView.GetDataRow(gridView.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    gridView.DeleteRow(gridView.FocusedRowHandle);
                }
                else
                {
                    string CAR_NO_REAL = gridView.GetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["CAR_NO_REAL"]).ToString().Trim();

                    if (!CAR_NO_REAL.Equals("9999"))
                    {
                        ShowMessageBox.XtraShowInformation("톤백 작업지시만 삭제하실 수 있습니다. \r\n출하주문지시를 삭제하시려면 ERP프로그램에서 출하주문지시 삭제 바랍니다 ");
                        return;
                    }

                    string condition = gridView.GetRowCellDisplayText(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"]);

                    if (!condition.Equals("계획"))
                    {
                        ShowMessageBox.XtraShowInformation("계획중인 출하주문지시만 삭제하실 수 있습니다");
                        return;
                    }


                    string WORK_START_DATE = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WO_NUMBER");
                    string work_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORK_SEQ");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업순번 " + work_num + " 작업지시를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        SQL = $@"
                        DELETE FROM BULK_ORDER 
                        WHERE PLANT_CODE  = '{gridView.GetFocusedRowCellValue("PLANT_CODE")}'
                            AND PROCESS_KEY = '{gridView.GetFocusedRowCellValue("PROCESS_KEY")}'
                            AND WO_NUMBER   = '{gridView.GetFocusedRowCellValue("WO_NUMBER")}'
                            AND WORK_SEQ    = '{gridView.GetFocusedRowCellValue("WORK_SEQ")}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                            return;
                        }
                    }
                }

                ShowMessageBox.XtraShowInformation("삭제 되었습니다.");

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click", ex);
                ShowMessageBox.XtraShowError("행을 삭제하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            //if (gridView.SelectedRowsCount == 0)
            //{
            //    ShowMessageBox.XtraShowInformation("상세내역을 선택해주세요.");
            //    return;
            //}

            //clsDevexpressGrid.GridViewDetailAddRow(gridView);

            //clsDevexpressGrid.SetDataGridViewDetail(gridView, "PLANT_CODE", clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PLANT_CODE"));
            //clsDevexpressGrid.SetDataGridViewDetail(gridView_Detail, "RESOURCE_NO", clsDevexpressGrid.GetFocusedRowCellValue(gridView_Detail, "RESOURCE_NO"));

            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
                return;
            }

            if (cboL_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("라인을 먼저 조회 해주세요.");
                cboL_Code.Focus();
                clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
                return;
            }

            var view = gridControl.FocusedView;

            if (view == gridView)
            {
                clsDevexpressGrid.GridViewAddRow(gridView);

                gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
                gridView.SetFocusedRowCellValue("PROCESS_KEY", vBulkProcessKey);
                gridView.SetFocusedRowCellValue("L_CODE", cboL_Code.EditValue);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["WO_NUMBER"], string.Format("{0:yyyyMMdd}", dtFromDelivery.EditValue));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["BATCH"], 1);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["BATCH_Q"], 0);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["WORK_START_DATE"], string.Format("{0:yyyyMMdd}", dtFromDelivery.EditValue));
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["CAR_NO_REAL"], "9999");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["CUST_NAME"], "톤백");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["C_CONDITION"], clsCommon.PcStatus.Plan);
            }
            else
            {
                clsDevexpressGrid.GridViewDetailAddRow(gridView);
            }
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void gridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (gridView.GetRowCellValue(e.RowHandle, gridView.Columns["C_CONDITION"]) == null)
                    return;

                string condition = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["C_CONDITION"]).ToString();

                if (e.RowHandle != this.gridView.FocusedRowHandle || e.Column.AbsoluteIndex == this.gridView.FocusedColumn.AbsoluteIndex)
                {
                    if (condition == clsCommon.PcStatus.Plan) //완료
                    {
                        e.Appearance.BackColor = Color.Yellow;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }

                    if (condition == clsCommon.PcStatus.InProgress) //완료
                    {
                        e.Appearance.BackColor = Color.LawnGreen;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }

                    if ((condition == clsCommon.PcStatus.Completed) || (condition == clsCommon.PcStatus.ForceCompleted)) //완료
                    {
                        e.Appearance.BackColor = Color.LightGray;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_endSave_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("완료처리 하실 출하지시를 선택하여 주세요");
                return;
            }

            string input_pq = XtraInputBox.Show("", "해당 출하지시에 입력 하실 상차량을 입력하여주세요", "");

            if (string.IsNullOrEmpty(input_pq))
            {
                return;
            }

            XMain_EndSave();

            XMain_Search();
        }

        private void XMain_EndSave()
        {
            SQL = $@"
            UPDATE BULK_ORDER SET  P_Q = '{3}', C_CONDITION = '{clsCommon.PcStatus.ForceCompleted}'
            WHERE PROCESS_KEY = '{clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PROCESS_KEY")}' 
                AND WO_NUMBER = '{clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WO_NUMBER")}'
                AND WORK_SEQ = '{clsDevexpressGrid.GetFocusedRowCellValue(gridView, "WORK_SEQ")}'
            ";

            Dbconn.conn.SQLrun(SQL);
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
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void gridView_ShownEditor(object sender, EventArgs e)
        {
            string bResourceNo = string.Empty;
            string sResourceNo = string.Empty;

            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (view.FocusedColumn.FieldName == "LOCATION")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                bResourceNo = view.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();
                sResourceNo = view.GetFocusedRowCellValue("RESOURCE_NO1")?.ToString();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.GetBin(cboPlant_Code.EditValue?.ToString(), vBulkProcessKey, cboL_Code.EditValue?.ToString(), "", string.IsNullOrWhiteSpace(sResourceNo) ? bResourceNo : sResourceNo), "", true, 0, false, true, false, new string[] { "CODE", "NAME" }, null, "CODE", "CODE");
                edit.Properties.PopupFormMinSize = new Size(200, 300);
            }

            if (view.FocusedColumn.FieldName == "NOTE")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.getNote(view.GetFocusedRowCellValue("PLANT_CODE")?.ToString(), view.GetFocusedRowCellValue("RESOURCE_NO")?.ToString(), "2"), "", false, 0, false, true, false);
                edit.Properties.PopupFormMinSize = new Size(200, 300);
            }
        }

        private void gridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
        }

        private void gridView_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            string sPlantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PLANT_CODE");
            string sProcessKey = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "PROCESS_KEY");
            string sLCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "L_CODE");

            if (e.Column.FieldName == "RESOURCE_NO")  // 수량이 변경된 경우
            {
                gridView.UpdateCurrentRow();

                string sLocation = string.Empty;
                string sScale = string.Empty;

                string sResourceNo = e.Value?.ToString();

                SQL = $@"
                /* 브랜드 품목 반제품 찾기 */
                SELECT P.RESOURCE_NO, d.IDNRK AS BLD_NO
                FROM SAP_DI_PRODUCT P
                    JOIN SAP_IN_BOM_CONM M ON m.PLANT_CODE = P.PLANT_CODE AND m.RESOURCE_NO = P.RESOURCE_NO AND M.P_TYPE = '2' AND M.STLST = '2'
                    JOIN SAP_IN_BOM_COND D ON d.PLANT_CODE = P.PLANT_CODE AND d.RESOURCE_NO = M.RESOURCE_NO AND D.P_TYPE = M.P_TYPE
                WHERE p.B_P = 'X' AND p.PLANT_CODE = '{sPlantCode}' AND p.RESOURCE_NO = '{sResourceNo}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, "RESOURCE_NO1", Dbconn.conn.getData(ds, "BLD_NO", 0));
                }

                gridView.SetFocusedValue(sResourceNo);

                string sNOTE = clsCommon.getLastVersion(sPlantCode, sResourceNo, out chk_version, out dNote_Per);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, "NOTE", sNOTE);

                clsCommon.getSelectBin(sPlantCode, sProcessKey, sLCode, sResourceNo, out sLocation, out sScale);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, "SCALE_CODE", sScale);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, "LOCATION", sLocation);
            }

            if (e.Column.FieldName == "LOCATION")  // 빈 변경
            {
                if (string.IsNullOrWhiteSpace(e.Value?.ToString()))
                {
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, "L_CODE", "");
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, "SCALE_CODE", "");
                    return;
                }

                SQL = $@"
                /* 빈에서 스케일과 라인 찾기 */
                SELECT a.SCALE_CODE, b.L_CODE
                FROM BIN a
                    INNER JOIN SCALE b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE AND b.SCALE_CODE = a.SCALE_CODE
                WHERE a.PLANT_CODE = '{sPlantCode}' AND a.PROCESS_KEY = '{sProcessKey}'
                    AND a.LOCATION = '{e.Value}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, "L_CODE", Dbconn.conn.getData(ds, "L_CODE", 0));
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, "SCALE_CODE", Dbconn.conn.getData(ds, "SCALE_CODE", 0));
                }
            }
        }

        /// <summary>
        /// 선택한 작업 복사
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWorkCopy_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            // GridView에서 현재 선택된 DataRow 가져오기
            DataRow dr = gridView.GetFocusedDataRow();
            if (dr != null)
            {
                // GridView의 DataSource(DataTable) 참조
                DataTable dt = ((DataView)gridView.DataSource).Table;

                // 새로운 로우 생성
                DataRow newRow = dt.NewRow();

                // 기존 로우 값 복사
                newRow.ItemArray = dr.ItemArray.Clone() as object[];

                // 🔹 일부 컬럼 값 변경
                newRow["WORK_SEQ"] = workNumber_maker(newRow["PLANT_CODE"]?.ToString(), newRow["PROCESS_KEY"]?.ToString(), newRow["WO_NUMBER"]?.ToString());  // 순번 +1
                newRow["U_TIME"] = DateTime.Now;                    // 입력일시 갱신
                newRow["U_USER"] = clsCommon.UserId;                        // 작업자 변경
                newRow["C_CONDITION"] = clsCommon.PcStatus.Plan;
                newRow["START_TIME"] = DBNull.Value;
                newRow["END_TIME"] = DBNull.Value;
                newRow["P_Q"] = DBNull.Value;

                // 새로운 로우를 DataTable에 추가
                dt.Rows.Add(newRow);
            }
        }

        private void txtResource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }
    }
}