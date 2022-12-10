using Microsoft.EntityFrameworkCore;
using webhook_api.Models;

namespace webhook_api.Context
{
    public class WebhookDBContext : DbContext
    {
        public WebhookDBContext(DbContextOptions options) : base(options)
        {
        }
      
        public DbSet<Header> Headers { get; set; }
        public DbSet<WebhookConfiguration> WebhookConfigurations { get; set; }
        public DbSet<WebhookStatus> WebhookStatus { get; set; }
        public DbSet<WebhookHistory> WebhookHistory { get; set; }
    }
}
