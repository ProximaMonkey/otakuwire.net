using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Otakuwire.Utilities
{
    public class UrlHelper
    {
        /// <summary>
        /// Assume the url input is Html Encoded.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string TrimIllegalCharacters(string url)
        {
            string[] charsNotAllowed = new string[] { ":", ";", ".", ">", ",", "<", "/", "?", "\"", "'", "|", "{", "}", 
                "[", "]", "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "=" };

            for (int i = 0; i < charsNotAllowed.Length; i++)
            {
                url = url.Trim().Replace(charsNotAllowed[i], "");
            }

            return url.ToLower().Replace("+", "-"); ;
        }

        public static string GetRootUrl(string url)
        {
            if (!url.Contains("http://"))
            {
                return "source";
            }
            else
            {
                url = url.Replace("http://", "");
                url = url.Replace(url.Substring(url.IndexOf("/")), "");
                return url;
            }
        }
    }
}
