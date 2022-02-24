using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleWebCrawler.Model;
using SimpleWebCrawler.Services.HtmlParser;
using SimpleWebCrawler.Services.Priter;
using SimpleWebCrawler.Services.QueueService;
using SimpleWebCrawler.Services.WebCrawlerProcessor;

namespace SimpleWebCrawler.UnitTests.Processor
{
    [TestClass]
    public class ProcessorTests
    {
        private Mock<IProducerService> _producerServiceMock;

        private Mock<IPrinter> _printerMock;

        private HtmlParser _htmlParser;

        private IProcessor _processor;

        [TestInitialize]
        public void Setup()
        {
            _producerServiceMock = new Mock<IProducerService>();
            _printerMock = new Mock<IPrinter>();
            _htmlParser = new HtmlParser();
            _processor = new Process(_producerServiceMock.Object, _printerMock.Object);
        }

        [TestMethod]
        public void Given_htmlpassed_ProcessLinksCalled_then_PrintConsole_called_AtleastOnce()
        {
            var _validHtmlString = File.ReadAllText("../../../Sample/samplehtml.html");

            var parsedAsHtmlDocument = _htmlParser.ParseHtmlResponse(_validHtmlString).GetAwaiter().GetResult();

            _processor.ProcessLinks(parsedAsHtmlDocument, "hello.com");

            _printerMock.Verify(v => v.PrintConsole(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Given_htmlpassed_ProcessLinksCalled_then_AddAndFlushToChannel_called_AtleastOnce()
        {
            var _validHtmlString = File.ReadAllText("../../../Sample/samplehtml.html");

            var parsedAsHtmlDocument = _htmlParser.ParseHtmlResponse(_validHtmlString).GetAwaiter().GetResult();

            _processor.ProcessLinks(parsedAsHtmlDocument, "hello.com");

            _producerServiceMock.Verify(v => v.AddAndFlushToChannel(It.IsAny<Final>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Given_htmlpassedwithwrong_ProcessLinksCalled_then_AddAndFlushToChannel_called_Never()
        {
            var _validHtmlString = "<dummy without href>";

            var parsedAsHtmlDocument = _htmlParser.ParseHtmlResponse(_validHtmlString).GetAwaiter().GetResult();

            _processor.ProcessLinks(parsedAsHtmlDocument, "hello.com");

            _producerServiceMock.Verify(v => v.AddAndFlushToChannel(It.IsAny<Final>()), Times.Never);
        }
    }
}
