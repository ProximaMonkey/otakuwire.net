<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Otakuwire.DomainModel.ValidationEntities.UserPreference>" %>

<%@ Import Namespace="Otakuwire.Utilities" %>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
	Otakuwire - Preference Panel
</asp:Content>

<asp:Content ID="meta" ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>

<asp:Content ID="script" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">

<% 
    Otakuwire.DomainModel.DataEntities.User user = (Otakuwire.DomainModel.DataEntities.User)Session["User"];
    
   if (!user.LoggedIn)
   {%>
        <h2>Please login to change preferences...</h2>
<% }
   else
   { %>
       <h2><%= Html.Encode(user.UserName)%>'s Preference</h2>
     <%  
       using (Html.BeginForm())
       {%>

        <fieldset>
            <%--<legend></legend>--%>
            
            <%= Html.ValidationSummary("Whoops, saving was unsuccessful... please correct the errors and try again.")%>
            
            <% if (ViewData["Message"] != null)
               { %>
                    <p>
                        <span class="preferencesaved"><%= ViewData["Message"]%></span>
                    </p>
            <% } %>
            
            <p>
                <label><b>Avatar:</b></label>
                <%= Html.Gravatar(Model.Email) %>
                <br />
                Otakuwire uses <a class="tag" href="http://www.gravatar.com/" target="_blank">Gravatar</a> to host avatars, so users can use their avatars cross-site. Please make sure your Email address in Otakuwire is the same as for Gravatar to be displayed properly.
            </p>
            <p>
                <label><b>Profile:</b> <%= Html.ActionLink(user.UserName, "Users", new { @id = Html.Encode(user.UserName) }, new { @class = "tag" })%></label>
            </p>
            <p>
                <label><b>Banzais*:</b><span class="uservotequota"><%= user.VoteQuota %></span></label>
                <br />
                * Banzais are like votes, but with a catch. Everyone starts off with 33 banzais when they register. You can promote posts that you like by giving them banzais, the more banzais a post has, the more popular it will be in ranking. You earn three banzais
                everytime you submit a post or make a comment.
            </p>
            <br />
            <p><label><b>Settings:</b></label></p>
            <p>
                <label for="FirstName">First Name: (optional)</label>
                <%= Html.TextBox("FirstName", Html.Encode(Model.FirstName))%>
                <%= Html.ValidationMessage("FirstName", "*")%>
            </p>
            <p>
                <label for="LastName">Last Name: (optional)</label>
                <%= Html.TextBox("LastName", Html.Encode(Model.LastName))%>
                <%= Html.ValidationMessage("LastName", "*")%>
            </p>
            <p>
                <label for="Location">Location: (optional)</label>
                <%= Html.TextBox("Location", Html.Encode(Model.Location))%>
                <%= Html.ValidationMessage("Location", "*")%>
            </p>
             <p>
                <label for="Email">Email:</label>
                <%= Html.TextBox("Email", Html.Encode(Model.Email))%>
                <%= Html.ValidationMessage("Email", "*")%>
            </p>
            <p>
                <label for="EmailConfirm">Confirm Email:</label>
                <%= Html.TextBox("EmailConfirm", Html.Encode(Model.EmailConfirm))%>
                <%= Html.ValidationMessage("EmailConfirm", "*")%>
            </p>
            <p>
                <label for="Password">Password:</label>
                <%= Html.Password("Password", Model.Password)%>
                <%= Html.ValidationMessage("Password", "*")%>
            </p>
            <p>
                <label for="PasswordConfirm">Confirm Password:</label>
                <%= Html.Password("PasswordConfirm", Model.PasswordConfirm)%>
                <%= Html.ValidationMessage("PasswordConfirm", "*")%>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
            
        </fieldset>

    <% }
   } %>

</asp:Content>

