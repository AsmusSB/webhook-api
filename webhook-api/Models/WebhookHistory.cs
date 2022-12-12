using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace webhook_api.Models
{
    public class WebhookHistory
    {
        [Key]
        public int Id { get; set; }
        public string Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public DateTime TimeStamp { get; set; }
        public int StatusId { get; set; }
    }
}