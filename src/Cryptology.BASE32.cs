﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pursue.Extension.Cryptologys
{
    public static partial class Cryptology
    {
        private static readonly string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string EncodeBase32(this string accountSecretKey)
        {
            var output = "";

            var data = Encoding.UTF8.GetBytes(accountSecretKey);
            for (var bitIndex = 0; bitIndex < data.Length * 8; bitIndex += 5)
            {
                var dualbyte = data[bitIndex / 8] << 8;
                if (bitIndex / 8 + 1 < data.Length)
                    dualbyte |= data[bitIndex / 8 + 1];
                dualbyte = 0x1f & (dualbyte >> (16 - bitIndex % 8 - 5));
                output += allowedCharacters[dualbyte];
            }

            return output;
        }

        public static string DecodeBase32(this string base32)
        {
            var output = new List<byte>();

            var bytes = base32.ToCharArray();
            for (var bitIndex = 0; bitIndex < base32.Length * 5; bitIndex += 8)
            {
                var dualbyte = allowedCharacters.IndexOf(bytes[bitIndex / 5]) << 10;
                if (bitIndex / 5 + 1 < bytes.Length)
                    dualbyte |= allowedCharacters.IndexOf(bytes[bitIndex / 5 + 1]) << 5;
                if (bitIndex / 5 + 2 < bytes.Length)
                    dualbyte |= allowedCharacters.IndexOf(bytes[bitIndex / 5 + 2]);

                dualbyte = 0xff & (dualbyte >> (15 - bitIndex % 5 - 8));
                output.Add((byte)dualbyte);
            }

            var key = Encoding.UTF8.GetString(output.ToArray());
            if (key.EndsWith("\0"))
            {
                key = key.Remove(key.IndexOf("\0", StringComparison.Ordinal), 1);
            }

            return key;
        }
    }
}