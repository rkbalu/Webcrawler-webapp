using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Simple_web_crawler.Channel;
using SimpleWebCrawler.Services.QueueService;
using SimpleWebCrawler.Model;

namespace SimpleWebCrawler.UnitTests.Queue
{


    [TestClass]
    public class ProducerTests
    {
        private Mock<IChannelBuffer> _channelBufferMock;

        private ProducerService _producerService;

        [TestInitialize]
        public void Setup()
        {
            _channelBufferMock = new Mock<IChannelBuffer>();
            _producerService = new ProducerService(_channelBufferMock.Object);
        }


        [TestMethod]
        public void given_AddAndFlushToChannelCalled_withsingleurl_channelbuffer_add_called_once()
        {
            _channelBufferMock.Setup(s => s.Flush(It.IsAny<string>()));

            _producerService.AddAndFlushToChannel("www.google.com").GetAwaiter().GetResult();

            _channelBufferMock.Verify(v => v.Flush(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void given_AddAndFlushToChannelCalled_with2urls_channelbuffer_add_called_exactly_twice()
        {
            _channelBufferMock.Setup(s => s.Flush(It.IsAny<string>()));

            var finalObject = new Final()
            {
                RequestedUrl = "https://www.monzo.com",
                AvailableUrl = new System.Collections.Generic.List<string>()
                {
                    "i/loans",
                    "i/currentaccount"
                }
            };

            _producerService.AddAndFlushToChannel(finalObject).GetAwaiter().GetResult();

            _channelBufferMock.Verify(v => v.Flush(It.IsAny<string>()), Times.Exactly(finalObject.AvailableUrl.Count));
        }

        [TestMethod]
        public void given_clearlcalled_channelbuffer_clear_called_once()
        {
            _channelBufferMock.Setup(s => s.Clear());

            _producerService.EndProducer();

            _channelBufferMock.Verify(v => v.Clear(), Times.Once);
        }

    }
}
