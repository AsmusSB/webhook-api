using AutoFixture;
using Azure.Core;
using Azure;
using FluentAssertions;
using webhook_api.Interfaces;
using webhook_api.Models;

namespace webhook_api.tests.Infrastructure
{
    public class MapperTests
    {
        private readonly IWebhookConfigurationMapper _configMapper = new WebhookConfigurationMapper();
        
        [Fact]
        public async Task ConfigurationMapper_MapsToCorrectValues()
        {
            //Arrange
            WebhookConfigurationApi whConfigApi = new WebhookConfigurationApi
            {
                TenantId = "tenantId",
                DestinationUrl = "url",
                RetryTimeSpan = 1,
                TryCount = 1,
                Headers = new List<HeaderApi>(),
                Webhooks = new List<WebhookStatusApi>()
            };

            //Act
            var result = _configMapper.Map(whConfigApi);

            //Assert
            result.Should().BeEquivalentTo(whConfigApi);
        }
    }
}