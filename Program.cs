using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace HTML_SCRAPE
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainPageLinks = GetMainPageLinks("https://newyork.craigslist.org/d/computer-gigs/search/cpg");
            var lstGigs = GetPageDetails(mainPageLinks);
        }
        static HtmlNode GetHtml(string url)
        {
            WebPage webpage = _browser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
        static List<string> GetMainPageLinks(string url)
        {
            var homePageLinks = new List<string>();
            var html = GetHtml(url);
            var links = html.CssSelect("a");

            foreach (var link in links)
            {
                if (link.Attributes["href"].Value.Contains(".html"))
                {
                    homePageLinks.Add(link.Attributes["href"].Value);
                }
            }
            return homePageLinks;
        }
        public class PageDetails
        {//Define class for scrape info
            public string title { get; set; }
            public string description { get; set; }
            public string url { get; set; }
        }
        static List<PageDetails> GetPageDetails(List<string> urls)
        {//Define method for getting info from html
            var lstPageDetails = new List<PageDetails>();
            foreach (var url in urls)
            {
                var htmlNode = GetHtml(url);
                var pageDetails = new PageDetails();
                pageDetails.title = htmlNode.OwnerDocument.DocumentNode
                  .SelectSingleNode("//html/head/title").InnerText;

                var description = htmlNode.OwnerDocument.DocumentNode
                  .SelectSingleNode("//html/body/section/section/section/section").InnerText;
                pageDetails.description = description
                  .Replace("\n        \n            QR Code Link to This Post\n            \n        \n", "");

                pageDetails.url = url;
                lstPageDetails.Add(pageDetails);
            }

            return lstPageDetails;
        }
    }
}
