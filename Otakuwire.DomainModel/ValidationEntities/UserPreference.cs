using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Otakuwire.DomainModel.ValidationEntities
{
    public class UserPreference : IDataErrorInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public string EmailConfirm { get; set; }

        public string Error
        {
            get
            {
                if (Password != PasswordConfirm)
                {
                    return "Please make sure your Password and Confirmation Password are the same.";
                }
                if (Email != EmailConfirm)
                {
                    return "Please make sure your Email and Confirmation Email are the same.";
                }

                return null;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                if ((propertyName == "Email") && !Regex.IsMatch(Email, ".+\\@.+\\..+"))
                {
                    return "Please enter a valid Email address";
                }
                if ((propertyName == "EmailConfirm") && !Regex.IsMatch(Email, ".+\\@.+\\..+"))
                {
                    return "Please enter a valid Confirmation Email address";
                }

                return null;
            }
        }
    }
}
