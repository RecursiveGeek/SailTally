<%@ Page Title="SailTally > Boat Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BoatResults.aspx.cs" Inherits="SailTally.BoatResults" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Boat Results</h1>
    <span id="spanNotice" runat="server">Make sure the <a href="CompleteResults.aspx">Complete Results</a> report has been run before running this report.</span>
    <p>Season <asp:DropDownList ID="listSeason" runat="server"></asp:DropDownList>&nbsp;<asp:Button ID="buttonBoatResults" runat="server" onclick="buttonBoatResults_Click" Text="View" /></p>
    <p><asp:Label ID="labelBoatResults" runat="server"></asp:Label></p>
</asp:Content>
