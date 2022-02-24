using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Simple_web_crawler.Channel;
using SimpleWebCrawler.client.Http;
using SimpleWebCrawler.Services.HtmlParser;
using SimpleWebCrawler.Model;
using Newtonsoft.Json;
using SimpleWebCrawler.Services.Priter;
using SimpleWebCrawler.Services.WebCrawlerProcessor;
using SimpleWebCrawler.Factory;

namespace SimpleWebCrawler.Services.QueueService
{
    public class ConsumerService : IConsumerService
    {
        private readonly IHtmlParser _htmlParser;

        private readonly IHttpClient _httpClient;

        private readonly IPrinter _printer;

        private readonly IProcessor _processor;

        private readonly IChannelFactory _channelFactory;

        public ConsumerService(IHtmlParser htmlParser,
            IHttpClient httpClient, IPrinter printer, IProcessor processor, IChannelFactory channelFactory)
        {
            _htmlParser = htmlParser;
            _httpClient = httpClient;
            _printer = printer;
            _processor = processor;
            _channelFactory = channelFactory;
        }

        public async Task StartConsumer()
        {
            await ReaderAsync(_channelFactory.GetChannel());
        }

        public async Task ReaderAsync(Channel<string[]> webCrawlerChannel)
        {
            while (await webCrawlerChannel.Reader.WaitToReadAsync())
            {
                while (true)
                {
                    try
                    {
                        var itemToExecute = await webCrawlerChannel.Reader.ReadAsync();

                        foreach (var item in itemToExecute)
                        {
                            var response = await _httpClient.GetAsyncContect(item.ToString());

                            var htmlStr = await response.Content.ReadAsStringAsync();

                            var parsedHtmlDocument = await _htmlParser.ParseHtmlResponse(htmlStr);

                            //TODO Just to avoid any issue with html parsing due to broken html. Need to workout in-detail
                            try
                            {
                                await _processor.ProcessLinks(parsedHtmlDocument, item);
                            }
                            catch (Exception e)
                            {
                                _printer.PrintConsole(e.ToString());
                            }
                        }
                    }
                    catch (ChannelClosedException)
                    {
                        _printer.PrintConsole("Exception fired!");
                    }
                }
            }

        }

    }
}
