<%@ Page Title="SailTally > About" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="SailTally.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>About</h1>
    <p><asp:Label ID="labelVersion" runat="server" Text="Version"></asp:Label></p>
    <p>SailTally is a 100% donated custom developed application for the Minnetonka Yacht Club one design sailboat racing.</p>
    <p><a href="Docs/SailTally.pdf">Documentation</a></p>
    <p><a href="License.txt">License Information</a></p>
    <p>Whether you sail for pleasure or racing competitively, be sure to keep it happy.  This effort has been done to give back to the sport that gives so much in terms of sport, nature, life, and camaraderie.</p>
    </asp:Content>
