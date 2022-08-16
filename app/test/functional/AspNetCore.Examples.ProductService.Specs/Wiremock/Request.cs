using System.Collections.Generic;

namespace AspNetCore.Examples.ProductService.Specs.Wiremock
{
    public class Request
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public string UrlPath { get; set; }
        public string UrlPathPatter { get; set; }
        public string UrlPattern { get; set; }
        public Dictionary<string,string> QueryParameters { get; set; }
        public Dictionary<string,string> Headers { get; set; }
        public Dictionary<string,string> Cookies { get; set; }
    }
}