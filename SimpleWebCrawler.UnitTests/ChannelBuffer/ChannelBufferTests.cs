using System;
using System.Threading.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleWebCrawler.Factory;
using Simple_web_crawler.Channel;
using System.Threading;

namespace SimpleWebCrawler.UnitTests.ChannelBuffer
{
    [TestClass]
    public class ChannelBufferTests
    {
        private Mock<IChannelFactory> _channelFactoryMock;
        private Channel<string[]> _testChannel;
        private ChannelBuffer1 _channelBuffer;
        private Mock<Channel<string[]>> _channelMock;

        [TestInitialize]
        public void Setup()
        {
            _testChannel = Channel.CreateBounded<string[]>(new BoundedChannelOptions(int.MaxValue)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,//TODO Could be changed to multiple writer to improve read performance
                SingleWriter = true
            });

            _channelFactoryMock = new Mock<IChannelFactory>();
            _channelBuffer = new ChannelBuffer1(_channelFactoryMock.Object);
            _channelMock = new Mock<Channel<string[]>>();
        }

        //TODO need to workout the mocking option for the channel writer...
        //[TestMethod]
        //public void Given_flush_called_then_WriteAsync_called_once()
        //{
        //    _channelMock.Setup(s => s.Writer.WriteAsync(It.IsAny<string[]>(), It.IsAny<CancellationToken>())).Returns(new System.Threading.Tasks.ValueTask());

        //    _channelFactoryMock.Setup(s => s.GetChannel()).Returns(_channelMock.Object);


        //    _channelBuffer.Flush("x").GetAwaiter().GetResult();

        //    Assert.AreEqual(1, 1);
        //}
    }
}
