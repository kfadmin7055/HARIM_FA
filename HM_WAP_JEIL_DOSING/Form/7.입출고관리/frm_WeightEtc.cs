using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
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
    public partial class frm_WeightEtc : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;
        private string[] sValid = null;

        DataSet authDs;

        public frm_WeightEtc()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void frm_WeightEtc_Load(object sender, EventArgs e)
        {
            authDs = clsSql.GetAuthDataSet(this.Name);

            dateEdit_workStDate.EditValue = DateTime.Today;
            dateEdit_workEdDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                    a.IS_NO, 
                    a.VEHICLEGROUPCODE,  
                    a.INCAR_NO, 
                    a.ETC_DETAIL,
                    b.I_TIME AS CHKIN_DATE,   
                    CEIL(a.IN_WEIGHT) AS IN_WEIGHT,  
                    CEIL(a.OUT_WEIGHT) AS OUT_WEIGHT, 
                    a.PC_STATUS, 
                    TO_CHAR(a.INCAR_DATE, 'YYYY-MM-DD HH24:MI:SS') AS INCAR_DATE,  
                    TO_CHAR(a.OUTCAR_DATE, 'YYYY-MM-DD HH24:MI:SS') AS OUTCAR_DATE, 
                    CEIL(a.IN_WEIGHT - a.OUT_WEIGHT) AS WEIGHT, 
                    ABS(CEIL(a.IN_WEIGHT - a.OUT_WEIGHT)) AS REAL_WEIGHT,
                    a.PC_STATUS,
                    a.I_USER,
                    a.USER_ID,
                    c.DRIVERNAME, c.DRIVERMOBILE
                FROM WAP_DECAR a
                    LEFT JOIN TMS_OUTPUT_RESULT  b ON b.IS_NO = a.IS_NO
                    LEFT JOIN TMS_INPUT_CARMASTER_CON c ON c.VEHICLENO = a.INCAR_NO
                WHERE
                    TO_CHAR(a.INCAR_DATE, 'YYYYMMDD') BETWEEN '{dateEdit_workStDate.DateTime.ToString("yyyyMMdd")}' AND '{dateEdit_workEdDate.DateTime.ToString("yyyyMMdd")}'
                    AND a.CAR_TYPE = '007'
                    AND NVL(a.DEL_FLAG, 'N') != 'Y'
                ORDER BY 
                    a.IS_NO
                ";

                DataSet ds1 = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds1.Tables[0], true, true);

                sValid = new string[] { "" };

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboETC_DETAIL, clsCommon.GetEtcCarType(), "", false, false, null, null, "NAME", "NAME");


                // 입차 상태
                repItemLkUpEdit_PC_STATUS.NullValuePrompt = "";
                repItemLkUpEdit_PC_STATUS.NullText = "";
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_PC_STATUS, clsCommon.GetCarStatus());

            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
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

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                if (clsCommon._strUserId == "kfirst") //김미정
                {

                }
                else
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

                if (e.RowHandle != this.gridView.FocusedRowHandle || e.Column.AbsoluteIndex == this.gridView.FocusedColumn.AbsoluteIndex)
                {
                    if (pc_status == "2") //출차완료
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

        private void weightProcess(string gubun)
        {
//            try
//            {
//                if (gridView.RowCount == 0)
//                {
//                    ShowMessageBox.XtraShowInformation("계근수동처리 하실 입차내역을 선택하여 주세요");
//                    return;
//                }

//                string is_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "IS_NO");
//                string incar_no = clsDevexpressGrid.GetFocusedRowCellValue(gridView, "INCAR_NO");

//                DialogResult result = ShowMessageBox.Confirm("선택하신 발급번호" + is_no + ", 차량번호" + incar_no + " 입차내역을 수동계량 하시겠습니까?");
//                if (result != DialogResult.Yes)
//                {
//                    return;
//                }

//                string scale_weight = string.Empty;

//                if (gubun == "PLC")
//                {
//                    string weight_end_chk = string.Empty;
//                    try
//                    {
//                        weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
//                    }
//                    catch
//                    {
//                        weight_end_chk = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0002");
//                    }


//                    if (weight_end_chk != "1")
//                    {
//                        ShowMessageBox.XtraShowInformation("계량스케일 안정상태가 아닙니다\r\n차량정차 후 실행해주세요");
//                        return;
//                    }

//                    string temp_weight1 = string.Empty;
//                    string temp_weight2 = string.Empty;
//                    try
//                    {
//                        temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
//                        clsUtil.Delay(200);
//                        temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
//                    }
//                    catch
//                    {
//                        temp_weight1 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0200");
//                        clsUtil.Delay(200);
//                        temp_weight2 = XGT_PLC.Tcp_Plc.PLC_Read_Word(clsCommon.plc_scale_ip, "%DW0201");
//                    }

//                    try
//                    {
//                        scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
//                    }
//                    catch (Exception)
//                    {
//                        scale_weight = clsUtil.GetDevice(Convert.ToInt32(temp_weight1), Convert.ToInt32(temp_weight2)).ToString();
//                    }
//                }
//                else if (gubun == "INPUT")
//                {
//                    string input_weight = XtraInputBox.Show("", "계량값을 입력해주세요", "");

//                    if (string.IsNullOrEmpty(input_weight))
//                    {
//                        ShowMessageBox.XtraShowInformation("계량값을 입력하지 않아 취소되었습니다");
//                        return;
//                    }

//                    scale_weight = input_weight;
//                }

//                int intScaleValue = 0;

//                bool isIntChk = int.TryParse(scale_weight, out intScaleValue);

//                if (!isIntChk)
//                {
//                    ShowMessageBox.XtraShowInformation("계량값을 정확하지 않습니다\r\n다시 실행 바랍니다");
//                    return;
//                }


//                string return_status = string.Empty;

//                mInoutSelect mInoutSelect = new mInoutSelect();
//                mInoutSelect.StartPosition = FormStartPosition.CenterScreen;
//                DialogResult dResult = mInoutSelect.ShowDialog();

//                if (dResult == DialogResult.Yes)
//                {
//                    return_status = clsCarProcess.InWeightProcess(incar_no, scale_weight, is_no);
//                }
//                else if (dResult == DialogResult.No)
//                {

//                    return_status = clsCarProcess.outChkProcess(incar_no, scale_weight, is_no);
//                }
//                else
//                {
//                    ShowMessageBox.XtraShowInformation("수동계량이 취소되었습니다\r\n입/출차 구분을 선택하여 주십쇼");
//                    return;
//                }


//                if (return_status == "OK")
//                {

//                    if (gubun == "PLC")
//                    {
///*                        try
//                        {
//                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0004", "1");
//                        }
//                        catch (Exception)
//                        {
//                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0004", "1");
//                        }

//                        clsUtil.Delay(4000);

//                        try
//                        {
//                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");
//                        }
//                        catch (Exception)
//                        {
//                            XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");
//                        }

//                        XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0005", "1");*/
//                    }

//                    ShowMessageBox.XtraShowInformation("계근수동계량처리가 완료되었습니다");
//                    XMain_Search();
//                }
//                else
//                {
//                    ShowMessageBox.XtraShowInformation("계근수동계량처리가 실패했습니다\r\n실패사유: " + return_status);
//                }
//            }
//            catch (Exception ex)
//            {
//                clsLog.logSave(this, "weightProcess", ex);
//            }
        }

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

            clsPrintReport.PrintWeighingSheet2(is_no);

            //mInOutPrintSel popForm = new mInOutPrintSel(is_no, false);
            //popForm.StartPosition = FormStartPosition.CenterScreen;
            //popForm.Show();
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
            }
            else
            {
                msgEndChk = "(안정안됨)";
            }

            btn_weightTest.Text = scale_weight + "\r\n" + msgEndChk;
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

        private void dateEdit_workStDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workEdDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            // 그리드에 새로운 행을 추가하고 편집 모드로 전환
            gridView.AddNewRow();
            int newRowHandle = gridView.FocusedRowHandle;
            gridView.SetRowCellValue(newRowHandle, gridView.Columns["I_TIME"], DateTime.Now);
            gridView.SetRowCellValue(newRowHandle, gridView.Columns["CAR_TYPE"], "007");
            gridView.SetRowCellValue(newRowHandle, gridView.Columns["IS_NO"], DateTime.Now.ToString("yyyyMMddHHmmsss"));
            gridView.SetRowCellValue(newRowHandle, gridView.Columns["WEIGHT_KG"], DateTime.Now.ToString("yyyyMMddHHmmsss"));

            gridView.SetRowCellValue(newRowHandle, gridView.Columns["DEL_FLAG"], "N");
            gridView.SetRowCellValue(newRowHandle, gridView.Columns["TEM_TYPE"], "Y");

            gridView.ShowEditor();
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            XDecar_Save();
        }

        private void XDecar_Save()
        {
            try
            {
                clsDevexpressGrid.GridEndEdit(gridView);
                Dbconn.conn.BeginTransaction();
                DataTable DT = (DataTable)gridControl.DataSource;

                foreach (DataRow dr in DT.Rows)
                {
                    DateTime? incarDate = null;
                    string sIncarDate = string.Empty;
                    string sOutcarDate = string.Empty;

                    if (!string.IsNullOrEmpty(dr["INCAR_DATE"].ToString()))
                    {
                        DateTime dtFrom = DateTime.Parse(dr["INCAR_DATE"].ToString());

                        sIncarDate = $"TO_DATE('{dtFrom.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (!string.IsNullOrEmpty(dr["INCAR_DATE"].ToString()) && !string.IsNullOrEmpty(dr["OUTCAR_DATE"].ToString()))
                    {
                        int time_diff = Convert.ToDateTime(Convert.ToDateTime(dr["OUTCAR_DATE"]).ToString("yyyy-MM-dd HH:mm:ss")).CompareTo(Convert.ToDateTime(Convert.ToDateTime(dr["INCAR_DATE"]).ToString("yyyy-MM-dd HH:mm:ss")));
                        if (time_diff < 0)
                        {
                            ShowMessageBox.XtraShowInformation("종료시간이 시작시간보다 빠르거나 같습니다");
                            return;
                        }

                        DateTime dtTo = DateTime.Parse(dr["OUTCAR_DATE"].ToString());

                        sOutcarDate = $"TO_DATE('{dtTo.ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')";
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO WAP_DECAR (
                           IS_NO, CAR_TYPE, INCAR_NO, ETC_DETAIL,
                           VEHICLEGROUPCODE, IN_WEIGHT, OUT_WEIGHT,
                           USER_ID, INCAR_DATE, OUTCAR_DATE, 
                           PC_STATUS, ERP_UP_YN, 
                           DEL_FLAG, I_TIME, I_USER) 
                        VALUES (
                           '{dr["IS_NO"]}'
                         , '007'
                         , '{dr["INCAR_NO"]}'
                         , '{dr["ETC_DETAIL"]}'
                         , '{dr["VEHICLEGROUPCODE"]}'
                         , '{dr["IN_WEIGHT"]}'
                         , '{dr["OUT_WEIGHT"]}'
                         , '{dr["USER_ID"]}'
                         , ({(string.IsNullOrEmpty(sIncarDate) ? "''" : $"{sIncarDate}")})
                         , ({(string.IsNullOrEmpty(sOutcarDate) ? "''" : $"{sOutcarDate}")})
                         , '{dr["PC_STATUS"]}'
                         , 'N'
                         , 'N'
                         , SYSDATE
                         , '{clsCommon.UserId}'
                        )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE WAP_DECAR
                        SET    CAR_TYPE         = '007'
                             , INCAR_NO         = '{dr["INCAR_NO"]}'
                             , ETC_DETAIL       = '{dr["ETC_DETAIL"]}'
                             , VEHICLEGROUPCODE = '{dr["VEHICLEGROUPCODE"]}'
                             , IN_WEIGHT        = '{dr["IN_WEIGHT"]}'
                             , OUT_WEIGHT       = '{dr["OUT_WEIGHT"]}'
                             , USER_ID          = '{dr["USER_ID"]}'
                             , INCAR_DATE       = ({(string.IsNullOrEmpty(sIncarDate) ? "''" : $"{sIncarDate}")})
                             , OUTCAR_DATE      = ({(string.IsNullOrEmpty(sOutcarDate) ? "''" : $"{sOutcarDate}")})
                             , PC_STATUS        = '{dr["PC_STATUS"]}'
                             , ERP_UP_YN = CASE WHEN ERP_UP_YN = 'Y' THEN 'M' ELSE 'N' END
                             , I_TIME           = SYSDATE
                             , I_USER           = '{clsCommon.UserId}'
                        WHERE IS_NO             = '{dr["IS_NO"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                            return;
                        }

                        SQL = $@"
                        SELECT 1 FROM TMS_OUTPUT_RESULT WHERE  IS_NO = '{gridView.GetFocusedRowCellValue("IS_NO")}'
                        ";

                        DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(ds) > 0)
                        {
                            SQL = $@"
                            UPDATE TMS_INPUT_PLOADM_CON
                            SET    PDE_YN           = 'M'
                                , ERP_UP_YN = CASE TO_CHAR(ERP_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(ERP_UP_YN) END
                                , TMS_UP_YN = CASE TO_CHAR(TMS_UP_YN) -- WHEN 'Y' THEN 'M' 
                                                            WHEN 'U' THEN 'M'
                                                                WHEN 'D' THEN 'X'
                                                                WHEN 'F' THEN 'N'
                                                                WHEN NULL THEN 'F'
                                                                ELSE TO_CHAR(TMS_UP_YN) END
                            WHERE  DISPATCHNO IN (SELECT DISPATCHNO FROM TMS_OUTPUT_RESULT WHERE  IS_NO = '{gridView.GetFocusedRowCellValue("IS_NO")}')
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 1)
                            {
                                Dbconn.conn.Rollback();
                                clsLog.logSave(this.Text, "btn_save_Click", SQL);
                                ShowMessageBox.XtraShowWarning("상차마감이 실패했습니다");
                                return;
                            }
                        }
                    }
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("차량 정보를 저장 했습니다");

                XMain_Search();
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_save_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "D"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (gridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("삭제하실 데이터를 선택하여 주세요");
                return;
            }

            clsDevexpressGrid.GridEndEdit(gridView);

            if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 차량정보를 삭제하시겠습니까?"))
            {
                return;
            }

            XDecar_Delete();
        }

        private void XDecar_Delete()
        {
            try
            {
                SQL = $"UPDATE WAP_DECAR SET DEL_FLAG = 'Y' WHERE IS_NO = '{gridView.GetFocusedRowCellValue("IS_NO")}'";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                SQL = $@"
                UPDATE TMS_INPUT_PLOADM_CON
                SET   ERP_UP_YN = 'X'
                    , TMS_UP_YN = 'X'
                WHERE  DISPATCHNO IN (SELECT DISPATCHNO FROM TMS_OUTPUT_RESULT WHERE  IS_NO = '{gridView.GetFocusedRowCellValue("IS_NO")}')
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
                    return;
                }

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_delete_Click()", ex);
                ShowMessageBox.XtraShowWarning("삭제를 실행하는 도중 에러가 발생했습니다");
            }
        }
    }
}