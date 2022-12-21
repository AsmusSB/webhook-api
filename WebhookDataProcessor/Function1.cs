using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebhookDataProcessor
{
    public static class Function1
    {
        [FunctionName("SendEmail")]
        public static void Run([ServiceBusTrigger("webhook-added", "send-email", Connection = "WebhookDataConnection")]string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"SEND EMAIL: {mySbMsg}");
        }

        [FunctionName("UpdateReport")]
        public static void Run1([ServiceBusTrigger("webhook-added", "update-report", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"UPDATE REPORT: {mySbMsg}");
        }

        //[FunctionName("SendWebhook")]
        //public static void Run2([ServiceBusTrigger("webhook-added", "send-webhook", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        //{
        //    //var whConfig = JsonConvert.DeserializeObject(mySbMsg, new JsonSerializerSettings
        //    //{
        //    //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    //});

        //    //ExecuteWebhookWithPollyRetry(whStatus)
        //    logger.LogInformation($"SEND WEBHOOK: {mySbMsg}");
        //}
    }
}
