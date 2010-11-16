using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Otakuwire.DomainModel;
using Otakuwire.DomainModel.DataEntities;
using Otakuwire.Utilities;

namespace Otakuwire.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(int? id)
        {
            int pageNumber;
            int postsPerPage = 10;
            int totalPosts;
            int totalPages;
            int randomPostNumber;

            string mediaType = Enum.GetNames(typeof(DomainModel.DataEntities.Post.Media)).ToList().Contains(Request.QueryString["m"]) == false ? "All" : Request.QueryString["m"];
            string sortOrder = String.IsNullOrEmpty(Request.QueryString["o"]) ? "New" : Request.QueryString["o"];
           
            if (id == null || id < 1)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber = (int)id;
            }

            List<Post> posts = new List<Post>();

            // Take a subset from the total posts table, by which page the user is navigating to.
            using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<Post> postTable = postDataContext.GetTable<Post>();

                if (mediaType != "All")
                {
                    // Order the posts by user selection.
                    switch (sortOrder)
                    {
                        default:
                        case "":
                        case "New":
                            {
                                posts = (from postEntity in postTable
                                         where postEntity.MediaType.ToString() == mediaType
                                         orderby postEntity.Date descending
                                         select postEntity).Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage).ToList();
                            }
                            break;

                        case "Hot":
                            {
                                posts = (from postEntity in postTable
                                         where postEntity.MediaType.ToString() == mediaType
                                         orderby (postEntity.VoteCount + postEntity.ViewCount) descending
                                         select postEntity).Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage).ToList();
                            }
                            break;

                        case "Week":
                            {
                                posts = (from postEntity in postTable
                                         where postEntity.MediaType.ToString() == mediaType && postEntity.Date > DateTime.Now.AddDays(-7)
                                         orderby postEntity.Date descending
                                         select postEntity).Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage).ToList();
                            }
                            break;

                        case "Month":
                            {
                                posts = (from postEntity in postTable
                                         where postEntity.MediaType.ToString() == mediaType && postEntity.Date > DateTime.Now.AddDays(-30)
                                         orderby postEntity.Date descending
                                         select postEntity).Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage).ToList();
                            }
                            break;
                    }

                    totalPosts = (from postEntity in postTable
                                  where postEntity.MediaType.ToString() == mediaType
                                  select postEntity).Count();
                }
                else
                {
                    // Order the posts by user selection.
                    switch (sortOrder)
                    {
                        default:
                        case "":
                        case "New":
                            {
                                posts = (from postEntity in postTable
                                         orderby postEntity.Date descending
                                         select postEntity).Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage).ToList();
                            }
                            break;

                        case "Hot":
                            {
                                posts = (from postEntity in postTable
                                         orderby (postEntity.VoteCount + postEntity.ViewCount) descending
                                         select postEntity).Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage).ToList();
                            }
                            break;

                        case "Week":
                            {
                                posts = (from postEntity in postTable
                                         where postEntity.Date > DateTime.Now.AddDays(-7)
                                         orderby postEntity.Date descending
                                         select postEntity).Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage).ToList();
                            }
                            break;

                        case "Month":
                            {
                                posts = (from postEntity in postTable
                                         where postEntity.Date > DateTime.Now.AddDays(-30)
                                         orderby postEntity.Date descending
                                         select postEntity).Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage).ToList();
                            }
                            break;
                    }

                    totalPosts = (from postEntity in postTable
                                  select postEntity).Count();
                }

                // Select a randome post for displaying to amuse the user.
                Random rand = new Random(DateTime.Now.Millisecond);
                randomPostNumber = (from postEntity in postTable select postEntity.ID).Skip(rand.Next(totalPosts - 2)).FirstOrDefault();

                totalPages = (int)Math.Ceiling((double)totalPosts / postsPerPage);

                if (pageNumber > totalPages)
                    pageNumber = totalPages;
            }

            // Get the number of comments for the posts.
            using (DataContext cmtDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<Comment> cmtTable = cmtDataContext.GetTable<Comment>();

                for (int i = 0; i < posts.Count; i++)
                {
                    posts[i].CommentsCount = (from cmtEntity in cmtTable
                                              where cmtEntity.ParentPostID == posts[i].ID
                                              select cmtEntity).Count();
                }
            }

            ViewData["Posts"] = posts;
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["RandomPostNumber"] = randomPostNumber;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(DomainModel.DataEntities.User user)
        {
            user.EncryptPassword();

            using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<DomainModel.DataEntities.User> userTable = userDataContext.GetTable<DomainModel.DataEntities.User>();

                var userVerified = (from userEntity in userTable
                                    where userEntity.UserName == user.UserName && userEntity.Password == user.Password
                                    select userEntity).FirstOrDefault();

                if (userVerified != null)
                {
                    userVerified.LoggedIn = true;
                    userVerified.LoginDate = DateTime.Now;

                    userDataContext.SubmitChanges(); // Save the user's last login date to database.

                    Session["User"] = userVerified; // Saving the user profile in session will allow easy access to user data.

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Message"] = "Login incorrect, please try again...";

                    return View(user);
                }
            }
        }

        public ActionResult Logout()
        {
            Session["User"] = new DomainModel.DataEntities.User();
            Session.Abandon();

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Register()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(DomainModel.ValidationEntities.UserRegistration userReg)
        {
            if (ModelState.IsValid) // Validation (fields and username) is delegated to UserRegistration class.
            {
                // Save user registration to the database.
                DomainModel.DataEntities.User user = new Otakuwire.DomainModel.DataEntities.User();
                user.UserName = userReg.UserName;
                user.Password = userReg.Password; user.EncryptPassword();
                user.Email = userReg.Email;
                user.RegistrationDate = DateTime.Now;
                user.LoginDate = DateTime.Now;
                user.VoteQuota = 33;

                using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<DomainModel.DataEntities.User> userTable = userDataContext.GetTable<DomainModel.DataEntities.User>();

                    userTable.InsertOnSubmit(user);
                    userDataContext.SubmitChanges();
                }

                user.LoggedIn = true;

                Session["User"] = user;

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Preference()
        {
            DomainModel.DataEntities.User user = (DomainModel.DataEntities.User)Session["User"];

            if (user.LoggedIn)
            {
                DomainModel.ValidationEntities.UserPreference userPref = new Otakuwire.DomainModel.ValidationEntities.UserPreference();

                userPref.FirstName = user.FirstName;
                userPref.LastName = user.LastName;
                userPref.Location = user.Location;
                userPref.Email = user.Email;
                userPref.EmailConfirm = user.Email;
                userPref.Password = String.Empty;
                userPref.PasswordConfirm = String.Empty;

                return View(userPref);
            }
            else
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Preference(DomainModel.ValidationEntities.UserPreference userPref)
        {
            DomainModel.DataEntities.User user = (DomainModel.DataEntities.User)Session["User"];

            if (user.LoggedIn)
            {
                if (ModelState.IsValid)
                {
                    using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
                    {
                        Table<DomainModel.DataEntities.User> userTable = userDataContext.GetTable<User>();

                        DomainModel.DataEntities.User userPrefToUpdate = (from userEntity in userTable where userEntity.UserName == user.UserName select userEntity).FirstOrDefault();

                        userPrefToUpdate.FirstName = userPref.FirstName;
                        userPrefToUpdate.LastName = userPref.LastName;
                        userPrefToUpdate.Email = userPref.Email;
                        userPrefToUpdate.Location = userPref.Location;

                        ((DomainModel.DataEntities.User)Session["User"]).FirstName = userPref.FirstName;
                        ((DomainModel.DataEntities.User)Session["User"]).LastName = userPref.LastName;
                        ((DomainModel.DataEntities.User)Session["User"]).Email = userPref.Email;
                        ((DomainModel.DataEntities.User)Session["User"]).Location = userPref.Location;

                        if (!String.IsNullOrEmpty(userPref.Password))
                        {
                            userPrefToUpdate.Password = userPref.Password;
                            userPrefToUpdate.EncryptPassword();
                        }

                        userDataContext.SubmitChanges();

                        ViewData["Message"] = "Preferences saved!";
                    }
                }

                return View(userPref);
            }
            else
            {
                return RedirectToAction("Preference");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SubmitPost()
        {
            if (UserLoggedIn())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitPost(DomainModel.ValidationEntities.PostSubmission post)
        {
            if (!UserLoggedIn())
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.SourceURI = AffiliationHelper.LinkToAffiliates(post.SourceURI);
                    post.Title = HtmlFilterHelper.Filter(post.Title, new string[] { });
                    post.Description = HtmlFilterHelper.Filter(post.Description, new string[] { "a", "b", "i", "p" });

                    // Verify that it's not a double post.
                    using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
                    {
                        Table<Post> postTable = postDataContext.GetTable<Post>();

                        DomainModel.DataEntities.Post postExists = (from postEntity in postTable
                                                                    where postEntity.Title == post.Title
                                                                    || (!String.IsNullOrEmpty(post.SourceURI) && post.SourceURI != "http://" && postEntity.SourceURI == post.SourceURI)
                                                                    || (!String.IsNullOrEmpty(post.Content) && postEntity.Content == post.Content)
                                                                    select postEntity).FirstOrDefault();

                        if (postExists != null)
                        {
                            ViewData["postAlreadyExists"] = true;
                            ViewData["existingPost"] = postExists;

                            return View();
                        }
                    }

                    if (post.MediaType == DomainModel.ValidationEntities.PostSubmission.Media.Video)
                    {
                        if (VideoPageHtmlParser.Provider == VideoPageHtmlParser.VideoProvider.YouTube)
                        {
                            post.Content = VideoPageHtmlParser.GetVideoObjectHtmlCode(post.SourceURI);
                        }
                    }
                    else if (post.MediaType == DomainModel.ValidationEntities.PostSubmission.Media.Question || post.MediaType == DomainModel.ValidationEntities.PostSubmission.Media.Blog)
                    {
                        post.SourceURI = String.Empty;
                        post.Description = String.Empty;
                        post.Content = HtmlFilterHelper.Filter(post.Content, new string[] { "a", "b", "i", "p" });
                        post.Content = HtmlParsingHelper.ReplaceLineBreaksWithHtml(post.Content);
                    }

                    using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
                    {
                        Table<Post> postTable = postDataContext.GetTable<Post>();

                        DomainModel.DataEntities.Post postEntity = new Post();

                        postEntity.Date = DateTime.Now;
                        postEntity.UserName = ((DomainModel.DataEntities.User)Session["User"]).UserName;
                        postEntity.SourceURI = post.SourceURI;
                        postEntity.MediaType = (DomainModel.DataEntities.Post.Media)post.MediaType;
                        postEntity.Title = post.Title;
                        postEntity.Description = post.Description;
                        postEntity.Content = post.Content;

                        postTable.InsertOnSubmit(postEntity);
                        postDataContext.SubmitChanges();
                    }

                    ViewData["postSubmitted"] = true;

                    UpdateUserVoteQuota();
                }
                catch
                {
                    ViewData["postError"] = true;
                }
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Post(int? id)
        {
            if (id == null || id < 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Post post = new Post();

                // Grab the post content.
                using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<Post> postTable = postDataContext.GetTable<Post>();

                    post = (from postEntity in postTable
                            where postEntity.ID == id
                            select postEntity).FirstOrDefault();

                    if (post != null) // Make sure the post object returned is not null, then update the post view count.
                    {
                        post.ViewCount++;

                        postDataContext.SubmitChanges();
                    }
                }

                // Grab all of the post comments.
                // At this time the comments are only ordered by their time stamp, from oldest to new.
                // Other alternatives to consider in the future are grouped threaded discussions, staircase type thread etc.
                using (DataContext cmtDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<Comment> cmtTable = cmtDataContext.GetTable<Comment>();

                    post.Comments = (from cmtEntity in cmtTable
                                     where cmtEntity.ParentPostID == post.ID
                                     orderby cmtEntity.Date ascending
                                     select cmtEntity).ToList();

                    post.CommentsCount = post.Comments.Count;
                }

                return View(post);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitComment()
        {
            // Only permit ajax calls to this method.
            if (!Request.IsAjaxRequest())
            {
                return null;
            }

            // Filter the comment for unsupported HTML tags.
            string commentText = HtmlFilterHelper.Filter(Request.Form["NewComment"], new string[] { "a", "b", "i", "p", "div" });
            commentText = HtmlParsingHelper.ReplaceLineBreaksWithHtml(commentText);

            // Retrieve the post ID for which the comments are made for.
            int postID; int.TryParse(Request.Form["PostID"], out postID);
            int commentID; int.TryParse(Request.Form["CommentID"], out commentID);

            if (!UserLoggedIn())
            {
                return Content("sorry, user must login to comment...");
            }

            if (String.IsNullOrEmpty(commentText) || commentText.Length < 15)
            {
                return Content("sorry, comment needs to be at least 15 characters...");
            }

            // postID should always be non-zero. If commentID is zero, that means the comment was made in response to the post.
            // If commentID is not zero, then the comment was made in response to the comment of the commentID, which belongs 
            // to the post of the postID.
            if (postID == 0)
            {
                return Content("sorry, there was a problem with the post or comment...");
            }

            int postedCommentID = 0; // This value will be returned by this action method for consumption by js.
            using (DataContext cmtDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<Comment> cmtTable = cmtDataContext.GetTable<Comment>();

                Comment comment = (from cmtEntity in cmtTable where cmtEntity.Content == commentText select cmtEntity).FirstOrDefault();

                if (comment != null)
                {
                    return Content("sorry, this seems like a duplicate comment...");
                }
                else
                {
                    comment = new Comment();
                    comment.ParentPostID = postID;
                    comment.ParentCommentID = commentID;
                    comment.UserName = ((DomainModel.DataEntities.User)Session["User"]).UserName;
                    comment.Content = commentText;
                    comment.Date = DateTime.Now;

                    cmtTable.InsertOnSubmit(comment);
                    cmtDataContext.SubmitChanges();

                    postedCommentID = comment.ID; // After comment is submitted the ID is also updated from 0 to its assigned ID.
                }
            }

            UpdateUserVoteQuota();

            return Content("comment-" + postedCommentID);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetTitle(DomainModel.ValidationEntities.PostSubmission post)
        {
            // Only permit ajax calls to this method.
            if (!Request.IsAjaxRequest())
            {
                return null;
            }

            // Make sure the link is valid, and only http.
            if (String.IsNullOrEmpty(post.SourceURI.Substring(6)) || post.SourceURI.Substring(0,7) != "http://")
                return Content("Link is not valid, could not retrieve title...");

            try
            {
                if (post.MediaType == DomainModel.ValidationEntities.PostSubmission.Media.Video)
                {
                    return Content(Server.HtmlDecode(VideoPageHtmlParser.GetVideoTitle(post.SourceURI)));
                }
                else
                {
                    return Content(Server.HtmlDecode(WebPageHtmlParser.GetWebPageTitle(post.SourceURI)));
                }
            }
            catch
            {
                return Content("Error with retriving Title, please check url...");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetDescription(DomainModel.ValidationEntities.PostSubmission post)
        {
            // Only permit ajax calls to this method.
            if (!Request.IsAjaxRequest())
            {
                return null;
            }

            // Make sure the link is valid, and only http.
            if (String.IsNullOrEmpty(post.SourceURI.Substring(6)) || post.SourceURI.Substring(0, 7) != "http://")
                return Content("Link is not valid, could not retrieve description...");

            try
            {
                if (post.MediaType == DomainModel.ValidationEntities.PostSubmission.Media.Video)
                {
                    return Content(Server.HtmlDecode(VideoPageHtmlParser.GetVideoDescription(post.SourceURI)));
                }
                else
                {
                    return Content(Server.HtmlDecode(WebPageHtmlParser.GetWebPageDescription(post.SourceURI)));
                }
            }
            catch
            {
                return Content("Error with retriving Description, please check url...");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Tags(int? id)
        {
            if (id == null)
            {
                // Return view with all tags.
            }
            else
            {
                // Return Index view view with posts that contain the tag.
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Vote(int? id)
        {
            // Only permit ajax calls to this method.
            if (!Request.IsAjaxRequest())
            {
                return null;
            }

            int postID = id.GetValueOrDefault();
           
            if (postID == 0)
            {
                return Content("error...");
            }

            if (!UserLoggedIn())
            {
                return Content("please login");
            }

            DomainModel.DataEntities.User userLoggedIn = (DomainModel.DataEntities.User)Session["User"];

            int voteQuota = 0;
            using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<DomainModel.DataEntities.User> userTable = userDataContext.GetTable<User>();

                DomainModel.DataEntities.User user = (from userEntity in userTable where userEntity.ID == userLoggedIn.ID select userEntity).FirstOrDefault();

                if (user.VoteQuota < 1)
                {
                    return Content("no banzais left!");
                }

                user.VoteQuota--;

                userDataContext.SubmitChanges();

                voteQuota = user.VoteQuota;

                userLoggedIn.VoteQuota = user.VoteQuota;
            }

            // Increment the vote count for the post.
            int voteCount = 0;
            using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<DomainModel.DataEntities.Post> postTable = postDataContext.GetTable<Post>();

                Post post = (from postEntity in postTable where postEntity.ID == postID select postEntity).FirstOrDefault();

                if (post == null)
                {
                    return Content("error...");
                }

                post.VoteCount++;
                voteCount = post.VoteCount;

                postDataContext.SubmitChanges();
            }

            return Content(voteCount.ToString() + "#" + voteQuota.ToString());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Viewed(int? id)
        {
            if (!Request.IsAjaxRequest())
            {
                return Content("error...");
            }

            if (id == null || id < 1)
            {
                return Content("error...");
            }

            Post post = new Post();

            // Grab the post content.
            using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<Post> postTable = postDataContext.GetTable<Post>();

                post = (from postEntity in postTable
                        where postEntity.ID == id
                        select postEntity).FirstOrDefault();

                if (post != null) // Make sure the post object returned is not null, then update the post view count.
                {
                    post.ViewCount++;

                    postDataContext.SubmitChanges();
                }
                else
                {
                    return Content("error...");
                }
            }

            return Content(post.ViewCount.ToString());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Users(string id)
        {
            string userName = id; // id is actually the userName value.

            if (String.IsNullOrEmpty(userName))
            {
                // Return view with all users.
                using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<DomainModel.DataEntities.User> userTable = userDataContext.GetTable<User>();

                    List<User> users = (from userEntity in userTable orderby userEntity.UserName ascending select userEntity).ToList();

                    ViewData["DisplayUserList"] = users;
                }
            }
            else
            {
                // Return view with specific user.
                using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
                {
                    Table<DomainModel.DataEntities.User> userTable = userDataContext.GetTable<User>();

                    User user = (from userEntity in userTable where userEntity.UserName == userName select userEntity).FirstOrDefault();

                    if (user == null)
                    {
                        return View();
                    }

                    ViewData["DisplayUser"] = user;

                    using (DataContext postDataContext = new DataContext(Constants.DatabaseConnectionString))
                    {
                        Table<DomainModel.DataEntities.Post> postTable = postDataContext.GetTable<Post>();

                        List<DomainModel.DataEntities.Post> posts = (from postEntity in postTable 
                                                                     where postEntity.UserName == user.UserName 
                                                                     orderby postEntity.Date descending 
                                                                     select postEntity).ToList();
                        ViewData["DisplayUserPosts"] = posts;
                    }

                    using (DataContext cmtDataContext = new DataContext(Constants.DatabaseConnectionString))
                    {
                        Table<DomainModel.DataEntities.Comment> cmtTable = cmtDataContext.GetTable<Comment>();

                        List<DomainModel.DataEntities.Comment> comments = (from cmtEntity in cmtTable
                                                                           where cmtEntity.UserName == user.UserName
                                                                           orderby cmtEntity.Date descending
                                                                           select cmtEntity).ToList();

                        ViewData["DisplayUserComments"] = comments;
                    }
                }
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Contact()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contact(DomainModel.DataEntities.Message message)
        {
            using (DataContext msgDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<DomainModel.DataEntities.Message> msgTable = msgDataContext.GetTable<Message>();

                msgTable.InsertOnSubmit(message);

                msgDataContext.SubmitChanges();
            }

            ViewData["Msgsent"] = true;

            return View();
        }

        private bool UserLoggedIn()
        {
            if (Session["User"] == null)
                return false;
            else
                return ((DomainModel.DataEntities.User)Session["User"]).LoggedIn;
        }

        private void UpdateUserVoteQuota()
        {
            if (!UserLoggedIn())
            {
                return;
            }

            DomainModel.DataEntities.User userLoggedIn = (DomainModel.DataEntities.User)Session["User"];

            using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
            {
                Table<DomainModel.DataEntities.User> userTable = userDataContext.GetTable<User>();

                DomainModel.DataEntities.User user = (from userEntity in userTable where userEntity.UserName == userLoggedIn.UserName select userEntity).FirstOrDefault();

                user.VoteQuota += 3;

                userDataContext.SubmitChanges();

                userLoggedIn.VoteQuota = user.VoteQuota; // Update the session data.
            }
        }
    }
}
