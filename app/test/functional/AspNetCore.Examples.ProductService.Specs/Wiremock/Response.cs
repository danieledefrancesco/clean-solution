using System.Collections.Generic;

namespace AspNetCore.Examples.ProductService.Specs.Wiremock
{
    public class Response
    {
        public int Status { get; set; }
        public string StatusMessage { get; set; }
        public Dictionary<string,string> Headers { get; set; }
        public Dictionary<string,string> Cookies { get; set; }
        public string Body { get; set; }
        
    }
}