using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Otakuwire.Utilities;

namespace Otakuwire.DomainModel.ValidationEntities
{
    public class PostSubmission : IDataErrorInfo
    {
        public enum Media // Make sure the Media enum matches the Post class Media enum.
        {
            NA,
            Audio,
            Article,
            Blog,
            Question,
            Image,
            Video
        }

        public string SourceURI { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public Media MediaType { get; set; }

        public string this[string propertyName]
        {
            get
            {
                if ((propertyName == "Title")
                    && String.IsNullOrEmpty(Title))
                {
                    return "Post requires a Title...";
                }

                return null;
            }
        }

        public string Error
        {
            get
            {
                if (MediaType == Media.Article || MediaType == Media.Video || MediaType == Media.Image)
                {
                    if ((String.IsNullOrEmpty(SourceURI) || SourceURI.Substring(0, 7) != "http://" || !WebPageHtmlParser.UrlExists(SourceURI)))
                    {
                        return "Invalid url...";
                    }

                    //if (String.IsNullOrEmpty(Description))
                    //{
                    //    return "Post requires a description...";
                    //}
                }

                // Consideration of the laziness of users has made me to decide to not require posts to have descriptions or content,
                // just a title will do.

                //if (MediaType == Media.Question || MediaType == Media.Blog)
                //{
                //    if (String.IsNullOrEmpty(Content))
                //    {
                //        return "Post requires content...";
                //    }
                //}

                return null;
            }
        }

    }
}
