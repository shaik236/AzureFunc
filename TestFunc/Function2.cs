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
    public static class Function2
    {
        [FunctionName("decrypt")]
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
            string key = "";

            if (!(string.IsNullOrEmpty(text)))
            {
                cipherText = CryptoFunctions.DecryptText(key, text);
            }

            string responseMessage = string.IsNullOrEmpty(text)
                ? ""
                : cipherText;

            return new OkObjectResult(responseMessage);
        }

    }
}
