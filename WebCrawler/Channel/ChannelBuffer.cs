using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SimpleWebCrawler.Factory;

namespace Simple_web_crawler.Channel
{
    public class ChannelBuffer1 : IChannelBuffer
    {
        private readonly IChannelFactory _channelFactory;
        public readonly Channel<string[]> Channel;

        public ChannelBuffer1(IChannelFactory channelFactory)
        {
            _channelFactory = channelFactory;
            Channel = _channelFactory.GetChannel();
        }


        public void Clear()
        {
            Channel.Writer.Complete();

        }

        public async Task Flush(string item)
        {
            var itemList = new List<string>()
            {
                item
            };

            await Channel.Writer.WriteAsync(itemList.ToArray());
        }

    }
}
