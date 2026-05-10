using Core.Class;
using DevExpress.DataAccess.Excel;
using DevExpress.XtraSpreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extension
{
    public static class KExcel
    {
        public static DataTable ToDataTableFromExcelDataSource(this ExcelDataSource excelDataSource)
        {
            IList list = ((IListSource)excelDataSource).GetList();
            DevExpress.DataAccess.Native.Excel.DataView dataView = (DevExpress.DataAccess.Native.Excel.DataView)list;
            List<PropertyDescriptor> props = dataView.Columns.ToList<PropertyDescriptor>();

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (DevExpress.DataAccess.Native.Excel.ViewRow item in list)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// 엑셀 셀 쓰기
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="iSheetNum"></param>
        /// <param name="cellName"></param>
        /// <param name="sValue"></param>
        public static void SetCellValue(this SpreadsheetControl spread, int iSheetNum, string cellName, string sValue)
        {
            spread.Document.Worksheets[iSheetNum].Cells[cellName].Value = sValue;
        }

        /// <summary>
        /// 엑셀 셀 읽기
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="iSheetNum"></param>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public static string GetCellValue(this SpreadsheetControl spread, int iSheetNum, string cellName)
        {
            return spread.Document.Worksheets[iSheetNum].Cells[cellName].Value.ToString();
        }
    }
}
