using Core.Class;
using Core.Enum;
using Core.Extension;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.MetadataServices;
using System.Text;
using System.Threading.Tasks;

namespace HARIM_FA_DOSING.Class
{
    public class PlcFunc_Test
    {
        public static int PlcGetDevice(DataTable dtPlc, int iPlc_Location, string sPLCAddress, int[] Cimon_Job_Data = null, int sourceIndex = 0)
        {
            int iPLCDataCount = 15;

            if (string.IsNullOrWhiteSpace(sPLCAddress))
            {
                clsLog.logSave($"**********PlcGetDevice 를 호출 받았지만 [sPLCAddress] 가 없습니다.**********", 0, "PLC");
                return 0;
            }

            clsUtil.Delay(Properties.Settings.Default.Delay);

            int batch = 0;

            // GBG4
            if (clsCommon.PlantCode == "PJ01" && clsCommon.GetProcessKey("갓돈배합") == clsCommon.ProcessCode)
            {
                MAIN_Test.aPlc2.GetADeviceEx(sPLCAddress, out batch);
                return batch;
            }
            // GBG4 -

            if (iPlc_Location == 1 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "Q")
                MAIN_Test.qPlc1.GetQDeviceEx(sPLCAddress, out batch);
            else if (iPlc_Location == 1 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "A")
                MAIN_Test.aPlc1.GetADeviceEx(sPLCAddress, out batch);
            else if (iPlc_Location == 2 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "Q")
                MAIN_Test.qPlc2.GetQDeviceEx(sPLCAddress, out batch);
            else if (iPlc_Location == 2 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "A")
                MAIN_Test.aPlc2.GetADeviceEx(sPLCAddress, out batch);
            else if (dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "XGI")
            {
                int[] temp = new int[2];
                if (clsXgiHandler.Read(0, sPLCAddress, iPLCDataCount, temp) == 0)
                {
                    clsXgiHandler.Read(0, sPLCAddress, iPLCDataCount, temp);
                }
                batch = temp[0];
            }
            else if (dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "CM")
            {
                batch = Cimon_Job_Data[sourceIndex];
            }

            clsLog.logSave($"PlcGetDevice(DataTable dtPlc, int iPlc_Location = {iPlc_Location}, string sPLCAddress = {sPLCAddress}, int[] Cimon_Job_Data = null, int sourceIndex = {sourceIndex} = 0)", 0, "PLC");

            return batch;
        }

        public static int[] PlcGetReadDeviceAddBlock(DataTable dtPlc, int iPlc_Location, string sPLCAddress, DataRow dr, int Dev, int j, string device, string number, string sErrMsg, int[] Cimon_Result_Data, string sWorkDate = "", string sWorkNum = "")
        {
            int iPLCDataCount = 15;

            if (string.IsNullOrWhiteSpace(sPLCAddress))
            {
                clsLog.logSave($"**********PlcGetReadDeviceAddBlock 를 호출 받았지만 [sPLCAddress] 가 없습니다.**********", 0, "PLC");
                return null;
            }

            clsUtil.Delay(Properties.Settings.Default.Delay);

            bool returnBool = true;
            int[] valData = new int[10]; // plc->pc 메모리 

            string sAddress = string.Empty;

            // GBG4
            if (clsCommon.PlantCode == "PJ01" && clsCommon.GetProcessKey("갓돈배합") == clsCommon.ProcessCode)
            {
                MAIN_Test.aPlc2.ReadADeviceAddBlockEx(device, int.Parse(number), (5 * j), iPLCDataCount, out valData, sErrMsg);
                return valData;
            }
            // GBG4 -

            if (iPlc_Location == 1 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "Q")
            {
                sAddress = $"{device}{number}";

                for (int k = 0; k < 3; k++)
                {
                    MAIN_Test.qPlc1.ReadQDeviceAddBlockEx(device, int.Parse(number), (5 * j), iPLCDataCount, out valData, sErrMsg);
                }
            }
            else if (iPlc_Location == 1 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "A")
            {
                sAddress = $"{device}{number}";

                for (int k = 0; k < 3; k++)
                {
                    MAIN_Test.aPlc1.ReadADeviceAddBlockEx(device, int.Parse(number), (5 * j), iPLCDataCount, out valData, sErrMsg);
                }
            }
            else if (iPlc_Location == 2 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "Q")
            {
                sAddress = $"{device}{number}";

                for (int k = 0; k < 3; k++)
                {
                    MAIN_Test.qPlc2.ReadQDeviceAddBlockEx(device, int.Parse(number), (5 * j), iPLCDataCount, out valData, sErrMsg);
                }
            }
            else if (iPlc_Location == 2 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "A")
            {
                sAddress = $"{device}{number}";

                for (int k = 0; k < 3; k++)
                {
                    MAIN_Test.aPlc2.ReadADeviceAddBlockEx(device, int.Parse(number), (5 * j), iPLCDataCount, out valData, sErrMsg);
                }
            }
            else if (iPlc_Location == 1 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "XGI")
            {
                int.TryParse(sPLCAddress.Substring(3), out int addr);

                sAddress = $"{addr + (5 * j)}";

                for (int k = 0; k < 3; k++)
                {
                    if (clsXgiHandler.Read(0, addr + (5 * j), iPLCDataCount, valData) == 0)
                    {
                        clsXgiHandler.Read(0, addr + (5 * j), iPLCDataCount, valData, sErrMsg);
                    }
                }
            }
            else if (iPlc_Location == 1 && dtPlc.Rows[iPlc_Location - 1]["PLC_TYPE"]?.ToString() == "CM")
            {
                sAddress = $"{Dev}";
                Array.Copy(Cimon_Result_Data, ((Dev - 1) * 100) + (j * 5), valData, 0, iPLCDataCount);
            }

            clsLog.logSave($"실적데이터 - 작업일자[{sWorkDate}] 순번 [{sWorkNum}] Scale [{dr["SCALE_CODE"]}] 스케일 [{sAddress}] 데이터 [{valData[0]}, {valData[1]}, {valData[2]}, {valData[3]}, {valData[4]}] ", 0, "PLC");

            clsLog.logSave($"PlcGetReadDeviceAddBlock(DataTable dtPlc, int iPlc_Location = {iPlc_Location}, string sPLCAddress = {sPLCAddress}, DataRow dr, int Dev = {Dev}, int j = {j}, string device = {device}, string number = {number}, string sErrMsg = {sErrMsg}, int[] Cimon_Result_Data, string sWorkDate = {sWorkDate}, string sWorkNum = {sWorkNum})", 0, "PLC");

            return valData;
        }

        /// <summary>
        /// 배열 읽기
        /// </summary>
        /// <param name="plcType"></param>
        /// <param name="iPlcType"></param>
        /// <param name="pWorkNum"></param>
        /// <returns></returns>
        public static bool PlcGetReadQDeviceBlockEx(DataTable dtPlc, int iPlc_Location, string sPLCAddress, ref int[] pWorkNum, string sErrMsg, int[] Cimon_Job_Data = null, int sourceIndex = 0)
        {
            int iPLCDataCount = 15;

            if (string.IsNullOrWhiteSpace(sPLCAddress))
            {
                clsLog.logSave($"**********PlcGetReadQDeviceBlockEx 를 호출 받았지만 [sPLCAddress] 가 없습니다.**********", 0, "PLC");
                return true;
            }

            clsUtil.Delay(Properties.Settings.Default.Delay);

            bool returnBool = true;
            DataRow[] drPlc = dtPlc.Select($"PLC_NO = {iPlc_Location}");
            string sPlcType = drPlc[0]["PLC_TYPE"]?.ToString();

            if (iPlc_Location == 1 && sPlcType == "Q")
            {
                if (MAIN_Test.qPlc1.ReadQDeviceBlockEx(sPLCAddress, iPLCDataCount, out pWorkNum, sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    return false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            // GBG
            else if (iPlc_Location == 1 && sPlcType == "A")
            {
                if (MAIN_Test.aPlc1.ReadADeviceBlockEx(sPLCAddress, iPLCDataCount, out pWorkNum, sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    returnBool = false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            else if (iPlc_Location == 2 && sPlcType == "Q")
            {
                if (MAIN_Test.qPlc2.ReadQDeviceBlockEx(sPLCAddress, iPLCDataCount, out pWorkNum, sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    returnBool = false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            // GBG
            else if (iPlc_Location == 2 && sPlcType == "A")
            {
                if (MAIN_Test.aPlc2.ReadADeviceBlockEx(sPLCAddress, iPLCDataCount, out pWorkNum, sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    returnBool = false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            else if (sPlcType == "XGI")
            {
                if (clsXgiHandler.Read(0, sPLCAddress, iPLCDataCount, pWorkNum) == 0)
                {
                    if (clsXgiHandler.Read(0, sPLCAddress, iPLCDataCount, pWorkNum, sErrMsg) == 0)
                    {
                        MAIN_Test.MainPlcConnChk = "N";
                        returnBool = false;
                    }
                    else
                    {
                        MAIN_Test.MainPlcConnChk = "Y";
                    }
                }
            }
            else if (sPlcType == "CM")
            {
                if (Cimon_Job_Data != null)
                {
                    if (iPLCDataCount == 1)
                    {
                        pWorkNum[0] = Cimon_Job_Data[sourceIndex];
                    }
                    // GBG -
                    else
                    {
                        if (Cimon_Job_Data != null)
                        {
                            Array.Copy(Cimon_Job_Data, sourceIndex, pWorkNum, 0, iPLCDataCount);
                        }
                        // GBG -
                    }
                }
                else
                {
                    if (clsCimonHandler2.Read(0, sPLCAddress, iPLCDataCount, pWorkNum) == 0)
                    {
                        clsUtil.Delay(500);
                        if (clsCimonHandler2.Read(0, sPLCAddress, iPLCDataCount, pWorkNum, sErrMsg) == 0)
                        {
                            MAIN_Test.MainPlcConnChk = "N";
                            returnBool = false;
                        }
                        else
                        {
                            MAIN_Test.MainPlcConnChk = "Y";
                        }
                    }
                }
            }

            clsLog.logSave($" - {sPlcType} PLC - MAP - {sErrMsg} [{pWorkNum[0]}, {pWorkNum[1]}, {pWorkNum[2]}]", 0, "PLC");

            clsLog.logSave($"PlcGetReadQDeviceBlockEx(DataTable dtPlc, int iPlc_Location = {iPlc_Location}, string sPLCAddress = {sPLCAddress}, ref int[] pWorkNum, string sErrMsg = {sErrMsg}, int[] Cimon_Job_Data = null, int sourceIndex = {sourceIndex})", 0, "PLC");

            if (pWorkNum != null) SetArrayLog(pWorkNum, "pWorkNum", "PlcGetReadQDeviceBlockEx");
            if (Cimon_Job_Data != null) SetArrayLog(Cimon_Job_Data, "Cimon_Job_Data", "PlcGetReadQDeviceBlockEx");

            return returnBool;
        }

        public static bool PLCSetWriteQDeviceBlock2Ex(DataTable dtPlc, int iPlc_Location, string sPLCAddress, ref short[] Sdata, int[] Sdata2, string sErrMsg = "")
        {
            int iPLCDataCount = 15;

            if (string.IsNullOrWhiteSpace(sPLCAddress))
            {
                clsLog.logSave($"**********PLCSetWriteQDeviceBlock2Ex 를 호출 받았지만 [sPLCAddress] 가 없습니다.**********", 0, "PLC");
                return true;
            }

            clsUtil.Delay(Properties.Settings.Default.Delay);

            bool returnBool = true;
            DataRow[] drPlc = dtPlc.Select($"PLC_NO = {iPlc_Location}");
            string sPlcType = drPlc[0]["PLC_TYPE"]?.ToString();

            if (iPlc_Location == 1 && sPlcType == "Q")
            {
                if (MAIN_Test.qPlc1.WriteQDeviceBlock2Ex(sPLCAddress, iPLCDataCount, ref Sdata, sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    return false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            else if (iPlc_Location == 1 && sPlcType == "A")
            {
                if (MAIN_Test.aPlc1.WriteADeviceBlock2Ex(sPLCAddress, iPLCDataCount, ref Sdata, sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    return false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            if (iPlc_Location == 2 && sPlcType == "Q")
            {
                if (MAIN_Test.qPlc2.WriteQDeviceBlock2Ex(sPLCAddress, iPLCDataCount, ref Sdata, sErrMsg) != 0)
                {
                    MAIN_Test.SubPlcConnChk = "N";
                    return false;
                }
                else
                {
                    MAIN_Test.SubPlcConnChk = "Y";
                }
            }
            else if (iPlc_Location == 2 && sPlcType == "A")
            {
                if (MAIN_Test.aPlc2.WriteADeviceBlock2Ex(sPLCAddress, iPLCDataCount, ref Sdata, sErrMsg) != 0)
                {
                    MAIN_Test.SubPlcConnChk = "N";
                    return false;
                }
                else
                {
                    MAIN_Test.SubPlcConnChk = "Y";
                }
            }
            else if (sPlcType == "XGI")
            {
                if (clsXgiHandler.Write(2, sPLCAddress, iPLCDataCount, Sdata2) == 0)
                {
                    if (clsXgiHandler.Write(2, sPLCAddress, iPLCDataCount, Sdata2, sErrMsg) == 0)
                    {
                        return false;
                    }
                }
            }
            else if (sPlcType == "CM")
            {
                int[] temp = new int[3] { Sdata2[0], Sdata2[1], Sdata2[2] };
                //_ = clsCimonHandler.TryWriteWord(vPLCAddress, temp);
                if (clsCimonHandler2.Write(2, sPLCAddress, iPLCDataCount, temp) == 0)
                {
                    clsUtil.Delay(1000);
                    if (clsCimonHandler2.Write(2, sPLCAddress, iPLCDataCount, temp, sErrMsg) == 0)
                    {
                        return false;
                    }
                }
            }

            clsLog.logSave($"빈 변경 데이터 전송 - Addr [{sPLCAddress}] Data [{Sdata2[0]}, {Sdata2[1]}, {Sdata2[2]}, {Sdata2[3]}, {Sdata2[4]}]", 0, "PLC");

            clsLog.logSave($"PLCSetWriteQDeviceBlock2Ex(DataTable dtPlc, int iPlc_Location = {iPlc_Location}, string sPLCAddress = {sPLCAddress}, ref short[] Sdata, int[] Sdata2, string sErrMsg = {sErrMsg})", 0, "PLC");

            if (Sdata != null) SetArrayLog(Sdata, "Sdata", "PLCSetWriteQDeviceBlock2Ex");

            if (Sdata2 != null) SetArrayLog(Sdata2, "Sdata2", "PLCSetWriteQDeviceBlock2Ex");

            return true;
        }

        public static bool PlcSetQDeviceEx(DataTable dtPlc, int iPlc_Location, string sPLCAddress, DataRow dr, int batch, int[] pWorkNum, string sErrMsg)
        {
            int iPLCDataCount = 15;

            if (string.IsNullOrWhiteSpace(sPLCAddress))
            {
                clsLog.logSave($"**********PlcSetQDeviceEx 를 호출 받았지만 [sPLCAddress] 가 없습니다.**********", 0, "PLC");
                return true;
            }

            clsUtil.Delay(Properties.Settings.Default.Delay);

            bool returnBool = true;
            DataRow[] drPlc = dtPlc.Select($"PLC_NO = {iPlc_Location}");
            string sPlcType = drPlc[0]["PLC_TYPE"]?.ToString();

            if (sPlcType == "Q" && iPlc_Location == 1)
            {
                if (MAIN_Test.qPlc1.SetQDeviceEx(sPLCAddress, pWorkNum[0], sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    returnBool = false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            else if (sPlcType == "A" && iPlc_Location == 1)
            {
                if (MAIN_Test.aPlc1.SetADeviceEx(sPLCAddress, pWorkNum[0], sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    returnBool = false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            else if (sPlcType == "Q" && iPlc_Location == 2)
            {
                if (MAIN_Test.qPlc2.SetQDeviceEx(sPLCAddress, pWorkNum[0], sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    returnBool = false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            else if (sPlcType == "A" && iPlc_Location == 2)
            {
                if (MAIN_Test.aPlc2.SetADeviceEx(sPLCAddress, pWorkNum[0], sErrMsg) != 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    returnBool = false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            else if (sPlcType == "XGI")
            {
                if (clsXgiHandler.Write(2, sPLCAddress, iPLCDataCount, pWorkNum, sErrMsg) == 0)
                {
                    MAIN_Test.MainPlcConnChk = "N";
                    returnBool = false;
                }
                else
                {
                    MAIN_Test.MainPlcConnChk = "Y";
                }
            }
            else if (sPlcType == "CM")
            {
                //_ = clsCimonHandler.TryWriteWord(sPLCAddress, temp);
                clsUtil.Delay(500);
                if (clsCimonHandler2.Write(2, sPLCAddress, iPLCDataCount, pWorkNum) == 0)
                {
                    clsUtil.Delay(500);
                    if (clsCimonHandler2.Write(2, sPLCAddress, iPLCDataCount, pWorkNum) == 0)
                    {
                        clsUtil.Delay(500);
                        if (clsCimonHandler2.Write(2, sPLCAddress, iPLCDataCount, pWorkNum) == 0) { }
                    }
                }
            }

            clsLog.logSave($"PlcSetQDeviceEx(DataTable dtPlc, int iPlc_Location = {iPlc_Location}, string sPLCAddress = {sPLCAddress}, DataRow dr, int batch =  {batch} , int[] pWorkNum, string sErrMsg = ", 0, "PLC");

            if (pWorkNum != null) SetArrayLog(pWorkNum, "pWorkNum", "PlcSetQDeviceEx");

            return returnBool;
        }

        public static bool PlcSetQDeviceEx2(DataTable dtPlc, int iPlc_Location, string sPLCAddress, DataRow dr, int batch, int[] pWorkNum, string sErrMsg)
        {
            int iPLCDataCount = 15;

            if (string.IsNullOrWhiteSpace(sPLCAddress))
            {
                clsLog.logSave($"**********PlcSetQDeviceEx2 를 호출 받았지만 [sPLCAddress] 가 없습니다.**********", 0, "PLC");
                return true;
            }

            clsUtil.Delay(Properties.Settings.Default.Delay);

            DataRow[] drPlc = dtPlc.Select($"PLC_NO = {iPlc_Location}");
            string sPlcType = drPlc[0]["PLC_TYPE"]?.ToString();

            // GBG
            if (sPlcType == "Q")
            {
                if (MAIN_Test.qPlc2.SetQDeviceEx(sPLCAddress, pWorkNum[0], sErrMsg) != 0)
                    return false;
            }
            else if (sPlcType == "A")
            {
                if (MAIN_Test.aPlc2.SetADeviceEx(sPLCAddress, pWorkNum[0], sErrMsg) != 0)
                    return false;
            }

            clsLog.logSave($"PlcSetQDeviceEx2(DataTable dtPlc, int iPlc_Location = {iPlc_Location}, string sPLCAddress = {sPLCAddress}, DataRow dr, int batch = {batch}, int[] pWorkNum, string sErrMsg = {sErrMsg})", 0, "PLC");

            if (pWorkNum != null) SetArrayLog(pWorkNum, "pWorkNum", "PlcSetQDeviceEx2");

            return true;
        }

        /// <summary>
        /// 영역, 어드레스 분리 쓰기
        /// </summary>
        /// <param name="plcType"></param>
        /// <param name="Dev"></param>
        /// <param name="j"></param>
        /// <param name="pWorkNum"></param>
        /// <param name="Recipe_Data"></param>
        /// <param name="Recipe_D1_Index"></param>
        /// <param name="Recipe_D2_Index"></param>
        /// <param name="Recipe_D3_Index"></param>
        /// <param name="Recipe_L1_Index"></param>
        /// <param name="Recipe_L2_Index"></param>
        /// <param name="device"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool PlcSetWriteQDeviceAddBlockEx(DataTable dtPlc, int iPlc_Location, string sPLCAddress, int Dev, int j, ref int[] pWorkNum, int[] Recipe_Data, ref int Recipe_D1_Index, ref int Recipe_D2_Index, ref int Recipe_D3_Index, ref int Recipe_L1_Index, ref int Recipe_L2_Index, string device, string number, string sErrMsg)
        {
            int iPLCDataCount = 15;
            if (string.IsNullOrWhiteSpace(sPLCAddress))
            {
                clsLog.logSave($"**********PlcSetWriteQDeviceAddBlockEx 를 호출 받았지만 [sPLCAddress] 가 없습니다.**********", 0, "PLC");
                return true;
            }

            clsUtil.Delay(Properties.Settings.Default.Delay);

            DataRow[] drPlc = dtPlc.Select($"PLC_NO = {iPlc_Location}");
            string sPlcType = drPlc[0]["PLC_TYPE"]?.ToString();

            if (sPlcType == "Q" && iPlc_Location == 1)
            {
                if (MAIN_Test.qPlc1.WriteQDeviceAddBlockEx(device, int.Parse(number), (j * 5), iPLCDataCount, ref pWorkNum, sErrMsg) != 0)
                    return false;
            }
            // GBG
            else if (sPlcType == "A" && iPlc_Location == 1)
            {
                if (MAIN_Test.aPlc1.WriteADeviceAddBlockEx(device, int.Parse(number), (j * 5), iPLCDataCount, ref pWorkNum, sErrMsg) != 0)
                    return false;
            }
            else if (sPlcType == "Q" && iPlc_Location == 2)
            {
                if (MAIN_Test.qPlc2.WriteQDeviceAddBlockEx(device, int.Parse(number), (j * 5), iPLCDataCount, ref pWorkNum, sErrMsg) != 0)
                    return false;
            }
            // GBG
            else if (sPlcType == "A" && iPlc_Location == 2)
            {
                if (MAIN_Test.aPlc2.WriteADeviceAddBlockEx(device, int.Parse(number), (j * 5), iPLCDataCount, ref pWorkNum, sErrMsg) != 0)
                    return false;
            }
            else if (sPlcType == "XGI")
            {
                int.TryParse(sPLCAddress.Substring(3), out int addr);

                if (clsXgiHandler.Write(2, addr + (j * 5), iPLCDataCount, pWorkNum) == 0)
                {
                    if (clsXgiHandler.Write(2, addr + (j * 5), iPLCDataCount, pWorkNum, sErrMsg) == 0)
                    {
                        return false;
                    }
                }
            }
            else if (sPlcType == "CM")
            {
                int[] temp = new int[5] { pWorkNum[0], pWorkNum[1], pWorkNum[2], pWorkNum[3], pWorkNum[4] };
                int offset = int.Parse(number) + (j * 5);
                string addr = device + offset.ToString();

                //_ = clsCimonHandler.TryWriteWord(addr, temp);
                //clsUtil.Delay(3000);

                switch (Dev)
                {
                    case 1:
                        Array.Copy(pWorkNum, 0, Recipe_Data, Recipe_D1_Index++ * 5, 5);
                        break;
                    case 2:
                        Array.Copy(pWorkNum, 0, Recipe_Data, 100 + Recipe_D2_Index++ * 5, 5);
                        break;
                    case 3:
                        Array.Copy(pWorkNum, 0, Recipe_Data, 200 + Recipe_D3_Index++ * 5, 5);
                        break;
                    case 4:
                        Array.Copy(pWorkNum, 0, Recipe_Data, 300 + Recipe_L1_Index++ * 5, 5);
                        break;
                    case 5:
                        Array.Copy(pWorkNum, 0, Recipe_Data, 400 + Recipe_L2_Index++ * 5, 5);
                        break;
                }
            }

            clsLog.logSave($"PlcSetWriteQDeviceAddBlockEx(DataTable dtPlc, int iPlc_Location = {iPlc_Location}, string sPLCAddress = {sPLCAddress}, int Dev = {Dev}, int j = {j}, ref int[] pWorkNum, int[] Recipe_Data, ref int Recipe_D1_Index = {Recipe_D1_Index}, ref int Recipe_D2_Index = {Recipe_D2_Index}, ref int Recipe_D3_Index = {Recipe_D3_Index}, ref int Recipe_L1_Index = {Recipe_L1_Index}, ref int Recipe_L2_Index = {Recipe_L2_Index}, string device = {device}, string number = {number}, string sErrMsg = {sErrMsg})", 0, "PLC");

            if (pWorkNum != null) SetArrayLog(pWorkNum, "pWorkNum", "PlcSetWriteQDeviceAddBlockEx");
            if (Recipe_Data != null) SetArrayLog(Recipe_Data, "Recipe_Data", "PlcSetWriteQDeviceAddBlockEx");

            return true;
        }

        /// <summary>
        /// 배열 쓰기
        /// </summary>
        /// <param name="plcType"></param>
        /// <param name="pWorkNum"></param>
        /// <returns></returns>
        public static bool SetWriteQDeviceBlockEx(DataTable dtPlc, int iPlc_Location, string sPLCAddress, ref int[] pWorkNum, string sErrMsg)
        {
            int iPLCDataCount = 15;

            if (string.IsNullOrWhiteSpace(sPLCAddress))
            {
                clsLog.logSave($"**********SetWriteQDeviceBlockEx 를 호출 받았지만 [sPLCAddress] 가 없습니다.**********", 0, "PLC");
                return true;
            }

            clsUtil.Delay(Properties.Settings.Default.Delay);

            DataRow[] drPlc = dtPlc.Select($"PLC_NO = {iPlc_Location}");
            string sPlcType = drPlc[0]["PLC_TYPE"]?.ToString();

            if (sPlcType == "Q" && iPlc_Location == 1)
            {

                if (MAIN_Test.qPlc1.WriteQDeviceBlockEx(sPLCAddress, iPLCDataCount, ref pWorkNum, sErrMsg) != 0)
                    return false;
            }
            // GBG
            else if (sPlcType == "A" && iPlc_Location == 1)
            {
                if (MAIN_Test.aPlc1.WriteADeviceBlockEx(sPLCAddress, iPLCDataCount, pWorkNum, sErrMsg) != 0)
                    return false;
            }
            else if (sPlcType == "Q" && iPlc_Location == 2)
            {

                if (MAIN_Test.qPlc2.WriteQDeviceBlockEx(sPLCAddress, iPLCDataCount, ref pWorkNum, sErrMsg) != 0)
                    return false;
            }
            // GBG
            else if (sPlcType == "A" && iPlc_Location == 2)
            {
                if (MAIN_Test.aPlc2.WriteADeviceBlockEx(sPLCAddress, iPLCDataCount, pWorkNum, sErrMsg) != 0)
                    return false;
            }
            else if (sPlcType == "XGI")
            {
                if (clsXgiHandler.Write(2, sPLCAddress, iPLCDataCount, pWorkNum) == 0)
                {
                    if (clsXgiHandler.Write(2, sPLCAddress, iPLCDataCount, pWorkNum, sErrMsg) == 0)
                    {
                        return false;
                    }
                }
            }
            else if (sPlcType == "CM")
            {
                int[] temp = new int[10];
                for (int i = 0; i < iPLCDataCount; i++)
                {
                    temp[i] = pWorkNum[i];
                }

                //_ = clsCimonHandler.TryWriteWord(sPLCAddress, temp);
                clsUtil.Delay(500);

                if (clsCimonHandler2.Write(2, sPLCAddress, iPLCDataCount, temp) == 0)
                {
                    clsUtil.Delay(500);
                    if (clsCimonHandler2.Write(2, sPLCAddress, iPLCDataCount, temp, sErrMsg) == 0)
                    {
                        return false;
                    }
                }
            }

            clsLog.logSave($"SetWriteQDeviceBlockEx(DataTable dtPlc, int iPlc_Location = {iPlc_Location}, string sPLCAddress = {sPLCAddress}, ref int[] pWorkNum, string sErrMsg = {sErrMsg})", 0, "PLC");

            if (pWorkNum != null) SetArrayLog(pWorkNum, "pWorkNum", "SetWriteQDeviceBlockEx");

            return true;
        }

        /// <summary>
        /// PLC 연결 상태 체크
        /// </summary>
        /// <param name="plcIP"></param>
        /// <param name="plcType"></param>
        /// <returns></returns>
        public static bool GetPlcCon(DataTable dt, out string sMsg)
        {
            switch (dt.Rows.Count)
            {
                case 1:
                    if ((dt.Rows[0]["PLC_TYPE"]?.ToString() == "Q" && MAIN_Test.MainPlcConnChk != "Y" && clsCommon._strMainPlcConnYn == "Y"))
                    {
                        MAIN_Test.qPlc1.Close();
                        //ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        sMsg = "PLC 1번 접속을 실패하였습니다";
                        return false;
                    }

                    if (dt.Rows[0]["PLC_TYPE"]?.ToString() == "A" && MAIN_Test.SubPlcConnChk != "Y")
                    {
                        MAIN_Test.aPlc1.Close();
                        //ShowMessageBox.XtraShowInformation("PLC 1번, 2번 을 먼저 연결 해주세요.");
                        sMsg = "PLC 2번 을 먼저 연결 해주세요.";
                        return false;
                    }

                    // GBG
                    if (dt.Rows[0]["PLC_TYPE"]?.ToString() == "XGI" && MAIN_Test.MainPlcConnChk != "Y")
                    {
                        //ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        sMsg = "PLC 1번을 연결 해주세요.";
                        return false;
                    }

                    if (dt.Rows[0]["PLC_TYPE"]?.ToString() == "CM" && MAIN_Test.MainPlcConnChk != "Y")
                    {
                        //ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                        sMsg = "PLC 1번을 연결 해주세요.";
                        return false;
                    }
                    // GBG -

                    break;
                case 2:
                    if (MAIN_Test.SubPlcConnChk != "M" && MAIN_Test.SubPlcConnChk != "")
                    {
                        if ((dt.Rows[1]["PLC_TYPE"]?.ToString() == "Q" && MAIN_Test.SubPlcConnChk != "Y" && clsCommon._strSubPlcConnYn == "Y"))
                        {
                            MAIN_Test.qPlc2.Close();
                            //ShowMessageBox.XtraShowInformation("PLC 1번을 연결 해주세요.");
                            sMsg = "PLC 1번 접속을 실패하였습니다";
                            return false;
                        }

                        if (dt.Rows[1]["PLC_TYPE"]?.ToString() == "A" && MAIN_Test.SubPlcConnChk != "Y")
                        {
                            MAIN_Test.aPlc2.Close();
                            //ShowMessageBox.XtraShowInformation("PLC 1번, 2번 을 먼저 연결 해주세요.");
                            sMsg = "PLC 2번 을 먼저 연결 해주세요.";
                            return false;
                        }

                        if ((MAIN_Test.MainPlcConnChk != "Y" && clsCommon._strMainPlcConnYn == "Y") || (MAIN_Test.SubPlcConnChk != "Y" && clsCommon._strSubPlcConnYn == "Y"))
                        {
                            //ShowMessageBox.XtraShowInformation("PLC 1번, 2번 을 먼저 연결 해주세요.");
                            sMsg = "PLC 1번, 2번 을 먼저 연결 해주세요.";
                            return false;
                        }
                    }
                    break;
                default:
                    break;
            }

            sMsg = "";

            return true;
        }

        private static void SetArrayLog(int[] arry, string arrName, string funName)
        {
            for (int i = 0; i < arry.Length; i++)
            {
                if (arry[i] > 0)
                    clsLog.logSave($"{funName} : {arrName} = {arry[i]}", 0, "PLC");
            }
        }

        private static void SetArrayLog(short[] arry, string arrName, string funName)
        {
            for (int i = 0; i < arry.Length; i++)
            {
                if (arry[i] > 0)
                    clsLog.logSave($"{funName} : {arrName} = {arry[i]}", 0, "PLC");
            }
        }
    }
}
