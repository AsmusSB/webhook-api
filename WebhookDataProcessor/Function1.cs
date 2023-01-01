using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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

        [FunctionName("SendWebhhook")]
        public static void Run2([ServiceBusTrigger("webhook-added", "send-webhook", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            WebhookConfiguration config = JsonConvert.DeserializeObject<WebhookConfiguration>(mySbMsg);
            List<WebhookStatus> statusList = config.Webhooks;
            foreach (var v in statusList)
            {
                //ExecuteWebhookWithPollyRetry(v);
                logger.LogInformation($"SEND WEBHOOK: {mySbMsg}");
            }
        }

        [FunctionName("SendAllWebhooksForTriggerDocumentUploaded")]
        public static void Run3([ServiceBusTrigger("webhook-added", "document-uploaded", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            //_webhookService.SendAllWebhooksWhereTriggerDocumentUploaded();
            logger.LogInformation($"WEBHOOKS SENT FOR UPLOADED DOCUMENTS: {mySbMsg}");
        }


        public class WebhookConfiguration
        {
            public List<Header>? Headers { get; set; }
            [DataType(DataType.Url)]
            public string DestinationUrl { get; set; }
            public string TenantId { get; set; }
            public int TryCount { get; set; }
            public int RetryTimeSpan { get; set; }
            public List<WebhookStatus> Webhooks { get; set; }

            [Key]
            public int Id { get; set; }
            [InverseProperty(nameof(WebhookStatus.Config))]
            private ICollection<List<WebhookStatus>> WebhookStatusCollection { get; set; }
            [InverseProperty(nameof(Header.Config))]
            private ICollection<List<Header>> HeadersCollection { get; set; }
        }

        public class Header
        {
            public string HeaderName { get; set; }
            public string HeaderValue { get; set; }
            [ForeignKey("WebhookHeader")]
            
            [Key]
            public int Id { get; set; }
            public WebhookConfiguration Config { get; set; }
        }

        public class WebhookStatus
        {
            public string Body { get; set; }
            public string TriggerEvent { get; set; }
            public int TimesSuccessfullyFired { get; set; }
            public int CurrentFailedAttempts { get; set; }
            public string Status { get; set; }
            [ForeignKey("WebhookStatus")]
            [Key]
            public int Id { get; set; }
            public WebhookConfiguration Config { get; set; }
        }
    }
}
