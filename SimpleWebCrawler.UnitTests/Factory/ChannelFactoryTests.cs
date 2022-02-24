using System;
using System.Threading.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleWebCrawler.Factory;

namespace SimpleWebCrawler.UnitTests.Factory
{
    [TestClass]
    public class ChannelFactoryTests
    {
        private ChannelFactory _channelFactory;
        private Channel<string[]> _testChannel;

        [TestInitialize]
        public void Setup()
        {
            _testChannel = Channel.CreateBounded<string[]>(new BoundedChannelOptions(int.MaxValue)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,//TODO Could be changed to multiple writer to improve read performance
                SingleWriter = true
            });

            _channelFactory = new ChannelFactory(_testChannel);
        }

        [TestMethod]
        public void Given_GetChannel_called_then_return_channel()
        {
            var actual = _channelFactory.GetChannel();

            Assert.AreEqual(_testChannel.GetType(), actual.GetType());
        }
    }
}
