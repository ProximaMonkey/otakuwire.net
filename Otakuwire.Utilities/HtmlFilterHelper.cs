using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace Otakuwire.Utilities
{
    // Implemented from code in "Pro ASP.net MVC Framework", first edition, by Steven Sanderson, page 467.
    public static class HtmlFilterHelper
    {
        static string[] RemoveChildrenOfTags = new string[] { "script", "style" };

        public static string Filter(string html, string[] allowedTags)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            StringBuilder buffer = new StringBuilder();
            Process(doc.DocumentNode, buffer, allowedTags);

            return buffer.ToString();
        }

        static void Process(HtmlNode node, StringBuilder buffer, string[] allowedTags)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Text:
                    buffer.Append(HttpUtility.HtmlEncode(((HtmlTextNode)node).Text));
                    break;

                case HtmlNodeType.Element:
                case HtmlNodeType.Document:
                    bool allowedTag = allowedTags.Contains(node.Name.ToLower());

                    if (allowedTag)
                    {
                        // Preserve the href attribute of <a></a> tags.
                        if (node.Name == "a" && node.Attributes.Contains("href"))
                        {
                            buffer.AppendFormat("<{0} {1}=\"{2}\">", new object[] { node.Name, "href", AffiliationHelper.LinkToAffiliates(node.Attributes["href"].Value) });
                        }
                        else
                        {
                            buffer.AppendFormat("<{0}>", node.Name);
                        }
                    }

                    if (!RemoveChildrenOfTags.Contains(node.Name))
                    {
                        foreach (HtmlNode childNode in node.ChildNodes)
                        {
                            Process(childNode, buffer, allowedTags);
                        }
                    }

                    if (allowedTag)
                    {
                        buffer.AppendFormat("</{0}>", node.Name);
                    }
                    break;
            }
        }
    }
}
