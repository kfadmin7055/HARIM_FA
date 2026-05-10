using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Core.Class;
using System.Collections.Generic;
using DevExpress.Charts.Native;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraGrid;
using System.Reflection.Emit;
using DevExpress.XtraEditors.Repository;
using System.Linq;
using Core.Extension;
using System.Reflection;
using DevExpress.Schedule;
using DevExpress.XtraSpreadsheet.Import.Xls;
using DevExpress.XtraTreeList.ViewInfo;
using System.Diagnostics;
using DevExpress.XtraCharts;

namespace HARIM_FA_DOSING
{
    public partial class frm_Dosing2 : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = string.Empty;
        private string vPlant_Code = string.Empty;
        private string vProcess_Code = string.Empty;
        private string vLine_Code = string.Empty;
        private string formname = string.Empty;
        DataSet authDs;
        private string[] sValid = null;
        public static string runProcessCd = string.Empty;

        bool chk_version = false;
        decimal dNote_Per = 0;

        private bool isInitializing = false;

        public frm_Dosing2(string plant_code, string process_key, string lcode, string formName)
        {
            InitializeComponent();
            formname = formName;

            vPlant_Code = plant_code;
            vProcess_Code = process_key;
            vLine_Code = lcode;

            clsDevexpressGrid.EditGridViewInit(gridView_work);
            gridView_work.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            gridView_work.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;

            clsDevexpressGrid.ReadGridViewInit(gridView_batchRun);
            gridView_batchRun.OptionsBehavior.Editable = true;

            clsDevexpressGrid.ReadGridViewInit(gridView_batchList);
            clsDevexpressGrid.ReadGridViewInit(gridView_batchResult);
            clsDevexpressGrid.ReadGridViewInit(gridView_batchLog);
        }

        #region 폼로드 이벤트
        private void frm_Dosing2_Load(object sender, EventArgs e)
        {
            try
            {
                string form_name = string.Empty;

                if (vProcess_Code == clsCommon.GetProcessKey("배합"))      // 031002	배합공정
                {
                    form_name = "frm_Dosing2";
                    gridColumn_BAD_CODE1.Visible = false;
                    gridColumn_BAD_QTY1.Visible = false;
                    gridColumn_BAD_CODE2.Visible = false;
                    gridColumn_BAD_QTY2.Visible = false;
                    gridColumn_BAD_CODE3.Visible = false;
                    gridColumn_BAD_QTY3.Visible = false;
                    gridColumn_BAD_CODE4.Visible = false;
                    gridColumn_BAD_QTY4.Visible = false;
                    gridColumn_BAD_CODE5.Visible = false;
                    gridColumn_BAD_QTY5.Visible = false;
                }
                else
                {
                    form_name = formname;
                    gridColumn_BAD_CODE1.Visible = true;
                    gridColumn_BAD_QTY1.Visible = true;
                    gridColumn_BAD_CODE2.Visible = true;
                    gridColumn_BAD_QTY2.Visible = true;
                    gridColumn_BAD_CODE3.Visible = true;
                    gridColumn_BAD_QTY3.Visible = true;
                    gridColumn_BAD_CODE4.Visible = true;
                    gridColumn_BAD_QTY4.Visible = true;
                    gridColumn_BAD_CODE5.Visible = true;
                    gridColumn_BAD_QTY5.Visible = true;
                }

                authDs = clsSql.GetAuthDataSet(form_name);

                gridView_work.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

                isInitializing = true;         // 초기화 완료

                //// 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                

                //진행상태 콤보박스 셋팅 
                clsDevexpressUtil.ItemLookUpEditSetup(lookUpEdit_workStatus, clsCommon.GetPcStatus(), "제품을 선택 해주세요.", false, 0, true, true);
                lookUpEdit_workStatus.ItemIndex = 0;

                //작업일자
                dateEdit_workDate.EditValue = DateTime.Today;

                //작업모드 콤보박스 셋팅
                comboBoxEdit_workMode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                comboBoxEdit_workMode.Properties.BeginUpdate();
                comboBoxEdit_workMode.Properties.Items.Add("단동");
                comboBoxEdit_workMode.Properties.Items.Add("연동");
                comboBoxEdit_workMode.Properties.EndUpdate();
                comboBoxEdit_workMode.SelectedIndex = 0;

                this.ActiveControl = btn_workSearch;

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
                //        ShowMessageBox.XtraShowWarning("도징 PLC 접속을 실패하였습니다 (PLC1)");
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

                layoutControlItem_workEnd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //작업조회
                XMain_Search();

                // P05	대용유배합공정
                if (vProcess_Code != clsCommon.GetProcessKey("배합"))
                {
                    layoutControlItem_workEnd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_Dosing2_Load", ex);
            }
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
                gridView_work.ShowLoadingPanel();

                SQL = $@"
                SELECT 
                      a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, NUM          -- 1
                    , a.P_TYPE , a.RESOURCE_NO , a.NOTE                               -- 2
                    , a.WORK_START_DATE, a.BATCH, a.R_BATCH, a.BATCH_Q, a.OR_Q        -- 3
                    , a.PRO_Q, a.BBATCH_Q, a.GUBUN, a.LOCATION_ED, a.LOCATION_ED2     -- 4
                    , a.REMARK, a.ICM_CODE, a.C_CONDITION, a.BU_YN                    -- 5
                    , a.BAD_CODE1, a.BAD_QTY1, a.BAD_CODE2, a.BAD_QTY2                -- 6
                    , a.BAD_CODE3, a.BAD_QTY3, a.BAD_CODE4, a.BAD_QTY4                -- 7
                    , a.BAD_CODE5, a.BAD_QTY5                                         -- 8
                    , a.I_TIME, a.EMPLOYEE_NO                                         -- 9
                    , CASE 
                        WHEN (a.END_TIME - a.START_TIME) * 24 * 60 > 0 
                        THEN FLOOR((a.PRO_Q / ((a.END_TIME - a.START_TIME) * 24 * 60)) * 60) 
                        ELSE 0 
                    END AS PROVITT
                FROM WORK_ORDER a
                WHERE a.PLANT_CODE = '{cboPlant_Code.EditValue?.ToString()}'
                    AND a.PROCESS_KEY = '{vProcess_Code}'
                    AND a.L_CODE = '{vLine_Code}'
                    AND NVL(a.DEL_FLAG,'N') != 'Y'
                    AND a.WORKDATE = '{string.Format("{0:yyyyMMdd}", dateEdit_workDate.EditValue)}'
                    AND ('{lookUpEdit_workStatus.EditValue}' IS NULL OR a.C_CONDITION = '{lookUpEdit_workStatus.EditValue}')
                ORDER BY INSTR('{clsCommon.GetPcStatusCode("완료")},{clsCommon.GetPcStatusCode("진행")},{clsCommon.GetPcStatusCode("계획")}', a.C_CONDITION), a.START_TIME, a.NUM
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl_work, gridView_work, ds.Tables[0], false, true);

                sValid = new string[] { "PLANT_CODE", "PROCESS_KEY", "L_CODE", "WORKDATE", "NUM", "P_TYPE", "RESOURCE_NO", "NOTE", "WORK_START_DATE", "LOCATION_ED", "ICM_CODE", "EMPLOYEE_NO" };


                clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), vProcess_Code), "배합비 버전이 없습니다.", false);

                Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "유형" }
                };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(vPlant_Code, $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 2, false, false, "KG"), "품목을 선택 해주세요.", true, parameterDict, "CODE", "NAME");

                parameterDict = new Dictionary<string, string>
                {
                    { "PER", "비율" }
                };

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboNOTE, clsCommon.getNote(vPlant_Code), "배합비 버전이 없습니다.", false);


                //gridcboBResource_No.NullValuePrompt = "";
                //gridcboBResource_No.NullText = "";
                //gridcboBResource_No.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //gridcboBResource_No.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoSearch;
                ////repItemLkUpEdit_RESOURCE_NO.CaseSensitiveSearch = true;
                //clsDevexpressGrid.ItemLookUpEditSetup(gridcboBResource_No, ds.Tables[0]);
                //gridcboBResource_No.PopupFormMinSize = new Size(500, 650);

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
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_T_BIN, clsCommon.GetBin(vPlant_Code, vProcess_Code, vLine_Code), "", true, true, null, null, "CODE", "CODE");


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
            XMain_Search();
        }

        /// <summary>
        /// 진행상태 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEdit_workStatus_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }


        #region 작업추가 버튼 클릭 이벤트
        private void btn_workAdd_Click(object sender, EventArgs e)
        {

            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("입력권한이 없습니다");
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
                clsDevexpressGrid.GridViewAddRow(gridView_work);

                gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, gridView_work.Columns["PLANT_CODE"], vPlant_Code);
                gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, gridView_work.Columns["PROCESS_KEY"], vProcess_Code);
                gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, gridView_work.Columns["L_CODE"], vLine_Code);
                gridView_work.SetFocusedRowCellValue("P_TYPE", "2");

                gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, gridView_work.Columns["BATCH_Q"], 0);     // 배치 량
                gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, gridView_work.Columns["BATCH"], 1);       // 배치 수
                gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, gridView_work.Columns["R_BATCH"], 0);     // 진행중인 배치 수
                gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, gridView_work.Columns["EMPLOYEE_NO"], clsCommon.UserId);
                gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, gridView_work.Columns["C_CONDITION"], "미입력");
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_workAdd_Click", ex);
            }
        }
        #endregion

        #region 작업삭제 버튼 클릭 이벤트
        private void btn_workDelete_Click(object sender, EventArgs e)
        {

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("삭제권한이 없습니다");
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

                    // 주동석
                    //// 031002 배합공정
                    //if (vProcess_Code == clsCommon.GetPcStatusCode("계획"))
                    //{
                    //    if (!condition.Equals("계획"))
                    //    {
                    //        ShowMessageBox.XtraShowInformation("계획중인 작업지시만 삭제하실 수 있습니다");
                    //        return;
                    //    }
                    //}

                    string PLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
                    string PROCESS_KEY = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
                    string L_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");

                    string WORKDATE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
                    string work_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");
                    string work_batch = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH");

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업순번 " + work_num + " 작업지시를 삭제하시겠습니까?");

                    if (result == DialogResult.Yes)
                    {
                        splashScreenManager.ShowWaitForm();

                        Dbconn.conn.BeginTransaction();
                        SQL = $@"
                        UPDATE WORK_ORDER SET DEL_FLAG = 'Y'
                        WHERE PLANT_CODE = '{PLANT_CODE}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{L_CODE}'
                            AND WORKDATE = '{WORKDATE}' AND NUM = '{work_num}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_workDelete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        DELETE FROM WORK_DETAIL
                        WHERE PLANT_CODE = '{PLANT_CODE}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{L_CODE}'
                        AND WORKDATE = '{WORKDATE}' AND NUM = '{work_num}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_workDelete_Click", SQL);
                            ShowMessageBox.XtraShowWarning("작업지시 삭제에 실패했습니다");
                            return;
                        }


                        if (condition.Equals("완료"))
                        {
                            SQL = $@"
                            DELETE FROM WORK_REMARK
                            WHERE PLANT_CODE = '{PLANT_CODE}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{L_CODE}'
                            AND WORKDATE = '{WORKDATE}' AND NUM = '{work_num}'
                            ";

                            Dbconn.conn.SQLrun(SQL);

                            SQL = $@"
                            DELETE FROM WORK_REMARK
                            WHERE PLANT_CODE = '{PLANT_CODE}' AND PROCESS_KEY = 'SAP_{vProcess_Code}' AND L_CODE = '{L_CODE}'
                            AND WORKDATE = '{WORKDATE}' AND NUM = '{work_num}'
                            ";

                            Dbconn.conn.SQLrun(SQL);
                        }

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

        private void gridView_work_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;

            //지시량 자동계산 (배치수 * 배치량 = 지시량) 
            try
            {
                if (e.Column.FieldName == "BATCH_Q")        // 배치량
                {
                    int sBATCH_Q = 0;
                    if (view.GetRowCellValue(e.RowHandle, view.Columns["BATCH"]) != null && !string.IsNullOrEmpty(view.GetRowCellValue(e.RowHandle, view.Columns["BATCH"]).ToString()))     // 배치수
                    {
                        sBATCH_Q = Convert.ToInt32(e.Value.ToString());
                        int batch = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, view.Columns["BATCH"]));

                        view.SetRowCellValue(e.RowHandle, view.Columns["OR_Q"], (sBATCH_Q * batch));     // 지시량
                    }

                    string sResource_NO = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["RESOURCE_NO"]).ToString() : "";
                    string sNOTE = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["NOTE"]).ToString() : "";
                    string sBU_YN = e.RowHandle >= 0 ? view.GetRowCellValue(e.RowHandle, view.Columns["BU_YN"]).ToString() : "";

                    mix_result(sResource_NO, sNOTE, sBATCH_Q.ToString(), chk_version, sBU_YN);

                }

                if (e.Column.FieldName == "BATCH")      // 배치수
                {
                    if (view.GetRowCellValue(e.RowHandle, view.Columns["BATCH_Q"]) != null && !string.IsNullOrEmpty(view.GetRowCellValue(e.RowHandle, view.Columns["BATCH_Q"]).ToString()))       // 배치량
                    {
                        int batch = Convert.ToInt32(e.Value.ToString());
                        int sBATCH_Q = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, view.Columns["BATCH_Q"]));

                        view.SetRowCellValue(e.RowHandle, view.Columns["OR_Q"], (sBATCH_Q * batch));     // 지시량
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
                // 031004	완료
                if (gridView_work.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.GetPcStatusCode("완료"))) //작지가 완료처리된것은 수정못하도록 에디트모드 off
                {
                    e.Cancel = true;        // 수정 불가
                }
                // 031003	진행
                else if (gridView_work.GetFocusedRowCellValue("C_CONDITION").ToString().Trim().Equals(clsCommon.GetPcStatusCode("진행")))  //작지가 진행중일 경우 배치수만 수정가능하도록 변경
                {
                    switch (gridView_work.FocusedColumn.FieldName)
                    {
                        case "RESOURCE_NO":
                        case "NOTE":
                        case "BATCH_Q":
                        case "LOCATION_ED":
                        case "LOCATION_ED2":
                            e.Cancel = true;
                            break;

                        default:
                            e.Cancel = false;
                            break;
                    }
                }
                else if (clsCommon._strDosPlcConnYn != "Y" && clsCommon._strMicPlcConnYn != "Y" && vProcess_Code == clsCommon.GetProcessKey("배합"))       // 031002	배합공정
                {
                    // e.Cancel = true;
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

                            if (int.Parse(dr["BATCH_Q"].ToString()) >= Int16.MaxValue)
                            {
                                dr.SetColumnError("BATCH_Q", "배치량이 최대치인 32768을 초과 했습니다.");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                                return;
                            }

                            if (Convert.ToInt16(dr["BATCH_Q"]) <= 0)
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

                            if (vProcess_Code == clsCommon.GetProcessKey("배합"))
                            {
                                if (Convert.ToInt16(dr["BATCH_Q"]) < 3000)
                                {
                                    dr.SetColumnError("BATCH_Q", "배치량을 최소 3000KG이상을 입력하여 주세요");
                                    ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH_Q"));
                                    return;
                                }
                            }

                            if (string.IsNullOrEmpty(dr["BATCH"].ToString()))
                            {
                                dr.SetColumnError("BATCH", "배치수를 입력하여 주세요");
                                ShowMessageBox.XtraShowWarning(dr.GetColumnError("BATCH"));
                                return;
                            }

                            if (Convert.ToInt16(dr["BATCH"]) <= 0)
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
                            //진행중인 작지를 수정했을 경우
                            if (dr["C_CONDITION"].ToString() == clsCommon.GetPcStatusCode("진행"))       // 031003	진행
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
                                WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{vLine_Code}'
                                AND WORKDATE = '{dr["WORKDATE"]}' AND NUM = '{dr["NUM"]}'
                                ";

                                DataSet tmpDs = Dbconn.conn.ExecutDataset(SQL);

                                if (Dbconn.conn.getRowCnt(tmpDs) == 1)
                                {
                                    if (!Dbconn.conn.getData(tmpDs, "BATCH", 0).Equals(dr["BATCH"].ToString()))
                                    {

                                        //PLC에 배치변경 전송
                                        try
                                        {
                                            if (clsMelsec.plc_dosing.WriteDeviceBlock("D0103", 1, ref batch) != 0)
                                            {
                                                clsLog.logSave(dr["WORKDATE"].ToString() + "/" + dr["NUM"].ToString() + ": PLC장비에 배치변경전송을 실패하였습니다", 0);
                                                ShowMessageBox.XtraShowWarning("PLC장비에 배치변경전송을 실패하였습니다", "알림");
                                                return;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            clsLog.logSave(this, "PLC BATCH CNT CHANGE FAIL", ex);
                                        }

                                        //현재 작업중인 배치 변경
                                        SQL = $@"
                                        UPDATE WORK_ORDER
                                        SET BATCH = '{batch}', OR_Q = '{(batch * sBATCH_Q)}'
                                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{vLine_Code}'
                                            AND WORKDATE = '{dr["WORKDATE"]}' AND NUM = '{dr["NUM"]}'
                                        ";

                                        if (Dbconn.conn.SQLrun(SQL) < 1)
                                        {
                                            Dbconn.conn.Rollback();
                                            ShowMessageBox.XtraShowInformation("현재 작업중인 배치를 업데이트 하는 도중 오류가 발생했습니다");
                                            clsLog.logSave("frm_Dosing2", "btn_workSave_Click", SQL);
                                            return;
                                        }

                                    }
                                }
                                else
                                {
                                    ShowMessageBox.XtraShowInformation("작업지시 정보를 찾을 수 없습니다");
                                    return;
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

        #region 작업지시정보 저장버튼 클릭이벤트
        private void btn_workSave_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "U"))
            {
                ShowMessageBox.XtraShowInformation("수정권한이 없습니다");
                return;
            }

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
                    ShowMessageBox.XtraShowInformation("입력권한이 없습니다");
                    return;
                }

                if (gridView_work.RowCount == 0)
                {
                    ShowMessageBox.XtraShowInformation("강제완료 하실 작업지시를 선택하여 주세요");
                    return;
                }

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
                    string insert_st_time = string.Empty;

                    string con_st = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "C_CONDITION");

                    if (con_st == clsCommon.GetPcStatusCode("완료"))     // 031004	완료
                    {
                        ShowMessageBox.XtraShowInformation("이미 완료처리 된 작업지시 입니다");
                        return;
                    }

                    if (con_st == clsCommon.GetPcStatusCode("진행"))     // 031003	진행
                    {
                        ShowMessageBox.XtraShowInformation("작업중인 작업지시는 작업완료처리 진행이 안됩니다");
                        return;
                    }

                    DialogResult result = ShowMessageBox.Confirm("선택하신 작업순번 " + work_num + " 작업지시를 강제완료 하시겠습니까?");
                    if (result == DialogResult.Yes)
                    {
                        splashScreenManager.ShowWaitForm();
                        Dbconn.conn.BeginTransaction();

                        SQL = $@"
                        UPDATE WORK_ORDER
                        SET R_BATCH = BATCH, PRO_Q = OR_Q, C_CONDITION = '{clsCommon.GetPcStatusCode("완료")}'
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
                            UPDATE PELLET_REPORT
                            SET BF_QTY = (SELECT OR_Q FROM WORK_ORDER WHERE PLANT_CODE = '{plantCode}'
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
                        if (vProcess_Code == clsCommon.GetPcStatusCode("계획"))
                        {
                            SQL = $@"
                            UPDATE WORK_ORDER SET START_TIME = SYSDATE, END_TIME = SYSDATE
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
                        if (!clsProcessDosing.InsertWorkRemark(vPlant_Code, vProcess_Code, vLine_Code, WORK_START_DATE, work_num))
                        {
                            Dbconn.conn.Rollback();
                            ShowMessageBox.XtraShowWarning("강제완료에 실패했습니다");
                            return;
                        }

                        Dbconn.conn.Commit();

                        // 작지 SAP 실적 생성
                        if (!clsProcessDosing.SetSAPWorkRemark(vPlant_Code, vProcess_Code, vLine_Code, WORK_START_DATE, work_num))
                        {
                            Dbconn.conn.Rollback();
                            ShowMessageBox.XtraShowWarning("강제완료에 실패했습니다");
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
        #endregion

        // 작업 시작
        private void work_start(string argPlant_Code, string argL_CODE, string argWorkDate, string argWorkNum, string argsResourceNo, string argNOTE)
        {
            //EthernetConnector connector = new EthernetConnector(new TcpSocket(clsCommon.plc_dosing_ip, 10260), false);

            try
            {
                //Task<bool> returnTaskResult = clsPlcConnManager.Plc_conn(connector);
                //bool connResult = await returnTaskResult;

                //if (!connResult)
                //{
                //    ShowMessageBox.XtraShowInformation("PLC 연결에 실패했습니다");
                //    return;
                //}

                ////생산중인 제품인지 있는 체크
                ////D601
                ////작업START value 1이상이면 생산중인 제품 존재

                //Task<int[]> returnTaskChkResult = clsPlcConnManager.ReadWord(connector, "0601", 1);
                //int[] readChkResult = await returnTaskChkResult;

                //if (readChkResult.Length != 1)
                //{
                //    ShowMessageBox.XtraShowInformation("PLC 읽기에 실패했습니다");
                //    clsLog.logSave("PLC 읽기 실패 : 0601 /" + readChkResult.Length, 0);
                //    return;
                //}

                //if (readChkResult[0] >= 1)
                //{
                //    ShowMessageBox.XtraShowInformation("도징 작업지시가 현재 진행중입니다");
                //    return;
                //}

                string WORK_START_DATE = argWorkDate;
                string work_num = argWorkNum;

                SQL = $@"
                SELECT PLANT_CODE, PROCESS_KEY, L_CODE , WORKDATE, NUM
                    , BATCH_Q, BATCH, RESOURCE_NO, LOCATION_ED, NVL(LOCATION_ED2, 0) as LOCATION_ED2
                FROM WORK_ORDER
                WHERE PLANT_CODE = '{argPlant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{argL_CODE}'
                    AND WORKDATE = '{WORK_START_DATE}' AND NUM = '{work_num}'
                ";

                DataSet tbinSearchDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tbinSearchDs) < 1)
                {
                    ShowMessageBox.XtraShowInformation("작업지시 정보를 찾을 수 없습니다");
                    return;
                }


                string t_bin = Dbconn.conn.getData(tbinSearchDs, "LOCATION_ED", 0);
                string res_no = Dbconn.conn.getData(tbinSearchDs, "RESOURCE_NO", 0);

                SQL = $@"
                SELECT RESOURCE_NO FROM BIN
                WHERE PLANT_CODE = '{argPlant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{argL_CODE}' AND LOCATION = '{t_bin}'
                ";

                DataSet tbinChkDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tbinChkDs) < 1)
                {
                    ShowMessageBox.XtraShowInformation("목적빈 정보를 찾을 수 없습니다");
                    return;
                }

                /*                string tbinResNo = Dbconn.conn.getData(tbinChkDs, "RESOURCE_NO", 0);

                                if (tbinResNo != res_no)
                                {
                                    ShowMessageBox.XtraShowInformation("현재 목적빈 제품이 생산하시려는 제품과 다릅니다");
                                    return;
                                }*/


                SQL = $@"
                SELECT A.SCALE_CODE, A.SV, A.MAX_Q FROM (
                SELECT W.SCALE_CODE, SUM(W.SET_VAL) AS SV, MAX(b.MAX_Q) AS MAX_Q 
                FROM WORK_DETAIL W
                    LEFT OUTER JOIN SCALE SC ON W.SCALE_CODE = b.SCALE_CODE
                WHERE W.PLANT_CODE = '{argPlant_Code}' AND W.PROCESS_KEY = '{vProcess_Code}' AND W.L_CODE = '{argL_CODE}'
                    AND W.WORKDATE = '{WORK_START_DATE}' AND W.NUM = '{work_num}'
                GROUP BY W.SCALE_CODE
                ) A WHERE A.SV > MAX_Q
                ";

                DataSet scaleChkDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(scaleChkDs) > 0)
                {
                    MessageBox.Show("스케일" + Dbconn.conn.getData(scaleChkDs, "SCALE_CODE", 0).Trim() + "의 최대용량이 넘었습니다\r\n" + "현재 계량용량 : " + Dbconn.conn.getData(scaleChkDs, "SV", 0).Trim() + "/ 최대용량 : " + Dbconn.conn.getData(scaleChkDs, "MAX_Q", 0).Trim());
                    return;
                }

                int[] WriteIndexData = new int[10];
                int start_addr = 161;

                int[] WriteSendRecipi = new int[400];

                //스케일번호 address 위치 입력
                // FROM WORK_DETAIL
                using (DataSet scaleAddIndexDs = clsProcessDosing.resultWorkSendIndex(vProcess_Code, WORK_START_DATE, work_num))
                {
                    int scaleAddCnt = Dbconn.conn.getRowCnt(scaleAddIndexDs);

                    if (scaleAddCnt < 1)
                    {
                        clsLog.logSave(this, "btn_workStart_Click", "저장된 생산지시 정보 없음 / " + work_num);
                        ShowMessageBox.XtraShowWarning("저장된 생산지시 정보가 없습니다");
                        return;
                    }

                    splashScreenManager.ShowWaitForm();
                    splashScreenManager.SetWaitFormCaption("작업지시를 전송중입니다");


                    splashScreenManager.SetWaitFormDescription("빈에 대한 배합비를 전송중입니다");
                    string preScaleCd = string.Empty;
                    string preScaleNo = string.Empty;

                    int recipi_index = 0;
                    for (int i = 0; i < scaleAddCnt; i++)
                    {
                        string scale_cd = Dbconn.conn.getData(scaleAddIndexDs, "SCALE_CODE", i);
                        string scale_no = Dbconn.conn.getData(scaleAddIndexDs, "SCALE_NO", i);
                        string fail = Dbconn.conn.getData(scaleAddIndexDs, "FAIL", i);                  // 낙차
                        string mix_result = Dbconn.conn.getData(scaleAddIndexDs, "SET_VAL", i);      // 설정값(Kg)
                        string bin_serial = Dbconn.conn.getData(scaleAddIndexDs, "BIN_SERIAL", i);
                        string in_scale = Dbconn.conn.getData(scaleAddIndexDs, "IN_SCALE", i);

                        if (scale_cd != preScaleCd)
                        {
                            if (i != 0)
                            {
                                WriteSendRecipi[(recipi_index * 5) + 0] = Convert.ToInt16(preScaleNo); //스케일 NO
                                WriteSendRecipi[(recipi_index * 5) + 1] = 0;
                                WriteSendRecipi[(recipi_index * 5) + 2] = 0;
                                WriteSendRecipi[(recipi_index * 5) + 3] = 0;
                                WriteSendRecipi[(recipi_index * 5) + 4] = 0;
                                recipi_index += 1;
                                start_addr += 5;
                            }

                            WriteIndexData[Convert.ToInt16(scale_no) - 1] = start_addr;
                        }

                        //배합비 전송
                        WriteSendRecipi[(recipi_index * 5) + 0] = Convert.ToInt16(scale_no); //스케일 NO
                        WriteSendRecipi[(recipi_index * 5) + 1] = Convert.ToInt16(bin_serial); //빈시리얼
                        WriteSendRecipi[(recipi_index * 5) + 2] = Convert.ToInt32(Math.Round(Convert.ToDouble(mix_result) * Convert.ToDouble(in_scale))); //배합량
                        WriteSendRecipi[(recipi_index * 5) + 3] = 0;  //동시빈
                        WriteSendRecipi[(recipi_index * 5) + 4] = Convert.ToInt16(fail); //낙차

                        preScaleNo = scale_no;
                        preScaleCd = scale_cd;

                        start_addr += 5;
                        recipi_index += 1;

                        if ((i + 1) == scaleAddCnt)
                        {
                            WriteSendRecipi[(recipi_index * 5) + 0] = Convert.ToInt16(preScaleNo); //스케일 NO
                            WriteSendRecipi[(recipi_index * 5) + 1] = 0;
                            WriteSendRecipi[(recipi_index * 5) + 2] = 0;
                            WriteSendRecipi[(recipi_index * 5) + 3] = 0;
                            WriteSendRecipi[(recipi_index * 5) + 4] = 0;
                        }
                    }

                } //using

                int[] sendRecipi1 = new int[200];
                int[] sendRecipi2 = new int[200];
                Array.Copy(WriteSendRecipi, 0, sendRecipi1, 0, 200);
                Array.Copy(WriteSendRecipi, 200, sendRecipi2, 0, 200);


                //bin 레시피 addr
                //Task<bool> returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "D0161", sendRecipi1);
                //bool sendResult = await returnTaskSendResult;
                //if (!sendResult)
                //{
                //    clsLog.logSave(this, "btn_workStart_Click", "PLC전송실패 / D161 ");
                //    ShowMessageBox.XtraShowWarning("PLC 전송에 실패했습니다 / "D161");
                //    return;
                //}

                //returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "D0361", sendRecipi2);
                //sendResult = await returnTaskSendResult;
                //if (!sendResult)
                //{
                //    clsLog.logSave(this, "btn_workStart_Click", "PLC전송실패 / D0361 ");
                //    ShowMessageBox.XtraShowWarning("PLC 전송에 실패했습니다 / "D0361");
                //    return;
                //}


                splashScreenManager.SetWaitFormDescription("스케일 시작 ADDR을 전송중입니다");
                //scale index
                //returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "151", WriteIndexData);
                //sendResult = await returnTaskSendResult;
                //if (!sendResult)
                //{
                //    clsLog.logSave(this, "btn_workStart_Click", "PLC전송실패 / D151 ");
                //    ShowMessageBox.XtraShowWarning("PLC 전송에 실패했습니다 / "D151");
                //    return;
                //}

                splashScreenManager.SetWaitFormDescription("작업지시 정보를 전송중입니다");
                //작업지시 데이터
                int[] SendWorkData = new int[40];
                int[] WorkData = new int[10];
                Array.Resize(ref WorkData, 1);
                Array.Clear(WorkData, 0, WorkData.Length);

                //Task<int[]> returnWorkResult = clsPlcConnManager.ReadWord(connector, "101", 1);
                //int[] readResult = await returnWorkResult;

                //if (readResult == null)
                //{
                //    clsLog.logSave(this, "btn_workStart_Click", "PLC읽기 실패 / D00101 ");
                //    ShowMessageBox.XtraShowWarning("PLC 읽기에 실패했습니다");
                //    return;
                //}

                //SendWorkData[0] = readResult[0] + 1; //로트번호 읽은후 순번


                string mix_time = string.Empty;
                string dry_time = string.Empty;
                string final_time = string.Empty;
                string fl_time = string.Empty;
                string fudge_time = string.Empty;

                // 주동석 테이블이 없음
                //SQL = "SELECT CEILING(MIX_QTY) as MIX_QTY, CEILING(MIX_TIME) as MIX_TIME, CEILING(DRY_TIME) as DRY_TIME,
                //    "CEILING(FINAL_TIME) as FINAL_TIME , CEILING(FLUSHING_TIME) as FLUSHING_TIME, CEILING(FUDGE_TIME) AS FUDGE_TIME
                //    $" FROM ERP_DBLINK.{clsCommon.erp_dosing_db_name}.DBO.V_MES_ATG_101_4
                //    $" WHERE RESOURCE_NO = '{argsResourceNo}' AND REVISION = '{argNOTE}'";
                ////SQL = string.Format(SQL, argsResourceNo, argNOTE);

                //DataSet workMixInfodDs = Dbconn.conn.ExecutDataset(SQL);

                //if (Dbconn.conn.getRowCnt(workMixInfodDs) < 1)
                //{
                //    ShowMessageBox.XtraShowWarning("배합계량정보가 없습니다 (이송시간, 믹싱타임, 드라이타임, 파이날타임)");
                //    clsLog.logSave(this, "btn_workStart_Click", "배합계량정보가 없습니다  / work_num);
                //    return;
                //}

                //mix_time = Dbconn.conn.getData(workMixInfodDs, "MIX_TIME", 0).Trim();
                //dry_time = Dbconn.conn.getData(workMixInfodDs, "DRY_TIME", 0).Trim();
                //final_time = Dbconn.conn.getData(workMixInfodDs, "FINAL_TIME", 0).Trim();
                //fl_time = Dbconn.conn.getData(workMixInfodDs, "FLUSHING_TIME", 0).Trim();
                //fudge_time = Dbconn.conn.getData(workMixInfodDs, "FUDGE_TIME", 0).Trim();


                if (string.IsNullOrEmpty(dry_time.Trim()))
                {
                    ShowMessageBox.XtraShowWarning("ERP배합설정 정보에 건조시간 정보가 없습니다");
                    clsLog.logSave(this, "btn_workStart_Click", "건조시간 정보가 없습니다  / " + work_num);
                    return;
                }

                if (string.IsNullOrEmpty(final_time.Trim()))
                {
                    ShowMessageBox.XtraShowWarning("ERP배합설정 정보에 파이날타임 정보가 없습니다");
                    clsLog.logSave(this, "btn_workStart_Click", "파이날타임 정보가 없습니다  / " + work_num);
                    return;
                }


                if (string.IsNullOrEmpty(fl_time.Trim()))
                {
                    ShowMessageBox.XtraShowWarning("ERP배합설정 정보에 플러싱타임 정보가 없습니다");
                    clsLog.logSave(this, "btn_workStart_Click", "플러싱타임 정보가 없습니다  / " + work_num);
                    return;
                }

                if (string.IsNullOrEmpty(fudge_time.Trim()))
                {
                    ShowMessageBox.XtraShowWarning("ERP배합설정 정보에 액상퍼지타임 정보가 없습니다");
                    clsLog.logSave(this, "btn_workStart_Click", "액상퍼지타임 정보가 없습니다  / " + work_num);
                    return;
                }

                SQL = $@"
                SELECT PLANT_CODE, PROCESS_KEY, L_CODE , WORKDATE, NUM, BATCH_Q, BATCH, LOCATION_ED, NVL(LOCATION_ED2, 0) as LOCATION_ED2
                FROM WORK_ORDER
                WHERE PLANT_CODE = '{argPlant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{argL_CODE}' 
                    AND WORKDATE = '{WORK_START_DATE}' AND NUM = '{work_num}'
                ";

                DataSet workSendDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(workSendDs) < 1)
                {
                    ShowMessageBox.XtraShowWarning("작업지시 정보가 없습니다");
                    clsLog.logSave(this, "btn_workStart_Click", "작업지시 정보 없음 / " + work_num);
                    return;
                }

                SendWorkData[0] = Convert.ToInt32((WORK_START_DATE + work_num.PadLeft(2, '0')).Substring(2, 4)); //작업지시
                SendWorkData[1] = Convert.ToInt32((WORK_START_DATE + work_num.PadLeft(2, '0')).Substring(6)); //작업순번
                SendWorkData[2] = Convert.ToInt16(Dbconn.conn.getData(workSendDs, "BATCH_Q", 0)); //배치량
                SendWorkData[3] = Convert.ToInt16(Dbconn.conn.getData(workSendDs, "BATCH", 0));  //배치수
                SendWorkData[4] = 0;
                SendWorkData[5] = Convert.ToInt16(Dbconn.conn.getData(workSendDs, "LOCATION_ED", 0)); //목적빈1
                SendWorkData[6] = Convert.ToInt16(Dbconn.conn.getData(workSendDs, "LOCATION_ED2", 0)); //목적빈2
                SendWorkData[7] = Convert.ToInt32(fl_time); //이송시간
                SendWorkData[8] = Convert.ToInt32(mix_time); ; //믹싱타임
                SendWorkData[9] = Convert.ToInt32(dry_time); ; //드라이타임
                SendWorkData[10] = Convert.ToInt32(final_time); ; //파이날타임

                //항생제코드 존재할 경우
                SQL = $@"
                SELECT SET_VAL, QTY_PCT
                FROM WORK_DETAIL
                WHERE INGRED_CODE = '962118' AND PLANT_CODE = '{argPlant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{argL_CODE}'
                    AND WORKDATE = '{WORK_START_DATE}' AND NUM = '{work_num}'
                ";

                DataSet workMolZeroDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(workMolZeroDs) == 0)
                {
                    SendWorkData[12] = 0;
                }
                else
                {
                    SendWorkData[12] = Convert.ToInt32((Convert.ToDouble(Dbconn.conn.getData(workMolZeroDs, "SET_VAL", 0)) * 1000));

                    clsLog.logSave("항곰팡이제 있음 : " + Convert.ToInt32((Convert.ToDouble(Dbconn.conn.getData(workMolZeroDs, "SET_VAL", 0)) * 1000)).ToString(), 0);
                }


                SendWorkData[13] = Convert.ToInt32(fudge_time); ; //액상퍼지타임


                //액상레시피 설정
                SQL = $@"
                SELECT  b.SCALE_NAME, a.SET_VAL, a.QTY_PCT
                FROM WORK_DETAIL a LEFT OUTER JOIN BIN b ON a.LOCATION = b.LOCATION
                LEFT OUTER JOIN SCALE b ON a.SCALE_CODE = b.SCALE_CODE
                LEFT OUTER JOIN INGRED c ON a.INGRED_CODE = c.RESOURCE_NO
                WHERE a.PLANT_CODE = '{argPlant_Code}' AND a.PROCESS_KEY = '{vProcess_Code}' AND a.L_CODE = '{argL_CODE}'
                    AND a.WORKDATE = '{WORK_START_DATE}' AND a.NUM = '{work_num}'
                    AND b.SCALE_CODE = 'LIQUID'
                ";

                DataSet LiquidDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(LiquidDs) > 0)
                {
                    Console.WriteLine("액상 : " + Dbconn.conn.getData(LiquidDs, "SET_VAL", 0));
                    //액상값 넣어주기
                    SendWorkData[15] = Convert.ToInt32((Convert.ToDouble(Dbconn.conn.getData(LiquidDs, "SET_VAL", 0)) / 1.04) / 0.046);
                }


                //스케일 레시피 합계 설정
                SQL = $@"
                SELECT  b.SCALE_NO, SUM(a.SET_VAL * b.IN_SCALE) as TOTAL
                FROM WORK_DETAIL a
                LEFT OUTER JOIN BIN b ON a.LOCATION = b.LOCATION
                LEFT OUTER JOIN SCALE c ON a.SCALE_CODE = c.SCALE_CODE
                LEFT OUTER JOIN INGRED d ON a.INGRED_CODE = d.RESOURCE_NO
                WHERE a.PLANT_CODE = '{argPlant_Code}' AND a.PROCESS_KEY = '{vProcess_Code}' AND a.L_CODE = '{argL_CODE}'
                    AND a.WORKDATE = '{WORK_START_DATE}' AND a.NUM = '{work_num}'
                    AND b.SCALE_CODE <> 'LIQUID' AND NVL(b.SCALE_CODE,'NO') <> 'NO'
                GROUP BY b.SCALE_NO
                ORDER BY b.SCALE_NO
                ";

                DataSet scaleSumDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(scaleSumDs) < 1)
                {
                    ShowMessageBox.XtraShowWarning("작업지시 정보가 없습니다");
                    clsLog.logSave(this, "btn_workStart_Click", "작업지시 정보 없음 / " + work_num);
                    return;
                }

                for (int i = 0; i < Dbconn.conn.getRowCnt(scaleSumDs); i++)
                {
                    string sum_scale_no = Dbconn.conn.getData(scaleSumDs, "SCALE_NO", i);
                    SendWorkData[30 + (Convert.ToInt16(sum_scale_no) - 1)] = Convert.ToInt32(Math.Round(Convert.ToDouble(Dbconn.conn.getData(scaleSumDs, "TOTAL", i))));
                }


                //작업정보 전송
                //returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "101", SendWorkData);
                //sendResult = await returnTaskSendResult;
                //if (!sendResult)
                //{
                //    clsLog.logSave(this, "btn_workStart_Click", "PLC전송실패 / D101 ");
                //    ShowMessageBox.XtraShowWarning("PLC 전송에 실패했습니다 / "D101");
                //    return;
                //}

                //work_order datarow
                SQL = $@"
                SELECT * FROM WORK_ORDER a
                WHERE a.PLANT_CODE = '{argPlant_Code}' AND a.PROCESS_KEY = '{vProcess_Code}' AND a.L_CODE = '{argL_CODE}'
                    AND a.WORKDATE = '{WORK_START_DATE}' AND a.NUM = '{work_num}'
                ";

                DataSet work_ds = Dbconn.conn.ExecutDataset(SQL);
                DataRow row = work_ds.Tables[0].Rows[0];

                clsUtil.Delay(150);

                // 주동석 테이블 없음
                //SQL = $"select * from ERP_DBLINK.{clsCommon.erp_dosing_db_name}.dbo.Z_MES_ATG_201_1
                //"where mes_order_no = 'BD01' + '{0}' and mes_seq = '{1}' and Status_Code = 'I' ";
                //SQL = string.Format(SQL, WORK_START_DATE, work_num);

                //if (Dbconn.conn.getRowCnt(Dbconn.conn.ExecutDataset(SQL)) == 0)
                //{
                //    //erp input
                //    // INSERT INTO Z_MES_ATG_201_1
                //    string erp_insert_chk = clsErpSql.InsertWorkOrder(vProcess_Code, WORK_START_DATE, work_num, row, "I");
                //    if (erp_insert_chk != "OK")
                //    {
                //        clsLog.logSave(this, "ERP WORKORDER ERROR", erp_insert_chk);
                //    }
                //}

                clsUtil.Delay(300);

                int[] startFlag = { 1 };
                //작업시작 
                //returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "601", startFlag);
                //sendResult = await returnTaskSendResult;
                //if (!sendResult)
                //{
                //    clsLog.logSave(this, "btn_workStart_Click", "PLC전송실패 / D601 ");
                //    clsUtil.Delay(500);
                //    returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "601", startFlag);
                //    sendResult = await returnTaskSendResult;
                //    if (!sendResult)
                //    {
                //        clsLog.logSave(this, "btn_workStart_Click", "PLC전송실패 / D601 ");
                //        clsUtil.Delay(500);
                //        returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "601", startFlag);
                //        sendResult = await returnTaskSendResult;
                //    }
                //}


                clsUtil.Delay(200);

                SQL = $@"
                UPDATE WORK_ORDER
                SET C_CONDITION = '{clsCommon.GetPcStatusCode("진행")}', R_BATCH = '1', START_TIME = SYSDATE
                WHERE PLANT_CODE = '{argPlant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{argL_CODE}'
                    AND WORKDATE = '{WORK_START_DATE}' AND NUM = '{work_num}'
                ";

                Dbconn.conn.SQLrun(SQL);

                string errMsg = "도징 작업지시를 진행했습니다(연동)";
                if (!clsProcessDosing.InsertLog(vPlant_Code, vProcess_Code, argL_CODE, WORK_START_DATE, work_num, "0", "031102", errMsg))
                {
                    clsLog.logSave(this, "btn_workStart_Click", "작업로그 입력에 실패했습니다/ " + WORK_START_DATE + "/" + work_num + "/" + "0");
                }

                clsUtil.Delay(400);

                // 주동석 테이블 없음
                //SQL = $"select * from ERP_DBLINK.{clsCommon.erp_dosing_db_name}.dbo.Z_MES_ATG_201_1
                //        "where mes_order_no = 'BD01' + '{0}' and mes_seq = '{1}' and Status_Code = 'I' ";
                //SQL = string.Format(SQL, WORK_START_DATE, work_num);

                ////ERP에 작업현황 올라갔는지 체크 없으면 다시 전송
                //if (Dbconn.conn.getRowCnt(Dbconn.conn.ExecutDataset(SQL)) == 0)
                //{
                //    //erp input
                //    // INSERT INTO Z_MES_ATG_201_1
                //    string erp_insert_chk = clsErpSql.InsertWorkOrder(vProcess_Code, WORK_START_DATE, work_num, row, "I");
                //    if (erp_insert_chk != "OK")
                //    {
                //        clsLog.logSave(this, "ERP WORKORDER ERROR", erp_insert_chk);
                //    }
                //}

                //작업이 시작상태인지 다시 체크 아니면 다시 전송
                //returnTaskChkResult = clsPlcConnManager.ReadWord(connector, "0601", 1);
                //readChkResult = await returnTaskChkResult;

                //if (readChkResult[0] != 1)
                //{
                //    clsUtil.Delay(500);

                //    //작업시작 
                //    returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "601", startFlag);
                //    sendResult = await returnTaskSendResult;
                //    if (!sendResult)
                //    {
                //        clsLog.logSave(this, "btn_workStart_Click", "PLC전송실패 / D601 ");
                //        clsUtil.Delay(500);
                //        returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "601", startFlag);
                //        sendResult = await returnTaskSendResult;
                //        if (!sendResult)
                //        {
                //            clsLog.logSave(this, "btn_workStart_Click", "PLC전송실패 / D601 ");
                //            clsUtil.Delay(500);
                //            returnTaskSendResult = clsPlcConnManager.WriteWord(connector, "601", startFlag);
                //        }

                //    }

                //}


                //작업지시 재조회
                XMain_Search();


            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_workStart_Click", ex);
                clsLog.logSave(this, "btn_workStart_Click", ex.StackTrace);
                ShowMessageBox.XtraShowError("작업을 시작하는 도중 에러가 발생했습니다");
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }

                //if (connector.IsConnected)
                //{
                //    connector.Disconnect();
                //}
            }
        }

        #region 작업지시 시작 버튼 클릭 이벤트
        private void btn_workStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (MAIN.DosPlcConnChk != "Y" || MAIN.MicPlcConnChk != "Y")
                {
                    ShowMessageBox.XtraShowInformation("배합, 마이크로 PLC 를 먼저 연결 해주세요.");
                    return;
                }

                if (clsDevexpressGrid.GetSelectRowCount(gridView_work) == 0)
                {
                    ShowMessageBox.XtraShowInformation("작업을 진행하실 작업지시를 선택하여 주세요");
                    return;
                }

                //작업지시정보
                DataRow row = gridView_work.GetDataRow(gridView_work.FocusedRowHandle);
                string plantCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
                string process_key = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PROCESS_KEY");
                string lCode = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");
                string work_date = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
                string work_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");


                if (DialogResult.Yes != ShowMessageBox.Confirm($"작업순번 {work_num}번을 작업진행하시겠습니까?"))
                {
                    return;
                }


                //전송유무 체크
                bool isChkStart = XWorkStart(plantCode, process_key, lCode, work_date, work_num);
                if (!isChkStart)
                {
                    return;
                }

                //작업진행 LOG
                string errMsg = "도징 작업지시를 진행하였습니다";
                if (!clsProcessDosing.InsertLog(vPlant_Code, vProcess_Code, vLine_Code, work_date, work_num, "0", "031102", errMsg))
                {
                    clsLog.logSave(this, "btn_workStart_Click", "작업로그 입력에 실패하였습니다/ " + work_date + "/" + work_num + "/" + "0");
                }

                //작업지시 재조회
                XMain_Search();

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                ShowMessageBox.XtraShowError("작업을 시작하는 도중 에러가 발생하였습니다");
            }
        }

        private bool XWorkStart(string plantCode, string process_key, string lCode, string work_date, string work_num)
        {
            string WorkStartSQL = string.Empty;

            DataSet Rs = null;

            int Dev = 0;
            string plcType = string.Empty;
            int j = 0;
            int[] pWorkNum = new int[15]; // 작업지시 정보전송 배열

            try
            {
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("작업지시정보를 PLC에 전송중입니다");

                #region 생산지시 번호 체크(DB 존재유무,작업중/완료 유무)
                WorkStartSQL = $@"
                SELECT BATCH, C_CONDITION 
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

                if (Dbconn.conn.getData(dukChkDs, "C_CONDITION", 0) != clsCommon.GetPcStatusCode("계획"))
                {
                    ShowMessageBox.XtraShowWarning("작업상태가 진행,완료 상태입니다\r\n현재 작업지시를 작업하실 수 없습니다.");
                    return false;
                }


                #endregion

                #region   PC->PLC 레시피쓰기 유무 체크

                clsUtil.Delay(1000);

                Array.Clear(pWorkNum, 0, pWorkNum.Length);

                splashScreenManager.SetWaitFormCaption("작업시작 여부를 체크 중입니다");

                // 작업가능 읽어오기
                if (MAIN.qPlc.ReadDeviceBlock("D0140", 1, out pWorkNum[0]) == 1)
                {
                    if (pWorkNum[0] != 1)
                    {
                        ShowMessageBox.XtraShowWarning("도징계량에서 작업지시시작이 불가능한 상태입니다(PLC1)");
                        clsLog.logSave(work_num + ": PLC1에서 레시피쓰기 불가능 ", 0);
                        return false;
                    }
                }

                if (MAIN.aPlc.ReadDeviceBlock("D0540", 1, out pWorkNum[0]) == 1)
                {
                    if (pWorkNum[0] != 1)
                    {
                        ShowMessageBox.XtraShowWarning("마이크로계량에서 작업지시시작이 불가능한 상태입니다(PLC2)");
                        clsLog.logSave(work_num + ": PLC2에서 레시피쓰기 불가능 ", 0);
                        return false;
                    }
                }

                Array.Clear(pWorkNum, 0, pWorkNum.Length);

                #endregion

                #region  PC->PLC 계량 파라미터 작업 (스케일별로 빈별로 설정값 WRITE)

                //// 스케일 배율 적용
                //WorkStartSQL = $@"
                //SELECT DECODE(A.LOCATION, NULL, 'H', A.SCALE_CODE) AS SCALE_CODE
                //    , A.LOCATION
                //    , A.INGRED_CODE
                //    , A.NUM
                //    , A.SET_VAL AS SET_VAL
                //    , NVL(C.HL_ERROR, 0) AS H_ERROR
                //    , NVL(C.LO_ERROR, 0) AS L_ERROR
                //FROM WORK_DETAIL A
                //    LEFT JOIN BIN C ON C.PLANT_CODE = A.PLANT_CODE AND C.PROCESS_KEY = A.PROCESS_KEY AND C.L_CODE = A.L_CODE AND A.LOCATION = C.LOCATION
                //WHERE A.PLANT_CODE = '{plantCode}'
                //    AND A.PROCESS_KEY = '{process_key}'
                //    AND A.L_CODE = '{lCode}'
                //    AND A.WORKDATE    = '{work_date}'
                //    AND A.NUM         = '{work_num}'
                //    AND A.SET_VAL    <> '0'
                //ORDER BY A.SCALE_CODE
                //    , A.SET_VAL DESC
                //    , A.INGRED_CODE
                //";

                //DataSet ScRoutds1 = Dbconn.conn.ExecutDataset(WorkStartSQL);

                DataSet ScRoutds1 = clsProcessDosing.resultWorkResult(plantCode, process_key, lCode, work_date, work_num);

                if (Dbconn.conn.getRowCnt(ScRoutds1) <= 0)
                {
                    clsLog.logSave(work_num + ": 존재하는 배합비가 없습니다", 0);
                    ShowMessageBox.XtraShowWarning("존재하는 배합비가 없습니다", "알림");
                    return false;
                }

                splashScreenManager.SetWaitFormCaption("레시피 정보를 PLC에 전송중입니다");

                Boolean handYn = false;
                string tmpScName = string.Empty;

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

                    if (lCode == "1")
                    {
                        switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                        {
                            case "DS101": Dev = 0000; plcType = "Q"; break;
                            case "DS102": Dev = 0100; plcType = "Q"; break;
                            case "DS103": Dev = 0200; plcType = "Q"; break;
                            case "DS104": Dev = 0300; plcType = "Q"; break;
                            case "DS105": Dev = 0400; plcType = "Q"; break;
                            case "DS106": Dev = 0000; plcType = "A"; break;
                            case "DS107": Dev = 0100; plcType = "A"; break;
                            case "DS108": Dev = 0200; plcType = "A"; break;
                            case "DS109": Dev = 0300; plcType = "A"; break;
                        }
                    }
                    else if (lCode == "2")
                    {
                        switch (ScRoutds1.Tables[0].Rows[i]["SCALE_CODE"])
                        {
                            case "DS201": Dev = 1000; plcType = "Q"; break;
                            case "DS202": Dev = 1100; plcType = "Q"; break;
                            case "DS203": Dev = 1200; plcType = "Q"; break;
                            case "DS204": Dev = 1300; plcType = "A"; break;
                            case "DS205": Dev = 1400; plcType = "A"; break;
                        }
                    }
                    else if (lCode == "3")
                    {
                        Dev = 1500; plcType = "Q";
                    }

                        // 스케일 주소
                        Dev += (j * 5);

                    //레시피배열 초기화
                    Array.Clear(pWorkNum, 0, pWorkNum.Length);

                    if (Dbconn.conn.getData(ScRoutds1, "LOCATION", i) == "소계")
                        continue;

                    pWorkNum[0] = Convert.ToInt32(Dbconn.conn.getData(ScRoutds1, "LOCATION", i));

                    //기본 KG단위, 스케일 테이블 단위컬럼 곱하기(배율)
                    pWorkNum[1] = (int)Math.Round(Convert.ToDouble(Dbconn.conn.getData(ScRoutds1, "SET_VAL", i)) * Convert.ToDouble(ScRoutds1.Tables[0].Rows[i]["IN_SCALE"]), 0); //설정량

                    if (plcType == "Q")
                    {
                        // 스케일 값을 PLC로 전송
                        if (MAIN.qPlc.WriteDeviceBlock("R" + Dev.ToString(), 2, ref pWorkNum[0]) != 0)
                        {
                            clsLog.logSave(work_num + ": 배합비전송을 실패하였습니다(빈정보,설정값)(D)", 0);
                            ShowMessageBox.XtraShowWarning("배합비전송을 실패하였습니다. 케이퍼스트에 문의해 주십시오", "알림");
                            return false;
                        }
                    }
                    else
                    {
                        // 스케일 값을 PLC로 전송
                        if (MAIN.aPlc.WriteDeviceBlock("R" + Dev.ToString(), 2, ref pWorkNum[0]) != 0)
                        {
                            clsLog.logSave(work_num + ": 배합비전송을 실패하였습니다(빈정보,설정값)(D)", 0);
                            ShowMessageBox.XtraShowWarning("배합비전송을 실패하였습니다. 케이퍼스트에 문의해 주십시오", "알림");
                            return false;
                        }
                    }

                    // 스케일 총합 체크

                    j++;
                }

                splashScreenManager.SetWaitFormCaption("수투입 여부 정보를 PLC에 전송중입니다");

                // 수투입 전송
                if (handYn == true)
                {   
                    if (MAIN.qPlc.SetDevice("D0110", 1) != 0)
                    {
                        ShowMessageBox.XtraShowWarning("수투입 여부 전송 실패");
                        clsLog.logSave(work_num + ": 수투입여부 전송 실패하였습니다(D)", 0);
                        return false;
                    }
                }
                else 
                {
                    if (MAIN.qPlc.SetDevice("D0110", 0) != 0)
                    {
                        ShowMessageBox.XtraShowWarning("수투입 여부 전송 실패");
                        clsLog.logSave(work_num + ": 수투입여부 전송 실패하였습니다(D)", 0);
                        return false;
                    }
                }

                splashScreenManager.SetWaitFormCaption("작업설정 정보를 PLC에 전송중입니다");

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
                PlcWork_Conv(plantCode, process_key, lCode, work_date, work_num, pWorkNum);

                // 배합 작업일자 배치수
                if (MAIN.qPlc.WriteDeviceBlock("D0100", 11, ref pWorkNum[0]) != 0)
                {
                    clsLog.logSave(work_num + ": 배합 작업지시 전송 실패하였습니다(PLC1)", 0);
                    ShowMessageBox.XtraShowWarning("배합 작업지시 전송 실패하였습니다.(PLC1)\r\n케이퍼스트에 문의해 주십시오", "알림");
                    return false;
                }

                /*
                작지1	D0500
                작지2	D0501
                작지3	D0502
                배치SV	D0503
                */
                // 마이크로 작업지시전송
                // 마이크로 작업일자 배치수
                if (MAIN.aPlc.WriteDeviceBlock("D0500", 5, ref pWorkNum[0]) != 0)
                {
                    clsLog.logSave(work_num + ": 마이크로 작업지시 전송 실패하였습니다(PLC1)", 0);
                    ShowMessageBox.XtraShowWarning("마이크로 작업지시 전송 실패하였습니다.(PLC1)\r\n케이퍼스트에 문의해 주십시오", "알림");
                    return false;
                }

                WorkStartSQL = $@"
                UPDATE WORK_ORDER
                SET START_TIME   = SYSDATE
                    , C_CONDITION  = '{clsCommon.GetPcStatusCode("진행")}'
                    , R_BATCH      = '1'
                WHERE PLANT_CODE = '{plantCode}'
                    AND PROCESS_KEY = '{process_key}'
                    AND L_CODE = '{lCode}'
                    AND WORKDATE   = '{work_date}'
                    AND NUM        = '{work_num}'
                ";

                Dbconn.conn.SQLrun(WorkStartSQL);

                // 팻푸드 어드레스 확인필요
                //MAIN.aPlc.SetDevice("R0500", 1);

                // 스케일별 배치완료 보기
                ////스케일 계량품목 없는 스케일 완료
                //WorkStartSQL = $@"
                //SELECT SCALE_CODE 
                //FROM SCALE 
                //WHERE SCALE_CODE NOT IN (
                //    SELECT SCALE_CODE
                //    FROM WORK_DETAIL
                //    WHERE PROCESS_KEY    = '{process_key}'
                //        AND WORKDATE     = '{work_date}'
                //        AND NUM          = '{work_num}'
                //        AND SET_VAL      <> '0'
                //        AND SCALE_CODE  NOT LIKE 'H%'
                //    GROUP BY SCALE_CODE
                //)
                //AND SCALE_CODE IN ('D1','D2','D3','D4','D5','D6','D7')
                //ORDER BY SCALE_CODE
                //";

                //DataSet noScaleDs = Dbconn.conn.ExecutDataset(WorkStartSQL);
                
                //for (int i=0; i < Dbconn.conn.getRowCnt(noScaleDs); i++ )
                //{
                //    string noScaleCd = Dbconn.conn.getData(noScaleDs, "SCALE_CODE", i);
                //    string RstDev = string.Empty;

                //    switch (noScaleCd)
                //    {
                //        case "D1": RstDev = "R0141"; break;
                //        case "D2": RstDev = "R0142"; break;
                //        case "D3": RstDev = "R0143"; break;
                //        case "D4": RstDev = "R0144"; break;
                //        case "D5": RstDev = "R0145"; break;
                //        case "D6": RstDev = "R0146"; break;
                //        case "D7": RstDev = "R0147"; break;
                //    }

                //    // 팻푸드 어드레스 확인필요
                //    if (!string.IsNullOrEmpty(RstDev))
                //    {
                //        clsMelsec.plc_dosing.SetDevice(RstDev, 2);
                //    }
                //}

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

        public static void PlcWork_Conv(string plantCode, string process_key, string lCode, string workDate, string workNum, int[] dev)
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
                , b.MIX_TIME, b.DRY_TIME, b.FINAL_TIME, b.LR_YN, b.CR_YN, b.MT_TIME, c.H
            FROM WORK_ORDER a
                INNER JOIN SAP_IN_BOM_CONM b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.NOTE = a.NOTE AND b.P_TYPE = '2'
                LEFT JOIN HAND c ON c.PLANT_CODE = a.PLANT_CODE
            WHERE a.PLANT_CODE = '{plantCode}'
                AND a.PROCESS_KEY = '{process_key}'
                AND a.L_CODE = '{lCode}'
                AND a.WORKDATE = '{workDate}'
                AND a.NUM = '{workNum}'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            DataRow dr = dt.Rows[0];

            Array.Clear(dev, 0, dev.Length);

            dev[0] = Convert.ToInt32(dr["WORKDATE"].ToString().Substring(0, 4));
            dev[1] = Convert.ToInt32(dr["WORKDATE"].ToString().Substring(4, 4));
            dev[2] = Convert.ToInt32(dr["NUM"].ToString());
            dev[3] = Convert.ToInt32(dr["BATCH"].ToString());
            dev[4] = Convert.ToInt32(dr["LOCATION_ED"].ToString());
            dev[5] = Convert.ToInt32(dr["LOCATION_ED2"].ToString() == "" ? "0" : dr["LOCATION_ED2"].ToString());
            dev[6] = Convert.ToInt32(dr["MIX_TIME"].ToString() == "" ? "0" : dr["MIX_TIME"].ToString());
            dev[7] = Convert.ToInt32(dr["DRY_TIME"].ToString() == "" ? "0" : dr["DRY_TIME"].ToString());
            dev[8] = Convert.ToInt32(dr["FINAL_TIME"].ToString() == "" ? "0" : dr["FINAL_TIME"].ToString());
            dev[9] = 1;
            dev[10] = Convert.ToInt32(dr["H"].ToString());
            dev[11] = 1;
        }
        #endregion

        #region 작업지시내역 조회
        private void work_result(string Plant_Code, string l_Code, string workdate, string num)
        {
            try
            {
                // FROM WORK_DETAIL
                DataSet ds = clsProcessDosing.resultWorkResult(Plant_Code, vProcess_Code, l_Code, workdate, num);

                clsDevexpressGrid.BindGridControl(gridControl_batchRun, gridView_batchRun, ds.Tables[0], false, true);

                sValid = new string[] { "" };


                clsDevexpressGrid.ItemLookUpEditSetup(gridcbo_run_RESOURCE_NO, clsCommon.GetResource(cboPlant_Code.EditValue?.ToString(), "", "", 0, true));

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

                    batchResultAdd(Plant_Code, l_Code, workdate, num);
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
        private void batch_result(string Plant_Code, string l_Code, string workdate, string num)
        {
            SQL = $@"
            SELECT BATCH, BA_Q, MIX_T, DRY_T, RMIX_T, ADDTM, BT_ST, I_TIME,
            SC3_TR_TIME, SC4_TR_TIME, SC5_TR_TIME, SC6_TR_TIME,
            SC7_TR_TIME, SC8_TR_TIME, SC9_TR_TIME, SC10_TR_TIME, LQ_T, MIX_DOWN_TR_TIME
            FROM BATCH
            WHERE PLANT_CODE = '{Plant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{l_Code}'
                AND WORKDATE = '{workdate}' AND NUM = '{num}'
            ORDER BY BATCH
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressGrid.BindGridControl(gridControl_Tab3, gridView_batchResult, ds.Tables[0], false, false);

            sValid = new string[] { "" };

        }
        #endregion

        #region 작업기록 조회 batch_log(string workdate, string num)
        private void batch_log(string Plant_Code, string l_Code, string workdate, string num)
        {
            SQL = $@"
            SELECT  BATCH, SEQ, LOG_CODE, ERR_MSG, ST_TIME, ED_TIME, I_TIME
            FROM BATCH_LOG
            WHERE PLANT_CODE = '{Plant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{l_Code}'
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


        private void batchList_result(string Plant_Code, string l_Code, string workdate, string num)
        {
            SQL = $@"
               SELECT a.BATCH, B.SCALE_CODE, a.LOCATION,a.INGRED_LOT AS INGRED_CODE, a.NAME
                    , c.SET_VAL, a.P_Q, ( c.SET_VAL -  a.P_Q) * -1 as P_CHA, a.P_Q_TIME
               FROM WORK_REMARK a
                    LEFT OUTER JOIN BIN b ON b.PLANT_CODE = a.PLANT_CODE AND  b.LOCATION = a.LOCATION
                    LEFT OUTER JOIN WORK_DETAIL c ON c.PLANT_CODE = a.PLANT_CODE AND c.PROCESS_KEY = a.PROCESS_KEY
                                    AND c.L_CODE = a.L_CODE AND c.WORKDATE = a.WORKDATE
                                    AND c.NUM = a.NUM AND c.INGRED_CODE = a.INGRED_LOT
               WHERE a.PLANT_CODE = '{Plant_Code}' AND a.PROCESS_KEY = '{vProcess_Code}' AND a.L_CODE = '{l_Code}'
                    AND a.WORKDATE = '{workdate}' AND a.NUM = '{num}'
               ORDER BY a.BATCH, a.P_Q DESC
               ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressGrid.BindGridControl(gridControl_Tab2, gridView_batchList, ds.Tables[0], false, false);

            sValid = new string[] { "" };

            clsDevexpressGrid.ItemLookUpEditSetup(gridBlCboResourceNo, clsCommon.GetResource(Plant_Code, "", "", 0, true));

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
                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("데이터를 불러오는 중입니다");

                DataRow row = gridView_work.GetDataRow(gridView_work.FocusedRowHandle);

                string selPLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
                string selL_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");
                string selData_workdate = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
                string selData_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");

                bool chk_version = false;
                decimal dNote_Per = 0;
                string sNOTE = clsProcessDosing.getLastVersion(selPLANT_CODE
                    , clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO")
                    , out chk_version, out dNote_Per);
                string bu_yn = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BU_YN");

                if (row.RowState == DataRowState.Added)
                {
                    

                    mix_result(gridView_work.GetRowCellValue(e.RowHandle, gridView_work.Columns["RESOURCE_NO"]).ToString(), sNOTE,
                        clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q"),
                        chk_version, bu_yn
                        );
                }
                else
                {
                    if (!string.IsNullOrEmpty(selData_workdate) && !string.IsNullOrEmpty(selData_num))
                    {
                        work_result(selPLANT_CODE, selL_CODE, selData_workdate, selData_num);
                        batchList_result(selPLANT_CODE, selL_CODE, selData_workdate, selData_num);
                        batch_result(selPLANT_CODE, selL_CODE, selData_workdate, selData_num);
                        batch_log(selPLANT_CODE, selL_CODE, selData_workdate, selData_num);
                    }
                }

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
        #endregion

        private void batchResultAdd(string Plant_Code, string l_Code, string workdate, string num)
        {
            DataSet ds1 = null;

            SQL = $"SELECT NVL(MAX(BATCH),0) AS MAX_BATCH FROM WORK_REMARK WHERE PROCESS_KEY = '{vProcess_Code}' AND WORKDATE = '{workdate}' AND NUM = '{num}' ";

            ds1 = Dbconn.conn.ExecutDataset(SQL);

            if (Convert.ToInt32(Dbconn.conn.getData(ds1, "MAX_BATCH", 0)) <= 0) return;
            int batch_cnt = Convert.ToInt32(Dbconn.conn.getData(ds1, "MAX_BATCH", 0));

            DataTable DT = (DataTable)gridControl_batchRun.DataSource;

            // 배치 컬럼 생성
            SetCreateBatch(batch_cnt, DT);

            SQL = $@"
            SELECT BATCH, LOCATION, RESOURCE_NO, P_Q, P_Q_TIME, INGRED_LOT
            FROM WORK_REMARK
            WHERE PLANT_CODE = '{Plant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{l_Code}'
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
                colBatch3.Width = 90;
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

        private void mix_result(string sResourceNo, string sNOTE, string sBATCH_Q, bool chk_version, string rc_chk)
        {
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
                    DataSet resultMixDs = clsProcessDosing.resultMixResult(vPlant_Code, vProcess_Code, vLine_Code, sResourceNo, sNOTE, sBATCH_Q, rc_chk);

                    if (resultMixDs == null || resultMixDs.Tables[0] == null) return;

                    string dupBin = string.Empty;
                    bool binSeqDupChk = clsProcessDosing.BinSeqDupChk(vPlant_Code, vProcess_Code, vLine_Code, sResourceNo, sNOTE, out dupBin);

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
            //if (clsCommon._strDosPlcConnYn != "Y" && clsCommon._strMicPlcConnYn != "Y")
            //{
            //    return;
            //}

            if (gridView_batchRun.RowCount == 0)
            {
                ShowMessageBox.XtraShowInformation("변경하실 계량내역 빈을 선택하여 주세요");
                return;
            }

            string selPLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
            string selL_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");

            string selWorkDate = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "WORKDATE");
            string selNum = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "NUM");
            string selBinCd = clsDevexpressGrid.GetFocusedRowCellValue(gridView_batchRun, "LOCATION");

            SQL = $@"
            SELECT C_CONDITION FROM WORK_ORDER
            WHERE PLANT_CODE = '{selPLANT_CODE}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{selL_CODE}'
                AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}'
            ";

            DataSet workStDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(workStDs) == 1)
            {
                int run_work_flag = 0;

                if (Dbconn.conn.getData(workStDs, "C_CONDITION", 0) == clsCommon.GetPcStatusCode("계획"))
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

                m_binChange mBinChange = new m_binChange(vPlant_Code, vProcess_Code, selL_CODE, selWorkDate, selNum, selBinCd, run_work_flag);
                mBinChange.TopMost = true;
                mBinChange.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = mBinChange.ShowDialog();
                if (result == DialogResult.OK)
                {
                    work_result(selPLANT_CODE, selL_CODE, selWorkDate, selNum);
                    batch_result(selPLANT_CODE, selL_CODE, selWorkDate, selNum);
                    batch_log(selPLANT_CODE, selL_CODE, selWorkDate, selNum);
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
            GridView view = sender as GridView;

            if (view.FocusedColumn.FieldName == "NOTE")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.getNote(vPlant_Code, view.GetFocusedRowCellValue("RESOURCE_NO")?.ToString(), "2"), "", false, 0, false, true, false, new string[] { "버전", "버전", "배합비" }, "PER");
                edit.Properties.PopupFormMinSize = new Size(500, 650);
            }

            if (view.FocusedColumn.FieldName == "LOCATION_ED" || view.FocusedColumn.FieldName == "LOCATION_ED2")
            {
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                edit.ShowPopup();
                edit.ClosePopup();

                // 여기
                clsDevexpressUtil.ItemLookUpEditSetup(edit, clsCommon.GetBin(vPlant_Code, vProcess_Code, vLine_Code, "", view.GetFocusedRowCellValue("RESOURCE_NO")?.ToString()), "", true, 0, false, true, false, new string[] { "CODE", "NAME" }, null, "CODE", "CODE");
                edit.Properties.PopupFormMinSize = new Size(500, 650);
            }
        }

        private void frm_Dosing2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (clsCommon._strDosPlcConnYn == "Y")
            //{
            //    DialogResult result = ShowMessageBox.Confirm("안내", "도징계량도중에 도징화면을 닫게 되시면 계량데이터 수집을 할 수 없습니다\r\n종료하시겠습니까?");

            //    if (result == DialogResult.Yes)
            //    {
            //        MAIN.DosPlcConnChk = "M";
            //    }
            //    else
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }

        private void reflashWorkTable(string Plant_Code, string l_Code, string workdate, string num)
        {
            XMain_Search();
            work_result(Plant_Code, l_Code, workdate, num);
            batch_result(Plant_Code, l_Code, workdate, num);
            batch_log(Plant_Code, l_Code, workdate, num);
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
            if (e.FocusedRowHandle >= 0 && e.FocusedRowHandle < gridView_work.RowCount)
            {
                try
                {
                    gridControl_batchRun.DataSource = null;
                    gridControl_Tab2.DataSource = null;
                    gridControl_Tab4.DataSource = null;

                    DataRow row = gridView_work.GetDataRow(gridView_work.FocusedRowHandle);

                    string selPLANT_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "PLANT_CODE");
                    string selL_CODE = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "L_CODE");

                    string selData_workdate = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE");
                    string selData_num = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM");

                    bool chk_version = false;
                    decimal dNote_Per = 0;
                    string sNOTE = clsProcessDosing.getLastVersion(selPLANT_CODE
                        , clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO")
                        , out chk_version, out dNote_Per);
                    string bu_yn = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BU_YN");

                    if (row.RowState == DataRowState.Added)
                    {
                        

                        mix_result(gridView_work.GetRowCellValue(e.FocusedRowHandle, gridView_work.Columns["RESOURCE_NO"]).ToString(), sNOTE,
                            clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q"),
                            chk_version, bu_yn
                            );
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(selData_workdate) && !string.IsNullOrEmpty(selData_num))
                        {
                            work_result(selPLANT_CODE, selL_CODE, selData_workdate, selData_num);
                            batchList_result(selPLANT_CODE, selL_CODE, selData_workdate, selData_num);
                            //batch_result(selData_workdate, selData_num);
                            batch_log(selPLANT_CODE, selL_CODE, selData_workdate, selData_num);
                        }
                    }
                }
                catch (Exception)
                {

                }
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
                                     WHERE PLANT_CODE = '{vPlant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{vLine_Code}'
                                     AND WORKDATE = TO_DATE('{workdate}', 'YYYYMMDD')
                                    )), 0)
                                ) ) * 60, 0)) AS PROVITY
                FROM (
                    SELECT WORK_START_DATE, 
                           SUM(PRO_Q) AS PRO_Q,
                           SUM(CASE 
                                   WHEN START_TIME IS NOT NULL AND END_TIME IS NOT NULL THEN 
                                       (TO_DATE(END_TIME, 'YYYY-MM-DD HH24:MI:SS') - TO_DATE(START_TIME, 'YYYY-MM-DD HH24:MI:SS')) * 1440 
                                   ELSE 0 
                               END) AS MIN_SUM
                    FROM WORK_ORDER
                    WHERE PLANT_CODE = '{vPlant_Code}' AND PROCESS_KEY = '{vProcess_Code}' AND L_CODE = '{vLine_Code}'
                      AND NVL(DEL_FLAG, 'N') != 'Y'
                      AND WORK_START_DATE = TO_DATE('{workdate}', 'YYYYMMDD')
                      AND C_CONDITION = '{clsCommon.GetPcStatusCode("완료")}'
                      AND (TO_DATE(END_TIME, 'YYYY-MM-DD HH24:MI:SS') - TO_DATE(START_TIME, 'YYYY-MM-DD HH24:MI:SS')) * 1440 > 0
                    GROUP BY WORK_START_DATE
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

        private void gridView_batchRun_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            // 예: "AMOUNT" 컬럼의 합계


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
                    LEFT OUTER JOIN BIN B ON a.LOCATION = B.LOCATION
                WHERE a.PLANT_CODE = '{vPlant_Code}' AND a.PROCESS_KEY = '{vProcess_Code}' AND a.L_CODE = '{vLine_Code}'
                    AND a.WORKDATE = '{clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "WORKDATE")}'
                    AND a.NUM = '{clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NUM")}'
                    AND a.BATCH = '{e.Column.FieldName.Substring(1, 1)}'
                GROUP BY B.SCALE_CODE
                ) A
                WHERE ROWNUM = 1
                ORDER BY A.PQ_TIME DESC
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

        private void gridcboNOTE_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;
            string sResourceNo = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO");
            string sBatchQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q");
            string bu_yn = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BU_YN");

            mix_result(sResourceNo, textEditor.EditValue?.ToString(), sBatchQ, chk_version, bu_yn);
        }

        /// <summary>
        /// 재가공 투입여부 체크 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridCHK_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit bu_yn = (CheckEdit)sender;
            string sResourceNo = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "RESOURCE_NO");
            string sNote = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "NOTE");
            string sBatchQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q");

            mix_result(sResourceNo, sNote,
                        clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q"),
                        chk_version, bu_yn.Checked == true ? "Y" : "N"
                        );
        }

        private void gridscboRESOURCE_NO_EditValueChanged(object sender, EventArgs e)
        {
            string cValue = string.Empty;

            TextEdit textEditor = (TextEdit)sender;

            string sResourceNo = textEditor.EditValue.ToString();
            string sBatchQ = clsDevexpressGrid.GetFocusedRowCellValue(gridView_work, "BATCH_Q");

            gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, "RESOURCE_NO", textEditor.EditValue);

            DataRow[] dr = clsCommon.GetResource(cboPlant_Code.EditValue?.ToString()).Select($"CODE = '{textEditor.EditValue}'");

            if (dr.Length > 0)
            {
                cValue = dr[0]["TYPE"].ToString();
            }

            int colonIndex = cValue.IndexOf(':');

            string result = colonIndex >= 0 ? cValue.Substring(0, colonIndex) : cValue;

            gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, "P_TYPE", result);

            string sNOTE = clsProcessDosing.getLastVersion(cboPlant_Code.EditValue?.ToString(), sResourceNo, out chk_version, out dNote_Per);

            gridView_work.SetRowCellValue(gridView_work.FocusedRowHandle, "NOTE", sNOTE);

            mix_result(sResourceNo, sNOTE, sBatchQ, chk_version, "N");
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
                SELECT LOCATION FROM BIN WHERE PLANT_CODE = '{vPlant_Code}' AND RESOURCE_NO = '{gridView_work.GetFocusedRowCellValue("RESOURCE_NO")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                {

                }

                // 입력한 값을 현재 포커스된 셀에 직접 저장
                GridView view = (gridControl_work.FocusedView as GridView);

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
                if (!isInitializing)
                    return;

                gridControl_work.DataSource = null;
                gridControl_batchRun.DataSource = null;
                gridControl_Tab2.DataSource = null;
                gridControl_Tab4.DataSource = null;

                vPlant_Code = cboPlant_Code.EditValue?.ToString();

                // 공정
                clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(vPlant_Code, clsCommon.GetProcessTypeCode("배합")), "", false, 0, false);

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(vPlant_Code, vProcess_Code), "", false, 0, false);
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
                if (!isInitializing)
                    return;

                gridControl_work.DataSource = null;
                gridControl_batchRun.DataSource = null;
                gridControl_Tab2.DataSource = null;
                gridControl_Tab4.DataSource = null;

                vProcess_Code = cboProcessKey.EditValue?.ToString();

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(vPlant_Code, vProcess_Code), "", false, 0, false);
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
                if (!isInitializing)
                    return;

                gridControl_work.DataSource = null;
                gridControl_batchRun.DataSource = null;
                gridControl_Tab2.DataSource = null;
                gridControl_Tab4.DataSource = null;

                vLine_Code = cboL_Code.EditValue?.ToString();

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
    }
}