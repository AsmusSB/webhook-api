using AutoFixture;
using Azure.Core;
using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;
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
            _serviceMock = new Mock<IWebhookService>();
            _controller = new WebhookController(_serviceMock.Object); // creates implementation in-memory
        }

        [Fact]
        public async Task CreateWebhook_ShouldReturnOkResponse_WhenWebhookCreated()
        {
            //Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var whConfigApi = _fixture.Create<WebhookConfigurationApi>();

            //Act
            var result = await _controller.CreateWebhook(whConfigApi);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<WebhookConfiguration>>();
        }
    }
}