using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Localization;
using webhook_api.Models;

namespace webhook_api.Interfaces
{
    public interface IWebhookConfigurationMapper
    {
        WebhookConfiguration Map(WebhookConfigurationApi source, List<HeaderApi>? headers, List<WebhookStatusApi> webhooks);
    }

    public class WebhookConfigurationMapper : IWebhookConfigurationMapper
    {
        private readonly IHeaderMapper _headerMapper;
        private readonly IWebhookStatusMapper _statusMapper;
        public WebhookConfigurationMapper(IHeaderMapper headerMapper, IWebhookStatusMapper statusMapper)
        {
            _headerMapper = headerMapper;
            _statusMapper = statusMapper;
        }

        public WebhookConfiguration Map(WebhookConfigurationApi source, List<HeaderApi>? headers, List<WebhookStatusApi> webhooks)
        {
            List<Header> headerList = new List<Header>();
            if (headers != null)
            {
                foreach (var h in headers)
                {
                    var header = _headerMapper.Map(h);
                    headerList.Add(header);
                }
            }

            List<WebhookStatus> statusList = new List<WebhookStatus>();
            foreach (var w in webhooks)
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
