namespace Core.Class
{
    public class Dbconn
    {
        //TEST SERVER
        //public static clsDBManager conn = new clsDBManager("server=211.46.7.109,7058; uid=sa; pwd=roqkfxla!23; database=at_ochang;");
        //public static clsDBManager erp_conn = new clsDBManager("server=211.46.7.109,7058; uid=sa; pwd=roqkfxla!23; database=at_ochang;");

        ////MES DB 접속자
        //public static clsDBManager conn = new clsDBManager("server=172.16.10.20; uid=atochang; pwd=Ochang!@3; database=at_ochang;");
        
        public static string GetConn()
        {
            // 한국제일
            string dbKFConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=211.46.7.24)(PORT=1521))(CONNECT_DATA=(SID=ORAKF)));user id=HARIMOERP;password=HARIMOERP;";
            //string dbKFConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.10.253)(PORT=1521))(CONNECT_DATA=(SID=fadev)));user id=HARIM_OP;password=tkdwnrhdwkd;";
            // 대전 테스트
            string dbJEILDJTestConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=170.170.103.71)(PORT=1521))(CONNECT_DATA=(SID=FADEV1)));user id=daejeon;password=dae_roqkfwk_2025!~;";
            // 대전 리얼
            string dbJEILDJRealConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=170.170.100.49)(PORT=21521))(CONNECT_DATA=(SID=DAEJEON)));user id=daejeon;password=dae_wkrdjqwk_2025!~;";
            // 김제
            string dbGIMJEConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.10.117.233)(PORT=1521))(CONNECT_DATA=(SID=fadev)));user id=HARIM_KJ;password=rlawprhdwkd;";
            // 정읍 사무
            string dbJEONGEUPConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.10.112.233)(PORT=1521))(CONNECT_DATA=(SID=fadev)));user id=HARIM_JE;password=WJDDMQRHDWKD;";
            // 정읍 배합
            string dbJEONGEUPPLCConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.1.1.33)(PORT=1521))(CONNECT_DATA=(SID=fadev)));user id=HARIM_JE;password=WJDDMQRHDWKD;";
            // 상주 사무
            string dbSANGJUConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.10.204.79)(PORT=1521))(CONNECT_DATA=(SID=fadev)));user id=HARIM_OP;password=tkdwnrhdwkd;";
            // 상주 배합
            string dbSANGJUPLCConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.10.253)(PORT=1521))(CONNECT_DATA=(SID=fadev)));user id=HARIM_OP;password=tkdwnrhdwkd;";
            // 함안 테스트
            string dbHAMANTestConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=170.170.103.71)(PORT=1521))(CONNECT_DATA=(SID=FADEV1)));user id=haman;password=ham_roqkfwk_2025!~;";
            // 함안 리얼
            string dbHAMANRealConn = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=170.170.250.50)(PORT=21521))(CONNECT_DATA=(SID=HAMAN)));user id=haman;password=ham_wkrdjqwk_2025!~;";

            string dbConn = string.Empty;

            if (clsCommon.PlantCode.ToString() == "PJ01")
            {
                if (clsCommon.PLCOnly)
                {
                    dbConn = dbJEILDJTestConn;
                    clsCommon.DbLocation = "JUNGBU1 TEST";
                }
                else
                {
                    dbConn = dbJEILDJRealConn;
                    clsCommon.DbLocation = "JUNGBU1";
                }
            }
           else if (clsCommon.PlantCode.ToString() == "PJ02")
           {
                if (clsCommon.PLCOnly)
                {
                    dbConn = dbJEILDJTestConn;
                    clsCommon.DbLocation = "JUNGBU2 TEST";
                }
                else
                {
                    dbConn = dbJEILDJRealConn;
                    clsCommon.DbLocation = "JUNGBU2";
                }
            }
            else if (clsCommon.PlantCode.ToString() == "P101")
            { 
                dbConn = dbGIMJEConn;
                clsCommon.DbLocation = "GIMJE";
            }
            else if (clsCommon.PlantCode.ToString() == "P102")
            {
                if (clsCommon.PLCOnly)
                {
                    dbConn = dbJEONGEUPPLCConn;
                    clsCommon.DbLocation = "JEONGEUP PLC Only";
                }
                else
                {
                    dbConn = dbJEONGEUPConn;
                    clsCommon.DbLocation = "JEONGEUP";
                }
                clsCommon.DbLocation = "JEONGEUP";
            }
            else if (clsCommon.PlantCode.ToString() == "P201")
            {
                if (clsCommon.PLCOnly)
                {
                    dbConn = dbSANGJUPLCConn;
                    clsCommon.DbLocation = "SANGJU PLC Only";
                }
                else
                {
                    dbConn = dbSANGJUConn;
                    clsCommon.DbLocation = "SANGJU";
                }                
            }
            else if (clsCommon.PlantCode.ToString() == "PJ04")
            {
                if (clsCommon.PLCOnly)
                {
                    dbConn = dbHAMANTestConn;
                    clsCommon.DbLocation = "HAMAN1 TEST";
                }
                else
                {
                    dbConn = dbHAMANRealConn;
                    clsCommon.DbLocation = "HAMAN1";
                }
            }
            else if (clsCommon.PlantCode.ToString() == "PJ05")
            {
                if (clsCommon.PLCOnly)
                {
                    dbConn = dbHAMANTestConn;
                    clsCommon.DbLocation = "HAMAN2 TEST";
                }
                else
                {
                    dbConn = dbHAMANRealConn;
                    clsCommon.DbLocation = "HAMAN2";
                }
            }
            else
            {
                dbConn = dbKFConn;
                clsCommon.DbLocation = "KFIRST";
            }

            return dbConn;
        }

        //MES DB 접속자
        public static clsDBManager conn = new clsDBManager(GetConn());

        //public static clsDBManager conn = new clsDBManager("data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.18.3)(PORT=1521))(CONNECT_DATA=(SID=ORAKF)));user id=HARIMOERP;password=HARIMOERP;");

        //ERP DB 접속자
        public static clsDBManager erp_conn = new clsDBManager("server=172.16.10.10,17001; uid=mes_user; pwd=a2nt0yg2tm2g@e@s#; database=ATL_MFG;");

        // ERP DB 접속자
        //public static clsDBManager erp_conn = new clsDBManager("data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=211.46.7.24)(PORT=1521))(CONNECT_DATA=(SID=ORAKF)));user id=HM_JEIL_ERP;password=kfirst7055;");
    }
}