using Core.Enum;
using System;
using System.Data;

namespace Core.Class
{
    public class clsCarProcess
    {

        /// <summary>
        /// 입차프로세스
        /// </summary>
        /// <param name="full_car_num">차량번호</param>
        /// <param name="in_weight">입차계근량</param>
        public static string InWeightProcess(string full_car_num, string in_weight, string is_no)
        {
            try
            {
                string SQL = string.Empty;

                full_car_num = full_car_num.Replace(" ", "").Trim();

                SQL = $@"
                UPDATE WAP_DECAR
                SET PC_STATUS = '9'
                WHERE INCAR_NO LIKE '%{full_car_num.Substring(full_car_num.Length - 4, 4)}'
                    AND PC_STATUS IN ('0', '1')
                    AND CONVERT(CHAR(8), CHKIN_TIME, 112) < '{DateTime.Now.ToString("yyyyMMdd")}'
                ";
                
                Dbconn.conn.SQLrun(SQL);

                SQL = $"SELECT IS_NO, CUST_CODE, INCAR_NO, VEHICLEGROUPCODE  FROM WAP_DECAR WHERE IS_NO = '{is_no}'  ";

                DataSet inChkDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(inChkDs) > 0)
                {
                    string in_is_no = Dbconn.conn.getData(inChkDs, "IS_NO", 0).Trim();
                    string in_cust_cd = Dbconn.conn.getData(inChkDs, "CUST_CODE", 0).Trim();
                    string in_car_num = Dbconn.conn.getData(inChkDs, "INCAR_NO", 0).Trim();
                    string in_VEHICLEGROUPCODE = Dbconn.conn.getData(inChkDs, "VEHICLEGROUPCODE", 0).Trim();

                    

                    SQL = $@"
                    UPDATE WAP_DECAR
                    SET INCAR_NO = '{full_car_num}', IN_WEIGHT = '{in_weight}'
                        , INCAR_DATE = SYSDATE, INCAR_TIME = SYSDATE
                        , PC_STATUS = '1'
                    WHERE IS_NO = '{in_is_no}'
                    ";
                    
                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        clsLog.logSave("clsCarProcess", "InWeightProcess", SQL);
                        return "차량정보를 업데이트 도중 에러가 발생했습니다";
                    }

/*                    try
                    {
                        XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0003", "1");
                    }
                    catch (Exception)
                    {
                        XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0003", "1");
                    }
*/
                    return "OK";
                }
                else
                {
                    return "입차등록이 미등록되거나 이미 입차 처리된 차량입니다";

                }

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsCarProcess", "InWeightProcess", ex);
                clsLog.logSave("clsCarProcess", "InWeightProcess", ex.StackTrace);
                clsLog.logSave("clsCarProcess", "InWeightProcess", ex.Source);
            }

            return "입차처리중 에러가 발생했습니다";
        }


        public static string outChkProcess(string full_car_num, string out_weight, string is_no)
        {
            try
            {
                string SQL = string.Empty;

                SQL = $@"
                SELECT IS_NO, CUST_CODE, INCAR_NO, VEHICLEGROUPCODE, CAR_TDETAIL, FLOOR(IN_WEIGHT) as IN_WEIGHT, INCAR_TIME
                FROM WAP_DECAR
                WHERE PC_STATUS = '1' AND IS_NO = '{is_no}'
                ";

                DataSet inChkDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(inChkDs) == 0)
                {
                    return "차량의 입차정보를 찾을 수 없습니다";
                }

                string in_is_no = Dbconn.conn.getData(inChkDs, "IS_NO", 0).Trim();
                string in_cust_cd = Dbconn.conn.getData(inChkDs, "CUST_CODE", 0).Trim();
                string in_car_num = Dbconn.conn.getData(inChkDs, "INCAR_NO", 0).Trim();
                string in_VEHICLEGROUPCODE = Dbconn.conn.getData(inChkDs, "VEHICLEGROUPCODE", 0).Trim();
                string in_weight = Dbconn.conn.getData(inChkDs, "IN_WEIGHT", 0).Trim();
                string in_car_tdetail = Dbconn.conn.getData(inChkDs, "CAR_TDETAIL", 0).Trim();


                if (in_VEHICLEGROUPCODE == "Y")
                {
                    if (in_car_tdetail == "벌크")
                    {
                        string beforWeight = string.Empty;
                        string preWeight = string.Empty;
                        string beforWeightTIme = string.Empty;

                        preWeight = out_weight;

                        SQL = $@"
                        SELECT PROCESS_KEY, WO_NUMBER, WORK_SEQ, WEIGHT, ERP_UP_YN
                        FROM BULK_ORDER WHERE PROCESS_KEY IN ('P06', 'P07') AND IS_NO = '{in_is_no}' AND PC_STATUS IN ('1','2')
                                AND C_CONDITION IN ('{clsCommon.PcStatus.Completed}', '{clsCommon.PcStatus.ForceCompleted}') AND CONVERT(CHAR(8), START_TIME , 112) = '{DateTime.Now.ToString("yyyyMMdd")}'
                                AND RIGHT(PART_NO,1) IN ('2','3')
                        ORDER BY START_TIME DESC
                        ";

                        clsLog.logSave(SQL, 0);
                        DataSet bulkDs = Dbconn.conn.ExecutDataset(SQL);

                        string erp_loc = "B02";

                        if (Dbconn.conn.getRowCnt(bulkDs) == 0)
                        {
                            return "차량의 입차정보를 찾을 수 없습니다";
                        }
                        else if (Dbconn.conn.getRowCnt(bulkDs) > 0)
                        {
                            string erp_up_yn = Dbconn.conn.getData(bulkDs, "ERP_UP_YN", 0);

                            if (erp_up_yn == "Y")
                            {
                                return "ERP에 업로드처리 된 상차지시입니다";
                            }

                            //차량소독여부전송
                            SQL = $@"
                            SELECT PROCESS_KEY, WO_NUMBER, WORK_SEQ, WEIGHT
                            FROM BULK_ORDER WHERE PROCESS_KEY IN ('P06', 'P07') AND IS_NO = '{in_is_no}' AND PC_STATUS = '1'
                                AND C_CONDITION <> '{clsCommon.PcStatus.Completed}' AND CONVERT(CHAR(8), START_TIME , 112) = '{DateTime.Now.ToString("yyyyMMdd")}'
                            ORDER BY START_TIME DESC ";

                            DataSet bulkDisinfectChkDs = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(bulkDisinfectChkDs) == 0)
                            {
                                //차량소독여부전송
                                //XGT_PLC.Tcp_Plc.PLC_Write_Word(clsCommon.plc_scale_ip, "%MX0002", "1");
                                //XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0002", "1");
                            }



                            SQL = $@"
                            SELECT PROCESS_KEY, WO_NUMBER, WORK_SEQ, PART_NO, WEIGHT, WEIGHT_TIME
                            FROM BULK_ORDER
                            WHERE PROCESS_KEY IN ('{clsCommon.GetProcessKey( "벌크")}', '{clsCommon.GetProcessKey("타이콘")}')
                                  AND RIGHT(PART_NO,1) IN ('2','3')  AND IS_NO = '{in_is_no}' AND WEIGHT IS NOT NULL AND PC_STATUS = '2'
                            ORDER BY WEIGHT_TIME DESC, START_TIME DESC
                            ";
                            
                            clsLog.logSave(SQL, 0);
                            DataSet bulkbeforDs = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(bulkbeforDs) == 0)
                            {
                                beforWeight = Dbconn.conn.getData(inChkDs, "IN_WEIGHT", 0);
                                beforWeightTIme = Convert.ToDateTime(Dbconn.conn.getData(inChkDs, "INCAR_TIME", 0)).ToString("yyyy-MM-dd HH:mm:ss");


                                //벌크,지대 혼합차량시 벌크상차 전 지대완료 되었는지 확인
                                SQL = $@"
                                SELECT RIGHT(PART_NO,1)
                                FROM BULK_ORDER
                                WHERE IS_NO = '{in_is_no}' AND RIGHT(PART_NO,1) IN ('1','3')
                                GROUP BY RIGHT(PART_NO,1)
                                ";

                                DataSet cntDs = Dbconn.conn.ExecutDataset(SQL);
                                if (Dbconn.conn.getRowCnt(cntDs) > 1)
                                {
                                    SQL = $@"
                                    SELECT PROCESS_KEY, WO_NUMBER, WORK_SEQ, FLOOR(BATCH_Q) AS BATCH_Q, QUANTITY
                                    FROM BULK_ORDER WHERE IS_NO = '{in_is_no}'
                                    AND RIGHT(PART_NO,1) = '1' AND C_CONDITION = '{clsCommon.PcStatus.Plan}'
                                    ";

                                    clsLog.logSave(SQL, 0);
                                    DataSet bulkChkDs = Dbconn.conn.ExecutDataset(SQL);

                                    if (Dbconn.conn.getRowCnt(bulkChkDs) > 0)
                                    {
                                        return "지대 상차가 선진행이 안되었습니다";
                                    }

                                    if (Dbconn.conn.getRowCnt(bulkChkDs) == 0)
                                    {
                                        SQL = $@"
                                        SELECT PROCESS_KEY, WO_NUMBER, WORK_SEQ, FLOOR(BATCH_Q) AS BATCH_Q, QUANTITY
                                        FROM BULK_ORDER
                                        WHERE IS_NO = '{in_is_no}' AND RIGHT(PART_NO,1) = '1' AND WEIGHT IS NULL
                                        ";
                                        
                                        clsLog.logSave(SQL, 0);
                                        DataSet packChkDs = Dbconn.conn.ExecutDataset(SQL);

                                        if (Dbconn.conn.getRowCnt(packChkDs) == 0)
                                        {
                                            return "지대 상차 계근량이 이미 입력처리 되어있습니다";
                                        }
                                        else if (Dbconn.conn.getRowCnt(packChkDs) > 0)
                                        {
                                            string tmpWeight = beforWeight;

                                            int tmpSumWeight = Convert.ToInt32(tmpWeight);

                                            for (int i = 0; i < Dbconn.conn.getRowCnt(packChkDs); i++)
                                            {
                                                string pProcessKey = Dbconn.conn.getData(packChkDs, "PROCESS_KEY", i);
                                                string pWoNum = Dbconn.conn.getData(packChkDs, "WO_NUMBER", i);
                                                string pWoSeq = Dbconn.conn.getData(packChkDs, "WORK_SEQ", i);
                                                string pBatchQ = Dbconn.conn.getData(packChkDs, "BATCH_Q", i);
                                                string pQty = Dbconn.conn.getData(packChkDs, "QUANTITY", i);

                                                SQL = $@"
                                                UPDATE BULK_ORDER
                                                SET PC_STATUS = '2', BEFORE_WEIGHT = '{tmpSumWeight}'
                                                    , WEIGHT = '{(tmpSumWeight + Convert.ToInt32(pBatchQ))}', WEIGHT_TIME = SYSDATE
                                                    , ERP_QTY = '{pQty}', ERP_WEIGHT = '{pBatchQ}'
                                                    , ERP_LOCATION = 'F01'
                                                    , BEFORE_WEIGHT_TIME = SYSDATE
                                                WHERE PROCESS_KEY = '{pProcessKey}' AND WO_NUMBER = '{pWoNum}' AND WORK_SEQ = '{pWoSeq}'
                                                ";

                                                clsLog.logSave(SQL, 0);
                                                Dbconn.conn.SQLrun(SQL);
                                                tmpSumWeight = tmpSumWeight + Convert.ToInt32(pBatchQ);

                                            } //for

                                            //지대무계만큼 입차계근량 합산처리
                                            beforWeight = Convert.ToString(tmpSumWeight);

                                        }
                                    }
                                }
                            }
                            else
                            {

                                string part_no = Dbconn.conn.getData(bulkbeforDs, "PART_NO", 0).Trim();

                                if (part_no.Substring(part_no.Length - 1, 1) == "2")
                                {
                                    erp_loc = "F01";

                                    SQL = $@"
                                    SELECT  *
                                    FROM BULK_ORDER WHERE PROCESS_KEY IN ('P07') AND IS_NO = '{in_is_no}'
                                        AND PC_STATUS = '2' AND WEIGHT IS NOT NULL
                                    ";
                                   
                                    clsLog.logSave(SQL, 0);
                                    DataSet bagbeforDs = Dbconn.conn.ExecutDataset(SQL);
                                    if (Dbconn.conn.getRowCnt(bagbeforDs) == 0)
                                    {
                                        beforWeight = Dbconn.conn.getData(inChkDs, "IN_WEIGHT", 0);
                                        beforWeightTIme = Convert.ToDateTime(Dbconn.conn.getData(inChkDs, "INCAR_TIME", 0)).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    else
                                    {
                                        beforWeight = Dbconn.conn.getData(bulkbeforDs, "WEIGHT", 0);
                                        beforWeightTIme = Convert.ToDateTime(Dbconn.conn.getData(bulkbeforDs, "WEIGHT_TIME", 0)).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                }
                                else
                                {
                                    beforWeight = Dbconn.conn.getData(bulkbeforDs, "WEIGHT", 0);
                                    beforWeightTIme = Convert.ToDateTime(Dbconn.conn.getData(bulkbeforDs, "WEIGHT_TIME", 0)).ToString("yyyy-MM-dd HH:mm:ss");

                                }
                            }
                        }


                        int actualWeight = Convert.ToInt32(preWeight) - Convert.ToInt32(beforWeight);


                        SQL = $@"
                        UPDATE BULK_ORDER SET PC_STATUS = '2', BEFORE_WEIGHT = '{beforWeight}'
                                , WEIGHT = '{preWeight}', WEIGHT_TIME = SYSDATE
                                ,ERP_QTY = '{actualWeight}', ERP_WEIGHT = '{actualWeight}'
                                , ERP_LOCATION = '{erp_loc}',
                        BEFORE_WEIGHT_TIME = CONVERT(DATETIME, '{beforWeightTIme}')
                        WHERE PROCESS_KEY = '{Dbconn.conn.getData(bulkDs, "PROCESS_KEY", 0)}' 
                            AND WO_NUMBER = '{Dbconn.conn.getData(bulkDs, "WO_NUMBER", 0)}'
                            AND WORK_SEQ = '{Dbconn.conn.getData(bulkDs, "WORK_SEQ", 0)}'
                        ";

                        clsLog.logSave(SQL, 0);

                        Dbconn.conn.SQLrun(SQL);

                    }
                    else if (in_car_tdetail == "카고")
                    {
                        SQL = $"SELECT PC_STATUS FROM WAP_DECAR WHERE IS_NO = '{in_is_no}' ";

                        DataSet statusChkDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(statusChkDs) > 0)
                        {
                            if (Dbconn.conn.getData(statusChkDs, "PC_STATUS", 0).Trim() == "1")
                            {
                                SQL = $"UPDATE WAP_DECAR SET PC_STATUS = '2' WHERE IS_NO = '{in_is_no}' ";

                                Dbconn.conn.SQLrun(SQL);
                            }
                        }

                        //차량소독여부전송
                        //XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0002", "1");
                    }
                    else
                    {
                        //차량소독여부전송
                        //XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0002", "1");
                    }

                    SQL = $@"
                    UPDATE WAP_DECAR
                    SET OUT_WEIGHT = '{out_weight}', OUTCAR_DATE = SYSDATE, OUTCAR_TIME = SYSDATE
                    WHERE IS_NO = '{in_is_no}' ";

                    Dbconn.conn.SQLrun(SQL);

                }
                else if (in_VEHICLEGROUPCODE == "N")
                {
                    string beforWeight = string.Empty;
                    string preWeight = string.Empty;
                    string beforWeightTIme = string.Empty;

                    SQL = $@"
                    SELECT IS_NO, INGRED_CODE, SPIV_CAR_WEIGHT, PC_STATUS
                    FROM WAP_GOCAR where IS_NO = '{in_is_no}' AND PC_STATUS IN ('0','1')
                        AND CONVERT(CHAR(8), I_TIME , 112) = '{DateTime.Now.ToString("yyyyMMdd")}'
                    ORDER BY PC_STATUS DESC
                    ";
                    
                    DataSet ingWeightChkDs = Dbconn.conn.ExecutDataset(SQL);

                    string SQL_WHERE = string.Empty;

                    if (Dbconn.conn.getRowCnt(ingWeightChkDs) == 0)
                    {
                        return "원료입고내역을 찾을 수 없습니다";
                    }
                    else if (Dbconn.conn.getRowCnt(ingWeightChkDs) > 0)
                    {
                        string ing_cd = Dbconn.conn.getData(ingWeightChkDs, "INGRED_CODE", 0);
                        string pc_status = Dbconn.conn.getData(ingWeightChkDs, "PC_STATUS", 0);
                        string weight_type = string.Empty;

                        SQL = $"SELECT WEIGHT_TYPE FROM INGRED WHERE RESOURCE_NO = '{ing_cd}' ";

                        DataSet weightTypeDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(weightTypeDs) == 1)
                        {
                            //계량타입 가져오기 (01 : 송장량, 02: 계근량)
                            weight_type = Dbconn.conn.getData(weightTypeDs, "WEIGHT_TYPE", 0);
                        }

                        if (weight_type == "02")
                        {
                            //원료입고상태가 없을 경우
                            if (pc_status == "0")
                            {
                                //사이로원료인지 확인 처리 후 ERP저장위치 공정창고로 변경
                                SQL = $"SELECT * FROM BIN WHERE PROCESS_KEY = 'P01' AND RESOURCE_NO = '{ing_cd}' ";

                                DataSet tmpBinSearchDs = Dbconn.conn.ExecutDataset(SQL);

                                if (Dbconn.conn.getRowCnt(tmpBinSearchDs) > 0)
                                {
                                    //사이로 원료일 경우 공정창고
                                    SQL_WHERE = "P01";
                                }
                                else
                                {
                                    //그외는 제품창고
                                    SQL_WHERE = "R01";
                                }
                            }

                            SQL = $@"
                            SELECT WEIGHT, WEIGHT_TIME
                            FROM WAP_GOCAR WHERE WEIGHT IS NOT NULL AND IS_NO = '{in_is_no}'
                                AND PC_STATUS = '2'
                            ORDER BY WEIGHT_TIME DESC
                            ";

                            DataSet beforWeightDs = Dbconn.conn.ExecutDataset(SQL);

                            //계근내역이 없을 경우
                            if (Dbconn.conn.getRowCnt(beforWeightDs) == 0)
                            {
                                beforWeight = Dbconn.conn.getData(inChkDs, "IN_WEIGHT", 0);
                                beforWeightTIme = Convert.ToDateTime(Dbconn.conn.getData(inChkDs, "INCAR_TIME", 0)).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else //계근내역이 있을시
                            {
                                beforWeight = Dbconn.conn.getData(beforWeightDs, "WEIGHT", 0);
                                beforWeightTIme = Convert.ToDateTime(Dbconn.conn.getData(beforWeightDs, "WEIGHT_TIME", 0)).ToString("yyyy-MM-dd HH:mm:ss");
                            }

                            preWeight = out_weight;

                            int actualWeight = Convert.ToInt32(beforWeight) - Convert.ToInt32(preWeight);

                            //원료내역 업데이트
                            SQL = $@"
                            UPDATE WAP_GOCAR SET N_WEIGHT = '{actualWeight}', BEFORE_WEIGHT = '{beforWeight}'
                                , BEFORE_WEIGHT_TIME = CONVERT(DATETIME, '{beforWeightTIme}')
                                , WEIGHT = '{preWeight}', WEIGHT_TIME = SYSDATE, PC_STATUS = '2'
                                ERP_LOCATION = '{SQL_WHERE}'
                            WHERE IS_NO = '{in_is_no}' AND INGRED_CODE = '{ing_cd}'
                            ";
                            
                            Dbconn.conn.SQLrun(SQL);

                        }
                        else
                        {
                            //사이로원료인지 확인 처리
                            SQL = $"SELECT * FROM BIN WHERE PROCESS_KEY = 'P01' AND RESOURCE_NO = '{ing_cd}' ";

                            DataSet tmpBinSearchDs = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(tmpBinSearchDs) > 0)
                            {
                                //사이로원료는 강제완료처리 해준다 (하차없이 사이로 직접투입)
                                SQL = $@"
                                UPDATE WAP_GOCAR
                                SET PC_STATUS = '2', ERP_LOCATION = 'P01' , N_WEIGHT = SPIV_CAR_WEIGHT, OFF_TIME = SYSDATE
                                WHERE IS_NO = '{in_is_no}'
                                    AND INGRED_CODE = '{ing_cd}'
                                ";

                                Dbconn.conn.SQLrun(SQL);
                            }
                        }

                        //차량소독여부전송
                        //XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0002", "1");
                    }

                    //출고량 업데이트
                    SQL = $@"
                    UPDATE WAP_DECAR 
                    SET OUT_WEIGHT = '{out_weight}' 
                        , OUTCAR_DATE = SYSDATE, OUTCAR_TIME = SYSDATE
                    WHERE IS_NO = '{in_is_no}'
                    ";
                    
                    Dbconn.conn.SQLrun(SQL);
                }


/*                try
                {
                    XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0003", "1");
                }
                catch (Exception)
                {
                    XGT_PLC.Tcp_Plc.PLC_Write_Bit(clsCommon.plc_scale_ip, "%MX0003", "1");
                }*/

                return "OK";

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsCarPorcess", "outChkProcess", ex);
                clsLog.logSave("clsCarPorcess", "outChkProcess", ex.StackTrace);
                clsLog.logSave("clsCarPorcess", "outChkProcess", ex.Source);
            }

            return "출차처리중 에러가 발생했습니다";
        }

    }
}
