using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace warmup
{
    public class TimerTriggerFunction
    {
        private static readonly HttpClient httpClient = new HttpClient();
        [FunctionName("TimerTriggerFunction")]
        public static async Task Run([TimerTrigger("0 */45 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            string apiUrl = Environment.GetEnvironmentVariable("API_URL");

            if (string.IsNullOrEmpty(apiUrl))
            {
                log.LogError("API_URL environment variable is not set.");
                return;
            }
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex) 
            { 
                log.LogError($"Error making HTTP request: {ex.Message}"); 
            }
        }
    }
}
