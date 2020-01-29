<%@ Page Title="SailTally > Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SailTally.Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Image ID="imageSplash" runat="server" ImageAlign="Right" ImageUrl="~/Images/SailTally.png" />
    <h1>Welcome!</h1>
    <p>Welcome to the SailTally Sailboat Racing Scoring application.</p>
    <p>SailTally is an <a href="http://sailtally.codeplex.com">open source</a> initiative to help sail racing clubs, supporting one-design racing.  Contributors always welcome!</p>
    <p>SailTally has been updated with the intent of supporting mobile devices.&nbsp; If you experience any issues from a particular mobile device, feel free to share your <a href="https://sailtally.codeplex.com/workitem/list/basic" target="_blank">feedback</a> with us, providing as many details on your hardware, environment, OS version, and so forth.</p>
</asp:Content>
