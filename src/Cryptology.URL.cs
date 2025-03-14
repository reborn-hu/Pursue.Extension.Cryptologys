using System.Net;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        /// <summary>
        /// 获取一个编码的UrlEncode字符串
        /// </summary>
        /// <param name="input">需要编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string EncodeUrl(this string input) => WebUtility.UrlEncode(input);

        /// <summary>
        ///  获取一个反UrlEncode编码的字符串
        /// </summary>
        /// <param name="input">需要反编码的字符串</param>
        /// <returns>反编码后的字符串</returns>
        public static string DecodeUrl(this string input) => WebUtility.UrlDecode(input);

        /// <summary>
        /// MFA使用的UrlEncode
        /// </summary>
        /// <param name="input">需要编码的字符串</param>
        /// <returns></returns>
        public static string EncodeMFA(this string input)
        {
            var result = new StringBuilder();
            var validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            foreach (var symbol in input)
                if (validChars.IndexOf(symbol) != -1)
                    result.Append(symbol);
                else
                    result.Append('%' + string.Format("{0:X2}", (int)symbol));

            return result.ToString().Replace(" ", "%20");
        }
    }
}