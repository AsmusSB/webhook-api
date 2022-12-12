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
        private readonly IFixture _fixture;
        private readonly Mock<IWebhookService> _serviceMock;
        private readonly IWebhookService _service;

        public WebhookServiceTests()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IWebhookService>>();
        }

        [Fact]
        public async Task SendWebhook()
        {
            //Arrange
            WebhookConfiguration wh = new WebhookConfiguration
            {
                Id = 1,
                TenantId = "tenant",
                DestinationUrl = "url",
                Headers = new List<Header>(),
                Webhooks = new List<WebhookStatus>(),
                TryCount = 1,
                RetryTimeSpan = 0
            };

            WebhookStatus whStatus = new WebhookStatus
            {
                Id = 1,
                Body = "message",
                TriggerEvent = "Trigger",
                TimesSuccessfullyFired = 1,
                CurrentFailedAttempts = 0,
                Status = "Waiting for trigger",
            };

            wh.Webhooks.Add(whStatus);

            HttpResponseMessage expected = new HttpResponseMessage();
            expected.StatusCode = (HttpStatusCode)StatusCodes.Status400BadRequest;

            //Act
            //var result = await _serviceMock.Object.SendWebhook(whStatus);
            var result = await _service.SendWebhook(whStatus);

            //Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
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