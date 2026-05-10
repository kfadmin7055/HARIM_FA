using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class clsMD5
    {
        public static string MD5_Code(string TextStr)
        {
            MD5 md5Hash = MD5.Create();
            return GetMd5Hash(md5Hash, TextStr);
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] array = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            checked
            {
                int num = array.Length - 1;
                for (int i = 0; i <= num; i++)
                {
                    stringBuilder.Append(array[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string md5Hash2 = GetMd5Hash(md5Hash, input);
            StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
            if (ordinalIgnoreCase.Compare(md5Hash2, hash) == 0)
            {
                return true;
            }

            return false;
        }

    }
}
