using Core.Class;
using Core.Enum;
using DevExpress.XtraRichEdit.Commands;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Formats.Asn1.AsnWriter;

namespace HARIM_FA_DOSING
{
    public class clsProcessDosing
    {
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

        public static void BinStock(string binCd, decimal stock)
        {
            try
            {
                string SQL = $@"
                UPDATE BIN  SET STOCK  =  NVL(STOCK,0) + ( {stock}  * 1)
                WHERE LOCATION = '{binCd}'
                ";

                Dbconn.conn.SQLrun(SQL);
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "BinStock", ex);
            }
        }


        public static bool BatchChagne(string process_key, string work_num, string wWork_Date, string ch_batch)
        {
            try
            {
                string SQL =
                "SELECT NVL(R_BATCH, 0) AS R_BATCH FROM WORK_ORDER WHERE  PROCESS_KEY = '{0}' AND WORKDATE = '{1}'";
                SQL = string.Format(SQL, process_key, wWork_Date);

                DataSet Ds = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(Ds) == 1)
                {
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "BatchChagne", ex);
                return false;
            }
        }

        public static bool BinSeqDupChk(string Plant_Code, string process_key, string lCode, string res_no, string note, out string dupBin)
        {
            dupBin = string.Empty;

            try
            {
                string
                SQL = $@"
                SELECT 
                       A.RESOURCE_NO,
                       A.LOCATION,
                       A.SCALE_CODE,
                       A.SEQ
                FROM BIN A
                JOIN SCALE B
                      ON A.PLANT_CODE = B.PLANT_CODE
                     AND A.PROCESS_KEY = B.PROCESS_KEY
                     AND A.L_CODE = B.L_CODE
                     AND A.SCALE_CODE = B.SCALE_CODE
                WHERE A.PLANT_CODE = '{Plant_Code}'
                  AND A.PROCESS_KEY = '{process_key}'
                  AND A.L_CODE = '{lCode}'
                  AND A.SEQ = '1'
                  AND A.RESOURCE_NO IN (
                        SELECT RESOURCE_NO
                        FROM BIN
                        WHERE PLANT_CODE  = '{Plant_Code}'
                          AND PROCESS_KEY = '{process_key}'
                          AND L_CODE      = '{lCode}'
                          AND SEQ = '1'
                        GROUP BY RESOURCE_NO
                        HAVING COUNT(*) > 1      -- 중복된 RESOURCE_NO 만
                  )
                ORDER BY A.RESOURCE_NO, A.LOCATION
                ";


                using (DataSet binSeqDs = Dbconn.conn.ExecutDataset(SQL))
                {
                    if (Dbconn.conn.getRowCnt(binSeqDs) > 0)
                    {
                        dupBin = Dbconn.conn.getData(binSeqDs, "LOCATION", 0);
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "BinSeqDupChk", ex);
                return false;
            }
        }

        public static string InsertWorkNum(string wWork_Date, DataRow insertRow)
        {
            try
            {
                if (insertRow == null)
                {
                    return "INSERT ROW NULL";
                }

                string wPlant = insertRow["PLANT_CODE"].ToString();
                string wProcess_Key = insertRow["PROCESS_KEY"].ToString();
                string wL_Code = insertRow["L_CODE"].ToString();
                string wNum = insertRow["NUM"].ToString();
                string wRESOURCE_NO = insertRow["RESOURCE_NO"].ToString();
                string w_note = insertRow["NOTE"].ToString();
                string w_batch = insertRow["BATCH"].ToString();
                string w_batch_q = insertRow["BATCH_Q"].ToString();
                string w_location_ed = insertRow["LOCATION_ED"].ToString();
                string w_location_ed2 = insertRow["LOCATION_ED2"].ToString();
                string wP_TYPE = "2";//insertRow["P_TYPE"].ToString();
                string wICM_CODE = insertRow["ICM_CODE"].ToString();
                string wBU_YN = insertRow["BU_YN"].ToString();
                string wTRANS_YN = insertRow["TRANS_YN"].ToString();
                string wEmployee_No = insertRow["EMPLOYEE_NO"].ToString();
                string wREMARK = insertRow["REMARK"].ToString();
                string wBAD_CODE1 = insertRow["BAD_CODE1"].ToString();
                string wBAD_QTY1 = insertRow["BAD_QTY1"].ToString();
                string wBAD_CODE2 = insertRow["BAD_CODE2"].ToString();
                string wBAD_QTY2 = insertRow["BAD_QTY2"].ToString();
                string wBAD_CODE3 = insertRow["BAD_CODE3"].ToString();
                string wBAD_QTY3 = insertRow["BAD_QTY3"].ToString();
                string wBAD_CODE4 = insertRow["BAD_CODE4"].ToString();
                string wBAD_QTY4 = insertRow["BAD_QTY4"].ToString();
                string wBAD_CODE5 = insertRow["BAD_CODE5"].ToString();
                string wBAD_QTY5 = insertRow["BAD_QTY5"].ToString();
                string wPACK_TYPE = insertRow["PACK_TYPE"].ToString();

                string sPRESSURE = insertRow["PRESSURE"].ToString();
                string sCOOKING1 = insertRow["COOKING1"].ToString();
                string sCOOKING2 = insertRow["COOKING2"].ToString();
                string sTEMP_UPPER = insertRow["TEMP_UPPER"].ToString();
                string sTEMP_LOWER = insertRow["TEMP_LOWER"].ToString();
                string sSTEAM_INPUT = insertRow["STEAM_INPUT"].ToString();
                string sBEFORE_INPUT = insertRow["BEFORE_INPUT"].ToString();
                string sAFTER_INPUT = insertRow["AFTER_INPUT"].ToString();
                string sFEEDER = insertRow["FEEDER"].ToString();
                string sROLL_GAP_LEFT = insertRow["ROLL_GAP_LEFT"].ToString();
                string sROLL_RPM_LEFT = insertRow["ROLL_RPM_LEFT"].ToString();
                string sROLL_GAP_RIGHT = insertRow["ROLL_GAP_RIGHT"].ToString();
                string sROLL_RPM_RIGHT = insertRow["ROLL_RPM_RIGHT"].ToString();

                string wErpOstatus = insertRow["ERP_OSTATUS"].ToString();
                string wErpIstatus = insertRow["ERP_ISTATUS"].ToString();

                string w_or_q = insertRow["OR_Q"].ToString(); //(Convert.ToInt32(w_batch) * Convert.ToDecimal(String.Format("{0:N0}", w_batch_q))).ToString();
                string w_pro_q = insertRow["PRO_Q"].ToString();
                
                string SQL = string.Empty;
                string SQL2 = string.Empty;
                string SQL3 = string.Empty;
                string SQL31 = string.Empty;

                Dbconn.conn.BeginTransaction();

                //bin match
                bool recipi_update_yn = false;
                if (insertRow.RowState == DataRowState.Modified)
                {
                    SQL = $@"
                    SELECT RESOURCE_NO, BATCH_Q, NOTE
                    FROM WORK_ORDER 
                    WHERE PLANT_CODE = '{wPlant}' AND PROCESS_KEY = '{wProcess_Key}' AND L_CODE = '{wL_Code}' AND WORKDATE = '{wWork_Date}' AND NUM = '{wNum}'";

                    using (DataSet work_ds = Dbconn.conn.ExecutDataset(SQL))
                    {
                        if (Dbconn.conn.getRowCnt(work_ds) == 1)
                        {
                            string befor_res_no = Dbconn.conn.getData(work_ds, "RESOURCE_NO", 0);
                            string befor_batch_q = Dbconn.conn.getData(work_ds, "BATCH_Q", 0);
                            string befor_note = Dbconn.conn.getData(work_ds, "NOTE", 0);

                            if (!befor_res_no.Equals(insertRow["RESOURCE_NO"].ToString()) ||
                                !befor_batch_q.Equals(insertRow["BATCH_Q"].ToString()) ||
                                !befor_note.Equals(insertRow["NOTE"].ToString())
                                )
                            {
                                recipi_update_yn = true;
                            }
                        }
                        else
                        {
                            clsLog.logSave("clsProcessDosing", "InsertWorkNum", SQL);
                            return "작업지시 정보가 불일치합니다";
                        }
                    }
                }

                SQL = $@"
                SELECT a.RESOURCE_NO, b.DESCRIPTION
                FROM BIN a
                    LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{wPlant}' AND a.LOCATION = '{w_location_ed}'
                ";

                using (DataSet ds = Dbconn.conn.ExecutDataset(SQL))
                {
                    if (Dbconn.conn.getRowCnt(ds) > 0)
                    {
                        if (Dbconn.conn.getData(ds, "RESOURCE_NO", 0) == "")
                        {
                            if (DialogResult.Yes != ShowMessageBox.Confirm($"['{w_location_ed}'] 목적빈에 제품이 설정 되있지 않습니다. {Environment.NewLine} 제품 변경 후 진행 하시겠습니까?"))
                            {
                                return "빈 변경 후 진행 해주세요.";
                            }

                            if (wPlant != "PJ01")
                            {
                                SQL = $@"
                                UPDATE BIN
                                SET RESOURCE_NO     = '{wRESOURCE_NO}'
                                    , STOCK           = '0'
                                WHERE PLANT_CODE    = '{wPlant}'
                                    AND LOCATION    =  '{w_location_ed}'
                                ";

                                if (Dbconn.conn.SQLrun(SQL) < 1)
                                {
                                    Dbconn.conn.Rollback();
                                    return "빈 정보 변경에 실패 했습니다.";
                                }
                            }
                        }
                        else if (Dbconn.conn.getData(ds, "RESOURCE_NO", 0) != wRESOURCE_NO)
                        {
                            if (DialogResult.Yes != ShowMessageBox.Confirm($"[{w_location_ed}] 목적빈에 [{Dbconn.conn.getData(ds, "DESCRIPTION", 0)}] 제품으로 설정 되어있습니다. {Environment.NewLine} 제품 변경 후 진행 하시겠습니까?"))
                            {
                                return "빈 변경 후 진행 해주세요.";
                            }

                            if (wPlant != "PJ01")
                            {
                                SQL = $@"
                                UPDATE BIN
                                SET RESOURCE_NO     = '{wRESOURCE_NO}'
                                    , STOCK           = '0'
                                WHERE PLANT_CODE    = '{wPlant}'
                                    AND LOCATION    =  '{w_location_ed}'
                                ";

                                if (Dbconn.conn.SQLrun(SQL) < 1)
                                {
                                    Dbconn.conn.Rollback();
                                    return "빈 정보 변경에 실패 했습니다.";
                                }
                            }
                        }
                    }
                }

                String wNUM = string.Empty;
                String wWORKDATE = string.Empty;
                String erp_status = string.Empty;

                if (insertRow.RowState == DataRowState.Added)
                {
                    wNUM = workNumMake(wPlant, wProcess_Key, wL_Code, wWork_Date);
                    wWORKDATE = wWork_Date;

                    SQL = $@"
                    INSERT INTO WORK_ORDER (
                       PLANT_CODE        -- 01
                     , PROCESS_KEY       -- 02
                     , L_CODE            -- 03
                     , WORKDATE          -- 04
                     , NUM               -- 05
                     , P_TYPE            -- 06
                     , RESOURCE_NO       -- 07
                     , NOTE              -- 08
                     , WORK_START_DATE   -- 09
                     , BATCH             -- 10
                     , R_BATCH           -- 11
                     , BATCH_Q           -- 12
                     , OR_Q              -- 13
                     , PRO_Q             -- 14
                     , BBATCH_Q          -- 15
                     , GUBUN             -- 16
                     , LOCATION_ED       -- 17
                     , LOCATION_ED2      -- 18
                     , REMARK            -- 19
                     , ICM_CODE          -- 20
                     , C_CONDITION       -- 21
                     , BU_YN             -- 25
                     , BAD_CODE1         -- 26
                     , BAD_QTY1          -- 27
                     , BAD_CODE2         -- 28
                     , BAD_QTY2          -- 29
                     , BAD_CODE3         -- 30
                     , BAD_QTY3          -- 31
                     , BAD_CODE4         -- 32
                     , BAD_QTY4          -- 33
                     , BAD_CODE5         -- 34
                     , BAD_QTY5          -- 35
                     , ERP_ISTATUS       -- 36
                     , ERP_OSTATUS       -- 38
                     , DEL_FLAG          -- 40
                     , I_TIME            -- 41
                     , EMPLOYEE_NO       -- 42
                     , PACK_TYPE         -- 43
                     , TRANS_YN          -- 44
                    )
                    VALUES (
                       '{wPlant}'                                   -- 01
                     , '{wProcess_Key}'                             -- 02
                     , '{wL_Code}'                                  -- 03
                     , '{wWORKDATE}'                                -- 04
                     , '{wNUM}'                                     -- 05
                     , '{wP_TYPE}'                                  -- 06
                     , '{wRESOURCE_NO}'                             -- 07
                     , '{w_note}'                                   -- 08
                     , '{wWORKDATE}'                                -- 09
                     , '{w_batch}'                                  -- 10
                     , '0'                                          -- 11
                     , '{w_batch_q}'                                -- 12
                     , '{w_or_q}'                                   -- 13
                     , '0'                                          -- 14
                     , '0'                                          -- 15
                     , 'Y'                                          -- 16
                     , '{w_location_ed}'                            -- 17
                     , '{w_location_ed2}'                           -- 18
                     , '{wREMARK}'                                  -- 19
                     , '{wICM_CODE}'                                -- 20
                     , '{clsCommon.PcStatus.Plan}'        -- 21
                     , '{wBU_YN}'                                   -- 25
                     , '{wBAD_CODE1}'                               -- 26
                     , '{wBAD_QTY1}'                                -- 27
                     , '{wBAD_CODE2}'                               -- 28
                     , '{wBAD_QTY2}'                                -- 29
                     , '{wBAD_CODE3}'                               -- 30
                     , '{wBAD_QTY3}'                                -- 31
                     , '{wBAD_CODE4}'                               -- 32
                     , '{wBAD_QTY4}'                                -- 33
                     , '{wBAD_CODE5}'                               -- 34
                     , '{wBAD_QTY5}'                                -- 35
                     , 'N'                                          -- 36
                     , 'N'                                          -- 38
                     , 'N'                                          -- 40
                     , SYSDATE                                      -- 41
                     , '{clsCommon.UserId}'                         -- 42
                     , '{wPACK_TYPE}'                               -- 43
                     , '{wTRANS_YN}'                                -- 44
                    )
                    ";

                    SQL2 = $@"
                    /* 후레이크 레포트 저장 */
                    INSERT INTO FLAKE_REPORT
                    (  PLANT_CODE
                        , PROCESS_KEY
                        , L_CODE
                        , WORKDATE
                        , NUM
                        , PRESSURE
                        , COOKING1
                        , COOKING2
                        , TEMP_UPPER
                        , TEMP_LOWER
                        , STEAM_INPUT
                        , BEFORE_INPUT
                        , AFTER_INPUT
                        , FEEDER
                        , ROLL_GAP_LEFT
                        , ROLL_RPM_LEFT
                        , ROLL_GAP_RIGHT
                        , ROLL_RPM_RIGHT
                        , I_TIME
                        , I_USER
                    )
                    VALUES
                    (  '{wPlant}'
                        , '{wProcess_Key}'
                        , '{wL_Code}'
                        , '{wWORKDATE}'
                        , '{wNUM}'
                        , '{sPRESSURE}'
                        , '{sCOOKING1}'
                        , '{sCOOKING2}'
                        , '{sTEMP_UPPER}'
                        , '{sTEMP_LOWER}'
                        , '{sSTEAM_INPUT}'
                        , '{sBEFORE_INPUT}'
                        , '{sAFTER_INPUT}'
                        , '{sFEEDER}'
                        , '{sROLL_GAP_LEFT}'
                        , '{sROLL_RPM_LEFT}'
                        , '{sROLL_GAP_RIGHT}'
                        , '{sROLL_RPM_RIGHT}'
                        , SYSDATE
                        , '{clsCommon.UserId}'
                    )
                    ";

                    recipi_update_yn = true;

                    erp_status = "I";
                }
                else if (insertRow.RowState == DataRowState.Modified)
                {
                    wWORKDATE = insertRow["WORKDATE"].ToString();
                    wNUM = insertRow["NUM"].ToString();

                    SQL = $@"   
                    UPDATE WORK_ORDER
                    SET    WORK_START_DATE  = '{wWORKDATE}'
                           , BATCH            = '{w_batch}'
                           , BATCH_Q          = '{w_batch_q}'
                           , OR_Q             = '{w_or_q}'
                           , PRO_Q            = '{w_pro_q}'
                           , RESOURCE_NO      = '{wRESOURCE_NO}'
                           , LOCATION_ED      = '{w_location_ed}'
                           , I_TIME           = SYSDATE
                           , ICM_CODE         = '{wICM_CODE}'
                           , EMPLOYEE_NO      = '{wEmployee_No}'
                           , REMARK           = '{wREMARK}'
                           , PACK_TYPE        = '{wPACK_TYPE}'
                           , TRANS_YN         = '{wTRANS_YN}'
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
                    WHERE  PLANT_CODE = '{wPlant}' AND PROCESS_KEY = '{wProcess_Key}' AND L_CODE = '{wL_Code}'
                        AND    WORKDATE     = '{wWORKDATE}'
                        AND    NUM          = '{wNUM}'
                    ";

                    SQL2 = $@"
                    /* 후레이크 레포트 수정 */
                    MERGE INTO FLAKE_REPORT d
                    USING (
                            SELECT '{wPlant}'        AS PLANT_CODE
                                , '{wProcess_Key}'  AS PROCESS_KEY
                                , '{wL_Code}'              AS L_CODE
                                , '{wWORKDATE}'              AS WORKDATE
                                , '{wNUM}'              AS NUM
                                , '{sAFTER_INPUT}'    AS AFTER_INPUT     -- 01
                                , '{sBEFORE_INPUT}'   AS BEFORE_INPUT    -- 02
                                , '{sCOOKING1}'       AS COOKING1        -- 03
                                , '{sCOOKING2}'       AS COOKING2        -- 04
                                , '{sFEEDER}'         AS FEEDER          -- 05
                                , SYSDATE                  AS I_TIME          -- 06
                                , '{clsCommon.UserId}'         AS I_USER          -- 07
                                , '{sPRESSURE}'       AS PRESSURE        -- 11
                                , '{sROLL_GAP_LEFT}'  AS ROLL_GAP_LEFT   -- 13
                                , '{sROLL_GAP_RIGHT}' AS ROLL_GAP_RIGHT  -- 14
                                , '{sROLL_RPM_LEFT}'  AS ROLL_RPM_LEFT   -- 15
                                , '{sROLL_RPM_RIGHT}' AS ROLL_RPM_RIGHT  -- 16
                                , '{sSTEAM_INPUT}'    AS STEAM_INPUT     -- 17
                                , '{sTEMP_LOWER}'     AS TEMP_LOWER      -- 18
                                , '{sTEMP_UPPER}'     AS TEMP_UPPER      -- 19
                                FROM DUAL
                            ) s
                    ON (d.L_CODE      = s.L_CODE
                        AND d.NUM      = s.NUM
                        AND d.PLANT_CODE  = s.PLANT_CODE
                        AND d.PROCESS_KEY = s.PROCESS_KEY
                        AND d.WORKDATE    = s.WORKDATE)

                    WHEN MATCHED THEN
                        UPDATE
                            SET d.AFTER_INPUT    = s.AFTER_INPUT
                                , d.BEFORE_INPUT   = s.BEFORE_INPUT
                                , d.COOKING1       = s.COOKING1
                                , d.COOKING2       = s.COOKING2
                                , d.FEEDER         = s.FEEDER
                                , d.I_TIME         = s.I_TIME
                                , d.I_USER         = s.I_USER
                                , d.PRESSURE       = s.PRESSURE
                                , d.ROLL_GAP_LEFT  = s.ROLL_GAP_LEFT
                                , d.ROLL_GAP_RIGHT = s.ROLL_GAP_RIGHT
                                , d.ROLL_RPM_LEFT  = s.ROLL_RPM_LEFT
                                , d.ROLL_RPM_RIGHT = s.ROLL_RPM_RIGHT
                                , d.STEAM_INPUT    = s.STEAM_INPUT
                                , d.TEMP_LOWER     = s.TEMP_LOWER
                                , d.TEMP_UPPER     = s.TEMP_UPPER

                    WHEN NOT MATCHED THEN
                        INSERT (
                                    AFTER_INPUT
                                , BEFORE_INPUT
                                , COOKING1
                                , COOKING2
                                , FEEDER
                                , I_TIME
                                , I_USER
                                , L_CODE
                                , NUM
                                , PLANT_CODE
                                , PRESSURE
                                , PROCESS_KEY
                                , ROLL_GAP_LEFT
                                , ROLL_GAP_RIGHT
                                , ROLL_RPM_LEFT
                                , ROLL_RPM_RIGHT
                                , STEAM_INPUT
                                , TEMP_LOWER
                                , TEMP_UPPER
                                , WORKDATE
                                )
                        VALUES (
                                    s.AFTER_INPUT
                                , s.BEFORE_INPUT
                                , s.COOKING1
                                , s.COOKING2
                                , s.FEEDER
                                , s.I_TIME
                                , s.I_USER
                                , s.L_CODE
                                , s.NUM
                                , s.PLANT_CODE
                                , s.PRESSURE
                                , s.PROCESS_KEY
                                , s.ROLL_GAP_LEFT
                                , s.ROLL_GAP_RIGHT
                                , s.ROLL_RPM_LEFT
                                , s.ROLL_RPM_RIGHT
                                , s.STEAM_INPUT
                                , s.TEMP_LOWER
                                , s.TEMP_UPPER
                                , s.WORKDATE
                                )
                    ";


                    SQL3 = $@"
                    UPDATE PELLET_REPORT
                    SET BF_QTY = '{w_or_q}'
                    WHERE BF_PLANT_CODE = '{wPlant}'
                            AND BF_PROCESS_KEY = '{wProcess_Key}' AND BF_L_CODE = '{wL_Code}'
                            AND BF_WORKDATE = '{wWORKDATE}' AND BF_NUM = '{wNUM}'
                    ";

                    erp_status = "U";
                }
                else
                {
                    return "작업지시 입력에 실패했습니다";
                }

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave("clsProcessDosing", "InsertWorkNum", SQL);
                    return "작업지시 입력을 실패했습니다";
                }

                if (wProcess_Key == clsCommon.GetProcessKey("후레이크", wPlant))
                {
                    if (Dbconn.conn.SQLrun(SQL2) < 1)
                    {
                        Dbconn.conn.Rollback();
                        clsLog.logSave("clsProcessDosing", "InsertWorkNum", SQL);
                        return "후레이크 작업지시 입력을 실패했습니다";
                    }
                }

                //if (!string.IsNullOrEmpty(pelletProcess))
                //{
                //    if (Dbconn.conn.SQLrun(SQL3) < 1)
                //    {
                //        Dbconn.conn.Rollback();
                //        clsLog.logSave("clsProcessDosing", "InsertWorkNum", SQL3);
                //        return "펠렛 작업지시 입력을 실패했습니다";
                //    }

                //    if (SQL31 != "" && Dbconn.conn.SQLrun(SQL31) < 1)
                //    {
                //        Dbconn.conn.Rollback();
                //        clsLog.logSave("clsProcessDosing", "InsertWorkNum", SQL3);
                //        return "펠렛 작업일지 입력을 실패했습니다";
                //    }
                //}

                //WORK_DETAIL
                if (recipi_update_yn)
                {
                    SQL = $"DELETE FROM WORK_DETAIL WHERE PLANT_CODE = '{wPlant}' AND PROCESS_KEY = '{wProcess_Key}' AND L_CODE = '{wL_Code}' AND WORKDATE = '{wWORKDATE}' AND NUM = '{wNUM}' ";

                    Dbconn.conn.SQLrun(SQL);

                    string dupBin = string.Empty;

                    bool binSeqDupChk = clsProcessDosing.BinSeqDupChk(wPlant, wProcess_Key, wL_Code, wRESOURCE_NO, w_note, out dupBin);
                    if (!binSeqDupChk)
                    {
                        Dbconn.conn.Rollback();
                        return "같은 원료빈이 중첩되는 빈이 존재합니다\r\n중첩된 빈 : " + dupBin;
                    }


                    using (DataSet resultMixDs = clsProcessDosing.resultMixResult(wPlant, wProcess_Key, wL_Code, wRESOURCE_NO, w_note, w_batch_q, wBU_YN))
                    {
                        if (Dbconn.conn.getRowCnt(resultMixDs) == 0)
                        {
                            Dbconn.conn.Rollback();
                            return "레시피에 대한 빈매칭 정보를 찾을수가 없습니다 ";
                        }

                        string oldScale = string.Empty;
                        int iBinSeq = 1;
                        int hand_cnt = 1;
                        for (int i = 0; i < Dbconn.conn.getRowCnt(resultMixDs); i++)
                        {
                            // 031002	배합공정
                            //if (wProcess_Key == clsCommon.GetProcessKey("배합"))
                            //{
                            //    if (Dbconn.conn.getData(resultMixDs, "SCALE_CODE", i).Trim() == "빈없음")
                            //    {
                            //        Dbconn.conn.Rollback();
                            //        return "빈이 매칭 안된 원료가 존재합니다";
                            //    }
                            //}

                            string scale_cd = string.Empty;

                            if (Dbconn.conn.getData(resultMixDs, "SCALE_CODE", i).Trim() == "빈없음")
                            {
                                scale_cd = "H" + hand_cnt.ToString();

                                hand_cnt = hand_cnt + 1;
                            }
                            else
                            {
                                scale_cd = Dbconn.conn.getData(resultMixDs, "SCALE_CODE", i);

                                if (oldScale != scale_cd)
                                {
                                    oldScale = scale_cd;
                                    iBinSeq = 1;
                                }
                                else
                                    iBinSeq++;
                            }

                            if (Dbconn.conn.getData(resultMixDs, "LOCATION", i) != "소계")
                            {
                                SQL = $@"
                                INSERT INTO WORK_DETAIL (
                                   PLANT_CODE, PROCESS_KEY, L_CODE , 
                                   WORKDATE, NUM, INGRED_CODE, 
                                   SCALE_CODE, LOCATION, QTY_PCT, 
                                   SET_VAL, RC_YN, BIN_TYPE, 
                                   HR_ERR, SEQ, BIN_SEQ, I_TIME) 
                                VALUES (
                                    '{wPlant}', '{wProcess_Key}','{wL_Code}',
                                    '{wWORKDATE}', '{wNUM}', '{Dbconn.conn.getData(resultMixDs, "RESOURCE_NO", i)}',
                                    '{scale_cd}', '{Dbconn.conn.getData(resultMixDs, "LOCATION", i)}', '{Dbconn.conn.getData(resultMixDs, "QTY_PCT", i)}',
                                    '{Dbconn.conn.getData(resultMixDs, "SET_VAL", i)}', '{Dbconn.conn.getData(resultMixDs, "BU_YN", i)}', '{Dbconn.conn.getData(resultMixDs, "BIN_GUBUN", i)}',
                                    '{Dbconn.conn.getData(resultMixDs, "HL_ERROR", i)}', '{(i + 1)}', {iBinSeq}, SYSDATE)
                                ";

                                if (Dbconn.conn.SQLrun(SQL) < 1)
                                {
                                    Dbconn.conn.Rollback();
                                    clsLog.logSave("clsProcessDOsing", "InsertWorkNum", SQL);
                                    return "레시피 입력도중 에러가 발생했습니다";
                                }
                            }
                        }
                    }
                }

                Dbconn.conn.Commit();

                ////erp input
                //string erp_insert_chk = clsErpSql.InsertWorkOrder(process_key, wWork_Date, wNUM, insertRow, erp_status);
                //if (erp_insert_chk != "OK")
                //{
                //    return erp_insert_chk;
                //}

                return "OK";
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "InsertWorkNum", ex);
                return "작업지시 입력에 실패했습니다";
            }
        }

        private static string workNumber_maker(string sWorkDate)
        {
            try
            {
                string return_seq = string.Empty;
                string SQL =
                "SELECT NVL(MAX(WORK_SEQ) + 1, 1) AS SEQ  " +
                "FROM PELLET_REPORT WHERE WORKDATE = '{0}' ";
                SQL = string.Format(SQL,
                    string.Format("{0:yyyyMMdd}", sWorkDate));

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
                clsLog.logSave("clsProcessDosing", "workNumber_maker", ex);
                return string.Empty;
            }
        }

        public static bool DeleteWorkNum(string process_key, string wWork_Date, string work_num)
        {
            try
            {
                Dbconn.conn.BeginTransaction();
                string SQL =
                "DELETE FROM WORK_ORDER WHERE PROCESS_KEY = '{0}' AND WORKDATE = '{1}' AND NUM = '{2}' ";
                SQL = string.Format(SQL, process_key, wWork_Date, work_num);

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    return false;
                }

                SQL = "DELETE FROM WORK_DETAIL  PROCESS_KEY = '{0}' AND WORKDATE = '{1}' AND NUM = '{2}' ";
                SQL = string.Format(SQL, process_key, wWork_Date, work_num);

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    return false;
                }

                Dbconn.conn.Commit();
                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "DeleteWorkNum", ex);
                return false;
            }
        }

        public static DataSet resultMixResult(string plantCode, string processCd, string lCode, string pCode, string note, string batch_q, string rc_chk)
        {
            try
            {
                string scale_name = string.Empty;

                scale_name = processCd == clsCommon.GetProcessKey("배합") ? "빈없음" : "수계량";

                string SQL = $@"
                WITH TEMPBIN AS (
                    SELECT a.*
                         , COUNT(*) OVER (PARTITION BY RESOURCE_NO) AS a_count
                    FROM bin a
                    WHERE PLANT_CODE = '{plantCode}' AND PROCESS_KEY = '{processCd}' AND L_CODE = '{lCode}'
                )
                , BASE_DATA AS (
                    SELECT NVL(c.SCALE_CODE, 'H') AS SCALE_CODE
                         , NVL(c.LOCATION, 'H') AS LOCATION
                         , a.RESOURCE_NO
                         , ({batch_q} * (a.MENGE * 0.01)) AS SET_VAL
                         , a.MENGE AS QTY_PCT
                         , a.BU_YN
                         , c.BIN_GUBUN
                         , c.HL_ERROR
                         , d.SCALE_NO
                         , '데이터' AS ROW_TYPE
                    FROM (
                        SELECT a.PLANT_CODE, TO_NCHAR(a.IDNRK) AS RESOURCE_NO, a.MENGE, 'N' AS BU_YN
                        FROM SAP_IN_BOM_COND a
                        WHERE a.PLANT_CODE = '{plantCode}'
                          AND a.RESOURCE_NO = '{pCode}' 
                          AND a.NOTE = '{note}'
                          AND a.P_TYPE = '2'
                          AND a.IDNRK NOT IN (
                              SELECT RESOURCE_NO_2 
                              FROM SAP_IN_PRODUCT_CP 
                              WHERE PLANT_CODE = a.PLANT_CODE AND RESOURCE_NO = a.RESOURCE_NO AND RESOURCE_NO_2 = a.IDNRK
                          )
                        UNION ALL
                        SELECT a.PLANT_CODE, TO_NCHAR(b.RESOURCE_NO_3) AS RESOURCE_NO, a.MENGE, 'N' AS BU_YN
                        FROM SAP_IN_BOM_COND a
                           INNER JOIN SAP_IN_PRODUCT_CP b 
                               ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO AND b.RESOURCE_NO_2 = a.IDNRK
                        WHERE a.PLANT_CODE = '{plantCode}'
                          AND a.RESOURCE_NO = '{pCode}' 
                          AND a.NOTE = '{note}'
                          AND a.P_TYPE = '2'
                        UNION ALL
                        SELECT DISTINCT a.PLANT_CODE, TO_NCHAR(b.RESOURCE_NO_2) AS RESOURCE_NO, b.PART_P AS MENGE, 'Y' AS BU_YN
                        FROM SAP_IN_BOM_COND a
                             INNER JOIN SAP_IN_PRODUCT_RC b 
                                 ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                        WHERE a.PLANT_CODE = '{plantCode}'
                          AND a.RESOURCE_NO = '{pCode}'
                          AND '{rc_chk}' = 'Y' 
                    ) a
                        LEFT JOIN TEMPBIN c 
                            ON ((c.a_count > 1 AND c.SEQ = 1) OR (c.a_count = 1)) 
                           AND c.RESOURCE_NO = a.RESOURCE_NO
                        LEFT JOIN SCALE d 
                            ON d.PLANT_CODE = c.PLANT_CODE 
                           AND d.PROCESS_KEY = c.PROCESS_KEY 
                           AND d.L_CODE = c.L_CODE 
                           AND d.SCALE_CODE = c.SCALE_CODE
                )
                -- 결과: 원본 + 소계
                SELECT
                    SCALE_CODE
                    , LOCATION
                    , RESOURCE_NO
                    , SET_VAL
                    , QTY_PCT
                    , BU_YN
                    , BIN_GUBUN
                    , HL_ERROR
                    , SCALE_NO
                    , ROW_TYPE
                FROM (
                    SELECT 
                        SCALE_CODE
                      , LOCATION
                      , RESOURCE_NO
                      , SET_VAL
                      , QTY_PCT
                      , BU_YN
                      , BIN_GUBUN
                      , HL_ERROR
                      , SCALE_NO
                      , ROW_TYPE
                    FROM BASE_DATA
                    UNION ALL
                    -- 소계 부분
                    SELECT 
                        SCALE_CODE
                      , TO_NCHAR('소계') AS LOCATION
                      , NULL AS RESOURCE_NO
                      , SUM(SET_VAL) AS SET_VAL
                      , SUM(QTY_PCT) AS QTY_PCT
                      , NULL AS BU_YN
                      , NULL AS BIN_GUBUN
                      , NULL AS HL_ERROR
                      , NULL AS SCALE_NO
                      , NULL AS ROW_TYPE
                    FROM BASE_DATA
                    GROUP BY SCALE_CODE
                )
                ORDER BY CASE WHEN SCALE_CODE = 'H' THEN 1 ELSE 0 END,  -- H는 큰 값(1)
                        SCALE_CODE ASC, RESOURCE_NO
                ";

                //string SQL = $@"
                //WITH TEMPBIN AS (
                //    SELECT a.*
                //         , COUNT(*) OVER (PARTITION BY RESOURCE_NO) AS a_count
                //    FROM bin a
                //     WHERE PLANT_CODE = '{plantCode}' AND PROCESS_KEY = '{processCd}' AND L_CODE = '{lCode}'
                //)

                //SELECT NVL(c.SCALE_CODE, 'H') AS SCALE_CODE, NVL(c.LOCATION, 'H') AS LOCATION
                //     , a.RESOURCE_NO, ({batch_q} * (a.MENGE * 0.01)) AS SET_VAL, a.MENGE AS QTY_PCT
                //    , a.BU_YN, c.BIN_GUBUN, c.HL_ERROR, d.SCALE_NO
                //FROM(
                //    SELECT a.PLANT_CODE, TO_NCHAR(a.IDNRK) AS RESOURCE_NO, a.MENGE, 'N' AS BU_YN
                //    FROM SAP_IN_BOM_COND a
                //    WHERE a.PLANT_CODE = '{plantCode}'
                //        AND a.RESOURCE_NO = '{pCode}' 
                //        AND a.NOTE = '{note}'
                //        AND a.P_TYPE = '2'
                //        AND a.IDNRK NOT IN (SELECT RESOURCE_NO_2 FROM SAP_IN_PRODUCT_CP WHERE RESOURCE_NO = a.RESOURCE_NO AND RESOURCE_NO_2 = a.IDNRK)
                //    UNION ALL
                //    SELECT a.PLANT_CODE, TO_NCHAR(b.RESOURCE_NO_3) AS RESOURCE_NO, a.MENGE, 'N' AS BU_YN
                //    FROM SAP_IN_BOM_COND a
                //       INNER JOIN SAP_IN_PRODUCT_CP b ON b.RESOURCE_NO = a.RESOURCE_NO AND b.RESOURCE_NO_2 = a.IDNRK
                //    WHERE a.PLANT_CODE = '{plantCode}'
                //        AND a.RESOURCE_NO = '{pCode}' 
                //        AND a.NOTE = '{note}'
                //        AND a.P_TYPE = '2'
                //    UNION ALL
                //    SELECT DISTINCT a.PLANT_CODE, TO_NCHAR(b.RESOURCE_NO_2) AS RESOURCE_NO, b.PART_P AS MENGE, 'Y' AS BU_YN
                //    FROM SAP_IN_BOM_COND a
                //        INNER JOIN SAP_IN_PRODUCT_RC b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                //    WHERE a.PLANT_CODE = '{plantCode}'
                //        AND a.RESOURCE_NO = '{pCode}'
                //        AND '{rc_chk}' = 'Y' 
                //) a
                //    LEFT JOIN TEMPBIN c ON ((c.a_count > 1 AND c.SEQ = 1) OR (c.a_count = 1)) AND c.RESOURCE_NO = a.RESOURCE_NO
                //    LEFT JOIN SCALE D ON d.PLANT_CODE = c.PLANT_CODE AND d.PROCESS_KEY = c.PROCESS_KEY AND d.L_CODE = c.L_CODE AND d.SCALE_CODE = c.SCALE_CODE
                //ORDER BY SCALE_CODE, 
                //         MENGE DESC, 
                //         RESOURCE_NO, 
                //         NVL(d.SCALE_NO, '9999')
                //";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);

                return ds;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }

        public static DataSet resultWorkResult(string plantCode, string processCd, string lCode, string workdate, string num)
        {
            try
            {
                string SQL = string.Empty;
                string scale_name = string.Empty;

                scale_name = processCd == clsCommon.PcStatus.Plan ? "빈없음" : "수계량";

                string[] noneDosing = new string[] { clsCommon.GetProcessKey("후레이크"), clsCommon.GetProcessKey("익스콘"), clsCommon.GetProcessKey("벌크 원료") };

                if (noneDosing.Contains(processCd))
                {
                    SQL = $@"
                    SELECT workd.WORKDATE
                        , workd.NUM
                        , CASE WHEN NVL(workd.LOCATION, ' ') = ' '
                            THEN '빈없음'
                            ELSE TO_CHAR(workd.SCALE_CODE)
                        END AS SCALE_CODE
                        , sc.SCALE_NO
                        , workd.LOCATION
                        , workd.INGRED_CODE AS RESOURCE_NO
                        , TRIM(d.DESCRIPTION) AS DESCRIPTION
                        , workd.QTY_PCT
                        , CASE WHEN wo.PRO_Q IS NULL OR wo.PRO_Q = 0
                                        THEN workd.SET_VAL
                                ELSE wo.PRO_Q * (workd.QTY_PCT * 0.01)
                            END SET_VAL
                        , sc.IN_SCALE
                        , workd.BIN_SEQ
                        , 0 AS SORT_ORDER
                    FROM WORK_DETAIL workd
                        LEFT JOIN WORK_ORDER wo ON wo.PLANT_CODE = workd.PLANT_CODE
                                            AND wo.PROCESS_KEY = workd.PROCESS_KEY
                                            AND wo.L_CODE = workd.L_CODE
                                            AND wo.WORKDATE = workd.WORKDATE
                                            AND wo.NUM = workd.NUM
                        LEFT JOIN SCALE sc
                            ON workd.PLANT_CODE = sc.PLANT_CODE
                            AND workd.PROCESS_KEY = sc.PROCESS_KEY
                            AND workd.L_CODE = sc.L_CODE
                            AND workd.SCALE_CODE = sc.SCALE_CODE
                        LEFT JOIN SAP_DI_PRODUCT d
                            ON d.PLANT_CODE = workd.PLANT_CODE
                            AND d.RESOURCE_NO = workd.INGRED_CODE
                    WHERE workd.PLANT_CODE = '{plantCode}'
                    AND workd.PROCESS_KEY = '{processCd}'
                    AND workd.L_CODE = '{lCode}'
                    AND workd.WORKDATE = '{workdate}'
                    AND workd.NUM = '{num}'
                    ";
                }
                else
                {
                    SQL = $@"
                    WITH TEMPBIN AS (
                                    SELECT a.*
                                            , COUNT(*) OVER (PARTITION BY RESOURCE_NO) AS a_count
                                    FROM bin a
                                        WHERE PLANT_CODE = '{plantCode}' AND PROCESS_KEY = '{processCd}' AND L_CODE = '{lCode}'
                                ),
                    DETAIL_UNION AS (
                            SELECT DISTINCT workd.WORKDATE
                                , workd.NUM
                                , CASE WHEN NVL(workd.LOCATION, ' ') = ' '
                                    THEN '빈없음'
                                    ELSE TO_CHAR(workd.SCALE_CODE)
                                END AS SCALE_CODE
                                , sc.SCALE_NO
                                , workd.LOCATION
                                , workd.INGRED_CODE AS RESOURCE_NO
                                , TRIM(d.DESCRIPTION) AS DESCRIPTION
                                , workd.QTY_PCT
                                , workd.SET_VAL
                                , sc.IN_SCALE
                                , workd.BIN_SEQ
                                , 0 AS SORT_ORDER
                            FROM WORK_DETAIL workd
                                LEFT JOIN TEMPBIN b
                                    ON ((b.a_count > 1 AND b.SEQ = 1) OR (b.a_count = 1))
                                    AND b.RESOURCE_NO = workd.INGRED_CODE
                                LEFT JOIN SCALE sc
                                    ON workd.PLANT_CODE = sc.PLANT_CODE
                                    AND workd.PROCESS_KEY = sc.PROCESS_KEY
                                    AND workd.L_CODE = sc.L_CODE
                                    AND workd.SCALE_CODE = sc.SCALE_CODE
                                LEFT JOIN SAP_DI_PRODUCT d
                                    ON d.PLANT_CODE = workd.PLANT_CODE
                                    AND d.RESOURCE_NO = workd.INGRED_CODE
                            WHERE workd.PLANT_CODE = '{plantCode}'
                            AND workd.PROCESS_KEY = '{processCd}'
                            AND workd.L_CODE = '{lCode}'
                            AND workd.WORKDATE = '{workdate}'
                            AND workd.NUM = '{num}'
                        UNION ALL
                        SELECT DISTINCT wr.WORKDATE
                                , wr.NUM
                                , TO_CHAR(b.SCALE_CODE) AS SCALE_CODE
                                , sc.SCALE_NO
                                , wr.LOCATION
                                , wr.INGRED_LOT AS RESOURCE_NO
                                , TRIM(wr.NAME) AS DESCRIPTION
                                , NULL AS QTY_PCT
                                , NULL AS SET_VAL
                                , sc.IN_SCALE
                                , 99 AS BIN_SEQ
                                , 0 AS SORT_ORDER
                            FROM WORK_REMARK wr
                                LEFT JOIN TEMPBIN b
                                    ON ((b.a_count > 1 AND b.SEQ = 1) OR (b.a_count = 1))
                                    AND b.RESOURCE_NO = wr.INGRED_LOT
                                LEFT JOIN SCALE sc
                                    ON b.PLANT_CODE = sc.PLANT_CODE
                                    AND b.PROCESS_KEY = sc.PROCESS_KEY
                                    AND b.L_CODE = sc.L_CODE
                                    AND b.SCALE_CODE = sc.SCALE_CODE
                            WHERE wr.PLANT_CODE = '{plantCode}'
                            AND wr.PROCESS_KEY = '{processCd}'
                            AND wr.L_CODE = '{lCode}'
                            AND wr.WORKDATE = '{workdate}'
                            AND wr.NUM = '{num}'
                            AND wr.LOCATION NOT IN (
                                SELECT LOCATION
                                    FROM WORK_DETAIL
                                WHERE PLANT_CODE = '{plantCode}'
                                    AND PROCESS_KEY = '{processCd}'
                                    AND L_CODE = '{lCode}'
                                    AND WORKDATE = '{workdate}'
                                    AND NUM = '{num}'
                                    AND LOCATION IS NOT NULL
                            )
                            AND wr.LOCATION NOT LIKE 'H%'
                    ),
                    -- 소계 데이터 (행)
                    SUB_TOTAL AS (
                        SELECT MIN(WORKDATE) AS WORKDATE
                                , MIN(NUM) AS NUM
                                , SCALE_CODE
                                , NULL AS SCALE_NO
                                , TO_NCHAR('소계') AS LOCATION
                                , NULL AS RESOURCE_NO
                                , TO_NCHAR('') AS DESCRIPTION
                                , SUM(NVL(QTY_PCT, 0)) AS QTY_PCT
                                , SUM(NVL(SET_VAL, 0)) AS SET_VAL
                                , NULL AS IN_SCALE
                                , 99 AS BIN_SEQ
                                , 1 AS SORT_ORDER
                            FROM DETAIL_UNION
                            WHERE SET_VAL IS NOT NULL
                            GROUP BY SCALE_CODE
                    )

                    -- 결과 출력
                    SELECT WORKDATE
                            , NUM
                            , SCALE_CODE
                            , SCALE_NO
                            , LOCATION
                            , RESOURCE_NO
                            , DESCRIPTION
                            , QTY_PCT
                            , SET_VAL
                            , BIN_SEQ
                            , IN_SCALE
                        FROM (
                        SELECT * FROM DETAIL_UNION
                        UNION ALL
                        SELECT * FROM SUB_TOTAL
                        )
                    ORDER BY CASE WHEN SCALE_CODE = 'H' THEN 1 ELSE 0 END,  -- H는 큰 값(1)
                            SCALE_CODE ASC, SORT_ORDER, QTY_PCT DESC, RESOURCE_NO, NVL(SCALE_NO, 9999) DESC
                    ";
                }

                return Dbconn.conn.ExecutDataset(SQL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }

        public static DataSet resultWorkSendIndex(string processCd, string workdate, string num)
        {
            try
            {
                string SQL = $@"
                    SELECT workd.WORKDATE, workd.NUM,  workd.SCALE_CODE, sc.SCALE_NO, b.BIN_SERIAL, workd.LOCATION, workd.INGRED_CODE as RESOURCE_NO,
                    workd.BMENG, workd.SET_VAL, b.FAIL, sc.IN_SCALE 
                    FROM WORK_DETAIL workd LEFT OUTER JOIN BIN b ON workd.LOCATION = b.LOCATION
                        LEFT OUTER JOIN SCALE sc ON workd.PLANT_CODE = sc.PLANT_CODE
                                    AND workd.PROCESS_KEY = sc.PROCESS_KEY
                                    AND workd.L_CODE = sc.L_CODE
                                    AND workd.SCALE_CODE = sc.SCALE_CODE
                        LEFT OUTER JOIN INGRED ing ON workd.PLANT_CODE = ing.PLANT_CODE AND workd.INGRED_CODE = ing.RESOURCE_NO
                    WHERE workd.PROCESS_KEY = '{processCd}' AND workd.WORKDATE = '{workdate}' AND workd.NUM = '{num}'
                        AND sc.SCALE_CODE <> 'LIQUID' AND NVL(sc.SCALE_CODE,'NO') <> 'NO'
                    ORDER BY workd.SEQ
                    ";

                return Dbconn.conn.ExecutDataset(SQL);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }

        public static void SetWorkOrderQty(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate, string sNum)
        {
            try
            {
                string SQL = $@"
                UPDATE WORK_ORDER a
                    SET OR_Q = (
                                SELECT SUM(b.SET_VAL) * a.Batch
                                FROM WORK_DETAIL b
                                WHERE b.PLANT_CODE = a.PLANT_CODE
                                    AND b.PROCESS_KEY = a.PROCESS_KEY
                                    AND b.L_CODE = a.L_CODE
                                    AND b.WORKDATE = a.WORKDATE
                                    AND b.NUM = a.NUM
                                    AND b.INGRED_CODE NOT IN (
                                        SELECT c.COMM_DTCODE
                                            FROM COMM_DIV x
                                                INNER JOIN COMM_CODE y
                                                    ON y.WK_DIVCODE = x.WK_DIVCODE
                                                INNER JOIN COMM_DTCODE c
                                                    ON c.WK_DIVCODE = x.WK_DIVCODE
                                                    AND c.COMM_CODE = y.COMM_CODE
                                            WHERE c.WK_DIVCODE = '03'
                                            AND c.COMM_CODE = '80'
                                        )
                            )
                WHERE a.PLANT_CODE = '{sPlantCode}'
                    AND a.PROCESS_KEY = '{sProcessKey}'
                    AND a.L_CODE = '{sLCode}'
                    AND a.WORKDATE = '{sWorkDate}'
                    AND a.NUM = '{sNum}'
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave("clsProcesssDosing", "SetWorkOrderQty", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 수정에 실패했습니다");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }

        public static bool InsertLog(string Plant_Code, string processCd, string l_code, string workdate, string num, string batch, string logCd, string errMsg)
        {
            try
            {
                string newSeq = string.Empty;

                string SQL = $@"
                SELECT NVL(MAX(SEQ),0) + 1 as SEQ FROM BATCH_LOG
                WHERE PROCESS_KEY = '{processCd}' AND WORKDATE = '{workdate}' AND NUM = '{num}' AND BATCH = '{batch}'
                ";

                DataSet newSeqDs = Dbconn.conn.ExecutDataset(SQL);
                if (Dbconn.conn.getRowCnt(newSeqDs) > 0)
                {
                    newSeq = Dbconn.conn.getData(newSeqDs, "SEQ", 0);
                }
                else
                {
                    return false;
                }


                SQL = $@" 
                INSERT INTO BATCH_LOG (
                   PLANT_CODE, PROCESS_KEY, L_CODE , 
                   NUM, BATCH, WORKDATE, 
                   SEQ, LOG_CODE, ERR_MSG, 
                   ST_TIME, ED_TIME, I_TIME) 
                VALUES('{Plant_Code}', '{processCd}', '{l_code}',
                    '{num}', '{batch}', '{workdate}',
                    '{newSeq}', '{logCd}', '{errMsg}', 
                    SYSDATE, SYSDATE, SYSDATE)
                ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "InsertLog", ex);
                return false;
            }
        }

        public static bool InsertWorkRemark(string plantCode, string processCd, string l_code, string workdate, string num)
        {
            try
            {
                String w_num = num;
                String wRESOURCE_NO = "";
                String w_note = "";
                String w_batch_q = "";

                string SQL = string.Empty;

                int last_batch = 0;

                SQL = $@"
                SELECT RESOURCE_NO, R_BATCH, BATCH, BATCH_Q, NOTE
                FROM WORK_ORDER WHERE PLANT_CODE = '{plantCode}' AND PROCESS_KEY = '{processCd}' AND L_CODE = '{l_code}'
                    AND WORKDATE = '{workdate}' AND NUM = '{num}'
                ";

                DataSet workDs = Dbconn.conn.ExecutDataset(SQL);
                if (Dbconn.conn.getRowCnt(workDs) > 0)
                {
                    last_batch = Convert.ToInt16(Dbconn.conn.getData(workDs, "BATCH", 0));
                    wRESOURCE_NO = Dbconn.conn.getData(workDs, "RESOURCE_NO", 0);
                    w_note = Dbconn.conn.getData(workDs, "NOTE", 0);
                    w_batch_q = Dbconn.conn.getData(workDs, "BATCH_Q", 0);
                }


                for (int i = 0; i < last_batch; i++)
                {
                    DataSet resultMixDs = clsProcessDosing.resultWorkResult(plantCode, processCd, l_code, workdate, num);

                    int mixReusltCnt = Dbconn.conn.getRowCnt(resultMixDs);
                    if (mixReusltCnt > 0)
                    {
                        int hand_cnt = 1;
                        for (int j = 0; j < mixReusltCnt; j++)
                        {
                            string BMENG = Dbconn.conn.getData(resultMixDs, "BMENG", j);
                            string RESOURCE_NO = Dbconn.conn.getData(resultMixDs, "RESOURCE_NO", j);
                            string description = Dbconn.conn.getData(resultMixDs, "DESCRIPTiON", j);
                            string SET_VAL = Dbconn.conn.getData(resultMixDs, "SET_VAL", j);
                            string location = Dbconn.conn.getData(resultMixDs, "LOCATION", j);

                            if (string.IsNullOrEmpty(location))
                            {
                                location = "H" + hand_cnt.ToString();
                                hand_cnt = hand_cnt + 1;
                            }

                            //SQL = "DELETE FROM WORK_REMARK WHERE PRO PROCESS_KEY = '{0}' WORKDATE = '{1}' NUM = '{2}' BATCH = '{3}' ";

                            if (location != "소계")
                            {
                                SQL = $@"
                                INSERT INTO WORK_REMARK
                                (PLANT_CODE, PROCESS_KEY, L_CODE , WORKDATE, NUM, BATCH, SEQ, IO_GUBUN, LOCATION, P_Q, IO_DATE
                                , RESOURCE_NO, INGRED_LOT, P_TYPE, I_TIME, P_Q_TIME) 
                                VALUES ('{plantCode}', '{processCd}', '{l_code}', '{workdate}', '{num}', '{(i + 1)}', '{(j + 1)}', 'I', '{location}', '{SET_VAL}', SYSDATE
                                , '{wRESOURCE_NO}', '{RESOURCE_NO}', 'I', SYSDATE, '0' )
                                ";

                                if (Dbconn.conn.SQLrun(SQL) < 1)
                                {
                                    clsLog.logSave("clsProcesssDosing", "InsertWorkRemark", SQL);
                                    return false;
                                }
                            }
                        }
                    }
                    else if (mixReusltCnt == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "InsertWorkRemark", ex);
                return false;
            }
        }

        #region 이전 SAP 공정 생성
        ///// <summary>
        ///// SAP 공정 생성
        ///// </summary>
        ///// <param name="sPlant_Code"></param>
        ///// <param name="sProcess_Code"></param>
        ///// <param name="sLine_Code"></param>
        ///// <param name="sWORKDATE"></param>
        ///// <param name="sNum"></param>
        //public static bool SetSAPWorkRemark(string sPlant_Code, string sProcess_Code, string sLine_Code, string sWORKDATE, string sNum)
        //{
        //    string SQL = $@"
        //    MERGE INTO WORK_REMARK target
        //    USING (
        //        SELECT
        //            PLANT_CODE
        //            , 'SAP_' || PROCESS_KEY AS PROCESS_KEY
        //            , L_CODE
        //            , WORKDATE
        //            , NUM
        //            , BATCH
        //            , MAX(SEQ) AS SEQ
        //            , 'I' AS IO_GUBUN
        //            , MAX(LOCATION) AS LOCATION
        //            , P_Q
        //            , SYSDATE AS IO_DATE
        //            , RESOURCE_NO
        //            , INGRED_CODE
        //            , '2' AS P_TYPE
        //            , SYSDATE AS I_TIME
        //            , '0' AS P_Q_TIME
        //        FROM (
        //            -- 본배합비
        //        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, wr.SEQ, wr.LOCATION
        //            , wr.INGRED_LOT AS INGRED_CODE, CASE WHEN rc.PART_P > 0 THEN wr.P_Q ELSE wd.SET_VAL END P_Q
        //        FROM WORK_ORDER wo
        //            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
        //                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM
        //            JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wr.PLANT_CODE AND wd.PROCESS_KEY = wr.PROCESS_KEY AND wd.L_CODE = wr.L_CODE
        //                    AND wd.WORKDATE = wr.WORKDATE AND wd.NUM = wr.NUM AND wd.INGRED_CODE = wr.INGRED_LOT
        //            LEFT JOIN SAP_IN_PRODUCT_RC rc ON rc.PLANT_CODE = wo.PLANT_CODE AND rc.RESOURCE_NO = wr.INGRED_LOT
        //        WHERE (NULL IS NULL OR wr.BATCH = NULL)
        //            AND wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
        //            AND wr.INGRED_LOT NOT IN (
        //                                        SELECT RESOURCE_NO_2
        //                                        FROM SAP_IN_UPPRODUCT_CP x
        //                                        WHERE x.PLANT_CODE = wo.PLANT_CODE AND x.RESOURCE_NO = wo.RESOURCE_NO AND x.RESOURCE_NO_2 = wr.INGRED_LOT AND NVL(x.SAP_UP_YN, 'X') != 'Y'
        //                                        UNION ALL
        //                                        SELECT RESOURCE_NO_3
        //                                        FROM SAP_IN_PRODUCT_CP x
        //                                        WHERE x.PLANT_CODE = wo.PLANT_CODE AND x.RESOURCE_NO = wo.RESOURCE_NO AND x.RESOURCE_NO_3 = wr.INGRED_LOT
        //                                        UNION ALL
        //                                        SELECT RESOURCE_NO
        //                                        FROM SAP_IN_PRODUCT_CH x
        //                                        WHERE x.PLANT_CODE = wo.PLANT_CODE AND x.RESOURCE_NO = wr.INGRED_LOT
        //                                        UNION ALL
        //                                        SELECT RESOURCE_NO
        //                                        FROM SAP_DI_PRODUCT x
        //                                        WHERE x.PLANT_CODE  = wo.PLANT_CODE AND wr.INGRED_LOT LIKE 'P%' AND x.RESOURCE_NO = wr.INGRED_LOT
        //                                        UNION ALL
        //                                        SELECT RESOURCE_NO
        //                                        FROM SAP_DI_PRODUCT x
        //                                        WHERE x.PLANT_CODE  = wo.PLANT_CODE AND x.RESOURCE_NO = wr.INGRED_LOT AND x.RESOURCE_TYPE = 'UNBW'
        //                                    )
        //        UNION ALL
        //        -- 제품 대체비율
        //        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
        //            ,  cp.RESOURCE_NO_2 AS INGRED_CODE, SUM(wr.P_Q)
        //        FROM WORK_ORDER wo
        //            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
        //                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM
        //            JOIN SAP_IN_PRODUCT_CP cp ON cp.PLANT_CODE = wo.PLANT_CODE AND cp.RESOURCE_NO = wr.RESOURCE_NO AND cp.RESOURCE_NO_3 = wr.INGRED_LOT
        //        WHERE (NULL IS NULL OR wr.BATCH = NULL)
        //            AND wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
        //        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, cp.RESOURCE_NO_2
        //        UNION ALL
        //        -- 원료 대체비율
        //        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
        //            ,  cp.RESOURCE_NO_2 AS INGRED_CODE, (SUM(wr.P_Q) * (cp.PART_P / 100))
        //        FROM WORK_ORDER wo
        //            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
        //                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM
        //            JOIN SAP_IN_PRODUCT_CH cp ON cp.PLANT_CODE = wo.PLANT_CODE AND cp.RESOURCE_NO = wr.INGRED_LOT
        //        WHERE (NULL IS NULL OR wr.BATCH = NULL)
        //            AND wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
        //        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH,  cp.RESOURCE_NO_2, cp.PART_P
        //        UNION ALL
        //        -- 프리믹스 배합비
        //        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
        //            ,  cond.IDNRK AS INGRED_CODE, (SUM(wo.BATCH_Q) * (cond.MENGE / 100))
        //        FROM WORK_ORDER wo
        //            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
        //                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM
        //            JOIN SAP_IN_BOM_COND cond ON cond.PLANT_CODE = wo.PLANT_CODE AND wr.INGRED_LOT LIKE 'P%' AND cond.RESOURCE_NO = wr.INGRED_LOT AND cond.P_TYPE = '1'
        //        WHERE (NULL IS NULL OR wr.BATCH = NULL)
        //            AND wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
        //        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, cond.IDNRK, cond.MENGE
        //        UNION ALL
        //        -- 프리믹스 대체비율
        //        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
        //                ,  cp.RESOURCE_NO_2 AS INGRED_CODE, SUM(wr.P_Q)
        //        FROM WORK_ORDER wo
        //            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
        //                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM
        //            JOIN SAP_IN_BOM_COND cond ON cond.PLANT_CODE = wo.PLANT_CODE AND wr.INGRED_LOT LIKE 'P%' AND cond.RESOURCE_NO = wr.INGRED_LOT AND cond.P_TYPE = '1'
        //            JOIN SAP_IN_PRODUCT_CP cp ON cp.PLANT_CODE = wo.PLANT_CODE AND cp.RESOURCE_NO = wr.RESOURCE_NO AND cp.RESOURCE_NO_3 = cond.IDNRK
        //        WHERE (NULL IS NULL OR wr.BATCH = NULL)
        //            AND wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
        //        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, cp.RESOURCE_NO_2
        //        UNION ALL
        //        -- 프리믹스 원료 대체비율
        //        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
        //                ,  cp.RESOURCE_NO_2 AS INGRED_CODE, (SUM(wr.P_Q) * (cp.PART_P / 100))
        //        FROM WORK_ORDER wo
        //            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
        //                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM
        //            JOIN SAP_IN_BOM_COND cond ON cond.PLANT_CODE = wo.PLANT_CODE AND wr.INGRED_LOT LIKE 'P%' AND cond.RESOURCE_NO = wr.INGRED_LOT AND cond.P_TYPE = '1'
        //            JOIN SAP_IN_PRODUCT_CH cp ON cp.PLANT_CODE = wo.PLANT_CODE AND cp.RESOURCE_NO = cond.IDNRK
        //        WHERE (NULL IS NULL OR wr.BATCH = NULL)
        //            AND wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
        //        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH, cp.RESOURCE_NO_2, cp.PART_P
        //        ORDER BY BATCH, SEQ
        //        )
        //        GROUP BY PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM, BATCH, P_Q
        //            , RESOURCE_NO, INGRED_CODE
        //    ) src
        //    ON (
        //        target.PLANT_CODE = src.PLANT_CODE AND
        //        target.PROCESS_KEY = src.PROCESS_KEY AND
        //        target.L_CODE = src.L_CODE AND
        //        target.WORKDATE = src.WORKDATE AND
        //        target.NUM = src.NUM AND
        //        target.BATCH = src.BATCH AND
        //        target.SEQ = src.SEQ AND
        //        target.IO_GUBUN = src.IO_GUBUN
        //    )
        //    WHEN MATCHED THEN
        //        UPDATE SET
        //            target.LOCATION     = src.LOCATION
        //            , target.P_Q          = src.P_Q
        //            , target.IO_DATE      = src.IO_DATE
        //            , target.RESOURCE_NO  = src.RESOURCE_NO
        //            , target.INGRED_LOT   = src.INGRED_CODE
        //            , target.P_TYPE       = src.P_TYPE
        //            , target.I_TIME       = src.I_TIME
        //            , target.P_Q_TIME     = src.P_Q_TIME
        //    WHEN NOT MATCHED THEN
        //        INSERT (
        //            PLANT_CODE
        //            , PROCESS_KEY
        //            , L_CODE
        //            , WORKDATE
        //            , NUM
        //            , BATCH
        //            , SEQ
        //            , IO_GUBUN
        //            , LOCATION
        //            , P_Q
        //            , IO_DATE
        //            , RESOURCE_NO
        //            , INGRED_LOT
        //            , P_TYPE
        //            , I_TIME
        //            , P_Q_TIME
        //        )
        //        VALUES (
        //            src.PLANT_CODE
        //            , src.PROCESS_KEY
        //            , src.L_CODE
        //            , src.WORKDATE
        //            , src.NUM
        //            , src.BATCH
        //            , src.SEQ
        //            , src.IO_GUBUN
        //            , src.LOCATION
        //            , src.P_Q
        //            , src.IO_DATE
        //            , src.RESOURCE_NO
        //            , src.INGRED_CODE
        //            , src.P_TYPE
        //            , src.I_TIME
        //            , src.P_Q_TIME
        //        )
        //    ";

        //    if (Dbconn.conn.SQLrun(SQL) < 1)
        //    {
        //        clsLog.logSave("clsProcesssDosing", "InsertWorkRemark", SQL);
        //        return false;
        //    }

        //    return true;
        //} 
        #endregion

        /// <summary>
        /// SAP 공정 생성
        /// </summary>
        /// <param name="sPlant_Code"></param>
        /// <param name="sProcess_Code"></param>
        /// <param name="sLine_Code"></param>
        /// <param name="sWORKDATE"></param>
        /// <param name="sNum"></param>
        public static bool SetSAPWorkRemark(string sPlant_Code, string sProcess_Code, string sLine_Code, string sWORKDATE, string sNum, string sRBatch = "")
        {
            try
            {
                string SQL = $@"
                MERGE INTO WORK_REMARK d
                USING (
                        SELECT a.PLANT_CODE
                                , 'SAP_' || a.PROCESS_KEY AS PROCESS_KEY
                                , a.L_CODE
                                , a.WORKDATE
                                , a.NUM
                                , '{sRBatch}' AS BATCH
                                , ROWNUM AS SEQ
                                , 'I' AS IO_GUBUN
                                , '' AS LOCATION
                                , d.IDNRK AS INGRED_LOT
                                , (a.BATCH_Q * (d.MENGE * 0.01)) AS P_Q
                                , 0 AS P_Q_TIME
                                , SYSDATE AS IO_DATE
                                , a.RESOURCE_NO
                                , '' AS NAME
                                , 2 AS P_TYPE
                                , NULL AS SEND_YN
                                , NULL AS R_YN
                                , NULL AS C_CONDITION
                                , SYSDATE AS I_TIME
                                , 'N' AS ERP_UP_YN
                        FROM WORK_ORDER a
                            LEFT JOIN (
                                        SELECT b.IDNRK
                                                , b.MENGE
                                                , b.RESOURCE_NO
                                                , a.BATCH
                                        FROM WORK_ORDER a
                                            INNER JOIN SAP_IN_BOM_COND b
                                                ON b.PLANT_CODE = a.PLANT_CODE
                                                AND b.RESOURCE_NO = a.RESOURCE_NO
                                                AND b.NOTE = a.NOTE
                                                AND b.P_TYPE = '1'
                                        WHERE a.PLANT_CODE = '{sPlant_Code}'
                                            AND a.PROCESS_KEY = '{sProcess_Code}'
                                            AND a.L_CODE = '{sLine_Code}'
                                            AND a.WORKDATE = '{sWORKDATE}'
                                            AND a.NUM = '{sNum}'
                                        ) d
                            ON d.RESOURCE_NO = a.RESOURCE_NO
                        WHERE a.PLANT_CODE = '{sPlant_Code}'
                            AND a.PROCESS_KEY = '{sProcess_Code}'
                            AND a.L_CODE = '{sLine_Code}'
                            AND a.WORKDATE = '{sWORKDATE}'
                            AND a.NUM = '{sNum}'
                        ) s
                ON (
                        d.PLANT_CODE = s.PLANT_CODE
                    AND d.PROCESS_KEY = s.PROCESS_KEY
                    AND d.L_CODE = s.L_CODE
                    AND d.WORKDATE = s.WORKDATE
                    AND d.NUM = s.NUM
                    AND d.BATCH = s.BATCH
                    AND d.SEQ = s.SEQ
                )
                WHEN NOT MATCHED THEN
                    INSERT (
                            PLANT_CODE
                        , PROCESS_KEY
                        , L_CODE
                        , WORKDATE
                        , NUM
                        , BATCH
                        , SEQ
                        , IO_GUBUN
                        , LOCATION
                        , INGRED_LOT
                        , P_Q
                        , P_Q_TIME
                        , IO_DATE
                        , RESOURCE_NO
                        , NAME
                        , P_TYPE
                        , SEND_YN
                        , R_YN
                        , C_CONDITION
                        , I_TIME
                        , ERP_UP_YN
                    )
                    VALUES (
                            s.PLANT_CODE
                        , s.PROCESS_KEY
                        , s.L_CODE
                        , s.WORKDATE
                        , s.NUM
                        , s.BATCH
                        , s.SEQ
                        , s.IO_GUBUN 
                        , s.LOCATION
                        , s.INGRED_LOT
                        , s.P_Q
                        , s.P_Q_TIME
                        , s.IO_DATE
                        , s.RESOURCE_NO
                        , s.NAME
                        , s.P_TYPE
                        , s.SEND_YN
                        , s.R_YN
                        , s.C_CONDITION
                        , SYSDATE
                        , s.ERP_UP_YN
                    )
                ";

                Dbconn.conn.SQLrun(SQL);

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "SetSAPWorkRemark", ex);
                return false;
            }
        }

        /// <summary>
        /// SAP 공정 생성
        /// </summary>
        /// <param name="sPlant_Code"></param>
        /// <param name="sProcess_Code"></param>
        /// <param name="sLine_Code"></param>
        /// <param name="sWORKDATE"></param>
        /// <param name="sNum"></param>
        public static bool SetSAPWorkRemark(string sPlant_Code, string sProcess_Code, string sLine_Code, string sWORKDATE, string sNum, string sRBatch = "", int iRatio = 100)
        {
            try
            {
                if (iRatio > 100)
                    iRatio = 100;

                if (sPlant_Code == "P201")
                {
                    return SetSAPWorkRemark(clsCommon.PlantCode, sProcess_Code, sLine_Code, sWORKDATE, sNum, sRBatch);
                }
                else
                {
                    string SQL = $@"
                    MERGE INTO WORK_REMARK target
                    USING (
                        SELECT
                            PLANT_CODE
                            , 'SAP_' || PROCESS_KEY AS PROCESS_KEY
                            , L_CODE
                            , WORKDATE
                            , NUM
                            , BATCH
                            , MAX(SEQ) AS SEQ
                            , 'I' AS IO_GUBUN
                             , MAX(
                                    CASE 
                                        WHEN LOCATION = 'H' THEN 0
                                        WHEN REGEXP_LIKE(LOCATION, '^[0-9]+$') THEN TO_NUMBER(LOCATION)
                                        ELSE 0
                                    END
                                ) AS LOCATION
                            , SUM(P_Q) AS P_Q
                            , SYSDATE AS IO_DATE
                            , RESOURCE_NO
                            , INGRED_CODE
                            , '2' AS P_TYPE
                            , SYSDATE AS I_TIME
                            , '0' AS P_Q_TIME
                        FROM (
                        -- 본배합비
                        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, TO_NUMBER(NVL('{sRBatch}', wr.BATCH)) AS BATCH, wr.SEQ, wr.LOCATION
                            , wr.INGRED_LOT AS INGRED_CODE
                            , CASE WHEN (wd.RC_YN = 'Y' AND wo.PLANT_CODE IN ('PJ01', 'PJ02', 'PJ04', 'PJ05')) 
                                    OR wr.PROCESS_KEY IN ('{clsCommon.GetProcessKey("후레이크")}', '{clsCommon.GetProcessKey("익스콘")}', '{clsCommon.GetProcessKey("벌크 원료")}') 
                                    THEN wr.P_Q ELSE DECODE('{iRatio}', 100, wd.SET_VAL, wd.SET_VAL * ('{iRatio}' / 100)) 
                                END P_Q
                        FROM WORK_ORDER wo
                            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM AND ('{sRBatch}' IS NULL OR wr.BATCH = 1)
                            JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wr.PLANT_CODE AND wd.PROCESS_KEY = wr.PROCESS_KEY AND wd.L_CODE = wr.L_CODE
                                    AND wd.WORKDATE = wr.WORKDATE AND wd.NUM = wr.NUM
                                    AND wd.LOCATION = wr.LOCATION AND wd.PARENT_BIN IS NULL
                                    AND wd.INGRED_CODE = wr.INGRED_LOT
                            LEFT JOIN SAP_IN_PRODUCT_RC rc ON rc.PLANT_CODE = wo.PLANT_CODE AND rc.RESOURCE_NO = wr.INGRED_LOT
                        WHERE wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
                            AND wr.INGRED_LOT NOT IN (
                                                        SELECT RESOURCE_NO_2
                                                        FROM SAP_IN_UPPRODUCT_CP x
                                                        WHERE x.PLANT_CODE = wo.PLANT_CODE AND x.RESOURCE_NO = wo.RESOURCE_NO AND x.RESOURCE_NO_2 = wr.INGRED_LOT AND NVL(x.SAP_UP_YN, 'X') != 'Y'
                                                        --UNION ALL
                                                        --SELECT RESOURCE_NO_3
                                                        --FROM SAP_IN_PRODUCT_CP x
                                                        --WHERE x.PLANT_CODE = wo.PLANT_CODE AND x.RESOURCE_NO = wo.RESOURCE_NO AND x.RESOURCE_NO_3 = wr.INGRED_LOT
                                                        UNION ALL
                                                        SELECT RESOURCE_NO
                                                        FROM SAP_IN_PRODUCT_CH x
                                                        WHERE x.PLANT_CODE = wo.PLANT_CODE AND x.RESOURCE_NO = wr.INGRED_LOT
                                                        UNION ALL
                                                        SELECT RESOURCE_NO
                                                        FROM SAP_DI_PRODUCT x
                                                        WHERE x.PLANT_CODE  = wo.PLANT_CODE AND wr.INGRED_LOT LIKE 'P%' AND x.RESOURCE_NO = wr.INGRED_LOT
                                                        UNION ALL
                                                        SELECT RESOURCE_NO
                                                        FROM SAP_DI_PRODUCT x
                                                        WHERE x.PLANT_CODE  = wo.PLANT_CODE AND x.RESOURCE_NO = wr.INGRED_LOT AND ((x.PLANT_CODE IN ('PJ01', 'PJ02', 'PJ04', 'PJ05') AND x.RESOURCE_NO IN (SELECT COMM_DTCODE FROM COMM_DTCODE WHERE WK_DIVCODE = '03' AND COMM_CODE = '88')) OR (x.RESOURCE_TYPE = 'UNBW'))
                                                        UNION ALL
                                                        SELECT RESOURCE_NO_2
                                                        FROM SAP_IN_PRODUCT_RC x
                                                        WHERE x.PLANT_CODE  = wo.PLANT_CODE AND x.RESOURCE_NO = wr.RESOURCE_NO AND NVL(x.SAP_UP_YN, 'N') != 'Y'
                                                    )
                        UNION ALL
                        -- 원료 대체비율
                        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, TO_NUMBER(NVL('{sRBatch}', wr.BATCH)) AS BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
                            ,  cp.RESOURCE_NO_2 AS INGRED_CODE, DECODE('{iRatio}', 100, (SUM(wd.SET_VAL) * (cp.PART_P / 100)), (SUM(wd.SET_VAL) * (cp.PART_P / 100)) * ('{iRatio}' / 100))
                        FROM WORK_ORDER wo
                            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM AND ('{sRBatch}' IS NULL OR wr.BATCH = 1)
                            JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wr.PLANT_CODE AND wd.PROCESS_KEY = wr.PROCESS_KEY AND wd.L_CODE = wr.L_CODE
                                    AND wd.WORKDATE = wr.WORKDATE AND wd.NUM = wr.NUM
                                    AND wd.LOCATION = wr.LOCATION AND wd.PARENT_BIN IS NULL
                                    AND wd.INGRED_CODE = wr.INGRED_LOT
                            JOIN SAP_IN_PRODUCT_CH cp ON cp.PLANT_CODE = wo.PLANT_CODE AND cp.RESOURCE_NO = wr.INGRED_LOT
                        WHERE wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
                        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH,  cp.RESOURCE_NO_2, cp.PART_P
                        UNION ALL
                        -- 프리믹스 배합비
                        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, TO_NUMBER(NVL('{sRBatch}', wr.BATCH)) AS BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
                            ,  cond.IDNRK AS INGRED_CODE, DECODE('{iRatio}', 100, (SUM(wo.BATCH_Q) * (cond.MENGE / 100)), (SUM(wo.BATCH_Q) * (cond.MENGE / 100)) * ('{iRatio}' / 100))
                        FROM WORK_ORDER wo
                            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM AND ('{sRBatch}' IS NULL OR wr.BATCH = 1)
                            JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wr.PLANT_CODE AND wd.PROCESS_KEY = wr.PROCESS_KEY AND wd.L_CODE = wr.L_CODE
                                    AND wd.WORKDATE = wr.WORKDATE AND wd.NUM = wr.NUM
                                    AND wd.LOCATION = wr.LOCATION AND wd.PARENT_BIN IS NULL
                                    AND wd.INGRED_CODE = wr.INGRED_LOT
                            JOIN SAP_IN_BOM_COND cond ON cond.PLANT_CODE = wo.PLANT_CODE AND wr.INGRED_LOT LIKE 'P%' AND cond.RESOURCE_NO = wr.INGRED_LOT AND cond.NOTE = wo.NOTE AND cond.P_TYPE = '1'
                        WHERE wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
                        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH,  cond.IDNRK, cond.MENGE
                        UNION ALL
                        -- 프리믹스 대체비율
                        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, TO_NUMBER(NVL('{sRBatch}', wr.BATCH)) AS BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
                                ,  cp.RESOURCE_NO_2 AS INGRED_CODE, DECODE('{iRatio}', 100, SUM(wd.SET_VAL), SUM(wd.SET_VAL) * ('{iRatio}' / 100))
                        FROM WORK_ORDER wo
                            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM AND ('{sRBatch}' IS NULL OR wr.BATCH = 1)
                            JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wr.PLANT_CODE AND wd.PROCESS_KEY = wr.PROCESS_KEY AND wd.L_CODE = wr.L_CODE
                                    AND wd.WORKDATE = wr.WORKDATE AND wd.NUM = wr.NUM
                                    AND wd.LOCATION = wr.LOCATION AND wd.PARENT_BIN IS NULL
                                    AND wd.INGRED_CODE = wr.INGRED_LOT
                            JOIN SAP_IN_BOM_COND cond ON cond.PLANT_CODE = wo.PLANT_CODE AND wr.INGRED_LOT LIKE 'P%' AND cond.RESOURCE_NO = wr.INGRED_LOT AND cond.NOTE = wo.NOTE AND cond.P_TYPE = '1'
                            JOIN SAP_IN_PRODUCT_CP cp ON cp.PLANT_CODE = wo.PLANT_CODE AND cp.RESOURCE_NO = wr.RESOURCE_NO AND cp.RESOURCE_NO_3 = cond.IDNRK
                        WHERE wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
                        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH,  cp.RESOURCE_NO_2
                        UNION ALL
                        -- 프리믹스 원료 대체비율
                        SELECT wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, TO_NUMBER(NVL('{sRBatch}', wr.BATCH)) AS BATCH, MIN(wr.SEQ) AS SEQ, MAX(wr.LOCATION) AS LOCATION
                                ,  cp.RESOURCE_NO_2 AS INGRED_CODE, DECODE('{iRatio}', 100, (SUM(wd.SET_VAL) * (cp.PART_P / 100)), (SUM(wd.SET_VAL) * (cp.PART_P / 100)) * ('{iRatio}' / 100))
                        FROM WORK_ORDER wo
                            JOIN WORK_REMARK wr ON wr.PLANT_CODE = wo.PLANT_CODE AND wr.PROCESS_KEY LIKE '%' || wo.PROCESS_KEY AND wr.L_CODE = wo.L_CODE
                                    AND wr.WORKDATE = wo.WORKDATE AND wr.NUM = wo.NUM AND ('{sRBatch}' IS NULL OR wr.BATCH = 1)
                            JOIN WORK_DETAIL wd ON wd.PLANT_CODE = wr.PLANT_CODE AND wd.PROCESS_KEY = wr.PROCESS_KEY AND wd.L_CODE = wr.L_CODE
                                    AND wd.WORKDATE = wr.WORKDATE AND wd.NUM = wr.NUM
                                    AND wd.LOCATION = wr.LOCATION AND wd.PARENT_BIN IS NULL
                                    AND wd.INGRED_CODE = wr.INGRED_LOT
                            JOIN SAP_IN_BOM_COND cond ON cond.PLANT_CODE = wo.PLANT_CODE AND wr.INGRED_LOT LIKE 'P%' AND cond.RESOURCE_NO = wr.INGRED_LOT AND cond.NOTE = wo.NOTE AND cond.P_TYPE = '1'
                            JOIN SAP_IN_PRODUCT_CH cp ON cp.PLANT_CODE = wo.PLANT_CODE AND cp.RESOURCE_NO = cond.IDNRK
                        WHERE wo.PLANT_CODE = '{sPlant_Code}' AND wr.PROCESS_KEY = '{sProcess_Code}' AND wo.L_CODE = '{sLine_Code}' AND wo.WORKDATE = '{sWORKDATE}' AND wo.NUM = '{sNum}'
                        GROUP BY wo.PLANT_CODE, wr.PROCESS_KEY, wo.L_CODE, wo.WORKDATE, wo.NUM, wo.RESOURCE_NO, wo.NOTE, wr.BATCH,  cp.RESOURCE_NO_2, cp.PART_P
                        ORDER BY BATCH, SEQ
                        )
                        GROUP BY PLANT_CODE, PROCESS_KEY, L_CODE, WORKDATE, NUM, BATCH, RESOURCE_NO, INGRED_CODE
                    ) src
                    ON (
                        target.PLANT_CODE = src.PLANT_CODE AND
                        target.PROCESS_KEY = src.PROCESS_KEY AND
                        target.L_CODE = src.L_CODE AND
                        target.WORKDATE = src.WORKDATE AND
                        target.NUM = src.NUM AND
                        target.BATCH = src.BATCH AND
                        target.SEQ = src.SEQ AND
                        target.IO_GUBUN = src.IO_GUBUN
                        AND target.INGRED_LOT = src.INGRED_CODE
                    )
                    WHEN MATCHED THEN
                        UPDATE SET
                            target.LOCATION     = src.LOCATION
                            , target.P_Q          = src.P_Q
                            , target.IO_DATE      = src.IO_DATE
                            , target.RESOURCE_NO  = src.RESOURCE_NO
                            , target.P_TYPE       = src.P_TYPE
                            , target.I_TIME       = src.I_TIME
                            , target.P_Q_TIME     = src.P_Q_TIME
                    WHEN NOT MATCHED THEN
                        INSERT (
                            PLANT_CODE
                            , PROCESS_KEY
                            , L_CODE
                            , WORKDATE
                            , NUM
                            , BATCH
                            , SEQ
                            , IO_GUBUN
                            , LOCATION
                            , P_Q
                            , IO_DATE
                            , RESOURCE_NO
                            , INGRED_LOT
                            , P_TYPE
                            , I_TIME
                            , P_Q_TIME
                        )
                        VALUES (
                            src.PLANT_CODE
                            , src.PROCESS_KEY
                            , src.L_CODE
                            , src.WORKDATE
                            , src.NUM
                            , src.BATCH
                            , src.SEQ
                            , src.IO_GUBUN
                            , src.LOCATION
                            , src.P_Q
                            , src.IO_DATE
                            , src.RESOURCE_NO
                            , src.INGRED_CODE
                            , src.P_TYPE
                            , src.I_TIME
                            , src.P_Q_TIME
                        )
                    ";

                    Dbconn.conn.SQLrun(SQL);

                    return true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "SetSAPWorkRemark", ex);
                return false;
            }
        }

        public static bool InsertWorkRemarkBatch(string plantCode, string processCd, string lCode, string workdate, string num, string batch)
        {
            try
            {
                String w_num = num;
                String wRESOURCE_NO = "";
                String w_note = "";
                String w_batch_q = "";

                string SQL = string.Empty;

                int last_batch = 0;

                SQL = "SELECT RESOURCE_NO, R_BATCH, BATCH, BATCH_Q, NOTE FROM WORK_ORDER WHERE PROCESS_KEY = '{0}' AND WORKDATE = '{1}' AND NUM = '{2}' ";
                SQL = string.Format(SQL,
                    processCd,
                    workdate,
                    num
                    );

                DataSet workDs = Dbconn.conn.ExecutDataset(SQL);
                if (Dbconn.conn.getRowCnt(workDs) > 0)
                {
                    last_batch = Convert.ToInt16(Dbconn.conn.getData(workDs, "BATCH", 0));
                    wRESOURCE_NO = Dbconn.conn.getData(workDs, "RESOURCE_NO", 0);
                    w_note = Dbconn.conn.getData(workDs, "NOTE", 0);
                    w_batch_q = Dbconn.conn.getData(workDs, "BATCH_Q", 0);
                }

                DataSet resultMixDs = clsProcessDosing.resultWorkResult(plantCode, processCd, lCode, workdate, num);

                int mixReusltCnt = Dbconn.conn.getRowCnt(resultMixDs);
                if (mixReusltCnt > 0)
                {
                    int hand_cnt = 1;
                    for (int j = 0; j < mixReusltCnt; j++)
                    {
                        string BMENG = Dbconn.conn.getData(resultMixDs, "BMENG", j);
                        string RESOURCE_NO = Dbconn.conn.getData(resultMixDs, "RESOURCE_NO", j);
                        string description = Dbconn.conn.getData(resultMixDs, "DESCRIPTiON", j);
                        string SET_VAL = Dbconn.conn.getData(resultMixDs, "SET_VAL", j);
                        string location = Dbconn.conn.getData(resultMixDs, "LOCATION", j);

                        if (string.IsNullOrEmpty(location))
                        {
                            location = "H" + hand_cnt.ToString();
                            hand_cnt = hand_cnt + 1;
                        }

                        SQL = "DELETE FROM WORK_REMARK WHERE PRO PROCESS_KEY = '{0}' WORKDATE = '{1}' NUM = '{2}' BATCH = '{3}' ";

                        //SQL = $@"
                        //    "INSERT INTO WORK_REMARK
                        //    "(PROCESS_KEY, WORKDATE, NUM, BATCH, IO_GUBUN, LOCATION, P_Q, IO_DATE, INGRED_CODE, [NAME],
                        //    " P_TYPE, I_TIME) 
                        //    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', SYSDATE, '{7}', '{8}',
                        //    " '{9}', SYSDATE ) ";

                        SQL = string.Format(SQL,
                            processCd,
                            workdate,
                            num,
                            batch,
                            "I",
                            location,
                            SET_VAL,
                            RESOURCE_NO,
                            description,
                            "I"
                            );

                        if (Dbconn.conn.SQLrun(SQL) < 1)
                        {
                            clsLog.logSave("clsProcesssDosing", "InsertWorkRemark", SQL);
                            return false;
                        }
                    }

                }
                else if (mixReusltCnt == 0)
                {
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsProcessDosing", "InsertWorkRemark", ex);
                return false;
            }
        }
    }
}
