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
    }
    public class WebhookService : IWebhookService
    {
        private readonly HttpClient _client;
        private readonly IDatabaseInterface _db;
        private readonly IWebhookConfigurationMapper _configMapper;

        private WebhookDBContext _databaseContext;

        public WebhookService(HttpClient client, IDatabaseInterface db, IWebhookConfigurationMapper configMapper, WebhookDBContext databaseContext)
        {
            _client = client;
            _db = db;
            _configMapper = configMapper;
            _databaseContext = databaseContext;
        }

        public async Task<WebhookConfiguration> CreateWebhookConfiguration(WebhookConfigurationApi webhookConfigurationApi)
        {
            WebhookConfiguration webhookConfig = _configMapper.Map(webhookConfigurationApi, webhookConfigurationApi.Headers, webhookConfigurationApi.Webhooks);
            int whId = _db.AddConfiguration(webhookConfig);
            return webhookConfig;
        }

        public async Task<WebhookStatus> ExecuteWebhookWithPollyRetry(WebhookStatus webhookStatus)
        {
            //var retryDelay = TimeSpan.FromSeconds(webhookStatus.Config.RetryTimeSpan);
            //var retryCount = webhookStatus.Config.TryCount;
            var retryDelay = TimeSpan.FromSeconds(1);
            var retryCount = 3;

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
            //try
            //{
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
                //try
                //{
                    response = await _client.PostAsync(webhookStatus.Config.DestinationUrl, content);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    throw;
                //}
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            HttpResponseMessage r2 = new HttpResponseMessage(); 

            return response;
        }

        //private void ManageDatabase(WebhookStatus webhookStatus, HttpResponseMessage response)
        //{
        //    bool alreadyExists = _db.GetAllWebhookStatuses().Any(x => x.Id == webhookStatus.Config.Id);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        Console.WriteLine(webhookStatus.Config.Id + ": saved in successful webhooks");
        //        _db.SaveSuccessWebhook(webhookStatus);

        //        if (alreadyExists)
        //        {
        //            Console.WriteLine(webhookStatus.Config.Id + " deleted from retry");
        //            _db.DeleteFromRetry(webhookStatus.Config.Id);
        //        }
        //    }
        //    else
        //    {
        //        if (alreadyExists)
        //        {
        //            Console.WriteLine(webhookStatus.Config.Id + " deleted from retry");
        //            _db.DeleteFromRetry(webhookStatus.Config.Id);
        //        }
        //        webhookStatus.CurrentFailedAttempts++;
        //        if (webhookStatus.CurrentFailedAttempts <= 3)
        //        {
        //            Console.WriteLine(webhookStatus.Config.Id + ": saved for retry later");
        //            _db.SaveForRetryLater(webhookStatus);
        //        }

        //        if (webhookStatus.CurrentFailedAttempts > 3)
        //        {
        //            // add statuscode to failed webhooks table
        //            Console.WriteLine(webhookStatus.Config.Id + ": saved in failed webhooks");
        //            _db.SaveFailedWebhook(webhookStatus);
        //        }
        //    }
        //}

        //public async Task<HttpResponseMessage> SendWebhook(WebhookConfiguration webhookConfiguration)
        //{
        //    var headerList = _db.GetHeadersByConfigId(webhookConfiguration.Id);

        //    if (headerList != null)
        //    {
        //        foreach (var h in headerList)
        //        {
        //            _client.DefaultRequestHeaders.Add(h.HeaderName, h.HeaderValue);
        //            _client.DefaultRequestHeaders.Add(h.HeaderName, h.HeaderValue);
        //        }
        //    }

        //    //if (webhookConfiguration.Headers != null)
        //    //{
        //    //    foreach (var h in webhookConfiguration.Headers)
        //    //    {
        //    //        _client.DefaultRequestHeaders.Add(h.HeaderName, h.HeaderValue);
        //    //    }
        //    //}

        //    var content = new StringContent(webhookConfiguration.TenantId, Encoding.UTF8, "application/json"); //change to body
        //    HttpResponseMessage response;
        //    try
        //    {
        //        response = await _client.PostAsync(webhookConfiguration.DestinationUrl, content);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        throw;
        //    }

        //    return response;
        //}
    }
}
