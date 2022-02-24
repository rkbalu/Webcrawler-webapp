using System.Collections.Generic;
using System.Threading.Tasks;
using Simple_web_crawler.Channel;
using SimpleWebCrawler.Model;

namespace SimpleWebCrawler.Services.QueueService
{
    public interface IProducerService
    {
        Task AddAndFlushToChannel(Final item);

        Task AddAndFlushToChannel(string item);

        void EndProducer();
    }
}