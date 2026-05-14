using System.Data;
using System;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Linq;
using DevExpress.XtraEditors;
using Core.Extension;
using System.Collections.Generic;

namespace Core.Class
{
    public static class clsCommon
    {
        //프로그램 시작경로 ROOT
        public static string gsCurrentDirectory = Application.StartupPath;

        //설정파일 경로 (ini)
        public static string tPath = gsCurrentDirectory + "\\setting.ini";

        public static string _strProgramName;

        public static string _strUserId;
        public static string _strUserName;
        public static string _strUserType;
        public static string _strMainPlcConnYn;
        public static string _strSubPlcConnYn;
        public static string _strPlantName;
        public static string _strPlantCode;
        public static string _strProcessCode;
        public static string _strDbLocation;
        public static string _strBarcodeConnYn;
        public static bool _strPLCOnly;

        //message box font size
        public static string strHtmlFontSize_M = "17";
        public static string strHtmlFontSize_S = "12";

        public static string argTyping = string.Empty;

        public static string plc_scale_ip = "172.16.5.50";

        public static string led_board_top_ip = "192.168.0.101";
        public static string led_board_down_ip = "192.168.0.100";

        public const int _statusMsg_time = 5000; //단위 ms
        public const int _cameraMsgDel_time = 60000; //단위 ms

        public static string vPLCAddress;
        public static int vPLCUnit;
        public static int vPLCDataCount;

        public static class PcStatus
        {
            // 계획
            public const string Plan = "031002";
            // 진행
            public const string InProgress = "031003";
            // 완료
            public const string Completed = "031004";
            // 취소
            public const string Canceled = "031005";
            // 강제완료
            public const string ForceCompleted = "031006";
            // 보류
            public const string OnHold = "031007";
        }

        /// <summary>
        /// 프로그램명
        /// </summary>
        public static string ProgramName
        {
            get
            {
                return _strProgramName;
            }
            set
            {
                _strProgramName = value;
            }
        }

        /// <summary>
        /// 유저 ID
        /// </summary>
        public static string UserId
        {
            get
            {
                return _strUserId;
            }
            set
            {
                _strUserId = value;
            }
        }

        /// <summary>
        /// 유저 이름
        /// </summary>
        public static string UserName
        {
            get
            {
                return _strUserName;
            }
            set
            {
                _strUserName = value;
            }
        }

        /// <summary>
        /// 유저 Type
        /// </summary>
        public static string UserType
        {
            get
            {
                return _strUserType;
            }
            set
            {
                _strUserType = value;
            }
        }

        /// <summary>
        /// PLC 연결여부
        /// </summary>
        public static string MainPlcConnYn
        {
            get
            {
                return _strMainPlcConnYn;
            }
            set
            {
                _strMainPlcConnYn = value;
            }
        }

        /// <summary>
        /// PLC 연결여부
        /// </summary>
        public static string SubPlcConnYn
        {
            get
            {
                return _strSubPlcConnYn;
            }
            set
            {
                _strSubPlcConnYn = value;
            }
        }

        /// <summary>
        /// 바코드 연결여부
        /// </summary>
        public static string BarcodeConnYn
        {
            get
            {
                return _strBarcodeConnYn;
            }
            set
            {
                _strBarcodeConnYn = value;
            }
        }

        /// <summary>
        /// 플랜트 코드
        /// </summary>
        public static string PlantName
        {
            get
            {
                return _strPlantName;
            }
            set
            {
                _strPlantName = value;
            }
        }

        /// <summary>
        /// 플랜트 코드
        /// </summary>
        public static string DbLocation
        {
            get
            {
                return _strDbLocation;
            }
            set
            {
                _strDbLocation = value;
            }
        }

        /// <summary>
        /// PLC 전용
        /// </summary>
        public static bool PLCOnly
        {
            get
            {
                return _strPLCOnly;
            }
            set
            {
                _strPLCOnly = value;
            }
        }

        /// <summary>
        /// 플랜트 코드
        /// </summary>
        public static string PlantCode
        {
            get
            {
                return _strPlantCode;
            }
            set
            {
                _strPlantCode = value;
            }
        }

        /// <summary>
        /// 공정 코드
        /// </summary>
        public static string ProcessCode
        {
            get
            {
                return _strProcessCode;
            }
            set
            {
                _strProcessCode = value;
            }
        }

        public static void SetLogo(PictureEdit logo, string sPlant)
        {
            if (sPlant == "하림")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.하림Ci_국문;
            }
            else if (sPlant == "올품")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.올품로고;
            }
            else if (sPlant == "제일")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.CJS_Logo_300X120;
            }
        }

        public static void SetSplashLogo(PictureEdit logo, string sPlant)
        {
            if (sPlant == "하림")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.하림Ci_국문;
            }
            else if (sPlant == "올품")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.올품_CI;
            }
            else if (sPlant == "제일")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.CJFSP;
            }
        }

        public static void SetLoginLogo(PictureEdit logo, string sPlant)
        {
            if (sPlant == "하림")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.HarimLogin;
            }
            else if (sPlant == "올품")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.올품Login;
            }
            else if (sPlant == "제일")
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.제일사료Login;
            }
            else
            {
                logo.Properties.ShowMenu = false;  // 우클릭 메뉴 비활성화 (선택사항)
                logo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
                logo.Image = Core.Properties.Resources.제일사료Login;
            }
        }

        //폼 권한 조회
        public static bool Auth_Form_Function(DataSet autoDs, String grant)
        {
            try
            {
                if (clsCommon._strUserType == "admin")
                {
                    return true;
                }

                if (autoDs.Tables[0].Rows.Count <= 0)
                {
                    return false;
                }

                string col_name = string.Empty;

                switch (grant)
                {
                    case "R": col_name = "READ_ATT"; break;
                    case "W": col_name = "WRITE_ATT"; break;
                    case "D": col_name = "DELETE_ATT"; break;
                    case "U": col_name = "UPDATE_ATT"; break;
                }

                if (autoDs.Tables[0].Rows[0][col_name].ToString() == "Y")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string GetMaxSeq(string sTableName, string sColumn, Dictionary<string, string> dicWhere)
        {
            string sReturnSeq = string.Empty;
            string sWhere = string.Empty;

            foreach (var item in dicWhere)
            {
                sWhere += $@"{item.Key} = '{item.Value}'
                ";
            }

            string SQL = $@"
            SELECT NVL(MAX({sColumn}) + 1, 1) AS SEQ
            FROM {sTableName}
            WHERE {sWhere}  
            ";

            using (DataSet Ds = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(Ds) == 0)
                {
                    clsLog.logSave($"작업순번 생성에러 / {sColumn} AS SEQ : " + SQL, 0);
                    return string.Empty;
                }

                sReturnSeq = Dbconn.conn.getData(Ds, "SEQ", 0);
                Ds.Dispose();
            }

            return sReturnSeq;
        }

        /// <summary>
        /// 작업지시에서 사용할 품목 리스트
        /// </summary>
        /// <param name="sPlantCode">플랜트</param>
        /// <param name="sProcessKey">공정코드</param>
        /// <param name="sRESOURCE_TYPE">공정타입</param>
        /// <param name="sRESOURCE_NO">원료코드</param>
        /// <param name="confirm">BOM 확정여부</param>
        /// <param name="codeView">코드 보임여부 (true : 0000001 : 품목명)</param>
        /// <param name="suView"></param>
        /// <param name="uom">품목 단위 (EA, KG 등 받은 단위의 품목만 보임)</param>
        /// <param name="sUseBin">빈에 장착된 품목만 보임 여부</param>
        /// <returns></returns>
        public static DataTable GetResource(string sPlantCode = "", string siProcessKey = "", string sRESOURCE_TYPE = "", string sRESOURCE_NO = "", int confirm = 0, bool codeView = false, bool suView = false, string uom = "", bool sUseBin = false, bool sBIYn = false, bool bUseYn = false)
        {
            string nullValue = string.Empty;
            string STLST = string.Empty;
            string sConfirm = string.Empty;
            string NameDisplay = string.Empty;
            string suDisplay = string.Empty;
            string whereUOM = string.Empty;
            string sJoinBin = string.Empty;
            string sJoinBI = string.Empty;
            string soProcessKey = string.Empty;
            string sUseYn = string.Empty;
            string sFert = string.Empty;

            soProcessKey = siProcessKey;

            if (siProcessKey == GetProcessKey("타이콘", sPlantCode))
                soProcessKey = GetProcessKey("벌크", sPlantCode);
            else if (siProcessKey == GetProcessKey("벌크 원료", sPlantCode))
                soProcessKey = GetProcessKey("PF배합", sPlantCode);

            // 품목 타입
            if (soProcessKey != GetProcessKey("포장", sPlantCode) && sRESOURCE_TYPE.Contains(clsCommon.GetResourceTypeCode("포장재료")))
                nullValue = $"AND a.RESOURCE_TYPE NOT IN ({sRESOURCE_TYPE})";
            else if (sRESOURCE_TYPE != null && !sRESOURCE_TYPE.Equals(""))
                nullValue = $"AND a.RESOURCE_TYPE IN ({sRESOURCE_TYPE})";

            if (sRESOURCE_TYPE.Replace("'", "") == clsCommon.GetResourceTypeCode("제품"))
            {
                sFert = @"
                AND (a.JD_GUBUN = 'E' OR (a.RESOURCE_NO IN (SELECT RESOURCE_NO FROM SAP_IN_BOM_CONM WHERE PLANT_CODE = a.PLANT_CODE)))

                AND NVL(a.DEL_YN, 'N') = 'N'
                ";
            }

            // 확정 배합비만 가져오기
            if (confirm == 1)
                STLST = "AND P_TYPE IN ('1', '2')";
            if (confirm == 2)
                STLST = "AND P_TYPE = '2' AND STLST = '2'";

            if (!string.IsNullOrEmpty(soProcessKey) && confirm > 0)
            {
                if (soProcessKey == GetProcessKey("수동생산"))
                {
                    soProcessKey = null;
                    uom = "";
                }

                string rSQL = $@"
                SELECT 1 FROM SAP_DI_ROUTING WHERE ARBPL = '{soProcessKey}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(rSQL);

                if (Dbconn.conn.getRowCnt(ds) > 0)
                    sConfirm = "AND c.RESOURCE_NO = a.RESOURCE_NO";
            }

            // 품목코드 : 품목명
            if (codeView)
                NameDisplay = "a.RESOURCE_NO || ':' || a.DESCRIPTION";
            else
                NameDisplay = "a.DESCRIPTION";

            // 수불코드
            if (suView)
                suDisplay = "AND a.SU_CODE IS NULL";

            // 단위
            if (!uom.Equals(""))
                whereUOM = $"AND a.UOM = '{uom}'";

            if (sUseBin)
                sJoinBin = "AND d.RESOURCE_NO = a.RESOURCE_NO";

            if (siProcessKey == GetProcessKey("벌크 원료", sPlantCode) && sBIYn)
                sJoinBI = "AND a.BI_YN = 'Y'";

            if (bUseYn)
                sUseYn = "AND NVL(a.USE_YN, 'Y') != 'N'";

            //if (new string[] { "PJ01", "PJ02", "PJ04", "PJ05" }.Contains(sPlantCode))
            string SQL = $@"
            WITH BOM AS (
                SELECT DISTINCT A.PLANT_CODE, A.P_TYPE, A.STLST, A.RESOURCE_NO, A.NOTE, A.DATUV, A.DATUV_TO
                FROM SAP_IN_BOM_CONM A
                    LEFT JOIN SAP_DI_ROUTING b ON b.PLANT_CODE = A.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE ('{sPlantCode}' IS NULL OR a.PLANT_CODE = '{sPlantCode}')
                    AND ('{sRESOURCE_NO}' IS NULL OR a.RESOURCE_NO LIKE '{sRESOURCE_NO}%')
                    {STLST}
                    AND (('{soProcessKey}' IS NULL OR ('{sPlantCode}' NOT IN ('PJ01', 'PJ02', 'PJ04', 'PJ05'))) OR b.ARBPL = '{soProcessKey}')
            ), BIN_RESOURCE AS (
                SELECT PLANT_CODE, RESOURCE_NO
                FROM BIN
                WHERE PLANT_CODE = '{sPlantCode}'
                AND PROCESS_KEY IN ('{soProcessKey}', '{clsCommon.GetProcessKey("인테이크 사이로")}')
            )
            
            -- 품목 리스트
            SELECT DISTINCT a.RESOURCE_NO AS CODE, {NameDisplay} AS NAME, b.COMM_DTCODE ||':'|| b.COMM_DTNM AS TYPE, a.UOM
            FROM SAP_DI_PRODUCT a
                LEFT JOIN BOM c ON c.RESOURCE_NO = a.RESOURCE_NO
                LEFT JOIN COMM_DTCODE b ON b.WK_DIVCODE = '10' AND b.COMM_CODE = '02' AND b.COMM_DTCODE = a.RESOURCE_TYPE
                LEFT JOIN BIN_RESOURCE d ON d.RESOURCE_NO = a.RESOURCE_NO
            WHERE 1 = 1
                AND ('{sPlantCode}' IS NULL OR a.PLANT_CODE = '{sPlantCode}')
                {suDisplay}
                AND ('{sRESOURCE_NO}' IS NULL OR a.RESOURCE_NO LIKE '{sRESOURCE_NO}%')
                {sUseYn}
                {nullValue}
                {whereUOM}
                {sConfirm}
                {sJoinBin}
                {sJoinBI}
                {sFert}
            ORDER BY {NameDisplay}
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 자동전송여부
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBinType()
        {
            string SQL = $@"
            -- 빈타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '23'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 스마트오더에서 사용할 품목 리스트
        /// </summary>
        /// <param name="sVehicleNo"></param>
        /// <param name="sPartner"></param>
        /// <param name="sPlantCode"></param>
        /// <param name="sRESOURCE_TYPE"></param>
        /// <param name="confirm"></param>
        /// <param name="codeView"></param>
        /// <returns></returns>
        public static DataTable GetResourceBySmorder(string sVehicleNo, string sPartner, string sPlantCode = "", string sRESOURCE_TYPE = "", int confirm = 0, bool codeView = false)
        {
            string nullValue = string.Empty;
            string STLST = string.Empty;
            string NameDisplay = string.Empty;

            // 품목 타입
            if (sRESOURCE_TYPE.Contains(clsCommon.GetResourceTypeCode("포장재료")))
                nullValue = $"AND a.RESOURCE_TYPE NOT IN ({sRESOURCE_TYPE})";
            else if (sRESOURCE_TYPE != null && !sRESOURCE_TYPE.Equals(""))
                nullValue = $"AND a.RESOURCE_TYPE IN ({sRESOURCE_TYPE})";

            // 확정 배합비만 가져오기
            if (confirm == 1)
                STLST = "AND P_TYPE IN ('1', '2')";
            if (confirm == 2)
                STLST = "AND P_TYPE = '2' AND STLST = '2'";

            // 품목코드 : 품목명
            if (codeView)
                NameDisplay = "a.RESOURCE_NO || ':' || a.DESCRIPTION";
            else
                NameDisplay = "a.DESCRIPTION";

            //if (new string[] { "PJ01", "PJ02", "PJ04", "PJ05" }.Contains(sPlantCode))
            string SQL = $@"
            WITH BOM AS (
                SELECT DISTINCT A.PLANT_CODE, A.P_TYPE, A.STLST, A.RESOURCE_NO, A.NOTE, A.DATUV, A.DATUV_TO
                FROM SAP_IN_BOM_CONM A
                    LEFT JOIN SAP_DI_ROUTING b ON b.PLANT_CODE = A.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE SYSDATE BETWEEN TO_DATE(A.DATUV, 'YY/MM/DD') AND TO_DATE(A.DATUV_TO, 'YY/MM/DD')
                    AND ('{sPlantCode}' IS NULL OR a.PLANT_CODE = '{sPlantCode}')
                    {STLST}
            )

            -- 품목 리스트
            SELECT DISTINCT a.RESOURCE_NO AS CODE, {NameDisplay} AS NAME, b.COMM_DTCODE ||':'|| b.COMM_DTNM AS TYPE, a.UOM
            FROM SAP_DI_PRODUCT a
                LEFT JOIN BOM c ON c.RESOURCE_NO = a.RESOURCE_NO
                LEFT JOIN COMM_DTCODE b ON b.WK_DIVCODE = '10' AND b.COMM_CODE = '02' AND b.COMM_DTCODE = a.RESOURCE_TYPE
                LEFT JOIN PRODUCT_SMORDER e ON e.PLANT_CODE = a.PLANT_CODE AND e.VEHICLENO = '{sVehicleNo}' AND e.PARTNER = '{sPartner}'
            WHERE 1 = 1
                AND ('{sPlantCode}' IS NULL OR a.PLANT_CODE = '{sPlantCode}')
                {nullValue}
            ORDER BY {NameDisplay}
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }


        public static DataTable GetPremix(string sPlantCode)
        {
            string SQL = $@"
            SELECT RESOURCE_NO AS CODE, RESOURCE_NO || ' : ' || DESCRIPTION AS NAME
            FROM SAP_DI_PRODUCT
            where RESOURCE_NO LIKE 'P%'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        public static string GetResourceName(string resourceNo)
        {
            string SQL = $@"
            -- 공정코드
            SELECT DESCRIPTION
            FROM SAP_DI_PRODUCT
            WHERE 1 = 1
                AND RESOURCE_NO = '{resourceNo}'
            ";

            string retrunValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["DESCRIPTION"].ToString() : "";

            return retrunValue;
        }

        /// <summary>
        /// 공정
        /// </summary>
        /// <param name="process_code"></param>
        /// <returns></returns>
        public static DataTable GetProcess(string plantCode = "", string processType = "", string process_code = "", string processName = "")
        {
            string is_no = string.Empty;

            string SQL = $@"
            -- 공정 리스트
            SELECT PROCESS_KEY AS CODE, PROCESS_DESC AS NAME
            FROM SAP_PROCESS_DIVISION
            WHERE 1 = 1
                AND ('{plantCode}' IS NULL OR PLANT_CODE = '{plantCode}')
                AND ('{processType}' IS NULL OR PROCESS_TYPE = '{processType}')
                AND ('{process_code}' IS NULL OR PROCESS_KEY = '{process_code}')
                AND ('{processName}' IS NULL OR PROCESS_DESC = '{processName}')
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 공정
        /// </summary>
        /// <param name="process_code"></param>
        /// <returns></returns>
        public static DataTable GetGridProcess(string plantCode = "", string processType = "", string process_code = "", string processName = "")
        {
            string is_no = string.Empty;

            string SQL = $@"
            -- 공정 리스트
            SELECT PROCESS_KEY AS CODE, PROCESS_DESC AS NAME
            FROM SAP_PROCESS_DIVISION
            WHERE 1 = 1
                AND ('{plantCode}' IS NULL OR PLANT_CODE = '{plantCode}')
                AND ('{processType}' IS NULL OR PROCESS_TYPE = '{processType}')
                AND ('{process_code}' IS NULL OR PROCESS_KEY = '{process_code}')
                AND ('{processName}' IS NULL OR PROCESS_DESC = '{processName}')
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 공정
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static string GetProcessKey(string processName, string plantCode = "")
        {
            if ((plantCode == "PJ02" || plantCode == "PJ05") && processName == "펠렛")
                processName = "특수EP 가공";

            string processKey = "@";
            string is_no = string.Empty;

            string SQL = $@"
            -- 공정코드
            SELECT PROCESS_KEY
            FROM SAP_PROCESS_DIVISION
            WHERE 1 = 1
                AND ('{plantCode}' IS NULL OR PLANT_CODE = '{plantCode}') AND PROCESS_DESC = '{processName}'
            ";

            if (Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0)
                processKey = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["PROCESS_KEY"].ToString();

            return processKey;
        }

        #region 공통코드
        /// <summary>
        /// 공통코드(중분류)
        /// </summary>
        /// <param name="sWK_DIVCODE"></param>
        /// <param name="sCOMM_CODE"></param>
        /// <returns></returns>
        public static DataTable GetCOMM_CODE(string sWK_DIVCODE, string sCOMM_CODE)
        {
            string CommCode = string.Empty;

            if (sCOMM_CODE.Contains("'"))
                CommCode = $"AND COMM_CODE IN ({sCOMM_CODE})";
            else if (!sCOMM_CODE.Equals(""))
                CommCode = $"AND COMM_CODE = '{sCOMM_CODE}'";

            string SQL = $@"
            -- 공통코드 중분류
            SELECT COMM_CODE AS CODE, COMM_NM AS NAME
            FROM COMM_CODE
            WHERE WK_DIVCODE = '{sWK_DIVCODE}' {CommCode}
            ORDER BY COMM_CODE
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 공통코드
        /// </summary>
        /// <param name="sWK_DIVCODE"></param>
        /// <param name="sCOMM_CODE"></param>
        /// <returns></returns>
        public static DataTable GetCOMM_DTCODE(string sWK_DIVCODE, string sCOMM_CODE, string sMiddleName = "")
        {
            string CommCode = string.Empty;

            if (!sMiddleName.Equals(""))
                CommCode = $"b.COMM_NM LIKE '%{sMiddleName}%'";
            else if (sCOMM_CODE.Contains("'"))
                CommCode = $"c.WK_DIVCODE = '{sWK_DIVCODE}' AND c.COMM_CODE IN ({sCOMM_CODE})";
            else
                CommCode = $"c.WK_DIVCODE = '{sWK_DIVCODE}' AND c.COMM_CODE = '{sCOMM_CODE}'";

            string SQL = $@"
            -- 공통코드 소분류
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE  {CommCode}
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 설비코드
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSBNo()
        {
            string SQL = $@"
            -- 설비코드
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '15'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 공통코드명으로 코드 찾기
        /// </summary>
        /// <param name="sWK_DIVCODE"></param>
        /// <param name="sCOMM_CODE"></param>
        /// <returns></returns>
        public static string GetCOMM_DTNAME(string sWK_DIVCODE, string sCOMM_CODE, string sName)
        {
            string SQL = $@"
            -- 공통코드 코드찾기
            SELECT COMM_DTCODE AS CODE
            FROM COMM_DTCODE
            WHERE WK_DIVCODE = '{sWK_DIVCODE}' 
                AND COMM_CODE = '{sCOMM_CODE}'
                AND COMM_DTNM = '{sName}'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 상하차 실적유형
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetRTType()
        {
            string SQL = $@"
            -- 상하차 실적유형
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '15'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 상차 마감 여부
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetPDYn()
        {
            string SQL = $@"
            -- 설비코드
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '16'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 자동전송
        /// </summary>
        /// <returns></returns>
        public static string GetAutoTrans()
        {
            string SQL = $@"
                -- 빈타입
                SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
                FROM COMM_DIV a
                    INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                    INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
                WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '01'
                ";

            string returnValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["NAME"].ToString() : "";

            return returnValue;
        }

        /// <summary>
        /// 자동전송
        /// </summary>
        /// <returns></returns>
        public static string GetTransAuto(string sPlantCode, string sPROCESS)
        {
            string SQL = $@"
            SELECT TRANS_GUBUN
            FROM SAP_PROCESS_DIVISION
            WHERE PLANT_CODE = '{sPlantCode}' AND PROCESS_KEY = '{sPROCESS}'
            ";

            string returnValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["TRANS_GUBUN"].ToString() : "";

            return returnValue;
        }

        /// <summary>
        /// 펠렛라인 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPelletLine(string sLocation)
        {
            string SQL = $@"
            -- 빈타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME, c.REF_1 AS REF
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '30'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 재고체크 예외빈 
        /// </summary>
        /// <returns></returns>
        public static string[] GetExeceptionBin()
        {
            string SQL = $@"
            -- 빈타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME, c.REF_1 AS REF
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '35'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            string[] values = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                values[i] = Convert.ToString(dt.Rows[i]["CODE"]);
            }

            return values;
        }

        /// <summary>
        /// 관리타입
        /// </summary>
        /// <returns></returns>
        public static DataTable GetManageType()
        {
            string SQL = $@"
            -- 관리타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '06'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 관리타입
        /// </summary>
        /// <returns></returns>
        public static float GetFontSize()
        {
            float code = 10f;

            string SQL = $@"
            -- 관리타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '60'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
                float.TryParse(ds.Tables[0].Rows[0]["CODE"]?.ToString(), out code);

            return code;
        }

        /// <summary>
        /// 전송구분
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTransFlag()
        {
            string SQL = $@"
            -- 전송구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '11'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 포장 정보
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMAGRV()
        {
            string SQL = $@"
            -- 포장 정보
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '60'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 트레일러 무게
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTR_WEIGHT()
        {
            string SQL = $@"
            -- 트레일러 무게
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '70'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 구매조직코드
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEKORG()
        {
            string SQL = $@"
            -- 구매조직코드
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '06' AND c.COMM_CODE = '01'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 전송구분
        /// </summary>
        /// <returns></returns>
        public static string GetTransFlagCode(string sName)
        {
            string SQL = $@"
            -- 전송구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '11'
                AND c.COMM_DTNM = '{sName}'
            ";

            string returnValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return returnValue;
        }

        /// <summary>
        /// 문서전송구분
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFieldFlag()
        {
            string SQL = $@"
            -- 문서전송구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '12'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 프로그램
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProgram()
        {
            string SQL = $@"
            -- 프로그램
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '20'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 공정 타입
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProcessType(string sName = "")
        {
            string SQL = $@"
            -- 공정 타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '50'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 공정 타입
        /// </summary>
        /// <returns></returns>
        public static string GetProcessTypeCode(string sName = "")
        {
            string SQL = $@"
            -- 공정 타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '01' AND c.COMM_CODE = '50'
            AND COMM_DTNM = '{sName}'
            ";

            string returnValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return returnValue;
        }

        /// <summary>
        /// 포장불량사유
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPackBad()
        {
            string SQL = $@"
            -- 포장불량사유
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '70'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 배합비 수량 변경 원료
        /// </summary>
        /// <returns></returns>
        public static bool GetSettingChangeYn(string sResourceNo)
        {
            string SQL = $@"
            -- 배합비 수량 변경 원료
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '81'
                and c.COMM_DTCODE = '{sResourceNo}'
            ";

            if (Dbconn.conn.getRowCnt(Dbconn.conn.ExecutDataset(SQL)) > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 교대조
        /// </summary>
        /// <returns></returns>
        public static DataTable GetICM()
        {
            string SQL = $@"
            -- 교대조
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '90'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        public static string GetICMGubun()
        {
            string SQL = $@"
            -- 교대조
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME, c.REF_1, c.REF_2
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '90'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            //if (hour >= 6 && hour < 18)      // 06:00 ~ 17:59
            //    timeType = "039001";
            //else if (hour >= 18 && hour < 24) // 18:00 ~ 23:59
            //    timeType = "039002";
            //else                              // 00:00 ~ 05:59
            //    timeType = "039003";

            string GetShiftCode(DataTable sDt)
            {
                TimeSpan now = DateTime.Now.TimeOfDay;

                foreach (DataRow dr in sDt.Rows)
                {
                    if (dr["REF_1"] == DBNull.Value || dr["REF_2"] == DBNull.Value)
                        continue;

                    if (string.IsNullOrWhiteSpace(dr["REF_1"].ToString()) ||
                        string.IsNullOrWhiteSpace(dr["REF_2"].ToString()))
                        continue;

                    TimeSpan start = TimeSpan.Parse(dr["REF_1"].ToString());
                    TimeSpan end = TimeSpan.Parse(dr["REF_2"].ToString());

                    if (start <= end)
                    {
                        if (now >= start && now < end)
                            return dr["CODE"].ToString();
                    }
                    else
                    {
                        // 자정 넘어가는 구간
                        if (now >= start || now < end)
                            return dr["CODE"].ToString();
                    }
                }

                return "";
            }

            //string shiftCode = GetShiftCode(dt);

            return GetShiftCode(dt);
        }

        

        /// <summary>
        /// 작업구분
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPcStatus()
        {
            string SQL = $@"
            -- 작업구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '10'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 작업구분
        /// </summary>
        /// <returns></returns>
        public static string GetPcStatusCode(string sName = "")
        {
            string SQL = $@"
            -- 작업구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '10'
            AND COMM_DTNM = '{sName}'
            ";

            string returnResult = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"]?.ToString() : "";

            return returnResult;
        }

        /// <summary>
        /// 계량유형
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMI_USE()
        {
            string SQL = $@"
            -- 계량유형
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '01'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 계근방식
        /// </summary>
        /// <returns></returns>
        public static DataTable GetWeightType()
        {
            string SQL = $@"
            -- 계근방식
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '04'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 제품유형
        /// </summary>
        /// <returns></returns>
        public static DataTable GetResourceType()
        {
            string SQL = $@"
            -- 제품유형
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '02'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 가공형태
        /// </summary>
        /// <returns></returns>
        public static DataTable GetLabor()
        {
            string SQL = $@"
            -- 가공형태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '05'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 제품유형
        /// </summary>
        /// <returns></returns>
        public static string GetResourceTypeCode(string sName = "")
        {
            string SQL = $@"
            -- 제품유형
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '02'
            AND COMM_DTNM = '{sName}'
            ";

            string returnValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return returnValue;
        }

        /// <summary>
        /// 가공형태
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProductType()
        {
            string SQL = $@"
            -- 가공형태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '05'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 가공형태
        /// </summary>
        /// <returns></returns>
        public static string GetProductTypeCode(string sName)
        {
            string SQL = $@"
            -- 가공형태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '05'
                AND c.COMM_DTNM = '{sName}'
            ";

            string resultValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return resultValue;
        }

        /// <summary>
        /// 단위
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUnit()
        {
            string SQL = $@"
            -- 단위
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '03'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 가공형태
        /// </summary>
        /// <returns></returns>
        public static DataTable GetIngredType()
        {
            string SQL = $@"
            -- 가공형태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '10' AND c.COMM_CODE = '10'
            ORDER BY c.DISPLAY_SEQ
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 제품형태
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEtcResourceType()
        {
            string SQL = $@"
            -- 제품형태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '25'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 제품형태
        /// </summary>
        /// <returns></returns>
        public static string GetEtcResourceTypeCode(string sName)
        {
            string SQL = $@"
            -- 제품형태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '25'
                AND c.COMM_DTNM = '{sName}'
            ";

            string resultValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return resultValue;
        }

        /// <summary>
        /// 이동유형
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMovType()
        {
            string SQL = $@"
            -- 이동유형
            SELECT COMM_CODE AS CODE, COMM_NM AS NAME
            FROM COMM_CODE
            WHERE WK_DIVCODE = '03' AND COMM_CODE IN ('C1','C2','CZ')
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 품목전환
        /// </summary>
        /// <param name="sWK_DIVCODE"></param>
        /// <param name="sCOMM_CODE"></param>
        /// <returns></returns>
        public static DataTable GetProcType()
        {
            string SQL = $@"
            -- 품목전환
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE IN ('A40','A50')
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 실적기준정보
        /// </summary>
        /// <returns></returns>
        public static DataTable GetResultType()
        {
            string SQL = $@"
            -- 실적기준정보
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '10'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 차량 그룹
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable getCarGroupType()
        {
            string SQL = $@"
            -- 차량 그룹
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '50'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 차량입고타입
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCarInputType(string[] aName = null)
        {
            string inCondition = string.Join("','", aName ?? Array.Empty<string>());
            string whereName = inCondition != "" ? "@" : "";

            string SQL = $@"
            -- 차량입고타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '13'
            AND ('{whereName}' IS NULL OR c.COMM_DTNM IN ('{inCondition}'))
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 차량입고타입
        /// </summary>
        /// <returns></returns>
        public static string GetCarInputTypeCode(string aName)
        {
            string SQL = $@"
            -- 차량입고타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '13'
                AND c.COMM_DTNM = '{aName}'
            ";

            string resultValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return resultValue;
        }

        /// <summary>
        /// 기타 차량 종류
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEtcCarType(string[] aName = null)
        {
            string inCondition = string.Join("','", aName ?? Array.Empty<string>());
            string whereName = inCondition != "" ? "@" : "";

            string SQL = $@"
            -- 차량입고타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '14'
            AND ('{whereName}' IS NULL OR c.COMM_DTNM IN ('{inCondition}'))
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 실적유형
        /// </summary>
        /// <returns></returns>
        public static string GetRTTypeCode(string sName)
        {
            string SQL = $@"
            -- 차량입고타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '17'
                AND c.COMM_DTNM = '{sName}'
            ";

            string resultValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return resultValue;
        }

        /// <summary>
        /// 입차상태
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCarStatus()
        {
            string SQL = $@"
            -- 입차상태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '12'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }


        /// <summary>
        /// 입차상태
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOutStatus()
        {
            string SQL = $@"
            -- 입차상태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '18'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 벌크상차진행상태
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBulkStatus()
        {
            string SQL = $@"
            -- 입차상태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '61'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 작업구분
        /// </summary>
        /// <returns></returns>
        public static string GetBulkStatusCode(string sName = "")
        {
            string SQL = $@"
            -- 작업구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '61'
            AND COMM_DTNM = '{sName}'
            ";

            string returnResult = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"]?.ToString() : "";

            return returnResult;
        }

        /// <summary>
        /// 입차상태
        /// </summary>
        /// <returns></returns>
        public static string GetCarStatusCode(string sName)
        {
            string SQL = $@"
            -- 입차상태
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '12'
                AND c.COMM_DTNM = '{sName}'
            ";

            string resultValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return resultValue;
        }

        /// <summary>
        /// 차량그룹
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCarGroup()
        {
            string SQL = $@"
            -- 차량그룹
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '50'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 내외자구분
        /// </summary>
        /// <returns></returns>
        public static DataTable GetInOutType()
        {
            string SQL = $@"
            -- 내외자구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '41'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// QR 포멧 마스터
        /// </summary>
        /// <returns></returns>
        public static DataTable GetQRFomatMaster()
        {
            string SQL = $@"
            -- QR 포멧 마스터
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '99'
            ORDER BY TO_NUMBER(c.COMM_DTCODE) 
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// QR 포멧 상세
        /// </summary>
        /// <returns></returns>
        public static DataTable GetQRFomatDetail()
        {
            string SQL = $@"
            -- QR 포멧 상세
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '100'
            ORDER BY TO_NUMBER(c.COMM_DTCODE) 
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 오더분류
        /// </summary>
        /// <returns></returns>
        public static DataTable GetESART()
        {
            string SQL = $@"
            -- 오더분류
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '45'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 오더분류
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOrderType()
        {
            string SQL = $@"
            -- 오더분류
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '46'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 차량 톤급
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetVEHICLETON()
        {
            string SQL = $@"
            -- 차량 톤급
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '50'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 차량 그룹
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetVEHICLEGROUP()
        {
            string SQL = $@"
            -- 차량 그룹
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '51'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 메세지 타입
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMSGType()
        {
            string SQL = $@"
            -- 메세지 타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '42'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 판매오더구분
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSalesType()
        {
            string SQL = $@"
            -- 판매오더구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '40'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 납품유형
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetLFART()
        {
            string SQL = $@"
            -- 납품유형
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '30'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 납품처상세구분
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDeliveryType()
        {
            string SQL = $@"
            -- 납품처상세구분
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '05' AND c.COMM_CODE = '20'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 휴게시간
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRestTime()
        {
            string SQL = $@"
            -- 휴게시간
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '12'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 휴게시간
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPackType()
        {
            string SQL = $@"
            -- 휴게시간
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '03' AND c.COMM_CODE = '14'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// PLC IP
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPLCIP(string sProdGubun, string sCOMM_DTCODE = "")
        {
            string sCOMM_CODE = string.Empty;

            if (sProdGubun == "스케일")
                sCOMM_CODE = "01";
            else if (sProdGubun == "빈")
                sCOMM_CODE = "02";
            else if (sProdGubun == "작업지시")
                sCOMM_CODE = "03";
            else if (sProdGubun == "도징")
                sCOMM_CODE = "10";

            string SQL = $@"
            -- PLC IP
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM || '(' || c.REF_1 || ')' AS NAME, c.REF_1 AS PLCIP
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '90' AND c.COMM_CODE = '{sCOMM_CODE}'
                AND ('{sCOMM_DTCODE}' IS NULL OR c.COMM_DTCODE = '{sCOMM_DTCODE}')
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// PLC 위치 마스터
        /// </summary>
        /// <returns></returns>
        public static string GetPLCLocation(string sCOMM_DTCODE = "")
        {
            string SQL = $@"
            -- PLC 위치 마스터
            SELECT c.COMM_DTCODE AS CODE
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '90' AND c.COMM_CODE = '80'
            AND c.COMM_DTNM = '{sCOMM_DTCODE}'
            ";

            string resultValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return resultValue;
        }

        /// <summary>
        /// PLC Address Type
        /// </summary>
        /// <returns></returns>
        public static string GetPLCAddressType(string sCOMM_DTCODE = "")
        {
            string SQL = $@"
            -- PLC Address Type
            SELECT c.COMM_DTCODE AS CODE
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '90' AND c.COMM_CODE = '81'
            AND c.COMM_DTNM = '{sCOMM_DTCODE}'
            ";

            string resultValue = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0 ? Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0]["CODE"].ToString() : "";

            return resultValue;
        }

        /// <summary>
        /// PLC ADDRESS
        /// </summary>
        /// <returns></returns>
        public static void GetPLCAddress(string sPlantCode, string sProcessKey, string sLCode, int iLocation, string sAddressId, int sAddSeq, out string vPLCAddress, out int vPLCUnit, out int vPLCDataCount)
        {
            DataRow dr = null;
            vPLCAddress = "";
            vPLCUnit = 0;
            vPLCDataCount = 0;

            string sPlcId = sPlantCode.Merge(sProcessKey.Merge(sLCode.Replace(sProcessKey, "")));

            string SQL = $@"
            -- PLC ADDRESS
            SELECT a.ADDRESS, a.ADDRESS_UNIT, a.ADDRESS_COUNT
            FROM PLC_ADDRESS_MAP a
            WHERE a.PLC_ID =  '{sPlcId}'
                and a.ADDRESS_ID = '{sAddressId}'
                and a.ADDRESS_SEQ = {sAddSeq}
                AND a.PLC_LOCATION = {iLocation}
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dr = ds.Tables[0].Rows[0];

                if (dr != null)
                {
                    vPLCAddress = dr["ADDRESS"]?.ToString();
                    vPLCUnit = dr["ADDRESS_UNIT"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ADDRESS_UNIT"]);
                    vPLCDataCount = dr["ADDRESS_COUNT"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ADDRESS_COUNT"]);
                }
            }
            else
            {
                clsLog.logSave($"********************[{sAddressId}]Data 가 없습니다.********************", 0, "PLC");
                Console.WriteLine($"********************[{sAddressId}]Data 가 없습니다.********************");
            }
        }

        /// <summary>
        /// PLC ADDRESS
        /// </summary>
        /// <returns></returns>
        public static void GetPLCAddress(string sPlantCode, string sProcessKey, string sLCode)
        {
            DataRow dr = null;
            vPLCAddress = "";
            vPLCUnit = 0;
            vPLCDataCount = 0;

            string sPlcId = sPlantCode.Merge(sProcessKey.Merge(sLCode.Replace(sProcessKey, "")));

            string SQL = $@"
            -- PLC ADDRESS
            SELECT a.ADDRESS, a.ADDRESS_UNIT, a.ADDRESS_COUNT, a.ADDRESS_ID, a.ADDRESS_SEQ, a.PLC_LOCATION
            FROM PLC_ADDRESS_MAP a
            WHERE a.PLC_ID =  '{sPlcId}'
            ";

            if (Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows.Count; i++)
                {
                    dr = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[i];

                    clsPlcAddress.SetByAlias($"{dr["ADDRESS_ID"]}{dr["PLC_LOCATION"]}", $"{dr["ADDRESS"]}");
                }
            }
        }

        public static DataTable GetPLCInfo(string sPlantCode, string sProcessKey, string sUseProcessKey = "")
        {
            string SQL = $@"
            SELECT PLANT_CODE, PROCESS_KEY, USE_PROCESS_KEY, PLC_NO, PLC_TYPE,
                IP, N_NO, PORT, 
                T_OUT, CHANGED_BY, DATE_CHANGED
            FROM SAP_PROCESS_PLC
            WHERE PLANT_CODE = '{sPlantCode}'
                AND ('{sProcessKey}' IS NULL OR PROCESS_KEY = '{sProcessKey}')
                AND ('{sUseProcessKey}' IS NULL OR USE_PROCESS_KEY = '{sUseProcessKey}')
            ORDER BY TO_NUMBER(PLC_NO)
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// PLC 위치 정보 가져오기
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sProcess_Code"></param>
        /// <returns></returns>
        public static int GetPlcLocation(string sPlantCode, string sProcess_Code)
        {
            string SQL = $@"
            SELECT PLANT_CODE, PROCESS_KEY, USE_PROCESS_KEY, PLC_NO, PLC_TYPE, 
                IP, N_NO, PORT, T_OUT, CHANGED_BY, DATE_CHANGED
            FROM SAP_PROCESS_PLC
            WHERE PLANT_CODE = '{sPlantCode}' AND USE_PROCESS_KEY = '{sProcess_Code}'
            ORDER BY PLANT_CODE, PROCESS_KEY, TO_NUMBER(PLC_NO)
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(ds) > 0)
                return int.Parse(Dbconn.conn.getData(ds, "PLC_NO", 0));
            else
                return 2;
        }

        /// <summary>
        /// 스케일 배율
        /// </summary>
        /// <returns></returns>
        public static DataTable GetGetInScale()
        {
            string SQL = $@"
            -- 스케일 배율
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '90' AND c.COMM_CODE = '90'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// PLC 타입
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetPlcType()
        {
            string SQL = $@"
            -- PLC 타입
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '90' AND c.COMM_CODE = '91'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 바코드 포트네임
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetBarcodeCom()
        {
            string SQL = $@"
            -- 바코드 포트네임
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '91' AND c.COMM_CODE = '01'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 바코드 통신 속도
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetBaudRate()
        {
            string SQL = $@"
            -- 바코드 통신 속도
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '91' AND c.COMM_CODE = '02'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 바코드 스탑 비트
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetStopBit()
        {
            string SQL = $@"
            -- 바코드 스탑 비트
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '91' AND c.COMM_CODE = '03'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 바코드 데이터 비트
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetDataBit()
        {
            string SQL = $@"
            -- PLC 암호
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '91' AND c.COMM_CODE = '04'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// PLC 암호
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetPlcPw()
        {
            string SQL = $@"
            -- PLC 암호
            SELECT c.COMM_DTCODE AS CODE, c.COMM_DTNM AS NAME
            FROM COMM_DIV a
                INNER JOIN COMM_CODE b ON b.WK_DIVCODE = a.WK_DIVCODE
                INNER JOIN COMM_DTCODE c ON c.WK_DIVCODE = a.WK_DIVCODE AND c.COMM_CODE = b.COMM_CODE
            WHERE c.WK_DIVCODE = '90' AND c.COMM_CODE = '95'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }
        #endregion

        /// <summary>
        /// 배합비 버전
        /// </summary>
        /// <param name="sResource"></param>
        /// <returns></returns>
        public static DataTable getNote(string sPlantCode = "", string sResource = "", string sP_type = "1")
        {
            string SQL = $@"
            -- 배합비 버전
            SELECT DISTINCT a.NOTE AS CODE, a.NOTE AS NAME
            FROM SAP_IN_BOM_CONM a
                LEFT JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                LEFT JOIN SAP_IN_BOM_COND c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                                                    AND c.NOTE = a.NOTE AND c.P_TYPE = a.P_TYPE
            WHERE ('{sPlantCode}' IS NULL OR a.PLANT_CODE = '{sPlantCode}')
                AND ('{sResource}' IS NULL OR a.RESOURCE_NO = '{sResource}')
                AND a.USE_YN = 'Y' AND a.P_TYPE = '{sP_type}' AND NVL(a.STLST, '1') = '{sP_type}'
            GROUP BY a.PLANT_CODE, a.NOTE, a.RESOURCE_NO, b.DESCRIPTION
            ORDER BY CODE DESC
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 빈
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sProcessKey"></param>
        /// <param name="LCode"></param>
        /// <param name="sBin_Gubun"></param>
        /// <returns></returns>
        public static DataTable GetBin(string sPlantCode, string sProcessKey = "", string LCode = "", string sBin_Gubun = "", string sResourceNo = "", string sLocation = "")
        {
            string SQL = $@"
            -- 빈 리스트
            SELECT DISTINCT a.LOCATION AS CODE, a.RESOURCE_NO || ':' || b.DESCRIPTION AS NAME, a.RESOURCE_NO, b.DESCRIPTION,
             b.RESOURCE_TYPE_DESC AS RESOURCE_TYPE
            FROM BIN a 
                LEFT JOIN SAP_DI_PRODUCT b on a.RESOURCE_NO = b.RESOURCE_NO
            WHERE ('{sBin_Gubun}' IS NULL OR b.RESOURCE_TYPE IN ('{sBin_Gubun}'))
                AND a.PLANT_CODE = '{sPlantCode}'
                AND ('{sProcessKey}' IS NULL OR a.PROCESS_KEY = '{sProcessKey}')
                AND ('{LCode}' IS NULL OR a.L_CODE = '{LCode}')
                AND ('{sResourceNo}' IS NULL OR a.RESOURCE_NO = '{sResourceNo}')
                AND ('{sLocation}' IS NULL OR a.LOCATION = '{sLocation}')
            ORDER BY a.LOCATION
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 스케일
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sProcessKey"></param>
        /// <param name="sLCode"></param>
        /// <returns></returns>
        public static DataTable GetScale(string sPlantCode = "", string sProcessKey = "", string sLCode = "")
        {
            string SQL = $@"
            -- 스케일 리스트
            SELECT SCALE_CODE AS CODE, SCALE_CODE || ' : ' || SCALE_NAME AS NAME
            FROM SCALE
            WHERE ('{sPlantCode}' IS NULL OR PLANT_CODE = '{sPlantCode}')
                AND ('{sProcessKey}' IS NULL OR PROCESS_KEY = '{sProcessKey}')
                AND ('{sLCode}' IS NULL OR L_CODE = '{sLCode}')
            ORDER BY SCALE_CODE
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// Y, N 구분
        /// </summary>
        /// <returns></returns>
        public static DataTable GetYn(string[] Code = null, string[] Display = null)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
                    new DataColumn("CODE"),
                    new DataColumn("NAME"),
                });

            dt.Rows.Add(Code == null ? "Y" : Code[0], Display == null ? "사용" : Display[0]);
            dt.Rows.Add(Code == null ? "N" : Code[1], Display == null ? "미사용" : Display[1]);

            //DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }
        
        /// <summary>
        /// 사용자
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDO_INSA(string sPlantCode)
        {
            string SQL = $@"
            -- 사용자
            SELECT EMPLOYEE_NO AS CODE, NAME
            FROM DO_INSA
            WHERE PLANT_CODE = '{sPlantCode}'
            ORDER BY EMPLOYEE_NO
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 거래처
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCustomer(bool codeView = true)
        {
            string NameDisplay = string.Empty;

            // 품목코드 : 품목명
            if (codeView)
                NameDisplay = "PARTNER || ' : ' || NAME_ORG1";
            else
                NameDisplay = "NAME_ORG1";

            string SQL = $@"
            -- 거래선 리스트
            SELECT 
                PARTNER AS CODE
                , {NameDisplay} AS NAME
            FROM SAP_DI_CUSTOMER
            ORDER BY PARTNER
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 저장소
        /// </summary>
        /// <param name="sLocation"></param>
        /// <returns></returns>
        public static DataTable GetLocation(string sPlantCode, string sLocation = "")
        {
            string InValue = sLocation == "" ? "" : "@";

            string SQL = $@"
            -- 저장소 리스트
            SELECT
                LOCATION AS CODE, DESCRIPTION AS NAME
            FROM SAP_DI_LOCATION
            WHERE USE_FLAG = 'Y'
                AND PLANT_CODE = '{sPlantCode}'
                AND ('{InValue}' IS NULL OR LOCATION IN ('{sLocation}'))
            ORDER BY LOCATION
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 거래처와 저장소
        /// </summary>
        /// <returns></returns>
        public static DataTable GetLoCustomer()
        {
            string SQL = $@"
            -- 거래처와 저장소 리스트
            SELECT 
            PARTNER AS CODE, NAME_ORG1 AS NAME
            FROM SAP_DI_CUSTOMER a
            UNION ALL
            SELECT 
            LOCATION AS CODE, DESCRIPTION AS NAME
            FROM SAP_DI_LOCATION
            WHERE USE_FLAG = 'Y'
            ORDER BY PARTNER
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        public static DataTable GetCarMaster(string sVEHICLEGROUPCODE = "", bool bUseYn = false)
        {
            string sUseYn = string.Empty;

            if (bUseYn)
                sUseYn = "AND NVL(USE_YN, 'Y') != 'N'";

            string SQL = $@"
            -- 차량 리스트
            SELECT 
                VEHICLECODE AS CODE, VEHICLENO AS NAME, CARRIERNAME AS TYPE
            FROM TMS_INPUT_CARMASTER_CON
            WHERE ('{sVEHICLEGROUPCODE}' IS NULL OR VEHICLEGROUPCODE = '{sVEHICLEGROUPCODE}')
                {sUseYn}
            ORDER BY VEHICLECODE
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            dt.Rows.Add("9999", "톤백");

            return dt;
        }

        public static DataTable GetPlant(string reMark = "", bool gridGubun = false, string[] sPlantCode = null)
        {
            string where = string.Empty;

            if (sPlantCode != null)
            {
                var plantCode = string.Join(",", sPlantCode.Select(c => $"'{c}'"));
                where = $@"AND PLANT_CODE IN ({plantCode})
                ";
            }

            if (!gridGubun)
                where = where + @"AND USER_YN = 'Y'
                ";

            string SQL = $@"
            -- 플랜코드 
            SELECT 
                PLANT_CODE AS CODE, P_DESCRIPTION AS NAME
            FROM SAP_DI_PLANT
            WHERE ('{reMark}' IS NULL OR REMARK = '{reMark}')
                    {where}
            ORDER BY PLANT_CODE
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        public static DataTable GetPlantTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CODE", typeof(string));
            dt.Columns.Add("NAME", typeof(string));

            dt.Rows.Add("P101", "김제사료공장");
            dt.Rows.Add("P102", "정읍사료공장");
            dt.Rows.Add("PJ01", "중부일반공장(제일사료)");
            dt.Rows.Add("PJ02", "중부특수공장(제일사료)");
            dt.Rows.Add("PJ04", "남부일반공장(제일사료)");
            dt.Rows.Add("PJ05", "남부수산공장(제일사료)");
            dt.Rows.Add("P201", "상주사료공장");
            dt.Rows.Add("Z999", "한국제일");

            return dt;
        }

        public static string GetPlantCode(string sName)
        {
            string SQL = $@"
            -- 플랜코드 코드 찾기
            SELECT 
                PLANT_CODE AS CODE, P_DESCRIPTION AS NAME
            FROM SAP_DI_PLANT
            WHERE P_DESCRIPTION = '{sName}'
            ORDER BY PLANT_CODE
            ";

            string sReturn = Dbconn.conn.ExecutDataset(SQL).Tables[0].Rows[0][0].ToString();

            return sReturn;
        }

        /// <summary>
        /// 일반 콤보 공정 라인
        /// </summary>
        /// <param name="processKey"></param>
        /// <returns></returns>
        public static DataTable GetLine(string sPlantCode, string processKey = "")
        {
            string SQL = $@"
            -- 라인 리스트
            SELECT 
                L_CODE AS CODE, PROCESS_DESC AS NAME
            FROM SAP_PROCESS_LDIVISION
            WHERE PLANT_CODE = '{sPlantCode}'
                AND ('{processKey}' IS NULL OR PROCESS_KEY = '{processKey}')
            ORDER BY L_CODE
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 그리드 공정 라인
        /// </summary>
        /// <param name="processKey"></param>
        /// <returns></returns>
        public static DataTable GetGridLine(string sPlantCode, string processKey = "")
        {
            string SQL = $@"
            -- 라인 리스트
            SELECT 
                L_CODE AS CODE, PROCESS_DESC AS NAME
            FROM SAP_PROCESS_LDIVISION
            WHERE PLANT_CODE = '{sPlantCode}' 
                AND ('{processKey}' IS NULL OR PROCESS_KEY = '{processKey}')
            ORDER BY L_CODE
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        public static DataTable GetProcessLine(string plantCode, string processName, string lineName)
        {
            string SQL = $@"
            -- 라인 코드 찾기
            SELECT a.PLANT_CODE, a.PROCESS_KEY, b.L_CODE, a.PROCESS_TYPE
            FROM SAP_PROCESS_DIVISION a
                JOIN SAP_PROCESS_LDIVISION b ON b.PLANT_CODE = a.PLANT_CODE AND b.PROCESS_KEY = a.PROCESS_KEY
            WHERE a.PLANT_CODE = '{plantCode}' AND TRIM(a.PROCESS_DESC) = '{processName}' AND TRIM(b.PROCESS_DESC) = '{lineName}'
                AND ROWNUM = 1     
            ORDER BY a.PLANT_CODE
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 주문번호 리스트
        /// </summary>
        /// <returns></returns>
        public static DataTable GetORDERNO()
        {
            string SQL = $@"
            -- 주문번호 리스트
            SELECT DISTINCT ORDERNO AS CODE, b.DESCRIPTION AS NAME
            FROM TMS_INPUT_PLOADD_CON a
                LEFT JOIN SAP_DI_PRODUCT b ON b.RESOURCE_NO = a.ITEMCODE
            ORDER BY ORDERNO
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 배차번호 리스트
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetDISPATCHNO()
        {
            string SQL = $@"
            -- 배차번호 리스트
            SELECT DISTINCT DISPATCHNO AS CODE, VEHICLENO AS NAME FROM TMS_INPUT_PLOADM_CON
            ORDER BY DISPATCHNO
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 구매 발주번호
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetEBELN()
        {
            string SQL = $@"
            -- 배차번호 리스트
            SELECT DISTINCT EBELN AS CODE, NAME2 AS NAME, LIFNR AS TYPE FROM SAP_INPUT_POORDERM_CON
            ORDER BY EBELN
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 구매 발주항번
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DataTable GetEBELP()
        {
            string SQL = $@"
            -- 배차번호 리스트
            SELECT DISTINCT EBELP AS CODE, MATNR AS NAME, TXZ01 AS TYPE FROM SAP_INPUT_POORDERD_CON
            ORDER BY EBELP
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        public static DataTable getPallet()
        {
            string SQL = $@"
            -- 파렛트 리스트
            SELECT PTMCD AS CODE, PTMCDNM AS NAME, WEIGHT AS TYPE FROM WAP_PA_MASTER
            ORDER BY PTMCD
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="gridView"></param>
        /// <returns></returns>
        public static string ValdationCheck(string[] sValid, DataRow dr, GridView gridView, int row = 0)
        {
            string sCheck = string.Empty;

            gridView.CloseEditor();
            gridView.UpdateCurrentRow();

            foreach (DataColumn col in dr.Table.Columns)
            {
                string columnName = col.ColumnName;

                if (sValid.Contains(columnName))
                {
                    object value = dr[columnName];

                    if (value == DBNull.Value ||
                        string.IsNullOrWhiteSpace(Convert.ToString(value)))
                    {
                        sCheck = columnName;

                        string caption = gridView.Columns.ColumnByFieldName(columnName)?.Caption ?? columnName;

                        ShowMessageBox.XtraShowWarning($"{caption} 항목은 필수 입력 항목 입니다.");

                        break;
                    }
                }
            }

            return sCheck;
        }

        /// <summary>
        /// 가루 외 원료인지 체크
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sResourceNo"></param>
        /// <returns></returns>
        public static bool GetAutoPellet(string sPlantCode, string sProcesskey, string sResourceNo, out string processKey)
        {
            if (!new string[] { "PJ01", "PJ04" }.Contains(sPlantCode))
            {
                processKey = "";
                return false;
            }

            string SQL = $@"
            SELECT PLANT_CODE, RESOURCE_NO
                , VORNR, ARBPL, LTXA1, I_TIME
            FROM SAP_DI_ROUTING
            WHERE PLANT_CODE = '{sPlantCode}'
                AND RESOURCE_NO = '{sResourceNo}'
                AND VORNR != '10'
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];
            DataRow dr = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];

                processKey = dr["ARBPL"]?.ToString();

                return true;
            }

            processKey = "";

            return false;
        }

        public static string IncreaseLastNumber(string source, int digits = 2)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            string[] parts = source.Split('-');
            if (parts.Length == 0)
                return source;

            string last = parts[parts.Length - 1];
            int num;

            if (!int.TryParse(last, out num))
                return source;

            num += 1;

            parts[parts.Length - 1] = num.ToString().PadLeft(digits, '0');
            return string.Join("-", parts);
        }

        public static string GetMiddleDigits(string input)
        {
            if (new string[] { "131", "132" }.Contains(input))
                return "6";

            if (new string[] { "601", "607" }.Contains(input))
                return "1";

            if (new string[] { "602", "603" }.Contains(input))
                return "2";

            if (new string[] { "716", "717" }.Contains(input))
                return "3";

            if (new string[] { "605", "606" }.Contains(input))
                return "4";

            if (new string[] { "611", "612" }.Contains(input))
                return "5";

            if (new string[] { "613", "614" }.Contains(input))
                return "6";

            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = input.Trim();

            // 숫자이면 true 아니면 false
            if (!int.TryParse(input, out _)) return string.Empty;

            if (input.Length == 3)
            {
                // 3자리: 가운데 1자리만
                return input.Substring(1, 1);
            }
            else if (input.Length == 4)
            {
                // 4자리: 가운데 2자리만
                return input.Substring(1, 2);
            }

            return string.Empty; // 3자리, 4자리가 아닐 경우
        }

        public static string GetPelletLCode(string sPlantCode, string sProcessKey, string sLocation)
        {
            string pLCode = string.Empty;

            string SQL = $@"
            SELECT L_CODE
            FROM PELLET_LOCATION
            WHERE PLANT_CODE = '{sPlantCode}'
                AND PROCESS_KEY = '{sProcessKey}'
                AND (LOCATION IS NULL OR LOCATION = '{sLocation}')
            ";

            DataTable dt = Dbconn.conn.ExecutDataset(SQL).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                pLCode = dt.Rows[0]["L_CODE"]?.ToString();
            }
            else
                pLCode = "PLP0101";

            return pLCode;
        }

        /// <summary>
        /// 우선순위 체크 빈 내보내기
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sProcess_key"></param>
        /// <param name="lCode"></param>
        /// <param name="sResourceNo"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static void getSelectBin(string sPlantCode, string sProcess_key, string lCode, string sResourceNo, out string location, out string scaleCode)
        {
            location = string.Empty;
            scaleCode = string.Empty;

            string SQL = SQL = $@"
            SELECT bin.LOCATION, bin.SCALE_CODE
            FROM Bin bin
            WHERE bin.PLANT_CODE = '{sPlantCode}'
                AND bin.PROCESS_KEY = '{sProcess_key}'
                AND bin.L_CODE = '{lCode}'
                AND bin.RESOURCE_NO = '{sResourceNo}'
            ";

            using (DataSet noteDs = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(noteDs) > 0)
                {
                    location = Dbconn.conn.getData(noteDs, "LOCATION", 0);
                    scaleCode = Dbconn.conn.getData(noteDs, "SCALE_CODE", 0);
                }
                else if (Dbconn.conn.getRowCnt(noteDs) == 0)
                {
                    location = "레시피가 없습니다";
                }
            }
        }

        /// <summary>
        /// 제품의 마지막 배합비 가져오기
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="pCode"></param>
        /// <param name="chk"></param>
        /// <param name="per"></param>
        /// <returns></returns>
        public static string getLastVersion(string sPlantCode, string pCode, out bool chk, out decimal per)
        {
            string note = string.Empty;
            string SQL = $"SELECT NOTE FROM SAP_IN_BOM_CONM WHERE P_TYPE = '2' AND STLST = '2' AND PLANT_CODE = '{sPlantCode}' AND RESOURCE_NO = '{pCode}' ORDER BY NOTE DESC";
            using (DataSet noteDs = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(noteDs) > 0)
                {
                    note = Dbconn.conn.getData(noteDs, "NOTE", 0);

                }
            }

            SQL = $@"
            SELECT 
                NVL(MAX(a.NOTE), 'NOT') AS NOTE,
                TO_NUMBER(ROUND(SUM(b.MENGE), 3)) AS PER
            FROM SAP_IN_BOM_CONM   a
                INNER JOIN SAP_IN_BOM_COND b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                                    AND b.NOTE = a.NOTE AND b.P_TYPE = a.P_TYPE
            WHERE a.PLANT_CODE = '{sPlantCode}' AND a.P_TYPE = '2' AND a.STLST = '2' AND a.RESOURCE_NO = '{pCode}' AND a.NOTE = '{note}'
            GROUP BY a.NOTE
            ORDER BY a.NOTE DESC
            ";

            per = 0;
            chk = false;

            using (DataSet noteDs = Dbconn.conn.ExecutDataset(SQL))
            {
                if (Dbconn.conn.getRowCnt(noteDs) > 0)
                {
                    decimal note_per = Convert.ToDecimal(Dbconn.conn.getData(noteDs, "PER", 0));

                    if (note_per < 100)
                    {
                        chk = true;
                        note = Dbconn.conn.getData(noteDs, "NOTE", 0);
                    }
                    else
                    {
                        chk = true;
                        note = Dbconn.conn.getData(noteDs, "NOTE", 0);
                    }
                    per = note_per;
                }
                else if (Dbconn.conn.getRowCnt(noteDs) == 0)
                {
                    chk = false;
                    note = "레시피가 없습니다";
                }
            }

            return note;

        }

        public static string SetTransFlag(int TransFlag = 1, object oldCode = null)
        {
            string returnValue = string.Empty;

            if (TransFlag == 1)
            {
                switch (oldCode)
                {
                    case "Y":
                        returnValue = "M";

                        break;
                    case "U":
                        returnValue = "M";

                        break;
                    case "D":
                        returnValue = "X";

                        break;
                    case "F":
                        returnValue = "N";

                        break;
                    default:
                        if (oldCode == null || oldCode == DBNull.Value)
                            returnValue = "N";
                        else
                            returnValue = oldCode.ToString();

                        break;
                }
            }
            else if (TransFlag == 2)
            {
                switch (oldCode)
                {
                    case "N":
                        returnValue = "F";

                        break;
                    case "M":
                        returnValue = "U";

                        break;
                    case "X":
                        returnValue = "D";

                        break;
                    case "G":
                        returnValue = "F";

                        break;
                    case "L":
                        returnValue = "U";

                        break;
                    case "R":
                        returnValue = "D";

                        break;
                    default:
                        if (oldCode == null || oldCode == DBNull.Value)
                            returnValue = "N";
                        else
                            returnValue = oldCode.ToString();

                        break;
                }
            }

            return returnValue;
        }
    }
}
