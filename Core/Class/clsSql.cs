using Org.BouncyCastle.Utilities;
using System;
using System.Data;

namespace Core.Class
{
    public class clsSql
    {
        public static string ResuntIsNo()
        {
            string is_no = string.Empty;

            /* 시퀀스가 없을 시 시퀀스 만들기
            CREATE SEQUENCE IS_NO_SEQ
            START WITH 1
            INCREMENT BY 1
            NOCACHE
            NOCYCLE; 
            */
            string SQL = "SELECT IS_NO_SEQ.NEXTVAL AS SEQ FROM DUAL";
            using (DataSet isnoDs = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(isnoDs) > 0)
                {
                    is_no = Dbconn.conn.getData(isnoDs, "SEQ", 0);
                }
            }

            return is_no;
        }

        public static bool InsertWeight(string inout_type, string carnum, string weight)
        {
            string SQL = "select * from INOUT_WEIGHT where INOUT_TYPE = '{0}' AND CAR_FULL_NUM = '{1}' AND DATEDIFF(SECOND, getdate(), I_TIME) > -200 ";
            SQL = string.Format(SQL, inout_type, carnum);

            DataSet inChkDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(inChkDs) == 0 )
            {
                SQL =
                "INSERT INTO INOUT_WEIGHT(INOUT_TYPE, CAR_FULL_NUM, INOUT_WEIGHT, I_TIME) " +
                "VALUES ('{0}','{1}','{2}',getdate()) ";
                SQL = string.Format(SQL,
                    inout_type,
                    carnum,
                    weight
                    );

                if (Dbconn.conn.SQLrun(SQL) < 0)
                {
                    return false;
                }

            }

            return true;
        }


        public static string CheckIsNo(string iv_no)
        {
            string ivNo = string.Empty;

            string SQL = $"SELECT IS_NO FROM WAP_GOCAR WHERE IV_NO = '{iv_no}'";
            
            DataSet ivDs = Dbconn.conn.ExecutDataset(SQL);

            return Dbconn.conn.getData(ivDs, "IS_NO", 0).Trim();
        }

        public static void EndWapDecar(string car_no)
        {
            string SQL = $"UPDATE WAP_DECAR SET PC_STATUS = '9' WHERE INCAR_NO LIKE '%{car_no}%' AND PC_STATUS IN ('0', '1') ";
            
            Dbconn.conn.SQLrun(SQL);
        }

        public static bool InCarStep0(string is_no, string cust_code, string car_no, string inout_type, string tdetail, string d_hp, string sWeight)
        {
            //string SQL = " INSERT INTO WAP_DECAR(CHKIN_TIME, PC_STATUS, CUST_CODE, D_HP) " +
            //" VALUES(CONVERT(DATETIME, '{4}'), '0', '{5}','{6}') ";
            //SQL = string.Format(SQL,
            //    is_no,
            //    car_no,
            //    inout_type,
            //    tdetail,
            //    DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"),
            //    cust_code,
            //    d_hp
            //    );

            string SQL = $@"
            INSERT INTO WAP_DECAR (
               IS_NO, CUST_CODE, INCAR_NO, 
               CAR_TYPE, CAR_TDETAIL, 
               IN_WEIGHT, 
               USER_ID, INCAR_DATE, INCAR_TIME, 
               CHKIN_TIME,
               PC_STATUS, ORDER_NO, U_TIME, U_USER) 
            VALUES ( 
                '{is_no}', '{cust_code}', '{car_no}',
                '{inout_type}', '{tdetail}',
                '{sWeight}',
                'InCarStep0', SYSDATE, SYSDATE,
                TO_DATE('{DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")}', 'YYYY-MM-DD HH24:MI:SS'),
                '0', '', SYSDATE, 'SYSTEM')
            ";

            if (Dbconn.conn.SQLrun(SQL) < 0)
            {
                clsLog.logSave("clsSql", "InCarStep0", SQL);
                return false;
            }
            return true;
        }


    }
}
