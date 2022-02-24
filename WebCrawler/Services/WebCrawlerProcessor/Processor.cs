using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using SimpleWebCrawler.Model;
using SimpleWebCrawler.Services.Priter;
using SimpleWebCrawler.Services.QueueService;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SimpleWebCrawler.Services.WebCrawlerProcessor
{
    public class Process : IProcessor
    {
        private readonly IProducerService _producerService;

        private readonly IPrinter _printer;

        public Process(IProducerService producerService, IPrinter printer)
        {
            _producerService = producerService;
            _printer = printer;
        }

        public async Task ProcessLinks(HtmlDocument htmlDocument, string item)
        {
            var finalModel = new Final
            {
                RequestedUrl = item,
                AvailableUrl = new List<string>()
            };

            foreach (var url in from xx in htmlDocument.DocumentNode.Descendants("a")
                                let url = xx.Attributes["href"].Value
                                select url)
            {

                if (url.StartsWith("/"))
                {
                    finalModel.AvailableUrl.Add(url);
                }
            }

            _printer.PrintConsole(JsonConvert.SerializeObject(finalModel));

            if (finalModel.AvailableUrl.Count > 1)
            {
                await _producerService.AddAndFlushToChannel(finalModel);
            }
            
        }

    }
}
