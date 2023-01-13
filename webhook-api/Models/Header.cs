using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webhook_api.Models
{
    public class HeaderApi
    {
        public string HeaderName { get; set; }
        public string HeaderValue { get; set; }
    }
    public class Header
    {
        public string HeaderName { get; set; }
        public string HeaderValue { get; set; }

        [ForeignKey("WebhookHeader")]
        
        [Key]
        public int Id { get; set; }
        public WebhookConfiguration Config { get; set; }
    }
}
