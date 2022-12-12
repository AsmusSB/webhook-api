using Microsoft.Net.Http.Headers;
using webhook_api.Models;

namespace webhook_api.Interfaces
{
    public interface IWebhookStatusMapper
    {
        WebhookStatus Map(WebhookStatusApi source);
        //WebhookStatusApi MapToApi(WebhookStatus source);
    }
    public class WebhookStatusMapper : IWebhookStatusMapper
    {
        public WebhookStatus Map(WebhookStatusApi source)
        {
            return new WebhookStatus()
            {
                Body = source.Body,
                TriggerEvent = source.TriggerEvent,
                TimesSuccessfullyFired = 0,
                CurrentFailedAttempts = 0,
                Status = "Waiting for trigger"
            };
        }
    }
}
