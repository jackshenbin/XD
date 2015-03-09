using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BOCOM.IVX.Controls
{
    /// <summary>
    /// desº”√‹À„∑®ø‚
    /// </summary>
    public class DesKey
    {
        private static byte[] KEY = { 66, 79, 67, 79, 77, 49, 50, 51 };//BOCOM123
        private static byte[] IV = { 98, 111, 99, 111, 109, 49, 50, 51 };//bocom123

        public static string Encrypt(string PlainText)
        {
            Deskey.Key = KEY;
            Deskey.IV = IV;
            return ByteToStr(Encrypt(PlainText, Deskey));
        }
        public static string Decrypt(string CypherText)
        {
            Deskey.Key = KEY;
            Deskey.IV = IV;
            string s = null;
            while (CypherText.Length > 0)
            {
                s = (char)Convert.ToInt32(CypherText.Substring(CypherText.Length - 2, 2), 16) + s;
                CypherText = CypherText.Substring(0, CypherText.Length - 2);
            }
            return Decrypt(StrToByte(s), Deskey);
        }

        static byte[] Encrypt(string PlainText, SymmetricAlgorithm key)
        {
            try
            {
                // Create a memory stream.
                MemoryStream ms = new MemoryStream();

                // Create a CryptoStream using the memory stream and the 
                // CSP DES key.  
                CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);

                // Create a StreamWriter to write a string
                // to the stream.
                StreamWriter sw = new StreamWriter(encStream);

                // Write the plaintext to the stream.
                sw.WriteLine(PlainText);

                // Close the StreamWriter and CryptoStream.
                sw.Close();
                encStream.Close();

                // Get an array of bytes that represents
                // the memory stream.
                byte[] buffer = ms.ToArray();

                // Close the memory stream.
                ms.Close();

                // Return the encrypted byte array.
                return buffer;
            }
            catch
            {
                return null;
            }
        }
        static byte[] StrToByte(string s)
        {
            byte[] b = null;
            if (s != null && s.Length > 0)
            {
                b = new byte[s.Length];
                for (int i = 0; i < s.Length; i++)
                {
                    b[i] = (byte)s.ToCharArray()[i];
                }
            }
            return b;
        }
        static string ByteToStr(byte[] b)
        {
            string s = null;
            if (b != null && b.Length > 0)
            {
                for (int i = 0; i < b.Length; i++)
                {

                    s += string.Format("{0:X2}", b[i]);
                }

            }
            return s;
        }
        static DESCryptoServiceProvider Deskey = new DESCryptoServiceProvider();

        // Decrypt the byte array.
        static string Decrypt(byte[] CypherText, SymmetricAlgorithm key)
        {
            try
            {
                // Create a memory stream to the passed buffer.
                MemoryStream ms = new MemoryStream(CypherText);

                // Create a CryptoStream using the memory stream and the 
                // CSP DES key. 
                CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);

                // Create a StreamReader for reading the stream.
                StreamReader sr = new StreamReader(encStream);

                // Read the stream as a string.
                string val = sr.ReadLine();

                // Close the streams.
                sr.Close();
                encStream.Close();
                ms.Close();

                return val;
            }
            catch
            {
                return null;
            }
        }

    }
}
