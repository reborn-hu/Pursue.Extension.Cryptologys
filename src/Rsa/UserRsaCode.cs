using Newtonsoft.Json;
using Pursue.Extension.Cache;
using System;
using System.Security.Cryptography;

namespace Pursue.Extension.Cryptologys
{
    public sealed class UserRsaCode : IUserRsaCode
    {
        private readonly RedisClient _redisClient;

        public UserRsaCode()
        {
            _redisClient = CacheFactory.GetRedisClient();
        }

        /// <summary>
        /// 生成用户临时证书
        /// </summary>
        /// <returns></returns>
        public UserRsaResponse CreateUserRsa()
        {
            try
            {
                var ras = RSA.Create();
                var publicKey = ras.ExportPublicKey(RsaType.Pkcs8, true);
                var privateKey = ras.ExportPrivateKey(RsaType.Pkcs8, true);

                var code = Cryptology.GuidString();

                var rsaKey = $"{RsaUserConst.REDIS_OAUTH_RSA}:{code}";

                _redisClient.StartPipe(scope =>
                {
                    scope.HSet(rsaKey, RsaUserConst.REDIS_OAUTH_PUBLIC_DKEY, publicKey);
                    scope.HSet(rsaKey, RsaUserConst.REDIS_OAUTH_PRIVATE_DKEY, privateKey);
                    scope.Expire(rsaKey, RsaUserConst.REDIS_OAUTH_IDCODE_EXPIRES * 2);
                });

                return new UserRsaResponse { Code = code, Key = publicKey };
            }
            catch (Exception)
            {
                throw new TimeoutException("操作超时");
            }
        }

        /// <summary>
        /// 置换通过公钥加密的报文信息转换成对于参数对象
        /// </summary>
        /// <typeparam name="TEntity">参数对象类型</typeparam>
        /// <param name="code">令牌</param>
        /// <param name="text">数据密文串</param>
        /// <returns></returns>
        public TEntity DecryptDataToObject<TEntity>(string code, string text) where TEntity : class, new()
        {
            try
            {
                var rsa = QueryUserRsa(code);
                var body = rsa.DecryptBigData(text, RSAEncryptionPadding.Pkcs1);

                return JsonConvert.DeserializeObject<TEntity>(body);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取用户临时证书
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public RSA QueryUserRsa(string code)
        {
            try
            {
                var rsaKey = $"{RsaUserConst.REDIS_OAUTH_RSA}:{code}";

                var publicKey = _redisClient.HGet(rsaKey, RsaUserConst.REDIS_OAUTH_PUBLIC_DKEY);
                var privateKey = _redisClient.HGet(rsaKey, RsaUserConst.REDIS_OAUTH_PRIVATE_DKEY);

                if (string.IsNullOrEmpty(publicKey) && string.IsNullOrEmpty(privateKey))
                {
                    return null;
                }

                var ras = RSA.Create();
                ras.ImportPublicKey(RsaType.Pkcs8, publicKey, true);
                ras.ImportPrivateKey(RsaType.Pkcs8, privateKey, true);

                _redisClient.Del(rsaKey);

                return ras;
            }
            catch
            {
                return null;
            }
        }
    }
}