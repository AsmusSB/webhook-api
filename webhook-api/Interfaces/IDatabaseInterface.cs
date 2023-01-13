using webhook_api.Models;

namespace webhook_api.Interfaces
{
    public interface IDatabaseInterface
    {
        void SaveWebhookStatusAndHistory(WebhookStatus entity, HttpResponseMessage response);
        List<WebhookStatus> GetAllWebhookStatuses();
        string AddConfiguration(WebhookConfiguration webhookConfiguration);
    }
}