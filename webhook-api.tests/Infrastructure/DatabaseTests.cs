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
using Xunit.Abstractions;
using NSubstitute;
using webhook_api.Context;

namespace webhook_api.tests.Infrastructure
{
    public class DatabaseTests
    {
        private readonly IFixture _fixture;
        //private readonly Mock<IDatabaseInterface> _db;
        private readonly IDatabaseInterface _db2 = Substitute.For<IDatabaseInterface>();

        public DatabaseTests()
        {
            _fixture = new Fixture();
            //_db = _fixture.Freeze<Mock<IDatabaseInterface>>();
            //_db2 = Substitute.For<IDatabaseInterface>();
        }

        [Fact]
        public async Task AddConfiguration_RetryTimeSpanTooLow_ShouldThrowOutOfRangeException()
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

            string expected = "RetryTimeSpan has to be between 1-5";

            //Act
            //_db.Setup(x => x.AddConfiguration(wh));
            var result = _db2.AddConfiguration(wh);

            //Assert
            //Assert.Throws<ArgumentOutOfRangeException>(() => _db.Object.AddConfiguration(wh));
            //Assert.Throws<ArgumentOutOfRangeException>(() => _db2.AddConfiguration(wh));
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task SaveWebhookStatusAndHistory()
        {
        }
    }
}