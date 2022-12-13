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

namespace webhook_api.tests.Controllers
{
    public class WebhookControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IWebhookService> _serviceMock;
        private readonly WebhookController _controller;

        public WebhookControllerTests()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IWebhookService>>();
            _controller = new WebhookController(_serviceMock.Object); // creates implementation in-memory
        }

        [Fact]
        public async Task CreateWebhook_ShouldReturnOkResponse_WhenWebhookCreated()
        {
            //Arrange
            var request = _fixture.Create<WebhookConfigurationApi>();
            var response = _fixture.Create<WebhookConfiguration>();
            _serviceMock.Setup(x => x.CreateWebhookConfiguration(request)).ReturnsAsync(response);

            //Act
            var result = await _controller.CreateWebhook(request);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<WebhookConfiguration>>();
            result.Should().BeOfType<WebhookConfiguration>();
        }
    }
}