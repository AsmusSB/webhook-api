using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Microsoft.EntityFrameworkCore;

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

        //public int TryCount
        //{
        //    get => TryCount;
        //    set
        //    {
        //        if (value < 1 | value > 5)
        //            throw new ArgumentOutOfRangeException("Invalid value - TryCount must be in the range 1-5");
        //        TryCount = value;
        //    }
        //}

        //public int RetryTimeSpan
        //{
        //    get => RetryTimeSpan;
        //    set
        //    {
        //        if (value < 1 | value > 5)
        //            throw new ArgumentOutOfRangeException("Invalid value - RetryTimeSpan must be in the range 1-5");
        //        RetryTimeSpan = value;
        //    }
        //}

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
