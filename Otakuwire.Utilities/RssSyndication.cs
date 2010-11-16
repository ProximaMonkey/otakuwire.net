using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Otakuwire.Utilities
{
    /// <summary>
    /// Class containing methods for retrieving syndication feeds.
    /// </summary>
    public class RssSyndication
    {
        /// <summary>
        /// Extracts and creates a list of feed items from RSS/ATOM URLs.
        /// </summary>
        /// <param name="feedUriList">List of URLs to extract feed items from.</param>
        /// <param name="feedItemCount">Number of feed items to be included from each source RSS/ATOM URL.</param>
        /// <param name="feedSummaryCharLength">Number of characters to be included in the feed summary.</param>
        /// <param name="stripHtmlTags">Strips the HTML tags out of the feed summary.</param>
        /// <returns>List of Feed items.</returns>
        public static List<RSS> BuildFeed(List<string> feedUriList, int feedItemCount, int feedSummaryCharLength, bool stripHtmlTags)
        {
            List<RSS> feedItemsList = new List<RSS>();

            // From each feed URL, collect a number of feeds, then sort the feed list by publishing date.
            foreach (string feedUri in feedUriList)
            {
                SyndicationFeed syndicationFeed = SyndicationFeed.Load(XmlReader.Create(feedUri));

                List<RSS> tempFeedList = new List<RSS>();

                foreach (SyndicationItem syndicationItem in syndicationFeed.Items)
                {
                    RSS feedItem = new RSS();

                    feedItem.FeedSource = syndicationFeed.Title.Text;
                    feedItem.FeedPublishDateTime = syndicationItem.PublishDate;
                    feedItem.FeedTitle = syndicationItem.Title.Text;
                    feedItem.FeedUrl = syndicationItem.Id;
                    feedItem.FeedSummary = syndicationItem.Summary.Text;

                    tempFeedList.Add(feedItem);
                }

                tempFeedList = (from feedItem in tempFeedList orderby feedItem.FeedPublishDateTime descending select feedItem).ToList();

                for (int i = 0; i < feedItemCount; i++)
                {
                    if (i < tempFeedList.Count)
                    {
                        feedItemsList.Add(tempFeedList[i]);
                    }
                }

                tempFeedList = null;
            }

            feedItemsList = (from feedItem in feedItemsList orderby feedItem.FeedPublishDateTime descending select feedItem).ToList();

            for (int i = 0; i < feedItemsList.Count; i++)
            {
                if (stripHtmlTags)
                {
                    feedItemsList[i].FeedSummary = HtmlFilterHelper.Filter(feedItemsList[i].FeedSummary, new string[] { });
                }

                feedItemsList[i].FeedSummary = feedItemsList[i].FeedSummary.Substring(0, feedSummaryCharLength) + "...";
            }

            return feedItemsList;
        }

        /// <summary>
        /// Struct for feed data.
        /// </summary>
        public class RSS
        {
            public string FeedSource { get; set; }
            public string FeedTitle { get; set; }
            public string FeedUrl { get; set; }
            public DateTimeOffset FeedPublishDateTime { get; set; }
            public string FeedSummary { get; set; }
        }
    }
}
