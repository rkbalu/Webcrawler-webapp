using System;
using System.Threading.Channels;

namespace SimpleWebCrawler.Factory
{
    public class ChannelFactory : IChannelFactory
    {
        private readonly Channel<string[]> _channel;

        public ChannelFactory(Channel<string[]> channel)
        {
            _channel = channel;
        }

        public Channel<string[]> GetChannel()
        {
            return _channel;
        }
    }
}
