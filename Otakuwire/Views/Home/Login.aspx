<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Otakuwire.DomainModel.DataEntities.User>" %>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Otakuwire - Login
</asp:Content>
<asp:Content ID="script" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">

<h2>Login</h2>

    <% using (Html.BeginForm())
       {%>
            <fieldset>
                <%--<legend></legend>--%>
                
                <% if (ViewData["Message"] != null)
                   { %>
                        <p class="validation-summary-errors"><%= Html.Encode(ViewData["Message"])%></p>
                <% } %>
                
                <p>
                    <label for="UserName">User Name:</label>
                    <%= Html.TextBox("UserName") %>
                </p>
                <p>
                    <label for="Password">Password:</label>
                    <%= Html.Password("Password") %>
                </p>
                <p>
                    <input type="submit" value="Login" />
                </p>
                <p>
                    <b><%=Html.ActionLink("Register", "Register", "Home")%></b>
                </p>
            </fieldset>
    <% } %>
</asp:Content>