using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class clsLotManager
    {
        public static string LotFromBincode(string process_key, string sbno = "")
        {
            string fromBin = string.Empty;
            try
            {
                if (process_key == "3202")
                {
                    fromBin = $" AND LOC_DCODE IN ( SELECT LOC_DCODE FROM PD_LOCATION WHERE PROCESS_KEY = '{process_key}' ) ";
                }
                else if (process_key == "3204")
                {
                    if (sbno == "1")
                    {
                        fromBin = " AND LOC_DCODE IN ('501','502','3203-1') ";
                    }
                    else if (sbno == "2")
                    {
                        fromBin = " AND LOC_DCODE IN ('503','504','3203-1') ";
                    }
                }
                else if (process_key == "3205")
                {
                    if (sbno == "1")
                    {
                        fromBin = " AND (LOC_DCODE IN (SELECT LOC_DCODE FROM PD_LOCATION WHERE PROCESS_KEY = '3201')) OR (LOC_DCODE IN ('511') )  ";
                    }
                    else if (sbno == "2")
                    {
                        fromBin = " AND (LOC_DCODE IN (SELECT LOC_DCODE FROM PD_LOCATION WHERE PROCESS_KEY = '3201')) OR (LOC_DCODE IN ('513') )  ";
                    }
                }
                else if (process_key == "3206")
                {
                    if (sbno == "1")
                    {
                        fromBin = " AND LOC_DCODE IN ('501','502','511','3221-1','411','412','413' ) ";
                    }
                    else if (sbno == "2")
                    {
                        fromBin = " AND LOC_DCODE IN ('503','504','513','3221-1','411','412','413' ) ";
                    }
                } else
                {
                    fromBin = " AND LOC_DCODE IN (SELECT LOC_DCODE FROM PD_LOCATION WHERE PROCESS_KEY = '3201')  ";
                }
            }
            catch (Exception ex)
            {
                fromBin = "FAIL";
                clsLog.logSave("clsLotManager", "LotFromBincode", ex);
            }

            return fromBin;
        }

        /// <summary>
        /// LOT 이동위치 빈코드로 반환 
        /// </summary>
        /// <param name="process_key">공정코드</param>
        /// <param name="sbno">호기정보</param>
        /// <param name="bin_code">이동위치</param>
        /// <returns></returns>
        public static string LotFromDetailBincode(string process_key, string sbno, string bin_code)
        {
            string fromBin = string.Empty;
            try
            {
                if (process_key == "3206")
                {
                    if (bin_code == "421" || bin_code == "422" || bin_code == "431" || bin_code == "432" || bin_code == "433" || bin_code == "434")
                    {
                        fromBin = "3221-1";
                    }
                    else if (bin_code == "501" || bin_code == "502")
                    {
                        fromBin = "511";
                    }
                    else if (bin_code == "503" || bin_code == "504")
                    {
                        fromBin = "513";
                    }
                    else
                    {
                        fromBin = bin_code;
                    }
                }
                else
                {
                    fromBin = bin_code;
                }
            }
            catch (Exception ex)
            {
                fromBin = "FAIL";
                clsLog.logSave("clsLotManager", "LotFromBincode", ex);
            }

            return fromBin;
        }

        /// <summary>
        /// 공정별 위동위치 반환
        /// </summary>
        /// <param name="process_key">공정코드</param>
        /// <returns>이동위치 Dictionary 반환</returns>
        public static Dictionary<string, string> LotToBincode(string process_key)
        {
            Dictionary<string, string> diction = new Dictionary<string, string>();

            string SQL = string.Empty;

            if (process_key == clsCommon.GetProcessKey("EP(익스콘)") || process_key == "3223")
            {
                SQL = $"SELECT LOC_DCODE, DESCRIPTION FROM PD_LOCATION WHERE PROCESS_KEY IN ('3204', '3205') ORDER BY LOC_DCODE ";
            }
            else
            {
                SQL = $"SELECT LOC_DCODE, DESCRIPTION FROM PD_LOCATION WHERE PROCESS_KEY = '{process_key}' ORDER BY LOC_DCODE ";
            }

            DataSet lotBinDs = Dbconn.conn.ExecutDataset(SQL);

            for(int i = 0; i < Dbconn.conn.getRowCnt(lotBinDs); i++)
            {
                diction.Add(Dbconn.conn.getData(lotBinDs, "DESCRIPTION", i),
                            Dbconn.conn.getData(lotBinDs, "LOC_DCODE", i)
                    );
            }
            return diction;
        }


        /// <summary>
        /// 로트이동관리 작지번호 생성
        /// </summary>
        /// <param name="work_day">이동일자(yyyymmdd)</param>
        /// <returns>작지번호순번</returns>
        public static string workNumMake(string work_day)
        {
            try
            {
                string SQL =
                "SELECT NVL(MAX(WORK_SEQ) + 1, 1) AS SEQ  " +
                "FROM INOUT_LOCATION_INFO WHERE WO_NUMBER = '{0}'";
                SQL = string.Format(SQL, work_day);

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
                clsLog.logSave("clsLotManager", "workNumMake", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 로트이동관리 등록
        /// </summary>
        /// <param name="dt_day">이동일자</param>
        /// <param name="from_loc">이전 위치코드</param>
        /// <param name="to_loc">이동할 위치코드</param>
        /// <param name="res_no">품목코드</param>
        /// <param name="qty">수량</param>
        /// <param name="input_lot">이동로트번호</param>
        /// <returns>로트이동관리 등록여부</returns>
        public static bool Inout_Location_Input(string dt_day, string from_loc, string to_loc, string res_no, string qty, string input_lot, bool isStockUse = false)
        {
            try
            {
                string SQL = "INSERT INTO INOUT_LOCATION_INFO ( " +
                    "   WO_NUMBER, WORK_SEQ, RESOURCE_NO,  " +
                    "   IN_Q, INPUT_LOT,  " +
                    "   BLOC_DCODE, ALOC_DCODE, IN_REV, I_TIME)  " +
                    "VALUES ( " +
                    $" /* WO_NUMBER */ '{dt_day}', " +
                    $" /* WORK_SEQ */ '{clsLotManager.workNumMake(dt_day)}', " +
                    $" /* RESOURCE_NO */ '{res_no}', " +
                    $" /* IN_Q */ '{qty}', " +
                    $" /* INPUT_LOT */ '{input_lot}', " +
                    $" /* BLOC_DCODE */ '{from_loc}', " +
                    $" /* ALOC_DCODE */ '{to_loc}', " +
                    $" /* IN_REV */ '{clsCommon.UserId}', " +
                    $" /* I_TIME */ SYSDATE ) ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    return false;
                }

                if (isStockUse)
                {
                    SQL = $"UPDATE ST_STK_MASTER SET STK_QTY = STK_QTY - {qty}  WHERE P_LOT = '{input_lot}' AND LOC_DCODE = '{from_loc}' ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("lotStockCheck", "Inout_Location_Input", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 로트존재여부 체크
        /// </summary>
        /// <param name="to_loc">이동할 위치코드</param>
        /// <param name="lot_num">이동할 로트</param>
        /// <returns>로트존재여부 있으면 true, 없으면 false 반환 </returns>
        public static bool LotStockMakeCheck(string to_loc, string lot_num)
        {
            try
            {
                string SQL = $"SELECT * FROM ST_STK_MASTER WHERE LOC_DCODE = '{to_loc}' AND P_LOT = '{lot_num}'";
                DataSet tmpDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tmpDs) > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "LotStockMakeCheck", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 로트번호 생성
        /// </summary>
        /// <param name="process_key">공정코드</param>
        /// <param name="work_order">작지번호</param>
        /// <param name="num">작지순번</param>
        /// <param name="batch">작업배치</param>
        /// <param name="to_loc">이동위치</param>
        /// <returns></returns>
        public static string MakeLot(string process_key, string work_order, string num, string batch,  string to_loc)
        {
            try
            {
                string SQL
                = "SELECT NVL(MAX(REGEXP_SUBSTR(P_LOT, '[^-]+',1,2)),0) + 1 AS CNT  " +
                  "FROM ST_STK_MASTER  " +
                   $"WHERE PROCESS_KEY = '{process_key}' " +
                   $"AND LOC_DCODE = '{to_loc}' " +
                   $"AND P_LOT LIKE '{work_order + num.PadLeft(3, '0')}%'";

                DataSet tmpDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tmpDs) == 0)
                {
                    clsLog.logSave("clsLotManager", "MakeLot", "로트순번 생성에러 : " + SQL);
                    return "0";
                }

                string seq = Dbconn.conn.getData(tmpDs, "CNT", 0);

                //LOT 생성
                string make_plot = work_order + num.PadLeft(3, '0') + "-" + seq.PadLeft(3, '0') + "-" + process_key + "-" + batch.PadLeft(3, '0');

                return make_plot;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "MakeLot", ex.Message);
                return "0";
            }
        }


        /// <summary>
        /// 로트재고 생성
        /// </summary>
        /// <param name="process_key">공정코드</param>
        /// <param name="res_no">품목코드</param>
        /// <param name="to_loc">로트생성 위치코드</param>
        /// <param name="p_lot">이동할 로트번호</param>
        /// <param name="qty">수량</param>
        /// <returns>로트재고 생성여부</returns>
        public static bool MakeLotStock(string process_key, string work_order, string num, string batch, string res_no, string to_loc, decimal qty)
        {
            try
            {
                string SQL
                = "SELECT NVL(MAX(REGEXP_SUBSTR(P_LOT, '[^-]+',1,2)),0) + 1 AS CNT  " +
                  "FROM ST_STK_MASTER  " +
                   $"WHERE PROCESS_KEY = '{process_key}' " +
                   $"AND LOC_DCODE = '{to_loc}' " +
                   $"AND P_LOT LIKE '{work_order + num.PadLeft(3, '0')}%'";

                DataSet tmpDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tmpDs) == 0)
                {
                    clsLog.logSave("clsLotManager", "MakeLotStock", "로트순번 생성에러 : " + SQL);
                    return false;
                }

                string seq = Dbconn.conn.getData(tmpDs, "CNT", 0);

                //LOT 생성
                string make_plot = work_order + num.PadLeft(3, '0') + "-" + seq.PadLeft(3, '0') + "-" + process_key + "-" + batch.PadLeft(3, '0');

                //단위 검색
                string uom = string.Empty;

                SQL = $"SELECT UOM FROM PRODUCT WHERE RESOURCE_NO = '{res_no}'";
                DataSet searchUomDs =  Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(searchUomDs) == 1)
                {
                    uom = Dbconn.conn.getData(searchUomDs, "UOM", 0);
                }

                SQL
                = "INSERT INTO ST_STK_MASTER( " +
                     "   RESOURCE_NO, LOC_DCODE, P_LOT,  " +
                     "   PROCESS_KEY, STK_QTY, STK_CRT_DD,  " +
                     "   HOLD_YN, UOM )  " +
                     "VALUES (  " +
                     $" /* RESOURCE_NO */'{res_no}', " +
                     $" /* LOC_DCODE */'{to_loc}', " +
                     $" /* P_LOT */'{make_plot}', " +
                     $" /* PROCESS_KEY */'{process_key}', " +
                     $" /* STK_QTY */'{qty}', " +
                     $" /* STK_CRT_DD */ SYSDATE, " +
                     $" /* HOLD_YN */ 'Y', " +
                     $" /* UOM */ '{uom}' "+
                     ") ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    clsLog.logSave("clsLotManager", "MakeLotStock", SQL);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "MakeLotStock", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 재고 이동 (이동하려는 곳에 재고 존재시 합산, 없을 시 차감 및 로트생성)
        /// </summary>
        /// <param name="lot_no">로트번호</param>
        /// <param name="from_locCode">이전 위치코드</param>
        /// <param name="to_locCode">이동 위치코드</param>
        /// <param name="useQty">이동수량</param>
        /// <returns>OK : 이동완료, 그외 에러메시지</returns>
        public static string LotMove(string lot_no, string from_locCode, string to_locCode, decimal useQty)
        {
            DataSet tmpDs = null;

            string SQL = "SELECT RESOURCE_NO, PROCESS_KEY, NVL(STK_QTY,0) AS STK_QTY FROM ST_STK_MASTER WHERE P_LOT = '{0}' AND LOC_DCODE = '{1}' ";
            SQL = string.Format(SQL, lot_no, from_locCode);
            tmpDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(tmpDs) == 0)
            {
                return "로트재고를 찾을 수 없습니다";
            }

            string res_no = Dbconn.conn.getData(tmpDs, "RESOURCE_NO", 0);
            string process_key = Dbconn.conn.getData(tmpDs, "PROCESS_KEY", 0);
            decimal stk_qty = Convert.ToDecimal(Dbconn.conn.getData(tmpDs, "STK_QTY", 0));

            if (useQty > stk_qty)
            {
                return "이동하려는 재고가 로트재고량보다 많습니다\r\n로트재고량 : " + stk_qty + "\r\n이동 재고량 : " + useQty;
            }

            SQL = "SELECT NVL(STK_QTY,0) AS STK_QTY FROM ST_STK_MASTER WHERE P_LOT = '{0}' AND LOC_DCODE = '{1}' ";
            SQL = string.Format(SQL, lot_no, to_locCode);
            tmpDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(tmpDs) == 0)
            {
                if (stk_qty == useQty)
                {
                    SQL = "UPDATE ST_STK_MASTER SET LOC_DCODE = '{0}' WHERE P_LOT = '{1}' AND LOC_DCODE = '{2}' ";
                    SQL = string.Format(SQL,
                        to_locCode,
                        lot_no,
                        from_locCode
                        );
                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        return "로트를 이동중 에러가 발생하였습니다 ";
                    }
                }
                else
                {
                    SQL = "UPDATE ST_STK_MASTER SET STK_QTY = STK_QTY - {0}  WHERE P_LOT = '{1}' AND LOC_DCODE = '{2}' ";
                    SQL = string.Format(SQL,
                            useQty,
                            lot_no,
                            from_locCode
                          );

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        return "로트를 이동중 에러가 발생하였습니다" ;
                    }

                    SQL = "INSERT INTO ST_STK_MASTER ( " +
                            "   RESOURCE_NO, LOC_DCODE, P_LOT,  " +
                            "   PROCESS_KEY, STK_QTY, STK_CRT_DD, HOLD_YN)  " +
                            "VALUES ( " +
                            $" /* RESOURCE_NO */'{res_no}', " +
                            $" /* LOC_DCODE */'{to_locCode}', " +
                            $" /* P_LOT */'{lot_no}', " +
                            $" /* PROCESS_KEY */'{process_key}', " +
                            $" /* STK_QTY */'{useQty}', " +
                            $" /* STK_CRT_DD */SYSDATE, " +
                            "  /* HOLD_YN */ 'Y' " +
                            " ) ";
                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        return "로트를 이동중 에러가 발생하였습니다";
                    }
                }
            }
            else
            {
                if (stk_qty == useQty)
                {
                    SQL = "UPDATE ST_STK_MASTER SET STK_QTY = '0' WHERE P_LOT = '{0}' AND LOC_DCODE = '{1}' ";
                    SQL = string.Format(SQL, lot_no, from_locCode);

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        return "로트를 이동중 에러가 발생하였습니다";
                    }
                }
                else
                {
                    SQL = "UPDATE ST_STK_MASTER SET STK_QTY = STK_QTY - {0}  WHERE P_LOT = '{1}' AND LOC_DCODE = '{2}' ";
                    SQL = string.Format(SQL,
                            useQty,
                            lot_no,
                            from_locCode
                          );

                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        return "로트를 이동중 에러가 발생하였습니다";
                    }
                }

                SQL = "UPDATE ST_STK_MASTER SET STK_QTY = STK_QTY + {0}, STK_CRT_DD = SYSDATE  WHERE P_LOT = '{1}' AND LOC_DCODE = '{2}' ";
                SQL = string.Format(SQL,
                        useQty,
                        lot_no,
                        to_locCode
                      );

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    return "로트를 이동중 에러가 발생하였습니다";
                }
            }

            return "OK";
        }

        /// <summary>
        /// 로트재고 차감 (마이너스차감 불가)
        /// </summary>
        /// <param name="lot_no">로트번호</param>
        /// <param name="from_locCode">이전 위치코드</param>
        /// <param name="useQty">차감수량</param>
        /// <returns>처리메시지(OK : 정상처리)</returns>
        public static string LotMinusUse(string lot_no, string from_locCode, decimal useQty)
        {
            try
            {
                DataSet tmpDs = null;

                string SQL
                 = "SELECT RESOURCE_NO, STK_QTY, PROCESS_KEY " +
                   "FROM ST_STK_MASTER  " +
                   "WHERE P_LOT = '{0}' " +
                   "AND LOC_DCODE = '{1}' " +
                   "AND STK_QTY > 0 " +
                   "ORDER BY STK_CRT_DD DESC ";

                SQL = string.Format(SQL,
                    lot_no,
                    from_locCode
                    );
                tmpDs = Dbconn.conn.ExecutDataset(SQL);
                int lotCnt = Dbconn.conn.getRowCnt(tmpDs);

                string process_key = string.Empty;
                string res_no = string.Empty;
                decimal qtyVal = useQty;
                decimal lotVal = 0;
                decimal useVal = 0;

                if (lotCnt == 0)
                {
                    return "로트재고를 찾을 수 없습니다";
                }
                else
                {
                    lotVal = Convert.ToDecimal(Dbconn.conn.getData(tmpDs, "STK_QTY", 0));



                    if (lotVal <= 0)
                    {
                        return "로트재고가 모두 소진되었습니다";
                    }
                }

                process_key = Dbconn.conn.getData(tmpDs, "PROCESS_KEY", 0);
                res_no = Dbconn.conn.getData(tmpDs, "RESOURCE_NO", 0);


                if (qtyVal == lotVal)
                {
                    qtyVal = qtyVal - lotVal;

                    useVal = lotVal;

                    lotVal = 0;

                }
                else if (qtyVal < lotVal)
                {
                    lotVal = lotVal - qtyVal;

                    useVal = qtyVal;

                    qtyVal = 0;


                }
                else if (qtyVal > lotVal)
                {
                    qtyVal = qtyVal - lotVal;

                    useVal = lotVal;

                    lotVal = 0;
                }



                SQL = "UPDATE ST_STK_MASTER SET STK_QTY = '{3}' WHERE RESOURCE_NO = '{0}' AND LOC_DCODE = '{1}' AND P_LOT = '{2}' ";
                SQL = string.Format(SQL,
                    res_no,
                    from_locCode,
                    lot_no,
                    lotVal.ToString()
                    );

                Dbconn.conn.SQLrun(SQL);


                if (qtyVal > 0)
                {
                    return "차감할 로트재고가 부족합니다";
                }


                return "OK";
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "LotAutoMinusUse", ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 로트재고 자동차감 (선입선출 차감) (마이너스차감 가능)
        /// </summary>
        /// <param name="res_no">품목코드</param>
        /// <param name="from_locCode">이전 위치코드</param>
        /// <param name="useQty">차감 수량</param>
        /// <returns>처리메시지(OK : 정상처리)</returns>
        public static Dictionary<string, string> LotAutoMinusUse(string res_no, string from_locCode, decimal useQty)
        {
            Dictionary<string, string> diction = new Dictionary<string, string>();

            try
            {
                DataSet tmpDs = null;

                string SQL = $@"
                SELECT P_LOT
                     , STK_QTY
                  FROM ST_STK_MASTER
                 WHERE RESOURCE_NO = '{res_no}'
                   AND LOC_DCODE   = '{from_locCode}'
                   AND STK_QTY     > 0
                 ORDER BY STK_CRT_DD";

                tmpDs = Dbconn.conn.ExecutDataset(SQL);

                int lotCnt = Dbconn.conn.getRowCnt(tmpDs);

                decimal qtyVal = useQty;
                decimal lotVal = 0;
                decimal useVal = 0;

                if (lotCnt == 0)
                {
                    clsLog.logSave("clsLotManager", "LotAutoMinusUse", SQL);

                    //로트재고 없을 경우 마지막 로트재고에 마이너스 처리를 해준다
                    SQL = $@"
                    SELECT P_LOT
                         , STK_QTY
                      FROM ST_STK_MASTER
                     WHERE RESOURCE_NO = '{res_no}'
                       AND LOC_DCODE   = '{from_locCode}'
                       AND STK_QTY     <= 0
                     ORDER BY STK_CRT_DD DESC";

                    DataSet noLotDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(noLotDs) > 0)
                    {
                        string pLot = Dbconn.conn.getData(noLotDs, "P_LOT", 0);

                        SQL = $@"
                        UPDATE ST_STK_MASTER
                           SET STK_QTY = STK_QTY - {useQty}
                         WHERE RESOURCE_NO = '{res_no}'
                           AND LOC_DCODE   = '{from_locCode}'
                           AND P_LOT       = '{pLot}'";

                        Dbconn.conn.SQLrun(SQL);

                        diction.Add(pLot, (useQty * -1).ToString());
                        return diction;
                    }
                    else
                    {
                        //해당 품목 로트재고 자체가 없을 경우 마이너스 재고수량만 리턴
                        diction.Add("LOT NUMBER NOT FOUND ", (useQty * -1).ToString());
                        return diction;
                    }
                }
                else
                {
                    for (int i = 0; i < lotCnt; i++)
                    {
                        string pLot = Dbconn.conn.getData(tmpDs, "P_LOT", i);

                        if (qtyVal > 0)
                        {
                            lotVal = Convert.ToDecimal(Dbconn.conn.getData(tmpDs, "STK_QTY", i));

                            if (qtyVal == lotVal)
                            {
                                qtyVal = qtyVal - lotVal;

                                useVal = lotVal;

                                lotVal = 0;

                            }
                            else if (qtyVal < lotVal)
                            {
                                lotVal = lotVal - qtyVal;

                                useVal = qtyVal;

                                qtyVal = 0;


                            }
                            else if (qtyVal > lotVal)
                            {
                                qtyVal = qtyVal - lotVal;

                                useVal = lotVal;

                                lotVal = 0;
                            }

                            SQL = "UPDATE ST_STK_MASTER SET STK_QTY = '{3}' WHERE RESOURCE_NO = '{0}' AND LOC_DCODE = '{1}' AND P_LOT = '{2}' ";
                            SQL = string.Format(SQL,
                                res_no,
                                from_locCode,
                                pLot,
                                lotVal.ToString()
                                );

                            Dbconn.conn.SQLrun(SQL);


                            if (qtyVal > 0 && i == (lotCnt - 1))
                            {

                                SQL = "UPDATE ST_STK_MASTER SET STK_QTY = STK_QTY - {3} WHERE RESOURCE_NO = '{0}' AND LOC_DCODE = '{1}' AND P_LOT = '{2}' ";
                                SQL = string.Format(SQL,
                                    res_no,
                                    from_locCode,
                                    pLot,
                                    lotVal.ToString()
                                    );

                                Dbconn.conn.SQLrun(SQL);

                                diction.Add(pLot, (lotVal + qtyVal * -1).ToString());
                            }else
                            {
                                diction.Add(pLot, useVal.ToString());

                                if (qtyVal <= 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                return diction;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "LotAutoMinusUse", ex.Message);
                diction.Clear();
                return diction;
            }
        }

        public static bool resnoStockCheck(string res_no, string loc_code, out decimal stock, out string err_msg)
        {
            stock = 0;
            err_msg = "";

            try
            {
                string SQL =
                       "SELECT NVL(SUM(STK_QTY),0) AS SUM_QTY " +
                       "FROM ST_STK_MASTER " +
                       "WHERE RESOURCE_NO = '{0}' " +
                       "AND LOC_DCODE = '{1}' ";

                SQL = string.Format(SQL,
                    res_no,
                    loc_code
                    );

                DataSet tmpDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tmpDs) == 0)
                {
                    err_msg = "재고로트를 찾을 수 없습니다";
                    return false;
                }

                decimal stock_qty = Convert.ToDecimal(Dbconn.conn.getData(tmpDs, "SUM_QTY", 0));
                if (stock_qty <= 0)
                {
                    err_msg = "해당 로트에 재고가 없습니다";
                    return false;
                }

                stock = stock_qty;

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "resnoStockCheck", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 로트재고수량 반환
        /// </summary>
        /// <param name="lot_no">로트번호</param>
        /// <param name="loc_code">위치코드</param>
        /// <param name="stock">out 재고수량</param>
        /// <param name="err_msg">out 처리메시지</param>
        /// <returns>재고수량 반환여부</returns>
        public static bool lotStockCheck(string lot_no, string loc_code, out decimal stock, out string err_msg)
        {
            stock = 0;
            err_msg = "";

            try
            {
                string SQL = "SELECT STK_QTY FROM ST_STK_MASTER WHERE LOC_DCODE = '{0}' AND P_LOT = '{1}' ";
                SQL = string.Format(SQL,
                        loc_code,
                        lot_no
                    );

                DataSet tmpDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tmpDs) == 0)
                {
                    err_msg = "해당 로트를 찾을 수 없습니다";
                    return false;
                }
                else
                {
                    err_msg = "OK";
                    stock = Convert.ToDecimal(Dbconn.conn.getData(tmpDs, "STK_QTY", 0));
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "lotStockCheck", ex.Message);
                err_msg = ex.Message;
                return false;
            }
        }


        public static bool lotExpDateChange(string lot_no, string expdate)
        {
            try
            {
                string changeLotNo = string.Empty;
                string argExpDate = expdate.Replace("-", "");

                if (argExpDate.Length != 8)
                {
                    ShowMessageBox.XtraShowWarning("유효기간 날짜 체계가 맞지않습니다");
                    return false;
                }

                string SQL = string.Empty;
                string[] spl_lot = lot_no.Split('-');

                if (spl_lot.Length == 2)
                {
                    SQL = "UPDATE WAP_GOCAR SET BARCODE = '{0}' || '-' || BARCODE WHERE BARCODE = '{1}' ";
                    SQL = string.Format(SQL, expdate, lot_no);

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        ShowMessageBox.XtraShowError("로트번호를 수정중 에러가 발생하였습니다 -1");
                        return false;
                    }

                    SQL = "UPDATE ST_STK_MASTER SET P_LOT = '{0}' || '-' || P_LOT  WHERE P_LOT = '{1}' ";
                    SQL = string.Format(SQL, expdate, lot_no);

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        ShowMessageBox.XtraShowError("로트번호를 수정중 에러가 발생하였습니다 -2");
                        return false;
                    }
                }
                else if (spl_lot.Length == 3)
                {
                    string update_lot = expdate + "-" + spl_lot[1] + "-" + spl_lot[2];

                    SQL = "UPDATE WAP_GOCAR SET BARCODE = '{0}' WHERE BARCODE = '{1}' ";
                    SQL = string.Format(SQL, update_lot, lot_no);

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        ShowMessageBox.XtraShowError("로트번호를 수정중 에러가 발생하였습니다 -1");
                        return false;
                    }

                    SQL = "UPDATE ST_STK_MASTER SET P_LOT = '{0}' WHERE P_LOT = '{1}' ";
                    SQL = string.Format(SQL, update_lot, lot_no);

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        ShowMessageBox.XtraShowError("로트번호를 수정중 에러가 발생하였습니다 -2");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "lotExpDateChange", ex.Message);
                return false;
            }
        }


        public static bool lotWorkRemark(string process_cd, string workdate, string num, string batch, out string err_msg)
        {
            err_msg = "";

            try
            {
                string bin_code = string.Empty;
                string res_no = string.Empty;

                string SQL = "SELECT WORK_PLAN, BIN_CODE FROM WORK_ORDER WHERE PROCESS_KEY = '{0}' AND WORKDATE = '{1}' AND NUM = '{2}' ";
                SQL = string.Format(SQL,
                                process_cd,
                                workdate,
                                num
                                );

                DataSet tmpDs = Dbconn.conn.ExecutDataset(SQL);
                if (Dbconn.conn.getRowCnt(tmpDs) == 0)
                {
                    err_msg = "작업지시를 찾을 수 없습니다";
                    return false;
                }

                bin_code = Dbconn.conn.getData(tmpDs, "BIN_CODE", 0);

                SQL
                = "SELECT INGRED_LOT, LOC_DCODE, RESOURCE_NO, P_Q " +
                    "FROM WORK_REMARK  " +
                    "WHERE PROCESS_KEY = '{0}'  " +
                    "AND WORKDATE = '{1}'  " +
                    "AND NUM = '{2}'  " +
                    "AND BATCH = '{3}' " +
                    "ORDER BY P_Q DESC ";

                SQL = string.Format(SQL,
                    process_cd,
                    workdate,
                    num,
                    batch
                    );

                tmpDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tmpDs) == 0)
                {
                    err_msg = "작업실적을 찾을 수 없습니다";
                    return false;
                }

                //빈사용실적 로트차감, 로트이동내역 등록
                string ingred_lot = string.Empty;
                string loc_dcode = string.Empty;
                string ingred_cd = string.Empty;
                string p_q = string.Empty;
                decimal sum_p_q = 0;

                Dictionary<string, string> diction = new Dictionary<string, string>();

                for (int i=0; i < Dbconn.conn.getRowCnt(tmpDs); i ++)
                {
                    ingred_lot = Dbconn.conn.getData(tmpDs, "INGRED_LOT", i);
                    loc_dcode = Dbconn.conn.getData(tmpDs, "LOC_DCODE", i);
                    ingred_cd = Dbconn.conn.getData(tmpDs, "RESOURCE_NO", i);
                    p_q = Dbconn.conn.getData(tmpDs, "P_Q", i);

                    sum_p_q += Convert.ToDecimal(p_q);

                    diction.Clear();
                    diction = clsLotManager.LotAutoMinusUse(ingred_cd, loc_dcode, Convert.ToDecimal(p_q));

                    foreach (var item in diction)
                    {
                        clsLog.logSave(item.Key + " / " + item.Value, 0);
                    }
                }

                SQL = "SELECT PROCESS_KEY FROM PD_LOCATION WHERE LOC_DCODE = '{0}' ";
                SQL = string.Format(SQL, bin_code);

                tmpDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(tmpDs) == 0)
                {
                    err_msg = "목적빈 정보를 찾을 수 없습니다";
                    return false;
                }

                string makeLotProcessKey = string.Empty;

                makeLotProcessKey = Dbconn.conn.getData(tmpDs, "PROCESS_KEY", 0);

                if (string.IsNullOrEmpty(makeLotProcessKey))
                {
                    err_msg = "목적빈 공정정보를 찾을 수 없습니다";
                    return false;
                }

                //빈로트재고 생성
                bool isMakeBinStock = MakeLotStock(makeLotProcessKey, workdate, num, batch, res_no, bin_code, sum_p_q);

                if (!isMakeBinStock)
                {
                    err_msg = "목적빈 빈로트재고 생성을 실패하였습니다";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "lotWorkRemark", ex.Message);
                err_msg = ex.Message;
                return false;
            }
        }

        public static bool lotStockRestore(string process_key, string workdate, string num, string batch)
        {
            string SQL = string.Empty;

            try
            {
                SQL = "SELECT RESOURCE_NO, INGRED_LOT, LOC_DCODE, P_Q FROM WORK_REMARK WHERE PROCESS_KEY = '{0}' AND WORKDATE = '{1}' AND NUM = '{2}' AND BATCH = '{3}' ";

                SQL = string.Format(SQL,
                    process_key,
                    workdate,
                    num,
                    batch
                    );

                DataSet lotpqDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(lotpqDs) == 0)
                {
                    return true;
                }

                string sResNo = string.Empty;
                string sIngredLot = string.Empty;
                string sLocDcode = string.Empty;
                string sPq = string.Empty;

                for (int i = 0; i < Dbconn.conn.getRowCnt(lotpqDs); i++)
                {
                    sResNo = Dbconn.conn.getData(lotpqDs, "RESOURCE_NO", i);
                    sIngredLot = Dbconn.conn.getData(lotpqDs, "INGRED_LOT", i);
                    sLocDcode = Dbconn.conn.getData(lotpqDs, "LOC_DCODE", i);
                    sPq = Dbconn.conn.getData(lotpqDs, "P_Q", i);

                    SQL = $"UPDATE ST_STK_MASTER SET STK_QTY = STK_QTY + {sPq}  WHERE RESOURCE_NO = '{sResNo}' AND LOC_DCODE = '{sLocDcode}' AND P_LOT = '{sIngredLot}' ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        clsLog.logSave("clsLotManager", "lotStockRestore", SQL);
                        return false;
                    }
                }//for

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "lotStockRestore", ex.Message);
                return false;
            }
        }

        public static bool lotStockRestore_Hand(string barno)
        {
            string SQL = string.Empty;

            try
            {
                SQL = "SELECT C_CONDITION FROM BFH_MASTER WHERE BAR_NO = '{0}' ";
                DataSet conDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(conDs) == 0)
                {
                    return true;
                }

                string c_condition = Dbconn.conn.getData(conDs, "C_CONDITION", 0);

                if (c_condition != "031004")
                {
                    return true;
                }


                SQL = "SELECT  INGRED_CODE, INGRED_LOT, LOC_DCODE, P_Q FROM BFH_DETAIL WHERE BAR_NO = '{0}' AND P_Q > 0 " +
                        "ORDER BY P_Q ";

                SQL = string.Format(SQL,
                    barno
                    );

                DataSet lotpqDs = Dbconn.conn.ExecutDataset(SQL);

                string sResNo = string.Empty;
                string sIngredLot = string.Empty;
                string sLocDcode = string.Empty;
                string sPq = string.Empty;

                for (int i = 0; i < Dbconn.conn.getRowCnt(lotpqDs); i++)
                {
                    sResNo = Dbconn.conn.getData(lotpqDs, "INGRED_CODE", i);
                    sIngredLot = Dbconn.conn.getData(lotpqDs, "INGRED_LOT", i);
                    sLocDcode = Dbconn.conn.getData(lotpqDs, "LOC_DCODE", i);
                    sPq = Dbconn.conn.getData(lotpqDs, "P_Q", i);

                    SQL = $"UPDATE ST_STK_MASTER SET STK_QTY = STK_QTY + {sPq}  WHERE RESOURCE_NO = '{sResNo}' AND LOC_DCODE = '{sLocDcode}' AND P_LOT = '{sIngredLot}' ";

                    if (Dbconn.conn.SQLrun(SQL) < 0)
                    {
                        clsLog.logSave("clsLotManager", "lotStockRestore", SQL);
                        return false;
                    }

                }//for

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsLotManager", "lotStockRestore", ex.Message);
                return false;
            }
        }

        public static bool lotStockCorrection(string process_key, string res_no, string loc, string qty, string lot_no, out string err_msg)
        {
            err_msg = "";
            string SQL = string.Empty;

            SQL = "SELECT STK_QTY FROM ST_STK_MASTER WHERE RESOURCE_NO = '{0}' AND LOC_DCODE = '{1}' AND P_LOT = '{2}' ";
            SQL = string.Format(SQL,
                res_no,
                loc,
                lot_no
                );

            DataSet beforQtyDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(beforQtyDs) == 0)
            {
                err_msg = "로트 재고를 찾을 수 없습니다";
                return false;
            }

            string beforStkQty = Dbconn.conn.getData(beforQtyDs, "STK_QTY", 0).Trim();

            if (Convert.ToDouble(beforStkQty) != Convert.ToDouble(qty))
            {
                SQL = "INSERT INTO ST_STK_UD ( " +
                    "   RESOURCE_NO, LOC_DCODE, SEQ, P_LOT,  " +
                    "   PROCESS_KEY, BEFORE_QTY, STK_QTY,  " +
                    "   STK_CRT_DD, I_TIME)  " +
                    "VALUES (  " +
                    $" /* RESOURCE_NO */ '{res_no}', " +
                    $" /* LOC_DCODE */ '{loc}', " +
                    $"(SELECT COUNT(*)+1 FROM ST_STK_UD WHERE LOC_DCODE = '{loc}' ), " +
                    $" /* P_LOT */ '{lot_no}', " +
                    $" /* PROCESS_KEY */ '{process_key}', " +
                    $" /* BEFORE_QTY */ '{beforStkQty}', " +
                    $" /* STK_QTY */ '{qty}', " +
                    " /* STK_CRT_DD */ SYSDATE, " +
                    " /* I_TIME */ SYSDATE  ) ";

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    err_msg = "재고보정내역을 입력도중 에러가 발생하였습니다";
                    return false;
                }

                SQL = "UPDATE ST_STK_MASTER SET STK_QTY = '{3}' WHERE RESOURCE_NO = '{0}' AND LOC_DCODE = '{1}' AND P_LOT = '{2}' ";
                SQL = string.Format(SQL,
                        res_no,
                        loc,
                        lot_no,
                        qty
                        );

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    err_msg = "재고를 수정하는 도중 에러가 발생하였습니다";
                    return false;
                }
            }

            return true;
        }
    }
}
