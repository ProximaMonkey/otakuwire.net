using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Otakuwire.Utilities
{
    public class VideoPageHtmlParser
    {
        public enum VideoProvider
        {
            YouTube,
            Veoh,
            Crunchyroll,
            NA
        }

        private static WebClient _client;
        private static HtmlDocument _doc;
        public static VideoProvider Provider { get; private set; }

        private static void GetHtmlDocument(string url)
        {
            _client = new WebClient();
            _doc = new HtmlDocument();
            _doc.LoadHtml(_client.DownloadString(url));

            if (url.Contains("youtube.com"))
            {
                Provider = VideoProvider.YouTube;
            }
            else if (url.Contains("veoh.com"))
            {
                Provider = VideoProvider.Veoh;
            }
            else if (url.Contains("crunchyroll.com"))
            {
                Provider = VideoProvider.Crunchyroll;
            }
            else
            {
                Provider = VideoProvider.NA;
            }
        }

        public static string GetVideoObjectHtmlCode(string url)
        {
            // Use HTML Agility Pack to parse the HTML DOM of a YouTube page, specifically look for the <link rel="canonical" href="/watch?v=XXXXXXXX">
            // tag, where "XXXXXXXX" is the video id.

            GetHtmlDocument(url);

            if (Provider == VideoProvider.YouTube)
            {
                string videoID = String.Empty;
                HtmlParsingHelper.GetHtmlNodeAttrValue(_doc.DocumentNode, "link", "rel", "canonical", "href", ref videoID);

                videoID = videoID.Replace("/watch?v=", String.Empty);

                return ("<object width=\"480\" height=\"295\"><param name=\"movie\" value=\"http://www.youtube.com/v/YOUTUBEVIDEOID&hl=en_US&fs=1&rel=0&color1=0x2b405b&color2=0x6b8ab6\"></param><param name=\"allowFullScreen\" value=\"true\"></param><param name=\"allowscriptaccess\" value=\"always\"></param><embed src=\"http://www.youtube.com/v/YOUTUBEVIDEOID&hl=en_US&fs=1&rel=0&color1=0x2b405b&color2=0x6b8ab6\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" width=\"480\" height=\"295\"></embed></object>").Replace("YOUTUBEVIDEOID", videoID);
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Retrieves the video title from the page that the video is embedded in. First tries to retrieve it from the meta tag, if fails, tries to retrieve 
        /// title from title tag.
        /// </summary>
        /// <returns></returns>
        public static string GetVideoTitle(string url)
        {
            // Two locations that the title of the video could be: inside a meta tag or inside of the title tag. Preferably use the meta tag for
            // more accurate information.

            GetHtmlDocument(url);

            string videoTitle = String.Empty;
            HtmlParsingHelper.GetHtmlNodeAttrValue(_doc.DocumentNode, "meta", "name", "title", "content", ref videoTitle);

            if (videoTitle == String.Empty)
            {
                HtmlParsingHelper.GetHtmlInnerTextValue(_doc.DocumentNode, "title", ref videoTitle);
            }

            return videoTitle;
        }

        public static string GetVideoDescription(string url)
        {
            GetHtmlDocument(url);

            string videoDescription = String.Empty;
            HtmlParsingHelper.GetHtmlNodeAttrValue(_doc.DocumentNode, "meta", "name", "description", "content", ref videoDescription);

            return videoDescription;
        }
    }
}
