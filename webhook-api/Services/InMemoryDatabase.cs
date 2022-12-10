//using webhook_api.Interfaces;
//using webhook_api.Models;

//namespace webhook_api.Services
//{
//    public class InMemoryDatabase : IDatabaseInterface<WebhookConfiguration, int>
//    {
//        private List<WebhookConfiguration> _retryWebhooksLater = new List<WebhookConfiguration>();
//        private List<WebhookConfiguration> _successWebhooks = new List<WebhookConfiguration>();
//        private List<WebhookConfiguration> _failedWebhooks = new List<WebhookConfiguration>();
        
//        static readonly Header[] _headers = new Header[]
//        {
//            new Header { HeaderName = "1", HeaderValue = "1"},
//            new Header { HeaderName = "2", HeaderValue = "2"},
//        };

//        private static List<WebhookConfiguration> _testList = new List<WebhookConfiguration>
//        {
//            new WebhookConfiguration{Headers = _headers, DestinationUrl = "https://eou05wu1ireya27.m.pipedream.net", TenantId = "tenant1"},
//            new WebhookConfiguration{Headers = _headers, DestinationUrl = "https://eou05wu1ireya27.m.pipedream.net", TenantId = "tenant2"},
//            new WebhookConfiguration{Headers = _headers, DestinationUrl = "https://eou05wu1ireya27.m.pipedream.net", TenantId = "tenant3"}
//        };
//        public WebhookConfiguration SaveForRetryLater(WebhookConfiguration webhook)
//        {
//            _retryWebhooksLater.Add(webhook);
//            return webhook;
//        }

//        public void SaveSuccessWebhook(WebhookConfiguration entity)
//        {
//            _successWebhooks.Add(entity);
//        }

//        public void SaveFailedWebhook(WebhookConfiguration entity)
//        {
//            _failedWebhooks.Add(entity);
//        }

//        public List<WebhookConfiguration> GetAllWebhooksRetry()
//        {
//            //return new List<WebhookConfiguration>(_testList);
//            return _testList;
//        }

//        public WebhookConfiguration GetById(int id)
//        {
//            return _testList.SingleOrDefault(x => x.Id == id);
//        }

//        public WebhookConfiguration DeleteFromRetry(int id)
//        {
//            WebhookConfiguration webhookConfiguration = _testList.SingleOrDefault(x => x.Id == id);
//            _retryWebhooksLater.Remove(webhookConfiguration);
//            return webhookConfiguration;
//        }

//        public List<Header>? GetHeadersByConfigId(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public int AddConfiguration(WebhookConfiguration entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void AddHeader(WebhookConfiguration i)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
