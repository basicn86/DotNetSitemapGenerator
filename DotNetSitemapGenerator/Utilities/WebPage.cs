using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSitemapGenerator.Utilities
{
    public class WebPage
    {
        //uri of the page
        public Uri Uri { get; set; }
        //html content of the page
        public string Content { get; set; }
        //was the page a redirect
        public bool Redirected { get; set; }
    }
}
