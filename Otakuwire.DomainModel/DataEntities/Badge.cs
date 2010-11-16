using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Otakuwire.DomainModel.DataEntities
{
    [Table(Name = "Badges")]
    public class Badge
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string Description { get; set; }
    }
}
