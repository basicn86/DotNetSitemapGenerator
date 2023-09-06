using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSitemapGenerator.Utilities
{
    public class HtmlParser
    {
        WebPage page;
        HtmlDocument doc;

        public HtmlParser(WebPage page)
        {
            this.page = page;

            doc = new HtmlDocument();
            doc.LoadHtml(page.Content);
        }

        public List<Uri> GetLinks()
        {
            List<Uri> links = new List<Uri>();
            HtmlNodeCollection? nodes = doc.DocumentNode.SelectNodes("//a");

            if (nodes == null) return links;

            foreach (HtmlNode link in nodes)
            {
                try
                {
                    string? href = link?.Attributes["href"]?.Value;
                    if (string.IsNullOrWhiteSpace(href)) continue;

                    //skip mailto links
                    if (href.StartsWith("mailto:")) continue;

                    //create the uri
                    Uri uri = new Uri(page.Uri, href);

                    //if the hostname (authority) does not match, continue
                    if (uri.Host != page.Uri.Host) continue;

                    links.Add(new Uri(page.Uri, href));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return links;
        }
    }
}
