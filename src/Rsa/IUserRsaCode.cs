using System.Security.Cryptography;

namespace Pursue.Extension.Cryptologys
{
    public interface IUserRsaCode
    {
        /// <summary>
        /// 生成用户临时证书
        /// </summary>
        /// <returns></returns>
        UserRsaResponse CreateUserRsa();

        /// <summary>
        /// 获取用户临时证书
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        RSA QueryUserRsa(string code);

        /// <summary>
        /// 置换通过公钥加密的报文信息转换成对于参数对象
        /// </summary>
        /// <typeparam name="TEntity">参数对象类型</typeparam>
        /// <param name="code">令牌</param>
        /// <param name="text">数据密文串</param>
        /// <returns></returns>
        TEntity DecryptDataToObject<TEntity>(string code, string text) where TEntity : class, new();
    }
}