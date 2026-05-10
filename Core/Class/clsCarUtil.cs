using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace Core.Class
{
    public class clsCarUtil
    {
        public static string returnCarGubun(string carno)
        {
            string returnType = string.Empty;
            string SQL
            = "SELECT VEHICLENO,VEHICLETONCODE,DECODE(SUBSTR(VEHICLETONCODE, -2), 'BK','벌크','카고') AS CAR_TYPE  " +
                "FROM TMS_INPUT_CARMASTER_CON  " +
                $"WHERE VEHICLECODE = '{carno}' ";

            DataSet carTypeDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(carTypeDs) == 1)
            {
                returnType = Dbconn.conn.getData(carTypeDs, "CAR_TYPE", 0);
            }

            carTypeDs.Dispose();
            return returnType;
        }

        public static string returnCarGubun2(string is_no)
        {
            string returnType = string.Empty;
            string SQL
                = "SELECT CARM.VEHICLETONCODE,DECODE( SUBSTR(CARM.VEHICLETONCODE, -2), 'BK','벌크','카고') AS CAR_TYPE   " +
                    "FROM WAP_DECAR WD " +
                    "LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CARM ON WD.INCAR_NO = CARM.VEHICLECODE " +
                    "WHERE CARM.VEHICLECODE IS NOT NULL " +
                    $"AND WD.IS_NO = '{is_no}' ";

            DataSet carTypeDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(carTypeDs) == 1)
            {
                returnType = Dbconn.conn.getData(carTypeDs, "CAR_TYPE", 0);
            }

            carTypeDs.Dispose();
            return returnType;
        }

        public static string returnOrderTypeCode(string is_no)
        {
            string returnType = string.Empty;
            string SQL
                = "SELECT AD.ORDERTYPECODE " +
                    "FROM TMS_INPUT_PLOADD_CON  AD " +
                    "JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO  " +
                    "AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO    " +
                    $"WHERE TMS_OUT_RESULT.IS_NO = '{is_no}' " +
                    "GROUP BY AD.ORDERTYPECODE ";

            DataSet orderTypeDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(orderTypeDs) == 1)
            {
                returnType = Dbconn.conn.getData(orderTypeDs, "ORDERTYPECODE", 0);
            }

            orderTypeDs.Dispose();
            return returnType;
        }

        public static string returnInWeight(string is_no)
        {
            try
            {
                string returnInWeight = "0";

                string SQL = $"SELECT NVL(IN_WEIGHT,0) AS IN_WEIGHT FROM WAP_DECAR WHERE IS_NO = '{is_no}' ";
                DataSet inWeightDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(inWeightDs) == 1)
                {
                    returnInWeight = Dbconn.conn.getData(inWeightDs, "IN_WEIGHT", 0);
                }

                inWeightDs.Dispose();
                return returnInWeight;
            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                return string.Empty;
            }
        }

        public static string returnCarNo(string is_no)
        {
            try
            {
                string returnCarNo = string.Empty;

                string SQL = $"SELECT INCAR_NO FROM WAP_DECAR WHERE IS_NO = '{is_no}' ";
                DataSet carNoDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(carNoDs) == 1)
                {
                    returnCarNo = Dbconn.conn.getData(carNoDs, "INCAR_NO", 0);
                }

                carNoDs.Dispose();
                return returnCarNo;
            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                return string.Empty;
            }
        }
        public static string returnCarType(string is_no)
        {
            try
            {
                string returnType = string.Empty;

                string SQL = $"SELECT CAR_TYPE FROM WAP_DECAR WHERE IS_NO = '{is_no}' ";
                DataSet carTypeDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(carTypeDs) == 1)
                {
                    returnType = Dbconn.conn.getData(carTypeDs, "CAR_TYPE", 0);
                }

                carTypeDs.Dispose();
                return returnType;
            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                return string.Empty;
            }
        }
        public static string factoryGroupSearch(string factory)
        {
            try
            {
                string whereQuery = string.Empty;
                switch (factory)
                {
                    case "P101": whereQuery = "('P101')"; break;
                    case "P102": whereQuery = "('P102')"; break;
                    case "P201": whereQuery = "('P201')"; break;
                    case "PJ01": whereQuery = "('PJ01', 'PJ02')"; break;
                    case "PJ04": whereQuery = "('PJ04', 'PJ05')"; break;
                }
                return whereQuery;
            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                return string.Empty;
            }
        }

        public static bool updateProductErpUpFlag(string isNo)
        {
            try
            {
                string carType = returnCarType(isNo);

                if (string.IsNullOrEmpty(carType))
                {
                    clsLog.logSave($"IS_NO : {isNo} CAR_TYPE 없음 ", 0);
                    return false;
                }


                string SQL = string.Empty;
                if (carType == "001")
                {
                    /*                    SQL = $"UPDATE WAP_DECAR SET ERP_UP_YN = 'F' WHERE IS_NO = '{isNo}' ";
                                        if (Dbconn.conn.SQLrun(SQL) < 1)
                                        {
                                            return false;
                                        }

                                        clsUtil.Delay(1000);*/

                    return true;
                    /*                    for (int i=0; i < 10; i++)
                                        {
                                            SQL = $"SELECT ERP_UP_YN FROM WAP_DECAR WHERE IS_NO = '{isNo}' AND ERP_UP_YN = 'F'";
                                            DataSet dbChkDs = Dbconn.conn.ExecutDataset(SQL);

                                            if (Dbconn.conn.getRowCnt(dbChkDs) == 0 )
                                            {
                                                dbChkDs.Dispose();
                                                return true;
                                            }
                                            dbChkDs.Dispose();
                                            clsUtil.Delay(1000);
                                        }
                                        return false;*/
                }
                else if (carType == "002" || carType == "003" || carType == "004" || carType == "005")
                {
                    //carType == "004" || carType == "005"

                    SQL = $"UPDATE TMS_INPUT_PLOADM_CON SET PDE_YN = 'Y', ERP_UP_YN ='F', TMS_UP_YN ='F'  WHERE DISPATCHNO IN (SELECT DISPATCHNO FROM TMS_OUTPUT_RESULT WHERE IS_NO = '{isNo}' )  ";
                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        return false;
                    }

                    SQL = $"UPDATE TMS_OUTPUT_RESULT SET ERP_UP_YN = 'F', TMS_UP_YN = 'F' WHERE IS_NO = '{isNo}' ";
                    if (Dbconn.conn.SQLrun(SQL) < 1)
                    {
                        return false;
                    }

                    clsUtil.Delay(1000);

                    return true;

                    /*                   
                                        for (int i = 0; i < 10; i++)
                                        {
                                            SQL = $"SELECT ERP_UP_YN FROM TMS_OUTPUT_RESULT WHERE IS_NO = '{isNo}' AND  ERP_UP_YN = 'F' ";
                                            DataSet dbChkDs = Dbconn.conn.ExecutDataset(SQL);

                                            if (Dbconn.conn.getRowCnt(dbChkDs) == 0)
                                            {
                                                dbChkDs.Dispose();
                                                return true;
                                            }

                                            dbChkDs.Dispose();
                                            clsUtil.Delay(1000);
                                        }
                                        return false;
                    */
                }

                return true;
            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
        }

        public static DataSet returnDeliveryDs(string devDate, string carCode)
        {
            string SQL
                = "SELECT ADM.DELIVERYDATE, ADM.DISPATCHNO,  " +
                    "LISTAGG(CAST( SAP_CUS.NAME_ORG1  AS VARCHAR(50)), ', ')  WITHIN GROUP (ORDER BY SAP_CUS.NAME_ORG1) AS NAME_ORG1, " +
                    "LISTAGG(CAST(SAP_PRO.DESCRIPTION AS VARCHAR(50)), ', ') WITHIN GROUP (ORDER BY SAP_PRO.DESCRIPTION) AS DESCRIPTION, SUM(AD.PLANQTY) AS PLANQTY " +
                    "FROM TMS_INPUT_PLOADM_CON ADM  " +
                    "JOIN TMS_INPUT_PLOADD_CON AD ON ADM.DISPATCHNO = AD.DISPATCHNO  " +
                    "LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CAR ON ADM.VEHICLECODE = CAR.VEHICLECODE  " +
                    $"LEFT OUTER JOIN SAP_DI_PRODUCT SAP_PRO ON AD.ITEMCODE = SAP_PRO.RESOURCE_NO  AND AD.PLANTCODE = SAP_PRO.PLANT_CODE   " +
                    "LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS ON AD.TOLOCATIONCODE = SAP_CUS.PARTNER  " +
                    "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO  " +
                    "AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO  " +
                    $"WHERE AD.PLANTCODE IN {factoryGroupSearch(clsCommon.PlantCode)}   AND NVL(TMS_OUT_RESULT.ERP_UP_YN,'N') = 'N' " +
                    $"AND ADM.DELIVERYDATE = '{devDate}' AND  ADM.VEHICLECODE  = '{carCode}' " +
                    "GROUP BY ADM.DELIVERYDATE, ADM.DISPATCHNO " +
                    "ORDER BY ADM.DISPATCHNO ";

            using (DataSet carDs = Dbconn.conn.ExecutDataset(SQL))
            {
                return carDs;
            }
        }

        public static DataSet returnChulgoCarFullDs(string carCode)
        {
            string SQL = "SELECT VEHICLECODE, VEHICLENO, DRIVERNAME, DRIVERMOBILE " +
                        $"FROM TMS_INPUT_CARMASTER_CON WHERE VEHICLECODE = '{carCode}' ";


            using (DataSet carDs = Dbconn.conn.ExecutDataset(SQL))
            {
                return carDs;
            }
        }

        public static string argCarCd = string.Empty;
        public static string argCarFullNum = string.Empty;

        public static (string carcd, string carfullnum) returnCarSelect(string carnum)
        {
            try
            {
                string SQL = "SELECT DRIVERNAME, REPLACE(VEHICLENO, ' ', '') AS VEHICLENO, VEHICLECODE, CARRIERNAME " +
                             "FROM TMS_INPUT_CARMASTER_CON " +
                             $"WHERE REPLACE(VEHICLENO, ' ', '') LIKE '%{carnum}' ";

                DataSet carInfoDs = Dbconn.conn.ExecutDataset(SQL);

                string returnCarCd = string.Empty;
                string returnCarFullNum = string.Empty;

                if (Dbconn.conn.getRowCnt(carInfoDs) == 1)
                {
                    returnCarCd = Dbconn.conn.getData(carInfoDs, "VEHICLECODE", 0);
                    returnCarFullNum = Dbconn.conn.getData(carInfoDs, "VEHICLENO", 0);
                }
                else
                {
                    returnCarFullNum = carnum;
                }

                return (returnCarCd, returnCarFullNum);
            }
            catch (Exception ex)
            {
                return ("", carnum);
            }
        }
        public static string str2hex(string strData)
        {
            string resultHex = string.Empty;
            byte[] arr_byteStr = Encoding.Default.GetBytes(strData);

            foreach (byte byteStr in arr_byteStr)
                resultHex += string.Format("{0:X2}", byteStr);

            return resultHex;
        }

        public static string[,] itemArray = new string[0, 0];
        public static QrCarInfo qrCarInfo = new QrCarInfo();


        public static string argCarInputType = string.Empty;
        public static string sel_del_no = string.Empty;
        public class QrCarInfo
        {
            //차량전체번호
            public string CarFullNum { get; set; }
            public string EBELN { get; set; }
            public string ESART { get; set; }
            public string SRM_PREVB_GR { get; set; }
            public string PARTNER { get; set; }
            public string EKORG { get; set; }
            public string SHIP_NAME { get; set; }
            public string INVOICE_WEIGHT { get; set; }

        }
    }
}