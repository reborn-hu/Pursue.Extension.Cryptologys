using System;
using System.Security.Cryptography;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        public static int Hash(this string secret, long iterationNumber, int digits = 6)
        {
            var counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(counter);

            var hash = new HMACSHA1(Encoding.UTF8.GetBytes(secret)).ComputeHash(counter);

            var offset = hash[hash.Length - 1] & 0xf;

            var binary = ((hash[offset] & 0x7f) << 24) | (hash[offset + 1] << 16) | (hash[offset + 2] << 8) | hash[offset + 3];

            var password = binary % (int)Math.Pow(10, digits);

            return password;
        }
    }
}