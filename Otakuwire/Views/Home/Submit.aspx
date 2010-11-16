<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Otakutechnica.DomainModel.Entities.Post>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Otakutechnica - Submit Post
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Submit Post</h2>

    <% using (Html.BeginForm())
       {%>

        <fieldset>
            <legend>Post</legend>
            <p>
                <label for="Title">Title:</label>
                <%= Html.TextBox("Title")%>
            </p>
            <p>
                <label for="Content">Content: (Simple HTML tags like "a", "b", "i", "p", "div" are allowed)</label>
                <%= Html.TextArea("Content", new { rows = 10, cols = 70 })%>
            </p>
            <p>
            <%-- RadioButton with the same name can only have one selected at a time. --%>
            <%= Html.RadioButton("MediaType", "Article", true)%> Article
            <%= Html.RadioButton("MediaType", "Blog", false)%> Blog Post
            <%= Html.RadioButton("MediaType", "Question", false)%> Question
            <%= Html.RadioButton("MediaType", "Image", false)%> Image
            <%= Html.RadioButton("MediaType", "Link", false)%> Link
            <%= Html.RadioButton("MediaType", "Video", false)%> Video
            </p>
            <p>
                <input type="submit" value="Submit" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

