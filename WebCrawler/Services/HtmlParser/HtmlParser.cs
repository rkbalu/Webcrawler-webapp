using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SimpleWebCrawler.Services.HtmlParser
{
    public class HtmlParser : IHtmlParser
    {
        public Task<HtmlDocument> ParseHtmlResponse(string htmlString)
        {
            return Task.Run(() =>
            {
                HtmlDocument htmlDoc = new HtmlDocument();//TODO can be injected or task not needed

                htmlDoc.LoadHtml(htmlString); 

                return htmlDoc;
            });
        }
    }
}
