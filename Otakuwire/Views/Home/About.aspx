<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Otakuwire - About
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">

    <h2>What is Otakuwire?</h2>
    <p> 
        Otakuwire is a social media aggregator made for Otakus to discover and share content from anywhere on the Internet, 
        by submitting links and stories, and voting and commenting on submitted links and stories. In other words, a story's
        popularity is in your control.
    </p>
    
    <h2>How does Otakuwire work?</h2>
    <ul>
        <li>
            <div><h3>Discover and Share</h3></div>
            <div>Find something interesting to <b>submit</b> as a post to Otakuwire. Once submitted, other members can find it and vote for 
            it or comment on it.</div>
        </li>
        <li>
            <div><h3>Select</h3></div>
            <div>Participate in voting using <b>banzais</b> or <b>commenting</b> on a post to increase its popularity. The system works if 
            many people join in on the process.</div>
        </li>
    
    </ul>

    <h2>Why another aggregator?</h2>
    <p>
        Yes, we know there are Digg and reddit, and there are Dannychoo and Japanprobe. But the information on those websites are either diluted by other topics 
        or published by a selected few, making it hard for anime/manga/gunpla/figures/japan fans to find and rate media they care about. So, we decided to
        make something fresh and solely deticated to the fans of the Otaku culture.
    </p>
    
</asp:Content>
