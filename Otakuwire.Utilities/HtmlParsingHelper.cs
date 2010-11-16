using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Otakuwire.Utilities
{
    public class HtmlParsingHelper
    {
        public static void GetHtmlInnerTextValue(HtmlNode node, string htmlNodeName, ref string returnHtmlNodeTextValue)
        {
            if (returnHtmlNodeTextValue != String.Empty)
            {
                return;
            }

            if (node.Name == htmlNodeName)
            {
                returnHtmlNodeTextValue = node.InnerText;
            }
            else
            {
                foreach (HtmlNode childNode in node.ChildNodes)
                {
                    GetHtmlInnerTextValue(childNode, htmlNodeName, ref returnHtmlNodeTextValue);
                }
            }
        }

        public static void GetHtmlNodeAttrValue(HtmlNode node, string htmlNodeName, string attrName, string attrValue, string returnAttrName, ref string returnAttrValue)
        {
            if (returnAttrValue != String.Empty)
            {
                return;
            }

            if (node.Name == htmlNodeName && node.Attributes.Contains(attrName) && node.Attributes[attrName].Value == attrValue && node.Attributes.Contains(returnAttrName))
            {
                returnAttrValue = node.Attributes[returnAttrName].Value;
            }
            else
            {
                foreach (HtmlNode childNode in node.ChildNodes)
                {
                    GetHtmlNodeAttrValue(childNode, htmlNodeName, attrName, attrValue, returnAttrName, ref returnAttrValue);
                }
            }
        }

        public static void GetHtmlNodeAttrValue(HtmlNode node, string htmlNodeName, string returnAttrName, ref string returnAttrValue)
        {
            if (returnAttrValue != String.Empty)
            {
                return;
            }

            if (node.Name == htmlNodeName && node.Attributes.Contains(returnAttrName))
            {
                returnAttrValue = node.Attributes[returnAttrName].Value;
            }
            else
            {
                foreach (HtmlNode childNode in node.ChildNodes)
                {
                    GetHtmlNodeAttrValue(childNode, htmlNodeName, returnAttrName, ref returnAttrValue);
                }
            }
        }

        public static string ReplaceLineBreaksWithHtml(string text)
        {
            return text.Replace("\n", "<br />");
        }
    }
}
