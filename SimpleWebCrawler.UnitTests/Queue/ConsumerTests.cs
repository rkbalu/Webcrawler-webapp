using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleWebCrawler.client.Http;
using SimpleWebCrawler.Services.HtmlParser;
using SimpleWebCrawler.Services.QueueService;
using Simple_web_crawler.Channel;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;
using System.Net;
using HtmlAgilityPack;
using SimpleWebCrawler.Model;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using SimpleWebCrawler.Services.Priter;
using SimpleWebCrawler.Services.WebCrawlerProcessor;
using SimpleWebCrawler.Factory;
using System.Threading.Channels;

namespace SimpleWebCrawler.UnitTests.Queue
{
    [TestClass]
    public class ConsumerTests
    {
        private Mock<IProducerService> _producerServiceMock;
        private Mock<IHtmlParser> _htmlParserMock;
        private Mock<IHttpClient> _httpClientMock;
        private Mock<IPrinter> _printerMock;
        private ConsumerService _consumerService;
        private ChannelBuffer1 _sampleChannelBuffer;

        private Mock<IUrlHelper> _urlMock;
        private string _validHtmlString = string.Empty;
        private IHtmlParser _htmlParser;
        private Mock<IProcessor> _processorMock;
        private Mock<IChannelFactory> _channelFactoryMock;

        private IChannelFactory _channelFactory;

        private Channel<string[]> _testChannel;

        [TestInitialize]
        public void Setup()
        {
            _producerServiceMock = new Mock<IProducerService>();
            _htmlParserMock = new Mock<IHtmlParser>();
            _httpClientMock = new Mock<IHttpClient>();
            _printerMock = new Mock<IPrinter>();
            _processorMock = new Mock<IProcessor>();
            _testChannel = Channel.CreateBounded<string[]>(new BoundedChannelOptions(int.MaxValue)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,//TODO Could be changed to multiple writer to improve read performance
                SingleWriter = true
            });
            _channelFactory = new ChannelFactory(_testChannel);
            _channelFactoryMock = new Mock<IChannelFactory>();
            _consumerService = new ConsumerService(_htmlParserMock.Object, _httpClientMock.Object, _printerMock.Object, _processorMock.Object, _channelFactoryMock.Object);
            _sampleChannelBuffer = new ChannelBuffer1(_channelFactory);
            _urlMock = new Mock<IUrlHelper>();
            _htmlParser = new HtmlParser();

        }


        [TestMethod]
        public void Given_BufferHasUrlSeeded_ReaderAsyncCalled_then_ProcessLinks_called_AtleastOnce()
        {
            var _validHtmlString = File.ReadAllText("../../../Sample/samplehtml.html");
            _httpClientMock.Setup(s => s.GetAsyncContect(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(_validHtmlString), StatusCode = HttpStatusCode.OK });
            _htmlParserMock.Setup(s => s.ParseHtmlResponse(It.IsAny<string>()))
                .ReturnsAsync(_htmlParser.ParseHtmlResponse(_validHtmlString).GetAwaiter().GetResult());
            _channelFactoryMock.Setup(s => s.GetChannel()).Returns(_testChannel);
            _sampleChannelBuffer.Flush("https://docs.python.org/3/").GetAwaiter().GetResult();

            _ = _consumerService.ReaderAsync(_channelFactoryMock.Object.GetChannel());


            _processorMock.Verify(v => v.ProcessLinks(It.IsAny<HtmlDocument>(), It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Given_BufferHasUrlSeeded_ReaderAsyncCalled_then_GetChannel_called_AtleastOnce()
        {
            var _validHtmlString = File.ReadAllText("../../../Sample/samplehtml.html");
            _httpClientMock.Setup(s => s.GetAsyncContect(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(_validHtmlString), StatusCode = HttpStatusCode.OK });
            _htmlParserMock.Setup(s => s.ParseHtmlResponse(It.IsAny<string>()))
                .ReturnsAsync(_htmlParser.ParseHtmlResponse(_validHtmlString).GetAwaiter().GetResult());
            _channelFactoryMock.Setup(s => s.GetChannel()).Returns(_testChannel);
            _sampleChannelBuffer.Flush("https://docs.python.org/3/").GetAwaiter().GetResult();

            _ = _consumerService.ReaderAsync(_channelFactoryMock.Object.GetChannel());


            _channelFactoryMock.Verify(v => v.GetChannel(), Times.Once);
        }

        [TestMethod]
        public void Given_BufferHasUrlSeeded_ReaderAsyncCalled_then_GetAsyncContect_called_AtleastOnce()
        {
            var _validHtmlString = File.ReadAllText("../../../Sample/samplehtml.html");
            _httpClientMock.Setup(s => s.GetAsyncContect(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(_validHtmlString), StatusCode = HttpStatusCode.OK });
            _htmlParserMock.Setup(s => s.ParseHtmlResponse(It.IsAny<string>()))
                .ReturnsAsync(_htmlParser.ParseHtmlResponse(_validHtmlString).GetAwaiter().GetResult());
            _channelFactoryMock.Setup(s => s.GetChannel()).Returns(_testChannel);
            _sampleChannelBuffer.Flush("https://docs.python.org/3/").GetAwaiter().GetResult();

            _ = _consumerService.ReaderAsync(_channelFactoryMock.Object.GetChannel());


            _httpClientMock.Verify(v => v.GetAsyncContect(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Given_BufferHasUrlSeeded_ReaderAsyncCalled_then_ProcessLinks_throwsexception_then_consoleprint_called_once()
        {
            var _validHtmlString = File.ReadAllText("../../../Sample/samplehtml.html");
            _httpClientMock.Setup(s => s.GetAsyncContect(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(_validHtmlString), StatusCode = HttpStatusCode.OK });
            _htmlParserMock.Setup(s => s.ParseHtmlResponse(It.IsAny<string>()))
                .ReturnsAsync(_htmlParser.ParseHtmlResponse(_validHtmlString).GetAwaiter().GetResult());
            _channelFactoryMock.Setup(s => s.GetChannel()).Returns(_testChannel);
            _sampleChannelBuffer.Flush("https://docs.python.org/3/").GetAwaiter().GetResult();

            _processorMock.Setup(s => s.ProcessLinks(It.IsAny<HtmlDocument>(), It.IsAny<string>())).ThrowsAsync(new Exception("Html Parser failed for some reason"));

            _ = _consumerService.ReaderAsync(_channelFactoryMock.Object.GetChannel());


            _printerMock.Verify(v => v.PrintConsole(It.IsAny<string>()), Times.Once);
        }
    }
}
