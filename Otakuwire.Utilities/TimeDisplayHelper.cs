using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Otakuwire.Utilities
{
    public class TimeDisplayHelper
    {
        public static string TimeDisplay(DateTime time)
        {
            // Display a meaningful timespan for users.
            // Timespan values calculate time lapse by the absolute time, so comparing a time stamp that is overnight would return
            // less than 1 day, even though the date has changed. So, we need to also use DateTime values to compare the
            // exact number of days from the date of the post to the current date.
            TimeSpan timeSpanSincePost = DateTime.Now.Subtract(time);
            int daysSincePost = DateTime.Now.Date.Subtract(time.Date).Days;

            if (daysSincePost >= 2)
            {
                return time.Date.ToShortDateString();
            }
            else if (daysSincePost == 1)
            {
                return "yesterday";
            }
            else if (timeSpanSincePost.Hours == 1)
            {
                return timeSpanSincePost.Hours.ToString() + " hour ago";
            }
            else if (timeSpanSincePost.Hours > 1)
            {
                return timeSpanSincePost.Hours.ToString() + " hours ago";
            }
            else if (timeSpanSincePost.Hours < 1 && timeSpanSincePost.Minutes > 5)
            {
                return timeSpanSincePost.Minutes.ToString() + " minutes ago";
            }
            else
            {
                return "just now";
            }
        }
    }
}
