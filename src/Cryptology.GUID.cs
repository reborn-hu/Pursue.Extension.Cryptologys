using System;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        /// <summary>
        /// 获取没有"-"符号分割的GUID
        /// </summary>
        public static string GUID => Guid.NewGuid().ToString("N");

        /// <summary>
        /// 获取GUID
        /// </summary>
        public static string GuidSymbol() => Guid.NewGuid().ToString();

        /// <summary>
        /// 获取没有"-"符号分割的GUID
        /// </summary>
        public static string GuidString() => Guid.NewGuid().ToString("N");
    }
}
