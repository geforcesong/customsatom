using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

namespace ProTemplate.Web.Utility
{
    public class EncryptionUtil
    {
        public static string Encrypt(string content)
        {
            return Convert.ToBase64String(SevenZip.Compression.LZMA.SevenZipHelper.Compress(Encoding.UTF8.GetBytes(content)));
        }

        public static string Decrypt(string content)
        {
            byte[] b = Convert.FromBase64String(content.Replace(" ", "+"));
            return System.Text.Encoding.UTF8.GetString(SevenZip.Compression.LZMA.SevenZipHelper.Decompress(b));
        }
    }
}