using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Otakuwire.DomainModel.DataEntities
{
    [Table(Name="Comments")]
    public class Comment
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column]
        public DateTime Date { get; set; }

        [Column]
        public string Content { get; set; }

        [Column]
        public string UserName { get; set; }

        [Column]
        public int ParentPostID { get; set; }

        [Column]
        public int ParentCommentID { get; set; }

        public List<Comment> Children { get; set; }
    }
}