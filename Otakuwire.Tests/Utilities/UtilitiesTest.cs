using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Otakuwire.Utilities;

namespace Otakuwire.Tests.Utilities
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void HtmlFilterClassTest()
        {
            string html = "<p><a href=\"www.google.com\">Google</a></p> <b>BOLD</b> <p><a href=\"www.tofu.com\">Tofu</a></p>";
            string filteredResult = HtmlFilterHelper.Filter(html, new string[] { "a", "b", "i", "p", "div" });
        }

        [TestMethod]
        public void VideoPageHtmlParserTest()
        {
            string url = "http://www.youtube.com/watch?v=bbUqEPUZ-ds&feature=topvideos";
            string videoObjectHtml = VideoPageHtmlParser.GetVideoObjectHtmlCode(url);
            string videoTitle = VideoPageHtmlParser.GetVideoTitle(url);
            string videoDescription = VideoPageHtmlParser.GetVideoDescription(url);
        }

        [TestMethod]
        public void WebPageHtmlParserTest()
        {
            bool urlExists = WebPageHtmlParser.UrlExists("http://www.google.com/");

            string url = "http://www.google.com";
            string webPageTitle = WebPageHtmlParser.GetWebPageTitle(url);
            string webPageDescrption = WebPageHtmlParser.GetWebPageDescription(url);
        }
    }
}
