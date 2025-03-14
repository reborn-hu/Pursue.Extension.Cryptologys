using System;
using System.Text;

namespace Pursue.Extension.Cryptologys 
{
    /// <summary>
    /// String操作扩展
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// 字节转16进制字符串
        /// </summary>
        /// <param name="byteDatas">字节</param>
        /// <returns></returns>
        internal static string ToHexString(this byte[] byteDatas)
        {
            if (byteDatas == null || byteDatas.Length == 0)
                return "";

            var hexString = new StringBuilder(64);

            for (int i = 0; i < byteDatas.Length; i++)
            {
                hexString.Append(string.Format("{0:x2}", byteDatas[i]));
            }

            return hexString.ToString().Trim();
        }

        /// <summary>
        /// 16进制字符串转字节
        /// </summary>
        /// <param name="hexString">16进制字符串</param>
        /// <returns></returns>
        internal static byte[] FromHexString(this string hexString)
        {
            if (hexString.Length % 2 != 0)
                hexString += " ";

            var returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return returnBytes;
        }
    }
}
