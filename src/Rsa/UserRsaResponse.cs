namespace Pursue.Extension.Cryptologys
{
    public sealed class UserRsaResponse
    {
        /// <summary>
        /// 交换令牌
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public string Key { get; set; }
    }
}
