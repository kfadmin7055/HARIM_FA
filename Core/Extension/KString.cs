using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Extension
{
    public static class KString
    {
        public static string sReturn = string.Empty;
        public static int iReturn = 0;

        public static string Merge(this string sValue, string sMerge)
        {
            return string.Concat(sValue, sMerge);
        }

        /// <summary>
        /// NULL 여부
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static bool IsNullEmpty(this string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return true;

            return false;
        }

        public static string IsNullValue(this string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "";

            return sValue;
        }

        public static string Merge(this string sValue, string[] sMerge)
        {
            if (sMerge.Length <= 1) return sValue.Merge(sMerge[0]);

            StringBuilder sb = new StringBuilder();

            sb.Append(sValue);

            for (int i = 0; i < sMerge.Length; i++)
            {
                sb.Append(sMerge[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 2번째 문자열 부터 콤마(,) 추가 하여 합치기
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static string CommaAddInString(this string[] sValue, bool bData = false)
        {
            try
            {
                sReturn = string.Empty;

                StringBuilder sb = new StringBuilder();
                string sSingleQuotation = string.Empty;

                sSingleQuotation = bData ? "'" : string.Empty;

                for (int i = 0; i < sValue.Length; i++)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");

                    sb.Append(string.Concat(sSingleQuotation, sValue[i], sSingleQuotation));
                }

                sReturn = sb.ToString();
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

            return sReturn;
        }

        /// <summary>
        /// 2번째 문자열 부터 콤마(,) 추가 하여 합치기
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static string CommaAddInString(this Dictionary<string, string> sValue, bool bWhere = false, bool bData = false)
        {
            try
            {
                sReturn = string.Empty;

                StringBuilder sb = new StringBuilder();
                string sSingleQuotation = string.Empty;

                sSingleQuotation = bData ? "'" : string.Empty;


                foreach (KeyValuePair<string, string> keyValuePair in sValue)
                {
                    if (sb.Length > 0)
                        sb.Append(bWhere ? " AND " : ", ");

                    sb.Append(string.Format("{0} = {1}", keyValuePair.Key, string.Concat(sSingleQuotation, keyValuePair.Value, sSingleQuotation)));
                }

                sReturn = sb.ToString();
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

            return sReturn;
        }

        public static string ToDate(this string sValue, string sFormat = "-")
        {
            sReturn = string.Empty;
            string dateFormat = string.Empty;

            if (sValue.IsNullEmpty())
                return "1900-01-01";

            switch (sFormat)
            {
                case "-":
                    dateFormat = "yyyy-MM-dd";
                    break;
                case ".":
                    dateFormat = "yyyy.MM.dd";
                    break;
                case "":
                    dateFormat = "yyyyMMdd";
                    break;
            }

            sReturn = Convert.ToDateTime(sValue).ToString(dateFormat);

            return sReturn;
        }

        public static bool ContainArray(this string sValue, string[] sArray)
        {
            for (int i = 0; i < sArray.Length; i++)
            {
                if (sArray[i] == sValue)
                    return true;
            }

            return false;
        }

        public static int Int32Parse(this string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return 0;

            return Int32.Parse(sValue);
        }

        public static int Int16Parse(this string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return 0;

            return Int16.Parse(sValue);
        }

        public static string LastString(this string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "";

            return sValue.Substring(sValue.Length - 1);
        }

        public static string GetOlnyNumber(this string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "";

            string onlyNumbers = Regex.Replace(sValue, "[^0-9]", "");
            int number = int.Parse(onlyNumbers);

            return number.ToString();
        }

        public static (string Device, string Number) SplitDeviceAndNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return ("", "");

            string device = new string(input.TakeWhile(char.IsLetter).ToArray());
            string number = new string(input.SkipWhile(char.IsLetter).ToArray());

            return (device, number);
        }
    }
}
