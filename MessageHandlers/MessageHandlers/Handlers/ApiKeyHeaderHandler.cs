using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Handlers
{
    public class ApiKeyHeaderHandler : DelegatingHandler
    {
        public const string _apiKeyHeader = "X-Api-key";

        public const string _apiQuerString = "api_key";

        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string apiKey = null;

            if (request.Headers.Contains(_apiKeyHeader))
            {
                apiKey = request.Headers.GetValues(_apiKeyHeader).FirstOrDefault();
            }
            else
            {
                //check query string because of swagger
                var queryString = request.GetQueryNameValuePairs();
                var keyValuePair = queryString.FirstOrDefault(x=> x.Key.ToLowerInvariant().Equals(_apiQuerString));
                if(!string.IsNullOrEmpty(keyValuePair.Value))
                {
                    apiKey = keyValuePair.Value;
                }
            }

            request.Properties.Add(_apiKeyHeader,apiKey);

            //here is step 2,3,4
            return base.SendAsync(request,cancellationToken);

        }        
    }

    public static class HttpRequestMessageApiKey
    {
        public static string GetApiKey(this HttpRequestMessage request)
        {
            if (request == null)
                return null;

            if (request.Properties.TryGetValue(ApiKeyHeaderHandler._apiKeyHeader, out object apiKey))
                return (string)apiKey;

            return null;
        }
    }
}