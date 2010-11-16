<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Otakuwire.DomainModel.DataEntities.Post>" %>

<%@ Import Namespace="System.Data.Linq" %>
<%@ Import Namespace="Otakuwire.DomainModel" %>
<%@ Import Namespace="Otakuwire.DomainModel.DataEntities" %>
<%@ Import Namespace="Otakuwire.Utilities" %>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Otakuwire - <%= Model.Title %></asp:Content>
<asp:Content ID="meta" ContentPlaceHolderID="MetaContent" runat="server">
    <meta name="description" content="<%= Model.Description %>" />
</asp:Content>
<asp:Content ID="script" ContentPlaceHolderID="ScriptContent" runat="server">

<script src="<%= Url.Content("~/Scripts/postComment.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/vote.js") %>" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="post">
        <div class="indent">
            <div class="vote">
                <div class="votewidget">
                    <div class="votewidgetcount">
                    
                    <% if (Model.VoteCount < 1000)
                       {%>
                            <%= Model.VoteCount%>   
                    <% }
                       else
                       {%>
                            <%= (Convert.ToDouble(Model.VoteCount) / 1000).ToString("N1")%>K
                    <% } %>   
                    
                    </div>
                    <div class="votewidgetbutton" title="Click to give a banzai to this article!">banzai!</div>
                    <div class="votewidgetmessage"></div>
                    <input type="hidden" name="id" value="<%= Model.ID %>" />
                </div>
            </div>
            <div class="postcontent">
            <% if (!String.IsNullOrEmpty(Model.SourceURI))
               {%>
                    <h2>
                        <a href="<%= Model.SourceURI %>" target="_blank"><%= Model.Title%></a>
                    </h2>
            <% }
               else
               {%>
                    <h2>
                        <%= Model.Title%>
                    </h2>
            <% } %>
            
                <p>
                    <a class="tag" href="#commentslist"><%= Model.CommentsCount.ToString() + " comments" %></a> 
                    <%= Html.ActionLink(Model.UserName, "Users", new { @id = Html.Encode(Model.UserName) }, new { @class = "tag" })%>
                    <%= Html.ActionLink(Model.MediaType.ToString(), "Index", new { @id = "", @m = Model.MediaType.ToString() }, new { @class = "tag" })%>
                    <b>(posted <%= Model.Date.ToShortDateString() %>)</b>
                </p>
                <% if (!String.IsNullOrEmpty(Model.Description))
                   { %>
                        <b>Description:</b>
                        <p>
                            <%= Model.Description%>
                        </p>
                        <b><a href="<%= Model.SourceURI %>" target="_blank"><%= Otakuwire.Utilities.UrlHelper.GetRootUrl(Model.SourceURI) %></a></b>
                <% } %>
                <% if (!String.IsNullOrEmpty(Model.Content))
                   { %>
                        <p>
                            <%= Model.Content%>
                        </p>
                <% } %>
                
                <div>
                    <% if (Session["User"] != null && ((Otakuwire.DomainModel.DataEntities.User)Session["User"]).LoggedIn)
                       {%>
                            <%= Html.ActionLink("Comment", "Post", new { id = Model.ID }, new { @id = "commentbutton" })%>
                    <% }
                       else
                       { %>
                            <%= Html.ActionLink("Login to Comment", "Login", null, new { @id = "logincommentbutton" })%>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <% if (Session["User"] != null && ((Otakuwire.DomainModel.DataEntities.User)Session["User"]).LoggedIn)
       {%>
            <div id="commentinput">
                <% using (Html.BeginForm("SubmitComment", "Home"))
                   { %>
                        <div style="margin: 0 8px 4px 0; float:left;">
                            <label for="NewComment">
                                What would you like to share? (Simple HTML tags like "a", "b", "i", "p" are allowed)</label>
                            <%= Html.TextArea("NewComment", new { rows = 6, cols = 80 })%>
                        </div>
                        <div style="margin:2px 0 2px 0;">
                            Please check your language and ego before posting, for tolerance is a virtue...
                        </div>
                        <input type="hidden" name="PostID" value="<%= Model.ID %>" />
                        <input type="hidden" name="CommentID" value="0" />
                        <input type="submit" value="Submit" id="commentsubmitbutton" />
                        <input type="button" value="Cancel" id="commentcancelbutton" />
                        <span id="commentsubmitresponsemsg"></span>
                <% } %>
            </div>
    <% } %>
    <div id="commentslist">
        <% if (Model.Comments.Count > 0)
           {%>
                <h3>Reader Comments</h3>
                
                <%  int commentCount = 0;
                    foreach (Otakuwire.DomainModel.DataEntities.Comment comment in Model.Comments)
                    {
                %>
                        <div class="comment" id="comment-<%=comment.ID%>">
                            <% // Retrieve the comment's user email to get Gravatar image.
                                using (DataContext userDataContext = new DataContext(Constants.DatabaseConnectionString))
                                {
                                    Table<Otakuwire.DomainModel.DataEntities.User> userTable = userDataContext.GetTable<User>();
                                    string userEmail = (from userEntity in userTable where userEntity.UserName == comment.UserName select userEntity.Email).FirstOrDefault();
                            %>
                                    <div class="commentheader">
                                        <%= Html.Gravatar(userEmail, new { s = "30" })%>
                                        <span><b><%= Html.ActionLink(Html.Encode(comment.UserName),"Users",new {@id=Html.Encode(comment.UserName)}) %></b></span>
                                        <span>(posted&nbsp;<%= TimeDisplayHelper.TimeDisplay(comment.Date) %>)</span>
                                    </div>
                            <% } %>
                            <p>
                                <%= comment.Content%>
                            </p>
                            <% if (Session["User"] != null && ((Otakuwire.DomainModel.DataEntities.User)Session["User"]).LoggedIn)
                               {%>
                            <a href="" id="<%=comment.ID%>" class="replycomment">Reply</a>
                            <% } %>
                            <input type="hidden" name="username" value="<%= Html.Encode(comment.UserName) %>" />
                        </div>
                    <% 
                        if (commentCount == 1)
                        {
                            %>
                            <div style="width: 728px; margin-left: auto; margin-right: auto; padding: 2px 0 2px 0;">
                                <script type="text/javascript"><!--
                                        google_ad_client = "pub-5035352349564486";
                                        /* 728x90, Otakuwire top center. */
                                        google_ad_slot = "2706874946";
                                        google_ad_width = 728;
                                        google_ad_height = 90;
                                    //-->
                                </script>

                                <script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                                </script>
                           </div>
                    <%  }
                        commentCount++;
                    
                    } %>
        <% }
           else
           {%>
                <h3>No comments! Be the first!</h3>
        <% } %>
    </div>
</asp:Content>
