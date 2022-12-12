using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Contrib.WaitAndRetry;
using webhook_api.Models;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;
using webhook_api.Context;
using webhook_api.Interfaces;
using System.Reflection.PortableExecutable;
using Microsoft.IdentityModel.Tokens;
using Azure.Core;

namespace webhook_api.Services
{
    public interface IWebhookService
    {
        Task<WebhookStatus> ExecuteWebhookWithPollyRetry(WebhookStatus webhookStatus);
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
            //var retryDelay = TimeSpan.FromSeconds(1);
            //var retryCount = 3;

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

        public async Task RetryAllWebhooks()
        {
            foreach (var wh in _db.GetAllWebhookStatuses())
            {
                if (wh.CurrentFailedAttempts is >= 0 and <= 3) // change to 1-3
                {
                    Console.WriteLine("trying to fire webhook: " + wh.Id);
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
            
            Console.WriteLine("statuscode: " + response.StatusCode);

            return response;
        }
    }
}
