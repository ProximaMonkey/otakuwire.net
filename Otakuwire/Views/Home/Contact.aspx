<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Otakuwire.DomainModel.DataEntities.Message>" %>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
	Otakuwire - Contact
</asp:Content>

<asp:Content ID="meta" ContentPlaceHolderID="MetaContent" runat="server">
</asp:Content>

<asp:Content ID="script" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">

<% if (ViewData["Msgsent"] != null && (bool)ViewData["Msgsent"])
   {%>
        <h2>Tahnk you for contacting us! We will try to get back to you if you had an inquiry.</h2>    
<% }
   else
   {%>
        <h2>Contact Otakuwire</h2>

        <% using (Html.BeginForm())
           {%>

            <fieldset>
                <p>
                    <label for="Name">Name:</label>
                    <%= Html.TextBox("Name")%>
                </p>
                <p>
                    <label for="Email">Email:</label>
                    <%= Html.TextBox("Email")%>
                </p>
                <p>
                    <label for="Content">Content:</label>
                    <%= Html.TextArea("Content", new { rows = 10, cols = 80 })%>
                </p>
                <p>
                    <input type="submit" value="submit" />
                </p>
            </fieldset>

        <% } %>
<% } %>

</asp:Content>


