<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Otakuwire.DomainModel" %>
<%@ Import Namespace="Otakuwire.Utilities" %>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Otakuwire - Users
</asp:Content>
<asp:Content ID="meta" ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>
<asp:Content ID="script" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        // If the ViewData["User"] is not null, then display a list of all the users in alphabitical order from ViewData["UserList"].

        Otakuwire.DomainModel.DataEntities.User user = (Otakuwire.DomainModel.DataEntities.User)Session["DisplayUser"];

        Otakuwire.DomainModel.DataEntities.User displayUser = (Otakuwire.DomainModel.DataEntities.User)ViewData["DisplayUser"];
        List<Otakuwire.DomainModel.DataEntities.Post> displayUserPosts = (List<Otakuwire.DomainModel.DataEntities.Post>)ViewData["DisplayUserPosts"];
        List<Otakuwire.DomainModel.DataEntities.Comment> displayUserComments = (List<Otakuwire.DomainModel.DataEntities.Comment>)ViewData["DisplayUserComments"];

        List<Otakuwire.DomainModel.DataEntities.User> displayUserList = (List<Otakuwire.DomainModel.DataEntities.User>)ViewData["DisplayUserList"];

        if (displayUser == null && displayUserList == null) // If the user was not found.
        { %>
            
            <h2>User profile not found...</h2>
            
      <%}
        else if (displayUserList != null) // displayUserList is populated by default when no information about which user to be displayed is given.
        { %>
            <h2>Users List: (<%= displayUserList.Count %> users)</h2>
            
        <% foreach (Otakuwire.DomainModel.DataEntities.User userEntity in displayUserList) 
           { %>
                <div class="userprofilelistitem">
                        <div style="float:left;">
                            <%= Html.Gravatar(userEntity.Email, new { s = "40" })%>
                        </div>
                        
                        <div style="float:left; margin-left: 8px;">
                            <h3 style="display:block"><%= Html.ActionLink(userEntity.UserName, "Users", new { @id = userEntity.UserName })%> <span class="uservotequota" style="float:right;"><%= userEntity.VoteQuota %></span></h3>
                            <div><b>Login date: <%= userEntity.LoginDate.Date.ToShortDateString() %></b></div>
                        </div>
                </div>
        <% } %>
    <%  }
        else // If browsing individual profiles users.
        {%>
            <h2><%= Html.Encode(displayUser.UserName) %>'s Profile</h2>
            
            <div style="float:left; margin: 8px 16px 0 8px;">
                <div><%= Html.Gravatar(displayUser.Email, new { s = "80" })%></div><br />
                <h3>Banzais: <span class="uservotequota" style="margin-left: 0px; font-size: 125%;"><%= displayUser.VoteQuota %></span></h3> 
                <h3>Registered: <%= displayUser.RegistrationDate.ToShortDateString() %></h3>
                <h3>Login date: <%= displayUser.LoginDate.ToShortDateString() %></h3>
                <h3>Location: <%= displayUser.Location %></h3>
            </div>
            
            <div style="float:left; width: 550px;">
                
                <% if (displayUserPosts.Count < 1)
                   {%>
                        <h2><%= Html.Encode(displayUser.UserName) %> has made no posts...</h2>
                <% }
                   else
                   { %>
                        <h2><%= displayUser.UserName %> has made <%= displayUserPosts.Count %> posts...</h2>
                    <% foreach (Otakuwire.DomainModel.DataEntities.Post postEntity in displayUserPosts)
                       {%>
                            <h3><%= Html.ActionLink(Html.Encode(postEntity.Title), "Post", new { @id = postEntity.ID + "/" + Otakuwire.Utilities.UrlHelper.TrimIllegalCharacters(Url.Encode(postEntity.Title)) })%></h3>
                    <% } %>
               <%  } %>
               
               <% if (displayUserComments.Count < 1)
                   {%>
                        <h2><%= Html.Encode(displayUser.UserName) %> has made no comments...</h2>
                <% }
                   else
                   { %>
                        <h2><%= displayUser.UserName %> has made <%= displayUserComments.Count%> comments...</h2>
                    <% foreach (Otakuwire.DomainModel.DataEntities.Comment cmtEntity in displayUserComments)
                       {%>
                            <h3><%= Html.ActionLink(cmtEntity.Content, "Post", new { @id = cmtEntity.ParentPostID + "#comment-" + cmtEntity.ID})%></h3>
                    <% } %>
               <%  } %>
            
            </div>
            
    <%  }%>
</asp:Content>
