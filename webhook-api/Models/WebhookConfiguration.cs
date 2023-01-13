using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webhook_api.Models
{
    public class WebhookConfigurationApi
    {
        public List<HeaderApi>? Headers { get; set; }
        [DataType(DataType.Url)]
        public string DestinationUrl { get; set; }
        public string TenantId { get; set; }
        public int TryCount { get; set; }
        public int RetryTimeSpan { get; set; }
        public List<WebhookStatusApi> Webhooks { get; set; }

        public WebhookConfigurationApi()
        {
        }
    }

    public class WebhookConfiguration
    {
        public List<Header>? Headers { get; set; }
        [DataType(DataType.Url)]
        public string DestinationUrl { get; set; }
        public string TenantId { get; set; }
        public int TryCount { get; set; }
        public int RetryTimeSpan { get; set; }
        public List<WebhookStatus> Webhooks { get; set; }

        [Key]
        public int Id { get; set; }
        [InverseProperty(nameof(WebhookStatus.Config))]
        private ICollection<List<WebhookStatus>> WebhookStatusCollection { get; set; }
        [InverseProperty(nameof(Header.Config))]
        private ICollection<List<Header>> HeadersCollection { get; set; }
    }
}
