<%@ Page Title="SailTally > Display Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayResults.aspx.cs" Inherits="SailTally.DisplayResults" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Display Results</h1>
    <p>Season <asp:DropDownList ID="listSeason" runat="server" AutoPostBack="True" onselectedindexchanged="listSeason_SelectedIndexChanged"></asp:DropDownList>&nbsp;Fleet&nbsp;<asp:DropDownList ID="listFleet" runat="server"></asp:DropDownList>
    <asp:Button ID="buttonView" runat="server" Text="View" onclick="buttonView_Click" /><asp:Button ID="buttonReset" runat="server" Text="Start Over" onclick="buttonReset_Click" /></p>
    <asp:Panel ID="panelSummary" runat="server">
        <asp:Label ID="labelResults" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>
