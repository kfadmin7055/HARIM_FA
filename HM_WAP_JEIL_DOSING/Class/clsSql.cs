using Core.Class;
using System;
using System.Data;

namespace HARIM_FA_DOSING
{
    public class clsSql
    {
        public static DataSet GetAuthDataSet(string FORM_NAME = "")
        {
            string SQL = null;

            if (FORM_NAME == "")
            {
                SQL = $@"
                SELECT 
                    A.PROGRAM_ID,
                    A.MENU_ID,
                    A.SCR_ID,
                    B.FORM_NAME,
                    C.MENU_NM,
                    B.SCR_NM,
                    D.COMM_DTNM AS MANAGE_TYPE,
                    A.READ_ATT,
                    A.WRITE_ATT,
                    A.DELETE_ATT,
                    A.UPDATE_ATT,
                    A.I_TIME,
                    D.COMM_DTCODE
                FROM 
                    SC_ATTRIBUTION A
                    JOIN SCR_MG B 
                        ON A.SCR_ID = B.SCR_ID 
                        AND A.PROGRAM_ID = B.PROGRAM_ID 
                        AND A.MENU_ID = B.MENU_ID
                    JOIN MENU_MG C 
                        ON A.MENU_ID = C.MENU_ID
                    JOIN COMM_DTCODE D 
                        ON A.MANAGE_TYPE = D.COMM_DTCODE
                WHERE 
                    A.MANAGE_TYPE = '{clsCommon.UserType}'
                ORDER BY 
                    A.MENU_ID, 
                    B.DISPLAY_SEQ
                ";
            }
            else
            {
                SQL = $@"
                SELECT 
                    A.PROGRAM_ID,
                    A.MENU_ID,
                    A.SCR_ID,
                    B.FORM_NAME,
                    C.MENU_NM,
                    B.SCR_NM,
                    D.COMM_DTNM AS MANAGE_TYPE,
                    A.READ_ATT,
                    A.WRITE_ATT,
                    A.DELETE_ATT,
                    A.UPDATE_ATT,
                    A.I_TIME,
                    D.COMM_DTCODE
                FROM 
                    SC_ATTRIBUTION A
                    JOIN SCR_MG B ON A.SCR_ID = B.SCR_ID AND A.PROGRAM_ID = B.PROGRAM_ID AND A.MENU_ID = B.MENU_ID
                    JOIN MENU_MG C ON A.MENU_ID = C.MENU_ID
                    JOIN COMM_DTCODE D ON A.MANAGE_TYPE = D.COMM_DTCODE
                WHERE 
                    A.MANAGE_TYPE = '{clsCommon.UserType}'
                    AND B.FORM_NAME = '{FORM_NAME.Trim()}'
                ORDER BY 
                    A.MENU_ID, 
                    B.DISPLAY_SEQ
                ";
            }

            DataSet resultDs = Dbconn.conn.ExecutDataset(SQL);
            return resultDs;
        }
    }
}
