using System.Threading.Channels;
using System.Threading.Tasks;
using Simple_web_crawler.Channel;

namespace SimpleWebCrawler.Services.QueueService
{
    public interface IConsumerService
    {
        Task StartConsumer();

        Task ReaderAsync(Channel<string[]> channelBuffer);
    }
}