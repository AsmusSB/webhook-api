﻿using System.Net;
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
        private readonly IDatabaseInterface _db;
        public WebhookController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("create-webhook-configuration")]
        public async Task<IActionResult> CreateWebhook(WebhookConfigurationApi webhookConfigurationApi)
        {
            WebhookConfiguration whConfig = await _webhookService.CreateWebhookConfiguration(webhookConfigurationApi);
            return Ok(whConfig);
                
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("send-webhook-configuration")]
        public async Task<IActionResult> SendWebhook(WebhookStatus webhookStatus)
        {
            await _webhookService.ExecuteWebhookWithPollyRetry(webhookStatus);
            return Ok(webhookStatus);
        }
        


        //[HttpGet]
        //[Microsoft.AspNetCore.Mvc.Route("getById-webhook-configuration")]
        //public IActionResult GetWebhookConfigurationById(int id)
        //{
        //    try
        //    {
        //        var webhook = _db.GetConfigurationById(id);
        //        if (webhook == null) return NotFound();
        //        return Ok(webhook);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpGet]
        //[Microsoft.AspNetCore.Mvc.Route("getAll-webhook-configuration")]
        //public IActionResult GetAllWebhookConfigurations()
        //{
        //    var whList = _db.GetAllWebhookConfigurations();

        //    if (whList == null) return NotFound();
        //    return Ok(whList);
        //}


        //[HttpDelete]
        //[Microsoft.AspNetCore.Mvc.Route("delete-webhook-configuration")]
        //public IActionResult DeleteWebhook(int id)
        //{
        //    try
        //    {
        //        var webhook = _db.DeleteFromRetry(id);
        //        return Ok(webhook);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}