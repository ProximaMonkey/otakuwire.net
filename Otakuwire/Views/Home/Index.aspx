<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Otakuwire.DomainModel" %>
<%@ Import Namespace="Otakuwire.DomainModel.DataEntities" %>
<%@ Import Namespace="Otakuwire.Utilities" %>

<asp:Content ID="index" ContentPlaceHolderID="TitleContent" runat="server">
    Otakuwire - the news aggregator for Otakus!
</asp:Content>
<asp:Content ID="meta" ContentPlaceHolderID="MetaContent" runat="server">

</asp:Content>

<asp:Content ID="script" ContentPlaceHolderID="ScriptContent" runat="server">

<script src="<%= Url.Content("~/Scripts/index.js") %>" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">    

<div id="subbanner">
<%  
    // Make sure the media type is a value of the Post.Media enum type. If not, set it to "All";
    string mediaType = Enum.GetNames(typeof(Otakuwire.DomainModel.DataEntities.Post.Media)).ToList().Contains(Request.QueryString["m"]) == false ? "All" : Request.QueryString["m"];
    string sortOrder = String.IsNullOrEmpty(Request.QueryString["o"]) ? "New" : Request.QueryString["o"];
    
    if (mediaType == "All")
    { %>
    
        <h1 style="float:left;">
        
            <% switch (sortOrder)
               { %>
                    <% default:
                       case "":
                       case "New":
                           {%>
                                New Posts
                        <% }
                           break;
                       case "Hot":
                           {%>
                                Hot Posts
                        <% }
                           break;
                       case "Week":
                           {%>
                                This Week's Posts
                        <% }
                           break;
                       case "Month":
                           {%>
                                This Month's Posts
                        <% }
                           break; %>
            <% } %>
        
        </h1>
        
 <% }
    else
    {%>

        <h1 style="float:left;">
        
            <% switch (sortOrder)
               { %>
                    <% default:
                       case "":
                       case "New":
                           {%>
                                New <%= mediaType %> Posts
                        <% }
                           break;
                       case "Hot":
                           {%>
                                Hot <%= mediaType %> Posts
                        <% }
                           break;
                       case "Week":
                           {%>
                                This Week's <%= mediaType %> Posts
                        <% }
                           break;
                       case "Month":
                           {%>
                                This Month's <%= mediaType %> Posts
                        <% }
                           break; %>
            <% } %>
        
        </h1>
        
 <% } %>
 
        <div id="tabbar">
            <% if (sortOrder == "New")
               {%>
                    <%= Html.ActionLink("new", "Index", new { @id = 1, @m = mediaType, @o = "New" }, new { @class = "tabcurrent" })%>
            <% }
               else
               {%>
                    <%= Html.ActionLink("new", "Index", new { @id = 1, @m = mediaType, @o = "New" }, new { @class = "tab" })%>
            <% } %>
                
            <% if (sortOrder == "Hot")
               {%>
                    <%= Html.ActionLink("hot", "Index", new { @id = 1, @m = mediaType, @o = "Hot" }, new { @class = "tabcurrent" })%>
            <% }
               else
               {%>
                    <%= Html.ActionLink("hot", "Index", new { @id = 1, @m = mediaType, @o = "Hot" }, new { @class = "tab" })%>
            <% } %>
            
            <% if (sortOrder == "Week")
               {%>
                    <%= Html.ActionLink("week", "Index", new { @id = 1, @m = mediaType, @o = "Week" }, new { @class = "tabcurrent" })%>
            <% }
               else
               {%>
                    <%= Html.ActionLink("week", "Index", new { @id = 1, @m = mediaType, @o = "Week" }, new { @class = "tab" })%>
            <% } %>
            
            <% if (sortOrder == "Month")
               {%>
                    <%= Html.ActionLink("month", "Index", new { @id = 1, @m = mediaType, @o = "Month" }, new { @class = "tabcurrent" })%>
            <% }
               else
               {%>
                    <%= Html.ActionLink("month", "Index", new { @id = 1, @m = mediaType, @o = "Month" }, new { @class = "tab" })%>    
            <% } %>
        </div>
    </div>
    
    <div id="post-listing">
        <% 
            if (ViewData["Posts"] != null)
            {
                foreach (var post in (List<Post>)ViewData["Posts"])
                {%>
                    <div class="post" id="post-<%= post.ID %>">
                        <div class="indent">
                            
                            <% string javascriptCode = "javascript:window.location.href='/Home/Post/" + post.ID + "/" + Otakuwire.Utilities.UrlHelper.TrimIllegalCharacters(Url.Encode(post.Title)) + "'"; %>
                            
                            <div class="stats" onclick="<%= javascriptCode %>">
                                <div class="votecounts">
                                    <% if (post.VoteCount < 1000) // Votes are called "banzais" on the website.
                                       {%>
                                            <%= post.VoteCount %>
                                       <% } 
                                       else {%>
                                            <%= (Convert.ToDouble(post.VoteCount)/1000).ToString("N1") %>K
                                    <% } %>
                                </div>
                                <div style="font-size:80%;">banzais</div>
                                <div class="viewcounts">
                                    <% if (post.ViewCount < 1000)
                                       {%>
                                            <%= post.ViewCount%>
                                       <% } 
                                       else {%>
                                            <%= (Convert.ToDouble(post.ViewCount) / 1000).ToString("N1")%>K
                                    <% } %>
                                </div>
                                <div style="font-size:80%;">views</div>
                            </div>
                            <div class="summary">
                                <h3 style="display:block;">
                                    <% if (post.MediaType == Post.Media.Question || post.MediaType == Post.Media.Blog || post.MediaType == Post.Media.Video)
                                       {%>
                                            <%= Html.ActionLink(Html.Encode(post.Title), "Post", new { @id = post.ID + "/" + Otakuwire.Utilities.UrlHelper.TrimIllegalCharacters(Url.Encode(post.Title)) })%>
                                    <% }
                                       else
                                       {%>
                                            <a id="<%= post.ID %>" class="redirectlink" href="<%= post.SourceURI %>" target="_blank"><%= Html.Encode(post.Title) %></a>
                                    <% } %>
                                    
                                    
                                     <% if (!String.IsNullOrEmpty(post.SourceURI))
                                        { %>
                                            <span style="float:right; font-size:60%; margin-left: 2px; color:#575757;">(<%= Otakuwire.Utilities.UrlHelper.GetRootUrl(post.SourceURI)%>)</span>
                                     <% } %>
                                     
                                </h3>
                                
                                <% if (!String.IsNullOrEmpty(post.Description))
                                   {%>
                                        <p>
                                            <% if (post.Description.Length > 150)
                                               { %>
                                                    <%= Html.Encode(post.Description.Substring(0, 150)) + "..."%>
                                            <% }
                                               else
                                               { %>
                                                    <%= Html.Encode(post.Description)%>
                                            <% } %>
                                        </p>
                                <% } %>
                            
                                <%= Html.ActionLink(post.CommentsCount.ToString() + " comments", "Post", new { @id = post.ID + "/" + Otakuwire.Utilities.UrlHelper.TrimIllegalCharacters(Url.Encode(post.Title)) }, new { @class = "tag" })%>
                                <%= Html.ActionLink(post.UserName, "Users", new { @id = Html.Encode(post.UserName) }, new { @class = "tag"})%>
                                <%= Html.ActionLink(post.MediaType.ToString(), "Index", new { @id = "", @m = post.MediaType.ToString() }, new { @class = "tag" })%>
                                <span style="float:right; font-size:80%; margin-left: 8px;">posted <%= TimeDisplayHelper.TimeDisplay(post.Date)%> by <%= Html.Encode(post.UserName) %></span>
                            </div>
                        </div>
                    </div>
        <%      }
            } %>
    </div>
    
    <div id="navbar">
            
        <%  // Display a prev. next. last. navigation bar at the lower left hand corner of the page.
            int pageNumber= (int)ViewData["PageNumber"];
            int totalPages = (int)ViewData["TotalPages"];

            if (totalPages == 1)
            {
                
            }
            else if (pageNumber == 1)
            { %>
                    <%= Html.ActionLink("next","Index",new {@id = pageNumber + 1, @m = mediaType, @o = sortOrder},new {@class = "nav"}) %>
                    <%= Html.ActionLink("last", "Index", new { @id = totalPages, @m = mediaType, @o = sortOrder }, new { @class = "nav" })%>
        <%  }
            else if (pageNumber > 1 && pageNumber < totalPages)
            { %>
                    <%= Html.ActionLink("first", "Index", new { @id = 1, @m = mediaType, @o = sortOrder }, new { @class = "nav" })%>
                    <%= Html.ActionLink("prev", "Index", new { @id = pageNumber - 1, @m = mediaType, @o = sortOrder }, new { @class = "nav" })%>
                    <%= Html.ActionLink("next", "Index", new { @id = pageNumber + 1, @m = mediaType, @o = sortOrder }, new { @class = "nav" })%>
                    <%= Html.ActionLink("last", "Index", new { @id = totalPages, @m = mediaType, @o = sortOrder }, new { @class = "nav" })%>
        <%  }
            else if (pageNumber == totalPages)
            { %>
                    <%= Html.ActionLink("first", "Index", new { @id = 1, @m = mediaType, @o = sortOrder }, new { @class = "nav" })%>
                    <%= Html.ActionLink("prev", "Index", new { @id = pageNumber - 1, @m = mediaType, @o = sortOrder }, new { @class = "nav" })%>
        <%  } %>
        
        <%= Html.ActionLink("whatever", "Post", new { @id = ViewData["RandomPostNumber"] }, new { @class = "nav", @style = "float:right;" })%>
        
    </div>
</asp:Content>
