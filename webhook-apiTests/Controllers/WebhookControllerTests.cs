using Microsoft.VisualStudio.TestTools.UnitTesting;
using webhook_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using webhook_api.Models;
using webhook_api.Services;
using Xunit;
using AutoFixture;
using Moq;
using webhook_api.Interfaces;

namespace webhook_api.Controllers.Tests
{
    [TestClass()]
    public class WebhookControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IWebhookService> _serviceMock;
        private readonly WebhookController _controller;
        private readonly RealDatabase _db;

        public WebhookControllerTests(RealDatabase db)
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IWebhookService>>();
            _controller = new WebhookController(_serviceMock.Object); // creates implementation in-memory
            _db = db;
        }

        [TestMethod]
        [ExpectedException((typeof(ArgumentOutOfRangeException)))]
        public async Task AddConfiguration_RetryTimeSpanTooLow_ShouldThrowOutOfRangeException()
        {
            //Arrange
            WebhookConfiguration wh = new WebhookConfiguration();

            //Act
            wh.Id = 1;
            wh.TenantId = "tenant";
            wh.DestinationUrl = "url";
            wh.Headers = new List<Header>();
            wh.Webhooks = new List<WebhookStatus>();
            wh.TryCount = 1;
            wh.RetryTimeSpan = 0;

            //Assert
            _db.AddConfiguration(wh);
        }

        [Fact]
        public async Task CreateWebhook_ShouldReturnOkResponse_WhenWebhookCreated()
        {
            //Arrange
            WebhookConfigurationApi whConfig = new WebhookConfigurationApi();
            var request = _fixture.Create<WebhookConfigurationApi>();
            var response = _fixture.Create<WebhookConfiguration>();
            _serviceMock.Setup(x => x.CreateWebhookConfiguration(request)).ReturnsAsync(response);

            //Act
            var result = await _controller.CreateWebhook(request).ConfigureAwait(false);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<WebhookConfiguration>>();
        }
    }
}