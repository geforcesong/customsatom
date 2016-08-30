using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace ProTemplate.Web.Utility
{
    public sealed class Encryptor
    {
        private static readonly string key = "adgaw334^*^&#$#$W2343qwreqwr12";
        private static readonly SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();

        private Encryptor()
        {
        }

        public static string Decrypt(string sqlInput)
        {
            string source = sqlInput == null ? string.Empty : sqlInput;
            if (!string.IsNullOrEmpty(source))
            {
                try
                {
                    byte[] buffer = Convert.FromBase64String(source);
                    MemoryStream stream = new MemoryStream(buffer, 0, buffer.Length);
                    mobjCryptoService.Key = GetLegalKey();
                    mobjCryptoService.IV = GetLegalIV();
                    ICryptoTransform transform = mobjCryptoService.CreateDecryptor();
                    CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cryptoStream);
                    return reader.ReadToEnd();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public static string Encrypt(string sqlInput)
        {
            string source = sqlInput == null ? string.Empty : sqlInput;
            if (!string.IsNullOrEmpty(source))
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(source);
                    MemoryStream stream = new MemoryStream();
                    mobjCryptoService.Key = GetLegalKey();
                    mobjCryptoService.IV = GetLegalIV();
                    ICryptoTransform transform = mobjCryptoService.CreateEncryptor();
                    CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();
                    stream.Close();
                    return Convert.ToBase64String(stream.ToArray());
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private static byte[] GetLegalIV()
        {
            string s = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
            mobjCryptoService.GenerateIV();
            int length = mobjCryptoService.IV.Length;
            if (s.Length > length)
            {
                s = s.Substring(0, length);
            }
            else if (s.Length < length)
            {
                s = s.PadRight(length, ' ');
            }
            return System.Text.Encoding.ASCII.GetBytes(s);
        }

        private static byte[] GetLegalKey()
        {
            string s = key;
            mobjCryptoService.GenerateKey();
            int length = mobjCryptoService.Key.Length;
            if (s.Length > length)
            {
                s = s.Substring(0, length);
            }
            else if (s.Length < length)
            {
                s = s.PadRight(length, ' ');
            }
            return System.Text.Encoding.ASCII.GetBytes(s);
        }
    }
}