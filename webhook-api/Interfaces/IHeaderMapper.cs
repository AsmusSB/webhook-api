using webhook_api.Models;

namespace webhook_api.Interfaces
{
    public interface IHeaderMapper
    {
        Header Map(HeaderApi source);
    }
    public class HeaderMapper : IHeaderMapper
    {
        public Header Map(HeaderApi source)
        {
            return new Header()
            {
                HeaderName = source.HeaderName,
                HeaderValue = source.HeaderValue
            };
        }
    }
}
