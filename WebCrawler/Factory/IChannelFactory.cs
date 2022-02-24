using System.Threading.Channels;

namespace SimpleWebCrawler.Factory
{
    public interface IChannelFactory
    {
        Channel<string[]> GetChannel();
    }
}