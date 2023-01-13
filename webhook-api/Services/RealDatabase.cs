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

        public void SaveWebhookStatusAndHistory(WebhookStatus webhookStatus, HttpResponseMessage response)
        {
            bool alreadyExists = GetAllWebhookStatuses().Any(x => x.Config.Id == webhookStatus.Config.Id);

            if (response.IsSuccessStatusCode)
            {
                if (alreadyExists)
                {
                    webhookStatus.TimesSuccessfullyFired++;
                    webhookStatus.CurrentFailedAttempts = 0;
                    webhookStatus.Status = "Waiting for trigger";
                    _db.WebhookStatus.Update(webhookStatus);

                    WebhookHistory webhookHistory = new WebhookHistory
                    {
                        Result = "Success",
                        StatusCode = response.StatusCode,
                        TimeStamp = DateTime.Now,
                        StatusId = webhookStatus.Id
                    };

                    _db.WebhookHistory.Add(webhookHistory);
                    _db.SaveChanges();
                }
                else
                {
                    webhookStatus.TimesSuccessfullyFired = 1; 
                    webhookStatus.CurrentFailedAttempts = 0;
                    webhookStatus.Status = "Waiting for trigger";
                    _db.WebhookStatus.Add(webhookStatus);

                    WebhookHistory webhookHistory = new WebhookHistory
                    {
                        Result = "Success",
                        StatusCode = response.StatusCode,
                        TimeStamp = DateTime.Now,
                        StatusId = webhookStatus.Id
                    };

                    _db.WebhookHistory.Add(webhookHistory);
                    _db.SaveChanges();
                }
            }
            else
            {
                if (alreadyExists & webhookStatus.CurrentFailedAttempts <= 3)
                {
                    webhookStatus.CurrentFailedAttempts++;
                    webhookStatus.Status = "Waiting to be retried later";

                    _db.WebhookStatus.Update(webhookStatus);
                    _db.SaveChanges();
                }
                if(webhookStatus.CurrentFailedAttempts > 3)
                {
                    WebhookHistory webhookHistory = new WebhookHistory
                    {
                        Result = "Failed",
                        StatusCode = response.StatusCode,
                        TimeStamp = DateTime.Now,
                        StatusId = webhookStatus.Id
                    };

                    _db.WebhookHistory.Add(webhookHistory); 
                    _db.SaveChanges();
                }
            }
        }

        public List<WebhookStatus> GetAllWebhookStatuses()
        {
            return _db.WebhookStatus.Include(x => x.Config).ThenInclude(y => y.Headers).ToList();
        }

        public string AddConfiguration(WebhookConfiguration webhook)
        {
            string result = "success";
            try
            {
                if (webhook.RetryTimeSpan < 1 || webhook.RetryTimeSpan > 5) throw new ArgumentOutOfRangeException(result = "RetryTimeSpan has to be between 1-5");
                if (webhook.TryCount < 1 || webhook.TryCount > 5) throw new ArgumentOutOfRangeException(result = "TryCount has to be between 1-5");
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
