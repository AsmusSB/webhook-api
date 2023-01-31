using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webhook_api.Models;
using webhook_api.Services;

namespace webhook_api.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly IWebhookService _webhookService;
        public WebhookController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("create-webhook-configuration")]
        public async Task<ActionResult<WebhookConfiguration>> CreateWebhook(WebhookConfigurationApi webhookConfigurationApi)
        {
            WebhookConfiguration whConfig = await _webhookService.CreateWebhookConfiguration(webhookConfigurationApi);
            return Ok(whConfig);
        }

        [Microsoft.AspNetCore.Mvc.Route("create-webhook-configuration-and-publish-message-webhook-added")]
        [HttpPost]
        public async Task<ActionResult<WebhookConfiguration>> CreateWebhookAndPublish(WebhookConfigurationApi whApi)
        {
            WebhookConfiguration whConfig = await _webhookService.CreateWebhookConfiguration(whApi);

            string connectionString =
                "";
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender("webhook-added");
            string body = JsonConvert.SerializeObject(whConfig, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var message = new ServiceBusMessage(body);
            await sender.SendMessageAsync(message);
            return Ok(whConfig);
        }

        [Microsoft.AspNetCore.Mvc.Route("create-webhook-configuration-and-publish-message-flow-completed")]
        [HttpPost]
        public async Task<ActionResult<WebhookConfiguration>> CreateWebhookAndPublish2(WebhookConfigurationApi whApi)
        {
            WebhookConfiguration whConfig = await _webhookService.CreateWebhookConfiguration(whApi);

            string connectionString =
                "";
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender("flow-completed");
            string body = JsonConvert.SerializeObject(whConfig, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var message = new ServiceBusMessage(body);
            await sender.SendMessageAsync(message);
            return Ok(whConfig);
        }

        [Microsoft.AspNetCore.Mvc.Route("create-webhook-configuration-and-publish-message-document-uploaded")]
        [HttpPost]
        public async Task<ActionResult<WebhookConfiguration>> CreateWebhookAndPublish3(WebhookConfigurationApi whApi)
        {
            WebhookConfiguration whConfig = await _webhookService.CreateWebhookConfiguration(whApi);

            string connectionString =
                "";
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender("document-uploaded");
            string body = JsonConvert.SerializeObject(whConfig, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var message = new ServiceBusMessage(body);
            await sender.SendMessageAsync(message);
            return Ok(whConfig);
        }
    }
}
