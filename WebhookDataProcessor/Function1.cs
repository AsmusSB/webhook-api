using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebhookDataProcessor
{
    public static class Function1
    {
        //public static readonly WebhookService _webhookService;


        //[FunctionName("SendAllWebhooksForTriggerDocumentUploaded")]
        //public static async Task Run([ServiceBusTrigger("webhook", "document-uploaded", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        //{
        //    await _webhookService.SendAllWebhooksWhereTriggerDocumentUploaded();
        //    logger.LogInformation("WEBHOOKS SENT FOR UPLOADED DOCUMENTS");
        //}



        [FunctionName("UpdateReport")]
        public static void Run1([ServiceBusTrigger("webhook-added", "update-report", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"UPDATE REPORT: {mySbMsg}");
        }

        [FunctionName("SendWebhhook")]
        public static void Run2([ServiceBusTrigger("webhook-added", "send-webhook", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"SEND WEBHOOK: {mySbMsg}");
        }
        [FunctionName("Function1")]
        public static void Run3([ServiceBusTrigger("webhook-added", "send-email", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"SEND EMAIL: {mySbMsg}");
        }
    }
}
