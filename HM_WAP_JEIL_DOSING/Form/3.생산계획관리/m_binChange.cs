using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.CodeParser;
using DevExpress.Data.Details;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using DevExpress.XtraScheduler.Native;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraSpreadsheet.Import.Xls;
using HARIM_FA_DOSING.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Ink;
using static System.Formats.Asn1.AsnWriter;

namespace HARIM_FA_DOSING
{
    public partial class m_binChange : DevExpress.XtraEditors.XtraForm
    {
        string SQL = string.Empty;

        string selPlantCode = string.Empty;
        string selProKey = string.Empty;
        string selLcode = string.Empty;
        string selWorkDate = string.Empty;
        string selNum = string.Empty;
        string selScale = string.Empty;

        string selScaleNo = string.Empty;
        string selScaleCode = string.Empty;
        string selLocation = string.Empty;

        string selResourceNo = string.Empty;
        string changeResourceNo = string.Empty;

        int selWorkRunFlag = 0;
        string selCondition = string.Empty;

        public static string vPLCAddress;
        public static int vPLCUnit;
        public static int vPLCDataCount;

        static int vPLC_Location = 0;

        public m_binChange(string plantCode, string processKey, string lcode, string workdate, string num, string Scale, string BinCd, string Resource, string sSetVal, string sQtyPct, int run_yn)
        {
            selPlantCode = plantCode;
            selProKey = processKey;
            selLcode = lcode;
            selWorkDate = workdate;
            selNum = num;
            selScaleCode = Scale;
            selLocation = BinCd;
            selResourceNo = Resource;

            vPLC_Location = clsCommon.GetPlcLocation(plantCode, processKey);

            selWorkRunFlag = run_yn;

            if (run_yn == 1) selCondition = clsCommon.PcStatus.Plan;
            else selCondition = clsCommon.GetPcStatusCode("진행");

            InitializeComponent();

            txtSelSetVal.EditValue = sSetVal;
            txtSelQctPct.EditValue = sQtyPct;
        }

        private void m_binChange_Load(object sender, EventArgs e)
        {
            // 빈 추가
            if (selWorkRunFlag == 1)
            {
                cboCurrentScale.ReadOnly = true;
                cboCurrentBin.ReadOnly = true;
                layoutControlItem2.Text = "선택 빈";
                layoutControlItem5.Text = "추가 빈";

                //layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layoutControlGroup.Text = "추가할 빈을 선택 해주세요. 빈 추가 후 선택 빈은 반드시 삭제되어야 합니다.";
                btn_ok.Text = "빈 추가 하기";
                btn_wait.Text = "빈 삭제 하기";
            }
            //else
            //{
            //    layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //}

            SQL = $@"
            SELECT workd.SCALE_CODE
                 , sc.SCALE_NO
                 , workd.LOCATION
                 , b.BIN_SERIAL
                 , workd.INGRED_CODE AS RESOURCE_NO
                 , workd.QTY_PCT
                 , workd.SET_VAL AS MIX_RESULT
            FROM WORK_DETAIL workd
                LEFT OUTER JOIN BIN b ON b.PLANT_CODE = workd.PLANT_CODE AND b.PROCESS_KEY = workd.PROCESS_KEY AND b.L_CODE = workd.L_CODE AND workd.LOCATION = b.LOCATION
                LEFT OUTER JOIN SCALE sc ON sc.PLANT_CODE = workd.PLANT_CODE AND sc.PROCESS_KEY = workd.PROCESS_KEY AND sc.L_CODE = workd.L_CODE AND workd.SCALE_CODE = sc.SCALE_CODE
                LEFT OUTER JOIN INGRED ing ON ing.PLANT_CODE = workd.PLANT_CODE AND workd.INGRED_CODE = ing.RESOURCE_NO
            WHERE workd.PLANT_CODE = '{selPlantCode}'
               AND workd.PROCESS_KEY = '{selProKey}'
               AND workd.L_CODE = '{selLcode}'
               AND workd.WORKDATE    = '{selWorkDate}'
               AND workd.NUM         = '{selNum}'
               AND workd.LOCATION    = '{selLocation}'
            ";

            DataSet binDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(binDs) > 0)
            {
                //selResourceNo = Dbconn.conn.getData(binDs, "RESOURCE_NO", 0);

                txt_workSeq.Text = selNum;

                SQL = $@"
                SELECT DISTINCT a.SCALE_CODE AS CODE, a.SCALE_CODE || ' : ' || b.SCALE_NAME  AS NAME
                FROM WORK_DETAIL a
                    INNER JOIN SCALE b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY AND b.L_CODE = a.L_CODE
                                 AND b.SCALE_CODE = a.SCALE_CODE
                WHERE a.PLANT_CODE = '{selPlantCode}'
                    AND a.PROCESS_KEY = '{selProKey}'
                    AND a.L_CODE = '{selLcode}'  
                    AND a.WORKDATE = '{selWorkDate}'
                    AND a.NUM = '{selNum}'
                ORDER BY a.SCALE_CODE
                ";

                DataSet dsPreS = Dbconn.conn.ExecutDataset(SQL);

                clsDevexpressUtil.ItemLookUpEditSetup(cboCurrentScale, dsPreS.Tables[0], "", false, 0);

                cboCurrentScale.EditValue = selScaleCode;

                cboChangeScale.EditValue = selScaleCode;

                cboCurrentBin.EditValue = selLocation;

                cboChangeBin.EditValue = selLocation;

                selScaleNo = Dbconn.conn.getData(binDs, "SCALE_NO", 0);

                // GBG
                // 하림-김제의 경우  D1, D2, M1(D4), M2(D5) 스케일만 빈변경 가능
                if (clsCommon.PlantCode == "P101")
                {
                    if (selScaleCode != "D1" && selScaleCode != "D2" && selScaleCode != "D4" && selScaleCode != "D5")
                    {
                        ShowMessageBox.XtraShowInformation($"{selScaleCode} 스케일은 빈변경이 가능하지 않습니다.");
                        this.Close();
                    }
                }

                // 하림-정읍의 경우  D1, D2, D3 스케일만 빈변경 가능
                if (clsCommon.PlantCode == "P102")
                {
                    if (selScaleCode != "D1" && selScaleCode != "D2" && selScaleCode != "D3")
                    {
                        ShowMessageBox.XtraShowInformation($"{selScaleCode} 스케일은 빈변경이 가능하지 않습니다.");
                        this.Close();
                    }
                }
                // GBG -
            }
            else
            {
                ShowMessageBox.XtraShowInformation("빈정보를 불러오지 못했습니다");
            }

            this.ActiveControl = cboChangeBin;
            cboChangeBin.Focus();
        }

        /// <summary>
        /// 빈보류
        /// </summary>
        /// <returns></returns>
        private bool Set_Plc_binWaitControl()
        {
            string plc_addr = string.Empty;

            List<string> plcType = new List<string>();
            DataTable dtPlc = clsCommon.GetPLCInfo(selPlantCode, selProKey);

            if (!PlcFunc.GetPlcCon(dtPlc, out MAIN.sErrMsg))
            {
                ShowMessageBox.XtraShowInformation(MAIN.sErrMsg);
                return false;
            }

            try
            {
                string Dev = string.Empty;
                int i = 0;

                short[] Sdata = new short[10];

                int in_scale = 1;

                // 공정별로 빈 파라미터의 데이타를 조회 한다. 

                DataTable chkDt = null;

                if (!WaitUploadSetDevice(dtPlc)) return false;
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
                clsMelsec.plc_scale_dosing.Close();
            }

            return true;
        }

        /*
         빈보류스케일	D0126
         보류빈	        D0127
         빈보류요구	    D0128
        */
        private bool WaitUploadSetDevice(DataTable dtPlc)
        {
            int Dev = 0;
            short[] Sdata = new short[10];
            // GBG
            int[] Sdata2 = new int[5] { short.Parse(selScaleNo), short.Parse(cboCurrentBin.EditValue?.ToString()), 1, 0, 0 };
            // GBG - 

            Sdata = new short[] { short.Parse(selScaleNo), short.Parse(cboCurrentBin.EditValue?.ToString()), 1 };

            clsCommon.GetPLCAddress(selPlantCode
                                        , selProKey
                                        , selLcode
                                        , vPLC_Location
                                        , PlcAddressType.BINLOCKYN.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

            return PlcFunc.PLCSetWriteQDeviceBlock2Ex(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref Sdata, Sdata2, "빈보류 파라미터 전송을 실패하였습니다.");
        }

        private bool Set_Plc_binChangeControl()
        {
            string plc_addr = string.Empty;

            try
            {
                DataTable dtPlc = clsCommon.GetPLCInfo(selPlantCode, "", selProKey);

                if (!PlcFunc.GetPlcCon(dtPlc, out MAIN.sErrMsg))
                {
                    ShowMessageBox.XtraShowInformation(MAIN.sErrMsg);
                    return false;
                }

                string Dev = string.Empty;

                short[] Sdata = new short[10];

                // GBG
                int iPlcType = 1;
                if (clsCommon.PlantCode == "PJ01")
                {
                    switch (selScaleCode)
                    {
                        case "DS101": iPlcType = 1; break;
                        case "DS102": iPlcType = 1; break;
                        case "DS103": iPlcType = 1; break;
                        case "DS104": iPlcType = 1; break;
                        case "DS105": iPlcType = 1; break;
                        case "DS106": iPlcType = 2; break;
                        case "DS107": iPlcType = 2; break;
                        case "DS108": iPlcType = 2; break;
                        case "DS109": iPlcType = 2; break;

                        case "DS201": iPlcType = 1; break;
                        case "DS202": iPlcType = 1; break;
                        case "DS203": iPlcType = 1; break;
                        case "DS204": iPlcType = 2; break;
                        case "DS205": iPlcType = 2; break;
                    }
                }
                // GBG -

                // GBG
                if (!ChangeUploadSetDevice(dtPlc, iPlcType)) return false;
                // GBG -
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
                clsMelsec.plc_scale_dosing.Close();
            }

            return true;
        }

        /*
         변경전스케일	D0121
         변경전빈	    D0122
         변경후스케일	D0123
         변경후빈	    D0124
         빈변경요구	    D0125
        */
        private bool ChangeUploadSetDevice(DataTable dtPlc, int plcIndex)
        {
            int Dev = 0;
            short[] Sdata = new short[10];
            // GBG
            int[] Sdata2 = new int[10] { short.Parse(selScaleNo), short.Parse(cboCurrentBin.EditValue?.ToString()), short.Parse(selScaleNo), short.Parse(cboChangeBin.EditValue.ToString()), 1, 0, 0, 0, 0, 0 };
            // GBG -

            Sdata = new short[] { short.Parse(selScaleNo), short.Parse(cboCurrentBin.EditValue?.ToString()), short.Parse(selScaleNo), short.Parse(cboChangeBin.EditValue.ToString()), 1 };

            clsCommon.GetPLCAddress(selPlantCode
                                        , selProKey
                                        , selLcode
                                        , vPLC_Location
                                        , PlcAddressType.BINCHANGE.GetDesc()
                                        , 1
                                        , out vPLCAddress
                                        , out vPLCUnit
                                        , out vPLCDataCount);

            return PlcFunc.PLCSetWriteQDeviceBlock2Ex(dtPlc, vPLC_Location, vPLCAddress, vPLCUnit, vPLCDataCount, ref Sdata, Sdata2, "빈보류 파라미터 전송을 실패하였습니다.");
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                string r_batch = string.Empty;
                // 빈 추가
                if (selWorkRunFlag == 1)
                {
                    if (cboChangeBin.EditValue == null || cboChangeBin.EditValue.ToString() == "")
                    {
                        this.DialogResult = DialogResult.None;
                        ShowMessageBox.XtraShowInformation("추가 할 빈을 선택하여 주세요");
                        return;
                    }

                    if (cboChangeBin.EditValue.ToString() == "H")
                    {
                        SQL = $@"
                        UPDATE WORK_DETAIL d
                            SET d.LOCATION    = 'H'
                                , d.SCALE_CODE    = 'H'
                        WHERE d.PLANT_CODE  = '{selPlantCode}'
                            AND d.PROCESS_KEY = '{selProKey}'
                            AND d.L_CODE      = '{selLcode}'
                            AND d.WORKDATE    = '{selWorkDate}'
                            AND d.LOCATION    = '{cboCurrentBin.EditValue}'
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            this.DialogResult = DialogResult.None;
                            clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                            ShowMessageBox.XtraShowInformation("빈변경을 하는 도중 오류가 발생했습니다");
                            return;
                        }

                        string errMsg = cboCurrentBin.EditValue?.ToString() + "빈을 수작업으로 변경 했습니다.";
                        if (!clsProcessDosing.InsertLog(selPlantCode, selProKey, selLcode, selWorkDate, selNum, "0", selCondition, errMsg))
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave("btn_ok_Click", "btn_ok_Click", SQL);
                            return;
                        }
                    }
                    else 
                    {
                        string sBin = cboChangeBin.EditValue?.ToString();

                        if (cboChangeScale.EditValue?.ToString() == "H")
                            sBin = "H";

                        SQL = $@"
                        INSERT INTO WORK_DETAIL (
                            PLANT_CODE, PROCESS_KEY, L_CODE , WORKDATE, NUM, 
                            INGRED_CODE, SCALE_CODE, LOCATION, 
                            QTY_PCT, SET_VAL,
                            HR_ERR, SEQ, BIN_SEQ, I_TIME) 
                        SELECT PLANT_CODE, PROCESS_KEY, L_CODE , WORKDATE, NUM, 
                            '{changeResourceNo}', '{cboChangeScale.EditValue}', '{sBin}', 
                            MAX(QTY_PCT), MAX(SET_VAL), 
                            MAX(HR_ERR), MAX(SEQ) + 1, MAX(NVL(BIN_SEQ, 0)) + 1, MAX(SYSDATE)
                        FROM WORK_DETAIL
                        WHERE PLANT_CODE = '{selPlantCode}'
                            AND PROCESS_KEY = '{selProKey}'
                            AND L_CODE = '{selLcode}'
                            AND WORKDATE = '{selWorkDate}'
                            AND NUM = '{selNum}'
                            AND LOCATION = '{cboCurrentBin.EditValue}'
                        GROUP BY PLANT_CODE, PROCESS_KEY, L_CODE , WORKDATE, NUM
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            this.DialogResult = DialogResult.None;
                            clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                            ShowMessageBox.XtraShowInformation("빈 추가를 하는 도중 오류가 발생했습니다");
                            return;
                        }

                        string errMsg = cboCurrentBin.EditValue?.ToString() + "빈을 추가 했습니다";
                        if (!clsProcessDosing.InsertLog(selPlantCode, selProKey, selLcode, selWorkDate, selNum, "0", selCondition, errMsg))
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave("btn_ok_Click", "btn_ok_Click", SQL);
                            return;
                        }
                    }

                    clsProcessDosing.SetWorkOrderQty(selPlantCode, selProKey, selLcode, selWorkDate, selNum);

                    //SelectBinDelete();
                }
                // 빈 변경
                else
                {
                    DataTable dtPlc = clsCommon.GetPLCInfo(selPlantCode, "", selProKey);

                    if (!PlcFunc.GetPlcCon(dtPlc, out MAIN.sErrMsg))
                    {
                        ShowMessageBox.XtraShowInformation(MAIN.sErrMsg);
                        return;
                    }

                    if (cboChangeBin.EditValue == null || cboChangeBin.EditValue?.ToString() == "" || (cboCurrentBin.EditValue?.ToString() == cboChangeBin.EditValue?.ToString()))
                    {
                        this.DialogResult = DialogResult.None;
                        ShowMessageBox.XtraShowInformation("변경 할 빈을 선택하여 주세요");
                        return;
                    }

                    SQL = $@"
                    SELECT NVL(R_BATCH, 0) + 1 AS R_BATCH
                    FROM WORK_ORDER
                    WHERE PLANT_CODE  = '{selPlantCode}'
                        AND PROCESS_KEY = '{selProKey}'
                        AND L_CODE      = '{selLcode}'
                        AND WORKDATE    = '{selWorkDate}'
                        AND NUM         = '{selNum}'
                    ";

                    using (DataSet rbatchDs = Dbconn.conn.ExecutDataset(SQL))
                    {
                        r_batch = Dbconn.conn.getData(rbatchDs, "R_BATCH", 0);
                    }

                    SQL = $@"
                    SELECT 
                       PLANT_CODE, PROCESS_KEY, L_CODE
                       , R_BATCH, LOCATION, NUM, WORKDATE
                    FROM BIN_CHANGE_HISTORY
                    WHERE PLANT_CODE = '{selPlantCode}' AND PROCESS_KEY = '{selProKey}' AND L_CODE = '{selLcode}'
                        AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}' AND R_BATCH = '{r_batch}' AND LOCATION = '{cboChangeBin.EditValue.ToString()}'
                    ";

                    using (DataSet ds = Dbconn.conn.ExecutDataset(SQL))
                    {
                        if (Dbconn.conn.getRowCnt(ds) > 0)
                        {
                            ShowMessageBox.XtraShowInformation("현배치에 변경 이력이 있는 빈입니다.");
                            return;
                        }
                    }

                    SQL = $@"
                    SELECT 
                    PROCESS_KEY, WORKDATE, NUM, 
                       INGRED_CODE AS RESOURCE_NO, SCALE_CODE, LOCATION, 
                       QTY_PCT, SET_VAL, BIN_TYPE, 
                       HR_ERR, SEQ, I_TIME
                    FROM WORK_DETAIL
                    WHERE PLANT_CODE = '{selPlantCode}' AND PROCESS_KEY = '{selProKey}' AND L_CODE = '{selLcode}'
                        AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}' AND LOCATION = '{cboCurrentBin.EditValue}'
                    ";

                    DataSet detailInfoDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(detailInfoDs) > 0)
                    {
                        Dbconn.conn.BeginTransaction();

                        SQL = $@"
                        SELECT 
                        PROCESS_KEY, WORKDATE, NUM, 
                           INGRED_CODE AS RESOURCE_NO, SCALE_CODE, LOCATION, 
                           QTY_PCT, SET_VAL, BIN_TYPE, 
                           HR_ERR, SEQ, I_TIME
                        FROM WORK_DETAIL
                        WHERE PLANT_CODE = '{selPlantCode}' AND PROCESS_KEY = '{selProKey}' AND L_CODE = '{selLcode}'
                            AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}' AND LOCATION = '{cboChangeBin.EditValue.ToString()}'
                        ";

                        using (DataSet ds = Dbconn.conn.ExecutDataset(SQL))
                        {
                            if (Dbconn.conn.getRowCnt(ds) > 0)
                            {
                                return;
                            }
                        }

                        //SQL = $@"
                        //DELETE 
                        //FROM WORK_DETAIL
                        //WHERE PLANT_CODE = '{selPlantCode}' AND PROCESS_KEY = '{selProKey}' AND L_CODE = '{selLcode}'
                        //AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}' AND LOCATION = '{cboCurrentBin.EditValue}'
                        //";

                        //if (Dbconn.conn.SQLrun(SQL) < 0)
                        //{
                        //    Dbconn.conn.Rollback();
                        //    this.DialogResult = DialogResult.None;
                        //    ShowMessageBox.XtraShowInformation("빈변경을 하는 도중 오류가 발생했습니다");
                        //    return;
                        //}

                        SQL = $@"
                        INSERT INTO WORK_DETAIL (
                            PLANT_CODE, PROCESS_KEY, L_CODE , WORKDATE, NUM, 
                            INGRED_CODE, SCALE_CODE, LOCATION, 
                            QTY_PCT, SET_VAL,
                            HR_ERR, SEQ, BIN_SEQ, PARENT_BIN, I_TIME) 
                        VALUES ( 
                            '{selPlantCode}', '{selProKey}', '{selLcode}', '{selWorkDate}', '{selNum}',
                            '{Dbconn.conn.getData(detailInfoDs, "RESOURCE_NO", 0)}', '{selScaleCode}', '{cboChangeBin.EditValue.ToString()}',
                            '0', '0',
                            NULL, '{Dbconn.conn.getData(detailInfoDs, "SEQ", 0)}'
                                 , (SELECT NVL(MAX(BIN_SEQ) + 1, 1)
                                    FROM WORK_DETAIL
                                    WHERE PLANT_CODE = '{selPlantCode}' AND PROCESS_KEY = '{selProKey}' AND L_CODE = '{selLcode}'
                                        AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}' AND SCALE_CODE = '{selScaleCode}')
                                 , (SELECT MAX(CASE WHEN NVL(PARENT_BIN, '') IS NULL THEN LOCATION ELSE PARENT_BIN END)
                                    FROM WORK_DETAIL
                                    WHERE PLANT_CODE = '{selPlantCode}' AND PROCESS_KEY = '{selProKey}' AND L_CODE = '{selLcode}'
                                        AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}' AND SCALE_CODE = '{selScaleCode}' AND LOCATION = '{cboCurrentBin.EditValue}')
                                , SYSDATE )
                        ";

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            Dbconn.conn.Rollback();
                            this.DialogResult = DialogResult.None;
                            clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                            ShowMessageBox.XtraShowInformation("빈변경을 하는 도중 오류가 발생했습니다");
                            return;
                        }

                        if (!(selPlantCode == "PJ04"
                                && Dbconn.conn.getRowCnt(
                                    Dbconn.conn.ExecutDataset($"SELECT * FROM SAP_DI_PRODUCT WHERE DESCRIPTION = '분쇄옥수수(소)' AND RESOURCE_NO = '{Dbconn.conn.getData(detailInfoDs, "RESOURCE_NO", 0)}'")
                                    ) > 0))
                        {
                            //빈 우선순위 변경빈으로 변경
                            SQL = $@"
                            UPDATE BIN
                            SET SEQ = '1'
                            WHERE  PLANT_CODE = '{selPlantCode}' AND PROCESS_KEY = '{selProKey}' AND L_CODE = '{selLcode}'
                                AND LOCATION = '{cboChangeBin.EditValue.ToString()}'
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 1)
                            {
                                Dbconn.conn.Rollback();
                                this.DialogResult = DialogResult.None;
                                clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                                ShowMessageBox.XtraShowInformation("빈변경을 하는 도중 오류가 발생했습니다");
                                return;
                            }

                            //나머지빈은 세컨드빈으로
                            SQL = $@"
                            UPDATE BIN SET SEQ = '2'
                            WHERE PLANT_CODE = '{selPlantCode}' AND PROCESS_KEY = '{selProKey}' AND L_CODE = '{selLcode}' 
                                AND LOCATION <> '{cboChangeBin.EditValue.ToString()}'
                                AND SCALE_CODE = '{Dbconn.conn.getData(detailInfoDs, "SCALE_CODE", 0)}' AND RESOURCE_NO = '{Dbconn.conn.getData(detailInfoDs, "RESOURCE_NO", 0)}'
                            ";

                            if (Dbconn.conn.SQLrun(SQL) < 1)
                            {
                                Dbconn.conn.Rollback();
                                this.DialogResult = DialogResult.None;
                                clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                                ShowMessageBox.XtraShowInformation("빈변경을 하는 도중 오류가 발생했습니다");
                                return;
                            }

                            SQL = $@"
                            SELECT 1
                                FROM WORK_ORDER a
                                WHERE a.PLANT_CODE  = '{selPlantCode}'
                                    AND a.PROCESS_KEY = '{selProKey}'
                                    AND a.L_CODE      = '{selLcode}'
                                    AND a.WORKDATE    = '{selWorkDate}'
                                    AND a.C_CONDITION = '{clsCommon.PcStatus.Plan}'
                            ";

                            using (DataSet ds = Dbconn.conn.ExecutDataset(SQL))
                            {
                                // 계획중인 작업지시에 동일 빈이 있는지 체크
                                SQL = $@"
                                SELECT * FROM WORK_DETAIL d
                                 WHERE d.PLANT_CODE  = '{selPlantCode}'
                                   AND d.PROCESS_KEY = '{selProKey}'
                                   AND d.L_CODE      = '{selLcode}'
                                   AND d.WORKDATE    = '{selWorkDate}'
                                   AND d.LOCATION    = '{cboCurrentBin.EditValue}'
                                   AND EXISTS (
                                           SELECT 1
                                             FROM WORK_ORDER a
                                            WHERE a.PLANT_CODE  = d.PLANT_CODE
                                              AND a.PROCESS_KEY = d.PROCESS_KEY
                                              AND a.L_CODE      = d.L_CODE
                                              AND a.WORKDATE    = d.WORKDATE
                                              AND a.NUM         = d.NUM
                                              AND a.C_CONDITION = '{clsCommon.PcStatus.Plan}'
                                              AND a.DEL_FLAG != 'Y'
                                       )
                                ";

                                DataSet ds2 = Dbconn.conn.ExecutDataset(SQL);

                                if (Dbconn.conn.getRowCnt(ds2) > 0)
                                {
                                    SQL = $@"
                                    UPDATE WORK_DETAIL d
                                       SET d.LOCATION    = '{cboChangeBin.EditValue}'
                                            , d.SCALE_CODE    = '{cboChangeScale.EditValue}'
                                    WHERE d.PLANT_CODE  = '{selPlantCode}'
                                       AND d.PROCESS_KEY = '{selProKey}'
                                       AND d.L_CODE      = '{selLcode}'
                                       AND d.WORKDATE    = '{selWorkDate}'
                                       AND d.LOCATION    = '{cboCurrentBin.EditValue}'
                                       AND EXISTS (
                                               SELECT 1
                                                 FROM WORK_ORDER a
                                                WHERE a.PLANT_CODE  = d.PLANT_CODE
                                                  AND a.PROCESS_KEY = d.PROCESS_KEY
                                                  AND a.L_CODE      = d.L_CODE
                                                  AND a.WORKDATE    = d.WORKDATE
                                                  AND a.NUM         = d.NUM
                                                  AND a.C_CONDITION = '{clsCommon.PcStatus.Plan}'
                                                  AND a.DEL_FLAG != 'Y'
                                           )
                                    ";

                                    if (Dbconn.conn.SQLrun(SQL) < 1)
                                    {
                                        Dbconn.conn.Rollback();
                                        this.DialogResult = DialogResult.None;
                                        clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                                        ShowMessageBox.XtraShowInformation("빈변경을 하는 도중 오류가 발생했습니다");
                                        return;
                                    }
                                }
                            }
                        }

                        //SQL = $@"
                        ///* TB01 : BIN_CHANGE_HISTORY */
                        //MERGE INTO BIN_CHANGE_HISTORY D
                        //USING (
                        //    SELECT     '{selLcode}'               AS L_CODE        /* 01 */
                        //            ,  '{cboCurrentBin.EditValue}'           AS LOCATION      /* 02 */
                        //            ,  '{selNum}'                    AS NUM           /* 03 */
                        //            ,  '{selPlantCode}'     AS PLANT_CODE    /* 04 */
                        //            ,  '{selProKey}'            AS PROCESS_KEY   /* 05 */
                        //            ,  '{r_batch}'            AS R_BATCH       /* 06 */
                        //            ,  '{selWorkDate}'       AS WORKDATE      /* 07 */
                        //            ,  SYSDATE                        AS I_TIME        /* 08 */
                        //    FROM DUAL
                        //    UNION ALL
                        //    SELECT     '{selLcode}'               AS L_CODE        /* 01 */
                        //            ,  '{cboChangeBin.EditValue.ToString()}'           AS LOCATION      /* 02 */
                        //            ,  '{selNum}'                    AS NUM           /* 03 */
                        //            ,  '{selPlantCode}'     AS PLANT_CODE    /* 04 */
                        //            ,  '{selProKey}'            AS PROCESS_KEY   /* 05 */
                        //            ,  '{r_batch}'            AS R_BATCH       /* 06 */
                        //            ,  '{selWorkDate}'       AS WORKDATE      /* 07 */
                        //            ,  SYSDATE                        AS I_TIME        /* 08 */
                        //    FROM DUAL
                        //) S
                        //ON (
                        //        D.L_CODE       = S.L_CODE
                        //    AND D.NUM          = S.NUM
                        //    AND D.PLANT_CODE   = S.PLANT_CODE
                        //    AND D.PROCESS_KEY  = S.PROCESS_KEY
                        //    AND D.WORKDATE     = S.WORKDATE
                        //    AND D.R_BATCH     = S.R_BATCH
                        //    AND D.LOCATION     = S.LOCATION
                        //)
                        //WHEN NOT MATCHED THEN
                        //    INSERT (
                        //            L_CODE          /* 01 */
                        //        , LOCATION        /* 02 */
                        //        , NUM             /* 03 */
                        //        , PLANT_CODE      /* 04 */
                        //        , PROCESS_KEY     /* 05 */
                        //        , R_BATCH         /* 06 */
                        //        , WORKDATE        /* 07 */
                        //        , I_TIME          /* 08 */
                        //    )
                        //    VALUES (
                        //            S.L_CODE        /* 01 */
                        //        , S.LOCATION      /* 02 */
                        //        , S.NUM           /* 03 */
                        //        , S.PLANT_CODE    /* 04 */
                        //        , S.PROCESS_KEY   /* 05 */
                        //        , S.R_BATCH       /* 06 */
                        //        , S.WORKDATE      /* 07 */
                        //        , S.I_TIME        /* 08 */
                        //    )
                        //";

                        SQL = $@"
                        /* TB01 : BIN_CHANGE_HISTORY */
                        INSERT INTO BIN_CHANGE_HISTORY (
                              L_CODE        /* 01 */
                            , LOCATION      /* 02 */
                            , NUM           /* 03 */
                            , PLANT_CODE    /* 04 */
                            , PROCESS_KEY   /* 05 */
                            , R_BATCH       /* 06 */
                            , WORKDATE      /* 07 */
                            , I_TIME        /* 08 */
                        )
                        SELECT     '{selLcode}'                          AS L_CODE        /* 01 */
                                ,  '{cboCurrentBin.EditValue}'           AS LOCATION      /* 02 */
                                ,  '{selNum}'                            AS NUM           /* 03 */
                                ,  '{selPlantCode}'                      AS PLANT_CODE    /* 04 */
                                ,  '{selProKey}'                         AS PROCESS_KEY   /* 05 */
                                ,  '{r_batch}'                           AS R_BATCH       /* 06 */
                                ,  '{selWorkDate}'                       AS WORKDATE      /* 07 */
                                ,  SYSDATE                               AS I_TIME        /* 08 */
                        FROM DUAL
                        UNION ALL
                        SELECT     '{selLcode}'                          AS L_CODE        /* 01 */
                                ,  '{cboChangeBin.EditValue}'            AS LOCATION      /* 02 */
                                ,  '{selNum}'                            AS NUM           /* 03 */
                                ,  '{selPlantCode}'                      AS PLANT_CODE    /* 04 */
                                ,  '{selProKey}'                         AS PROCESS_KEY   /* 05 */
                                ,  '{r_batch}'                           AS R_BATCH       /* 06 */
                                ,  '{selWorkDate}'                       AS WORKDATE      /* 07 */
                                ,  SYSDATE                               AS I_TIME        /* 08 */
                        FROM DUAL
                        ";

                        Dbconn.conn.SQLrun(SQL);

                        string errMsg = cboCurrentBin.EditValue + "빈에서 " + cboChangeBin.EditValue.ToString() + "빈으로 변경했습니다";

                        if (!clsProcessDosing.InsertLog(selPlantCode, selProKey, selLcode, selWorkDate, selNum, r_batch, selCondition, errMsg))
                        {
                            Dbconn.conn.Rollback();
                            clsLog.logSave("btn_ok_Click", "btn_ok_Click", SQL);
                            return;
                        }

                        Dbconn.conn.Commit();
                        this.DialogResult = DialogResult.OK;

                        if (selWorkRunFlag == 2)
                        {
                            bool returnTaskSendResult = Set_Plc_binChangeControl();

                            if (!returnTaskSendResult)
                            {
                                Dbconn.conn.Rollback();
                                ShowMessageBox.XtraShowWarning("빈변경을 실패했습니다");
                                return;
                            }
                        }

                        ShowMessageBox.XtraShowInformation("빈 변경을 했습니다.");
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        ShowMessageBox.XtraShowInformation("작업지시 정보를 찾을 수 없습니다");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_ok_Click", ex);
                ShowMessageBox.XtraShowWarning("빈변경을 하는 도중 에러가 발생했습니다" + SQL);
            }

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_wait_Click(object sender, EventArgs e)
        {
            if (selWorkRunFlag == 1)
            {
                SelectBinDelete();
            }
            else
            {
                DialogResult result = ShowMessageBox.Confirm(cboCurrentBin.Text + " 빈번호를 보류하시겠습니까?");
                if (result != DialogResult.Yes)
                {
                    return;
                }

                bool returnTaskSendResult = Set_Plc_binWaitControl();

                if (!returnTaskSendResult)
                {
                    ShowMessageBox.XtraShowWarning("빈보류를 실패했습니다");
                    return;
                }

                string r_batch = string.Empty;

                SQL = $"SELECT NVL(R_BATCH,0) as R_BATCH FROM WORK_ORDER WHERE PROCESS_KEY = '{selProKey}' AND WORKDATE = '{selWorkDate}' AND NUM = '{selNum}' ";

                using (DataSet rbatchDs = Dbconn.conn.ExecutDataset(SQL))
                {
                    r_batch = Dbconn.conn.getData(rbatchDs, "R_BATCH", 0);
                }

                string errMsg = cboCurrentBin.EditValue + "빈을 빈보류했습니다";
                if (!clsProcessDosing.InsertLog(selPlantCode, selProKey, selLcode, selWorkDate, selNum, r_batch, "031104", errMsg))
                {
                    return;
                }
            }

            this.Close();
        }

        private void SelectBinDelete()
        {
            DialogResult result = ShowMessageBox.Confirm("[" + cboCurrentBin.Text + "] 빈을 레시페어서 삭제 하시겠습니까?");
            if (result != DialogResult.Yes)
            {
                return;
            }

            SQL = $@"
                DELETE
                FROM WORK_DETAIL a
                WHERE a.PLANT_CODE = '{selPlantCode}'
                    AND a.PROCESS_KEY = '{selProKey}'
                    AND a.L_CODE = '{selLcode}'
                    AND a.WORKDATE = '{selWorkDate}'
                    AND a.NUM = '{selNum}'
                    AND a.LOCATION = '{cboCurrentBin.EditValue}'
                ";

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                Dbconn.conn.Rollback();
                this.DialogResult = DialogResult.None;
                clsLog.logSave("m_binChange", "btn_ok_Click", SQL);
                ShowMessageBox.XtraShowInformation("빈 삭제를 하는 도중 오류가 발생했습니다");
                return;
            }

            clsProcessDosing.SetWorkOrderQty(selPlantCode, selProKey, selLcode, selWorkDate, selNum);

            string errMsg = $"[{cboCurrentBin.EditValue?.ToString()}] 빈을 삭제 했습니다";
            if (!clsProcessDosing.InsertLog(selPlantCode, selProKey, selLcode, selWorkDate, selNum, "0", selCondition, errMsg))
            {
                Dbconn.conn.Rollback();
                clsLog.logSave("btn_ok_Click", "btn_ok_Click", SQL);
                return;
            }
        }

        private void cboCurrentBin_EditValueChanged(object sender, EventArgs e)
        {
            SQL = $@"
            SELECT a.INGRED_CODE AS RESOURCE_NO, a.SET_VAL, a.QTY_PCT
            FROM WORK_DETAIL a
            WHERE a.PLANT_CODE = '{selPlantCode}'
                AND a.PROCESS_KEY = '{selProKey}'
                AND a.L_CODE = '{selLcode}'
                AND a.WORKDATE    = '{selWorkDate}'
                AND a.NUM         = '{selNum}'
                AND a.LOCATION    = '{cboCurrentBin.EditValue}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
            {
                selResourceNo = Dbconn.conn.getData(ds, "RESOURCE_NO", 0);
                txtSelSetVal.EditValue = Dbconn.conn.getData(ds, "SET_VAL", 0);
                txtSelQctPct.EditValue = Dbconn.conn.getData(ds, "QTY_PCT", 0);
            }

            SetChageBin(selPlantCode, selProKey, selLcode, selScaleCode, selResourceNo);
        }

        private void SetChageBin(string sPlantCode, string sProKey, string sLcode, string sScaleCode, string sResourceNo)
        {
            cboChangeBin.Properties.NullText = "변경할 원료빈을 찾지 못했습니다";
            SQL = $@"
            SELECT TO_CHAR(a.LOCATION) AS CODE, TO_CHAR(a.LOCATION || ' : ' || b.DESCRIPTION) AS NAME
            FROM BIN a
                INNER JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
            WHERE a.PLANT_CODE = '{sPlantCode}' AND a.PROCESS_KEY = '{sProKey}' AND a.L_CODE = '{sLcode}'
                AND ('{sScaleCode}' IS NULL OR a.SCALE_CODE = '{sScaleCode}')
                AND ('{sResourceNo}' IS NULL OR a.RESOURCE_NO = '{sResourceNo}')
            UNION ALL
            SELECT 'H' AS CODE, 'H : {sResourceNo}' AS NAME FROM DUAL WHERE '{sScaleCode}' = 'H'
            ORDER BY CODE
            ";

            DataSet dsNew = Dbconn.conn.ExecutDataset(SQL);

            clsDevexpressUtil.ItemLookUpEditSetup(cboChangeBin, dsNew.Tables[0], "", false, 0);
        }

        private void cboCurrentScale_EditValueChanged(object sender, EventArgs e)
        {
            SetChangeScale(selPlantCode, selProKey, selLcode, selWorkDate, selNum);
        }

        private void SetChangeScale(string sPlantCode, string sProKey, string sLcode, string sWorkDate, string sNum)
        {
             SQL = $@"
            SELECT DISTINCT TO_CHAR(a.SCALE_CODE) AS CODE, TO_CHAR(a.SCALE_CODE || ' : ' || a.SCALE_NAME)  AS NAME
            FROM SCALE a
            WHERE a.PLANT_CODE = '{sPlantCode}'
                AND a.PROCESS_KEY = '{sProKey}'
                AND a.L_CODE = '{sLcode}'  
            UNION ALL
            SELECT 'H' AS CODE, 'H' AS NAME FROM DUAL
            ORDER BY CODE
            ";

            DataSet dsPreS = Dbconn.conn.ExecutDataset(SQL);

            clsDevexpressUtil.ItemLookUpEditSetup(cboChangeScale, dsPreS.Tables[0], "", false, 0);

            SQL = $@"
            SELECT a.LOCATION AS CODE, a.LOCATION || ' : ' || b.DESCRIPTION  AS NAME
            FROM WORK_DETAIL a
                INNER JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.INGRED_CODE
            WHERE a.PLANT_CODE = '{selPlantCode}'
                AND a.PROCESS_KEY = '{selProKey}'
                AND a.L_CODE = '{selLcode}'  
                AND a.WORKDATE = '{selWorkDate}'
                AND a.NUM = '{selNum}'
                AND a.SCALE_CODE = '{cboCurrentScale.EditValue}' 
            ";

            DataSet dsPre = Dbconn.conn.ExecutDataset(SQL);

            clsDevexpressUtil.ItemLookUpEditSetup(cboCurrentBin, dsPre.Tables[0], "", false, 0);
        }

        private void cboChangeScale_EditValueChanged(object sender, EventArgs e)
        {
            SetChageBin(selPlantCode, selProKey, selLcode, cboChangeScale.EditValue?.ToString(), chkAddIngred.Checked == true ? "" : selResourceNo);

            if (cboChangeScale.EditValue?.ToString() == "H")
                cboChangeBin.EditValue = "H";
        }

        private void cboChangeBin_EditValueChanged(object sender, EventArgs e)
        {
            SQL = $@"
            SELECT a.RESOURCE_NO
            FROM BIN a
            WHERE a.PLANT_CODE = '{selPlantCode}' AND a.PROCESS_KEY = '{selProKey}' AND a.L_CODE = '{selLcode}'
                AND ('{cboChangeScale.EditValue}' = 'H' OR a.SCALE_CODE = '{cboChangeScale.EditValue}')
                AND a.LOCATION = '{cboChangeBin.EditValue}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            changeResourceNo = Dbconn.conn.getData(ds, "RESOURCE_NO", 0);
        }

        private void chkAddIngred_CheckedChanged(object sender, EventArgs e)
        {
            SetChageBin(selPlantCode, selProKey, selLcode, cboChangeScale.EditValue?.ToString() == "H" ? "" : cboChangeScale.EditValue?.ToString(), chkAddIngred.Checked == true ? "" : selResourceNo);
        }
    }
}