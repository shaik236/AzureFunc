using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace TestFunc
{
    public static class Function1
    {
        [FunctionName("encrypt")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processing a request.");

            string text = string.Empty;

            string body = await req.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(body))
            {
                log.LogInformation("Request body is empty");
            }
            else
            {
                dynamic b = JObject.Parse(body);
                text = b.text;
            }

            string cipherText = string.Empty;
            string key = "ADSasdADasdfASDasdsad";

            if (!(string.IsNullOrEmpty(text)))
            {
                cipherText = EncryptAES(key, text);
            }

            string responseMessage = string.IsNullOrEmpty(text)
                ? ""
                : cipherText;

            return new OkObjectResult(responseMessage);
        }


        public static string EncryptAES(string key, string input)
        {
            //if (input.Length % 2 == 1) 
            //{ 
            // input = input + "0"; 
            //} 
            byte[] keyBuffer = StringToByteArray(key);
            // byte[] inputBuffer = StringToByteArray(getHexString(input)); 
            byte[] inputBuffer = Encoding.ASCII.GetBytes(input);
            byte[] cipherbuffer = AESEncrypt(inputBuffer, keyBuffer);
            string s = BitConverter.ToString(cipherbuffer).Replace("-", "");
            return BitConverter.ToString(cipherbuffer).Replace("-", "");
        }

        public static byte[] StringToByteArray(string hex)
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

        public static byte[] AESEncrypt(byte[] original, byte[] key)
        {
            AesCryptoServiceProvider cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.Key = key;
            cryptoProvider.Mode = CipherMode.ECB;
            cryptoProvider.Padding = PaddingMode.Zeros;
            ICryptoTransform transform = cryptoProvider.CreateEncryptor();
            return transform.TransformFinalBlock(original, 0, original.Length);
        }
        public static int GetHexVal(char hex)
        {
            int num = hex; return (num - ((num < 58) ? 48 : 55));
        }

    }

    


}
