using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Otakuwire.DomainModel.DataEntities
{
    [Table(Name = "Votes")]
    public class Vote
    {
        public enum Parent
        {
            NA,
            Post,
            Comment
        }

        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column]
        public int Value { get; set; }

        [Column(DbType = "nvarchar(10)")]
        public Parent ParentType { get; set; }

        [Column]
        public int ParentID { get; set; }

        [Column]
        public string UserName { get; set; }
    }
}
