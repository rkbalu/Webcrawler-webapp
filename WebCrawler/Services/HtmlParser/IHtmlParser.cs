using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SimpleWebCrawler.Services.HtmlParser
{
    public interface IHtmlParser
    {
        Task<HtmlDocument> ParseHtmlResponse(string htmlString);
    }
}
