using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Otakuwire.DomainModel.DataEntities
{
    [Table(Name = "Users")]
    public class User
    {
        public enum Privilege
        {
            User,
            Admin
        }

        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column]
        public string UserName { get; set; }

        [Column]
        public string Password { get; set; }

        [Column]
        public string FirstName { get; set; }

        [Column]
        public string LastName { get; set; }

        [Column]
        public string Email { get; set; }

        [Column]
        public string Location { get; set; }

        [Column]
        public int VoteQuota { get; set; }

        [Column]
        public DateTime RegistrationDate { get; set; }

        [Column]
        public DateTime LoginDate { get; set; }

        [Column(DbType = "nvarchar(10)")]
        public Privilege PrivilegeType { get; set; }

        public bool LoggedIn { get; set; }

        public void EncryptPassword()
        {
            Password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");
        }
    }
}
