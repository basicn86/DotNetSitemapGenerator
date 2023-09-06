using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace DotNetSitemapGenerator.Utilities
{
    public static class HttpDownloader
    {
        private static HttpClient client;
        private const int MAX_REDIRECTS = 10;

        static HttpDownloader()
        {
            //http client handler
            var handler = new HttpClientHandler();

            //allow auto redirects
            handler.AllowAutoRedirect = true;

            //set max redirects
            handler.MaxAutomaticRedirections = MAX_REDIRECTS;

            //set http client
            client = new HttpClient(handler);

            client.Timeout = TimeSpan.FromSeconds(30);
        }

        public static async Task<WebPage> DownloadPageAsync(Uri uri)
        {
            WebPage page = new WebPage();
            HttpResponseMessage response = await client.GetAsync(uri);

            //verify status code is 200
            response.EnsureSuccessStatusCode();

            page.Uri = response.RequestMessage.RequestUri;
            page.Content = await response.Content.ReadAsStringAsync();
            page.Redirected = response.RequestMessage.RequestUri != uri;

            return page;
        }
    }
}
