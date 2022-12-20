using System.Net;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Contrib.WaitAndRetry;
using webhook_api.Interfaces;
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

        //[HttpPost]
        //[Microsoft.AspNetCore.Mvc.Route("send-webhook-configuration")]
        //public async Task<IActionResult> SendWebhook(WebhookStatus webhookStatus)
        //{
        //    await _webhookService.ExecuteWebhookWithPollyRetry(webhookStatus);
        //    return Ok(webhookStatus);
        //}
    }
}
