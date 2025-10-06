
namespace Client.Middlewares
{
    public class ClientCredentialMiddleware : DelegatingHandler
    {
        private readonly IConfiguration _configuration;

        public ClientCredentialMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-API-KEY", _configuration.GetValue<string>("APIKEY"));

            return base.SendAsync(request, cancellationToken);
        }
    }
}