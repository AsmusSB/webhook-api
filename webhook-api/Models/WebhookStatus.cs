using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace webhook_api.Models
{
    public class WebhookStatusApi
    {
        public string Body { get; set; }
        public string TriggerEvent { get; set; }
        
        public WebhookStatusApi()
        {
        }
    }
    public class WebhookStatus
    {
        public string Body { get; set; }
        public string TriggerEvent { get; set; }
        public int TimesSuccessfullyFired { get; set; }
        public int CurrentFailedAttempts { get; set; }
        public string Status { get; set; }
        [ForeignKey("WebhookStatus")]
        [Key]
        public int Id { get; set; }
        public WebhookConfiguration Config { get; set; }
    }
}
