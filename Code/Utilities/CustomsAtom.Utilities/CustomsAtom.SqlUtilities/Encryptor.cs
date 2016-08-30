/*=====================================================================

  File:      Encryptor.cs for String Utilities Example
  Summary:   Provides regular expression matching and replacing 
             functionality for Transact-SQL callers.
  Date:	     November 11, 2004

---------------------------------------------------------------------
  This file is part of the Microsoft SQL Server Code Samples.
  Copyright (C) Microsoft Corporation.  All rights reserved.

  This source code is intended only as a supplement to Microsoft
  Development Tools and/or on-line documentation.  See these other
  materials for detailed information regarding Microsoft code samples.

  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
======================================================= */

#region Using directives

using System;
using System.Data.SqlTypes;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.SqlServer.Server;

#endregion

namespace CustomsAtom.SqlUtilities
{
    /// <summary>
    /// This class is provides regular expression operations for Transact-SQL callers
    /// </summary>
    public sealed class Encryptor
    {
        // Fields
        private static readonly string key = "adgaw334^*^&#$#$W2343qwreqwr12";
        private static readonly SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();

        private Encryptor()
        {
        }

        [SqlFunction]
        public static string Decrypt(SqlString sqlInput)
        {
            string source = (sqlInput.IsNull) ? string.Empty : sqlInput.Value;
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

        [SqlFunction]
        public static string Encrypt(SqlString sqlInput)
        {
            string source = (sqlInput.IsNull) ? string.Empty : sqlInput.Value;
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
            return Encoding.ASCII.GetBytes(s);
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
            return Encoding.ASCII.GetBytes(s);
        }
    }
}