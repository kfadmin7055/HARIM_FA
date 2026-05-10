using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using System;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using Core.Class;

namespace HARIM_FA_DOSING
{
    public class clsPrintExcel
    {
        public static double IntRound(double Value, int Digit)
        {
            double Temp = Math.Pow(10.0, Digit);
            return Math.Round(Value * Temp) / Temp;
        }

        public static double IntCeiling(double Value, int Digit)
        {
            double Temp = Math.Pow(10.0, Digit);
            return Math.Ceiling(Value * Temp) / Temp;
        }

        public static void PrintChulgoAllSheet(string is_no)
        {
            try
            {
                string factory = clsCommon.PlantCode;
                string carInType = clsCarUtil.returnCarType(is_no);
                string carGubun = clsCarUtil.returnCarGubun2(is_no);
                string carOrderType = clsCarUtil.returnOrderTypeCode(is_no);

                string SQL = string.Empty;

                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\form_m.xlsx", DocumentFormat.Xlsx);


                //계량번호
                printSheet.Document.Worksheets[0].Cells["O4"].Value = is_no;

                SQL = "SELECT CARM.VEHICLENO, CARM.DRIVERNAME, CARM.DRIVERMOBILE, CARM.CARRIERNAME " +
                        "FROM WAP_DECAR WD  " +
                        "LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CARM ON WD.INCAR_NO = CARM.VEHICLECODE  " +
                        "WHERE CARM.VEHICLECODE IS NOT NULL  " +
                        $"AND WD.IS_NO = '{is_no}'  ";

                DataSet carDs = Dbconn.conn.ExecutDataset(SQL);
                if (Dbconn.conn.getRowCnt(carDs) > 0)
                {
                    printSheet.Document.Worksheets[0].Cells["E4"].Value = Dbconn.conn.getData(carDs, "VEHICLENO", 0); //차량번호
                    printSheet.Document.Worksheets[0].Cells["E7"].Value = Dbconn.conn.getData(carDs, "CARRIERNAME", 0); //배송거래처
                    printSheet.Document.Worksheets[0].Cells["E9"].Value = Dbconn.conn.getData(carDs, "DRIVERMOBILE", 0) + " (" + Dbconn.conn.getData(carDs, "DRIVERNAME", 0) + ")"; //기사연락처
                }

                carDs.Dispose();

                SQL = "SELECT INCAR_NO, IN_WEIGHT, " +
                  "IN_WEIGHT, " +
                  "TO_CHAR(INCAR_DATE, 'YYYY-MM-DD HH24:MI') AS INCAR_DATE, " +
                  "OUT_WEIGHT, " +
                  "TO_CHAR(OUTCAR_DATE, 'YYYY-MM-DD HH24:MI') AS OUTCAR_DATE, " +
                  "ABS(IN_WEIGHT - OUT_WEIGHT) AS REAL_WEIGHT, ETC_DETAIL  " +
                  $"FROM WAP_DECAR WHERE PC_STATUS = '20' AND IS_NO = '{is_no}'  ";

                DataSet inDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(inDs) == 0)
                {
                    return;
                }

                string sInWeight = Dbconn.conn.getData(inDs, "IN_WEIGHT", 0);
                string sInWeightTime = Dbconn.conn.getData(inDs, "INCAR_DATE", 0);
                string sOutWeight = Dbconn.conn.getData(inDs, "OUT_WEIGHT", 0);
                string sOutWeightTime = Dbconn.conn.getData(inDs, "OUTCAR_DATE", 0);
                string sWeigth = Dbconn.conn.getData(inDs, "REAL_WEIGHT", 0);
                inDs.Dispose();

                printSheet.Document.Worksheets[0].Cells["E5"].Value = sInWeightTime;
                printSheet.Document.Worksheets[0].Cells["O5"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                printSheet.Document.Worksheets[0].Cells["E6"].Value = sOutWeightTime;
                printSheet.Document.Worksheets[0].Cells["O6"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));

                //피무계
                int intPweight = 0;
                SQL = $"SELECT NVL(SUM(WEIGHT * PD_QTY),0) AS P_SUM FROM WAP_OUT_ADD WHERE IS_NO = '{is_no}' ";
                DataSet pWeightDs = Dbconn.conn.ExecutDataset(SQL);
                string sPweight = Dbconn.conn.getData(pWeightDs, "P_SUM", 0);

                if (clsUtil.isNumber(sPweight))
                {
                    if (Convert.ToDecimal(sPweight) > 0)
                    {
                        if (factory == "PJ01" || factory == "PJ04") //대전,함안제일사료 피무계 올림처림
                        {
                            sPweight = Convert.ToString(IntCeiling(Convert.ToDouble(sPweight), -1));
                        }
                        else
                        {
                            sPweight = Convert.ToString(IntRound(Convert.ToDouble(sPweight), -1));
                        }

                        intPweight = Convert.ToInt32(sPweight);
                        //피중량
                        printSheet.Document.Worksheets[0].Cells["O7"].Value = String.Format("{0:#,###}", Convert.ToInt32(sPweight));
                    }
                }

                //실중량
                if (clsUtil.isNumber(sWeigth))
                {
                    printSheet.Document.Worksheets[0].Cells["O8"].Value = String.Format("{0:#,###}", Convert.ToDecimal(sWeigth) - intPweight);
                }


                if (carInType == "002" || carInType == "003")
                {
                    SQL
                    = "SELECT AD.PLANTCODE, ADM.DELIVERYDATE, AD.FROMLOCATIONCODE , AD.TOLOCATIONCODE, ADM.VEHICLECODE, CAR.VEHICLENO,SAP_CUS.NAME_ORG1, ADM.DISPATCHNO, AD.ORDERNO, AD.ORDERLINENO, ADM.DRIVERNAME, ADM.DRIVERMOBILE,  " +
                        "AD.ITEMCODE,SAP_PRO.DESCRIPTION, AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY, (SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE, SAP_PRO.UOM, NVL(TMS_OUT_RESULT.PD_YN,'N') AS PD_YN,  " +
                        "NVL(DECODE(SAP_PRO.UOM, 'EA' , TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT) , 0) AS WEIGHT2,  " +
                        "NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT,( AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) ) / (SAP_MA.UMREN / SAP_MA.UMREZ)  AS PACK_CNT , " +
                        "DECODE((SAP_MA.UMREN / SAP_MA.UMREZ), 1, '타이콘','지대') AS PACK_TYPE,  TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT  " +
                        "FROM TMS_INPUT_PLOADM_CON ADM  " +
                        "JOIN TMS_INPUT_PLOADD_CON AD ON ADM.DISPATCHNO = AD.DISPATCHNO  " +
                        "LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CAR ON ADM.VEHICLECODE = CAR.VEHICLECODE  " +
                        "LEFT OUTER JOIN (SELECT RESOURCE_NO, DESCRIPTION, UOM FROM SAP_DI_PRODUCT GROUP BY RESOURCE_NO, DESCRIPTION, UOM) SAP_PRO  " +
                        "ON AD.ITEMCODE = SAP_PRO.RESOURCE_NO  " +
                        "LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS ON AD.TOLOCATIONCODE = SAP_CUS.PARTNER  " +
                        "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO  " +
                        "AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO   " +
                        "LEFT OUTER JOIN SAP_MARM SAP_MA ON AD.ITEMCODE = SAP_MA.MATNR AND SAP_MA.MEINH = 'KG'  " +
                        $"WHERE TMS_OUT_RESULT.IS_NO = '{is_no}'  " +
                        "ORDER BY ADM.DELIVERYDATE DESC,  ADM.DISPATCHNO, AD.ORDERNO, AD.ORDERLINENO  ";
                }
                else if (carInType == "004" || carInType == "005")
                {
                    SQL
                    = "SELECT SAP_SM.TRAID AS VEHICLENO, SAP_SM.WERKS_GR AS PLANTCODE, SAP_PLANT.P_DESCRIPTION AS NAME_ORG1, SAP_SM.TRANS_DATE AS DELIVERYDATE,  SAP_SD.TKNUM AS DISPATCHNO,  " +
                        "SAP_SD.VBELN AS ORDERNO, SAP_SD.POSNR AS ORDERLINENO, SAP_SD.MATNR AS ITEMCODE,SAP_PRO.DESCRIPTION, SAP_SD.LFIMG  * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY,   " +
                        "(SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE, SAP_PRO.UOM,  " +
                        "NVL(TMS_OUT_RESULT.PD_YN,'N') AS PD_YN, NVL( DECODE(SAP_PRO.UOM, 'EA' , TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT), 0) AS WEIGHT,  " +
                        "NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT, SAP_SD.LFIMG as PACK_CNT,   " +
                        "DECODE((SAP_MA.UMREN / SAP_MA.UMREZ), 1, '타이콘','지대') AS PACK_TYPE,  TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT " +
                        "FROM SAP_INPUT_PROTRANSM SAP_SM JOIN SAP_INPUT_PROTRANSD SAP_SD  " +
                        "ON SAP_SM.TKNUM = SAP_SD.TKNUM   " +
                        "LEFT OUTER JOIN SAP_DI_PRODUCT SAP_PRO ON SAP_SD.MATNR = SAP_PRO.RESOURCE_NO  AND SAP_SM.WERKS_GR = SAP_PRO.PLANT_CODE   " +
                        "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON SAP_SM.TKNUM = TMS_OUT_RESULT.DISPATCHNO   " +
                        "AND SAP_SD.VBELN = TMS_OUT_RESULT.ORDERNO AND SAP_SD.POSNR = TMS_OUT_RESULT.ORDERLINENO   " +
                        "LEFT OUTER JOIN SAP_MARM SAP_MA ON SAP_SD.MATNR = SAP_MA.MATNR AND SAP_MA.MEINH = 'KG'    " +
                        "LEFT OUTER JOIN SAP_DI_PLANT SAP_PLANT ON SAP_SM.WERKS_GR = SAP_PLANT.PLANT_CODE  " +
                        $"WHERE  TMS_OUT_RESULT.IS_NO = '{is_no}'  " +
                        "ORDER BY  SAP_SD.TKNUM, SAP_SD.VBELN,SAP_SD.POSNR   ";
                }

                DataSet ListDs = Dbconn.conn.ExecutDataset(SQL);

                int excelListPos = 11;

                for (int r = 0; r < Dbconn.conn.getRowCnt(ListDs); r++)
                {
                    string list_DESCRIPTION = Dbconn.conn.getData(ListDs, "DESCRIPTION", r);
                    string list_STD_LOT_SIZE = Dbconn.conn.getData(ListDs, "STD_LOT_SIZE", r);
                    string list_PACK_TYPE = Dbconn.conn.getData(ListDs, "PACK_TYPE", r);
                    string list_QTY = Dbconn.conn.getData(ListDs, "QTY", r);
                    string list_WEIGHT = Dbconn.conn.getData(ListDs, "WEIGHT", r);
                    string list_UOM = Dbconn.conn.getData(ListDs, "UOM", r);

                    printSheet.Document.Worksheets[0].Cells["E" + excelListPos.ToString()].Value = list_DESCRIPTION;
                    printSheet.Document.Worksheets[0].Cells["J" + excelListPos.ToString()].Value = list_STD_LOT_SIZE + " x " + list_QTY + " " + list_UOM;
                    printSheet.Document.Worksheets[0].Cells["O" + excelListPos.ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(list_WEIGHT)) + " KG";

                    excelListPos = excelListPos + 1;
                }

                ListDs.Dispose();

                //파레트정보
                SQL = "SELECT PM.PTMCDNM, WOA.WEIGHT * WOA.PD_QTY AS SUM_QTY, WOA.PD_QTY   " +
                         "FROM WAP_PA_MASTER PM JOIN WAP_OUT_ADD WOA ON PM.PTMCD = WOA.PTMCD " +
                         $"WHERE WOA.IS_NO = '{is_no}'   ";

                DataSet custPaInputDs = Dbconn.conn.ExecutDataset(SQL);
                int paInputCnt = Dbconn.conn.getRowCnt(custPaInputDs);

                for (int p = 0; p < paInputCnt; p++)
                {
                    if (paInputCnt > 0)
                    {
                        if (p == 0)
                        {
                            printSheet.Document.Worksheets[0].Cells["C35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["C36"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                        }

                        if (p == 1)
                        {
                            printSheet.Document.Worksheets[0].Cells["D35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["D36"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                        }

                        if (p == 2)
                        {
                            printSheet.Document.Worksheets[0].Cells["G35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["G36"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                        }

                        if (p == 3)
                        {
                            printSheet.Document.Worksheets[0].Cells["I35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["I36"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                        }

                        if (p == 4)
                        {
                            printSheet.Document.Worksheets[0].Cells["M35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["M36"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                        }
                    }
                } //for 파레트

                printSheet.Document.CalculateFull();
                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;

                Margins pageMargins = printSheet.Document.Worksheets[0].ActiveView.Margins;
                pageMargins.Left = 0F;
                pageMargins.Top = 0.0F;
                pageMargins.Right = 0;
                pageMargins.Bottom = 0;
                //pageMargins.Header = 1;
                //pageMargins.Footer = 0.4F;

                printSheet.Options.Print.ShowMarginsWarning = false;
                //printSheet.Print();
                printSheet.ShowPrintPreview();

            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Source);
            }
        }

        //출고인수증 발행
        public static void PrintChulgoSheet(string is_no)
        {
            try
            {
                string factory = clsCommon.PlantCode;

                string SQL = string.Empty;

                string carInType = clsCarUtil.returnCarType(is_no);
                string carGubun = clsCarUtil.returnCarGubun2(is_no);
                string carOrderType = clsCarUtil.returnOrderTypeCode(is_no);

                string sOutDate = string.Empty;

                string sInSaupja = string.Empty;
                string sInHouse = string.Empty;

                string sOutSaupja = string.Empty;
                string sOutHouse = string.Empty;

                SQL = $"SELECT P_DESCRIPTION, REMARK FROM SAP_DI_PLANT WHERE PLANT_CODE = '{factory}' ";
                DataSet saupDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(saupDs) > 0)
                {
                    sInSaupja = Dbconn.conn.getData(saupDs, "REMARK", 0);
                    sInHouse = Dbconn.conn.getData(saupDs, "P_DESCRIPTION", 0);
                }

                SQL = $"SELECT TO_CHAR(OUTCAR_DATE, 'YYYY-MM-DD') AS OUT_DATE FROM WAP_DECAR WHERE IS_NO = '{is_no}' ";
                DataSet outDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(saupDs) > 0)
                {
                    sOutDate = Dbconn.conn.getData(outDs, "OUT_DATE", 0);
                }

                if (carInType == "002" || carInType == "003")
                {
                    SQL = "SELECT AD.TOLOCATIONCODE " +
                        "FROM TMS_INPUT_PLOADD_CON AD " +
                        "JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO  " +
                        "AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO    " +
                        $"WHERE TMS_OUT_RESULT.IS_NO = '{is_no}' " +
                        "GROUP BY AD.TOLOCATIONCODE ";
                }
                else if (carInType == "004" || carInType == "005")
                {
                    SQL = "SELECT SAP_SM.WERKS_GR AS TOLOCATIONCODE, SAP_SM.WERKS_GI AS FROMLOCATION " +
                            "FROM SAP_INPUT_PROTRANSM SAP_SM JOIN SAP_INPUT_PROTRANSD SAP_SD  " +
                            "ON SAP_SM.TKNUM = SAP_SD.TKNUM   " +
                            "LEFT OUTER JOIN SAP_DI_PRODUCT SAP_PRO ON SAP_SD.MATNR = SAP_PRO.RESOURCE_NO  AND SAP_SM.WERKS_GR = SAP_PRO.PLANT_CODE   " +
                            "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON SAP_SM.TKNUM = TMS_OUT_RESULT.DISPATCHNO   " +
                            "AND SAP_SD.VBELN = TMS_OUT_RESULT.ORDERNO AND SAP_SD.POSNR = TMS_OUT_RESULT.ORDERLINENO   " +
                            "LEFT OUTER JOIN SAP_MARM SAP_MA ON SAP_SD.MATNR = SAP_MA.MATNR AND SAP_MA.MEINH = 'KG'    " +
                            "LEFT OUTER JOIN SAP_DI_PLANT SAP_PLANT ON SAP_SM.WERKS_GR = SAP_PLANT.PLANT_CODE  " +
                            $"WHERE TMS_OUT_RESULT.IS_NO = '{is_no}' " +
                            "GROUP BY SAP_SM.WERKS_GR, SAP_SM.WERKS_GI ";
                }

                DataSet cusListDs = Dbconn.conn.ExecutDataset(SQL);

                for (int i = 0; i < Dbconn.conn.getRowCnt(cusListDs); i++)
                {
                    int excelListPos = 14;

                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                    printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\igo.xls", DocumentFormat.Xls);

                    string sCustCd = Dbconn.conn.getData(cusListDs, "TOLOCATIONCODE", i);


                    if (carOrderType == "NL" || carOrderType == "NLCC" || carOrderType.Contains("ZLO"))
                    {
                        printSheet.Document.Worksheets[0].Cells["K2"].Value = "이 고 명 세 서";
                    }
                    else
                    {
                        printSheet.Document.Worksheets[0].Cells["K2"].Value = "출 고 명 세 서";
                    }

                    //출고일,발행번호
                    printSheet.Document.Worksheets[0].Cells["F3"].Value = sOutDate;
                    printSheet.Document.Worksheets[0].Cells["F4"].Value = is_no;

                    /*
                        dt.Rows.Add("P101", "김제하림");
                        dt.Rows.Add("P102", "정읍하림");
                        dt.Rows.Add("P201", "상주올품");
                        dt.Rows.Add("PJ01", "대전제일사료");
                        dt.Rows.Add("PJ04", "함안제일사료");
                    */

                    string sNum = string.Empty;
                    string sName = string.Empty;
                    string sSangho = string.Empty;
                    string sAddress = string.Empty;

                    if (factory == "P101")
                    {
                        sNum = "403-85-19030";
                        sName = "정호석,김흥국";
                        sSangho = "(주)하림 김제사료공장";
                        sAddress = "전라북도 김제시 만경읍 만경공단1길 25";
                    }
                    else if (factory == "P102")
                    {
                        sNum = "404-85-07274";
                        sName = "정호석,김홍국";
                        sSangho = "(주)하림 정읍사료공장";
                        sAddress = "전라북도 정읍시 신태인읍 신태인공단길 17-28";
                    }
                    else if (factory == "P201")
                    {
                        sNum = "479-85-01158";
                        sName = "강기철";
                        sSangho = "(주)올품 상주사료공장";
                        sAddress = "경상북도 상주시 화서면 영남제일로 4371-25";
                    }
                    else if (factory == "PJ01")
                    {
                        sNum = "305-86-08366";
                        sName = "권천년 ";
                        sSangho = "(주)제일사료 대전공장";
                        sAddress = "대전광역시 대덕구 대전로1331번길 240";
                    }
                    else if (factory == "PJ04")
                    {
                        sNum = "305-86-08366";
                        sName = "권천년";
                        sSangho = "(주)제일사료 함안공장";
                        sAddress = "경상남도 함안군 법수면 윤외공단길 83";
                    }

                    //등록번호
                    printSheet.Document.Worksheets[0].Cells["H8"].Value = sNum;
                    //성명
                    printSheet.Document.Worksheets[0].Cells["S8"].Value = sName;
                    //상호명
                    printSheet.Document.Worksheets[0].Cells["H9"].Value = sSangho;
                    //주소
                    printSheet.Document.Worksheets[0].Cells["H10"].Value = sAddress;


                    //출고
                    printSheet.Document.Worksheets[0].Cells["AC8"].Value = sInSaupja;
                    printSheet.Document.Worksheets[0].Cells["AC9"].Value = sInHouse;

                    string inHouse = string.Empty;
                    //입고
                    SQL = $"SELECT NAME_ORG1 FROM SAP_DI_CUSTOMER WHERE PARTNER = '{sCustCd}'  ";
                    DataSet inCusDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(inCusDs) > 0)
                    {
                        inHouse = Dbconn.conn.getData(inCusDs, "NAME_ORG1", 0);
                        printSheet.Document.Worksheets[0].Cells["AC10"].Value = inHouse;
                    }
                    inCusDs.Dispose();

                    if (carInType == "004" || carInType == "005")
                    {
                        //sCustCd  //보내는공장
                        string sToCd = Dbconn.conn.getData(cusListDs, "FROMLOCATION", i);  //받는공장

                        //출고
                        printSheet.Document.Worksheets[0].Cells["AC8"].Value = string.Empty;
                        printSheet.Document.Worksheets[0].Cells["AC9"].Value = string.Empty;

                        //입고
                        printSheet.Document.Worksheets[0].Cells["AC10"].Value = sInSaupja;
                        printSheet.Document.Worksheets[0].Cells["AC11"].Value = sInHouse;

                        if (factory == "P101")
                        {
                            printSheet.Document.Worksheets[0].Cells["AC10"].Value = "(주)하림";
                            printSheet.Document.Worksheets[0].Cells["AC11"].Value = "김제사료공장";
                        }
                        else if (factory == "P102")
                        {
                            printSheet.Document.Worksheets[0].Cells["AC10"].Value = "(주)하림";
                            printSheet.Document.Worksheets[0].Cells["AC11"].Value = "정읍사료공장";
                        }
                        else if (factory == "P201")
                        {
                            printSheet.Document.Worksheets[0].Cells["AC10"].Value = "(주)올품";
                            printSheet.Document.Worksheets[0].Cells["AC11"].Value = "상주사료공장";
                        }
                        else if (factory == "PJ01")
                        {
                            printSheet.Document.Worksheets[0].Cells["AC10"].Value = "(주)제일사료";
                            printSheet.Document.Worksheets[0].Cells["AC11"].Value = "대전공장";
                        }
                        else if (factory == "PJ04")
                        {
                            printSheet.Document.Worksheets[0].Cells["AC10"].Value = "(주)제일사료";
                            printSheet.Document.Worksheets[0].Cells["AC11"].Value = "함안공장";
                        }
                    }


                    if (carInType == "002" || carInType == "003")
                    {

                        SQL
                        = "SELECT AD.PLANTCODE, ADM.DELIVERYDATE, AD.FROMLOCATIONCODE , AD.TOLOCATIONCODE, ADM.VEHICLECODE, CAR.VEHICLENO,SAP_CUS.NAME_ORG1, ADM.DISPATCHNO, AD.ORDERNO, AD.ORDERLINENO, ADM.DRIVERNAME, ADM.DRIVERMOBILE,  " +
                            "AD.ITEMCODE,SAP_PRO.DESCRIPTION, AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY, (SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE, SAP_PRO.UOM, NVL(TMS_OUT_RESULT.PD_YN,'N') AS PD_YN,  " +
                            "NVL(DECODE(SAP_PRO.UOM, 'EA' , TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT) , 0) AS WEIGHT2,  " +
                            "NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT,( AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) ) / (SAP_MA.UMREN / SAP_MA.UMREZ)  AS PACK_CNT , " +
                            "DECODE((SAP_MA.UMREN / SAP_MA.UMREZ), 1, '타이콘','지대') AS PACK_TYPE,  TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT " +
                            "FROM TMS_INPUT_PLOADM_CON ADM  " +
                            "JOIN TMS_INPUT_PLOADD_CON AD ON ADM.DISPATCHNO = AD.DISPATCHNO  " +
                            "LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CAR ON ADM.VEHICLECODE = CAR.VEHICLECODE  " +
                            "LEFT OUTER JOIN (SELECT RESOURCE_NO, DESCRIPTION, UOM FROM SAP_DI_PRODUCT GROUP BY RESOURCE_NO, DESCRIPTION, UOM) SAP_PRO  " +
                            "ON AD.ITEMCODE = SAP_PRO.RESOURCE_NO  " +
                            "LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS ON AD.TOLOCATIONCODE = SAP_CUS.PARTNER  " +
                            "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO  " +
                            "AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO   " +
                            "LEFT OUTER JOIN SAP_MARM SAP_MA ON AD.ITEMCODE = SAP_MA.MATNR AND SAP_MA.MEINH = 'KG'  " +
                            $"WHERE TMS_OUT_RESULT.IS_NO = '{is_no}' AND AD.TOLOCATIONCODE = '{sCustCd}' " +
                            "ORDER BY ADM.DELIVERYDATE DESC,  ADM.DISPATCHNO, AD.ORDERNO, AD.ORDERLINENO  ";
                    }
                    else if (carInType == "004" || carInType == "005")
                    {
                        SQL
                        = "SELECT SAP_SM.TRAID AS VEHICLENO, SAP_SM.WERKS_GR AS PLANTCODE, SAP_PLANT.P_DESCRIPTION AS NAME_ORG1, SAP_SM.TRANS_DATE AS DELIVERYDATE,  SAP_SD.TKNUM AS DISPATCHNO,  " +
                            "SAP_SD.VBELN AS ORDERNO, SAP_SD.POSNR AS ORDERLINENO, SAP_SD.MATNR AS ITEMCODE,SAP_PRO.DESCRIPTION, SAP_SD.LFIMG  * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY,   " +
                            "(SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE, SAP_PRO.UOM,  " +
                            "NVL(TMS_OUT_RESULT.PD_YN,'N') AS PD_YN, NVL( DECODE(SAP_PRO.UOM, 'EA' , TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT), 0) AS WEIGHT,  " +
                            "NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT, SAP_SD.LFIMG as PACK_CNT,   " +
                            "DECODE((SAP_MA.UMREN / SAP_MA.UMREZ), 1, '타이콘','지대') AS PACK_TYPE,  TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT " +
                            "FROM SAP_INPUT_PROTRANSM SAP_SM JOIN SAP_INPUT_PROTRANSD SAP_SD  " +
                            "ON SAP_SM.TKNUM = SAP_SD.TKNUM   " +
                            "LEFT OUTER JOIN SAP_DI_PRODUCT SAP_PRO ON SAP_SD.MATNR = SAP_PRO.RESOURCE_NO  AND SAP_SM.WERKS_GR = SAP_PRO.PLANT_CODE   " +
                            "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON SAP_SM.TKNUM = TMS_OUT_RESULT.DISPATCHNO   " +
                            "AND SAP_SD.VBELN = TMS_OUT_RESULT.ORDERNO AND SAP_SD.POSNR = TMS_OUT_RESULT.ORDERLINENO   " +
                            "LEFT OUTER JOIN SAP_MARM SAP_MA ON SAP_SD.MATNR = SAP_MA.MATNR AND SAP_MA.MEINH = 'KG'    " +
                            "LEFT OUTER JOIN SAP_DI_PLANT SAP_PLANT ON SAP_SM.WERKS_GR = SAP_PLANT.PLANT_CODE  " +
                            $"WHERE  TMS_OUT_RESULT.IS_NO = '{is_no}' AND SAP_SM.WERKS_GR = '{sCustCd}' " +
                            "ORDER BY  SAP_SD.TKNUM, SAP_SD.VBELN,SAP_SD.POSNR   ";

                    }

                    DataSet ListDs = Dbconn.conn.ExecutDataset(SQL);

                    string fromLocation = string.Empty;
                    for (int r = 0; r < Dbconn.conn.getRowCnt(ListDs); r++)
                    {
                        string list_DESCRIPTION = Dbconn.conn.getData(ListDs, "DESCRIPTION", r);
                        string list_STD_LOT_SIZE = Dbconn.conn.getData(ListDs, "STD_LOT_SIZE", r);
                        string list_PACK_TYPE = Dbconn.conn.getData(ListDs, "PACK_TYPE", r);
                        string list_QTY = Dbconn.conn.getData(ListDs, "QTY", r);
                        string list_WEIGHT = Dbconn.conn.getData(ListDs, "WEIGHT", r);

                        fromLocation = Dbconn.conn.getData(ListDs, "FROMLOCATIONCODE", r);

                        printSheet.Document.Worksheets[0].Cells["A" + excelListPos.ToString()].Value = (r + 1).ToString();
                        printSheet.Document.Worksheets[0].Cells["B" + excelListPos.ToString()].Value = list_DESCRIPTION;
                        printSheet.Document.Worksheets[0].Cells["M" + excelListPos.ToString()].Value = list_STD_LOT_SIZE;
                        printSheet.Document.Worksheets[0].Cells["R" + excelListPos.ToString()].Value = list_PACK_TYPE;
                        printSheet.Document.Worksheets[0].Cells["V" + excelListPos.ToString()].Value = Convert.ToInt32(list_QTY);
                        printSheet.Document.Worksheets[0].Cells["Y" + excelListPos.ToString()].Value = Convert.ToDecimal(list_WEIGHT);

                        excelListPos = excelListPos + 1;
                    }

                    ListDs.Dispose();

                    SQL = "SELECT CARM.VEHICLENO, CARM.DRIVERNAME, CARM.DRIVERMOBILE, CARM.CARRIERNAME " +
                            "FROM WAP_DECAR WD  " +
                            "LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CARM ON WD.INCAR_NO = CARM.VEHICLECODE  " +
                            "WHERE CARM.VEHICLECODE IS NOT NULL  " +
                            $"AND WD.IS_NO = '{is_no}'  ";

                    DataSet carDs = Dbconn.conn.ExecutDataset(SQL);
                    if (Dbconn.conn.getRowCnt(carDs) > 0)
                    {
                        printSheet.Document.Worksheets[0].Cells["E44"].Value = Dbconn.conn.getData(carDs, "CARRIERNAME", 0); //배송거래처
                        printSheet.Document.Worksheets[0].Cells["W43"].Value = Dbconn.conn.getData(carDs, "VEHICLENO", 0); //차량번호
                        printSheet.Document.Worksheets[0].Cells["AB43"].Value = Dbconn.conn.getData(carDs, "DRIVERMOBILE", 0); //기사연락처
                        printSheet.Document.Worksheets[0].Cells["AG43"].Value = Dbconn.conn.getData(carDs, "DRIVERNAME", 0); //배송기사명
                    }

                    //반품일 경우 주소 교체 (출발지,도착지 반대로)
                    if (carOrderType.Contains("ZLR"))
                    {
                        SQL = $"SELECT NAME_ORG1 FROM SAP_DI_CUSTOMER WHERE PARTNER = '{fromLocation}' ";
                        DataSet fromCusDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(fromCusDs) == 1)
                        {
                            printSheet.Document.Worksheets[0].Cells["AC8"].Value = Dbconn.conn.getData(fromCusDs, "NAME_ORG1", 0);
                        }

                        printSheet.Document.Worksheets[0].Cells["AC9"].Value = string.Empty;
                        printSheet.Document.Worksheets[0].Cells["E44"].Value = Dbconn.conn.getData(carDs, "CARRIERNAME", 0);

                        fromCusDs.Dispose();
                    }
                    carDs.Dispose();

                    //파레트, 피무계 정보
                    if (carInType == "002" || carInType == "003")
                    {
                        SQL = "SELECT PM.PTMCDNM, WOA.WEIGHT * WOA.PD_QTY AS SUM_QTY, WOA.PD_QTY   " +
                                "FROM WAP_PA_MASTER PM JOIN WAP_OUT_ADD WOA ON PM.PTMCD = WOA.PTMCD " +
                                $"WHERE WOA.IS_NO = '{is_no}' AND WOA.PARTNER = '{sCustCd}'  ";

                        DataSet custPaInputDs = Dbconn.conn.ExecutDataset(SQL);
                        int paInputCnt = Dbconn.conn.getRowCnt(custPaInputDs);

                        for (int p = 0; p < paInputCnt; p++)
                        {
                            if (paInputCnt > 0)
                            {
                                if (p == 0)
                                {
                                    printSheet.Document.Worksheets[0].Cells["E42"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["E43"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                                }

                                if (p == 1)
                                {
                                    printSheet.Document.Worksheets[0].Cells["H42"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["H43"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                                }

                                if (p == 2)
                                {
                                    printSheet.Document.Worksheets[0].Cells["K42"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["K43"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                                }

                                if (p == 3)
                                {
                                    printSheet.Document.Worksheets[0].Cells["P42"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["P43"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                                }

                                if (p == 4)
                                {
                                    printSheet.Document.Worksheets[0].Cells["T42"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["T43"].Value = Dbconn.conn.getData(custPaInputDs, "PD_QTY", p);
                                }
                            }
                        }
                    }

                    printSheet.Document.CalculateFull();
                    printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;

                    Margins pageMargins = printSheet.Document.Worksheets[0].ActiveView.Margins;
                    pageMargins.Left = 0.2F;
                    pageMargins.Top = 0.15F;
                    pageMargins.Right = 0;
                    pageMargins.Bottom = 0;
                    //pageMargins.Header = 1;
                    //pageMargins.Footer = 0.4F;

                    printSheet.Options.Print.ShowMarginsWarning = false;
                    //printSheet.Print();
                    printSheet.ShowPrintPreview();

                } //for

                cusListDs.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Source);
            }

        }


        //검근표 발행
        public static void PrintWeighingSheet(string is_no, bool f_print = false)
        {
            try
            {
                string SQL = string.Empty;

                string factory = clsCommon.PlantCode; //공장코드
                string carInType = clsCarUtil.returnCarType(is_no); //입차구분
                string carType = clsCarUtil.returnCarGubun2(is_no); //차량구분

                int pageCnt = 0;
                DataSet workCntDs = null;

                if (carType == "벌크")
                {
                    SQL = "SELECT WD.DISPATCHNO, WD.ORDERNO, WD.ORDERLINENO " +
                        "FROM ( " +
                        "SELECT  WD.CAR_TYPE,WD.IS_NO, TMS_RESULT.DISPATCHNO, TMS_RESULT.ORDERNO, TMS_RESULT.ORDERLINENO, TMS_RESULT.I_TIME " +
                        "FROM WAP_DECAR WD LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CARMASTER  " +
                        "ON WD.INCAR_NO = CARMASTER.VEHICLECODE  " +
                        "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_RESULT ON WD.IS_NO = TMS_RESULT.IS_NO " +
                        "WHERE WD.PC_STATUS = '20' " +
                        ") WD  " +
                        $"WHERE WD.IS_NO = '{is_no}' " +
                        "ORDER BY WD.I_TIME ";

                    workCntDs = Dbconn.conn.ExecutDataset(SQL);
                    pageCnt = Dbconn.conn.getRowCnt(workCntDs);

                    if (pageCnt == 0)
                    {
                        clsLog.logSave(SQL, 0);
                        return;
                    }
                }
                else
                {
                    ShowMessageBox.Confirm("차량타입이 벌크타입이 아닙니다");
                    return;
                }

                for (int i = 0; i < pageCnt; i++)
                {
                    string sDisNo = Dbconn.conn.getData(workCntDs, "DISPATCHNO", i);
                    string sOrderNo = Dbconn.conn.getData(workCntDs, "ORDERNO", i);
                    string sOrderLineNo = Dbconn.conn.getData(workCntDs, "ORDERLINENO", i);

                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                    printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\weight.xlsx", DocumentFormat.Xlsx);

                    //로그인 공장코드정보
                    string Factory = clsCommon.PlantCode;

                    if (Factory == "P101" || Factory == "P102")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_harim, printSheet.Document.Worksheets[0].Cells["B20"]);
                    }
                    else if (Factory == "PJ01" || Factory == "PJ04")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_jeil, printSheet.Document.Worksheets[0].Cells["B20"]);
                    }
                    else if (Factory == "P201")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_allpum, printSheet.Document.Worksheets[0].Cells["B20"]);
                    }

                    SQL = "SELECT AD.PLANTCODE, ADM.DELIVERYDATE, AD.TOLOCATIONCODE, ADM.VEHICLECODE, CAR.VEHICLENO,SAP_CUS.NAME_ORG1,SAP_CUS2.NAME_ORG1 AS NAME_ORG2 , ADM.DISPATCHNO, AD.ORDERNO, AD.ORDERLINENO, ADM.DRIVERNAME, ADM.DRIVERMOBILE,  " +
                            "AD.ITEMCODE,SAP_PRO.DESCRIPTION, AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY, (SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE, SAP_PRO.UOM, NVL(TMS_OUT_RESULT.PD_YN,'N') AS PD_YN,  " +
                            "TMS_OUT_RESULT.WEIGHT AS WEIGTH,  " +
                            "NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT,( AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) ) / (SAP_MA.UMREN / SAP_MA.UMREZ)  AS PACK_CNT , " +
                            "TMS_OUT_RESULT.BI_NUM, NVL(SAP_CUS.MOD_NUMBER, SAP_CUS.TEL_NUMBER_1) AS TEL_NUMBER_1, SAP_CUS.RLTYP, TMS_OUT_RESULT.ZERO_W, TMS_OUT_RESULT.BEFORE_WEIGHT,  " +
                            "TO_CHAR(BEFORE_WEIGHT_TIME, 'YYYY-MM-DD HH24:MI') AS BEFORE_WEIGHT_TIME, TO_CHAR(WEIGHT_TIME, 'YYYY-MM-DD HH24:MI') AS WEIGHT_TIME, ADM.DISPATCHMEMO,  AD.ITEM_REMARK   " +
                            "FROM TMS_INPUT_PLOADM_CON ADM  " +
                            "JOIN TMS_INPUT_PLOADD_CON AD ON ADM.DISPATCHNO = AD.DISPATCHNO  " +
                            "LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CAR ON ADM.VEHICLECODE = CAR.VEHICLECODE  " +
                            "LEFT OUTER JOIN (SELECT RESOURCE_NO, DESCRIPTION, UOM FROM SAP_DI_PRODUCT GROUP BY RESOURCE_NO, DESCRIPTION, UOM) SAP_PRO  " +
                            "ON AD.ITEMCODE = SAP_PRO.RESOURCE_NO  " +
                            "LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS ON AD.TOLOCATIONCODE = SAP_CUS.PARTNER " +
                            "LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS2 ON AD.FROMLOCATIONCODE = SAP_CUS2.PARTNER  " +
                            "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO  " +
                            "AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO   " +
                            "LEFT OUTER JOIN SAP_MARM SAP_MA ON AD.ITEMCODE = SAP_MA.MATNR AND SAP_MA.MEINH = 'KG'  " +
                            $"WHERE TMS_OUT_RESULT.DISPATCHNO = '{sDisNo}' AND TMS_OUT_RESULT.ORDERNO = '{sOrderNo}' AND TMS_OUT_RESULT.ORDERLINENO = '{sOrderLineNo}'  ";

                    DataSet reportDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(reportDs) == 0)
                    {
                        clsLog.logSave(SQL, 0);
                        continue;
                    }

                    string sDispatchno = Dbconn.conn.getData(reportDs, "DISPATCHNO", 0);

                    string sCarFullNum = Dbconn.conn.getData(reportDs, "VEHICLENO", 0);
                    string sDriverName = Dbconn.conn.getData(reportDs, "DRIVERNAME", 0);
                    string sDriverHp = Dbconn.conn.getData(reportDs, "DRIVERMOBILE", 0);
                    string sCustNm = Dbconn.conn.getData(reportDs, "NAME_ORG1", 0);
                    string sFromNm = Dbconn.conn.getData(reportDs, "NAME_ORG2", 0);
                    string sPlanQty = Dbconn.conn.getData(reportDs, "PLANQTY", 0);
                    string sDescName = Dbconn.conn.getData(reportDs, "DESCRIPTION", 0);
                    string sBiNum = Dbconn.conn.getData(reportDs, "BI_NUM", 0);
                    string sCusHp = Dbconn.conn.getData(reportDs, "TEL_NUMBER_1", 0);
                    string sWeigth = Dbconn.conn.getData(reportDs, "WEIGTH", 0);


                    string sInWeight = Dbconn.conn.getData(reportDs, "BEFORE_WEIGHT", 0);
                    string sInWeightTime = Dbconn.conn.getData(reportDs, "BEFORE_WEIGHT_TIME", 0);
                    string sOutWeight = Dbconn.conn.getData(reportDs, "ZERO_W", 0);
                    string sOutWeightTime = Dbconn.conn.getData(reportDs, "WEIGHT_TIME", 0);

                    string sCusAddr = Dbconn.conn.getData(reportDs, "RLTYP", 0);

                    string sItmCd = Dbconn.conn.getData(reportDs, "ITEMCODE", 0);
                    string sMemo = Dbconn.conn.getData(reportDs, "ITEM_REMARK", 0);

                    //차량번호
                    printSheet.Document.Worksheets[0].Cells["E5"].Value = sCarFullNum;

                    //기사명
                    printSheet.Document.Worksheets[0].Cells["K5"].Value = sDriverName;

                    //계량번호
                    printSheet.Document.Worksheets[0].Cells["O5"].Value = is_no;

                    //입차시간
                    printSheet.Document.Worksheets[0].Cells["E6"].Value = sInWeightTime;

                    //입차중량
                    if (clsUtil.isNumber(sInWeight))
                    {
                        printSheet.Document.Worksheets[0].Cells["O6"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                    }

                    //출차시간
                    printSheet.Document.Worksheets[0].Cells["E7"].Value = sOutWeightTime;

                    //출차중량
                    if (clsUtil.isNumber(sOutWeight))
                    {
                        printSheet.Document.Worksheets[0].Cells["O7"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));
                    }

                    //거래처
                    printSheet.Document.Worksheets[0].Cells["E8"].Value = sCustNm;

                    int intPweight = 0;
                    //피무계합산
                    if (carInType == "001")
                    {
                        SQL = $"SELECT NVL(SUM(WEIGHT * PD_QTY),0) AS P_SUM FROM WAP_IN_ADD WHERE IS_NO = '{is_no}'";
                    }
                    else if (carInType == "002" || carInType == "003")
                    {
                        SQL = $"SELECT NVL(SUM(WEIGHT * PD_QTY),0) AS P_SUM FROM WAP_OUT_ADD WHERE IS_NO = '{is_no}' ";
                    }

                    DataSet pWeightDs = Dbconn.conn.ExecutDataset(SQL);
                    string sPweight = Dbconn.conn.getData(pWeightDs, "P_SUM", 0); //피합산무계

                    if (clsUtil.isNumber(sPweight))
                    {
                        if (Convert.ToDecimal(sPweight) > 0)
                        {
                            if (carInType == "001")
                            {

                            }
                            else
                            {
                                if (factory == "PJ01" || factory == "PJ04") //대전,함안제일사료 피무계 올림처림
                                {
                                    sPweight = Convert.ToString(IntCeiling(Convert.ToDouble(sPweight), -1));
                                }
                                else
                                {
                                    sPweight = Convert.ToString(IntRound(Convert.ToDouble(sPweight), -1));
                                }
                            }


                            intPweight = Convert.ToInt32(sPweight);
                            //피중량
                            printSheet.Document.Worksheets[0].Cells["O8"].Value = sPweight;
                        }
                    }

                    //배송처
                    printSheet.Document.Worksheets[0].Cells["E9"].Value = sCusAddr;

                    //실중량
                    if (clsUtil.isNumber(sWeigth))
                    {
                        printSheet.Document.Worksheets[0].Cells["O9"].Value = String.Format("{0:#,###}", Convert.ToDecimal(sWeigth) - intPweight);
                    }

                    //연락처
                    printSheet.Document.Worksheets[0].Cells["E10"].Value = sCusHp;

                    /*  벌크는 제외
                    //송장량
                    if (Factory != "P201")
                    {
                        if (clsUtil.isNumber(sPlanQty))
                        {
                            printSheet.Document.Worksheets[0].Cells["O10"].Value = String.Format("{0:#,###}", Convert.ToInt32(sPlanQty));
                        }
                    }
                    */

                    //품목
                    printSheet.Document.Worksheets[0].Cells["E13"].Value = sDescName;

                    if (string.IsNullOrEmpty(sMemo))
                    {
                        SQL = $"SELECT REMARK FROM BULK_ORDER WHERE DISPATCHNO = '{sDisNo}' AND ORDERNO = '{sOrderNo}' AND ORDERLINENO = '{sOrderLineNo}' ";
                        DataSet bulkRemarkDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(bulkRemarkDs) > 0)
                        {
                            sMemo = Dbconn.conn.getData(bulkRemarkDs, "REMARK", 0);
                            sMemo = sMemo.Replace("<br/>", "");
                        }
                    }

                    //비고
                    printSheet.Document.Worksheets[0].Cells["E14"].Value = sMemo;

                    //낟가리
                    printSheet.Document.Worksheets[0].Cells["E15"].Value = "";

                    //봉인번호
                    printSheet.Document.Worksheets[0].Cells["E16"].Value = sBiNum;

                    //년월일
                    printSheet.Document.Worksheets[0].Cells["M15"].Value = DateTime.Now.ToString("yyyy") + "년 " + DateTime.Now.ToString("MM") + "월 " + DateTime.Now.ToString("dd") + "일";

                    //배송기사확인
                    string deliveryEmp = string.Empty;

                    if (!string.IsNullOrEmpty(sDriverName))
                    {
                        deliveryEmp = "배송기사 : " + sDriverName;
                    }

                    printSheet.Document.Worksheets[0].Cells["M17"].Value = deliveryEmp;

                    if (carType == "벌크")
                    {
                        //배합사료성분표
                        SQL = "SELECT " +
                                "MIN(DECODE(FIELD_FLAG, 'A', FIELD_VALUE01,'')) AS A1, " +
                                "MIN(DECODE(FIELD_FLAG, 'B', FIELD_VALUE01,'')) AS B1, " +
                                "MIN(DECODE(FIELD_FLAG, 'C', FIELD_VALUE01,'')) AS C1, " +
                                "MIN(DECODE(FIELD_FLAG, 'D', FIELD_VALUE01,'')) AS D1, " +
                                "MIN(DECODE(FIELD_FLAG, 'E', FIELD_VALUE01,'')) AS E1, " +
                                "MIN(DECODE(FIELD_FLAG, 'F', FIELD_VALUE01,'')) AS F1, " +
                                "MIN(DECODE(FIELD_FLAG, 'G', FIELD_VALUE01,'')) AS G1, " +
                                "MIN(DECODE(FIELD_FLAG, 'H', FIELD_VALUE01,'')) AS H1, " +
                                "MIN(DECODE(FIELD_FLAG, 'K', FIELD_VALUE01,'')) AS K1, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'L' AND TO_NUMBER(SEQ_NO) = '1') THEN FIELD_VALUE01 END ) AS L1, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'L' AND TO_NUMBER(SEQ_NO) = '2') THEN FIELD_VALUE01 END ) AS L2, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'L' AND TO_NUMBER(SEQ_NO) = '3') THEN FIELD_VALUE01 END ) AS L3, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'L' AND TO_NUMBER(SEQ_NO) = '4') THEN FIELD_VALUE01 END ) AS L4, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '1') THEN FIELD_VALUE01 END ) AS M1, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '2') THEN FIELD_VALUE01 END ) AS M2, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '3') THEN FIELD_VALUE01 END ) AS M3, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '4') THEN FIELD_VALUE01 END ) AS M4, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '5') THEN FIELD_VALUE01 END ) AS M5, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '6') THEN FIELD_VALUE01 END ) AS M6, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '7') THEN FIELD_VALUE01 END ) AS M7, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '8') THEN FIELD_VALUE01 END ) AS M8, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'M' AND TO_NUMBER(SEQ_NO) = '9') THEN FIELD_VALUE01 END ) AS M9, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '1') THEN FIELD_VALUE01 END ) AS N1, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '2') THEN FIELD_VALUE01 END ) AS N2, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '3') THEN FIELD_VALUE01 END ) AS N3, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '4') THEN FIELD_VALUE01 END ) AS N4, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '5') THEN FIELD_VALUE01 END ) AS N5, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '6') THEN FIELD_VALUE01 END ) AS N6, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '7') THEN FIELD_VALUE01 END ) AS N7, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '1') THEN FIELD_VALUE02 END ) AS N11, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '2') THEN FIELD_VALUE02 END ) AS N22, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '3') THEN FIELD_VALUE02 END ) AS N33, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '4') THEN FIELD_VALUE02 END ) AS N44, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '5') THEN FIELD_VALUE02 END ) AS N55, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '6') THEN FIELD_VALUE02 END ) AS N66, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'N' AND TO_NUMBER(SEQ_NO) = '7') THEN FIELD_VALUE02 END ) AS N77, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'O' AND TO_NUMBER(SEQ_NO) = '1') THEN FIELD_VALUE01 END ) AS O1, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'O' AND TO_NUMBER(SEQ_NO) = '2') THEN FIELD_VALUE01 END ) AS O2, " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'O' AND TO_NUMBER(SEQ_NO) = '3') THEN FIELD_VALUE01 END ) AS O3,  " +
                                "MIN(CASE WHEN (FIELD_FLAG = 'J' AND TO_NUMBER(SEQ_NO) = '1') THEN FIELD_VALUE01 END ) AS J1  " +
                                "FROM SAP_IN_PLA  " +
                                $"WHERE RESOURCE_NO = '{sItmCd}'  ";

                        DataSet dosingPrintDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(dosingPrintDs) == 1)
                        {
                            //제품명
                            printSheet.Document.Worksheets[0].Cells["E25"].Value = sDescName;

                            //1. 성분등록번호
                            printSheet.Document.Worksheets[0].Cells["B27"].Value = "1. 성분등록번호 : " + Dbconn.conn.getData(dosingPrintDs, "A1", 0).Trim();

                            //2. 사료의명칭
                            printSheet.Document.Worksheets[0].Cells["B28"].Value = "2. 사료의명칭 : " + Dbconn.conn.getData(dosingPrintDs, "B1", 0).Trim();

                            //3. 사료의 형태
                            printSheet.Document.Worksheets[0].Cells["B29"].Value = "3. 사료의 형태 : " + Dbconn.conn.getData(dosingPrintDs, "C1", 0).Trim();

                            //4. 사료의 용도
                            printSheet.Document.Worksheets[0].Cells["B30"].Value = "4. 사료의 용도 : " + Dbconn.conn.getData(dosingPrintDs, "D1", 0).Trim();

                            //5. 실제 중량
                            printSheet.Document.Worksheets[0].Cells["B31"].Value = "5. 실제 중량 : " + Dbconn.conn.getData(dosingPrintDs, "E1", 0).Trim();

                            //6. 제조년월일
                            printSheet.Document.Worksheets[0].Cells["B32"].Value = "6. 제조년월일 : " + Dbconn.conn.getData(dosingPrintDs, "F1", 0).Trim();

                            //7. 유통기한
                            printSheet.Document.Worksheets[0].Cells["B33"].Value = "7. 유통기한 : " + Dbconn.conn.getData(dosingPrintDs, "G1", 0).Trim();

                            //8. 판매원
                            printSheet.Document.Worksheets[0].Cells["B34"].Value = "8. 판매원 : " + Dbconn.conn.getData(dosingPrintDs, "H1", 0).Trim();

                            //9. 재포장내용
                            printSheet.Document.Worksheets[0].Cells["B35"].Value = "9. 재포장내용 : 해당없음";

                            //10. 동물의약품
                            printSheet.Document.Worksheets[0].Cells["B36"].Value = Dbconn.conn.getData(dosingPrintDs, "K1", 0).Trim();

                            //12.주의사항
                            string CAUTIONS = string.Empty;


                            string m1 = Dbconn.conn.getData(dosingPrintDs, "M1", 0).Trim();
                            string m2 = Dbconn.conn.getData(dosingPrintDs, "M2", 0).Trim();
                            string m3 = Dbconn.conn.getData(dosingPrintDs, "M3", 0).Trim();
                            string m4 = Dbconn.conn.getData(dosingPrintDs, "M4", 0).Trim();
                            string m5 = Dbconn.conn.getData(dosingPrintDs, "M5", 0).Trim();
                            string m6 = Dbconn.conn.getData(dosingPrintDs, "M6", 0).Trim();
                            string m7 = Dbconn.conn.getData(dosingPrintDs, "M7", 0).Trim();
                            string m8 = Dbconn.conn.getData(dosingPrintDs, "M8", 0).Trim();
                            string m9 = Dbconn.conn.getData(dosingPrintDs, "M9", 0).Trim();

                            CAUTIONS = CAUTIONS + " \r\n";

                            if (!string.IsNullOrEmpty(m1))
                            {
                                CAUTIONS = CAUTIONS + m1 + "\r\n";
                            }


                            if (!string.IsNullOrEmpty(m2))
                            {
                                CAUTIONS = CAUTIONS + m2 + "\r\n";
                            }


                            if (!string.IsNullOrEmpty(m3))
                            {
                                CAUTIONS = CAUTIONS + m3 + "\r\n";
                            }


                            if (!string.IsNullOrEmpty(m4))
                            {
                                CAUTIONS = CAUTIONS + m4 + "\r\n";
                            }


                            if (!string.IsNullOrEmpty(m5))
                            {
                                m5 = m5.Replace(".", "\r\n");
                                CAUTIONS = CAUTIONS + m5 + "\r\n";
                            }


                            if (!string.IsNullOrEmpty(m6))
                            {
                                CAUTIONS = CAUTIONS + m6 + "\r\n";
                            }

                            if (!string.IsNullOrEmpty(m7))
                            {
                                CAUTIONS = CAUTIONS + m7 + "\r\n";
                            }

                            if (!string.IsNullOrEmpty(m8))
                            {
                                CAUTIONS = CAUTIONS + m8 + "\r\n";
                            }

                            if (!string.IsNullOrEmpty(m9))
                            {
                                CAUTIONS = CAUTIONS + m9 + "\r\n";
                            }

                            printSheet.Document.Worksheets[0].Cells["B39"].Alignment.WrapText = true;
                            printSheet.Document.Worksheets[0].Cells["B39"].AutoFitRows();

                            printSheet.Document.Worksheets[0].Cells["B39"].Value = CAUTIONS;
                            printSheet.Document.Worksheets[0].Cells["B39"].AutoFitRows();
                            printSheet.Document.Worksheets[0].Cells["B39"].AutoFitColumns();


                            printSheet.Document.Worksheets[0].Cells["C49"].Value = Dbconn.conn.getData(dosingPrintDs, "J1", 0).Trim();

                            //13.등록성분량
                            printSheet.Document.Worksheets[0].Cells["M32"].Value = Dbconn.conn.getData(dosingPrintDs, "N1", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["M33"].Value = Dbconn.conn.getData(dosingPrintDs, "N2", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["M34"].Value = Dbconn.conn.getData(dosingPrintDs, "N3", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["M35"].Value = Dbconn.conn.getData(dosingPrintDs, "N4", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["M36"].Value = Dbconn.conn.getData(dosingPrintDs, "N5", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["M37"].Value = Dbconn.conn.getData(dosingPrintDs, "N6", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["M38"].Value = Dbconn.conn.getData(dosingPrintDs, "N7", 0).Trim();

                            printSheet.Document.Worksheets[0].Cells["O32"].Value = Dbconn.conn.getData(dosingPrintDs, "N11", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["O33"].Value = Dbconn.conn.getData(dosingPrintDs, "N22", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["O34"].Value = Dbconn.conn.getData(dosingPrintDs, "N33", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["O35"].Value = Dbconn.conn.getData(dosingPrintDs, "N44", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["O36"].Value = Dbconn.conn.getData(dosingPrintDs, "N55", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["O37"].Value = Dbconn.conn.getData(dosingPrintDs, "N66", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["O38"].Value = Dbconn.conn.getData(dosingPrintDs, "N77", 0).Trim();

                            //14.사용한 원료의 명칭
                            printSheet.Document.Worksheets[0].Cells["M41"].Value = Dbconn.conn.getData(dosingPrintDs, "O1", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["M47"].Value = Dbconn.conn.getData(dosingPrintDs, "O2", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["M49"].Value = Dbconn.conn.getData(dosingPrintDs, "O3", 0).Trim();

                            //제조업자 상호 및 전화번호
                            printSheet.Document.Worksheets[0].Cells["C52"].Value = Dbconn.conn.getData(dosingPrintDs, "L1", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["C54"].Value = Dbconn.conn.getData(dosingPrintDs, "L2", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["C56"].Value = Dbconn.conn.getData(dosingPrintDs, "L3", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["C58"].Value = Dbconn.conn.getData(dosingPrintDs, "L4", 0).Trim();
                        }
                    }

                    reportDs.Dispose();

                    printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;

                    Margins pageMargins = printSheet.Document.Worksheets[0].ActiveView.Margins;
                    pageMargins.Left = 0.2F;
                    pageMargins.Top = 0.15F;
                    pageMargins.Right = 0;
                    pageMargins.Bottom = 0;
                    //pageMargins.Header = 1;
                    //pageMargins.Footer = 0.4F;

                    printSheet.Options.Print.ShowMarginsWarning = false;
                    //printSheet.Print();
                    printSheet.ShowPrintPreview();
                }

                clsUtil.Delay(800);

            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Source);
            }

        }

        public static void PrintWeighingSheetCopy(string is_no)
        {
            try
            {
                string SQL = string.Empty;

                string factory = clsCommon.PlantCode; //공장코드
                string carInType = clsCarUtil.returnCarType(is_no); //CAR_TYPE
                string carType = clsCarUtil.returnCarGubun2(is_no); //CAR GUBUN

                int pageCnt = 0;
                DataSet workCntDs = null;

                SQL = "SELECT WD.DISPATCHNO, WD.ORDERNO, WD.ORDERLINENO " +
                    "FROM ( " +
                    "SELECT  WD.CAR_TYPE,WD.IS_NO, TMS_RESULT.DISPATCHNO, TMS_RESULT.ORDERNO, TMS_RESULT.ORDERLINENO, TMS_RESULT.I_TIME " +
                    "FROM WAP_DECAR WD LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CARMASTER  " +
                    "ON WD.INCAR_NO = CARMASTER.VEHICLECODE  " +
                    "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_RESULT ON WD.IS_NO = TMS_RESULT.IS_NO " +
                    "WHERE WD.PC_STATUS = '20' " +
                    ") WD  " +
                    $"WHERE WD.IS_NO = '{is_no}' " +
                    "ORDER BY WD.I_TIME ";

                workCntDs = Dbconn.conn.ExecutDataset(SQL);
                pageCnt = Dbconn.conn.getRowCnt(workCntDs);

                if (pageCnt == 0)
                {
                    clsLog.logSave(SQL, 0);
                    return;
                }

                for (int i = 0; i < pageCnt; i++)
                {
                    string sDisNo = Dbconn.conn.getData(workCntDs, "DISPATCHNO", i);
                    string sOrderNo = Dbconn.conn.getData(workCntDs, "ORDERNO", i);
                    string sOrderLineNo = Dbconn.conn.getData(workCntDs, "ORDERLINENO", i);

                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                    printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\weight2.xlsx", DocumentFormat.Xlsx);

                    //공장코드
                    string Factory = clsCommon.PlantCode;

                    //공장코드에 따른 로고이미지 삽입
                    if (Factory == "P101" || Factory == "P102")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_harim, printSheet.Document.Worksheets[0].Cells["C21"]);
                    }
                    else if (Factory == "PJ01" || Factory == "PJ04")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_jeil, printSheet.Document.Worksheets[0].Cells["C21"]);
                    }
                    else if (Factory == "P201")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_allpum, printSheet.Document.Worksheets[0].Cells["C21"]);
                    }

                    SQL = "SELECT AD.PLANTCODE, ADM.DELIVERYDATE, AD.TOLOCATIONCODE, ADM.VEHICLECODE, CAR.VEHICLENO,SAP_CUS.NAME_ORG1,SAP_CUS2.NAME_ORG1 AS NAME_ORG2 , ADM.DISPATCHNO, AD.ORDERNO, AD.ORDERLINENO, ADM.DRIVERNAME, ADM.DRIVERMOBILE,  " +
                            "AD.ITEMCODE,SAP_PRO.DESCRIPTION, AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY, (SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE, SAP_PRO.UOM, NVL(TMS_OUT_RESULT.PD_YN,'N') AS PD_YN,  " +
                            " TMS_OUT_RESULT.WEIGHT AS WEIGTH,  " +
                            "NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT,( AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) ) / (SAP_MA.UMREN / SAP_MA.UMREZ)  AS PACK_CNT , " +
                            "TMS_OUT_RESULT.BI_NUM, NVL(SAP_CUS.MOD_NUMBER, SAP_CUS.TEL_NUMBER_1) AS TEL_NUMBER_1, SAP_CUS.RLTYP, TMS_OUT_RESULT.ZERO_W, TMS_OUT_RESULT.BEFORE_WEIGHT,  " +
                            "TO_CHAR(BEFORE_WEIGHT_TIME, 'YYYY-MM-DD HH24:MI') AS BEFORE_WEIGHT_TIME, TO_CHAR(WEIGHT_TIME, 'YYYY-MM-DD HH24:MI') AS WEIGHT_TIME, ADM.DISPATCHMEMO,  AD.ITEM_REMARK   " +
                            "FROM TMS_INPUT_PLOADM_CON ADM  " +
                            "JOIN TMS_INPUT_PLOADD_CON AD ON ADM.DISPATCHNO = AD.DISPATCHNO  " +
                            "LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CAR ON ADM.VEHICLECODE = CAR.VEHICLECODE  " +
                            "LEFT OUTER JOIN (SELECT RESOURCE_NO, DESCRIPTION, UOM FROM SAP_DI_PRODUCT GROUP BY RESOURCE_NO, DESCRIPTION, UOM) SAP_PRO  " +
                            "ON AD.ITEMCODE = SAP_PRO.RESOURCE_NO  " +
                            "LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS ON AD.TOLOCATIONCODE = SAP_CUS.PARTNER " +
                            "LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS2 ON AD.FROMLOCATIONCODE = SAP_CUS2.PARTNER  " +
                            "LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT ON AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO  " +
                            "AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO   " +
                            "LEFT OUTER JOIN SAP_MARM SAP_MA ON AD.ITEMCODE = SAP_MA.MATNR AND SAP_MA.MEINH = 'KG'  " +
                            $"WHERE TMS_OUT_RESULT.DISPATCHNO = '{sDisNo}' AND TMS_OUT_RESULT.ORDERNO = '{sOrderNo}' AND TMS_OUT_RESULT.ORDERLINENO = '{sOrderLineNo}'  ";

                    DataSet reportDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(reportDs) == 0)
                    {
                        clsLog.logSave(SQL, 0);
                        continue;
                    }

                    string sCarFullNum = Dbconn.conn.getData(reportDs, "VEHICLENO", 0);
                    string sDriverName = Dbconn.conn.getData(reportDs, "DRIVERNAME", 0);
                    string sDriverHp = Dbconn.conn.getData(reportDs, "DRIVERMOBILE", 0);
                    string sCustNm = Dbconn.conn.getData(reportDs, "NAME_ORG1", 0);
                    string sFromNm = Dbconn.conn.getData(reportDs, "NAME_ORG2", 0);
                    string sPlanQty = Dbconn.conn.getData(reportDs, "PLANQTY", 0);
                    string sDescName = Dbconn.conn.getData(reportDs, "DESCRIPTION", 0);
                    string sBiNum = Dbconn.conn.getData(reportDs, "BI_NUM", 0);
                    string sCusHp = Dbconn.conn.getData(reportDs, "TEL_NUMBER_1", 0);
                    string sWeigth = Dbconn.conn.getData(reportDs, "WEIGTH", 0);


                    string sInWeight = Dbconn.conn.getData(reportDs, "BEFORE_WEIGHT", 0);
                    string sInWeightTime = Dbconn.conn.getData(reportDs, "BEFORE_WEIGHT_TIME", 0);
                    string sOutWeight = Dbconn.conn.getData(reportDs, "ZERO_W", 0);
                    string sOutWeightTime = Dbconn.conn.getData(reportDs, "WEIGHT_TIME", 0);

                    string sCusAddr = Dbconn.conn.getData(reportDs, "RLTYP", 0);

                    string sItmCd = Dbconn.conn.getData(reportDs, "ITEMCODE", 0);
                    string sMemo = Dbconn.conn.getData(reportDs, "ITEM_REMARK", 0);

                    //차량번호
                    printSheet.Document.Worksheets[0].Cells["E5"].Value = sCarFullNum;
                    printSheet.Document.Worksheets[0].Cells["E24"].Value = sCarFullNum;

                    //기사명
                    printSheet.Document.Worksheets[0].Cells["K5"].Value = sDriverName;
                    printSheet.Document.Worksheets[0].Cells["K24"].Value = sDriverName;

                    //계량번호
                    printSheet.Document.Worksheets[0].Cells["O5"].Value = is_no;
                    printSheet.Document.Worksheets[0].Cells["O24"].Value = is_no;

                    //입차시간
                    printSheet.Document.Worksheets[0].Cells["E6"].Value = sInWeightTime;
                    printSheet.Document.Worksheets[0].Cells["E25"].Value = sInWeightTime;

                    //입차중량
                    if (clsUtil.isNumber(sInWeight))
                    {
                        printSheet.Document.Worksheets[0].Cells["O6"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                        printSheet.Document.Worksheets[0].Cells["O25"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                    }

                    //출차시간
                    printSheet.Document.Worksheets[0].Cells["E7"].Value = sOutWeightTime;
                    printSheet.Document.Worksheets[0].Cells["E26"].Value = sOutWeightTime;

                    //출차중량
                    if (clsUtil.isNumber(sOutWeight))
                    {
                        printSheet.Document.Worksheets[0].Cells["O7"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));
                        printSheet.Document.Worksheets[0].Cells["O26"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));
                    }

                    //거래처
                    printSheet.Document.Worksheets[0].Cells["E8"].Value = sCustNm;
                    printSheet.Document.Worksheets[0].Cells["E27"].Value = sCustNm;

                    int intPweight = 0;
                    //피무계합산
                    if (carInType == "001")
                    {
                        SQL = $"SELECT NVL(SUM(WEIGHT * PD_QTY),0) AS P_SUM FROM WAP_IN_ADD WHERE IS_NO = '{is_no}'";
                    }
                    else if (carInType == "002" || carInType == "003")
                    {
                        SQL = $"SELECT NVL(SUM(WEIGHT * PD_QTY),0) AS P_SUM FROM WAP_OUT_ADD WHERE IS_NO = '{is_no}' ";
                    }

                    DataSet pWeightDs = Dbconn.conn.ExecutDataset(SQL);
                    string sPweight = Dbconn.conn.getData(pWeightDs, "P_SUM", 0);

                    if (clsUtil.isNumber(sPweight))
                    {
                        if (Convert.ToDecimal(sPweight) > 0)
                        {
                            if (carInType == "001")
                            {

                            }
                            else
                            {
                                if (factory == "PJ01" || factory == "PJ04") //대전,함안제일사료 피무계 올림처림
                                {
                                    sPweight = Convert.ToString(IntCeiling(Convert.ToDouble(sPweight), -1));
                                }
                                else
                                {
                                    sPweight = Convert.ToString(IntRound(Convert.ToDouble(sPweight), -1));
                                }
                            }

                            intPweight = Convert.ToInt32(sPweight);
                            //피중량
                            printSheet.Document.Worksheets[0].Cells["O8"].Value = sPweight;
                            printSheet.Document.Worksheets[0].Cells["O27"].Value = sPweight;
                        }
                    }

                    //배송처
                    printSheet.Document.Worksheets[0].Cells["E9"].Value = sCusAddr;
                    printSheet.Document.Worksheets[0].Cells["E28"].Value = sCusAddr;

                    //실중량
                    if (clsUtil.isNumber(sWeigth))
                    {
                        printSheet.Document.Worksheets[0].Cells["O9"].Value = String.Format("{0:#,###}", Convert.ToDecimal(sWeigth) - intPweight);
                        printSheet.Document.Worksheets[0].Cells["O28"].Value = String.Format("{0:#,###}", Convert.ToDecimal(sWeigth) - intPweight);
                    }

                    //연락처
                    printSheet.Document.Worksheets[0].Cells["E10"].Value = sCusHp;
                    printSheet.Document.Worksheets[0].Cells["E29"].Value = sCusHp;

                    /*
                    //송장량  
                    if (Factory != "P201")
                    {
                        if (clsUtil.isNumber(sPlanQty))
                        {
                            printSheet.Document.Worksheets[0].Cells["O10"].Value = String.Format("{0:#,###}", Convert.ToInt32(sPlanQty));
                        }
                    }
                    */

                    //품목
                    printSheet.Document.Worksheets[0].Cells["E13"].Value = sDescName;
                    printSheet.Document.Worksheets[0].Cells["E32"].Value = sDescName;


                    if (string.IsNullOrEmpty(sMemo))
                    {
                        SQL = $"SELECT REMARK FROM BULK_ORDER WHERE DISPATCHNO = '{sDisNo}' AND ORDERNO = '{sOrderNo}' AND ORDERLINENO = '{sOrderLineNo}' ";
                        DataSet bulkRemarkDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(bulkRemarkDs) > 0)
                        {
                            sMemo = Dbconn.conn.getData(bulkRemarkDs, "REMARK", 0);
                            sMemo = sMemo.Replace("<br/>", "");
                        }
                    }

                    //비고
                    printSheet.Document.Worksheets[0].Cells["E14"].Value = sMemo;
                    printSheet.Document.Worksheets[0].Cells["E33"].Value = sMemo;

                    //낟가리
                    printSheet.Document.Worksheets[0].Cells["E15"].Value = "";

                    //봉인번호
                    printSheet.Document.Worksheets[0].Cells["E16"].Value = sBiNum;
                    printSheet.Document.Worksheets[0].Cells["E35"].Value = sBiNum;

                    //년월일
                    printSheet.Document.Worksheets[0].Cells["M15"].Value = DateTime.Now.ToString("yyyy") + "년 " + DateTime.Now.ToString("MM") + "월 " + DateTime.Now.ToString("dd") + "일";
                    printSheet.Document.Worksheets[0].Cells["M34"].Value = DateTime.Now.ToString("yyyy") + "년 " + DateTime.Now.ToString("MM") + "월 " + DateTime.Now.ToString("dd") + "일";

                    //배송기사확인
                    string deliveryEmp = string.Empty;

                    if (!string.IsNullOrEmpty(sDriverName))
                    {
                        deliveryEmp = "배송기사 : " + sDriverName;
                    }

                    printSheet.Document.Worksheets[0].Cells["M17"].Value = deliveryEmp;
                    printSheet.Document.Worksheets[0].Cells["M36"].Value = deliveryEmp;

                    printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;

                    Margins pageMargins = printSheet.Document.Worksheets[0].ActiveView.Margins;
                    pageMargins.Left = 0.2F;
                    pageMargins.Top = 0.15F;
                    pageMargins.Right = 0;
                    pageMargins.Bottom = 0;
                    //pageMargins.Header = 1;
                    //pageMargins.Footer = 0.4F;

                    printSheet.Options.Print.ShowMarginsWarning = false;
                    //printSheet.Print();
                    printSheet.ShowPrintPreview();
                }

                clsUtil.Delay(800);

            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Source);
            }

        }

        public static void PrintWeighingSheet2(string is_no)
        {
            try
            {
                string SQL = string.Empty;

                string factory = clsCommon.PlantCode;
                string carInType = clsCarUtil.returnCarType(is_no);
                string carType = clsCarUtil.returnCarGubun2(is_no);

                //원료, 기타차량만
                if (carInType == "001" || carInType == "002" || carInType == "007")
                {
                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                    printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\weight.xlsx", DocumentFormat.Xlsx);

                    /*
                    dt.Rows.Add("P101", "김제하림");
                    dt.Rows.Add("P102", "정읍하림");
                    dt.Rows.Add("P201", "상주올품");
                    dt.Rows.Add("PJ01", "대전제일사료");
                    dt.Rows.Add("PJ04", "함안제일사료");
                    */

                    string Factory = clsCommon.PlantCode;

                    if (Factory == "P101" || Factory == "P102")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_harim, printSheet.Document.Worksheets[0].Cells["B21"]);
                    }
                    else if (Factory == "PJ01" || Factory == "PJ04")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_jeil, printSheet.Document.Worksheets[0].Cells["B21"]);
                    }
                    else if (Factory == "P201")
                    {
                        printSheet.Document.Worksheets[0].Pictures.AddPicture(global::Core.Properties.Resources.excel_allpum, printSheet.Document.Worksheets[0].Cells["B21"]);
                    }

                    SQL = "SELECT INCAR_NO, IN_WEIGHT, " +
                          "IN_WEIGHT, " +
                          "TO_CHAR(INCAR_DATE, 'YYYY-MM-DD HH24:MI') AS INCAR_DATE, " +
                          "OUT_WEIGHT, " +
                          "TO_CHAR(OUTCAR_DATE, 'YYYY-MM-DD HH24:MI') AS OUTCAR_DATE, " +
                          "ABS(IN_WEIGHT - OUT_WEIGHT) AS REAL_WEIGHT, ETC_DETAIL  " +
                          $"FROM WAP_DECAR WHERE PC_STATUS = '20' AND IS_NO = '{is_no}'  ";

                    DataSet listDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(listDs) == 0)
                    {
                        return;
                    }

                    string sCarFullNum = Dbconn.conn.getData(listDs, "INCAR_NO", 0);
                    string sEtcDetail = Dbconn.conn.getData(listDs, "ETC_DETAIL", 0);

                    string sInWeight = Dbconn.conn.getData(listDs, "IN_WEIGHT", 0);
                    string sInWeightTime = Dbconn.conn.getData(listDs, "INCAR_DATE", 0);
                    string sOutWeight = Dbconn.conn.getData(listDs, "OUT_WEIGHT", 0);
                    string sOutWeightTime = Dbconn.conn.getData(listDs, "OUTCAR_DATE", 0);
                    string sWeigth = Dbconn.conn.getData(listDs, "REAL_WEIGHT", 0);

                    if (carInType == "002")
                    {
                        SQL = $"SELECT VEHICLENO FROM TMS_INPUT_CARMASTER_CON WHERE VEHICLECODE = '{sCarFullNum}' ";
                        DataSet carNameDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(carNameDs) > 0)
                        {
                            sCarFullNum = Dbconn.conn.getData(carNameDs, "VEHICLENO", 0);
                        }
                    }


                    string sDescName = string.Empty;

                    string sCustNm = string.Empty;
                    string sPlanQty = string.Empty;

                    int forCnt = 1;

                    SQL = "SELECT WGD.R_GR_QNTY " +
                        "FROM WAP_GOCARD WGD  " +
                        "JOIN INGRED ING ON WGD.MATNR = ING.RESOURCE_NO AND WGD.WERKS = ING.PLANT_CODE  " +
                        "AND WGD.PC_STATUS = '2' " +
                        $"WHERE WGD.IS_NO = '{is_no}' AND NVL(ING.MANUALYN,'N') = 'Y' ";

                    DataSet invoiceCntDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(invoiceCntDs) > 1)
                    {
                        forCnt = Dbconn.conn.getRowCnt(invoiceCntDs);
                    }

                    for (int i = 0; i < forCnt; i++)
                    {
                        if (carInType == "001")
                        {
                            SQL = "SELECT SUM(WG.INVOICE_WEIGHT) AS INVOICE_WEIGHT, MIN(SAP_CUS.NAME_ORG1) AS CUST_NAME " +
                                 "FROM WAP_GOCAR WG LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS ON WG.PARTNER = SAP_CUS.PARTNER   " +
                                 $"WHERE IS_NO = '{is_no}' ";

                            DataSet custDs = Dbconn.conn.ExecutDataset(SQL);
                            if (Dbconn.conn.getRowCnt(custDs) > 0)
                            {
                                sCustNm = Dbconn.conn.getData(custDs, "CUST_NAME", 0);
                                sPlanQty = Dbconn.conn.getData(custDs, "INVOICE_WEIGHT", 0);
                            }

                            custDs.Dispose();

                            //수동입력 연속입력 바코드일 경우 입력송장량 따로 출력
                            if (Dbconn.conn.getRowCnt(invoiceCntDs) > 1)
                            {
                                sPlanQty = Dbconn.conn.getData(invoiceCntDs, "R_GR_QNTY", i);
                            }

                            SQL = "SELECT MIN(SAP_PRO.DESCRIPTION) AS DESCRIPTION, COUNT(*) AS ITM_CNT " +
                                    "FROM WAP_GOCARD WG LEFT OUTER JOIN (SELECT RESOURCE_NO, DESCRIPTION FROM SAP_DI_PRODUCT GROUP BY  RESOURCE_NO, DESCRIPTION) SAP_PRO ON WG.MATNR = SAP_PRO.RESOURCE_NO " +
                                    $"WHERE IS_NO = '{is_no}' ";

                            DataSet descDs = Dbconn.conn.ExecutDataset(SQL);
                            if (Dbconn.conn.getRowCnt(descDs) > 0)
                            {
                                int pro_cnt = Convert.ToInt16(Dbconn.conn.getData(descDs, "ITM_CNT", 0));
                                sDescName = Dbconn.conn.getData(descDs, "DESCRIPTION", 0);
                                if (pro_cnt > 1)
                                {
                                    sDescName = sDescName + " 외 " + (pro_cnt - 1).ToString() + "개";
                                }
                            }
                            descDs.Dispose();
                        }

                        //차량번호
                        printSheet.Document.Worksheets[0].Cells["E5"].Value = sCarFullNum;

                        //계량번호
                        printSheet.Document.Worksheets[0].Cells["O5"].Value = is_no;

                        //입차시간
                        printSheet.Document.Worksheets[0].Cells["E6"].Value = sInWeightTime;

                        //입차중량
                        if (clsUtil.isNumber(sInWeight))
                        {
                            printSheet.Document.Worksheets[0].Cells["O6"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                        }

                        //출차시간
                        printSheet.Document.Worksheets[0].Cells["E7"].Value = sOutWeightTime;

                        //출차중량
                        if (clsUtil.isNumber(sOutWeight))
                        {
                            printSheet.Document.Worksheets[0].Cells["O7"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));
                        }

                        //거래처
                        printSheet.Document.Worksheets[0].Cells["E8"].Value = sCustNm;

                        string sPweight = "0";
                        int intPweight = 0;
                        //피무계합산
                        if (carInType == "001")
                        {
                            SQL = $"SELECT NVL(SUM(WEIGHT * PD_QTY),0) AS P_SUM FROM WAP_IN_ADD WHERE IS_NO = '{is_no}'";
                            DataSet pWeightDs = Dbconn.conn.ExecutDataset(SQL);

                            sPweight = Dbconn.conn.getData(pWeightDs, "P_SUM", 0);

                            if (Convert.ToDecimal(sPweight) > 0)
                            {
                                if (factory == "P201")
                                {

                                }
                                else
                                {
                                    sPweight = Convert.ToString(IntRound(Convert.ToDouble(sPweight), -1));
                                }

                                intPweight = Convert.ToInt32(sPweight);
                                //피중량
                                printSheet.Document.Worksheets[0].Cells["O8"].Value = sPweight;
                            }

                            //송장량
                            if (clsUtil.isNumber(sPlanQty))
                            {
                                printSheet.Document.Worksheets[0].Cells["O10"].Value = String.Format("{0:#,###}", Convert.ToInt32(sPlanQty));
                            }
                        }

                        //배송처
                        printSheet.Document.Worksheets[0].Cells["E9"].Value = "";

                        //실중량
                        if (clsUtil.isNumber(sWeigth))
                        {
                            printSheet.Document.Worksheets[0].Cells["O9"].Value = String.Format("{0:#,###}", Convert.ToDecimal(sWeigth) - intPweight);
                        }

                        if (carInType == "001")
                        {
                            //품목
                            printSheet.Document.Worksheets[0].Cells["E13"].Value = sDescName;
                        }
                        else if (carInType == "007")
                        {
                            //기타
                            printSheet.Document.Worksheets[0].Cells["E13"].Value = sEtcDetail;
                        }

                        string etcAddress1 = string.Empty;
                        string etcAddress2 = string.Empty;
                        string etcAddress3 = string.Empty;


                        if (Factory == "P101" || Factory == "P102")
                        {
                            if (Factory == "P101")
                            {
                                etcAddress1 = "상호: ㈜ 하림김제사료공장";
                                etcAddress2 = "주소: 전북 김제시 만경읍 만경공단 1길 25";
                                etcAddress3 = "전화: 주간) 063-545-2304~5, 야간응답기) 063-545-2306";
                            }

                            if (Factory == "P102")
                            {
                                etcAddress1 = "상호: ㈜ 하림정읍사료공장";
                                etcAddress2 = "주소: 전북 정읍시 신태인읍 신태인공단길 17-28";
                                etcAddress3 = "전화: 주간) 063-545-2304~5, 야간응답기) 063-545-2306";
                            }
                        }

                        printSheet.Document.Worksheets[0].Cells["C52"].Value = etcAddress1;
                        printSheet.Document.Worksheets[0].Cells["C54"].Value = etcAddress2;
                        printSheet.Document.Worksheets[0].Cells["C56"].Value = etcAddress3;

                        //년월일
                        printSheet.Document.Worksheets[0].Cells["M15"].Value = string.Empty;
                        printSheet.Document.Worksheets[0].Cells["M16"].Value = string.Empty;
                        printSheet.Document.Worksheets[0].Cells["M17"].Value = string.Empty;

                        printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;

                        Margins pageMargins = printSheet.Document.Worksheets[0].ActiveView.Margins;
                        pageMargins.Left = 0.2F;
                        pageMargins.Top = 0.15F;
                        pageMargins.Right = 0;
                        pageMargins.Bottom = 0;
                        //pageMargins.Header = 1;
                        //pageMargins.Footer = 0.4F;

                        printSheet.Options.Print.ShowMarginsWarning = false;
                        //printSheet.Print();
                        printSheet.ShowPrintPreview();

                    } //for cust

                    invoiceCntDs.Dispose();
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.StackTrace);
                clsLog.logSave(MethodBase.GetCurrentMethod().ReflectedType.FullName, MethodBase.GetCurrentMethod().Name, ex.Source);
            }

        }

    }
}


