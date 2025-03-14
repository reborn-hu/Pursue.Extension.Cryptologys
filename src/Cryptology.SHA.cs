using System;
using System.Security.Cryptography;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        /// <summary>
        /// SHA512算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DecodeSha512(this string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] array = SHA512.Create().ComputeHash(bytes);
            var stringBuilder = new StringBuilder();
            foreach (byte b in array)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// HmacSHA256算法,返回的结果始终是32位
        /// </summary>
        /// <param name="input"> 加密内容-响应消息体 待加密的内容</param>
        /// <param name="encryptKey"> 秘钥key 加密的键，可以是任何数据</param>
        /// <param name="encoding"> 字符集</param>
        /// <returns>加密密文</returns>
        public static string EncodeHmacSha(this string input, string encryptKey, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;

            using (var hmacsha256 = new HMACSHA256(encoding.GetBytes(encryptKey)))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(encoding.GetBytes(input));
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
