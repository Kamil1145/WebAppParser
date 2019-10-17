using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppParser.Controllers
{
    public class ParseManager
    {

        public Dictionary<string, string> Selector(string url)
        {
            if (url.Contains("olx"))
            {
                OlxParser olx = new OlxParser();
                var property = olx.Scrape(url);
                return property;
            }
            else
            {
                OtoDomParser otoDom = new OtoDomParser();
                var property = otoDom.Scrape(url);
                return property;
            }

        }
    }
}
