using System.Collections.Generic;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        public static KeyValuePair<string, string> CreatePassword(this string input)
        {
            string gUID = GUID;
            string value = ("$$" + input + "$$.$$" + gUID + "$$").EncodeDES(gUID.Substring(0, 16));
            return new KeyValuePair<string, string>(gUID, value);
        }

        public static string VerifyPassword(this string input, string key)
        {
            return ("$$" + input + "$$.$$" + key + "$$").EncodeDES(key.Substring(0, 16));
        }

        public static string CreateToken(this object input)
        {
            return $"$${input}$$_$$PURSUE_REBORN$$".EncodeAES("$$PURSUEREBORN$$");
        }
    }
}
