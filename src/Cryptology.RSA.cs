using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        private static readonly Dictionary<RSAEncryptionPadding, int> dicPaddingLimit = new Dictionary<RSAEncryptionPadding, int>()
        {
            [RSAEncryptionPadding.Pkcs1] = 11,
            [RSAEncryptionPadding.OaepSHA1] = 42,
            [RSAEncryptionPadding.OaepSHA256] = 66,
            [RSAEncryptionPadding.OaepSHA384] = 98,
            [RSAEncryptionPadding.OaepSHA512] = 130,
        };

        /// <summary>
        /// 导出RSA公钥
        /// </summary>
        /// <param name="rsa">令牌</param>
        /// <param name="type">类型枚举</param>
        /// <param name="isPemFormat">是否转换成PEM格式,仅当私钥类型为PKCS#1和PKCS#8时有效</param>
        /// <returns></returns>
        public static string ExportPublicKey(this RSA rsa, RsaType type, bool isPemFormat = false)
        {
            var key = type switch
            {
                RsaType.Pkcs1 => Convert.ToBase64String(rsa.ExportRSAPublicKey()),
                RsaType.Pkcs8 => Convert.ToBase64String(rsa.ExportPK8PublicKey()),
                RsaType.Xml => rsa.ExportXmlPublicKey(),
                _ => string.Empty
            };

            if (type != RsaType.Xml && isPemFormat)
                key = type.GetPublicKeyFormat(key);

            return key;
        }

        /// <summary>
        /// 导出RSA私钥
        /// </summary>
        /// <param name="rsa">令牌</param>
        /// <param name="type">类型枚举</param>
        /// <param name="isPemFormat">是否转换成PEM格式,仅当私钥类型为PKCS#1和PKCS#8时有效</param>
        /// <returns></returns>
        public static string ExportPrivateKey(this RSA rsa, RsaType type, bool isPemFormat = false)
        {
            var key = type switch
            {
                RsaType.Pkcs1 => Convert.ToBase64String(rsa.ExportRSAPrivateKey()),
                RsaType.Pkcs8 => Convert.ToBase64String(rsa.ExportPkcs8PrivateKey()),
                RsaType.Xml => rsa.ExportXmlPrivateKey(),
                _ => string.Empty
            };

            if (type != RsaType.Xml && isPemFormat)
                key = type.GetPrivateKeyFormat(key);

            return key;
        }

        /// <summary>
        /// 导入RSA私钥
        /// </summary>
        /// <param name="rsa">令牌</param>
        /// <param name="type">类型枚举</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="isPemFormat">是否转换成PEM格式,仅当私钥类型为PKCS#1和PKCS#8时有效</param>
        /// <returns></returns>
        public static void ImportPrivateKey(this RSA rsa, RsaType type, string privateKey, bool isPemFormat = false)
        {
            if (isPemFormat)
                privateKey = privateKey.RemoveFormat();

            switch (type)
            {
                case RsaType.Pkcs1:
                    rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                    break;
                case RsaType.Pkcs8:
                    rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
                    break;
                case RsaType.Xml:
                    rsa.ImportXmlPrivateKey(privateKey);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        /// 导入RSA公钥
        /// </summary>
        /// <param name="rsa">令牌</param>
        /// <param name="type">类型枚举</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="isPemFormat">是否转换成PEM格式,仅当私钥类型为PKCS#1和PKCS#8时有效</param>
        /// <returns></returns>
        public static void ImportPublicKey(this RSA rsa, RsaType type, string publicKey, bool isPemFormat = false)
        {
            if (isPemFormat)
                publicKey = publicKey.RemoveFormat();

            switch (type)
            {
                case RsaType.Pkcs1:
                    rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                    break;
                case RsaType.Pkcs8:
                    rsa.ImportPK8PublicKey(Convert.FromBase64String(publicKey));
                    break;
                case RsaType.Xml:
                    rsa.ImportXmlPublicKey(publicKey);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        /// 加签
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="dataString"></param>
        /// <param name="padding"></param>
        /// <param name="trimChar"></param>
        /// <returns></returns>
        public static string EncryptBigData(this RSA rsa, string dataString, RSAEncryptionPadding padding, char trimChar = '$')
        {
            var pointer = 0;
            var data = Encoding.UTF8.GetBytes(dataString);
            var splitLength = (rsa.KeySize / 8) - dicPaddingLimit[padding];

            var sb = new StringBuilder();
            for (int i = 0; i < Convert.ToInt32(Math.Ceiling(data.Length * 1.0 / splitLength)); i++)
            {
                byte[] current = pointer + splitLength < data.Length ? data[pointer..(pointer + splitLength)] : data[pointer..];
                sb.Append(Convert.ToBase64String(rsa.Encrypt(current, padding)));
                sb.Append(trimChar);
                pointer += splitLength;
            }
            return sb.ToString().TrimEnd(trimChar);
        }

        /// <summary>
        /// 解签
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="dataString"></param>
        /// <param name="padding"></param>
        /// <param name="trimChar"></param>
        /// <returns></returns>
        public static string DecryptBigData(this RSA rsa, string dataString, RSAEncryptionPadding padding, char trimChar = '$')
        {
            var data = dataString.Split(trimChar, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.Append(Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(item), padding)));
            }
            return sb.ToString();
        }
    }
}
