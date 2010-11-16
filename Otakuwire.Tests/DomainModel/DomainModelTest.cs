using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Otakuwire.DomainModel;
using Otakuwire.DomainModel.DataEntities;

namespace Otakuwire.Tests.DomainModel
{
    /// <summary>
    /// Summary description for DomainModelTest
    /// </summary>
    [TestClass]
    public class DomainModelTest
    {
        [TestMethod]
        public void UserClassTest()
        {
            // Arrange
            // Create the user.
            User user = new User();
            user.UserName = "ksun";
            user.Password = "moo2moo";
            user.FirstName = "Ke";
            user.LastName = "Sun";
            user.Email = "kesun.421@gmail.com";
            user.PrivilegeType = User.Privilege.Admin;
            user.RegistrationDate = DateTime.Now;
            user.LoginDate = DateTime.Now;

            // Act
            // Submit the user.
            try
            {
                using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<User> userTable = userDataContext.GetTable<User>();

                    userTable.InsertOnSubmit(user);
                    userDataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
            }

            user = null;

            // Retrieve the user.
            try
            {
                using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<User> userTable = userDataContext.GetTable<User>();

                    user = (from userEntity in userTable where userEntity.UserName == "ksun" select userEntity).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }

            // Assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void PostClassTest()
        {
            Post post = new Post();
            post.Title = "A Test Post";
            post.Date = DateTime.Now;
            post.MediaType = Post.Media.Article;
            post.Content = "Yeppy Tai Yai Yay!";
            post.Flagged = false;

            // Submit a post.
            try
            {
                using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<Post> postTable = postDataContext.GetTable<Post>();

                    postTable.InsertOnSubmit(post);
                    postDataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
            }

            post = null;

            // Retrieve the post.
            try
            {
                using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<Post> postTable = postDataContext.GetTable<Post>();

                    post = (from postEntity in postTable where postEntity.Title.Contains("Test") select postEntity).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }

            Assert.IsNotNull(post);
        }

        [TestMethod]
        public void CommentClassTest()
        {

        }

        [TestMethod]
        public void VoteClassTest()
        {
            Vote vote1 = new Vote { ParentID = 1, ParentType = Vote.Parent.Comment, Value = 1 };
            Vote vote2 = new Vote { ParentID = 1, ParentType = Vote.Parent.Comment, Value = -1 };
            Vote vote3 = new Vote { ParentID = 1, ParentType = Vote.Parent.Comment, Value = -1 };
            Vote vote4 = new Vote { ParentID = 1, ParentType = Vote.Parent.Comment, Value = -1 };
            Vote vote5 = new Vote { ParentID = 1, ParentType = Vote.Parent.Comment, Value = 1 };

            // Submit a bunch of votes for a comment.
            using (DataContext voteDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<Vote> voteTable = voteDataContext.GetTable<Vote>();

                voteTable.InsertOnSubmit(vote1);
                voteTable.InsertOnSubmit(vote2);
                voteTable.InsertOnSubmit(vote3);
                voteTable.InsertOnSubmit(vote4);
                voteTable.InsertOnSubmit(vote5);

                voteDataContext.SubmitChanges();
            }

            // Count the total number of votes.
            using (DataContext voteDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<Vote> voteTable = voteDataContext.GetTable<Vote>();

                var voteCount = (from voteEntity in voteTable 
                                 where voteEntity.ParentID == 1 && voteEntity.ParentType == Vote.Parent.Comment 
                                 select voteEntity.Value).Sum();
            }
        }
    }
}
