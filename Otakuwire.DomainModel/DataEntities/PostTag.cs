using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Otakuwire.DomainModel.DataEntities
{
    [Table(Name = "PostTags")]
    public class PostTag
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column]
        public int TagID { get; set; }

        [Column]
        public int PostID { get; set; }
    }
}
