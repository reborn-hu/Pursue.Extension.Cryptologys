using System;
using System.Security.Cryptography;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        /// <summary>
        /// 获取一个加密的MD5字符串
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5String(this string input)
        {
            using var md5 = MD5.Create();
            var strResult = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "");
            return strResult;
        }
    }
}
