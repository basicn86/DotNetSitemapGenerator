using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSitemapGenerator.Utilities
{
    public static class SitemapGenerator
    {
        public static string GenerateSitemap(List<Uri> uris)
        {
            string result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
            result += "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n";
            foreach (Uri uri in uris)
            {
                result += "<url>\n";
                result += $"<loc>{uri.AbsoluteUri}</loc>\n";
                result += "</url>\n";
            }
            result += "</urlset>\n";
            return result;
        }
    }
}
