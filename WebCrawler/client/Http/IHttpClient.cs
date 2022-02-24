using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace SimpleWebCrawler.client.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken cancellationToken);

        Task<HttpResponseMessage> GetAsyncContect(string requsetUrl);

    }
}
