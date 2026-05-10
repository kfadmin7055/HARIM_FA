using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using System;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Core.Enum;

namespace Core.Class
{
    public class clsPrintExcel
    {
        //상차지시서 발행
        public static void PrintDeliverySheet(string is_no)
        {
            try
            {
                string SQL = string.Empty;

                SQL = "SELECT IS_NO, INCAR_NO, D_HP from WAP_DECAR where IS_NO = '{0}' ";
                SQL = string.Format(SQL, is_no);
                DataSet decarDs = Dbconn.conn.ExecutDataset(SQL);

                string full_car_num = Dbconn.conn.getData(decarDs, "INCAR_NO", 0).Trim();
                string d_hp = Dbconn.conn.getData(decarDs, "D_HP", 0).Trim();

                string del_emp = string.Empty;
                string del_tel = string.Empty;
                string del_type = string.Empty;

                if (full_car_num.Length == 4)
                {
                    //SQL = "SELECT VEHICLENO ,DRIVERNAME, DRIVERMOBILE, VEHICLEGROUPCODE FROM ERP_DBLINK.ATN_DST.DBO.V_MES_ATG_113_1 WHERE REPLACE(VEHICLENO, ' ', '') = '8632' ";
                }
                else
                {
                    DataSet ccarInfoDs = clsCarCommon.GetCarMaster(Regex.Replace(full_car_num, @"\D", ""));

                    del_emp = Dbconn.conn.getData(ccarInfoDs, "DRIVERNAME", 0);
                    del_tel = Dbconn.conn.getData(ccarInfoDs, "DRIVERMOBILE", 0);
                    del_type = Dbconn.conn.getData(ccarInfoDs, "VEHICLEGROUPCODE", 0);
                }

                if (d_hp.Length > 7)
                {
                    d_hp = d_hp.Replace("-", "");
                    del_emp = string.Empty;
                    del_tel = d_hp.Substring(0, 3) + "-" + d_hp.Substring(3, 4) + "-" + d_hp.Substring(7);
                }
 
                SQL = $@"
                SELECT TRIM(CUST_NO) as CUST_NO
                FROM WAP_DECAR WD
                    INNER JOIN BULK_ORDER BO ON WD.IS_NO = BO.IS_NO 
                    LEFT OUTER JOIN BIN B ON BO.LOCATION = B.LOCATION 
                WHERE WD.IS_NO = '{is_no}'
                GROUP BY TRIM(CUST_NO)
                ";

                SQL = string.Format(SQL, is_no);
                DataSet custListDs = Dbconn.conn.ExecutDataset(SQL);

                for (int j = 0; j < Dbconn.conn.getRowCnt(custListDs); j++)
                {

                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                    printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\sang.xlsx", DocumentFormat.Xlsx);

                    //NO
                    printSheet.Document.Worksheets[0].Cells["G5"].Value = DateTime.Now.ToShortDateString();
                    printSheet.Document.Worksheets[0].Cells["AC5"].Value = DateTime.Now.ToShortDateString();

                    SQL = "SELECT TOP 1 DELIVERY_NO, ORDER_NO FROM BULK_ORDER WHERE IS_NO = '{0}' ";
                    SQL = string.Format(SQL, is_no);

                    DataSet deliveryNoDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(deliveryNoDs) > 0)
                    {
                        printSheet.Document.Worksheets[0].Cells["P5"].Value = Dbconn.conn.getData(deliveryNoDs, "DELIVERY_NO", 0);
                        printSheet.Document.Worksheets[0].Cells["AL5"].Value = Dbconn.conn.getData(deliveryNoDs, "DELIVERY_NO", 0);
                    }

                    printSheet.Document.Worksheets[0].Cells["G7"].Value = full_car_num;
                    printSheet.Document.Worksheets[0].Cells["AC7"].Value = full_car_num;

                    printSheet.Document.Worksheets[0].Cells["P7"].Value = del_type;
                    printSheet.Document.Worksheets[0].Cells["AL7"].Value = del_type;

                    printSheet.Document.Worksheets[0].Cells["G8"].Value = del_emp;
                    printSheet.Document.Worksheets[0].Cells["AC8"].Value = del_emp;

                    printSheet.Document.Worksheets[0].Cells["P8"].Value = del_tel;
                    printSheet.Document.Worksheets[0].Cells["AL8"].Value = del_tel;


                    //SQL = @"
                    //SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS ROWNUM,
                    //BO.CUST_NAME, BO.PART_NAME, ERP_ING.UOM,  BO.REMARK, 
                    //CASE RIGHT(BO.PART_NO, 1) WHEN '3' THEN '벌크 ' + RIGHT(B.SCALE_CODE, 1) + '호기' 
                    //WHEN '2' THEN '전면 상차대'  WHEN '3' THEN '후면 상차대' 
                    //ELSE '제품상차장' END AS LOC  ,  
                    //CASE RIGHT(BO.PART_NO, 1) WHEN '1' THEN '지대' WHEN '2' THEN '톤백' WHEN '3' THEN '벌크' END PART_TYPE ,  
                    //BO.QUANTITY, FLOOR(BO.BATCH_Q / BO.QUANTITY) AS PART_CNT,  
                    //FLOOR(BO.BATCH_Q) AS BATCH_Q  
                    //FROM WAP_DECAR WD INNER JOIN BULK_ORDER BO ON WD.IS_NO = BO.IS_NO  
                    //LEFT OUTER JOIN BIN B ON BO.LOCATION = B.LOCATION  
                    //LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING ON BO.PART_NO = ERP_ING.RESOURCE_NO
                    //WHERE WD.IS_NO = '{0}'  AND TRIM(BO.CUST_NO) = '{1}' 
                    //ORDER BY BO.CUST_NAME, BO.PART_NAME   ";

                    SQL = $@"
                    SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS ROWNUM,
                        BO.CUST_NAME, BO.PART_NAME, ERP_ING.UOM,  BO.REMARK, 
                        CASE RIGHT(BO.PART_NO, 1) WHEN '3' THEN '벌크 ' + RIGHT(B.SCALE_CODE, 1) + '호기' 
                                                    WHEN '2' THEN '전면 상차대'  
                                                    WHEN '3' THEN '후면 상차대' 
                            ELSE '제품상차장' END AS LOC,  
                        CASE RIGHT(BO.PART_NO, 1) WHEN '1' THEN '지대' 
                                                    WHEN '2' THEN '톤백'
                                                    WHEN '3' THEN '벌크'
                            END PART_TYPE,  
                        BO.QUANTITY, FLOOR(BO.BATCH_Q / BO.QUANTITY) AS PART_CNT,  
                        FLOOR(BO.BATCH_Q) AS BATCH_Q  
                    FROM WAP_DECAR WD
                        INNER JOIN BULK_ORDER BO ON WD.IS_NO = BO.IS_NO  
                        LEFT OUTER JOIN BIN B ON BO.LOCATION = B.LOCATION  
                        LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING ON BO.PART_NO = ERP_ING.RESOURCE_NO
                    WHERE WD.IS_NO = '{is_no}'  AND TRIM(BO.CUST_NO) = '{Dbconn.conn.getData(custListDs, "CUST_NO", j)}' 
                    ORDER BY BO.CUST_NAME, BO.PART_NAME";

                    DataSet itemListDs = Dbconn.conn.ExecutDataset(SQL);

                    int sum_cnt = 0;
                    int sum_weight = 0;

                    for (int i = 0; i < Dbconn.conn.getRowCnt(itemListDs); i++)
                    {
                        if (i > 19)
                        {
                            break;
                        }

                        string prt_remark = Dbconn.conn.getData(itemListDs, "REMARK", 0);
                        string prt_cust_name = string.Empty;

                        if (i == 0)
                        {
                            if (!string.IsNullOrEmpty(prt_remark) )
                            {
                                prt_cust_name = Dbconn.conn.getData(itemListDs, "CUST_NAME", 0) + "  (" + Dbconn.conn.getData(itemListDs, "REMARK", 0) + ")";
                            }
                            else
                            {
                                prt_cust_name = Dbconn.conn.getData(itemListDs, "CUST_NAME", 0);
                            }

                            printSheet.Document.Worksheets[0].Cells["G6"].Value = prt_cust_name;
                            printSheet.Document.Worksheets[0].Cells["AC6"].Value = prt_cust_name;
                        }

                        printSheet.Document.Worksheets[0].Cells["B" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "ROWNUM", i);
                        printSheet.Document.Worksheets[0].Cells["X" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "ROWNUM", i);

                        printSheet.Document.Worksheets[0].Cells["C" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "PART_NAME", i);
                        printSheet.Document.Worksheets[0].Cells["Y" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "PART_NAME", i);

                        printSheet.Document.Worksheets[0].Cells["K" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "LOC", i);
                        printSheet.Document.Worksheets[0].Cells["AG" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "LOC", i);

                        printSheet.Document.Worksheets[0].Cells["O" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "UOM", i);
                        printSheet.Document.Worksheets[0].Cells["AK" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "UOM", i);

                        printSheet.Document.Worksheets[0].Cells["M" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "PART_TYPE", i);
                        printSheet.Document.Worksheets[0].Cells["AI" + (i + 11).ToString()].Value = Dbconn.conn.getData(itemListDs, "PART_TYPE", i);

                        printSheet.Document.Worksheets[0].Cells["Q" + (i + 11).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "QUANTITY", i)));
                        printSheet.Document.Worksheets[0].Cells["AM" + (i + 11).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "QUANTITY", i)));

                        printSheet.Document.Worksheets[0].Cells["S" + (i + 11).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "BATCH_Q", i)));
                        printSheet.Document.Worksheets[0].Cells["AO" + (i + 11).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "BATCH_Q", i)));

                        sum_cnt += Convert.ToInt32(Dbconn.conn.getData(itemListDs, "QUANTITY", i));
                        sum_weight += Convert.ToInt32(Dbconn.conn.getData(itemListDs, "BATCH_Q", i));
                    }

                    printSheet.Document.Worksheets[0].Cells["Q31"].Value = String.Format("{0:#,###}", sum_cnt);
                    printSheet.Document.Worksheets[0].Cells["AM31"].Value = String.Format("{0:#,###}", sum_cnt);

                    printSheet.Document.Worksheets[0].Cells["S31"].Value = String.Format("{0:#,###}", sum_weight);
                    printSheet.Document.Worksheets[0].Cells["AO31"].Value = String.Format("{0:#,###}", sum_weight);

                    printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;

                    //DevExpress.Spreadsheet.Margins pageMargins = printSheet.Document.Worksheets[0].ActiveView.Margins;
                    //pageMargins.Left = 0.2F;
                    //pageMargins.Top = 0.2F;
                    //pageMargins.Right = 0;
                    //pageMargins.Bottom = 0;
                    //pageMargins.Header = 1;
                    //pageMargins.Footer = 0.4F;

                    //printSheet.ShowPrintPreview();
                    printSheet.Options.Print.ShowMarginsWarning = false;
                    printSheet.Print();


                }

                clsUtil.Delay(800);
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintExcel", "PrintDeliverySheet", ex);
                clsLog.logSave("clsPrintExcel", "PrintDeliverySheet", ex.StackTrace);
                clsLog.logSave("clsPrintExcel", "PrintDeliverySheet", ex.Source);
            }

        }

        //출고인수증 발행
        public static void PrintChulgoSheet(string is_no)
        {
            try
            {
                string SQL = string.Empty;

                SQL = $@"
                SELECT IS_NO, CUST_CODE, CAR_TDETAIL, VEHICLEGROUPCODE, INCAR_NO,
                    CONVERT(CHAR(10), INCAR_DATE, 23) as INCAR_DATE, INCAR_TIME,
                    OUTCAR_DATE, OUTCAR_TIME, IN_WEIGHT, OUT_WEIGHT
                FROM WAP_DECAR
                WHERE IS_NO = '{is_no}'
                ";

                DataSet weightDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(weightDs) == 0)
                {
                    return;
                }

                string car_tdetail = Dbconn.conn.getData(weightDs, "CAR_TDETAIL", 0);

                if (car_tdetail == "카고" || car_tdetail == "벌크")
                {
                    SQL = $@"
                    SELECT TRIM(BO.ORDER_NO) as ORDER_NO
                    FROM WAP_DECAR WD
                        INNER JOIN BULK_ORDER BO ON WD.IS_NO = BO.IS_NO  
                        LEFT OUTER JOIN BIN B ON BO.LOCATION = B.LOCATION  
                    WHERE WD.IS_NO = '{is_no}'
                        AND BO.C_CONDITION = '{clsCommon.PcStatus.Completed}'  
                        AND BO.PC_STATUS = '2'  
                    GROUP BY TRIM(BO.ORDER_NO)
                    ";

                    DataSet custListDs = Dbconn.conn.ExecutDataset(SQL);

                    string sTdetail = car_tdetail == "벌크" ? "AND RIGHT(BO.PART_NO, 1) IN ('1')" : "";

                    for (int i = 0; i < Dbconn.conn.getRowCnt(custListDs); i++)
                    {
                        //if (car_tdetail == "카고")
                        //{
                        //    SQL = @"
                        //    SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS ROWNUM,
                        //    BO.CUST_NAME, BO.CUST_NO, BO.PART_NAME, ERP_ING.UOM, BO.REMARK,
                        //    CASE RIGHT(BO.PART_NO, 1) WHEN '3' THEN '벌크 ' + RIGHT(B.SCALE_CODE, 1) + '호기' 
                        //    WHEN '2' THEN '전면 상차대'  WHEN '3' THEN '후면 상차대' 
                        //    ELSE '제품상차장' END AS LOC  ,  
                        //    CASE RIGHT(BO.PART_NO, 1) WHEN '1' THEN '지대' WHEN '2' THEN '톤백' WHEN '3' THEN '벌크' END PART_TYPE ,  
                        //    BO.QUANTITY, FLOOR(BO.BATCH_Q / BO.QUANTITY) AS PART_CNT,  
                        //    FLOOR(BO.BATCH_Q) AS BATCH_Q  
                        //    FROM WAP_DECAR WD INNER JOIN BULK_ORDER BO ON WD.IS_NO = BO.IS_NO  
                        //    LEFT OUTER JOIN BIN B ON BO.LOCATION = B.LOCATION  
                        //    LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING ON BO.PART_NO = ERP_ING.RESOURCE_NO
                        //    WHERE BO.PC_STATUS = '2'  
                        //    AND WD.IS_NO = '{0}'  AND TRIM(BO.ORDER_NO) = '{1}' 
                        //    AND BO.C_CONDITION = '{clsCommon.PcStatus.Completed}'   
                        //    ORDER BY BO.CUST_NAME, BO.PART_NAME   ";

                        //}else if (car_tdetail == "벌크")
                        //{
                        //    SQL = @"
                        //    SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS ROWNUM,
                        //    BO.CUST_NAME, BO.CUST_NO, BO.PART_NAME, ERP_ING.UOM, BO.REMARK, 
                        //    CASE RIGHT(BO.PART_NO, 1) WHEN '3' THEN '벌크 ' + RIGHT(B.SCALE_CODE, 1) + '호기' 
                        //    WHEN '2' THEN '전면 상차대'  WHEN '3' THEN '후면 상차대' 
                        //    ELSE '제품상차장' END AS LOC  ,  
                        //    CASE RIGHT(BO.PART_NO, 1) WHEN '1' THEN '지대' WHEN '2' THEN '톤백' WHEN '3' THEN '벌크' END PART_TYPE ,  
                        //    BO.QUANTITY, FLOOR(BO.BATCH_Q / BO.QUANTITY) AS PART_CNT,  
                        //    FLOOR(BO.BATCH_Q) AS BATCH_Q  
                        //    FROM WAP_DECAR WD INNER JOIN BULK_ORDER BO ON WD.IS_NO = BO.IS_NO  
                        //    LEFT OUTER JOIN BIN B ON BO.LOCATION = B.LOCATION  
                        //    LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING ON BO.PART_NO = ERP_ING.RESOURCE_NO
                        //    WHERE BO.PC_STATUS = '2'  
                        //    AND WD.IS_NO = '{0}'  AND TRIM(BO.ORDER_NO) = '{1}'
                               
                        //    AND BO.C_CONDITION = '{clsCommon.PcStatus.Completed}'   
                        //    ORDER BY BO.CUST_NAME, BO.PART_NAME   ";
                        //}

                        SQL = $@"
                            SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS ROWNUM,
                            BO.CUST_NAME, BO.CUST_NO, BO.PART_NAME, ERP_ING.UOM, BO.REMARK,
                            CASE RIGHT(BO.PART_NO, 1) WHEN '3' THEN '벌크 ' + RIGHT(B.SCALE_CODE, 1) + '호기' 
                            WHEN '2' THEN '전면 상차대'  WHEN '3' THEN '후면 상차대' 
                            ELSE '제품상차장' END AS LOC  ,  
                            CASE RIGHT(BO.PART_NO, 1) WHEN '1' THEN '지대' WHEN '2' THEN '톤백' WHEN '3' THEN '벌크' END PART_TYPE ,  
                            BO.QUANTITY, FLOOR(BO.BATCH_Q / BO.QUANTITY) AS PART_CNT,  
                            FLOOR(BO.BATCH_Q) AS BATCH_Q  
                            FROM WAP_DECAR WD INNER JOIN BULK_ORDER BO ON WD.IS_NO = BO.IS_NO  
                            LEFT OUTER JOIN BIN B ON BO.LOCATION = B.LOCATION  
                            LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING ON BO.PART_NO = ERP_ING.RESOURCE_NO
                            WHERE BO.PC_STATUS = '2'  
                            AND WD.IS_NO = '{is_no}'  AND TRIM(BO.ORDER_NO) = '{Dbconn.conn.getData(custListDs, "ORDER_NO", i)}' 
                            {sTdetail}
                            AND BO.C_CONDITION = '{clsCommon.PcStatus.Completed}'   
                            ORDER BY BO.CUST_NAME, BO.PART_NAME   ";

                        DataSet itemListDs = Dbconn.conn.ExecutDataset(SQL);

                        string del_emp = string.Empty;
                        string del_tel = string.Empty;
                        string del_type = string.Empty;
                        string d_hp = string.Empty;

                        SQL = "select IS_NO, INCAR_NO, D_HP from WAP_DECAR where IS_NO = '{0}' ";
                        SQL = string.Format(SQL, is_no);
                        DataSet decarDs = Dbconn.conn.ExecutDataset(SQL);

                        string full_car_num = Dbconn.conn.getData(decarDs, "INCAR_NO", 0).Trim();
                        d_hp = Dbconn.conn.getData(decarDs, "D_HP", 0).Trim();

                        DataSet ccarInfoDs = clsCarCommon.GetCarMaster(full_car_num);

                        del_emp = Dbconn.conn.getData(ccarInfoDs, "DRIVERNAME", 0);
                        //del_tel = Dbconn.conn.getData(ccarInfoDs, "DRIVERMOBILE", 0);
                        del_type = Dbconn.conn.getData(ccarInfoDs, "VEHICLEGROUPCODE", 0);

                        //기사연락처 사용안함
/*                        if (d_hp.Length > 7)
                        {
                            d_hp = d_hp.Replace("-", "");
                            del_tel = d_hp.Substring(0, 3) + "-" + d_hp.Substring(3, 4) + "-" + d_hp.Substring(7);
                        }*/

                        int sum_cnt = 0;
                        int sum_weight = 0;

                        SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                        printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\chul.xlsx", DocumentFormat.Xlsx);


                        int rowNum = 0;


                        for (int j = 0; j < Dbconn.conn.getRowCnt(itemListDs); j++)
                        {
                            if (j == 9)
                            {
                                printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
                                printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\chul.xlsx", DocumentFormat.Xlsx);
                                rowNum = 0;
                            }

                            string prt_cust_name = Dbconn.conn.getData(itemListDs, "CUST_NAME", 0);
                            string prt_remark = Dbconn.conn.getData(itemListDs, "REMARK", 0);

                            if (j == 0 || j == 9)
                            {
                                //출고일자
                                printSheet.Document.Worksheets[0].Cells["D3"].Value = DateTime.Now.ToShortDateString();
                                printSheet.Document.Worksheets[0].Cells["M3"].Value = DateTime.Now.ToShortDateString();
                                printSheet.Document.Worksheets[0].Cells["V3"].Value = DateTime.Now.ToShortDateString();

                                //출고번호, 문서번호
                                SQL = "SELECT TOP 1 DELIVERY_NO, ORDER_NO FROM BULK_ORDER WHERE IS_NO = '{0}' AND ORDER_NO = '{1}' ";
                                SQL = string.Format(SQL, is_no, Dbconn.conn.getData(custListDs, "ORDER_NO", i));

                                DataSet orderNoDs = Dbconn.conn.ExecutDataset(SQL);
                                if (Dbconn.conn.getRowCnt(orderNoDs) > 0)
                                {
                                    //출고번호
                                    printSheet.Document.Worksheets[0].Cells["D4"].Value = Dbconn.conn.getData(orderNoDs, "DELIVERY_NO", 0);
                                    printSheet.Document.Worksheets[0].Cells["M4"].Value = Dbconn.conn.getData(orderNoDs, "DELIVERY_NO", 0);
                                    printSheet.Document.Worksheets[0].Cells["V4"].Value = Dbconn.conn.getData(orderNoDs, "DELIVERY_NO", 0);

                                    //문서번호
                                    printSheet.Document.Worksheets[0].Cells["G4"].Value = Dbconn.conn.getData(orderNoDs, "ORDER_NO", 0);
                                    printSheet.Document.Worksheets[0].Cells["P4"].Value = Dbconn.conn.getData(orderNoDs, "ORDER_NO", 0);
                                    printSheet.Document.Worksheets[0].Cells["Y4"].Value = Dbconn.conn.getData(orderNoDs, "ORDER_NO", 0);

                                }

                                //거래처명
                                printSheet.Document.Worksheets[0].Cells["D5"].Value = prt_cust_name;
                                printSheet.Document.Worksheets[0].Cells["M5"].Value = prt_cust_name;
                                printSheet.Document.Worksheets[0].Cells["V5"].Value = prt_cust_name;

                                //거래처주소
                                SQL = $@"
	                            SELECT ADDR, TEL_NO
                                FROM TMS_INPUT_CARMASTER_CON 
	                            WHERE CUST_NO = '{0}'
                                ";

                                SQL = string.Format(SQL,
                                    Dbconn.conn.getData(itemListDs, "CUST_NO", j)
                                    );
                                DataSet custAddrDs =  Dbconn.conn.ExecutDataset(SQL);

                                if (Dbconn.conn.getRowCnt(custAddrDs) > 0)
                                {
                                    printSheet.Document.Worksheets[0].Cells["D6"].Value = Dbconn.conn.getData(custAddrDs, "ADDR", 0);
                                    printSheet.Document.Worksheets[0].Cells["M6"].Value = Dbconn.conn.getData(custAddrDs, "ADDR", 0);
                                    printSheet.Document.Worksheets[0].Cells["V6"].Value = Dbconn.conn.getData(custAddrDs, "ADDR", 0);

                                    del_tel = Dbconn.conn.getData(custAddrDs, "TEL_NO", 0);
                                }    

                                //연락처 (거래처)
                                printSheet.Document.Worksheets[0].Cells["G5"].Value = del_tel;
                                printSheet.Document.Worksheets[0].Cells["P5"].Value = del_tel;
                                printSheet.Document.Worksheets[0].Cells["Y5"].Value = del_tel;

                                //메모
                                printSheet.Document.Worksheets[0].Cells["D19"].Value = prt_remark;
                                printSheet.Document.Worksheets[0].Cells["M19"].Value = prt_remark;
                                printSheet.Document.Worksheets[0].Cells["V19"].Value = prt_remark;

                                //차량번호
                                printSheet.Document.Worksheets[0].Cells["D20"].Value = full_car_num;
                                printSheet.Document.Worksheets[0].Cells["M20"].Value = full_car_num;
                                printSheet.Document.Worksheets[0].Cells["V20"].Value = full_car_num;

                                //기사서명
                                printSheet.Document.Worksheets[0].Cells["G20"].Value = del_emp;
                                printSheet.Document.Worksheets[0].Cells["P20"].Value = del_emp;
                                printSheet.Document.Worksheets[0].Cells["Z20"].Value = del_emp;
                            }

                            //제품명
                            printSheet.Document.Worksheets[0].Cells["C" + (8 + rowNum).ToString()].Value = Dbconn.conn.getData(itemListDs, "PART_NAME", j);
                            printSheet.Document.Worksheets[0].Cells["L" + (8 + rowNum).ToString()].Value = Dbconn.conn.getData(itemListDs, "PART_NAME", j);
                            printSheet.Document.Worksheets[0].Cells["U" + (8 + rowNum).ToString()].Value = Dbconn.conn.getData(itemListDs, "PART_NAME", j);

                            //규격
                            printSheet.Document.Worksheets[0].Cells["F" + (8 + rowNum).ToString()].Value = Dbconn.conn.getData(itemListDs, "UOM", j);
                            printSheet.Document.Worksheets[0].Cells["O" + (8 + rowNum).ToString()].Value = Dbconn.conn.getData(itemListDs, "UOM", j);
                            printSheet.Document.Worksheets[0].Cells["X" + (8 + rowNum).ToString()].Value = Dbconn.conn.getData(itemListDs, "UOM", j);

                            //수량
                            printSheet.Document.Worksheets[0].Cells["G" + (8 + rowNum).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "QUANTITY", j)));
                            printSheet.Document.Worksheets[0].Cells["P" + (8 + rowNum).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "QUANTITY", j)));
                            printSheet.Document.Worksheets[0].Cells["Y" + (8 + rowNum).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "QUANTITY", j)));


                            //중량
                            printSheet.Document.Worksheets[0].Cells["H" + (8 + rowNum).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "BATCH_Q", j)));
                            printSheet.Document.Worksheets[0].Cells["Q" + (8 + rowNum).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "BATCH_Q", j)));
                            printSheet.Document.Worksheets[0].Cells["Z" + (8 + rowNum).ToString()].Value = String.Format("{0:#,###}", Convert.ToInt32(Dbconn.conn.getData(itemListDs, "BATCH_Q", j)));

                            sum_cnt += Convert.ToInt32(Dbconn.conn.getData(itemListDs, "QUANTITY", j));
                            sum_weight += Convert.ToInt32(Dbconn.conn.getData(itemListDs, "BATCH_Q", j));


                            if (j == 8)
                            {
                                printSheet.Document.Worksheets[0].Cells["G18"].Value = String.Format("{0:#,###}", sum_cnt);
                                printSheet.Document.Worksheets[0].Cells["P18"].Value = String.Format("{0:#,###}", sum_cnt);
                                printSheet.Document.Worksheets[0].Cells["Y18"].Value = String.Format("{0:#,###}", sum_cnt);

                                printSheet.Document.Worksheets[0].Cells["H18"].Value = String.Format("{0:#,###}", sum_weight);
                                printSheet.Document.Worksheets[0].Cells["Q18"].Value = String.Format("{0:#,###}", sum_weight);
                                printSheet.Document.Worksheets[0].Cells["Z18"].Value = String.Format("{0:#,###}", sum_weight);

                                printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                                printSheet.Options.Print.ShowMarginsWarning = false;

                                printSheet.Print();
                                printSheet.Dispose();
                                //printSheet.ShowPrintPreview();

                                sum_cnt = 0;
                                sum_weight = 0;
                            }

                            rowNum = rowNum +  1;
                        }

                        if (Dbconn.conn.getRowCnt(itemListDs) > 9 || Dbconn.conn.getRowCnt(itemListDs) < 9)
                        {
                            printSheet.Document.Worksheets[0].Cells["G18"].Value = String.Format("{0:#,###}", sum_cnt);
                            printSheet.Document.Worksheets[0].Cells["P18"].Value = String.Format("{0:#,###}", sum_cnt);
                            printSheet.Document.Worksheets[0].Cells["Y18"].Value = String.Format("{0:#,###}", sum_cnt);

                            printSheet.Document.Worksheets[0].Cells["H18"].Value = String.Format("{0:#,###}", sum_weight);
                            printSheet.Document.Worksheets[0].Cells["Q18"].Value = String.Format("{0:#,###}", sum_weight);
                            printSheet.Document.Worksheets[0].Cells["Z18"].Value = String.Format("{0:#,###}", sum_weight);

                            printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;
                            printSheet.Options.Print.ShowMarginsWarning = false;
                            printSheet.Print();
                            //printSheet.ShowPrintPreview();

                        }

                    }

                    clsUtil.Delay(800);

                }
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

                SQL = $@"
                SELECT IS_NO, CUST_CODE, CAR_TDETAIL, VEHICLEGROUPCODE, INCAR_NO,
                    CONVERT(CHAR(10), INCAR_DATE, 23) as INCAR_DATE, INCAR_TIME,
                    OUTCAR_DATE, OUTCAR_TIME, IN_WEIGHT, OUT_WEIGHT
                FROM WAP_DECAR
                WHERE IS_NO = '{is_no}'
                ";

                DataSet weightDs = Dbconn.conn.ExecutDataset(SQL);

                if (Dbconn.conn.getRowCnt(weightDs) == 0)
                {
                    return;
                }

                string prt_is_no = string.Empty;
                string prt_deli_day = string.Empty;
                string prt_car_num = string.Empty;
                string car_tdetail = Dbconn.conn.getData(weightDs, "CAR_TDETAIL", 0);
                string cust_code = string.Empty;

                prt_is_no = Dbconn.conn.getData(weightDs, "IS_NO", 0);
                prt_deli_day = Dbconn.conn.getData(weightDs, "INCAR_DATE", 0);
                prt_car_num = Dbconn.conn.getData(weightDs, "INCAR_NO", 0);
                cust_code = Dbconn.conn.getData(weightDs, "CUST_CODE", 0);

                if (f_print)
                {
                    string weightDate = Dbconn.conn.getData(weightDs, "INCAR_DATE", 0);

                    if (string.IsNullOrEmpty(weightDate))
                    {
                        return;
                    }
                }

/*                if (car_tdetail == "카고")
                {
                    f_print = true;
                }
*/
                string argSql = string.Empty;

                if (car_tdetail == "입고" || car_tdetail == "카고")
                {
                    argSql = " ";
                }
                else
                {
                    argSql = " AND WEIGHT IS NOT NULL ";
                }

                SQL = $@"
                SELECT PART_NO, PART_NAME, CUST_NAME, CONVERT(CHAR(5), WEIGHT_TIME, 8) AS WEIGHT_TIME,
                CONVERT(CHAR(5), BEFORE_WEIGHT_TIME, 8) AS BEFORE_WEIGHT_TIME,
                BEFORE_WEIGHT, WEIGHT,
                ERP_QTY, ERP_WEIGHT, REMARK
                FROM BULK_ORDER WHERE IS_NO = '{is_no}'
                    {argSql}
                ORDER BY WEIGHT_TIME
                ";

                SQL = string.Format(SQL, is_no);

                DataSet weightListDs = Dbconn.conn.ExecutDataset(SQL);

                int weightLisCnt = 0;

                if (car_tdetail == "입고" || car_tdetail == "카고")
                {
                    weightLisCnt = 1;
                }
                else if (car_tdetail == "벌크")
                {
                    weightLisCnt = Dbconn.conn.getRowCnt(weightListDs);

                }else
                {
                    weightLisCnt = 1;
                }

                for (int i = 0; i < weightLisCnt; i++)
                {
                    SpreadsheetControl printSheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();

                    if (car_tdetail == "입고")
                    {
                        printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\weight2.xlsx", DocumentFormat.Xlsx);
                    }
                    else
                    {
                        printSheet.LoadDocument(Application.StartupPath + "\\excel_form\\weight.xlsx", DocumentFormat.Xlsx);
                    }
                    string prt_p_name = string.Empty;
                    string prt_p_code = string.Empty;
                    string prt_c_name = string.Empty;

                    string prt_before_weight = string.Empty;
                    string prt_weight = string.Empty;
                    string prt_erp_weight = string.Empty;
                    string prt_s_time = string.Empty;
                    string prt_e_time = string.Empty;

                    string prt_remark = string.Empty;

                    if (car_tdetail == "카고" || car_tdetail == "벌크")
                    {
                        prt_p_name = Dbconn.conn.getData(weightListDs, "PART_NAME", i);
                        prt_p_code = Dbconn.conn.getData(weightListDs, "PART_NO", i);
                        prt_c_name = Dbconn.conn.getData(weightListDs, "CUST_NAME", i);

                        prt_remark = Dbconn.conn.getData(weightListDs, "REMARK", i);

                        if (car_tdetail == "카고" && i >= 1)
                        {
                            break;
                        }

                        if (car_tdetail == "벌크")
                        {
                            prt_before_weight = Dbconn.conn.getData(weightListDs, "BEFORE_WEIGHT", i);
                            prt_weight = Dbconn.conn.getData(weightListDs, "WEIGHT", i);
                            prt_erp_weight = Dbconn.conn.getData(weightListDs, "ERP_WEIGHT", i);
                            prt_s_time = Dbconn.conn.getData(weightListDs, "BEFORE_WEIGHT_TIME", i);
                            prt_e_time = Dbconn.conn.getData(weightListDs, "WEIGHT_TIME", i);
                        }
                        else if (car_tdetail == "카고")
                        {
                            SQL = $@"
                            SELECT CONVERT(CHAR(5), INCAR_TIME, 8) AS S_TIME,
                                CONVERT(CHAR(5), OUTCAR_TIME, 8) AS E_TIME,
                                FLOOR(OUT_WEIGHT) as OUT_WEIGHT, FLOOR(IN_WEIGHT) as IN_WEIGHT,
                                FLOOR(OUT_WEIGHT - IN_WEIGHT) AS CHA_WEIGHT
                                FROM WAP_DECAR
                            WHERE IS_NO = '{is_no}'
                            ";

                            DataSet cagoInfoDs = Dbconn.conn.ExecutDataset(SQL);

                            if (Dbconn.conn.getRowCnt(cagoInfoDs) == 1)
                            {
                                prt_before_weight = Dbconn.conn.getData(cagoInfoDs, "IN_WEIGHT", i);
                                prt_weight = Dbconn.conn.getData(cagoInfoDs, "OUT_WEIGHT", i);
                                prt_erp_weight = Dbconn.conn.getData(cagoInfoDs, "CHA_WEIGHT", i);
                                prt_s_time = Dbconn.conn.getData(cagoInfoDs, "S_TIME", i);
                                prt_e_time = Dbconn.conn.getData(cagoInfoDs, "E_TIME", i);
                            }

                            if (f_print)
                            {
                                prt_before_weight = Dbconn.conn.getData(cagoInfoDs, "IN_WEIGHT", i);

                                SQL = $"SELECT FLOOR(ISNULL(SUM(P_Q),0)) AS PQ_SUM FROM BULK_ORDER WHERE IS_NO = '{is_no}' AND C_CONDITION = '{clsCommon.PcStatus.Completed}' ";

                                DataSet searchWeightDs = Dbconn.conn.ExecutDataset(SQL);

                                if (Dbconn.conn.getRowCnt(searchWeightDs) > 0)
                                {
                                    prt_weight = (Convert.ToInt32(Dbconn.conn.getData(searchWeightDs, "PQ_SUM", 0)) + Convert.ToInt32(prt_before_weight)).ToString();
                                }

                                prt_erp_weight = (Convert.ToInt32(prt_weight) - Convert.ToInt32(prt_before_weight)).ToString();

                                prt_s_time = Dbconn.conn.getData(cagoInfoDs, "S_TIME", i);
                                prt_e_time = DateTime.Now.ToString("HH:mm");
                            }
                        }
                    }
                    else if (car_tdetail == "입고")
                    {
                        SQL = $@"
                        SELECT TRIM(PARTNER) as VENDOR, TRIM(NAME_ORG1) as NAME
                        FROM SAP_DI_CUSTOMER
                        WHERE TRIM(PARTNER) = '{cust_code}'
                        ";

                        using (DataSet vendorDs = Dbconn.conn.ExecutDataset(SQL))
                        {
                            if (Dbconn.conn.getRowCnt(vendorDs) == 1)
                            {
                                prt_c_name = Dbconn.conn.getData(vendorDs, "NAME", i);
                            }
                        }

                        SQL = $@"
                        SELECT TOP 1  WG.INGRED_CODE, ERP_ING.DESCRIPTION
                        FROM WAP_GOCAR WG
                            LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING ON WG.INGRED_CODE = ERP_ING.RESOURCE_NO
                        WHERE WG.IS_NO = '{0}'
                        ";

                        SQL = string.Format(SQL, is_no);
                        using (DataSet ingDs = Dbconn.conn.ExecutDataset(SQL))
                        {
                            if (Dbconn.conn.getRowCnt(ingDs) == 1)
                            {
                                prt_p_name = Dbconn.conn.getData(ingDs, "DESCRIPTION", i);
                            }
                        }

                        SQL = $@"
                        SELECT CONVERT(CHAR(5), INCAR_TIME, 8) AS S_TIME,
                            CONVERT(CHAR(5), OUTCAR_TIME, 8) AS E_TIME,
                            FLOOR(OUT_WEIGHT) as OUT_WEIGHT, FLOOR(IN_WEIGHT) as IN_WEIGHT,
                            FLOOR(IN_WEIGHT - OUT_WEIGHT) AS CHA_WEIGHT
                        FROM WAP_DECAR WHERE IS_NO = '{is_no}'
                        ";

                        DataSet cagoInfoDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(cagoInfoDs) == 1)
                        {
                            prt_before_weight = Dbconn.conn.getData(cagoInfoDs, "OUT_WEIGHT", i);
                            prt_weight = Dbconn.conn.getData(cagoInfoDs, "IN_WEIGHT", i);
                            prt_erp_weight = Dbconn.conn.getData(cagoInfoDs, "CHA_WEIGHT", i);
                            prt_s_time = Dbconn.conn.getData(cagoInfoDs, "S_TIME", i);
                            prt_e_time = Dbconn.conn.getData(cagoInfoDs, "E_TIME", i);
                        }
                    }
                    else
                    {
                        SQL = $@"
                        SELECT CONVERT(CHAR(5), INCAR_TIME, 8) AS S_TIME,
                            CONVERT(CHAR(5), OUTCAR_TIME, 8) AS E_TIME,
                            FLOOR(OUT_WEIGHT) as OUT_WEIGHT, FLOOR(IN_WEIGHT) as IN_WEIGHT,
                            ABS(CEILING(IN_WEIGHT - OUT_WEIGHT))  AS CHA_WEIGHT
                        FROM WAP_DECAR WHERE IS_NO = '{is_no}'
                        ";

                        DataSet cagoInfoDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(cagoInfoDs) == 1)
                        {
                            prt_before_weight = Dbconn.conn.getData(cagoInfoDs, "IN_WEIGHT", i);
                            prt_weight = Dbconn.conn.getData(cagoInfoDs, "OUT_WEIGHT", i);
                            prt_erp_weight = Dbconn.conn.getData(cagoInfoDs, "CHA_WEIGHT", i);
                            prt_s_time = Dbconn.conn.getData(cagoInfoDs, "S_TIME", i);
                            prt_e_time = Dbconn.conn.getData(cagoInfoDs, "E_TIME", i);
                        }
                    }

                    string prt_no = string.Empty;

                    SQL = $"SELECT TOP 1 DELIVERY_NO FROM BULK_ORDER WHERE IS_NO = '{prt_is_no}' ";
                    
                    DataSet deliveryDs =  Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(deliveryDs) > 0)
                    {
                        prt_no = Dbconn.conn.getData(deliveryDs, "DELIVERY_NO", 0);
                    }

                    //NO
                    printSheet.Document.Worksheets[0].Cells["O2"].Value = prt_no;
                    printSheet.Document.Worksheets[0].Cells["AN2"].Value = prt_no;
                    printSheet.Document.Worksheets[0].Cells["BH2"].Value = prt_no;

                    //년월일
                    printSheet.Document.Worksheets[0].Cells["G5"].Value = prt_deli_day;
                    printSheet.Document.Worksheets[0].Cells["AF5"].Value = prt_deli_day;
                    printSheet.Document.Worksheets[0].Cells["AZ5"].Value = prt_deli_day;

                    //차량번호
                    printSheet.Document.Worksheets[0].Cells["G6"].Value = prt_car_num;
                    printSheet.Document.Worksheets[0].Cells["AF6"].Value = prt_car_num;
                    printSheet.Document.Worksheets[0].Cells["AZ6"].Value = prt_car_num;

                    if (car_tdetail == "입고" || car_tdetail == "카고" || car_tdetail == "벌크")
                    {
                        //품명
                        printSheet.Document.Worksheets[0].Cells["G7"].Value = prt_p_name;
                        printSheet.Document.Worksheets[0].Cells["AF7"].Value = prt_p_name;
                        printSheet.Document.Worksheets[0].Cells["AZ7"].Value = prt_p_name;
                    }else
                    {
                        //품명
                        printSheet.Document.Worksheets[0].Cells["G7"].Value = car_tdetail;
                        printSheet.Document.Worksheets[0].Cells["AF7"].Value = car_tdetail;
                        printSheet.Document.Worksheets[0].Cells["AZ7"].Value = car_tdetail;
                    }


                    if (!string.IsNullOrEmpty(prt_remark) )
                    {
                        prt_c_name = prt_c_name + " (" + prt_remark + ")";
                    }

                    //거래처
                    printSheet.Document.Worksheets[0].Cells["G8"].Value = prt_c_name;
                    printSheet.Document.Worksheets[0].Cells["AF8"].Value = prt_c_name;
                    printSheet.Document.Worksheets[0].Cells["AZ8"].Value = prt_c_name;

                    //시간1차
                    printSheet.Document.Worksheets[0].Cells["J9"].Value = prt_s_time;
                    printSheet.Document.Worksheets[0].Cells["AI9"].Value = prt_s_time;
                    printSheet.Document.Worksheets[0].Cells["BC9"].Value = prt_s_time;

                    
                    //시간2차
                    printSheet.Document.Worksheets[0].Cells["O9"].Value = prt_e_time;
                    printSheet.Document.Worksheets[0].Cells["AN9"].Value = prt_e_time;
                    printSheet.Document.Worksheets[0].Cells["BH9"].Value = prt_e_time;

                    decimal pa_weight = 0;

                    SQL = $@"
                    SELECT ISNULL(FLOOR(SUM(PM.WEIGHT * INADD.PD_QTY)),0) AS PA_SUM 
                    FROM WAP_IN_ADD INADD
                        LEFT OUTER JOIN WAP_PA_MASTER PM ON INADD.PTMCD = PM.PTMCD 
                    WHERE INADD.IS_NO = '{is_no}' ";
                    
                    DataSet palletSumDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(palletSumDs) > 0)
                    {
                        pa_weight = Convert.ToInt32(Dbconn.conn.getData(palletSumDs, "PA_SUM", 0));
                       // prt_weight = (Convert.ToInt32(prt_weight) + pa_weight).ToString();
                    }

                    //피무계합산
                    if (car_tdetail == "입고")
                    {
                        SQL = $@"
                        SELECT SUM(A.P_WEIGHT) AS P_WEIGHT  FROM
                        (
                            SELECT CASE WHEN EA = 1 THEN 0 
                            WHEN (EA <> 1) AND (SPIV_CAR_WEIGHT / EA) >= 100 THEN 3 * EA 
                            ELSE 0.23 * EA 
                            END P_WEIGHT 
                            FROM WAP_GOCAR  
                            WHERE IS_NO = '{0}'
                        ) A
                        ";
                    }
                    else
                    {
                        if (car_tdetail == "카고")
                        {
                            SQL = $@"
                            SELECT ISNULL(SUM(a.P_WEIGHT),0) as P_WEIGHT  FROM (
                            SELECT CASE WHEN RIGHT(PART_NO, 1) = '2' THEN 3 * (QUANTITY / 1000)
                            WHEN RIGHT(PART_NO, 1) = '1' THEN 0.23 * QUANTITY
                            ELSE 0 END P_WEIGHT
                            FROM BULK_ORDER WHERE IS_NO = '{0}'
                            ) a
                            ";
                        }
                    }

                    SQL = string.Format(SQL, is_no);

                    if (car_tdetail == "입고" || car_tdetail == "카고")
                    {

                        DataSet pSumDs = Dbconn.conn.ExecutDataset(SQL);
                        if (Dbconn.conn.getRowCnt(pSumDs) > 0)
                        {
                            pa_weight = pa_weight + Convert.ToDecimal(Dbconn.conn.getData(pSumDs, "P_WEIGHT", 0));
                        }
                    }


                    //공차중량
                    printSheet.Document.Worksheets[0].Cells["G11"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_before_weight));
                    printSheet.Document.Worksheets[0].Cells["AF11"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_before_weight));
                    printSheet.Document.Worksheets[0].Cells["AZ11"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_before_weight));

  
                    //피중량
                    printSheet.Document.Worksheets[0].Cells["L13"].Value = String.Format("{0:N2}", Convert.ToDecimal(pa_weight)) + " KG" ;
                    printSheet.Document.Worksheets[0].Cells["AK13"].Value = String.Format("{0:N2}", Convert.ToDecimal(pa_weight)) + " KG";
                    printSheet.Document.Worksheets[0].Cells["BE13"].Value = String.Format("{0:N2}", Convert.ToDecimal(pa_weight)) + " KG";


                    //파레트수량
                    string pa_cnt = "";

                    /*                   
                         SQL =
                        "SELECT ISNULL(SUM(INADD.PD_QTY),0) AS PA_COUNT 
                        "FROM WAP_IN_ADD INADD LEFT OUTER JOIN WAP_PA_MASTER PM
                        "ON INADD.PTMCD = PM.PTMCD 
                        "WHERE INADD.IS_NO = '{0}' ";
                    */

                     SQL = $@"
                    SELECT  REPLACE(PM.PTMCDNM + ' : ' + CAST(INADD.PD_QTY AS NVARCHAR), ' 좌대','') as PA_CNT
                    FROM WAP_IN_ADD INADD
                        LEFT OUTER JOIN WAP_PA_MASTER PM ON INADD.PTMCD = PM.PTMCD  
                    WHERE INADD.IS_NO = '{0}'
                    ";

                    SQL = string.Format(SQL, is_no);

                    DataSet palletCntDs = Dbconn.conn.ExecutDataset(SQL);

                    if (Dbconn.conn.getRowCnt(palletCntDs) > 0)
                    {
                        for (int s=0; s < Dbconn.conn.getRowCnt(palletCntDs); s++ )
                        {
                            pa_cnt = pa_cnt + Dbconn.conn.getData(palletCntDs, "PA_CNT", s);

                            if (s != Dbconn.conn.getRowCnt(palletCntDs) - 1)
                            {
                                pa_cnt = pa_cnt + ", ";
                            }
                        }
                    }

                    printSheet.Document.Worksheets[0].Cells["G13"].Value = pa_cnt;
                    printSheet.Document.Worksheets[0].Cells["AF13"].Value = pa_cnt;
                    printSheet.Document.Worksheets[0].Cells["AZ13"].Value = pa_cnt;


                    if (pa_weight > 0)
                    {
                        if (f_print)
                        {
                            prt_weight = (Convert.ToInt32(prt_weight) + Convert.ToInt32(pa_weight)).ToString();
                        }
                        else
                        {
                            prt_erp_weight = (Convert.ToInt32(prt_erp_weight) - Convert.ToInt32(pa_weight)).ToString();
                        }
                    }

                    //총중량
                    printSheet.Document.Worksheets[0].Cells["G10"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_weight));
                    printSheet.Document.Worksheets[0].Cells["AF10"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_weight));
                    printSheet.Document.Worksheets[0].Cells["AZ10"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_weight));


                    //실중량
                    printSheet.Document.Worksheets[0].Cells["G12"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_erp_weight));
                    printSheet.Document.Worksheets[0].Cells["AF12"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_erp_weight));
                    printSheet.Document.Worksheets[0].Cells["AZ12"].Value = String.Format("{0:#,###}", Convert.ToInt32(prt_erp_weight));

                    DataSet carInfoDs = clsCarCommon.GetCarMaster(prt_car_num);
                    string d_emp_name = string.Empty;

                    if (Dbconn.conn.getRowCnt(carInfoDs) > 0)
                    {
                        d_emp_name = Dbconn.conn.getData(carInfoDs, "DRIVERNAME", 0);
                    }

                    //기사성명
                    printSheet.Document.Worksheets[0].Cells["O14"].Value = d_emp_name;
                    printSheet.Document.Worksheets[0].Cells["AN14"].Value = d_emp_name;
                    printSheet.Document.Worksheets[0].Cells["BH14"].Value = d_emp_name;

                    /*
                    if (car_tdetail == "카고" || car_tdetail == "벌크")
                    {

                        //배합사료성분표
                        SQL = $@"
                        SELECT INGREDIENT_NO, FEED_NAME, FEED_TYPE, FEED_USE, 
                            CODE_DESC01, CODE_QTY01, CODE_INFO01, CODE_DESC02, CODE_QTY02, CODE_INFO02,
                            CODE_DESC03, CODE_QTY03, CODE_INFO3 as CODE_INFO03, CODE_DESC04, CODE_QTY04, CODE_INFO04,
                            CODE_DESC05, CODE_QTY05, CODE_INFO05, CODE_DESC06, CODE_QTY06, CODE_INFO06,
                            CODE_DESC07, CODE_QTY07, CODE_INFO07, CODE_DESC08, CODE_QTY08, CODE_INFO08,
                            CODE_DESC09, CODE_QTY09, CODE_INFO09, CODE_DESC10, CODE_QTY10, CODE_INFO10,
                            FEED_USE_NAME, COM_DESC01, COM_DESC02, MFG_DT,
                            COM_DESC11, COM_DESC12, COM_DESC13, COM_DESC14, COM_DESC15, COM_DESC16, COM_DESC17 
                        FROM ERP_DBLINK.ATL_MFG.DBO.V_MES_ATG_114_1
                        WHERE RESOURCE_NO = '{prt_p_code}'
                        ";

                        DataSet dosingPrintDs = Dbconn.conn.ExecutDataset(SQL);

                        if (Dbconn.conn.getRowCnt(dosingPrintDs) == 1)
                        {
                            //1.성분등록번호
                            printSheet.Document.Worksheets[0].Cells["H18"].Value = Dbconn.conn.getData(dosingPrintDs, "INGREDIENT_NO", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AG18"].Value = Dbconn.conn.getData(dosingPrintDs, "INGREDIENT_NO", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["BA18"].Value = Dbconn.conn.getData(dosingPrintDs, "INGREDIENT_NO", 0).Trim();

                            //2.사료의명칭및형태
                            printSheet.Document.Worksheets[0].Cells["H19"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_NAME", 0).Trim() + " (" + Dbconn.conn.getData(dosingPrintDs, "FEED_TYPE", 0).Trim() + ")";
                            printSheet.Document.Worksheets[0].Cells["AG19"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_NAME", 0).Trim() + " (" + Dbconn.conn.getData(dosingPrintDs, "FEED_TYPE", 0).Trim() + ")";
                            printSheet.Document.Worksheets[0].Cells["BA19"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_NAME", 0).Trim() + " (" + Dbconn.conn.getData(dosingPrintDs, "FEED_TYPE", 0).Trim() + ")";

                            //3.사료의 용도
                            printSheet.Document.Worksheets[0].Cells["H20"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_USE", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AG20"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_USE", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["BA20"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_USE", 0).Trim();


                            int cnt_qty = 0;

                            int excel_x_pos = 24;

                            string DESC1 = "B";
                            string QTY1 = "J";
                            string DESC2 = "AA";
                            string QTY2 = "AI";
                            string DESC3 = "AU";
                            string QTY3 = "BC";

                            //4.등록성분량
                            for (int j = 0; j < 10; j++)
                            {
                                decimal qty = Convert.ToDecimal(Dbconn.conn.getData(dosingPrintDs, "CODE_QTY" + (j + 1).ToString().PadLeft(2, '0'), 0).Trim());

                                if (qty != 0)
                                {

                                    string value_qty;
                                    string value_unit = Dbconn.conn.getData(dosingPrintDs, "CODE_INFO" + (j + 1).ToString().PadLeft(2, '0'), 0).Trim();

                                    if (value_unit.Contains("이상") || value_unit.Contains("이하"))
                                    {
                                        value_qty = string.Format("{0, 10:N2}", qty);
                                    }
                                    else
                                    {
                                        value_qty = string.Format("{0, 10:N0}", qty);
                                    }

                                    printSheet.Document.Worksheets[0].Cells[DESC1 + excel_x_pos.ToString()].Value = Dbconn.conn.getData(dosingPrintDs, "CODE_DESC" + (j + 1).ToString().PadLeft(2, '0'), 0).Trim();
                                    printSheet.Document.Worksheets[0].Cells[QTY1 + excel_x_pos.ToString()].Value = value_qty + " " + value_unit;

                                    printSheet.Document.Worksheets[0].Cells[DESC2 + excel_x_pos.ToString()].Value = Dbconn.conn.getData(dosingPrintDs, "CODE_DESC" + (j + 1).ToString().PadLeft(2, '0'), 0).Trim();
                                    printSheet.Document.Worksheets[0].Cells[QTY2 + excel_x_pos.ToString()].Value = value_qty + " " + value_unit;

                                    printSheet.Document.Worksheets[0].Cells[DESC3 + excel_x_pos.ToString()].Value = Dbconn.conn.getData(dosingPrintDs, "CODE_DESC" + (j + 1).ToString().PadLeft(2, '0'), 0).Trim();
                                    printSheet.Document.Worksheets[0].Cells[QTY3 + excel_x_pos.ToString()].Value = value_qty + " " + value_unit;

                                    excel_x_pos = excel_x_pos + 1;

                                    if (excel_x_pos > 28)
                                    {
                                        DESC1 = "M";
                                        QTY1 = "Q";
                                        DESC2 = "AL";
                                        QTY2 = "AP";
                                        DESC3 = "BF";
                                        QTY3 = "BJ";
                                        excel_x_pos = 24;
                                    }
                                }

                            }

                            //5.사용한 원료의 명칭
                            printSheet.Document.Worksheets[0].Cells["B33"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_USE_NAME", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AA33"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_USE_NAME", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AU33"].Value = Dbconn.conn.getData(dosingPrintDs, "FEED_USE_NAME", 0).Trim();

                            //주의
                            printSheet.Document.Worksheets[0].Cells["C34"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC01", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AB34"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC01", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AV34"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC01", 0).Trim();

                            printSheet.Document.Worksheets[0].Cells["C35"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC02", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AB35"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC02", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AV35"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC02", 0).Trim();

                            //6.제조년월일
                            printSheet.Document.Worksheets[0].Cells["B36"].Value = "제조년월일 :  " + Dbconn.conn.getData(dosingPrintDs, "MFG_DT", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AA36"].Value = "제조년월일 :  " + Dbconn.conn.getData(dosingPrintDs, "MFG_DT", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AU36"].Value = "제조년월일 :  " + Dbconn.conn.getData(dosingPrintDs, "MFG_DT", 0).Trim();

                            //9.주의사항
                            printSheet.Document.Worksheets[0].Cells["B42"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC11", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AA42"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC11", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AU42"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC11", 0).Trim();

                            printSheet.Document.Worksheets[0].Cells["B43"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC12", 0).Trim() + Dbconn.conn.getData(dosingPrintDs, "COM_DESC13", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AA43"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC12", 0).Trim() + Dbconn.conn.getData(dosingPrintDs, "COM_DESC13", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AU43"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC12", 0).Trim() + Dbconn.conn.getData(dosingPrintDs, "COM_DESC13", 0).Trim();

                            printSheet.Document.Worksheets[0].Cells["B44"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC14", 0).Trim() + Dbconn.conn.getData(dosingPrintDs, "COM_DESC15", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AA44"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC14", 0).Trim() + Dbconn.conn.getData(dosingPrintDs, "COM_DESC15", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AU44"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC14", 0).Trim() + Dbconn.conn.getData(dosingPrintDs, "COM_DESC15", 0).Trim();

                            printSheet.Document.Worksheets[0].Cells["B45"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC16", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AA45"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC16", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AU45"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC16", 0).Trim();

                            printSheet.Document.Worksheets[0].Cells["B46"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC17", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AA46"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC17", 0).Trim();
                            printSheet.Document.Worksheets[0].Cells["AU46"].Value = Dbconn.conn.getData(dosingPrintDs, "COM_DESC17", 0).Trim();
                        }
                    }
                    */

                    printSheet.Unit = DevExpress.Office.DocumentUnit.Inch;

                    Margins pageMargins = printSheet.Document.Worksheets[0].ActiveView.Margins;
                    pageMargins.Left = 0.2F;
                    pageMargins.Top = 0.15F;
                    pageMargins.Right = 0;
                    pageMargins.Bottom = 0;
                    //pageMargins.Header = 1;
                    //pageMargins.Footer = 0.4F;
                    //printSheet.ShowPrintPreview();

                    printSheet.Options.Print.ShowMarginsWarning = false;
                    printSheet.Print();
                    //printSheet.ShowPrintPreview();
                }

                clsUtil.Delay(800);

                /*            PrintableComponentLink printLink = new PrintableComponentLink();
                            printLink.PrintingSystemBase = new PrintingSystemBase();
                            printLink.Landscape = false;
                            printLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                            printLink.Margins = new System.Drawing.Printing.Margins(50, 0, 50, 0);
                            printLink.Component = printSheet;*/
                //printLink.Print();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex);
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex.StackTrace);
                clsLog.logSave("clsPrintExcel", "PrintWeighingSheet", ex.Source);
            }

        }

    }
}


