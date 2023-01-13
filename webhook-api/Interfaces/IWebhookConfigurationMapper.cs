using webhook_api.Models;

namespace webhook_api.Interfaces
{
    public interface IWebhookConfigurationMapper
    {
        WebhookConfiguration Map(WebhookConfigurationApi source);
    }

    public class WebhookConfigurationMapper : IWebhookConfigurationMapper
    {
        private readonly IHeaderMapper _headerMapper;
        private readonly IWebhookStatusMapper _statusMapper;

        public WebhookConfigurationMapper()
        {
        }

        public WebhookConfigurationMapper(IHeaderMapper headerMapper, IWebhookStatusMapper statusMapper)
        {
            _headerMapper = headerMapper;
            _statusMapper = statusMapper;
        }
        public WebhookConfiguration Map(WebhookConfigurationApi source)
        {
            List<Header> headerList = new List<Header>();
            if (source.Headers != null)
            {
                foreach (var h in source.Headers)
                {
                    var header = _headerMapper.Map(h);
                    headerList.Add(header);
                }
            }

            List<WebhookStatus> statusList = new List<WebhookStatus>();
            foreach (var w in source.Webhooks)
            {
                var webhook = _statusMapper.Map(w);
                statusList.Add(webhook);
            }

            return new WebhookConfiguration()
            {
                Headers = headerList,
                DestinationUrl = source.DestinationUrl,
                TenantId = source.TenantId,
                TryCount = source.TryCount,
                RetryTimeSpan = source.RetryTimeSpan,
                Webhooks = statusList
            };
        }
    }
}
