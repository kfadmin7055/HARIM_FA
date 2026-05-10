using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class clsShipErpSql
    {
        public static void returnCarInfo(string VEHICLENO, out string car_no, out string del_emp, out string del_tel, out string VEHICLEGROUPCODE)
        {
            string SQL = string.Empty;
            SQL = $"SELECT CAR_NO, DRIVERNAME , DRIVERMOBILE , VEHICLEGROUPCODE FROM CARS WHERE VEHICLENO = '{VEHICLENO}' ";
            using (DataSet carinfo_ds = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(carinfo_ds) < 1)
                {
                    car_no = "";
                    del_emp = "";
                    del_tel = "";
                    VEHICLEGROUPCODE = "";
                }else
                {
                    car_no = Dbconn.conn.getData(carinfo_ds, "CAR_NO", 0).Trim();
                    del_emp = Dbconn.conn.getData(carinfo_ds, "DRIVERNAME", 0).Trim();
                    del_tel = Dbconn.conn.getData(carinfo_ds, "DRIVERMOBILE", 0).Trim();
                    VEHICLEGROUPCODE = Dbconn.conn.getData(carinfo_ds, "VEHICLEGROUPCODE", 0).Trim();
                }

            }
        }


        public static DataSet returnCarNoInfo(string car_no)
        {
            string SQL = string.Empty;
            SQL = $"SELECT CAR_NO, VEHICLENO, DRIVERNAME , DRIVERMOBILE , VEHICLEGROUPCODE, CAR_PWD FROM CARS WHERE CAR_NO = '{car_no}' ";
            using (DataSet carinfo_ds = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(carinfo_ds) > 1)
                {
                    return carinfo_ds;
                }

                return carinfo_ds;
            }
        }

        public static DataSet returnCustInfo(string cust_no)
        {
            string SQL = string.Empty;
            SQL = $"SELECT CUST_NO, CUST_NAME, ADDR , TEL_NO  FROM CUSTOMERS WHERE CUST_NO = '{cust_no}' ";
            using (DataSet custinfo_ds = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(custinfo_ds) > 1)
                {
                    return custinfo_ds;
                }
                return custinfo_ds;
            }
        }

        //출하입차정보 V_MES_ATG_113_1
        public static string InsertCarInInfo(string del_no, string VEHICLENO, string crud)
        {
            string SQL = string.Empty;

            string car_no = string.Empty;
            string del_emp = string.Empty;
            string del_tel = string.Empty;
            string VEHICLEGROUPCODE = string.Empty;

            returnCarInfo(VEHICLENO, out car_no, out del_emp, out del_tel, out VEHICLEGROUPCODE);


            if (string.IsNullOrEmpty(car_no))
            {
                clsLog.logSave("clsShipErpSql", "InsertCarInInfo", "NO CAR INFO : " + VEHICLENO);
                return "차량정보를 찾을 수 없습니다";
            }

            SQL = $" INSERT INTO {"clsCommon.erp_bulk_db_name"}.dbo.Z_MES_ATG_308_1 " +
                " (delivery_no, car_no, VEHICLENO, DRIVERNAME, DRIVERMOBILE, ship_to_dt, interface_status, status_code) " +
                " VALUES(" +
                "'{0}', " + //delivery_no (배차번호)
                "'{1}', " + //car_no (차량코드)
                "'{2}', " + //VEHICLENO (차량번호)
                "'{3}', " + //DRIVERNAME (배송기사)
                "'{4}', " + //DRIVERMOBILE (배송기사연락처)
                "SYSDATE, " + //ship_to_dt (입차시간)
                "'{5}', " +  //interface_status
                "'{6}'  " + //status_code
                ") ";

            SQL = string.Format(SQL,
                    del_no, // 0 : 배차번호
                    car_no, // 1 : 차량코드
                    VEHICLENO, // 2 : 차량번호
                    del_emp, // 3: 배송기사
                    del_tel, // 4: 배송기사연락처
                    "0",
                    crud
                );

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave("clsShipErpSql", "InsertCarInInfo", SQL);
                return "차량입차정보 입력을 실패했습니다(ERP)";
            }
            
            return "OK";
        }


        public static string InsertCarOutHeader(string del_no,  string order_no, string order_line,  string part_no, string qty, string weight, string VEHICLENO )
        {
            string SQL = string.Empty;

            string car_no = string.Empty;
            string del_emp = string.Empty;
            string del_tel = string.Empty;
            string VEHICLEGROUPCODE = string.Empty;

            returnCarInfo(VEHICLENO, out car_no, out del_emp, out del_tel, out VEHICLEGROUPCODE);

            if (string.IsNullOrEmpty(car_no))
            {
                clsLog.logSave("clsShipErpSql", "InsertCarOutHeader", "NO CAR INFO : " + VEHICLENO);
                return "차량정보를 찾을 수 없습니다";
            }

            SQL =
                $"INSERT INTO {"clsCommon.erp_bulk_db_name"}.DBO.Z_MES_ATG_305_1 " +
                "(DELIVERY_NO, ORDER_NO, ORDER_LINE_SKEY, PART_NO, QTY, WEIGHT, CAR_NO, VEHICLENO, DRIVERNAME, DRIVERMOBILE, " +
                "SHIP_FROM_DT, INTERFACE_STATUS, STATUS_CODE )  " +
                "VALUES ( " +
                "'{0}',  " + //DELIVERY_NO
                "'{1}',  " + //ORDER_NO
                "'{2}',  " + //ORDER_LINE_SKEY
                "'{3}',  " + //PART_NO
                "'{4}',  " + //QTY
                "'{5}',  " + //WEIGHT
                "'{6}',  " + //CAR_NO
                "'{7}',  " + //VEHICLENO
                "'{8}',  " + //DRIVERNAME
                "'{9}',  " + //DRIVERMOBILE
                " SYSDATE,  " + //SHIP_FROM_DT
                "'0',  " + //INTERFACE_STATUS
                "'I' " + //STATUS_CODE
                " )";

            SQL = string.Format(SQL,
                        del_no,         // 0 : 배차번호
                        order_no,       // 1 : 영업주문서번호
                        order_line,     // 2 : 라인일련번호
                        part_no,        // 3 : 품목코드
                        qty,            // 4 : 수량
                        weight,         // 5 : 계근중량
                        car_no,         // 6 : 차량코드
                        VEHICLENO,    // 7 : 차량번호
                        del_emp,        // 8 : 배송기사
                        del_tel         // 9 : 배송기사 연락처
            );

            if (Dbconn.conn.SQLrun(SQL) < 1)
            {
                clsLog.logSave("clsShipErpSql", "InsertCarOutHeader", SQL);
                return "차량출하헤더 입력을 실패했습니다(ERP)";
            }

            return "OK";
        }
    }
}
