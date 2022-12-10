namespace webhook_api.Services
{
    public class WebhookBackgroundService : IHostedService, IDisposable
    {
        private int _executionCount;
        private readonly ILogger<WebhookBackgroundService> _logger;
        private Timer? _timer;
        public IServiceProvider Services { get; }

        public WebhookBackgroundService(ILogger<WebhookBackgroundService> logger, IServiceProvider services)
        {
            Services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var backgroundServiceInterval = TimeSpan.FromSeconds(10);
            _logger.LogInformation("Webhook Background Service running.");
            _timer = new Timer(RetryAllWebhooks, null, TimeSpan.Zero, backgroundServiceInterval);
            return Task.CompletedTask;
        }

        private async void RetryAllWebhooks(object? state)
        {
            var count = Interlocked.Increment(ref _executionCount);
            _logger.LogInformation(
                "Webhook Background Service is working. Count: {Count}", count);

            using var scope = Services.CreateScope();
            var scopedProcessingService =
                scope.ServiceProvider
                    .GetRequiredService<IWebhookService>();

            await scopedProcessingService.RetryAllWebhooks();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Webhook Background Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}