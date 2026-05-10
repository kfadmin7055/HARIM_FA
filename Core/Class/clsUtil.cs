using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class clsUtil
    {

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static string GetIniValue(string section, string key, string filePath)
        {
            StringBuilder temp = new StringBuilder(255);
            try
            {
                int i = GetPrivateProfileString(section, key, "", temp, 255, filePath);
            }
            catch (System.Exception ex)
            {
                clsLog.logSave("clsUtil", "GetIniValue", ex);
                ShowMessageBox.XtraShowError("ini파일 읽기를 실패했습니다");
            }
            return temp.ToString();
        }
        public static void SetIniValue(string section, string key, string val, string filePath)
        {
            try
            {
                WritePrivateProfileString(section, key, val, filePath);
            }
            catch (System.Exception ex)
            {
                clsLog.logSave("clsUtil", "SetIniValue", ex);
                ShowMessageBox.XtraShowError("ini파일 쓰기를 실패했습니다");
            }
        }

        /// <summary>
        /// 딜레이함수
        /// </summary>
        /// <param name="MS">단위 ms </param>
        /// <returns></returns>
        public static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        // Dword 읽기 함수
        public static int GetDevice(int n1, int n2)
        {
            int Decimal = 0;
            int i;
            int n = 1;

            try
            {
                string ss = Convert.ToString(n2, 2).PadLeft(16, '0') + Convert.ToString(n1, 2).PadLeft(16, '0');
                char[] szBinary = ss.ToCharArray(0, 32);

                if (szBinary[0] == '0')
                {
                    for (i = 31; i >= 0; i--)
                    {
                        Decimal = Decimal + ((szBinary[i] - '0') * n);
                        n = n * 2;
                    }
                }
                else
                {
                    for (i = 31; i >= 0; i--)
                    {
                        if (szBinary[i] == '1') szBinary[i] = '0';
                        else szBinary[i] = '1';
                    }
                    n = 1;
                    Decimal = 0;
                    for (i = 31; i >= 0; i--)
                    {
                        Decimal = Decimal + ((szBinary[i] - '0') * n);
                        n = n * 2;
                    }
                    Decimal = Decimal + 1;
                    Decimal = Decimal * -1;
                }

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsUtil", "GetDevice", ex);
                clsLog.logSave("clsUtil", "GetDevice", ex.StackTrace);
                clsLog.logSave("clsUtil", "GetDevice", ex.Source);
            }

            return Decimal;
        }

        public static bool isNumber(string num)
        {
            try
            {
                string s = num;
                int i = Int32.Parse(s);
            }
            catch (FormatException ex)
            {
                return false;
            }
            return true;
        }
    }
}
