using Core.Class;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;
using System.Security.Cryptography;
using System.Linq;
using System.Threading;
using ACTETHERLib;
using DevExpress.XtraGrid;
using System.Drawing.Drawing2D;
using DevExpress.XtraGrid.Columns;
using System.Drawing;
using System.Diagnostics;

namespace HARIM_FA_DOSING
{
    public partial class frm_Scale : DevExpress.XtraEditors.XtraForm
    {
        DataSet authDs;
        private string SQL = String.Empty;
        private string[] sValid = null;

        public frm_Scale()
        {
            InitializeComponent();
            clsDevexpressGrid.EditGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        #region 폼로드 이벤트
        private void frm_Scale_Load(object sender, EventArgs e)
        {
            clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

            gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            authDs = clsSql.GetAuthDataSet(this.Name);

            if (!clsCommon.Auth_Form_Function(authDs, "D"))
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            else
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            //clsMelsec.plc_scale_dosing.ActHostAddress = "192.168.18.4";
            //clsMelsec.plc_scale_dosing.ActStationNumber = 1;
            //clsMelsec.plc_scale_dosing.ActPortNumber = 5002;
            //clsMelsec.plc_scale_dosing.ActSourceStationNumber = 30;
            //clsMelsec.plc_scale_dosing.ActTimeOut = 2000;

            // 플랜트
            clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", true, 0, false, true);
            cboPlant_Code.EditValue = clsCommon.PlantCode;

            XMain_Search();

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
        }
        #endregion

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                -- 스케일 마스터
                SELECT  'N' AS CHK
                      , PLANT_CODE                          -- 공장 코드
                      , SCALE_CODE                          -- 계근기 코드
                      , SCALE_NAME                          -- 계근기 명칭
                      , PROCESS_KEY                         -- 공정 코드
                      , L_CODE     -- 라인 코드
                      , MAX_Q                               -- 최대 적재 가능 중량
                      , ER_Q                                -- 잔량
                      , W_WAIT                              -- 계근 대기 시간 (시작 전)
                      , W_REQ                               -- 계량소요
                      , W_STP                               -- 계근 종료 대기 시간
                      , R_WAIT                              -- 배출 대기 시간
                      , R_REQ                               -- 계량소요
                      , R_STP                               -- 배출 종료 대기 시간
                      , MAX_HZ                              -- 최대 진동 허용값 (예상)
                      , IN_SCALE                            -- 내부 계근 여부 (Y/N)
                      , SCALE_NO                            -- 계근기 번호
                      , PLC_ADDRESS                         -- PLC 주소 (제어용)
                      , STD                                 -- PLC 시작주소
                      , SDD                                 -- PLC 전송 개수
                      , SC_BON                              -- 스케일 배율
                      , I_TIME                              -- 입력 시간 (등록 일시)
                FROM SCALE
                WHERE ('{cboPlant_Code.EditValue}' IS NULL OR PLANT_CODE = '{cboPlant_Code.EditValue}')
                    AND ('{cboProcessKey.EditValue}' IS NULL OR PROCESS_KEY = '{cboProcessKey.EditValue}')
                    AND ('{cboL_Code.EditValue}' IS NULL OR L_CODE = '{cboL_Code.EditValue?.ToString()}')
                    AND ('{txtName.EditValue}' IS NULL OR SCALE_NAME LIKE '%{txtName.EditValue}%')
                ORDER BY PLANT_CODE, PROCESS_KEY, L_CODE, SCALE_CODE
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridControl, gridView, ds.Tables[0], true, true);

                gridView.SetFixCol(new string[] {  "CHK"
                    , "PLANT_CODE"
                    , "PROCESS_KEY"
                    , "L_CODE"
                    , "SCALE_CODE"
                    , "SCALE_NAME"
                    , "SCALE_NO" });

                gridView.Columns["CHK"].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                sValid = new string[] { "PLANT_CODE", "SCALE_CODE", "PROCESS_KEY", "L_CODE" };

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

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboPlant, clsCommon.GetPlant("", true));

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPROCESS_KEY, clsCommon.GetGridProcess());

                // 라인
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), ""));

                gridCHK.ValueChecked = "Y";
                gridCHK.ValueUnchecked = "N";
                gridCHK.NullStyle = StyleIndeterminate.Unchecked;
                gridCHK.CheckStyle = CheckStyles.Standard;

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridsCboPLC_ADDRESS, clsCommon.GetPLCIP("스케일"), "", false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridCboIN_SCALE, clsCommon.GetGetInScale(), "", false, false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }
        #endregion

        #region PLC업로드 버튼클릭 이벤트
        private void btnPLCUpload_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            string plc_addr = string.Empty;

            DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

            List<string> plcIP = new List<string>();
            List<string> plcType = new List<string>();
            if (dtPlc != null && dtPlc.Rows.Count > 0)
            {
                for (int i = 0; i < dtPlc.Rows.Count; i++)
                {
                    plcIP.Add(dtPlc.Rows[i]["IP"]?.ToString());
                    plcType.Add(dtPlc.Rows[i]["PLC_TYPE"]?.ToString());
                }
            }

            if (!GetPlcCon(plcIP, plcType)) return;

            if (DialogResult.Yes != MessageBox.Show("스케일 설정 데이타를 PLC에 전송하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
            {
                return;
            }

            try
            {
                string Dev = string.Empty;

                short[] Sdata = new short[10];

                int in_scale = 1;

                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("스케일을 PLC에 전송중입니다");

                // 공정별로 빈 파라미터의 데이타를 조회 한다. 

                // 그리드의 편집 상태 먼저 취소
                gridView1.CloseEditor();               // 현재 셀 에디터 닫기
                gridView1.CancelUpdateCurrentRow();   // 현재 행의 수정 내용 취소

                DataTable dt = gridControl.DataSource as DataTable;

                clsDevexpressGrid.GridEndEdit(gridView);

                DataTable chkDt = dt
                    .AsEnumerable()
                    .Where(row => row.Field<string>("CHK") == "Y")
                    .OrderBy(row => row.Field<string>("PLC_ADDRESS"))
                    .CopyToDataTable();

                if (chkDt == null || chkDt.Rows.Count == 0)
                {
                    // 데이터가 없음 → 처리 로직
                    MessageBox.Show("체크된 스케일이 없습니다.");
                    return;
                }

                if (!UploadSetDevice(chkDt, dtPlc)) return;

                ShowMessageBox.XtraShowInformation("스케일 정보를 PLC로 Uplaod 완료하였습니다");
            }
            catch (COMException exx)
            {
                clsLog.logSave(this, MethodBase.GetCurrentMethod().Name, exx);
                ShowMessageBox.XtraShowWarning("MELSEC PLC 연결모듈 불러오기에 실패하였습니다");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                if (splashScreenManager.IsSplashFormVisible)
                {
                    splashScreenManager.CloseWaitForm();
                }
            }
        }

        private bool UploadSetDevice(DataTable chkDt, DataTable dtPlc)
        {
            string Dev = string.Empty;
            string plcIP = string.Empty;

            short[] Sdata = new short[10];
            // GBG
            int[] Sdata2 = new int[10];
            // GBG -

            int in_scale = 1;

            List<PlcUdpClient> clients = new List<PlcUdpClient>();

            string[] plcAddresses = chkDt.AsEnumerable()
            .Select(row => row.Field<string>("PLC_ADDRESS"))
            .Where(addr => !string.IsNullOrWhiteSpace(addr)) // 공백 또는 null 제거 (선택)
            .Distinct()
            .ToArray();

            // 연결
            foreach (var ip in plcAddresses)
            {
                DataRow[] resultRows = chkDt.AsEnumerable()
                   .Where(row => row.Field<string>("PLC_ADDRESS") == ip)
                   .ToArray();

                for (int i = 0; i < resultRows.Length; i++)
                {
                    DataRow dr = resultRows[i];

                    Dev = dr["STD"]?.ToString();

                    // GBG
                    if (string.IsNullOrEmpty(Dev.Trim())) continue;
                    // GBG -

                    plcIP = dr["PLC_ADDRESS"]?.ToString();

                    in_scale = Convert.ToInt16(dr["IN_SCALE"]);

                    // 빈 파라미터를 스케일별로 쏴준다. 
                    Array.Clear(Sdata, 0, Sdata.Length);

                    //double temp = Convert.ToDouble(chkDt.Rows[i]["MAX_Q"]) * in_scale;
                    //if (temp > short.MaxValue)
                    //{
                    //    ShowMessageBox.XtraShowWarning($"(최대용량, 잔량 * 스케일배율)은 SCALE ({short.MaxValue})용량 최대값을 초과 할 수 없습니다.");
                    //    return false;
                    //}

                    Sdata[0] = (short)Math.Round(Convert.ToDouble(dr["MAX_Q"]) * in_scale);          // 최대 용량
                    Sdata[1] = (short)Math.Round(Convert.ToDouble(dr["ER_Q"]) * in_scale);           // 잔량
                    Sdata[2] = Convert.ToInt16(Convert.ToDouble(dr["W_WAIT"]) * 10);                 // 계량대기
                    Sdata[3] = Convert.ToInt16(Convert.ToDouble(dr["W_REQ"]) * 10);                  // 계량소요
                    Sdata[4] = Convert.ToInt16(Convert.ToDouble(dr["W_STP"]) * 10);                  // 계량안정
                    Sdata[5] = Convert.ToInt16(Convert.ToDouble(dr["R_WAIT"]) * 10);                 // 방출대기
                    Sdata[6] = Convert.ToInt16(Convert.ToDouble(dr["R_REQ"]) * 10);                  // 방출소요
                    Sdata[7] = Convert.ToInt16(Convert.ToDouble(dr["R_STP"]) * 10);                  // 방출안정

                    DataRow[] drPlc = dtPlc.Select($"IP = '{plcIP}'");

                    if (drPlc == null || drPlc.Length == 0)
                        continue;

                    if (drPlc[0]["PLC_NO"].ToString() == "1" && drPlc[0]["PLC_TYPE"].ToString() == "Q")
                    {
                        if (MAIN.qPlc1.WriteDeviceBlock2(Dev, 10, ref Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                            return false;
                        }
                    }
                    else if (drPlc[0]["PLC_NO"].ToString() == "2" && drPlc[0]["PLC_TYPE"].ToString() == "Q")
                    {
                        if (MAIN.qPlc2.WriteDeviceBlock2(Dev, 10, ref Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                            return false;
                        }
                    }
                    // GBG
                    else if (drPlc[0]["PLC_NO"].ToString() == "1" && drPlc[0]["PLC_TYPE"].ToString() == "A")
                    {
                        if (MAIN.aPlc1.WriteDeviceBlock2(Dev, 8, ref Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                            return false;
                        }
                    }
                    else if (drPlc[0]["PLC_NO"].ToString() == "2" && drPlc[0]["PLC_TYPE"].ToString() == "A")
                    {
                        if (MAIN.aPlc2.WriteDeviceBlock2(Dev, 8, ref Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                            return false;
                        }
                    }
                    else if (dtPlc.Rows[0]["PLC_TYPE"].ToString() == "XGI")
                    {
                        if (!Dev.StartsWith("%")) continue;

                        Array.Clear(Sdata2, 0, Sdata.Length);
                        Sdata2[0] = (short)Math.Round(Convert.ToDouble(dr["MAX_Q"]) * in_scale);          // 최대 용량
                        Sdata2[1] = (short)Math.Round(Convert.ToDouble(dr["ER_Q"]) * in_scale);           // 잔량
                        Sdata2[2] = Convert.ToInt16(Convert.ToDouble(dr["W_WAIT"]) * 10);                 // 계량대기
                        Sdata2[3] = Convert.ToInt16(Convert.ToDouble(dr["W_REQ"]) * 10);                  // 계량소요
                        Sdata2[4] = Convert.ToInt16(Convert.ToDouble(dr["W_STP"]) * 10);                  // 계량안정
                        Sdata2[5] = Convert.ToInt16(Convert.ToDouble(dr["R_WAIT"]) * 10);                 // 방출대기
                        Sdata2[6] = Convert.ToInt16(Convert.ToDouble(dr["R_REQ"]) * 10);                  // 방출소요
                        Sdata2[7] = Convert.ToInt16(Convert.ToDouble(dr["R_STP"]) * 10);                  // 방출안정

                        if (clsXgiHandler.Write(2, Dev, 8, Sdata2) == 0)
                        {
                            if (clsXgiHandler.Write(2, Dev, 8, Sdata2) == 0)
                            {
                                ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                                return false;
                            }
                        }
                    }
                    else if (drPlc[0]["PLC_TYPE"].ToString() == "CM")
                    {
                        if (!Dev.StartsWith("D")) continue;

                        Array.Clear(Sdata2, 0, Sdata.Length);
                        Sdata2[0] = (short)Math.Round(Convert.ToDouble(dr["MAX_Q"]) * in_scale);          // 최대 용량
                        Sdata2[1] = (short)Math.Round(Convert.ToDouble(dr["ER_Q"]) * in_scale);           // 잔량
                        Sdata2[2] = Convert.ToInt16(Convert.ToDouble(dr["W_WAIT"]) * 10);                 // 계량대기
                        Sdata2[3] = Convert.ToInt16(Convert.ToDouble(dr["W_REQ"]) * 10);                  // 계량소요
                        Sdata2[4] = Convert.ToInt16(Convert.ToDouble(dr["W_STP"]) * 10);                  // 계량안정
                        Sdata2[5] = Convert.ToInt16(Convert.ToDouble(dr["R_WAIT"]) * 10);                 // 방출대기
                        Sdata2[6] = Convert.ToInt16(Convert.ToDouble(dr["R_REQ"]) * 10);                  // 방출소요
                        Sdata2[7] = Convert.ToInt16(Convert.ToDouble(dr["R_STP"]) * 10);                  // 방출안정

                        //_ = clsCimonHandler.TryWriteWord(Dev, Sdata2);
                        //clsUtil.Delay(1000);

                        if (clsCimonHandler2.Write(2, Dev, 10, Sdata2) == 0)
                        {
                            if (clsCimonHandler2.Write(2, Dev, 10, Sdata2) == 0)
                            {
                                ShowMessageBox.XtraShowError("스케일 파라미터 전송을 실패하였습니다.", "알림");
                                return false;
                            }
                        }
                        clsUtil.Delay(500);
                    }
                    // GBG -
                }
            }

            return true;
        }
        #endregion

        #region PLC읽기 버튼클릭 이벤트
        private void btnPLCDownLoad_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            string platCode = string.Empty;
            string processKey = string.Empty;
            string lCode = string.Empty;
            string scale_code = string.Empty;
            string Dev = string.Empty;
            int in_scale = 1;
            string sPlcIp = string.Empty;

            string SQL = null;

            DataTable dtPlc = clsCommon.GetPLCInfo(clsCommon.PlantCode, clsCommon.ProcessCode);

            List<string> plcIP = new List<string>();
            List<string> plcType = new List<string>();
            if (dtPlc != null && dtPlc.Rows.Count > 0)
            {
                for (int i = 0; i < dtPlc.Rows.Count; i++)
                {
                    plcIP.Add(dtPlc.Rows[i]["IP"]?.ToString());
                    plcType.Add(dtPlc.Rows[i]["PLC_TYPE"]?.ToString());
                }
            }

            if (!GetPlcCon(plcIP, plcType)) return;

            if (DialogResult.Yes != ShowMessageBox.Confirm("스케일정보 데이터를 PLC에서 읽어오시겠습니까?", "프로그램 스케일정보 영역에 PLC 스케일정보데이터를 덮어쓰기 합니다"))
            {
                return;
            }

            try
            {
                short[] Sdata = new short[15];
                // GBG
                int[] Sdata2 = new int[15];
                // GBG -

                splashScreenManager.ShowWaitForm();
                splashScreenManager.SetWaitFormCaption("PLC 스케일정보를 읽어오는중입니다.");

                // 그리드의 편집 상태 먼저 취소
                gridView1.CloseEditor();               // 현재 셀 에디터 닫기
                gridView1.CancelUpdateCurrentRow();   // 현재 행의 수정 내용 취소

                clsDevexpressGrid.GridEndEdit(gridView);

                DataTable dt = gridControl.DataSource as DataTable;

                DataTable chkDt = dt
                    .AsEnumerable()
                    .Where(row => row.Field<string>("CHK") == "Y")
                    .OrderBy(row => row.Field<string>("PLC_ADDRESS"))
                    .CopyToDataTable();

                if (chkDt == null || chkDt.Rows.Count == 0)
                {
                    // 데이터가 없음 → 처리 로직
                    MessageBox.Show("체크된 스케일이 없습니다.");
                    return;
                }

                for (int i = 0; i < chkDt.Rows.Count; i++)
                {
                    platCode = chkDt.Rows[i]["PLANT_CODE"]?.ToString();
                    processKey = chkDt.Rows[i]["PROCESS_KEY"]?.ToString();
                    lCode = chkDt.Rows[i]["L_CODE"]?.ToString();
                    scale_code = chkDt.Rows[i]["SCALE_CODE"]?.ToString();
                    Dev = chkDt.Rows[i]["STD"]?.ToString();

                    in_scale = Convert.ToInt16(chkDt.Rows[i]["IN_SCALE"]);

                    if (string.IsNullOrEmpty(Dev))
                    {
                        continue;
                    }

                    Array.Clear(Sdata, 0, Sdata.Length);

                    DataRow[] drPlc = dtPlc.Select($"IP = '{plcIP}'");

                    if (drPlc[0]["PLC_NO"].ToString() == "1" && drPlc[0]["PLC_TYPE"].ToString() == "Q")
                    {
                        if (MAIN.qPlc1.ReadDeviceBlock2(Dev, 10, out Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                            return;
                        }
                    }
                    if (drPlc[0]["PLC_NO"].ToString() == "2" && drPlc[0]["PLC_TYPE"].ToString() == "Q")
                    {
                        if (MAIN.qPlc2.ReadDeviceBlock2(Dev, 10, out Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                            return;
                        }
                    }
                    // GBG
                    else if (drPlc[0]["PLC_NO"].ToString() == "1" && drPlc[0]["PLC_TYPE"].ToString() == "A")
                    {
                        if (MAIN.aPlc1.ReadDeviceBlock2(Dev, 10, out Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                            return;
                        }
                    }
                    else if (drPlc[0]["PLC_NO"].ToString() == "2" && drPlc[0]["PLC_TYPE"].ToString() == "A")
                    {
                        if (MAIN.aPlc2.ReadDeviceBlock2(Dev, 10, out Sdata[0]) != 0)
                        {
                            ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                            return;
                        }
                    }
                    else if (plcType[0] == "XGI")
                    {
                        if (clsXgiHandler.Read(0, Dev, 10, Sdata2) == 0)
                        {
                            if (clsXgiHandler.Read(0, Dev, 10, Sdata2) == 0)
                            {
                                ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                                return;
                            }
                        }
                    }
                    else if (plcType[0] == "CM")
                    {
                        //_ = clsCimonHandler.TryReadWord(Dev, 10, Sdata2);
                        if (clsCimonHandler2.Read(0, Dev, 10, Sdata2) == 0)
                        {
                            clsUtil.Delay(500);
                            if (clsCimonHandler2.Read(0, Dev, 10, Sdata2) == 0)
                            {
                                ShowMessageBox.XtraShowError("SCALE 파라미터 전송을 실패하였습니다.", "알림");
                                return;
                            }
                        }
                    }
                    // GBG -

                    /*
                    MAX_Q =   용량             2000
                    ER_Q =    잔량             2001    
                    W_WAIT    계량대기         2002
                    W_REQ     계량소요         2003
                    W_STP     계량안정         2004
                    R_WAIT    방출대기         2005
                    R_REQ     방출소요         2006
                    R_STP     방출안정         2007
                              편차             2008
                    */

                    SQL = $@"
                    UPDATE SCALE
                    SET   MAX_Q  = '{(ushort)Sdata[0] / in_scale}'
                        , ER_Q   = '{(ushort)Sdata[1] / in_scale}'
                        , W_WAIT = '{(ushort)Sdata[2] / 10}'
                        , W_REQ  = '{(ushort)Sdata[3] / 10}'
                        , W_STP  = '{(ushort)Sdata[4] / 10}'
                        , R_WAIT = '{(ushort)Sdata[5] / 10}'
                        , R_REQ  = '{(ushort)Sdata[6] / 10}'
                        , R_STP  = '{(ushort)Sdata[7] / 10}'
                    WHERE PLANT_CODE = '{platCode}'
                        AND PROCESS_KEY = '{processKey}'
                        AND L_CODE = '{lCode}'
                        AND SCALE_CODE = '{scale_code}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        ShowMessageBox.XtraShowError("스케일정보를 DB에 입력하는데 실패하였습니다");
                        break;
                    }
                }

                XMain_Search();
                ShowMessageBox.XtraShowInformation("PLC에서 스케일 정보를 읽기 완료하였습니다");

            }
            catch (COMException exx)
            {
                clsLog.logSave(this, "btn_plcSend_Click", exx);
                ShowMessageBox.XtraShowWarning("MELSEC PLC 연결모듈 불러오기에 실패하였습니다");
            }
            catch (Exception ex)
            {
                clsLog.logSave(ex.Message.ToString(), 1);
                ShowMessageBox.XtraShowError(ex.Message.ToString());
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

        #region 저장버튼 클릭이벤트
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!clsCommon.Auth_Form_Function(authDs, "W"))
            {
                ShowMessageBox.XtraShowInformation("권한이 없습니다");
                return;
            }

            if (DialogResult.Yes != ShowMessageBox.Confirm("스케일정보 데이터를 저장하시겠습니까?"))
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
                    string rValid = clsCommon.ValdationCheck(sValid, dr, gridView);

                    if (!string.IsNullOrWhiteSpace(rValid))
                    {
                        gridView.FocusedColumn = gridView.Columns[rValid]; // 이동할 컬럼명
                        gridView.ShowEditor(); // 편집 모드 진입 (선택)
                        Dbconn.conn.Rollback();
                        return;
                    }

                    dr.ClearErrors();

                    //input check
                    if (string.IsNullOrEmpty(dr["SCALE_NAME"].ToString()))
                    {
                        ShowMessageBox.XtraShowWarning("스케일명을 입력하여주세요");
                        dr.SetColumnError("SCALE_NAME", "스케일명을 입력하여주세요");
                        return;
                    }

                    if (dr.RowState == DataRowState.Added)
                    {
                        SQL = $@"
                        INSERT INTO SCALE (
                            PLANT_CODE, SCALE_CODE, SCALE_NAME, 
                            PROCESS_KEY, L_CODE , MAX_Q, 
                            ER_Q, W_WAIT, W_REQ, W_STP, 
                            R_WAIT, R_REQ, R_STP, MAX_HZ, 
                            IN_SCALE, SCALE_NO, PLC_ADDRESS, 
                            STD, SDD, I_TIME) 
                        VALUES ( 
                            '{dr["PLANT_CODE"]}', '{dr["SCALE_CODE"]}', '{dr["SCALE_NAME"]}'
                            , '{dr["PROCESS_KEY"]}', '{dr["L_CODE"]}', '{dr["MAX_Q"]}'
                            , '{dr["ER_Q"]}', '{dr["W_WAIT"]}', '{dr["W_REQ"]}', '{dr["W_STP"]}'
                            , '{dr["R_WAIT"]}', '{dr["R_REQ"]}', '{dr["R_STP"]}', '{dr["MAX_HZ"]}'
                            , '{dr["IN_SCALE"]}', '{dr["SCALE_CODE"].ToString().GetOlnyNumber()}', '{dr["PLC_ADDRESS"]}'
                            , '{dr["STD"]}', '{dr["SDD"]}', SYSDATE)
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 저장에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        SQL = $@"
                        UPDATE SCALE
                        SET SCALE_NAME= '{dr["SCALE_NAME"]}', MAX_Q = '{dr["MAX_Q"]}', ER_Q = '{dr["ER_Q"]}'
                            , W_WAIT = '{dr["W_WAIT"]}', W_REQ = '{dr["W_REQ"]}', W_STP = '{dr["W_STP"]}'
                            ,R_WAIT = '{dr["R_WAIT"]}', R_REQ = '{dr["R_REQ"]}', R_STP = '{dr["R_STP"]}'
                            , MAX_HZ = '{dr["MAX_HZ"]}', IN_SCALE = '{dr["IN_SCALE"]}'
                            , PLC_ADDRESS = '{dr["PLC_ADDRESS"]}', STD = '{dr["STD"]}', SDD = '{dr["SDD"]}', SC_BON = '{dr["SC_BON"]}'
                            ,  I_TIME = SYSDATE
                        WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND SCALE_CODE = '{dr["SCALE_CODE"]}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 0)
                        {
                            clsLog.logSave(this.Text, "btn_save_Click", SQL);
                            dr.RowError = "데이터 수정에 실패했습니다";
                            ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                            return;
                        }
                    }
                    dr.AcceptChanges();
                    gridView.RefreshData();
                }

                ShowMessageBox.XtraShowInformation("저장 되었습니다.");

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
        #endregion

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

                if (gridView.SelectedRowsCount == 0)
                {
                    XtraMessageBox.Show("삭제하실 스케일을 선택하여 주세요");
                    return;
                }

                clsDevexpressGrid.GridEndEdit(gridView);

                if (DialogResult.Yes != ShowMessageBox.Confirm("선택된 스케일을 삭제하시겠습니까?"))
                {
                    return;
                }

                DataTable dt = (gridControl.DataSource as DataTable);

                if (dt == null)
                    return;

                DataTable chkDt = dt
                    .AsEnumerable()
                    .Where(row => row.Field<string>("CHK") == "Y")
                    .CopyToDataTable();

                for (int i = 0; i < chkDt.Rows.Count; i++)
                {
                    var dr = chkDt.Rows[i];

                    dr.ClearErrors();

                    SQL = $@"
                    DELETE FROM SCALE
                    WHERE PLANT_CODE = '{dr["PLANT_CODE"]}' AND PROCESS_KEY = '{dr["PROCESS_KEY"]}'
                        AND L_CODE = '{dr["L_CODE"]}'
                        AND SCALE_CODE = '{dr["SCALE_CODE"]}'
                    ";

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        clsLog.logSave(this.Text, "btn_delete_Click", SQL);
                        ShowMessageBox.XtraShowWarning("데이터 삭제에 실패했습니다");
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

            ShowMessageBox.XtraShowInformation("데이터를 삭제 했습니다.");
        }

        #region 재조회버튼 클릭이벤트
        private void btn_reflash_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }


        #endregion

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (1 + e.RowHandle).ToString();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_rowAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDefault()) return;

            clsDevexpressGrid.GridViewAddRow(gridView);

            gridView.SetFocusedRowCellValue("PLANT_CODE", cboPlant_Code.EditValue);
            gridView.SetFocusedRowCellValue("PROCESS_KEY", cboProcessKey.EditValue);
            gridView.SetFocusedRowCellValue("L_CODE", cboProcessKey.EditValue.ToString().Merge(cboL_Code.EditValue.ToString()));
        }

        private bool CheckDefault()
        {
            if (cboPlant_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("플랜트를 먼저 조회 해주세요.");
                cboPlant_Code.Focus();
                return false;
            }

            if (cboProcessKey.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("공정을 먼저 조회 해주세요.");
                cboProcessKey.Focus();
                return false;
            }

            if (cboL_Code.EditValue?.ToString() == "")
            {
                ShowMessageBox.XtraShowWarning("라인을 먼저 조회 해주세요.");
                cboL_Code.Focus();
                return false;
            }

            return true;
        }

        private void btn_rowDel_Click(object sender, EventArgs e)
        {
            clsDevexpressGrid.GridEndEdit(gridView);
            clsDevexpressGrid.GridViewLastAddRowDelete(gridView);
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

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridView.UpdateCurrentRow();

            if (e.Column.FieldName == "PROCESS_KEY")  // 수량이 변경된 경우
            {
                //라인
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), gridView.GetFocusedRowCellValue("PROCESS_KEY").ToString()), "", false);
            }
        }

        private void gridcboPROCESS_KEY_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textEditor = (TextEdit)sender;

            //라인
            clsDevexpressGrid.ItemLookUpEditSetup(gridcboL_CODE, clsCommon.GetGridLine(cboPlant_Code.EditValue?.ToString(), textEditor.EditValue?.ToString()), "", false);

            gridView.SetRowCellValue(gridView.FocusedRowHandle, "PROCESS_KEY", textEditor.EditValue?.ToString());
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcessKey, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);

            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
        }

        private void cboProcessKey_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridControl.DataSource = null;

                //라인
                clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcessKey.EditValue?.ToString()), "", false, 0, true);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            XMain_Search();
        }

        private void gridsCboPLC_ADDRESS_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                TextEdit textEditor = (TextEdit)sender;

                DataTable dt = clsCommon.GetPLCIP("빈", textEditor.EditValue.ToString());

                // 선택된 row에서 type 가져오기
                DataRow[] rows = dt.Select($"CODE = '{textEditor.EditValue.ToString()}'");
                if (rows.Length > 0)
                {
                    string typeValue = rows[0]["PLCIP"].ToString();

                    // 현재 행의 type 컬럼에 값 세팅
                    gridView.SetFocusedRowCellValue("PLC_ADDRESS", typeValue);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "cboProcess_EditValueChanged", ex);
                ShowMessageBox.XtraShowWarning("이벤트를 처리하는 도중 에러가 발생했습니다");
            }
        }

        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);

            // 헤더 클릭 && 대상 컬럼이 CHK일 때
            if (hitInfo.InColumn && hitInfo.Column.FieldName == "CHK")
            {
                // 현재 체크 상태 확인
                bool foundUnchecked = false;

                for (int i = 0; i < view.RowCount; i++)
                {
                    object val = view.GetRowCellValue(i, "CHK");

                    view.SetRowCellValue(i, "CHK", val.ToString() == "N" ? "Y" : "N");
                }

                // 강제로 헤더 다시 그림
                view.InvalidateColumnHeader(view.Columns["CHK"]);
            }
        }

        private void gridView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridView view = sender as GridView;
            DataRow dr = null;

            // 현재 행의 RowHandle 가져오기
            int rowHandle = view.FocusedRowHandle;
            string fieldName = view.FocusedColumn.FieldName;

            // DataRowView로부터 DataRow 얻기
            DataRowView drv = view.GetRow(rowHandle) as DataRowView;

            if (drv != null)
            {
                dr = drv.Row;

                if (dr.RowState != DataRowState.Added && sValid.Contains(fieldName))
                {
                    if (!view.IsNewItemRow(rowHandle))
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }
        }

        private static bool GetPlcCon(List<string> plcIP, List<string> plcType)
        {
            switch (plcIP.Count)
            {
                case 1:
                    if ((plcType[0] == "Q" && MAIN.MainPlcConnChk != "Y"))
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        return false;
                    }

                    if (plcType[0] == "A" && MAIN.SubPlcConnChk != "Y")
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번, 2번 을 먼저 연결 해주세요.");
                        return false;
                    }

                    // GBG
                    if (plcType[0] == "XGI" && MAIN.MainPlcConnChk != "Y")
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        return false;
                    }

                    if (plcType[0] == "CM" && MAIN.MainPlcConnChk != "Y")
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        return false;
                    }
                    // GBG -

                    break;
                case 2:
                    if ((MAIN.MainPlcConnChk != "Y") || (MAIN.SubPlcConnChk != "Y"))
                    {
                        ShowMessageBox.XtraShowInformation("PLC 1번, 2번 을 먼저 연결 해주세요.");
                        return false;
                    }
                    break;
                default:
                    break;
            }

            return true;
        }
    }

}