using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Otakuwire.Utilities
{
    public class AffiliationHelper
    {
        public static string LinkToAffiliates(string url)
        {
            Amazon(ref url);

            return url;
        }

        private static void Amazon(ref string url)
        {
            if (url.Contains("http://www.amazon.com/") && url.Contains("ref="))
            {
                url += "&tag=otakuwire.net-20";
            }
        }
    }
}
