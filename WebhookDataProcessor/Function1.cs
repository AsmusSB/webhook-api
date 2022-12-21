using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace WebhookDataProcessor
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([ServiceBusTrigger("webhook-added", "send-email", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"SEND EMAIL: {mySbMsg}");
        }

        [FunctionName("UpdateReport")]
        public static void Run1([ServiceBusTrigger("webhook-added", "update-report", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"UPDATE REPORT: {mySbMsg}");
        }
    }
}
