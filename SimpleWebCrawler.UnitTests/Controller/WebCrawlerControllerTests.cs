using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleWebCrawler.Services.QueueService;
using WebCrawler.Controllers;

namespace SimpleWebCrawler.UnitTests.Controller
{
    [TestClass]
    public class WebCrawlerControllerTests
    {
        private Mock<ILogger<WebCrawlerController>> _loggerMock;

        private Mock<IProducerService> _producerServiceMock;

        private Mock<IConsumerService> _consumerServiceMock;

        private WebCrawlerController _webCrawlerController;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<WebCrawlerController>>();
            _producerServiceMock = new Mock<IProducerService>();
            _consumerServiceMock = new Mock<IConsumerService>();
            _webCrawlerController = new WebCrawlerController(_loggerMock.Object, _producerServiceMock.Object, _consumerServiceMock.Object);

        }

        [TestMethod]
        public void When_getmethod_called_AddAndFlushToChannelcalled_once()
        {
            _producerServiceMock.Setup(s => s.AddAndFlushToChannel(It.IsAny<string>()));

            _consumerServiceMock.Setup(s => s.StartConsumer());

            _webCrawlerController.Get("https://www.monzo.com").GetAwaiter().GetResult();

            _producerServiceMock.Verify(v => v.AddAndFlushToChannel(It.IsAny<string>()), Times.Once);

        }

        [TestMethod]
        public void When_getmethod_called_StartConsumercalled_once()
        {
            _producerServiceMock.Setup(s => s.AddAndFlushToChannel(It.IsAny<string>()));

            _consumerServiceMock.Setup(s => s.StartConsumer());

            _webCrawlerController.Get("https://www.monzo.com").GetAwaiter().GetResult();

            _consumerServiceMock.Verify(v => v.StartConsumer(), Times.Once);

        }
        //TODO improve this test to mock the logger and make sure logger is called when required.
        //[TestMethod]
        public void When_getmethod_called_LogInformationcalled_once()
        {
            _producerServiceMock.Setup(s => s.AddAndFlushToChannel(It.IsAny<string>()));

            _consumerServiceMock.Setup(s => s.StartConsumer());

            _webCrawlerController.Get("https://www.monzo.com").GetAwaiter().GetResult();

            _loggerMock.Verify(v => v.LogInformation(It.IsAny<string>()), Times.Once);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void When_getmethod_called_AddAndFlushToChannel_failed_then_StartConsumer_called_none()
        {
            _producerServiceMock.Setup(s => s.AddAndFlushToChannel(It.IsAny<string>())).ThrowsAsync(new Exception("Dummy exception"));

            _consumerServiceMock.Setup(s => s.StartConsumer());

            _webCrawlerController.Get("https://www.monzo.com").GetAwaiter().GetResult();

            _consumerServiceMock.Verify(v => v.StartConsumer(), Times.Never);

        }
    }
}
