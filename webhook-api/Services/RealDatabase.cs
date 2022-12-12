using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using webhook_api.Context;
using webhook_api.Interfaces;
using webhook_api.Models;

namespace webhook_api.Services
{
    public class RealDatabase : IDatabaseInterface
    {
        private WebhookDBContext _db;
        public RealDatabase(WebhookDBContext db)
        {
            _db = db;
        }

        public WebhookConfiguration SaveForRetryLater(WebhookConfiguration webhook)
        {
            WebhookConfiguration tempWebhook = GetConfigurationById(webhook.Id);
            if (tempWebhook != null)
            { // changes every parameter except Id in database
                tempWebhook.TenantId = webhook.TenantId;
                tempWebhook.TryCount = webhook.TryCount;
                tempWebhook.DestinationUrl = webhook.DestinationUrl;
                tempWebhook.RetryTimeSpan = webhook.RetryTimeSpan;
                tempWebhook.Headers = webhook.Headers;
            }
            else
            {
                _db.WebhookConfigurations.Add(webhook);
            }
            _db.SaveChanges();
            return webhook;
        }

        public void SaveSuccessWebhook(WebhookConfiguration entity)
        {
            throw new NotImplementedException();
        }

        public void SaveFailedWebhook(WebhookConfiguration entity)
        {
            throw new NotImplementedException();
        }

        public void SaveWebhookStatusAndHistory(WebhookStatus webhookStatus, HttpResponseMessage response)
        {
            //try
            //{
                bool alreadyExists = GetAllWebhookStatuses().Any(x => x.Config.Id == webhookStatus.Config.Id);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("successful webhook updated for id: " + webhookStatus.Config.Id);
                    if (alreadyExists)
                    {
                        Console.WriteLine("webhook already exists ind database");
                        webhookStatus.TimesSuccessfullyFired++;
                        webhookStatus.CurrentFailedAttempts = 0;
                        webhookStatus.Status = "Waiting for trigger";
                        _db.WebhookStatus.Update(webhookStatus);

                        WebhookHistory webhookHistory = new WebhookHistory();
                        webhookHistory.Result = "Success";
                        webhookHistory.StatusCode = response.StatusCode;
                        webhookHistory.TimeStamp = DateTime.Now;
                        webhookHistory.StatusId = webhookStatus.Id;
                        _db.WebhookHistory.Add(webhookHistory);
                        _db.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("wh doesnt exist in database already");
                        webhookStatus.TimesSuccessfullyFired = 1; 
                        webhookStatus.CurrentFailedAttempts = 0;
                        webhookStatus.Status = "Waiting for trigger";
                        _db.WebhookStatus.Add(webhookStatus);
                        
                        WebhookHistory webhookHistory = new WebhookHistory();
                        webhookHistory.Result = "Success";
                        webhookHistory.StatusCode = response.StatusCode;
                        webhookHistory.TimeStamp = DateTime.Now;
                        webhookHistory.StatusId = webhookStatus.Id;
                        _db.WebhookHistory.Add(webhookHistory);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    Console.WriteLine(webhookStatus.Config.Id + " webhook updated: failed attempt");
                    if (alreadyExists & webhookStatus.CurrentFailedAttempts <= 3)
                    {
                        webhookStatus.CurrentFailedAttempts++;
                        webhookStatus.Status = "Waiting to be retried";

                        _db.WebhookStatus.Update(webhookStatus);
                        _db.SaveChanges();
                    }
                    if(webhookStatus.CurrentFailedAttempts > 3)
                    {
                        WebhookHistory webhookHistory = new WebhookHistory();
                        webhookHistory.Result = "Failed"; 
                        webhookHistory.StatusCode = response.StatusCode;
                        webhookHistory.TimeStamp = DateTime.Now;
                        webhookHistory.StatusId = webhookStatus.Id;
                        _db.WebhookHistory.Add(webhookHistory); 
                        _db.SaveChanges();
                    }
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
        }

        public List<WebhookConfiguration> GetAllWebhookConfigurations()
        {
            return _db.WebhookConfigurations.ToList();
        }

        public List<WebhookStatus> GetAllWebhookStatuses()
        {
            return _db.WebhookStatus.Include(x => x.Config).ThenInclude(y => y.Headers).ToList();
        }

        public List<Header> GetAllHeaders()
        {
            return _db.Headers.ToList();
        }

        public WebhookConfiguration GetConfigurationById(int id)
        {
            List<WebhookConfiguration> webhookList = _db.WebhookConfigurations.ToList();
            foreach (var wh in webhookList)
            {
                if (wh.Id == id)
                {
                    return wh;
                }
            }

            return null;
        }

        public WebhookConfiguration DeleteFromRetry(int id)
        {
            WebhookConfiguration tempWebhook = GetConfigurationById(id);
            if (tempWebhook != null)
            {
                _db.WebhookConfigurations.Remove(tempWebhook);
                _db.SaveChanges();
                return tempWebhook;
            }
            return null;
        }

        public List<Header>? GetHeadersByConfigId(int id)
        {
            List<Header> headerList = new List<Header>();

            foreach (var h in _db.Headers.ToList())
            {
                if (h.Config.Id == id)
                {
                    headerList.Add(h);
                }
            }
            return headerList;
        }

        public string AddConfiguration(WebhookConfiguration webhook)
        {
            string result = "success";
            try
            {
                if (webhook.RetryTimeSpan < 1 || webhook.RetryTimeSpan > 5) throw new ArgumentOutOfRangeException(result = "RetryTimeSpan has to be between 1-5");
                if (webhook.TryCount < 1 || webhook.TryCount > 5) throw new ArgumentOutOfRangeException(result = "TryCount cannot be between 1-5");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
                return result;
            }
            EntityEntry<WebhookConfiguration> newConfig = _db.WebhookConfigurations.Add(webhook);
            _db.SaveChanges();
            return result;
        }
    }
}
