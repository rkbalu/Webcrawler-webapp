using System;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleWebCrawler.Services.HtmlParser;

namespace SimpleWebCrawler.UnitTests
{
    [TestClass]
    public class HtmlParserTests
    {
        private string testHtmlString = string.Empty;

        private HtmlParser htmlParser;

        [TestInitialize]
        public void Setup()
        {
            testHtmlString = "<strong>This text is important!</strong>";

            htmlParser = new HtmlParser();
        }

        [TestMethod]
        public void Given_a_Valid_Html_ParseHtmlResponse_method_parse_return_Htmldocument()
        {
            var actualResult = htmlParser.ParseHtmlResponse(testHtmlString).Result;

            Assert.AreEqual(typeof(HtmlDocument), actualResult.GetType());
        }

    }
}
