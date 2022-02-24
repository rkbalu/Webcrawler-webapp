using System;
using SimpleWebCrawler.Model;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SimpleWebCrawler.Services.Priter
{
    [ExcludeFromCodeCoverage]
    public class Printer : IPrinter
    {
        public void PrintConsole(string item)
        {
            Console.WriteLine(item);
        }
    }
}
