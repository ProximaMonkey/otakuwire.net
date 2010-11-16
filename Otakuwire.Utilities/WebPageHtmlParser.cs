using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Otakuwire.Utilities
{
    public class WebPageHtmlParser
    {
        private static WebClient _client;
        private static HtmlDocument _doc;

        private static void GetHtmlDocument(string url)
        {
            _client = new WebClient();
            _doc = new HtmlDocument();
            _doc.LoadHtml(_client.DownloadString(url));
        }

        public static bool UrlExists(string url)
        {
            try
            {
                WebResponse webResponse = WebRequest.Create(url).GetResponse();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string GetWebPageTitle(string url)
        {
            GetHtmlDocument(url);

            string webPageTitle = String.Empty;
            HtmlParsingHelper.GetHtmlNodeAttrValue(_doc.DocumentNode, "meta", "name", "title", "content", ref webPageTitle);

            if (webPageTitle == String.Empty)
            {
                HtmlParsingHelper.GetHtmlInnerTextValue(_doc.DocumentNode, "title", ref webPageTitle);
            }

            return webPageTitle;
        }

        public static string GetWebPageDescription(string url)
        {
            GetHtmlDocument(url);

            string webPageDescription = String.Empty;
            HtmlParsingHelper.GetHtmlNodeAttrValue(_doc.DocumentNode, "meta", "name", "description", "content", ref webPageDescription);

            if (webPageDescription == String.Empty)
            {
                HtmlParsingHelper.GetHtmlNodeAttrValue(_doc.DocumentNode, "meta", "name", "DESCRIPTION", "content", ref webPageDescription);
            }

            return webPageDescription;
        }
    }
}
