using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebCrawler.client.Http
{
    public class HttpClientWrapper: IHttpClient
    {
        public readonly HttpMessageInvoker HttpClient;

        public HttpClientWrapper(HttpMessageInvoker httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken cancellationToken)
        {
            return await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUrl), cancellationToken);
        }

        public async Task<HttpResponseMessage> GetAsyncContect(string requsetUrl)
        {
            var response = await GetAsync(requsetUrl, new CancellationToken());

            return response;
        }

    }
}
