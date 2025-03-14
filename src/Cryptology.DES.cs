using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        private static readonly byte[] _keys = { 0x00, 0x0E, 0x02, 0x04, 0x05, 0x09, 0x03, 0x06, 0x07, 0x01, 0x08, 0x0A, 0x0B, 0x0C, 0x0D, 0x0F };

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="input">需加密字段</param>
        /// <param name="encryptKey">加密密文</param>
        /// <returns>加密后的密文字符串</returns>
        public static string EncodeDES(this string input, string encryptKey = "48273ECB1524F95PURSUE")
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
            byte[] rgbIV = _keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(input);
            using (var DCSP = Aes.Create())
            {
                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cStream.FlushFinalBlock();
                        return Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            ;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input">需解密字段</param>
        /// <param name="decryptKey">解密密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeDES(this string input, string decryptKey = "48273ECB1524F95PURSUE")
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
            byte[] rgbIV = _keys;
            byte[] inputByteArray = Convert.FromBase64String(input);
            using (var DCSP = Aes.Create())
            {
                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
        }
    }
}
