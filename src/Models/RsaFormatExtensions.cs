using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    internal static class RsaFormatExtensions
    {
        internal static string GetPublicKeyFormat(this RsaType Type, string PublicKey)
        {
            return Type switch
            {
                RsaType.Pkcs1 => GetPK1PublicKey(PublicKey),
                RsaType.Pkcs8 => GetPK8PublicKey(PublicKey),
                _ => throw new Exception($"公钥类型{Type}不支持pem格式"),
            };
        }

        internal static string GetPrivateKeyFormat(this RsaType Type, string PrivateKey)
        {
            return Type switch
            {
                RsaType.Pkcs1 => GetPK1PrivateKey(PrivateKey),
                RsaType.Pkcs8 => GetPK8PrivateKey(PrivateKey),
                _ => throw new Exception($"私钥类型{Type}不支持pem格式"),
            };
        }

        internal static byte[] ExportPK8PublicKey(this RSA Rsa)
        {
            var param = Rsa.ExportParameters(false);
            var paramRsaKey = new RsaKeyParameters(false, new BigInteger(1, param.Modulus), new BigInteger(1, param.Exponent));

            using (var context = new StringWriter())
            {
                var pem = new PemWriter(context);

                pem.WriteObject(paramRsaKey);

                return Convert.FromBase64String(context.ToString().RemoveFormat());
            }
        }

        internal static void ImportPK8PublicKey(this RSA Rsa, byte[] PublicKey)
        {
            var paramRsaKey = (RsaKeyParameters)PublicKeyFactory.CreateKey(PublicKey);
            var param = new RSAParameters
            {
                Modulus = paramRsaKey.Modulus.ToByteArrayUnsigned(),
                Exponent = paramRsaKey.Exponent.ToByteArrayUnsigned()
            };
            Rsa.ImportParameters(param);
        }

        internal static string RemoveFormat(this string Key)
        {
            return Key
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("-----BEGIN RSA PUBLIC KEY-----", "").Replace("-----END RSA PUBLIC KEY-----", "")
                .Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "")
                .Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "")
                .Replace(Environment.NewLine, "");
        }

        internal static string GetPK1PublicKey(string PublicKey)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-----BEGIN RSA PUBLIC KEY-----");
            AppendBody(sb, PublicKey);
            sb.AppendLine("-----END RSA PUBLIC KEY-----");
            return sb.ToString();
        }

        internal static string GetPK1PrivateKey(string PublicKey)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-----BEGIN RSA PRIVATE KEY-----");
            AppendBody(sb, PublicKey);
            sb.AppendLine("-----END RSA PRIVATE KEY-----");
            return sb.ToString();
        }

        internal static string GetPK8PublicKey(string PublicKey)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-----BEGIN PUBLIC KEY-----");
            AppendBody(sb, PublicKey);
            sb.AppendLine("-----END PUBLIC KEY-----");

            return sb.ToString();
        }

        internal static string GetPK8PrivateKey(string PublicKey)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-----BEGIN PRIVATE KEY-----");
            AppendBody(sb, PublicKey);
            sb.AppendLine("-----END PRIVATE KEY-----");
            return sb.ToString();
        }

        internal static void AppendBody(StringBuilder Sb, string Key)
        {
            var count = Convert.ToInt32(Math.Ceiling(Key.Length * 1.0 / 64));
            for (int i = 0; i < count; i++)
            {
                int start = i * 64, end = start + 64;

                if (end >= Key.Length)
                    end = Key.Length;

                Sb.AppendLine(Key[start..end]);
            }
        }
    }
}