using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TestFunc
{
    public static class CryptoFunctions
    {
        private static string cipherkey = "argasgasgasgasgd(|A";

        public static string EncryptText(string key, string input)
        {
            key = cipherkey;
            byte[] keyBuffer = StringToByteArray(key);
            byte[] inputBuffer = Encoding.ASCII.GetBytes(input);
            byte[] cipherbuffer = AESEncrypt(inputBuffer, keyBuffer);
            string s = BitConverter.ToString(cipherbuffer).Replace("-", "");
            return BitConverter.ToString(cipherbuffer).Replace("-", "");
        }

        private static byte[] StringToByteArray(string hex)
        {
            if ((hex.Length % 2) == 1)
            {
                throw new Exception("The binary key or input text cannot have an odd number of digits");
            }
            byte[] buffer = new byte[hex.Length >> 1];
            for (int i = 0; i < (hex.Length >> 1); i++)
            {
                buffer[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));
            }
            return buffer;
        }

        private static byte[] AESEncrypt(byte[] original, byte[] key)
        {
            AesCryptoServiceProvider cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.Key = key;
            cryptoProvider.Mode = CipherMode.ECB;
            cryptoProvider.Padding = PaddingMode.Zeros;
            ICryptoTransform transform = cryptoProvider.CreateEncryptor();
            return transform.TransformFinalBlock(original, 0, original.Length);
        }
        private static int GetHexVal(char hex)
        {
            int num = hex; return (num - ((num < 58) ? 48 : 55));
        }

        public static string DecryptText(string key, string input)
        {
            key = cipherkey;
            byte[] keyBuffer = StringToByteArray(key);
            byte[] inputBuffer = StringToByteArray(input);
            string decryptedText = AESDecrypt(inputBuffer, keyBuffer);
            return decryptedText;
        }

        private static string AESDecrypt(byte[] original, byte[] key)
        {
            AesCryptoServiceProvider cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.Key = key;
            cryptoProvider.Mode = CipherMode.ECB;
            cryptoProvider.Padding = PaddingMode.None;

            ICryptoTransform transform = cryptoProvider.CreateDecryptor();

            byte[] origValue = transform.TransformFinalBlock(original, 0, original.Length);

            string result = Encoding.UTF8.GetString(origValue).Replace("\0", "");

            return result;
        }
    }
}
