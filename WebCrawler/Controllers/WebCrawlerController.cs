using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleWebCrawler.Services.QueueService;

namespace WebCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebCrawlerController : ControllerBase
    {
        private readonly ILogger<WebCrawlerController> _logger;

        private readonly IProducerService _producerService;

        private readonly IConsumerService _consumerService;

        public WebCrawlerController(ILogger<WebCrawlerController> logger, IProducerService producerService, IConsumerService consumerService)
        {
            _logger = logger;
            _producerService = producerService;
            _consumerService = consumerService;
        }

        [HttpGet("{domainToCrawl}")]
        public async Task Get(string domainToCrawl)
        {
            try
            {
                _logger.LogInformation($"Crawling started for a website:");

                //seed initial url from the request 
                await _producerService.AddAndFlushToChannel($"https://{domainToCrawl}");

                //start consumer
                await _consumerService.StartConsumer();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
