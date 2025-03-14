namespace Pursue.Extension.Cryptologys
{
    internal sealed class RsaUserConst
    {
        /// <summary>
        /// IdCode 有效时间 
        /// </summary>
        internal const int REDIS_OAUTH_IDCODE_EXPIRES = 60;

        /// <summary>
        /// Oauth缓存加密键
        /// </summary>
        internal const string REDIS_OAUTH_RSA = "Oauth:Rsa";

        /// <summary>
        /// HASH PublicKey DataKey
        /// </summary>
        internal const string REDIS_OAUTH_PUBLIC_DKEY = "PublicKey";

        /// <summary>
        /// HASH privateKey DataKey
        /// </summary>
        internal const string REDIS_OAUTH_PRIVATE_DKEY = "privateKey";
    }
}