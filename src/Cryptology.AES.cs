using System.Security.Cryptography;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        private const string Key = "78a36443538e450f8bbb5f1c512462f9";
        private const string Iv = "aca02afa4f8576fb";

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="key">秘钥</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        public static string EncodeAES(this string input, string key = Key, string iv = Iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var transform = aes.CreateEncryptor())
                {
                    var encryptArray = Encoding.UTF8.GetBytes(input);
                    var resultArray = transform.TransformFinalBlock(encryptArray, 0, encryptArray.Length);

                    return resultArray.ToHexString();
                }
            }
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <param name="key">秘钥</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        public static string DecodeAES(this string input, string key = Key, string iv = Iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var cTransform = aes.CreateDecryptor())
                {
                    var dncryptArray = input.FromHexString();
                    var resultArray = cTransform.TransformFinalBlock(dncryptArray, 0, dncryptArray.Length);

                    return Encoding.UTF8.GetString(resultArray);
                }
            }
        }
    }
}