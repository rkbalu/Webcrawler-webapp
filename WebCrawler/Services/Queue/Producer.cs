using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Simple_web_crawler.Channel;
using SimpleWebCrawler.Factory;
using SimpleWebCrawler.Model;

namespace SimpleWebCrawler.Services.QueueService
{
    public class ProducerService : IProducerService
    {
        private readonly IChannelBuffer _channelBuffer;

        public ProducerService(IChannelBuffer channelBuffer)
        {
            _channelBuffer = channelBuffer;
        }

        public async Task AddAndFlushToChannel(string item)
        {
            await _channelBuffer.Flush(item);
        }

        public async Task AddAndFlushToChannel(Final item)
        {
            foreach (string availableUrl in item.AvailableUrl)
            {
                await _channelBuffer.Flush(string.Concat(new Uri(item.RequestedUrl).GetLeftPart(UriPartial.Authority), availableUrl));//TODO need to remove the hardcode url   
            }
        }

        public void EndProducer()
        {
            _channelBuffer.Clear();
        }

    }
}
