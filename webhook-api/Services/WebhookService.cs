using System.Net;
using System.Text;
using Polly;
using Polly.Contrib.WaitAndRetry;
using webhook_api.Models;
using Polly.Extensions.Http;
using webhook_api.Context;
using webhook_api.Interfaces;

namespace webhook_api.Services
{
    public interface IWebhookService
    {
        Task<WebhookConfiguration> CreateWebhookConfiguration(WebhookConfigurationApi webhookConfigurationApi);
        Task RetryAllWebhooks();
        Task<HttpResponseMessage> SendWebhook(WebhookStatus webhookStatus);

    }
    public class WebhookService : IWebhookService
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly IDatabaseInterface _db;
        private readonly IWebhookConfigurationMapper _configMapper;

        private readonly WebhookDBContext _databaseContext;

        public WebhookService(IDatabaseInterface db, IWebhookConfigurationMapper configMapper, WebhookDBContext databaseContext)
        {
            _db = db;
            _configMapper = configMapper;
            _databaseContext = databaseContext;
        }

        public WebhookService() {}

        public async Task<WebhookConfiguration> CreateWebhookConfiguration(WebhookConfigurationApi webhookConfigurationApi)
        {
            WebhookConfiguration webhookConfig = _configMapper.Map(webhookConfigurationApi);
           
            _db.AddConfiguration(webhookConfig);
            return webhookConfig;
        }

        public async Task<WebhookStatus> ExecuteWebhookWithPollyRetry(WebhookStatus webhookStatus)
        {
            var retryDelay = TimeSpan.FromSeconds(webhookStatus.Config.RetryTimeSpan);
            var retryCount = webhookStatus.Config.TryCount;

            webhookStatus.Status = "Sending";
            _databaseContext.Update(webhookStatus);
            _databaseContext.SaveChanges();

            IAsyncPolicy<HttpResponseMessage> waitAndRetryPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrTransientHttpError()
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(retryDelay, retryCount));

            var response = await waitAndRetryPolicy.ExecuteAsync(() => SendWebhook(webhookStatus));
            _db.SaveWebhookStatusAndHistory(webhookStatus, response);

            return webhookStatus;
        }

        public async Task SendAllWebhooksWhereTriggerDocumentUploaded()
        {
            foreach (var wh in _db.GetAllWebhookStatuses())
            {
                if (wh.TriggerEvent == "document-uploaded")
                {
                    await ExecuteWebhookWithPollyRetry(wh);
                }
            }
        }

        public async Task RetryAllWebhooks()
        {
            foreach (var wh in _db.GetAllWebhookStatuses())
            {
                if (wh.CurrentFailedAttempts is >= 1 and <= 3)
                {
                    await ExecuteWebhookWithPollyRetry(wh);
                }
            }
        }
        public async Task<HttpResponseMessage> SendWebhook(WebhookStatus webhookStatus)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = (HttpStatusCode)StatusCodes.Status400BadRequest;

            if (webhookStatus.Config.Headers != null)
            {
                foreach (var h in webhookStatus.Config.Headers)
                {
                    if (!_client.DefaultRequestHeaders.Contains(h.HeaderName))
                    {
                        _client.DefaultRequestHeaders.Add(h.HeaderName, h.HeaderValue);
                    }
                }
            }

            var content = new StringContent(webhookStatus.Body, Encoding.UTF8, "application/json");
            try
            {
                response = await _client.PostAsync(webhookStatus.Config.DestinationUrl, content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return response;
            }

            return response;
        }
    }
}
