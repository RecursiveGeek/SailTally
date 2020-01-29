<%@ Page Title="SailTally > Trophy Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrophyResults.aspx.cs" Inherits="SailTally.TrophyResults" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <h1>Trophy Results</h1>
    <span id="spanNotice" runat="server">Make sure the <a href="CompleteResults.aspx">Complete Results</a> report has been run before running this report.</span>
    <p>Season <asp:DropDownList ID="listSeason" runat="server"></asp:DropDownList>&nbsp;<asp:Button ID="buttonTrophyResults" runat="server" Text="View" onclick="buttonTrophyResults_Click" OnClientClick="displayWorking();" /></p>
    <p><asp:Label ID="labelTrophyResults" runat="server"></asp:Label></p>
</asp:Content>
