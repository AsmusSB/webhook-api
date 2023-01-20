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
        //    logger.LogInformation($"webhook: {mySbMsg}");
        //}

        //[FunctionName("UpdateReport")]
        //public static void Run1([ServiceBusTrigger("webhook-added", "update-report", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        //{
        //    logger.LogInformation($"UPDATE REPORT: {mySbMsg}");
        //}

        //[FunctionName("SendWebhhook")]
        //public static void Run2([ServiceBusTrigger("webhook-added", "send-webhook", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        //{
        //    logger.LogInformation($"SEND WEBHOOK: {mySbMsg}");
        //}
        [FunctionName("SendEmail")]
        public static void Run3([ServiceBusTrigger("webhook-added", "send-email", Connection = "WebhookDataConnection")] string mySbMsg, ILogger logger)
        {
            logger.LogInformation($"SEND EMAIL: {mySbMsg}");
        }
    }
}
