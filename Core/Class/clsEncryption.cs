using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class clsEncryption
    {
        // 암호화 AES256
        public static string EncryptString(string InputText, string Password)
        {
            string EncryptedData = "";
            try
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
                ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(PlainText, 0, PlainText.Length);
                cryptoStream.FlushFinalBlock();

                byte[] CipherBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                EncryptedData = Convert.ToBase64String(CipherBytes);
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsUtil", "EncryptString", ex);
                ShowMessageBox.XtraShowError("암호화 진행하는 도중 오류가 발생했습니다");
            }
            return EncryptedData;
        }

        // 복호화 AES256
        public static string DecryptString(string InputText, string Password)
        {
            string DecryptedData = "";
            try
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                byte[] PlainText = new byte[EncryptedData.Length];
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

                memoryStream.Close();
                cryptoStream.Close();
                DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

            }
            catch (Exception ex)
            {
                clsLog.logSave("clsUtil", "DecryptString", ex);
                ShowMessageBox.XtraShowError("복호화 진행하는 도중 오류가 발생했습니다");
            }
            return DecryptedData;
        }

        // 암호화 SHA-256
        public static string SHA256Hash(string InputText)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(InputText));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
    }
}
