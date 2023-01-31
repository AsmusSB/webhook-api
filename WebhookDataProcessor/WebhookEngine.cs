using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
//using webhook_api.Services;

namespace WebhookDataProcessor
{
    public static class WebhookEngine
    {
        //public static readonly WebhookService _webhookService;


        //[FunctionName("SendAllWebhooksForTriggerDocumentUploaded")]
        //public static async Task Run([ServiceBusTrigger("webhook", "document-uploaded", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        //{
        //    //await _webhookService.SendAllWebhooksWhereTriggerDocumentUploaded();
        //    logger.LogInformation("WEBHOOKS SENT FOR UPLOADED DOCUMENTS");
        //}

        [FunctionName("WebhookAdded")]
        public static void Run2([ServiceBusTrigger("webhook-added", "send-email", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"WEBHOOK ADDED: {mySbMsg}");
        }

        [FunctionName("FlowCompleted")]
        public static void Run3([ServiceBusTrigger("flow-completed", "send-email", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"FLOW COMPLETED: {mySbMsg}");
        }

        [FunctionName("DocumentUploaded")]
        public static void Run4([ServiceBusTrigger("document-uploaded", "send-email", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"DOCUMENT UPLOADED: {mySbMsg}");
        }
    }
}
