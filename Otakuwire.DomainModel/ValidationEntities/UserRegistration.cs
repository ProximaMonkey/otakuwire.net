using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Otakuwire.DomainModel.ValidationEntities
{
    public class UserRegistration : IDataErrorInfo
    {
        public string UserName { get; set; }
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
                if (UserNameAlreadyUsed())
                {
                    return "The User Name has already been registered, please pick a new one.";
                }

                return null;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                if ((propertyName == "UserName") && (string.IsNullOrEmpty(UserName) || !Regex.IsMatch(UserName, "^([a-zA-Z0-9-_]{3,16})$")))
                {
                    return "Please enter a valid User Name";
                }
                if ((propertyName == "Password") && string.IsNullOrEmpty(Password))
                {
                    return "Please enter a Password";
                }
                if ((propertyName == "PasswordConfirm") && string.IsNullOrEmpty(PasswordConfirm))
                {
                    return "Please confirm your Password";
                }
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

        private bool UserNameAlreadyUsed()
        {
            using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<DomainModel.DataEntities.User> userTable = userDataContext.GetTable<DomainModel.DataEntities.User>();

                var userRegsitered = (from userEntity in userTable where userEntity.UserName == UserName select userEntity).FirstOrDefault();

                if (userRegsitered != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
