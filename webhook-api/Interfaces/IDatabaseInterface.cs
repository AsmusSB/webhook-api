using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using webhook_api.Context;
using webhook_api.Models;
using static System.Net.WebRequestMethods;
using System.Linq;

namespace webhook_api.Interfaces
{
    public interface IDatabaseInterface
    {
        void SaveWebhookStatusAndHistory(WebhookStatus entity, HttpResponseMessage response);
        List<WebhookStatus> GetAllWebhookStatuses();
        string AddConfiguration(WebhookConfiguration webhookConfiguration);
        //WebhookConfiguration SaveForRetryLater(WebhookConfiguration webhookConfiguration);
        //void SaveSuccessWebhook(WebhookConfiguration entity);
        //void SaveFailedWebhook(WebhookConfiguration entity);
        //List<WebhookConfiguration> GetAllWebhookConfigurations();
        //List<Header> GetAllHeaders();
        //WebhookConfiguration GetConfigurationById(int id);
        //WebhookConfiguration DeleteFromRetry(int id);
        //List<Header>? GetHeadersByConfigId(int id);
    }
}