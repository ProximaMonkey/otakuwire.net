using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Otakuwire.DomainModel.DataEntities
{
    [Table(Name = "Posts")]
    public class Post
    {
        public enum Media
        {
            NA,
            Audio,
            Article,
            Blog,
            Question,
            Image,
            Video
        }

        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column]
        public string SourceURI { get; set; }

        [Column]
        public string Title { get; set; }

        [Column]
        public string Description { get; set; }

        [Column]
        public string Content { get; set; }

        [Column]
        public DateTime Date { get; set; }

        [Column(DbType = "nvarchar(20)")]
        public Media MediaType { get; set; }

        [Column]
        public string UserName { get; set; }

        [Column]
        public bool Flagged { get; set; }

        [Column]
        public int ViewCount { get; set; }

        [Column]
        public int VoteCount { get; set; }

        public List<Comment> Comments { get; set; }

        public int CommentsCount { get; set; }
    }
}
