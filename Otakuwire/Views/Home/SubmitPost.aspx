<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Otakuwire.DomainModel.ValidationEntities.PostSubmission>" %>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Otakuwire - Post Submission
</asp:Content>

<asp:Content ID="script" ContentPlaceHolderID="ScriptContent" runat="server">

    <script src="<%= Url.Content("~/Scripts/submitPost.js") %>" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">
    <% if (ViewData["postAlreadyExists"] != null && (bool)ViewData["postAlreadyExists"]) 
        {
            Otakuwire.DomainModel.DataEntities.Post post = (Otakuwire.DomainModel.DataEntities.Post)ViewData["existingPost"];
           
           %>
        
            <h2>Post already seems to exist:</h2>
            <h3><%= Html.ActionLink(Html.Encode(post.Title), "Post", new { @id = post.ID })%></h3>
    <% }
        else if (ViewData["postSubmitted"] != null && (bool)ViewData["postSubmitted"])
       { %>
            <h2>Post submitted, it should show up on the main page shortly, thanks for sharing!</h2>
            <h3><%= Html.ActionLink("Take me back to the posts","Index") %></h3>
    <% }
       else if (ViewData["postError"] != null && (bool)ViewData["postError"])
       { %>
           <h2>Whoops! There seems to be an error with posting, please try again later!</h2>
    <% }
       else
       { %>
        
        <h2 id="submitTitle">Submit Question</h2>
        
        <p>
            <%= Html.ValidationSummary("Whoops... posting was unsuccessful, please correct the errors and try again.") %>
        </p>
        
        <% using (Html.BeginForm("SubmitPost","Home"))
           {%>
            <fieldset>
                <legend>What would you like to share?</legend>
                <p>
                Before you post, please know that:
                </p>
                <ul>
                    <li>Your material is from the <b>root source</b>, don't post a link to another forum topic etc.</li>
                    <li>Your material is related to the <b>Otaku culture</b> (anime, gunpla, figures, games, food, japan and the likes).</li>
                    <li>If your material is <b>NSFW</b>, please label it so in the title!</li>
                    <li>Be <b>polite</b>, no one likes to read a post written by a jerk.</li>
                    <li>Videos from <b>YouTube</b> will be embeded into the post automatically.</li>
                    <li><b>Asian characters</b> might be garbled when retrieved from a url, retyping them should correct it.</li>
                </ul>
                
                <p>
                    <%-- RadioButton with the same name can be selected one at a time. --%>
                    <%= Html.RadioButton("MediaType", "Question", true)%>
                    Question
                    
                    <%= Html.RadioButton("MediaType", "Blog", false)%>
                    Blog post
                    
                    <%= Html.RadioButton("MediaType", "Article", false)%>
                    Article link
                    
                    <%= Html.RadioButton("MediaType", "Image", false)%>
                    Image link
                    
                    <%= Html.RadioButton("MediaType", "Video", false)%>
                    Video link
                </p>
                
                <p id="_source">
                    <label for="SourceURI">
                        URL:</label>
                    <%= Html.TextBox("SourceURI", "http://")%>
                </p>
                
                <p id="_title">
                    <label for="Title">
                        Title:</label>
                    <%= Html.TextBox("Title")%>
                </p>
                
                <p id="_description">
                    <label for="Description">
                        Description:</label>
                    <%= Html.TextArea("Description", new { rows = 10, cols = 80 })%>
                </p>
                
                <p id="_content">
                    <label for="Content">
                        What's on your mind? (Simple HTML tags like "a", "b", "i", "p" are allowed)</label>
                    <%= Html.TextArea("Content", new { rows = 10, cols = 80 })%>
                </p>
                
                <p>
                    <input type="submit" value="submit" />
                    <input type="button" value="retry" />
                    <%  %>
                </p>
                
            </fieldset>
        <% } %>
    <% } %>
    
</asp:Content>
