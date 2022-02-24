using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SimpleWebCrawler.Model
{
    [ExcludeFromCodeCoverage]
    public class Final
    {
        [JsonProperty("initialUrl")]
        public string InitialUrl { get; set; }

        [JsonProperty("requestedUrl")]
        public string RequestedUrl { get; set; }

        [JsonProperty("availableUrl")]
        public List<string> AvailableUrl { get; set; }
    }
}
