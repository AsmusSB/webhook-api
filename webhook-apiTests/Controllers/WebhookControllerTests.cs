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
        private readonly WebhookController _sut;

        public WebhookControllerTests(WebhookController controller, IWebhookService service, IFixture fixture, Mock<IWebhookService> serviceMock, WebhookController sut)
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IWebhookService>>();
            _sut = new WebhookController(_serviceMock.Object); // creates implementation in-memory
        }

        [TestMethod]
        [ExpectedException((typeof(ArgumentOutOfRangeException)))]
        public async Task AddConfigurationRetryTimeSpanTooLow()
        {
            //Arrange
            WebhookConfigurationApi whConfig = new WebhookConfigurationApi();

            //Act
            whConfig.TenantId = "123";
            whConfig.DestinationUrl = "url";
            whConfig.TryCount = 1;
            whConfig.RetryTimeSpan = 0;
            whConfig.Headers = new List<HeaderApi>();
            whConfig.Webhooks = new List<WebhookStatusApi>();

            //Assert
            await _sut.CreateWebhook(whConfig);
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
            var result = await _sut.CreateWebhook(request);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<WebhookConfiguration>>();
        }
    }
}