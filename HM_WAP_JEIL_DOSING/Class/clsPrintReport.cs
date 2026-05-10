using Core.Class;
using Core.Extension;
using DevExpress.CodeParser.Diagnostics;
using DevExpress.Drawing.Internal.Fonts.Interop;
using DevExpress.Spreadsheet;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraSpreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static DevExpress.CodeParser.CodeStyle.Formatting.Rules;
using static DevExpress.Data.Filtering.Helpers.SubExprHelper.ThreadHoppingFiltering;

namespace HARIM_FA_DOSING
{
    public class clsPrintReport
    {
        private static string sD24 = string.Empty;
        private static string sD25 = string.Empty;
        private static string sD26 = string.Empty;
        private static string sD27 = string.Empty;
        private static string sD28 = string.Empty;
        private static string sD29 = string.Empty;
        private static string sD30 = string.Empty;
        private static string sD31 = string.Empty;
        private static string sD32 = string.Empty;
        private static string sD33 = string.Empty;
        private static string sM28 = string.Empty;
        private static string sP28 = string.Empty;
        private static string sM40 = string.Empty;
        private static string sC97 = string.Empty;
        private static string sF99 = string.Empty;
        private static string sC36 = string.Empty;
        private static List<string> lG49 = null;

        public static double IntCeiling(double Value, int Digit)
        {
            double Temp = Math.Pow(10.0, Digit);
            return Math.Ceiling(Value * Temp) / Temp;
        }

        public static void PrintMainIngredReport(string sWorkDate)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\주원료하차일지.xlsx", DocumentFormat.Xlsx);

                string SQL = $@"
                SELECT 
                     a.WORK_NUMBER
                   , a.WORK_SEQ
                   , a.RESOURCE_NO
                   , p.DESCRIPTION
                   , a.LOCATION
                   , FLOOR(a.INPUT_QTY) AS INPUT_QTY
                   , TO_CHAR(a.START_TIME, 'HH24:MI') AS START_TIME
                   , TO_CHAR(a.END_TIME, 'HH24:MI') AS END_TIME
                   , a.INCAR_NO
                   , a.DRIVER_NAME
                   , a.WORK_NAME
                   , a.REMARK
                   , a.I_TIME
                   , a.I_USER
                FROM MAINMATERIALS_REPORT a
                    LEFT JOIN SAP_DI_PRODUCT p ON a.RESOURCE_NO = p.RESOURCE_NO
                WHERE a.WORK_NUMBER = '{sWorkDate}'
                ORDER BY a.WORK_NUMBER, a.WORK_SEQ
                ";

                DataSet listDs = Dbconn.conn.ExecutDataset(SQL);

                printSheet.SetCellValue(0, "N4", Convert.ToDateTime(sWorkDate.Substring(0, 4) + "-" + sWorkDate.Substring(4, 2) + "-" + sWorkDate.Substring(6, 2)).ToString("yyyy년 MM월 dd일 ddd요일"));

                for (int i = 0; i < Dbconn.conn.getRowCnt(listDs); i++)
                {
                    printSheet.SetCellValue(0, "B" + (i + 9).ToString(), Dbconn.conn.getData(listDs, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, "F" + (i + 9).ToString(), Dbconn.conn.getData(listDs, "LOCATION", i));
                    printSheet.SetCellValue(0, "H" + (i + 9).ToString(), Dbconn.conn.getData(listDs, "START_TIME", i));
                    printSheet.SetCellValue(0, "J" + (i + 9).ToString(), Dbconn.conn.getData(listDs, "END_TIME", i));
                    printSheet.SetCellValue(0, "L" + (i + 9).ToString(), Dbconn.conn.getData(listDs, "INCAR_NO", i));
                    printSheet.SetCellValue(0, "N" + (i + 9).ToString(), Dbconn.conn.getData(listDs, "DRIVER_NAME", i));
                    printSheet.SetCellValue(0, "Q" + (i + 9).ToString(), Dbconn.conn.getData(listDs, "REMARK", i));

                    if (i > 27)
                    {
                        break;
                    }
                }

                SQL = "SELECT CHK_REMARK FROM MAINMATERIALS_CHK_LIST WHERE WORK_NUMBER = '{0}' ";
                SQL = string.Format(SQL,
                        sWorkDate
                        );

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);
                if (Dbconn.conn.getRowCnt(chkListDs) > 0)
                {
                    printSheet.SetCellValue(0, "A38", Dbconn.conn.getData(chkListDs, "CHK_REMARK", 0));
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowRibbonPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintMainIngredReport", ex);
            }
        }

        public static void PrintSubIngredReport(string sPlantCode, string sWorkDate)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\부원료투입일지.xlsx", DocumentFormat.Xlsx);

                printSheet.SetCellValue(0, "J4", "일  자 :  " + Convert.ToDateTime(sWorkDate.Substring(0, 4) + "-" + sWorkDate.Substring(4, 2) + "-" + sWorkDate.Substring(6, 2)).ToString("yyyy년 MM월 dd일 ddd요일"));

                string SQL = $@"
                SELECT a.WORK_NUMBER
                     , a.WORK_SEQ
                     , a.RESOURCE_NO
                     , p.DESCRIPTION
                     , a.SPCS
                     , a.LOCATION
                     , FLOOR(a.INPUT_QTY) AS INPUT_QTY
                     , TO_CHAR(a.START_TIME, 'HH24:MI') AS START_TIME
                     , TO_CHAR(a.END_TIME, 'HH24:MI') AS END_TIME
                     , a.CHK_YN1
                     , a.CHK_YN2
                     , a.CHK_YN3
                     , a.CHK_YN4
                     , a.CHK_END_DATE
                FROM SUBMATERIALS_REPORT report
                LEFT OUTER JOIN SAP_DI_PRODUCT p ON p.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = p.RESOURCE_NO
                WHERE a.PLANT_CODE = '{sPlantCode}' AND a.WORK_NUMBER = '{sWorkDate}'
                ORDER BY a.WORK_NUMBER
                       , a.WORK_SEQ
                ";

                DataSet listDs = Dbconn.conn.ExecutDataset(SQL);

                int rowCount = Dbconn.conn.getRowCnt(listDs);
                int rowNum = 0;
                int writePos = 8;

                if (rowCount > 18)
                {
                    printSheet.Document.Worksheets.RemoveAt(0);
                }
                else
                {
                    printSheet.Document.Worksheets.RemoveAt(1);
                }

                for (int i = 0; i < rowCount; i++)
                {

                    if (i == 19)
                    {
                        //next page setting
                        printSheet.SetCellValue(0, "J40", "일  자 :  " + Convert.ToDateTime(sWorkDate.Substring(0, 4) + "-" + sWorkDate.Substring(4, 2) + "-" + sWorkDate.Substring(6, 2)).ToString("yyyy년 MM월 dd일 ddd요일"));

                        rowNum = 0;
                        writePos = 44;
                    }


                    printSheet.SetCellValue(0, "B" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, "D" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "SPCS", i));
                    printSheet.SetCellValue(0, "F" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "LOCATION", i));

                    printSheet.SetCellValue(0, "H" + (rowNum + writePos).ToString(), String.Format("{0:#,###}", Dbconn.conn.getData(listDs, "INPUT_QTY", i)));
                    printSheet.SetCellValue(0, "J" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "START_TIME", i));
                    printSheet.SetCellValue(0, "K" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "END_TIME", i));

                    printSheet.SetCellValue(0, "L" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "CHK_YN1", i) == "Y" ? "O" : "X");
                    printSheet.SetCellValue(0, "M" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "CHK_YN2", i) == "Y" ? "O" : "X");
                    printSheet.SetCellValue(0, "N" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "CHK_YN3", i) == "Y" ? "O" : "X");
                    printSheet.SetCellValue(0, "O" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "CHK_YN4", i) == "Y" ? "O" : "X");

                    printSheet.SetCellValue(0, "P" + (rowNum + writePos).ToString(), Dbconn.conn.getData(listDs, "CHK_END_DATE", i));

                    rowNum = rowNum + 1;
                }

                SQL = $"SELECT CHK_MEMO1, CHK_MEMO2, CHK_REMARK FROM SUBMATERIALS_CHK_LIST WHERE PLANT_CODE = '{sPlantCode}' AND WORK_NUMBER = '{sWorkDate}' ";

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) > 0)
                {
                    printSheet.SetCellValue(0, "F30", Dbconn.conn.getData(chkListDs, "CHK_MEMO1", 0));
                    printSheet.SetCellValue(0, "N30", Dbconn.conn.getData(chkListDs, "CHK_MEMO2", 0));
                    printSheet.SetCellValue(0, "A32", Dbconn.conn.getData(chkListDs, "CHK_REMARK", 0));

                    if (rowCount > 18)
                    {
                        printSheet.SetCellValue(0, "F66", Dbconn.conn.getData(chkListDs, "CHK_MEMO1", 0));
                        printSheet.SetCellValue(0, "N66", Dbconn.conn.getData(chkListDs, "CHK_MEMO2", 0));
                        printSheet.SetCellValue(0, "A68", Dbconn.conn.getData(chkListDs, "CHK_REMARK", 0));
                    }
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintSubIngredReport", ex);
            }
        }

        public static void PrintMicroReport(string plantCode, string sWorkDate)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\마이크로투입일지.xlsx", DocumentFormat.Xlsx);

                string SQL = $@"
                SELECT 
                     a.WORK_SEQ
                   , a.RESOURCE_NO
                   , p.DESCRIPTION
                   , a.LOCATION
                   , FLOOR(a.INPUT_QTY) AS INPUT_QTY
                   , a.EMPLOYEE_NO
                   , TO_CHAR(a.INPUT_TIME, 'HH24:MI') AS INPUT_TIME
                FROM MICRO_INPUT_REPORT a
                LEFT JOIN SAP_DI_PRODUCT p ON p.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = p.RESOURCE_NO
                WHERE a.PLANT_CODE = '{plantCode}'
                    AND a.WORK_NUMBER = '{sWorkDate}'
                ORDER BY a.WORK_NUMBER, a.WORK_SEQ
                ";

                DataSet listDs = Dbconn.conn.ExecutDataset(SQL);

                printSheet.SetCellValue(0, "M4", Convert.ToDateTime(sWorkDate.Substring(0, 4) + "-" + sWorkDate.Substring(4, 2) + "-" + sWorkDate.Substring(6, 2)).ToString("yyyy년 MM월 dd일 ddd요일"));


                int rowNum = 6;

                string inputPos = "B";
                string locPos = "D";
                string desPos = "E";
                string qtyPos = "I";

                for (int i = 0; i < Dbconn.conn.getRowCnt(listDs); i++)
                {
                    if (i == 32)
                    {
                        rowNum = 6;
                        inputPos = "K";
                        locPos = "M";
                        desPos = "N";
                        qtyPos = "R";
                    }

                    printSheet.SetCellValue(0, inputPos + (rowNum).ToString(), Dbconn.conn.getData(listDs, "INPUT_TIME", i));
                    printSheet.SetCellValue(0, locPos + (rowNum).ToString(), Dbconn.conn.getData(listDs, "LOCATION", i));
                    printSheet.SetCellValue(0, desPos + (rowNum).ToString(), Dbconn.conn.getData(listDs, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, qtyPos + (rowNum).ToString(), String.Format("{0:#,###}", Dbconn.conn.getData(listDs, "INPUT_QTY", i)));

                    rowNum = rowNum + 1;
                }

                SQL = $@"
                SELECT 
                     WORK_NUMBER
                   , CHK_LIST1
                   , CHK_REMARK1
                   , CHK_LIST2
                   , CHK_REMARK2
                   , CHK_LIST3
                   , CHK_REMARK3
                   , CHK_LIST4
                   , CHK_REMARK4
                   , CHK_LIST5
                   , CHK_REMARK5
                   , I_TIME
                   , I_USER
                FROM MICRO_INPUT_CHK_LIST
                WHERE PLANT_CODE = '{plantCode}'
                    AND WORK_NUMBER = '{sWorkDate}'
                ";

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) > 0)
                {
                    string chk_list1 = Dbconn.conn.getData(chkListDs, "CHK_LIST1", 0);
                    string chk_remark1 = Dbconn.conn.getData(chkListDs, "CHK_REMARK1", 0);
                    string chk_list2 = Dbconn.conn.getData(chkListDs, "CHK_LIST2", 0);
                    string chk_remark2 = Dbconn.conn.getData(chkListDs, "CHK_REMARK2", 0);
                    string chk_list3 = Dbconn.conn.getData(chkListDs, "CHK_LIST3", 0);
                    string chk_remark3 = Dbconn.conn.getData(chkListDs, "CHK_REMARK3", 0);
                    string chk_list4 = Dbconn.conn.getData(chkListDs, "CHK_LIST4", 0);
                    string chk_remark4 = Dbconn.conn.getData(chkListDs, "CHK_REMARK4", 0);
                    string chk_list5 = Dbconn.conn.getData(chkListDs, "CHK_LIST5", 0);
                    string chk_remark5 = Dbconn.conn.getData(chkListDs, "CHK_REMARK5", 0);

                    printSheet.SetCellValue(0, "J39", chk_list1.Contains("Y") ? "Y" : "N");
                    printSheet.SetCellValue(0, "J40", chk_list2.Contains("Y") ? "Y" : "N");
                    printSheet.SetCellValue(0, "J41", chk_list3.Contains("Y") ? "Y" : "N");
                    printSheet.SetCellValue(0, "J42", chk_list4.Contains("Y") ? "Y" : "N");
                    printSheet.SetCellValue(0, "J43", chk_list5.Contains("Y") ? "Y" : "N");

                    printSheet.SetCellValue(0, "L39", chk_remark1);
                    printSheet.SetCellValue(0, "L40", chk_remark2);
                    printSheet.SetCellValue(0, "L41", chk_remark3);
                    printSheet.SetCellValue(0, "L42", chk_remark4);
                    printSheet.SetCellValue(0, "L43", chk_remark5);
                }
                else
                {
                    printSheet.SetCellValue(0, "J39", "N");
                    printSheet.SetCellValue(0, "J40", "N");
                    printSheet.SetCellValue(0, "J41", "N");
                    printSheet.SetCellValue(0, "J42", "N");
                    printSheet.SetCellValue(0, "J43", "N");
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintMicroReport", ex);
            }
        }

        public static void PrintGrindingReport(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\분쇄일지.xlsx", DocumentFormat.Xlsx);

                printSheet.SetCellValue(0, "P6", Convert.ToDateTime(sWorkDate.Substring(0, 4) + "-" + sWorkDate.Substring(4, 2) + "-" + sWorkDate.Substring(6, 2)).ToString("yyyy.MM.dd(ddd)"));


                string SQL = $@"
                SELECT a.RESOURCE_NO
                     , p.DESCRIPTION
                     , TO_CHAR(a.RUN_ST, 'HH24:MI') AS RUN_ST
                     , TO_CHAR(a.RUN_ET, 'HH24:MI') AS RUN_ET
                     , a.SC_FORE
                     , a.SC_BG
                     , a.SCREEN
                     , a.FEED_RATE
                     , a.LOAD
                     , a.GR_WV
                     , a.GR_QTY
                     , a.LOCATION_ST1
                     , a.LOCATION_ED1
                     , a.DUST_WV1
                     , a.DUST_WV2
                     , a.TH_GR_QTY
                     , a.PRO_QTY
                     , a.I_TIME
                     , ROUND((a.RUN_ET - a.RUN_ST) * 24 * 60) AS RUN_TOTAL
                FROM SMASH_REPORT a
                       LEFT OUTER JOIN SAP_DI_PRODUCT p ON p.PLANT_CODE = a.PLANT_CODE AND a.RESOURCE_NO = p.RESOURCE_NO
                 WHERE a.PLANT_CODE = '{sPlantCode}'
                        AND a.PROCESS_KEY = '{sPlantCode}'
                        AND a.L_CODE = '{sLCode}'
                        AND a.WORK_NUMBER = '{sWorkDate}'
                ";

                DataSet reportListDs = Dbconn.conn.ExecutDataset(SQL);

                for (int i = 0; i < Dbconn.conn.getRowCnt(reportListDs); i++)
                {
                    printSheet.SetCellValue(0, "C" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, "F" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_ST", i));
                    printSheet.SetCellValue(0, "G" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_ET", i));
                    printSheet.SetCellValue(0, "H" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_TOTAL", i));
                    printSheet.SetCellValue(0, "I" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "SC_FORE", i));
                    printSheet.SetCellValue(0, "J" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "SC_BG", i));
                    printSheet.SetCellValue(0, "K" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "LOCATION_ST1", i));
                    printSheet.SetCellValue(0, "L" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "LOCATION_ED1", i));
                    printSheet.SetCellValue(0, "M" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "SCREEN", i));
                    printSheet.SetCellValue(0, "N" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "FEED_RATE", i));
                    printSheet.SetCellValue(0, "O" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "LOAD", i));
                    printSheet.SetCellValue(0, "P" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "GR_WV", i));
                    printSheet.SetCellValue(0, "Q" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "DUST_WV1", i));
                    printSheet.SetCellValue(0, "R" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "DUST_WV2", i));
                    printSheet.SetCellValue(0, "S" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "GR_QTY", i));
                    printSheet.SetCellValue(0, "T" + (10 + i).ToString(), Dbconn.conn.getData(reportListDs, "PRO_QTY", i));
                }

                SQL = $@"
                SELECT WORK_NUMBER
                     , CHK_WORK1      , CHK_RESULT1      , CHK_CHDAY1      , CHK_REMARK1
                     , CHK_WORK2      , CHK_RESULT2      , CHK_CHDAY2      , CHK_REMARK2
                     , CHK_WORK3      , CHK_RESULT3      , CHK_CHDAY3      , CHK_REMARK3
                     , CHK_WORK4      , CHK_RESULT4      , CHK_CHDAY4      , CHK_REMARK4
                     , CHK_WORK5      , CHK_RESULT5      , CHK_CHDAY5      , CHK_REMARK5
                     , CHK_WORK6      , CHK_RESULT6      , CHK_CHDAY6      , CHK_REMARK6
                     , CHK_WORK7      , CHK_RESULT7      , CHK_CHDAY7      , CHK_REMARK7
                     , CHK_WORK8      , CHK_RESULT8      , CHK_CHDAY8      , CHK_REMARK8
                     , CHK_WORK9      , CHK_RESULT9      , CHK_CHDAY9      , CHK_REMARK9
                     , CHK_WORK10     , CHK_RESULT10     , CHK_CHDAY10     , CHK_REMARK10
                     , CHK_WORK11     , CHK_RESULT11     , CHK_CHDAY11     , CHK_REMARK11
                     , I_TIME
                     , I_USER
                  FROM SMASH_CHK_LIST
                 WHERE PLANT_CODE = '{sPlantCode}' AND WORK_NUMBER = '{sWorkDate}'
                ";

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) == 1)
                {
                    printSheet.SetCellValue(0, "L35", Dbconn.conn.getData(chkListDs, "CHK_WORK1", 0));
                    printSheet.SetCellValue(0, "N35", Dbconn.conn.getData(chkListDs, "CHK_RESULT1", 0));
                    printSheet.SetCellValue(0, "P35", Dbconn.conn.getData(chkListDs, "CHK_CHDAY1", 0));
                    printSheet.SetCellValue(0, "R35", Dbconn.conn.getData(chkListDs, "CHK_REMARK1", 0));

                    printSheet.SetCellValue(0, "L36", Dbconn.conn.getData(chkListDs, "CHK_WORK2", 0));
                    printSheet.SetCellValue(0, "N36", Dbconn.conn.getData(chkListDs, "CHK_RESULT2", 0));
                    printSheet.SetCellValue(0, "P36", Dbconn.conn.getData(chkListDs, "CHK_CHDAY2", 0));
                    printSheet.SetCellValue(0, "R36", Dbconn.conn.getData(chkListDs, "CHK_REMARK2", 0));

                    printSheet.SetCellValue(0, "L37", Dbconn.conn.getData(chkListDs, "CHK_WORK3", 0));
                    printSheet.SetCellValue(0, "N37", Dbconn.conn.getData(chkListDs, "CHK_RESULT3", 0));
                    printSheet.SetCellValue(0, "P37", Dbconn.conn.getData(chkListDs, "CHK_CHDAY3", 0));
                    printSheet.SetCellValue(0, "R37", Dbconn.conn.getData(chkListDs, "CHK_REMARK3", 0));

                    printSheet.SetCellValue(0, "L38", Dbconn.conn.getData(chkListDs, "CHK_WORK4", 0));
                    printSheet.SetCellValue(0, "N38", Dbconn.conn.getData(chkListDs, "CHK_RESULT4", 0));
                    printSheet.SetCellValue(0, "P38", Dbconn.conn.getData(chkListDs, "CHK_CHDAY4", 0));
                    printSheet.SetCellValue(0, "R38", Dbconn.conn.getData(chkListDs, "CHK_REMARK4", 0));

                    printSheet.SetCellValue(0, "L39", Dbconn.conn.getData(chkListDs, "CHK_WORK5", 0));
                    printSheet.SetCellValue(0, "N39", Dbconn.conn.getData(chkListDs, "CHK_RESULT5", 0));
                    printSheet.SetCellValue(0, "P39", Dbconn.conn.getData(chkListDs, "CHK_CHDAY5", 0));
                    printSheet.SetCellValue(0, "R39", Dbconn.conn.getData(chkListDs, "CHK_REMARK5", 0));

                    printSheet.SetCellValue(0, "L40", Dbconn.conn.getData(chkListDs, "CHK_WORK6", 0));
                    printSheet.SetCellValue(0, "N40", Dbconn.conn.getData(chkListDs, "CHK_RESULT6", 0));
                    printSheet.SetCellValue(0, "P40", Dbconn.conn.getData(chkListDs, "CHK_CHDAY6", 0));
                    printSheet.SetCellValue(0, "R40", Dbconn.conn.getData(chkListDs, "CHK_REMARK6", 0));

                    printSheet.SetCellValue(0, "L41", Dbconn.conn.getData(chkListDs, "CHK_WORK7", 0));
                    printSheet.SetCellValue(0, "N41", Dbconn.conn.getData(chkListDs, "CHK_RESULT7", 0));
                    printSheet.SetCellValue(0, "P41", Dbconn.conn.getData(chkListDs, "CHK_CHDAY7", 0));
                    printSheet.SetCellValue(0, "R41", Dbconn.conn.getData(chkListDs, "CHK_REMARK7", 0));

                    printSheet.SetCellValue(0, "L42", Dbconn.conn.getData(chkListDs, "CHK_WORK8", 0));
                    printSheet.SetCellValue(0, "N42", Dbconn.conn.getData(chkListDs, "CHK_RESULT8", 0));
                    printSheet.SetCellValue(0, "P42", Dbconn.conn.getData(chkListDs, "CHK_CHDAY8", 0));
                    printSheet.SetCellValue(0, "R42", Dbconn.conn.getData(chkListDs, "CHK_REMARK8", 0));

                    printSheet.SetCellValue(0, "L43", Dbconn.conn.getData(chkListDs, "CHK_WORK9", 0));
                    printSheet.SetCellValue(0, "N43", Dbconn.conn.getData(chkListDs, "CHK_RESULT9", 0));
                    printSheet.SetCellValue(0, "P43", Dbconn.conn.getData(chkListDs, "CHK_CHDAY9", 0));
                    printSheet.SetCellValue(0, "R43", Dbconn.conn.getData(chkListDs, "CHK_REMARK9", 0));

                    printSheet.SetCellValue(0, "L44", Dbconn.conn.getData(chkListDs, "CHK_WORK10", 0));
                    printSheet.SetCellValue(0, "N44", Dbconn.conn.getData(chkListDs, "CHK_RESULT10", 0));
                    printSheet.SetCellValue(0, "P44", Dbconn.conn.getData(chkListDs, "CHK_CHDAY10", 0));
                    printSheet.SetCellValue(0, "R44", Dbconn.conn.getData(chkListDs, "CHK_REMARK10", 0));

                    printSheet.SetCellValue(0, "L45", Dbconn.conn.getData(chkListDs, "CHK_WORK11", 0));
                    printSheet.SetCellValue(0, "N45", Dbconn.conn.getData(chkListDs, "CHK_RESULT11", 0));
                    printSheet.SetCellValue(0, "P45", Dbconn.conn.getData(chkListDs, "CHK_CHDAY11", 0));
                    printSheet.SetCellValue(0, "R45", Dbconn.conn.getData(chkListDs, "CHK_REMARK11", 0));

                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintGrindingReport", ex);
            }
        }

        /// <summary>
        /// 배합일지
        /// </summary>
        /// <param name="sPlantCode"></param>
        /// <param name="sProcessKey"></param>
        /// <param name="sLCode"></param>
        /// <param name="sWorkDate"></param>
        public static void PrintDosingReport(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\배합작업일지.xlsx", DocumentFormat.Xlsx);
                SetSheet1(sPlantCode, sProcessKey, sLCode, sWorkDate, printSheet);
                Setsheet2(sPlantCode, sProcessKey, sLCode, sWorkDate, printSheet);

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintDosingReport", ex);
            }
        }

        private static void Setsheet2(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate, SpreadsheetControl printSheet)
        {
            string SQL = $@"
            SELECT 
                    a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.RESOURCE_NO, b.DESCRIPTION, a.LOCATION_ED
                , SUM(a.BATCH) AS BATCH, SUM(a.BATCH_Q) AS BATCH_Q, SUM(a.PRO_Q) AS PRO_Q
            FROM WORK_ORDER a
                JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
            WHERE a.PLANT_CODE = '{sPlantCode}'
                AND a.PROCESS_KEY = '{sProcessKey}'
                AND a.L_CODE = '{sLCode}'
                AND NVL(a.DEL_FLAG,'N') != 'Y'
                AND a.WORKDATE = '{sWorkDate}'
                AND a.C_CONDITION = '031004'
            GROUP BY a.PLANT_CODE, a.PROCESS_KEY, a.L_CODE, a.WORKDATE, a.RESOURCE_NO, b.DESCRIPTION, a.LOCATION_ED
            ORDER BY a.RESOURCE_NO
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            printSheet.SetCellValue(1, "Q7", printSheet.GetCellValue(0, "Q7"));
            printSheet.SetCellValue(1, "G7", printSheet.GetCellValue(0, "G7"));
            printSheet.SetCellValue(1, "L7", printSheet.GetCellValue(0, "L7"));
            printSheet.SetCellValue(1, "E8", printSheet.GetCellValue(0, "E8"));
            printSheet.SetCellValue(1, "N8", printSheet.GetCellValue(0, "N8"));
            printSheet.SetCellValue(1, "E9", printSheet.GetCellValue(0, "E9"));
            printSheet.SetCellValue(1, "N9", printSheet.GetCellValue(0, "N9"));
            printSheet.SetCellValue(1, "E10", printSheet.GetCellValue(0, "E10"));
            printSheet.SetCellValue(1, "N10", printSheet.GetCellValue(0, "N10"));

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];

                printSheet.SetCellValue(1, "B" + (13 + i).ToString(), dr["RESOURCE_NO"]?.ToString());
                printSheet.SetCellValue(1, "D" + (13 + i).ToString(), dr["DESCRIPTION"]?.ToString());
                printSheet.SetCellValue(1, "J" + (13 + i).ToString(), dr["LOCATION_ED"]?.ToString());
                printSheet.SetCellValue(1, "L" + (13 + i).ToString(), dr["BATCH"]?.ToString());
                printSheet.SetCellValue(1, "O" + (13 + i).ToString(), dr["BATCH_Q"]?.ToString());
                printSheet.SetCellValue(1, "Q" + (13 + i).ToString(), dr["PRO_Q"]?.ToString());
            }
        }

        private static void SetSheet1(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate, SpreadsheetControl printSheet)
        {
            string SQL =
                "SELECT WORK_START_DATE, DAY_WORKMAN, NIGHT_WORKMAN, DOS_START_TIME, DOS_END_TIME, DOS_PRO_Q, DOS_INCOM_Q,  " +
                "DOS_PRO_BATCH, DOS_LOT_QTY, CHK_WORK_BEFOR_CHK1, CHK_WORK_BEFOR_CHK1_REMARK, CHK_WORK_BEFOR_CHK2, CHK_WORK_BEFOR_CHK2_REMARK, " +
                "CHK_WORK_BEFOR_CHK3, CHK_WORK_BEFOR_CHK3_REMARK, CHK_WORK_BEFOR_CHK4, CHK_WORK_BEFOR_CHK4_REMARK,  " +
                "CHK_WORK_BEFOR_CHK5, CHK_WORK_BEFOR_CHK5_REMARK, CHK_WORK_RUN_CHK1, CHK_WORK_RUN_CHK1_REMARK,  " +
                "CHK_WORK_RUN_CHK2, CHK_WORK_RUN_CHK2_REMARK, CHK_WORK_RUN_CHK3, CHK_WORK_RUN_CHK3_REMARK, CHK_WORK_END_CHK1, CHK_WORK_END_CHK1_REMARK,  " +
                "CHK_WORK_END_CHK2, CHK_WORK_END_CHK2_REMARK, CHK_WORK_END_CHK3, CHK_WORK_END_CHK3_REMARK, CHK_USE_TANK_1, CHK_USE_TANK_2,  " +
                "CHK_TANK1_TEMP1, CHK_TANK1_TEMP2, CHK_TANK1_TEMP3, CHK_TANK2_TEMP1, CHK_TANK2_TEMP2, CHK_TANK2_TEMP3, CHK_TANK3_TEMP1,  " +
                "CHK_TANK3_TEMP2, CHK_TANK3_TEMP3, DOS_LOAD_EQ1_NO_VALUE, DOS_LOAD_EQ1_FULL_VALUE, DOS_LOAD_EQ1_OVER_VALUE, DOS_LOAD_EQ2_NO_VALUE,  " +
                "DOS_LOAD_EQ2_FULL_VALUE, DOS_LOAD_EQ2_OVER_VALUE, DOS_LOAD_EQ3_NO_VALUE, DOS_LOAD_EQ3_FULL_VALUE, DOS_LOAD_EQ3_OVER_VALUE, " +
                "DOS_EXTEND_REMARK, DOS_MANUAL_INPUT_REMARK, DOS_WKMONTH_REMARK, DOS_REMARK, I_TIME, I_USER  " +
                "FROM DOS_REPORT " +
                $"WHERE WORK_START_DATE = '{sWorkDate}' ";

            printSheet.SetCellValue(0, "Q7", Convert.ToDateTime(sWorkDate.Substring(0, 4) + "-" + sWorkDate.Substring(4, 2) + "-" + sWorkDate.Substring(6, 2)).ToString("yy.MM.dd(ddd)"));

            DataSet listDs = Dbconn.conn.ExecutDataset(SQL);

            if (Dbconn.conn.getRowCnt(listDs) == 1)
            {
                printSheet.SetCellValue(0, "G7", Dbconn.conn.getData(listDs, "DAY_WORKMAN", 0));
                printSheet.SetCellValue(0, "L7", Dbconn.conn.getData(listDs, "NIGHT_WORKMAN", 0));
                printSheet.SetCellValue(0, "E8", Convert.ToDateTime(Dbconn.conn.getData(listDs, "DOS_START_TIME", 0)).ToString("HH:mm"));
                printSheet.SetCellValue(0, "N8", Convert.ToDateTime(Dbconn.conn.getData(listDs, "DOS_END_TIME", 0)).ToString("HH:mm"));
                printSheet.SetCellValue(0, "E9", Dbconn.conn.getData(listDs, "DOS_PRO_Q", 0));
                printSheet.SetCellValue(0, "N9", Dbconn.conn.getData(listDs, "DOS_INCOM_Q", 0));
                printSheet.SetCellValue(0, "E10", Dbconn.conn.getData(listDs, "DOS_PRO_BATCH", 0));
                printSheet.SetCellValue(0, "N10", Dbconn.conn.getData(listDs, "DOS_LOT_QTY", 0));

                string chk_befor_chk1 = Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK1", 0);
                if (chk_befor_chk1.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P14", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P14", "X");
                }
                printSheet.SetCellValue(0, "R14", Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK1_REMARK", 0));

                string chk_befor_chk2 = Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK2", 0);
                if (chk_befor_chk2.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P15", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P15", "X");
                }
                printSheet.SetCellValue(0, "R15", Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK2_REMARK", 0));


                string chk_befor_chk3 = Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK3", 0);
                if (chk_befor_chk3.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P16", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P16", "X");
                }
                printSheet.SetCellValue(0, "R16", Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK3_REMARK", 0));


                string chk_befor_chk4 = Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK4", 0);
                if (chk_befor_chk4.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P17", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P17", "X");
                }
                printSheet.SetCellValue(0, "R17", Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK4_REMARK", 0));


                string chk_befor_chk5 = Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK5", 0);
                if (chk_befor_chk5.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P18", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P18", "X");
                }
                printSheet.SetCellValue(0, "R18", Dbconn.conn.getData(listDs, "CHK_WORK_BEFOR_CHK5_REMARK", 0));



                string chk_run_chk1 = Dbconn.conn.getData(listDs, "CHK_WORK_RUN_CHK1", 0);
                if (chk_run_chk1.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P19", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P19", "X");
                }
                printSheet.SetCellValue(0, "R19", Dbconn.conn.getData(listDs, "CHK_WORK_RUN_CHK1_REMARK", 0));


                string chk_run_chk2 = Dbconn.conn.getData(listDs, "CHK_WORK_RUN_CHK2", 0);
                if (chk_run_chk2.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P20", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P20", "X");
                }
                printSheet.SetCellValue(0, "R20", Dbconn.conn.getData(listDs, "CHK_WORK_RUN_CHK2_REMARK", 0));


                string chk_run_chk3 = Dbconn.conn.getData(listDs, "CHK_WORK_RUN_CHK3", 0);
                if (chk_run_chk3.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P21", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P21", "X");
                }
                printSheet.SetCellValue(0, "R21", Dbconn.conn.getData(listDs, "CHK_WORK_RUN_CHK3_REMARK", 0));


                string chk_end_chk1 = Dbconn.conn.getData(listDs, "CHK_WORK_END_CHK1", 0);
                if (chk_end_chk1.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P22", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P22", "X");
                }
                printSheet.SetCellValue(0, "R22", Dbconn.conn.getData(listDs, "CHK_WORK_END_CHK1_REMARK", 0));


                string chk_end_chk2 = Dbconn.conn.getData(listDs, "CHK_WORK_END_CHK2", 0);
                if (chk_end_chk2.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P23", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P23", "X");
                }
                printSheet.SetCellValue(0, "R23", Dbconn.conn.getData(listDs, "CHK_WORK_END_CHK2_REMARK", 0));


                string chk_end_chk3 = Dbconn.conn.getData(listDs, "CHK_WORK_END_CHK3", 0);
                if (chk_end_chk3.Equals("Y"))
                {
                    printSheet.SetCellValue(0, "P24", "O");
                }
                else
                {
                    printSheet.SetCellValue(0, "P24", "X");
                }
                printSheet.SetCellValue(0, "R24", Dbconn.conn.getData(listDs, "CHK_WORK_END_CHK3_REMARK", 0));


                printSheet.SetCellValue(0, "J25", Dbconn.conn.getData(listDs, "CHK_USE_TANK_1", 0));
                printSheet.SetCellValue(0, "N25", Dbconn.conn.getData(listDs, "CHK_USE_TANK_2", 0));

                printSheet.SetCellValue(0, "J26", Dbconn.conn.getData(listDs, "CHK_TANK1_TEMP1", 0));
                printSheet.SetCellValue(0, "N26", Dbconn.conn.getData(listDs, "CHK_TANK1_TEMP2", 0));
                printSheet.SetCellValue(0, "R26", Dbconn.conn.getData(listDs, "CHK_TANK1_TEMP3", 0));

                printSheet.SetCellValue(0, "J27", Dbconn.conn.getData(listDs, "CHK_TANK2_TEMP1", 0));
                printSheet.SetCellValue(0, "N27", Dbconn.conn.getData(listDs, "CHK_TANK2_TEMP2", 0));
                printSheet.SetCellValue(0, "R27", Dbconn.conn.getData(listDs, "CHK_TANK2_TEMP3", 0));

                printSheet.SetCellValue(0, "J28", Dbconn.conn.getData(listDs, "CHK_TANK3_TEMP1", 0));
                printSheet.SetCellValue(0, "N28", Dbconn.conn.getData(listDs, "CHK_TANK3_TEMP2", 0));
                printSheet.SetCellValue(0, "R28", Dbconn.conn.getData(listDs, "CHK_TANK3_TEMP3", 0));

                printSheet.SetCellValue(0, "N46", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ1_NO_VALUE", 0));
                printSheet.SetCellValue(0, "P46", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ1_FULL_VALUE", 0));
                printSheet.SetCellValue(0, "R46", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ1_OVER_VALUE", 0));
                printSheet.SetCellValue(0, "N47", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ2_NO_VALUE", 0));
                printSheet.SetCellValue(0, "P47", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ2_FULL_VALUE", 0));
                printSheet.SetCellValue(0, "R47", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ2_OVER_VALUE", 0));
                printSheet.SetCellValue(0, "N48", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ3_NO_VALUE", 0));
                printSheet.SetCellValue(0, "P48", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ3_FULL_VALUE", 0));
                printSheet.SetCellValue(0, "R48", Dbconn.conn.getData(listDs, "DOS_LOAD_EQ3_OVER_VALUE", 0));


                string extend_remark = Dbconn.conn.getData(listDs, "DOS_EXTEND_REMARK", 0);
                string[] arr_extend = extend_remark.Split('\r');
                for (int i = 0; i < arr_extend.Length; i++)
                {
                    printSheet.SetCellValue(0, "B" + (30 + i).ToString(), arr_extend[i]);
                }


                string manual_remark = Dbconn.conn.getData(listDs, "DOS_MANUAL_INPUT_REMARK", 0);
                string[] manual_extend = manual_remark.Split('\r');
                for (int i = 0; i < manual_extend.Length; i++)
                {
                    printSheet.SetCellValue(0, "K" + (30 + i).ToString(), manual_extend[i]);
                }


                string wkmonth_remark = Dbconn.conn.getData(listDs, "DOS_WKMONTH_REMARK", 0);
                string[] wkmonth_extend = wkmonth_remark.Split('\r');

                for (int i = 0; i < wkmonth_extend.Length; i++)
                {
                    if (i > 3)
                    {
                        break;
                    }

                    printSheet.SetCellValue(0, "B" + (45 + i).ToString(), wkmonth_extend[i]);
                }

                printSheet.SetCellValue(0, "B50", Dbconn.conn.getData(listDs, "DOS_REMARK", 0));
            }
        }

        public static void PrintPackReport(string plantCode, string processKey, string lCode, string sWorkDate, string sbno)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\포장작업일지.xlsx", DocumentFormat.Xlsx);

                printSheet.SetCellValue(0, "R4", sWorkDate.Substring(0, 4) + "년 " + sWorkDate.Substring(4, 2) + "월 " + sWorkDate.Substring(6, 2) + "일");

                printSheet.SetCellValue(0, "D1", "포장 작업 일지 ( " + sbno + " 팀)");

                string SQL = $@"
                SELECT pack.WORK_NUMBER
                     , pack.WORK_SEQ
                     , pack.SBNO
                     , pack.RESOURCE_NO
                     , TRIM(p.DESCRIPTION) AS DESCRIPTION
                     , TO_CHAR(pack.RUN_ST, 'HH24:MI') AS RUN_ST
                     , TO_CHAR(pack.RUN_ET, 'HH24:MI') AS RUN_ET
                     , (ROUND((pack.RUN_ET - pack.RUN_ST) * 24 * 60)) AS RUN_TOTAL
                     , pack.LOCATION_ST
                     , (pack.PRO_Q / 1000) AS PRO_Q
                     , pack.OR_QTY
                     , pack.PRO_QTY
                     , pack.F_Q
                     , pack.E_Q
                     , pack.PA_Q
                     , pack.DIFF
                     , pack.SAMPLE_TLY
                     , pack.DAN1
                     , pack.DAN2
                     , pack.DAN3
                     , pack.WORK_CHK
                FROM PACK_REPORT pack
                LEFT OUTER JOIN SAP_DI_PRODUCT p
                  ON pack.RESOURCE_NO = p.RESOURCE_NO
                WHERE pack.WORK_NUMBER = '{sWorkDate}'
                  AND pack.SBNO = '{sbno}'
                ";

                DataSet reportListDs = Dbconn.conn.ExecutDataset(SQL);

                for (int i = 0; i < Dbconn.conn.getRowCnt(reportListDs); i++)
                {
                    printSheet.SetCellValue(0, "B" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, "D" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_ST", i));
                    printSheet.SetCellValue(0, "F" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_ET", i));
                    printSheet.SetCellValue(0, "H" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_TOTAL", i));

                    printSheet.SetCellValue(0, "J" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "LOCATION_ST", i));
                    printSheet.SetCellValue(0, "K" + (8 + i).ToString(), string.Format("{0:0.0}", double.Parse(Dbconn.conn.getData(reportListDs, "PRO_Q", i))));
                    printSheet.SetCellValue(0, "L" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "OR_QTY", i));
                    printSheet.SetCellValue(0, "M" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "PRO_QTY", i));
                    printSheet.SetCellValue(0, "N" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "F_Q", i));
                    printSheet.SetCellValue(0, "O" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "E_Q", i));
                    printSheet.SetCellValue(0, "P" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "PA_Q", i));
                    printSheet.SetCellValue(0, "Q" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "DIFF", i));
                    printSheet.SetCellValue(0, "S" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "SAMPLE_TLY", i));
                    printSheet.SetCellValue(0, "T" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "DAN1", i));
                    printSheet.SetCellValue(0, "U" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "DAN2", i));
                    printSheet.SetCellValue(0, "V" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "DAN3", i));
                }

                SQL = $@"
                SELECT CHK_LIST1
                     , CHK_LIST2
                     , CHK_LIST3
                     , CHK_LIST4
                     , REMARK1
                     , REMARK2
                     , I_TIME
                     , I_USER
                FROM PACK_CHK_LIST
                WHERE WORK_NUMBER = '{sWorkDate}'
                  AND SBNO = '{sbno}'
                ";

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) == 1)
                {
                    printSheet.SetCellValue(0, "F25", Dbconn.conn.getData(chkListDs, "CHK_LIST1", 0));
                    printSheet.SetCellValue(0, "R25", Dbconn.conn.getData(chkListDs, "CHK_LIST2", 0));
                    printSheet.SetCellValue(0, "F26", Dbconn.conn.getData(chkListDs, "CHK_LIST3", 0));
                    printSheet.SetCellValue(0, "R26", Dbconn.conn.getData(chkListDs, "CHK_LIST4", 0));


                    string remark1 = Dbconn.conn.getData(chkListDs, "REMARK1", 0);
                    string[] remark1_extend = remark1.Split('\r');

                    for (int i = 0; i < remark1_extend.Length; i++)
                    {
                        printSheet.SetCellValue(0, "A" + (28 + i).ToString(), remark1_extend[i]);
                    }

                    string remark2 = Dbconn.conn.getData(chkListDs, "REMARK2", 0);
                    string[] remark2_extend = remark2.Split('\r');

                    for (int i = 0; i < remark2_extend.Length; i++)
                    {
                        printSheet.SetCellValue(0, "M" + (28 + i).ToString(), remark2_extend[i]);
                    }

                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintPackReport", ex);
            }

        }

        public static void PrintBagReport(string plantCode, string processkey, string lCode, string sWorkDate)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\톤백작업일지.xlsx", DocumentFormat.Xlsx);

                printSheet.SetCellValue(0, "N4", sWorkDate.Substring(0, 4) + "년 " + sWorkDate.Substring(4, 2) + "월 " + sWorkDate.Substring(6, 2) + "일");

                string SQL = $@"
                SELECT 
                     br.RESOURCE_NO
                   , p.DESCRIPTION
                   , br.LOCATION_ST
                   , br.BAG_NUMBER
                   , TO_CHAR(br.RUN_ST, 'HH24:MI') AS RUN_ST
                   , TO_CHAR(br.RUN_ET, 'HH24:MI') AS RUN_ET
                   , br.RUN_TOTAL
                   , br.WAIT_TIME
                   , FLOOR(br.OR_QTY) AS OR_QTY
                   , br.PRO_QTY1
                   , br.PRO_QTY2
                   , br.USE_END_QTY
                   , br.END_QTY
                   , br.F_Q
                   , br.E_Q
                   , br.TOTAL_Q
                   , br.SAMPLE_TLY
                FROM BAG_REPORT br
                LEFT JOIN SAP_DI_PRODUCT p ON br.PLANT_CODE = p.PLANT_CODE AND br.RESOURCE_NO = p.RESOURCE_NO
                WHERE br.PLANT_CODE = '{plantCode}'
                    AND br.PROCESS_KEY = '{processkey}'
                    AND br.L_CODE = '{lCode}'
                    AND br.WORK_NUMBER = '{sWorkDate}'
                ORDER BY br.WORK_NUMBER
                ";

                DataSet reportListDs = Dbconn.conn.ExecutDataset(SQL);

                for (int i = 0; i < Dbconn.conn.getRowCnt(reportListDs); i++)
                {
                    printSheet.SetCellValue(0, "B" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, "F" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "LOCATION_ST", i));
                    printSheet.SetCellValue(0, "G" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "BAG_NUMBER", i));
                    printSheet.SetCellValue(0, "I" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_ST", i));
                    printSheet.SetCellValue(0, "K" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_ET", i));
                    printSheet.SetCellValue(0, "M" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_TOTAL", i));
                    printSheet.SetCellValue(0, "O" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "WAIT_TIME", i));
                    printSheet.SetCellValue(0, "Q" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "OR_QTY", i));
                    printSheet.SetCellValue(0, "S" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "PRO_QTY1", i));
                    printSheet.SetCellValue(0, "U" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "PRO_QTY2", i));
                    printSheet.SetCellValue(0, "W" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "USE_END_QTY", i));
                    printSheet.SetCellValue(0, "Y" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "END_QTY", i));
                    printSheet.SetCellValue(0, "AA" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "F_Q", i));
                    printSheet.SetCellValue(0, "AC" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "E_Q", i));
                    printSheet.SetCellValue(0, "AE" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "TOTAL_Q", i));
                    printSheet.SetCellValue(0, "AG" + (8 + i).ToString(), Dbconn.conn.getData(reportListDs, "SAMPLE_TLY", i));
                }


                SQL = $@"
                SELECT 
                PLANT_CODE, WORK_NUMBER, PROCESS_KEY, 
                   L_CODE, REAL_CHK1, REAL_CHK2, 
                   REAL_CHK3, MAG_CLEAN_1_1, MAG_CLEAN_1_2, 
                   MAG_CLEAN_2_1, MAG_CLEAN_2_2, MAG_CLEAN_3_1, 
                   MAG_CLEAN_3_2, MAG_CLEAN_4_1, MAG_CLEAN_4_2, 
                   MAG_CLEAN_5_1, MAG_CLEAN_5_2, MAG_CLEAN_6_1, 
                   MAG_CLEAN_6_2, MAG_CLEAN_7_1, MAG_CLEAN_7_2, 
                   MAG_CLEAN_8, BIN_CHK_1_1, BIN_CHK_1_2, 
                   BIN_CHK_1_3, BIN_CHK_1_4, BIN_CHK_2_1, 
                   BIN_CHK_2_2, BIN_CHK_2_3, BIN_CHK_2_4, 
                   BIN_CHK_3_1, BIN_CHK_3_2, BIN_CHK_3_3, 
                   BIN_CHK_3_4, DUCK_CHK_TIME1, DUCK_CHK_KG1, 
                   DUCK_TIME2, DUCK_CHK_KG2, DUCK_TIME3, 
                   DUCK_CHK_KG3, DUCK_TIME4, DUCK_CHK_KG4, 
                   REMARK1, REMARK2, I_TIME, 
                   I_USER
                FROM BAG_CHK_LIST
                WHERE PLANT_CODE = '{plantCode}'
                    AND PROCESS_KEY = '{processkey}'
                    AND L_CODE = '{lCode}'
                    AND WORK_NUMBER = '{sWorkDate}'
                ";

                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) == 1)
                {
                    printSheet.SetCellValue(0, "C31", Dbconn.conn.getData(chkListDs, "REAL_CHK1", 0));
                    printSheet.SetCellValue(0, "C32", Dbconn.conn.getData(chkListDs, "REAL_CHK2", 0));
                    printSheet.SetCellValue(0, "C33", Dbconn.conn.getData(chkListDs, "REAL_CHK3", 0));
                    printSheet.SetCellValue(0, "H32", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_1_1", 0));
                    printSheet.SetCellValue(0, "H33", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_1_2", 0));
                    printSheet.SetCellValue(0, "J32", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_2_1", 0));
                    printSheet.SetCellValue(0, "J33", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_2_2", 0));
                    printSheet.SetCellValue(0, "L32", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_3_1", 0));
                    printSheet.SetCellValue(0, "L33", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_3_2", 0));
                    printSheet.SetCellValue(0, "N32", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_4_1", 0));
                    printSheet.SetCellValue(0, "N33", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_4_2", 0));
                    printSheet.SetCellValue(0, "P32", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_5_1", 0));
                    printSheet.SetCellValue(0, "P33", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_5_2", 0));
                    printSheet.SetCellValue(0, "R32", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_6_1", 0));
                    printSheet.SetCellValue(0, "R33", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_6_2", 0));
                    printSheet.SetCellValue(0, "U32", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_7_1", 0));
                    printSheet.SetCellValue(0, "U33", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_7_2", 0));
                    printSheet.SetCellValue(0, "W32", Dbconn.conn.getData(chkListDs, "MAG_CLEAN_8", 0));

                    printSheet.SetCellValue(0, "AA32", Dbconn.conn.getData(chkListDs, "BIN_CHK_1_1", 0));
                    printSheet.SetCellValue(0, "AC32", Dbconn.conn.getData(chkListDs, "BIN_CHK_1_2", 0));
                    printSheet.SetCellValue(0, "AE32", Dbconn.conn.getData(chkListDs, "BIN_CHK_1_3", 0));
                    printSheet.SetCellValue(0, "AG32", Dbconn.conn.getData(chkListDs, "BIN_CHK_1_4", 0));
                    printSheet.SetCellValue(0, "AA33", Dbconn.conn.getData(chkListDs, "BIN_CHK_2_1", 0));
                    printSheet.SetCellValue(0, "AC33", Dbconn.conn.getData(chkListDs, "BIN_CHK_2_2", 0));
                    printSheet.SetCellValue(0, "AE33", Dbconn.conn.getData(chkListDs, "BIN_CHK_2_3", 0));
                    printSheet.SetCellValue(0, "AG33", Dbconn.conn.getData(chkListDs, "BIN_CHK_2_4", 0));
                    printSheet.SetCellValue(0, "AA34", Dbconn.conn.getData(chkListDs, "BIN_CHK_3_1", 0));
                    printSheet.SetCellValue(0, "AC34", Dbconn.conn.getData(chkListDs, "BIN_CHK_3_2", 0));
                    printSheet.SetCellValue(0, "AE34", Dbconn.conn.getData(chkListDs, "BIN_CHK_3_3", 0));
                    printSheet.SetCellValue(0, "AG34", Dbconn.conn.getData(chkListDs, "BIN_CHK_3_4", 0));

                    printSheet.SetCellValue(0, "Z36", Dbconn.conn.getData(chkListDs, "DUCK_CHK_TIME1", 0));
                    printSheet.SetCellValue(0, "Z37", Dbconn.conn.getData(chkListDs, "DUCK_CHK_TIME2", 0));
                    printSheet.SetCellValue(0, "Z38", Dbconn.conn.getData(chkListDs, "DUCK_CHK_TIME3", 0));
                    printSheet.SetCellValue(0, "Z39", Dbconn.conn.getData(chkListDs, "DUCK_CHK_TIME4", 0));
                    printSheet.SetCellValue(0, "AD36", Dbconn.conn.getData(chkListDs, "DUCK_CHK_KG1", 0));
                    printSheet.SetCellValue(0, "AD37", Dbconn.conn.getData(chkListDs, "DUCK_CHK_KG2", 0));
                    printSheet.SetCellValue(0, "AD38", Dbconn.conn.getData(chkListDs, "DUCK_CHK_KG3", 0));
                    printSheet.SetCellValue(0, "AD39", Dbconn.conn.getData(chkListDs, "DUCK_CHK_KG4", 0));

                    printSheet.SetCellValue(0, "B35", Dbconn.conn.getData(chkListDs, "REMARK1", 0));
                    printSheet.SetCellValue(0, "M35", Dbconn.conn.getData(chkListDs, "REMARK2", 0));
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintMilkReport", ex);
            }
        }

        public static string FindExcelFormFolder()
        {
            try
            {
                // ① 실행 파일 경로 (실제 EXE가 존재하는 위치)
                string exePath = Application.StartupPath;
                string excelFormPath = Path.Combine(exePath, "excel_form");

                // ② ClickOnce 데이터 디렉토리 (읽기/쓰기용)
                string dataPath = string.Empty;
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    dataPath = ApplicationDeployment.CurrentDeployment.DataDirectory;
                }

                // ③ excel_form 폴더가 실행 경로에 존재하는지 확인
                if (Directory.Exists(excelFormPath))
                {
                    return excelFormPath;
                }

                // ④ 실행경로 기준 상위 폴더에도 없는 경우, DataDirectory 쪽도 확인
                string altPath = Path.Combine(dataPath, "excel_form");
                if (!string.IsNullOrEmpty(dataPath) && Directory.Exists(altPath))
                {
                    return altPath;
                }

                // ⑤ 모두 없으면 경고 표시
                MessageBox.Show(
                    $"excel_form 폴더를 찾을 수 없습니다.\n\n시도한 경로:\n1) {excelFormPath}\n2) {altPath}",
                    "폴더 없음",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("폴더 탐색 중 오류가 발생했습니다.\n\n" + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        public static void PrintSetWeighingSheet(string sPlantCode, string sIsNo, string sDISPATCHNO = "", string sORDERNO = "", string sORDERLINENO = "", string sGubun = "")
        {
            //string[] aCarType = new string[] { "\\excel_form\\WeighingSheet_Bulk.xlsx", "\\excel_form\\WeighingSheet_Bag.xlsx", "\\excel_form\\WeighingSheet_Etc.xlsx" };
            string sExcelFormat = "\\excel_form\\WeighingSheet.xlsx";

            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();

                string excelPath = FindExcelFormFolder();
                string filePath = null;

                if (!string.IsNullOrEmpty(excelPath))
                {
                    filePath = Path.Combine(excelPath, "WeighingSheet.xlsx");
                }

                printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);

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

                //string sPath = System.IO.Path.Combine(
                //    ApplicationDeployment.CurrentDeployment.DataDirectory,
                //    "WeighingSheet.xlsx"); // 실제 파일 이름

                //if (clsCommon.UserId == "kfirst")
                //    ShowMessageBox.XtraShowInformation($"{sPath}");

                //printSheet.LoadDocument($"{sPath}", DocumentFormat.Xlsx);

                //if (clsCommon.UserId == "kfirst")
                //    ShowMessageBox.XtraShowInformation($"{sPath}");

                string SQL = $@"
                -- 검근표 차량정보
                SELECT a.IS_NO              -- 01 발급번호
                     , a.CAR_TYPE           -- 02 차량타입
                     , a.INCAR_NO           -- 03 차량번호
                     , a.VEHICLEGROUPCODE   -- 04 차량그룹
                     , a.WEIGHT_KG          -- 05 ?
                     , a.IN_WEIGHT          -- 06 입차무게
                     , a.OUT_WEIGHT         -- 07 출차무게
                     , a.TR_YN              -- 08 트레일러여부
                     , a.TR_WEIGHT          -- 09 트레일러무게
                     , a.USER_ID            -- 10 사용자ID
                     , a.INCAR_DATE         -- 11 일차일
                     , a.OUTCAR_DATE        -- 12 출차일
                     , a.PC_STATUS          -- 13 진행상태
                     , a.ERP_UP_YN          -- 14 ERP 전송여부
                     , a.ERP_TNUMBER        -- 15
                     , a.DEL_FLAG           -- 16 삭제여부
                     , a.TEM_TYPE           -- 17 
                     , a.PRINT_YN           -- 18
                     , a.I_TIME             -- 19
                     , a.I_USER             -- 20
                     , b.DRIVERNAME         -- 21 운전자명
                FROM WAP_DECAR a            -- TB01
                    LEFT JOIN TMS_INPUT_CARMASTER_CON b ON b.CARRIERCODE = a.INCAR_NO
                WHERE a.IS_NO = '{sIsNo}'
                ";

                DataSet dsDeCar = Dbconn.conn.ExecutDataset(SQL);

                if (sGubun == "Plo")
                {
                    SQL = $@"
                    -- 검근표 상차 실적
                    SELECT a.RT_TYPE, a.IS_NO, a.DISPATCHNO, 
                       a.ORDERNO, a.ORDERLINENO, a.PD_YN, 
                       a.RESOURCE_NO, b.DESCRIPTION, a.ZERO_W, a.QTY, 
                       a.QTY * CASE WHEN '{Factory}' = 'P201' THEN 0 ELSE TO_NUMBER(c.REF_1 )END AS PWEIGHT,
                       a.WEIGHT, a.CH_YN, a.I_TIME, a.PLANT_CODE
                    FROM TMS_OUTPUT_RESULT a
                        JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                        JOIN COMM_DTCODE c ON c.WK_DIVCODE = '03' AND c.COMM_CODE = '99' AND c.COMM_DTCODE = '039901'
                    WHERE a.IS_NO = '{sIsNo}'
                        AND a.DISPATCHNO = '{sDISPATCHNO}'
                        AND a.ORDERNO = '{sORDERNO}'
                        AND a.ORDERLINENO = '{sORDERLINENO}'
                    ";
                }
                else
                {
                    SQL = $@"
                    /* 검근표 원료 실적 */
                    SELECT 
                        a.IS_NO, a.EBELN, b.EBELP
                        , a.ESART, a.SRM_PREV_GR, a.PARTNER, a.EKORG
                        , a.SHIP_NAME, a.INVOICE_WEIGHT
                        , b.SEQ, b.MTART, b.MATNR, c.DESCRIPTION
                        , b.MAGRV, b.MEINS, b.GR_QNTY
                        , b.GR_QNTY_EA, b.GR_QNTY_BOX, b.GR_QNTY_KG
                        , b.GR_QNTY_KG * d.REF_1 AS PWEIGHT
                        , b.WERKS, b.EA_PACK_UNIT, b.REMARK
                        , b.JJO, b.SBDAY, b.I_TIME, b.I_USER
                        , '' AS BI_NUM
                    FROM WAP_GOCAR a
                        JOIN WAP_GOCARD b ON b.IS_NO = a.IS_NO AND b.EBELN = a.EBELN
                        JOIN SAP_DI_PRODUCT c ON c.PLANT_CODE = b.WERKS AND c.RESOURCE_NO = b.MATNR
                        JOIN COMM_DTCODE d ON d.WK_DIVCODE = '03' AND d.COMM_CODE = '99' AND d.COMM_DTCODE = '039901'
                    WHERE a.IS_NO = '{sIsNo}'
                    ";
                }

                DataSet dsOutResult = Dbconn.conn.ExecutDataset(SQL);

                SQL = $@"
                -- 검근표 상차지시
                SELECT a.DISPATCHNO, SUM(a.PLANQTY) AS PLANQTY
                   , a.TOLOCATIONCODE, NVL(b.DESCRIPTION, c.NAME_ORG1) AS DESCRIPTION
                   , c.NAME_ORG1 AS CUSTOMERNAME
                   , c.TEL_NUMBER_1, c.MOD_NUMBER
                FROM TMS_INPUT_PLOADD_CON a
                    LEFT JOIN SAP_DI_LOCATION b ON b.PLANT_CODE = a.PLANTCODE AND b.LOCATION = a.TOLOCATIONCODE
                    LEFT JOIN SAP_DI_CUSTOMER c ON c.PARTNER = a.TOLOCATIONCODE
                WHERE a.DISPATCHNO = '{sDISPATCHNO}'
                GROUP BY a.DISPATCHNO, a.TOLOCATIONCODE, NVL(b.DESCRIPTION, c.NAME_ORG1) , c.NAME_ORG1, c.TEL_NUMBER_1, c.MOD_NUMBER
                ";

                DataSet dsCust = Dbconn.conn.ExecutDataset(SQL);

                SetWeighingSheet(printSheet, dsDeCar, dsOutResult, dsCust);

                SQL = $@"
                -- 검근표 성분표
                SELECT a.PLANT_CODE, a.RESOURCE_NO, b.DESCRIPTION, a.FIELD_FLAG 
                    , a.FIELD_NAME, a.SEQ_NO, a.FIELD_VALUE01, a.FIELD_VALUE02
                    , a.ERDAT, ERZET, a.AEDAT, a.AEZET, a.TRANS_DATE
                FROM SAP_IN_PLA a
                    JOIN SAP_DI_PRODUCT b ON b.PLANT_CODE = a.PLANT_CODE
                                        AND b.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{sPlantCode}' AND a.RESOURCE_NO = '{Dbconn.conn.getData(dsOutResult, "RESOURCE_NO", 0)}'
                ";

                DataSet dsPla = Dbconn.conn.ExecutDataset(SQL);

                SetInPLA(printSheet, dsPla);

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintSetWeighingSheet", ex);
            }
        }

        /// <summary>
        /// 벌크 검근표
        /// </summary>
        /// <param name="printSheet"></param>
        /// <param name="dsDeCar"></param>
        /// <param name="dsOutResult"></param>
        /// <param name="dsCust"></param>
        private static void SetWeighingSheet(SpreadsheetControl printSheet, DataSet dsDeCar, DataSet dsOutResult, DataSet dsCust)
        {
            int iInWeight = int.Parse(Dbconn.conn.getData(dsDeCar, "IN_WEIGHT", 0));
            //int iOutWeight = int.Parse(Dbconn.conn.getData(dsDeCar, "OUT_WEIGHT", 0));

            int iOutWeight = 0;
            string val = Dbconn.conn.getData(dsDeCar, "OUT_WEIGHT", 0);

            if (!string.IsNullOrEmpty(val))
            {
                int.TryParse(val, out iOutWeight);
            }

            string sInWeight = iInWeight.ToString("N0");
            string sOutWeight = iOutWeight.ToString("N0");

            string sResult = string.Empty;

            if (Dbconn.conn.getData(dsDeCar, "CAR_TYPE", 0).Contains("001"))
                sResult = (iInWeight - iOutWeight).ToString("N0");
            else
                sResult = (iOutWeight - iInWeight).ToString("N0");

            printSheet.SetCellValue(0, "E5", Dbconn.conn.getData(dsDeCar, "INCAR_NO", 0));          // 차량번호
            printSheet.SetCellValue(0, "K5", Dbconn.conn.getData(dsDeCar, "DRIVERNAME", 0));        // 기사명
            printSheet.SetCellValue(0, "O5", Dbconn.conn.getData(dsDeCar, "IS_NO", 0));             // 계량번호

            printSheet.SetCellValue(0, "E6", Dbconn.conn.getData(dsDeCar, "INCAR_DATE", 0));        // 입차시간
            printSheet.SetCellValue(0, "O6", sInWeight);                                            // 입차중량

            printSheet.SetCellValue(0, "E7", Dbconn.conn.getData(dsDeCar, "OUTCAR_DATE", 0));       // 출차시간
            printSheet.SetCellValue(0, "O7", sOutWeight);                                           // 출차중량

            printSheet.SetCellValue(0, "E8", Dbconn.conn.getData(dsCust, "CUSTOMERNAME", 0));       // 거래처
            printSheet.SetCellValue(0, "O8", Dbconn.conn.getData(dsOutResult, "PWEIGHT", 0));       // 피중량

            printSheet.SetCellValue(0, "E9", Dbconn.conn.getData(dsCust, "DESCRIPTION", 0));        // 배송처
            printSheet.SetCellValue(0, "O9", sResult);                                              // 실중량

            printSheet.SetCellValue(0, "F10", Dbconn.conn.getData(dsCust, "DRIVERMOBILE", 0));      // 연락처
            //printSheet.SetCellValue(0, "J10", Dbconn.conn.getData(dsCust, "MOD_NUMBER", 0));        // 모바일
            printSheet.SetCellValue(0, "O10", dsCust.Tables[0].Rows.Count == 0 ? "" : int.Parse(Dbconn.conn.getData(dsCust, "PLANQTY", 0)).ToString("N0"));           // 송장량

            printSheet.SetCellValue(0, "E13", Dbconn.conn.getData(dsOutResult, "DESCRIPTION", 0));      // 품목
            printSheet.SetCellValue(0, "E14", "");                                                      // 비고
            printSheet.SetCellValue(0, "E16", Dbconn.conn.getData(dsOutResult, "BI_NUM", 0));           // 봉인번호

            printSheet.SetCellValue(0, "N15", DateTime.Now.ToString("yyyy년MM월dd일"));
        }

        private static void SetPLAData(DataTable dt)
        {
            sD24 = string.Empty;
            sD25 = string.Empty;
            sD26 = string.Empty;
            sD27 = string.Empty;
            sD28 = string.Empty;
            sD29 = string.Empty;
            sD30 = string.Empty;
            sD31 = string.Empty;
            sD32 = string.Empty;
            sD33 = string.Empty;
            sM28 = string.Empty;
            sP28 = string.Empty;
            sM40 = string.Empty;
            sC97 = string.Empty;
            sF99 = string.Empty;
            sC36 = string.Empty;

            lG49 = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                string fieldName = dr["FIELD_NAME"].ToString();
                string fieldValue = dr["FIELD_VALUE01"].ToString();

                string fieldValue2 = dr["FIELD_VALUE02"].ToString();

                switch (fieldName)
                {
                    case "성분등록번호": sD24 = fieldValue; break;
                    case "사료의명칭": sD25 = fieldValue; break;
                    case "사료의 형태": sD26 = fieldValue; break;
                    case "사료의 용도": sD27 = fieldValue; break;
                    case "본사": sD28 = fieldValue; break;
                    case "출고지": sD29 = fieldValue; break;
                    case "공장": sD30 = fieldValue; break;
                    case "판매원": sD31 = fieldValue; break;
                    case "제조년월": sD32 = fieldValue; break;
                    case "유통기한": sD33 = fieldValue; break;
                    case "등록성분량":

                        if (sM28 == fieldName) sM28 = fieldValue;
                        else sM28 += "\n".Merge(fieldValue);

                        if (sP28 == fieldName) sP28 = fieldValue2;
                        else sP28 += "\n".Merge(fieldValue2);
                        break;

                    case "사용한원료의명칭": sM40 += fieldValue; break;
                    case "유전자 변형 옥수수 포함": sC97 = fieldValue; break;
                    case "동물용 의약품 첨가 내용": sF99 = fieldValue; break;

                    case "주의사항":
                        if (sC36 == fieldName) sC36 = fieldValue;
                        else sC36 += "\n".Merge(fieldValue);
                        break;
                    case "제조업자 상호 및 전화번호":
                        lG49.Add(fieldValue);
                        break;
                }
            }
        }


        /// <summary>
        /// 벌크 성분표
        /// </summary>
        /// <param name="printSheet"></param>
        /// <param name="dsPla"></param>
        private static void SetInPLA(SpreadsheetControl printSheet, DataSet dsPla)
        {
            SetPLAData(dsPla.Tables[0]);

            printSheet.SetCellValue(0, "E22", Dbconn.conn.getData(dsPla, "DESCRIPTION", 0));
            printSheet.SetCellValue(0, "D24", sD24);                                // 성분등록번호
            printSheet.SetCellValue(0, "D25", sD25);                                // 사용한원료의명칭
            printSheet.SetCellValue(0, "D26", sD26);                                // 사료의 형태
            printSheet.SetCellValue(0, "D27", sD27);                                // 사료의 용도

            printSheet.SetCellValue(0, "D28", sD28);                                // 실제 중량
            printSheet.SetCellValue(0, "D29", sD29);                                // 제조년월
            printSheet.SetCellValue(0, "D30", sD30);                                // 유통기한
            printSheet.SetCellValue(0, "D31", sD31);                                // 재포장내용
            printSheet.SetCellValue(0, "D32", sD32);                                // 판매원
            printSheet.SetCellValue(0, "D33", sD33);                                // 동물의약품

            printSheet.SetCellValue(0, "M28", sM28);                                // 성분명
            printSheet.SetCellValue(0, "P28", sP28);                                // 성분량

            printSheet.SetCellValue(0, "M40", sM40);                                // 사용한 원료의 명칭

            //printSheet.SetCellValue(0, "C97", "SUB_NOTE";                         // 유전자 변형 옥수수 포함
            //printSheet.SetCellValue(0, "F99", "SPTMNM" + "CTVQTY" + ", ";         // 동물용 의약품 첨가 내용
            printSheet.SetCellValue(0, "C36", sC36);                                // 주의사항
            for (int i = 0; i < lG49.Count; i++)
            {
                printSheet.SetCellValue(0, "G" + (49 + i), lG49[i]);
            }

        }

        public static void PrinWeighingReport_HS(string is_no, string posnr = "")
        {
            try
            {
                string SQL = string.Empty;
                int totalW = 0;
                int zeroW = 0;
                int realW = 0;

                SQL = $@"
                WITH SMORDER AS (
                    SELECT a.ERP_UP_YN, a.POSNR, a.VDATU, a.KUNNR_SP, a.KUNNR_SH, a.KWMENG, a.VRKME, a.IS_NO, a.BSTKD, a.MATNR
                         , MAX(a.POSNR) OVER (PARTITION BY a.IS_NO) AS MAXP, I_TIME
                         , b.ZERO_W
                    FROM SAP_OUTPUT_SMORDER a
                        LEFT JOIN TMS_OUTPUT_RESULT b ON b.IS_NO = a.IS_NO AND a.BSTKD = 'SM' || b.DISPATCHNO
                                                                                        AND TO_NUMBER(a.POSNR) = b.ORDERLINENO
                    WHERE ('{is_no}' IS NULL OR a.IS_NO = '{is_no}') 
                )

                SELECT DISTINCT
                     a.IS_NO              -- 발급번호
                   , a.CAR_TYPE
                   , e.VEHICLENO AS INCAR_NO           -- 차량전체번호
                   , f.NAME_ORG1 AS CUST_NAME
                   , CASE WHEN b.POSNR = '10' THEN a.IN_WEIGHT ELSE c.ZERO_W END IN_WEIGHT       -- 입차중량
                   , NVL(b.ZERO_W, a.OUT_WEIGHT) AS OUT_WEIGHT         -- 출차중량
                   , CASE WHEN b.POSNR = '10' THEN a.INCAR_DATE ELSE c.I_TIME END INCAR_DATE         -- 입차일시
                   , TO_CHAR(CASE WHEN b.POSNR = '10' THEN a.INCAR_DATE ELSE c.I_TIME END, 'HH24:MI') AS INCAR_TIME
                   , CASE WHEN b.POSNR = b.MAXP THEN a.OUTCAR_DATE ELSE b.I_TIME END OUTCAR_DATE        -- 출차일시
                   , TO_CHAR(CASE WHEN b.POSNR = b.MAXP THEN a.OUTCAR_DATE ELSE b.I_TIME END, 'HH24:MI') AS OUTCAR_TIME
                   , a.PC_STATUS          -- 진행상태
                   , b.ERP_UP_YN          -- ERP 전송상태 
                   , b.POSNR
                   , CASE WHEN b.MATNR IS NULL THEN '' ELSE TO_CHAR(d.DESCRIPTION) END RESOURCE_NO
                   , b.VDATU
                   , b.KUNNR_SP
                   , b.KUNNR_SH
                   , b.KWMENG
                   , b.VRKME
                FROM WAP_DECAR a
                    LEFT JOIN SMORDER b ON b.IS_NO = a.IS_NO
                    LEFT JOIN SMORDER c ON c.IS_NO = a.IS_NO AND c.POSNR = b.POSNR - 10
                    LEFT JOIN SAP_DI_PRODUCT d ON d.RESOURCE_NO = b.MATNR
                    LEFT JOIN SAP_DI_CUSTOMER f ON f.PARTNER = b.KUNNR_SP
                    LEFT JOIN TMS_INPUT_CARMASTER_CON E ON e.VEHICLECODE = a.INCAR_NO
                WHERE ('{is_no}' IS NULL OR a.IS_NO = '{is_no}') 
                    AND ('{posnr}' IS NULL OR b.POSNR = '{posnr}')
                    AND a.CAR_TYPE = '{clsCommon.GetCarInputTypeCode("홍성물류")}'
                ORDER BY DECODE(b.ERP_UP_YN, '', 1, 'N', 2, 'M', 3, 'C', 4, 'F', 5, 'U', 6, 'D', 7, 8) ASC, a.IS_NO DESC, b.POSNR ASC
                ";

                DataSet listDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(listDs) < 1) return;

                for (int i = 0; i < Dbconn.conn.getRowCnt(listDs); i++)
                {
                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();

                    string excelPath = FindExcelFormFolder();
                    string filePath = null;

                    if (!string.IsNullOrEmpty(excelPath))
                    {
                        filePath = Path.Combine(excelPath, "WeighingSheet_HS.xlsx");
                    }

                    printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);

                    printSheet.Document.Worksheets[0].Cells["A3"].Value = $"계량번호 : {is_no}";

                    string inTimeStr = listDs.Tables[0].Rows[i]["INCAR_DATE"].ToString();
                    string formattedDate = string.Empty;

                    if (DateTime.TryParse(inTimeStr, out DateTime inTime))
                    {
                        formattedDate = inTime.ToString("yyyy-MM-dd");
                    }

                    totalW = Int32.Parse(listDs.Tables[0].Rows[i]["OUT_WEIGHT"].ToString());
                    zeroW = Int32.Parse(listDs.Tables[0].Rows[i]["IN_WEIGHT"].ToString());
                    realW = Int32.Parse(listDs.Tables[0].Rows[i]["KWMENG"].ToString());

                    printSheet.Document.Worksheets[0].Cells["B6"].Value = formattedDate;
                    printSheet.Document.Worksheets[0].Cells["B7"].Value = listDs.Tables[0].Rows[i]["INCAR_NO"].ToString();
                    printSheet.Document.Worksheets[0].Cells["B8"].Value = listDs.Tables[0].Rows[i]["CUST_NAME"].ToString();
                    printSheet.Document.Worksheets[0].Cells["B9"].Value = listDs.Tables[0].Rows[i]["RESOURCE_NO"].ToString();

                    printSheet.Document.Worksheets[0].Cells["B10"].Value = listDs.Tables[0].Rows[i]["OUTCAR_TIME"].ToString();
                    printSheet.Document.Worksheets[0].Cells["C10"].Value = totalW.ToString("#,##0") + " Kg";        // 출차중량(총중량)
                    printSheet.Document.Worksheets[0].Cells["B11"].Value = listDs.Tables[0].Rows[i]["INCAR_TIME"].ToString();
                    printSheet.Document.Worksheets[0].Cells["C11"].Value = zeroW.ToString("#,##0") + " Kg";
                    printSheet.Document.Worksheets[0].Cells["C12"].Value = realW.ToString("#,##0") + " Kg";

                    printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                    printSheet.Options.Print.ShowMarginsWarning = false;
                    //printSheet.Print();
                    printSheet.ShowPrintPreview();

                    clsUtil.Delay(800);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintExcel", "PrintDeliverySheet", ex);
                clsLog.logSave("clsPrintExcel", "PrintDeliverySheet", ex.StackTrace);
                clsLog.logSave("clsPrintExcel", "PrintDeliverySheet", ex.Source);
            }
        }

        /// <summary>
        /// 지대 검근표
        /// </summary>
        /// <param name="printSheet"></param>
        /// <param name="dsDeCar"></param>
        /// <param name="dsOutResult"></param>
        /// <param name="dsCust"></param>
        private static void SetBag(SpreadsheetControl printSheet, DataSet dsDeCar, DataSet dsOutResult, DataSet dsCust)
        {
            string sInWeight = Dbconn.conn.getData(dsDeCar, "IN_WEIGHT", 0);
            string sOutWeight = Dbconn.conn.getData(dsDeCar, "OUT_WEIGHT", 0);

            string sResult = (int.Parse(sOutWeight) - int.Parse(sInWeight)).ToString();

            printSheet.SetCellValue(0, "E5", Dbconn.conn.getData(dsDeCar, "INCAR_NO", 0));          // 차량번호
            printSheet.SetCellValue(0, "E6", Dbconn.conn.getData(dsDeCar, "INCAR_TIME", 0));        // 입차시간
            printSheet.SetCellValue(0, "E7", Dbconn.conn.getData(dsDeCar, "OUTCAR_TIME", 0));       // 출차시간
            printSheet.SetCellValue(0, "E8", "제일사료(함안)");                                      // 거래처

            printSheet.SetCellValue(0, "O5", Dbconn.conn.getData(dsDeCar, "IS_NO", 0));             // 계량번호
            printSheet.SetCellValue(0, "O6", sInWeight);                                            // 입차중량
            printSheet.SetCellValue(0, "O7", sOutWeight);                                           // 출차중량
            printSheet.SetCellValue(0, "O8", Dbconn.conn.getData(dsOutResult, "PWEIGHT", 0));       // 피중량
            printSheet.SetCellValue(0, "O9", sResult);                                              // 실중량
            printSheet.SetCellValue(0, "O10", Dbconn.conn.getData(dsCust, "PLANQTY", 0));           // 송장량

            //printSheet.SetCellValue(0, "O8", ExDe(PDWeight)
            //printSheet.SetCellValue(0, "C39", ExDe(P899910)
            //printSheet.SetCellValue(0, "D39", ExDe(P899920)
            //printSheet.SetCellValue(0, "G39", ExDe(P899950)
            //printSheet.SetCellValue(0, "I39", ExDe(P899940)
            //printSheet.SetCellValue(0, "M39", ExDe(POTHER)

            //printSheet.SetCellValue(0, "E" & i + 12, Reader1.Item("ITEM_CODE").ToString()
            //printSheet.SetCellValue(0, "J" & i + 12, ExDe(Reader1.Item("PD_QTY").ToString()) & " x " & Reader1.Item("PACKU").ToString()
            //printSheet.SetCellValue(0, "N" & i + 12, ExDe(Reader1.Item("PD_UWEIGHT").ToString()) & " kg"
            //printSheet.SetCellValue(0, "O" & i + 12, Reader1.Item("PACKT").ToString()

            //printSheet.SetCellValue(0, "O9", ExDe(Int(ActWeight))

            //printSheet.SetCellValue(0, "E" & i + 12, FindStr("PTMCDVW@COMMLOG", "PTMNM", "PTMCD='" & .Document.Worksheets(0).Cells("E" & i + 12).Value.ToString() & "' AND PACKTYPE='" & .Document.Worksheets(0).Cells("O" & i + 12).Value.ToString() & "' AND PTMNM NOT LIKE '%중단%' AND PTMNM NOT LIKE '%사용불가%'", False)
            //printSheet.SetCellValue(0, "O" & i + 12, "";
        }

        /// <summary>
        /// 기타 차량 검근표
        /// </summary>
        /// <param name="printSheet"></param>
        /// <param name="dsDeCar"></param>
        /// <param name="dsOutResult"></param>
        /// <param name="dsCust"></param>
        private static void SetEtc(SpreadsheetControl printSheet, DataSet dsDeCar, DataSet dsOutResult, DataSet dsCust)
        {
            string sInWeight = Dbconn.conn.getData(dsDeCar, "IN_WEIGHT", 0);
            string sOutWeight = Dbconn.conn.getData(dsDeCar, "OUT_WEIGHT", 0);

            string sResult = (int.Parse(sOutWeight) - int.Parse(sInWeight)).ToString();

            printSheet.SetCellValue(0, "E5", Dbconn.conn.getData(dsDeCar, "INCAR_NO", 0));          // 차량번호
            printSheet.SetCellValue(0, "E6", Dbconn.conn.getData(dsDeCar, "INCAR_TIME", 0));        // 입차시간
            printSheet.SetCellValue(0, "E7", Dbconn.conn.getData(dsDeCar, "OUTCAR_TIME", 0));       // 출차시간
            printSheet.SetCellValue(0, "E8", "제일사료(함안)");                                      // 거래처
            printSheet.SetCellValue(0, "E9", Dbconn.conn.getData(dsCust, "DESCRIPTION", 0));        // 배송처

            printSheet.SetCellValue(0, "O5", Dbconn.conn.getData(dsDeCar, "IS_NO", 0));             // 계량번호
            printSheet.SetCellValue(0, "O6", sInWeight);                                            // 입차중량
            printSheet.SetCellValue(0, "O7", sOutWeight);                                           // 출차중량
            printSheet.SetCellValue(0, "O8", Dbconn.conn.getData(dsOutResult, "PWEIGHT", 0));       // 피중량
            printSheet.SetCellValue(0, "O9", sResult);                                              // 실중량
            printSheet.SetCellValue(0, "O10", Dbconn.conn.getData(dsCust, "PLANQTY", 0));           // 송장량

            printSheet.SetCellValue(0, "F10", Dbconn.conn.getData(dsCust, "TEL_NUMBER_1", 0));      // 연락처
            printSheet.SetCellValue(0, "J10", Dbconn.conn.getData(dsCust, "MOD_NUMBER", 0));        // 모바일

            printSheet.SetCellValue(0, "E12", ""); // 기타 종류

            //printSheet.SetCellValue(0, "E17", TmpLotNo
        }

        public static void PrintPelletReport(string sPlantCode, string sProcessKey, string sLCode, string sWorkDate)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\가공펠렛일지.xlsx", DocumentFormat.Xlsx);


                for (int j = 1; j < 3; j++)
                {
                    string SQL = $@"
                    SELECT 
                         a.RESOURCE_NO
                       , TRIM(p.DESCRIPTION) AS DESCRIPTION
                       , TO_CHAR(a.RUN_ST, 'HH24:MI') AS RUN_ST
                       , TO_CHAR(a.RUN_ET, 'HH24:MI') AS RUN_ET
                       , FLOOR(a.QTY / 1000) AS QTY
                       , a.REMAIN_QTY
                       , TO_CHAR(a.PROTY / 1000, 'FM9990.0') AS PROTY
                       , ROUND((a.RUN_ET - a.RUN_ST) * 1440) AS TOTAL_OPER
                       , a.DY_ST
                       , a.DY_THICK
                       , a.WORK_START_DATE
                       , a.ICM_CODE
                       , a.EMPLOYEE_NO
                       , a.DY_SP
                       , a.CURRENT_1
                       , a.CURRENT_2
                       , a.FEEDER_RATE
                       , a.CRUMBLE_YN
                       , a.HARDNESS
                       , a.PDI
                       , a.SBNO
                       , a.CLEAN_QTY
                       , a.LOCATION_ED
                       , a.LOCATION_ST
                       , a.HZ
                       , a.CD_TEMP
                       , a.P_TEMP
                       , a.COL_TEMP
                       , a.REMARK
                       , a.EMPLOYEE_NO
                    FROM PELLET_REPORT a
                    LEFT JOIN SAP_DI_PRODUCT p ON a.PLANT_CODE = p.PLANT_CODE AND a.RESOURCE_NO = p.RESOURCE_NO
                    WHERE a.PLANT_CODE = '{sPlantCode}'
                        AND a.PROCESS_KEY = '{sPlantCode}'
                        AND a.L_CODE = '{sLCode}'
                        AND a.WORKDATE = '{sWorkDate}'
                        AND NVL(a.DEL_FLAG, 'N') != 'Y'
                        AND a.SBNO = '{j}'
                    ORDER BY a.SBNO, a.WORKDATE, a.WORK_SEQ
                    ";

                    DataSet reportListDs = Dbconn.conn.ExecutDataset(SQL);

                    for (int i = 0; i < Dbconn.conn.getRowCnt(reportListDs); i++)
                    {
                        int excel_loc_index = 8;
                        if (j == 1)
                        {
                            excel_loc_index = 8;
                        }
                        else if (j == 2)
                        {
                            excel_loc_index = 18;
                        }

                        printSheet.SetCellValue(0, "D" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "DESCRIPTION", i));
                        printSheet.SetCellValue(0, "E" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_ST", i));
                        printSheet.SetCellValue(0, "G" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "RUN_ET", i));
                        printSheet.SetCellValue(0, "H" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "REMAIN_QTY", i));
                        printSheet.SetCellValue(0, "I" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "TOTAL_OPER", i));
                        printSheet.SetCellValue(0, "J" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "QTY", i));
                        printSheet.SetCellValue(0, "K" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "PROTY", i));
                        printSheet.SetCellValue(0, "L" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "LOCATION_ST", i));
                        printSheet.SetCellValue(0, "M" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "LOCATION_ED", i));


                        printSheet.SetCellValue(0, "N" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "DY_ST", i));
                        printSheet.SetCellValue(0, "O" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "DY_THICK", i));
                        printSheet.SetCellValue(0, "P" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "DY_SP", i));
                        printSheet.SetCellValue(0, "Q" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "CURRENT_1", i));
                        printSheet.SetCellValue(0, "R" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "CURRENT_2", i));
                        printSheet.SetCellValue(0, "S" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "FEEDER_RATE", i));
                        printSheet.SetCellValue(0, "T" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "CD_TEMP", i));
                        printSheet.SetCellValue(0, "U" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "P_TEMP", i));
                        printSheet.SetCellValue(0, "V" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "COL_TEMP", i));
                        printSheet.SetCellValue(0, "W" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "CRUMBLE_YN", i));
                        printSheet.SetCellValue(0, "X" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "PDI", i));
                        printSheet.SetCellValue(0, "Y" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "HARDNESS", i));
                        printSheet.SetCellValue(0, "Z" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "CLEAN_QTY", i));
                        printSheet.SetCellValue(0, "AA" + (excel_loc_index + i).ToString(), Dbconn.conn.getData(reportListDs, "HZ", i));

                    }
                }


                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintPelletReport", ex);
            }
        }

        public static void PrintMilkReport(string sWorkDate)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\대용유작업일지.xlsx", DocumentFormat.Xlsx);

                printSheet.SetCellValue(0, "Z4", Convert.ToDateTime(sWorkDate.Substring(0, 4) + "-" + sWorkDate.Substring(4, 2) + "-" + sWorkDate.Substring(6, 2)).ToString("yy.MM.dd(ddd)"));

                string SQL = $@"
                SELECT 
                     WORK_NUMBER
                   , WORK_SEQ
                   , SEQ
                   , RESOURCE_NO
                   , DESCRIPTION
                   , WEIGHT_ST
                   , WEIGHT_ET
                   , INPUT_ST
                   , INPUT_ET
                   , DOS_ST
                   , DOS_ET
                   , PACK_ST
                   , PACK_ET
                   , CLEAN_ST
                   , CELAN_ET
                   , ETC_ST
                   , ETC_ET
                   , REMARK_ST
                   , REMARK_ET
                   , DOS_Q
                   , OR_QTY
                   , PRO_QTY
                   , F_Q
                   , E_Q
                   , PA_Q
                   , DIFF
                   , OEGWAN
                   , DAN1
                   , DAN2
                   , DAN3
                   , I_TIME
                   , I_USER
                FROM MILK_REPORT
                WHERE WORK_NUMBER = '{sWorkDate}'
                ORDER BY WORK_NUMBER, WORK_SEQ
                ";

                DataSet reportListDs = Dbconn.conn.ExecutDataset(SQL);

                for (int i = 0; i < Dbconn.conn.getRowCnt(reportListDs); i++)
                {
                    printSheet.SetCellValue(0, "B" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "DESCRIPTION", i));

                    printSheet.SetCellValue(0, "E" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "WEIGHT_ST", i));
                    printSheet.SetCellValue(0, "E" + (8 + ((i * 2) + 1)).ToString(), Dbconn.conn.getData(reportListDs, "WEIGHT_ET", i));
                    printSheet.SetCellValue(0, "G" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "INPUT_ST", i));
                    printSheet.SetCellValue(0, "G" + (8 + ((i * 2) + 1)).ToString(), Dbconn.conn.getData(reportListDs, "INPUT_ET", i));
                    printSheet.SetCellValue(0, "I" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "DOS_ST", i));
                    printSheet.SetCellValue(0, "I" + (8 + ((i * 2) + 1)).ToString(), Dbconn.conn.getData(reportListDs, "DOS_ET", i));
                    printSheet.SetCellValue(0, "K" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "PACK_ST", i));
                    printSheet.SetCellValue(0, "K" + (8 + ((i * 2) + 1)).ToString(), Dbconn.conn.getData(reportListDs, "PACK_ET", i));
                    printSheet.SetCellValue(0, "K" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "CLEAN_ST", i));
                    printSheet.SetCellValue(0, "K" + (8 + ((i * 2) + 1)).ToString(), Dbconn.conn.getData(reportListDs, "CELAN_ET", i));
                    printSheet.SetCellValue(0, "K" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "ETC_ST", i));
                    printSheet.SetCellValue(0, "K" + (8 + ((i * 2) + 1)).ToString(), Dbconn.conn.getData(reportListDs, "ETC_ET", i));
                    printSheet.SetCellValue(0, "K" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "REMARK_ST", i));
                    printSheet.SetCellValue(0, "K" + (8 + ((i * 2) + 1)).ToString(), Dbconn.conn.getData(reportListDs, "REMARK_ET", i));

                    printSheet.SetCellValue(0, "S" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "DOS_Q", i));
                    printSheet.SetCellValue(0, "T" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "OR_QTY", i));
                    printSheet.SetCellValue(0, "U" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "PRO_QTY", i));
                    printSheet.SetCellValue(0, "V" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "F_Q", i));
                    printSheet.SetCellValue(0, "W" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "E_Q", i));
                    printSheet.SetCellValue(0, "X" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "PA_Q", i));
                    printSheet.SetCellValue(0, "Y" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "DIFF", i));
                    printSheet.SetCellValue(0, "Z" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "OEGWAN", i));
                    printSheet.SetCellValue(0, "AA" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "DAN1", i));
                    printSheet.SetCellValue(0, "AB" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "DAN2", i));
                    printSheet.SetCellValue(0, "AC" + (8 + ((i * 2))).ToString(), Dbconn.conn.getData(reportListDs, "DAN3", i));
                }

                SQL = $@"
                SELECT 
                     CHK_LIST1
                   , CHK_LIST2
                   , CHK_LIST3
                   , CHK_LIST4
                   , CHK_LIST5
                   , CHK_LIST6
                   , CHK_LIST7
                   , REMARK
                FROM MILK_CHK_LIST
                WHERE WORK_NUMBER = '{sWorkDate}'
                ";


                DataSet chkListDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(chkListDs) > 0)
                {
                    printSheet.SetCellValue(0, "M20".ToString(), Dbconn.conn.getData(chkListDs, "CHK_LIST1", 0));
                    printSheet.SetCellValue(0, "Z20".ToString(), Dbconn.conn.getData(chkListDs, "CHK_LIST2", 0));
                    printSheet.SetCellValue(0, "M21".ToString(), Dbconn.conn.getData(chkListDs, "CHK_LIST3", 0));
                    printSheet.SetCellValue(0, "Z21".ToString(), Dbconn.conn.getData(chkListDs, "CHK_LIST4", 0));
                    printSheet.SetCellValue(0, "M22".ToString(), Dbconn.conn.getData(chkListDs, "CHK_LIST5", 0));
                    printSheet.SetCellValue(0, "Z22".ToString(), Dbconn.conn.getData(chkListDs, "CHK_LIST6", 0));
                    printSheet.SetCellValue(0, "M23".ToString(), Dbconn.conn.getData(chkListDs, "CHK_LIST7", 0));
                    printSheet.SetCellValue(0, "A25".ToString(), Dbconn.conn.getData(chkListDs, "REMARK", 0));
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintMilkReport", ex);
            }
        }

        public static double IntRound(double Value, int Digit)
        {
            //double Temp = Math.Pow(10.0, Digit);
            //return Math.Round(Value * Temp) / Temp;
            double temp = Math.Pow(10.0, Digit);
            return Math.Round(Value * temp, MidpointRounding.AwayFromZero) / temp;
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


                DataSet cusListDs = Dbconn.conn.ExecutDataset(SQL);


                for (int i = 0; i < Dbconn.conn.getRowCnt(cusListDs); i++)
                {
                    int excelListPos = 14;

                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();

                    //string sExcelFormat = "\\excel_form\\WeighingSheet.xlsx";

                    string excelPath = FindExcelFormFolder();
                    string filePath = null;

                    if (!string.IsNullOrEmpty(excelPath))
                    {
                        filePath = Path.Combine(excelPath, "igo.xls");
                    }

                    printSheet.LoadDocument($"{filePath}", DocumentFormat.Xls);

                    //SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                    //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\igo.xls", DocumentFormat.Xls);

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
                        printSheet.Document.Worksheets[0].Cells["Y" + excelListPos.ToString()].Value = Convert.ToInt32(list_WEIGHT);

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
                        printSheet.Document.Worksheets[0].Cells["E39"].Value = Dbconn.conn.getData(carDs, "CARRIERNAME", 0); //배송거래처
                        printSheet.Document.Worksheets[0].Cells["W38"].Value = Dbconn.conn.getData(carDs, "VEHICLENO", 0); //차량번호
                        printSheet.Document.Worksheets[0].Cells["AB38"].Value = Dbconn.conn.getData(carDs, "DRIVERMOBILE", 0); //기사연락처
                        printSheet.Document.Worksheets[0].Cells["AG38"].Value = Dbconn.conn.getData(carDs, "DRIVERNAME", 0); //배송기사명
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
                        printSheet.Document.Worksheets[0].Cells["E39"].Value = Dbconn.conn.getData(carDs, "CARRIERNAME", 0);

                        fromCusDs.Dispose();
                    }
                    carDs.Dispose();

                    //파레트, 피무계 정보
                    if (carInType == "002" || carInType == "003")
                    {
                        SQL = "SELECT PM.PTMCDNM, WOA.WEIGHT * PD_QTY AS SUM_QTY   " +
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
                                    printSheet.Document.Worksheets[0].Cells["E37"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["E38"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
                                }

                                if (p == 1)
                                {
                                    printSheet.Document.Worksheets[0].Cells["H37"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["H38"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
                                }

                                if (p == 2)
                                {
                                    printSheet.Document.Worksheets[0].Cells["K37"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["K38"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
                                }

                                if (p == 3)
                                {
                                    printSheet.Document.Worksheets[0].Cells["P37"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["P38"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
                                }

                                if (p == 4)
                                {
                                    printSheet.Document.Worksheets[0].Cells["T37"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                                    printSheet.Document.Worksheets[0].Cells["T38"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
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
                }

                cusListDs.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintExcel", "PrintChulgoSheet", ex);
                clsLog.logSave("clsPrintExcel", "PrintChulgoSheet", ex.StackTrace);
                clsLog.logSave("clsPrintExcel", "PrintChulgoSheet", ex.Source);
            }

        }


        //검근표 발행
        public static void PrintWeighingSheet(string is_no, bool f_print = false)
        {
            try
            {
                string SQL = string.Empty;

                string carInType = clsCarUtil.returnCarType(is_no);
                string carType = clsCarUtil.returnCarGubun2(is_no);

                int pageCnt = 0;
                DataSet workCntDs = null;

                if (carType == "벌크" || carType == "카고")
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
                else if (carType == "")
                {
                    SQL = $@"
                    SELECT 
                        a.IS_NO, 
                        a.VEHICLEGROUPCODE,  
                        a.INCAR_NO, 
                        a.ETC_DETAIL,
                        b.I_TIME AS CHKIN_DATE,   
                        CEIL(a.IN_WEIGHT) AS IN_WEIGHT,  
                        CEIL(a.OUT_WEIGHT) AS OUT_WEIGHT, 
                        a.PC_STATUS, 
                        TO_CHAR(a.INCAR_DATE, 'YYYY-MM-DD HH24:MI:SS') AS INCAR_DATE,  
                        TO_CHAR(a.OUTCAR_DATE, 'YYYY-MM-DD HH24:MI:SS') AS OUTCAR_DATE, 
                        CEIL(a.IN_WEIGHT - a.OUT_WEIGHT) AS WEIGHT, 
                        ABS(CEIL(a.IN_WEIGHT - a.OUT_WEIGHT)) AS REAL_WEIGHT,
                        a.PC_STATUS,
                        a.I_USER,
                        c.DRIVERNAME, c.DRIVERMOBILE
                    FROM WAP_DECAR a
                        LEFT JOIN TMS_OUTPUT_RESULT  b ON b.IS_NO = a.IS_NO
                        LEFT JOIN TMS_INPUT_CARMASTER_CON c ON c.VEHICLENO = a.INCAR_NO
                    WHERE a.IS_NO = '{is_no}'
                    ORDER BY 
                        a.IS_NO
                    ";

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
                    return;
                }

                for (int i = 0; i < pageCnt; i++)
                {
                    string sDisNo = Dbconn.conn.getData(workCntDs, "DISPATCHNO", i);
                    string sOrderNo = Dbconn.conn.getData(workCntDs, "ORDERNO", i);
                    string sOrderLineNo = Dbconn.conn.getData(workCntDs, "ORDERLINENO", i);

                    //SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                    //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\weight.xlsx", DocumentFormat.Xlsx);

                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();

                    string excelPath = FindExcelFormFolder();
                    string filePath = null;

                    if (!string.IsNullOrEmpty(excelPath))
                    {
                        filePath = Path.Combine(excelPath, "WeighingSheet.xlsx");
                    }

                    string temp = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));
                    File.Copy(filePath, temp, true);

                    printSheet.LoadDocument($"{temp}", DocumentFormat.Xlsx);

                    //로그인 공장코드정보
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

                    SQL = $@"
                    SELECT  AD.PLANTCODE
                          , ADM.DELIVERYDATE
                          , AD.TOLOCATIONCODE
                          , ADM.VEHICLECODE
                          , CAR.VEHICLENO
                          , SAP_CUS.NAME_ORG1
                          , SAP_CUS2.NAME_ORG1 AS NAME_ORG2
                          , ADM.DISPATCHNO
                          , AD.ORDERNO
                          , AD.ORDERLINENO
                          , ADM.DRIVERNAME
                          , ADM.DRIVERMOBILE
                          , AD.ITEMCODE
                          , SAP_PRO.DESCRIPTION
                          , AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY
                          , (SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE
                          , SAP_PRO.UOM
                          , NVL(TMS_OUT_RESULT.PD_YN, 'N') AS PD_YN
                          , NVL(DECODE(SAP_PRO.UOM, 'EA', TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT), 0) AS WEIGTH
                          , NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT
                          , (AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ)) / (SAP_MA.UMREN / SAP_MA.UMREZ) AS PACK_CNT
                          , TMS_OUT_RESULT.BI_NUM
                          , NVL(SAP_CUS.MOD_NUMBER, SAP_CUS.TEL_NUMBER_1) AS TEL_NUMBER_1 --SAP_CUS.TEL_NUMBER_1
                          , SAP_CUS.RLTYP
                          , NVL(TMS_OUT_RESULT.ZERO_W, decar.OUT_WEIGHT) AS ZERO_W
                          , NVL(TMS_OUT_RESULT.BEFORE_WEIGHT, decar.IN_WEIGHT) AS BEFORE_WEIGHT
                          , TO_CHAR(NVL(BEFORE_WEIGHT_TIME, decar.INCAR_DATE), 'YYYY-MM-DD HH24:MI') AS BEFORE_WEIGHT_TIME
                          , TO_CHAR(NVL(WEIGHT_TIME, decar.OUTCAR_DATE), 'YYYY-MM-DD HH24:MI') AS WEIGHT_TIME
                          , ADM.DISPATCHMEMO
                    FROM    TMS_INPUT_PLOADM_CON ADM
                        JOIN    TMS_INPUT_PLOADD_CON AD
                            ON  ADM.DISPATCHNO = AD.DISPATCHNO
                        LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CAR
                            ON  ADM.VEHICLECODE = CAR.VEHICLECODE
                        LEFT OUTER JOIN (
                                SELECT RESOURCE_NO, DESCRIPTION, UOM
                                FROM SAP_DI_PRODUCT
                                GROUP BY RESOURCE_NO, DESCRIPTION, UOM
                            ) SAP_PRO
                            ON  AD.ITEMCODE = SAP_PRO.RESOURCE_NO
                        LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS
                            ON  AD.TOLOCATIONCODE = SAP_CUS.PARTNER
                        LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS2
                            ON  AD.FROMLOCATIONCODE = SAP_CUS2.PARTNER
                        LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT
                            ON  AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO
                            AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO
                            AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO
                        LEFT OUTER JOIN SAP_MARM SAP_MA
                            ON  AD.ITEMCODE = SAP_MA.MATNR
                            AND SAP_MA.MEINH = 'KG'
                        LEFT JOIN WAP_DECAR decar ON decar.IS_NO = TMS_OUT_RESULT.IS_NO
                    WHERE   TMS_OUT_RESULT.DISPATCHNO = '{sDisNo}'
                    AND     TMS_OUT_RESULT.ORDERNO = '{sOrderNo}'
                    AND     TMS_OUT_RESULT.ORDERLINENO = '{sOrderLineNo}'
                    ";

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
                    string sMemo = Dbconn.conn.getData(reportDs, "DISPATCHMEMO", 0);

                    //차량번호
                    printSheet.Document.Worksheets[0].Cells["E5"].Value = sCarFullNum;

                    //기사명
                    printSheet.Document.Worksheets[0].Cells["K5"].Value = sDriverName;

                    //계량번호
                    printSheet.Document.Worksheets[0].Cells["O5"].Value = is_no;

                    //입차시간
                    printSheet.Document.Worksheets[0].Cells["E6"].Value = sInWeightTime;

                    //입차중량
                    if (double.TryParse(sInWeight, out _))
                    {
                        printSheet.Document.Worksheets[0].Cells["O6"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                    }

                    //출차시간
                    printSheet.Document.Worksheets[0].Cells["E7"].Value = sOutWeightTime;

                    //출차중량
                    if (double.TryParse(sOutWeight, out _))
                    {
                        printSheet.Document.Worksheets[0].Cells["O7"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));
                    }

                    //거래처
                    printSheet.Document.Worksheets[0].Cells["E8"].Value = sCustNm;

                    int intPweight = 0;
                    string sPweight = "0";

                    //피무계합산
                    if (carType == "벌크")
                    {
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
                        sPweight = Dbconn.conn.getData(pWeightDs, "P_SUM", 0);

                        if (new string[] { "P101", "P102" }.Contains(Factory))
                        {
                            if (Convert.ToDecimal(sPweight) > 0)
                                sPweight = Convert.ToString(IntRound(Convert.ToDouble(sPweight), -1));
                        }
                        else if (new string[] { "PJ01", "PJ02", "PJ04", "PJ05" }.Contains(Factory))
                        {
                            if (Convert.ToDecimal(sPweight) > 0)
                                sPweight = Convert.ToString(Math.Ceiling(Convert.ToDouble(sPweight) / 10.0) * 10);
                        }
                        else
                            if (Convert.ToDecimal(sPweight) > 0)
                            sPweight = Convert.ToDouble(sPweight).ToString();

                        intPweight = Convert.ToInt32(sPweight);

                        if (double.TryParse(sPweight, out _))
                        {
                            if (Convert.ToDecimal(sPweight) > 0)
                            {
                                if (Factory == "PJ04")
                                {
                                    sPweight = Convert.ToString(Math.Ceiling(Convert.ToDouble(sPweight) / 10) * 10); //Convert.ToString(IntRound(Convert.ToDouble(sPweight), -1));
                                }
                                //피중량
                                printSheet.Document.Worksheets[0].Cells["O8"].Value = sPweight;
                            }
                        }

                    }

                    //송장량
                    if (double.TryParse(sPlanQty, out _))
                    {
                        printSheet.Document.Worksheets[0].Cells["O10"].Value = String.Format("{0:#,###}", Convert.ToInt32(sPlanQty));
                    }

                    //배송처
                    printSheet.Document.Worksheets[0].Cells["E9"].Value = sCusAddr;

                    //실중량
                    if (double.TryParse(sWeigth, out _))
                    {
                        printSheet.Document.Worksheets[0].Cells["O9"].Value = String.Format("{0:#,###}", carType == "카고" ? Convert.ToDecimal(sOutWeight) - Convert.ToDecimal(sInWeight) - intPweight : Convert.ToDecimal(sWeigth) - intPweight);
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
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex);
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex.StackTrace);
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex.Source);
            }

        }

        public static void PrintWeighingSheetCopy(string is_no)
        {
            try
            {
                string SQL = string.Empty;

                string carInType = clsCarUtil.returnCarType(is_no);
                string carType = clsCarUtil.returnCarGubun2(is_no);

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
                    return;
                }

                for (int i = 0; i < pageCnt; i++)
                {
                    string sDisNo = Dbconn.conn.getData(workCntDs, "DISPATCHNO", i);
                    string sOrderNo = Dbconn.conn.getData(workCntDs, "ORDERNO", i);
                    string sOrderLineNo = Dbconn.conn.getData(workCntDs, "ORDERLINENO", i);

                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();

                    //string sExcelFormat = "\\excel_form\\WeighingSheet.xlsx";

                    string excelPath = FindExcelFormFolder();
                    string filePath = null;

                    if (!string.IsNullOrEmpty(excelPath))
                    {
                        filePath = Path.Combine(excelPath, "weight3.xlsx");
                    }

                    printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);


                    //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\weight3.xlsx", DocumentFormat.Xlsx);

                    //공장코드
                    string Factory = clsCommon.PlantCode;

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
                            "NVL(DECODE(SAP_PRO.UOM, 'EA' , TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT) , 0) AS WEIGTH,  " +
                            "NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT,( AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) ) / (SAP_MA.UMREN / SAP_MA.UMREZ)  AS PACK_CNT , " +
                            "TMS_OUT_RESULT.BI_NUM, NVL(SAP_CUS.MOD_NUMBER, SAP_CUS.TEL_NUMBER_1) AS TEL_NUMBER_1, SAP_CUS.RLTYP, TMS_OUT_RESULT.ZERO_W, TMS_OUT_RESULT.BEFORE_WEIGHT,  " +
                            "TO_CHAR(BEFORE_WEIGHT_TIME, 'YYYY-MM-DD HH24:MI') AS BEFORE_WEIGHT_TIME, TO_CHAR(WEIGHT_TIME, 'YYYY-MM-DD HH24:MI') AS WEIGHT_TIME, ADM.DISPATCHMEMO   " +
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
                    string sMemo = Dbconn.conn.getData(reportDs, "DISPATCHMEMO", 0);

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
                    if (double.TryParse(sInWeight, out _))
                    {
                        printSheet.Document.Worksheets[0].Cells["O6"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                        printSheet.Document.Worksheets[0].Cells["O25"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                    }

                    //출차시간
                    printSheet.Document.Worksheets[0].Cells["E7"].Value = sOutWeightTime;
                    printSheet.Document.Worksheets[0].Cells["E26"].Value = sOutWeightTime;

                    //출차중량
                    if (double.TryParse(sOutWeight, out _))
                    {
                        printSheet.Document.Worksheets[0].Cells["O7"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));
                        printSheet.Document.Worksheets[0].Cells["O26"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));
                    }

                    //거래처
                    printSheet.Document.Worksheets[0].Cells["E8"].Value = sCustNm;
                    printSheet.Document.Worksheets[0].Cells["E27"].Value = sCustNm;

                    int intPweight = 0;
                    string sPweight = "0";

                    SQL = $@"
                        SELECT ING.WEIGHT_TYPE
                        FROM WAP_GOCARD WGD
                        JOIN INGRED ING 
                          ON WGD.MATNR = ING.RESOURCE_NO
                        WHERE WGD.IS_NO = '{is_no}'
                        ";

                    DataSet dsW = Dbconn.conn.ExecutDataset(SQL);

                    //피무계합산
                    if (Dbconn.conn.getData(dsW, "WEIGHT_TYPE", 0) == "02")
                    {
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
                        sPweight = Dbconn.conn.getData(pWeightDs, "P_SUM", 0);

                        if (new string[] { "P101", "P102", "PJ01", "PJ02" }.Contains(Factory))
                            if (Convert.ToDecimal(sPweight) > 0)
                                sPweight = Convert.ToString(IntRound(Convert.ToDouble(sPweight), -1));
                            else if (new string[] { "PJ04", "PJ05" }.Contains(Factory))
                                if (Convert.ToDecimal(sPweight) > 0)
                                    sPweight = Convert.ToString(Math.Ceiling(Convert.ToDouble(sPweight) / 10.0) * 10);
                                else
                                if (Convert.ToDecimal(sPweight) > 0)
                                    sPweight = Convert.ToDouble(sPweight).ToString();

                        intPweight = Convert.ToInt32(sPweight);

                        //송장량
                        if (double.TryParse(sPweight, out _))
                        {
                            if (Convert.ToDecimal(sPweight) > 0)
                            {
                                intPweight = Convert.ToInt32(sPweight);
                                //피중량
                                printSheet.Document.Worksheets[0].Cells["O8"].Value = intPweight;
                                printSheet.Document.Worksheets[0].Cells["O27"].Value = intPweight;
                            }
                        }
                    }

                    //배송처
                    printSheet.Document.Worksheets[0].Cells["E9"].Value = sCusAddr;
                    printSheet.Document.Worksheets[0].Cells["E28"].Value = sCusAddr;

                    //실중량
                    if (double.TryParse(sWeigth, out _))
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
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex);
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex.StackTrace);
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex.Source);
            }

        }

        public static void PrintWeighingSheet2(string is_no)
        {
            try
            {
                string SQL = string.Empty;

                string carInType = clsCarUtil.returnCarType(is_no);
                string carType = clsCarUtil.returnCarGubun2(is_no);

                //원료, 기타차량만
                if (carInType == "001" || carInType == "007")
                {
                    //SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                    //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\weight.xlsx", DocumentFormat.Xlsx);

                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();

                    string excelPath = FindExcelFormFolder();
                    string filePath = null;

                    if (!string.IsNullOrEmpty(excelPath))
                    {
                        filePath = Path.Combine(excelPath, "WeighingSheet.xlsx");
                    }

                    printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);

                    /*dt.Rows.Add("P101", "김제하림");
                    dt.Rows.Add("P102", "정읍하림");
                    dt.Rows.Add("P201", "상주올품");
                    dt.Rows.Add("PJ01", "대전제일사료");
                    dt.Rows.Add("PJ04", "함안제일사료");*/

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
                          $"FROM WAP_DECAR WHERE PC_STATUS IN ('20', '9') AND IS_NO = '{is_no}'  ";

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


                    string sDescName = string.Empty;

                    string sCustNm = string.Empty;
                    string sPlanQty = string.Empty;

                    int forCnt = 1;

                    SQL = "SELECT WGD.R_GR_QNTY " +
                        "FROM WAP_GOCARD WGD  " +
                        "JOIN INGRED ING ON WGD.MATNR = ING.RESOURCE_NO " +
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
                        if (double.TryParse(sInWeight, out _))
                        {
                            printSheet.Document.Worksheets[0].Cells["O6"].Value = String.Format("{0:#,###}", Convert.ToInt32(sInWeight));
                        }

                        //출차시간
                        printSheet.Document.Worksheets[0].Cells["E7"].Value = sOutWeightTime;

                        //출차중량
                        if (double.TryParse(sOutWeight, out _))
                        {
                            printSheet.Document.Worksheets[0].Cells["O7"].Value = String.Format("{0:#,###}", Convert.ToInt32(sOutWeight));
                        }

                        //거래처
                        printSheet.Document.Worksheets[0].Cells["E8"].Value = sCustNm;

                        string sPweight = "0";
                        int intPweight = 0;

                        SQL = $@"
                        SELECT ING.WEIGHT_TYPE
                        FROM WAP_GOCARD WGD
                        JOIN INGRED ING 
                          ON WGD.MATNR = ING.RESOURCE_NO
                        WHERE WGD.IS_NO = '{is_no}'
                        ";

                        DataSet dsW = Dbconn.conn.ExecutDataset(SQL);

                        //피무계합산
                        if (carInType == "001")
                        {
                            SQL = $"SELECT NVL(SUM(WEIGHT * PD_QTY),0) AS P_SUM FROM WAP_IN_ADD WHERE IS_NO = '{is_no}'";
                            DataSet pWeightDs = Dbconn.conn.ExecutDataset(SQL);

                            sPweight = Dbconn.conn.getData(pWeightDs, "P_SUM", 0);

                            if (Convert.ToDecimal(sPweight) > 0)
                            {
                                if (Factory == "P201")
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
                        if (double.TryParse(sWeigth, out _))
                        {
                            printSheet.Document.Worksheets[0].Cells["O9"].Value = String.Format("{0:#,###}", Convert.ToDecimal(sWeigth) - (decimal)intPweight);
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

                    } //for

                    invoiceCntDs.Dispose();
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet2", ex);
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet2", ex.StackTrace);
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet2", ex.Source);
            }

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


                //SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\form_m.xlsx", DocumentFormat.Xlsx);
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();

                string excelPath = FindExcelFormFolder();
                string filePath = null;

                if (!string.IsNullOrEmpty(excelPath))
                {
                    filePath = Path.Combine(excelPath, "weight_ETC.xlsx");
                }

                printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);

                //계량번호
                printSheet.Document.Worksheets[0].Cells["O4"].Value = is_no;

                SQL = $@"
                SELECT   NVL(CARM.VEHICLENO, WD.INCAR_NO) AS VEHICLENO
                       , CARM.DRIVERNAME
                       , CARM.DRIVERMOBILE
                       , CARM.CARRIERNAME
                FROM     WAP_DECAR WD
                LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CARM
                       ON WD.INCAR_NO = CARM.VEHICLECODE
                WHERE    WD.IS_NO = '{is_no}'
                ";

                DataSet carDs = Dbconn.conn.ExecutDataset(SQL);
                if (Dbconn.conn.getRowCnt(carDs) > 0)
                {
                    printSheet.Document.Worksheets[0].Cells["E4"].Value = Dbconn.conn.getData(carDs, "VEHICLENO", 0); //차량번호
                    printSheet.Document.Worksheets[0].Cells["E7"].Value = Dbconn.conn.getData(carDs, "CARRIERNAME", 0); //배송거래처
                    printSheet.Document.Worksheets[0].Cells["E9"].Value = Dbconn.conn.getData(carDs, "DRIVERMOBILE", 0) + " (" + Dbconn.conn.getData(carDs, "DRIVERNAME", 0) + ")"; //기사연락처
                }

                carDs.Dispose();

                SQL = $@"
                SELECT   INCAR_NO
                       , IN_WEIGHT
                       , IN_WEIGHT
                       , TO_CHAR(INCAR_DATE, 'YYYY-MM-DD HH24:MI') AS INCAR_DATE
                       , OUT_WEIGHT
                       , TO_CHAR(OUTCAR_DATE, 'YYYY-MM-DD HH24:MI') AS OUTCAR_DATE
                       , ABS(IN_WEIGHT - OUT_WEIGHT) AS REAL_WEIGHT
                       , ETC_DETAIL
                FROM     WAP_DECAR
                WHERE    PC_STATUS = '20'
                AND      IS_NO = '{is_no}'
                ";

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

                if (double.TryParse(Convert.ToString(sPweight), out double temp))
                {
                    if (Convert.ToDecimal(sPweight) > 0)
                    {
                        if (factory == "PJ04") //함안제일사료 피무계 올림처림
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
                if (double.TryParse(Convert.ToString(sWeigth), out temp))
                {
                    printSheet.Document.Worksheets[0].Cells["O8"].Value = String.Format("{0:#,###}", Convert.ToDecimal(sWeigth) - intPweight);
                }


                if (carInType == "002" || carInType == "003")
                {
                    SQL = $@"
                    SELECT   AD.PLANTCODE
                           , ADM.DELIVERYDATE
                           , AD.FROMLOCATIONCODE
                           , AD.TOLOCATIONCODE
                           , ADM.VEHICLECODE
                           , CAR.VEHICLENO
                           , SAP_CUS.NAME_ORG1
                           , ADM.DISPATCHNO
                           , AD.ORDERNO
                           , AD.ORDERLINENO
                           , ADM.DRIVERNAME
                           , ADM.DRIVERMOBILE
                           , AD.ITEMCODE
                           , SAP_PRO.DESCRIPTION
                           , AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY
                           , (SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE
                           , SAP_PRO.UOM
                           , NVL(TMS_OUT_RESULT.PD_YN, 'N') AS PD_YN
                           , NVL(DECODE(SAP_PRO.UOM, 'EA', TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT), 0) AS WEIGHT2
                           , NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT
                           , (AD.PLANQTY * (SAP_MA.UMREN / SAP_MA.UMREZ)) / (SAP_MA.UMREN / SAP_MA.UMREZ) AS PACK_CNT
                           , DECODE((SAP_MA.UMREN / SAP_MA.UMREZ), 1, '타이콘', '지대') AS PACK_TYPE
                           , TMS_OUT_RESULT.QTY
                           , TMS_OUT_RESULT.WEIGHT
                    FROM     TMS_INPUT_PLOADM_CON ADM
                    JOIN     TMS_INPUT_PLOADD_CON AD
                           ON ADM.DISPATCHNO = AD.DISPATCHNO
                    LEFT OUTER JOIN TMS_INPUT_CARMASTER_CON CAR
                           ON ADM.VEHICLECODE = CAR.VEHICLECODE
                    LEFT OUTER JOIN (
                               SELECT   RESOURCE_NO
                                      , DESCRIPTION
                                      , UOM
                               FROM     SAP_DI_PRODUCT
                               GROUP BY RESOURCE_NO, DESCRIPTION, UOM
                           ) SAP_PRO
                           ON AD.ITEMCODE = SAP_PRO.RESOURCE_NO
                    LEFT OUTER JOIN SAP_DI_CUSTOMER SAP_CUS
                           ON AD.TOLOCATIONCODE = SAP_CUS.PARTNER
                    LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT
                           ON AD.DISPATCHNO = TMS_OUT_RESULT.DISPATCHNO
                          AND AD.ORDERNO = TMS_OUT_RESULT.ORDERNO
                          AND AD.ORDERLINENO = TMS_OUT_RESULT.ORDERLINENO
                    LEFT OUTER JOIN SAP_MARM SAP_MA
                           ON AD.ITEMCODE = SAP_MA.MATNR
                          AND SAP_MA.MEINH = 'KG'
                    WHERE    TMS_OUT_RESULT.IS_NO = '{is_no}'
                    ORDER BY ADM.DELIVERYDATE DESC
                           , ADM.DISPATCHNO
                           , AD.ORDERNO
                           , AD.ORDERLINENO
                    ";
                }
                else if (carInType == "004" || carInType == "005")
                {
                    SQL = $@"
                    SELECT   SAP_SM.TRAID AS VEHICLENO
                            , SAP_SM.WERKS_GR AS PLANTCODE
                            , SAP_PLANT.P_DESCRIPTION AS NAME_ORG1
                            , SAP_SM.TRANS_DATE AS DELIVERYDATE
                            , SAP_SD.TKNUM AS DISPATCHNO
                            , SAP_SD.VBELN AS ORDERNO
                            , SAP_SD.POSNR AS ORDERLINENO
                            , SAP_SD.MATNR AS ITEMCODE
                            , SAP_PRO.DESCRIPTION
                            , SAP_SD.LFIMG * (SAP_MA.UMREN / SAP_MA.UMREZ) AS PLANQTY
                            , (SAP_MA.UMREN / SAP_MA.UMREZ) AS STD_LOT_SIZE
                            , SAP_PRO.UOM
                            , NVL(TMS_OUT_RESULT.PD_YN, 'N') AS PD_YN
                            , NVL(DECODE(SAP_PRO.UOM, 'EA', TMS_OUT_RESULT.QTY, TMS_OUT_RESULT.WEIGHT), 0) AS WEIGHT
                            , NVL(TMS_OUT_RESULT.BAG_WEIGHT, 0) AS BAG_WEIGHT
                            , SAP_SD.LFIMG AS PACK_CNT
                            , DECODE((SAP_MA.UMREN / SAP_MA.UMREZ), 1, '타이콘', '지대') AS PACK_TYPE
                            , TMS_OUT_RESULT.QTY
                            , TMS_OUT_RESULT.WEIGHT
                    FROM     SAP_INPUT_PROTRANSM SAP_SM
                    JOIN     SAP_INPUT_PROTRANSD SAP_SD
                            ON SAP_SM.TKNUM = SAP_SD.TKNUM
                    LEFT OUTER JOIN SAP_DI_PRODUCT SAP_PRO
                            ON SAP_SD.MATNR = SAP_PRO.RESOURCE_NO
                            AND SAP_SM.WERKS_GR = SAP_PRO.PLANT_CODE
                    LEFT OUTER JOIN TMS_OUTPUT_RESULT TMS_OUT_RESULT
                            ON SAP_SM.TKNUM = TMS_OUT_RESULT.DISPATCHNO
                            AND SAP_SD.VBELN = TMS_OUT_RESULT.ORDERNO
                            AND SAP_SD.POSNR = TMS_OUT_RESULT.ORDERLINENO
                    LEFT OUTER JOIN SAP_MARM SAP_MA
                            ON SAP_SD.MATNR = SAP_MA.MATNR
                            AND SAP_MA.MEINH = 'KG'
                    LEFT OUTER JOIN SAP_DI_PLANT SAP_PLANT
                            ON SAP_SM.WERKS_GR = SAP_PLANT.PLANT_CODE
                    WHERE    TMS_OUT_RESULT.IS_NO = '{is_no}'
                    ORDER BY SAP_SD.TKNUM
                            , SAP_SD.VBELN
                            , SAP_SD.POSNR
                    ";
                }
                else
                {
                    SQL = $@"
                    SELECT 
                        a.IS_NO, 
                        a.VEHICLEGROUPCODE,  
                        a.INCAR_NO, 
                        a.ETC_DETAIL AS DESCRIPTION,
                        b.I_TIME AS CHKIN_DATE,   
                        CEIL(a.IN_WEIGHT) AS IN_WEIGHT,  
                        CEIL(a.OUT_WEIGHT) AS OUT_WEIGHT, 
                        a.PC_STATUS, 
                        TO_CHAR(a.INCAR_DATE, 'YYYY-MM-DD HH24:MI:SS') AS INCAR_DATE,  
                        TO_CHAR(a.OUTCAR_DATE, 'YYYY-MM-DD HH24:MI:SS') AS OUTCAR_DATE, 
                        CEIL(a.IN_WEIGHT - a.OUT_WEIGHT) AS WEIGHT, 
                        ABS(CEIL(a.IN_WEIGHT - a.OUT_WEIGHT)) AS REAL_WEIGHT,
                        a.PC_STATUS,
                        a.I_USER,
                        c.DRIVERNAME, c.DRIVERMOBILE
                    FROM WAP_DECAR a
                        LEFT JOIN TMS_OUTPUT_RESULT  b ON b.IS_NO = a.IS_NO
                        LEFT JOIN TMS_INPUT_CARMASTER_CON c ON c.VEHICLENO = a.INCAR_NO
                    WHERE
                        a.IS_NO = '{is_no}'
                    ORDER BY 
                        a.IS_NO
                    ";
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
                SQL = "SELECT PM.PTMCDNM, WOA.WEIGHT * PD_QTY AS SUM_QTY   " +
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
                            printSheet.Document.Worksheets[0].Cells["C36"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
                        }

                        if (p == 1)
                        {
                            printSheet.Document.Worksheets[0].Cells["D35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["D36"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
                        }

                        if (p == 2)
                        {
                            printSheet.Document.Worksheets[0].Cells["G35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["G36"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
                        }

                        if (p == 3)
                        {
                            printSheet.Document.Worksheets[0].Cells["I35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["I36"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
                        }

                        if (p == 4)
                        {
                            printSheet.Document.Worksheets[0].Cells["M35"].Value = Dbconn.conn.getData(custPaInputDs, "PTMCDNM", p);
                            printSheet.Document.Worksheets[0].Cells["M36"].Value = Dbconn.conn.getData(custPaInputDs, "SUM_QTY", p);
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
                clsLog.logSave("clsPrintExcel", "PrintChulgoSheet", ex);
                clsLog.logSave("clsPrintExcel", "PrintChulgoSheet", ex.StackTrace);
                clsLog.logSave("clsPrintExcel", "PrintChulgoSheet", ex.Source);
            }
        }

        internal static void PrintMainIngredWorkLog(string sWorkDate, DataSet dsMain)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\부원료투입일지.xlsx", DocumentFormat.Xlsx);

                string excelPath = FindExcelFormFolder();
                string filePath = null;

                if (!string.IsNullOrEmpty(excelPath))
                {
                    filePath = Path.Combine(excelPath, "MainIngredWorkLog.xlsx");
                }

                printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);

                string SQL = $@"
                SELECT  bii.RESOURCE_NO                                           -- 품명
                    , p.DESCRIPTION                                               -- 품명   
                    , bii.INPUT_DATE                                              -- 입고일
                    , TO_CHAR(bii.INPUT_START_TIME, 'HH24:MI:SS') AS INPUT_START_TIME         -- 투입시간
                    , TO_CHAR(bii.INPUT_END_TIME, 'HH24:MI:SS') AS INPUT_END_TIME         -- 투입시간
                    , bii.INPUT_LOCATION                                          -- 투입장소
                    , sdl.DESCRIPTION AS LOCATION_DESC                                    -- 투입장소
                    , bii.INPUT_QTY                                               -- 투입량
                    , bii.WEIGHT                                                   -- 중량
                    , bii.CAR_NO                                                   -- 차량번호
                    , bii.B_W                                                     -- 비중
                    , bii.REMARKS                                                 -- 비고
                    , bii.I_USER                                                  -- 근무자
                    , d.NAME
                FROM BIN_INGRED_INPUT bii
                    LEFT JOIN SAP_DI_PRODUCT p ON p.PLANT_CODE = bii.PLANT_CODE AND p.RESOURCE_NO = bii.RESOURCE_NO
                    LEFT JOIN SAP_DI_LOCATION sdl ON sdl.PLANT_CODE = bii.PLANT_CODE AND sdl.LOCATION = bii.INPUT_LOCATION
                    LEFT JOIN DO_INSA d ON d.EMPLOYEE_NO = bii.I_USER
                WHERE bii.PLANT_CODE = '{clsCommon.PlantCode}'
                    AND bii.INPUT_DATE = '{Convert.ToDateTime(sWorkDate).ToString("yyyyMMdd")}'
                ";

                printSheet.SetCellValue(0, "A8", "작성일 :  " + Convert.ToDateTime(sWorkDate).ToString("yyyy년 MM월 dd일 ddd요일", new System.Globalization.CultureInfo("ko-KR")));

                for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                {

                    printSheet.SetCellValue(0, "B" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, "E" + (i + 11).ToString(), String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(dsMain, "WEIGHT", i))));
                    printSheet.SetCellValue(0, "G" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "CAR_NO", i));

                    printSheet.SetCellValue(0, "I" + (i + 11).ToString(), String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(dsMain, "B_W", i))));
                    printSheet.SetCellValue(0, "J" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "INPUT_START_TIME", i) + " ~ " + Dbconn.conn.getData(dsMain, "INPUT_END_TIME", i));

                    printSheet.SetCellValue(0, "M" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "LOCATION_DESC", i));
                    printSheet.SetCellValue(0, "O" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "NAME", i));
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintSubIngredReport", ex);
            }
        }

        internal static void PrintSubIngredWorkLog(string sWorkDate, DataSet dsMain)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\부원료투입일지.xlsx", DocumentFormat.Xlsx);

                string excelPath = FindExcelFormFolder();
                string filePath = null;

                if (!string.IsNullOrEmpty(excelPath))
                {
                    filePath = Path.Combine(excelPath, "SubIngredWorkLog.xlsx");
                }

                printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);

                printSheet.SetCellValue(0, "A8", "작성일 :  " + Convert.ToDateTime(sWorkDate).ToString("yyyy년 MM월 dd일 ddd요일", new System.Globalization.CultureInfo("ko-KR")));

                for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                {
                    printSheet.SetCellValue(0, "B" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, "D" + (i + 11).ToString(), string.IsNullOrEmpty(Dbconn.conn.getData(dsMain, "INPUT_DATE", i)) ? "" : DateTime.ParseExact(Dbconn.conn.getData(dsMain, "INPUT_DATE", i), "yyyyMMdd", null).ToString("yyyy-MM-dd"));
                    printSheet.SetCellValue(0, "E" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "INPUT_START_TIME", i) + " ~ " + Dbconn.conn.getData(dsMain, "INPUT_END_TIME", i));
                    printSheet.SetCellValue(0, "G" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "LOCATION_DESC", i));

                    string value = Dbconn.conn.getData(dsMain, "INPUT_QTY", i)?.ToString();

                    int qty = string.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value);

                    string result = String.Format("{0:#,###}", qty);

                    printSheet.SetCellValue(0, "H" + (i + 11).ToString(), result);
                    printSheet.SetCellValue(0, "J" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "REMARKS", i));
                    printSheet.SetCellValue(0, "O" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "NAME", i));
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintSubIngredReport", ex);
            }
        }

        internal static void PrintSubIngredReceivingInsp(string sWorkDate, DataSet dsMain, DataSet dsSub)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\부원료투입일지.xlsx", DocumentFormat.Xlsx);

                string excelPath = FindExcelFormFolder();
                string filePath = null;

                if (!string.IsNullOrEmpty(excelPath))
                {
                    filePath = Path.Combine(excelPath, "SubIngredReceivingInsp.xlsx");
                }

                printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);

                printSheet.SetCellValue(0, "A8", "작성일 :  " + Convert.ToDateTime(sWorkDate).ToString("yyyy년 MM월 dd일 ddd요일", new System.Globalization.CultureInfo("ko-KR")));

                int n;

                for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                {
                    // 원료명
                    printSheet.SetCellValue(0, "B" + (i + 10).ToString(), Dbconn.conn.getData(dsMain, "DESCRIPTION", i));

                    // 모선명
                    printSheet.SetCellValue(0, "D" + (i + 10).ToString(), Dbconn.conn.getData(dsMain, "PARTNER_DESC", i));

                    // 중량
                    printSheet.SetCellValue(0, "F" + (i + 10).ToString(), int.TryParse(Dbconn.conn.getData(dsMain, "R_GR_QNTY", i), out n) ? n.ToString("#,###") : "0");
                    //printSheet.SetCellValue(0, "E" + (i + 10).ToString(), Dbconn.conn.getData(dsMain, "INPUT_START_TIME", i) + " ~ " + Dbconn.conn.getData(dsMain, "INPUT_END_TIME", i));

                    // 차량번호
                    printSheet.SetCellValue(0, "G" + (i + 10).ToString(), Dbconn.conn.getData(dsMain, "INCAR_NO", i));
                     
                    // 비중
                    printSheet.SetCellValue(0, "H" + (i + 10).ToString(), Dbconn.conn.getData(dsMain, "B_W", i));
                    // 외관검사
                    printSheet.SetCellValue(0, "J" + (i + 10).ToString(), Dbconn.conn.getData(dsMain, "VINSPECTION", i));
                    // 비고
                    printSheet.SetCellValue(0, "L" + (i + 10).ToString(), Dbconn.conn.getData(dsMain, "REMARKS", i));
                }

                printSheet.SetCellValue(1, "A8", "작성일 :  " + Convert.ToDateTime(sWorkDate).ToString("yyyy년 MM월 dd일 ddd요일", new System.Globalization.CultureInfo("ko-KR")));

                for (int i = 0; i < dsSub.Tables[0].Rows.Count; i++)
                {
                    printSheet.SetCellValue(1, "B" + (i + 10).ToString(), Dbconn.conn.getData(dsSub, "DESCRIPTION", i));

                    printSheet.SetCellValue(1, "D" + (i + 10).ToString(), Dbconn.conn.getData(dsSub, "CHARG_TEXT", i));

                    printSheet.SetCellValue(1, "F" + (i + 10).ToString(), int.TryParse(Dbconn.conn.getData(dsSub, "R_GR_QNTY", i), out n) ? n.ToString("#,###") : "0");
                    //printSheet.SetCellValue(0, "E" + (i + 10).ToString(), Dbconn.conn.getData(dsMain, "INPUT_START_TIME", i) + " ~ " + Dbconn.conn.getData(dsMain, "INPUT_END_TIME", i));
                    printSheet.SetCellValue(1, "G" + (i + 10).ToString(), Dbconn.conn.getData(dsSub, "INCAR_NO", i));

                    printSheet.SetCellValue(1, "H" + (i + 10).ToString(), Dbconn.conn.getData(dsSub, "VINSPECTION", i));
                    printSheet.SetCellValue(1, "I" + (i + 10).ToString(), Dbconn.conn.getData(dsSub, "MFG_DATE", i));
                    printSheet.SetCellValue(1, "J" + (i + 10).ToString(), Dbconn.conn.getData(dsSub, "EXP_DATE", i));
                    printSheet.SetCellValue(1, "K" + (i + 10).ToString(), Dbconn.conn.getData(dsSub, "REMARKS", i));
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintSubIngredReceivingInsp", ex);
            }
        }

        internal static void PrintDosingWorkLog(string sWorkDate, DataSet dsMain)
        {
            try
            {
                SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                //printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\report\\부원료투입일지.xlsx", DocumentFormat.Xlsx);

                string excelPath = FindExcelFormFolder();
                string filePath = null;

                if (!string.IsNullOrEmpty(excelPath))
                {
                    filePath = Path.Combine(excelPath, "DosingWorkLog.xlsx");
                }

                printSheet.LoadDocument($"{filePath}", DocumentFormat.Xlsx);

                printSheet.SetCellValue(0, "A8", "작성일 :  " + Convert.ToDateTime(sWorkDate).ToString("yyyy년 MM월 dd일 ddd요일", new System.Globalization.CultureInfo("ko-KR")));
                printSheet.SetCellValue(0, "U8", "(근무시간 :              ~             )");

                int n;

                for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                {
                    if (i == 25)
                        break;

                    printSheet.SetCellValue(0, "E" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "DESCRIPTION", i));
                    printSheet.SetCellValue(0, "J" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "BATCH", i));
                    printSheet.SetCellValue(0, "L" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "NOTE", i));

                    printSheet.SetCellValue(0, "Q" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "START_TIME", i) + " ~ " + Dbconn.conn.getData(dsMain, "END_TIME", i));

                    printSheet.SetCellValue(0, "U" + (i + 11).ToString(), int.TryParse(Dbconn.conn.getData(dsMain, "BBATCH_Q", i), out n) ? n.ToString("#,###") : "0");

                    printSheet.SetCellValue(0, "W" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "DOSING_YN", i));
                    printSheet.SetCellValue(0, "Y" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "VINSPECTION", i));

                    printSheet.SetCellValue(0, "AA" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "PELLET_BIN", i));
                    printSheet.SetCellValue(0, "AC" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "BIN", i));

                    printSheet.SetCellValue(0, "AD" + (i + 11).ToString(), int.TryParse(Dbconn.conn.getData(dsMain, "ERP_Q", i), out n) ? n.ToString("#,###") : "0");

                    printSheet.SetCellValue(0, "AE" + (i + 11).ToString(), Dbconn.conn.getData(dsMain, "REMARK", i));
                }

                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                printSheet.Options.Print.ShowMarginsWarning = false;
                printSheet.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintReport", "PrintDosingWorkLog", ex);
            }
        }
    }
}
