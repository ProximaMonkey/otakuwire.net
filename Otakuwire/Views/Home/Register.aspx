<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Otakuwire.DomainModel.ValidationEntities.UserRegistration>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Otakuwire - Registration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Register</h2>

    <%= Html.ValidationSummary("Registration was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="User Name">User Name: (Characters allowed: a-z, A-Z, 0-9, "-" and "_" ; Length 3 to 16 characters long.)</label>
                <%= Html.TextBox("UserName") %>
                <%= Html.ValidationMessage("UserName", "*") %>
            </p>
            <p>
                <label for="Password">Password:</label>
                <%= Html.Password("Password") %>
                <%= Html.ValidationMessage("Password", "*") %>
            </p>
            <p>
                <label for="Password">Confirm Password:</label>
                <%= Html.Password("PasswordConfirm")%>
                <%= Html.ValidationMessage("PasswordConfirm", "*")%>
            </p>
            <p>
                <label for="Email">Email:</label>
                <%= Html.TextBox("Email") %>
                <%= Html.ValidationMessage("Email", "*") %>
            </p>
            <p>
                <label for="Email">Confirm Email:</label>
                <%= Html.TextBox("EmailConfirm") %>
                <%= Html.ValidationMessage("EmailConfirm", "*")%>
            </p>
            <p>
                <input type="submit" value="Register" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>

