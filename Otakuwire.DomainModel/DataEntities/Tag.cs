using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Otakuwire.DomainModel.DataEntities
{
    [Table(Name = "Tags")]
    public class Tag
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column]
        public string Name { get; set; }
    }
}
