using System;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        /// <summary>
        /// 获取一个编码的Base64字符串
        /// </summary>
        /// <param name="input">需要编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string EncodeBase64(this string input) => Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

        /// <summary>
        /// 获取一个解码的字符串
        /// </summary>
        /// <param name="input">需要反编码的字符串</param>
        /// <returns>反编码后的字符串</returns>
        public static string DecodeBase64(this string input) => Encoding.UTF8.GetString(Convert.FromBase64String(input));
    }
}
