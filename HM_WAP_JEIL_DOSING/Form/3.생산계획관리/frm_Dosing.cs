using Core.Class;
using Core.Enum;
using Core.Extension;
using Core.Extension;
using DevExpress.ChartRangeControlClient.Core;
using DevExpress.Charts.Native;
using DevExpress.CodeParser;
using DevExpress.DataAccess.Native.EntityFramework;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Pdf.Native.BouncyCastle.Utilities.Collections;
using DevExpress.PivotGrid.QueryMode;
using DevExpress.Schedule;
using DevExpress.Utils;
using DevExpress.Utils.Gesture;
using DevExpress.Xpo;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraBars.Docking.Helpers;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Filtering.Templates;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraSpreadsheet.Import.Xls;
using DevExpress.XtraSpreadsheet.Layout;
using DevExpress.XtraSpreadsheet.UI;
using DevExpress.XtraTreeList.ViewInfo;
using HARIM_FA_DOSING.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static DevExpress.Utils.Diagnostics.GUIResources;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using static System.Formats.Asn1.AsnWriter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HARIM_FA_DOSING
{
    public partial class frm_Dosing : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = string.Empty;
        private string vPlant_Code = string.Empty;
        private string vProcess_Code = string.Empty;
        private string vLine_Code = string.Empty;
        private string formname = string.Empty;
        private string vProcessName = string.Empty;

        DataSet authDs;
        private string[] sValid = null;
        public static string runProcessCd = string.Empty;
        public bool bWorkStatus = false;

        private static decimal vdMerge = 0;

        public static string vPLCAddress;
        public static int vPLCUnit;
        public static int vPLCDataCount;

        bool chk_version = false;
        decimal dNote_Per = 0;

        private bool isInitializing = false;

        bool isVisible = true;

        string[] aPassProcess = new string[] { clsCommon.GetProcessKey("후레이크"), clsCommon.GetProcessKey("익스콘"), clsCommon.GetProcessKey("벌크 원료"), clsCommon.GetProcessKey("수동생산") };

        static int vPLC_Location = 0;
        private static string sMsg = string.Empty;

        private static bool vWorkStart = false;

        private int totalSeconds = 0;
        private bool bTransFlag = false;

        private static int[] Cimon_Job_Data = new int[150];
        const int WORK_YYYY = 10;
        const int BATCH_PV = 22;

        public static string workNumMake(string process_key, string wWork_Date)
        {
            try
            {
                string SQL = $@"
                SELECT NVL(MAX(NUM) + 1, 1) AS SEQ
                FROM WORK_ORDER
                WHERE PROCESS_KEY = '{process_key}'
                  AND WORKDATE = '{wWork_Date}'
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

        public frm_Dosing(string plant_code, string process_key, string lcode, string formName, string sProcessName)
        {
            InitializeComponent();
            formname = formName;

            vPlant_Code = plant_code;
            vProcess_Code = process_key;
            vLine_Code = lcode;
            vProcessName = sProcessName;


            gridView_work.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            gridView_work.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;

            clsDevexpressGrid.EditGridViewInit(gridView_work, Properties.Settings.Default.FontSize);

            clsDevexpressGrid.EditGridViewInit(gridView_batchRun, Properties.Settings.Default.FontSize);

            clsDevexpressGrid.ReadGridViewInit(gridView_batchList, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(gridView_batchResult, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.ReadGridViewInit(gridView_batchLog, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            //clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl_work, gridView_work);
        }

        #region 폼로드 이벤트
        private void frm_Dosing_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            gridView_work.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl_work, gridView_work);

            try

            {
                if (vProcess_Code == clsCommon.GetProcessKey("배합"))      // 031002	배합공정
                {
                    //formname = "frm_dosing";
                    gridColumn_BAD_CODE1.Visible = false;
                    gridColumn_BAD_QTY1.Visible = false;
                    //gridColumn_BAD_CODE2.Visible = false;
                    //gridColumn_BAD_QTY2.Visible = false;
                    //gridColumn_BAD_CODE3.Visible = false;
                    //gridColumn_BAD_QTY3.Visible = false;
                    //gridColumn_BAD_CODE4.Visible = false;
                    //gridColumn_BAD_QTY4.Visible = false;
                    //gridColumn_BAD_CODE5.Visible = false;
                    //gridColumn_BAD_QTY5.Visible = false;
                }
                else
                {
                    gridColumn_BAD_CODE1.Visible = true;
                    gridColumn_BAD_QTY1.Visible = true;
                    //gridColumn_BAD_CODE2.Visible = true;
                    //gridColumn_BAD_QTY2.Visible = true;
                    //gridColumn_BAD_CODE3.Visible = true;
                    //gridColumn_BAD_QTY3.Visible = true;
                    //gridColumn_BAD_CODE4.Visible = true;
                    //gridColumn_BAD_QTY4.Visible = true;
                    //gridColumn_BAD_CODE5.Visible = true;
                    //gridColumn_BAD_QTY5.Visible = true;
                }

                if (aPassProcess.Contains(vProcess_Code))
                {
                    layoutControlItem_workStart.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItem_workEnd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    lciWorkStart.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lciWorkEnd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    gridColumn_work_BATCH.Visible = false;
                    BU_YN.Visible = false;
                }
                else
                {
                    layoutControlItem_workStart.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem_workEnd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    lciWorkStart.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lciWorkEnd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    gridColumn_work_BATCH.Visible = true;
                    BU_YN.Visible = true;
                }

                gridView_batchRun.OptionsCustomization.AllowSort = false;

                authDs = clsSql.GetAuthDataSet(formname);

                //gridView_work.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;

                //// 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false);
                cboPlant_Code.EditValue = vPlant_Code;

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                //작업모드 콤보박스 셋팅
                cboOperWorkMode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                cboOperWorkMode.Properties.BeginUpdate();
                cboOperWorkMode.Properties.Items.Add("단동");
                cboOperWorkMode.Properties.Items.Add("연동");
                cboOperWorkMode.Properties.EndUpdate();
                cboOperWorkMode.SelectedIndex = 0;

                this.ActiveControl = btn_workSearch;

                vWorkStart = false;

                work_watch_timer.Interval = 5000;
                work_watch_timer.Enabled = true;

                //if (clsCommon._strPlcConnYn == "Y")
                //{
                //    clsMelsec.plc_dosing.ActHostAddress = clsCommon.GetPLCIP("도징").Rows[0]["PLCIP"].ToString();
                //    clsMelsec.plc_dosing.ActStationNumber = 1;
                //    clsMelsec.plc_dosing.ActPortNumber = 5002;
                //    clsMelsec.plc_dosing.ActSourceStationNumber = 2;
                //    clsMelsec.plc_dosing.ActTimeOut = 2000;

                //    if (clsMelsec.plc_dosing.Open() != 0)
                //    {
                //        MAIN.PlcConnChk = "N";
                //        work_watch_timer.Enabled = false;
                //        ShowMessageBox.XtraShowWarning("PLC 1번 접속을 실패하였습니다");
                //    }
                //    else
                //    {
                //        MAIN.PlcConnChk = "Y";
                //        work_watch_timer.Interval = 3000;
                //        work_watch_timer.Enabled = true;
                //    }
                //}
                //else
                //{
                //    work_watch_timer.Enabled = false;
                //}

                //작업조회
                XMain_Search();

                InitControl();

                isInitializing = true;         // 초기화 완료

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl_work));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_Dosing_Load", ex);
            }
        }

        private void InitControl()
        {
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "배합비 버전이 없습니다.", false);

            clsDevexpressUtil.ItemLookUpEditSetup(cboDelYn, clsCommon.GetYn(null, new string[] { "삭제", "미삭제" }), "전체선택", false, 2, true);

            gridCHK.ValueChecked = "Y";
            gridCHK.ValueUnchecked = "N";
            gridCHK.NullStyle = StyleIndeterminate.Unchecked;
            gridCHK.CheckStyle = CheckStyles.Standard;

            // 031002	배합공정
            //목적빈
            repItemLkUpEdit_T_BIN.TextEditStyle = TextEditStyles.Standard;
            repItemLkUpEdit_T_BIN.NullText = ""; // 선택 전 빈 값 허용
            repItemLkUpEdit_T_BIN.NullValuePrompt = "";
            repItemLkUpEdit_T_BIN.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch;
            repItemLkUpEdit_T_BIN.TextEditStyle = TextEditStyles.DisableTextEditor;
            clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_T_BIN, clsCommon.GetBin(cboPlant_Code.EditValue?.ToString(), "", ""), "", true, true, null, null, "CODE", "CODE");


            // 교대조
            gridcboICM_CODE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboICM_CODE, clsCommon.GetICM(), "교대조를 선택 해주세요.", false, false);

            //작업자정보
            repItemLkUpEdit_EMPLOYEE_NO.NullValuePrompt = "";
            repItemLkUpEdit_EMPLOYEE_NO.NullText = "";
            clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_EMPLOYEE_NO, clsCommon.GetDO_INSA(cboPlant_Code.EditValue?.ToString()));

            //계획상태
            /*
            031001	전체
            031002	계획
            031003	진행
            031004	완료
            031005	취소
            031006	강제완료
            031007	보류
             */
            clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_C_CONDITION, clsCommon.GetPcStatus());

            clsDevexpressGrid.ItemLookUpEditSetup(gridCboPackType, clsCommon.GetPackType(), "", false, false);
        }
        #endregion

        #region 작업지시 조회
        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="vProcess_Code"></param>
        /// <param name="work_status"></param>
        private void XMain_Search()
        {
            try
            {
                txtRemark.EditValue = "";

                gridView_work.ShowLoadingPanel();

                string sSelectAdd = string.Empty;

                string sLeftJoinAdd = string.Empty;

                if (cboProcessKey.EditValue?.ToString() == clsCommon.GetProcessKey("배합"))
                {
                    sSelectAdd = @" , c.B_W, c.END_TIME
                    , c.MESH_10, c.MESH_18, c.MESH_35
                    , c.MESH_6, c.MESH_8, c.MESH_PAN 
                    , c.PROD_QTY, c.PROD_TIME, c.PROTY
                    , c.START_TIME, c.STOP_TIME
                    ";

                    sLeftJoinAdd = @"LEFT JOIN WORK_ORDER_ADD c ON c.PLANT_CODE = a.PLANT_CODE AND c.PROCESS_KEY = a.PROCESS_KEY AND c.L_CODE = a.L_CODE
                                                AND c.WORKDATE = a.WORKDATE AND c.NUM = a.NUM";
                }
                else if (cboProcessKey.EditValue?.ToString() == clsCommon.GetProcessKey("후레이크"))
                {
                    sSelectAdd = @", c.BLOWER_TEMP, c.COOKING_1ST, c.COOKING_2ND
                    , c.COOKING_PRESSURE, c.DENSITY, c.END_TIME
                    , c.EXHAUST_HUMIDITY, c.EXHAUST_TEMP, c.INLET_TEMP
                    , c.L_CODE, c.LOWER_TEMP_1, c.LOWER_TEMP_2
                    , c.LOWER_TEMP_3, c.MOISTURE_PCT
                    , c.PROD_QTY, c.THICKNESS_MM
                    , c.PROD_TIME, c.PRODUCTIVITY, c.START_TIME
                    , c.STEAM_PRESSURE, c.STEAM_PRESSURE_2, c.STOP_TIME
                    ";

                    sLeftJoinAdd = @"LEFT JOIN FLAKE_REPORT_ADD c ON c.PLANT_CODE = a.PLANT_CODE AND c.PROCESS_KEY = a.PROCESS_KEY AND c.L_CODE = a.L_CODE
                                                AND c.WORKDATE = a.WORKDATE AND c.NUM = a.NUM";
                }

                if (cboProcessKey.EditValue?.ToString() == clsCommon.GetProcessKey("익스콘"))
                {
                    sSelectAdd = @", c.BARREL_TEMP, c.CONDITION_TEMP, c.DENSITY
                    , c.DRYER_TEMP, c.END_TIME, c.F_R_RATIO
                    , c.I_TIME, c.PROD_QTY, c.STOP_TIME
                    , c.PROD_TIME, c.PRODUCTIVITY, c.START_TIME
                    ";

                    sLeftJoinAdd = @"LEFT JOIN EXCON_REPORT_ADD c ON c.PLANT_CODE = a.PLANT_CODE AND c.PROCESS_KEY = a.PROCESS_KEY AND c.L_CODE = a.L_CODE
                                                AND c.WORKDATE = a.WORKDATE AND c.NUM = a.NUM";
                }

                SQL = $@"
                SELECT 
                      a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.NUM        -- 1
                    , a.P_TYPE , a.RESOURCE_NO , a.NOTE                               -- 2
                    , a.WORK_START_DATE, a.BATCH, a.R_BATCH, a.BATCH_Q, a.OR_Q        -- 3
                    , a.PRO_Q, a.BBATCH_Q, a.GUBUN, a.LOCATION_ED, a.LOCATION_ED2     -- 4
                    , a.REMARK, a.ICM_CODE, a.C_CONDITION, a.BU_YN, a.TRANS_YN        -- 5
                    , a.BAD_CODE1, a.BAD_QTY1, a.BAD_CODE2, a.BAD_QTY2                -- 6
                    , a.BAD_CODE3, a.BAD_QTY3, a.BAD_CODE4, a.BAD_QTY4                -- 7
                    , a.BAD_CODE5, a.BAD_QTY5                                         -- 8
                    , a.I_TIME, a.EMPLOYEE_NO                                         -- 9
                    -- // GBG
                    , a.START_TIME, a.END_TIME, a.ERP_OSTATUS, a.ERP_ISTATUS
                    -- // GBG -
                    , CASE WHEN (a.END_TIME - a.START_TIME) * 24 * 60 > 0 
                            THEN FLOOR((a.PRO_Q / ((a.END_TIME - a.START_TIME) * 24 * 60)) * 60) 
                            ELSE 0 
                        END AS PROVITT
                    , a.PACK_TYPE
                    , b.PRESSURE
                    , b.COOKING1
                    , b.COOKING2
                    , b.TEMP_UPPER
                    , b.TEMP_LOWER
                    , b.STEAM_INPUT
                    , b.BEFORE_INPUT
                    , b.AFTER_INPUT
                    , b.FEEDER
                    , b.ROLL_GAP_LEFT
                    , b.ROLL_RPM_LEFT
                    , b.ROLL_GAP_RIGHT
                    , b.ROLL_RPM_RIGHT
                    , a.OR_Q + ((a.BATCH_Q * cp.PART_P / 100) * a.BATCH) AS SUM_OR_Q
                    {sSelectAdd}
                FROM WORK_ORDER a
                    LEFT JOIN FLAKE_REPORT b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE AND b.WORKDATE = a.WORKDATE AND b.NUM = a.NUM
                    LEFT JOIN SAP_IN_UPPRODUCT_CP cp ON cp.PLANT_CODE = a.PLANT_CODE AND cp.RESOURCE_NO = a.RESOURCE_NO
                    {sLeftJoinAdd}
                WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}'
                    AND a.PROCESS_KEY = '{cboProcessKey.EditValue?.ToString()}'
                    AND a.L_CODE = '{cboL_Code.EditValue?.ToString()}'
                    AND ('{cboDelYn.EditValue}' IS NULL OR NVL(a.DEL_FLAG,'N') = '{cboDelYn.EditValue}')
                    AND a.WORKDATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                ORDER BY INSTR('{clsCommon.PcStatus.Plan},{clsCommon.GetPcStatusCode("진행")},{clsCommon.PcStatus.Completed},{clsCommon.PcStatus.ForceCompleted},{clsCommon.PcStatus.Canceled}', a.C_CONDITION), CASE 
                                                                                WHEN a.END_TIME IS NOT NULL THEN TO_NUMBER(TO_CHAR(a.END_TIME, 'YYYYMMDDHH24MISS'))
                                                                                ELSE a.NUM
                                                                            END DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl_work, gridView_work, ds.Tables[0], false, true);

                gridView_work.SetFixCol(new string[] {  "WORKDATE"
                    , "NUM"
                    , "RESOURCE_NO"
                    , "NOTE" });

                sValid = new string[] { "PLANT_CODE", "PROCESS_KEY", "L_CODE", "WORKDATE", "P_TYPE", "RESOURCE_NO", "NOTE", "LOCATION_ED", "EMPLOYEE_NO" };

                for (int i = 0; i < sValid.Length; i++)
                {
                    GridColumn col = gridView_work.Columns[sValid[i]];
                    if (col != null)
                    {
                        col.OptionsColumn.ShowCaption = true; // 캡션 보이게
                        col.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.edit_16x16;
                        col.ImageOptions.Alignment = StringAlignment.Near;   // 아이콘을 캡션 왼쪽 정렬
                    }
                }

                string[] hiddenCols = { "PRESSURE"
                                    , "COOKING1"
                                    , "COOKING2"
                                    , "TEMP_UPPER"
                                    , "TEMP_LOWER"
                                    , "STEAM_INPUT"
                                    , "BEFORE_INPUT"
                                    , "AFTER_INPUT"
                                    , "FEEDER"
                                    , "ROLL_GAP_LEFT"
                                    , "ROLL_RPM_LEFT"
                                    , "ROLL_GAP_RIGHT"
                                    , "ROLL_RPM_RIGHT"
                };

                if (cboProcessKey.EditValue?.ToString() == clsCommon.GetProcessKey("후레이크", cboPlant_Code.EditValue?.ToString()))
                {
                    foreach (string colName in hiddenCols)
                    {
                        if (gridView_work.Columns[colName] != null)
                        {
                            gridView_work.Columns[colName].Visible = true;
                            gridView_work.Columns[colName].OptionsColumn.ShowInCustomizationForm = true;
                        }
                    }
                }

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 2, true, false, "KG", false, true), "품목을 선택 해주세요.", false);
                gridscboRESOURCE_NO.PopupFormSize = new Size(400, 400); // 가로 500, 세로 300
                DevExpress.XtraGrid.Views.Grid.GridView popupView = gridscboRESOURCE_NO.View as DevExpress.XtraGrid.Views.Grid.GridView;
                popupView.Columns["NAME"].Width = 180;

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboNOTE, clsCommon.getNote(cboPlant_Code.EditValue?.ToString(), ""), "배합비 버전이 없습니다.", false);

                MAIN.wStatus = true;

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (gridView_work.LoadingPanelVisible)
                {
                    gridView_work.HideLoadingPanel();
                }
            }
        }
        #endregion

        private void plcConn_chk()
        {
            // 172.43.218.22
            //EthernetConnector connector = new EthernetConnector(new TcpSocket(clsCommon.plc_dosing_ip, 10260), false);
            try
            {
                //PLC 연결
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("PLC통신 연결체크중입니다");

                //Task<bool> returnTaskResult = clsPlcConnManager.Plc_conn(connector);
                //bool connResult = await returnTaskResult;

                //if (!connResult)
                //{
                //    ShowMessageBox.XtraShowInformation("PLC 연결에 실패했습니다");
                //    return;
                //}
            }
            catch (Exception)
            {

            }
            finally
            {
                //if (connector.IsConnected)
                //{
                //    connector.Disconnect();
                //}

                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        #region 세로모드 체크박스 클릭
        private void checkEdit_Horizontal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit_Horizontal.Checked)
            {
                splitContainerControl.Horizontal = true;
            }
            else
            {
                splitContainerControl.Horizontal = false;
            }
        }
        #endregion

        #region 작업지시 조회 함수
        /// <summary>
        /// 조회 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_workSearch_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }
        #endregion

        /// <summary>
        /// 날짜변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateEdit_workDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
                return;

            XMain_Search();
        }

        /// <summary>
        /// 진행상태 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEdit_workStatus_EditValueChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
                return;

            XMain_Search();
        }

        #region 작업추가 버튼 클릭 이벤트
        private void btn_workAdd_Click(object sender, EventArgs e)
        {
            MAIN.wStatus = false;
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            // 031002	배합공정
            //if (clsCommon._strPlcConnYn != "Y" && clsCommon.dosing_process_code == vProcess_Code)
            //{
            //    ShowMessageBox.XtraShowInformation("사용권한이 업습니다\r\n제어실에서만 동작가능합니다");
            //    return;
            //}

            try
            {
                vWorkStart = false;

                NewRow();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_workAdd_Click", ex);
            }
        }

        /// <summary>
        /// 작업복사 버튼 이벤트
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

            // 031002	배합공정
            //if (clsCommon._strPlcConnYn != "Y" && clsCommon.dosing_process_code == vProcess_Code)
            //{
            //    ShowMessageBox.XtraShowInformation("사용권한이 업습니다\r\n제어실에서만 동작가능합니다");
            //    return;
            //}


            try
            {
                NewRowCopy();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_workAdd_Click", ex);
            }
        }

        private void NewRow()
        {
            DataTable dt = gridControl_work.DataSource as DataTable;

            if (dt != null)
            {
                string sWorkDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);

                if (!new string[] { "PJ04", "PJ05" }.Contains(cboPlant_Code.EditValue?.ToString()) && sWorkDate != string.Format("{0:yyyyMMdd}", DateTime.Now))
                {
                    DialogResult result = ShowMessageBox.Confirm("현재일자와 작업일자가 상이 합니다. 이대로 진행 하시겠습니까?");

                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                DataRow newRow = dt.NewRow();
                newRow["PLANT_CODE"] = cboPlant_Code.EditValue?.ToString();
                newRow["PROCESS_KEY"] = cboProcessKey.EditValue?.ToString();
                newRow["L_CODE"] = cboL_Code.EditValue?.ToString();
                newRow["WORKDATE"] = sWorkDate;
                newRow["P_TYPE"] = "2";

                // GBG
                // 하림-김제의 경우 작업 추가 시 자동으로 배치량을 4200으로 등록 되도록 요청
                if (clsCommon.PlantCode == "P101")
                {
                    newRow["BATCH_Q"] = 4200;     // 배치 량
                }
                else if (clsCommon.PlantCode == "P201")
                {
                    newRow["BATCH_Q"] = 5000;     // 배치 량
                    newRow["BATCH"] = 5;
                    newRow["OR_Q"] = 25000;
                }
                else
                {
                    newRow["BATCH"] = 1;
                }

                newRow["R_BATCH"] = 0;
                newRow["EMPLOYEE_NO"] = clsCommon.UserId;

                newRow["ICM_CODE"] = clsCommon.GetICMGubun();
                newRow["C_CONDITION"] = "미입력";
                newRow["PACK_TYPE"] = "031401";
                //newRow["NUM"] = workNumMake(cboProcessKey.EditValue?.ToString(), sWorkDate);
                dt.Rows.InsertAt(newRow, 0); // 무조건 맨 위에 삽입
                gridView_work.FocusedRowHandle = 0;
                gridView_work.TopRowIndex = 0;
            }
        }

        private void NewRowCopy()
        {
            DataTable dt = gridControl_work.DataSource as DataTable;

            DataRow dr = gridView_work.GetFocusedDataRow();

            if (dt != null)
            {
                string sWorkDate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);

                DataRow newRow = dt.NewRow();
                newRow["PLANT_CODE"] = dr["PLANT_CODE"];
                newRow["PROCESS_KEY"] = dr["PROCESS_KEY"];
                newRow["L_CODE"] = dr["L_CODE"];
                newRow["WORKDATE"] = dr["WORKDATE"];
                newRow["P_TYPE"] = "2";

                newRow["RESOURCE_NO"] = dr["RESOURCE_NO"];
                newRow["NOTE"] = dr["NOTE"];
                newRow["BATCH_Q"] = dr["BATCH_Q"];
                newRow["BU_YN"] = dr["BU_YN"];
                newRow["LOCATION_ED"] = dr["LOCATION_ED"];

                // GBG
                // 하림-김제의 경우 작업 추가 시 자동으로 배치량을 4200으로 등록 되도록 요청
                if (clsCommon.PlantCode == "P101")
                {
                    newRow["BATCH_Q"] = 4200;     // 배치 량
                }

                // GBG - 

                newRow["BATCH"] = dr["BATCH"];
                newRow["R_BATCH"] = 0;
                newRow["OR_Q"] = dr["OR_Q"];
                newRow["EMPLOYEE_NO"] = clsCommon.UserId;

                newRow["ICM_CODE"] = clsCommon.GetICMGubun();
                newRow["C_CONDITION"] = "미입력";
                newRow["PACK_TYPE"] = dr["PACK_TYPE"];
                //newRow["NUM"] = workNumMake(cboProcessKey.EditValue?.ToString(), sWorkDate);
                dt.Rows.InsertAt(newRow, 0); // 무조건 맨 위에 삽입
            }
        }

        #endregion

        #region 작업삭제 버튼 클릭 이벤트
        private void btn_workDelete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            // 주동석
            //// 031002	배합공정
            //if (clsCommon._strPlcConnYn != "Y" && clsCommon.dosing_process_code == vProcess_Code)
            //{
            //    ShowMessageBox.XtraShowInformation("사용권한이 업습니다\r\n제어실에서만 동작가능합니다");
            //    return;
            //}

            XMain_Delete();
        }

        private void XMain_Delete()
        {
            try
            {
                if (gridView_work.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("삭제하실 작업지시를 선택하여 주세요");
                    return;
                }

                DataRow row = gridView_work.GetDataRow(gridView_work.FocusedRowHandle);

                if (row.RowState == DataRowState.Added)
                {
                    gridView_work.DeleteRow(gridView_work.FocusedRowHandle);
                }
                else
                {
                    string condition = gridView_work.GetRowCellDisplayText(gridView_work.FocusedRowHandle, gridView_work.Columns["C_CONDITION"]);

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

                    string PLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
                    string PROCESS_KEY = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
                    string L_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");

                    string WORKDATE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
                    string work_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");
                    string work_batch = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH");

                    string RESOURCE_NO = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO");

                    string LOCATION_ED = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "LOCATION_ED");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업순번 " + work_num + " 작업지시를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        splashScreenManager.ShowWaitForm();

                        Dbconn.conn.BeginTransaction();
                        SQL = $@"
                        -- 작업 삭제
                        UPDATE WORK_ORDER SET DEL_FLAG = 'Y'
                            , ERP_OSTATUS = 'X', ERP_ISTATUS = 'X'
                        WHERE PLANT_CODE = '{PLANT_CODE}' AND PROCESS_KEY = '{PROCESS_KEY}' AND L_CODE = '{L_CODE}'
                            AND WORKDATE = '{WORKDATE}' AND NUM = '{work_num}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_workDelete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                            return;
                        }

                        //string pelletProcess = string.Empty;

                        //clsCommon.GetAutoPellet(PLANT_CODE, PROCESS_KEY, RESOURCE_NO, out pelletProcess);

                        //// 가루 외 원료 일 때만 펠렛 작업지시 생성
                        //if (!string.IsNullOrEmpty(pelletProcess))
                        //{
                        //    string SQL3 = $@"
                        //    UPDATE PELLET_REPORT
                        //    SET DEL_FLAG = 'Y', ERP_UP_YN = 'X'
                        //    WHERE BF_PLANT_CODE = '{PLANT_CODE}'      -- 11
                        //        AND BF_PROCESS_KEY = '{PROCESS_KEY}'    -- 12
                        //        AND BF_L_CODE = '{L_CODE}'              -- 13
                        //        AND BF_WORKDATE = '{WORKDATE}'          -- 14
                        //        AND BF_NUM = '{work_num}'               -- 15
                        //    ";

                        //    if (Dbconn.conn.SQLrun(SQL3) < 1)
                        //    {
                        //        Dbconn.conn.Rollback();
                        //        clsLog.logSave("clsProcessDosing", "InsertWorkNum", SQL3);
                        //        ShowMessageBox.XtraShowWarning("펠렛 작업지시 입력을 실패했습니다");
                        //    }
                        //}

                        Dbconn.conn.Commit();
                        XMain_Search();

                    }
                }
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_workDelete_Click", ex);
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
        #endregion

        #region 작업지시 그리드 셀변경 이벤트

        private void gridView_work_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
        }

        private void gridView_work_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            //지시량 자동계산 (배치수 * 배치량 = 지시량) 
            try
            {
                if (e.Column.FieldName == "BATCH_Q")        // 배치량
                {
                    int sBATCH_Q = 0;

                    string sPlantCode = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["PLANT_CODE"]).ToString() : "";
                    string sProcessKey = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["PROCESS_KEY"]).ToString() : "";
                    string sLCode = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["L_CODE"]).ToString() : "";
                    string sResource_NO = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["RESOURCE_NO"]).ToString() : "";
                    string sNOTE = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["NOTE"]).ToString() : "";
                    string sBU_YN = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["BU_YN"]).ToString() : "";

                    if (view.GetRowCellValue(e.RowHandle, view.Columns["BATCH"]) != null && !string.IsNullOrEmpty(view.GetRowCellValue(e.RowHandle, view.Columns["BATCH"]).ToString()))     // 배치수
                    {
                        sBATCH_Q = Convert.ToInt32(e.Value.ToString());
                        int batch = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, view.Columns["BATCH"]));

                        SQL = $@"
                        SELECT SUM(a.PART_P) AS MENGE, 'Y' AS BU_YN
                        FROM SAP_IN_PRODUCT_RC a
                        WHERE a.PLANT_CODE = '{sPlantCode}'
                            AND a.RESOURCE_NO = '{sResource_NO}'
                        ";

                        DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                        if (sBU_YN == "Y")
                            vdMerge = decimal.Parse(Dbconn.conn.getData(ds, "MENGE", 0));
                        else
                            vdMerge = 0;

                        view.SetRowCellValue(e.RowHandle, view.Columns["OR_Q"], (sBATCH_Q * batch + (sBATCH_Q * vdMerge * batch / 100)));     // 지시량 = 배치수 * 배치량 + (배치수 * 부산물량 * 배치량 / 100)

                        SQL = $@"
                        SELECT {sBATCH_Q * batch + (sBATCH_Q * vdMerge * batch / 100)} + {sBATCH_Q} * (a.PART_P / 100) AS MENGE
                        FROM SAP_IN_UPPRODUCT_CP a
                        WHERE a.PLANT_CODE = '{sPlantCode}'
                            AND a.RESOURCE_NO = '{sResource_NO}'
                        ";

                        ds = Dbconn.conn.ExecutDataset(SQL);

                        view.SetRowCellValue(e.RowHandle, view.Columns["SUM_OR_Q"], Dbconn.conn.getData(ds, "MENGE", 0));
                    }

                    mix_result(sPlantCode, sProcessKey, sLCode, sResource_NO, sNOTE, sBATCH_Q.ToString(), chk_version, sBU_YN);

                }

                if (e.Column.FieldName == "BATCH")      // 배치수
                {
                    string sPlantCode = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["PLANT_CODE"]).ToString() : "";
                    string sProcessKey = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["PROCESS_KEY"]).ToString() : "";
                    string sLCode = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["L_CODE"]).ToString() : "";
                    string sResource_NO = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["RESOURCE_NO"]).ToString() : "";
                    string sNOTE = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["NOTE"]).ToString() : "";
                    string sBU_YN = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["BU_YN"]).ToString() : "";

                    if (view.GetRowCellValue(e.RowHandle, view.Columns["BATCH_Q"]) != null && !string.IsNullOrEmpty(view.GetRowCellValue(e.RowHandle, view.Columns["BATCH_Q"]).ToString()))       // 배치량
                    {
                        int batch = Convert.ToInt32(e.Value.ToString());
                        int sBATCH_Q = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, view.Columns["BATCH_Q"]));

                        SQL = $@"
                        SELECT SUM(a.PART_P) AS MENGE, 'Y' AS BU_YN
                        FROM SAP_IN_PRODUCT_RC a
                        WHERE a.PLANT_CODE = '{sPlantCode}'
                            AND a.RESOURCE_NO = '{sResource_NO}'
                        ";

                        DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                        if (sBU_YN == "Y")
                            vdMerge = decimal.Parse(Dbconn.conn.getData(ds, "MENGE", 0));
                        else
                            vdMerge = 0;

                        gridView_work.SetRowCellValue(e.RowHandle, view.Columns["OR_Q"], ((sBATCH_Q * batch) + (sBATCH_Q * vdMerge * batch / 100)));

                        SQL = $@"
                        -- 작업지시량
                        SELECT {sBATCH_Q * batch + (sBATCH_Q * vdMerge * batch / 100)} + {sBATCH_Q} * (a.PART_P / 100) AS MENGE
                        FROM SAP_IN_UPPRODUCT_CP a
                        WHERE a.PLANT_CODE = '{sPlantCode}'
                            AND a.RESOURCE_NO = '{sResource_NO}'
                        ";

                        ds = Dbconn.conn.ExecutDataset(SQL);

                        view.SetRowCellValue(e.RowHandle, view.Columns["SUM_OR_Q"], Dbconn.conn.getData(ds, "MENGE", 0));
                    }

                    if (view.GetRowCellValue(e.RowHandle, view.Columns["C_CONDITION"]) != null && view.GetRowCellValue(e.RowHandle, view.Columns["C_CONDITION"]).ToString() == clsCommon.GetPcStatusCode("진행"))      // 031003	진행
                    {
                        int batch = Convert.ToInt32(e.Value.ToString());
                        int r_batch = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, view.Columns["R_BATCH"]));      // 현재 배치수

                        if (batch < r_batch)
                        {
                            ShowMessageBox.XtraShowInformation("진행중인 배치보다 배치수를 작게 설정못합니다");
                            view.CancelUpdateCurrentRow();
                            view.SetRowCellValue(e.RowHandle, view.Columns["BATCH"], view.GetRowCellValue(e.RowHandle, view.Columns["BATCH"]));
                        }
                    }
                }

                if (e.Column.FieldName == "TRANS_YN")
                {
                    bTransFlag = true;
                }

                if (e.Column.FieldName == "LOCATION_ED" && vProcess_Code != "FBP010")      // 배치수
                {
                    if (view.GetFocusedRowCellValue("LOCATION_ED")?.ToString() != "")
                    {
                        DataTable dt = clsCommon.GetBin(view.GetRowCellValue(e.RowHandle, view.Columns["PLANT_CODE"]).ToString(), "", "", "", "", e.Value.ToString());

                        if (dt == null || dt.Rows.Count == 0)
                        {
                            if (DialogResult.Yes != ShowMessageBox.Confirm("빈정보에 등록된 빈이 아닙니다 계속 진행하시겠습니까?"))
                            {
                                view.SetFocusedRowCellValue("LOCATION_ED", "");
                            }
                        }
                    }
                }
            }
            catch
            {
                view.SetRowCellValue(e.RowHandle, view.Columns["OR_Q"], 0);     // 지시량
            }
        }

        #endregion

        #region 작업지시 그리드 에디터표시 이벤트
        private void gridView_work_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (gridView_work.IsNewItemRow(gridView_work.FocusedRowHandle))
                {
                    // 필요 시 편집 자체를 막고 싶다면:
                    return;
                }

                // 031004	완료
                if (gridView_work.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.Completed)
                    || gridView_work.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.ForceCompleted)
                    || gridView_work.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.Canceled)) //작지가 완료처리된것은 수정못하도록 에디트모드 off
                {
                    if (clsCommon.Auth_Form_Function(authDs, "U"))
                    {
                        switch (gridView_work.FocusedColumn.FieldName)
                        {
                            case "ICM_CODE":
                            case "LOCATION_ED":
                            case "LOCATION_ED2":
                            case "TRANS_YN":
                                e.Cancel = false;
                                break;
                            default:
                                e.Cancel = true;
                                break;
                        }
                    }
                    else
                        if (gridView_work.FocusedColumn.FieldName != "TRANS_YN")
                        e.Cancel = true;        // 수정 불가
                }
                // 031003	진행
                else if (gridView_work.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.InProgress))  //작지가 진행중일 경우 배치수만 수정가능하도록 변경
                {
                    switch (gridView_work.FocusedColumn.FieldName)
                    {
                        case "RESOURCE_NO":
                        case "NOTE":
                        case "BATCH_Q":
                        case "LOCATION_ED":
                        case "LOCATION_ED2":
                        case "BU_YN":
                            e.Cancel = true;
                            break;

                        default:
                            e.Cancel = false;
                            break;
                    }
                }
                else if (gridView_work.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.Plan))  //작지가 진행중일 경우 배치수만 수정가능하도록 변경
                {
                    switch (gridView_work.FocusedColumn.FieldName)
                    {
                        case "RESOURCE_NO":
                        case "NOTE":
                        case "BU_YN":
                            e.Cancel = true;
                            break;
                        case "PRO_Q":
                            if (aPassProcess.Contains(vProcess_Code))
                                e.Cancel = false;
                            else
                                e.Cancel = true;
                            break;
                        default:
                            e.Cancel = false;
                            break;
                    }
                }
                else if (clsCommon._strMainPlcConnYn != "Y" && clsCommon._strSubPlcConnYn != "Y" && cboProcessKey.EditValue?.ToString() == clsCommon.GetProcessKey("배합"))       // 031002	배합공정
                {
                    // e.Cancel = true;
                }
                else
                {
                    switch (gridView_work.FocusedColumn.FieldName)
                    {
                        case "PRO_Q":
                            e.Cancel = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_work_ShowingEditor", ex);
            }
        }
        #endregion

        private void XMain_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView_work);

                DataTable DT = (DataTable)gridControl_work.DataSource;

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
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView_work);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView_work.FocusedColumn = gridView_work.Columns[rValid]; // 이동할 컬럼명
                        gridView_work.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    string return_msg;
                    if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified)
                    {
                        // 031003	진행
                        if (dr["C_CONDITION"].ToString() != clsCommon.GetPcStatusCode("진행"))
                        {
                            //input check
                            if (string.IsNullOrEmpty(dr["RESOURCE_NO"].ToString()))     // 레시피 넘버
                            {
                                dr.SetColumnError("RESOURCE_NO", "배합제품을 선택하여주세요");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("RESOURCE_NO"));
                                return;
                            }

                            if (string.IsNullOrEmpty(dr["NOTE"].ToString()) || dr["NOTE"].ToString().Contains("레시피가 없습니다"))
                            {
                                dr.SetColumnError("NOTE", "레시피를 선택하여주세요");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("NOTE"));
                                return;
                            }


                            if (string.IsNullOrEmpty(dr["BATCH_Q"].ToString()))
                            {
                                dr.SetColumnError("BATCH_Q", "배치량을 입력하여 주세요");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                                return;
                            }

                            // GBG
                            //if (int.Parse(dr["BATCH_Q"].ToString()) >= Int16.MaxValue)
                            //{
                            //    dr.SetColumnError("BATCH_Q", "배치량이 최대치인 32768을 초과 했습니다.");
                            //    ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                            //    return;
                            //}
                            // GBG -

                            if (Convert.ToInt32(dr["BATCH_Q"]) <= 0)
                            {
                                dr.SetColumnError("BATCH_Q", "배치량을 0이상을 입력하여 주세요");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                                return;
                            }

                            //// 031002	배합공정
                            //if (vProcess_Code == clsCommon.GetProcessKey("배합"))
                            //{
                            //    if (Convert.ToInt16(dr["BATCH_Q"]) < 3000)
                            //    {
                            //        dr.SetColumnError("BATCH_Q", "배치량을 최소 3000KG이상을 입력하여 주세요");
                            //        ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                            //        return;
                            //    }

                            //    if (Convert.ToInt16(dr["BATCH_Q"]) > 5000)
                            //    {
                            //        dr.SetColumnError("BATCH_Q", "배치량을 최대 5000KG이하로 입력하여 주세요");
                            //        ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                            //        return;
                            //    }
                            //}
                            //else if (vProcess_Code != clsCommon.GetProcessKey("배합"))     // P05	대용유배합공정
                            //{
                            //    if (Convert.ToInt16(dr["BATCH_Q"]) > 1000)
                            //    {
                            //        dr.SetColumnError("BATCH_Q", "배치량을 최대 1000KG이하로 입력하여 주세요");
                            //        ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                            //        return;
                            //    }
                            //}

                            //if (vProcess_Code == clsCommon.GetProcessKey("배합"))
                            //{
                            //    if (Convert.ToInt16(dr["BATCH_Q"]) < 3000)
                            //    {
                            //        dr.SetColumnError("BATCH_Q", "배치량을 최소 3000KG이상을 입력하여 주세요");
                            //        ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                            //        return;
                            //    }
                            //}

                            if (string.IsNullOrEmpty(dr["BATCH"].ToString()))
                            {
                                dr.SetColumnError("BATCH", "배치수를 입력하여 주세요");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH"));
                                return;
                            }

                            if (Convert.ToInt32(dr["BATCH"]) <= 0)
                            {
                                dr.SetColumnError("BATCH", "배치수를 1이상 입력하여 주세요");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH"));
                                return;
                            }

                            if (string.IsNullOrEmpty(dr["LOCATION_ED"].ToString()))
                            {
                                dr.SetColumnError("LOCATION_ED", "목적빈을 선택하여 주세요");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("LOCATION_ED"));
                                return;
                            }
                        }

                        if (dr.RowState == DataRowState.Modified)
                        {
                            if (bTransFlag)
                            {
                                SQL = $@"   
                                -- 배합작업 전송여부
                                UPDATE WORK_ORDER
                                SET TRANS_YN = '{dr["TRANS_YN"]}'
                                WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"]}'
                                    AND WORKDATE = '{dr["WORKDATE"]}' AND NUM = '{dr["NUM"]}'
                                ";

                                if (Dbconn.conn.SQLrun(SQL) < 1)
                                {
                                    Dbconn.conn.Rollback();
                                    ShowMessageBox.XtraShowInformation("전송 불가 업데이트 도중 오류가 발생했습니다.");
                                    clsLog.logSave("frm_Dosing", "btn_workSave_Click", SQL);
                                    return;
                                }

                                continue;
                            }

                            //진행중인 작지를 수정했을 경우
                            if (dr["C_CONDITION"].ToString() == clsCommon.GetPcStatusCode("진행"))       // 031003	진행
                            {
                                if (!aPassProcess.Contains(vProcess_Code))
                                {
                                    int batch = Convert.ToInt16(dr["BATCH"]);
                                    int r_batch = Convert.ToInt16(dr["R_BATCH"]);
                                    int sBATCH_Q = Convert.ToInt16(dr["BATCH_Q"]);

                                    if (batch < r_batch)
                                    {
                                        dr.SetColumnError("BATCH", "진행중인 배치보다 배치수를 작게 설정못합니다");
                                        ShowMessageBox.XtraShowInformation(dr.GetColumnError("BATCH"));
                                        return;
                                    }

                                    SQL = $@"
                                    SELECT BATCH FROM WORK_ORDER 
                                    WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"].ToString().Replace(dr["PLANT_CODE"].ToString(), "")}'
                                    AND WORKDATE = '{dr["WORKDATE"]}' AND NUM = '{dr["NUM"]}'
                                    ";

                                    DataSet tmpDs = Dbconn.conn.ExecutDataset(SQL);

                                    if (Dbconn.conn.getRowCnt(tmpDs) == 1)
                                    {
                                        if (!Dbconn.conn.getData(tmpDs, "BATCH", 0).Equals(batch))
                                        {
                                            //PLC에 배치변경 전송
                                            try
                                            {
                                                int[] pWorkNum = new int[15]; // 작업지시 정보전송 배열

                                                DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

                                                if (!PlcFunc.GetPlcCon(dtPlc, out MAIN.sErrMsg))
                                                {
                                                    ShowMessageBox.XtraShowInformation(MAIN.sErrMsg);
                                                    return;
                                                }

                                                vPLC_Location = clsCommon.GetPlcLocation(vPlant_Code, cboProcessKey.EditValue?.ToString());

                                                clsCommon.GetPLCAddress(dr["PLANT_CODE"]?.ToString()
                                                                , dr["PROCESS_KEY"]?.ToString()
                                                                , dr["L_CODE"]?.ToString()
                                                                , vPLC_Location
                                                                , PlcAddressType.CURRENTBATCH.GetDesc()
                                                                , 1
                                                                , out vPLCAddress
                                                                , out vPLCUnit
                                                                , out vPLCDataCount);

                                                if (!PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref pWorkNum, "현재 배치수를 가져오는데 실패했습니다."))
                                                {
                                                    return;
                                                }

                                                if (pWorkNum[0] <= batch)
                                                {
                                                    clsCommon.GetPLCAddress(dr["PLANT_CODE"]?.ToString()
                                                                , dr["PROCESS_KEY"]?.ToString()
                                                                , dr["L_CODE"]?.ToString()
                                                                , vPLC_Location
                                                                , PlcAddressType.BATCHCOUNT.GetDesc()
                                                                , 1
                                                                , out vPLCAddress
                                                                , out vPLCUnit
                                                                , out vPLCDataCount);

                                                    var (device, number) = vPLCAddress.SplitDeviceAndNumber();

                                                    Array.Clear(pWorkNum, 0, pWorkNum.Length);

                                                    pWorkNum[0] = batch;

                                                    if (!PlcFunc.PlcSetQDeviceEx(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, null, batch, pWorkNum, "배치수 변경을 실패하였습니다"))
                                                    {
                                                        return;
                                                    }

                                                    if (clsCommon.PlantCode == "PJ01" && dtPlc.Rows.Count > 1)
                                                    {
                                                        clsCommon.GetPLCAddress(dr["PLANT_CODE"]?.ToString()
                                                                , dr["PROCESS_KEY"]?.ToString()
                                                                , dr["L_CODE"]?.ToString()
                                                                , 2
                                                                , PlcAddressType.BATCHCOUNT.GetDesc()
                                                                , 1
                                                                , out vPLCAddress
                                                                , out vPLCUnit
                                                                , out vPLCDataCount);

                                                        Array.Clear(pWorkNum, 0, pWorkNum.Length);

                                                        pWorkNum[0] = batch;

                                                        if (!PlcFunc.PlcSetQDeviceEx2(dtPlc, 2, vPLCAddress, vPLCUnit, vPLCDataCount, null, batch, pWorkNum, "배치수 변경을 실패하였습니다"))
                                                        {
                                                            return;
                                                        }
                                                    }

                                                    string errMsg = $"[{batch}] 배치로 변경했습니다";

                                                    if (!clsProcessDosing.InsertLog(dr["PLANT_CODE"]?.ToString(), dr["PROCESS_KEY"]?.ToString(), dr["L_CODE"]?.ToString(), dr["WORKDATE"]?.ToString(), dr["NUM"]?.ToString(), r_batch.ToString(), clsCommon.GetPcStatusCode("계획"), errMsg))
                                                    {
                                                        Dbconn.conn.Rollback();
                                                        clsLog.logSave("btn_ok_Click", "btn_ok_Click", SQL);
                                                        return;
                                                    }

                                                    // GBG -
                                                }
                                                else
                                                {
                                                    ShowMessageBox.XtraShowWarning("현재 진행중인 배치보다 배치수가 작을 수 없습니다.");

                                                    return;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                clsLog.logSave(this, "PLC BATCH CNT CHANGE FAIL", ex);
                                            }

                                            //현재 작업중인 배치 변경
                                            SQL = $@"
                                            -- 배치, 작업지시량, 전송여부 수정
                                            UPDATE WORK_ORDER
                                            SET BATCH = '{batch}', OR_Q = '{dr["OR_Q"]}', TRANS_YN = '{dr["TRANS_YN"]}'
                                            WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND L_CODE = '{dr["L_CODE"].ToString().Replace(dr["PLANT_CODE"].ToString(), "")}'
                                                AND WORKDATE = '{dr["WORKDATE"]}' AND NUM = '{dr["NUM"]}'
                                            ";

                                            if (Dbconn.conn.SQLrun(SQL) < 1)
                                            {
                                                Dbconn.conn.Rollback();
                                                ShowMessageBox.XtraShowInformation("현재 작업중인 배치를 업데이트 하는 도중 오류가 발생했습니다");
                                                clsLog.logSave("frm_Dosing", "btn_workSave_Click", SQL);
                                                return;
                                            }

                                            //SQL = $@"
                                            //-- 펠렛 이전공정작업지시량 수정
                                            //UPDATE PELLET_REPORT
                                            //SET BF_QTY = '{dr["OR_Q"]}'
                                            //WHERE BF_PLANT_CODE = '{dr["PLANT_CODE"]}'
                                            //        AND BF_PROCESS_KEY = '{dr["PROCESS_KEY"]}' AND BF_L_CODE = '{dr["L_CODE"].ToString().Replace(dr["PLANT_CODE"].ToString(), "")}'
                                            //        AND BF_WORKDATE = '{dr["WORKDATE"]}' AND BF_NUM = '{dr["NUM"]}'
                                            //";

                                            //Dbconn.conn.SQLrun(SQL);
                                        }
                                    }
                                    else
                                    {
                                        ShowMessageBox.XtraShowInformation("작업지시 정보를 찾을 수 없습니다");
                                        return;
                                    }
                                }
                                // 후레이크, 익스콘
                                else
                                {
                                    return_msg = clsProcessDosing.InsertWorkNum(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), dr);
                                    if (return_msg != "OK")
                                    {
                                        dr.RowError = return_msg;
                                        ShowMessageBox.XtraShowWarning(dr.RowError);
                                        return;
                                    }
                                }
                            } //진행중 작업지시 배치수정
                        }

                        // 031003	진행
                        if (dr["C_CONDITION"].ToString() != clsCommon.GetPcStatusCode("진행"))
                        {
                            // INSERT INTO WORK_DETAIL
                            return_msg = clsProcessDosing.InsertWorkNum(string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue), dr);
                            if (return_msg != "OK")
                            {
                                dr.RowError = return_msg;
                                ShowMessageBox.XtraShowWarning(dr.RowError);
                                return;
                            }
                        }
                    }

                    dr.AcceptChanges();
                    gridView_work.RefreshData();
                } //foreach

                ShowMessageBox.XtraShowInformation("작업지시 정보가 저장 되었습니다.");
                XMain_Search();

                bTransFlag = false;

                SQL = $@"
                SELECT PLANT_CODE
                    , PROCESS_KEY
                    , L_CODE
                    , WORKDATE
                    , NUM
                    , RESOURCE_NO
                    , NOTE
                FROM WORK_ORDER
                WHERE PLANT_CODE = '{vPlant_Code}'
                    AND PROCESS_KEY = '{vProcess_Code}'
                    AND L_CODE = '{vLine_Code}'
                    AND C_CONDITION = '{clsCommon.PcStatus.InProgress}'
                    AND NVL(DEL_FLAG, 'N') != 'Y'
                    AND WORKDATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                ORDER BY NUM
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {
                    vWorkStart = true;
                }
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

        /// <summary>
        /// 단일 쓰기
        /// </summary>
        /// <param name="plcType"></param>
        /// <param name="handYn"></param>
        //private static void PlcSetQDeviceEx(List<string> plcType, int iPlcType, bool handYn)
        //{
        //    if (plcType[iPlcType] == "Q")
        //        MAIN.qPlc1.SetQDeviceEx(vPLCAddress, handYn == true ? 1 : 0, sErrMsg);
        //    // GBG
        //    else if (plcType[iPlcType] == "A")
        //    {
        //        MAIN.aPlc2.SetADeviceEx(vPLCAddress, handYn == true ? 1 : 0, sErrMsg);
        //    }
        //    else if (plcType[iPlcType] == "XGI")
        //    {
        //        int[] temp = new int[2];
        //        temp[0] = handYn == true ? 1 : 0;
        //        if (clsXgiHandler.Write(2, vPLCAddress, vPLCDataCount, temp) == 0)
        //        {
        //            clsXgiHandler.Write(2, vPLCAddress, vPLCDataCount, temp, sErrMsg);
        //        }
        //    }
        //    else if (plcType[iPlcType] == "CM")
        //    {
        //        int[] temp = new int[1];
        //        temp[0] = handYn == true ? 1 : 0;

        //        //_ = clsCimonHandler.TryWriteWord(vPLCAddress, temp);
        //        clsUtil.Delay(500);
        //        if (clsCimonHandler2.Write(2, vPLCAddress, vPLCDataCount, temp) == 0)
        //        {
        //            clsUtil.Delay(500);
        //            if (clsCimonHandler2.Write(2, vPLCAddress, vPLCDataCount, temp) == 0)
        //            {
        //                //ShowMessageBox.XtraShowError("수투입 여부 전송을 실패하였습니다.", "알림");
        //                //return false;
        //            }
        //        }
        //    }
        //}



        #region 작업지시정보 저장버튼 클릭이벤트
        private void btn_workSave_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            //gridView_work.PostEditor();
            //gridView_work.UpdateCurrentRow();

            //gridView_work.EndUpdate();

            // 031002	배합공정
            //if (clsCommon._strPlcConnYn != "Y" && clsCommon.dosing_process_code == vProcess_Code)
            //{
            //    ShowMessageBox.XtraShowInformation("사용권한이 업습니다\r\n제어실에서만 동작가능합니다");
            //    return;
            //}

            if (DialogResult.Yes != ShowMessageBox.Confirm("작업지시정보 데이터를 저장하시겠습니까?"))
            {
                return;
            }

            clsDevexpressGrid.GridEndEdit(gridView_work);

            XMain_Save();
        }
        #endregion

        #region 작업지시 강제완료 버튼클릭 이벤트
        private void btn_workEnd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!clsCommon.Auth_Form_Function(authDs, "W"))
                {
                    ShowMessageBox.XtraShowInformation("권한이 없습니다");
                    return;
                }

                if (gridView_work.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("강제완료 하실 작업지시를 선택하여 주세요");
                    return;
                }

                SetWorkEnd();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_workEnd_Click", ex);
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

        private void SetWorkEnd()
        {
            DataRow row = gridView_work.GetDataRow(gridView_work.FocusedRowHandle);

            if (row.RowState != DataRowState.Added)
            {
                string plantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
                string processKey = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
                string lCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");
                string work_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");
                string WORK_START_DATE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
                string work_batch = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH");
                string st_time = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "START_TIME");
                string proQty = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PRO_Q");
                string orQty = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "OR_Q");

                string wErpOstatus = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "ERP_OSTATUS");
                string wErpIstatus = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "ERP_ISTATUS");

                string insert_st_time = string.Empty;

                string con_st = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "C_CONDITION");

                string OR_Q = string.Empty;

                if (con_st == clsCommon.PcStatus.Completed)     // 031004	완료
                {
                    ShowMessageBox.XtraShowInformation("이미 완료처리 된 작업지시 입니다");
                    return;
                }

                if (!aPassProcess.Contains(processKey) && con_st == clsCommon.GetPcStatusCode("진행"))     // 031003	진행
                {
                    if (DialogResult.Yes == ShowMessageBox.Confirm("작업을 [강제 완료]로 종료 하시겠습니까?"))
                    {
                        SQL = $@"
                        -- 강제완료
                        UPDATE WORK_ORDER
                        SET END_TIME = SYSDATE
                            , C_CONDITION = '{clsCommon.GetPcStatusCode("강제완료")}'
                        WHERE PLANT_CODE = '{plantCode}'
                            AND PROCESS_KEY = '{processKey}'
                            AND L_CODE = '{lCode}'
                            AND WORKDATE = '{WORK_START_DATE}'
                            AND NUM = '{work_num}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave(this.Text, "SetWorkEnd()", SQL);
                            ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                            return;
                        }

                        // 작지 SAP 실적 생성
                        if (!clsProcessDosing.SetSAPWorkRemark(plantCode, processKey, lCode, WORK_START_DATE, work_num, "", 100))
                        {
                            ShowMessageBox.XtraShowWarning("SAP 실적 생성에 실패했습니다");
                        }

                        ShowMessageBox.XtraShowWarning("강제완료 되었습니다.");

                        return;
                    }
                }

                DialogResult result = ShowMessageBox.Confirm("선택하신 작업순번 " + work_num + " 작업지시를 강제완료 하시겠습니까?");
                if (result == DialogResult.Yes)
                {
                    splashScreenManager.ShowWaitForm();
                    Dbconn.conn.BeginTransaction();

                    //SQL = $@"
                    //SELECT SUM(b.SET_VAL) * A.BATCH AS SVAL
                    //FROM WORK_ORDER A
                    //    JOIN WORK_DETAIL b ON b.PLANT_CODE = A.PLANT_CODE AND b.PROCESS_KEY = A.PROCESS_KEY AND b.L_CODE = A.L_CODE
                    //                    AND b.WORKDATE = A.WORKDATE AND b.NUM = A.NUM  
                    //WHERE a.PLANT_CODE = '{plantCode}'
                    //        AND a.PROCESS_KEY = '{processKey}' AND a.L_CODE = '{lCode}'
                    //        AND a.WORKDATE = '{WORK_START_DATE}' AND a.NUM = '{work_num}'
                    // GROUP BY A.BATCH
                    //";

                    //DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    //if (Dbconn.conn.getRowCnt(ds) > 0)
                    //{
                    //    OR_Q = Dbconn.conn.getData(ds, "SVAL", 0);
                    //}

                    //if (decimal.TryParse(proQty, out decimal val) && val == 0)
                    //{
                    //    ShowMessageBox.XtraShowWarning("생산량을 저장 후 완료 해주세요.");
                    //    return;
                    //}

                    OR_Q = (string.IsNullOrEmpty(proQty) || proQty == "0") ? orQty : proQty;

                    SQL = $@"
                    -- 배치완료 ERP 전송내역 수정
                    UPDATE WORK_ORDER
                    SET R_BATCH = BATCH, PRO_Q = '{OR_Q}', C_CONDITION = '{clsCommon.PcStatus.Completed}'
                        , END_TIME = SYSDATE
                        , ERP_OSTATUS = CASE WHEN '{clsCommon.GetTransAuto(plantCode, processKey)}' = 'Y'
                                                    THEN 'F'
                                                    ELSE 'N'
                                                END
                        , ERP_ISTATUS = CASE WHEN '{clsCommon.GetTransAuto(plantCode, processKey)}' = 'Y' THEN 'F' ELSE 'N' END
                        , ERP_OERR_CNT = 0
                        , ERP_IERR_CNT = 0
                    WHERE PLANT_CODE = '{plantCode}'
                            AND PROCESS_KEY = '{processKey}' AND L_CODE = '{lCode}'
                            AND WORKDATE = '{WORK_START_DATE}' AND NUM = '{work_num}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave(this.Text, "btn_workEnd_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                        return;
                    }

                    SQL = $@"
                        SELECT 1 FROM PELLET_REPORT
                        WHERE BF_PLANT_CODE = '{plantCode}'
                                AND BF_PROCESS_KEY = '{processKey}' AND BF_L_CODE = '{lCode}'
                                AND BF_WORKDATE = '{WORK_START_DATE}' AND BF_NUM = '{work_num}'
                        ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        SQL = $@"
                        -- 펠렛 작업지시 수정
                        UPDATE PELLET_REPORT
                        SET BF_QTY = (SELECT OR_Q FROM WORK_ORDER WHERE PLANT_CODE = '{plantCode}'
                                AND PROCESS_KEY = '{processKey}' AND L_CODE = '{lCode}'
                                AND WORKDATE = '{WORK_START_DATE}' AND NUM = '{work_num}')
                            , QTY = (SELECT OR_Q FROM WORK_ORDER WHERE PLANT_CODE = '{plantCode}'
                                AND PROCESS_KEY = '{processKey}' AND L_CODE = '{lCode}'
                                AND WORKDATE = '{WORK_START_DATE}' AND NUM = '{work_num}')
                            , ERP_UP_YN = CASE WHEN ERP_UP_YN = 'Y' THEN 'M' ELSE 'N' END
                        WHERE BF_PLANT_CODE = '{plantCode}'
                                AND BF_PROCESS_KEY = '{processKey}' AND BF_L_CODE = '{lCode}'
                                AND BF_WORKDATE = '{WORK_START_DATE}' AND BF_NUM = '{work_num}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_workEnd_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    // 031002	배합공정
                    if (vProcess_Code == clsCommon.GetProcessKey("배합"))
                    {
                        SQL = $@"
                        -- 배합작업지시 수정
                        UPDATE WORK_ORDER SET START_TIME = SYSDATE, END_TIME = SYSDATE
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
                        WHERE PLANT_CODE = '{plantCode}'
                            AND PROCESS_KEY = '{processKey}' AND L_CODE = '{lCode}'
                            AND WORKDATE = '{WORK_START_DATE}' AND NUM = '{work_num}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_workEnd_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        SELECT 1 FROM WORK_ORDER
                        WHERE PLANT_CODE = '{plantCode}'
                            AND PROCESS_KEY = '{processKey}' AND L_CODE = '{lCode}'
                            AND WORKDATE= '{WORK_START_DATE}' AND NUM = '{work_num}'
                        ";

                        DataSet insertDataDs = Dbconn.conn.ExecutDataset(SQL);
                        row = insertDataDs.Tables[0].Rows[0];
                    }

                    // 작지 실적 생성
                    if (!clsProcessDosing.InsertWorkRemark(plantCode, processKey, lCode, WORK_START_DATE, work_num))
                    {
                        Dbconn.conn.Rollback();
                        ShowMessageBox.XtraShowWarning("강제완료에 실패했습니다");
                        return;
                    }

                    Dbconn.conn.Commit();

                    // 작지 SAP 실적 생성
                    if (!clsProcessDosing.SetSAPWorkRemark(plantCode, processKey, lCode, WORK_START_DATE, work_num, "", 100))
                    {
                        Dbconn.conn.Rollback();
                        ShowMessageBox.XtraShowWarning("SAP 실적 생성에 실패했습니다");
                        return;
                    }

                    string erpResult = string.Empty;

                    ShowMessageBox.XtraShowInformation("강제완료처리가 완료되었습니다");
                }
            }
            else
            {
                ShowMessageBox.XtraShowInformation("해당 작업지시는 저장을 완료하신후에 강제완료 하여 주시길 바랍니다");
                return;
            }

            XMain_Search();
        }
        #endregion

        #region 작업지시 시작 버튼 클릭 이벤트
        private bool GetPlcConStats(out string sOutMsg, out List<string> plcType)
        {
            DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

            DataRow[] drPlc = dtPlc.Select($"PLC_NO = {vPLC_Location}");
            string sPlcType = drPlc[0]["PLC_TYPE"]?.ToString();

            plcType = new List<string>();
            if (dtPlc != null && dtPlc.Rows.Count > 0)
            {
                for (int i = 0; i < dtPlc.Rows.Count; i++)
                {
                    plcType.Add(dtPlc.Rows[i]["PLC_TYPE"]?.ToString());
                }
            }

            switch (plcType.Count)
            {
                case 1:
                    if ((plcType[0] == "Q" && MAIN.MainPlcConnChk != "Y") || (plcType[0] == "XGI" && MAIN.MainPlcConnChk != "Y"))
                    {
                        cboOperWorkMode.SelectedIndex = 0;
                        work_watch_timer.Stop();
                        sOutMsg = "PLC 1번을 연결 해주세요.";
                        return false;
                    }

                    if (plcType[0] == "A" && MAIN.SubPlcConnChk != "Y")
                    {
                        cboOperWorkMode.SelectedItem = 0;
                        work_watch_timer.Stop();
                        sOutMsg = "PLC 1번, 2번 을 먼저 연결 해주세요.";
                        return false;
                    }

                    // GBG
                    if (plcType[0] == "XGI" && MAIN.MainPlcConnChk != "Y")
                    {
                        cboOperWorkMode.SelectedItem = 0;
                        work_watch_timer.Stop();
                        sOutMsg = "PLC 1번을 연결 해주세요.";
                        return false;
                    }

                    if (plcType[0] == "CM" && MAIN.MainPlcConnChk != "Y")
                    {
                        cboOperWorkMode.SelectedItem = 0;
                        work_watch_timer.Stop();
                        sOutMsg = "PLC 1번을 연결 해주세요.";
                        return false;
                    }
                    // GBG -
                    break;
                case 2:
                    if ((plcType[0] ==
                        "Q" && MAIN.MainPlcConnChk != "Y") || (plcType[1] == "A" && MAIN.SubPlcConnChk != "Y"))
                    {
                        cboOperWorkMode.SelectedItem = 0;
                        work_watch_timer.Stop();
                        sOutMsg = "PLC 1번, 2번 을 먼저 연결 해주세요.";
                        return false;
                    }
                    break;
                default:
                    break;
            }

            sOutMsg = "";
            return true;
        }

        private void btn_workStart_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            try
            {
                //작업지시정보
                DataRow row = gridView_work.GetDataRow(gridView_work.FocusedRowHandle);
                string plantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
                string process_key = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
                string lCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");
                string work_date = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
                string work_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");

                DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

                if (!PlcFunc.GetPlcCon(dtPlc, out MAIN.sErrMsg))
                {
                    ShowMessageBox.XtraShowInformation(MAIN.sErrMsg);
                    return;
                }

                if (clsDevexpressGrid.GetSelectRowCount(gridView_work) == 0)
                {
                    ShowMessageBox.XtraShowInformation("작업을 진행하실 작업지시를 선택하여 주세요");
                    return;
                }

                //MAIN mainForm = Application.OpenForms["MAIN"] as MAIN;
                //if (mainForm != null)
                //{
                //    mainForm.work_watch_timer.Stop();  // 🔸 PLC 타이머 정지
                //}

                DialogResult workStart = ShowMessageBox.Confirm($"작업순번 {work_num}번을 작업진행하시겠습니까?");

                if (workStart == DialogResult.No)
                    return;

                //if (mainForm != null)
                //{
                //    mainForm.work_watch_timer.Start();  // 🔸 PLC 타이머 정지
                //}

                //전송유무 체크
                bool isChkStart = XWorkStart(dtPlc, plantCode, process_key, lCode, work_date, work_num);
                if (!isChkStart)
                {
                    return;
                }

                bWorkStatus = true;

                // plc 작업지시 영역 초기화 
                clsLog.logSave(work_date + work_num + "작업시작", 0);

                //작업진행 LOG
                string errMsg = "도징 작업지시를 진행하였습니다";
                if (!clsProcessDosing.InsertLog(plantCode, process_key, lCode, work_date, work_num, "0", "031102", errMsg))
                {
                    clsLog.logSave(this, "btn_workStart_Click", "작업로그 입력에 실패하였습니다/ " + work_date + "/" + work_num + "/" + "0");
                }

                //작업지시 재조회
                XMain_Search();

                vWorkStart = true;

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                ShowMessageBox.XtraShowError("작업을 시작하는 도중 에러가 발생하였습니다");
            }
        }

        #region 작업시작
        private bool XWorkStart(DataTable dtPlc, string plantCode, string process_key, string lCode, string work_date, string work_num)
        {
            string WorkStartSQL = string.Empty;

            DataSet Rs = null;

            int Dev = 0;
            int DosDev = 100;
            int MicDev = 600;
            int iPlcType = 1;
            string splcType = string.Empty;
            int j = 0;
            int[] pWorkNum = new int[15]; // 작업지시 정보전송 배열

            // GBG
            int[] Recipe_Data = new int[500];
            int[] Recipe_Scale = new int[100];
            //int[] Recipe_D1 = new int[100];
            //int[] Recipe_D2 = new int[100];
            //int[] Recipe_D3 = new int[100];
            //int[] Recipe_L1 = new int[100];
            //int[] Recipe_L2 = new int[100];

            int Recipe_D1_Index = 0;
            int Recipe_D2_Index = 0;
            int Recipe_D3_Index = 0;
            int Recipe_L1_Index = 0;
            int Recipe_L2_Index = 0;

            string sLocationEd = string.Empty;
            string sResourceNo = string.Empty;
            string sNote = string.Empty;
            string sOrQ = string.Empty;
            string sPackType = string.Empty;
            // GBG -

            try
            {
                ShowSplahScreenManager("작업지시정보를 PLC에 전송중입니다");

                #region 생산지시 번호 체크(DB 존재유무,작업중/완료 유무)
                WorkStartSQL = $@"
                -- 생산지시 번호 체크(DB 존재유무,작업중/완료 유무)
                SELECT BATCH, C_CONDITION, RESOURCE_NO, NOTE, LOCATION_ED, OR_Q, PACK_TYPE
                FROM WORK_ORDER
                WHERE PLANT_CODE = '{plantCode}'
                    AND PROCESS_KEY = '{process_key}'
                    AND L_CODE = '{lCode}'
                    AND WORKDATE = '{work_date}'  AND NUM = '{work_num}'
                ";

                DataSet dukChkDs = Dbconn.conn.ExecutDataset(WorkStartSQL);

                if (Dbconn.conn.getRowCnt(dukChkDs) < 1)
                {
                    ShowMessageBox.XtraShowWarning("작업지시 데이타를 찾을 수 없습니다");
                    return false;
                }
                else
                {
                    sLocationEd = Dbconn.conn.getData(dukChkDs, "LOCATION_ED", 0);
                    sResourceNo = Dbconn.conn.getData(dukChkDs, "RESOURCE_NO", 0);
                    sNote = Dbconn.conn.getData(dukChkDs, "NOTE", 0);
                    sOrQ = Dbconn.conn.getData(dukChkDs, "OR_Q", 0);
                    sPackType = Dbconn.conn.getData(dukChkDs, "PACK_TYPE", 0);
                }

                if (Dbconn.conn.getData(dukChkDs, "C_CONDITION", 0) != clsCommon.PcStatus.Plan)
                {
                    ShowMessageBox.XtraShowWarning("작업상태가 진행,완료 상태입니다\r\n현재 작업지시를 작업하실 수 없습니다.");
                    return false;
                }

                SQL = $@"
                SELECT a.RESOURCE_NO, b.DESCRIPTION, a.STOCK
                FROM BIN a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{plantCode}' AND a.LOCATION = '{sLocationEd}'
                ";

                using (DataSet ds = Dbconn.conn.ExecutDataset(SQL))
                {
                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        if (Dbconn.conn.getData(ds, "RESOURCE_NO", 0) == "")
                        {
                            if (DialogResult.Yes != ShowMessageBox.Confirm($"['{sLocationEd}'] 목적빈에 제품이 설정 되있지 않습니다. {Environment.NewLine} 제품 변경 후 진행 하시겠습니까?"))
                            {
                                ShowMessageBox.XtraShowWarning("빈 변경 후 진행 해주세요.");
                                return false;
                            }

                            SQL = $@"
                            -- 빈 재고 원료 및 재고 초기화
                            UPDATE BIN
                            SET RESOURCE_NO     = '{sResourceNo}'
                                , STOCK           = '0'
                            WHERE PLANT_CODE    = '{plantCode}'
                                AND LOCATION    =  '{sLocationEd}'
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 1)
                            {
                                Dbconn.conn.Rollback();
                                clsLog.logSave(this.Text, "btn_save_Click", SQL);
                                return false;
                            }
                        }
                        else if (Dbconn.conn.getData(ds, "RESOURCE_NO", 0) != sResourceNo)
                        {
                            string[] sExcBin = clsCommon.GetExeceptionBin();

                            if (plantCode == "PJ01" && Convert.ToDouble(Dbconn.conn.getData(ds, "STOCK", 0)) > 0 && !sExcBin.Contains(sLocationEd))
                            {
                                ShowMessageBox.XtraShowWarning("빈에 재고가 있습니다. 확인 후 진행 해 주세요.");
                                return false;
                            }

                            if (cboOperWorkMode.SelectedIndex != 1)
                            {
                                if ((DialogResult.Yes != ShowMessageBox.Confirm($"[{sLocationEd}] 목적빈에 [{Dbconn.conn.getData(ds, "DESCRIPTION", 0)}] 제품으로 설정 되어있습니다. {Environment.NewLine} 제품 변경 후 진행 하시겠습니까?")))
                                {
                                    ShowMessageBox.XtraShowWarning("빈 변경 후 진행 해주세요.");
                                    return false;
                                }
                            }

                            SQL = $@"
                            -- 빈 원료 및 재고 초기화
                            UPDATE BIN
                            SET RESOURCE_NO     = '{sResourceNo}'
                                , STOCK           = '0'
                            WHERE PLANT_CODE    = '{plantCode}'
                                AND LOCATION    =  '{sLocationEd}'
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 1)
                            {
                                Dbconn.conn.Rollback();
                                clsLog.logSave(this.Text, "btn_save_Click", SQL);
                                return false;
                            }
                        }
                    }
                }

                SQL = $@"
                SELECT a.SCALE_CODE, SUM(SET_VAL) as SET_VAL, MAX(b.MAX_Q) AS MAX_Q
                FROM WORK_DETAIL a
                    INNER JOIN SCALE b ON b.PLANT_CODE = a.PLANT_CODE
                        AND b.PROCESS_KEY = a.PROCESS_KEY
                        AND b.L_CODE = a.L_CODE 
                        and b.SCALE_CODE = a.SCALE_CODE
                WHERE a.PLANT_CODE = '{plantCode}'
                    AND a.PROCESS_KEY = '{process_key}'
                    AND a.L_CODE = '{lCode}'
                    AND a.WORKDATE = '{work_date}'
                    AND a.NUM = '{work_num}'
                GROUP BY a.SCALE_CODE
                ";

                DataRow dr = null;

                double setVal = 0;
                double maxQ = 0;
                string sScale = string.Empty;

                using (DataSet ds = Dbconn.conn.ExecutDataset(SQL))
                {
                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            dr = ds.Tables[0].Rows[i];

                            setVal = double.TryParse(dr["SET_VAL"]?.ToString(), out double val) ? val : 0;
                            maxQ = double.TryParse(dr["MAX_Q"]?.ToString(), out double val2) ? val2 : 0;
                            sScale = dr["SCALE_CODE"]?.ToString();

                            if (setVal > maxQ)
                            {
                                ShowMessageBox.XtraShowWarning($"[{sScale}] 스케일 최대 용량을 초과 했습니다. {Environment.NewLine} 레시피 확인 후 진행 해주세요.");
                                return false;
                            }
                        }
                    }
                }

                WorkStartSQL = $@"
                SELECT a.INGRED_CODE, b.DESCRIPTION
                FROM   WORK_DETAIL a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.INGRED_CODE
                WHERE  a.PLANT_CODE  = '{plantCode}'
                  AND  a.PROCESS_KEY = '{process_key}'
                  AND  a.L_CODE      = '{lCode}'
                  AND  a.WORKDATE    = '{work_date}'
                  AND  a.NUM         = '{work_num}'
                GROUP BY a.INGRED_CODE, b.DESCRIPTION
                HAVING COUNT(*) > 1
                ";

                dukChkDs = Dbconn.conn.ExecutDataset(WorkStartSQL);

                if (Dbconn.conn.getRowCnt(dukChkDs) > 0)
                {
                    DialogResult result = ShowMessageBox.Confirm($"[{dukChkDs.Tables[0].Rows[0]["DESCRIPTION"]}] 원료가 두개 이상있습니다. \n 이대로 진행 하시겠습니까?");

                    if (result == DialogResult.No) return false;
                }

                #endregion

                #region   PC->PLC 레시피쓰기 유무 체크

                clsUtil.Delay(1000);

                Array.Clear(pWorkNum, 0, pWorkNum.Length);

                ShowSplahScreenManager("작업시작 여부를 체크 중입니다");

                vPLC_Location = clsCommon.GetPlcLocation(vPlant_Code, cboProcessKey.EditValue?.ToString());

                clsCommon.GetPLCAddress(plantCode
                                            , process_key
                                            , lCode
                                            // GBG
                                            , vPLC_Location
                                            // GBG -
                                            , PlcAddressType.ISWORKABLE.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                if (!PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref pWorkNum, "도징계량에서 작업지시시작이 불가능한 상태입니다"))
                {
                    ShowMessageBox.XtraShowError("도징계량에서 작업지시시작이 불가능한 상태입니다", "알림");
                    return false;
                }

                if (pWorkNum[0] != 1)
                {
                    ShowMessageBox.XtraShowWarning("도징계량에서 작업지시시작이 불가능한 상태입니다", "알림");
                    clsLog.logSave($"{vPLCAddress} {"도징계량에서 작업지시시작이 불가능한 상태입니다"} ", 0);
                    return false;
                }

                // GBG -
                //if (MAIN.aPlc.ReadDeviceBlock("D" + (MicDev + 40), 1, out pWorkNum[0]) == 1)
                //{
                //    if (pWorkNum[0] != 1)
                //    {
                //        ShowMessageBox.XtraShowWarning("마이크로계량에서 작업지시시작이 불가능한 상태입니다(PLC2)");
                //        clsLog.logSave(work_num + ": PLC2에서 레시피쓰기 불가능 ", 0);
                //        return false;
                //    }
                //}

                Array.Clear(pWorkNum, 0, pWorkNum.Length);

                #endregion

                #region  PC->PLC 계량 파라미터 작업 (스케일별로 빈별로 설정값 WRITE)

                //DataSet ScRoutds1 = Dbconn.conn.ExecutDataset(WorkStartSQL);

                DataSet ScRoutds1 = clsProcessDosing.resultWorkResult(plantCode, process_key, lCode, work_date, work_num);

                if (Dbconn.conn.getRowCnt(ScRoutds1) <= 0)
                {
                    clsLog.logSave(work_num + ": 존재하는 배합비가 없습니다", 0);
                    ShowMessageBox.XtraShowWarning("존재하는 배합비가 없습니다", "알림");
                    return false;
                }

                ShowSplahScreenManager("레시피 정보를 PLC에 전송중입니다");

                Boolean handYn = false;
                string tmpScName = string.Empty;

                // GBG
                Array.Clear(Recipe_Data, 0, Recipe_Data.Length);

                // GBG -

                for (int i = 0; i < Dbconn.conn.getRowCnt(ScRoutds1); i++)
                {
                    if (tmpScName != Dbconn.conn.getData(ScRoutds1, "SCALE_CODE", i))
                    {
                        tmpScName = Dbconn.conn.getData(ScRoutds1, "SCALE_CODE", i);

                        j = 0;
                    }



                    if (tmpScName == "H")
                    {
                        handYn = true;
                        continue;
                    } // 수투입(약품)쪽은 필터링을 한다. 

                    if (clsCommon.PlantCode == "PJ01" && clsCommon.GetProcessKey("배합") == process_key)
                    {
                        if (lCode == process_key.Merge("1"))
                        {
                            switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                            {
                                case "DS101": Dev = 1; iPlcType = 1; break;
                                case "DS102": Dev = 2; iPlcType = 1; break;
                                case "DS103": Dev = 3; iPlcType = 1; break;
                                case "DS104": Dev = 4; iPlcType = 1; break;
                                case "DS105": Dev = 5; iPlcType = 1; break;
                                case "DS106": Dev = 1; iPlcType = 2; break;
                                case "DS107": Dev = 2; iPlcType = 2; break;
                                case "DS108": Dev = 3; iPlcType = 2; break;
                                case "DS109": Dev = 4; iPlcType = 2; break;
                            }
                        }
                        else if (lCode == process_key.Merge("2"))
                        {
                            switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                            {
                                // GBG
                                //case "P1": Dev = 1; break;
                                case "DS201": Dev = 1; iPlcType = 1; break;
                                case "DS202": Dev = 2; iPlcType = 1; break;
                                case "DS203": Dev = 3; iPlcType = 1; break;
                                case "DS204": Dev = 1; iPlcType = 2; break;
                                case "DS205": Dev = 2; iPlcType = 2; break;
                                    // GBG -
                            }
                        }
                    }
                    // GBG
                    else if (clsCommon.PlantCode == "PJ01" && clsCommon.GetProcessKey("PF배합") == process_key)
                    {
                        switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                        {
                            case "FS001": Dev = 1; break;
                        }
                        // GBG
                        iPlcType = 1;
                        // GBGB -
                    }
                    // GBG -
                    if (clsCommon.PlantCode == "PJ04" && clsCommon.GetProcessKey("배합") == process_key)
                    {
                        if (lCode == process_key.Merge("1"))
                        {
                            switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                            {
                                case "D1": Dev = 1; break;
                                case "D2": Dev = 2; break;
                                case "D3": Dev = 3; break;
                                case "D4": Dev = 4; break;
                                case "D5": Dev = 5; break;
                                case "D6": Dev = 6; break;
                                case "L1": Dev = 7; break;
                                case "L2": Dev = 8; break;
                                case "L3": Dev = 9; break;
                            }
                        }
                        else if (lCode == process_key.Merge("2"))
                        {
                            switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                            {
                                case "D1": Dev = 1; break;
                                case "D2": Dev = 2; break;
                                case "D3": Dev = 3; break;
                                case "D4": Dev = 1; break;
                                case "D5": Dev = 2; break;
                            }
                        }
                        else if (lCode == process_key.Merge("3"))
                        {
                            Dev = 1; iPlcType = 1;
                        }
                    }
                    else if (clsCommon.PlantCode == "PJ04" && clsCommon.GetProcessKey("PF배합") == process_key)
                    {
                        if (lCode == process_key.Merge("1"))
                        {
                            switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                            {
                                case "P1": Dev = 1; iPlcType = 2; break;
                            }
                        }
                        else if (lCode == process_key.Merge("2"))
                        {
                            switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                            {
                                case "P1": Dev = 1; iPlcType = 2; break;
                            }
                        }
                    }
                    else if (clsCommon.PlantCode == "PJ05" && clsCommon.GetProcessKey("배합", clsCommon.PlantCode) == process_key)
                    {
                        if (lCode == process_key.Merge("1"))
                        {
                            switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                            {
                                case "SD1": Dev = 1; break;
                                case "SD2": Dev = 2; break;
                            }
                        }
                    }
                    else if (clsCommon.GetProcessKey("갓돈배합") == process_key)
                    {
                        switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                        {
                            case "PS001": Dev = 1; break;
                            case "PS002": Dev = 2; break;
                            case "PS003": Dev = 3; break;
                            case "PS004": Dev = 4; break;
                        }
                        // GBG4
                        iPlcType = 2;
                        // GBG4 -
                    }
                    else if (clsCommon.PlantCode == "PJ02" && clsCommon.GetProcessKey("배합", clsCommon.PlantCode) == process_key)
                    {
                        switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                        {
                            case "ES001": Dev = 1; break;
                            case "ES002": Dev = 2; break;
                            case "ES101": Dev = 3; break;
                        }

                        // GBG
                        iPlcType = 1;
                        // GBG -
                    }
                    else if (clsCommon.PlantCode == "P201")
                    {
                        switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                        {
                            case "D1": Dev = 1; break;
                            case "D2": Dev = 2; break;
                            case "D3": Dev = 3; break;
                            case "D4": Dev = 4; break;
                            case "D5": Dev = 5; break;
                            case "D6": Dev = 6; break;
                            case "L1": Dev = 7; break;
                            case "L2": Dev = 8; break;
                        }
                    }
                    // GBG
                    else if (clsCommon.PlantCode == "P101")
                    {
                        switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                        {
                            case "D1": Dev = 1; break;
                            case "D2": Dev = 2; break;
                            case "D3": Dev = 3; break;
                            case "D4": Dev = 6; break;
                            case "D5": Dev = 7; break;
                            case "L1": Dev = 4; break;
                            case "L2": Dev = 5; break;
                            // GBG
                            case "L5": Dev = 8; break;
                                // GBG -
                        }
                    }
                    else if (clsCommon.PlantCode == "P102")
                    {
                        switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                        {
                            case "D1": Dev = 1; break;
                            case "D2": Dev = 2; break;
                            case "D3": Dev = 3; break;
                            case "L1": Dev = 4; break;
                            case "L2": Dev = 5; break;
                        }
                    }
                    // GBG -

                    //// 스케일 주소
                    //Dev += (j * 5);

                    //레시피배열 초기화
                    Array.Clear(pWorkNum, 0, pWorkNum.Length);

                    if (Dbconn.conn.getData(ScRoutds1, "LOCATION", i) == "소계")
                        continue;

                    pWorkNum[0] = Convert.ToInt32(Dbconn.conn.getData(ScRoutds1, "LOCATION", i));

                    //기본 KG단위, 스케일 테이블 단위컬럼 곱하기(배율)
                    pWorkNum[1] = (int)Math.Round(Convert.ToDouble(Dbconn.conn.getData(ScRoutds1, "SET_VAL", i)) * Convert.ToDouble(ScRoutds1.Tables[0].Rows[i]["IN_SCALE"]), 0); //설정량

                    clsCommon.GetPLCAddress(plantCode
                                        , process_key
                                        , lCode
                                        // GBG
                                        //, plcType[0] == "Q" || plcType[0] == "XGI" ? 1 : 2
                                        , iPlcType
                                        // GBG -
                                        , PlcAddressType.RACIPE.GetDesc()
                                        , Dev
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

                    var (device, number) = vPLCAddress.SplitDeviceAndNumber();

                    if (!PlcFunc.PlcSetWriteQDeviceAddBlockEx(dtPlc, iPlcType, vPLCAddress, vPLCUnit, vPLCDataCount, Dev, j, ref pWorkNum, Recipe_Data, ref Recipe_D1_Index, ref Recipe_D2_Index, ref Recipe_D3_Index, ref Recipe_L1_Index, ref Recipe_L2_Index, device, number, "배합비전송을 실패하였습니다(빈정보,설정값)"))
                    {
                        ShowMessageBox.XtraShowError("배합비전송을 실패하였습니다(빈정보,설정값)", "알림");
                        return false;
                    }

                    clsLog.logSave($"배합 전송 - Addr [{vPLCAddress}] Data [{pWorkNum[0]},{pWorkNum[1]},{pWorkNum[2]},{pWorkNum[3]},{pWorkNum[4]}]", 1);
                    // GBG -

                    // 스케일 총합 체크

                    j++;
                }

                // GBG
                if (clsCommon.PlantCode == "P102")
                {
                    int addr = 0;
                    //int[] scaleSize = new int[5] { 50, 50, 10, 20, 20 };
                    int[] scaleSize = new int[5] { 100, 100, 60, 60, 60 };
                    for (int i = 0; i < 5; i++)
                    {
                        Array.Copy(Recipe_Data, i * 100, Recipe_Scale, 0, 100);
                        addr = 26000 + (i * 100);
                        //_ = clsCimonHandler.TryWriteWord("D0" + addr.ToString(), Recipe_Scale);
                        clsUtil.Delay(200);
                        if (clsCimonHandler2.Write(2, addr, scaleSize[i], Recipe_Scale) == 0)
                        {
                            clsUtil.Delay(200);
                            if (clsCimonHandler2.Write(2, addr, scaleSize[i], Recipe_Scale) == 0)
                            {
                                ShowMessageBox.XtraShowError("배합비 전송을 실패하였습니다.", "알림");
                                return false;
                            }
                        }
                        clsLog.logSave($"배합 전송 - Addr [{"D" + addr.ToString()}] 갯수 [{Recipe_L1_Index}]", 1);
                    }
                }
                // GBG -

                ShowSplahScreenManager("수투입 여부 정보를 PLC에 전송중입니다");

                clsCommon.GetPLCAddress(plantCode
                                    , process_key
                                    , lCode
                                    // GBG
                                    , vPLC_Location
                                    // GBG -
                                    , PlcAddressType.HANDYN.GetDesc()
                                    , 1
                                    , out vPLCAddress
                                    , out vPLCUnit
                                    , out vPLCDataCount);

                int[] temp = new int[1];

                temp[0] = handYn == true ? 1 : 0;

                if (!PlcFunc.PlcSetQDeviceEx(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, null, 0, temp, "수투입 여부 전송 실패"))
                {
                    ShowMessageBox.XtraShowError("수투입 여부 전송 실패", "알림");
                    return false;
                }

                ShowSplahScreenManager("작업설정 정보를 PLC에 전송중입니다");

                /*
                작지1	        D0100
                작지2	        D0101
                작지3	        D0102
                배치SV	        D0103
                목적빈1	        D0104
                목적빈2	        D0105
                믹싱시간SV	    D0106
                건조시간SV	    D0107
                파이널시간SV	    D0108
                배합이송타임	    D0109
                수투입	        D0110
                제품이송시간	    D0111
                */
                //작업지시전송
                if (!PlcWork_Conv(plantCode, process_key, lCode, work_date, work_num, pWorkNum))
                {
                    return false;
                }

                clsCommon.GetPLCAddress(plantCode
                                        , process_key
                                        , lCode
                                        , vPLC_Location
                                        , PlcAddressType.WORKINFO.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

                if (!PlcFunc.SetWriteQDeviceBlockEx(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref pWorkNum, "배합 작업지시 전송 실패하였습니다"))
                {
                    ShowMessageBox.XtraShowError("배합 작업지시 전송 실패하였습니다", "알림");
                    return false;
                }

                if (clsCommon.PlantCode == "PJ01" && process_key != clsCommon.GetProcessKey("PF배합", clsCommon.PlantCode) && dtPlc.Rows.Count > 1)
                {
                    clsCommon.GetPLCAddress(plantCode
                                            , process_key
                                            , lCode
                                            , 2
                                            , PlcAddressType.WORKINFO.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                    if (!PlcFunc.SetWriteQDeviceBlockEx(dtPlc, 2, vPLCAddress, vPLCUnit, vPLCDataCount, ref pWorkNum, "배합 작업지시 전송 실패하였습니다"))
                    {
                        ShowMessageBox.XtraShowError("배합 작업지시 전송 실패하였습니다", "알림");
                        return false;
                    }
                }

                // GBG -
                WorkStartSQL = $@"
                -- 배합작업 진행 수정
                UPDATE WORK_ORDER
                SET START_TIME   = SYSDATE
                    , C_CONDITION  = '{clsCommon.GetPcStatusCode("진행")}'
                    , R_BATCH      = '1'
                    , ERP_OSTATUS = CASE WHEN ERP_OSTATUS = 'Y' THEN 'M' ELSE 'N' END
                    , ERP_ISTATUS = CASE WHEN ERP_ISTATUS = 'Y' THEN 'M' ELSE 'N' END
                    , MIXINGYN = CASE WHEN '{lCode}' IN ('MXP0102', 'MXP0301') THEN 'N' ELSE 'Y' END
                WHERE PLANT_CODE = '{plantCode}'
                    AND PROCESS_KEY = '{process_key}'
                    AND L_CODE = '{lCode}'
                    AND WORKDATE   = '{work_date}'
                    AND NUM        = '{work_num}'
                ";

                Dbconn.conn.SQLrun(WorkStartSQL);

                Array.Clear(temp, 0, temp.Length);

                // 작업시작
                clsCommon.GetPLCAddress(plantCode
                                    , process_key
                                    , lCode
                                    // GBG
                                    , vPLC_Location
                                    // GBG -
                                    , PlcAddressType.WORKSTART.GetDesc()
                                    , 1
                                    , out vPLCAddress
                                    , out vPLCUnit
                                    , out vPLCDataCount);

                pWorkNum[0] = 1;

                //Thread.Sleep(3000);

                if (!PlcFunc.PlcSetQDeviceEx(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, null, 0, pWorkNum, "작업시작 전송 실패"))
                {
                    ShowMessageBox.XtraShowError("작업시작 전송 실패", "알림");
                    return false;
                }

                // GBG
                clsLog.logSave($"작업 시작 - 작업일자 [{work_date}] 순번 [{work_num}]", 0, "PLC");
                // GBG -

                // GBG
                if (clsCommon.PlantCode == "PJ01" && process_key != clsCommon.GetProcessKey("PF배합", clsCommon.PlantCode) && dtPlc.Rows.Count > 1)
                {
                    // 작업시작
                    clsCommon.GetPLCAddress(plantCode
                                        , process_key
                                        , lCode
                                        , 2
                                        , PlcAddressType.WORKSTART.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

                    pWorkNum[0] = 1;

                    if (!PlcFunc.PlcSetQDeviceEx2(dtPlc, 2, vPLCAddress, vPLCUnit, vPLCDataCount, null, 0, pWorkNum, "작업시작 전송 실패"))
                    {
                        ShowMessageBox.XtraShowError("작업시작 전송 실패", "알림");
                        return false;
                    }
                }

                string pelletProcess = string.Empty;

                clsCommon.GetAutoPellet(plantCode, process_key, sResourceNo, out pelletProcess);

                if (!string.IsNullOrEmpty(pelletProcess))
                {
                    // 가루 외 원료 일 때만 펠렛 작업지시 생성
                    SQL = $@"
                    INSERT INTO PELLET_REPORT (
                          PLANT_CODE       -- 01
                        , PROCESS_KEY      -- 02
                        , L_CODE           -- 03
                        , WORKDATE         -- 04
                        , WORK_SEQ         -- 05
                        , SEQ              -- 05
                        , P_TYPE           -- 06
                        , RESOURCE_NO      -- 07
                        , NOTE             -- 08
                        , ICM_CODE         -- 09
                        , EMPLOYEE_NO      -- 10
                        , BF_PLANT_CODE    -- 11
                        , BF_PROCESS_KEY   -- 12
                        , BF_L_CODE        -- 13
                        , BF_WORKDATE      -- 14
                        , BF_NUM           -- 15
                        , BF_QTY           -- 16
                        , BF_PACK_TYPE     -- 17
                        , ERP_UP_YN        -- 18
                        , ERP_ISTATUS
                        , ERP_OSTATUS
                        , C_CONDITION
                    )
                    VALUES (
                          '{plantCode}'                                    -- 01
                        , '{pelletProcess}'   -- 02
                        , '{clsCommon.GetPelletLCode(plantCode, pelletProcess, sLocationEd)}'                                           -- 03
                        , '{work_date}'                                 -- 04
                        , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) 
                            FROM PELLET_REPORT a WHERE a.PLANT_CODE = '{plantCode}'
                                AND a.PROCESS_KEY = '{pelletProcess}'
                                AND a.L_CODE = '{clsCommon.GetPelletLCode(plantCode, pelletProcess, sLocationEd)}'
                                AND a.WORKDATE = '{work_date}')                                             -- 05
                        , (SELECT NVL(MAX(WORK_SEQ) + 1, 1) 
                            FROM PELLET_REPORT a WHERE a.PLANT_CODE = '{plantCode}'
                                AND a.PROCESS_KEY = '{pelletProcess}'
                                AND a.L_CODE = '{clsCommon.GetPelletLCode(plantCode, pelletProcess, sLocationEd)}'
                                AND a.WORKDATE = '{work_date}')                                             -- 05
                        , '2'                                           -- 06
                        , '{sResourceNo}'                              -- 07
                        , '{sNote}'                                    -- 08
                        , ' '                                           -- 09
                        , ' '                                           -- 10
                        , '{plantCode}'                                    -- 11
                        , '{process_key}'                              -- 12
                        , '{lCode}'                                   -- 13
                        , '{work_date}'                                 -- 14
                        , '{work_num}'                                      -- 15
                        , '{sOrQ}'                                    -- 16
                        , '{sPackType}'                                -- 17
                        , 'N'                                           -- 18
                        , 'N'
                        , 'N'
                        , '{clsCommon.PcStatus.Plan}'
                    )
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave("clsProcessDosing", "InsertWorkNum", SQL);
                        ShowMessageBox.XtraShowError("펠렛 작업지시 입력을 실패했습니다", "경고");
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
                            '{clsCommon.GetPelletLCode(plantCode, pelletProcess, sLocationEd)}'           /* 07 */
                        , '{plantCode}'       /* 14 */
                        , '{pelletProcess}'      /* 15 */
                        , (SELECT NVL(MAX(WORK_SEQ), 1) 
                        FROM PELLET_REPORT a WHERE a.PLANT_CODE = '{plantCode}'
                            AND a.PROCESS_KEY = '{pelletProcess}'
                            AND a.L_CODE = '{clsCommon.GetPelletLCode(plantCode, pelletProcess, sLocationEd)}'
                            AND a.WORKDATE = '{work_date}')             /* 22 */
                        , '{work_date}'         /* 23 */
                    )
                    ";

                    if (SQL != "" && Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave("clsProcessDosing", "InsertWorkNum", SQL);
                        ShowMessageBox.XtraShowError("펠렛 작업지시 입력을 실패했습니다", "경고");
                    }
                }

                //GBG -
                #endregion

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex.Source);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());

            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            return true;
        }

        private void ShowSplahScreenManager(string sMsg)
        {
            if (splashScreenManager.IsSplashFormVisible)
            {
                splashScreenManager.CloseWaitForm();
            }

            splashScreenManager.ShowWaitForm();

            splashScreenManager.SetWaitFormCaption(sMsg);
        }

        #endregion

        public static bool PlcWork_Conv(string plantCode, string process_key, string lCode, string workDate, string workNum, int[] dev)
        {
            string SQL = $@"
            WITH HAND AS (
                SELECT a.PLANT_CODE
                    , CASE WHEN b.RESOURCE_NO IS NULL THEN 1 ELSE 0 END H
                FROM WORK_DETAIL a
                    LEFT JOIN BIN b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE AND b.RESOURCE_NO = a.INGRED_CODE
                WHERE a.PLANT_CODE = '{plantCode}'
                    AND a.PROCESS_KEY = '{process_key}'
                    AND a.L_CODE = '{lCode}'
                    AND a.WORKDATE = '{workDate}'
                    AND a.NUM = '{workNum}' 
                    AND b.RESOURCE_NO IS NULL
                    AND ROWNUM = 1
            )

            SELECT a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.NUM
                , a.LOCATION_ED, a.LOCATION_ED2, a.RESOURCE_NO, a.NOTE, a.BATCH
                , CASE WHEN '{lCode.Replace(process_key, "")}' = '1' THEN b.MIX_TIME ELSE b.MIX_TIME2 END MIX_TIME
                , b.DRY_TIME, b.FINAL_TIME, b.LR_YN, b.CR_YN, b.MT_TIME, NVL(c.H, 0) AS H
            FROM WORK_ORDER a
                INNER JOIN SAP_IN_BOM_CONM b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE AND b.P_TYPE = '2' --AND b.STLST = '2'
                LEFT JOIN HAND c ON c.PLANT_CODE = a.PLANT_CODE
            WHERE a.PLANT_CODE = '{plantCode}'
                AND a.PROCESS_KEY = '{process_key}'
                AND a.L_CODE = '{lCode}'
                AND a.WORKDATE = '{workDate}'
                AND a.NUM = '{workNum}'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            if (dt.Rows.Count == 0)
            {
                ShowMessageBox.XtraShowError("확정된 배합비가 없습니다. 배합비를 확인 바랍니다.");
                return false;
            }

            DataRow dr = dt.Rows[0];

            Array.Clear(dev, 0, dev.Length);

            dev[0] = Convert.ToInt32(dr["WORKDATE"].ToString().Substring(0, 4));
            dev[1] = Convert.ToInt32(dr["WORKDATE"].ToString().Substring(4, 4));
            dev[2] = Convert.ToInt32(dr["NUM"].ToString());
            dev[3] = Convert.ToInt32(dr["BATCH"].ToString());
            dev[4] = Convert.ToInt32(dr["LOCATION_ED"].ToString());
            dev[5] = Convert.ToInt32(dr["LOCATION_ED2"].ToString() == "" ? "0" : dr["LOCATION_ED2"].ToString());
            dev[6] = Convert.ToInt32(dr["MIX_TIME"].ToString() == "" ? "0" : int.Parse(dr["MIX_TIME"].ToString()).ToString());
            dev[7] = Convert.ToInt32(dr["DRY_TIME"].ToString() == "" ? "0" : int.Parse(dr["DRY_TIME"].ToString()).ToString());
            dev[8] = Convert.ToInt32(dr["FINAL_TIME"].ToString() == "" ? "0" : int.Parse(dr["FINAL_TIME"].ToString()).ToString());
            dev[9] = 1;
            dev[10] = Convert.ToInt32(dr["H"].ToString());
            dev[11] = 1;

            return true;
        }
        #endregion

        #region 작업지시내역 조회
        private void work_result(string Plant_Code, string Process_Key, string l_Code, string workdate, string num)
        {
            try
            {
                // FROM WORK_DETAIL
                DataSet ds = clsProcessDosing.resultWorkResult(Plant_Code, Process_Key, l_Code, workdate, num);

                clsDevexpressGrid.BindGridControl(gridControl_batchRun, gridView_batchRun, ds.Tables[0], false, true);

                sValid = new string[] { "" };


                clsDevexpressGrid.ItemLookUpEditSetup(gridcbo_run_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString(), "", "", 0, true));

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {
                    if (gridView_batchRun.Columns.Count > 8)
                    {
                        int maxColumnIndex = gridView_batchRun.Columns.Count - 1;

                        for (int i = maxColumnIndex; i > 5; i--)
                        {
                            gridView_batchRun.Columns.RemoveAt(i);
                        }
                    }

                    batchResultAdd(Plant_Code, Process_Key, l_Code, workdate, num);

                    //decimal total = 0;

                    //SQL = $@"
                    //SELECT RESOURCE_NO_2
                    //FROM SAP_IN_UPPRODUCT_CP 
                    //WHERE PLANT_CODE = '{Plant_Code}'
                    //    AND RESOURCE_NO = '{gridView_work.GetFocusedRowCellValue("RESOURCE_NO")}'
                    //";

                    //DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

                    //string resourceNo2 = dt.Rows[0]["RESOURCE_NO_2"] == DBNull.Value ? "" : dt.Rows[0]["RESOURCE_NO_2"].ToString();

                    //for (int i = 0; i < gridView_batchRun.RowCount; i++)
                    //{
                    //    int rowHandle = gridView_batchRun.GetVisibleRowHandle(i);

                    //    if (gridView_batchRun.IsGroupRow(rowHandle))
                    //        continue;

                    //    object loc = gridView_batchRun.GetRowCellValue(rowHandle, "LOCATION");

                    //    if (loc != null && loc.ToString() == "소계")
                    //        continue;

                    //    if (gridView_batchRun.GetRowCellValue(rowHandle, "RESOURCE_NO")?.ToString() == resourceNo2)
                    //        continue;

                    //    object val = gridView_batchRun.GetRowCellValue(rowHandle, "SET_VAL");

                    //    if (val != null && val != DBNull.Value)
                    //        total += Convert.ToDecimal(val);
                    //}

                    //object bat = gridView_work.GetFocusedRowCellValue("BATCH");

                    //if (bat != null && bat != DBNull.Value)
                    //    total = total * Convert.ToDecimal(bat);

                    //gridView_work.SetFocusedRowCellValue("OR_Q", total);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "work_result(string workdate, string num)", ex);
                clsLog.logSave(this, "work_result(string workdate, string num)", ex.StackTrace);
                ShowMessageBox.XtraShowError("작업지시내역을 조회하는중 오류가 발생했습니다.");
            }
        }
        #endregion

        #region 배치작업내역 조회 batch_result(string workdate, string num)
        private void batch_result(string Plant_Code, string Process_Key, string l_Code, string workdate, string num)
        {
            SQL = $@"
            SELECT BATCH, BA_Q, MIX_T, DRY_T, RMIX_T, ADDTM, BT_ST, I_TIME,
            SC3_TR_TIME, SC4_TR_TIME, SC5_TR_TIME, SC6_TR_TIME,
            SC7_TR_TIME, SC8_TR_TIME, SC9_TR_TIME, SC10_TR_TIME, LQ_T, MIX_DOWN_TR_TIME
            FROM BATCH
            WHERE PLANT_CODE = '{Plant_Code}' AND PROCESS_KEY = '{Process_Key}' AND L_CODE = '{l_Code}'
                AND WORKDATE = '{workdate}' AND NUM = '{num}'
            ORDER BY BATCH
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressGrid.BindGridControl(gridControl_Tab3, gridView_batchResult, ds.Tables[0], false, false);

            sValid = new string[] { "" };

        }
        #endregion

        #region 작업기록 조회 batch_log(string workdate, string num)
        private void batch_log(string Plant_Code, string Process_Key, string l_Code, string workdate, string num)
        {
            SQL = $@"
            SELECT  BATCH, SEQ, LOG_CODE, ERR_MSG, ST_TIME, ED_TIME, I_TIME
            FROM BATCH_LOG
            WHERE PLANT_CODE = '{Plant_Code}' AND PROCESS_KEY = '{Process_Key}' AND L_CODE = '{l_Code}'
                AND WORKDATE = '{workdate}' AND NUM = '{num}'
            ORDER BY BATCH, SEQ
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressGrid.BindGridControl(gridControl_Tab4, gridView_batchLog, ds.Tables[0], false, false);

            sValid = new string[] { "" };


            clsDevexpressGrid.ItemLookUpEditSetup(repItemLookUpEdit_LogCode, "03", "11");

            tabPage_workLog.Caption = "작업기록(" + Dbconn.conn.getRowCnt(ds).ToString() + ")";
        }
        #endregion


        private void batchList_result(string Plant_Code, string Process_Key, string l_Code, string workdate, string num)
        {
            SQL = $@"
               SELECT a.BATCH, B.SCALE_CODE, a.LOCATION,a.INGRED_LOT AS INGRED_CODE, a.NAME
                    , c.SET_VAL, a.P_Q, ( c.SET_VAL -  a.P_Q) * -1 as P_CHA, a.P_Q_TIME
               FROM WORK_REMARK a
                    LEFT OUTER JOIN BIN b ON b.PLANT_CODE = a.PLANT_CODE AND  b.LOCATION = a.LOCATION
                    LEFT OUTER JOIN WORK_DETAIL c ON c.PLANT_CODE = a.PLANT_CODE AND c.PROCESS_KEY = a.PROCESS_KEY
                                    AND c.L_CODE = a.L_CODE AND c.WORKDATE = a.WORKDATE
                                    AND c.NUM = a.NUM AND c.INGRED_CODE = a.INGRED_LOT
               WHERE a.PLANT_CODE = '{Plant_Code}' AND a.PROCESS_KEY = '{Process_Key}' AND a.L_CODE = '{l_Code}'
                    AND a.WORKDATE = '{workdate}' AND a.NUM = '{num}'
               ORDER BY a.BATCH, a.P_Q DESC
               ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressGrid.BindGridControl(gridControl_Tab2, gridView_batchList, ds.Tables[0], false, false);

            sValid = new string[] { "" };

            clsDevexpressGrid.ItemLookUpEditSetup(gridBlCboResourceNo, clsCommon.GetResource(Plant_Code, Process_Key, "", "", 0, true));

            gridView_batchList.OptionsView.ShowGroupPanel = true;
            gridView_batchList.Columns["BATCH"].GroupIndex = 0;

            gridView_batchList.OptionsView.ShowGroupedColumns = false;
            gridView_batchList.ExpandAllGroups();
            gridView_batchList.OptionsView.ShowGroupPanel = false;

        }

        #region 작업지시 그리드 로우셀 클릭 이벤트
        private void gridView_work_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                //ShowSplahScreenManager("작업지시정보를 PLC에 전송중입니다");

                SetBatchResult();
                SetCondition();

                GetRemark(false);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private void SetCondition()
        {
            string sCondition = gridView_work.GetFocusedRowCellValue("C_CONDITION")?.ToString();

            if (sCondition == clsCommon.PcStatus.Plan)
            {
                layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else if (sCondition == clsCommon.GetPcStatusCode("진행"))
            {
                layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
                layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        private void SetBatchResult()
        {
            DataRow row = gridView_work.GetDataRow(gridView_work.FocusedRowHandle);

            string sPlantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
            bool chk_version = false;
            decimal dNote_Per = 0;
            string sRESOURCE_NO = gridView_work.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();
            string sNOTE = clsCommon.getLastVersion(sPlantCode
                , clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO")
                , out chk_version, out dNote_Per);
            string sBATCH_Q = gridView_work.GetFocusedRowCellValue("BATCH_Q")?.ToString();
            string bu_yn = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BU_YN");

            string selPLANT_CODE = sPlantCode;
            string selPROCESS_KEY = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
            string selL_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");
            string selData_workdate = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
            string selData_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");

            if (row.RowState == DataRowState.Added)
            {
                mix_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, sRESOURCE_NO, sNOTE, sBATCH_Q, chk_version, bu_yn);
            }
            else
            {
                if (!string.IsNullOrEmpty(selData_workdate) && !string.IsNullOrEmpty(selData_num))
                {
                    work_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selData_workdate, selData_num);
                    batchList_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selData_workdate, selData_num);
                    batch_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selData_workdate, selData_num);
                    batch_log(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selData_workdate, selData_num);
                }
            }
        }
        #endregion

        private void batchResultAdd(string Plant_Code, string Process_Key, string l_Code, string workdate, string num)
        {
            DataSet ds1 = null;

            SQL = $"SELECT NVL(MAX(BATCH),0) AS MAX_BATCH FROM WORK_REMARK WHERE PROCESS_KEY = '{Process_Key}' AND WORKDATE = '{workdate}' AND NUM = '{num}' ";

            ds1 = Dbconn.conn.ExecutDataset(SQL);

            if (Convert.ToInt32(Dbconn.conn.getData(ds1, "MAX_BATCH", 0)) <= 0) return;
            int batch_cnt = Convert.ToInt32(Dbconn.conn.getData(ds1, "MAX_BATCH", 0));

            DataTable DT = (DataTable)gridControl_batchRun.DataSource;

            // 배치 컬럼 생성
            SetCreateBatch(batch_cnt, DT);

            SQL = $@"
            SELECT BATCH, LOCATION, RESOURCE_NO, P_Q, P_Q_TIME, INGRED_LOT
            FROM WORK_REMARK
            WHERE PLANT_CODE = '{Plant_Code}' AND PROCESS_KEY = '{Process_Key}' AND L_CODE = '{l_Code}'
                AND WORKDATE = '{workdate}' AND NUM = '{num}' AND IO_GUBUN = 'I'
            ORDER BY BATCH,LOCATION
            ";

            DataSet binRounteDs = Dbconn.conn.ExecutDataset(SQL);

            int temp_batch = 1;
            string filterEx = string.Empty;
            for (int i = 6; i < gridView_batchRun.Columns.Count; i++)
            {
                if (gridView_batchRun.Columns[i].FieldName.Contains("B"))
                {
                    for (int r = 0; r < gridView_batchRun.RowCount; r++)
                    {
                        // bin
                        if (gridView_batchRun.GetRowCellValue(r, "LOCATION").ToString().Trim() == "H")
                        {
                            filterEx = $"INGRED_LOT ='{gridView_batchRun.GetRowCellValue(r, "RESOURCE_NO")}' AND BATCH='{temp_batch.ToString()}' ";

                            DataRow[] row = binRounteDs.Tables[0].Select(filterEx);

                            if (row.Length > 0)
                            {
                                gridView_batchRun.SetRowCellValue(r, gridView_batchRun.Columns[i], row[0]["P_Q"]);
                            }
                        }
                        else
                        {
                            filterEx = $"LOCATION='{gridView_batchRun.GetRowCellValue(r, "LOCATION")}' AND BATCH='{temp_batch.ToString()}' ";

                            DataRow[] row = binRounteDs.Tables[0].Select(filterEx);

                            if (row.Length > 0)
                            {
                                gridView_batchRun.SetRowCellValue(r, gridView_batchRun.Columns[i], row[0]["P_Q"]);
                            }
                        }
                    }
                }

                if (gridView_batchRun.Columns[i].FieldName.Contains("M"))
                {
                    for (int r = 0; r < gridView_batchRun.RowCount; r++)
                    {
                        // bin
                        if (gridView_batchRun.GetRowCellValue(r, "LOCATION").ToString().Trim() == "H")
                        {
                            filterEx = $"INGRED_LOT='{gridView_batchRun.GetRowCellValue(r, "RESOURCE_NO")}' AND BATCH='{temp_batch.ToString()}' ";

                            DataRow[] row = binRounteDs.Tables[0].Select(filterEx);

                            if (row.Length > 0)
                            {
                                Decimal mVal = Convert.ToDecimal(gridView_batchRun.GetRowCellValue(r, "SET_VAL")) - Convert.ToDecimal(row[0]["P_Q"]);
                                gridView_batchRun.SetRowCellValue(r, gridView_batchRun.Columns[i], mVal);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(gridView_batchRun.GetRowCellValue(r, "SET_VAL").ToString()))
                            {
                                gridView_batchRun.SetRowCellValue(r, gridView_batchRun.Columns[i], 0);
                            }
                            else
                            {
                                filterEx = $"LOCATION='{gridView_batchRun.GetRowCellValue(r, "LOCATION")}' AND BATCH='{temp_batch.ToString()}' ";

                                DataRow[] row = binRounteDs.Tables[0].Select(filterEx);

                                if (row.Length > 0)
                                {
                                    Decimal mVal = Convert.ToDecimal(row[0]["P_Q"]) - Convert.ToDecimal(gridView_batchRun.GetRowCellValue(r, "SET_VAL"));
                                    gridView_batchRun.SetRowCellValue(r, gridView_batchRun.Columns[i], mVal);
                                }
                            }

                        }

                    }

                }

                if (gridView_batchRun.Columns[i].FieldName.Contains("T"))
                {
                    for (int r = 0; r < gridView_batchRun.RowCount; r++)
                    {
                        // bin
                        if (gridView_batchRun.GetRowCellValue(r, "LOCATION").ToString().Trim() == "H")
                        {
                            filterEx = $"INGRED_LOT ='{gridView_batchRun.GetRowCellValue(r, "RESOURCE_NO")}' AND BATCH='{temp_batch.ToString()}' ";

                            DataRow[] row = binRounteDs.Tables[0].Select(filterEx);

                            if (row.Length > 0)
                            {
                                gridView_batchRun.SetRowCellValue(r, gridView_batchRun.Columns[i], row[0]["P_Q_TIME"]);
                            }
                        }
                        else
                        {
                            filterEx = $"LOCATION='{gridView_batchRun.GetRowCellValue(r, "LOCATION")}' AND BATCH='{temp_batch.ToString()}' ";

                            DataRow[] row = binRounteDs.Tables[0].Select(filterEx);

                            if (row.Length > 0)
                            {
                                gridView_batchRun.SetRowCellValue(r, gridView_batchRun.Columns[i], row[0]["P_Q_TIME"]);
                            }
                        }
                    }

                    temp_batch += 1;
                }
            }
        }

        /// <summary>
        /// 그리드 배치 컬럼 생성
        /// </summary>
        /// <param name="batch_cnt"></param>
        /// <param name="DT"></param>
        private void SetCreateBatch(int batch_cnt, DataTable DT)
        {
            for (int i = 1; i < (batch_cnt + 1); i++)
            {
                GridColumn colBatch1 = new GridColumn();
                colBatch1.Visible = true;
                colBatch1.Width = 100;
                colBatch1.DisplayFormat.FormatString = "0.000";
                colBatch1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                colBatch1.Caption = i.ToString() + " 배치 계량값";
                colBatch1.FieldName = "B" + i.ToString();
                DT.Columns.Add("B" + i.ToString(), typeof(double));
                colBatch1.OptionsColumn.AllowEdit = true;
                colBatch1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                colBatch1.OptionsColumn.ReadOnly = true;
                colBatch1.SummaryItem.DisplayFormat = "{0:0.000} Kg";
                colBatch1.SummaryItem.FieldName = "B" + i.ToString();
                colBatch1.SummaryItem.Mode = DevExpress.Data.SummaryMode.AllRows;
                colBatch1.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;


                gridView_batchRun.Columns.Add(colBatch1);
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (i % 2 == 0)
                {
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(204, 229, 255);
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                }
                else
                {
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                }



                GridColumn colBatch2 = new GridColumn();
                colBatch2.Visible = true;
                colBatch2.Width = 90;
                colBatch2.Caption = i.ToString() + " 배치 편차";
                colBatch2.FieldName = "M" + i.ToString();
                DT.Columns.Add("M" + i.ToString(), typeof(double));
                colBatch2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                colBatch2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                colBatch2.DisplayFormat.FormatString = "0.000";
                colBatch2.OptionsColumn.AllowEdit = true;
                colBatch2.OptionsColumn.ReadOnly = true;
                colBatch2.SummaryItem.DisplayFormat = "{0:0.000} Kg";
                colBatch2.SummaryItem.FieldName = "M" + i.ToString();
                colBatch2.SummaryItem.Mode = DevExpress.Data.SummaryMode.AllRows;
                colBatch2.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                colBatch2.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(153, 204, 255);
                gridView_batchRun.Columns.Add(colBatch2);
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;


                if (i % 2 == 0)
                {
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(204, 229, 255);
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;

                }
                else
                {
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                }


                GridColumn colBatch3 = new GridColumn();
                colBatch3.Visible = true;
                colBatch3.Width = 110;
                colBatch3.Caption = i.ToString() + " 배치 시간";
                colBatch3.FieldName = "T" + i.ToString();
                DT.Columns.Add("T" + i.ToString(), typeof(int));
                colBatch3.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                colBatch3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                colBatch3.DisplayFormat.FormatString = "{0:n0}";
                colBatch3.OptionsColumn.AllowEdit = true;
                colBatch3.OptionsColumn.ReadOnly = true;

                colBatch3.SummaryItem.DisplayFormat = "{0:n0} MAX Sec";
                colBatch3.SummaryItem.FieldName = "T" + i.ToString();
                colBatch3.SummaryItem.Mode = DevExpress.Data.SummaryMode.AllRows;
                colBatch3.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                colBatch3.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(153, 204, 255);
                gridView_batchRun.Columns.Add(colBatch3);
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;


                if (i % 2 == 0)
                {
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(204, 229, 255);
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                }
                else
                {
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    gridView_batchRun.Columns[gridView_batchRun.Columns.Count - 1].AppearanceCell.Options.UseBackColor = true;
                }

            }

            gridControl_batchRun.DataSource = DT;
        }

        private void mix_result(string Plant_Code, string Process_Key, string l_Code, string sResourceNo, string sNOTE, string sBATCH_Q, bool chk_version, string rc_chk)
        {
            sBATCH_Q = sBATCH_Q == "" ? "0" : sBATCH_Q;

            if (!string.IsNullOrEmpty(sResourceNo))
            {
                if (gridView_batchRun.Columns.Count > 6)
                {
                    int maxColumnIndex = gridView_batchRun.Columns.Count - 1;

                    for (int i = maxColumnIndex; i > 5; i--)
                    {
                        gridView_batchRun.Columns.RemoveAt(i);
                    }
                }

                //gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, "NOTE", sNOTE);

                if (chk_version)
                {
                    DataSet resultMixDs = clsProcessDosing.resultMixResult(Plant_Code, Process_Key, l_Code, sResourceNo, sNOTE, sBATCH_Q, rc_chk);

                    if (resultMixDs == null || resultMixDs.Tables[0] == null) return;

                    // 중복 RESOURCE_NO 확인
                    var query = resultMixDs.Tables[0].AsEnumerable()
                                  .Select(r => r.Field<string>("RESOURCE_NO"))
                                  .GroupBy(x => x)
                                  .Where(g => g.Count() > 1)
                                  .ToList();

                    if (query.Count > 0)
                    {
                        // 중복된 RESOURCE_NO 리스트
                        foreach (var g in query)
                            Console.WriteLine($"중복값: {g.Key}, 갯수: {g.Count()}");
                    }

                    string dupBin = string.Empty;
                    bool binSeqDupChk = clsProcessDosing.BinSeqDupChk(Plant_Code, Process_Key, l_Code, sResourceNo, sNOTE, out dupBin);

                    if (!binSeqDupChk)
                    {
                        ShowMessageBox.XtraShowInformation("같은 원료빈이 중첩되는 빈이 존재합니다\r\n중첩된 빈 : " + dupBin);
                        gridControl_batchRun.DataSource = null;
                        return;
                    }

                    clsDevexpressGrid.BindGridControl(gridControl_batchRun, gridView_batchRun, resultMixDs.Tables[0], false, false);

                    sValid = new string[] { "" };


                    clsDevexpressGrid.ItemLookUpEditSetup(gridcbo_run_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()));
                }
                else
                {
                    gridControl_batchRun.DataSource = null;
                }
            }
            else
            {
                gridControl_batchRun.DataSource = null;
            }
        }

        /// <summary>
        /// 빈 관리
        /// 계획 상태 빈추가
        /// 진행중 빈변경, 빈보류
        /// </summary>
        private void binChangeEvent()
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView_batchRun.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("변경하실 계량내역 빈을 선택하여 주세요");
                return;
            }

            string selPLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
            string selPROCESS_KEY = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
            string selL_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");

            string selWorkDate = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "WORKDATE");
            string selNum = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "NUM");
            string selScale = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "SCALE_CODE");
            string selBinCd = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "LOCATION");
            string selResourcesNo = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "RESOURCE_NO");

            string selSetValue = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "SET_VAL");
            string selQtyPct = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "QTY_PCT");

            SQL = $@"
            SELECT C_CONDITION FROM WORK_ORDER
            WHERE PLANT_CODE = '{selPLANT_CODE}' AND PROCESS_KEY = '{selPROCESS_KEY}' AND L_CODE = '{selL_CODE}'
                AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}'
            ";

            DataSet workStDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(workStDs) == 1)
            {
                int run_work_flag = 0;

                if (Dbconn.conn.getData(workStDs, "C_CONDITION", 0) == clsCommon.PcStatus.Plan)
                {
                    run_work_flag = 1;
                }
                else if (Dbconn.conn.getData(workStDs, "C_CONDITION", 0) == clsCommon.GetPcStatusCode("진행"))
                {
                    run_work_flag = 2;
                }
                else
                {
                    ShowMessageBox.XtraShowInformation("계획, 진행 중인 작업지시만 빈변경을 하실 수 있습니다");
                    return;
                }

                m_binChange mBinChange = new m_binChange(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selWorkDate, selNum, selScale, selBinCd, selResourcesNo, selSetValue, selQtyPct, run_work_flag);
                mBinChange.TopMost = true;
                mBinChange.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = mBinChange.ShowDialog();
                if (result == DialogResult.OK)
                {
                    work_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selWorkDate, selNum);
                    batch_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selWorkDate, selNum);
                    batch_log(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selWorkDate, selNum);
                }
            }
            else
            {
                ShowMessageBox.XtraShowWarning("작업지시를 저장하신 후 빈을 변경하여 주세요");
            }
        }

        private void btn_binChange_Click(object sender, EventArgs e)
        {
            binChangeEvent();
        }

        private void repItemBtnEdit_binChange_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            binChangeEvent();
        }

        private void gridView_work_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridView_work_ShownEditor(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (view.FocusedColumn.FieldName == "NOTE")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;

                // NOTE 기존 값 백업
                object oldValue = view.GetFocusedRowCellValue("NOTE");

                // LookUpEdit 데이터 바인딩
                clsDevexpressUtil.ItemLookUpEditSetup(
                    edit,
                    clsCommon.getNote(
                        view.GetFocusedRowCellValue("PLANT_CODE")?.ToString(),
                        view.GetFocusedRowCellValue("RESOURCE_NO")?.ToString(),
                        "2"
                    ),
                    "",
                    false, 0, false, true, false
                );

                // 팝업 사용 시 BeginInvoke로 지연 실행 (중요)
                edit.BeginInvoke(new Action(() =>
                {
                    edit.ShowPopup();
                    edit.ClosePopup();
                }));

                // 에디터에 기존 값 복원
                edit.EditValue = oldValue;

                // 셀 값도 한번 더 복원 (DevExpress 안정성용)
                view.SetFocusedRowCellValue("NOTE", oldValue);

                edit.Properties.PopupFormMinSize = new Size(200, 300);
            }

            if (view.FocusedColumn.FieldName == "LOCATION_ED" || view.FocusedColumn.FieldName == "LOCATION_ED2")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.GetBin(cboPlant_Code.EditValue?.ToString(), cboPlant_Code.EditValue?.ToString() == ("P101") && cboProcessKey.EditValue?.ToString() == clsCommon.GetProcessKey("배합")?.ToString() ? clsCommon.GetProcessKey("펠렛")?.ToString() : "", "", "", view.GetFocusedRowCellValue("RESOURCE_NO")?.ToString()), "", true, 0, false, true, false, new string[] { "CODE", "NAME" }, null, "CODE", "CODE");
                edit.Properties.PopupFormMinSize = new Size(200, 300);
            }

            //if (view.FocusedColumn.FieldName == "RESOURCE_NO")
            //{
            //    SearchLookUpEdit edit = (SearchLookUpEdit)view.ActiveEditor;
            //    edit.ShowPopup();
            //    edit.ClosePopup();

            //    // 여기
            //    clsDevexpressUtil.ItemSearchLookUpEditSetup(edit, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString(), $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 2, true, false, "KG", false, true, true), "품목을 선택 해주세요.", false);
            //    edit.Properties.PopupFormMinSize = new Size(200, 300);
            //}
            if (view.FocusedColumn.FieldName == "RESOURCE_NO")
            {
                SearchLookUpEdit edit = view.ActiveEditor as SearchLookUpEdit;
                if (edit == null)
                    return;

                var data = clsCommon.GetResource(
                    cboPlant_Code.EditValue?.ToString(),
                    cboProcessKey.EditValue?.ToString(),
                    $"'{clsCommon.GetResourceTypeCode("포장재료")}'",
                    "",
                    2, true, false, "KG", false, true, true
                );

                clsDevexpressUtil.ItemSearchLookUpEditSetup(
                    edit,
                    data,
                    "품목을 선택 해주세요.",
                    false
                );

                edit.Properties.PopupFormMinSize = new Size(200, 300);
                edit.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                edit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                edit.Properties.ImmediatePopup = true;

                DevExpress.XtraGrid.Views.Grid.GridView popupView = edit.Properties.View;

                popupView.OptionsFind.AlwaysVisible = true;
                popupView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
                popupView.OptionsFind.HighlightFindResults = true;
                popupView.OptionsFind.FindFilterColumns = "CODE;NAME;TYPE";
                popupView.OptionsFind.ClearFindOnClose = false;

                edit.Popup -= Edit_Popup;
                edit.Popup += Edit_Popup;

                BeginInvoke(new Action(() =>
                {
                    edit.ShowPopup();
                }));
            }
        }

        private void Edit_Popup(object sender, EventArgs e)
        {
            SearchLookUpEdit edit = sender as SearchLookUpEdit;
            if (edit == null)
                return;

            DevExpress.XtraGrid.Views.Grid.GridView view = edit.Properties.View;

            if (view != null)
            {
                view.ShowFindPanel();
                view.Focus();
            }
        }

        private void frm_Dosing_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (clsCommon._strMainPlcConnYn == "Y")
            //{
            //    DialogResult result = ShowMessageBox.Confirm("안내", "도징계량도중에 도징화면을 닫게 되시면 계량데이터 수집을 할 수 없습니다\r\n종료하시겠습니까?");

            //    if (result == DialogResult.Yes)
            //    {
            //        MAIN.MainPlcConnChk = "M";
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }

        private void reflashWorkTable(string Plant_Code, string Process_Key, string l_Code, string workdate, string num)
        {
            XMain_Search();
            work_result(Plant_Code, Process_Key, l_Code, workdate, num);
            batch_result(Plant_Code, Process_Key, l_Code, workdate, num);
            batch_log(Plant_Code, Process_Key, l_Code, workdate, num);
        }

        private void gridView_batchRun_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void gridView_work_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (e.FocusedColumn == null)
                return;

            if (e.FocusedColumn.FieldName == "RESOURCE_NO")
            {
                gridView_work.ShowEditor();

                if (((SearchLookUpEdit)gridView_work.ActiveEditor) == null)
                    return;

                ((SearchLookUpEdit)gridView_work.ActiveEditor).ShowPopup();
            }

            if (e.FocusedColumn.FieldName == "LOCATION_ED" || e.FocusedColumn.FieldName == "LOCATION_ED2")
            {
                gridView_work.ShowEditor();

                if (((LookUpEdit)gridView_work.ActiveEditor) == null)
                    return;

                ((LookUpEdit)gridView_work.ActiveEditor).ShowPopup();
            }
        }

        /// <summary>
        /// 작업지시 선택시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_work_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (e.FocusedRowHandle == DevExpress.XtraGrid.GridControl.NewItemRowHandle)
            //{
            //    NewRowInit();
            //}

            if (e.FocusedRowHandle >= 0 && e.FocusedRowHandle < gridView_work.RowCount)
            {
                try
                {
                    gridControl_batchRun.DataSource = null;
                    gridControl_Tab2.DataSource = null;
                    gridControl_Tab4.DataSource = null;

                    DataRow row = gridView_work.GetDataRow(gridView_work.FocusedRowHandle);
                    string sPlantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");

                    bool chk_version = false;
                    decimal dNote_Per = 0;
                    string sRESOURCE_NO = gridView_work.GetRowCellValue(e.FocusedRowHandle, gridView_work.Columns["RESOURCE_NO"]).ToString();
                    string sNOTE = clsCommon.getLastVersion(sPlantCode
                        , clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO")
                        , out chk_version, out dNote_Per);
                    string sBATCH_Q = gridView_work.GetRowCellValue(e.FocusedRowHandle, gridView_work.Columns["BATCH_Q"]).ToString();
                    string bu_yn = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BU_YN");

                    string selPLANT_CODE = sPlantCode;
                    string selPROCESS_KEY = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
                    string selL_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");

                    string selData_workdate = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
                    string selData_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");

                    SetCondition();

                    if (row.RowState == DataRowState.Added)
                    {
                        mix_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, sRESOURCE_NO, sNOTE, sBATCH_Q, chk_version, bu_yn);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(selData_workdate) && !string.IsNullOrEmpty(selData_num))
                        {
                            work_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selData_workdate, selData_num);
                            batchList_result(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selData_workdate, selData_num);
                            //batch_result(selData_workdate, selData_num);
                            batch_log(selPLANT_CODE, selPROCESS_KEY, selL_CODE, selData_workdate, selData_num);
                        }
                    }

                    GetRemark(false);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                gridControl_batchRun.DataSource = null;
                gridControl_Tab2.DataSource = null;
                gridControl_Tab4.DataSource = null;
            }
        }


        private void gridView_work_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "C_CONDITION")
            {
                int sumText = 0;

                string workdate = string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue);

                SQL = $@"
                SELECT FLOOR(NVL(
                                (SUM(A.PRO_Q) / 
                                (SUM(A.MIN_SUM) - 
                                    NVL(SUM((SELECT REST_MINUTES
                                     FROM REST_TIME
                                     WHERE PLANT_CODE = '{gridView_work.GetFocusedRowCellValue("PLANT_CODE")}'
                                        AND PROCESS_KEY = '{gridView_work.GetFocusedRowCellValue("PROCESS_KEY")}'
                                        AND L_CODE = '{gridView_work.GetFocusedRowCellValue("L_CODE")}'
                                        AND WORKDATE = TO_DATE('{workdate}', 'YYYYMMDD')
                                    )), 0)
                                ) ) * 60, 0)) AS PROVITY
                FROM (
                    SELECT WORKDATE, 
                           SUM(PRO_Q) AS PRO_Q,
                           SUM(CASE 
                                   WHEN START_TIME IS NOT NULL AND END_TIME IS NOT NULL THEN 
                                       (END_TIME - START_TIME) * 1440 
                                   ELSE 0 
                               END) AS MIN_SUM
                    FROM WORK_ORDER
                    WHERE PLANT_CODE = '{gridView_work.GetFocusedRowCellValue("PLANT_CODE")}'
                        AND PROCESS_KEY = '{gridView_work.GetFocusedRowCellValue("PROCESS_KEY")}'
                        AND L_CODE = '{gridView_work.GetFocusedRowCellValue("L_CODE")}'
                        AND NVL(DEL_FLAG, 'N') != 'Y'
                        AND WORKDATE = TO_DATE('{workdate}', 'YYYYMMDD')
                        AND C_CONDITION = '{clsCommon.PcStatus.Completed}'
                        AND (END_TIME - START_TIME) * 1440 > 0
                    GROUP BY WORKDATE
                ) A
                ";

                DataSet proDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(proDs) > 0)
                {
                    sumText = Convert.ToInt32(Dbconn.conn.getData(proDs, "PROVITY", 0));
                }

                e.Info.DisplayText = "생산성 : " + String.Format("{0:#,###}", sumText);
            }
        }

        private void gridcboNOTE_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit textEditor = (DevExpress.XtraEditors.TextEdit)sender;
            string sPlantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
            string sProcessKey = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
            string sLCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");

            string sResourceNo = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO");
            string sBatchQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q");
            string bu_yn = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BU_YN");

            mix_result(sPlantCode, sProcessKey, sLCode, sResourceNo, textEditor.EditValue?.ToString(), sBatchQ, chk_version, bu_yn);
        }

        /// <summary>
        /// 재가공 투입여부 체크 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridCHK_CheckedChanged(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView_work);

            CheckEdit bu_yn = (CheckEdit)sender;
            string sPlantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
            string sProcessKey = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
            string sLCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");
            string sResourceNo = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO");
            string sNote = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NOTE");
            string sBatchQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q");
            string sBatch = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH");

            if (gridView_work.FocusedColumn.FieldName != "BU_YN")
                return;

            SQL = $@"
            SELECT SUM(a.PART_P) AS MENGE, 'Y' AS BU_YN
            FROM SAP_IN_PRODUCT_RC a
            WHERE a.PLANT_CODE = '{sPlantCode}'
                AND a.RESOURCE_NO = '{sResourceNo}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);



            if ((bu_yn.Checked == true && ds != null && ds.Tables[0].Rows.Count == 0) || ds.Tables[0].Rows[0]["MENGE"].ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("등록된 부산물이 없습니다.");
                gridView_work.SetFocusedRowCellValue("BU_YN", "N");
                bu_yn.Checked = false;

                return;
            }

            vdMerge = bu_yn.Checked == true ? decimal.Parse(ds.Tables[0].Rows[0]["MENGE"].ToString()) : 0;

            if (!string.IsNullOrEmpty(sBatchQ) && !string.IsNullOrEmpty(sBatch))
                gridView_work.SetFocusedRowCellValue("OR_Q", (int.Parse(sBatchQ) * int.Parse(sBatch) + (bu_yn.Checked == true ? ((int.Parse(sBatchQ) * (vdMerge * int.Parse(sBatch))) / 100) : 0)).ToString());          // 지시량 = 배치수 * 배치량 + (배치수 * 부산물량 * 배치량 / 100)


            SQL = $@"
            SELECT {int.Parse(sBatchQ) * int.Parse(sBatch) + (bu_yn.Checked == true ? ((int.Parse(sBatchQ) * (vdMerge * int.Parse(sBatch))) / 100) : 0)} + {int.Parse(sBatchQ)} * (a.PART_P / 100) AS MENGE
            FROM SAP_IN_UPPRODUCT_CP a
            WHERE a.PLANT_CODE = '{sPlantCode}'
                AND a.RESOURCE_NO = '{sResourceNo}'
            ";

            ds = Dbconn.conn.ExecutDataset(SQL);

            gridView_work.SetFocusedRowCellValue("SUM_OR_Q", Dbconn.conn.getData(ds, "MENGE", 0));

            gridView1.InvalidateFooter(); // Footer 강제 갱신

            clsDevexpressGrid.GridViewAddRow(gridView_batchRun);

            SetBatchResult();
        }

        private void gridscboRESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit textEditor = (DevExpress.XtraEditors.TextEdit)sender;

            string sPlantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
            string sProcessKey = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
            string sLCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");

            string sResourceNo = textEditor.EditValue?.ToString();
            string sBatchQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q");

            gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, "RESOURCE_NO", textEditor.EditValue);

            gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, "P_TYPE", 2);

            string sNOTE = clsCommon.getLastVersion(sPlantCode, sResourceNo, out chk_version, out dNote_Per);

            gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, "NOTE", sNOTE);

            mix_result(sPlantCode, sProcessKey, sLCode, sResourceNo, sNOTE, sBatchQ, chk_version, "N");
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
                btn_workAdd_Click(sender, e);
            }

            // 행 삭제
            if (e.KeyCode == Keys.Delete)
            {
                btn_workDelete_Click(sender, e);
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
            gridControl_work.Focus();
            gridView_work.FocusedRowHandle = 0;
            gridView_work.FocusedColumn = gridView_work.VisibleColumns[0];
        }

        private void repItemLkUpEdit_T_BIN_ProcessNewValue(object sender, ProcessNewValueEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.DisplayValue?.ToString()))
            {
                e.Handled = true;

                SQL = $@"
                SELECT LOCATION FROM BIN WHERE PLANT_CODE = '{gridView_work.GetFocusedRowCellValue("PLANT_CODE")}' AND RESOURCE_NO = '{gridView_work.GetFocusedRowCellValue("RESOURCE_NO")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {

                }

                // 입력한 값을 현재 포커스된 셀에 직접 저장
                DevExpress.XtraGrid.Views.Grid.GridView view = (gridControl_work.FocusedView as DevExpress.XtraGrid.Views.Grid.GridView);

                if (view != null && view.FocusedColumn != null)
                {
                    var dt = repItemLkUpEdit_T_BIN.DataSource as DataTable;
                    string input = e.DisplayValue.ToString();

                    if (dt != null && !dt.AsEnumerable().Any(r => r["CODE"].ToString() == input))
                    {
                        dt.Rows.Add(input, input); // Code, Name
                    }

                    view.SetRowCellValue(view.FocusedRowHandle, "LOCATION_ED", e.DisplayValue.ToString());
                }
            }
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridControl_work.DataSource = null;
                gridControl_batchRun.DataSource = null;
                gridControl_Tab2.DataSource = null;
                gridControl_Tab4.DataSource = null;

                // 공정
                clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString(), clsCommon.GetProcessTypeCode("배합"), "", vProcessName), "", false, 0, false);
                cboProcessKey.EditValue = vProcess_Code;

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);
                cboL_Code.EditValue = vLine_Code;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void cboProcessKey_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridControl_work.DataSource = null;
                gridControl_batchRun.DataSource = null;
                gridControl_Tab2.DataSource = null;
                gridControl_Tab4.DataSource = null;

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, false);
                cboL_Code.EditValue = vLine_Code;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridControl_work.DataSource = null;
                gridControl_batchRun.DataSource = null;
                gridControl_Tab2.DataSource = null;
                gridControl_Tab4.DataSource = null;

                //vLine_Code = cboL_Code.EditValue?.ToString();

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        public void RunCustomFunction()
        {
            XMain_Search();

            gridView_batchRun.LeftCoord += 100000;
        }

        private void btnBinManage_Click(object sender, EventArgs e)
        {
            binChangeEvent();
        }

        private void gridView_batchRun_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (gridView_work != null && gridView_work.GetFocusedRowCellValue("BU_YN")?.ToString() == "Y")
            {
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize &&
                e.IsTotalSummary && ((GridSummaryItem)e.Item).FieldName == "RESOURCE_NO")
                {
                    string sResourceNo = gridView_work.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();
                    string sNote = gridView_work.GetFocusedRowCellValue("NOTE")?.ToString();

                    SQL = $@"
                    SELECT SUM(MENGE) AS MENGE
                    FROM (
                        SELECT a.MENGE
                        FROM SAP_IN_BOM_COND a
                        WHERE a.PLANT_CODE = '{gridView_work.GetFocusedRowCellValue("PLANT_CODE")}'
                                AND a.RESOURCE_NO = '{sResourceNo}' 
                                AND a.NOTE = '{sNote}'
                                AND a.P_TYPE = '2'
                                AND a.IDNRK NOT IN (
                                    SELECT RESOURCE_NO_2 
                                    FROM SAP_IN_PRODUCT_CP 
                                    WHERE RESOURCE_NO = a.RESOURCE_NO AND RESOURCE_NO_2 = a.IDNRK
                                )
                        UNION ALL
                        SELECT a.MENGE
                        FROM SAP_IN_BOM_COND a
                            INNER JOIN SAP_IN_PRODUCT_CP b 
                                ON b.RESOURCE_NO = a.RESOURCE_NO AND b.RESOURCE_NO_2 = a.IDNRK
                        WHERE a.PLANT_CODE = '{gridView_work.GetFocusedRowCellValue("PLANT_CODE")}'
                            AND a.RESOURCE_NO = '{sResourceNo}' 
                            AND a.NOTE = '{sNote}'
                            AND a.P_TYPE = '2'
                        )
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    e.TotalValue = $"부산물 제외 배합비 : {ds.Tables[0].Rows[0]["MENGE"]} %";  // 임의의 텍스트
                }
            }
        }

        private void gridView_batchRun_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            // 예: "AMOUNT" 컬럼의 합계

            if (gridView_work.GetFocusedRowCellValue("BU_YN")?.ToString() == "N" && e.Column != null && e.Column.FieldName == "RESOURCE_NO")
            {
                e.Handled = true;  // 셀 자체를 안 그림 → 완전히 안 보임
            }
            else if (gridView_work.GetFocusedRowCellValue("BU_YN")?.ToString() == "Y" && e.Column != null && e.Column.FieldName == "RESOURCE_NO")
            {
                e.Handled = false;  // 셀 자체를 안 그림 → 완전히 안 보임
            }


            if (e.Column.FieldName == "SET_VAL")
            {
                object summaryValue = view.Columns["SET_VAL"].SummaryItem.SummaryValue;

                if (summaryValue != null && summaryValue != DBNull.Value)
                {
                    double sum = Convert.ToDouble(summaryValue);

                    // ✅ 합계의 절반
                    double half = sum / 2.0;

                    // ✅ 소수점 3자리 고정 포맷
                    e.Info.DisplayText = half.ToString().Merge(" KG");
                }
            }

            if (e.Column.FieldName == "QTY_PCT")
            {
                object summaryValue = view.Columns["QTY_PCT"].SummaryItem.SummaryValue;

                if (summaryValue != null && summaryValue != DBNull.Value)
                {
                    double sum = Convert.ToDouble(summaryValue);

                    // ✅ 합계의 절반
                    double half = sum / 2.0;

                    // ✅ 소수점 3자리 고정 포맷
                    e.Info.DisplayText = half.ToString().Merge(" %");
                }
            }

            if (e.Column.FieldName.Substring(0, 1) == "T")
            {
                int sumText = 0;
                string scale = string.Empty;

                SQL = $@"
                SELECT A.SCALE_CODE, A.PQ_TIME AS PQ_TIME
                FROM (
                    SELECT B.SCALE_CODE, SUM(a.P_Q_TIME) AS PQ_TIME
                    FROM WORK_REMARK a
                        LEFT OUTER JOIN BIN B ON B.PLANT_CODE = a.PLANT_CODE AND B.PROCESS_KEY = a.PROCESS_KEY AND B.L_CODE = a.L_CODE 
                                            AND a.LOCATION = B.LOCATION
                    WHERE a.PLANT_CODE = '{gridView_work.GetFocusedRowCellValue("PLANT_CODE")}'
                        AND a.PROCESS_KEY = '{gridView_work.GetFocusedRowCellValue("PROCESS_KEY")}'
                        AND a.L_CODE = '{gridView_work.GetFocusedRowCellValue("L_CODE")}'
                        AND a.WORKDATE = '{clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE")}'
                        AND a.NUM = '{clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM")}'
                        AND a.BATCH = '{e.Column.FieldName.Substring(1, 1)}'
                    GROUP BY B.SCALE_CODE
                    ORDER BY PQ_TIME DESC
                ) A
                WHERE ROWNUM = 1
                ";

                DataSet pqDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(pqDs) > 0)
                {
                    sumText = Convert.ToInt32(Dbconn.conn.getData(pqDs, "PQ_TIME", 0));
                    scale = Dbconn.conn.getData(pqDs, "SCALE_CODE", 0);
                }

                e.Info.DisplayText = scale + "/" + String.Format("{0:#,###}", sumText) + "초";
                e.Painter.DrawObject(e.Info);
                e.Handled = true;
            }
        }

        private void work_watch_timer_Tick(object sender, EventArgs e)
        {
            work_watch_timer.Stop();
            string sPlantCode = string.Empty;
            string sProcessKey = string.Empty;
            string sLCode = string.Empty;
            string sWorkDate = string.Empty;
            string sNum = string.Empty;
            int[] tmpDatas = new int[20];
            string vBatchNum = string.Empty;
            string vResourceName = string.Empty;

            try
            {
                DataSet ds = null;

                if (!vWorkStart) return;

                if (MAIN.MainPlcConnChk != "Y") return;

                DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

                int tPLC_Location = clsCommon.GetPlcLocation(vPlant_Code, cboProcessKey.EditValue?.ToString());

                clsCommon.GetPLCAddress(vPlant_Code
                                            , vProcess_Code
                                            , vLine_Code
                                            // GBG
                                            , tPLC_Location
                                            // GBG -
                                            , PlcAddressType.ISWORKABLE.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                if (!PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, tPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, "도징계량에서 작업지시시작이 불가능한 상태입니다"))
                {
                    ShowMessageBox.XtraShowError("도징계량에서 작업지시시작이 불가능한 상태입니다", "알림");
                    return;
                }

                if (tmpDatas[0] != 1)
                {
                    clsCommon.GetPLCAddress(vPlant_Code
                                            , vProcess_Code
                                            , vLine_Code
                                            //, dtPlc.Rows[0]["PLC_TYPE"]?.ToString() == "Q" ? 1 : 2
                                            , tPLC_Location
                                            , PlcAddressType.WORKINFO.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                    PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, tPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, "배합 작지 PLC 읽기 실패", Cimon_Job_Data, WORK_YYYY);

                    sWorkDate = tmpDatas[0].ToString().PadLeft(4, '0') + tmpDatas[1].ToString().PadLeft(4, '0');

                    sNum = tmpDatas[2].ToString();

                    clsCommon.GetPLCAddress(vPlant_Code
                                            , vProcess_Code
                                            , vLine_Code
                                            , tPLC_Location
                                            , PlcAddressType.CURRENTBATCH.GetDesc()
                                            , 1
                                            , out vPLCAddress
                                            , out vPLCUnit
                                            , out vPLCDataCount);

                    bool result = PlcFunc.PlcGetReadQDeviceBlockEx(dtPlc, tPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref tmpDatas, "현재 배치 수 읽기 실패", Cimon_Job_Data, BATCH_PV);

                    if (!result)
                    {
                        clsLog.logSave(this, "", "PLC 읽기 실패");
                        return;
                    }

                    if (tmpDatas == null || tmpDatas.Length == 0)
                    {
                        clsLog.logSave(this, "", "PLC 데이터 없음");
                        return;
                    }

                    vBatchNum = tmpDatas[0].ToString();

                    SQL = $@"
                    SELECT WO.PROCESS_KEY, WO.RESOURCE_NO, P.DESCRIPTION
                    FROM WORK_ORDER WO
                        JOIN SAP_DI_PRODUCT P ON WO.RESOURCE_NO = P.RESOURCE_NO
                    WHERE WO.PLANT_CODE   = '{vPlant_Code}'
                        AND WO.PROCESS_KEY  IN ('{vProcess_Code}')
                        AND WO.L_CODE       = '{vLine_Code}'
                        AND WO.WORKDATE     = '{sWorkDate}'
                        AND WO.NUM          = '{sNum}'
                    ";

                    using (DataSet pSearchDs = Dbconn.conn.ExecutDataset(SQL))
                    {
                        if (Dbconn.conn.getRowCnt(pSearchDs) > 0)
                        {
                            vResourceName = Dbconn.conn.getData(pSearchDs, "DESCRIPTION", 0);
                        }
                    }

                    txtEdt_runWorkNum.Text = sWorkDate;
                    txtEdt_runNum.Text = sNum;
                    txtEdt_runWorkBatch.Text = vBatchNum;
                    txtEdt_runWorkProduct.Text = vResourceName;
                }
                else
                {
                    txtEdt_runWorkNum.Text = string.Empty;
                    txtEdt_runNum.Text = string.Empty;
                    txtEdt_runWorkBatch.Text = string.Empty;
                    txtEdt_runWorkProduct.Text = string.Empty;
                }

                //작업지시 연동모드일 경우 다음 작업지시 시작
                if (cboOperWorkMode.EditValue?.ToString() == "연동")
                {
                    string SQL = $@"
                    SELECT PLANT_CODE
                       , PROCESS_KEY
                       , L_CODE
                       , WORKDATE
                       , NUM
                       , RESOURCE_NO
                       , NOTE
                    FROM WORK_ORDER
                    WHERE PLANT_CODE = '{vPlant_Code}'
                        AND PROCESS_KEY = '{vProcess_Code}'
                        AND L_CODE = '{vLine_Code}'
                        AND C_CONDITION = '{clsCommon.PcStatus.InProgress}'
                        AND NVL(DEL_FLAG, 'N') != 'Y'
                        AND WORKDATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                    ORDER BY NUM
                    ";

                    ds = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(ds) == 0)
                    {
                        SQL = $@"
                        SELECT PLANT_CODE
                           , PROCESS_KEY
                           , L_CODE
                           , WORKDATE
                           , NUM
                           , RESOURCE_NO
                           , NOTE
                        FROM WORK_ORDER
                        WHERE PLANT_CODE = '{vPlant_Code}'
                            AND PROCESS_KEY = '{vProcess_Code}'
                            AND L_CODE = '{vLine_Code}'
                            AND C_CONDITION = '{clsCommon.PcStatus.Plan}'
                            AND NVL(DEL_FLAG, 'N') != 'Y'
                            AND WORKDATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                        ORDER BY NUM";

                        using (DataSet NextWorkDs = Dbconn.conn.ExecutDataset(SQL))
                        {
                            if (Dbconn.conn.getRowCnt(NextWorkDs) > 0)
                            {
                                sPlantCode = Dbconn.conn.getData(NextWorkDs, "PLANT_CODE", 0);
                                sProcessKey = Dbconn.conn.getData(NextWorkDs, "PROCESS_KEY", 0);
                                sLCode = Dbconn.conn.getData(NextWorkDs, "L_CODE", 0);
                                sWorkDate = Dbconn.conn.getData(NextWorkDs, "WORKDATE", 0);
                                sNum = Dbconn.conn.getData(NextWorkDs, "NUM", 0);

                                clsUtil.Delay(2600);

                                if (!PlcFunc.GetPlcCon(dtPlc, out MAIN.sErrMsg))
                                {
                                    ShowMessageBox.XtraShowInformation(MAIN.sErrMsg);
                                    return;
                                }

                                //전송유무 체크
                                bool isChkStart = XWorkStart(dtPlc, sPlantCode, sProcessKey, sLCode, sWorkDate, sNum);
                                if (!isChkStart)
                                {
                                    return;
                                }

                                XMain_Search();
                            }

                            work_watch_timer.Start();
                        }
                    }

                    clsUtil.Delay(500);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "work_watch_timer_Tick(object sender, EventArgs e)", ex);
                ShowMessageBox.XtraShowWarning("연동 처리하는 도중 에러가 발생했습니다");
            }
            finally
            {
                work_watch_timer.Start();
            }
        }

        private void gridView_batchRun_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (gridView_batchRun.GetRowCellValue(e.RowHandle, gridView_batchRun.Columns["LOCATION"]).ToString() == "소계")
            {
                e.Appearance.BackColor = Color.LightGray;
                e.Appearance.ForeColor = Color.DarkGreen;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
        }

        private void txtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = null;

                if (ShowMessageBox.Confirm("입력하신 전달사항을 저장하시겠습니까?") == DialogResult.Yes)
                {
                    try
                    {
                        DataTable dt = GetRemark(true);

                        if (dt != null)
                        {
                            dr = dt.Rows[0];

                            if (dr != null && dr["RESOURCE_NO"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["RESOURCE_NO"].ToString()))
                            {
                                SQL = $@"
                                UPDATE WORK_ORDER
                                SET    REMARK          = '{txtRemark.EditValue}'
                                WHERE  PLANT_CODE      = '{dr["PLANT_CODE"]}'
                                    AND    PROCESS_KEY     = '{dr["PROCESS_KEY"]}'
                                    AND    L_CODE          = '{dr["L_CODE"]}'
                                    AND    WORKDATE        = '{dr["WORKDATE"]}'
                                    AND    NUM             = '{dr["NUM"]}'
                                ";

                                if (Dbconn.conn.SQLrun(SQL) < 0)
                                {
                                    clsLog.logSave(this.Text, "btn_save_Click", SQL);
                                    dr.RowError = "데이터 수정에 실패했습니다";
                                    ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                                    return;
                                }
                            }
                        }

                        ShowMessageBox.XtraShowInformation("전달사항이 저장 되었습니다.");

                        tgRemark.IsOn = true;
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
            }
        }

        private DataTable GetRemark(bool isCheck)
        {
            tgRemark.IsOn = true;

            SQL = $@"
            SELECT a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.NUM, a.RESOURCE_NO, a.REMARK
            FROM WORK_ORDER a
            WHERE a.PLANT_CODE = '{gridView_work.GetFocusedRowCellValue("PLANT_CODE")}'
                AND a.PROCESS_KEY = '{gridView_work.GetFocusedRowCellValue("PROCESS_KEY")}'
                AND a.L_CODE = '{gridView_work.GetFocusedRowCellValue("L_CODE")}'
                AND NVL(a.DEL_FLAG,'N') != 'Y'
                AND a.WORKDATE = '{gridView_work.GetFocusedRowCellValue("WORKDATE")}'
                AND a.NUM = '{gridView_work.GetFocusedRowCellValue("NUM")}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["REMARK"]?.ToString()) && ds.Tables[0].Rows[0]["REMARK"]?.ToString() != "")
                {
                    tgRemark.IsOn = true;

                    if (txtRemark.EditValue == ds.Tables[0].Rows[0]["REMARK"]?.ToString())
                        txtRemark.EditValue = ds.Tables[0].Rows[0]["REMARK"]?.ToString();
                }
                else if (!isCheck)
                {
                    tgRemark.IsOn = false;
                    txtRemark.EditValue = "";
                }
                return ds.Tables[0];
            }
            else if (isCheck)
            {
                tgRemark.IsOn = false;
                ShowMessageBox.XtraShowWarning("작업지시를 먼저 저장 해주세요.");
                return null; // 값이 없으면 null 반환
            }

            return null;
        }

        private void tRemarkText_Tick(object sender, EventArgs e)
        {
            var color = isVisible ? Color.Blue : Color.Red;

            txtRemark.Properties.Appearance.ForeColor = color;
            txtRemark.Properties.Appearance.Options.UseForeColor = true;
            txtRemark.LookAndFeel.UseDefaultLookAndFeel = false;

            isVisible = !isVisible;

            txtRemark.Refresh();
        }

        private void tgRemark_Toggled(object sender, EventArgs e)
        {
            if (tgRemark.IsOn)
            {
                txtRemark.ReadOnly = true;
                tRemarkText.Start();
            }
            else
            {
                txtRemark.ReadOnly = false;
                tRemarkText.Stop();
                txtRemark.ForeColor = Color.Black;
            }
        }

        private void gridView_batchRun_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridView_batchRun.UpdateCurrentRow();

            string sPlantCode = gridView_work.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
            string sProcessKey = gridView_work.GetFocusedRowCellValue("PROCESS_KEY")?.ToString();
            string sLCode = gridView_work.GetFocusedRowCellValue("L_CODE")?.ToString();
            string sWorkDate = gridView_work.GetFocusedRowCellValue("WORKDATE")?.ToString();
            string sNum = gridView_work.GetFocusedRowCellValue("NUM")?.ToString();
            string sBatch = gridView_work.GetFocusedRowCellValue("BATCH")?.ToString();

            string sLocation = gridView_batchRun.GetFocusedRowCellValue("LOCATION")?.ToString();
            string sResource = gridView_batchRun.GetFocusedRowCellValue("RESOURCE_NO")?.ToString();
            string sDescription = gridView_batchRun.GetFocusedRowCellValue("DESCRIPTION")?.ToString();

            string sQtyPct = gridView_batchRun.GetFocusedRowCellValue("QTY_PCT")?.ToString();
            string sSetVal = gridView_batchRun.GetFocusedRowCellValue("SET_VAL")?.ToString();

            if (e.Column.FieldName == "SET_VAL")
            {
                DialogResult result = ShowMessageBox.Confirm($"{sLocation} : {sDescription} 빈의 설정값(Kg)을 변경 하시겠습니까?");

                if (result == DialogResult.Yes)
                {
                    SQL = $@"
                    UPDATE WORK_DETAIL
                    SET SET_VAL = '{sSetVal}', I_TIME = SYSDATE
                    WHERE PLANT_CODE = '{sPlantCode}'
                        AND PROCESS_KEY = '{sProcessKey}'
                        AND L_CODE = '{sLCode}'
                        AND WORKDATE = '{sWorkDate}'
                        AND NUM = '{sNum}'
                        AND LOCATION = '{sLocation}'
                        AND INGRED_CODE = '{sResource}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        this.DialogResult = DialogResult.None;
                        clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                        ShowMessageBox.XtraShowInformation("빈 설정값을 변경 하는 도중 오류가 발생했습니다");
                        return;
                    }

                    string errMsg = $"[{sLocation}] 빈 설정값이 [{sSetVal}] 으로 변경 되었습니다.";

                    if (!clsProcessDosing.InsertLog(sPlantCode, sProcessKey, sLCode, sWorkDate, sNum, "0", clsCommon.GetPcStatusCode("계획"), errMsg))
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave("btn_ok_Click", "btn_ok_Click", SQL);
                        return;
                    }

                    SQL = $@"
                    SELECT SUM(SET_VAL) * '{sBatch}' AS SETVAL
                    FROM WORK_DETAIL
                    WHERE PLANT_CODE = '{sPlantCode}'
                        AND PROCESS_KEY = '{sProcessKey}'
                        AND L_CODE = '{sLCode}'
                        AND WORKDATE = '{sWorkDate}'
                        AND NUM = '{sNum}'
                        AND INGRED_CODE NOT IN (SELECT c.COMM_DTCODE AS CODE
                                                FROM COMM_DIV a
                                                    INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                                                    INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                                                WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '80')
                    ";

                    DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                    string sSetValSum = Dbconn.conn.getData(ds, "SETVAL", 0);
                    clsProcessDosing.SetWorkOrderQty(sPlantCode, sProcessKey, sLCode, sWorkDate, sNum);

                    gridView_work.SetFocusedRowCellValue("OR_Q", sSetValSum);

                    ShowMessageBox.XtraShowInformation($"설정값(Kg)을 {sSetVal} 으로 변경 했습니다.");
                }
            }

            if (e.Column.FieldName == "QTY_PCT")
            {
                DialogResult result = ShowMessageBox.Confirm($"{sLocation} : {sDescription} 빈의 배합률(%)을 변경 하시겠습니까?");

                if (result == DialogResult.Yes)
                {
                    SQL = $@"
                    UPDATE WORK_DETAIL
                    SET QTY_PCT = '{sQtyPct}', I_TIME = SYSDATE
                    WHERE PLANT_CODE = '{sPlantCode}'
                        AND PROCESS_KEY = '{sProcessKey}'
                        AND L_CODE = '{sLCode}'
                        AND WORKDATE = '{sWorkDate}'
                        AND NUM = '{sNum}'
                        AND LOCATION = '{sLocation}'
                        AND INGRED_CODE = '{sResource}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        Dbconn.conn.Rollback();
                        this.DialogResult = DialogResult.None;
                        clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                        ShowMessageBox.XtraShowInformation("빈 배합률을 변경 하는 도중 오류가 발생했습니다");
                        return;
                    }

                    string errMsg = $"[{sLocation}] 빈 배합률이 [{sQtyPct}] 으로 변경 되었습니다.";

                    if (!clsProcessDosing.InsertLog(sPlantCode, sProcessKey, sLCode, sWorkDate, sNum, "0", clsCommon.GetPcStatusCode("계획"), errMsg))
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave("btn_ok_Click", "btn_ok_Click", SQL);
                        return;
                    }

                    ShowMessageBox.XtraShowInformation($"배합률(%)을 {sQtyPct} 으로 변경 했습니다.");
                }
            }
        }

        private void gridView_batchRun_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (gridView_batchRun.IsNewItemRow(gridView_batchRun.FocusedRowHandle))
                {
                    // 필요 시 편집 자체를 막고 싶다면:
                    // e.Cancel = true;
                    return;
                }

                // 031004	완료
                if (gridView_work.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.PcStatus.Plan))  //작지가 진행중일 경우 배치수만 수정가능하도록 변경
                {
                    switch (gridView_batchRun.FocusedColumn.FieldName)
                    {
                        case "SET_VAL":
                        case "QTY_PCT":
                            if (clsCommon.GetSettingChangeYn(gridView_batchRun.GetFocusedRowCellValue("RESOURCE_NO")?.ToString()))
                                e.Cancel = false;
                            else
                                e.Cancel = true;
                            break;

                        default:
                            e.Cancel = true;
                            break;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_batchRun_ShowingEditor", ex);
            }
        }

        private void cboOperWorkMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboOperWorkMode.SelectedIndex == 1)
            //{
            //    work_watch_timer.Interval = 5000;
            //    work_watch_timer.Enabled = true;
            //}
            //else
            //{
            //    work_watch_timer.Enabled = false;
            //}
        }

        private void btnWorkStart_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView_work.RowCount == 0)
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
                DataTable DT = (DataTable)gridControl_work.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("선택된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                string sPlantCode = gridView_work.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                string sProcessKey = gridView_work.GetFocusedRowCellValue("PROCESS_KEY")?.ToString();
                string sLCode = gridView_work.GetFocusedRowCellValue("L_CODE")?.ToString();
                string sWorkDate = gridView_work.GetFocusedRowCellValue("WORKDATE")?.ToString();
                string sNum = gridView_work.GetFocusedRowCellValue("NUM")?.ToString();

                SQL = $@"
                SELECT START_TIME
                FROM WORK_ORDER
                WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                    AND WORKDATE = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{sNum}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {

                    if (ds.Tables[0].Rows[0]["START_TIME"]?.ToString() != "")
                    {
                        ShowMessageBox.XtraShowWarning("이미 시작된 작업입니다.");
                        return;
                    }


                    SQL = $@"
                    /* 작업 시작시간 업데이트 */
                    UPDATE WORK_ORDER
                    SET START_TIME = SYSDATE
                        , C_CONDITION = '{clsCommon.GetPcStatusCode("진행")}'
                    WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                        AND WORKDATE = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{sNum}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        clsLog.logSave(this.Text, "btnWorkStart_Click", SQL);
                        ShowMessageBox.XtraShowWarning("작업 시작이 실패했습니다");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btnWorkStart_Click", ex.Message + "/" + ex.StackTrace);
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
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

            if (gridView_work.RowCount == 0)
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
                DataTable DT = (DataTable)gridControl_work.DataSource;

                if (DT.Rows.Count == 0)
                {
                    ShowMessageBox.XtraShowInformation("선택된 데이터가 없습니다");
                    return;
                }

                if (DT == null)
                {
                    return;
                }

                string sPlantCode = gridView_work.GetFocusedRowCellValue("PLANT_CODE")?.ToString();
                string sProcessKey = gridView_work.GetFocusedRowCellValue("PROCESS_KEY")?.ToString();
                string sLCode = gridView_work.GetFocusedRowCellValue("L_CODE")?.ToString();
                string sWorkDate = gridView_work.GetFocusedRowCellValue("WORKDATE")?.ToString();
                string sNum = gridView_work.GetFocusedRowCellValue("NUM")?.ToString();

                SQL = $@"
                SELECT START_TIME, END_TIME
                FROM WORK_ORDER
                WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                    AND WORKDATE = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{sNum}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {
                    if (ds.Tables[0].Rows[0]["START_TIME"]?.ToString() == "")
                    {
                        ShowMessageBox.XtraShowWarning("작업 시작 먼저 해주세요.");
                        return;
                    }

                    if (ds.Tables[0].Rows[0]["END_TIME"]?.ToString() != "")
                    {
                        ShowMessageBox.XtraShowWarning("이미 종료된 작업입니다.");
                        return;
                    }

                    //SQL = $@"
                    ///* 작업 종료시간 업데이트 */
                    //UPDATE WORK_ORDER
                    //SET END_TIME = SYSDATE
                    //    , C_CONDITION = '{clsCommon.GetPcStatusCode("완료")}'
                    //    , ERP_OSTATUS = CASE WHEN '{clsCommon.GetTransAuto(sPlantCode, sProcessKey)}' = 'Y' 
                    //                                    THEN 'F' 
                    //                                    ELSE 'N' 
                    //                                END 
                    //        , ERP_ISTATUS = CASE WHEN '{clsCommon.GetTransAuto(sPlantCode, sProcessKey)}' = 'Y' THEN 'F' ELSE 'N' END
                    //        , ERP_OERR_CNT = 0
                    //        , ERP_IERR_CNT = 0
                    //WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sProcessKey}' AND L_CODE = '{sLCode}'
                    //    AND WORKDATE = TO_CHAR(TO_DATE('{sWorkDate}', 'YYYY-MM-DD'), 'YYYYMMDD') AND NUM = '{sNum}'
                    //";

                    //if (Dbconn.conn.SQLrun(SQL) < 1)
                    //{
                    //    clsLog.logSave(this.Text, "btnWorkStart_Click", SQL);
                    //    ShowMessageBox.XtraShowWarning("작업 종료가 실패했습니다");
                    //    return;
                    //}

                    SetWorkEnd();
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btnWorkStart_Click", ex.Message + "/" + ex.StackTrace);
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }

            XMain_Search();
        }

        private void gridView_work_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            // SummaryItem 캐스팅
            var item = e.Item as DevExpress.XtraGrid.GridColumnSummaryItem;
            if (item == null) return;

            // END_TIME 푸터에 총 시간 표시
            if (!e.IsTotalSummary || item.FieldName != "END_TIME")
                return;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                totalSeconds = 0;
            }
            else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {
                // 각 row 에서 START_TIME, END_TIME 가져오기
                var startObj = gridView_work.GetRowCellValue(e.RowHandle, "START_TIME");
                var endObj = gridView_work.GetRowCellValue(e.RowHandle, "END_TIME");

                if (startObj != null && endObj != null &&
                    startObj != DBNull.Value && endObj != DBNull.Value)
                {
                    DateTime st = Convert.ToDateTime(startObj);
                    DateTime et = Convert.ToDateTime(endObj);

                    totalSeconds += (int)(et - st).TotalSeconds;
                }
            }
            else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                TimeSpan ts = TimeSpan.FromSeconds(totalSeconds);
                e.TotalValue = $"배합 총시간 : {ts.ToString(@"hh\:mm\:ss")}";   // HH:mm:ss 출력
                e.TotalValueReady = true;
            }
        }

        private void gridControl_batchRun_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void gridView_work_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "BATCH")
            {
                if (e.ListSourceRowIndex < 0)
                    return;

                string rBatch = Convert.ToString(
                    gridView_work.GetListSourceRowCellValue(e.ListSourceRowIndex, "R_BATCH"));

                string batch = Convert.ToString(e.Value);

                // 🔥 전각 → 반각 변환
                rBatch = Microsoft.VisualBasic.Strings.StrConv(rBatch ?? "", Microsoft.VisualBasic.VbStrConv.Narrow);
                batch = Microsoft.VisualBasic.Strings.StrConv(batch ?? "", Microsoft.VisualBasic.VbStrConv.Narrow);

                if (!string.IsNullOrEmpty(rBatch) && !string.IsNullOrEmpty(batch))
                {
                    e.DisplayText = $"{rBatch} / {batch}";
                }
            }
        }

        private void gridView_work_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value != null)
            {
                e.Value = Microsoft.VisualBasic.Strings.StrConv(
                    e.Value.ToString(),
                    Microsoft.VisualBasic.VbStrConv.Narrow
                );
            }
        }
    }
}