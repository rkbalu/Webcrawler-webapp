using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SimpleWebCrawler.Model;

namespace SimpleWebCrawler.Services.WebCrawlerProcessor
{
    public interface IProcessor
    {
        Task ProcessLinks(HtmlDocument htmlDocument, string item);
    }
}
