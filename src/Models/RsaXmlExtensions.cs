using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Pursue.Extension.Cryptologys
{
    internal static class RsaXmlExtensions
    {
        /// <summary>
        /// 导入XML私钥
        /// </summary>
        /// <param name="Rsa">令牌</param>
        /// <param name="PrivateKey">私钥</param>
        internal static void ImportXmlPrivateKey(this RSA Rsa, string PrivateKey)
        {
            try
            {
                var root = XElement.Parse(PrivateKey);
                var param = new RSAParameters()
                {
                    Modulus = Convert.FromBase64String(root.Element("Modulus").Value),
                    Exponent = Convert.FromBase64String(root.Element("Exponent").Value),
                    P = Convert.FromBase64String(root.Element("P").Value),
                    Q = Convert.FromBase64String(root.Element("Q").Value),
                    D = Convert.FromBase64String(root.Element("D").Value),
                    DP = Convert.FromBase64String(root.Element("DP").Value),
                    DQ = Convert.FromBase64String(root.Element("DQ").Value),
                    InverseQ = Convert.FromBase64String(root.Element("InverseQ").Value),
                };
                Rsa.ImportParameters(param);
            }
            catch (Exception ex)
            {
                throw new Exception("XML私钥不正确", ex);
            }
        }

        /// <summary>
        /// 导入XML公钥
        /// </summary>
        /// <param name="Rsa">令牌</param>
        /// <param name="PublicKey">公钥</param>
        internal static void ImportXmlPublicKey(this RSA Rsa, string PublicKey)
        {
            try
            {
                var root = XElement.Parse(PublicKey);
                var param = new RSAParameters()
                {
                    Modulus = Convert.FromBase64String(root.Element("Modulus").Value),
                    Exponent = Convert.FromBase64String(root.Element("Exponent").Value),
                };
                Rsa.ImportParameters(param);
            }
            catch (Exception ex)
            {
                throw new Exception("XML公钥不正确", ex);
            }
        }

        /// <summary>
        /// 导出XML私钥
        /// </summary>
        /// <param name="Rsa">令牌</param>
        /// <returns></returns>
        internal static string ExportXmlPrivateKey(this RSA Rsa)
        {
            var param = Rsa.ExportParameters(true);
            var element = new XElement("RsaKeyValue");
            element.Add(new XElement("Modulus", Convert.ToBase64String(param.Modulus)));
            element.Add(new XElement("Exponent", Convert.ToBase64String(param.Exponent)));
            element.Add(new XElement("P", Convert.ToBase64String(param.P)));
            element.Add(new XElement("Q", Convert.ToBase64String(param.Q)));
            element.Add(new XElement("D", Convert.ToBase64String(param.D)));
            element.Add(new XElement("DP", Convert.ToBase64String(param.DP)));
            element.Add(new XElement("DQ", Convert.ToBase64String(param.DQ)));
            element.Add(new XElement("InverseQ", Convert.ToBase64String(param.InverseQ)));
            return element.ToString();
        }

        /// <summary>
        ///  导出XML公钥
        /// </summary>
        /// <param name="Rsa">令牌</param>
        /// <returns></returns>
        internal static string ExportXmlPublicKey(this RSA Rsa)
        {
            var param = Rsa.ExportParameters(false);
            var element = new XElement("RsaKeyValue");
            element.Add(new XElement("Modulus", Convert.ToBase64String(param.Modulus)));
            element.Add(new XElement("Exponent", Convert.ToBase64String(param.Exponent)));
            return element.ToString();
        }
    }
}