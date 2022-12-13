using AutoFixture;
using Azure.Core;
using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using webhook_api.Controllers;
using webhook_api.Interfaces;
using webhook_api.Models;
using webhook_api.Services;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace webhook_api.tests.Infrastructure
{
    public class WebhookServiceTests
    {
        private readonly IWebhookService _webhookService = new WebhookService();
        
        [Fact]
        public async Task SendWebhook_InvalidUrl_ReturnsBadRequest()
        {
            //Arrange
            WebhookConfiguration wh = new WebhookConfiguration
            {
                DestinationUrl = "InvalidUrl",
            };

            WebhookStatus whStatus = new WebhookStatus
            {
                Body = "message",
                Config = wh
            };

            HttpResponseMessage expected = new HttpResponseMessage();
            expected.StatusCode = (HttpStatusCode)StatusCodes.Status400BadRequest;

            //Act
            var result = await _webhookService.SendWebhook(whStatus);

            //Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public async Task SendWebhook_ValidUrl_DoesntReturnBadRequest()
        {
            //Arrange
            WebhookConfiguration wh = new WebhookConfiguration
            {
                DestinationUrl = "https://eou05wu1ireya27.m.pipedream.net",
            };

            WebhookStatus whStatus= new WebhookStatus
            {
                Body = "message",
                Config = wh
            };

            HttpResponseMessage expected = new HttpResponseMessage();
            expected.StatusCode = (HttpStatusCode)StatusCodes.Status400BadRequest;

            //Act
            var result = await _webhookService.SendWebhook(whStatus);

            //Assert
            Assert.NotEqual(expected.StatusCode, result.StatusCode);
            Assert.Equal((HttpStatusCode)StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task ExecuteWebhookWithPollyRetry()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}